using SellerVendor.Areas.Seller.Models.DBContext;
using SellerVendor.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;
using MarketplaceWebServiceOrders;
using MarketplaceWebServiceOrders.Model;
using SellerVendor.Areas.Seller.Models;


namespace SellerVendor.Areas.Seller.Models
{
    public class Get_Seller_API_DATA
    {
        public static SellerAdminContext admin_db = new SellerAdminContext();
        public static SellerContext db = new SellerContext();
        //public static Seller_Market seller_db = new Seller_Market();
        public static API_Context api_db = new API_Context();

        #region Calling Sales Order API
        /// <summary>
        /// Collect Seller List to Execute API
        /// </summary>     
        public string Collect_Seller(int? ddl_marketplaceAPI, DateTime? txt_from, DateTime? txt_to, int sellers_id, int? id)
        {
            string strMsg = "S";
            try
            {
                List<seller_parameter_list> lstSeller = new List<seller_parameter_list>();
                var seller_List_Admin = admin_db.tbl_sellers.Where(a => a.isactive == 1 && a.id == sellers_id).FirstOrDefault();
                if (seller_List_Admin != null)
                {
                    int strId = Convert.ToInt32(seller_List_Admin.id);
                    var seller_Market_List = db.tbl_sellermarketplace.Where(a => a.isactive == 1 && a.tbl_seller_id == strId && a.m_marketplace_id == ddl_marketplaceAPI).FirstOrDefault();

                    if (seller_Market_List != null)
                    {
                        seller_parameter_list obj = new seller_parameter_list();
                        obj.id = seller_Market_List.id.ToString();
                        obj.my_unique_id = seller_Market_List.my_unique_id.ToString(); ///// SELLER ID 
                        obj.my_seller_id = seller_Market_List.tbl_seller_id.ToString(); ///// SELLER ID - Primary Key Value                           
                        obj.t_access_Key_id = seller_Market_List.t_access_Key_id.ToString();
                        //obj.t_auth_token = seller_Market_List.t_auth_token.ToString();
                        obj.t_secret_Key = seller_Market_List.t_secret_Key.ToString();
                        obj.market_palce_id = seller_Market_List.market_palce_id.ToString();
                        obj.m_marketplace_id = seller_Market_List.m_marketplace_id.ToString();
                        lstSeller.Add(obj);
                    }
                        //foreach (var itemMkt in seller_Market_List)
                        //{
                        //    seller_parameter_list obj = new seller_parameter_list();
                        //    obj.id = itemMkt.id.ToString();

                        //    obj.my_unique_id = itemMkt.my_unique_id.ToString(); ///// SELLER ID 
                        //    obj.my_seller_id = itemMkt.tbl_seller_id.ToString(); ///// SELLER ID - Primary Key Value                           
                        //    obj.t_access_Key_id = itemMkt.t_access_Key_id.ToString();
                        //    obj.t_auth_token = itemMkt.t_auth_token.ToString();
                        //    obj.t_secret_Key = itemMkt.t_secret_Key.ToString();
                        //    obj.market_palce_id = itemMkt.market_palce_id.ToString();
                        //    obj.m_marketplace_id = itemMkt.m_marketplace_id.ToString();
                        //    lstSeller.Add(obj);
                        //}
                    
                    if (lstSeller.Count > 0)
                    {
                        strMsg = Start_API_Calling_ListOrder(lstSeller, txt_from, txt_to, id);
                    }
                }
            }
            catch
            {
                strMsg = "F";
            }

            return strMsg;
        }

