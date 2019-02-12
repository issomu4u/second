/*******************************************************************************
 * Copyright 2009-2017 Amazon Services. All Rights Reserved.
 * Licensed under the Apache License, Version 2.0 (the "License"); 
 *
 * You may not use this file except in compliance with the License. 
 * You may obtain a copy of the License at: http://aws.amazon.com/apache2.0
 * This file is distributed on an "AS IS" BASIS, WITHOUT WARRANTIES OR 
 * CONDITIONS OF ANY KIND, either express or implied. See the License for the 
 * specific language governing permissions and limitations under the License.
 *******************************************************************************
 * Marketplace Web Service Orders
 * API Version: 2013-09-01
 * Library Version: 2017-02-22
 * Generated: Thu Mar 02 12:41:05 UTC 2017
 */

using MarketplaceWebServiceOrders.Model;
using System;
using System.Collections.Generic;
//using Seller_Amazon_API.Models;
using SellerVendor.Areas.Seller.Models.DBContext;

using System.Linq;
using SellerVendor.Models;
using SellerVendor.Areas.Seller.Models;

namespace MarketplaceWebServiceOrders {

    /// <summary>
    /// Runnable sample code to demonstrate usage of the C# client.
    ///
    /// To use, import the client source as a console application,
    /// and mark this class as the startup object. Then, replace
    /// parameters below with sensible values and run.
    /// </summary>
    public class MarketplaceWebServiceOrdersSample {

        public static SellerAdminContext admin_db = new SellerAdminContext();
        public static Seller_Market seller_db = new Seller_Market();

