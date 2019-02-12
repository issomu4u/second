using MarketplaceWebServiceOrders;
using MarketplaceWebServiceOrders.Model;
using SellerVendor.Areas.Seller.Models.DBContext;
using SellerVendor.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using System.Xml;
using System.Data;
using System.Configuration;
using MySql.Data.MySqlClient;
using SellerVendor.Areas.Seller.Controllers;

namespace SellerVendor.Areas.Seller.Models
{
    public class Amazon
    {
        public SellerContext dba = new SellerContext();
        public SellerAdminContext db = new SellerAdminContext();
        List<tbl_SalesOrder> customers = new List<tbl_SalesOrder>();
        List<tbl_SalesOrder> customersList = new List<tbl_SalesOrder>();
        List<AmazonListItem_Seller> customersItem = new List<AmazonListItem_Seller>();

        public string Call_API_List_Order(List<api_parameter> lstApiDetails, int strSeller_Id, int? id)
        {
            string success = "";
            string responseXml = "";
            //api_parameter objApi = new api_parameter();
            //seller_parameter_list objApi = new seller_parameter_list();
            foreach (var objApi in lstApiDetails)
            {
                int my_seller_id = objApi.id; // = seller.id.ToString();
                string my_unique_id = objApi.Seller_ID; // = seller.t_access_Key_id.ToString();
                string t_secret_Key = objApi.Secret_Key; // = seller.my_unique_id.ToString();
                string t_access_Key_id = objApi.AWS_Access_Key_ID; // = seller.t_loginName.ToString();
                string t_auth_token = objApi.auth_token; // = seller.t_password.ToString();
                int m_marketplace_id = objApi.m_marketplace_id; // = seller.t_loginName.ToString();
                string market_palce_id = objApi.marketplaceid; // = seller.t_password.ToString();
                DateTime createA = objApi.createafter;
                DateTime createB = objApi.createbefore;
                // TODO: Set the below configuration variables before attempting to run

                // Developer AWS access key
                string accessKey = t_access_Key_id; // "AKIAJ2LOHS2WG5TTUTHQ"; // "replaceWithAccessKey";

                // Developer AWS secret key
                string secretKey = t_secret_Key; //"bk3xsBJLL2CH6BXyc2jRev+0Lf3Mg8aDZM4jHrht";// "replaceWithSecretKey";

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


                //MarketplaceWebServiceOrders client = new MarketplaceWebServiceOrdersClient(accessKey, secretKey, appName, appVersion, config);
                MarketplaceWebServiceOrdersClient client = new MarketplaceWebServiceOrdersClient(accessKey, secretKey, appName, appVersion, config);// error 
               
                MarketplaceWebServiceOrdersSample sample = new MarketplaceWebServiceOrdersSample(client);

                // Uncomment the operation you'd like to test here
                // TODO: Modify the request created in the Invoke method to be valid

                try
                {
                    IMWSResponse response = null;
                    response = sample.InvokeListOrders_Amazon(createA, createB, market_palce_id, my_unique_id);
                    ResponseHeaderMetadata rhmd = response.ResponseHeaderMetadata;
                    responseXml = response.ToXML();
                    success = xml_Parse(responseXml);
                    if (success == "S")
                    {
                        success = xml_parse_item(my_unique_id, my_seller_id, t_auth_token, customers, sample);
                        if (success == "S")
                        {
                            success = saveXMLData(customers, strSeller_Id, id);
                        }
                        else
                        {
                            success = "F";
                        }
                    }
                    else
                    {
                        success = "F";
                    }
                }
                catch (MarketplaceWebServiceOrdersException ex)
                {
                    success = "F";
                }
                catch (Exception ex)
                {
                    success = "F";
                }
            }
            return success;
        }