        public string Start_API_Calling_ListOrder(List<seller_parameter_list> lstSellerData, DateTime? txt_from, DateTime? txt_to, int? id)
        {
            string strStatus = "S";
            try
            {
                foreach (var seller in lstSellerData)
                {
                    api_parameter objApi = new api_parameter();
                    DateTime dt = System.DateTime.Now.AddMinutes(-2);
                    DateTime DtStart;
                    DateTime DtEnd;
                    int Sid = Convert.ToInt32(seller.my_seller_id.ToString());
                    objApi.id = Sid;
                    objApi.Seller_ID = seller.my_unique_id.ToString();
                    objApi.Secret_Key = seller.t_secret_Key.ToString();
                    objApi.AWS_Access_Key_ID = seller.t_access_Key_id.ToString();
                    objApi.marketplaceid = seller.market_palce_id.ToString();
                    objApi.m_marketplace_id = Convert.ToInt32(seller.m_marketplace_id.ToString());
                    objApi.auth_token = "";
                    //objApi.Seller_ID = "ALX5VZP4KM90V"; //seller.my_unique_id.ToString(); //"A215Z7QR1LOVAK"; // seller.my_unique_id.ToString();
                    //objApi.Secret_Key = "tB1OE8WvQB4xZ8bOtD8aFIfW2H1uwfAIfYDMfVx8";//seller.t_secret_Key.ToString(); // "bk3xsBJLL2CH6BXyc2jRev+0Lf3Mg8aDZM4jHrht"; // seller.t_secret_Key.ToString();
                    //objApi.AWS_Access_Key_ID = "AKIAILZANI6ZKK4Z3PAQ";//seller.t_access_Key_id.ToString(); // "AKIAJ2LOHS2WG5TTUTHQ"; // seller.t_access_Key_id.ToString();
                    //objApi.marketplaceid = "A21TJRUUN4KGV";//seller.market_palce_id.ToString(); //"A21TJRUUN4KGV"; // seller.market_palce_id.ToString();
                    //objApi.m_marketplace_id = Convert.ToInt32(seller.m_marketplace_id.ToString());
                    //objApi.auth_token = ""; // seller.t_auth_token.ToString();

                    ///////////// Get Data Last Run ////////////////
                    //var seller_plan_List = db.tbl_seller_api_plan.Where(a => a.seller_id == Sid).FirstOrDefault();
                    //if (seller_plan_List != null)
                    //{
                    //    int api_execution_hours = Convert.ToInt32(seller_plan_List.api_execution_hours.ToString());
                    //    string last_execution_date = seller_plan_List.last_execution_date.ToString();
                    //    DtStart = Convert.ToDateTime(last_execution_date);
                    //    DtEnd = DtStart.AddHours(api_execution_hours);
                    //}
                    //else
                    //{
                    //    DtStart = dt.AddHours(-24);
                    //    DtEnd = dt;
                    //}




                    DtStart = Convert.ToDateTime(txt_from);// DateTime.Parse("2017-12-01 23:59:59");//DtStart = Convert.ToDateTime(txt_from);//.AddHours(-24);
                    DtEnd = Convert.ToDateTime(txt_to);// DateTime.Parse("2017-12-31 23:59:59"); //DtEnd = Convert.ToDateTime(txt_to);
                    objApi.createafter = DtStart; //objApi.createafter = Convert.ToDateTime(txt_from);                
                    objApi.createbefore = DtEnd;//objApi.createbefore = Convert.ToDateTime(txt_to);
                    //objApi.createafter = System.DateTime.Now.AddDays(-50);
                    //objApi.createbefore = System.DateTime.Now.AddMinutes(-5);
                    if (DtEnd <= System.DateTime.Now)
                    {
                        //return "F";
                        List<api_parameter> lstApiDetails = new List<api_parameter>();
                        lstApiDetails.Add(objApi);
                         //MarketplaceWebServiceOrders.Amazon objAmazonClass = new MarketplaceWebServiceOrders.Amazon();//error

                         Amazon objAmazonClass = new Amazon();
                         //MarketplaceWebServiceOrders.Amazon objAmazonClass = new MarketplaceWebServiceOrders.Amazon();
                         string strResponseXMLFormat = objAmazonClass.Call_API_List_Order(lstApiDetails, Sid, id);
                        if (strResponseXMLFormat == "S")
                        {
                            tbl_api_meter objapimeter = new tbl_api_meter();
                            /////////////// UPDATE TABLE IN DATABASE ///////////////////
                            //objapimeter.id = Sid;
                            objapimeter.seller_id = Sid;//  seller.my_unique_id.ToString();// "OrderList/OrderListItem";
                            objapimeter.m_marketplace_id = Convert.ToInt32(seller.m_marketplace_id.ToString()); //"OrderList/OrderListItem";
                            objapimeter.api_type = "OrderList/OrderListItem";
                            objapimeter.till_datetime = DtEnd;// "OrderList/OrderListItem";
                            objapimeter.MarketId = seller.market_palce_id.ToString(); // "OrderList/OrderListItem";

                            //api_db.tbl_api_meter.Add(objapimeter);
                            //api_db.SaveChanges();

                            /////////////// Update api last time //////////////////
                            tbl_seller_api_plan objsellerapimeter = new tbl_seller_api_plan();
                            var get_details = db.tbl_seller_api_plan.Where(a => a.id == Sid).FirstOrDefault();
                            if (get_details != null)
                            {
                                get_details.last_execution_date = DtEnd;
                                //db.Entry(get_details).State = EntityState.Modified;
                                //db.SaveChanges();
                            }
                            else
                            {
                                objsellerapimeter.seller_id = Sid;
                                objsellerapimeter.api_execution_hours = 2;
                                objsellerapimeter.last_execution_date = DtEnd;
                                //db.tbl_seller_api_plan.Add(objsellerapimeter);
                                //db.SaveChanges();

                            }

                        }
                    }
                }
            }
            catch
            {
                strStatus = "F";
            }
            return strStatus;
        }

