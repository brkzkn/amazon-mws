using Amazon.Runtime;
using Amazon.SQS;
using Amazon.SQS.Model;
using Autofac;
using NLog;
using ReadersHub.Business.Service.Criterions;
using ReadersHub.Business.Service.FeedTemps;
using ReadersHub.Business.Service.Store;
using ReadersHub.Common.Constants;
using ReadersHub.Common.Dto.Criterion;
using ReadersHub.Common.Dto.FeedTemp;
using ReadersHub.Common.Dto.Store;
using ReadersHub.NotificationService.AppConfig;
using System;
using System.Collections.Concurrent;
using System.Collections.Generic;
using System.Configuration;
using System.Globalization;
using System.IO;
using System.Linq;
using System.ServiceProcess;
using System.Threading.Tasks;
using System.Timers;
using System.Xml.Linq;

namespace ReadersHub.NotificationService
{
    public partial class Service1 : ServiceBase
    {
        private readonly string SQS_URL;
        private readonly string SELLER_ID;
        private readonly string NEW_PREFIX;
        private readonly string USED_PREFIX;
        private readonly string SHIPPING_STD;
        private readonly string SHIPPING_COMP;

        private readonly AmazonSQSClient sqsClient;
        private readonly ICriterionService _criterionService;
        private readonly IStoreService _storeService;
        private readonly IFeedTempService _feedTempService;
        private Autofac.IContainer container;
        private Helper _helper;
        private List<FeedTempDto> FeedTempList;
        private static Logger logger;
        private StoreDto _storeDto;
        private Dictionary<string, string> Settings;
        private readonly int INTERVAL;
        private bool UpdateCriterion;
        private bool IsProcessCompleted;
        private ConcurrentBag<FeedTempDto> QueueList;

        public Service1()
        {
            logger = LogManager.GetCurrentClassLogger();
            logger.Log(LogLevel.Info, "Initialize logger");
            InitializeComponent();
            try
            {
                InitializeSettings();
            }
            catch (Exception ex)
            {
                logger.Log(LogLevel.Info, ex.Message);
                throw;
            }
            logger.Log(LogLevel.Info, "Initialize settings");

            container = ContainerConfig.Configure();
            _criterionService = container.BeginLifetimeScope().Resolve<ICriterionService>();
            _feedTempService = container.BeginLifetimeScope().Resolve<IFeedTempService>();
            _storeService = container.BeginLifetimeScope().Resolve<IStoreService>();
            QueueList = new ConcurrentBag<FeedTempDto>();
            logger.Log(LogLevel.Info, "Initialize services");

            NEW_PREFIX = Settings["NEW_PREFIX"];
            USED_PREFIX = Settings["USED_PREFIX"];
            SHIPPING_STD = Settings["SHIPPING_STD"];
            SHIPPING_COMP = Settings["SHIPPING_COMP"];
            SQS_URL = Settings["SQS_URL"];
            SELLER_ID = Settings["SELLER_ID"];

            INTERVAL = int.Parse(ConfigurationManager.AppSettings["HeartBeatIntervalSecond"]) * 1000;

            sqsClient = InitializeSQSClient();
            logger.Log(LogLevel.Info, "Initialize SQS Client");
            _storeDto = _storeService.GetStoreBySellerId(SELLER_ID);
            if (_storeDto == null)
            {
                throw new Exception("Girilen SELLER_ID geçersiz.");
            }

            _helper = new Helper(_criterionService, _storeDto, SELLER_ID);
            FeedTempList = new List<FeedTempDto>();
            logger.Log(LogLevel.Info, "Initialize completed");
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

        private AmazonSQSClient InitializeSQSClient()
        {
            string accessKey = Settings["ACCESS_KEY_ID"];
            string secretKey = Settings["SECRET_KEY"];
            return new AmazonSQSClient(accessKey, secretKey, Amazon.RegionEndpoint.USWest2);
        }

        public void OnDebug()
        {
            logger.Log(LogLevel.Info, "SERVICE START");
            timer.Interval = INTERVAL;
            timer.Elapsed += Timer_Elapsed;
            timer.Enabled = true;
            timer.Start();

            ProcessNotification();
            processNotificationTimer.Interval = 2000;
            processNotificationTimer.Elapsed += ProcessNotification_Timer_Elapsed;
            processNotificationTimer.Enabled = true;
            processNotificationTimer.Start();
        }

        private void Timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            if (UpdateCriterion)
            {
                try
                {
                    UpdateCriterion = false;
                    var dto = new CriterionDto()
                    {
                        Key = CriterionKeys.NotificationServiceHeartBeat,
                        StoreId = _storeDto.Id,
                        Value = DateTime.Now.ToString("dd.MM.yyyy hh:mm:ss")
                    };
                    _criterionService.UpdateOrCreate(dto.Key, dto);

                    UpdateCriterion = true;
                }
                catch (Exception ex)
                {
                    logger.Log(LogLevel.Error, ex.ToString());
                    UpdateCriterion = true;
                }
            }
        }

