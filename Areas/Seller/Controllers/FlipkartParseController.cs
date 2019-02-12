using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;
using System.Diagnostics;
using System.Xml;
using System.Data.Entity;
using Newtonsoft.Json;
using System.IO;
using Newtonsoft.Json.Linq;
using System.Text;
using System.Web.Script.Serialization;
using SellerVendor.Areas.Seller.Models.DBContext;
using SellerVendor.Models;
using SellerVendor.Areas.Seller.Models;
using System.Net;

namespace SellerVendor.Areas.Seller.Controllers
{
    public class FlipkartParseController : Controller
    {
        public SellerContext dba = new SellerContext();
        public SellerAdminContext db = new SellerAdminContext();
        comman_function cf = null;

        List<parseJson> objjson = null;
        List<shipmentDetails> objshipment = new List<shipmentDetails>();
        public ActionResult Index()
        {
            objjson = new List<parseJson>();
            List<orderItems> objorderitem = null;
           
            //List<shipments> objshipment = null;
            priceComponents obj = new priceComponents();
            try
            {
                string text;
                string text1;

                var fileStream = new FileStream(@"c:\\abctext.txt", FileMode.Open, FileAccess.Read);
                using (var streamReader = new StreamReader(fileStream, Encoding.UTF8))
                {
                    text = streamReader.ReadToEnd();                
                    var jss = new JavaScriptSerializer();
                    var table = jss.Deserialize<dynamic>(text);
                    var get_data = table["orderItems"];
                    var _data = JsonConvert.DeserializeObject<parseJson>(text);

                    if (_data != null)
                    {
                        int kk = 0;
                        objorderitem = new List<orderItems>();
                        
                        foreach (var d in _data.orderItems)
                        {
                            orderItems obj_item = new orderItems();
                            kk++;
                            obj_item.orderItemId = d.orderItemId;
                            obj_item.orderId = d.orderId;
                            obj_item.hsn = d.hsn;
                            obj_item.status = d.status;
                            obj_item.hold = d.hold;
                            obj_item.orderDate = d.orderDate;
                            obj_item.dispatchAfterDate = d.dispatchAfterDate;
                            obj_item.dispatchByDate = d.dispatchByDate;
                            obj_item.updatedAt = d.updatedAt;
                            obj_item.sla = d.sla;
                            obj_item.title = d.title;
                            obj_item.listingId = d.listingId;
                            obj_item.fsn = d.fsn;
                            obj_item.sku = d.sku;
                            obj_item.shippingPincode = d.shippingPincode;
                            obj_item.paymentType = d.paymentType;
                            obj_item.quantity = d.quantity;
                            obj_item.price = d.price;
                            obj_item.shippingFee = d.shippingFee;
                            obj_item.sellingPrice = d.priceComponents.sellingPrice;
                            obj_item.customerPrice = d.priceComponents.customerPrice;
                            obj_item.shippingCharge = d.priceComponents.shippingCharge;
                            obj_item.totalPrice = d.priceComponents.totalPrice;
                            obj_item.shipmentDetails=new List<shipments>();

                            var fileStream1 = new FileStream(@"c:\\vineetdetails.txt", FileMode.Open, FileAccess.Read);
                            using (var streamReader1 = new StreamReader(fileStream1, Encoding.UTF8))
                            {

                                text1 = streamReader1.ReadToEnd();
                                var jss1 = new JavaScriptSerializer();
                                var table1 = jss1.Deserialize<dynamic>(text1);
                                var get_data1 = table1["shipments"];
                                var _data1 = JsonConvert.DeserializeObject<shipmentDetails>(text1);
                                if (_data1 != null)
                                {

                                    foreach (var item in _data1.shipments)
                                    {
                                        if (item != null)
                                        {

                                            if (obj_item.orderId == item.orderId)
                                            {
                                                shipments obj_ship = new shipments();
                                                
                                                obj_ship.orderId = item.orderId;
                                                obj_ship.shipmentId = item.shipmentId;
                                                obj_ship.weighingRequired = item.weighingRequired;
                                                if (item.returnAddress != null)
                                                {
                                                 obj_ship.returnAddress = item.returnAddress;
                                                }
                                                obj_ship.orderItems = new List<Common.orderItems>();

                                                if (item.orderItems != null)
                                                {
                                                    foreach (var item1 in item.orderItems)
                                                    {
                                                        Common.orderItems objo = new Common.orderItems();
                                                        objo.id = item1.id;
                                                        objo.fragile = item1.fragile;
                                                        objo.dangerous = item1.dangerous;
                                                        objo.large = item1.large;

                                                        obj_ship.orderItems.Add(objo);
                                                    }// end of for each(item1)
                                                    
                                                }// end of if(item.orders)
                                                obj_ship.deliveryAddress  = new deliveryAddress();
                                                if (item.deliveryAddress != null)
                                                {
                                                    obj_ship.deliveryAddress.firstName = item.deliveryAddress.firstName;
                                                    if (item.deliveryAddress.lastName != null && item.deliveryAddress.lastName != "" && item.deliveryAddress.lastName != " ")
                                                    {
                                                        obj_ship.deliveryAddress.lastName = item.deliveryAddress.lastName;
                                                    }
                                                    obj_ship.deliveryAddress.addressLine1 = item.deliveryAddress.addressLine1;
                                                    obj_ship.deliveryAddress.addressLine2 = item.deliveryAddress.addressLine2;
                                                    obj_ship.deliveryAddress.city = item.deliveryAddress.city;
                                                    obj_ship.deliveryAddress.state = item.deliveryAddress.state;
                                                    obj_ship.deliveryAddress.pincode = item.deliveryAddress.pincode;
                                                    obj_ship.deliveryAddress.stateCode = item.deliveryAddress.stateCode;
                                                    obj_ship.deliveryAddress.stateName = item.deliveryAddress.stateName;
                                                    obj_ship.deliveryAddress.landmark = item.deliveryAddress.landmark;
                                                    obj_ship.deliveryAddress.contactNumber = item.deliveryAddress.contactNumber;
                                                  
                                                }
                                                obj_ship.billingAddress = new billingAddress();
                                                if (item.billingAddress != null)
                                                {
                                                    obj_ship.billingAddress.firstName = item.billingAddress.firstName;
                                                    if (item.billingAddress.lastName != null && item.billingAddress.lastName != "" && item.billingAddress.lastName != " ")
                                                    {
                                                        obj_ship.billingAddress.lastName = item.billingAddress.lastName;
                                                    }
                                                    obj_ship.billingAddress.addressLine1 = item.billingAddress.addressLine1;
                                                    obj_ship.billingAddress.addressLine2 = item.billingAddress.addressLine2;
                                                    obj_ship.billingAddress.city = item.billingAddress.city;
                                                    obj_ship.billingAddress.state = item.billingAddress.state;
                                                    obj_ship.billingAddress.pincode = item.billingAddress.pincode;
                                                    obj_ship.billingAddress.stateCode = item.billingAddress.stateCode;
                                                    obj_ship.billingAddress.stateName = item.billingAddress.stateName;
                                                    obj_ship.billingAddress.landmark = item.billingAddress.landmark;
                                                    obj_ship.billingAddress.contactNumber = item.billingAddress.contactNumber;

                                                }
                                                obj_ship.buyerDetails = new buyerDetails();
                                                if (item.buyerDetails != null)
                                                {
                                                    if (item.buyerDetails.firstName != null)
                                                    {
                                                        obj_ship.buyerDetails.firstName = item.buyerDetails.firstName;
                                                    }
                                                    if (item.buyerDetails.lastName != null)
                                                    {
                                                        obj_ship.buyerDetails.lastName = item.buyerDetails.lastName;
                                                    }
                                                    if (item.buyerDetails.contactNumber != null)
                                                    {
                                                        obj_ship.buyerDetails.contactNumber = item.buyerDetails.contactNumber;
                                                    }
                                                    if (item.buyerDetails.contactNumber != null)
                                                    {
                                                        obj_ship.buyerDetails.contactNumber = item.buyerDetails.contactNumber;
                                                    }
                                                }
                                                if (item.sellerAddress != null)
                                                {
                                                }
                                                
                                                obj_item.shipmentDetails.Add(obj_ship);
                                            }// end of if (orderid)
                                        }                                     
                                    }// end of foreach(item)
                                }// end of if(_data1)
                            }// end of using(streamReader1)
                            objorderitem.Add(obj_item);
                        }// end of foreach(d)
                        objjson.Add(new parseJson
                        {
                            orderItems = objorderitem,
                            //  shipmentDetails = objshipment,
                        });
                    }// end of if(data null)
                    else
                    {
                        int jjj = 0;
                        jjj++;
                    }
                    //saveFlipkartData(objjson);
                }                
            }
            catch(Exception ex)
            {
            }
            return View(objjson);
        }