        # endregion

        #region Calling SettlementAPI
        /// <summary>
        /// this is for call Settlement API
        /// </summary>
        /// <param name="ddl_marketplaceAPI"></param>
        /// <param name="txt_from"></param>
        /// <param name="txt_to"></param>
        /// <param name="sellers_id"></param>
        /// <param name="id"></param>
        /// <returns></returns>
        public string Collect_Seller_Settlement(int? ddl_marketplaceAPI, DateTime? txt_from, DateTime? txt_to, int sellers_id, int id)
        {
            string strMsg = "S";
            try
            {
                List<seller_parameter_list> lstSeller = new List<seller_parameter_list>();
                var seller_List_Admin = admin_db.tbl_sellers.Where(a => a.isactive == 1 && a.id == sellers_id).FirstOrDefault();
                if (seller_List_Admin != null)
                {
                    int strId = Convert.ToInt32(seller_List_Admin.id);
                    var seller_Market_List = db.tbl_sellermarketplace.Where(a => a.isactive == 1 && a.tbl_seller_id == strId && a.m_marketplace_id == ddl_marketplaceAPI).FirstOrDefault();

                    if (seller_Market_List != null)
                    {
                        seller_parameter_list obj = new seller_parameter_list();
                        obj.id = seller_Market_List.id.ToString();
                        obj.my_unique_id = seller_Market_List.my_unique_id.ToString(); ///// SELLER ID 
                        obj.my_seller_id = seller_Market_List.tbl_seller_id.ToString(); ///// SELLER ID - Primary Key Value                           
                        obj.t_access_Key_id = seller_Market_List.t_access_Key_id.ToString();
                        //obj.t_auth_token = seller_Market_List.t_auth_token.ToString();
                        obj.t_secret_Key = seller_Market_List.t_secret_Key.ToString();
                        obj.market_palce_id = seller_Market_List.market_palce_id.ToString();
                        obj.m_marketplace_id = seller_Market_List.m_marketplace_id.ToString();
                        lstSeller.Add(obj);
                    }                   
                    if (lstSeller.Count > 0)
                    {
                        strMsg = Start_API_Calling_Settlement(lstSeller, txt_from, txt_to,id);
                    }
                }
            }
            catch
            {
                strMsg = "F";
            }

            return strMsg;
        }

        public string Start_API_Calling_Settlement(List<seller_parameter_list> lstSellerData, DateTime? txt_from, DateTime? txt_to, int id)
        {
            string strStatus = "S";
            try
            {
                foreach (var seller in lstSellerData)
                {
                    api_parameter objApi = new api_parameter();
                    DateTime dt = System.DateTime.Now.AddMinutes(-2);
                    DateTime DtStart;
                    DateTime DtEnd;
                    int Sid = Convert.ToInt32(seller.my_seller_id.ToString());
                    objApi.id = Sid;
                    objApi.Seller_ID = seller.my_unique_id.ToString();
                    objApi.Secret_Key = seller.t_secret_Key.ToString();
                    objApi.AWS_Access_Key_ID = seller.t_access_Key_id.ToString();
                    objApi.marketplaceid = seller.market_palce_id.ToString();
                    objApi.m_marketplace_id = Convert.ToInt32(seller.m_marketplace_id.ToString());
                    objApi.auth_token = "";

                    DtStart = Convert.ToDateTime(txt_from);
                    DtEnd = Convert.ToDateTime(txt_to);
                    //DtStart = DateTime.Parse("2017-12-01 23:59:59");//DtStart = Convert.ToDateTime(txt_from);//.AddHours(-24);
                   // DtEnd = DateTime.Parse("2017-12-31 23:59:59"); //DtEnd = Convert.ToDateTime(txt_to);
                    objApi.createafter = DtStart; //objApi.createafter = Convert.ToDateTime(txt_from);                
                    objApi.createbefore = DtEnd;//objApi.createbefore = Convert.ToDateTime(txt_to);
                   
                    if (DtEnd <= System.DateTime.Now)
                    {
                        //return "F";
                        List<api_parameter> lstApiDetails = new List<api_parameter>();
                        lstApiDetails.Add(objApi);
                    
                        //Amazon objAmazonClass = new Amazon();
                        SettlementAPI objsettlementapi = new SettlementAPI();
                        string strResponseXMLFormat = objsettlementapi.Call_API_Settlement(lstApiDetails, Sid, id);                      
                    }
                }
            }
            catch
            {
                strStatus = "F";
            }
            return strStatus;
        }
         # endregion

