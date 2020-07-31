/*******************************************************************************
 * Copyright 2009-2016 Amazon Services. All Rights Reserved.
 * Licensed under the Apache License, Version 2.0 (the "License"); 
 *
 * You may not use this file except in compliance with the License. 
 * You may obtain a copy of the License at: http://aws.amazon.com/apache2.0
 * This file is distributed on an "AS IS" BASIS, WITHOUT WARRANTIES OR 
 * CONDITIONS OF ANY KIND, either express or implied. See the License for the 
 * specific language governing permissions and limitations under the License.
 *******************************************************************************
 * Marketplace Web Service Products
 * API Version: 2011-10-01
 * Library Version: 2016-06-01
 * Generated: Mon Jun 13 10:07:51 PDT 2016
 */

using MarketplaceWebServiceProducts.Model;
using System;
using System.Collections.Generic;

namespace MarketplaceWebServiceProducts {

    /// <summary>
    /// Runnable sample code to demonstrate usage of the C# client.
    ///
    /// To use, import the client source as a console application,
    /// and mark this class as the startup object. Then, replace
    /// parameters below with sensible values and run.
    /// </summary>
    public class MarketplaceWebServiceProductsSample {

        private readonly MarketplaceWebServiceProducts client;

        public MarketplaceWebServiceProductsSample(MarketplaceWebServiceProducts client)
        {
            this.client = client;
        }

        public GetCompetitivePricingForASINResponse InvokeGetCompetitivePricingForASIN()
        {
            // Create a request.
            GetCompetitivePricingForASINRequest request = new GetCompetitivePricingForASINRequest();
            string sellerId = "example";
            request.SellerId = sellerId;
            string mwsAuthToken = "example";
            request.MWSAuthToken = mwsAuthToken;
            string marketplaceId = "example";
            request.MarketplaceId = marketplaceId;
            ASINListType asinList = new ASINListType();
            request.ASINList = asinList;
            return this.client.GetCompetitivePricingForASIN(request);
        }

        public GetCompetitivePricingForSKUResponse InvokeGetCompetitivePricingForSKU()
        {
            // Create a request.
            GetCompetitivePricingForSKURequest request = new GetCompetitivePricingForSKURequest();
            string sellerId = "example";
            request.SellerId = sellerId;
            string mwsAuthToken = "example";
            request.MWSAuthToken = mwsAuthToken;
            string marketplaceId = "example";
            request.MarketplaceId = marketplaceId;
            SellerSKUListType sellerSKUList = new SellerSKUListType();
            request.SellerSKUList = sellerSKUList;
            return this.client.GetCompetitivePricingForSKU(request);
        }

        public GetLowestOfferListingsForASINResponse InvokeGetLowestOfferListingsForASIN(string sellerId, string marketplaceId, List<string> asinList, string itemCondition)
        {
            // Create a request.
            GetLowestOfferListingsForASINRequest request = new GetLowestOfferListingsForASINRequest();
            request.SellerId = sellerId;
            request.MarketplaceId = marketplaceId;
            request.ItemCondition = itemCondition;
            request.ExcludeMe = true;

            request.ASINList = new Model.ASINListType()
            {
                ASIN = asinList,
            };
            

            return this.client.GetLowestOfferListingsForASIN(request);
        }

        public GetLowestOfferListingsForSKUResponse InvokeGetLowestOfferListingsForSKU()
        {
            // Create a request.
            GetLowestOfferListingsForSKURequest request = new GetLowestOfferListingsForSKURequest();
            string sellerId = "example";
            request.SellerId = sellerId;
            string mwsAuthToken = "example";
            request.MWSAuthToken = mwsAuthToken;
            string marketplaceId = "example";
            request.MarketplaceId = marketplaceId;
            SellerSKUListType sellerSKUList = new SellerSKUListType();
            request.SellerSKUList = sellerSKUList;
            string itemCondition = "example";
            request.ItemCondition = itemCondition;
            bool excludeMe = true;
            request.ExcludeMe = excludeMe;
            return this.client.GetLowestOfferListingsForSKU(request);
        }

        public GetLowestPricedOffersForASINResponse InvokeGetLowestPricedOffersForASIN(string sellerId, string marketplaceId, string asin, string itemCondition)
        {
            // Create a request.
            GetLowestPricedOffersForASINRequest request = new GetLowestPricedOffersForASINRequest();
            request.SellerId = sellerId;
            request.MarketplaceId = marketplaceId;
            request.ASIN = asin;
            request.ItemCondition = itemCondition;

            return this.client.GetLowestPricedOffersForASIN(request);
        }

        public GetLowestPricedOffersForSKUResponse InvokeGetLowestPricedOffersForSKU()
        {
            // Create a request.
            GetLowestPricedOffersForSKURequest request = new GetLowestPricedOffersForSKURequest();
            string sellerId = "example";
            request.SellerId = sellerId;
            string mwsAuthToken = "example";
            request.MWSAuthToken = mwsAuthToken;
            string marketplaceId = "example";
            request.MarketplaceId = marketplaceId;
            string sellerSKU = "example";
            request.SellerSKU = sellerSKU;
            string itemCondition = "example";
            request.ItemCondition = itemCondition;
            return this.client.GetLowestPricedOffersForSKU(request);
        }