        public string saveFlipkartData(List<parseJson> objjson)
        {
            cf = new comman_function();
            bool ss = cf.session_check();
            try
            {
                tbl_sales_order objsale = new tbl_sales_order();
                tbl_customer_details objcustumer = new tbl_customer_details();
                tbl_sales_order_details objsaledetails = new tbl_sales_order_details();              
                int SellerId = Convert.ToInt32(Session["SellerID"]);
                string datasaved = "";
                string lowername = "Flipkart".ToLower();
                if (objjson != null)
                {
                    foreach (var item in objjson[0].orderItems)
                    {
                        string Message;
                        bool flag = false;
                        var getmarketplacedetails = db.m_marketplace.Where(a => a.isactive == 1).ToList();// get details from marketplace table in seller admin
                        var getdetails = dba.tbl_sales_order.Where(a => a.amazon_order_id == item.orderId && a.order_status == item.status).FirstOrDefault();
                        if(getdetails == null)
                        {
                            if (item.shipmentDetails != null && item.shipmentDetails.Count > 0)
                            {
                                if (item.shipmentDetails[0].billingAddress != null)
                                {
                                    objcustumer.Address_1 = item.shipmentDetails[0].billingAddress.addressLine1;
                                    objcustumer.Address_2 = item.shipmentDetails[0].billingAddress.addressLine2;
                                    objcustumer.shipping_Buyer_Name = item.shipmentDetails[0].billingAddress.firstName;
                                    objcustumer.City = item.shipmentDetails[0].billingAddress.city;
                                    objcustumer.Postal_Code = item.shipmentDetails[0].billingAddress.pincode;
                                    objcustumer.State_Region = item.shipmentDetails[0].billingAddress.stateName;
                                    objcustumer.last_name = item.shipmentDetails[0].billingAddress.lastName;
                                    objcustumer.state_code = item.shipmentDetails[0].billingAddress.stateCode;
                                    objcustumer.landmark = item.shipmentDetails[0].billingAddress.landmark;
                                    objcustumer.contact_no = item.shipmentDetails[0].billingAddress.contactNumber;
                                    objcustumer.state = item.shipmentDetails[0].billingAddress.state;
                                    objcustumer.tbl_seller_id = SellerId;
                                    dba.tbl_customer_details.Add(objcustumer);
                                    dba.SaveChanges();
                                }// end of  if(item.billing address)
                            }// end of if(item.shipping details)

                            objsale.amazon_order_id = item.orderId;
                            objsale.order_item_id = item.orderItemId;
                            objsale.order_status = item.status;
                            objsale.purchase_date = Convert.ToDateTime(item.orderDate);
                            objsale.payment_method = item.paymentType;
                            objsale.bill_amount =Convert.ToDouble(item.price);
                            objsale.tbl_sellers_id = SellerId;
                            objsale.created_on = DateTime.Now;
                            objsale.is_active = 1;
                            objsale.tbl_Customer_Id = objcustumer.id;
                            objsale.buyer_name = objcustumer.shipping_Buyer_Name;
                            objsale.earliest_ship_date =Convert.ToDateTime(item.dispatchByDate);
                            objsale.dispatch_afterdate = Convert.ToDateTime(item.dispatchAfterDate);
                            objsale.t_Hold = item.hold;
                            objsale.n_item_orderstatus = 1;

                            foreach (var abc in getmarketplacedetails)
                            {
                                string name = abc.name.ToLower();
                                if (lowername == name)
                                {
                                    objsale.tbl_Marketplace_Id = abc.id;
                                    break;
                                }
                            }//end of for each loop ( for get marketplace details)

                            dba.tbl_sales_order.Add(objsale);
                            dba.SaveChanges();

                           

                            objsaledetails.asin = item.fsn;
                            objsaledetails.sku_no = item.sku;
                            objsaledetails.sla_flipkart =Convert.ToInt16(item.sla);
                            objsaledetails.listing_id = item.listingId;
                            objsaledetails.product_name = item.title;
                            objsaledetails.quantity_ordered = item.quantity;
                            objsaledetails.order_item_id = item.orderItemId;
                            objsaledetails.dispatch_bydate =Convert.ToDateTime(item.dispatchByDate);
                            objsaledetails.dispatchAfter_date = Convert.ToDateTime(item.dispatchAfterDate);
                            objsaledetails.shipping_pincode = item.shippingPincode;
                            objsaledetails.selling_price = Convert.ToDouble(item.sellingPrice);
                            objsaledetails.hsn = item.hsn;
                            objsaledetails.amazon_order_id = item.orderId;
                            objsaledetails.item_price_amount = Convert.ToDouble(item.priceComponents.sellingPrice);
                            if (item.priceComponents.shippingCharge != 0 && item.priceComponents.shippingCharge != null)
                            {
                                objsaledetails.shipping_price_Amount = Convert.ToDouble(item.priceComponents.shippingCharge);
                            }
                            else
                            {
                                objsaledetails.shipping_price_Amount = 0;
                            }
                            if (item.shipmentDetails != null && item.shipmentDetails.Count > 0)
                            {
                                objsaledetails.shipment_Id = item.shipmentDetails[0].shipmentId;
                            }

                            objsaledetails.is_active = 1;
                            objsaledetails.tbl_seller_id = SellerId;
                            objsaledetails.tbl_sales_order_id = objsale.id;
                            objsaledetails.status_updated_by = SellerId;
                            objsaledetails.status_updated_on = DateTime.Now;
                            objsaledetails.n_order_status_id = 1;

                            dba.tbl_sales_order_details.Add(objsaledetails);
                            dba.SaveChanges();


                           

                        }// end of if(getdetails)

                    }// end of foreach(item)
                }// end of if(objson)
                return datasaved;
            }// end of try
            catch(Exception ex)
            {
            }
            return "";
        }



