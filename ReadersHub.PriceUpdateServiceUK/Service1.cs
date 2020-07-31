using _21stSolution.Extensions;
using Autofac;
using MarketplaceWebServiceProducts;
using MarketplaceWebServiceProducts.Model;
using NLog;
using ReadersHub.Business.Service.Criterions;
using ReadersHub.Business.Service.Product;
using ReadersHub.PriceUpdateServiceUK.Config;
using System;
using System.Collections.Generic;
using System.Configuration;
using System.IO;
using System.Linq;
using System.ServiceProcess;
using System.Timers;

namespace ReadersHub.PriceUpdateServiceUK
{
    public partial class Service1 : ServiceBase
    {
        private readonly IProductService _productService;
        private readonly ICriterionService _criterionService;

        private IContainer container;
        private static Logger logger;
        private readonly int INTERVAL;
        private readonly string SELLER_ID;
        private readonly string MARKETPLACE_ID;
        private readonly string CURRENCY_CODE;
        private MarketplaceWebServiceProducts.MarketplaceWebServiceProducts client;

        private Helper _helper;
        private Dictionary<string, string> Settings;

        private bool UpdateCriterion;
        private bool IsProcessCompleted;
        public Service1()
        {
            InitializeComponent();
            InitializeSettings();

            container = ContainerConfig.Configure();
            _criterionService = container.BeginLifetimeScope().Resolve<ICriterionService>();
            _productService = container.BeginLifetimeScope().Resolve<IProductService>();
            logger = LogManager.GetCurrentClassLogger();
            client = InitializeClient();

            SELLER_ID = Settings["SELLER_ID"];
            MARKETPLACE_ID = Settings["MARKET_PLACE_ID"];
            CURRENCY_CODE = Settings["CURRENCY_CODE"];
            INTERVAL = int.Parse(ConfigurationManager.AppSettings["HeartBeatIntervalSecond"]) * 1000;

            _helper = new Helper(_criterionService, SELLER_ID);

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

        protected override void OnStart(string[] args)
        {
        }

        protected override void OnStop()
        {
        }

        public void OnDebug()
        {
            logger.Log(LogLevel.Info, "SERVICE START");
            timer.Interval = INTERVAL;
            timer.Elapsed += Timer_Elapsed;
            timer.Enabled = true;
            timer.Start();

            UpdatePriceForUK();
            updateTimer.Interval = 30000;
            updateTimer.Elapsed += UpdateTimer_Elapsed;
            updateTimer.Enabled = true;
            updateTimer.Start();
        }

        private void Timer_Elapsed(object sender, ElapsedEventArgs e)
        {
            logger.Log(LogLevel.Info, "UPDATE CRITERION");
            UpdateCriterion = true;
        }

        private void UpdateTimer_Elapsed(object sender, ElapsedEventArgs e)
        {
            if (IsProcessCompleted)
            {
                UpdatePriceForUK();
            }
            else
            {
                logger.Log(LogLevel.Info, "PROCESS NOT FINISH");
            }
        }

        public void UpdatePriceForUK()
        {
            List<string> asinList = new List<string>();
            try
            {
                IsProcessCompleted = false;

                if (UpdateCriterion)
                {
                    _helper.InitializeCriterion();
                    UpdateCriterion = false;
                }
                // Create a request.
                var request = new GetLowestOfferListingsForASINRequest();
                request.SellerId = SELLER_ID;
                request.MarketplaceId = MARKETPLACE_ID;
                request.ExcludeMe = true;
                request.ItemCondition = "new";


                //asinList = _productService.GetISBNForUpdate();
                asinList.Add("0521004969");
                if (asinList.Count == 0)
                {
                    logger.Log(LogLevel.Info, "ASIN count value is 0");
                    IsProcessCompleted = true;
                    return;
                }

                request.ASINList = new ASINListType()
                {
                    ASIN = asinList.Distinct().Take(20).ToList(),
                };

                GetLowestOfferListingsForASINResponse result = client.GetLowestOfferListingsForASIN(request);
                if (result.GetLowestOfferListingsForASINResult != null)
                {
                    foreach (var item in result.GetLowestOfferListingsForASINResult)
                    {
                        if (item.Product == null)
                        {
                            /// TODO: Update_Time
                            /// 
                            _productService.UpdateProductPrice(item.ASIN, CURRENCY_CODE, -1, -1, false);
                            continue;
                        }

                        if (item.Product.LowestOfferListings.LowestOfferListing.Count > 0)
                        {
                            decimal isbnPrice =
                                item.Product.LowestOfferListings.LowestOfferListing.Min(x => x.Price.ListingPrice.Amount);
                            
                            //foreach (var lowestPrice in item.Product.LowestOfferListings.LowestOfferListing)
                            //{
                            //    isbnPrice = lowestPrice.Price.ListingPrice.Amount;
                            //    string feedbackRating = "0";
                            //    if (!string.IsNullOrEmpty(lowestPrice.Qualifiers.SellerPositiveFeedbackRating))
                            //    {
                            //        feedbackRating = lowestPrice.Qualifiers.SellerPositiveFeedbackRating.Substring(0, 2);
                            //        if (lowestPrice.Qualifiers.SellerPositiveFeedbackRating.Length > 4)
                            //        {
                            //            feedbackRating = lowestPrice.Qualifiers.SellerPositiveFeedbackRating.Substring(3, 2);
                            //        }
                            //    }
                            //    string feedbackCount = lowestPrice.SellerFeedbackCount.ToString();
                            //    string subcondition = lowestPrice.Qualifiers.ItemSubcondition.ToLower();
                            //    /// Kriterlere uyan en düşük fiyatlı ürünü bulunca döngüyü sonlandırıyoruz.
                            //    /// 
                            //    //if (_helper.CheckCriteria(feedbackRating, feedbackCount, "", subcondition, ""))
                            //    //{
                            //    //    break;
                            //    //}
                            //}
                            var asinPrice = _helper.GetMinimumPrice(isbnPrice, false);
                            _productService.UpdateProductPrice(item.ASIN, CURRENCY_CODE, isbnPrice, asinPrice, false);
                        }
                        else
                        {
                            _productService.UpdateProductPrice(item.ASIN, CURRENCY_CODE, -1, -1, false);
                        }
                    }
                }
                //System.Threading.Thread.Sleep(1000);
                request.ItemCondition = "used";
                result = client.GetLowestOfferListingsForASIN(request);
                if (result.GetLowestOfferListingsForASINResult != null)
                {
                    foreach (var item in result.GetLowestOfferListingsForASINResult)
                    {
                        if (item.Product == null)
                        {
                            /// TODO: Update_Time
                            /// 
                            _productService.UpdateProductPrice(item.ASIN, CURRENCY_CODE, -1, -1, false);
                            continue;
                        }

                        if (item.Product.LowestOfferListings.LowestOfferListing.Count > 0)
                        {
                            decimal isbnPrice =
                                item.Product.LowestOfferListings.LowestOfferListing.Min(x => x.Price.ListingPrice.Amount);

                            //foreach (var lowestPrice in item.Product.LowestOfferListings.LowestOfferListing)
                            //{
                            //    isbnPrice = lowestPrice.Price.ListingPrice.Amount;
                            //    string feedbackRating = "0";
                            //    if (!string.IsNullOrEmpty(lowestPrice.Qualifiers.SellerPositiveFeedbackRating))
                            //    {
                            //        feedbackRating = lowestPrice.Qualifiers.SellerPositiveFeedbackRating.Substring(0, 2);
                            //        if (lowestPrice.Qualifiers.SellerPositiveFeedbackRating.Length > 4)
                            //        {
                            //            feedbackRating = lowestPrice.Qualifiers.SellerPositiveFeedbackRating.Substring(3, 2);
                            //        }
                            //    }
                            //    string feedbackCount = lowestPrice.SellerFeedbackCount.ToString();
                            //    string subcondition = lowestPrice.Qualifiers.ItemSubcondition.ToLower();
                            //    //if (_helper.CheckCriteria(feedbackRating, feedbackCount, "", subcondition, ""))
                            //    //{
                            //    //    break;
                            //    //}
                            //}

                            var asinPrice = _helper.GetMinimumPrice(isbnPrice, true);
                            _productService.UpdateProductPrice(item.ASIN, CURRENCY_CODE, isbnPrice, asinPrice, true);
                        }
                        else
                        {
                            _productService.UpdateProductPrice(item.ASIN, CURRENCY_CODE, -1, -1, true);
                        }
                    }
                }
                IsProcessCompleted = true;
            }
            catch (Exception ex)
            {
                IsProcessCompleted = true;

                /// TODO: Bu durumda ne yapacağız?
                /// 
                LogEventInfo logInfo = new LogEventInfo()
                {
                    Exception = ex,
                    Level = LogLevel.Error
                };
                logger.Log(logInfo);
                if (asinList.Count > 0)
                {
                    foreach (var asin in asinList)
                    {
                        _productService.UpdateProductPrice(asin, CURRENCY_CODE, -1, -1, true);
                    }
                }

                if (!ex.Message.IsNullOrEmpty() && ex.Message.Contains(" is not a valid ASIN"))
                {

                    var isbn = ex.Message.Remove(10);
                    _productService.Delete(isbn);
                }
            }
        }

        private MarketplaceWebServiceProducts.MarketplaceWebServiceProducts InitializeClient()
        {
            if (client != null)
            {
                return client;
            }

            // The client application name
            string appName = "CSharpSampleCode";

            // The client application version
            string appVersion = "1.0";

            // The endpoint for region service and version (see developer guide)
            // ex: https://mws.amazonservices.com
            string serviceURL = Settings["MWS_DESTINATION"];

            // Create a configuration object
            MarketplaceWebServiceProductsConfig config = new MarketplaceWebServiceProductsConfig();
            config.ServiceURL = serviceURL;

            // Set other client connection configurations here if needed
            // Create the client itself
            string MWS_API_ACCESS_KEY_ID = Settings["ACCESS_KEY_ID"];
            string MWS_API_SECRET_KEY = Settings["SECRET_KEY"];

            return new MarketplaceWebServiceProductsClient(appName, appVersion, MWS_API_ACCESS_KEY_ID, MWS_API_SECRET_KEY, config);
        }
    }
}
