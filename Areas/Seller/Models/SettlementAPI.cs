
using SellerVendor.Areas.Seller.Models.DBContext;
using SellerVendor.Models;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Web;
using SellerVendor.Areas.Seller.AmazonAPI.settlement.Model;
using SellerVendor.Areas.Seller.AmazonModel.settlementModel;
using SellerVendor.Areas.Seller.AmazonAPI.settlement;
using System.Xml;
using SellerVendor.Areas.Seller.Controllers;

namespace SellerVendor.Areas.Seller.Models
{
    public class SettlementAPI
    {
        public SellerContext dba = new SellerContext();
        public SellerAdminContext db = new SellerAdminContext();
        CronJobController objsales = new CronJobController();
       

        public string Call_API_Settlement1(List<api_parameter> lstApiDetails, int strSeller_Id, int id)//when we use XML data report
        {
            string success = "";
            string responseXml = "";
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

                String sellerid = my_unique_id;//"ALX5VZP4KM90V";
                String Secret_Key = t_secret_Key;// "tB1OE8WvQB4xZ8bOtD8aFIfW2H1uwfAIfYDMfVx8";
                String AWS_Access_Key_ID = t_access_Key_id;// "AKIAILZANI6ZKK4Z3PAQ";
                String marketplaceid = market_palce_id;//"A21TJRUUN4KGV";

                //String sellerid = "ALX5VZP4KM90V";
                //String Secret_Key = "tB1OE8WvQB4xZ8bOtD8aFIfW2H1uwfAIfYDMfVx8";
                //String AWS_Access_Key_ID =  "AKIAILZANI6ZKK4Z3PAQ";
                //String marketplaceid = "A21TJRUUN4KGV";

                String accessKeyId = AWS_Access_Key_ID;
                String secretAccessKey = Secret_Key;

                const string applicationName = "AmazonJavascriptScratchpad";
                const string applicationVersion = "1.0.0.0";
                MarketplaceWebServiceConfig config = new MarketplaceWebServiceConfig();
                config.ServiceURL = "https://mws.amazonservices.in";



                MarketplaceWebService service = new MarketplaceWebServiceClient(
                   accessKeyId,
                   secretAccessKey,
                   applicationName,
                   applicationVersion,
                   config);

                //const string merchantId = my_unique_id;// "ALX5VZP4KM90V";
                //const string marketplaceId = market_palce_id;// "A21TJRUUN4KGV";

                try
                {
                    GetReportListRequest request = new GetReportListRequest();
                    DateTime DtStart;
                    DateTime DtEnd;
                    DtStart = createA;//DateTime.Parse("2017-12-01 23:59:59");
                    DtEnd = createB;//DateTime.Parse("2017-12-08 23:59:59");

                    //DtStart = DateTime.Parse("2018-01-01 23:59:59");
                    //DtEnd = DateTime.Parse("2018-01-31 23:59:59");

                    request.Merchant = sellerid;// merchantId;               
                    request.AvailableFromDate = DtStart;
                    request.AvailableToDate = DtEnd;

                    List<string> list2 = new List<string>();
                   
                    list2.Add("_GET_V2_SETTLEMENT_REPORT_DATA_XML_");
                    request.ReportTypeList = new TypeList();
                    request.ReportTypeList.Type = list2; // _GET_V2_SETTLEMENT_REPORT_DATA_XML_; // TypeList.WithType(list1);
                
                   List<string> InvokeGetReportList = new List<string>();
                
                   InvokeGetReportList = GetReportListSample.InvokeGetReportList(service, request);
                   foreach (var item in InvokeGetReportList)
                    {
                       GetReportRequest request1 = new GetReportRequest();
                       request1.Merchant = sellerid;// merchantId;                                       
                       request1.ReportId = item;
                       string strFolderPath = HttpContext.Current.Server.MapPath("~/UploadExcel/");//"~/UploadExcel/" + seller_id + "/settlement/" + filename                    
                       strFolderPath = strFolderPath + item + ".xml";//use plz uncomment this line when use .xml
                   // string strFolderPath = HttpContext.Current.Server.MapPath("~/" + item + ".xml");
                       FileStream file1 = File.Open(strFolderPath, FileMode.OpenOrCreate, FileAccess.ReadWrite);
                       request1.Report = file1;                                     
                    GetReportResponse resp = service.GetReport(request1);
                    ResponseHeaderMetadata rhmd = resp.ResponseHeaderMetadata;
                    file1.Flush();
                    byte[] bytes = new byte[file1.Length];
                    int numBytesToRead = (int)file1.Length;
                    int numBytesRead = 0;
                    while (numBytesToRead > 0)
                    {
                        // Read may return anything from 0 to numBytesToRead.
                        int n = file1.Read(bytes, numBytesRead, numBytesToRead);
                        // Break when the end of the file is reached.
                        if (n == 0)
                            break;
                        numBytesRead += n;
                        numBytesToRead -= n;
                    }
                    string xmldata = System.Text.Encoding.UTF8.GetString(bytes);
                   
                    Read_SettlementData(xmldata, strSeller_Id, id,m_marketplace_id);
                    int errw = 0;
                    errw++;
                }                             
            }               
                catch (MarketplaceWebServiceException ex)
                {
                    success = "F";
                }
                catch (Exception ex)
                {
                    success = "F";
                }

            }//end of for each loop
            return success;
        }