        private void ProcessNotification_Timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            if (!IsProcessCompleted)
                return;
            ProcessNotification();
        }

        protected override void OnStart(string[] args)
        {
            logger.Log(LogLevel.Info, "SERVICE START");
            timer.Interval = INTERVAL;
            timer.Elapsed += Timer_Elapsed;
            timer.Enabled = true;
            timer.Start();

            ProcessNotification();
            processNotificationTimer.Interval = 3000;
            processNotificationTimer.Elapsed += ProcessNotification_Timer_Elapsed;
            processNotificationTimer.Enabled = true;
            processNotificationTimer.Start();
        }

        protected override void OnStop()
        {
        }

        protected override void OnShutdown()
        {
            base.OnShutdown();
        }

        private void ProcessNotification()
        {
            IsProcessCompleted = false;
            List<Task<ReceiveMessageResponse>> taskList = new List<Task<ReceiveMessageResponse>>();
            Func<ReceiveMessageResponse> function = () => sqsClient.ReceiveMessage(new ReceiveMessageRequest()
            {
                MaxNumberOfMessages = 10,
                QueueUrl = SQS_URL
            });
            for (int index = 0; index < 1; ++index)
                taskList.Add(Task<ReceiveMessageResponse>.Factory.StartNew(function));
            try
            {
                Task.WaitAll(taskList.ToArray());
            }
            catch (AggregateException ex)
            {
                List<Task<ReceiveMessageResponse>> source = taskList;
                if (!source.Any(x => x.Status == TaskStatus.RanToCompletion))
                {
                    IsProcessCompleted = true;
                    return;
                }
                if (!(ex.InnerException is AmazonUnmarshallingException))
                {
                    LogEventInfo logEvent = new LogEventInfo()
                    {
                        Exception = ex,
                        Level = LogLevel.Error
                    };
                    logger.Log(logEvent);
                }
            }

            Parallel.ForEach(taskList, (Action<Task<ReceiveMessageResponse>>)(task =>
            {
                if (task.Status != TaskStatus.RanToCompletion)
                    return;
                ProcessMessage(task.Result.Messages);
                DeleteMessage(task.Result.Messages);
            }));
            if (QueueList.Count >= 100)
            {
                using (StreamWriter streamWriter = new StreamWriter(string.Format("C:\\ReadersHub\\file\\p_{0}_{1}.txt", (object)this.SELLER_ID, (object)Guid.NewGuid())))
                {
                    foreach (FeedTempDto queue in this.QueueList)
                        streamWriter.WriteLine(string.Format("{0}#{1}#{2}", (object)queue.Sku, (object)queue.Price.ToString("0.##"), (object)queue.CreateDate.Ticks));
                    streamWriter.Close();
                }
                QueueList = new ConcurrentBag<FeedTempDto>();
            }
            IsProcessCompleted = true;
        }