        #region calling flipkart order api
        /// <summary>
        /// this is for calling sales order data through flipkart api
        /// </summary>
        /// <returns></returns>
        public string CallFlipkartSalesOrderAPI(SalesOrderRequest objsalesorderrequest)
        {
            string responseMessage = string.Empty;
            try
            {
              // string sss=@"{\"filter\": {\"orderDate\": { \"fromDate\": "2017-12-01","toDate": "2017-12-05" } }}";
                //string publicKey = "c48983b1-a228-4afa-9753-cc5f124c14ba";
                //string appid = "ba56584a49a0293089669a980aabb465b800";
                //string appsecret = "38ad1904a2ebc2eb00a24960a3cfa3956";
                
                string baseAddress = "https://api.flipkart.net/";
                string apiPath = "sellers/orders/search";

                var request = (HttpWebRequest)WebRequest.Create(baseAddress + apiPath);
                request.Method = "POST";
                request.ContentType = "application/json";
                request.Accept = "application/json";

                string jsonOrder = JsonConvert.SerializeObject(objsalesorderrequest);
                var data = Encoding.UTF8.GetBytes(jsonOrder);
                request.ContentLength = data.Length;
                request.Headers["Authorization"] = "Bearer 66a71783-c252-40f9-96f9-5c3520e81ae3";
                Stream streams = request.GetRequestStream();
                streams.Write(data, 0, data.Length);
                streams.Close();

                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                Stream dataStream = response.GetResponseStream();
                if (((HttpWebResponse)response).StatusDescription == "OK")
                {
                    using (var stream = response.GetResponseStream())
                    {
                        StreamReader reader = new StreamReader(stream);
                        responseMessage = reader.ReadToEnd();
                    }
                }
            }
            catch(Exception ex)
            {
            }
            return responseMessage; 
        }



