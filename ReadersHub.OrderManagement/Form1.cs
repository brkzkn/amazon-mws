using MarketplaceWebServiceOrders;
using MarketplaceWebServiceProducts;
using MarketplaceWebServiceProducts.Model;
using System;
using System.Collections.Generic;
using System.Linq;
using System.Windows.Forms;

namespace ReadersHub.OrderManagement
{
    public partial class Form1 : Form
    {
        private MarketplaceWebServiceOrdersClient Client;
        private MarketplaceWebServiceProductsClient ProductClient;
        public Form1()
        {
            InitializeComponent();
            button1_Click(null, null);
        }

        private void button1_Click(object sender, EventArgs e)
        {
            Client = InitializeClient();
            ProductClient = InitializeProductClient();
            List<string> marketPlaceId = new List<string>();
            marketPlaceId.Add("A1F83G8C2ARO7P");

            List<string> sellerId = new List<string>();
            sellerId.Add("");

            //var response = Client.ListOrders(new MarketplaceWebServiceOrders.Model.ListOrdersRequest()
            //{
            //    MarketplaceId = marketPlaceId,
            //    SellerId = "A309XP5TP81EH",
            //    CreatedAfter = DateTime.Now.AddDays(-10)
            //});

            var response = Client.ListOrderItems(new MarketplaceWebServiceOrders.Model.ListOrderItemsRequest()
            {
                SellerId = "A309XP5TP81EH",
                AmazonOrderId = "205-8956488-7457130"
            });
            List<string> asinList = response.ListOrderItemsResult.OrderItems.Select(x => x.SellerSKU).ToList();
            IdListType asins = new IdListType();
            asins.Id = asinList;

            var pRes = ProductClient.GetMatchingProductForId(new GetMatchingProductForIdRequest()
            {
                SellerId = "A309XP5TP81EH",
                MarketplaceId = "A1F83G8C2ARO7P",
                IdType = "SellerSKU",
                IdList = asins
            });
            int a = 0;
        }

        private MarketplaceWebServiceProductsClient InitializeProductClient()
        {
            if (ProductClient != null)
            {
                return ProductClient;
            }

            // The client application name
            string appName = "OrderManagement";

            // The client application version
            string appVersion = "1.0";

            // The endpoint for region service and version (see developer guide)
            // ex: https://mws.amazonservices.com
            string serviceURL = "https://mws.amazonservices.co.uk";

            // Create a configuration object
            MarketplaceWebServiceProductsConfig config = new MarketplaceWebServiceProductsConfig();
            config.ServiceURL = serviceURL;

            // Set other client connection configurations here if needed
            // Create the client itself
            string MWS_API_ACCESS_KEY_ID = "AKIAJUHDQT2353NL5IBQ";
            string MWS_API_SECRET_KEY = "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";

            return new MarketplaceWebServiceProductsClient(appName, appVersion, MWS_API_ACCESS_KEY_ID, MWS_API_SECRET_KEY, config);
        }

        private MarketplaceWebServiceOrdersClient InitializeClient()
        {
            if (Client != null)
            {
                return Client;
            }

            // The client application name
            string appName = "OrderManagement";

            // The client application version
            string appVersion = "1.0";

            // The endpoint for region service and version (see developer guide)
            // ex: https://mws.amazonservices.com
            string serviceURL = "https://mws.amazonservices.co.uk";

            // Create a configuration object
            MarketplaceWebServiceOrdersConfig config = new MarketplaceWebServiceOrdersConfig();
            config.ServiceURL = serviceURL;

            // Set other client connection configurations here if needed
            // Create the client itself
            string MWS_API_ACCESS_KEY_ID = "AKIAJUHDQT2353NL5IBQ";
            string MWS_API_SECRET_KEY = "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";

            return new MarketplaceWebServiceOrdersClient(MWS_API_ACCESS_KEY_ID, MWS_API_SECRET_KEY, appName, appVersion, config);
        }
    }
}
