using Newtonsoft.Json;
using SellerVendor.Areas.Seller.Models;
using SellerVendor.Areas.Seller.Models.DBContext;
using SellerVendor.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Web;
using System.Web.Mvc;
using System.Web.Script.Serialization;
using System.Xml;
using System.Runtime.InteropServices;
using Excel = Microsoft.Office.Interop.Excel;
using System.Data;
using System.Globalization;
using System.Text.RegularExpressions;
using LinqToExcel;
using LinqToExcel.Attributes;
using System.Configuration;
using MySql.Data.MySqlClient;
namespace SellerVendor.Areas.Seller.Controllers
{
    public class SalesController : Controller
    {
        public SellerContext dba = new SellerContext();
        public SellerAdminContext db = new SellerAdminContext();
        comman_function cf = null;

        List<tbl_SalesOrder> customers = new List<tbl_SalesOrder>();

        /// <summary>
        /// parse data from Xml and bind in a customer list
        /// </summary>
        /// <returns></returns>
        public ActionResult Index1()
        {

            tbl_SalesOrder lstDataDetail = null;

            //Load the XML file in XmlDocument.
            XmlDocument doc = new XmlDocument();
            doc.Load(Server.MapPath("~/Areas/XML/XMLFile2.xml"));

            //Loop through the selected Nodes.

            XmlNodeList xnList = doc.SelectNodes("/AmazonEnvelope/Message/Order");

            foreach (XmlNode xn in xnList)
            {
                XmlNode anode = xn;
                if (anode != null)
                {
                    XmlNode addre = anode.SelectSingleNode("FulfillmentData");
                    if (addre != null)
                    {
                        XmlNode addres = addre.SelectSingleNode("Address");


                        if (addres != null)
                        {
                            XmlNodeList order = anode.SelectNodes("OrderItem");//get order details 

                            if (order != null)
                            {
                                List<OrderItem> obj = new List<OrderItem>();
                                string abc = "";
                                string xyz = "";
                                string asin = "";
                                string sku = "";
                                string itemstatus = "";
                                string productname = "";
                                string quantity = "";
                                foreach (XmlNode aa in order)
                                {
                                    if (aa != null)
                                    {
                                        if (aa.SelectSingleNode("ASIN") != null)
                                        {
                                            asin = aa.SelectSingleNode("ASIN").InnerText == null ? "" : aa["ASIN"].InnerText;
                                        }
                                        if (aa.SelectSingleNode("SKU") != null)
                                        {
                                            sku = aa.SelectSingleNode("SKU").InnerText == null ? "" : aa["SKU"].InnerText;
                                        }
                                        if (aa.SelectSingleNode("ItemStatus") != null)
                                        {
                                            itemstatus = aa.SelectSingleNode("ItemStatus").InnerText == null ? "" : aa["ItemStatus"].InnerText;
                                        }
                                        if (aa.SelectSingleNode("ProductName") != null)
                                        {
                                            productname = aa.SelectSingleNode("ProductName").InnerText == null ? "" : aa["ProductName"].InnerText;
                                        }
                                        if (aa.SelectSingleNode("Quantity") != null)
                                        {
                                            quantity = aa.SelectSingleNode("Quantity").InnerText == null ? "" : aa["Quantity"].InnerText;
                                        }
                                        XmlNode prom = aa.SelectSingleNode("Promotion");//get promotion 
                                        if (prom != null)
                                        {
                                            if (prom.SelectSingleNode("PromotionIDs") != null)
                                            {
                                                abc = prom.SelectSingleNode("PromotionIDs").InnerText == null ? "" : prom["PromotionIDs"].InnerText;
                                            }
                                            if (prom.SelectSingleNode("ShipPromotionDiscount") != null)
                                            {

                                                xyz = prom.SelectSingleNode("ShipPromotionDiscount").InnerText == null ? "" : prom["ShipPromotionDiscount"].InnerText;
                                            }
                                        }
                                        //////////// ////changes in item price////////////////////////////////////////////
                                        XmlNodeList itempriceslist = aa.SelectNodes("ItemPrice/Component");
                                        List<ItemPrice> objitemprice = new List<ItemPrice>();
                                        if (itempriceslist != null)
                                        {

                                            string type = "";
                                            string amount = "";
                                            string shipamount = "";
                                            foreach (XmlNode pricelist in itempriceslist)
                                            {
                                                if (pricelist != null)
                                                {
                                                    if (pricelist.SelectSingleNode("Type") != null)
                                                    {

                                                        type = pricelist.SelectSingleNode("Type").InnerText == null ? "" : pricelist["Type"].InnerText;
                                                        if (type == "Principal")
                                                        {
                                                            if (pricelist.SelectSingleNode("Amount") != null)
                                                            {
                                                                amount = pricelist.SelectSingleNode("Amount").InnerText == null ? "" : pricelist["Amount"].InnerText;
                                                            }
                                                        }
                                                        if (type == "Shipping")
                                                        {
                                                            if (pricelist.SelectSingleNode("Amount") != null)
                                                            {
                                                                shipamount = pricelist.SelectSingleNode("Amount").InnerText == null ? "" : pricelist["Amount"].InnerText;
                                                            }
                                                        }
                                                    }



                                                }


                                            }//End of For each loop pricelist 
                                            if (shipamount == "")
                                                shipamount = "0";
                                            if (amount == "")
                                                amount = "0";
                                            objitemprice.Add(new ItemPrice
                                            {
                                                //Type = type,
                                                pAmonu = Convert.ToDecimal(amount),
                                                shipAmonu = Convert.ToDecimal(shipamount)

                                            });
                                        }




                                        /////////////////////////End changes in item price///////////////////////////

                                        obj.Add(new OrderItem
                                        {
                                            ASIN = asin,
                                            SKU = sku,
                                            ItemStatus = itemstatus,
                                            ProductName = productname,
                                            Quantity = quantity,

                                            PromotionIDs = abc,
                                            ShipPromotionDiscount = xyz,
                                            itemprice = objitemprice
                                        });
                                    }//Enddddddddd
                                }
                                customers.Add(new tbl_SalesOrder
                                {

                                    AmazonOrderID = (anode.SelectSingleNode("AmazonOrderID") == null) ? "" : anode["AmazonOrderID"].InnerText,
                                    MerchantOrderID = anode.SelectSingleNode("MerchantOrderID") == null ? "" : anode["MerchantOrderID"].InnerText,
                                    PurchaseDate = anode.SelectSingleNode("PurchaseDate").InnerText == null ? "" : anode["PurchaseDate"].InnerText,
                                    LastUpdatedDate = anode.SelectSingleNode("LastUpdatedDate").InnerText == null ? "" : anode["LastUpdatedDate"].InnerText,
                                    OrderStatus = anode.SelectSingleNode("OrderStatus").InnerText == null ? "" : anode["OrderStatus"].InnerText,
                                    SalesChannel = anode.SelectSingleNode("SalesChannel").InnerText == null ? "" : anode["SalesChannel"].InnerText,
                                    IsBusinessOrder = anode.SelectSingleNode("IsBusinessOrder").InnerText == null ? "" : anode["IsBusinessOrder"].InnerText,
                                    FulfillmentChannel = addre.SelectSingleNode("FulfillmentChannel").InnerText == null ? "" : addre["FulfillmentChannel"].InnerText,
                                    ShipServiceLevel = addre.SelectSingleNode("ShipServiceLevel").InnerText == null ? "" : addre["ShipServiceLevel"].InnerText,
                                    City = addres.SelectSingleNode("City").InnerText == null ? "" : addres["City"].InnerText,
                                    State = addres.SelectSingleNode("State").InnerText == null ? "" : addres["State"].InnerText,
                                    PostalCode = addres.SelectSingleNode("PostalCode").InnerText == null ? "" : addres["PostalCode"].InnerText,
                                    Country = addres.SelectSingleNode("Country").InnerText == null ? "" : addres["Country"].InnerText,
                                    Orders = obj
                                });


                            }//END OF IF(ORDER!= null)
                        }
                    }

                }

            }
            // saveXMLData(customers);
            return View(customers);
        }


        public ActionResult Index()
        {
            cf = new comman_function();
            if (!cf.session_check())
                return RedirectToAction("Login", "Home");
            int SellerId = Convert.ToInt32(Session["SellerID"]);
            tbl_SalesOrder lstDataDetail = null;
            List<AmazonOrderDetails> obj = null;
            try
            {
                //Load the XML file in XmlDocument.
                XmlDocument doc = new XmlDocument();

                //doc.Load(Server.MapPath("~/Areas/XML/AmazonOrderTest.xml"));
                doc.Load(Server.MapPath("~/Areas/XML/AmazonorderItem.xml"));
                string a = doc.NamespaceURI;

                //Loop through the selected Nodes.
                XmlNodeList xnList = doc.SelectNodes("/ListOrdersResponse/ListOrdersResult/Orders/Order");

                int kk = 0;
                foreach (XmlNode xn in xnList)
                {
                    kk++;
                    decimal amount = 0;
                    string lastDate = "", puchasedate = "", amazonorderid = "", ordertype = "", buyeremail = "", isreplacement = "", lastupdateDate = "",
                        shipservicelevel = "", noofitemshipped = "", orderstatus = "", saleschannel = "", isbusinessorder = "", itemunshipped = "",
                        buyername = "", ispremiumorder = "", earliestshipdate = "", marketplaceid = "", fullfillmentchannel = "", paymentmode = "",
                        isprime = "", shipmentservicelevel = "", sellerorderid = "", paymentmethoddetail = "", currencycode = "", stateregion = "", city = "", name = "",
                        postalcode = "", address1 = "", address2 = "", countrycode = "", detailsamazonid = "";
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
                        if (anode["IsBusinessOrder"] != null)
                        {
                            isbusinessorder = anode.SelectSingleNode("IsBusinessOrder").InnerText == null ? "" : anode["IsBusinessOrder"].InnerText;
                        }
                        if (anode["NumberOfItemsUnshipped"] != null)
                        {
                            itemunshipped = anode.SelectSingleNode("NumberOfItemsUnshipped").InnerText == null ? "" : anode["NumberOfItemsUnshipped"].InnerText;
                        }
                        if (anode["BuyerName"] != null)
                        {
                            buyername = anode.SelectSingleNode("BuyerName").InnerText == null ? "" : anode["BuyerName"].InnerText;
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
                        }//end of if(address)
                        else { }//end of else(address)

                        //////////////////////////////////////code for fetch data from amazon order list//////////////////////

                        XmlDocument doc2 = new XmlDocument();
                        //doc2.Load(Server.MapPath("~/Areas/XML/amazonorderItemList.xml"));
                        doc2.Load(Server.MapPath("~/Areas/XML/AmazonLTOrderList.xml"));
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
                                obj = new List<AmazonOrderDetails>();
                                if (Amazonid == detailsamazonid)
                                {
                                    XmlNodeList detail = itemdetails.SelectNodes("OrderItems/OrderItem");
                                    if (detail != null)
                                    {

                                        foreach (XmlNode orderdetails in detail)
                                        {

                                            //List<AmazonOrderDetails> obj = null; 
                                            int orderquantity = 0, quantityshippped = 0;
                                            decimal shiptaxamount = 0, promotionamount = 0, shippingpriceamount = 0, itempriceamount = 0, itemtaxamount = 0, shippingdisamount = 0, giftwraptaxamount = 0, giftwrappriceamount = 0,
                                                producttaxprice = 0, productprice = 0, shippingtaxprice = 0, shipping = 0, gifttax = 0, giftprice = 0;
                                            string title = "", asin = "", sellersku = "", orderitemid = "", shiptaxcurrenceycode = "", promotioncurrecode = "",
                                                promotionid = "", shippingpricecode = "", itempricecode = "", itemtaxcode = "", shippingdiscode = "", isgift = "",
                                                giftwraptaxcode = "", giftwrappricecode = "";
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

                                                XmlNode shippingtax = orderdetails.SelectSingleNode("ShippingTax");
                                                if (shippingtax != null)
                                                {
                                                    if (shippingtax["CurrencyCode"] != null)
                                                    {
                                                        shiptaxcurrenceycode = shippingtax.SelectSingleNode("CurrencyCode").InnerText == null ? "" : shippingtax["CurrencyCode"].InnerText;
                                                    }
                                                    if (shippingtax["Amount"] != null)
                                                    {
                                                        shiptaxamount = Convert.ToDecimal(shippingtax.SelectSingleNode("Amount").InnerText == null ? "" : shippingtax["Amount"].InnerText);
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
                                                        promotionamount = Convert.ToDecimal(promotiondiscount.SelectSingleNode("Amount").InnerText == null ? "" : promotiondiscount["Amount"].InnerText);
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
                                                        shippingpriceamount = Convert.ToDecimal(shippingprice.SelectSingleNode("Amount").InnerText == null ? "" : shippingprice["Amount"].InnerText);
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
                                                        itempriceamount = Convert.ToDecimal(itemprice.SelectSingleNode("Amount").InnerText == null ? "" : itemprice["Amount"].InnerText);
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
                                                        itemtaxamount = Convert.ToDecimal(itemtax.SelectSingleNode("Amount").InnerText == null ? "" : itemtax["Amount"].InnerText);
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
                                                        shippingdisamount = Convert.ToDecimal(shippingdiscount.SelectSingleNode("Amount").InnerText == null ? "" : shippingdiscount["Amount"].InnerText);
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
                                                        giftwraptaxamount = Convert.ToDecimal(giftwraptax.SelectSingleNode("Amount").InnerText == null ? "" : giftwraptax["Amount"].InnerText);
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
                                                        giftwrappriceamount = Convert.ToDecimal(giftwrapprice.SelectSingleNode("Amount").InnerText == null ? "" : giftwrapprice["Amount"].InnerText);
                                                    }
                                                }// end of if(giftwraptax)

                                                // ------------------------get categorytax use sku No from inventory table--------------------////

                                                var get_inventory = dba.tbl_inventory.Where(w => w.sku.ToLower() == sellersku.ToLower() && w.tbl_sellers_id == SellerId).FirstOrDefault();// get sku no from inventory table 
                                                if (get_inventory != null)
                                                {
                                                    var get_cateory = dba.tbl_item_category.Where(q => q.tbl_sellers_id == SellerId && q.id == get_inventory.tbl_item_category_id && q.isactive == 1).FirstOrDefault();// get categroy tax from category table
                                                    if (get_cateory != null)
                                                    {
                                                        var categort_tax = get_cateory.tax_rate;
                                                        var get_subcategory = dba.tbl_item_subcategory.Where(r => r.tbl_item_category_id == get_cateory.id && r.isactive == 1 && r.tbl_sellers_id == SellerId).FirstOrDefault();// get subcategory tax from subcategory table
                                                        if (get_subcategory != null)
                                                        {
                                                            var sub_categorytax = get_subcategory.tax_rate;
                                                            if (sub_categorytax != null)
                                                            {
                                                                producttaxprice = itempriceamount * Convert.ToDecimal(sub_categorytax) / 100;
                                                                productprice = itempriceamount - producttaxprice;
                                                                shippingtaxprice = shippingpriceamount * Convert.ToDecimal(sub_categorytax) / 100;
                                                                shipping = shippingpriceamount - shippingtaxprice;
                                                                gifttax = giftwrappriceamount * Convert.ToDecimal(sub_categorytax) / 100;
                                                                giftprice = giftwrappriceamount - gifttax;
                                                            }
                                                        }// end of if (get_subcategory)
                                                        else
                                                        {
                                                            producttaxprice = itempriceamount * Convert.ToDecimal(categort_tax) / 100;
                                                            productprice = itempriceamount - producttaxprice;
                                                            shippingtaxprice = shippingpriceamount * Convert.ToDecimal(categort_tax) / 100;
                                                            shipping = shippingpriceamount - shippingtaxprice;
                                                            gifttax = giftwrappriceamount * Convert.ToDecimal(categort_tax) / 100;
                                                            giftprice = giftwrappriceamount - gifttax;
                                                        }
                                                    }// end of if(get_cateory)
                                                }// end of if(get_inventory)

                                                //-------------------------------------End------------------------------------------------------///
                                                obj.Add(new AmazonOrderDetails
                                                {
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
                                                });
                                            }// end of if(order details)
                                        }//end of froeach loop orderdetails
                                    }//end of if(detail!=null)
                                }//end of if(where compare)
                            }
                        }//end of if(xnList1)