        public string xml_Parse(string strXML)
        {
            string Success = "S";
            tbl_SalesOrder lstDataDetail = null;
            List<AmazonOrderDetails> obj = null;
            try
            {
                //Load the XML file in XmlDocument.
                XmlDocument doc = new XmlDocument();

                //doc.Load(Server.MapPath("~/Areas/XML/AmazonOrderTest.xml"));
                //doc.Load(Server.MapPath("~/Areas/XML/AmazonLTOrder.xml"));
                string str = strXML.Replace("<ListOrdersResponse xmlns=\"https://mws.amazonservices.com/Orders/2013-09-01\">", "<ListOrdersResponse xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xsi:noNamespaceSchemaLocation=\"amzn-envelope.xsd\" >");
                doc.LoadXml(str);
                string a = doc.NamespaceURI;

                //Loop through the selected Nodes.
                XmlNodeList xnList = doc.SelectNodes("/ListOrdersResponse/ListOrdersResult/Orders/Order");

                int kk = 0;
                foreach (XmlNode xn in xnList)
                {
                    kk++;
                    decimal amount = 0, exceutionamount = 0;
                    string lastDate = "", puchasedate = "", amazonorderid = "", ordertype = "", buyeremail = "", isreplacement = "", lastupdateDate = "",
                        shipservicelevel = "", noofitemshipped = "", orderstatus = "", saleschannel = "", shippedamazonTFM = "", isbusinessorder = "", itemunshipped = "",
                        latestdeliverdate = "", buyername = "", earliestdeliverydate = "", ispremiumorder = "", earliestshipdate = "", marketplaceid = "", fullfillmentchannel = "", paymentmode = "",
                        isprime = "", shipmentservicelevel = "", sellerorderid = "", paymentmethoddetail = "", currencycode = "", stateregion = "", city = "", name = "",
                        postalcode = "", address1 = "", address2 = "", countrycode = "",contactno ="",addresstype="", detailsamazonid = "", tfmshipmentstatus = "", paymentexceutionMethod = "",
                        exceutioncurrencycode = "";
                    XmlNode anode = xn;
                    if (anode != null)
                    {
                        string Amazonid = anode.SelectSingleNode("AmazonOrderId").InnerText == null ? "" : anode["AmazonOrderId"].InnerText;
                        if (anode["LatestShipDate"] != null)
                        {
                            lastDate = anode.SelectSingleNode("LatestShipDate").InnerText == null ? "" : anode["LatestShipDate"].InnerText;
                        }
                        if (anode["PurchaseDate"] != null)
                        {
                            puchasedate = anode.SelectSingleNode("PurchaseDate").InnerText == null ? "" : anode["PurchaseDate"].InnerText;
                        }
                        if (anode["AmazonOrderId"] != null)
                        {
                            amazonorderid = anode.SelectSingleNode("AmazonOrderId").InnerText == null ? "" : anode["AmazonOrderId"].InnerText;
                        }
                        if (anode["OrderType"] != null)
                        {
                            ordertype = anode.SelectSingleNode("OrderType").InnerText == null ? "" : anode["OrderType"].InnerText;
                        }
                        if (anode["BuyerEmail"] != null)
                        {
                            buyeremail = anode.SelectSingleNode("BuyerEmail").InnerText == null ? "" : anode["BuyerEmail"].InnerText;
                        }
                        if (anode["IsReplacementOrder"] != null)
                        {
                            isreplacement = anode.SelectSingleNode("IsReplacementOrder").InnerText == null ? "" : anode["IsReplacementOrder"].InnerText;
                        }
                        if (anode["LastUpdateDate"] != null)
                        {
                            lastupdateDate = anode.SelectSingleNode("LastUpdateDate").InnerText == null ? "" : anode["LastUpdateDate"].InnerText;
                        }
                        if (anode["ShipServiceLevel"] != null)
                        {
                            shipservicelevel = anode.SelectSingleNode("ShipServiceLevel").InnerText == null ? "" : anode["ShipServiceLevel"].InnerText;
                        }
                        if (anode["NumberOfItemsShipped"] != null)
                        {
                            noofitemshipped = anode.SelectSingleNode("NumberOfItemsShipped").InnerText == null ? "" : anode["NumberOfItemsShipped"].InnerText;
                        }
                        if (anode["OrderStatus"] != null)
                        {
                            orderstatus = anode.SelectSingleNode("OrderStatus").InnerText == null ? "" : anode["OrderStatus"].InnerText;
                        }
                        if (anode["SalesChannel"] != null)
                        {
                            saleschannel = anode.SelectSingleNode("SalesChannel").InnerText == null ? "" : anode["SalesChannel"].InnerText;
                        }

                        if (anode["ShippedByAmazonTFM"] != null)
                        {
                            shippedamazonTFM = anode.SelectSingleNode("ShippedByAmazonTFM").InnerText == null ? "" : anode["ShippedByAmazonTFM"].InnerText;
                        }

                        if (anode["IsBusinessOrder"] != null)
                        {
                            isbusinessorder = anode.SelectSingleNode("IsBusinessOrder").InnerText == null ? "" : anode["IsBusinessOrder"].InnerText;
                        }

                        if (anode["LatestDeliveryDate"] != null)
                        {
                            latestdeliverdate = anode.SelectSingleNode("LatestDeliveryDate").InnerText == null ? "" : anode["LatestDeliveryDate"].InnerText;
                        }

                        if (anode["NumberOfItemsUnshipped"] != null)
                        {
                            itemunshipped = anode.SelectSingleNode("NumberOfItemsUnshipped").InnerText == null ? "" : anode["NumberOfItemsUnshipped"].InnerText;
                        }
                        if (anode["BuyerName"] != null)
                        {
                            buyername = anode.SelectSingleNode("BuyerName").InnerText == null ? "" : anode["BuyerName"].InnerText;
                        }

                        if (anode["EarliestDeliveryDate"] != null)
                        {
                            earliestdeliverydate = anode.SelectSingleNode("EarliestDeliveryDate").InnerText == null ? "" : anode["EarliestDeliveryDate"].InnerText;
                        }

                        if (anode["IsPremiumOrder"] != null)
                        {
                            ispremiumorder = anode.SelectSingleNode("IsPremiumOrder").InnerText == null ? "" : anode["IsPremiumOrder"].InnerText;
                        }
                        if (anode["EarliestShipDate"] != null)
                        {
                            earliestshipdate = anode.SelectSingleNode("EarliestShipDate").InnerText == null ? "" : anode["EarliestShipDate"].InnerText;
                        }
                        if (anode["MarketplaceId"] != null)
                        {
                            marketplaceid = anode.SelectSingleNode("MarketplaceId").InnerText == null ? "" : anode["MarketplaceId"].InnerText;
                        }
                        if (anode["FulfillmentChannel"] != null)
                        {
                            fullfillmentchannel = anode.SelectSingleNode("FulfillmentChannel").InnerText == null ? "" : anode["FulfillmentChannel"].InnerText;
                        }

                        if (anode["TFMShipmentStatus"] != null)
                        {
                            tfmshipmentstatus = anode.SelectSingleNode("TFMShipmentStatus").InnerText == null ? "" : anode["TFMShipmentStatus"].InnerText;
                        }

                        if (anode["PaymentMethod"] != null)
                        {
                            paymentmode = anode.SelectSingleNode("PaymentMethod").InnerText == null ? "" : anode["PaymentMethod"].InnerText;
                        }
                        if (anode["IsPrime"] != null)
                        {
                            isprime = anode.SelectSingleNode("IsPrime").InnerText == null ? "" : anode["IsPrime"].InnerText;
                        }
                        if (anode["ShipmentServiceLevelCategory"] != null)
                        {
                            shipmentservicelevel = anode.SelectSingleNode("ShipmentServiceLevelCategory").InnerText == null ? "" : anode["ShipmentServiceLevelCategory"].InnerText;
                        }
                        if (anode["SellerOrderId"] != null)
                        {
                            sellerorderid = anode.SelectSingleNode("SellerOrderId").InnerText == null ? "" : anode["SellerOrderId"].InnerText;
                        }

                        XmlNode paymentmethod = anode.SelectSingleNode("PaymentMethodDetails");
                        if (paymentmethod != null)
                        {
                            if (paymentmethod["PaymentMethodDetail"] != null)
                            {
                                paymentmethoddetail = paymentmethod.SelectSingleNode("PaymentMethodDetail").InnerText == null ? "" : paymentmethod["PaymentMethodDetail"].InnerText;
                            }

                        }//end of if(paymentmethod)
                        else
                        {
                            //return View(true);

                        }//end of else paymentmethod

                        XmlNode ordertotal = anode.SelectSingleNode("OrderTotal");
                        if (ordertotal != null)
                        {
                            if (ordertotal["CurrencyCode"] != null)
                            {
                                currencycode = ordertotal.SelectSingleNode("CurrencyCode").InnerText == null ? "" : ordertotal["CurrencyCode"].InnerText;
                            }
                            if (ordertotal["Amount"] != null)
                            {
                                amount = Convert.ToDecimal(ordertotal.SelectSingleNode("Amount").InnerText == null ? "" : ordertotal["Amount"].InnerText);
                            }
                        }//end of if (totalorder)
                        else { }// end of ordertotal

                        XmlNode address = anode.SelectSingleNode("ShippingAddress");
                        if (address != null)
                        {
                            if (address["StateOrRegion"] != null)
                            {
                                stateregion = address.SelectSingleNode("StateOrRegion").InnerText == null ? "" : address["StateOrRegion"].InnerText;
                            }
                            if (address["City"] != null)
                            {
                                city = address.SelectSingleNode("City").InnerText == null ? "" : address["City"].InnerText;
                            }
                            if (address["CountryCode"] != null)
                            {
                                countrycode = address.SelectSingleNode("CountryCode").InnerText == null ? "" : address["CountryCode"].InnerText;
                            }
                            if (address["PostalCode"] != null)
                            {
                                postalcode = address.SelectSingleNode("PostalCode").InnerText == null ? "" : address["PostalCode"].InnerText;
                            }
                            if (address["Name"] != null)
                            {
                                name = address.SelectSingleNode("Name").InnerText == null ? "" : address["Name"].InnerText;
                            }
                            if (address["AddressLine1"] != null)
                            {
                                address1 = address.SelectSingleNode("AddressLine1").InnerText == null ? "" : address["AddressLine1"].InnerText;
                            }
                            if (address["AddressLine2"] != null)
                            {
                                address2 = address.SelectSingleNode("AddressLine2").InnerText == null ? "" : address["AddressLine2"].InnerText;
                            }
                            if (address["Phone"] != null)
                            {
                                contactno = address.SelectSingleNode("Phone").InnerText == null ? "" : address["Phone"].InnerText;
                            }
                            if (address["AddressType"] != null)
                            {
                                addresstype = address.SelectSingleNode("AddressType").InnerText == null ? "" : address["AddressType"].InnerText;
                            }
                        }//end of if(address)
                        else { }//end of else(address)

                        //---------------new node found ----------------------//
                        XmlNode paymentexceution = anode.SelectSingleNode("PaymentExecutionDetail/PaymentExecutionDetailItem");
                        if (paymentexceution != null)
                        {
                            if (paymentexceution["PaymentMethod"] != null)
                            {
                                paymentexceutionMethod = paymentexceution.SelectSingleNode("PaymentMethod").InnerText == null ? "" : paymentexceution["PaymentMethod"].InnerText;
                            }
                            XmlNode exceutionpayment = paymentexceution.SelectSingleNode("Payment");
                            if (exceutionpayment != null)
                            {
                                if (exceutionpayment["CurrencyCode"] != null)
                                {
                                    exceutioncurrencycode = exceutionpayment.SelectSingleNode("CurrencyCode").InnerText == null ? "" : exceutionpayment["CurrencyCode"].InnerText;
                                }
                                if (exceutionpayment["Amount"] != null)
                                {
                                    exceutionamount = Convert.ToDecimal(exceutionpayment.SelectSingleNode("Amount").InnerText == null ? "" : exceutionpayment["Amount"].InnerText);
                                }
                            }//end of if (totalorder)
                            else { }// end of ordertotal
                        }
                        else { }

                        //---------------------End----------------------------//

                        /////////////////////////////////////////////End //////////////////////////////////////////////////////////

                        customers.Add(new tbl_SalesOrder
                        {
                            LatestShipDate = lastDate,
                            OrderType = ordertype,
                            PurchaseDate = puchasedate,
                            AmazonOrderID = amazonorderid,
                            BuyerEmail = buyeremail,
                            IsReplacementOrder = isreplacement,
                            LastUpdatedDate = lastupdateDate,
                            ShipServiceLevel = shipservicelevel,
                            NoOfItemshipped = Convert.ToInt16(noofitemshipped),
                            OrderStatus = orderstatus,
                            SalesChannel = saleschannel,
                            IsBusinessOrder = isbusinessorder,
                            NoOfItemUnshippes = Convert.ToInt16(itemunshipped),
                            BuyerName = buyername,
                            IsPremiumOrder = ispremiumorder,
                            EarliestShipDate = earliestshipdate,
                            Marketplaceid = marketplaceid,
                            FulfillmentChannel = fullfillmentchannel,
                            PaymentMethod = paymentmode,
                            IsPrime = isprime,
                            ShipServiceLevelCategory = shipmentservicelevel,
                            SellerOrderId = sellerorderid,
                            PaymentMethodDetail = paymentmethoddetail,
                            CurrencyCode = currencycode,
                            BillAmount = amount,
                            StateRegion = stateregion,
                            City = city,
                            CountryCode = countrycode,
                            PostalCode = postalcode,
                            ShippingName = name,
                            Address_1 = address1,
                            Address_2 = address2,
                            ContactNo = contactno,
                            Addresstype = addresstype,
                            ShippedAmazonTFM = shippedamazonTFM,
                            LatestDeliveryDate = latestdeliverdate,
                            EarliestDeliveryDate = earliestdeliverydate,
                            TFMShipmentStatus = tfmshipmentstatus,
                            ExceutionPaymentMethod = paymentexceutionMethod,
                            ExceutionCurrencyCode = exceutioncurrencycode,
                            ExceutionBillAmount = exceutionamount,
                            OrderDetails = obj,
                        });

                    }// end of if(anode!=null)
                    else //if (anode != null)
                    {
                        int jjj = 0;
                        jjj++;
                    }

                    // saveXMLData(customers);
                }// end of foreach loop xnList
            }// end of try
            catch (Exception ex)
            {
                Success = "F";
            }

            return Success;
        }