        public string Read_SettlementData(string strXML, int SellerId, int id, int m_marketplace_id)
        {
            string Success = "S";
            List<AmazonreconciliationOrder> objjson1 = new List<AmazonreconciliationOrder>();
            List<reconciliationorder> objreconciliationorder = new List<reconciliationorder>();
            try
            {
                Dictionary<string, reconciliationorder> dictionary = new Dictionary<string, reconciliationorder>();
                XmlDocument doc = new XmlDocument();
                reconciliationorder obj_item = obj_item = new reconciliationorder();

              
                doc.LoadXml(strXML);
                string a = doc.NamespaceURI;

                #region settlementdetail
                XmlNode ordertotal = doc.SelectSingleNode("/AmazonEnvelope/Message/SettlementReport/SettlementData");
                if (ordertotal != null)
                {
                    if (ordertotal["AmazonSettlementID"] != null)
                    {
                        obj_item.settlement_id = ordertotal.SelectSingleNode("AmazonSettlementID").InnerText == null ? "" : ordertotal["AmazonSettlementID"].InnerText;
                    }
                    if (ordertotal["TotalAmount"] != null)
                    {
                        obj_item.total_amount = ordertotal.SelectSingleNode("TotalAmount").InnerText == null ? "" : ordertotal["TotalAmount"].InnerText;
                    }
                    if (ordertotal["StartDate"] != null)
                    {
                        obj_item.settlement_start_date = ordertotal.SelectSingleNode("StartDate").InnerText == null ? "" : ordertotal["StartDate"].InnerText;
                    }
                    if (ordertotal["EndDate"] != null)
                    {
                        obj_item.settlement_end_date = ordertotal.SelectSingleNode("EndDate").InnerText == null ? "" : ordertotal["EndDate"].InnerText;
                    }
                    if (ordertotal["DepositDate"] != null)
                    {
                        obj_item.deposit_date = ordertotal.SelectSingleNode("DepositDate").InnerText == null ? "" : ordertotal["DepositDate"].InnerText;
                    }
                }
                #region OtherTransaction

                XmlNodeList transaction = doc.SelectNodes("/AmazonEnvelope/Message/SettlementReport/OtherTransaction");
                if (transaction != null)
                {
                    //reconciliationorder obj_item3 = null;
                    foreach (XmlNode item_detail1 in transaction)
                    {

                        //obj_item3 = new reconciliationorder();
                        if (obj_item.otherTransatanctionList == null)
                            obj_item.otherTransatanctionList = new List<settlement_amt_type>();

                        if (item_detail1 != null)
                        {
                            string type1 = "";
                            string amount1 = "";
                            if (item_detail1.SelectSingleNode("TransactionType") != null)
                            {

                                type1 = item_detail1.SelectSingleNode("TransactionType").InnerText == null ? "" : item_detail1["TransactionType"].InnerText;
                                if (item_detail1.SelectSingleNode("Amount") != null)
                                {
                                    amount1 = item_detail1.SelectSingleNode("Amount").InnerText == null ? "" : item_detail1["Amount"].InnerText;
                                }
                                settlement_amt_type subobj8 = new settlement_amt_type();
                                subobj8.description = type1;
                                subobj8.amount = Math.Round(Convert.ToDouble(amount1), 2);
                                if (obj_item.otherTransatanctionList == null)
                                    obj_item.otherTransatanctionList = new List<settlement_amt_type>();

                                obj_item.otherTransatanctionList.Add(subobj8);

                            }
                        }// end o
                        // objreconciliationorder.Add(obj_item);
                        ///obj_item.otherTransatanctionList.Add(subobj);
                        ///
                    }

                }
                #endregion

                objreconciliationorder.Add(obj_item);
                #endregion

                reconciliationorder obj_item1 = null;
                #region orderfetch
                XmlNodeList xnList = doc.SelectNodes("/AmazonEnvelope/Message/SettlementReport/Order");
                int kk = 0;
                foreach (XmlNode xn in xnList)
                {
                    kk++;
                    //reconciliationorder obj_item1 = null;
                    string orderidd = "";
                    if (xn["AmazonOrderID"] != null)
                    {
                        orderidd = xn.SelectSingleNode("AmazonOrderID").InnerText == null ? "" : xn["AmazonOrderID"].InnerText;
                    }
                    XmlNode anode = xn;
                    if (dictionary.ContainsKey(orderidd))
                    {
                        obj_item1 = dictionary[orderidd];
                        settlement_amt_type subobj = new settlement_amt_type();


                    }// end of if dictionary
                    else
                    {
                        if (anode != null)
                        {
                            obj_item1 = new reconciliationorder();
                            obj_item1.order_amount_typesDict = new Dictionary<string, List<settlement_amt_type>>();

                            obj_item1.transaction_type = "Order";
                            if (anode["AmazonOrderID"] != null)
                            {
                                obj_item1.order_id = anode.SelectSingleNode("AmazonOrderID").InnerText == null ? "" : anode["AmazonOrderID"].InnerText;
                            }
                            if (anode["MerchantOrderID"] != null)
                            {
                                obj_item1.merchant_order_id = anode.SelectSingleNode("MerchantOrderID").InnerText == null ? "" : anode["MerchantOrderID"].InnerText;
                            }
                            if (anode["ShipmentID"] != null)
                            {
                                obj_item1.shipment_id = anode.SelectSingleNode("ShipmentID").InnerText == null ? "" : anode["ShipmentID"].InnerText;
                            }
                            if (anode["MarketplaceName"] != null)
                            {
                                obj_item1.marketplace_name = anode.SelectSingleNode("MarketplaceName").InnerText == null ? "" : anode["MarketplaceName"].InnerText;
                            }
                            XmlNode fullfillment = anode.SelectSingleNode("Fulfillment");
                            if (fullfillment != null)
                            {
                                if (fullfillment["MerchantFulfillmentID"] != null)
                                {
                                    obj_item1.fulfillment_id = fullfillment.SelectSingleNode("MerchantFulfillmentID").InnerText == null ? "" : fullfillment["MerchantFulfillmentID"].InnerText;
                                }
                                if (fullfillment["PostedDate"] != null)
                                {
                                    obj_item1.posted_date = fullfillment.SelectSingleNode("PostedDate").InnerText == null ? "" : fullfillment["PostedDate"].InnerText;
                                }
                                #region item
                                XmlNodeList xnItem = fullfillment.SelectNodes("Item");
                                string sku = "";
                                if (xnItem != null)
                                {
                                    foreach (XmlNode item_detail in xnItem)
                                    {
                                        if (item_detail["AmazonOrderItemCode"] != null)
                                        {
                                            obj_item1.order_itemId = item_detail.SelectSingleNode("AmazonOrderItemCode").InnerText == null ? "" : item_detail["AmazonOrderItemCode"].InnerText;
                                        }
                                        if (item_detail["SKU"] != null)
                                        {
                                            obj_item1.sku = item_detail.SelectSingleNode("SKU").InnerText == null ? "" : item_detail["SKU"].InnerText;
                                            sku = item_detail.SelectSingleNode("SKU").InnerText == null ? "" : item_detail["SKU"].InnerText;
                                        }
                                        if (item_detail["Quantity"] != null)
                                        {
                                            obj_item1.quantity_purchased = item_detail.SelectSingleNode("Quantity").InnerText == null ? "" : item_detail["Quantity"].InnerText;
                                        }

                                        List<settlement_amt_type> li = null;
                                        if (obj_item1.order_amount_typesDict.ContainsKey(obj_item1.sku))
                                        {
                                            li = obj_item1.order_amount_typesDict[obj_item1.sku];
                                        }
                                        else
                                        {
                                            li = new List<settlement_amt_type>();
                                        }

                                        #region itemprice
                                        XmlNodeList itempriceslist = item_detail.SelectNodes("ItemPrice/Component");
                                        if (itempriceslist != null)
                                        {
                                            string type = "";
                                            string amount = "";
                                            foreach (XmlNode pricelist in itempriceslist)
                                            {
                                                if (pricelist != null)
                                                {
                                                    if (pricelist.SelectSingleNode("Type") != null)
                                                    {
                                                        type = pricelist.SelectSingleNode("Type").InnerText == null ? "" : pricelist["Type"].InnerText;
                                                        if (pricelist.SelectSingleNode("Amount") != null)
                                                        {
                                                            amount = pricelist.SelectSingleNode("Amount").InnerText == null ? "" : pricelist["Amount"].InnerText;
                                                        }
                                                        settlement_amt_type subobj1 = new settlement_amt_type();
                                                        subobj1.amount = Math.Round(Convert.ToDouble(amount), 2);
                                                        if (type == "Tax")
                                                            type = "Product Tax";

                                                        if (type == "GiftWrap")
                                                            type = "Gift wrap";

                                                        if (type == "GiftWrapTax")
                                                            type = "Gift Wrap Tax";

                                                        if (type == "ShippingTax")
                                                            type = "Shipping tax";
                                                        subobj1.description = type;
                                                        li.Add(subobj1);
                                                    }  // end of if itempricelist     
                                                }
                                            }
                                        }//End of For each loop pricelist 
                                        #endregion

                                        #region itemfee
                                        XmlNodeList itemfee = item_detail.SelectNodes("ItemFees/Fee");
                                        if (itemfee != null)
                                        {
                                            string type1 = "";
                                            string amount1 = "";
                                            foreach (XmlNode itemfeelist in itemfee)
                                            {
                                                if (itemfeelist != null)
                                                {
                                                    if (itemfeelist.SelectSingleNode("Type") != null)
                                                    {
                                                        type1 = itemfeelist.SelectSingleNode("Type").InnerText == null ? "" : itemfeelist["Type"].InnerText;
                                                        if (itemfeelist.SelectSingleNode("Amount") != null)
                                                        {
                                                            amount1 = itemfeelist.SelectSingleNode("Amount").InnerText == null ? "" : itemfeelist["Amount"].InnerText;
                                                        }
                                                        settlement_amt_type subobj5 = new settlement_amt_type();
                                                        subobj5.amount = Math.Round(Convert.ToDouble(amount1), 2);
                                                        if (type1 == "FBAWeightBasedFee")
                                                            type1 = "FBA Weight Handling Fee";

                                                        if (type1 == "FBAWeightBasedFee CGST")
                                                            type1 = "FBA Weight Handling Fee CGST";

                                                        if (type1 == "FBAWeightBasedFee SGST")
                                                            type1 = "FBA Weight Handling Fee SGST";

                                                        if (type1 == "FixedClosingFee")
                                                            type1 = "Fixed closing fee";

                                                        if (type1 == "FixedClosingFee IGST")
                                                            type1 = "Fixed closing fee IGST";

                                                        if (type1 == "ShippingChargeback")
                                                            type1 = "Shipping Chargeback";

                                                        if (type1 == "ShippingChargeback CGST")
                                                            type1 = "Shipping Chargeback CGST";

                                                        if (type1 == "ShippingChargeback SGST")
                                                            type1 = "Shipping Chargeback SGST";

                                                        subobj5.description = type1;
                                                        li.Add(subobj5);

                                                    }
                                                }// end of if itemfeelist

                                            }// end of froeach itemfeelist

                                        }// end of if itemfee                                      
                                        #endregion

                                        #region promotion
                                        XmlNode promotion = item_detail.SelectSingleNode("Promotion");
                                        string shippingtype = "";
                                        string shippingamount = "";
                                        // string type1 = "";
                                        if (promotion != null)
                                        {
                                            if (promotion["Type"] != null)
                                            {
                                                shippingtype = promotion.SelectSingleNode("Type").InnerText == null ? "" : promotion["Type"].InnerText;
                                            }
                                            if (promotion["Amount"] != null)
                                            {
                                                shippingamount = promotion.SelectSingleNode("Amount").InnerText == null ? "" : promotion["Amount"].InnerText;
                                            }
                                            settlement_amt_type subobj6 = new settlement_amt_type();
                                            subobj6.amount = Math.Round(Convert.ToDouble(shippingamount), 2);
                                            if (shippingtype == "Shipping")
                                                shippingtype = "Shipping discount";
                                            subobj6.description = shippingtype;
                                            //subobj6.description = shippingtype + "discount";
                                            li.Add(subobj6);
                                        }
                                        #endregion
                                        obj_item1.order_amount_typesDict.Add(obj_item1.order_itemId, li);// sku;
                                        //obj_item1.order_amount_typesDict.Add(sku, li);not solve issue
                                    }// end of for each item_detail

                                }// end of if item
                                # endregion
                            }// end of if Fullfillment

                        }// end of if anode
                        objreconciliationorder.Add(obj_item1);
                        dictionary.Add(obj_item1.order_id, obj_item1);
                    }// end of else                 
                }// end of for each loop
                #endregion
                #region fetchRefund
                XmlNodeList xnrefund = doc.SelectNodes("/AmazonEnvelope/Message/SettlementReport/Refund");

                foreach (XmlNode xn in xnrefund)
                {
                    kk++;
                    //reconciliationorder obj_item1 = null;

                    string orderidd = "";
                    if (xn["AmazonOrderID"] != null)
                    {
                        orderidd = xn.SelectSingleNode("AmazonOrderID").InnerText == null ? "" : xn["AmazonOrderID"].InnerText;
                    }
                    XmlNode anoderefund = xn;
                    if (dictionary.ContainsKey(orderidd))
                    {
                        obj_item1 = dictionary[orderidd];
                        settlement_amt_type subobj = new settlement_amt_type();

                        if (obj_item1.refund_amount_typesDict == null)
                            obj_item1.refund_amount_typesDict = new Dictionary<string, List<settlement_amt_type>>();
                    }// end of if dictionary
                    else
                    {
                        obj_item1 = new reconciliationorder();
                        obj_item1.transaction_type = "Refund";
                        if (anoderefund != null)
                        {
                            //obj_item1 = new reconciliationorder();
                            obj_item1.order_id = orderidd;
                            obj_item1.transaction_type = "Refund";
                            obj_item1.refund_amount_typesDict = new Dictionary<string, List<settlement_amt_type>>();
                        }// end of if anode
                        objreconciliationorder.Add(obj_item1);
                        dictionary.Add(obj_item1.order_id, obj_item1);
                    }// end of else

                    XmlNode fullfillment = anoderefund.SelectSingleNode("Fulfillment");
                    if (fullfillment != null)
                    {
                        #region item
                        XmlNodeList xnItem = fullfillment.SelectNodes("AdjustedItem");
                        if (xnItem != null)
                        {
                            foreach (XmlNode item_detail in xnItem)
                            {
                                //
                                if (item_detail["AmazonOrderItemCode"] != null)
                                {
                                    obj_item1.order_itemId = item_detail.SelectSingleNode("AmazonOrderItemCode").InnerText == null ? "" : item_detail["AmazonOrderItemCode"].InnerText;
                                }
                                if (item_detail["SKU"] != null)
                                {
                                    obj_item1.sku = item_detail.SelectSingleNode("SKU").InnerText == null ? "" : item_detail["SKU"].InnerText;
                                }
                                List<settlement_amt_type> li = null;
                                if (obj_item1.refund_amount_typesDict.ContainsKey(obj_item1.sku))
                                {
                                    li = obj_item1.refund_amount_typesDict[obj_item1.sku];
                                }
                                else
                                {
                                    li = new List<settlement_amt_type>();
                                    obj_item1.refund_amount_typesDict.Add(obj_item1.sku, li);
                                }

                                #region itemprice
                                XmlNodeList itempriceslist = item_detail.SelectNodes("ItemPriceAdjustments/Component");
                                if (itempriceslist != null)
                                {
                                    string type = "";
                                    string amount = "";
                                    foreach (XmlNode pricelist in itempriceslist)
                                    {
                                        if (pricelist != null)
                                        {

                                            if (pricelist.SelectSingleNode("Type") != null)
                                            {
                                                type = pricelist.SelectSingleNode("Type").InnerText == null ? "" : pricelist["Type"].InnerText;
                                                if (pricelist.SelectSingleNode("Amount") != null)
                                                {
                                                    amount = pricelist.SelectSingleNode("Amount").InnerText == null ? "" : pricelist["Amount"].InnerText;
                                                }
                                                settlement_amt_type subobj1 = new settlement_amt_type();
                                                subobj1.amount = Math.Round(Convert.ToDouble(amount), 2);
                                                if (type == "Tax")
                                                    type = "Product Tax";

                                                if (type == "GiftWrap")
                                                    type = "Gift wrap";

                                                if (type == "GiftWrapTax")
                                                    type = "Gift Wrap Tax";

                                                if (type == "ShippingTax")
                                                    type = "Shipping tax";
                                                subobj1.description = type;
                                                li.Add(subobj1);
                                            }  // end of if itempricelist     
                                        }
                                    }
                                }//End of For each loop pricelist 
                                #endregion

                                #region itemfee
                                XmlNodeList itemfee = item_detail.SelectNodes("ItemFeeAdjustments/Fee");
                                if (itemfee != null)
                                {
                                    string type1 = "";
                                    string amount1 = "";
                                    foreach (XmlNode itemfeelist in itemfee)
                                    {
                                        if (itemfeelist != null)
                                        {
                                            if (itemfeelist.SelectSingleNode("Type") != null)
                                            {
                                                type1 = itemfeelist.SelectSingleNode("Type").InnerText == null ? "" : itemfeelist["Type"].InnerText;
                                                if (itemfeelist.SelectSingleNode("Amount") != null)
                                                {
                                                    amount1 = itemfeelist.SelectSingleNode("Amount").InnerText == null ? "" : itemfeelist["Amount"].InnerText;
                                                }
                                                settlement_amt_type subobj5 = new settlement_amt_type();
                                                subobj5.amount = Math.Round(Convert.ToDouble(amount1), 2);

                                                if (type1 == "FixedClosingFee")
                                                    type1 = "Fixed closing fee";

                                                if (type1 == "FixedClosingFee IGST")
                                                    type1 = "Fixed closing fee IGST";

                                                if (type1 == "RefundCommission")
                                                    type1 = "Refund commission";

                                                if (type1 == "RefundCommission IGST")
                                                    type1 = "Refund commission IGST";

                                                if (type1 == "ShippingChargeback")
                                                    type1 = "Shipping Chargeback";

                                                if (type1 == "ShippingChargeback CGST")
                                                    type1 = "Shipping Chargeback CGST";

                                                if (type1 == "ShippingChargeback SGST")
                                                    type1 = "Shipping Chargeback SGST";

                                                subobj5.description = type1;
                                                li.Add(subobj5);
                                            }
                                        }// end of if itemfeelist
                                    }// end of froeach itemfeelist
                                }// end of if itemfee                                      
                                #endregion

                                #region promotion
                                XmlNode promotion = item_detail.SelectSingleNode("PromotionAdjustment");
                                string shippingtype = "";
                                string shippingamount = "";
                                if (promotion != null)
                                {
                                    if (promotion["Type"] != null)
                                    {
                                        shippingtype = promotion.SelectSingleNode("Type").InnerText == null ? "" : promotion["Type"].InnerText;
                                    }
                                    if (promotion["Amount"] != null)
                                    {
                                        shippingamount = promotion.SelectSingleNode("Amount").InnerText == null ? "" : promotion["Amount"].InnerText;
                                    }
                                    settlement_amt_type subobj6 = new settlement_amt_type();
                                    subobj6.amount = Math.Round(Convert.ToDouble(shippingamount), 2);
                                    if (shippingtype == "Shipping")
                                        shippingtype = "Shipping discount";
                                    subobj6.description = shippingtype;
                                    li.Add(subobj6);
                                }
                                #endregion
                            }// end of for each item_detail
                        }// end of if item
                        # endregion
                    }// end of if Fullfillment
                }// end of froeach
                #endregion
                objjson1.Add(new AmazonreconciliationOrder
                {
                    reconciliationorder = objreconciliationorder,
                });
                string abc = "API";
                short sourcetype = 2;
                //objsales.SaveBulksettlementdata(objjson1, id, m_marketplace_id, SellerId);
            }// end of try 
            catch (Exception ex)
            {
            }
            return Success;
        }