                        else { }// end of xnlist1

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
                            OrderDetails = obj,
                        });

                    }// end of if(anode!=null)
                    else //if (anode != null)
                    {
                        int jjj = 0;
                        jjj++;
                    }

                    //saveXMLData(customers);
                }// end of foreach loop xnList
            }// end of try
            catch (Exception ex)
            {
            }
            return View(customers);
        }

        /// <summary>
        /// save xml data into  salesorder table in db 
        /// </summary>
        /// <param name="customers"></param>
        /// <returns></returns>

        public string saveXMLData(List<tbl_SalesOrder> customers)
        {
            cf = new comman_function();
            bool ss = cf.session_check();
            tbl_sales_order objsale = new tbl_sales_order();
            tbl_customer_details objcustumer = new tbl_customer_details();
            tbl_order_history objhistory = new tbl_order_history();
            int SellerId = Convert.ToInt32(Session["SellerID"]);
            try
            {
                string datasaved = "";
                string lowername = "";
                string stateregion = "", shippingname = "", city = "", country = "", postalcode = "", address1 = "", address2 = "";

                if (customers != null)
                {
                    foreach (var item in customers)
                    {
                        string Message;
                        bool flag = false;
                        string abc1 = item.SalesChannel.ToLower();
                        lowername = abc1.Split('.').First(); ;// convert name to lower case for ckh in db and split also
                        var getmarketplacedetails = db.m_marketplace.Where(a => a.isactive == 1).ToList();// get details from marketplace table in seller admin
                        //var getdetails = dba.tbl_sales_order.Where(a => a.amazon_order_id == item.AmazonOrderID).FirstOrDefault();
                        var getfullfilled = dba.m_fullfilled.ToList();
                        //var customer_details = dba.tbl_customer_details.Where(a => a.tbl_seller_id == SellerId).ToList();
                        //if (getdetails.amazon_order_id != null)
                        //{
                        //    getdetails.order_status = item.OrderStatus;
                        //    dba.Entry(getdetails).State = EntityState.Modified;
                        //    dba.SaveChanges();
                        //    flag = true;
                        //    Message = "Order status is updated successfully";


                        //}// end of if (getdetails.orderstatus)
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

                        //if (getdetails == null)
                        // {
                        var customer_details = dba.tbl_customer_details.Where(a => a.tbl_seller_id == SellerId && a.State_Region.ToLower() == stateregion && a.shipping_Buyer_Name.ToLower() == shippingname && a.City.ToLower() == city && a.Country_Code.ToLower() == country && a.Postal_Code.ToLower() == postalcode && a.Address_1.ToLower() == address1 && a.Address_2.ToLower() == address2).FirstOrDefault();
                        if (customer_details != null)
                        {
                            customer_details.customer_count += 1;
                            dba.Entry(customer_details).State = EntityState.Modified;
                            db.SaveChanges();
                        }// end of if (customer_details)
                        //if (customer_details != null)
                        //{
                        // foreach (var custdetails in customer_details)
                        //{
                        // if (stateregion == custdetails.State_Region.ToLower() && shippingname == custdetails.shipping_Buyer_Name.ToLower() && city == custdetails.City.ToLower() && country == custdetails.Country_Code.ToLower() && postalcode == custdetails.Postal_Code.ToLower() && address1 == custdetails.Address_1.ToLower() && address2 == custdetails.Address_2.ToLower())
                        //{
                        //objcustumer.customer_count = 1;
                        //}
                        //}// end of foreach(custdetails)
                        // }// end of if (customer_details)

                        else
                        {
                            objcustumer.State_Region = item.StateRegion;
                            objcustumer.shipping_Buyer_Name = item.ShippingName;
                            objcustumer.City = item.City;
                            objcustumer.Country_Code = item.CountryCode;
                            objcustumer.Postal_Code = item.PostalCode;
                            objcustumer.Address_1 = item.Address_1;
                            objcustumer.Address_2 = item.Address_2;
                            objcustumer.tbl_seller_id = SellerId;
                            dba.tbl_customer_details.Add(objcustumer);
                            dba.SaveChanges();
                        }
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
                        objsale.sales_channel = item.SalesChannel;
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
                        //objsale.n_item_orderstatus = 1;

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

                        foreach (var abc in getmarketplacedetails)
                        {
                            string name = abc.name.ToLower();
                            if (lowername == name)
                            {
                                objsale.tbl_Marketplace_Id = abc.id;
                                break;
                            }
                        }//end of for each loop ( for get marketplace details)
                        var getdetails = dba.tbl_sales_order_status.Where(a => a.is_active == 0).ToList();
                        string name1 = item.OrderStatus.ToLower();
                        foreach (var item1 in getdetails)
                        {
                            string name = item1.sales_order_status.ToLower();
                            if (name == name1)
                            {
                                objsale.n_item_orderstatus = item1.id;
                                break;
                            }
                        }// end of foreach item1

                        dba.tbl_sales_order.Add(objsale);
                        dba.SaveChanges();



                        if (item.OrderDetails != null)
                        {
                            tbl_sales_order_details objsaledetails = new tbl_sales_order_details();
                            foreach (var itemdetails in item.OrderDetails)
                            {
                                string promotiondiscount = Convert.ToString(itemdetails.promotion_amount);
                                string shippingdiscount = Convert.ToString(itemdetails.shipping_discount_amt);

                                string shippingtaxamount = Convert.ToString(itemdetails.shippingtaxprice);
                                string shippingprice = Convert.ToString(itemdetails.shipping);
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
                                    objsaledetails.shipping_tax_Amount = Convert.ToDouble(itemdetails.shippingtaxprice);
                                }

                                if (promotiondiscount == "")
                                {
                                    objsaledetails.promotion_amount = 0;
                                }
                                else
                                {
                                    objsaledetails.promotion_amount = Convert.ToDouble(itemdetails.promotion_amount);
                                }

                                if (shippingprice == "")
                                {
                                    objsaledetails.shipping_price_Amount = 0;
                                }
                                else
                                {
                                    objsaledetails.shipping_price_Amount = Convert.ToDouble(itemdetails.shipping);
                                }

                                if (itempriceamount == "")
                                {
                                    objsaledetails.item_price_amount = 0;
                                }
                                else
                                {
                                    objsaledetails.item_price_amount = Convert.ToDouble(itemdetails.productprice);
                                }

                                if (itemtaxamount == "")
                                {
                                    objsaledetails.item_tax_amount = 0;
                                }
                                else
                                {
                                    objsaledetails.item_tax_amount = Convert.ToDouble(itemdetails.producttaxprice);
                                }

                                if (shippingdiscount == "")
                                {
                                    objsaledetails.shipping_discount_amt = 0;
                                }
                                else
                                {
                                    objsaledetails.shipping_discount_amt = Convert.ToDouble(itemdetails.shipping_discount_amt);
                                }
                                if (giftwraptaxamount == "")
                                {
                                    objsaledetails.giftwraptax_amount = 0;
                                }
                                else
                                {
                                    objsaledetails.giftwraptax_amount = Convert.ToDouble(itemdetails.gifttax);
                                }
                                if (giftwrappriceamount == "")
                                {
                                    objsaledetails.giftwrapprice_amount = 0;
                                }
                                else
                                {
                                    objsaledetails.giftwrapprice_amount = Convert.ToDouble(itemdetails.giftprice);
                                }
                                objsaledetails.giftwrapprice_code = itemdetails.giftwrappricecode;
                                objsaledetails.giftwraptaxcode = itemdetails.giftwraptaxcode;
                                objsaledetails.is_active = 1;
                                objsaledetails.tbl_seller_id = SellerId;
                                objsaledetails.tbl_sales_order_id = objsale.id;
                                objsaledetails.status_updated_by = SellerId;
                                objsaledetails.status_updated_on = DateTime.Now;
                                objsaledetails.n_order_status_id = Convert.ToInt16(objsale.n_item_orderstatus);
                                objsaledetails.amazon_order_id = objsale.amazon_order_id;
                                objsaledetails.dispatch_bydate = objsale.earliest_ship_date;
                                objsaledetails.dispatchAfter_date = objsale.purchase_date;
                                dba.tbl_sales_order_details.Add(objsaledetails);
                                dba.SaveChanges();
                                //--------------------------------- save data in tax table----------------------///
                                tbl_tax objtax = new tbl_tax();
                                objtax.giftwarp_tax = Convert.ToDouble(giftwraptaxamount);
                                objtax.shippint_tax_amount = Convert.ToDouble(shippingtaxamount);
                                objtax.product_tax = Convert.ToDouble(itemtaxamount);
                                objtax.tbl_seller_id = SellerId;
                                objtax.tbl_referneced_id = objsaledetails.id;
                                objtax.reference_type = 3;
                                objtax.isactive = 1;
                                dba.tbl_tax.Add(objtax);
                                dba.SaveChanges();

                                /////------------------------------------End--------------------------------------///

                                //---------------------------- update tblinventory_details status when  fullfilled by AFN
                                if (objsale.fullfillment_channel == "AFN")
                                {
                                    var get_inventorydetails = dba.tbl_inventory.Where(a => a.sku.ToLower() == objsaledetails.sku_no.ToLower() && a.tbl_sellers_id == SellerId).FirstOrDefault();
                                    if (get_inventorydetails != null)
                                    {
                                        var get_inventoryItemdetails = dba.tbl_inventory_details.Where(a => a.tbl_inventory_id == get_inventorydetails.id && a.m_marketplace_id == 3 && a.tbl_sellers_id == SellerId && a.m_item_status_id == 1).ToList();
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


                                //--------------- save data in table order history -------------------//
                                var getstatus = dba.tbl_sales_order_status.Where(a => a.is_active == 0).ToList();

                                objhistory.created_on = DateTime.Now;
                                objhistory.tbl_orders_id = objsale.id;
                                objhistory.tbl_seller_id = SellerId;
                                objhistory.tbl_orderDetails_Id = objsaledetails.id;
                                objhistory.ASIN = objsaledetails.asin;
                                objhistory.SKU = objsaledetails.sku_no;
                                objhistory.Quantity = objsaledetails.quantity_ordered;
                                string itemstatus = objsale.order_status.ToLower();
                                foreach (var status in getstatus)
                                {
                                    string name = status.sales_order_status.ToLower();
                                    if (itemstatus == name)
                                    {
                                        objhistory.t_order_status = status.id;
                                        break;
                                    }
                                }//end of for each loop ( for get status details)                                   
                                objhistory.OrigialOrderID = objsale.amazon_order_id;
                                objhistory.ShipmentDate = objsale.earliest_ship_date;
                                objhistory.tbl_marketplace_id = objsale.tbl_Marketplace_Id;
                                dba.tbl_order_history.Add(objhistory);
                                dba.SaveChanges();

                                //-----------------------------End------------------------------------//
                            }// end of foreach loop (item.orderDetails)
                        }// end of if(item.orderdetails)
                        //}// end of if(get details)
                    }//end of foreach loop(item)
                }//end of if(customer)
                return datasaved;
            }
            catch (Exception ex)
            {
            }
            return "";
        }

        /// <summary>
        /// This action is for listing of Sales Details 
        /// </summary>
        /// <returns></returns>
        public ActionResult MySalesOrderList()
        {
            cf = new comman_function();
            if (!cf.session_check())
                return RedirectToAction("Login", "Home");
            return View();
        }
        public JsonResult GetSaleDetails1()
        {
            cf = new comman_function();
            bool ss = cf.session_check();
            int SellerId = Convert.ToInt32(Session["SellerID"]);
            var get_salesdetails = (from ob_tbl_sales_order in dba.tbl_sales_order
                                    join ob_tbl_sales_order_details in dba.tbl_sales_order_details on ob_tbl_sales_order.id
                                     equals ob_tbl_sales_order_details.tbl_sales_order_id

                                    select new SellerUtility
                                    {
                                        ob_tbl_sales_order = ob_tbl_sales_order,
                                        ob_tbl_sales_order_details = ob_tbl_sales_order_details

                                    }).Where(a => a.ob_tbl_sales_order.is_active == 1 && a.ob_tbl_sales_order.tbl_sellers_id == SellerId).OrderByDescending(a => a.ob_tbl_sales_order.id).ToList();

            return new JsonResult { Data = get_salesdetails, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        public JsonResult GetSaleDetails()
        {
            List<viewSalesOrder> lst_salesorder = new List<viewSalesOrder>();
            try
            {
                cf = new comman_function();              
                int sellers_id = Convert.ToInt32(Session["SellerID"]);
                var get_salesdetails = (from ob_tbl_sales_order in dba.tbl_sales_order.OrderByDescending(a => a.id)                                      
                                        join ob_tbl_sales_order_status in dba.tbl_sales_order_status on ob_tbl_sales_order.n_item_orderstatus equals (ob_tbl_sales_order_status.id)
                                        into JoinedEmpDept4
                                        from proj4 in JoinedEmpDept4.DefaultIfEmpty()
                                        join ob_m_fullfilled in dba.m_fullfilled on ob_tbl_sales_order.n_fullfilled_id equals (ob_m_fullfilled.id)
                                        into JoinedEmpDept5
                                        from proj5 in JoinedEmpDept5.DefaultIfEmpty()

                                        join ob_tbl_customer_details in dba.tbl_customer_details on ob_tbl_sales_order.tbl_Customer_Id equals (ob_tbl_customer_details.id)
                                        into JoinedEmpDept6
                                        from proj6 in JoinedEmpDept6.DefaultIfEmpty()

                                        select new SellerUtility
                                        {
                                            ob_tbl_sales_order = ob_tbl_sales_order,                                         
                                            ob_tbl_sales_order_status = proj4,
                                            ob_m_fullfilled = proj5,
                                            ob_tbl_customer_details = proj6
                                        }).Where(a => a.ob_tbl_sales_order.is_active == 1 && a.ob_tbl_sales_order.tbl_sellers_id == sellers_id && a.ob_tbl_sales_order.order_status != "Cancelled").ToList();


                string hold = "";
                string skuno = "";
                int counter = 0;
                foreach (var item in get_salesdetails)
                {
                    counter++;
                    if (counter > 3500)
                    {
                        break;
                    } 
                    viewSalesOrder salesorder = new viewSalesOrder();
                    var abc = db.m_marketplace.Where(a => a.isactive == 1 && a.id == item.ob_tbl_sales_order.tbl_Marketplace_Id).FirstOrDefault();

                    var get_orderitemdetails = dba.tbl_sales_order_details.Where(a => a.tbl_sales_order_id == item.ob_tbl_sales_order.id).FirstOrDefault();// to get sales order details 
                    if (get_orderitemdetails != null)
                    {
                        skuno = get_orderitemdetails.sku_no.ToLower();
                    }

                    //var get_itemdetails = dba.tbl_inventory.Where(a => a.tbl_sellers_id == sellers_id && a.isactive == 1 && a.sku.ToLower() == skuno).FirstOrDefault();
                    //if (get_itemdetails != null)
                    //{
                    //    salesorder.itemcount = Convert.ToInt16(get_itemdetails.item_count);
                    //}

                    //var get_itemdetails = dba.tbl_inventory.Where(a => a.tbl_sellers_id == sellers_id && a.isactive == 1).ToList();// to get Inventory Details list
                    //if (get_itemdetails != null)
                    //{
                    //    foreach (var item1 in get_itemdetails)
                    //    {
                    //        string itemsku = item1.sku.ToLower();
                    //        if (skuno == itemsku)
                    //        {
                    //            salesorder.itemcount = Convert.ToInt16(item1.item_count);
                    //        }
                    //    }// end of foreach(item1)
                    //}// end of if(get_itemdetails)
                    salesorder.marketplaceid = abc.id;
                    salesorder.id = item.ob_tbl_sales_order.id;
                    salesorder.ImagePath = abc.logo_path;
                    salesorder.amazon_order_id = item.ob_tbl_sales_order.amazon_order_id;
                    salesorder.BuyerName = item.ob_tbl_sales_order.buyer_name;
                    salesorder.purchase_date = item.ob_tbl_sales_order.purchase_date;
                    salesorder.PaymentMethodDetail = item.ob_tbl_sales_order.payment_method_detail;
                    salesorder.BillAmount = item.ob_tbl_sales_order.bill_amount;
                    salesorder.OrderStatus = item.ob_tbl_sales_order.order_status;


                    //salesorder.BuyerEmail = item.ob_tbl_sales_order.buyer_emil;                  
                    //salesorder.PaymentMethod = item.ob_tbl_sales_order.payment_method;                  
                    //salesorder.OrderType = item.ob_tbl_sales_order.order_type;
                    //salesorder.FullfillmentChannel = item.ob_tbl_sales_order.fullfillment_channel;                   
                    //salesorder.order_invoice_status = Convert.ToInt16(item.ob_tbl_sales_order.order_invoice_status);
                    //salesorder.DispatchAfterDate = Convert.ToDateTime(item.ob_tbl_sales_order.dispatch_afterdate);
                    //salesorder.DispatchByDate = Convert.ToDateTime(item.ob_tbl_sales_order.earliest_ship_date);
                    //salesorder.n_fullfilled_id = Convert.ToInt16(item.ob_tbl_sales_order.n_fullfilled_id);                 
                    //if (item.ob_tbl_sales_order.t_Hold != null)
                    //{
                    //    hold = item.ob_tbl_sales_order.t_Hold.ToLower();
                    //    if (hold == "false")
                    //    {
                    //        salesorder.Hold = "No";
                    //    }
                    //    else
                    //    {
                    //        salesorder.Hold = "Yes";
                    //    }
                    //}
                    //if (item.ob_tbl_customer_details != null)
                    //{
                    //    salesorder.ContactNO = item.ob_tbl_customer_details.contact_no;
                    //}

                    //if (item.ob_m_fullfilled != null)
                    //{
                    //    salesorder.FullfilledName = item.ob_m_fullfilled.Name;
                    //}

                    //if (item.ob_tbl_sales_order_status != null)
                    //{
                    //    salesorder.SalesOrderStatus = item.ob_tbl_sales_order_status.sales_order_status;
                    //}                 
                    lst_salesorder.Add(salesorder);
                }
            }
            catch(Exception ex)
            {
            }
            return new JsonResult { Data = lst_salesorder, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        /// <summary>
        /// get data for Sale order Details table on the basis of ID
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public JsonResult GetSaleMappingDetails(viewSalesOrder obj)
        {
            cf = new comman_function();
            bool ss = cf.session_check();
            if (obj != null)
            {
                int iId = obj.id;
                if (iId != 0)
                {
                    int SellerId = Convert.ToInt32(Session["SellerID"]);
                    var get_salesdetails = (from ob_tbl_sales_order_details in dba.tbl_sales_order_details

                                            join ob_tbl_sales_order_status in dba.tbl_sales_order_status on ob_tbl_sales_order_details.n_order_status_id
                                            equals ob_tbl_sales_order_status.id

                                            join ob_tbl_inventory in dba.tbl_inventory on ob_tbl_sales_order_details.sku_no.ToLower() equals (ob_tbl_inventory.sku.ToLower())
                                             into JoinedEmpDept4
                                            from proj4 in JoinedEmpDept4.DefaultIfEmpty()

                                            join ob_tbl_sales_order in dba.tbl_sales_order on ob_tbl_sales_order_details.tbl_sales_order_id equals (ob_tbl_sales_order.id)
                                            into JoinedEmpDept5
                                            from proj5 in JoinedEmpDept5.DefaultIfEmpty()
                                            select new SellerUtility
                                            {                                                
                                                ob_tbl_sales_order_details = ob_tbl_sales_order_details,
                                                ob_tbl_sales_order_status = ob_tbl_sales_order_status,
                                                ob_tbl_inventory = proj4,
                                                ob_tbl_sales_order = proj5
                                                // ob_tbl_inventory = dba.tbl_inventory.Where(a => a.sku.ToLower() == ob_tbl_sales_order_details.sku_no.ToLower()).FirstOrDefault()
                                            }).Where(a => a.ob_tbl_sales_order_details.is_active == 1 && a.ob_tbl_sales_order_details.tbl_seller_id == SellerId && a.ob_tbl_sales_order_details.tbl_sales_order_id == obj.id).OrderByDescending(a => a.ob_tbl_sales_order_details.id).ToList();
                    return new JsonResult { Data = get_salesdetails, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
                }
                else
                {
                    return new JsonResult { Data = null, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
                }
            }
            else
            {
                return new JsonResult { Data = null, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
            }
        }



        /// <summary>
        /// get  Customer details on the basic of sale order id 
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public JsonResult GetSaleCustomerDetails(viewSalesOrder obj)
        {
            cf = new comman_function();
            bool ss = cf.session_check();
            int SellerId = Convert.ToInt32(Session["SellerID"]);
            var get_salesdetails = (from ob_tbl_sales_order in dba.tbl_sales_order
                                    join ob_tbl_customer_details in dba.tbl_customer_details on ob_tbl_sales_order.tbl_Customer_Id
                                     equals ob_tbl_customer_details.id

                                    select new SellerUtility
                                    {
                                        ob_tbl_sales_order = ob_tbl_sales_order,
                                        ob_tbl_customer_details = ob_tbl_customer_details

                                    }).Where(a => a.ob_tbl_sales_order.is_active == 1 && a.ob_tbl_sales_order.tbl_sellers_id == SellerId && a.ob_tbl_sales_order.id == obj.id).OrderByDescending(a => a.ob_tbl_sales_order.id).ToList();

            return new JsonResult { Data = get_salesdetails, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }
        /// <summary>
        /// To Get all details of tbl_sales_status
        /// </summary>
        /// <returns></returns>
        public JsonResult FillSalesStatus()
        {
            cf = new comman_function();
            bool ss = cf.session_check();
            int SellerId = Convert.ToInt32(Session["SellerID"]);
            var GetStatusDetails = dba.tbl_sales_order_status.Where(a => a.is_active == 0).ToList();
            return new JsonResult { Data = GetStatusDetails, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }
        public JsonResult FillCourierName()
        {
            cf = new comman_function();
            bool ss = cf.session_check();
            int SellerId = Convert.ToInt32(Session["SellerID"]);
            var GetCourierDetails = dba.tbl_courier_comapny.Where(a => a.is_active == 0 && a.tbl_seller_id == SellerId).ToList();
            return new JsonResult { Data = GetCourierDetails, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        /// <summary>
        /// Update sales order status
        /// </summary>
        /// <param name="ob"></param>
        /// <returns></returns>
        /// 
        public JsonResult FillStatusorder(int? id)// this is for when we click on master id
        {
            cf = new comman_function();
            bool ss = cf.session_check();
            int SellerId = Convert.ToInt32(Session["SellerID"]);
            string Name = Convert.ToString(Session["UserName"]);

            var GetSalesOrder = (from ob_tbl_sales_order in dba.tbl_sales_order.Where(a => a.id == id && a.is_active == 1 && a.tbl_sellers_id == SellerId)
                                 select new
                                 {
                                     ob_tbl_sales_order = ob_tbl_sales_order,
                                 }).ToList();

            return new JsonResult { Data = GetSalesOrder, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        public JsonResult FillItemSerailNo(int? orderdetailid)
        {
            cf = new comman_function();
            bool ss = cf.session_check();
            int SellerId = Convert.ToInt32(Session["SellerID"]);
            var get_details = dba.tbl_sales_order_details.Where(a => a.id == orderdetailid && a.is_active == 1 && a.tbl_seller_id == SellerId).FirstOrDefault();
            var GetDetails = (from ob_tbl_inventory_details in dba.tbl_inventory_details.Where(a => a.item_uid == get_details.sku_no && a.tbl_sellers_id == SellerId)
                              select new
                              {
                                  ob_tbl_inventory_details = ob_tbl_inventory_details,
                              }).ToList();
            return new JsonResult { Data = GetDetails, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        public JsonResult FillStatusorder2(int? orderdetailid)// this is for when we click on order details popup
        {
            cf = new comman_function();
            bool ss = cf.session_check();
            int SellerId = Convert.ToInt32(Session["SellerID"]);

            List<SellerUtility> lst = new List<SellerUtility>();
            var GetSalesOrder2 = (from ob_tbl_sales_order_details in dba.tbl_sales_order_details.Where(a => a.id == orderdetailid && a.is_active == 1 && a.tbl_seller_id == SellerId)
                                  join ob_tbl_sales_order in dba.tbl_sales_order on ob_tbl_sales_order_details.tbl_sales_order_id
                                     equals ob_tbl_sales_order.id
                                  select new SellerUtility
                                  {
                                      ob_tbl_sales_order_details = ob_tbl_sales_order_details,
                                      ob_tbl_sales_order = ob_tbl_sales_order,
                                      ob_tbl_inventory = dba.tbl_inventory.Where(a => a.sku.ToLower() == ob_tbl_sales_order_details.sku_no.ToLower()).FirstOrDefault()
                                      //ob_tbl_inventory_details = dba.tbl_inventory_details.Where(a => a.tbl_inventory_id == ob_tbl_inventory.id)

                                  }).ToList();

            foreach (var item in GetSalesOrder2)
            {
                SellerUtility obSellerUtility = new Models.SellerUtility();
                obSellerUtility.ob_tbl_inventory = item.ob_tbl_inventory;
                if (item.ob_tbl_inventory != null)
                {
                    obSellerUtility.ob_tbl_inventory_details = dba.tbl_inventory_details.Where(a => a.tbl_inventory_id == item.ob_tbl_inventory.id).FirstOrDefault();
                }
                obSellerUtility.ob_tbl_sales_order_details = item.ob_tbl_sales_order_details;
                obSellerUtility.ob_tbl_sales_order = item.ob_tbl_sales_order;

                lst.Add(obSellerUtility);
            }
            return new JsonResult { Data = lst, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }
        [HttpPost]
        public JsonResult UpdateSalesOrderByStatus(tbl_order_delivery ob)
        {
            cf = new comman_function();
            bool ss = cf.session_check();
            string Message = null;
            string statename = "";
            string Customerstate = "";
            bool flag = false;
            int SellerId = Convert.ToInt32(Session["SellerID"]);
            try
            {
                ob.is_active = 0;
                ob.created_on = DateTime.Now;
                ob.tbl_seller_id = SellerId;
                dba.tbl_order_delivery.Add(ob);
                dba.SaveChanges();
                var get_details = dba.tbl_sales_order.Where(a => a.id == ob.tbl_sales_order_id).FirstOrDefault();
                var getsale_details = dba.tbl_sales_order_details.Where(a => a.tbl_sales_order_id == get_details.id).FirstOrDefault();
                var getsaledetails = dba.tbl_sales_order_details.Where(a => a.tbl_sales_order_id == get_details.id).ToList();
                var saledetails = dba.tbl_sales_order_details.Where(a => a.id == ob.tbl_sale_orderdetails_id).FirstOrDefault();

                string sales_skuno = getsale_details.sku_no;
                var inventorydetails = dba.tbl_inventory.Where(a => a.isactive == 1 && a.tbl_sellers_id == SellerId).ToList();

                var getsellerdetails = db.tbl_sellers.Where(a => a.id == get_details.tbl_sellers_id && a.isactive == 1).FirstOrDefault();//to get seller details from tbl seller in admin db.
                var getcustomerdetails = dba.tbl_customer_details.Where(a => a.tbl_Sales_OrderId == get_details.id).FirstOrDefault();// to get customer details from tbl customerdetails in seller admin db.

                var getcountrydetails = db.tbl_country.Where(a => a.status == 1 && a.countrylevel == 0 && a.id == getsellerdetails.country).FirstOrDefault();// to get country name from country table in admin db.
                var getstatedetails = db.tbl_country.Where(m => m.id == getsellerdetails.state && m.countrylevel == 1 && m.status == 1).FirstOrDefault();//to get state name from country table in admin db.

                if (getstatedetails != null)
                {
                    statename = getstatedetails.countryname;
                }
                if (getcustomerdetails != null)
                {
                    Customerstate = getcustomerdetails.State_Region;
                }
                int orderquantity = getsale_details.quantity_ordered;
                int marketplace_id = get_details.tbl_Marketplace_Id;
                var publish_details = dba.tbl_publish_item.Where(a => a.tbl_seller_id == SellerId && a.is_active == 1 && a.t_MarketPlace_id == marketplace_id).FirstOrDefault();
                if (saledetails != null)
                {
                    saledetails.n_order_status_id = ob.n_sale_order_status;
                    if (saledetails.n_order_status_id == 3)
                    {
                        saledetails.n_order_invoice_status = 1;
                    }
                    dba.Entry(saledetails).State = EntityState.Modified;
                    dba.SaveChanges();
                }
                if (get_details != null)
                {

                    var count = dba.tbl_sales_order_details.Where(a => a.tbl_sales_order_id == ob.tbl_sales_order_id && a.n_order_status_id != 3 && a.n_order_status_id != 2).Count();
                    if (count == 0)
                    {
                        //get_details.n_item_orderstatus = ob.n_sale_order_status;
                        get_details.n_item_orderstatus = 3;
                        if (get_details.n_item_orderstatus == 3)
                        {
                            get_details.order_invoice_status = 1;
                        }
                        dba.Entry(get_details).State = EntityState.Modified;
                        dba.SaveChanges();
                        flag = true;
                        Message = "Item Status is updated successfully";
                    }
                    else
                    {
                        get_details.n_item_orderstatus = 6;
                        dba.Entry(get_details).State = EntityState.Modified;
                        dba.SaveChanges();
                        flag = true;
                        Message = "Item Status is updated successfully";
                    }

                }//end of if(getdetails)                    
                else
                {
                    flag = false;
                    Message = "some error occurred !!";
                }
                if (ob.n_sale_order_status == 3)// check condition if status is shipped then execute otherwise no
                {
                    if (inventorydetails != null)
                    {
                        foreach (var item in inventorydetails)
                        {
                            if (sales_skuno == item.sku)// chk sku no 
                            {
                                item.item_count = item.item_count - orderquantity;
                                dba.Entry(item).State = EntityState.Modified;
                                dba.SaveChanges();

                                var categorydetails = dba.tbl_item_category.Where(a => a.tbl_sellers_id == SellerId && a.id == item.tbl_item_category_id).FirstOrDefault();

                                string sellerstate = statename.ToLower();
                                string customerstate = Customerstate.ToLower();
                                if (sellerstate == customerstate)
                                {
                                    double totaltax = categorydetails.tax_rate;
                                    double cgst = totaltax / 2;
                                    double sgst = totaltax - cgst;
                                }// end of if(sellerstate== customersate)
                                else
                                {
                                    double igst = categorydetails.tax_rate;

                                }// end if else(sellerstate == customerstate)                                
                            }
                        }// end of foreach(inventorydetails)
                    }//end of if(inventorydetails)

                    if (publish_details != null)
                    {
                        publish_details.t_current_item = publish_details.t_current_item - orderquantity;
                        dba.Entry(publish_details).State = EntityState.Modified;
                        dba.SaveChanges();
                    }// end of if(publish details)
                }// end of if(statusid)

            }

            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
                throw;
            }

            return new JsonResult { Data = new { Message = Message, Status = flag } };
        }


        [HttpPost]
        public JsonResult UpdateSalesDetailsOrderByStatus(tbl_order_delivery ob)
        {
            cf = new comman_function();
            bool ss = cf.session_check();
            string Message = null;
            string statename = "";
            string Customerstate = "";
            bool flag = false;
            int SellerId = Convert.ToInt32(Session["SellerID"]);

            try
            {
                //------------------------------- data save in a order- delivery table ---------------------//
                ob.is_active = 0;
                ob.created_on = DateTime.Now;
                ob.tbl_seller_id = SellerId;
                dba.tbl_order_delivery.Add(ob);
                dba.SaveChanges();

                //-------------------------------------End --------------------------------------------------//

                //------------------------- data save in aorder-history table --------------------//
                tbl_order_history objhistory = new tbl_order_history();
                objhistory.tbl_orders_id = ob.tbl_sales_order_id;
                objhistory.tbl_seller_id = SellerId;
                objhistory.t_order_status = ob.n_sale_order_status;
                objhistory.t_remarks = ob.t_Remarks;
                objhistory.updated_by = SellerId;
                objhistory.created_on = DateTime.Now;
                objhistory.tbl_orderDetails_Id = ob.tbl_sale_orderdetails_id;
                dba.tbl_order_history.Add(objhistory);
                dba.SaveChanges();

                //-----------------------------------End ---------------------------------------------//

                // -------------------------data save in a order Payment Details----------------------//

                tbl_order_payment_ledger objpayment = new tbl_order_payment_ledger();
                objpayment.created_date = DateTime.Now;
                objpayment.reconciled_on = DateTime.Now;
                objpayment.is_Reconciled = Convert.ToString(1);
                objpayment.tbl_order_id = ob.tbl_sales_order_id;
                objpayment.tbl_seller_id = SellerId;
                objpayment.t_Amount = Convert.ToDecimal(ob.t_shipping_price);
                objpayment.t_payment_type = Convert.ToString(1);
                objpayment.t_remarks = ob.t_Remarks;
                objpayment.tblorder_details_id = ob.tbl_sale_orderdetails_id;
                dba.tbl_order_payment_ledger.Add(objpayment);
                dba.SaveChanges();

                //---------------------------------------End------------------------------------------//

                var getsale_details = dba.tbl_sales_order_details.Where(a => a.id == ob.tbl_sale_orderdetails_id).FirstOrDefault();
                var get_details = dba.tbl_sales_order.Where(a => a.id == ob.tbl_sales_order_id).FirstOrDefault();
                string sales_skuno = getsale_details.sku_no;
                var inventorydetails = dba.tbl_inventory.Where(a => a.isactive == 1 && a.tbl_sellers_id == SellerId).ToList();

                var getsellerdetails = db.tbl_sellers.Where(a => a.id == get_details.tbl_sellers_id && a.isactive == 1).FirstOrDefault();//to get seller details from tbl seller in admin db.
                var getcustomerdetails = dba.tbl_customer_details.Where(a => a.id == get_details.tbl_Customer_Id).FirstOrDefault();// to get customer details from tbl customerdetails in seller admin db.

                var getcountrydetails = db.tbl_country.Where(a => a.status == 1 && a.countrylevel == 0 && a.id == getsellerdetails.country).FirstOrDefault();// to get country name from country table in admin db.
                var getstatedetails = db.tbl_country.Where(m => m.id == getsellerdetails.state && m.countrylevel == 1 && m.status == 1).FirstOrDefault();//to get state name from country table in admin db.

                if (getstatedetails != null)
                {
                    statename = getstatedetails.countryname;
                }
                if (getcustomerdetails != null)
                {
                    Customerstate = getcustomerdetails.State_Region;
                }
                int orderquantity = getsale_details.quantity_ordered;
                int marketplace_id = get_details.tbl_Marketplace_Id;
                var publish_details = dba.tbl_publish_item.Where(a => a.tbl_seller_id == SellerId && a.is_active == 1 && a.t_MarketPlace_id == marketplace_id).FirstOrDefault();
                if (getsale_details != null)
                {
                    getsale_details.n_order_status_id = ob.n_sale_order_status;
                    if (getsale_details.n_order_status_id == 3)
                    {
                        getsale_details.n_order_invoice_status = 1;
                    }
                    dba.Entry(getsale_details).State = EntityState.Modified;
                    dba.SaveChanges();
                }
                if (get_details != null)
                {

                    var count = dba.tbl_sales_order_details.Where(a => a.tbl_sales_order_id == ob.tbl_sales_order_id && a.n_order_status_id != 3 && a.n_order_status_id != 2).Count();
                    if (count == 0)
                    {
                        get_details.n_item_orderstatus = 3;
                        if (get_details.n_item_orderstatus == 3)
                        {
                            get_details.order_invoice_status = 1;
                        }
                        dba.Entry(get_details).State = EntityState.Modified;
                        dba.SaveChanges();
                        flag = true;
                        Message = "Item Status is updated successfully";
                    }
                    else
                    {
                        get_details.n_item_orderstatus = 6;
                        dba.Entry(get_details).State = EntityState.Modified;
                        dba.SaveChanges();
                        flag = true;
                        Message = "Item Status is updated successfully";
                    }

                }//end of if(getdetails)                    
                else
                {
                    flag = false;
                    Message = "some error occurred !!";
                }
                if (ob.n_sale_order_status == 3)// check condition if status is shipped then execute otherwise no
                {
                    if (inventorydetails != null)
                    {
                        foreach (var item in inventorydetails)
                        {
                            if (sales_skuno == item.sku)// chk sku no 
                            {
                                item.item_count = item.item_count - orderquantity;
                                dba.Entry(item).State = EntityState.Modified;
                                dba.SaveChanges();

                                var categorydetails = dba.tbl_item_category.Where(a => a.tbl_sellers_id == SellerId && a.id == item.tbl_item_category_id).FirstOrDefault();

                                string sellerstate = statename.ToLower();
                                string customerstate = Customerstate.ToLower();
                                if (sellerstate == customerstate)
                                {
                                    double totaltax = categorydetails.tax_rate;
                                    double cgst = totaltax / 2;
                                    double sgst = totaltax - cgst;
                                    if (getsale_details != null)
                                    {
                                        getsale_details.i_cgst = Convert.ToDouble(cgst);
                                        getsale_details.i_sgst = Convert.ToDouble(sgst);
                                        dba.Entry(getsale_details).State = EntityState.Modified;
                                        dba.SaveChanges();
                                    }


                                }// end of if(sellerstate== customersate)
                                else
                                {
                                    double igst = categorydetails.tax_rate;
                                    getsale_details.i_igst = Convert.ToDouble(igst);
                                    dba.Entry(getsale_details).State = EntityState.Modified;
                                    dba.SaveChanges();

                                }
                            }
                        }// end of foreach(inventorydetails)
                    }//end of if(inventorydetails)

                    if (publish_details != null)
                    {
                        publish_details.t_current_item = publish_details.t_current_item - orderquantity;
                        dba.Entry(publish_details).State = EntityState.Modified;
                        dba.SaveChanges();
                    }// end of if(publish details)
                }// end of if(statusid)

            }

            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
                throw;
            }

            return new JsonResult { Data = new { Message = Message, Status = flag } };
        }

        public JsonResult updateorderstatus(tbl_sales_order_details ob)
        {
            string Message = null;
            bool flag = false;
            try
            {
                var getdetails = dba.tbl_sales_order_details.Where(a => a.id == ob.id).FirstOrDefault();
                if (getdetails != null)
                {
                    getdetails.n_order_status_id = ob.n_order_status_id;
                    dba.Entry(getdetails).State = EntityState.Modified;
                    dba.SaveChanges();
                    flag = true;
                    Message = "Order Status is updated successfully";
                }
            }
            catch (Exception ex)
            {
                Debug.WriteLine(ex.Message);
                throw;
            }
            return new JsonResult { Data = new { Message = Message, Status = flag } };
        }

        /// <summary>
        /// this is for Print Sale Invoice
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        ///  PrintSaleOrder objsaleorder = null;
        ///  
        PrintSaleOrder objsaleorder = null;
        public PartialViewResult SaleInvoicePrint(int? id)
        {
            cf = new comman_function();
            bool ss = cf.session_check();
            //PrintSaleOrder objsaleorder = null;
            try
            {
                objsaleorder = new PrintSaleOrder();
                int SellerId = Convert.ToInt32(Session["SellerID"]);
                var getdetails = dba.tbl_sales_order.Where(a => a.id == id && a.tbl_sellers_id == SellerId).FirstOrDefault();
                var getsale_details = dba.tbl_sales_order_details.Where(a => a.tbl_sales_order_id == getdetails.id).FirstOrDefault();
                string sales_skuno = getsale_details.sku_no;
                objsaleorder.ddlsaleorderdetailList = dba.tbl_sales_order_details.Where(a => a.tbl_sales_order_id == getdetails.id).ToList();// to get sale order details from sale order detail table.

                var getsellerdetails = db.tbl_sellers.Where(a => a.id == getdetails.tbl_sellers_id && a.isactive == 1).FirstOrDefault();//to get seller details from tbl seller in admin db.
                var getcustomerdetails = dba.tbl_customer_details.Where(a => a.tbl_Sales_OrderId == getdetails.id).FirstOrDefault();// to get customer details from tbl customerdetails in seller admin db.
                var getcountrydetails = db.tbl_country.Where(a => a.status == 1 && a.countrylevel == 0 && a.id == getsellerdetails.country).FirstOrDefault();// to get country name from country table in admin db.
                var getstatedetails = db.tbl_country.Where(m => m.id == getsellerdetails.state && m.countrylevel == 1 && m.status == 1).FirstOrDefault();//to get state name from country table in admin db.

                var inventorydetails = dba.tbl_inventory.Where(a => a.isactive == 1 && a.tbl_sellers_id == SellerId).ToList(); // to get inventory details list


                if (getdetails != null)
                {
                    objsaleorder.id = getdetails.id;
                    objsaleorder.amazon_order_id = getdetails.amazon_order_id;
                    DateTime dat = Convert.ToDateTime(getdetails.purchase_date);
                    objsaleorder.purchase_date = dat.ToString("dd.MM.yyyy");
                    words(Convert.ToInt16(getdetails.bill_amount));
                    if (getcustomerdetails != null)
                    {
                        objsaleorder.Address_1 = getcustomerdetails.Address_1;
                        objsaleorder.Address_2 = getcustomerdetails.Address_2;
                        objsaleorder.shipping_Buyer_Name = getcustomerdetails.shipping_Buyer_Name;
                        objsaleorder.City = getcustomerdetails.City;
                        objsaleorder.Country_Code = getcustomerdetails.Country_Code;
                        objsaleorder.Postal_Code = getcustomerdetails.Postal_Code;
                        objsaleorder.State_Region = getcustomerdetails.State_Region;
                    }// end of if(getcustomerdetails)
                    if (getsellerdetails != null)
                    {
                        objsaleorder.address = getsellerdetails.address;
                        objsaleorder.business_name = getsellerdetails.business_name;
                        objsaleorder.city = getsellerdetails.city;
                        objsaleorder.gstin = getsellerdetails.gstin;
                        objsaleorder.pan = getsellerdetails.pan;
                        objsaleorder.pincode = getsellerdetails.pincode;
                    }// end of if(getsellerdetails)

                    if (getcountrydetails != null)
                    {
                        objsaleorder.countryname = getcountrydetails.countryname;
                    }// end of if(getcountrydetails)
                    if (getstatedetails != null)
                    {
                        objsaleorder.statename = getstatedetails.countryname;
                    }// end of if(getstatedetails)

                    //if (inventorydetails != null)
                    //{
                    //    foreach (var item in inventorydetails)
                    //    {
                    //        if (sales_skuno == item.sku)
                    //        {
                    //            var categorydetails = dba.tbl_item_category.Where(a => a.tbl_sellers_id == SellerId && a.id == item.tbl_item_category_id).FirstOrDefault();
                    //            double tax = categorydetails.tax_rate;
                    //           string sellerstate = objsaleorder.statename.ToLower();
                    //           string customerstate = objsaleorder.State_Region.ToLower();
                    //           if (sellerstate == customerstate)
                    //           {

                    //           }// end of if(sellerstate== customersate)
                    //           else
                    //           {
                    //           }// end if else(sellerstate == customerstate)

                    //        }// end of if(skuno)
                    //    }// end of for each(inventory details)
                    //}// end of if(inventory details)

                }// end of if(get details)

            }
            catch (Exception ex)
            { }
            return PartialView(objsaleorder);
        }

        public PartialViewResult SaleInvoicePrintForDetails(int? id)
        {
            //PrintSaleOrder objsaleorder = null;
            cf = new comman_function();
            bool ss = cf.session_check();
            try
            {
                objsaleorder = new PrintSaleOrder();
                int SellerId = Convert.ToInt32(Session["SellerID"]);

                var getsale_details = dba.tbl_sales_order_details.Where(a => a.id == id && a.tbl_seller_id == SellerId).FirstOrDefault();
                var getdetails = dba.tbl_sales_order.Where(a => a.id == getsale_details.tbl_sales_order_id && a.tbl_sellers_id == SellerId).FirstOrDefault();
                string sales_skuno = getsale_details.sku_no;
                objsaleorder.ddlsaleorderdetailList = dba.tbl_sales_order_details.Where(a => a.id == id).ToList();// to get sale order details from sale order detail table.

                var getsellerdetails = db.tbl_sellers.Where(a => a.id == getdetails.tbl_sellers_id && a.isactive == 1).FirstOrDefault();//to get seller details from tbl seller in admin db.
                var getcustomerdetails = dba.tbl_customer_details.Where(a => a.id == getdetails.tbl_Customer_Id).FirstOrDefault();// to get customer details from tbl customerdetails in seller admin db.
                var getcountrydetails = db.tbl_country.Where(a => a.status == 1 && a.countrylevel == 0 && a.id == getsellerdetails.country).FirstOrDefault();// to get country name from country table in admin db.
                var getstatedetails = db.tbl_country.Where(m => m.id == getsellerdetails.state && m.countrylevel == 1 && m.status == 1).FirstOrDefault();//to get state name from country table in admin db.

                var inventorydetails = dba.tbl_inventory.Where(a => a.isactive == 1 && a.tbl_sellers_id == SellerId).ToList(); // to get inventory details list


                if (getdetails != null)
                {
                    objsaleorder.id = getdetails.id;
                    objsaleorder.amazon_order_id = getdetails.amazon_order_id;
                    DateTime dat = Convert.ToDateTime(getdetails.purchase_date);
                    objsaleorder.purchase_date = dat.ToString("dd.MM.yyyy");
                    words(Convert.ToInt16(getdetails.bill_amount));
                    if (getcustomerdetails != null)
                    {
                        objsaleorder.Address_1 = getcustomerdetails.Address_1;
                        objsaleorder.Address_2 = getcustomerdetails.Address_2;
                        objsaleorder.shipping_Buyer_Name = getcustomerdetails.shipping_Buyer_Name;
                        objsaleorder.City = getcustomerdetails.City;
                        objsaleorder.Country_Code = getcustomerdetails.Country_Code;
                        objsaleorder.Postal_Code = getcustomerdetails.Postal_Code;
                        objsaleorder.State_Region = getcustomerdetails.State_Region;
                    }// end of if(getcustomerdetails)
                    if (getsellerdetails != null)
                    {
                        objsaleorder.address = getsellerdetails.address;
                        objsaleorder.business_name = getsellerdetails.business_name;
                        objsaleorder.city = getsellerdetails.city;
                        objsaleorder.gstin = getsellerdetails.gstin;
                        objsaleorder.pan = getsellerdetails.pan;
                        objsaleorder.pincode = getsellerdetails.pincode;
                    }// end of if(getsellerdetails)

                    if (getcountrydetails != null)
                    {
                        objsaleorder.countryname = getcountrydetails.countryname;
                    }// end of if(getcountrydetails)
                    if (getstatedetails != null)
                    {
                        objsaleorder.statename = getstatedetails.countryname;
                    }// end of if(getstatedetails)
                    if (getsale_details.i_cgst != null && getsale_details.i_sgst != null)
                    {
                        var unit = getsale_details.item_price_amount;
                        var tax = getsale_details.i_cgst + getsale_details.i_sgst;
                        objsaleorder.price = Convert.ToDouble((unit * tax / 100));
                        objsaleorder.taxprice = Convert.ToDouble((unit * tax / 100) / 2);
                        objsaleorder.Unitprice = Convert.ToDouble(unit - objsaleorder.price);

                    }
                    else
                    {
                        if (getsale_details.i_igst != null)
                        {
                            var unit = getsale_details.item_price_amount;
                            var tax = getsale_details.i_igst;
                            objsaleorder.price = Convert.ToDouble((unit * tax / 100));
                            objsaleorder.Unitprice = Convert.ToDouble(unit - objsaleorder.price);
                        }
                    }

                }// end of if(get details)

            }
            catch (Exception ex)
            { }
            return PartialView(objsaleorder);
        }

        public static List<OrderInvoiceprint> objorderinvoice = new List<OrderInvoiceprint>();
        /// <summary>
        /// OrderInvoicePrint is used for select checkbox 
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public PartialViewResult OrderInvoicePrint(int? id)
        {
            //objorderinvoice = new List<OrderInvoiceprint>();
            cf = new comman_function();
            bool ss = cf.session_check();
            List<PrintSaleOrder> objprintorder = null;
            try
            {
                objprintorder = new List<PrintSaleOrder>();
                if (id != null)
                {

                    objsaleorder = new PrintSaleOrder();
                    int SellerId = Convert.ToInt32(Session["SellerID"]);

                    var getsale_details = dba.tbl_sales_order_details.Where(a => a.id == id && a.tbl_seller_id == SellerId).FirstOrDefault();
                    var getdetails = dba.tbl_sales_order.Where(a => a.id == getsale_details.tbl_sales_order_id && a.tbl_sellers_id == SellerId).FirstOrDefault();
                    string sales_skuno = getsale_details.sku_no;
                    objsaleorder.ddlsaleorderdetailList = dba.tbl_sales_order_details.Where(a => a.id == id).ToList();// to get sale order details from sale order detail table.

                    var getsellerdetails = db.tbl_sellers.Where(a => a.id == getdetails.tbl_sellers_id && a.isactive == 1).FirstOrDefault();//to get seller details from tbl seller in admin db.
                    var getcustomerdetails = dba.tbl_customer_details.Where(a => a.id == getdetails.tbl_Customer_Id).FirstOrDefault();// to get customer details from tbl customerdetails in seller admin db.
                    var getcountrydetails = db.tbl_country.Where(a => a.status == 1 && a.countrylevel == 0 && a.id == getsellerdetails.country).FirstOrDefault();// to get country name from country table in admin db.
                    var getstatedetails = db.tbl_country.Where(m => m.id == getsellerdetails.state && m.countrylevel == 1 && m.status == 1).FirstOrDefault();//to get state name from country table in admin db.
                    var inventorydetails = dba.tbl_inventory.Where(a => a.isactive == 1 && a.tbl_sellers_id == SellerId).ToList(); // to get inventory details list


                    if (getdetails != null)
                    {
                        objsaleorder.id = getdetails.id;
                        objsaleorder.amazon_order_id = getdetails.amazon_order_id;
                        DateTime dat = Convert.ToDateTime(getdetails.purchase_date);
                        objsaleorder.purchase_date = dat.ToString("dd.MM.yyyy");
                        words(Convert.ToInt16(getdetails.bill_amount));
                        if (getcustomerdetails != null)
                        {
                            objsaleorder.Address_1 = getcustomerdetails.Address_1;
                            objsaleorder.Address_2 = getcustomerdetails.Address_2;
                            objsaleorder.shipping_Buyer_Name = getcustomerdetails.shipping_Buyer_Name;
                            objsaleorder.City = getcustomerdetails.City;
                            objsaleorder.Country_Code = getcustomerdetails.Country_Code;
                            objsaleorder.Postal_Code = getcustomerdetails.Postal_Code;
                            objsaleorder.State_Region = getcustomerdetails.State_Region;
                        }// end of if(getcustomerdetails)
                        if (getsellerdetails != null)
                        {
                            objsaleorder.address = getsellerdetails.address;
                            objsaleorder.business_name = getsellerdetails.business_name;
                            objsaleorder.city = getsellerdetails.city;
                            objsaleorder.gstin = getsellerdetails.gstin;
                            objsaleorder.pan = getsellerdetails.pan;
                            objsaleorder.pincode = getsellerdetails.pincode;
                        }// end of if(getsellerdetails)

                        if (getcountrydetails != null)
                        {
                            objsaleorder.countryname = getcountrydetails.countryname;
                        }// end of if(getcountrydetails)
                        if (getstatedetails != null)
                        {
                            objsaleorder.statename = getstatedetails.countryname;
                        }// end of if(getstatedetails)
                        if (getsale_details.i_cgst != null && getsale_details.i_sgst != null)
                        {
                            var unit = getsale_details.item_price_amount;
                            var tax = getsale_details.i_cgst + getsale_details.i_sgst;
                            objsaleorder.price = Convert.ToDouble((unit * tax / 100));
                            objsaleorder.taxprice = Convert.ToDouble((unit * tax / 100) / 2);
                            objsaleorder.Unitprice = Convert.ToDouble(unit - objsaleorder.price);

                        }
                        else
                        {
                            if (getsale_details.i_igst != null)
                            {
                                var unit = getsale_details.item_price_amount;
                                var tax = getsale_details.i_igst;
                                objsaleorder.price = Convert.ToDouble((unit * tax / 100));
                                objsaleorder.Unitprice = Convert.ToDouble(unit - objsaleorder.price);
                            }
                        }

                    }// end of if(get details)
                    objprintorder.Add(objsaleorder);
                } // end of if(id != null)

                objorderinvoice.Add(new OrderInvoiceprint
                {
                    PrintSaleOrder = objprintorder,
                    //  shipmentDetails = objshipment,
                });
            }
            catch (Exception ex)
            { }
            return PartialView(objorderinvoice);
        }

        public string words(int numbers)
        {
            int number = numbers;

            if (number == 0) return "Zero";
            if (number == -2147483648) return "Minus Two Hundred and Fourteen Crore Seventy Four Lakh Eighty Three Thousand Six Hundred and Forty Eight";
            int[] num = new int[4];
            int first = 0;
            int u, h, t;
            System.Text.StringBuilder sb = new System.Text.StringBuilder();
            if (number < 0)
            {
                sb.Append("Minus ");
                number = -number;
            }
            string[] words0 = {"" ,"One ", "Two ", "Three ", "Four ",
                            "Five " ,"Six ", "Seven ", "Eight ", "Nine "};
            string[] words1 = {"Ten ", "Eleven ", "Twelve ", "Thirteen ", "Fourteen ",
                            "Fifteen ","Sixteen ","Seventeen ","Eighteen ", "Nineteen "};
            string[] words2 = {"Twenty ", "Thirty ", "Forty ", "Fifty ", "Sixty ",
                            "Seventy ","Eighty ", "Ninety "};
            string[] words3 = { "Thousand ", "Lakh ", "Crore " };
            num[0] = number % 1000; // units
            num[1] = number / 1000;
            num[2] = number / 100000;
            num[1] = num[1] - 100 * num[2]; // thousands
            num[3] = number / 10000000; // crores
            num[2] = num[2] - 100 * num[3]; // lakhs
            for (int i = 3; i > 0; i--)
            {
                if (num[i] != 0)
                {
                    first = i;
                    break;
                }
            }
            for (int i = first; i >= 0; i--)
            {
                if (num[i] == 0) continue;
                u = num[i] % 10; // ones
                t = num[i] / 10;
                h = num[i] / 100; // hundreds
                t = t - 10 * h; // tens
                if (h > 0) sb.Append(words0[h] + "Hundred ");
                if (u > 0 || t > 0)
                {
                    if (h > 0 || i == 0) sb.Append("and ");
                    if (t == 0)
                        sb.Append(words0[u]);
                    else if (t == 1)
                        sb.Append(words1[u]);
                    else
                        sb.Append(words2[t - 2] + words0[u]);
                }
                if (i != 0) sb.Append(words3[i - 1]);
            }

            string a = sb.ToString().TrimEnd();
            objsaleorder.AmountWords = a + "Only";
            //objorder.AmountWords = a + "Only";
            //return sb.ToString().TrimEnd();
            return null;
        }


        public ActionResult TabDetails(int? id)
        {
            cf = new comman_function();
            if (!cf.session_check())
                return RedirectToAction("Login", "Home");
            Tab onjtab = new Tab();
            int SellerId = Convert.ToInt32(Session["SellerID"]);
            var getdetails = dba.tbl_sales_order.Where(a => a.id == id && a.tbl_sellers_id == SellerId).FirstOrDefault();
            if (getdetails != null)
            {
                onjtab.amazon_order_id = getdetails.amazon_order_id;
                onjtab.orderDate = String.Format("{0:D}", getdetails.purchase_date);
                onjtab.shipdate = String.Format("{0:D}", getdetails.Latest_ShipDate);
                onjtab.Time = getdetails.Latest_ShipDate.ToString("hh:mm tt");
                onjtab.orderstatus = getdetails.order_status;
                onjtab.ordertype = getdetails.order_type;
                onjtab.Paymentmethoddetails = getdetails.payment_method_detail;
                onjtab.billamount = Convert.ToString(getdetails.bill_amount);
                onjtab.buyeremail = getdetails.buyer_emil;
                onjtab.Order_status = getdetails.n_item_orderstatus;
                onjtab.Orderid = getdetails.id;

                //if (onjtab.Order_status != null)
                //{
                //    if (onjtab.Order_status == 1)
                //    {
                //    }
                //}

                var get_expense = dba.m_tbl_expense.Where(a => a.settlement_order_id == getdetails.amazon_order_id).ToList();
                if (get_expense != null)
                {
                    foreach (var item in get_expense)
                    {
                        var get_settlementfee = dba.m_settlement_fee.Where(a => a.id == item.expense_type_id).FirstOrDefault();
                        if (get_settlementfee != null)
                        {
                            string Name = get_settlementfee.return_fee;
                        }
                    }// end of foreach(item)
                }// end of if(get_expense)




            }
            return View(onjtab);
        }

        /// <summary>
        /// This is for Replacement Order by Amazon
        /// </summary>
        /// <returns></returns>
        /// 
        List<AmazonReplacementOrder> objjson = null;
        public ActionResult ReplacementOrder1()
        {
            objjson = new List<AmazonReplacementOrder>();
            List<ReplacementOrderDetails> objReplacementDetails = null;
            try
            {
                string text;
                string text1;
                var fileStream = new FileStream(@"c:\\amazon-replacment-report.txt", FileMode.Open, FileAccess.Read);
                using (var streamReader = new StreamReader(fileStream, Encoding.UTF8))
                {
                    text = streamReader.ReadToEnd();
                    string[] headers = text.Split(new[] { '\r', '\n' }, StringSplitOptions.RemoveEmptyEntries);

                    if (headers != null)
                    {
                        int kk = 0;
                        objReplacementDetails = new List<ReplacementOrderDetails>();
                        for (var i = 0; i < headers.Length; i++)
                        {
                            if (i == 0)
                                continue;

                            ReplacementOrderDetails obj_item = new ReplacementOrderDetails();
                            var get_data = headers[i].Split(new[] { '\t' }, StringSplitOptions.RemoveEmptyEntries);
                            if (get_data != null)
                            {

                                obj_item.ShipmentDate = get_data[0].ToString();
                                obj_item.SKU = get_data[1].ToString();
                                obj_item.ASIN = get_data[2].ToString();
                                obj_item.FullfillmentcenterId = get_data[3].ToString();
                                obj_item.OriginalFullfillmentcenterID = get_data[4].ToString();
                                obj_item.Quantity = get_data[5].ToString();
                                obj_item.ReplacementReasonCode = get_data[6].ToString();
                                obj_item.ReplacementAmazonOrderID = get_data[7].ToString();
                                obj_item.OrigialAmazonOrderID = get_data[8].ToString();

                            }// end of if(get_data)
                            objReplacementDetails.Add(obj_item);
                        }// end of forloop
                        objjson.Add(new AmazonReplacementOrder
                        {
                            ReplacementOrderDetails = objReplacementDetails,
                        });

                    }// end of if(headers)
                    else
                    {
                        int jjj = 0;
                        jjj++;
                    }
                    //savereplacementorder(objjson);
                }// end of using
            }// end of try block
            catch (Exception ex)
            {
            }
            return View(objjson);
        }
        /// <summary>
        /// save data in tbl history and update tbl_order table 
        /// </summary>
        /// <param name="objjson"></param>
        /// <returns></returns>
        //public string savereplacementorder(List<AmazonReplacementOrder> objjson)
        //{
        //    string datasaved = "";
        //    try
        //    {
        //        tbl_order_history objhistory = new tbl_order_history();
        //        var getdetails = dba.tbl_sales_order_status.Where(a => a.is_active == 0).ToList();
        //        string name = "Replacement".ToLower();
        //        int SellerId = Convert.ToInt32(Session["SellerID"]);
        //        if (objjson != null)
        //        {
        //            foreach (var item in objjson[0].ReplacementOrderDetails)
        //            {
        //                var getorderhistorydetails = dba.tbl_order_history.Where(a => a.OrderID == item.ReplacementAmazonOrderID).FirstOrDefault();
        //                if (getorderhistorydetails == null)
        //                {
        //                    objhistory.SKU = item.SKU;
        //                    objhistory.ASIN = item.ASIN;
        //                    objhistory.ShipmentDate = Convert.ToDateTime(item.ShipmentDate);
        //                    objhistory.ReplacementReasonCode = item.ReplacementReasonCode;
        //                    objhistory.OrderID = item.ReplacementAmazonOrderID;
        //                    objhistory.Quantity = Convert.ToInt16(item.Quantity);
        //                    objhistory.OrigialOrderID = item.OrigialAmazonOrderID;
        //                    objhistory.OriginalFullfillmentcenterID = item.OriginalFullfillmentcenterID;
        //                    objhistory.FullfillmentcenterId = item.FullfillmentcenterId;
        //                    objhistory.tbl_marketplace_id = 3;
        //                    foreach (var item1 in getdetails)
        //                    {
        //                        string name1 = item1.sales_order_status.ToLower();
        //                        if (name == name1)
        //                        {
        //                            objhistory.t_order_status = item1.id;
        //                            break;
        //                        }
        //                    }// end of foreach item
        //                    objhistory.tbl_seller_id = SellerId;
        //                    objhistory.updated_on = DateTime.Now;
        //                    objhistory.created_on = DateTime.Now;
        //                    dba.tbl_order_history.Add(objhistory);
        //                    dba.SaveChanges();
        //                    ////--------------------------------update tblsalesorder---------------------///
        //                    var getdata = dba.tbl_sales_order.Where(a => a.is_active == 1 && a.tbl_sellers_id == SellerId).ToList();
        //                    if (getdata != null)
        //                    {
        //                        foreach (var data in getdata)
        //                        {
        //                            string amazonid = objhistory.OrigialOrderID.ToLower();
        //                            string oriamazonid = data.amazon_order_id.ToLower();
        //                            if (amazonid == oriamazonid)
        //                            {
        //                                data.n_item_orderstatus = 7;
        //                                dba.Entry(data).State = EntityState.Modified;
        //                                dba.SaveChanges();
        //                                break;
        //                            }// end of if(amazonid == oriamazonid)
        //                        }// end of foreach
        //                    }// end of if(getdata)
        //                    /////////////////-----------------------END---------------------------------/////
        //                }
        //            }// end of foreach
        //        }// end of if(objjson)
        //        return datasaved;
        //    }
        //    catch (Exception ex)
        //    {
        //    }
        //    return "";
        //}

        /// <summary>
        /// Get All tbl history data
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public JsonResult GetOrderHistoryDetails(viewSalesOrder obj)
        {
            cf = new comman_function();
            bool ss = cf.session_check();
            int SellerId = Convert.ToInt32(Session["SellerID"]);
            List<SellerUtility> lst = new List<SellerUtility>();
            var get_historydetails = (from ob_tbl_order_history in dba.tbl_order_history
                                      join ob_tbl_sales_order_status in dba.tbl_sales_order_status on ob_tbl_order_history.t_order_status
                                      equals ob_tbl_sales_order_status.id
                                      select new SellerUtility
                                      {
                                          ob_tbl_order_history = ob_tbl_order_history,
                                          ob_tbl_sales_order_status = ob_tbl_sales_order_status
                                      }).Where(a => a.ob_tbl_order_history.tbl_seller_id == SellerId).OrderByDescending(a => a.ob_tbl_order_history.Id).ToList();
            foreach (var item in get_historydetails)
            {
                SellerUtility obSellerUtility = new Models.SellerUtility();
                var abc = db.m_marketplace.Where(a => a.isactive == 1 && a.id == item.ob_tbl_order_history.tbl_marketplace_id).FirstOrDefault();
                obSellerUtility.ob_tbl_order_history = item.ob_tbl_order_history;
                obSellerUtility.ob_tbl_sales_order_status = item.ob_tbl_sales_order_status;
                obSellerUtility.ob_m_marketplace = abc;
                lst.Add(obSellerUtility);
            }
            return new JsonResult { Data = lst, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        List<AmazonreconciliationOrder> objjson1 = null;
        public ActionResult ReconciliationOrder()
        {
            objjson1 = new List<AmazonreconciliationOrder>();
            List<reconciliationorder> objreconciliationorder = new List<reconciliationorder>();
            try
            {
                string text;
                string text1;
                Excel.Application xlApp;
                Excel.Workbook xlWorkBook;
                Excel.Worksheet xlWorkSheet;
                Excel.Range range;
                DataTable dt = new DataTable();
                string str;
                int rCnt;
                int cCnt;
                int rw = 0;
                int cl = 0;

                xlApp = new Excel.Application();
                xlWorkBook = xlApp.Workbooks.Open(@"c:\\Copy8317826449017487666.xls", 0, true, 5, "", "", true, Microsoft.Office.Interop.Excel.XlPlatform.xlWindows, "\t", false, false, 0, true, 1, 0);
                xlWorkSheet = (Excel.Worksheet)xlWorkBook.Worksheets.get_Item(1);

                range = xlWorkSheet.UsedRange;
                rw = range.Rows.Count;
                cl = range.Columns.Count;
                for (int i = 1; i <= rw; i++)
                {
                    DataRow dtrow = dt.NewRow();
                    for (int j = 1; j <= cl; j++)
                    {
                        int cell = j - 1;
                        var stsr = (range.Cells[i, j] as Excel.Range).Value2;
                        if (i == 1)
                            dt.Columns.Add(stsr);
                        else
                            dtrow[cell] = stsr;
                    }
                    if (i != 1)
                        dt.Rows.Add(dtrow);
                }
                if (dt.Rows.Count > 0)
                {
                    for (int k = 0; k < dt.Rows.Count; k++)
                    {
                        reconciliationorder obj_item = new reconciliationorder();
                        obj_item.settlement_id = dt.Rows[k][0].ToString();
                        obj_item.settlement_start_date = dt.Rows[k][1].ToString();
                        obj_item.settlement_end_date = dt.Rows[k][2].ToString();
                        obj_item.deposit_date = dt.Rows[k][3].ToString();
                        obj_item.total_amount = dt.Rows[k][4].ToString();
                        obj_item.currency = dt.Rows[k][5].ToString();
                        obj_item.transaction_type = dt.Rows[k][6].ToString();
                        obj_item.order_id = dt.Rows[k][7].ToString();
                        obj_item.merchant_order_id = dt.Rows[k][8].ToString();
                        obj_item.adjustment_id = dt.Rows[k][9].ToString();
                        obj_item.shipment_id = dt.Rows[k][10].ToString();
                        obj_item.marketplace_name = dt.Rows[k][11].ToString();
                        obj_item.amount_type = dt.Rows[k][12].ToString();
                        obj_item.amount_description = dt.Rows[k][13].ToString();
                        obj_item.amount = dt.Rows[k][14].ToString();
                        obj_item.fulfillment_id = dt.Rows[k][15].ToString();
                        obj_item.posted_date = dt.Rows[k][16].ToString();
                        obj_item.posted_date_time = dt.Rows[k][17].ToString();
                        obj_item.order_item_code = dt.Rows[k][18].ToString();
                        obj_item.merchant_order_item_id = dt.Rows[k][19].ToString();
                        obj_item.merchant_adjustment_item_id = dt.Rows[k][20].ToString();
                        obj_item.sku = dt.Rows[k][21].ToString();
                        obj_item.quantity_purchased = dt.Rows[k][22].ToString();
                        obj_item.promotion_id = dt.Rows[k][23].ToString();
                        objreconciliationorder.Add(obj_item);
                    }
                }
                objjson1.Add(new AmazonreconciliationOrder
                {
                    reconciliationorder = objreconciliationorder,
                });

                //Savesettlementdata(objjson1,0,3);// for save settlement data in expense table

                GC.Collect();
                GC.WaitForPendingFinalizers();
                Marshal.ReleaseComObject(range);
                Marshal.ReleaseComObject(xlWorkSheet);
                //close and release
                xlWorkBook.Close();
                Marshal.ReleaseComObject(xlWorkBook);
                //quit and release
                xlApp.Quit();
                Marshal.ReleaseComObject(xlApp);
            }
            catch (Exception ex)
            {
            }
            return View(objjson1);
        }

        public void handleOrder(string file_settlement_id, reconciliationorder obj)
        {
            int SellerId = Convert.ToInt32(Session["SellerID"]);
            try
            {
                string sku = "";
                if (obj.order_id == "403-9291094-5853145" || obj.order_id == "406-1128348-5744302" || obj.order_id == "405-8281746-7323528")
                {

                }
                if (obj.order_amount_typesDict != null)
                {
                    int orderSKUcounter = 0;
                    foreach (KeyValuePair<string, List<settlement_amt_type>> pair in obj.order_amount_typesDict)
                    {
                        //orderSKUcounter++;
                        sku = pair.Key;
                        List<settlement_amt_type> order_amount_typesDict = pair.Value;

                        List<List<settlement_amt_type>> superlist = new List<List<settlement_amt_type>>();

                        //foreach (settlement_amt_type item in order_amount_typesDict)
                        if (order_amount_typesDict.Count > 19)
                        {
                            for (int k = 0; k < order_amount_typesDict.Count; k++)
                            {
                                settlement_amt_type item = order_amount_typesDict[k];
                                if (item.description == "Principal")
                                {
                                    List<settlement_amt_type> a = new List<settlement_amt_type>();
                                    superlist.Add(a);
                                    a.Add(item);
                                    k++;
                                    item = order_amount_typesDict[k];
                                    while (item.description != "Principal")
                                    {
                                        a.Add(item);
                                        k++;

                                        if (k == order_amount_typesDict.Count)
                                            break;
                                        else
                                            item = order_amount_typesDict[k];
                                    }//end while

                                    if (k < order_amount_typesDict.Count) k--;
                                }
                            }//end for
                        }
                        else
                        {

                            List<settlement_amt_type> a = new List<settlement_amt_type>();
                            superlist.Add(a);
                            for (int k = 0; k < order_amount_typesDict.Count; k++)
                            {
                                settlement_amt_type item = order_amount_typesDict[k];
                                a.Add(item);
                            }
                        }
                        //loop thru all the items for a SKU
                        foreach (List<settlement_amt_type> myskulist in superlist)
                        {
                            tbl_settlement_order obj_settlement = null;
                            string unique_order_id = obj.order_id + "-" + orderSKUcounter;
                            orderSKUcounter++;
                            Dictionary<String, expense_tax_class> expenseId_Dict = new Dictionary<String, expense_tax_class>();

                            foreach (settlement_amt_type item in myskulist)
                            {
                                if (item.description == null || item.description == "")
                                    continue;

                                var Getsettlementorder = dba.tbl_settlement_order.Where(a => a.settlement_id == file_settlement_id && a.tbl_seller_id == SellerId && a.Sku_no.ToLower() == sku.ToLower() && a.unique_order_id == unique_order_id).FirstOrDefault();

                                if (Getsettlementorder == null)
                                {
                                    if (obj_settlement == null)
                                    {
                                        obj_settlement = new tbl_settlement_order();
                                        obj_settlement.Order_Id = obj.order_id;
                                        obj_settlement.unique_order_id = unique_order_id;
                                        obj_settlement.tbl_seller_id = SellerId;
                                        obj_settlement.created_on = DateTime.Now;
                                        obj_settlement.Sku_no = sku;
                                        obj_settlement.settlement_id = file_settlement_id;
                                        if (item.posteddatetime != null && item.posteddatetime != "")
                                        {
                                            string datetime = item.posteddatetime.Replace("UTC", "");
                                            string datetime2 = datetime.Replace(".", "-").TrimEnd();
                                            DateTime ddd = cf.MyDateTimeConverter(datetime2);
                                            //DateTime ddd = DateTime.ParseExact(datetime2, "dd-MM-yyyy HH:mm:ss", CultureInfo.InvariantCulture);
                                            obj_settlement.posted_date = ddd;
                                        }

                                        if (item.qty != null)
                                            obj_settlement.quantity = Convert.ToInt16(item.qty);
                                        dba.tbl_settlement_order.Add(obj_settlement);
                                        dba.SaveChanges();
                                    }
                                }
                                // #region Order check

                                if (item.description != null)
                                {
                                    if (item.description == "Principal")
                                    {
                                        obj_settlement.principal_price = item.amount;
                                        handleCredit((Decimal)item.amount);
                                    }
                                    else if (item.description == "Shipping")
                                    {
                                        obj_settlement.shipping_price = item.amount;
                                        handleCredit((Decimal)item.amount);
                                    }
                                    else if (item.description == "Gift wrap")
                                    {
                                        obj_settlement.giftwrap_price = item.amount;
                                        handleCredit((Decimal)item.amount);
                                    }
                                    else if (item.description == "Gift Wrap Tax")
                                    {
                                        obj_settlement.giftwarp_tax = item.amount;
                                        handleCredit((Decimal)item.amount);
                                    }
                                    else if (item.description == "Product Tax")
                                    {
                                        obj_settlement.product_tax = item.amount;
                                        handleCredit((Decimal)item.amount);
                                    }
                                    else if (item.description == "Shipping tax")
                                    {
                                        obj_settlement.shipping_tax = item.amount;
                                        handleCredit((Decimal)item.amount);
                                    }
                                    else if (item.description == "Shipping discount")
                                    {
                                        obj_settlement.shipping_discount = item.amount;
                                        handleCredit((Decimal)item.amount);
                                    }
                                    else if (item.description == "Shipping tax discount")
                                    {
                                        obj_settlement.shipping_tax_discount = item.amount;
                                        handleCredit((Decimal)item.amount);
                                    }
                                    else if (item.description == "SAFE-T Reimbursement")
                                    {
                                        obj_settlement.SAFE_T_Reimbursement = item.amount;
                                        handleCredit((Decimal)item.amount);
                                    }
                                    else if (item.description == "INCORRECT_FEES_ITEMS")
                                    {
                                        obj_settlement.INCORRECT_FEES_ITEMS = item.amount;
                                        handleCredit((Decimal)item.amount);
                                    }
                                    else
                                    {
                                        var Tbl_SettlementFees = dba.m_settlement_fee.ToList();
                                        var get_settlement_transactiontype = dba.m_settlement_transaction_type.ToList();
                                        var get_expensedata = Tbl_SettlementFees.Where(a => a.return_fee.ToLower() == item.description.ToLower()).FirstOrDefault();
                                        if (get_expensedata != null)
                                        {
                                            //sharad101
                                            if (Convert.ToDouble(item.amount) < 0)
                                                handleDebit((Decimal)Convert.ToDouble(item.amount) * (-1));
                                            else
                                            {
                                                handleCredit((Decimal)Convert.ToDouble(item.amount));
                                            }

                                            var id = get_expensedata.id;
                                            m_tbl_expense objexpense = new m_tbl_expense();
                                            objexpense.reference_number = file_settlement_id;
                                            objexpense.tbl_seller_id = SellerId;
                                            // objexpense.tbl_order_historyid = get_orderhistory.Id;
                                            objexpense.expense_type_id = id;
                                            objexpense.expense_amount = Convert.ToDouble(item.amount);
                                            objexpense.date_created = DateTime.Now;
                                            objexpense.settlement_order_id = unique_order_id;
                                            objexpense.sku_no = sku;
                                            objexpense.Original_order_id = obj.order_id;
                                            if (item.posteddatetime != null && item.posteddatetime != "")
                                            {
                                                string datetime = item.posteddatetime.Replace("UTC", "");
                                                string datetime2 = datetime.Replace(".", "-").TrimEnd();
                                                DateTime ddd = cf.MyDateTimeConverter(datetime2);
                                                //DateTime ddd = DateTime.ParseExact(datetime2, "dd-MM-yyyy HH:mm:ss", CultureInfo.InvariantCulture);
                                                objexpense.settlement_datetime = ddd;
                                            }
                                            //objexpense.promotion_id = item.promotion_id;
                                            if (item.qty != null)
                                                objexpense.quantity_purchased = Convert.ToInt16(item.qty);
                                            objexpense.t_transactionType_id = 1; //Order

                                            dba.m_tbl_expense.Add(objexpense);
                                            dba.SaveChanges();

                                            expense_tax_class taxobj = new expense_tax_class();
                                            taxobj.expense_db_id = objexpense.id;
                                            expenseId_Dict.Add(item.description, taxobj);
                                        }
                                        else
                                        {
                                            //TBD - Taxes
                                            if (item.description.Contains("CGST") || item.description.Contains("SGST") || item.description.Contains("IGST"))
                                            {
                                                //if (item.amount == 0)
                                                //    continue;

                                                string ss = item.description.Replace("CGST", "");
                                                ss = ss.Replace("SGST", "");
                                                ss = ss.Replace("IGST", "").Trim();
                                                if (expenseId_Dict.ContainsKey(ss))
                                                {
                                                    if (item.amount < 0)
                                                        handleDebit((Decimal)item.amount * (-1));
                                                    else
                                                        handleCredit((Decimal)item.amount);

                                                    expense_tax_class taxobj = expenseId_Dict[ss];
                                                    if (item.description.Contains("CGST"))
                                                    {
                                                        taxobj.cgst = item.amount;
                                                    }
                                                    else if (item.description.Contains("SGST"))
                                                    {
                                                        taxobj.sgst = item.amount;
                                                    }
                                                    else if (item.description.Contains("IGST"))
                                                    {
                                                        taxobj.igst = item.amount;
                                                    }
                                                }//end if
                                                else
                                                {
                                                    if (expenseId_Dict.Count > 0)
                                                    {
                                                        bool found = false;
                                                        foreach (KeyValuePair<string, expense_tax_class> pair1 in expenseId_Dict)
                                                        {
                                                            string k = pair1.Key;
                                                            k = Regex.Replace(k, @"\s+", "");
                                                            //k = k.Replace(' ', '');
                                                            if (k == ss)
                                                            {
                                                                found = true;
                                                                //total_debitAmt += (Decimal)item.amount;
                                                                if (item.amount < 0)
                                                                    handleDebit((Decimal)item.amount * (-1));
                                                                else
                                                                    handleDebit((Decimal)item.amount);

                                                                expense_tax_class taxobj = pair1.Value;
                                                                if (item.description.Contains("CGST"))
                                                                {
                                                                    taxobj.cgst = item.amount;
                                                                }
                                                                else if (item.description.Contains("SGST"))
                                                                {
                                                                    taxobj.sgst = item.amount;
                                                                }
                                                                else if (item.description.Contains("IGST"))
                                                                {
                                                                    taxobj.igst = item.amount;
                                                                }

                                                                break;
                                                            }


                                                        }//end for loop
                                                        if (found == false)
                                                        {
                                                            //goes in suspense account
                                                        }
                                                    }//end if

                                                    //key not found
                                                    int aaaa = 0;
                                                }//end else
                                            }
                                            else
                                            {
                                                int aaaa = 0;
                                                aaaa++;
                                                //head not found -goes in suspense
                                            }

                                        }//end else taxes
                                    }//end else
                                }
                                //}
                            }//end inner foreach - looping for one record
                            if (obj_settlement != null)
                            {
                                dba.Entry(obj_settlement).State = EntityState.Modified;
                                dba.SaveChanges();

                                foreach (KeyValuePair<string, expense_tax_class> pair1 in expenseId_Dict)
                                {
                                    expense_tax_class taxobj = pair1.Value;
                                    tbl_tax obj1 = new tbl_tax();
                                    obj1.reference_type = 2; //settlement_order
                                    obj1.tbl_referneced_id = taxobj.expense_db_id;
                                    obj1.isactive = 1;
                                    obj1.tbl_seller_id = SellerId;
                                    obj1.sgst_amount = Convert.ToDouble(taxobj.sgst);
                                    obj1.Igst_amount = Convert.ToDouble(taxobj.igst);
                                    obj1.CGST_amount = Convert.ToDouble(taxobj.cgst);
                                    //obj1.giftwarp_tax = Convert.ToDouble(giftraptax);
                                    //objtax.tbl_history_id = get_orderhistory.Id;
                                    dba.tbl_tax.Add(obj1);
                                    dba.SaveChanges();
                                }
                            }
                        }//end foreach
                    }
                }
                if (obj.refund_amount_typesDict != null)
                {
                    handleRefund(file_settlement_id, obj);
                }
                if (obj.easyship_amount_typesDict != null)
                {
                    int cnt = 0;
                    foreach (KeyValuePair<string, List<settlement_amt_type>> pair in obj.easyship_amount_typesDict)
                    {
                        string shipment_id = pair.Key;
                        List<settlement_amt_type> easyship_amount_typesDict = pair.Value;

                        handleEasyShip(sku, cnt, obj.order_id, file_settlement_id, easyship_amount_typesDict);
                        cnt++;

                    }//end outer for loop
                }//end easy ship handling
            }
            catch (Exception e)
            {
                int a = 0;
            }

        }//end function

        public void handleEasyShip(string sku, int index, string orderid, string file_settlement_id, List<settlement_amt_type> easyship_amount_typesDict)
        {
            int SellerId = Convert.ToInt32(Session["SellerID"]);
            string unique_order_id = orderid + "-" + index;
            expense_tax_class taxobj = null;
            foreach (settlement_amt_type item in easyship_amount_typesDict)
            {
                var Tbl_SettlementFees = dba.m_settlement_fee.ToList();
                var get_settlement_transactiontype = dba.m_settlement_transaction_type.ToList();
                var get_expensedata = Tbl_SettlementFees.Where(a => a.return_fee.ToLower() == item.description.ToLower()).FirstOrDefault();
                if (get_expensedata != null)
                {
                    var id = get_expensedata.id;
                    m_tbl_expense objexpense = new m_tbl_expense();
                    objexpense.reference_number = file_settlement_id;
                    objexpense.tbl_seller_id = SellerId;
                    // objexpense.tbl_order_historyid = get_orderhistory.Id;
                    objexpense.expense_type_id = id;
                    objexpense.expense_amount = Convert.ToDouble(item.amount);

                    if (item.amount < 0)
                        handleDebit((Decimal)item.amount * (-1));
                    else
                        handleCredit((Decimal)item.amount);

                    objexpense.date_created = DateTime.Now;
                    objexpense.settlement_order_id = unique_order_id;
                    objexpense.sku_no = sku;
                    objexpense.Original_order_id = orderid;
                    if (item.posteddatetime != null && item.posteddatetime != "")
                    {
                        string datetime = item.posteddatetime.Replace("UTC", "");
                        string datetime2 = datetime.Replace(".", "-").TrimEnd();
                        DateTime ddd = cf.MyDateTimeConverter(datetime2);
                        objexpense.settlement_datetime = ddd;
                    }
                    //objexpense.promotion_id = item.promotion_id;

                    if (item.qty != null)
                        objexpense.quantity_purchased = Convert.ToInt16(item.qty);
                    objexpense.t_transactionType_id = 1; //Order

                    dba.m_tbl_expense.Add(objexpense);
                    dba.SaveChanges();

                    taxobj = new expense_tax_class();
                    taxobj.expense_db_id = objexpense.id;
                }
                else
                {
                    //TBD - Taxes
                    if (item.description.Contains("CGST") || item.description.Contains("SGST") || item.description.Contains("IGST"))
                    {
                        if (item.description.Contains("CGST"))
                        {
                            taxobj.cgst = item.amount;
                        }
                        else if (item.description.Contains("SGST"))
                        {
                            taxobj.sgst = item.amount;
                        }
                        else if (item.description.Contains("IGST"))
                        {
                            taxobj.igst = item.amount;
                        }

                        if (item.amount < 0)
                            handleDebit((Decimal)item.amount * (-1));
                        else
                            handleCredit((Decimal)item.amount);

                        tbl_tax obj1 = new tbl_tax();
                        obj1.reference_type = 2; //settlement_order
                        obj1.tbl_referneced_id = taxobj.expense_db_id;
                        obj1.isactive = 1;
                        obj1.tbl_seller_id = SellerId;
                        obj1.sgst_amount = Convert.ToDouble(taxobj.sgst);
                        obj1.Igst_amount = Convert.ToDouble(taxobj.igst);
                        obj1.CGST_amount = Convert.ToDouble(taxobj.cgst);
                        dba.tbl_tax.Add(obj1);
                        dba.SaveChanges();
                    }
                    else
                    {
                        int aaaa = 0;
                        aaaa++;
                    }

                }//end else taxes

            }//end for loop

        }


        public void handleRefund(string file_settlement_id, reconciliationorder obj)
        {
            int SellerId = Convert.ToInt32(Session["SellerID"]);
            try
            {
                int orderSKUcounter = 0;
                foreach (KeyValuePair<string, List<settlement_amt_type>> pair in obj.refund_amount_typesDict)
                {
                    //orderSKUcounter++;
                    string sku = pair.Key;
                    List<settlement_amt_type> refund_amount_typesDict = pair.Value;

                    List<List<settlement_amt_type>> superlist = new List<List<settlement_amt_type>>();

                    //foreach (settlement_amt_type item in order_amount_typesDict)
                    //assuming that if theer are less than 19 values then there is only one order with one oreder item
                    if (refund_amount_typesDict.Count > 19)
                    {
                        for (int k = 0; k < refund_amount_typesDict.Count; k++)
                        {
                            settlement_amt_type item = refund_amount_typesDict[k];
                            if (item.description == "Principal")
                            {
                                List<settlement_amt_type> a = new List<settlement_amt_type>();
                                superlist.Add(a);
                                a.Add(item);
                                k++;
                                item = refund_amount_typesDict[k];
                                while (item.description != "Principal")
                                {
                                    a.Add(item);
                                    k++;

                                    if (k == refund_amount_typesDict.Count)
                                        break;
                                    else
                                        item = refund_amount_typesDict[k];
                                }//end while

                                if (k < refund_amount_typesDict.Count) k--;
                            }
                        }//end for
                    }
                    else
                    {
                        List<settlement_amt_type> a = new List<settlement_amt_type>();
                        superlist.Add(a);
                        for (int k = 0; k < refund_amount_typesDict.Count; k++)
                        {
                            settlement_amt_type item = refund_amount_typesDict[k];

                            a.Add(item);

                        }
                    }

                    //loop thru all the items for a SKU
                    foreach (List<settlement_amt_type> myskulist in superlist)
                    {
                        tbl_order_history obj_Refund = null;
                        string unique_order_id = obj.order_id + "-" + orderSKUcounter;
                        orderSKUcounter++;
                        Dictionary<String, expense_tax_class> expenseId_Dict = new Dictionary<String, expense_tax_class>();
                        if (obj.order_id == "405-6852114-2146724")
                        {

                        }

                        foreach (settlement_amt_type item in myskulist)
                        {
                            var GetRefundOrderRecord = dba.tbl_order_history.Where(a => a.settlement_id == file_settlement_id && a.tbl_seller_id == SellerId && a.unique_order_id == unique_order_id).FirstOrDefault();

                            if (GetRefundOrderRecord == null)
                            {
                                if (obj_Refund == null)
                                {
                                    obj_Refund = new tbl_order_history();
                                    obj_Refund.tbl_marketplace_id = m_marketplaceID;
                                    obj_Refund.OrderID = obj.order_id;
                                    obj_Refund.unique_order_id = unique_order_id;
                                    obj_Refund.tbl_seller_id = SellerId;
                                    obj_Refund.created_on = DateTime.Now;
                                    obj_Refund.SKU = sku;
                                    obj_Refund.t_order_status = 9; //refund
                                    obj_Refund.settlement_id = file_settlement_id;
                                    obj_Refund.fulfillment_id = obj.fulfillment_id;
                                    if (item.posteddatetime != null && item.posteddatetime != "")
                                    {
                                        string datetime = item.posteddatetime.Replace("UTC", "");
                                        string datetime2 = datetime.Replace(".", "-").TrimEnd();
                                        //DateTime ddd = DateTime.ParseExact(datetime2, "dd-MM-yyyy HH:mm:ss", CultureInfo.InvariantCulture);
                                        DateTime ddd = cf.MyDateTimeConverter(datetime2);
                                        obj_Refund.ShipmentDate = ddd;
                                    }

                                    if (item.qty != null)
                                        obj_Refund.Quantity = Convert.ToInt16(item.qty);
                                    dba.tbl_order_history.Add(obj_Refund);
                                    dba.SaveChanges();
                                }
                            }
                            // #region Order check

                            if (item.description != null)
                            {
                                if (item.description == "Principal")
                                {
                                    obj_Refund.amount_per_unit = item.amount;
                                    if (item.amount < 0)
                                        total_debitAmt += (Decimal)item.amount * (-1);
                                    else
                                        total_debitAmt += (Decimal)item.amount;
                                }
                                else if (item.description == "Shipping")
                                {
                                    obj_Refund.shipping_price = item.amount;
                                    if (item.amount < 0)
                                        total_debitAmt += (Decimal)item.amount * (-1);
                                    else
                                        total_debitAmt += (Decimal)item.amount;
                                }
                                else if (item.description == "Gift wrap")
                                {
                                    obj_Refund.Giftwrap_price = item.amount;
                                    if (item.amount < 0)
                                        total_debitAmt += (Decimal)item.amount * (-1);
                                    else
                                        total_debitAmt += (Decimal)item.amount;
                                }
                                else if (item.description == "Gift Wrap Tax")
                                {
                                    obj_Refund.gift_wrap_tax = item.amount;
                                    if (item.amount < 0)
                                        total_debitAmt += (Decimal)item.amount * (-1);
                                    else
                                        total_debitAmt += (Decimal)item.amount;
                                }
                                else if (item.description == "Product Tax")
                                {
                                    obj_Refund.product_tax = item.amount;
                                    if (item.amount < 0)
                                        total_debitAmt += (Decimal)item.amount * (-1);
                                    else
                                        total_debitAmt += (Decimal)item.amount;
                                }
                                else if (item.description == "Shipping tax")
                                {
                                    obj_Refund.shipping_tax = item.amount;
                                    if (item.amount < 0)
                                        total_debitAmt += (Decimal)item.amount * (-1);
                                    else
                                        total_debitAmt += (Decimal)item.amount;
                                }
                                else if (item.description == "Shipping discount")
                                {
                                    obj_Refund.shipping_discount = item.amount;
                                    if (item.amount < 0)
                                        total_debitAmt += (Decimal)item.amount * (-1);
                                    else
                                        total_debitAmt += (Decimal)item.amount;
                                }
                                else if (item.description == "Shipping tax discount")
                                {
                                    obj_Refund.shipping_tax_discount = item.amount;
                                    if (item.amount < 0)
                                        total_debitAmt += (Decimal)item.amount * (-1);
                                    else
                                        total_debitAmt += (Decimal)item.amount;
                                }
                                else
                                {
                                    var Tbl_SettlementFees = dba.m_settlement_fee.ToList();
                                    var get_settlement_transactiontype = dba.m_settlement_transaction_type.ToList();
                                    var get_expensedata = Tbl_SettlementFees.Where(a => a.return_fee.ToLower() == item.description.ToLower()).FirstOrDefault();
                                    if (get_expensedata != null)
                                    {
                                        var id = get_expensedata.id;
                                        m_tbl_expense objexpense = new m_tbl_expense();
                                        objexpense.reference_number = file_settlement_id;
                                        objexpense.tbl_seller_id = SellerId;
                                        // objexpense.tbl_order_historyid = get_orderhistory.Id;
                                        objexpense.expense_type_id = id;
                                        objexpense.expense_amount = Convert.ToDouble(item.amount);

                                        if (objexpense.expense_amount < 0)
                                            total_debitAmt += (Decimal)Convert.ToDouble(item.amount) * (-1);
                                        else
                                            total_creditAmt += (Decimal)Convert.ToDouble(item.amount);

                                        objexpense.date_created = DateTime.Now;
                                        objexpense.settlement_order_id = unique_order_id;
                                        objexpense.tbl_order_historyid = obj_Refund.Id;
                                        objexpense.sku_no = sku;
                                        objexpense.Original_order_id = obj.order_id;
                                        if (item.posteddatetime != null && item.posteddatetime != "")
                                        {
                                            string datetime = item.posteddatetime.Replace("UTC", "");
                                            string datetime2 = datetime.Replace(".", "-").TrimEnd();
                                            DateTime ddd = cf.MyDateTimeConverter(datetime2);
                                            objexpense.settlement_datetime = ddd;
                                        }
                                        //objexpense.promotion_id = item.promotion_id;

                                        if (item.qty != null)
                                            objexpense.quantity_purchased = Convert.ToInt16(item.qty);
                                        objexpense.t_transactionType_id = 2; //Refund

                                        dba.m_tbl_expense.Add(objexpense);
                                        dba.SaveChanges();

                                        expense_tax_class taxobj = new expense_tax_class();
                                        taxobj.expense_db_id = objexpense.id;
                                        expenseId_Dict.Add(item.description, taxobj);
                                    }
                                    else
                                    {
                                        //TBD - Taxes
                                        if (item.description.Contains("CGST") || item.description.Contains("SGST") || item.description.Contains("IGST"))
                                        {
                                            string ss = item.description.Replace("CGST", "");
                                            ss = ss.Replace("SGST", "");
                                            ss = ss.Replace("IGST", "").Trim();
                                            if (expenseId_Dict.ContainsKey(ss))
                                            {
                                                expense_tax_class taxobj = expenseId_Dict[ss];
                                                if (item.description.Contains("CGST"))
                                                {
                                                    taxobj.cgst = item.amount;
                                                    total_creditAmt += (Decimal)item.amount;
                                                }
                                                else if (item.description.Contains("SGST"))
                                                {
                                                    taxobj.sgst = item.amount;
                                                    total_creditAmt += (Decimal)item.amount;
                                                }
                                                else if (item.description.Contains("IGST"))
                                                {
                                                    taxobj.igst = item.amount;
                                                    total_creditAmt += (Decimal)item.amount;
                                                }
                                            }//end if
                                        }

                                    }//end else taxes
                                }//end else
                            }
                            //}
                        }//end inner foreach - looping for one record
                        if (obj_Refund != null)
                        {
                            dba.Entry(obj_Refund).State = EntityState.Modified;
                            dba.SaveChanges();

                            foreach (KeyValuePair<string, expense_tax_class> pair1 in expenseId_Dict)
                            {
                                expense_tax_class taxobj = pair1.Value;
                                tbl_tax obj1 = new tbl_tax();
                                obj1.reference_type = 7; //Order_history
                                obj1.tbl_referneced_id = taxobj.expense_db_id;
                                obj1.isactive = 1;
                                obj1.tbl_seller_id = SellerId;
                                obj1.sgst_amount = Convert.ToDouble(taxobj.sgst);
                                obj1.Igst_amount = Convert.ToDouble(taxobj.igst);
                                obj1.CGST_amount = Convert.ToDouble(taxobj.cgst);
                                //obj1.giftwarp_tax = Convert.ToDouble(giftraptax);
                                //objtax.tbl_history_id = get_orderhistory.Id;
                                dba.tbl_tax.Add(obj1);
                                dba.SaveChanges();
                            }
                        }
                    }//end foreach
                }
            }
            catch (Exception e)
            {
                int a = 0;
            }
        }

        int m_marketplaceID;
        int SellerId;
        Decimal total_debitAmt = 0;
        Decimal total_creditAmt = 0;
        Decimal suspense_Amount = 0;
        public string suspense_data = "";

        List<Decimal> list_debit, list_credit;

        private void handleDebit(Decimal amt)
        {
            if (list_debit == null)
                list_debit = new List<Decimal>();

            list_debit.Add(amt);
            total_debitAmt += amt;
        }

        private void handleCredit(Decimal amt)
        {
            if (list_credit == null)
                list_credit = new List<Decimal>();

            list_credit.Add(amt);
            total_creditAmt += amt;
        }

        public string Savesettlementdata(List<AmazonreconciliationOrder> objjson1, int settlementuploadtype, int marketplaceID, string strFileName, short sourcetype)
        {
            total_debitAmt = 0;
            total_creditAmt = 0;
            suspense_Amount = 0;
            list_debit = new List<Decimal>();
            list_credit = new List<Decimal>();
            m_marketplaceID = marketplaceID;

            cf = new comman_function();
            bool ss = cf.session_check();
            string datasaved = "S";
            cf = new comman_function();

            tbl_tax storageFee_tax = null;
            try
            {
                int SellerId = Convert.ToInt32(Session["SellerID"]);
                int current_running_no = 0;
                string Reference_No = "";
                if (objjson1 != null)
                {
                    foreach (var ref_no in objjson1[0].reconciliationorder)
                    {
                        Reference_No = ref_no.settlement_id;
                        break;
                    }
                    var get_upload = dba.tbl_settlement_upload.Where(a => a.tbl_seller_id == SellerId && a.settlement_refernece_no == Reference_No).FirstOrDefault();
                    if (get_upload == null)
                    {


                        var get_seller_setting = dba.tbl_seller_setting.Where(a => a.tbl_seller_id == SellerId && a.type == 2).FirstOrDefault();
                        if (get_seller_setting != null)
                        {
                            current_running_no = Convert.ToInt16(get_seller_setting.current_running_no);
                        }
                        string type1 = "";
                        string last_readtype;
                        int settelmentupload_id = 0;
                        string file_settlement_id = "";
                        tbl_settlement_upload objuplaod = null;

                        int totCounter = 0;

                        int uniqueupload = 0;
                        foreach (var item in objjson1[0].reconciliationorder)
                        {
                            totCounter++;
                            last_readtype = type1;
                            type1 = "";
                            if (item.transaction_type != null)
                                type1 = item.transaction_type.ToLower();

                            if (item.order_id == "408-8793722-2959511")
                            {

                            }

                            if (item.transaction_type == "SAFE-T Reimbursement" || item.transaction_type == "Cancellation" || (item.transaction_type == "other-transaction" && item.order_id != null))
                            {
                                handleOrder(file_settlement_id, item);
                            }
                            else if (item.transaction_type == "Order")
                            {

                                handleOrder(file_settlement_id, item);
                            }
                            else if (item.transaction_type == "Refund")
                            {
                                handleRefund(file_settlement_id, item);
                            }
                            else if (item.deposit_date != "" && item.settlement_start_date != "" && item.settlement_end_date != "")
                            {
                                objuplaod = new tbl_settlement_upload();

                                objuplaod.market_place_id = marketplaceID;
                                objuplaod.tbl_seller_id = SellerId;
                                objuplaod.uploaded_on = DateTime.Now;
                                objuplaod.uploaded_by = SellerId;
                                objuplaod.file_name = strFileName;
                                objuplaod.Source = sourcetype;
                                objuplaod.voucher_running_no = current_running_no;
                                DateTime ddd2;
                                if (item.settlement_start_date != "" && item.settlement_start_date != null)
                                {
                                    string strT = item.settlement_start_date;
                                    string datetime3 = item.settlement_start_date.Replace("UTC", "");
                                    string datetime4 = datetime3.Replace(".", "-").TrimEnd();
                                    //DateTime ddd2;
                                    ddd2 = cf.MyDateTimeConverter(datetime4);

                                    objuplaod.settlement_from = ddd2;
                                }

                                DateTime ddd1;
                                if (item.settlement_end_date != "" && item.settlement_end_date != null)
                                {
                                    string datetime5 = item.settlement_end_date.Replace("UTC", "");
                                    string datetime6 = datetime5.Replace(".", "-").TrimEnd();
                                    ddd1 = cf.MyDateTimeConverter(datetime6);
                                    //DateTime ddd1 = DateTime.ParseExact(datetime6, "dd-MM-yyyy HH:mm:ss", CultureInfo.InvariantCulture);
                                    objuplaod.settlement_to = ddd1;
                                }

                                DateTime ddd3;
                                if (item.deposit_date != "" && item.deposit_date != null)
                                {
                                    string datetime7 = item.deposit_date.Replace("UTC", "");
                                    string datetime8 = datetime7.Replace(".", "-").TrimEnd();
                                    ddd3 = cf.MyDateTimeConverter(datetime8);
                                    //DateTime ddd3 = DateTime.ParseExact(datetime8, "dd-MM-yyyy HH:mm:ss", CultureInfo.InvariantCulture);
                                    objuplaod.deposit_date = ddd3;
                                }
                                file_settlement_id = item.settlement_id;
                                objuplaod.settlement_refernece_no = item.settlement_id;
                                objuplaod.settlement_type = Convert.ToInt16(settlementuploadtype);
                                objuplaod.status = 0;

                                if (item.otherTransatanctionList != null)
                                {
                                    foreach (var otherT in item.otherTransatanctionList)
                                    {
                                        if (otherT.description == "Previous Reserve Amount Balance")
                                        {
                                            objuplaod.previous_reserve_amount = Convert.ToDecimal(otherT.amount);
                                            total_creditAmt += (Decimal)objuplaod.previous_reserve_amount;
                                        }
                                        else if (otherT.description == "NonSubscriptionFeeAdj")
                                        {
                                            objuplaod.Nonsubscription_feeadj += Convert.ToDecimal(otherT.amount);
                                            if (Convert.ToDecimal(otherT.amount) < 0)
                                            {
                                                handleDebit(Convert.ToDecimal(otherT.amount) * (-1));
                                                //total_debitAmt += ;
                                            }
                                            else
                                            {
                                                handleCredit(Convert.ToDecimal(otherT.amount));
                                                //total_creditAmt += Convert.ToDecimal(otherT.amount);
                                            }

                                        }
                                        else if (otherT.description == "Current Reserve Amount")
                                        {
                                            objuplaod.current_reserve_amount = Convert.ToDecimal(otherT.amount);
                                            if (objuplaod.current_reserve_amount < 0)
                                            {
                                                handleDebit((Decimal)objuplaod.current_reserve_amount * (-1));
                                                //total_debitAmt += (Decimal)objuplaod.current_reserve_amount * (-1);
                                            }
                                            else
                                            {
                                                handleCredit((Decimal)objuplaod.current_reserve_amount);
                                                //total_creditAmt += (Decimal)objuplaod.current_reserve_amount;
                                            }
                                        } //sharad
                                        else if (otherT.description == "INCORRECT_FEES_ITEMS")
                                        {
                                            if (objuplaod.INCORRECT_FEES_ITEMS == null)
                                                objuplaod.INCORRECT_FEES_ITEMS = 0;

                                            objuplaod.INCORRECT_FEES_ITEMS += Convert.ToDecimal(otherT.amount);

                                            handleCredit(Convert.ToDecimal(otherT.amount));

                                        }
                                        else if (otherT.description == "TransactionTotalAmount")
                                        {
                                            if (objuplaod.Cost_of_Advertising == null)
                                                objuplaod.Cost_of_Advertising = 0;

                                            objuplaod.Cost_of_Advertising += Convert.ToDecimal(otherT.amount);

                                            handleDebit(Convert.ToDecimal(otherT.amount));
                                        }
                                        else if (otherT.description == "Storage Fee") //sharad21
                                        {
                                            if (objuplaod.Storage_Fee == null)
                                                objuplaod.Storage_Fee = 0;

                                            objuplaod.Storage_Fee += Convert.ToDecimal(otherT.amount);

                                            handleDebit(Convert.ToDecimal(otherT.amount));
                                        }
                                        else if (otherT.description == "StorageBillingCGST")
                                        {
                                            if (storageFee_tax == null)
                                                storageFee_tax = new tbl_tax();
                                            storageFee_tax.reference_type = 10; //settlement_upload
                                            storageFee_tax.isactive = 1;
                                            storageFee_tax.tbl_seller_id = SellerId;

                                            storageFee_tax.CGST_amount = Convert.ToDouble(otherT.amount);
                                            handleDebit(Convert.ToDecimal(otherT.amount));

                                            //obj1.giftwarp_tax = Convert.ToDouble(giftraptax);
                                            //objtax.tbl_history_id = get_orderhistory.Id;
                                            //dba.tbl_tax.Add(obj1);
                                            //dba.SaveChanges();
                                        }
                                        else if (otherT.description == "StorageBillingSGST")
                                        {
                                            if (storageFee_tax == null)
                                                storageFee_tax = new tbl_tax();

                                            storageFee_tax.reference_type = 10; //settlement_upload

                                            storageFee_tax.isactive = 1;
                                            storageFee_tax.tbl_seller_id = SellerId;
                                            storageFee_tax.sgst_amount = Convert.ToDouble(otherT.amount);
                                            handleDebit(Convert.ToDecimal(otherT.amount));
                                        }
                                        else if (otherT.description == "BalanceAdjustment") //sharad21
                                        {
                                            if (objuplaod.BalanceAdjustment == null)
                                                objuplaod.BalanceAdjustment = 0;

                                            objuplaod.BalanceAdjustment += Convert.ToDecimal(otherT.amount);
                                            handleCredit(Convert.ToDecimal(otherT.amount));
                                        }
                                        else if (otherT.description == "Payable to Amazon") //sharad91
                                        {
                                            if (objuplaod.Payable_to_Amazon == null)
                                                objuplaod.Payable_to_Amazon = 0;

                                            objuplaod.Payable_to_Amazon += Convert.ToDecimal(otherT.amount);
                                            handleDebit(Convert.ToDecimal(otherT.amount));
                                        }

                                        else if (otherT.description == "FBAInboundTransportationFee") //vineet new 
                                        {
                                            if (objuplaod.FBAInboundTransportationFee == null)
                                                objuplaod.FBAInboundTransportationFee = 0;

                                            objuplaod.FBAInboundTransportationFee += Convert.ToDecimal(otherT.amount);
                                            handleDebit(Convert.ToDecimal(otherT.amount));
                                        }
                                        else if (otherT.description == "CGST")
                                        {
                                            if (storageFee_tax == null)
                                                storageFee_tax = new tbl_tax();
                                            storageFee_tax.reference_type = 10; //settlement_upload
                                            storageFee_tax.isactive = 1;
                                            storageFee_tax.tbl_seller_id = SellerId;
                                            handleDebit((Decimal)Convert.ToDouble(otherT.amount));
                                            storageFee_tax.CGST_amount = Convert.ToDouble(otherT.amount);
                                            //?
                                        }
                                        else if (otherT.description == "SGST")
                                        {
                                            if (storageFee_tax == null)
                                                storageFee_tax = new tbl_tax();

                                            storageFee_tax.reference_type = 10; //settlement_upload
                                            storageFee_tax.isactive = 1;
                                            storageFee_tax.tbl_seller_id = SellerId;
                                            storageFee_tax.sgst_amount = Convert.ToDouble(otherT.amount);
                                            handleDebit((Decimal)Convert.ToDouble(otherT.amount));
                                            //?
                                        }
                                    }
                                }

                                dba.tbl_settlement_upload.Add(objuplaod);
                                dba.SaveChanges();
                                settelmentupload_id = objuplaod.Id;

                                if (storageFee_tax != null)
                                {
                                    storageFee_tax.tbl_referneced_id = settelmentupload_id;
                                    dba.tbl_tax.Add(storageFee_tax);
                                    //dba.SaveChanges();
                                }

                                tbl_imp_bank_transfers objbank = new tbl_imp_bank_transfers();
                                objbank.tbl_settlement_upload_id = objuplaod.Id;
                                if (item.total_amount != null && item.total_amount != "")
                                {
                                    objbank.amount = Convert.ToDecimal(item.total_amount);
                                    // total_debitAmt += (decimal)objbank.amount;
                                    handleDebit((Decimal)objbank.amount);
                                }
                                else
                                {
                                    objbank.amount = 0;
                                }
                                dba.tbl_imp_bank_transfers.Add(objbank);
                                dba.SaveChanges();
                            }
                            else if (item.otherTransatanctionList != null)
                            {

                                foreach (var otherT in item.otherTransatanctionList)
                                {
                                    if (otherT.description == "Previous Reserve Amount Balance")
                                    {
                                        objuplaod.previous_reserve_amount = Convert.ToDecimal(otherT.amount);
                                        total_creditAmt += (Decimal)objuplaod.previous_reserve_amount;
                                    }
                                    else if (otherT.description == "NonSubscriptionFeeAdj")
                                    {
                                        if (objuplaod.Nonsubscription_feeadj == null)
                                            objuplaod.Nonsubscription_feeadj = 0;

                                        objuplaod.Nonsubscription_feeadj += Convert.ToDecimal(otherT.amount);

                                        if (Convert.ToDecimal(otherT.amount) < 0)
                                        {
                                            handleDebit(Convert.ToDecimal(otherT.amount) * (-1));
                                            //total_debitAmt += ;
                                        }
                                        else
                                        {
                                            handleCredit(Convert.ToDecimal(otherT.amount));
                                            //total_creditAmt += Convert.ToDecimal(otherT.amount);
                                        }
                                    }
                                    else if (otherT.description == "Current Reserve Amount")
                                    {
                                        objuplaod.current_reserve_amount = Convert.ToDecimal(otherT.amount);
                                        if (objuplaod.current_reserve_amount < 0)
                                        {
                                            handleDebit((Decimal)objuplaod.current_reserve_amount * (-1));
                                            //total_debitAmt += (Decimal)objuplaod.current_reserve_amount * (-1);
                                        }
                                        else
                                        {
                                            handleCredit((Decimal)objuplaod.current_reserve_amount);
                                            //total_creditAmt += (Decimal)objuplaod.current_reserve_amount;
                                        }
                                    } //sharad
                                    else if (otherT.description == "INCORRECT_FEES_ITEMS")
                                    {
                                        objuplaod.INCORRECT_FEES_ITEMS += Convert.ToDecimal(otherT.amount);
                                        handleCredit(Convert.ToDecimal(otherT.amount));
                                    }
                                    else if (otherT.description == "TransactionTotalAmount")
                                    {
                                        objuplaod.Cost_of_Advertising += Convert.ToDecimal(otherT.amount);
                                        handleDebit(Convert.ToDecimal(otherT.amount));
                                    }
                                    else if (otherT.description == "Storage Fee") //sharad21
                                    {
                                        if (objuplaod.Storage_Fee == null)
                                            objuplaod.Storage_Fee = 0;

                                        objuplaod.Storage_Fee += Convert.ToDecimal(otherT.amount);
                                        handleDebit(Convert.ToDecimal(otherT.amount));
                                    }
                                    else if (otherT.description == "StorageBillingCGST")
                                    {
                                        if (storageFee_tax == null)
                                            storageFee_tax = new tbl_tax();
                                        storageFee_tax.reference_type = 10; //settlement_upload
                                        storageFee_tax.isactive = 1;
                                        storageFee_tax.tbl_seller_id = SellerId;

                                        storageFee_tax.CGST_amount = Convert.ToDouble(otherT.amount);
                                        handleDebit(Convert.ToDecimal(otherT.amount));

                                        //obj1.giftwarp_tax = Convert.ToDouble(giftraptax);
                                        //objtax.tbl_history_id = get_orderhistory.Id;
                                        //dba.tbl_tax.Add(obj1);
                                        //dba.SaveChanges();
                                    }
                                    else if (otherT.description == "StorageBillingSGST")
                                    {
                                        if (storageFee_tax == null)
                                            storageFee_tax = new tbl_tax();

                                        storageFee_tax.reference_type = 10; //settlement_upload

                                        storageFee_tax.isactive = 1;
                                        storageFee_tax.tbl_seller_id = SellerId;
                                        storageFee_tax.sgst_amount = Convert.ToDouble(otherT.amount);
                                        handleDebit(Convert.ToDecimal(otherT.amount));
                                    }
                                    else if (otherT.description == "BalanceAdjustment") //sharad21
                                    {
                                        if (objuplaod.BalanceAdjustment == null)
                                            objuplaod.BalanceAdjustment = 0;

                                        objuplaod.BalanceAdjustment += Convert.ToDecimal(otherT.amount);
                                        handleCredit(Convert.ToDecimal(otherT.amount));
                                    }
                                    else if (otherT.description == "Payable to Amazon") //sharad91
                                    {
                                        if (objuplaod.Payable_to_Amazon == null)
                                            objuplaod.Payable_to_Amazon = 0;

                                        objuplaod.Payable_to_Amazon += Convert.ToDecimal(otherT.amount);
                                        handleDebit(Convert.ToDecimal(otherT.amount));
                                    }

                                    else if (otherT.description == "FBAInboundTransportationFee") //vineet new 
                                    {
                                        if (objuplaod.FBAInboundTransportationFee == null)
                                            objuplaod.FBAInboundTransportationFee = 0;

                                        objuplaod.FBAInboundTransportationFee += Convert.ToDecimal(otherT.amount);
                                        handleDebit(Convert.ToDecimal(otherT.amount));
                                    }
                                    else if (otherT.description == "CGST")
                                    {
                                        if (storageFee_tax == null)
                                            storageFee_tax = new tbl_tax();
                                        storageFee_tax.reference_type = 10; //settlement_upload
                                        storageFee_tax.isactive = 1;
                                        storageFee_tax.tbl_seller_id = SellerId;
                                        handleDebit((Decimal)Convert.ToDouble(otherT.amount));
                                        storageFee_tax.CGST_amount = Convert.ToDouble(otherT.amount);
                                        //?
                                    }
                                    else if (otherT.description == "SGST")
                                    {
                                        if (storageFee_tax == null)
                                            storageFee_tax = new tbl_tax();

                                        storageFee_tax.reference_type = 10; //settlement_upload
                                        storageFee_tax.isactive = 1;
                                        storageFee_tax.tbl_seller_id = SellerId;
                                        storageFee_tax.sgst_amount = Convert.ToDouble(otherT.amount);
                                        handleDebit((Decimal)Convert.ToDouble(otherT.amount));
                                        //?
                                    }
                                }
                                dba.Entry(objuplaod).State = EntityState.Modified;

                                dba.SaveChanges();
                            }
                        }//end for
                        string connectionstring = ConfigurationManager.ConnectionStrings["SellerConnection"].ConnectionString;
                        MySqlConnection con = new MySqlConnection(connectionstring);
                        var get_unique_count_details = "SELECT count(DISTINCT aa.id) FROM(SELECT DISTINCT amazon_order_id AS id FROM `tbl_sales_order` where tbl_sellers_id = " + SellerId + " UNION ALL SELECT DISTINCT OrderID AS id FROM `tbl_order_history` where tbl_seller_id =" + SellerId + " UNION ALL SELECT DISTINCT Order_Id AS id FROM `tbl_settlement_order` where tbl_seller_id =" + SellerId + " ) AS aa";
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
                        var diff_order = 0;
                        var get_balance_details = db.tbl_user_login.Where(a => a.tbl_sellers_id == SellerId).FirstOrDefault();
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
                            db.SaveChanges();

                            Session["WalletBalance"] = get_balance_details.wallet_balance.ToString();
                            Session["TotalOrders"] = totalOrder.ToString();
                        }
                        var get_upload_settlement_details = dba.tbl_settlement_upload.Where(a => a.tbl_seller_id == SellerId && a.Id == objuplaod.Id).FirstOrDefault();
                        if (get_upload_settlement_details != null)
                        {
                            //suspense account
                            if (total_creditAmt != total_debitAmt)
                            {
                                suspense_Amount = total_debitAmt - total_creditAmt;
                                get_upload_settlement_details.suspense_amt = suspense_Amount;
                            }
                            get_upload_settlement_details.new_order_uploaded = diff_order;
                            //get_upload_settlement_details.new_order_uploaded = uniqueupload;
                            dba.Entry(get_upload_settlement_details).State = EntityState.Modified;
                            dba.SaveChanges();
                        }
                        if (get_seller_setting != null)
                        {
                            if (current_running_no != 0)
                            {
                                get_seller_setting.current_running_no += 1;
                                dba.Entry(get_seller_setting).State = EntityState.Modified;
                                dba.SaveChanges();
                            }
                        }
                    }
                    else
                    {
                        datasaved = "File with same Settlement Reference number already Exists. ";
                    }
                }// end of if(Condition null)


            }// end of try
            catch (Exception ex)
            {
                datasaved = "F" + ex.Message.ToString(); ;
            }// end of catch()
            return datasaved;
        }
        /// <summary>
        /// To get Order Settlement Details 
        /// </summary>
        /// <param name="obj"></param>
        /// <returns></returns>
        public JsonResult GetOrderSettlementDetails1(viewSalesOrder obj)
        {
            cf = new comman_function();
            bool ss = cf.session_check();
            List<Ordertext2> lstOrdertext2 = new List<Ordertext2>();
            Ordertext2 obj_Ordertext2 = new Ordertext2();
            int SellerId = Convert.ToInt32(Session["SellerID"]);
            var get_settlementdetails = (from ob_tbl_sales_order in dba.tbl_sales_order.Where(a => a.id == obj.id)
                                         join ob_m_tbl_expense in dba.m_tbl_expense on ob_tbl_sales_order.amazon_order_id equals (ob_m_tbl_expense.settlement_order_id)
                                         into JoinedEmpDept2
                                         from proj2 in JoinedEmpDept2
                                         select new SellerUtility
                                         {
                                             ob_tbl_sales_order = ob_tbl_sales_order,
                                             ob_m_tbl_expense = proj2
                                         }).ToList();

            int i = 0;
            foreach (var Item in get_settlementdetails)
            {
                if (Item != null)
                {
                    var exp_id = Item.ob_m_tbl_expense.expense_type_id;
                    var get_expdetails = dba.m_settlement_fee.Where(a => a.id == exp_id).FirstOrDefault();
                    var gettax_details = dba.tbl_tax.Where(a => a.tbl_referneced_id == Item.ob_m_tbl_expense.id && a.reference_type == 2).FirstOrDefault();


                    if (get_expdetails != null)
                    {
                        string nam = get_expdetails.return_fee;
                        if (nam == "Principal")
                        {
                            if (get_expdetails.id == exp_id)
                            {
                                obj_Ordertext2.OrderID = Item.ob_m_tbl_expense.settlement_order_id;
                                obj_Ordertext2.ReferenceID = Item.ob_m_tbl_expense.reference_number;
                                obj_Ordertext2.Principal = Convert.ToDouble(Item.ob_m_tbl_expense.expense_amount);
                            }
                        }
                        else if (nam == "Product Tax")
                        {
                            if (get_expdetails.id == exp_id)
                            {
                                obj_Ordertext2.Product_Tax = Convert.ToDouble(Item.ob_m_tbl_expense.expense_amount);
                            }
                        }
                        else if (nam == "Shipping")
                        {
                            if (get_expdetails.id == exp_id)
                            {
                                obj_Ordertext2.Shipping = Convert.ToDouble(Item.ob_m_tbl_expense.expense_amount);
                            }
                        }

                        else if (nam == "Shipping tax")
                        {
                            if (get_expdetails.id == exp_id)
                            {
                                obj_Ordertext2.Shipping_Tax = Convert.ToDouble(Item.ob_m_tbl_expense.expense_amount);
                            }
                        }
                        else if (nam == "FBA Weight Handling Fee")
                        {
                            if (get_expdetails.id == exp_id)
                            {
                                obj_Ordertext2.FBAFEE = Convert.ToDouble(Item.ob_m_tbl_expense.expense_amount);
                                if (gettax_details != null)
                                {
                                    obj_Ordertext2.FBACGST = Convert.ToDouble(gettax_details.CGST_amount);
                                    obj_Ordertext2.FBASGST = Convert.ToDouble(gettax_details.sgst_amount);
                                }
                            }
                        }
                        else if (nam == "Technology Fee")
                        {
                            if (get_expdetails.id == exp_id)
                            {
                                obj_Ordertext2.TechnologyFee = Convert.ToDouble(Item.ob_m_tbl_expense.expense_amount);
                                if (gettax_details != null)
                                {
                                    obj_Ordertext2.TechnologyIGST = Convert.ToDouble(gettax_details.Igst_amount);
                                }
                            }
                        }
                        else if (nam == "Commission")
                        {
                            if (get_expdetails.id == exp_id)
                            {
                                obj_Ordertext2.CommissionFee = Convert.ToDouble(Item.ob_m_tbl_expense.expense_amount);
                                if (gettax_details != null)
                                {
                                    obj_Ordertext2.CommissionIGST = Convert.ToDouble(gettax_details.Igst_amount);
                                }
                            }
                        }
                        else if (nam == "Fixed closing fee")
                        {
                            if (get_expdetails.id == exp_id)
                            {
                                obj_Ordertext2.FixedClosingFee = Convert.ToDouble(Item.ob_m_tbl_expense.expense_amount);
                                if (gettax_details != null)
                                {
                                    obj_Ordertext2.FixedclosingIGST = Convert.ToDouble(gettax_details.Igst_amount);
                                }
                            }
                        }
                        else if (nam == "Shipping Chargeback")
                        {
                            if (get_expdetails.id == exp_id)
                            {
                                obj_Ordertext2.ShippingChargebackFee = Convert.ToDouble(Item.ob_m_tbl_expense.expense_amount);
                                if (gettax_details != null)
                                {
                                    obj_Ordertext2.shippingchargeCGST = Convert.ToDouble(gettax_details.CGST_amount);
                                    obj_Ordertext2.shippingchargeSGST = Convert.ToDouble(gettax_details.sgst_amount);
                                }
                            }
                        }
                        else if (nam == "Shipping discount")
                        {
                            if (get_expdetails.id == exp_id)
                            {
                                obj_Ordertext2.ShippingDiscountFee = Convert.ToDouble(Item.ob_m_tbl_expense.expense_amount);
                                if (gettax_details != null)
                                {
                                    obj_Ordertext2.Shippingtaxdiscount = Convert.ToDouble(gettax_details.rateoftax_amount);

                                }

                            }
                        }
                    }// end of if(get_expdetails)
                    //if (i == 0)
                    //{
                    //    obj_Ordertext2.OrderID = Item.ob_m_tbl_expense.settlement_order_id;
                    //    obj_Ordertext2.ReferenceID = Item.ob_m_tbl_expense.reference_number;
                    //    obj_Ordertext2.Principal = Convert.ToDouble(Item.ob_m_tbl_expense.expense_amount);
                    //}
                    //else if (i == 1)
                    //{
                    //    obj_Ordertext2.Product_Tax = Convert.ToDouble(Item.ob_m_tbl_expense.expense_amount);

                    //}
                    //else if (i == 2)
                    //{
                    //    obj_Ordertext2.Shipping = Convert.ToDouble(Item.ob_m_tbl_expense.expense_amount);

                    //}
                    //else if (i == 3)
                    //{
                    //    obj_Ordertext2.Shipping_Tax = Convert.ToDouble(Item.ob_m_tbl_expense.expense_amount);

                    //}
                    //else
                    //{
                    //    break;
                    //}

                    //Ordertext2 objtax = new Ordertext2();
                    //if (gettax_details != null)
                    //{
                    //    objtax.OrderCGST = Convert.ToDouble(gettax_details.CGST_amount);
                    //    objtax.OrderIGST = Convert.ToDouble(gettax_details.Igst_amount);
                    //    objtax.OrderSGST = Convert.ToDouble(gettax_details.sgst_amount);
                    //}
                    //objtax.SettlementType = get_expdetails.return_fee;
                    //objtax.ReferenceNo = Item.ob_m_tbl_expense.reference_number;
                    //objtax.SettlementDate = Convert.ToDateTime(Item.ob_m_tbl_expense.settlement_datetime);
                    //objtax.OrderPrice = Convert.ToDouble(Item.ob_m_tbl_expense.expense_amount);
                    //objtax.OrderSettlementID = Item.ob_m_tbl_expense.settlement_order_id;
                    //objtax.settlement_id = Item.ob_m_tbl_expense.id;
                    //lst_sellermarketplace.Add(objtax);
                    i++;
                }
            }// end of foreach
            obj_Ordertext2.SumOrder = obj_Ordertext2.Principal + obj_Ordertext2.Product_Tax + obj_Ordertext2.Shipping + obj_Ordertext2.Shipping_Tax;
            obj_Ordertext2.SumFee = obj_Ordertext2.FBAFEE + obj_Ordertext2.TechnologyFee + obj_Ordertext2.CommissionFee + obj_Ordertext2.FixedClosingFee + obj_Ordertext2.ShippingChargebackFee + obj_Ordertext2.ShippingDiscountFee;
            obj_Ordertext2.SumTaxFee = obj_Ordertext2.FBACGST + obj_Ordertext2.FBASGST + obj_Ordertext2.TechnologyIGST + obj_Ordertext2.CommissionIGST + obj_Ordertext2.FixedclosingIGST + obj_Ordertext2.shippingchargeCGST + obj_Ordertext2.shippingchargeSGST + obj_Ordertext2.Shippingtaxdiscount;

            obj_Ordertext2.NetTotal = obj_Ordertext2.SumOrder + (obj_Ordertext2.SumFee + obj_Ordertext2.SumTaxFee);
            lstOrdertext2.Add(obj_Ordertext2);
            return new JsonResult { Data = lstOrdertext2, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        public JsonResult GetOrderSettlementDetails(viewSalesOrder obj)
        {
            cf = new comman_function();
            bool ss = cf.session_check();
            List<Ordertext2> lstOrdertext2 = new List<Ordertext2>();
            Ordertext2 obj_Ordertext2 = new Ordertext2();
            int SellerId = Convert.ToInt32(Session["SellerID"]);

            var get_saleorder = dba.tbl_sales_order.Where(a => a.id == obj.id && a.tbl_sellers_id == SellerId).FirstOrDefault();
            if (get_saleorder != null)
            {
                obj_Ordertext2.OrderID = get_saleorder.amazon_order_id;

                var get_settlementorder = dba.tbl_settlement_order.Where(a => a.Order_Id == get_saleorder.amazon_order_id && a.tbl_seller_id == SellerId).FirstOrDefault();
                if (get_settlementorder != null)
                {
                    obj_Ordertext2.ReferenceID = get_settlementorder.settlement_id;
                    obj_Ordertext2.Principal = Convert.ToDouble(get_settlementorder.principal_price);
                    obj_Ordertext2.Product_Tax = Convert.ToDouble(get_settlementorder.product_tax);
                    obj_Ordertext2.Shipping = Convert.ToDouble(get_settlementorder.shipping_price);
                    obj_Ordertext2.Shipping_Tax = Convert.ToDouble(get_settlementorder.shipping_tax);
                    obj_Ordertext2.GiftwarpPrice = Convert.ToDouble(get_settlementorder.giftwrap_price);
                    obj_Ordertext2.GiftwarpTax = Convert.ToDouble(get_settlementorder.giftwarp_tax);
                }// end of if (get_settlementorder)

                var get_expensedata = dba.m_tbl_expense.Where(a => a.Original_order_id == get_saleorder.amazon_order_id && a.tbl_seller_id == SellerId && a.t_transactionType_id == 1).ToList();
                if (get_expensedata != null)
                {
                    foreach (var item in get_expensedata)
                    {
                        var exp_id = item.expense_type_id;
                        var get_expdetails = dba.m_settlement_fee.Where(a => a.id == exp_id).FirstOrDefault();
                        var gettax_details = dba.tbl_tax.Where(a => a.tbl_referneced_id == item.id && a.reference_type == 2).FirstOrDefault();
                        if (get_expdetails != null)
                        {
                            string nam = get_expdetails.return_fee;
                            if (nam == "FBA Weight Handling Fee")
                            {
                                if (get_expdetails.id == exp_id)
                                {
                                    obj_Ordertext2.FBAFEE = Convert.ToDouble(item.expense_amount);
                                    if (gettax_details != null)
                                    {
                                        obj_Ordertext2.FBACGST = Convert.ToDouble(gettax_details.CGST_amount);
                                        obj_Ordertext2.FBASGST = Convert.ToDouble(gettax_details.sgst_amount);
                                    }
                                }
                            }
                            else if (nam == "Technology Fee")
                            {
                                if (get_expdetails.id == exp_id)
                                {
                                    obj_Ordertext2.TechnologyFee = Convert.ToDouble(item.expense_amount);
                                    if (gettax_details != null)
                                    {
                                        obj_Ordertext2.TechnologyIGST = Convert.ToDouble(gettax_details.Igst_amount);
                                        obj_Ordertext2.TechnologyCGST = Convert.ToDouble(gettax_details.CGST_amount);
                                        obj_Ordertext2.TechnologySGST = Convert.ToDouble(gettax_details.sgst_amount);
                                    }
                                }
                            }
                            else if (nam == "Commission")
                            {
                                if (get_expdetails.id == exp_id)
                                {
                                    obj_Ordertext2.CommissionFee = Convert.ToDouble(item.expense_amount);
                                    if (gettax_details != null)
                                    {
                                        obj_Ordertext2.CommissionIGST = Convert.ToDouble(gettax_details.Igst_amount);
                                        obj_Ordertext2.CommissionCGST = Convert.ToDouble(gettax_details.CGST_amount);
                                        obj_Ordertext2.CommissionSGST = Convert.ToDouble(gettax_details.sgst_amount);
                                    }
                                }
                            }

                            else if (nam == "Fixed closing fee")
                            {
                                if (get_expdetails.id == exp_id)
                                {
                                    obj_Ordertext2.FixedClosingFee = Convert.ToDouble(item.expense_amount);
                                    if (gettax_details != null)
                                    {
                                        obj_Ordertext2.FixedclosingIGST = Convert.ToDouble(gettax_details.Igst_amount);
                                        obj_Ordertext2.FixedclosingCGST = Convert.ToDouble(gettax_details.CGST_amount);
                                        obj_Ordertext2.FixedclosingSGST = Convert.ToDouble(gettax_details.sgst_amount);
                                    }
                                }
                            }
                            else if (nam == "Shipping Chargeback")
                            {
                                if (get_expdetails.id == exp_id)
                                {
                                    obj_Ordertext2.ShippingChargebackFee = Convert.ToDouble(item.expense_amount);
                                    if (gettax_details != null)
                                    {
                                        obj_Ordertext2.shippingchargeCGST = Convert.ToDouble(gettax_details.CGST_amount);
                                        obj_Ordertext2.shippingchargeSGST = Convert.ToDouble(gettax_details.sgst_amount);
                                    }
                                }
                            }
                            else if (nam == "Shipping discount")
                            {
                                if (get_expdetails.id == exp_id)
                                {
                                    obj_Ordertext2.ShippingDiscountFee = Convert.ToDouble(item.expense_amount);
                                    if (gettax_details != null)
                                    {
                                        obj_Ordertext2.Shippingtaxdiscount = Convert.ToDouble(gettax_details.rateoftax_amount);
                                    }
                                }
                            }
                            else if (nam == "Refund commission")
                            {
                                if (get_expdetails.id == exp_id)
                                {
                                    obj_Ordertext2.RefundCommision = Convert.ToDouble(item.expense_amount);
                                    if (gettax_details != null)
                                    {
                                        obj_Ordertext2.RefundCommisionIGST = Convert.ToDouble(gettax_details.Igst_amount);
                                    }
                                }
                            }
                        }// end of if(get_expdetails)
                    }// end of foreach loop
                }// end of if(get_expensedata)
            }// end of if(get_saleorder)
            obj_Ordertext2.SumOrder = obj_Ordertext2.Principal + obj_Ordertext2.Product_Tax + obj_Ordertext2.Shipping + obj_Ordertext2.Shipping_Tax;
            obj_Ordertext2.SumFee = obj_Ordertext2.FBAFEE + obj_Ordertext2.TechnologyFee + obj_Ordertext2.CommissionFee + obj_Ordertext2.FixedClosingFee + obj_Ordertext2.ShippingChargebackFee + obj_Ordertext2.ShippingDiscountFee;
            double taxfee = obj_Ordertext2.FBACGST + obj_Ordertext2.FBASGST + obj_Ordertext2.TechnologyIGST + obj_Ordertext2.TechnologyCGST + obj_Ordertext2.TechnologySGST + obj_Ordertext2.CommissionIGST + obj_Ordertext2.CommissionCGST +
                                       obj_Ordertext2.CommissionSGST + obj_Ordertext2.FixedclosingIGST + obj_Ordertext2.FixedclosingCGST + obj_Ordertext2.FixedclosingSGST + obj_Ordertext2.shippingchargeCGST + obj_Ordertext2.shippingchargeSGST + obj_Ordertext2.Shippingtaxdiscount;

            decimal amount = Convert.ToDecimal(taxfee);//obj_Ordertext2.SumTaxFee
            decimal amtresult = decimal.Round(amount, 2, MidpointRounding.AwayFromZero);
            obj_Ordertext2.SumTaxFee = Convert.ToDouble(amtresult);


            obj_Ordertext2.NetTotal = obj_Ordertext2.SumOrder + (obj_Ordertext2.SumFee + obj_Ordertext2.SumTaxFee);
            lstOrdertext2.Add(obj_Ordertext2);
            return new JsonResult { Data = lstOrdertext2, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        /// <summary>
        /// This is for Reimbursement Data Parse Code from Amazon
        /// </summary>
        /// <returns></returns>
        ///  List<AmazonreconciliationOrder> objjson1 = null;
        ///  
        List<ReimburesementDetail> details = null;
        public ActionResult ReimbursementData()
        {
            details = new List<ReimburesementDetail>();
            List<Reimbursement> objReimbursement = new List<Reimbursement>();
            try
            {
                string text;
                string text1;
                Excel.Application xlApp;
                Excel.Workbook xlWorkBook;
                Excel.Worksheet xlWorkSheet;
                Excel.Range range;
                DataTable dt = new DataTable();
                string str;
                int rCnt;
                int cCnt;
                int rw = 0;
                int cl = 0;

                xlApp = new Excel.Application();
                xlWorkBook = xlApp.Workbooks.Open(@"c:\\Reimbursement.xls", 0, true, 5, "", "", true, Microsoft.Office.Interop.Excel.XlPlatform.xlWindows, "\t", false, false, 0, true, 1, 0);
                xlWorkSheet = (Excel.Worksheet)xlWorkBook.Worksheets.get_Item(1);
                range = xlWorkSheet.UsedRange;
                rw = range.Rows.Count;
                cl = range.Columns.Count;
                for (int i = 1; i <= rw; i++)
                {
                    DataRow dtrow = dt.NewRow();
                    for (int j = 1; j <= cl; j++)
                    {
                        int cell = j - 1;
                        var stsr = (range.Cells[i, j] as Excel.Range).Value2;
                        if (i == 1)
                            dt.Columns.Add(stsr);
                        else
                            dtrow[cell] = stsr;
                    }
                    if (i != 1)
                        dt.Rows.Add(dtrow);
                }
                if (dt.Rows.Count > 0)
                {
                    for (int k = 0; k < dt.Rows.Count; k++)
                    {
                        Reimbursement obj_item = new Reimbursement();
                        obj_item.Approval_date = dt.Rows[k][0].ToString();
                        obj_item.Reimbursement_id = dt.Rows[k][1].ToString();
                        obj_item.Case_id = dt.Rows[k][2].ToString();
                        obj_item.Amazon_order_id = dt.Rows[k][3].ToString();
                        obj_item.Reason = dt.Rows[k][4].ToString();
                        obj_item.Sku = dt.Rows[k][5].ToString();
                        obj_item.FnSku = dt.Rows[k][6].ToString();
                        obj_item.Asin = dt.Rows[k][7].ToString();
                        obj_item.Produc_Name = dt.Rows[k][8].ToString();
                        obj_item.Condition = dt.Rows[k][9].ToString();
                        obj_item.Currency_Unit = dt.Rows[k][10].ToString();
                        obj_item.Amazon_per_unit = dt.Rows[k][11].ToString();
                        obj_item.Amount_Total = dt.Rows[k][12].ToString();
                        obj_item.Quantity_Cash = dt.Rows[k][13].ToString();
                        objReimbursement.Add(obj_item);
                    }
                }
                details.Add(new ReimburesementDetail
                {
                    ReimbursementOrderDetails = objReimbursement,
                });

                SaveReimbursementData(details);// for save settlement data in expense table

                GC.Collect();
                GC.WaitForPendingFinalizers();
                Marshal.ReleaseComObject(range);
                Marshal.ReleaseComObject(xlWorkSheet);
                //close and release
                xlWorkBook.Close();
                Marshal.ReleaseComObject(xlWorkBook);
                //quit and release
                xlApp.Quit();
                Marshal.ReleaseComObject(xlApp);
            }
            catch (Exception ex)
            {
            }
            return View(details);
        }

        public string SaveReimbursementData(List<ReimburesementDetail> details)
        {
            cf = new comman_function();
            bool ss = cf.session_check();
            string datasaved = "";
            try
            {
                tbl_order_history objhistory = new tbl_order_history();
                var getdetails = dba.tbl_sales_order_status.Where(a => a.is_active == 0).ToList();
                string name = "Reimbursement".ToLower();
                int SellerId = Convert.ToInt32(Session["SellerID"]);
                if (details != null)
                {
                    foreach (var item in details[0].ReimbursementOrderDetails)
                    {
                        objhistory.created_on = DateTime.Now;
                        objhistory.tbl_seller_id = SellerId;
                        objhistory.updated_by = SellerId;
                        objhistory.case_id = item.Case_id;
                        objhistory.reimbursement_id = item.Reimbursement_id;
                        objhistory.OrderID = item.Amazon_order_id;
                        objhistory.ReplacementReasonCode = item.Reason;
                        objhistory.SKU = item.Sku;
                        objhistory.ASIN = item.Asin;
                        objhistory.approval_date = Convert.ToDateTime(item.Approval_date);
                        if (item.Amazon_per_unit != "")
                        {
                            objhistory.amount_per_unit = Convert.ToDouble(item.Amazon_per_unit);
                        }
                        else
                        {
                            objhistory.amount_per_unit = 0;
                        }
                        if (item.Quantity_Cash != "")
                        {
                            objhistory.Quantity = Convert.ToInt16(item.Quantity_Cash);
                        }
                        else
                        {
                            objhistory.Quantity = 1;
                        }
                        objhistory.tbl_marketplace_id = 3;
                        foreach (var item1 in getdetails)
                        {
                            string name1 = item1.sales_order_status.ToLower();
                            if (name == name1)
                            {
                                objhistory.t_order_status = item1.id;
                                break;
                            }
                        }// end of foreach item1
                        dba.tbl_order_history.Add(objhistory);
                        dba.SaveChanges();
                        ////--------------------------------update tblsalesorder---------------------///
                        string amazonid = objhistory.OrderID.ToLower();
                        var getdata = dba.tbl_sales_order.Where(a => a.is_active == 1 && a.tbl_sellers_id == SellerId && a.amazon_order_id.ToLower() == amazonid).ToList();
                        if (getdata != null)
                        {
                            foreach (var data in getdata)
                            {
                                data.n_item_orderstatus = objhistory.t_order_status;
                                dba.Entry(data).State = EntityState.Modified;
                                dba.SaveChanges();
                            }// end of foreach
                        }// end of if(getdata)
                        ///////////////////-----------------------END---------------------------------/////
                    }// end of foreach(item)
                }// end of if(details)
                return datasaved;
            }// end of try block
            catch (Exception ex)
            {
            }// end of catch block
            return "";
        }


        /// <summary>
        /// this is for Returns Data Parse Code
        /// </summary>
        /// <returns></returns>
        public ActionResult ReturnsData()
        {
            List<ReturensDataDetail> returnsdetails = new List<ReturensDataDetail>();
            List<ReturnsData> objReturnsData = new List<ReturnsData>();
            try
            {
                string text;
                string text1;
                Excel.Application xlApp;
                Excel.Workbook xlWorkBook;
                Excel.Worksheet xlWorkSheet;
                Excel.Range range;
                DataTable dt = new DataTable();
                string str;
                int rCnt;
                int cCnt;
                int rw = 0;
                int cl = 0;

                xlApp = new Excel.Application();
                xlWorkBook = xlApp.Workbooks.Open(@"c:\\ReturnsData.xls", 0, true, 5, "", "", true, Microsoft.Office.Interop.Excel.XlPlatform.xlWindows, "\t", false, false, 0, true, 1, 0);
                xlWorkSheet = (Excel.Worksheet)xlWorkBook.Worksheets.get_Item(1);
                range = xlWorkSheet.UsedRange;
                rw = range.Rows.Count;
                cl = range.Columns.Count;
                for (int i = 1; i <= rw; i++)
                {
                    DataRow dtrow = dt.NewRow();
                    for (int j = 1; j <= cl; j++)
                    {
                        int cell = j - 1;
                        var stsr = (range.Cells[i, j] as Excel.Range).Value2;
                        if (i == 1)
                            dt.Columns.Add(stsr);
                        else
                            dtrow[cell] = stsr;
                    }
                    if (i != 1)
                        dt.Rows.Add(dtrow);
                }
                if (dt.Rows.Count > 0)
                {
                    for (int k = 0; k < dt.Rows.Count; k++)
                    {
                        ReturnsData obj_item = new ReturnsData();
                        obj_item.ReturnDate = dt.Rows[k][0].ToString();
                        obj_item.Order_id = dt.Rows[k][1].ToString();
                        obj_item.sku = dt.Rows[k][2].ToString();
                        obj_item.Asin = dt.Rows[k][3].ToString();
                        obj_item.Fnsku = dt.Rows[k][4].ToString();
                        obj_item.Product_name = dt.Rows[k][5].ToString();
                        obj_item.quantity = dt.Rows[k][6].ToString();
                        obj_item.fullfillment_centerid = dt.Rows[k][7].ToString();
                        obj_item.DetailedDepostion = dt.Rows[k][8].ToString();
                        obj_item.Reason = dt.Rows[k][9].ToString();
                        obj_item.Licence_paltenumber = dt.Rows[k][10].ToString();
                        obj_item.Customer_comments = dt.Rows[k][11].ToString();

                        objReturnsData.Add(obj_item);
                    }
                }
                returnsdetails.Add(new ReturensDataDetail
                {
                    ReturnsDataDetails = objReturnsData,
                });

                //SaveReimbursementData(details);// for save settlement data in expense table

                GC.Collect();
                GC.WaitForPendingFinalizers();
                Marshal.ReleaseComObject(range);
                Marshal.ReleaseComObject(xlWorkSheet);
                //close and release
                xlWorkBook.Close();
                Marshal.ReleaseComObject(xlWorkBook);
                //quit and release
                xlApp.Quit();
                Marshal.ReleaseComObject(xlApp);
            }// end of try block
            catch (Exception ex)
            {
            }// end of catch block
            return View(returnsdetails);
        }

        /// <summary>
        ///Used for Browse and Upload Settlement Excel file 
        /// </summary>
        /// <returns></returns>
        public ActionResult UploadSettlementExcel()
        {
            cf = new comman_function();
            if (!cf.session_check())
                return RedirectToAction("Login", "Home");
            int sellers_id = Convert.ToInt32(Session["SellerID"]);
            

            List<SelectListItem> lst1_loc = cf.GetMarketPlace(sellers_id);
            ViewData["MarKetPlace"] = lst1_loc;
            return View();
        }

        [HttpPost]
        public ActionResult UploadSettlementExcel(HttpPostedFileBase postedFile, FormCollection form)
        {
            string strFolderPath = "";
            try
            {
                //if (!cf.session_check())
                //    return RedirectToAction("Login", "Home");
                SellerId = Convert.ToInt32(Session["SellerID"]);
                cf = new comman_function();
                List<SelectListItem> lst1_loc = cf.GetMarketPlace(SellerId);
                ViewData["MarKetPlace"] = lst1_loc;

                string strmarketplace = form["ddl_marketplace"].ToString();
                int marketplaceID = 0;
                if (strmarketplace != null && strmarketplace != "" && strmarketplace != "0")
                    marketplaceID = Convert.ToInt32(strmarketplace);
                else
                {
                    ViewBag.Message = "Select Marketplace";
                    return View("UploadSettlementExcel");
                }


                if (SellerId == 0)
                {
                    ViewBag.Message = "Please login again";
                    return View("UploadSettlementExcel");
                }
                if (postedFile != null)
                {
                    //string strFolderPath = Server.MapPath(System.Configuration.ConfigurationManager.AppSettings["UploadExcel"]);
                    strFolderPath = Server.MapPath("~/UploadExcel/" + SellerId + "/settlement/");
                    if (!Directory.Exists(strFolderPath))
                    {
                        Directory.CreateDirectory(strFolderPath);
                    }
                    string strFileName = Path.GetFileName(postedFile.FileName);
                    postedFile.SaveAs(strFolderPath + strFileName);

                   
                    string strFundTransfer = form["ddlFundType"].ToString();              
                    
                    // string strmarketplace = form["ddl_marketplace"].ToString();
                    string success = "";

                    Browse_Upload_Excel_Utility obj = new Browse_Upload_Excel_Utility();
                    if (strmarketplace == "3")
                    {
                        //Amazon
                        short sourcetype = 1;
                        try
                        {
                            objjson1 = obj.ReadSettlementFile_Amazon_flatfile(strFolderPath + strFileName, SellerId);
                        }
                        catch (Exception e)
                        {

                        }
                        if (objjson1 == null)
                        {
                            objjson1 = obj.ReadSettlementFile_Amazon_v2(strFolderPath + strFileName, SellerId);
                        }
                        int typeID = 0;
                        // int marketplaceID = 0;

                        if (strFundTransfer != null && strFundTransfer != "")
                            typeID = Convert.ToInt32(strFundTransfer);
                        // if (strmarketplace != null && strmarketplace != "")
                        //     marketplaceID = Convert.ToInt32(strmarketplace);
                        //string strFundTransfer = ddlFundType
                        int upload_tbl_id = 0;

                        if (objjson1 != null)
                        {
                            success = Savesettlementdata(objjson1, typeID, marketplaceID, strFileName, sourcetype);// for save settlement data in expense table
                        }
                    }
                    else if (strmarketplace == "1")
                    {
                        //flipkart                       
                        //objjson1 = obj.ReadSettlementFile_Flipkart(strFolderPath + strFileName, SellerId);

                        int typeID = 0;
                        if (strFundTransfer != null && strFundTransfer != "")
                            typeID = Convert.ToInt32(strFundTransfer);

                        if (objjson1 != null)
                        {
                            //success = Savesettlementdata(objjson1, typeID, marketplaceID, strFileName);// for save settlement data in expense table
                        }
                    }
                    ViewBag.Message = success == "S" ? "File uploaded successfully." : "Unable to upload. " + success;
                }
            }
            catch (Exception ex)
            {
                ViewData["Message"] = strFolderPath + " == Unable to upload. " + ex.Message.ToString();
            }
            return View();
        }



        [HttpPost]
        public ActionResult UploadTaxExcel(HttpPostedFileBase postedFile, FormCollection form)
        {
            try
            {
                //if (!cf.session_check())
                //    return RedirectToAction("Login", "Home");
                SellerId = Convert.ToInt32(Session["SellerID"]);
                cf = new comman_function();
                List<SelectListItem> lst1_loc = cf.GetMarketPlace(SellerId);
                ViewData["MarKetPlace"] = lst1_loc;

                string strmarketplace = form["ddl_marketplace"].ToString();
                int marketplaceID = 0;
                if (strmarketplace != null && strmarketplace != "" && strmarketplace != "0")
                    marketplaceID = Convert.ToInt32(strmarketplace);
                else
                {
                    ViewBag.Message = "Select Marketplace";
                    return View("UploadSettlementExcel");
                }


                if (SellerId == 0)
                {
                    ViewBag.Message = "Please login again";
                    return View("UploadSettlementExcel");
                }
                if (postedFile != null)
                {
                    string strFolderPath = Server.MapPath(System.Configuration.ConfigurationManager.AppSettings["UploadExcel"]);

                    strFolderPath = Server.MapPath("~/UploadExcel/" + SellerId + "/Tax/");
                    if (!Directory.Exists(strFolderPath))
                    {
                        Directory.CreateDirectory(strFolderPath);
                    }
                    string strFileName = Path.GetFileName(postedFile.FileName);
                    postedFile.SaveAs(strFolderPath + strFileName);

                    int addorder = 0;
                    string orderchecked = form["checked"].ToString();
                    if (orderchecked != "false")
                    {
                        addorder = 1;
                    }
                    Browse_Upload_Excel_Utility obj = new Browse_Upload_Excel_Utility();
                    obj.csv(strFolderPath + strFileName, 1, SellerId, marketplaceID, addorder, strFileName);

                    string success = "S";// Savesettlementdata(objjson1);// for save settlement data in expense table

                    ViewBag.Message = success == "S" ? "File uploaded successfully." : "Unable to upload. " + success;
                }
            }
            catch (Exception ex)
            {
                ViewBag.Message = "Unable to upload. " + ex.Message.ToString();
            }
            return View("UploadSettlementExcel");
        }


        List<tbl_SalesOrder> objdata = null;



        ///--------------------------------------Changes------------------------------------
        ///
        public List<tbl_order_history> historyObj_list = null;
        public List<tbl_sales_order_details> SalesObj_list = null;


        public int Saveorderuploaddata(tbl_sales_order objsale, int marketplaceID, int FileUploadId, List<OrderItem> obj_order_items, int SellerId)
        {
            int datasaved = 0;
            //var getdetails = dba.tbl_sales_order_status.Where(a => a.is_active == 0).ToList();
            try
            {
                //cf = new comman_function();
                //bool ss = cf.session_check();
                //    return RedirectToAction("Login", "Home");
                //tbl_sales_order objsale = new tbl_sales_order();

                //List<tbl_order_history> historyObj_list = new List<tbl_order_history>();

                if (objsale != null)
                {


                    var get_saleorder = dba.tbl_sales_order.Where(a => a.tbl_sellers_id == SellerId && a.amazon_order_id == objsale.amazon_order_id).FirstOrDefault();
                    if (get_saleorder == null)
                    {
                        /*objsale.amazon_order_id = objdata.amazon_order_id;
                        objsale.purchase_date = Convert.ToDateTime(objdata.purchase_date);
                        objsale.last_updated_date = Convert.ToDateTime(objdata.last_updated_date);
                        objsale.order_status = objdata.order_status;
                        objsale.is_business_order = objdata.is_business_order;
                        objsale.sales_channel = objdata.sales_channel;
                        objsale.m_ship_service_level_id = objdata.m_ship_service_level_id;
                        objsale.created_on = DateTime.Now;
                        objsale.tbl_sellers_id = SellerId;
                        objsale.is_active = 1;
                        objsale.tbl_Marketplace_Id = marketplaceID;
                    objsale.tbl_order_upload_id =FileUploadId;
                        objsale.bill_amount = Convert.ToDouble(objdata.bill_amount); //sharad

                        if (objdata.fullfillment_channel == "Amazon")
                        {
                            objsale.n_fullfilled_id = 1;
                        }
                        else
                        {
                            objsale.n_fullfilled_id = 2;
                        }*/

                        /*string name1 = objsale.order_status.ToLower();
                        foreach (var item1 in getdetails)
                        {
                            string name = item1.sales_order_status.ToLower();
                            if (name == name1)
                            {
                                objsale.n_item_orderstatus = item1.id;
                                break;
                            }
                        }// end of foreach item1
                        */

                        dba.tbl_sales_order.Add(objsale);
                        dba.SaveChanges();

                        if (obj_order_items != null)
                        {
                            //double OrderBillAmount = 0;

                            foreach (var details in obj_order_items)
                            {
                                tbl_sales_order_details objsaledetails = new tbl_sales_order_details();
                                objsaledetails.asin = details.ASIN;
                                objsaledetails.product_name = details.ProductName;
                                objsaledetails.sku_no = details.SKU;
                                if (details.Quantity != null && details.Quantity != "")
                                {
                                    objsaledetails.quantity_ordered = Convert.ToInt16(details.Quantity);
                                }
                                else
                                {
                                    objsaledetails.quantity_ordered = 0;
                                }

                                objsaledetails.promotion_ids = details.PromotionIDs;
                                if (details.ShipPromotionDiscount != null && details.ShipPromotionDiscount != "")
                                {
                                    objsaledetails.promotion_amount = Convert.ToDouble(details.ShipPromotionDiscount);
                                }
                                else
                                {
                                    objsaledetails.promotion_amount = 0;
                                }
                                if (details.itemprice != null)
                                {
                                    foreach (var itemprice in details.itemprice)
                                    {
                                        objsaledetails.item_price_amount = Convert.ToDouble(itemprice.pAmonu);
                                        objsaledetails.shipping_price_Amount = Convert.ToDouble(itemprice.shipAmonu);
                                        objsaledetails.giftwrapprice_amount = Convert.ToDouble(itemprice.giftwrapAmonu);
                                    }
                                }// end of if(details.itemprice)
                                objsaledetails.is_active = 1;
                                objsaledetails.tbl_seller_id = SellerId;
                                objsaledetails.tbl_sales_order_id = objsale.id;
                                objsaledetails.status_updated_by = SellerId;
                                objsaledetails.status_updated_on = DateTime.Now;
                                objsaledetails.n_order_status_id = Convert.ToInt16(objsale.n_item_orderstatus);
                                objsaledetails.amazon_order_id = objsale.amazon_order_id;
                                objsaledetails.dispatch_bydate = objsale.purchase_date;
                                objsaledetails.dispatchAfter_date = objsale.purchase_date;
                                dba.tbl_sales_order_details.Add(objsaledetails);
                                dba.SaveChanges();
                                //SalesObj_list.Add(objsaledetails);

                                //---------------------End------------------------------//



                                //---------------------Data save in History Table--------------//


                                //tbl_order_history objhistory = new tbl_order_history();
                                //objhistory.created_on = DateTime.Now;
                                //objhistory.tbl_orders_id = objsale.id;
                                //objhistory.tbl_seller_id = SellerId;
                                //objhistory.tbl_orderDetails_Id = objsaledetails.id;
                                //objhistory.ASIN = objsaledetails.asin;
                                //objhistory.SKU = objsaledetails.sku_no;
                                //objhistory.Quantity = objsaledetails.quantity_ordered;
                                //objhistory.t_order_status = objsale.n_item_orderstatus;

                                //objhistory.OrderID = objsale.amazon_order_id;
                                //objhistory.ShipmentDate = objsale.earliest_ship_date;
                                //objhistory.tbl_marketplace_id = objsale.tbl_Marketplace_Id;
                                //historyObj_list.Add(objhistory);
                            }// end of foreach loop(details)



                            //-------------------------------End----------------------------//

                        }// end of if(item.OrderDetails)
                    }// end of if(get_saleorder)


                }// end of if(objdata != null)


            }// end of try block
            catch (Exception ex)
            {
                datasaved = 0;
            }// end of catch block
            return datasaved;
        }
        [HttpPost]
        public ActionResult UploadOrderExcel(HttpPostedFileBase postedFile, FormCollection form)
        {
            long elapsedMs;
            long elapsedMs1, elapsedMs2;
            try
            {
                SellerId = Convert.ToInt32(Session["SellerID"]);
                cf = new comman_function();
                List<SelectListItem> lst1_loc = cf.GetMarketPlace(SellerId);
                ViewData["MarKetPlace"] = lst1_loc;
                //if (!cf.session_check())
                //    return RedirectToAction("Login", "Home");

                string strmarketplace = form["ddl_marketplace"].ToString();
                int marketplaceID = 0;
                if (strmarketplace != null && strmarketplace != "" && strmarketplace != "0")
                    marketplaceID = Convert.ToInt32(strmarketplace);
                else
                {
                    ViewBag.Message = "Select Marketplace";
                    return View("UploadSettlementExcel");
                }
                if (SellerId == 0)
                {
                    ViewBag.Message = "Please login again";
                    return View("UploadSettlementExcel");
                }
                if (postedFile != null)
                {
                    short sourcetype = 1;
                    string strFileName = Path.GetFileNameWithoutExtension(postedFile.FileName) + ".xml";
                    var watch = System.Diagnostics.Stopwatch.StartNew();
                    string strFolderPath = Server.MapPath("~/UploadExcel/" + SellerId + "/OrderSales/");
                    var get_orderupload_details = dba.tbl_order_upload.Where(a => a.tbl_seller_id == SellerId && a.filename == strFileName).FirstOrDefault();
                    if (get_orderupload_details == null)
                    {
                        if (!Directory.Exists(strFolderPath))
                        {
                            Directory.CreateDirectory(strFolderPath);
                        }

                        postedFile.SaveAs(strFolderPath + strFileName);
                        watch.Stop();
                        elapsedMs = watch.ElapsedMilliseconds;

                        watch = System.Diagnostics.Stopwatch.StartNew();
                        int current_running_no = 0;

                        var get_seller_setting = dba.tbl_seller_setting.Where(a => a.tbl_seller_id == SellerId && a.type == 3).FirstOrDefault();
                        if (get_seller_setting != null)
                        {
                            current_running_no = Convert.ToInt16(get_seller_setting.current_running_no);
                        }
                        tbl_order_upload obj_upload = new tbl_order_upload();
                        obj_upload.date_uploaded = DateTime.Now;
                        obj_upload.voucher_running_no = current_running_no;
                        obj_upload.filename = strFileName;
                        obj_upload.type = 1; //order xml
                        obj_upload.tbl_seller_id = SellerId;
                        obj_upload.tbl_Marketplace_id = marketplaceID;
                        obj_upload.source = sourcetype;
                        dba.tbl_order_upload.Add(obj_upload);
                        dba.SaveChanges();

                        xml_Parse(strFolderPath + strFileName, marketplaceID, obj_upload.id);

                        if (get_seller_setting != null)
                        {
                            current_running_no++;
                            get_seller_setting.current_running_no = current_running_no;
                            dba.Entry(get_seller_setting).State = EntityState.Modified;
                            dba.SaveChanges();

                        }
                        watch.Stop();
                        elapsedMs1 = watch.ElapsedMilliseconds;

                        watch = System.Diagnostics.Stopwatch.StartNew();
                        string success = "";
                        watch.Stop();
                        elapsedMs2 = watch.ElapsedMilliseconds;

                        ViewBag.Message = "File uploaded successfully.";
                    }
                    else
                    {
                        ViewBag.Message = "File Already Exist!";
                    }
                }
            }
            catch (Exception ex)
            {
                ViewBag.Message = "Unable to upload. " + ex.Message.ToString();
            }
            //RedirectToAction( "UploadSettlementExcel");
            return View("UploadSettlementExcel");
            //return RedirectToAction("UploadSettlementExcel");
        }

        public int xml_Parse(string strFilePath, int marketplaceID, int FileUploadId)
        {
            int SellerId = Convert.ToInt32(Session["SellerID"]);
            int current_running_no = 0;
            string startdate = "", enddate = "";
            XmlDocument contentxml = new XmlDocument();

            try
            {
                contentxml.Load(strFilePath);
                StringWriter sw = new StringWriter();
                XmlTextWriter xw = new XmlTextWriter(sw);
                contentxml.WriteTo(xw);
                string str = sw.ToString();
                //Load the XML file in XmlDocument.
                XmlDocument doc = new XmlDocument();
                doc.LoadXml(str);
                string a = doc.NamespaceURI;
                //Loop through the selected Nodes.
                XmlNodeList xnList = doc.SelectNodes("/AmazonEnvelope/Message/Order");
                decimal orderbillamount;

                dba.Configuration.AutoDetectChangesEnabled = false;

                historyObj_list = new List<tbl_order_history>();
                SalesObj_list = new List<tbl_sales_order_details>();
                var getdetails = dba.tbl_sales_order_status.Where(aa => aa.is_active == 0).ToList();
                Dictionary<string, int> orderstatus_dict = new Dictionary<string, int>();
                foreach (var item1 in getdetails)
                {
                    string name = item1.sales_order_status.ToLower();
                    orderstatus_dict.Add(name, item1.id);
                }
                int kk = 0;
                //int count = xnList.Count;
                //xmlNode["name"].InnerText;
                //var first = xnList[0].(SelectSingleNode("PurchaseDate").InnerText == null ? "" : ["PurchaseDate"].InnerText)); //first element
                //var last = xnList[xnList.Count - 1]; //last element
                foreach (XmlNode xn in xnList)
                {
                    if( kk == 0)
                    {
                        startdate = Convert.ToString(xn.SelectSingleNode("PurchaseDate").InnerText == null ? "" : xn["PurchaseDate"].InnerText);
                    }
                    if( kk == xnList.Count -1)
                    {
                        enddate = Convert.ToString(xn.SelectSingleNode("PurchaseDate").InnerText == null ? "" : xn["PurchaseDate"].InnerText);
                    }


                    kk++;

                    XmlNode anode = xn;
                    if (anode != null)
                    {                     
                        XmlNode addre = anode.SelectSingleNode("FulfillmentData");
                        if (addre != null)
                        {
                            XmlNode addres = addre.SelectSingleNode("Address");
                            if (addres != null)
                            {
                                XmlNodeList order = anode.SelectNodes("OrderItem");//get order details 
                                if (order != null)
                                {
                                    List<OrderItem> obj1 = new List<OrderItem>();
                                    string abc = "";
                                    string xyz = "";
                                    string asin = "";
                                    string sku = "";
                                    string itemstatus = "";
                                    string productname = "";
                                    string quantity = "";
                                    orderbillamount = 0; //sharad
                                    foreach (XmlNode aa in order)
                                    {
                                        if (aa != null)
                                        {
                                            if (aa.SelectSingleNode("ASIN") != null)
                                            {
                                                asin = aa.SelectSingleNode("ASIN").InnerText == null ? "" : aa["ASIN"].InnerText;
                                            }
                                            if (aa.SelectSingleNode("SKU") != null)
                                            {
                                                sku = aa.SelectSingleNode("SKU").InnerText == null ? "" : aa["SKU"].InnerText;
                                            }
                                            if (aa.SelectSingleNode("ItemStatus") != null)
                                            {
                                                itemstatus = aa.SelectSingleNode("ItemStatus").InnerText == null ? "" : aa["ItemStatus"].InnerText;
                                            }
                                            if (aa.SelectSingleNode("ProductName") != null)
                                            {
                                                productname = aa.SelectSingleNode("ProductName").InnerText == null ? "" : aa["ProductName"].InnerText;
                                            }
                                            if (aa.SelectSingleNode("Quantity") != null)
                                            {
                                                quantity = aa.SelectSingleNode("Quantity").InnerText == null ? "" : aa["Quantity"].InnerText;
                                            }
                                            XmlNode prom = aa.SelectSingleNode("Promotion");//get promotion 
                                            if (prom != null)
                                            {
                                                if (prom.SelectSingleNode("PromotionIDs") != null)
                                                {
                                                    abc = prom.SelectSingleNode("PromotionIDs").InnerText == null ? "" : prom["PromotionIDs"].InnerText;
                                                }
                                                if (prom.SelectSingleNode("ShipPromotionDiscount") != null)
                                                {

                                                    xyz = prom.SelectSingleNode("ShipPromotionDiscount").InnerText == null ? "" : prom["ShipPromotionDiscount"].InnerText;
                                                }
                                            }
                                            //////////// ////changes in item price////////////////////////////////////////////
                                            XmlNodeList itempriceslist = aa.SelectNodes("ItemPrice/Component");
                                            List<ItemPrice> objitemprice = new List<ItemPrice>();
                                            if (itempriceslist != null)
                                            {
                                                string type = "";
                                                string amount = "";
                                                string shipamount = "";
                                                string giftwrap = "";
                                                string taxamount = "";
                                                foreach (XmlNode pricelist in itempriceslist)
                                                {
                                                    if (pricelist != null)
                                                    {
                                                        if (pricelist.SelectSingleNode("Type") != null)
                                                        {
                                                            type = pricelist.SelectSingleNode("Type").InnerText == null ? "" : pricelist["Type"].InnerText;
                                                            if (type == "Principal")
                                                            {
                                                                if (pricelist.SelectSingleNode("Amount") != null)
                                                                {
                                                                    amount = pricelist.SelectSingleNode("Amount").InnerText == null ? "" : pricelist["Amount"].InnerText;
                                                                }
                                                            }
                                                            else
                                                                if (type == "Shipping")
                                                                {
                                                                    if (pricelist.SelectSingleNode("Amount") != null)
                                                                    {
                                                                        shipamount = pricelist.SelectSingleNode("Amount").InnerText == null ? "" : pricelist["Amount"].InnerText;
                                                                    }
                                                                }
                                                                else
                                                                    if (type == "GiftWrap")
                                                                    {
                                                                        if (pricelist.SelectSingleNode("Amount") != null)
                                                                        {
                                                                            giftwrap = pricelist.SelectSingleNode("Amount").InnerText == null ? "" : pricelist["Amount"].InnerText;
                                                                        }
                                                                    }
                                                                    else
                                                                        if (type == "Tax")
                                                                        {
                                                                            if (pricelist.SelectSingleNode("Amount") != null)
                                                                            {
                                                                                //taxamount = pricelist.SelectSingleNode("Amount").InnerText == null ? "" : pricelist["Amount"].InnerText;
                                                                            }
                                                                        }
                                                        }
                                                    }
                                                }//End of For each loop pricelist 
                                                if (shipamount == "")
                                                    shipamount = "0";
                                                if (amount == "")
                                                    amount = "0";
                                                if (giftwrap == "")
                                                    giftwrap = "0";
                                                //if (taxamount == "")
                                                    //taxamount = "0";
                                                objitemprice.Add(new ItemPrice
                                                {
                                                    //Type = type,
                                                    pAmonu = Convert.ToDecimal(amount),
                                                    shipAmonu = Convert.ToDecimal(shipamount),
                                                    giftwrapAmonu = Convert.ToDecimal(giftwrap),
                                                    //ptaxamount = Convert.ToDecimal(taxamount)


                                                });
                                                orderbillamount += Convert.ToDecimal(amount) + Convert.ToDecimal(shipamount) + Convert.ToDecimal(giftwrap);
                                            }
                                            /////////////////////////End changes in item price///////////////////////////
                                            obj1.Add(new OrderItem
                                            {
                                                ASIN = asin,
                                                SKU = sku,
                                                ItemStatus = itemstatus,
                                                ProductName = productname,
                                                Quantity = quantity,

                                                PromotionIDs = abc,
                                                ShipPromotionDiscount = xyz,
                                                itemprice = objitemprice
                                            });
                                        }//End
                                    }

                                    tbl_sales_order obj = new tbl_sales_order();
                                    obj.amazon_order_id = (anode.SelectSingleNode("AmazonOrderID") == null) ? "" : anode["AmazonOrderID"].InnerText;
                                    obj.purchase_date = Convert.ToDateTime(anode.SelectSingleNode("PurchaseDate").InnerText == null ? "" : anode["PurchaseDate"].InnerText);
                                    obj.last_updated_date = Convert.ToDateTime(anode.SelectSingleNode("LastUpdatedDate").InnerText == null ? "" : anode["LastUpdatedDate"].InnerText);
                                    obj.order_status = anode.SelectSingleNode("OrderStatus").InnerText == null ? "" : anode["OrderStatus"].InnerText;
                                    obj.sales_channel = anode.SelectSingleNode("SalesChannel").InnerText == null ? "" : anode["SalesChannel"].InnerText;
                                    obj.is_business_order = anode.SelectSingleNode("IsBusinessOrder").InnerText == null ? "" : anode["IsBusinessOrder"].InnerText;
                                    obj.fullfillment_channel = addre.SelectSingleNode("FulfillmentChannel").InnerText == null ? "" : addre["FulfillmentChannel"].InnerText;
                                    obj.m_ship_service_level_id = addre.SelectSingleNode("ShipServiceLevel").InnerText == null ? "" : addre["ShipServiceLevel"].InnerText;

                                    obj.ship_city = addres.SelectSingleNode("City").InnerText == null ? "" : addres["City"].InnerText;
                                    if (addres.SelectSingleNode("State")!= null)
                                    {
                                        obj.ship_state = addres.SelectSingleNode("State").InnerText == null ? "" : addres["State"].InnerText;
                                    }
                                    obj.ship_postal_code = addres.SelectSingleNode("PostalCode").InnerText == null ? "" : addres["PostalCode"].InnerText;
                                    obj.ship_country = addres.SelectSingleNode("Country").InnerText == null ? "" : addres["Country"].InnerText;

                                    obj.bill_amount = Convert.ToDouble(orderbillamount);
                                    obj.created_on = DateTime.Now;
                                    obj.tbl_sellers_id = SellerId;
                                    obj.is_active = 1;
                                    obj.tbl_Marketplace_Id = marketplaceID;
                                    obj.tbl_order_upload_id = FileUploadId;
                                    if (obj.fullfillment_channel == "Amazon")
                                    {
                                        obj.n_fullfilled_id = 1;
                                    }
                                    else
                                    {
                                        obj.n_fullfilled_id = 2;
                                    }
                                    string name1 = obj.order_status.ToLower();
                                    if (orderstatus_dict.ContainsKey(name1))
                                    {
                                        obj.n_item_orderstatus = orderstatus_dict[name1];
                                    }

                                    var get_saleorder = dba.tbl_sales_order.Where(aa => aa.tbl_sellers_id == SellerId && aa.amazon_order_id == obj.amazon_order_id && aa.n_item_orderstatus == obj.n_item_orderstatus).FirstOrDefault();
                                    if (get_saleorder == null)
                                    {
                                        dba.tbl_sales_order.Add(obj);
                                        dba.SaveChanges();

                                        if (obj1 != null)
                                        {
                                            //double OrderBillAmount = 0;

                                            foreach (var details in obj1)
                                            {
                                                var chkSku = dba.tbl_inventory.Where(aa => aa.sku == details.SKU).FirstOrDefault();
                                                if (chkSku == null)
                                                {
                                                    tbl_inventory objInventory = new tbl_inventory();
                                                    objInventory.sku = details.SKU;
                                                    objInventory.tbl_sellers_id = SellerId;
                                                    objInventory.tbl_item_category_id = 19;
                                                    objInventory.tbl_item_subcategory_id = 14;
                                                    objInventory.item_name = details.ProductName;
                                                    objInventory.isactive = 1;
                                                    if (details.itemprice != null)
                                                    {
                                                        foreach (var itemprice in details.itemprice)
                                                        {
                                                           objInventory.selling_price = Convert.ToInt16(itemprice.pAmonu);                                                           
                                                        }
                                                    }                                                                                          
                                                    dba.tbl_inventory.Add(objInventory);
                                                    dba.SaveChanges();
                                                }

                                                tbl_sales_order_details objsaledetails = new tbl_sales_order_details();
                                                objsaledetails.asin = details.ASIN;
                                                objsaledetails.product_name = details.ProductName;
                                                objsaledetails.sku_no = details.SKU;
                                                if (details.Quantity != null && details.Quantity != "")
                                                {
                                                    objsaledetails.quantity_ordered = Convert.ToInt16(details.Quantity);
                                                }
                                                else
                                                {
                                                    objsaledetails.quantity_ordered = 0;
                                                }

                                                objsaledetails.promotion_ids = details.PromotionIDs;
                                                if (details.ShipPromotionDiscount != null && details.ShipPromotionDiscount != "")
                                                {
                                                    objsaledetails.promotion_amount = Convert.ToDouble(details.ShipPromotionDiscount);
                                                }
                                                else
                                                {
                                                    objsaledetails.promotion_amount = 0;
                                                }
                                                if (details.itemprice != null)
                                                {
                                                    foreach (var itemprice in details.itemprice)
                                                    {
                                                        objsaledetails.item_price_amount = Convert.ToDouble(itemprice.pAmonu);
                                                        objsaledetails.shipping_price_Amount = Convert.ToDouble(itemprice.shipAmonu);
                                                        objsaledetails.giftwrapprice_amount = Convert.ToDouble(itemprice.giftwrapAmonu);
                                                        //objsaledetails.item_tax_amount = Convert.ToDouble(itemprice.ptaxamount);
                                                    }
                                                }// end of if(details.itemprice)
                                                objsaledetails.is_active = 1;
                                                objsaledetails.tbl_seller_id = SellerId;
                                                objsaledetails.tbl_sales_order_id = obj.id;
                                                objsaledetails.status_updated_by = SellerId;
                                                objsaledetails.status_updated_on = DateTime.Now;
                                                objsaledetails.n_order_status_id = Convert.ToInt16(obj.n_item_orderstatus);
                                                objsaledetails.amazon_order_id = obj.amazon_order_id;
                                                objsaledetails.dispatch_bydate = obj.purchase_date;
                                                objsaledetails.dispatchAfter_date = obj.purchase_date;
                                                SalesObj_list.Add(objsaledetails);
                                                //dba.tbl_sales_order_details.Add(objsaledetails);
                                                //dba.SaveChanges();

                                            }// end of foreach loop(details)
                                        }// end of if(item.OrderDetails)
                                    }// end of if(get_saleorder)                               
                                }
                            }
                        }
                    }// end of if(anode!=null)
                    else //if (anode != null)
                    {
                        int jjj = 0;
                        jjj++;
                    }
                    // saveXMLData(customers);
                }// end of foreach loop xnList



                int i = 0;
                foreach (var detailObj in SalesObj_list)
                {
                    i++;
                    dba.tbl_sales_order_details.Add(detailObj);
                    if (i == 100)
                    {
                        i = 0;
                        dba.SaveChanges();
                    }
                }
           
                string connectionstring = ConfigurationManager.ConnectionStrings["SellerConnection"].ConnectionString;
                MySqlConnection con = new MySqlConnection(connectionstring);

                int uniqueupload = 0;
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

                var get_balance_details = db.tbl_user_login.Where(aa => aa.tbl_sellers_id == SellerId).FirstOrDefault();
                if (get_balance_details != null)
                {
                    var last_order = get_balance_details.total_orders;
                    var diff_order = uniqueupload - last_order;
                    var plan_rate = get_balance_details.applied_plan_rate * diff_order;
                    get_balance_details.wallet_balance = get_balance_details.wallet_balance - Convert.ToDouble(plan_rate);
                    int totalOrder = Convert.ToInt16(diff_order);
                    if (get_balance_details.total_orders != null)
                        totalOrder = totalOrder + Convert.ToInt16(get_balance_details.total_orders);
                    get_balance_details.total_orders = totalOrder;
                   
                    db.Entry(get_balance_details).State = EntityState.Modified;
                    db.SaveChangesAsync();

                    Session["WalletBalance"] = get_balance_details.wallet_balance.ToString();
                    Session["TotalOrders"] = totalOrder.ToString();
                }
                var get_upload_settlement_details = dba.tbl_order_upload.Where(aa => aa.tbl_seller_id == SellerId && aa.id == FileUploadId).FirstOrDefault();
                if (get_upload_settlement_details != null)
                {
                    get_upload_settlement_details.new_order_uploaded = uniqueupload;
                    get_upload_settlement_details.from_date = Convert.ToDateTime(startdate);
                    get_upload_settlement_details.to_date = Convert.ToDateTime(enddate);

                    dba.Entry(get_upload_settlement_details).State = EntityState.Modified;

                }
                dba.SaveChangesAsync();
                
            }// end of try
            catch (Exception ex)
            {

            }

            return current_running_no;
        }

        //---------------------------------End----------------------





        /// <summary>
        /// This is for Physically Goods Return
        /// </summary>
        /// <returns></returns>
        public ActionResult ReturnGoods(FormCollection form, int? ddl_market_place, int? ddl_eftcod, DateTime? txt_from, DateTime? txt_to, int? ddl_export)
        {
            cf = new comman_function();
            if (!cf.session_check())
                return RedirectToAction("Login", "Home");
            int sellers_id = Convert.ToInt32(Session["SellerID"]);
            ViewData["ExportList"] = cf.GetExcelExportList();
            ViewData["FullfilmentList"] = cf.GetFullilmentList();
            List<SelectListItem> lst1_loc = cf.GetMarketPlace(sellers_id);
            ViewData["MarKetPlace"] = lst1_loc;
            List<partial_tbl_order_history> obj = new List<partial_tbl_order_history>();
            string MarketPlace = "";

            try
            {
                if (txt_from != null && txt_to != null)
                {
                    var get_details = dba.tbl_order_history.Where(a => a.tbl_seller_id == sellers_id && a.t_order_status == 9 && a.tbl_marketplace_id == ddl_market_place).ToList();
                    string xyz = "";
                    if (ddl_eftcod != null)
                    {
                        if (ddl_eftcod == 1)
                        {
                            xyz = "MFN";
                        }
                        else if (ddl_eftcod == 2)
                        {
                            xyz = "AFN";
                        }
                        else
                        {
                            xyz = "Both";
                        }
                    }

                    if (get_details != null && xyz != null)
                    {
                        if (xyz == "Both")
                        {
                            get_details = get_details.ToList();
                        }
                        else
                        {
                            get_details = get_details.Where(a => a.fulfillment_id == xyz).ToList();
                        }
                    }
                    if (get_details != null && ddl_market_place != null && ddl_market_place != 0)
                    {
                        //get_details = get_details.Where(a => a.tbl_marketplace_id == ddl_market_place).ToList();
                        MarketPlace = lst1_loc.Where(p => p.Value == ddl_market_place.ToString()).First().Text;
                    }
                    if (get_details != null && txt_from != null && txt_to != null)
                    {
                        get_details = get_details.Where(a => Convert.ToDateTime(a.ShipmentDate).Date >= txt_from && Convert.ToDateTime(a.ShipmentDate).Date <= txt_to).ToList();
                    }

                    foreach (var item in get_details)
                    {
                        double refundFBAFEE = 0, refundFBACGST = 0, refundFBASGST = 0, refundFBAIGST = 0, refundTechnologyFee = 0, refundTechnologyIGST = 0, refundTechnologyCGST = 0, refundTechnologySGST = 0, refundCommissionFee = 0, refundCommissionIGST = 0, refundCommissionCGST = 0,
                                                       refundCommissionSGST = 0, refundFixedClosingFee = 0, refundFixedclosingIGST = 0,
                                                       refundFixedclosingCGST = 0, refundFixedclosingSGST = 0, refundShippingChargebackFee = 0,
                                                       refundshippingchargeCGST = 0, refundshippingchargeSGST = 0, refundshippingchargeIGST = 0, refundToatal=0;


                        double RefundFBAFEE = 0, RefundFBACGST = 0, RefundFBASGST = 0, RefundFBAIGST = 0, RefundTechnologyFee = 0, RefundTechnologyIGST = 0, RefundTechnologyCGST = 0,
                               RefundTechnologySGST = 0, RefundCommissionFee = 0, RefundCommissionIGST = 0, RefundCommissionCGST = 0, RefundCommissionSGST = 0,
                               RefundFixedClosingFee = 0, RefundFixedclosingIGST = 0, RefundFixedclosingCGST = 0, RefundFixedclosingSGST = 0,
                               RefundShippingChargebackFee = 0, RefundshippingchargeIGST = 0, RefundshippingchargeCGST = 0, RefundshippingchargeSGST = 0;

                        double FBAFEE = 0, FBACGST = 0, FBASGST = 0, FBAIGST = 0, TechnologyFee = 0, TechnologyIGST = 0, TechnologyCGST = 0,TechnologySGST = 0,CommissionFee = 0,CommissionIGST = 0,CommissionCGST = 0,
                                                       CommissionSGST = 0, FixedClosingFee = 0, FixedclosingIGST = 0,
                                                       FixedclosingCGST = 0, FixedclosingSGST = 0, ShippingChargebackFee = 0,
                                                       shippingchargeCGST = 0, shippingchargeSGST = 0, shippingchargeIGST = 0;


                        double OrderFBAFEE = 0, OrderFBACGST = 0, OrderFBASGST = 0, OrderFBAIGST = 0, OrderTechnologyFee = 0, OrderTechnologyIGST = 0, OrderTechnologyCGST = 0,
                               OrderTechnologySGST = 0, OrderCommissionFee = 0, OrderCommissionIGST = 0, OrderCommissionCGST = 0, OrderCommissionSGST = 0,
                               OrderFixedClosingFee = 0, OrderFixedclosingIGST = 0, OrderFixedclosingCGST = 0, OrderFixedclosingSGST = 0,
                               OrderShippingChargebackFee = 0, OrdershippingchargeIGST = 0, OrdershippingchargeCGST = 0, OrdershippingchargeSGST = 0, orderToatal = 0;

                        partial_tbl_order_history objtbl = new partial_tbl_order_history();
                        objtbl.returngoods = item.physically_type;
                        objtbl.sku = item.SKU;
                        objtbl.physicallyreturn = Convert.ToString(item.physically_type);
                        objtbl.condition = Convert.ToString(item.condition_type);
                        objtbl.OrderDate = Convert.ToDateTime(item.ShipmentDate).ToString("yyyy-MM-dd");
                        objtbl.MarketPlaceName = MarketPlace;
                        if (item.amount_per_unit < 0)
                        {
                            objtbl.principalvalue = Convert.ToDouble(item.amount_per_unit + (item.shipping_price.HasValue ? item.shipping_price : 0)) * (-1);
                        }
                        else
                        {
                            objtbl.principalvalue = Convert.ToDouble(item.amount_per_unit + (item.shipping_price.HasValue ? item.shipping_price : 0));
                        }

                        objtbl.IGST = Convert.ToDouble((item.product_tax.HasValue? item.product_tax:0) + (item.shipping_tax.HasValue ? item.shipping_tax:0)) * (-1);

                        objtbl.TotalValue = objtbl.principalvalue + objtbl.IGST;
                        objtbl.ReturnType = "";
                        objtbl.ConditionType = "";
                        if (objtbl.physicallyreturn == "1")
                        {
                            objtbl.ReturnType = "Yes";
                        }
                        else if (objtbl.physicallyreturn == "2")
                        {
                            objtbl.ReturnType = "No";
                        }
                        if (objtbl.condition == "1")
                        {
                            objtbl.ConditionType = "Good";
                        }
                        else if (objtbl.condition == "2")
                        {
                            objtbl.ConditionType = "Bad";
                        }
                        if (item.physically_selected_date != null)
                        {
                            objtbl.PhysicallyDate = Convert.ToDateTime(item.physically_selected_date).ToString("yyyy-MM-dd");
                        }                       
                        objtbl.id = item.Id;
                        string orderid= item.OrderID;
                        objtbl.order_id = item.OrderID;
                        objtbl.fullfillment_id = item.fulfillment_id;


                        if(orderid == "4902943566")
                        {

                        }
                        var get_expense = dba.m_tbl_expense.Where(a => a.tbl_seller_id == sellers_id && a.Original_order_id == orderid && a.t_transactionType_id == 2 && a.sku_no == item.SKU).ToList();
                        var get_expenseorder = dba.m_tbl_expense.Where(a => a.tbl_seller_id == sellers_id && a.Original_order_id == orderid && a.t_transactionType_id == 1 && a.sku_no == item.SKU).ToList();
                        if (ddl_market_place == 5)
                        {
                            #region refund                  
                            if (get_expense != null && get_expense.Count > 0)
                            {
                                foreach (var refund in get_expense)
                                {
                                    var exp_ID = refund.expense_type_id;
                                    var get_detailss = dba.m_settlement_fee.Where(a => a.id == exp_ID).FirstOrDefault();
                                    var getExp_tax_details = dba.tbl_tax.Where(a => a.tbl_referneced_id == refund.id && a.reference_type == 7).FirstOrDefault();
                                    if (get_detailss != null)
                                    {
                                        string nam = get_detailss.return_fee;
                                        if (nam == "Marketplace Commission")
                                        {
                                            if (get_detailss.id == exp_ID)
                                            {
                                                refundFBAFEE = refundFBAFEE + Convert.ToDouble(refund.expense_amount);
                                                RefundFBAFEE = refundFBAFEE;
                                                if (getExp_tax_details != null)
                                                {
                                                    refundFBAIGST = refundFBAIGST + Convert.ToDouble(getExp_tax_details.Igst_amount);
                                                    refundFBACGST = refundFBACGST + Convert.ToDouble(getExp_tax_details.CGST_amount);
                                                    refundFBASGST = refundFBASGST + Convert.ToDouble(getExp_tax_details.sgst_amount);

                                                    RefundFBACGST = refundFBACGST;
                                                    RefundFBASGST = refundFBASGST;
                                                    RefundFBAIGST = refundFBAIGST;
                                                }
                                            }
                                        }
                                        else if (nam == "Logistics Charges")
                                        {
                                            if (get_detailss.id == exp_ID)
                                            {
                                                refundTechnologyFee = refundTechnologyFee + Convert.ToDouble(refund.expense_amount);
                                                RefundTechnologyFee = refundTechnologyFee;
                                                if (getExp_tax_details != null)
                                                {
                                                    refundTechnologyIGST = refundTechnologyIGST + Convert.ToDouble(getExp_tax_details.Igst_amount);
                                                    refundTechnologyCGST = refundTechnologyCGST + Convert.ToDouble(getExp_tax_details.CGST_amount);
                                                    refundTechnologySGST = refundTechnologySGST + Convert.ToDouble(getExp_tax_details.sgst_amount);

                                                    RefundTechnologyIGST = refundTechnologyIGST;
                                                    RefundTechnologyCGST = refundTechnologyCGST;
                                                    RefundTechnologySGST = refundTechnologySGST;
                                                }
                                            }
                                        }
                                        else if (nam == "PG Commission")
                                        {
                                            if (get_detailss.id == exp_ID)
                                            {
                                                refundCommissionFee = refundCommissionFee + Convert.ToDouble(refund.expense_amount);
                                                RefundCommissionFee = refundCommissionFee;
                                                if (getExp_tax_details != null)
                                                {
                                                    refundCommissionIGST = refundCommissionIGST + Convert.ToDouble(getExp_tax_details.Igst_amount);
                                                    refundCommissionCGST = refundCommissionCGST + Convert.ToDouble(getExp_tax_details.CGST_amount);
                                                    refundCommissionSGST = refundCommissionSGST + Convert.ToDouble(getExp_tax_details.sgst_amount);

                                                    RefundCommissionIGST = refundCommissionIGST;
                                                    RefundCommissionCGST = refundCommissionCGST;
                                                    RefundCommissionSGST = refundCommissionSGST;
                                                }
                                            }
                                        }
                                        else if (nam == "Penalty")
                                        {
                                            if (get_detailss.id == exp_ID)
                                            {
                                                refundFixedClosingFee = refundFixedClosingFee + Convert.ToDouble(refund.expense_amount)*(-1);
                                                RefundFixedClosingFee = refundFixedClosingFee;
                                                if (getExp_tax_details != null)
                                                {
                                                    refundFixedclosingIGST = refundFixedclosingIGST + Convert.ToDouble(getExp_tax_details.Igst_amount) * (-1);
                                                    refundFixedclosingCGST = refundFixedclosingCGST + Convert.ToDouble(getExp_tax_details.CGST_amount) * (-1);
                                                    refundFixedclosingSGST = refundFixedclosingSGST + Convert.ToDouble(getExp_tax_details.sgst_amount) * (-1);

                                                    RefundFixedclosingIGST = refundFixedclosingIGST;
                                                    RefundFixedclosingCGST = refundFixedclosingCGST;
                                                    RefundFixedclosingSGST = refundFixedclosingSGST;
                                                }
                                            }
                                        }
                                        else if (nam == "Net Adjustments")
                                        {
                                            if (get_detailss.id == exp_ID)
                                            {
                                                refundShippingChargebackFee = refundShippingChargebackFee + Convert.ToDouble(refund.expense_amount);
                                               RefundShippingChargebackFee = refundShippingChargebackFee;
                                                if (getExp_tax_details != null)
                                                {
                                                    refundshippingchargeIGST = refundshippingchargeIGST + Convert.ToDouble(getExp_tax_details.Igst_amount);
                                                    refundshippingchargeCGST = refundshippingchargeCGST + Convert.ToDouble(getExp_tax_details.CGST_amount);
                                                    refundshippingchargeSGST = refundshippingchargeSGST + Convert.ToDouble(getExp_tax_details.sgst_amount);

                                                    RefundshippingchargeCGST = refundshippingchargeCGST;
                                                    RefundshippingchargeSGST = refundshippingchargeSGST;
                                                    RefundshippingchargeIGST = refundshippingchargeIGST;
                                                }
                                            }
                                        }

                                       

                                       
                                    }
                                }// end of for each loop
                                double totalexpval = RefundFBAFEE + RefundTechnologyFee + RefundCommissionFee + RefundFixedClosingFee + RefundShippingChargebackFee;
                                double totaltax_val = RefundFBACGST + RefundFBACGST + RefundFBAIGST + RefundTechnologyIGST + RefundTechnologyCGST + RefundTechnologySGST + RefundCommissionIGST
                                                      + RefundCommissionSGST + RefundCommissionCGST + RefundFixedclosingIGST + RefundFixedclosingSGST + RefundFixedclosingCGST +
                                                      RefundshippingchargeCGST + RefundshippingchargeSGST + RefundshippingchargeIGST;

                                refundToatal = totalexpval + totaltax_val;
                            }// end of if get expense
                            #endregion

                            #region order
                            if (get_expenseorder != null && get_expenseorder.Count > 0)
                            {
                                foreach (var order in get_expenseorder)
                                {
                                    var exp_ID = order.expense_type_id;
                                    var get_detailss = dba.m_settlement_fee.Where(a => a.id == exp_ID).FirstOrDefault();
                                    var getExp_tax_details = dba.tbl_tax.Where(a => a.tbl_referneced_id == order.id && a.reference_type == 2).FirstOrDefault();
                                    if (get_detailss != null)
                                    {
                                        string nam = get_detailss.return_fee;
                                        if (nam == "Marketplace Commission")
                                        {
                                            if (get_detailss.id == exp_ID)
                                            {
                                                FBAFEE = FBAFEE + Convert.ToDouble(order.expense_amount)*(-1);
                                                OrderFBAFEE = FBAFEE;
                                                if (getExp_tax_details != null)
                                                {
                                                    FBAIGST = FBAIGST + Convert.ToDouble(getExp_tax_details.Igst_amount) * (-1);
                                                    FBACGST = FBACGST + Convert.ToDouble(getExp_tax_details.CGST_amount) * (-1);
                                                    FBASGST = FBASGST + Convert.ToDouble(getExp_tax_details.sgst_amount) * (-1);

                                                    OrderFBACGST = FBACGST;
                                                    OrderFBASGST = FBASGST;
                                                    OrderFBAIGST = FBAIGST;
                                                }
                                            }
                                        }
                                        else if (nam == "Logistics Charges")
                                        {
                                            if (get_detailss.id == exp_ID)
                                            {
                                                TechnologyFee = TechnologyFee + Convert.ToDouble(order.expense_amount) * (-1);
                                                OrderTechnologyFee = TechnologyFee;
                                                if (getExp_tax_details != null)
                                                {
                                                    TechnologyIGST = TechnologyIGST + Convert.ToDouble(getExp_tax_details.Igst_amount) * (-1);
                                                    TechnologyCGST = TechnologyCGST + Convert.ToDouble(getExp_tax_details.CGST_amount) * (-1);
                                                    TechnologySGST = TechnologySGST + Convert.ToDouble(getExp_tax_details.sgst_amount) * (-1);

                                                    OrderTechnologyIGST = TechnologyIGST;
                                                    OrderTechnologyCGST = TechnologyCGST;
                                                    OrderTechnologySGST = TechnologySGST;
                                                }
                                            }
                                        }
                                        else if (nam == "PG Commission")
                                        {
                                            if (get_detailss.id == exp_ID)
                                            {
                                                CommissionFee = CommissionFee + Convert.ToDouble(order.expense_amount) * (-1);
                                                OrderCommissionFee = CommissionFee;
                                                if (getExp_tax_details != null)
                                                {
                                                    CommissionIGST = CommissionIGST + Convert.ToDouble(getExp_tax_details.Igst_amount) * (-1);
                                                    CommissionCGST = CommissionCGST + Convert.ToDouble(getExp_tax_details.CGST_amount) * (-1);
                                                    CommissionSGST = CommissionSGST + Convert.ToDouble(getExp_tax_details.sgst_amount) * (-1);

                                                    OrderCommissionIGST = CommissionIGST;
                                                    OrderCommissionCGST = CommissionCGST;
                                                    OrderCommissionSGST = CommissionSGST;
                                                }
                                            }
                                        }
                                        else if (nam == "Penalty")
                                        {
                                            if (get_detailss.id == exp_ID)
                                            {
                                                FixedClosingFee = FixedClosingFee + Convert.ToDouble(order.expense_amount) * (-1);
                                                OrderFixedClosingFee = FixedClosingFee;
                                                if (getExp_tax_details != null)
                                                {
                                                    FixedclosingIGST = FixedclosingIGST + Convert.ToDouble(getExp_tax_details.Igst_amount) ;
                                                    FixedclosingCGST = FixedclosingCGST + Convert.ToDouble(getExp_tax_details.CGST_amount) ;
                                                    FixedclosingSGST = FixedclosingSGST + Convert.ToDouble(getExp_tax_details.sgst_amount) ;

                                                    OrderFixedclosingIGST = FixedclosingIGST;
                                                    OrderFixedclosingCGST = FixedclosingCGST;
                                                    OrderFixedclosingSGST = FixedclosingSGST;
                                                }
                                            }
                                        }
                                        else if (nam == "Net Adjustments")
                                        {
                                            if (get_detailss.id == exp_ID)
                                            {
                                                ShippingChargebackFee = ShippingChargebackFee + Convert.ToDouble(order.expense_amount) * (-1);
                                                OrderShippingChargebackFee = ShippingChargebackFee;
                                                if (getExp_tax_details != null)
                                                {
                                                    shippingchargeIGST = shippingchargeIGST + Convert.ToDouble(getExp_tax_details.Igst_amount) ;
                                                    shippingchargeCGST = shippingchargeCGST + Convert.ToDouble(getExp_tax_details.CGST_amount) ;
                                                    shippingchargeSGST = shippingchargeSGST + Convert.ToDouble(getExp_tax_details.sgst_amount) ;

                                                    OrdershippingchargeCGST = shippingchargeCGST;
                                                    OrdershippingchargeSGST = shippingchargeSGST;
                                                    OrdershippingchargeIGST = shippingchargeIGST;
                                                }
                                            }
                                        }
                                    }
                                   
                                }
                                double totalexpval = OrderFBAFEE + OrderTechnologyFee + OrderCommissionFee + OrderFixedClosingFee + OrderShippingChargebackFee;
                                double totaltax_val = OrderFBACGST + OrderFBACGST + OrderFBAIGST + OrderTechnologyIGST + OrderTechnologyCGST + OrderTechnologySGST + OrderCommissionIGST
                                                      + OrderCommissionSGST + OrderCommissionCGST + OrderFixedclosingIGST + OrderFixedclosingSGST + OrderFixedclosingCGST +
                                                      OrdershippingchargeCGST + OrdershippingchargeSGST + OrdershippingchargeIGST;

                                orderToatal = totalexpval + totaltax_val;
                            }
                            #endregion

                            objtbl.NetRealization = orderToatal - refundToatal;
                       }// end of if
                        obj.Add(objtbl);

                    }
                    /////----------------------------------Export Excel----------------------------------
                    string sendOnMail = form["sendmail"];
                    string value1 = form["command"];
                    if (value1 == "Export" || sendOnMail == "Send On Mail")
                    {
                        if (ddl_export != null)
                        {
                            bool mail = false;
                            if (sendOnMail != null) mail = true;
                            string EmailID = form["txt_email"];

                            DataTable header = new DataTable();
                            string colum = " ";
                            for (int t = 0; t < 6; t++)
                            {
                                header.Columns.Add(colum);
                                colum = colum + " ";
                            }
                            DataRow errow = header.NewRow();
                            errow[2] = " ";
                            header.Rows.Add(errow);
                            DataRow drow = header.NewRow();
                            drow[1] = "From Date";
                            drow[2] = txt_from;
                            drow[4] = "Report Taken On";
                            drow[5] = DateTime.Now.ToString("dd/MMM/yyyy");
                            header.Rows.Add(drow);
                            DataRow brow = header.NewRow();
                            brow[1] = "To Date";
                            brow[2] = txt_to;

                            header.Rows.Add(brow);
                            DataRow erow = header.NewRow();
                            erow[2] = " ";
                            header.Rows.Add(erow);
                            DataTable export_dt = new DataTable();
                            export_dt = cf.CreateReturnDatatable(obj);
                            int type = Convert.ToInt32(ddl_export);
                            if (export_dt.Rows.Count > 0)
                            {
                                int r = cf.Export("Return Oreder Report", header, export_dt, Response, type, mail, EmailID);
                            }
                            else ViewData["Message"] = "There are No records In Table";
                        }
                        else ViewData["Message"] = "Please Select Export Type";
                    }
                }// end of if()
                //----------------------------------------END-----------------------------------------////
            }// end of try block
            catch(Exception ex)
            {
            }
            return View(obj);
        }


        public JsonResult SaveRetutnGoods(partial_tbl_order_history objdata)
        {
            cf = new comman_function();
            bool ss = cf.session_check();
            int sellers_id = Convert.ToInt32(Session["SellerID"]);
            var get_historydetails = dba.tbl_order_history.Where(a => a.Id == objdata.id && a.tbl_seller_id == sellers_id).FirstOrDefault();
            if (get_historydetails != null)
            {
                get_historydetails.condition_type = Convert.ToInt16(objdata.condition);
                get_historydetails.physically_type = Convert.ToInt16(objdata.physicallyreturn);
                get_historydetails.physically_selected_date = objdata.physically_selected_date;
                get_historydetails.physically_updated_date = DateTime.Now;
                dba.Entry(get_historydetails).State = EntityState.Modified;
                dba.SaveChanges();
                objdata.Msg = "Your Return Order is updated successfully";
            }
            return Json(objdata.Msg, JsonRequestBehavior.AllowGet);
        }
        /// <summary>
        /// this is  for claim request section
        /// </summary>
        /// <param name="form"></param>
        /// <param name="ddl_market_place"></param>
        /// <param name="txt_from"></param>
        /// <param name="txt_to"></param>
        /// <returns></returns>

        public ActionResult ClaimReturn(FormCollection form, int? ddl_market_place, DateTime? txt_from, DateTime? txt_to)
        {
            cf = new comman_function();
            if (!cf.session_check())
                return RedirectToAction("Login", "Home");
            int sellers_id = Convert.ToInt32(Session["SellerID"]);
            cf = new comman_function();
            ViewData["FullfilmentList"] = cf.GetFullilmentList();
            List<SelectListItem> lst1_loc = cf.GetMarketPlace(sellers_id);
            ViewData["MarKetPlace"] = lst1_loc;
            List<partial_tbl_order_history> obj = new List<partial_tbl_order_history>();

            if (txt_from != null && txt_to != null)
            {
                var get_details = dba.tbl_order_history.Where(a => a.tbl_seller_id == sellers_id && a.t_order_status == 9 && a.condition_type == 2).ToList();
                if (get_details != null)
                {
                    if (txt_from != null && txt_to != null)
                    {
                        get_details = get_details.Where(a => Convert.ToDateTime(a.ShipmentDate).Date >= txt_from && Convert.ToDateTime(a.ShipmentDate).Date <= txt_to).ToList();
                    }
                }
                if (get_details != null && ddl_market_place != null && ddl_market_place != 0)
                {
                    get_details = get_details.Where(a => a.tbl_marketplace_id == ddl_market_place).ToList();
                }
                foreach (var item in get_details)
                {
                    partial_tbl_order_history objtbl = new partial_tbl_order_history();
                    objtbl.returngoods = item.physically_type;
                    objtbl.sku = item.SKU;
                    objtbl.physicallyreturn = Convert.ToString(item.physically_type);
                    objtbl.condition = Convert.ToString(item.condition_type);
                    objtbl.OrderDate = Convert.ToDateTime(item.ShipmentDate).ToString("yyyy-MM-dd");
                    objtbl.ClaimReceived_refund = Convert.ToDouble(item.SAFE_T_Reimbursement);

                    var get_settlement_order = dba.tbl_settlement_order.Where(a => a.tbl_seller_id == sellers_id && a.Order_Id == item.OrderID && a.Sku_no == item.SKU).FirstOrDefault();
                    if (get_settlement_order != null)
                    {
                        objtbl.ClaimReceived_order = Convert.ToDouble(get_settlement_order.SAFE_T_Reimbursement);
                    }

                    var getsale_order = dba.tbl_sales_order.Where(a => a.tbl_sellers_id == sellers_id && a.amazon_order_id == item.OrderID).FirstOrDefault();// to get sale order value

                    //if (getsale_order != null)
                    //{
                    //    var get_saleorder_detail = dba.tbl_sales_order_details.Where(a => a.tbl_sales_order_id == getsale_order.id && a.sku_no == item.SKU).FirstOrDefault();
                    //    if (get_saleorder_detail != null)
                    //    {
                    //        objtbl.principalvalue = get_saleorder_detail.item_price_amount + get_saleorder_detail.shipping_price_Amount;

                    //        var get_tax = dba.tbl_tax.Where(a => a.tbl_referneced_id == get_saleorder_detail.id && a.reference_type == 3 && a.tbl_seller_id == sellers_id).FirstOrDefault();
                    //        {
                    //            if (get_tax != null)
                    //            {
                    //                objtbl.IGST = Convert.ToDouble(get_tax.Igst_amount);
                    //                objtbl.SGST = Convert.ToDouble(get_tax.sgst_amount);
                    //                objtbl.CGST = Convert.ToDouble(get_tax.CGST_amount);
                    //            }
                    //        }
                    //        objtbl.TotalValue = objtbl.principalvalue + objtbl.IGST + objtbl.SGST + objtbl.CGST;
                    //    }
                    //}// end of if(getsale_order)

                    objtbl.principalvalue =Convert.ToDouble(item.amount_per_unit + item.shipping_price + item.Giftwrap_price + item.product_tax + item.shipping_tax + item.gift_wrap_tax);

                    objtbl.id = item.Id;
                    objtbl.order_id = item.OrderID;
                    objtbl.fullfillment_id = item.fulfillment_id;
                    obj.Add(objtbl);
                }
            }

            return View(obj);
        }


        public JsonResult SaveClaimGoods(partial_tbl_order_history objdata)
        {
            cf = new comman_function();
            bool ss = cf.session_check();
            int sellers_id = Convert.ToInt32(Session["SellerID"]);
            tbl_claim_request obj_claim = new tbl_claim_request();
            obj_claim.claim_created_on = DateTime.Now;
            obj_claim.claim_request_date = objdata.claimselected_date;
            obj_claim.tbl_history_id = objdata.id;
            obj_claim.claim_request_type = Convert.ToInt16(objdata.claim_return_type);
            dba.tbl_claim_request.Add(obj_claim);
            dba.SaveChanges();
            objdata.Msg = "success";
            return Json(objdata.Msg, JsonRequestBehavior.AllowGet);
        }

        /// <summary>
        /// this is only for fetch Data from API 
        /// </summary>
        /// <returns></returns>     
        public ActionResult SettlementXMLData()
        {
            List<AmazonreconciliationOrder> objjson1 = new List<AmazonreconciliationOrder>();
            List<reconciliationorder> objreconciliationorder = new List<reconciliationorder>();
            try
            {
                Dictionary<string, reconciliationorder> dictionary = new Dictionary<string, reconciliationorder>();
                XmlDocument doc = new XmlDocument();
                reconciliationorder obj_item = obj_item = new reconciliationorder();

                doc.Load(Server.MapPath("~/Areas/XML/SettlementAPI.xml"));
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
               // Savesettlementdata(objjson1, 2, 3, abc, sourcetype);
            }// end of try 
            catch (Exception ex)
            {
            }
            return View();
        }


       // List<ReturensDataDetail> objdata = null;
        public ActionResult CallFilpkartData()
        {
            Browse_Upload_Excel_Utility obj = new Browse_Upload_Excel_Utility();
            string filename = "8236.21.xlsx";
            string path = System.IO.Path.Combine(Server.MapPath("~/UploadExcel/" + 19 + "/settlement/" + filename));
            //obj.ReadSettlementFile_Flipkart(path, 19);

            return View();
        }
    }
}