        #region Product API
        /// <summary>
        /// this is for call product API
        /// </summary>
        /// <returns></returns>
        public string Collect_Product_List(int? ddl_marketplaceAPI, DateTime? txt_from, DateTime? txt_to, int sellers_id, int id)
        {
            string strMsg = "S";
            try
            {
                List<seller_parameter_list> lstSeller = new List<seller_parameter_list>();
                var seller_List_Admin = admin_db.tbl_sellers.Where(a => a.isactive == 1 && a.id == 19).FirstOrDefault();
                if (seller_List_Admin != null)
                {
                    int strId = Convert.ToInt32(seller_List_Admin.id);
                    var seller_Market_List = db.tbl_sellermarketplace.Where(a => a.isactive == 1 && a.tbl_seller_id == strId && a.m_marketplace_id == 3).FirstOrDefault();

                    if (seller_Market_List != null)
                    {
                        seller_parameter_list obj = new seller_parameter_list();
                        obj.id = seller_Market_List.id.ToString();
                        obj.my_unique_id = seller_Market_List.my_unique_id.ToString(); ///// SELLER ID 
                        obj.my_seller_id = seller_Market_List.tbl_seller_id.ToString(); ///// SELLER ID - Primary Key Value                           
                        obj.t_access_Key_id = seller_Market_List.t_access_Key_id.ToString();
                        obj.t_auth_token = seller_Market_List.t_auth_token.ToString();
                        obj.t_secret_Key = seller_Market_List.t_secret_Key.ToString();
                        obj.market_palce_id = seller_Market_List.market_palce_id.ToString();
                        obj.m_marketplace_id = seller_Market_List.m_marketplace_id.ToString();
                        lstSeller.Add(obj);
                    }
                    if (lstSeller.Count > 0)
                    {
                        strMsg = Start_API_Calling_Product(lstSeller, txt_from, txt_to, id);
                    }
                }
            }
            catch
            {
                strMsg = "F";
            }

            return strMsg;
        }

        public string Start_API_Calling_Product(List<seller_parameter_list> lstSellerData, DateTime? txt_from, DateTime? txt_to, int id)
        {
            string strStatus = "S";
            try
            {
                foreach (var seller in lstSellerData)
                {
                    api_parameter objApi = new api_parameter();
                    DateTime dt = System.DateTime.Now.AddMinutes(-2);
                    DateTime DtStart;
                    DateTime DtEnd;
                    int Sid = Convert.ToInt32(seller.my_seller_id.ToString());
                    objApi.id = Sid;
                    objApi.Seller_ID = seller.my_unique_id.ToString();
                    objApi.Secret_Key = seller.t_secret_Key.ToString();
                    objApi.AWS_Access_Key_ID = seller.t_access_Key_id.ToString();
                    objApi.marketplaceid = seller.market_palce_id.ToString();
                    objApi.m_marketplace_id = Convert.ToInt32(seller.m_marketplace_id.ToString());
                    objApi.auth_token = "";


                    //DtStart = DateTime.Parse("2018-01-22 23:59:59");
                    //DtEnd = DateTime.Parse("2018-01-23 23:59:59");
                    DtStart = Convert.ToDateTime(txt_from);
                    DtEnd = Convert.ToDateTime(txt_to);
                    //DtStart = DateTime.Parse("2017-12-01 23:59:59");//DtStart = Convert.ToDateTime(txt_from);//.AddHours(-24);
                    // DtEnd = DateTime.Parse("2017-12-31 23:59:59"); //DtEnd = Convert.ToDateTime(txt_to);
                    objApi.createafter = DtStart; //objApi.createafter = Convert.ToDateTime(txt_from);                
                    objApi.createbefore = DtEnd;//objApi.createbefore = Convert.ToDateTime(txt_to);

                    if (DtEnd <= System.DateTime.Now)
                    {
                        //return "F";
                        List<api_parameter> lstApiDetails = new List<api_parameter>();
                        lstApiDetails.Add(objApi);

                        //Amazon objAmazonClass = new Amazon();
                        ProductListingAPI objproductapi = new ProductListingAPI();
                        string strResponseXMLFormat = objproductapi.Call_API_Product(lstApiDetails, Sid, id);
                    }
                }
            }
            catch
            {
                strStatus = "F";
            }
            return strStatus;
        }
        # endregion
    }
}