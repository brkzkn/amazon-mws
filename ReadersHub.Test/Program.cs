using F23.StringSimilarity;
using MWSSubscriptionsService;
using MWSSubscriptionsService.Model;
using System;
using System.Globalization;

namespace ReadersHub.Test
{
    class Program
    {
        //private const string MWS_API_ACCESS_KEY_ID = "AKIAIK7LXITZLKXJSRFA";
        //private const string MWS_API_SECRET_KEY = "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";
        //private const string MWS_API_SELLER_ID = "APXCOATCYJK17";
        //private const string MWS_API_MARKET_PLACE_ID = "ATVPDKIKX0DER";
        //private const string MWS_DESTINATION = "https://mws.amazonservices.com";

        private const string MWS_API_SELLER_ID = "A1PGAFULU9K5EM";
        private const string MWS_API_MARKET_PLACE_ID = "A1F83G8C2ARO7P";
        private const string MWS_API_ACCESS_KEY_ID = "AKIAIKBB2MWFK2PB4RRA";
        private const string MWS_API_SECRET_KEY = "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";
        private const string MWS_DESTINATION = "https://mws.amazonservices.co.uk";

        //private const string SqsUrl = "https://sqs.us-west-2.amazonaws.com/070692396532/burak";
        //private const string SqsUrl = "https://sqs.us-west-2.amazonaws.com/305507537056/toffeeuk";
        //private const string SqsUrl = "https://sqs.us-east-2.amazonaws.com/821316882071/sqs_albatros";
        private const string SqsUrl = "https://sqs.us-west-2.amazonaws.com/243824410737/giftus";

        static void Main(string[] args)
        {
            // The client application name
            string appName = "CSharpSampleCode";

            // The client application version
            string appVersion = "1.0";

            // The endpoint for region service and version (see developer guide)
            // ex: https://mws.amazonservices.com

            // Create a configuration object
            var config = new MWSSubscriptionsServiceConfig();
            config.ServiceURL = MWS_DESTINATION;
            // Set other client connection configurations here if needed
            // Create the client itself
            var client = new MWSSubscriptionsServiceClient(MWS_API_ACCESS_KEY_ID, MWS_API_SECRET_KEY, appName, appVersion, config);

            var sample = new MWSSubscriptionsServiceSample(client);
            try
            {
                IMWSResponse response = null;
                // response = sample.InvokeDeleteSubscription(MWS_API_SELLER_ID, MWS_API_MARKET_PLACE_ID, SqsUrl);
                // response = sample.InvokeDeregisterDestination(MWS_API_SELLER_ID, MWS_API_MARKET_PLACE_ID, SqsUrl);

                // response = sample.InvokeCreateSubscription(MWS_API_SELLER_ID, MWS_API_MARKET_PLACE_ID, SqsUrl);
                // response = sample.InvokeRegisterDestination(MWS_API_SELLER_ID, MWS_API_MARKET_PLACE_ID, SqsUrl);

                // response = sample.InvokeListRegisteredDestinations(MWS_API_SELLER_ID, MWS_API_MARKET_PLACE_ID);
                // response = sample.InvokeListSubscriptions(MWS_API_SELLER_ID, MWS_API_MARKET_PLACE_ID);

                // response = sample.InvokeGetSubscription(MWS_API_SELLER_ID, MWS_API_MARKET_PLACE_ID, "AnyOfferChanged", SqsUrl);
                // response = sample.InvokeGetServiceStatus(MWS_API_SELLER_ID, MWS_API_MARKET_PLACE_ID);

                response = sample.InvokeSendTestNotificationToDestination(MWS_API_SELLER_ID, MWS_API_MARKET_PLACE_ID, SqsUrl);
                // response = sample.InvokeUpdateSubscription(MWS_API_SELLER_ID, MWS_API_MARKET_PLACE_ID, SqsUrl);


                Console.WriteLine("Response:");
                ResponseHeaderMetadata rhmd = response.ResponseHeaderMetadata;
                // We recommend logging the request id and timestamp of every call.
                Console.WriteLine("RequestId: " + rhmd.RequestId);
                Console.WriteLine("Timestamp: " + rhmd.Timestamp);
                string responseXml = response.ToXML();
                Console.WriteLine(responseXml);
            }
            catch (MWSSubscriptionsServiceException ex)
            {
                // Exception properties are important for diagnostics.
                ResponseHeaderMetadata rhmd = ex.ResponseHeaderMetadata;
                Console.WriteLine("Service Exception:");
                if (rhmd != null)
                {
                    Console.WriteLine("RequestId: " + rhmd.RequestId);
                    Console.WriteLine("Timestamp: " + rhmd.Timestamp);
                }
                Console.WriteLine("Message: " + ex.Message);
                Console.WriteLine("StatusCode: " + ex.StatusCode);
                Console.WriteLine("ErrorCode: " + ex.ErrorCode);
                Console.WriteLine("ErrorType: " + ex.ErrorType);
                throw ex;
            }
        }


        //static void Main(string[] args)
        //{
        //    // TODO: Set the below configuration variables before attempting to run


        //    // The client application name
        //    string appName = "CSharpSampleCode";

        //    // The client application version
        //    string appVersion = "1.0";

        //    // The endpoint for region service and version (see developer guide)
        //    // ex: https://mws.amazonservices.com
        //    string serviceURL = MWS_DESTINATION;