        public GetMatchingProductResponse InvokeGetMatchingProduct()
        {
            // Create a request.
            GetMatchingProductRequest request = new GetMatchingProductRequest();
            string sellerId = "example";
            request.SellerId = sellerId;
            string mwsAuthToken = "example";
            request.MWSAuthToken = mwsAuthToken;
            string marketplaceId = "example";
            request.MarketplaceId = marketplaceId;
            ASINListType asinList = new ASINListType();
            request.ASINList = asinList;
            return this.client.GetMatchingProduct(request);
        }

        public GetMatchingProductForIdResponse InvokeGetMatchingProductForId()
        {
            // Create a request.
            GetMatchingProductForIdRequest request = new GetMatchingProductForIdRequest();
            string sellerId = "example";
            request.SellerId = sellerId;
            string mwsAuthToken = "example";
            request.MWSAuthToken = mwsAuthToken;
            string marketplaceId = "example";
            request.MarketplaceId = marketplaceId;
            string idType = "example";
            request.IdType = idType;
            IdListType idList = new IdListType();
            request.IdList = idList;
            return this.client.GetMatchingProductForId(request);
        }

        public GetMyFeesEstimateResponse InvokeGetMyFeesEstimate()
        {
            // Create a request.
            GetMyFeesEstimateRequest request = new GetMyFeesEstimateRequest();
            string sellerId = "example";
            request.SellerId = sellerId;
            string mwsAuthToken = "example";
            request.MWSAuthToken = mwsAuthToken;
            FeesEstimateRequestList feesEstimateRequestList = new FeesEstimateRequestList();
            request.FeesEstimateRequestList = feesEstimateRequestList;
            return this.client.GetMyFeesEstimate(request);
        }

        public GetMyPriceForASINResponse InvokeGetMyPriceForASIN()
        {
            // Create a request.
            GetMyPriceForASINRequest request = new GetMyPriceForASINRequest();
            string sellerId = "example";
            request.SellerId = sellerId;
            string mwsAuthToken = "example";
            request.MWSAuthToken = mwsAuthToken;
            string marketplaceId = "example";
            request.MarketplaceId = marketplaceId;
            ASINListType asinList = new ASINListType();
            request.ASINList = asinList;
            return this.client.GetMyPriceForASIN(request);
        }

        public GetMyPriceForSKUResponse InvokeGetMyPriceForSKU()
        {
            // Create a request.
            GetMyPriceForSKURequest request = new GetMyPriceForSKURequest();
            string sellerId = "example";
            request.SellerId = sellerId;
            string mwsAuthToken = "example";
            request.MWSAuthToken = mwsAuthToken;
            string marketplaceId = "example";
            request.MarketplaceId = marketplaceId;
            SellerSKUListType sellerSKUList = new SellerSKUListType();
            request.SellerSKUList = sellerSKUList;
            return this.client.GetMyPriceForSKU(request);
        }

        public GetProductCategoriesForASINResponse InvokeGetProductCategoriesForASIN()
        {
            // Create a request.
            GetProductCategoriesForASINRequest request = new GetProductCategoriesForASINRequest();
            string sellerId = "example";
            request.SellerId = sellerId;
            string mwsAuthToken = "example";
            request.MWSAuthToken = mwsAuthToken;
            string marketplaceId = "example";
            request.MarketplaceId = marketplaceId;
            string asin = "example";
            request.ASIN = asin;
            return this.client.GetProductCategoriesForASIN(request);
        }

        public GetProductCategoriesForSKUResponse InvokeGetProductCategoriesForSKU()
        {
            // Create a request.
            GetProductCategoriesForSKURequest request = new GetProductCategoriesForSKURequest();
            string sellerId = "example";
            request.SellerId = sellerId;
            string mwsAuthToken = "example";
            request.MWSAuthToken = mwsAuthToken;
            string marketplaceId = "example";
            request.MarketplaceId = marketplaceId;
            string sellerSKU = "example";
            request.SellerSKU = sellerSKU;
            return this.client.GetProductCategoriesForSKU(request);
        }

        public GetServiceStatusResponse InvokeGetServiceStatus()
        {
            // Create a request.
            GetServiceStatusRequest request = new GetServiceStatusRequest();
            string sellerId = "example";
            request.SellerId = sellerId;
            string mwsAuthToken = "example";
            request.MWSAuthToken = mwsAuthToken;
            return this.client.GetServiceStatus(request);
        }

        public ListMatchingProductsResponse InvokeListMatchingProducts()
        {
            // Create a request.
            ListMatchingProductsRequest request = new ListMatchingProductsRequest();
            string sellerId = "example";
            request.SellerId = sellerId;
            string mwsAuthToken = "example";
            request.MWSAuthToken = mwsAuthToken;
            string marketplaceId = "example";
            request.MarketplaceId = marketplaceId;
            string query = "example";
            request.Query = query;
            string queryContextId = "example";
            request.QueryContextId = queryContextId;
            return this.client.ListMatchingProducts(request);
        }


    }
}