        public string xml_parse_item(string SellerId, int my_seller_id, string strAuthToken, List<tbl_SalesOrder> customers, MarketplaceWebServiceOrdersSample sample)
        {
            string Success = "S";
            //tbl_SalesOrder lstDataDetail = null;
            List<AmazonOrderDetails> obj1 = null;
            string responseXml = "";
            try
            {
                string amazonorderid = "", detailsamazonid = "";
                /////////////// Now Go For Order List Item ////////////////////
                foreach (var ordlst in customers)
                {
                    if (ordlst.AmazonOrderID != null)
                    {
                        XmlDocument doc2 = new XmlDocument();
                        IMWSResponse response = null;
                        amazonorderid = ordlst.AmazonOrderID;
                        string Amazonid = ordlst.AmazonOrderID;
                        response = sample.InvokeListOrderItems_Amazon(SellerId, strAuthToken, amazonorderid);
                        ResponseHeaderMetadata rhmdItem = response.ResponseHeaderMetadata;
                        responseXml = response.ToXML();
                        string str = responseXml.Replace("<ListOrderItemsResponse xmlns=\"https://mws.amazonservices.com/Orders/2013-09-01\">", "<ListOrderItemsResponse xmlns:xsi=\"http://www.w3.org/2001/XMLSchema-instance\" xsi:noNamespaceSchemaLocation=\"amzn-envelope.xsd\" >");
                        doc2.LoadXml(str);
                        string ab = doc2.NamespaceURI;

                        XmlNodeList xnList1 = doc2.SelectNodes("/ListOrderItemsResponse/ListOrderItemsResult");
                        if (xnList1 != null)
                        {
                            foreach (XmlNode itemdetails in xnList1)
                            {
                                if (itemdetails["AmazonOrderId"] != null)
                                {
                                    detailsamazonid = itemdetails.SelectSingleNode("AmazonOrderId") == null ? "" : itemdetails["AmazonOrderId"].InnerText;
                                }
                                //obj = new List<AmazonOrderDetails>();
                                if (Amazonid == detailsamazonid)
                                {
                                    XmlNodeList detail = itemdetails.SelectNodes("OrderItems/OrderItem");
                                    if (detail != null)
                                    {

                                        foreach (XmlNode orderdetails in detail)
                                        {

                                            //List<AmazonOrderDetails> obj = null; 
                                            int orderquantity = 0, quantityshippped = 0, numberofitems = 0;
                                            double shiptaxamount = 0, promotionamount = 0, shippingpriceamount = 0, itempriceamount = 0, itemtaxamount = 0, shippingdisamount = 0, giftwraptaxamount = 0, giftwrappriceamount = 0,
                                                producttaxprice = 0, productprice = 0, shippingtaxprice = 0, shipping = 0, gifttax = 0, giftprice = 0;
                                            string title = "", asin = "", sellersku = "", orderitemid = "", shiptaxcurrenceycode = "", promotioncurrecode = "",
                                                promotionid = "", shippingpricecode = "", itempricecode = "", itemtaxcode = "", shippingdiscode = "", isgift = "",
                                                giftwraptaxcode = "", giftwrappricecode = "", conditionsubtypeId = "", conditionId = "";
                                            if (orderdetails != null)
                                            {
                                                if (orderdetails["QuantityOrdered"] != null)
                                                {
                                                    orderquantity = Convert.ToInt16(orderdetails.SelectSingleNode("QuantityOrdered").InnerText == null ? "" : orderdetails["QuantityOrdered"].InnerText);
                                                }
                                                if (orderdetails["Title"] != null)
                                                {
                                                    title = orderdetails.SelectSingleNode("Title").InnerText == null ? "" : orderdetails["Title"].InnerText;
                                                }
                                                if (orderdetails["IsGift"] != null)
                                                {
                                                    isgift = orderdetails.SelectSingleNode("IsGift").InnerText == null ? "" : orderdetails["IsGift"].InnerText;
                                                }
                                                if (orderdetails["ASIN"] != null)
                                                {
                                                    asin = orderdetails.SelectSingleNode("ASIN").InnerText == null ? "" : orderdetails["ASIN"].InnerText;
                                                }
                                                if (orderdetails["SellerSKU"] != null)
                                                {
                                                    sellersku = orderdetails.SelectSingleNode("SellerSKU").InnerText == null ? "" : orderdetails["SellerSKU"].InnerText;
                                                }
                                                if (orderdetails["OrderItemId"] != null)
                                                {
                                                    orderitemid = orderdetails.SelectSingleNode("OrderItemId").InnerText == null ? "" : orderdetails["OrderItemId"].InnerText;
                                                }
                                                if (orderdetails["QuantityShipped"] != null)
                                                {
                                                    quantityshippped = Convert.ToInt16(orderdetails.SelectSingleNode("QuantityShipped").InnerText == null ? "" : orderdetails["QuantityShipped"].InnerText);
                                                }

                                                if (orderdetails["ConditionSubtypeId"] != null)
                                                {
                                                    conditionsubtypeId = orderdetails.SelectSingleNode("ConditionSubtypeId").InnerText == null ? "" : orderdetails["ConditionSubtypeId"].InnerText;
                                                }

                                                if (orderdetails["ConditionId"] != null)
                                                {
                                                    conditionId = orderdetails.SelectSingleNode("ConditionId").InnerText == null ? "" : orderdetails["ConditionId"].InnerText;
                                                }
                                                ///////////////// CHECK FOR SKU CODE IF NOT EXIST THEN INSERT INTO TABLE /////////////////
                                                //var chkSku = dba.tbl_inventory.Where(a => a.sku == sellersku && a.tbl_sellers_id == my_seller_id).FirstOrDefault();
                                                //if (chkSku == null)
                                                //{
                                                //    tbl_inventory objInventory = new tbl_inventory();
                                                //    objInventory.sku = sellersku;
                                                //    objInventory.tbl_sellers_id = my_seller_id;
                                                //    //objInventory.tbl_item_category_id = 19;
                                                //    //objInventory.tbl_item_subcategory_id = 14;
                                                //    objInventory.item_name = title;//"American Tourist Bag";
                                                //    dba.tbl_inventory.Add(objInventory);
                                                //    dba.SaveChanges();
                                                //}

                                                XmlNode shippingtax = orderdetails.SelectSingleNode("ShippingTax");
                                                if (shippingtax != null)
                                                {
                                                    if (shippingtax["CurrencyCode"] != null)
                                                    {
                                                        shiptaxcurrenceycode = shippingtax.SelectSingleNode("CurrencyCode").InnerText == null ? "" : shippingtax["CurrencyCode"].InnerText;
                                                    }
                                                    if (shippingtax["Amount"] != null)
                                                    {
                                                        shiptaxamount = Convert.ToDouble(shippingtax.SelectSingleNode("Amount").InnerText == null ? "" : shippingtax["Amount"].InnerText);
                                                    }
                                                }// end of if(shippingtax)

                                                XmlNode promotiondiscount = orderdetails.SelectSingleNode("PromotionDiscount");
                                                if (promotiondiscount != null)
                                                {
                                                    if (promotiondiscount["CurrencyCode"] != null)
                                                    {
                                                        promotioncurrecode = promotiondiscount.SelectSingleNode("CurrencyCode").InnerText == null ? "" : promotiondiscount["CurrencyCode"].InnerText;
                                                    }
                                                    if (promotiondiscount["Amount"] != null)
                                                    {
                                                        promotionamount = Convert.ToDouble(promotiondiscount.SelectSingleNode("Amount").InnerText == null ? "" : promotiondiscount["Amount"].InnerText);
                                                    }
                                                }// end of if(promotiondiscount)


                                                XmlNode promotiondID = orderdetails.SelectSingleNode("PromotionIds");
                                                if (promotiondID != null)
                                                {
                                                    if (promotiondID["PromotionId"] != null)
                                                    {
                                                        promotionid = promotiondID.SelectSingleNode("PromotionId").InnerText == null ? "" : promotiondID["PromotionId"].InnerText;
                                                    }
                                                }// end of if(promotiondID)

                                                XmlNode shippingprice = orderdetails.SelectSingleNode("ShippingPrice");
                                                if (shippingprice != null)
                                                {
                                                    if (shippingprice["CurrencyCode"] != null)
                                                    {
                                                        shippingpricecode = shippingprice.SelectSingleNode("CurrencyCode").InnerText == null ? "" : shippingprice["CurrencyCode"].InnerText;
                                                    }
                                                    if (shippingprice["Amount"] != null)
                                                    {
                                                        shippingpriceamount = Convert.ToDouble(shippingprice.SelectSingleNode("Amount").InnerText == null ? "" : shippingprice["Amount"].InnerText);
                                                    }
                                                }// end of if(shippingprice)

                                                XmlNode itemprice = orderdetails.SelectSingleNode("ItemPrice");
                                                if (itemprice != null)
                                                {
                                                    if (itemprice["CurrencyCode"] != null)
                                                    {
                                                        itempricecode = itemprice.SelectSingleNode("CurrencyCode").InnerText == null ? "" : itemprice["CurrencyCode"].InnerText;
                                                    }
                                                    if (itemprice["Amount"] != null)
                                                    {
                                                        itempriceamount = Convert.ToDouble(itemprice.SelectSingleNode("Amount").InnerText == null ? "" : itemprice["Amount"].InnerText);
                                                    }
                                                }// end of if(itemprice)

                                                XmlNode itemtax = orderdetails.SelectSingleNode("ItemTax");
                                                if (itemtax != null)
                                                {
                                                    if (itemtax["CurrencyCode"] != null)
                                                    {
                                                        itemtaxcode = itemtax.SelectSingleNode("CurrencyCode").InnerText == null ? "" : itemtax["CurrencyCode"].InnerText;
                                                    }
                                                    if (itemtax["Amount"] != null)
                                                    {
                                                        itemtaxamount = Convert.ToDouble(itemtax.SelectSingleNode("Amount").InnerText == null ? "" : itemtax["Amount"].InnerText);
                                                    }
                                                }// end of if(itemtax)

                                                XmlNode shippingdiscount = orderdetails.SelectSingleNode("ShippingDiscount");
                                                if (shippingdiscount != null)
                                                {
                                                    if (shippingdiscount["CurrencyCode"] != null)
                                                    {
                                                        shippingdiscode = shippingdiscount.SelectSingleNode("CurrencyCode").InnerText == null ? "" : shippingdiscount["CurrencyCode"].InnerText;
                                                    }
                                                    if (shippingdiscount["Amount"] != null)
                                                    {
                                                        shippingdisamount = Convert.ToDouble(shippingdiscount.SelectSingleNode("Amount").InnerText == null ? "" : shippingdiscount["Amount"].InnerText);
                                                    }
                                                }// end of if(shippingdiscount)

                                                XmlNode giftwraptax = orderdetails.SelectSingleNode("GiftWrapTax");
                                                if (giftwraptax != null)
                                                {
                                                    if (giftwraptax["CurrencyCode"] != null)
                                                    {
                                                        giftwraptaxcode = giftwraptax.SelectSingleNode("CurrencyCode").InnerText == null ? "" : giftwraptax["CurrencyCode"].InnerText;
                                                    }
                                                    if (giftwraptax["Amount"] != null)
                                                    {
                                                        giftwraptaxamount = Convert.ToDouble(giftwraptax.SelectSingleNode("Amount").InnerText == null ? "" : giftwraptax["Amount"].InnerText);
                                                    }
                                                }// end of if(giftwraptax)
                                                XmlNode giftwrapprice = orderdetails.SelectSingleNode("GiftWrapPrice");
                                                if (giftwrapprice != null)
                                                {
                                                    if (giftwrapprice["CurrencyCode"] != null)
                                                    {
                                                        giftwrappricecode = giftwrapprice.SelectSingleNode("CurrencyCode").InnerText == null ? "" : giftwrapprice["CurrencyCode"].InnerText;
                                                    }
                                                    if (giftwrapprice["Amount"] != null)
                                                    {
                                                        giftwrappriceamount = Convert.ToDouble(giftwrapprice.SelectSingleNode("Amount").InnerText == null ? "" : giftwrapprice["Amount"].InnerText);
                                                    }
                                                }// end of if(giftwraptax)

                                                XmlNode productinfo = orderdetails.SelectSingleNode("ProductInfo");
                                                if (productinfo != null)
                                                {
                                                    if (productinfo["NumberOfItems"] != null)
                                                    {
                                                        numberofitems = Convert.ToInt16(orderdetails.SelectSingleNode("NumberOfItems").InnerText == null ? "" : orderdetails["NumberOfItems"].InnerText);
                                                    }                                                 
                                                }// end of if(giftwraptax)

                                                //// ------------------------get categorytax use sku No from inventory table--------------------////


                                                //var get_inventory = dba.tbl_inventory.Where(w => w.sku.ToLower() == sellersku.ToLower() && w.tbl_sellers_id == my_seller_id).FirstOrDefault();// get sku no from inventory table 
                                                //if (get_inventory != null)
                                                //{
                                                //    var get_cateory = dba.tbl_item_category.Where(q => q.tbl_sellers_id == my_seller_id && q.id == get_inventory.tbl_item_category_id && q.isactive == 1).FirstOrDefault();// get categroy tax from category table
                                                //    if (get_cateory != null)
                                                //    {
                                                //        var category_slabs = dba.tbl_category_slabs.Where(a => a.m_category_id == get_cateory.id).OrderByDescending(a => a.id).FirstOrDefault();//get last tax from category slabs table  
                                                //        if (category_slabs != null)
                                                //        {
                                                //            var categort_tax = category_slabs.tax_rate;
                                                //            productprice = (itempriceamount * 100) / (100 + Convert.ToDouble(categort_tax));
                                                //            producttaxprice = itempriceamount - productprice;// * Convert.ToDouble(sub_categorytax) / 100;
                                                //            shipping = (shippingpriceamount * 100) / (100 + Convert.ToDouble(categort_tax));
                                                //            shippingtaxprice = shippingpriceamount - shipping;
                                                //            giftprice = (giftwrappriceamount * 100) / (100 + Convert.ToDouble(categort_tax));
                                                //            gifttax = giftwrappriceamount - giftprice;
                                                //            //producttaxprice = itempriceamount * Convert.ToDouble(categort_tax) / 100;
                                                //            //productprice = itempriceamount - producttaxprice;
                                                //            //shippingtaxprice = shippingpriceamount * Convert.ToDouble(categort_tax) / 100;
                                                //            //shipping = shippingpriceamount - shippingtaxprice;
                                                //            //gifttax = giftwrappriceamount * Convert.ToDouble(categort_tax) / 100;
                                                //            //giftprice = giftwrappriceamount - gifttax;
                                                //        }// end of category_slabs
                                                //    }// end of if(get_cateory)
                                                //}// end of if(get_inventory)

                                                ////-------------------------------------End------------------------------------------------------///


                                                //ordlst.OrderDetails.Add(new AmazonOrderDetails
                                                customersItem.Add(new AmazonListItem_Seller
                                                {
                                                    orderid = amazonorderid,
                                                    quantity_ordered = orderquantity,
                                                    product_name = title,
                                                    asin = asin,
                                                    sku_no = sellersku,
                                                    order_item_id = orderitemid,
                                                    quantity_shipped = quantityshippped,
                                                    shipping_tax_curre_code = shiptaxcurrenceycode,
                                                    shipping_tax_Amount = shiptaxamount,
                                                    promotion_currency_code = promotioncurrecode,
                                                    promotion_amount = promotionamount,
                                                    promotion_ids = promotionid,
                                                    shipping_discount_amt = shippingdisamount,
                                                    shipping_discount_code = shippingdiscode,
                                                    shipping_price_Amount = shippingpriceamount,
                                                    shipping_price_curr_code = shippingpricecode,
                                                    item_tax_amount = itemtaxamount,
                                                    item_tax_curre_code = itemtaxcode,
                                                    item_price_curr_code = itempricecode,
                                                    item_price_amount = itempriceamount,
                                                    is_gift = isgift,
                                                    giftwrappriceamount = giftwrappriceamount,
                                                    giftwrappricecode = giftwrappricecode,
                                                    giftwraptaxamount = giftwraptaxamount,
                                                    giftwraptaxcode = giftwraptaxcode,
                                                    producttaxprice = producttaxprice,
                                                    productprice = productprice,
                                                    shippingtaxprice = shippingtaxprice,
                                                    shipping = shipping,
                                                    gifttax = gifttax,
                                                    giftprice = giftprice,

                                                    ConditionSubtypeId = conditionsubtypeId,
                                                    ConditionId = conditionId,
                                                    NumberOfItems = numberofitems,
                                                });
                                            }// end of if(order details)

                                        }//end of froeach loop orderdetails

                                    }//end of if(detail!=null)

                                }//end of if(where compare)
                            }
                        }//end of if(xnList1)
                        // show me sir
                        else { }// end of xnlist1
                        ///////////////////////////////////////////// End //////////////////////////////////////////////////////////
                    }
                }
            }// end of try
            catch (MarketplaceWebServiceOrdersException ex)
            {
                Success = "F";
            }
            catch (Exception ex)
            {
                Success = "F";
            }

            return Success;
        }

