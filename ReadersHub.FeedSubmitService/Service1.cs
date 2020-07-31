using Autofac;
using MarketplaceWebService;
using MarketplaceWebService.Model;
using NLog;
using ReadersHub.Business.Service.Criterions;
using ReadersHub.Business.Service.FeedTemps;
using ReadersHub.Business.Service.Store;
using ReadersHub.Common.Constants;
using ReadersHub.Common.Dto.Criterion;
using ReadersHub.Common.Dto.FeedTemp;
using ReadersHub.Common.Dto.Store;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.IO;
using System.Linq;
using System.Reflection;
using System.ServiceProcess;
using System.Timers;

namespace ReadersHub.FeedSubmitService
{
    public partial class Service1 : ServiceBase
    {
        private readonly string SQS_URL;
        private readonly string SELLER_ID;
        private readonly string ACCESS_KEY_ID;
        private readonly string SECRET_KEY;
        private readonly string MARKET_PLACE_ID;
        private readonly string MWS_DESTINATION;
        private readonly string NEW_PREFIX;
        private readonly string USED_PREFIX;
        private readonly string NEW_MIN_PRICE;
        private readonly string USED_MIN_PRICE;
        private MarketplaceWebServiceClient client;
        private static Logger logger;
        private readonly IFeedTempService _feedTempService;
        private readonly ICriterionService _criterionService;
        private readonly IStoreService _storeService;
        private IContainer container;
        private readonly int INTERVAL;
        private readonly int SUBMIT_INTERVAL;
        private StoreDto _storeDto;
        private bool IsProcessComplated;
        private Dictionary<string, string> Settings;

        public Service1()
        {
            InitializeComponent();
            InitializeSettings();

            container = ContainerConfig.Configure();
            _criterionService = container.BeginLifetimeScope().Resolve<ICriterionService>();
            _feedTempService = container.BeginLifetimeScope().Resolve<IFeedTempService>();
            _storeService = container.BeginLifetimeScope().Resolve<IStoreService>();

            logger = LogManager.GetCurrentClassLogger();
            NEW_PREFIX = Settings["NEW_PREFIX"];
            USED_PREFIX = Settings["USED_PREFIX"];
            NEW_MIN_PRICE = Settings["NEW_MIN_PRICE"];
            USED_MIN_PRICE = Settings["USED_MIN_PRICE"];
            SQS_URL = Settings["SQS_URL"];
            SELLER_ID = Settings["SELLER_ID"];
            ACCESS_KEY_ID = Settings["ACCESS_KEY_ID"];
            SECRET_KEY = Settings["SECRET_KEY"];
            MARKET_PLACE_ID = Settings["MARKET_PLACE_ID"];
            MWS_DESTINATION = Settings["MWS_DESTINATION"];
            INTERVAL = int.Parse(ConfigurationManager.AppSettings["HeartBeatIntervalSecond"]) * 1000;
            SUBMIT_INTERVAL = int.Parse(ConfigurationManager.AppSettings["SubmitInterval"]) * 1000;
            client = GetClient();

            _storeDto = _storeService.GetStoreBySellerId(SELLER_ID);
            if (_storeDto == null)
            {
                throw new Exception("Girilen SELLER_ID geçersiz.");
            }
        }

        private void InitializeSettings()
        {
            string path = ConfigurationManager.AppSettings["SettingsPath"];
            Settings = File
            .ReadAllLines(path)
            .Select(x => x.Split(','))
            .Where(x => x.Length > 1)
            .ToDictionary(x => x[0].Trim(), x => x[1].Trim());
        }

        public void OnDebug()
        {
            logger.Log(LogLevel.Info, "SERVICE START");
            timer.Interval = INTERVAL;
            timer.Elapsed += Timer_Elapsed;
            timer.Enabled = true;
            timer.Start();

            ProcessSubmitFeeds();
            submitTimer.Interval = SUBMIT_INTERVAL;
            submitTimer.Elapsed += SubmitTimer_Elapsed;
            submitTimer.Enabled = true;
            submitTimer.Start();
        }

        private void Timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            var dto = new CriterionDto()
            {
                Key = CriterionKeys.FeedSubmitServiceHeartBeat,
                StoreId = _storeDto.Id,
                Value = DateTime.Now.ToString("dd.MM.yyyy hh:mm:ss")
            };
            _criterionService.UpdateOrCreate(dto.Key, dto);
        }