        public string CallFlipkartOrderDetailsAPI(string itemorderid, SalesOrderRequest searchrequest)
        {
            string responseMessage1 = string.Empty;
            try
            {               
                string baseAddress = "https://api.flipkart.net";
                string apiPath = "/sellers/v2/orders/shipments?orderItemIds=" + itemorderid;

                var request = (HttpWebRequest)WebRequest.Create(baseAddress + apiPath);
                request.Method = "GET";              
                request.Accept = "application/json";               
                request.Headers["Authorization"] = "Bearer 66a71783-c252-40f9-96f9-5c3520e81ae3";            
                HttpWebResponse response = (HttpWebResponse)request.GetResponse();
                Stream dataStream = response.GetResponseStream();
                if (((HttpWebResponse)response).StatusDescription == "OK")
                {
                    using (var stream = response.GetResponseStream())
                    {
                        StreamReader reader = new StreamReader(stream);
                        responseMessage1 = reader.ReadToEnd();
                    }
                }
            }
            catch (Exception ex)
            {
            }
            return responseMessage1;
        }



        public ActionResult GetSalesOrderSearchData()
        {
            List<parseJson> lstAPIData = new List<parseJson>();
            parseJson SearchResponseNew = new parseJson();
            shipmentDetails objresponse = new shipmentDetails();
            try
            {
                SalesOrderRequest searchrequest = new SalesOrderRequest();
                searchrequest.filter = new filter();
                searchrequest.filter.orderDate = new orderDate();
                searchrequest.filter.orderDate.fromDate = "2018-01-01";
                searchrequest.filter.orderDate.toDate = "2018-01-31";


                string response = CallFlipkartSalesOrderAPI(searchrequest);
                    //ParseJson(response);
                    SearchResponseNew = JsonConvert.DeserializeObject<parseJson>(response);
                    lstAPIData.Add(SearchResponseNew);
                    string itemorderid = "";
                    string comma = ",";
                    if (lstAPIData.Count > 0)
                    {
                        foreach (var item in lstAPIData[0].orderItems)
                        {
                            if (itemorderid=="")
                                itemorderid = item.orderItemId;
                            else
                                itemorderid = itemorderid + comma + item.orderItemId;
                        }
                    }
                    string response1 = "";
                    if (itemorderid != "" && itemorderid != null)
                    {
                       response1 =  CallFlipkartOrderDetailsAPI(itemorderid, searchrequest);
                      objresponse = JsonConvert.DeserializeObject<shipmentDetails>(response1);
                      //lstAPIData.Add(objresponse);
                    }
                    //saveFlipkartData(lstAPIData, response1);

                    parseresult(response, response1);
            }
            catch (Exception ex)
            {
                return null;
            }
            //return lstAPIData;
            return View();
        }