        public static void Main()//(string[] args)
        {
            var seller_List_Admin= admin_db.tbl_sellers.Where(a => a.isactive == 1).ToList();
            if (seller_List_Admin != null)
            {
                foreach (var item in seller_List_Admin)
                {
                    int strId = Convert.ToInt32(item.id.ToString());
                    var seller_Market_List = seller_db.tbl_sellermarketplace.Where(a => a.isactive == 1 && a.tbl_seller_id == strId).ToList();

                    foreach (var itemMkt in seller_Market_List)
                    {
                        string strMktId = itemMkt.id.ToString();
                        string my_unique_id = itemMkt.my_unique_id.ToString();
                        string t_loginName = itemMkt.t_loginName.ToString();
                        string t_password = itemMkt.t_password.ToString();
                        string t_access_Key_id = itemMkt.t_access_Key_id.ToString();
                        string t_auth_token = itemMkt.t_auth_token.ToString();
                        string t_secret_Key = itemMkt.t_secret_Key.ToString();
                        string market_palce_id = itemMkt.market_palce_id.ToString();
                    }
                }
            }
            
            // TODO: Set the below configuration variables before attempting to run

            // Developer AWS access key
            string accessKey = "AKIAJ2LOHS2WG5TTUTHQ"; // "replaceWithAccessKey";

            // Developer AWS secret key
            string secretKey = "bk3xsBJLL2CH6BXyc2jRev+0Lf3Mg8aDZM4jHrht";// "replaceWithSecretKey";

            // The client application name
            string appName = "AmazonJavascriptScratchpad"; // "CSharpSampleCode";

            // The client application version
            string appVersion = "1.0";

            // The endpoint for region service and version (see developer guide)
            // ex: https://mws.amazonservices.com
            string serviceURL = "https://mws.amazonservices.in"; // "replaceWithServiceURL";

            // Create a configuration object
            MarketplaceWebServiceOrdersConfig config = new MarketplaceWebServiceOrdersConfig();
            config.ServiceURL = serviceURL;
            // Set other client connection configurations here if needed
            // Create the client itself
            MarketplaceWebServiceOrders client = new MarketplaceWebServiceOrdersClient(accessKey, secretKey, appName, appVersion, config);

            MarketplaceWebServiceOrdersSample sample = new MarketplaceWebServiceOrdersSample(client);

            // Uncomment the operation you'd like to test here
            // TODO: Modify the request created in the Invoke method to be valid

            try 
            {
                IMWSResponse response = null;
                // response = sample.InvokeGetOrder();
                // response = sample.InvokeGetServiceStatus();
                 response = sample.InvokeListOrderItems();
                // response = sample.InvokeListOrderItemsByNextToken();
                 response = sample.InvokeListOrders();
                // response = sample.InvokeListOrdersByNextToken();
                Console.WriteLine("Response:");
                ResponseHeaderMetadata rhmd = response.ResponseHeaderMetadata;
                // We recommend logging the request id and timestamp of every call.
                Console.WriteLine("RequestId: " + rhmd.RequestId);
                Console.WriteLine("Timestamp: " + rhmd.Timestamp);
                string responseXml = response.ToXML();
                Console.WriteLine(responseXml);
            }
            catch (MarketplaceWebServiceOrdersException ex)
            {
                // Exception properties are important for diagnostics.
                ResponseHeaderMetadata rhmd = ex.ResponseHeaderMetadata;
                Console.WriteLine("Service Exception:");
                if(rhmd != null)
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

        private readonly MarketplaceWebServiceOrders client;

        public MarketplaceWebServiceOrdersSample(MarketplaceWebServiceOrders client)
        {
            this.client = client;
        }

        public GetOrderResponse InvokeGetOrder()
        {
            // Create a request.
            GetOrderRequest request = new GetOrderRequest();
            string sellerId = "A215Z7QR1LOVAK"; // "example";
            request.SellerId = sellerId;
            string mwsAuthToken = ""; // "example";
            request.MWSAuthToken = mwsAuthToken;
            List<string> amazonOrderId = new List<string>();
            //amazonOrderId.Add("408-4800687-5969928");
            request.AmazonOrderId = amazonOrderId;
            return this.client.GetOrder(request);
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

        public ListOrderItemsResponse InvokeListOrderItems_Amazon(string sellerId, string mwsAuthToken, string amazonOrderId)
        {
            // Create a request.
            ListOrderItemsRequest request = new ListOrderItemsRequest();
            //string sellerId = "example";
            request.SellerId = sellerId;
            //string mwsAuthToken = "example";
            request.MWSAuthToken = mwsAuthToken;
            //string amazonOrderId = "example";
            request.AmazonOrderId = amazonOrderId;
            return this.client.ListOrderItems(request);
        }

        public ListOrderItemsResponse InvokeListOrderItems()
        {
            // Create a request.
            ListOrderItemsRequest request = new ListOrderItemsRequest();
            string sellerId = "example";
            request.SellerId = sellerId;
            string mwsAuthToken = "example";
            request.MWSAuthToken = mwsAuthToken;
            string amazonOrderId = "example";
            request.AmazonOrderId = amazonOrderId;
            return this.client.ListOrderItems(request);
        }

        public ListOrderItemsByNextTokenResponse InvokeListOrderItemsByNextToken()
        {
            // Create a request.
            ListOrderItemsByNextTokenRequest request = new ListOrderItemsByNextTokenRequest();
            string sellerId = "example";
            request.SellerId = sellerId;
            string mwsAuthToken = "example";
            request.MWSAuthToken = mwsAuthToken;
            string nextToken = "example";
            request.NextToken = nextToken;
            return this.client.ListOrderItemsByNextToken(request);
        }

        public ListOrdersResponse InvokeListOrders_Amazon(DateTime createA, DateTime createB, string marketplaceid, string Seller_ID)
        {
            //api_parameter objParam = new api_parameter();
            ListOrdersRequest request = new ListOrdersRequest();
            request.SellerId = Seller_ID;
            request.CreatedAfter = createA;
            request.CreatedBefore = createB;
            List<string> marketplaceId = new List<string>();
            marketplaceId.Add(marketplaceid);
            request.MarketplaceId = marketplaceId;
            return this.client.ListOrders(request);
        }

        public ListOrdersResponse InvokeListOrders()
        {
            // Create a request.
            ListOrdersRequest request = new ListOrdersRequest();
            string sellerId = "A215Z7QR1LOVAK"; // "example";
            request.SellerId = sellerId;
            string mwsAuthToken = ""; // "example";
            request.MWSAuthToken = mwsAuthToken;
            DateTime createdAfter = new DateTime();
            request.CreatedAfter = System.DateTime.Now.AddMonths(-2);// createdAfter;
            DateTime createdBefore = new DateTime();
            request.CreatedBefore = System.DateTime.Now.AddMinutes(-5);//  createdBefore;
            DateTime lastUpdatedAfter = new DateTime();
            //request.LastUpdatedAfter = lastUpdatedAfter;
            DateTime lastUpdatedBefore = new DateTime();
            //request.LastUpdatedBefore = lastUpdatedBefore;
            List<string> orderStatus = new List<string>();
            //request.OrderStatus = orderStatus;
            List<string> marketplaceId = new List<string>();
            marketplaceId.Add("A21TJRUUN4KGV");
            request.MarketplaceId = marketplaceId;
            List<string> fulfillmentChannel = new List<string>();
            //request.FulfillmentChannel = fulfillmentChannel;
            List<string> paymentMethod = new List<string>();
            //request.PaymentMethod = paymentMethod;
            string buyerEmail = "";
            //request.BuyerEmail = buyerEmail;
            string sellerOrderId = "";
            //request.SellerOrderId = sellerOrderId;
            decimal maxResultsPerPage = 1;
            //request.MaxResultsPerPage = maxResultsPerPage;
            List<string> tfmShipmentStatus = new List<string>();
            //request.TFMShipmentStatus = tfmShipmentStatus;
            return this.client.ListOrders(request);
        }

        public ListOrdersResponse InvokeListOrders1()
        {
            // Create a request.
            ListOrdersRequest request = new ListOrdersRequest();
            string sellerId = "A215Z7QR1LOVAK"; // "example";
            request.SellerId = sellerId;
            string mwsAuthToken = ""; // "example";
            request.MWSAuthToken = mwsAuthToken;
            DateTime createdAfter = new DateTime();
            request.CreatedAfter = System.DateTime.Now.AddHours(-24); // createdAfter;
            DateTime createdBefore = new DateTime();
            request.CreatedBefore = System.DateTime.Now.AddMinutes(-5); // createdBefore;
            //DateTime lastUpdatedAfter = new DateTime();
            //request.LastUpdatedAfter = lastUpdatedAfter;
            //DateTime lastUpdatedBefore = new DateTime();
            //request.LastUpdatedBefore = lastUpdatedBefore;
            List<string> orderStatus = new List<string>();
            request.OrderStatus = orderStatus;
            List<string> marketplaceId = new List<string>();
            marketplaceId.Add("A21TJRUUN4KGV");
            request.MarketplaceId = marketplaceId;
            List<string> fulfillmentChannel = new List<string>();
            request.FulfillmentChannel = fulfillmentChannel;
            List<string> paymentMethod = new List<string>();
            request.PaymentMethod = paymentMethod;
            string buyerEmail = "";
            request.BuyerEmail = buyerEmail;
            string sellerOrderId = "";
            request.SellerOrderId = sellerOrderId;
            decimal maxResultsPerPage = 1;
            request.MaxResultsPerPage = maxResultsPerPage;
            List<string> tfmShipmentStatus = new List<string>();
            request.TFMShipmentStatus = tfmShipmentStatus;
            return this.client.ListOrders(request);
        }

        public ListOrdersByNextTokenResponse InvokeListOrdersByNextToken()
        {
            // Create a request.
            ListOrdersByNextTokenRequest request = new ListOrdersByNextTokenRequest();
            string sellerId = "example";
            request.SellerId = sellerId;
            string mwsAuthToken = "example";
            request.MWSAuthToken = mwsAuthToken;
            string nextToken = "example";
            request.NextToken = nextToken;
            return this.client.ListOrdersByNextToken(request);
        }


    }
}