        //    // Create a configuration object
        //    MarketplaceWebServiceProductsConfig config = new MarketplaceWebServiceProductsConfig();
        //    config.ServiceURL = serviceURL;
        //    // Set other client connection configurations here if needed
        //    // Create the client itself
        //    MarketplaceWebServiceProducts.MarketplaceWebServiceProducts client = new MarketplaceWebServiceProductsClient(appName, appVersion, MWS_API_ACCESS_KEY_ID, MWS_API_SECRET_KEY, config);

        //    MarketplaceWebServiceProductsSample sample = new MarketplaceWebServiceProductsSample(client);

        //    // Uncomment the operation you'd like to test here
        //    // TODO: Modify the request created in the Invoke method to be valid

        //    try
        //    {
        //        //MarketplaceWebServiceProducts.Model.IMWSResponse response = null;
        //        // response = sample.InvokeGetCompetitivePricingForASIN();
        //        // response = sample.InvokeGetCompetitivePricingForSKU();
        //        var asinList = new List<string>();
        //        asinList.Add("0684802317");
        //        asinList.Add("0785821910");

        //        MarketplaceWebServiceProducts.Model.GetLowestOfferListingsForASINResponse response = sample.InvokeGetLowestOfferListingsForASIN(MWS_API_SELLER_ID, MWS_API_MARKET_PLACE_ID, asinList, "new");
        //        // response = sample.InvokeGetLowestOfferListingsForSKU();

        //        // response = sample.InvokeGetLowestPricedOffersForASIN(MWS_API_SELLER_ID, MWS_API_MARKET_PLACE_ID, "0684802317", "used");
        //        // response = sample.InvokeGetLowestPricedOffersForSKU();
        //        // response = sample.InvokeGetMatchingProduct();
        //        // response = sample.InvokeGetMatchingProductForId();
        //        // response = sample.InvokeGetMyFeesEstimate();
        //        // response = sample.InvokeGetMyPriceForASIN();
        //        // response = sample.InvokeGetMyPriceForSKU();
        //        // response = sample.InvokeGetProductCategoriesForASIN();
        //        // response = sample.InvokeGetProductCategoriesForSKU();
        //        // response = sample.InvokeGetServiceStatus();
        //        // response = sample.InvokeListMatchingProducts();
        //        Console.WriteLine("Response:");
        //        MarketplaceWebServiceProducts.Model.ResponseHeaderMetadata rhmd = response.ResponseHeaderMetadata;
        //        // We recommend logging the request id and timestamp of every call.
        //        Console.WriteLine("RequestId: " + rhmd.RequestId);
        //        Console.WriteLine("Timestamp: " + rhmd.Timestamp);
        //        string responseXml = response.ToXML();
        //        Console.WriteLine(responseXml);
        //    }
        //    catch (MarketplaceWebServiceProductsException ex)
        //    {
        //        // Exception properties are important for diagnostics.
        //        MarketplaceWebServiceProducts.Model.ResponseHeaderMetadata rhmd = ex.ResponseHeaderMetadata;
        //        Console.WriteLine("Service Exception:");
        //        if (rhmd != null)
        //        {
        //            Console.WriteLine("RequestId: " + rhmd.RequestId);
        //            Console.WriteLine("Timestamp: " + rhmd.Timestamp);
        //        }
        //        Console.WriteLine("Message: " + ex.Message);
        //        Console.WriteLine("StatusCode: " + ex.StatusCode);
        //        Console.WriteLine("ErrorCode: " + ex.ErrorCode);
        //        Console.WriteLine("ErrorType: " + ex.ErrorType);
        //        throw ex;
        //    }
        //}

        static int ComputeLevenshteinDistance(string source, string target)
        {
            if ((source == null) || (target == null)) return 0;
            if ((source.Length == 0) || (target.Length == 0)) return 0;
            if (source == target) return source.Length;

            int sourceWordCount = source.Length;
            int targetWordCount = target.Length;

            // Step 1
            if (sourceWordCount == 0)
                return targetWordCount;

            if (targetWordCount == 0)
                return sourceWordCount;

            int[,] distance = new int[sourceWordCount + 1, targetWordCount + 1];

            // Step 2
            for (int i = 0; i <= sourceWordCount; distance[i, 0] = i++) ;
            for (int j = 0; j <= targetWordCount; distance[0, j] = j++) ;

            for (int i = 1; i <= sourceWordCount; i++)
            {
                for (int j = 1; j <= targetWordCount; j++)
                {
                    // Step 3
                    int cost = (target[j - 1] == source[i - 1]) ? 0 : 1;

                    // Step 4
                    distance[i, j] = Math.Min(Math.Min(distance[i - 1, j] + 1, distance[i, j - 1] + 1), distance[i - 1, j - 1] + cost);
                }
            }

            return distance[sourceWordCount, targetWordCount];
        }

        static double CalculateSimilarity(string source, string target)
        {
            if ((source == null) || (target == null)) return 0.0;
            if ((source.Length == 0) || (target.Length == 0)) return 0.0;
            if (source == target) return 1.0;

            int stepsToSame = ComputeLevenshteinDistance(source, target);
            return (1.0 - ((double)stepsToSame / (double)Math.Max(source.Length, target.Length)));
        }

    }
}