        private void SubmitTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            if (IsProcessComplated)
            {
                ProcessSubmitFeeds();
            }
            else
            {
                logger.Log(LogLevel.Info, "PROCESS NOT FINISH");
            }
        }

        protected override void OnStart(string[] args)
        {
            logger.Log(LogLevel.Info, "SERVICE START");
            timer.Interval = INTERVAL;
            timer.Elapsed += Timer_Elapsed;
            timer.Enabled = true;
            timer.Start();

            submitTimer.Interval = SUBMIT_INTERVAL;
            submitTimer.Elapsed += SubmitTimer_Elapsed;
            submitTimer.Enabled = true;
            submitTimer.Start();
        }

        protected override void OnStop()
        {
        }

        public void ProcessSubmitFeeds()
        {
            string fileName = "";
            List<FeedTempDto> feedList = new List<FeedTempDto>();
            try
            {
                IsProcessComplated = false;
                logger.Log(LogLevel.Info, "PROCESS SUBMIT FEED");
                string[] files = Directory.GetFiles("C:\\ReadersHub\\file", string.Format("p_{0}_*.txt", (object)SELLER_ID));
                List<FeedTempDto> feedTempDtoList = new List<FeedTempDto>();
                foreach (string tmpFileName in files)
                {
                    if (!this.IsFileLocked(tmpFileName))
                    {
                        using (StreamReader streamReader = new StreamReader(tmpFileName))
                        {
                            while (streamReader.Peek() >= 0)
                            {
                                string[] strArray = streamReader.ReadLine().Split(new string[1] { "#" }, StringSplitOptions.RemoveEmptyEntries);
                                string sku = strArray[0];
                                decimal price = decimal.Parse(strArray[1]);
                                DateTime dateTime = new DateTime(long.Parse(strArray[2]));
                                feedTempDtoList.Add(new FeedTempDto()
                                {
                                    Sku = sku,
                                    Price = price,
                                    CreateDate = dateTime
                                });
                            }
                        }
                    }
                }
                foreach (var feedTempDto in feedTempDtoList.OrderByDescending(x => x.CreateDate))
                {
                    if (!feedList.Any(x => x.Sku == feedTempDto.Sku))
                    {
                        feedList.Add(feedTempDto);
                    }
                }

                logger.Log(LogLevel.Info, string.Format("FEED GET COMPLETED. FeedCount: {0}", feedList.Count));
                if (feedList.Count == 0)
                {
                    logger.Log(LogLevel.Info, "EMPTY FEED_LIST");
                    IsProcessComplated = true;
                }
                else
                {
                    fileName = this.PrepareFile(feedList);
                    logger.Log(LogLevel.Info, "FILE PREPARED");
                    SubmitFile(fileName);
                    logger.Log(LogLevel.Info, "FILE SUBMIT COMPLETED");
                    foreach (string path in files)
                        File.Delete(path);
                    IsProcessComplated = true;
                }
            }
            catch (Exception ex)
            {
                IsProcessComplated = true;
                logger.Log(LogLevel.Error, ex.ToString());
            }
        }

        private SubmitFeedResult SubmitFile(string fileName)
        {
            string feedType = "_POST_FLAT_FILE_PRICEANDQUANTITYONLY_UPDATE_DATA_";

            SubmitFeedResponse response = new SubmitFeedResponse();

            SubmitFeedRequest sfrequest = new SubmitFeedRequest();
            sfrequest.Merchant = SELLER_ID;
            IdList merchantId = new IdList();
            List<string> IdList = new List<string>();
            IdList.Add(MARKET_PLACE_ID);
            merchantId.Id = IdList;
            sfrequest.MarketplaceIdList = merchantId;

            using (FileStream stream = new FileStream(fileName, FileMode.Open, FileAccess.ReadWrite))
            {
                sfrequest.FeedContent = stream;
                sfrequest.ContentMD5 = MarketplaceWebServiceClient.CalculateContentMD5(sfrequest.FeedContent);
                sfrequest.FeedContent.Position = 0;
                sfrequest.FeedType = feedType.ToString();
                sfrequest.PurgeAndReplace = true;

                response = client.SubmitFeed(sfrequest);
            }

            return response.SubmitFeedResult;
        }

        private string PrepareFile(List<FeedTempDto> feedList)
        {
            string strAppDir = Path.GetDirectoryName(Assembly.GetExecutingAssembly().GetName().CodeBase);

            string fileName = $"{DateTime.Now.Day}-{DateTime.Now.Month}-{DateTime.Now.Year}T{DateTime.Now.Hour}-{DateTime.Now.Minute}-{DateTime.Now.Second}.txt";
            string folderName = @"C:\ReadersHub\amazonSQSText\";
            string fullPath = Path.Combine(folderName, fileName);
            if (!Directory.Exists(folderName))
            {
                Directory.CreateDirectory(folderName);
            }

            File.Create(fullPath).Close();
            var file = new StreamWriter(fullPath);
            file.WriteLine("sku\tprice\tminimum-seller-allowed-price\tmaximum-seller-allowed-price\tquantity\tleadtime-to-ship\tfulfillment-channel");

            var usedMinPrice = Convert.ToDecimal(USED_MIN_PRICE, CultureInfo.InvariantCulture);
            var newMinPrice = Convert.ToDecimal(NEW_MIN_PRICE, CultureInfo.InvariantCulture);

            foreach (var feed in feedList)
            {
                if (feed.Sku.StartsWith(NEW_PREFIX) && feed.Price < newMinPrice)
                {
                    file.WriteLine($"{feed.Sku}\t{newMinPrice.ToString("0.##")}");
                }
                else if (feed.Sku.StartsWith(USED_PREFIX) && feed.Price < usedMinPrice)
                {
                    file.WriteLine($"{feed.Sku}\t{usedMinPrice.ToString("0.##")}");
                }
                else
                {
                    file.WriteLine($"{feed.Sku}\t{(feed.Price - (decimal)0.01).ToString("0.##")}");
                }
            }
            file.Close();

            return fullPath;
        }

        private bool IsFileLocked(string fileName)
        {
            FileStream fileStream = null;
            FileInfo fileInfo = new FileInfo(fileName);
            try
            {
                fileStream = fileInfo.Open(FileMode.Open, FileAccess.Read, FileShare.None);
            }
            catch (IOException ex)
            {
                return true;
            }
            finally
            {
                if (fileStream != null)
                    fileStream.Close();
            }
            return false;
        }

        private MarketplaceWebServiceClient GetClient()
        {
            if (client != null)
            {
                return client;
            }
            MarketplaceWebServiceConfig mwsConfig = new MarketplaceWebServiceConfig();
            mwsConfig.ServiceURL = MWS_DESTINATION;
            mwsConfig.SetUserAgentHeader("PowerSellerBook", "1.0", "C#", new string[] { });

            return new MarketplaceWebServiceClient(ACCESS_KEY_ID, SECRET_KEY, mwsConfig);
        }
    }
}