        public string saveXMLData(List<tbl_SalesOrder> customers, int SellerId, int? id)
        {
            string Success = "S";
            tbl_sales_order objsale = new tbl_sales_order();
            tbl_customer_details objcustumer = new tbl_customer_details();
           // tbl_order_history objhistory = new tbl_order_history();
            //int SellerId = Convert.ToInt32(Session["SellerID"]);
            try
            {
                string datasaved = "";
                string lowername = "";
                string stateregion = "", shippingname = "", city = "", country = "", postalcode = "", address1 = "", address2 = "";


                if (customers != null)
                {
                    //var getdd = customers.Where(a => a.AmazonOrderID == "171-3495522-2936336").ToList();

                    //customers = getdd;

                    var get_upload_order_details = dba.tbl_order_upload.Where(aa => aa.tbl_seller_id == SellerId && aa.id == id).FirstOrDefault();
                    if (get_upload_order_details != null)
                    {
                        get_upload_order_details.status = "Processing";
                        get_upload_order_details.processing_datetime = DateTime.Now;
                        dba.Entry(get_upload_order_details).State = EntityState.Modified;
                        dba.SaveChanges();
                    }
                    foreach (var item in customers)
                    {
                       
                        string Message;
                        bool flag = false;
                        string abc1 = item.SalesChannel.ToLower();
                        lowername = abc1.Split('.').First(); ;// convert name to lower case for ckh in db and split also
                        var getmarketplacedetails = db.m_marketplace.Where(a => a.isactive == 1).ToList();// get details from marketplace table in seller admin
                        var getdetails = dba.tbl_sales_order.Where(a => a.amazon_order_id == item.AmazonOrderID && a.tbl_sellers_id == SellerId).FirstOrDefault();
                        var getfullfilled = dba.m_fullfilled.ToList();

                        if (item.StateRegion != null)
                        {
                            stateregion = item.StateRegion.ToLower();
                        }
                        if (item.ShippingName != null)
                        {
                            shippingname = item.ShippingName.ToLower();
                        }
                        if (item.City != null)
                        {
                            city = item.City.ToLower();
                        }
                        if (item.CountryCode != null)
                        {
                            country = item.CountryCode.ToLower();
                        }
                        if (item.PostalCode != null)
                        {
                            postalcode = item.PostalCode.ToLower();
                        }
                        if (item.Address_1 != null)
                        {
                            address1 = item.Address_1.ToLower();
                        }
                        if (item.Address_2 != null)
                        {
                            address2 = item.Address_2.ToLower();
                        }

                        if (getdetails != null) //////////////////// Order Already Exists ///////////////
                        {
                            //var getdetails1 = dba.tbl_sales_order_status.Where(a => a.is_active == 0 && a.sales_order_status.ToLower() == item.OrderStatus.ToLower()).FirstOrDefault();
                            //if (getdetails1 != null)
                            //{
                            //    getdetails.n_item_orderstatus = getdetails1.id;
                            //}
                            //getdetails.order_status = item.OrderStatus;
                            //dba.Entry(getdetails).State = EntityState.Modified;
                            //dba.SaveChanges();
                        
                            //objhistory.created_on = DateTime.Now;
                            //objhistory.tbl_orders_id = getdetails.id;
                            //objhistory.tbl_seller_id = SellerId;
                            //objhistory.t_order_status = getdetails.n_item_orderstatus;

                            //var getsaledetails = dba.tbl_sales_order_details.Where(a => a.tbl_sales_order_id == getdetails.id).FirstOrDefault();
                            //if (getsaledetails != null)
                            //{
                            //    objhistory.tbl_orderDetails_Id = getsaledetails.id;
                            //    objhistory.ASIN = getsaledetails.asin;
                            //    objhistory.SKU = getsaledetails.sku_no;
                            //    objhistory.Quantity = getsaledetails.quantity_ordered;
                            //}
                            //objhistory.OrigialOrderID = getdetails.amazon_order_id;
                            //objhistory.ShipmentDate = getdetails.earliest_ship_date;
                            //objhistory.tbl_marketplace_id = getdetails.tbl_Marketplace_Id;
                            //dba.tbl_order_history.Add(objhistory);
                            //dba.SaveChanges();
                        }
                        else //if (1==1)
                        {
                            var customer_details = dba.tbl_customer_details.Where(a => a.tbl_seller_id == SellerId && a.State_Region.ToLower() == stateregion && a.shipping_Buyer_Name.ToLower() == shippingname && a.City.ToLower() == city && a.Country_Code.ToLower() == country && a.Postal_Code.ToLower() == postalcode && a.Address_1.ToLower() == address1 && a.Address_2.ToLower() == address2).FirstOrDefault();
                            //if (customer_details != null)
                            //{
                                //customer_details.customer_count += 1;
                                //dba.Entry(customer_details).State = EntityState.Modified;
                                //dba.SaveChanges();
                           // }
                            //else
                           // {
                                objcustumer.State_Region = item.StateRegion;
                                objcustumer.shipping_Buyer_Name = item.ShippingName;
                                objcustumer.City = item.City;
                                objcustumer.Country_Code = item.CountryCode;
                                objcustumer.Postal_Code = item.PostalCode;
                                objcustumer.Address_1 = item.Address_1;
                                objcustumer.Address_2 = item.Address_2;
                                objcustumer.contact_no = item.ContactNo;
                                objcustumer.tbl_seller_id = SellerId;
                                dba.tbl_customer_details.Add(objcustumer);
                                dba.SaveChanges();
                           // }
                            string billamt = Convert.ToString(item.BillAmount);
                            objsale.Latest_ShipDate = Convert.ToDateTime(item.LatestShipDate);
                            objsale.order_type = item.OrderType;
                            objsale.purchase_date = Convert.ToDateTime(item.PurchaseDate);
                            objsale.amazon_order_id = item.AmazonOrderID;
                            objsale.buyer_emil = item.BuyerEmail;
                            objsale.is_Replacement_order = item.IsReplacementOrder;
                            objsale.last_updated_date = Convert.ToDateTime(item.LastUpdatedDate);
                            objsale.m_ship_service_level_id = item.ShipServiceLevel;
                            objsale.no_of_itemshipped = Convert.ToInt16(item.NoOfItemshipped);
                            objsale.order_status = item.OrderStatus;
                            objsale.sales_channel = "Sale Customer Amazon";//item.SalesChannel;
                            objsale.is_business_order = item.IsBusinessOrder;
                            objsale.no_of_item_unshippes = Convert.ToInt16(item.NoOfItemUnshippes);
                            objsale.payment_method_detail = item.PaymentMethodDetail;
                            objsale.buyer_name = item.BuyerName;
                            objsale.currency_code = item.CurrencyCode;
                            if (billamt == "")
                            {
                                objsale.bill_amount = 0;
                            }
                            else
                            {
                                objsale.bill_amount = Convert.ToDouble(item.BillAmount);
                            }

                            objsale.is_premium_order = item.IsPremiumOrder;
                            objsale.earliest_ship_date = Convert.ToDateTime(item.EarliestShipDate);
                            objsale.marketplace_id = item.Marketplaceid;
                            objsale.n_item_orderstatus = 1;

                            if (item.FulfillmentChannel == "AFN")
                            {
                                objsale.n_fullfilled_id = 1;
                            }
                            else
                            {
                                objsale.n_fullfilled_id = 2;
                            }
                            objsale.fullfillment_channel = item.FulfillmentChannel;
                            objsale.payment_method = item.PaymentMethod;
                            objsale.is_prime = item.IsPrime;
                            objsale.ship_service_category = item.ShipServiceLevelCategory;
                            objsale.seller_order_id = item.SellerOrderId;
                            objsale.created_on = DateTime.Now;
                            objsale.is_active = 1;
                            objsale.tbl_sellers_id = SellerId;
                            objsale.tbl_Customer_Id = objcustumer.id;
                            objsale.tbl_order_upload_id = id;
                            objsale.LastUpdatedDateUTC = DateTime.UtcNow;

                            foreach (var abc in getmarketplacedetails)
                            {
                                string name = abc.name.ToLower();
                                if (lowername == name)
                                {
                                    objsale.tbl_Marketplace_Id = abc.id;
                                    break;
                                }
                            }//end of for each loop ( for get marketplace details)
                            var getdetails1 = dba.tbl_sales_order_status.Where(a => a.is_active == 0 && a.sales_order_status.ToLower() == item.OrderStatus.ToLower()).FirstOrDefault();
                            if (getdetails1 != null)
                            {
                                objsale.n_item_orderstatus = getdetails1.id;
                            }

                           
                            dba.tbl_sales_order.Add(objsale);
                            dba.SaveChanges();

                            foreach (var itemdetails in customersItem)
                            {
                                double tax_amount = 0;
                                if (itemdetails != null)
                                {
                                    string strOrdId = itemdetails.orderid;
                                    if (item.AmazonOrderID == strOrdId)
                                    {
                                        tbl_sales_order_details objsaledetails = new tbl_sales_order_details();
                                        string promotiondiscount = Convert.ToString(itemdetails.promotion_amount);
                                        string shippingdiscount = Convert.ToString(itemdetails.shipping_discount_amt);

                                        string shippingtaxamount = Convert.ToString(itemdetails.shipping_tax_Amount);
                                        string shippingprice = Convert.ToString(itemdetails.shipping_price_Amount);
                                        string itempriceamount = Convert.ToString(itemdetails.productprice);
                                        string itemtaxamount = Convert.ToString(itemdetails.producttaxprice);
                                        string giftwraptaxamount = Convert.ToString(itemdetails.gifttax);
                                        string giftwrappriceamount = Convert.ToString(itemdetails.giftprice);

                                        objsaledetails.quantity_ordered = itemdetails.quantity_ordered;
                                        objsaledetails.product_name = itemdetails.product_name;
                                        objsaledetails.is_gift = itemdetails.is_gift;
                                        objsaledetails.asin = itemdetails.asin;
                                        objsaledetails.sku_no = itemdetails.sku_no;
                                        objsaledetails.order_item_id = itemdetails.order_item_id;
                                        objsaledetails.quantity_shipped = itemdetails.quantity_shipped;
                                        objsaledetails.promotion_ids = itemdetails.promotion_ids;
                                        objsaledetails.shipping_tax_curre_code = itemdetails.shipping_tax_curre_code;
                                        objsaledetails.promotion_currency_code = itemdetails.promotion_currency_code;
                                        objsaledetails.shipping_price_curr_code = itemdetails.shipping_price_curr_code;
                                        objsaledetails.item_price_curr_code = itemdetails.item_price_curr_code;
                                        objsaledetails.item_tax_curre_code = itemdetails.item_tax_curre_code;
                                        objsaledetails.shipping_discount_code = itemdetails.shipping_discount_code;
                                        if (shippingtaxamount == "")
                                        {
                                            objsaledetails.shipping_tax_Amount = 0;
                                        }
                                        else
                                        {
                                            objsaledetails.shipping_tax_Amount = itemdetails.shipping_tax_Amount;
                                        }

                                        if (promotiondiscount == "")
                                        {
                                            objsaledetails.promotion_amount = 0;
                                        }
                                        else
                                        {
                                            objsaledetails.promotion_amount = itemdetails.promotion_amount;
                                        }

                                        if (shippingprice == "")
                                        {
                                            objsaledetails.shipping_price_Amount = 0;
                                        }
                                        else
                                        {
                                            objsaledetails.shipping_price_Amount = itemdetails.shipping_price_Amount;
                                        }

                                        if (itempriceamount == "")
                                        {
                                            objsaledetails.item_price_amount = 0;
                                        }
                                        else
                                        {
                                            objsaledetails.item_price_amount = itemdetails.item_price_amount; //itemdetails.productprice;
                                        }

                                        if (itemtaxamount == "")
                                        {
                                            objsaledetails.item_tax_amount = 0;
                                        }
                                        else
                                        {
                                            objsaledetails.item_tax_amount = itemdetails.item_tax_amount; //itemdetails.producttaxprice;
                                            tax_amount = itemdetails.item_tax_amount;
                                        }

                                        if (shippingdiscount == "")
                                        {
                                            objsaledetails.shipping_discount_amt = 0;
                                        }
                                        else
                                        {
                                            objsaledetails.shipping_discount_amt = itemdetails.shipping_discount_amt;
                                        }
                                        if (giftwraptaxamount == "")
                                        {
                                            objsaledetails.giftwraptax_amount = 0;
                                        }
                                        else
                                        {
                                            objsaledetails.giftwraptax_amount = itemdetails.gifttax;
                                        }
                                        if (giftwrappriceamount == "")
                                        {
                                            objsaledetails.giftwrapprice_amount = 0;
                                        }
                                        else
                                        {
                                            objsaledetails.giftwrapprice_amount = itemdetails.giftprice;
                                        }
                                        objsaledetails.giftwrapprice_code = itemdetails.giftwrappricecode;
                                        objsaledetails.giftwraptaxcode = itemdetails.giftwraptaxcode;
                                        objsaledetails.is_active = 1;
                                        objsaledetails.tbl_seller_id = SellerId;
                                        objsaledetails.tbl_sales_order_id = objsale.id;
                                        objsaledetails.status_updated_by = SellerId;
                                        objsaledetails.status_updated_on = DateTime.Now;
                                        objsaledetails.n_order_status_id = 1;
                                        objsaledetails.amazon_order_id = objsale.amazon_order_id;
                                        objsaledetails.dispatch_bydate = objsale.earliest_ship_date;
                                        objsaledetails.dispatchAfter_date = objsale.purchase_date;
                                        objsaledetails.is_tax_calculated = 0;
                                        objsaledetails.tax_updatedby_taxfile = 0;
                                        dba.tbl_sales_order_details.Add(objsaledetails);
                                        dba.SaveChanges();

                                       

                                        /////////////////// CHECK FOR SKU CODE IF NOT EXIST THEN INSERT INTO TABLE /////////////////
                                        var chkSku = dba.tbl_inventory.Where(a => a.sku == itemdetails.sku_no && a.tbl_sellers_id == SellerId && a.isactive == 1).FirstOrDefault();
                                        if (chkSku == null)
                                        {
                                            tbl_inventory objInventory = new tbl_inventory();
                                            objInventory.sku = itemdetails.sku_no;
                                            objInventory.tbl_sellers_id = SellerId;
                                            objInventory.item_name = itemdetails.product_name;
                                            objInventory.selling_price = Convert.ToInt16(itemdetails.productprice);
                                            objInventory.isactive = 1;
                                            objInventory.tbl_marketplace_id = objsale.tbl_Marketplace_Id;
                                            objInventory.asin_no = itemdetails.asin;
                                            objInventory.lastupdated = DateTime.Now;
                                            objInventory.tbl_item_category_id = 1;
                                            objInventory.hsn_code = "01111111";
                                            objInventory.item_code = "LG258-S";
                                            objInventory.tax_update = 18;
                                            objInventory.mrp = 1000;
                                            objInventory.t_effectiveBought_price = 800;
                                            objInventory.selling_price = 950;
                                            objInventory.brand = "SH";
                                            dba.tbl_inventory.Add(objInventory);
                                            dba.SaveChanges();
                                        }
                                                    double categort_tax = 0;
                                                    double igst_tax = 0, cgst_tax = 0, sgst_tax = 0, igst_amount = 0, sgst_amount = 0, cgst_amount = 0, productprice = 0, taxrate = 0;
                                                   
                                                    
                                        // ------------------------get categorytax use sku No from inventory table--------------------////

                                                    #region taxcalculated
                                                    //-------------------------------calculate tax from state wise -----------------------

                                                    double itemtax = 0;
                                                    double item_price = 0, shipping_price = 0, shipp_pricewithouttax = 0, itempricewithout_tax = 0, shipping_price_tax = 0, item_price_tax = 0;
                                                    string customerstate = "";

                                                    var get_saleorderdetails = dba.tbl_sales_order_details.Where(t => t.tbl_seller_id == SellerId && t.id == objsaledetails.id).FirstOrDefault();
                                                    if (get_saleorderdetails != null)
                                                    {
                                                        var get_sales_orders = dba.tbl_sales_order.Where(t => t.id == get_saleorderdetails.tbl_sales_order_id && t.tbl_sellers_id == SellerId).FirstOrDefault();
                                                        if (get_sales_orders != null)
                                                        {
                                                            var get_customerdetails = dba.tbl_customer_details.Where(a => a.tbl_seller_id == SellerId && a.id == get_sales_orders.tbl_Customer_Id).FirstOrDefault();
                                                            if (get_customerdetails.State_Region != null)
                                                            {
                                                                customerstate = get_customerdetails.State_Region.ToLower();
                                                            }
                                                            shipp_pricewithouttax = get_saleorderdetails.shipping_price_Amount;
                                                            itempricewithout_tax = get_saleorderdetails.item_price_amount;
                                                            var seller_details = db.tbl_sellers.Where(t => t.id == SellerId).FirstOrDefault();

                                                            double amount = Convert.ToDouble(get_saleorderdetails.item_price_amount + get_saleorderdetails.shipping_price_Amount + get_saleorderdetails.giftwrapprice_amount);
                                                            double promotionamount = 0;
                                                            if (get_saleorderdetails.promotion_amount != null)
                                                                promotionamount = get_saleorderdetails.promotion_amount;
                                                            if (get_saleorderdetails.item_promotionAmount != null)
                                                                promotionamount += Convert.ToDouble(get_saleorderdetails.item_promotionAmount);

                                                            var totalamount = amount - promotionamount;

                                                            double totaltax = 0;
                                                            if (get_saleorderdetails.item_tax_amount != null)
                                                                totaltax = get_saleorderdetails.item_tax_amount;
                                                            if (get_saleorderdetails.shipping_tax_Amount != null)
                                                                totaltax += get_saleorderdetails.shipping_tax_Amount;

                                                            if (get_saleorderdetails.giftwraptax_amount != null)
                                                                totaltax += Convert.ToDouble(get_saleorderdetails.giftwraptax_amount);

                                                            if (totaltax == null)
                                                                totaltax = 0;

                                                            var finalamt = totalamount;

                                                            if (seller_details != null)
                                                            {
                                                                var getcountrydetails = db.tbl_country.Where(t => t.status == 1 && t.countrylevel == 0 && t.id == seller_details.country).FirstOrDefault();// to get country name from country table in admin db.
                                                                var getstatedetails = db.tbl_country.Where(m => m.id == seller_details.state && m.countrylevel == 1 && m.status == 1).FirstOrDefault();//to get state name from country table in admin db.
                                                                string sellerstate = getstatedetails.countryname.ToLower();
                                                                if (sellerstate == customerstate)
                                                                {
                                                                    decimal result = 0;
                                                                    double tax_percent = 0;
                                                                    string value = Convert.ToString((totaltax * 100) / finalamt);
                                                                    if (value != "NaN" && value != "" && value != null)
                                                                    {
                                                                        decimal abcd = Convert.ToDecimal(value);
                                                                        result = decimal.Round(abcd, 2, MidpointRounding.AwayFromZero);
                                                                        tax_percent = Convert.ToDouble(result);
                                                                        item_price_tax = (finalamt * tax_percent) / 100;
                                                                    }

                                                                    item_price = get_saleorderdetails.item_price_amount - item_price_tax;
                                                                    shipping_price_tax = (get_saleorderdetails.shipping_price_Amount * tax_percent) / 100;
                                                                    shipping_price = get_saleorderdetails.shipping_price_Amount - shipping_price_tax;
                                                                    cgst_tax = Convert.ToDouble(result) / 2;
                                                                    sgst_tax = Convert.ToDouble(result) - cgst_tax;

                                                                    //string calculatedted_tax1 = Convert.ToString((finalamt * tax_percent) / 100);
                                                                    string calculatedted_tax1 = Convert.ToString(item_price_tax);
                                                                    decimal abcd_2 = Convert.ToDecimal(calculatedted_tax1);
                                                                    decimal result_2 = decimal.Round(abcd_2, 2, MidpointRounding.AwayFromZero);
                                                                    double cal_tax = Convert.ToDouble(result_2);

                                                                    cgst_amount = totaltax / 2;//(cal_tax) / 2; 
                                                                    sgst_amount = totaltax - cgst_amount; //(cal_tax - cgst_amount);  
                                                                   //double taxamt = sgst_amount + cgst_amount;

                                                                   if (totaltax != (sgst_amount + cgst_amount))
                                                                   {                                                                     
                                                                       cgst_amount += totaltax-sgst_amount;
                                                                   }
                                                                    //---------------save data in salesldeger tax table------------//
                                                                    string cgst_taxrate = "";
                                                                    string sgst_taxrate = "";
                                                                    string input_decimal_number = Convert.ToString(cgst_tax);
                                                                    var regex = new System.Text.RegularExpressions.Regex("(?<=[\\.])[0-9]+");
                                                                    if (regex.IsMatch(input_decimal_number))
                                                                    {
                                                                        string decimal_places = regex.Match(input_decimal_number).Value;
                                                                        cgst_taxrate = decimal_places;
                                                                    }
                                                                    else
                                                                    {
                                                                        cgst_taxrate = input_decimal_number;
                                                                    }
                                                                    string input_decimal_number1 = Convert.ToString(sgst_tax);
                                                                    var regex1 = new System.Text.RegularExpressions.Regex("(?<=[\\.])[0-9]+");
                                                                    if (regex1.IsMatch(input_decimal_number1))
                                                                    {
                                                                        string decimal_places1 = regex1.Match(input_decimal_number1).Value;
                                                                        sgst_taxrate = decimal_places1;
                                                                    }
                                                                    else
                                                                    {
                                                                        sgst_taxrate = input_decimal_number1;
                                                                    }

                                                                    string Taxname = "CGST@" + cgst_taxrate + "%";
                                                                    string taxname = "SGST@" + sgst_taxrate + "%";
                                                                    if (cgst_taxrate != "" && cgst_taxrate != null && cgst_taxrate != "0")
                                                                    {
                                                                        var gettaxdetails = dba.tbl_Salesledger_tax.Where(t => t.tax_name == Taxname).FirstOrDefault();
                                                                        if (gettaxdetails == null)
                                                                        {
                                                                            try
                                                                            {
                                                                                tbl_Salesledger_tax obj_salestax = new tbl_Salesledger_tax();
                                                                                obj_salestax.tax_name = Taxname;
                                                                                obj_salestax.tax_percentage = Convert.ToDouble(cgst_taxrate);
                                                                                dba.tbl_Salesledger_tax.Add(obj_salestax);
                                                                                dba.SaveChanges();
                                                                            }
                                                                            catch (Exception ex)
                                                                            {
                                                                            }
                                                                        }
                                                                    }
                                                                    if (sgst_taxrate != "" && sgst_taxrate != null && sgst_taxrate != "0")
                                                                    {
                                                                        var gettaxdetail = dba.tbl_Salesledger_tax.Where(t => t.tax_name == taxname).FirstOrDefault();
                                                                        if (gettaxdetail == null)
                                                                        {
                                                                            try
                                                                            {
                                                                                tbl_Salesledger_tax obj_salestax = new tbl_Salesledger_tax();
                                                                                obj_salestax.tax_name = taxname;
                                                                                obj_salestax.tax_percentage = Convert.ToDouble(sgst_taxrate);
                                                                                dba.tbl_Salesledger_tax.Add(obj_salestax);
                                                                                dba.SaveChanges();
                                                                            }
                                                                            catch (Exception ex)
                                                                            {
                                                                            }
                                                                        }
                                                                    }
                                                                    //-------------------------------end---------------------------/
                                                                }
                                                                else
                                                                {
                                                                    string value1 = Convert.ToString((totaltax * 100) / finalamt);
                                                                    if (value1 != "NaN" && value1 != "" && value1 != null)
                                                                    {
                                                                        decimal abcd1 = Convert.ToDecimal(value1);
                                                                        decimal result1 = decimal.Round(abcd1, 2, MidpointRounding.AwayFromZero);
                                                                        igst_tax = Convert.ToDouble(result1);
                                                                    }

                                                                    //item_price_tax =Convert.ToDouble((finalamt * igst_tax) / 100);
                                                                    decimal price_tax = Convert.ToDecimal((finalamt * igst_tax) / 100);
                                                                    decimal result3 = decimal.Round(price_tax, 2, MidpointRounding.AwayFromZero);
                                                                    item_price_tax = Convert.ToDouble(result3);
                                                                    item_price = get_saleorderdetails.item_price_amount - item_price_tax;
                                                                    shipping_price_tax = (get_saleorderdetails.shipping_price_Amount * igst_tax) / 100;
                                                                    shipping_price = get_saleorderdetails.shipping_price_Amount - shipping_price_tax;
                                                                    igst_amount = totaltax;// item_price_tax;

                                                                    string igst_taxrate = "";
                                                                    string input_decimal_number4 = Convert.ToString(igst_tax);
                                                                    var regex4 = new System.Text.RegularExpressions.Regex("(?<=[\\.])[0-9]+");
                                                                    if (regex4.IsMatch(input_decimal_number4))
                                                                    {
                                                                        string decimal_places4 = regex4.Match(input_decimal_number4).Value;
                                                                        igst_taxrate = decimal_places4;
                                                                    }
                                                                    else
                                                                    {
                                                                        igst_taxrate = input_decimal_number4;
                                                                    }
                                                                    if (igst_taxrate != "" && igst_taxrate != null && igst_taxrate != "0")
                                                                    {
                                                                        string igstTaxname = "IGST@" + igst_taxrate + "%";
                                                                        var gettaxdetails = dba.tbl_Salesledger_tax.Where(t => t.tax_name == igstTaxname).FirstOrDefault();
                                                                        if (gettaxdetails == null)
                                                                        {
                                                                            try
                                                                            {
                                                                                tbl_Salesledger_tax obj_salestax = new tbl_Salesledger_tax();
                                                                                obj_salestax.tax_name = igstTaxname;
                                                                                obj_salestax.tax_percentage = Convert.ToDouble(igst_taxrate);
                                                                                dba.tbl_Salesledger_tax.Add(obj_salestax);
                                                                                dba.SaveChanges();
                                                                            }
                                                                            catch (Exception ex)
                                                                            {
                                                                            }
                                                                        }
                                                                    }
                                                                }
                                                                var get_taxdetails = dba.tbl_tax.Where(t => t.tbl_referneced_id == objsaledetails.id && t.reference_type == 3 && t.tbl_seller_id == SellerId).FirstOrDefault();
                                                                if (get_taxdetails == null)
                                                                {
                                                                    if (igst_tax != 0 || cgst_tax != 0.0 || sgst_tax != 0.0)
                                                                    {
                                                                        tbl_tax objtax = new tbl_tax();
                                                                        objtax.tbl_seller_id = SellerId;
                                                                        objtax.tbl_referneced_id = objsaledetails.id;
                                                                        objtax.reference_type = 3;
                                                                        objtax.isactive = 1;
                                                                        if (cgst_tax != null && cgst_tax != 0.0 && sgst_tax != null && sgst_tax != 0.0)
                                                                        {
                                                                            objtax.cgst_tax = cgst_tax;
                                                                            objtax.sgst_tax = sgst_tax;
                                                                            objtax.CGST_amount = cgst_amount;
                                                                            objtax.sgst_amount = sgst_amount;
                                                                        }
                                                                        else
                                                                        {
                                                                            objtax.igst_tax = igst_tax;
                                                                            objtax.Igst_amount = igst_amount;
                                                                        }
                                                                        dba.tbl_tax.Add(objtax);
                                                                        dba.SaveChanges();
                                                                    }
                                                                }// end of if(get_taxdetails)  

                                                            }// end of if(seller_details)


                                                        }// end of get_sales_orders                                                       
                                                    }// end of if get_saleorderdetails

                                                    //-----------------------------------------End-----------------------------------------
                                                    #endregion                                                                                                         
                                        //-------------------------------------End------------------------------------------------------///

                                        

                                        //---------------------------- update tblinventory_details status when  fullfilled by AFN
                                        if (objsale.fullfillment_channel == "AFN")
                                        {
                                            var get_inventorydetails = dba.tbl_inventory.Where(a => a.sku.ToLower() == objsaledetails.sku_no.ToLower() && a.tbl_sellers_id == SellerId).FirstOrDefault();
                                            if (get_inventorydetails != null)
                                            {
                                                var get_inventoryItemdetails = dba.tbl_inventory_details.Where(a => a.tbl_inventory_id == get_inventorydetails.id && a.m_marketplace_id == objsale.tbl_Marketplace_Id && a.tbl_sellers_id == SellerId && a.m_item_status_id == 1).ToList();
                                                if (get_inventoryItemdetails != null)
                                                {
                                                    int mycount = 0;
                                                    foreach (var item1 in get_inventoryItemdetails)
                                                    {
                                                        if (mycount < get_inventoryItemdetails.Count)
                                                        {
                                                            item1.m_item_status_id = 2;
                                                            dba.Entry(item1).State = EntityState.Modified;
                                                            dba.SaveChanges();
                                                            mycount++;
                                                        }
                                                        else
                                                        {
                                                            break;
                                                        }
                                                    }
                                                }// end of if(get_inventoryItemdetails)
                                            }// end of if(get_inventorydetails)
                                        }// end of if(objsale.fullfillment_channel)


                                    }
                                }
                            }

                        }// end of if(get details)
                    }//end of foreach loop(item)



                    string connectionstring = ConfigurationManager.ConnectionStrings["SellerConnection"].ConnectionString;
                    MySqlConnection con = new MySqlConnection(connectionstring);

                    int uniqueupload = 0;
                    int diff_order = 0;
                    int total_order = 0;
                    var get_unique_count_details = "SELECT count(DISTINCT aa.id) FROM(SELECT DISTINCT Order_Id AS id FROM `tbl_settlement_order` where tbl_seller_id = " + SellerId + " UNION ALL SELECT DISTINCT amazon_order_id AS id FROM `tbl_sales_order` where tbl_sellers_id =" + SellerId + " ) AS aa";
                    MySqlCommand cmd = new MySqlCommand(get_unique_count_details, con);
                    con.Open();
                    MySqlDataReader dr = cmd.ExecuteReader();
                    if (dr.HasRows)
                    {
                        dr.Read();
                        uniqueupload = Convert.ToInt32(dr[0]);
                    }
                    cmd.Dispose();
                    con.Close();

                    MySqlConnection con1 = new MySqlConnection(connectionstring);
                    var get_unique_orderby_id = "SELECT count( aa.id )FROM (SELECT tbl_order_upload_id AS id FROM `tbl_sales_order` where tbl_sellers_id =" + SellerId + " && tbl_order_upload_id = " + id + ") AS aa";
                    MySqlCommand cmd2 = new MySqlCommand(get_unique_orderby_id, con1);
                    con1.Open();
                    MySqlDataReader dr1 = cmd2.ExecuteReader();
                    if (dr1.HasRows)
                    {
                        dr1.Read();
                        total_order = Convert.ToInt32(dr1[0]);
                    }
                    cmd2.Dispose();
                    con1.Close();

                    var get_balance_details = db.tbl_user_login.Where(aa => aa.tbl_sellers_id == SellerId).FirstOrDefault();
                    if (get_balance_details != null)
                    {
                        var last_order = get_balance_details.total_orders;
                        diff_order = Convert.ToInt16(uniqueupload - last_order);
                        var plan_rate = get_balance_details.applied_plan_rate * diff_order;
                        get_balance_details.wallet_balance = get_balance_details.wallet_balance - Convert.ToDouble(plan_rate);
                        int totalOrder = Convert.ToInt16(diff_order);
                        if (get_balance_details.total_orders != null)
                            totalOrder = totalOrder + Convert.ToInt16(get_balance_details.total_orders);
                        get_balance_details.total_orders = totalOrder;
                        db.Entry(get_balance_details).State = EntityState.Modified;
                        db.SaveChangesAsync();
                        //Session["WalletBalance"] = get_balance_details.wallet_balance.ToString();
                        //Session["TotalOrders"] = totalOrder.ToString();
                    }
                    //var get_upload_settlement_details = dba.tbl_order_upload.Where(aa => aa.tbl_seller_id == SellerId && aa.id == id).FirstOrDefault();
                    if (get_upload_order_details != null)
                    {
                        get_upload_order_details.new_order_uploaded = total_order;                       
                        get_upload_order_details.status = "Completed";
                        get_upload_order_details.completed_datetime = DateTime.Now;
                        dba.Entry(get_upload_order_details).State = EntityState.Modified;
                    }
                    dba.SaveChangesAsync();                   
                }//end of if(customer)
                //return "S";
            }
            catch (Exception ex)
            {
                Success = "F";
                var get_upload_orderdetails = dba.tbl_order_upload.Where(aa => aa.tbl_seller_id == SellerId && aa.id == id).FirstOrDefault();
                get_upload_orderdetails.status = "File Format is not Correct or Some Error Occurred";
                get_upload_orderdetails.completed_datetime = DateTime.Now;
                dba.Entry(get_upload_orderdetails).State = EntityState.Modified;
                dba.SaveChangesAsync();
                Writelog log = new Writelog();
                log.write_exception_log(SellerId.ToString(), "CronJobController", "bulkxml_Parse", DateTime.Now, ex);
                throw ex;
                
            }
            return Success;
        }
    }
}