        private void ProcessMessage(List<Message> messages)
        {
            try
            {
                var companyPrice = Convert.ToDecimal(SHIPPING_COMP, CultureInfo.InvariantCulture);

                foreach (Message message in messages)
                {
                    var body = XDocument.Parse(message.Body);
                    string asin = body.Descendants("ASIN").SingleOrDefault().Value;
                    string itemCondition = body.Descendants("ItemCondition").SingleOrDefault().Value;
                    List<decimal> offerPrice = new List<decimal>();
                    foreach (XElement offer in body.Descendants("Offer"))
                    {
                        string sellerFeedbackRating = offer.Descendants("SellerPositiveFeedbackRating").SingleOrDefault().Value;
                        string feedbackCount = offer.Descendants("FeedbackCount").SingleOrDefault().Value;
                        string sellerId = offer.Descendants("SellerId").SingleOrDefault().Value;
                        string subCondition = offer.Descendants("SubCondition").SingleOrDefault().Value;

                        var countryEntity = offer.Descendants("Country").SingleOrDefault();
                        string country = string.Empty;
                        if (countryEntity != null)
                            country = countryEntity.Value;
                        if (_helper.CheckCriteria("", "", sellerId, "", "") && sellerId != SELLER_ID)
                        {
                            string listing = offer.Descendants("ListingPrice").Descendants("Amount").SingleOrDefault().Value;
                            string shipping = offer.Descendants("Shipping").Descendants("Amount").SingleOrDefault().Value;
                            decimal listingPrice = Convert.ToDecimal(listing, CultureInfo.InvariantCulture);
                            decimal shippingPrice = Convert.ToDecimal(shipping, CultureInfo.InvariantCulture);
                            offerPrice.Add(listingPrice + shippingPrice - companyPrice);
                        }
                    }
                    offerPrice = offerPrice.OrderBy(x => x).ToList();
                    decimal price = 0;

#if GIFT
                    if (offerPrice.Count >= 4)
                        price = offerPrice[3] - (decimal)0.01;
                    else if (offerPrice.Count == 3)
                        price = offerPrice[2] - (decimal)0.01;
                    else if (offerPrice.Count == 2)
                        price = offerPrice[1] - (decimal)0.01;
                    else
                        price = offerPrice.Min() - (decimal)0.01;
#elif TOFFEE
                    if (offerPrice.Count >= 2)
                        price = offerPrice[1] - (decimal)0.01;
                    else
                        price = offerPrice.Min() - (decimal)0.01;
#else
                    price = offerPrice.Min() - (decimal)0.01;
#endif


                    QueueList.Add(new FeedTempDto()
                    {
                        Asin = asin,
                        CreateDate = DateTime.Now,
                        Price = price,
                        Status = "Active",
                        Sku = itemCondition == "new" ? NEW_PREFIX + asin : USED_PREFIX + asin,
                        SellerId = SELLER_ID,
                        Condition = itemCondition
                    });
                }
            }
            catch (Exception ex)
            {
                IsProcessCompleted = true;
                LogEventInfo logEvent = new LogEventInfo()
                {
                    Exception = ex,
                    Level = LogLevel.Error
                };
                logger.Log(logEvent);
            }
        }

        private void DeleteMessage(List<Message> messages)
        {
            List<DeleteMessageBatchRequestEntry> entries = new List<DeleteMessageBatchRequestEntry>();
            foreach (Message message in messages)
            {
                entries.Add(new DeleteMessageBatchRequestEntry()
                {
                    ReceiptHandle = message.ReceiptHandle,
                    Id = message.MessageId
                });
            }

            try
            {
                sqsClient.DeleteMessageBatch(SQS_URL, entries);
            }
            catch (Exception ex)
            {
            }
        }

        private decimal ToPercent(decimal value, int percent)
        {
            return value + (value * percent) / 100;
        }
    }
}