        public string Call_API_Settlement(List<api_parameter> lstApiDetails, int strSeller_Id, int id)//when we use Flat File V2 Report
        {
            string success = "";
            string responseXml = "";
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

                String sellerid = my_unique_id;//"ALX5VZP4KM90V";
                String Secret_Key = t_secret_Key;// "tB1OE8WvQB4xZ8bOtD8aFIfW2H1uwfAIfYDMfVx8";
                String AWS_Access_Key_ID = t_access_Key_id;// "AKIAILZANI6ZKK4Z3PAQ";
                String marketplaceid = market_palce_id;//"A21TJRUUN4KGV";
             
                String accessKeyId = AWS_Access_Key_ID;
                String secretAccessKey = Secret_Key;

                const string applicationName = "AmazonJavascriptScratchpad";
                const string applicationVersion = "1.0.0.0";
                MarketplaceWebServiceConfig config = new MarketplaceWebServiceConfig();
                config.ServiceURL = "https://mws.amazonservices.in";



                MarketplaceWebService service = new MarketplaceWebServiceClient(
                   accessKeyId,
                   secretAccessKey,
                   applicationName,
                   applicationVersion,
                   config);              

                try
                {
                    GetReportListRequest request = new GetReportListRequest();
                    DateTime DtStart;
                    DateTime DtEnd;
                    DtStart = createA;//DateTime.Parse("2017-12-01 23:59:59");
                    DtEnd = createB;//DateTime.Parse("2017-12-08 23:59:59");

                    //DtStart = DateTime.Parse("2018-01-01 23:59:59");
                    //DtEnd = DateTime.Parse("2018-01-31 23:59:59");

                    request.Merchant = sellerid;// merchantId;               
                    request.AvailableFromDate = DtStart;
                    request.AvailableToDate = DtEnd;

                    List<string> list2 = new List<string>();
                    list2.Add("_GET_V2_SETTLEMENT_REPORT_DATA_FLAT_FILE_");
                    //list2.Add("_GET_V2_SETTLEMENT_REPORT_DATA_FLAT_FILE_V2_");//_GET_V2_SETTLEMENT_REPORT_DATA_FLAT_FILE_                
                    request.ReportTypeList = new TypeList();
                    request.ReportTypeList.Type = list2; // _GET_V2_SETTLEMENT_REPORT_DATA_XML_; // TypeList.WithType(list1);

                    List<string> InvokeGetReportList = new List<string>();

                    InvokeGetReportList = GetReportListSample.InvokeGetReportList(service, request);
                    foreach (var item in InvokeGetReportList)
                    {
                        GetReportRequest request1 = new GetReportRequest();
                        request1.Merchant = sellerid;// merchantId;                                       
                        request1.ReportId = item;

                        string filename = item + ".txt";
                       string strFolderPath = HttpContext.Current.Server.MapPath("~/UploadExcel/" + strSeller_Id + "/settlement/");//"~/UploadExcel/" + seller_id + "/settlement/" + filename
                        if (!Directory.Exists(strFolderPath))
                        {
                            Directory.CreateDirectory(strFolderPath);
                        }
                        strFolderPath = strFolderPath + item + ".txt";
                      
                        FileStream file1 = File.Open(strFolderPath, FileMode.OpenOrCreate, FileAccess.ReadWrite);
                        request1.Report = file1;
                        GetReportResponse resp = service.GetReport(request1);
                        ResponseHeaderMetadata rhmd = resp.ResponseHeaderMetadata;
                        file1.Close();
                        //file1.Flush();
                        //byte[] bytes = new byte[file1.Length];
                        //int numBytesToRead = (int)file1.Length;
                        //int numBytesRead = 0;
                        //while (numBytesToRead > 0)
                        //{
                           
                        //   int n = file1.Read(bytes, numBytesRead, numBytesToRead);
                           
                        //    if (n == 0)
                        //        break;
                        //    numBytesRead += n;
                        //    numBytesToRead -= n;
                        //}
                        //string xmldata = System.Text.Encoding.UTF8.GetString(bytes);
                        //strFolderPath = xmldata;
                        Browse_Upload_Excel_Utility obj = new Browse_Upload_Excel_Utility();
                        CronJobController obj_crown = new CronJobController();
                        List<AmazonreconciliationOrder> objjson1 = null;
                        try
                        {
                            objjson1 = obj.ReadSettlementFile_Amazon_flatfile(strFolderPath, strSeller_Id);
                        }
                        catch (Exception e)
                        {
                        }
                        try
                        {
                            if (objjson1 == null)
                            {
                                objjson1 = obj.ReadSettlementFile_Amazon_v2(strFolderPath, strSeller_Id);
                            }
                        }
                        catch (Exception e)
                        {
                            Writelog log = new Writelog();
                            log.write_exception_log(id.ToString(), "CronJobController", "while ReadSettlementFile_Amazon_v2", DateTime.Now, e);
                        }
                        try
                        {
                            if (objjson1 != null)
                            {
                                success = obj_crown.SaveBulksettlementdata(objjson1, id, m_marketplace_id, strSeller_Id, filename);// for save 
                            }
                        }
                        catch (Exception e)
                        {
                            Writelog log = new Writelog();
                            log.write_exception_log(id.ToString(), "CronJobController", "SettlementCrown", DateTime.Now, e);
                        }



                        //Read_SettlementData(xmldata, strSeller_Id, id, m_marketplace_id);
                        int errw = 0;
                        errw++;
                    }
                }
                catch (MarketplaceWebServiceException ex)
                {
                    success = "F";
                }
                catch (Exception ex)
                {
                    success = "F";
                }

            }//end of for each loop
            return success;
        }

    }
}