        private void ParseJson(string response)
        {
            throw new NotImplementedException();
        }

        public string parseresult(string response, string response1)
        {
            objjson = new List<parseJson>();
            List<orderItems> objorderitem = null;           
            priceComponents obj = new priceComponents();
            try
            {
                if (response != "" && response != null)
                {

                    var _data = JsonConvert.DeserializeObject<parseJson>(response);

                    if (_data != null)
                    {
                        int kk = 0;
                        objorderitem = new List<orderItems>();

                        foreach (var d in _data.orderItems)
                        {
                            orderItems obj_item = new orderItems();
                            kk++;
                            obj_item.orderItemId = d.orderItemId;
                            obj_item.orderId = d.orderId;
                            obj_item.hsn = d.hsn;
                            obj_item.status = d.status;
                            obj_item.hold = d.hold;
                            obj_item.orderDate = d.orderDate;
                            obj_item.dispatchAfterDate = d.dispatchAfterDate;
                            obj_item.dispatchByDate = d.dispatchByDate;
                            obj_item.updatedAt = d.updatedAt;
                            obj_item.sla = d.sla;
                            obj_item.title = d.title;
                            obj_item.listingId = d.listingId;
                            obj_item.fsn = d.fsn;
                            obj_item.sku = d.sku;
                            obj_item.shippingPincode = d.shippingPincode;
                            obj_item.paymentType = d.paymentType;
                            obj_item.quantity = d.quantity;
                            obj_item.price = d.price;
                            obj_item.shippingFee = d.shippingFee;
                            obj_item.sellingPrice = d.priceComponents.sellingPrice;
                            obj_item.customerPrice = d.priceComponents.customerPrice;
                            obj_item.shippingCharge = d.priceComponents.shippingCharge;
                            obj_item.totalPrice = d.priceComponents.totalPrice;
                            obj_item.shipmentDetails = new List<shipments>();

                            if (response1 != null && response1 != "")
                            {
                                var _data1 = JsonConvert.DeserializeObject<shipmentDetails>(response1);
                                if (_data1 != null)
                                {

                                    foreach (var item in _data1.shipments)
                                    {
                                        if (item != null)
                                        {

                                            if (obj_item.orderId == item.orderId)
                                            {
                                                shipments obj_ship = new shipments();

                                                obj_ship.orderId = item.orderId;
                                                obj_ship.shipmentId = item.shipmentId;
                                                obj_ship.weighingRequired = item.weighingRequired;
                                                if (item.returnAddress != null)
                                                {
                                                    obj_ship.returnAddress = item.returnAddress;
                                                }
                                                obj_ship.orderItems = new List<Common.orderItems>();

                                                if (item.orderItems != null)
                                                {
                                                    foreach (var item1 in item.orderItems)
                                                    {
                                                        Common.orderItems objo = new Common.orderItems();
                                                        objo.id = item1.id;
                                                        objo.fragile = item1.fragile;
                                                        objo.dangerous = item1.dangerous;
                                                        objo.large = item1.large;

                                                        obj_ship.orderItems.Add(objo);
                                                    }// end of for each(item1)

                                                }// end of if(item.orders)
                                                obj_ship.deliveryAddress = new deliveryAddress();
                                                if (item.deliveryAddress != null)
                                                {
                                                    obj_ship.deliveryAddress.firstName = item.deliveryAddress.firstName;
                                                    if (item.deliveryAddress.lastName != null && item.deliveryAddress.lastName != "" && item.deliveryAddress.lastName != " ")
                                                    {
                                                        obj_ship.deliveryAddress.lastName = item.deliveryAddress.lastName;
                                                    }
                                                    obj_ship.deliveryAddress.addressLine1 = item.deliveryAddress.addressLine1;
                                                    obj_ship.deliveryAddress.addressLine2 = item.deliveryAddress.addressLine2;
                                                    obj_ship.deliveryAddress.city = item.deliveryAddress.city;
                                                    obj_ship.deliveryAddress.state = item.deliveryAddress.state;
                                                    obj_ship.deliveryAddress.pincode = item.deliveryAddress.pincode;
                                                    obj_ship.deliveryAddress.stateCode = item.deliveryAddress.stateCode;
                                                    obj_ship.deliveryAddress.stateName = item.deliveryAddress.stateName;
                                                    obj_ship.deliveryAddress.landmark = item.deliveryAddress.landmark;
                                                    obj_ship.deliveryAddress.contactNumber = item.deliveryAddress.contactNumber;

                                                }
                                                obj_ship.billingAddress = new billingAddress();
                                                if (item.billingAddress != null)
                                                {
                                                    obj_ship.billingAddress.firstName = item.billingAddress.firstName;
                                                    if (item.billingAddress.lastName != null && item.billingAddress.lastName != "" && item.billingAddress.lastName != " ")
                                                    {
                                                        obj_ship.billingAddress.lastName = item.billingAddress.lastName;
                                                    }
                                                    obj_ship.billingAddress.addressLine1 = item.billingAddress.addressLine1;
                                                    obj_ship.billingAddress.addressLine2 = item.billingAddress.addressLine2;
                                                    obj_ship.billingAddress.city = item.billingAddress.city;
                                                    obj_ship.billingAddress.state = item.billingAddress.state;
                                                    obj_ship.billingAddress.pincode = item.billingAddress.pincode;
                                                    obj_ship.billingAddress.stateCode = item.billingAddress.stateCode;
                                                    obj_ship.billingAddress.stateName = item.billingAddress.stateName;
                                                    obj_ship.billingAddress.landmark = item.billingAddress.landmark;
                                                    obj_ship.billingAddress.contactNumber = item.billingAddress.contactNumber;

                                                }
                                                obj_ship.buyerDetails = new buyerDetails();
                                                if (item.buyerDetails != null)
                                                {
                                                    if (item.buyerDetails.firstName != null)
                                                    {
                                                        obj_ship.buyerDetails.firstName = item.buyerDetails.firstName;
                                                    }
                                                    if (item.buyerDetails.lastName != null)
                                                    {
                                                        obj_ship.buyerDetails.lastName = item.buyerDetails.lastName;
                                                    }
                                                    if (item.buyerDetails.contactNumber != null)
                                                    {
                                                        obj_ship.buyerDetails.contactNumber = item.buyerDetails.contactNumber;
                                                    }
                                                    if (item.buyerDetails.contactNumber != null)
                                                    {
                                                        obj_ship.buyerDetails.contactNumber = item.buyerDetails.contactNumber;
                                                    }
                                                }
                                                if (item.sellerAddress != null)
                                                {
                                                }

                                                obj_item.shipmentDetails.Add(obj_ship);
                                            }// end of if (orderid)
                                        }
                                    }// end of foreach(item)
                                }// end of if(_data1)

                            }// end of if (response1)
                            //}// end of using(streamReader1)

                            objorderitem.Add(obj_item);
                        }// end of foreach(d)
                        objjson.Add(new parseJson
                        {
                            orderItems = objorderitem,
                            //  shipmentDetails = objshipment,
                        });
                    }// end of if(data null)
                    else
                    {
                        int jjj = 0;
                        jjj++;
                    }
                    saveFlipkartData(objjson);
                    //}

                }// end of if (response)
            }
            catch (Exception ex)
            {
            } 
            return "";
        }

        #endregion

    }
}
