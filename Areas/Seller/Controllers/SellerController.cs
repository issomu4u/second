using SellerVendor.Areas.Seller.Models;
using SellerVendor.Areas.Seller.Models.DBContext;
using SellerVendor.Models;
using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.Validation;
using System.Diagnostics;
using System.IO;
using System.Linq;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Mvc;
using System.Globalization;
using System.Configuration;
using MySql.Data.MySqlClient;
using System.Data;

namespace SellerVendor.Areas.Seller.Controllers
{
    public class SellerController : Controller
    {
        public SellerContext dba = new SellerContext();
        public SellerAdminContext db = new SellerAdminContext();
        comman_function cf = null;
        public ActionResult Index()
        {
            DateTime now = DateTime.Now;
            cf = new comman_function();
            if (!cf.session_check())
                return RedirectToAction("Login", "Home");

            DashboardData objdash = new DashboardData();

            int sumamount = Convert.ToInt32(Session["Warning"]);
            objdash.UserAmount = sumamount;
            int SellerId = Convert.ToInt32(Session["SellerID"]);
            DateTime TodayDate = DateTime.Now;  // will give you today's date
            DateTime fromdate = TodayDate.Date.AddDays(-1);
            DateTime todate = fromdate.Add(TimeSpan.Parse("23:59:59"));

            DateTime todayfromdate = TodayDate.Date;
            DateTime todaytodate = todayfromdate.Add(TimeSpan.Parse("23:59:59"));

            var get_saleDetails = dba.tbl_sales_order.Where(a => a.tbl_sellers_id == SellerId && a.is_active == 1 && a.created_on >= fromdate && a.created_on <= todate).ToList();// to get previous amount details
            double previousamount = 0, todayamount = 0;

            if (get_saleDetails != null)
            {
                foreach (var item in get_saleDetails)
                {
                    previousamount = previousamount + item.bill_amount;
                }// end of foreach(item)
            }// end of if(get_saleDetails)

            var get_todaysaleDetails = dba.tbl_sales_order.Where(a => a.tbl_sellers_id == SellerId && a.is_active == 1 && a.created_on >= todayfromdate && a.created_on <= todaytodate).ToList();// to get current amount details
            if (get_todaysaleDetails != null)
            {
                foreach (var item1 in get_todaysaleDetails)
                {
                    todayamount = todayamount + item1.bill_amount;
                }// end of foreach(item1)
            }// end of if(get_todaysaleDetails)
            var get_salepreviousItemCount = dba.tbl_sales_order.Where(a => a.tbl_sellers_id == SellerId && a.is_active == 1 && a.created_on >= fromdate && a.created_on <= todate).Count();// to get previous item count

            if (get_salepreviousItemCount > 0)
            {
                objdash.previousItem = get_salepreviousItemCount;
            }
            var get_salecurrentItemCount = dba.tbl_sales_order.Where(a => a.tbl_sellers_id == SellerId && a.is_active == 1 && a.created_on >= todayfromdate && a.created_on <= todaytodate).Count();// to get current item count
            if (get_salecurrentItemCount > 0)
            {
                objdash.currentItem = get_salecurrentItemCount;
            }
            var get_salependingorderCount = dba.tbl_sales_order_details.Where(a => a.tbl_seller_id == SellerId && a.is_active == 1 && a.n_order_status_id == 1).Count();// to get pending order count
            if (get_salependingorderCount > 0)
            {
                objdash.pendingorder = get_salependingorderCount;
            }
            objdash.PreviousAmount = previousamount;
            objdash.CurrentAmount = todayamount;

            var get_allsaleorders = dba.tbl_sales_order.Where(a => a.tbl_sellers_id == SellerId).Count();
            if (get_allsaleorders > 0)
            {
                objdash.TotalOrders = get_allsaleorders;
            }
            var get_allsalecancelledorders = dba.tbl_sales_order.Where(a => a.tbl_sellers_id == SellerId &&  (a.order_status=="Canceled" || a.order_status =="Cancelled")).Count();
            if (get_allsalecancelledorders > 0)
            {
                objdash.TotalCancelledOrders = get_allsalecancelledorders;
            }
            var get_allsettlementorders = dba.tbl_settlement_order.Where(a => a.tbl_seller_id == SellerId).Count();
            if (get_allsettlementorders > 0)
            {
                objdash.TotalSettlementOrders = get_allsettlementorders;
            }

            var get_allrefundorders = dba.tbl_order_history.Where(a => a.tbl_seller_id == SellerId && a.t_order_status == 9).Count();
            if (get_allrefundorders > 0)
            {
                objdash.TotalRefundOrders = get_allrefundorders;
            }

            var get_allreturnorders = dba.tbl_order_history.Where(a => a.tbl_seller_id == SellerId && a.t_order_status == 10).Count();
            if (get_allreturnorders > 0)
            {
                objdash.TotalReturnOrders = get_allreturnorders;
            }
            return View(objdash);
            //return View();
        }




        #region dashboard

        public JsonResult GetDashboardData()
        {
            DashboardData objdash = new DashboardData();

            int sumamount = Convert.ToInt32(Session["Warning"]);
            objdash.UserAmount = sumamount;
            int SellerId = Convert.ToInt32(Session["SellerID"]);
            DateTime TodayDate = DateTime.Now;  // will give you today's date
            DateTime fromdate = TodayDate.Date.AddDays(-1);
            DateTime todate = fromdate.Add(TimeSpan.Parse("23:59:59"));

            DateTime todayfromdate = TodayDate.Date;
            DateTime todaytodate = todayfromdate.Add(TimeSpan.Parse("23:59:59"));

            var get_saleDetails = dba.tbl_sales_order.Where(a => a.tbl_sellers_id == SellerId && a.is_active == 1 && a.created_on >= fromdate && a.created_on <= todate).ToList();// to get previous amount details
            double previousamount = 0, todayamount = 0;          
            if (get_saleDetails != null)
            {
                foreach (var item in get_saleDetails)
                {
                    previousamount = previousamount + item.bill_amount;
                }// end of foreach(item)
            }// end of if(get_saleDetails)

            var get_todaysaleDetails = dba.tbl_sales_order.Where(a => a.tbl_sellers_id == SellerId && a.is_active == 1 && a.created_on >= todayfromdate && a.created_on <= todaytodate).ToList();// to get current amount details
            if (get_todaysaleDetails != null)
            {
                foreach (var item1 in get_todaysaleDetails)
                {
                    todayamount = todayamount + item1.bill_amount;
                }// end of foreach(item1)
            }// end of if(get_todaysaleDetails)
            var get_salepreviousItemCount = dba.tbl_sales_order.Where(a => a.tbl_sellers_id == SellerId && a.is_active == 1 && a.created_on >= fromdate && a.created_on <= todate).Count();// to get previous item count

            if (get_salepreviousItemCount > 0)
            {
                objdash.previousItem = get_salepreviousItemCount;
            }
            var get_salecurrentItemCount = dba.tbl_sales_order.Where(a => a.tbl_sellers_id == SellerId && a.is_active == 1 && a.created_on >= todayfromdate && a.created_on <= todaytodate).Count();// to get current item count
            if (get_salecurrentItemCount > 0)
            {
                objdash.currentItem = get_salecurrentItemCount;
            }
            var get_salependingorderCount = dba.tbl_sales_order_details.Where(a => a.tbl_seller_id == SellerId && a.is_active == 1 && a.n_order_status_id == 1).Count();// to get pending order count
            if (get_salependingorderCount > 0)
            {
                objdash.pendingorder = get_salependingorderCount;
            }
            objdash.PreviousAmount = previousamount;
            objdash.CurrentAmount = todayamount;

            var get_allsaleorders = dba.tbl_sales_order.Where(a => a.tbl_sellers_id == SellerId).Count();
            if (get_allsaleorders > 0)
            {
                objdash.TotalOrders = get_allsaleorders;
            }
            var get_allsalecancelledorders = dba.tbl_sales_order.Where(a => a.tbl_sellers_id == SellerId && a.order_status == "Cancelled").Count();
            if (get_allsalecancelledorders > 0)
            {
                objdash.TotalCancelledOrders = get_allsalecancelledorders;
            }
            var get_allsettlementorders = dba.tbl_settlement_order.Where(a => a.tbl_seller_id == SellerId).Count();
            if (get_allsettlementorders > 0)
            {
                objdash.TotalSettlementOrders = get_allsettlementorders;
            }

            var get_allrefundorders = dba.tbl_order_history.Where(a => a.tbl_seller_id == SellerId && a.t_order_status == 9).Count();
            if (get_allrefundorders > 0)
            {
                objdash.TotalRefundOrders = get_allrefundorders;
            }

            var get_allreturnorders = dba.tbl_order_history.Where(a => a.tbl_seller_id == SellerId && a.t_order_status == 10).Count();
            if (get_allreturnorders > 0)
            {
                objdash.TotalReturnOrders = get_allreturnorders;
            }
            return Json(objdash, JsonRequestBehavior.AllowGet);
        }



        #endregion

        public void DeleteSettlementfile()//it is used when we want to delete data 
        {
            try
            {
                int sellers_id = 0;//Convert.ToInt32(Session["SellerID"]);
                string settlement_refernece_no = "";
                tbl_settlement_upload obj_upload = new tbl_settlement_upload();
                string connectionstring = ConfigurationManager.ConnectionStrings["SellerConnection"].ConnectionString;
                MySqlConnection con = new MySqlConnection(connectionstring);

                var get_order = " select * from tbl_settlement_order where  tbl_seller_id = " + sellers_id + " and settlement_id =" + settlement_refernece_no + "";
                MySqlDataAdapter da1 = new MySqlDataAdapter(get_order, con);
                DataTable dtexpense1 = new DataTable();
                da1.Fill(dtexpense1);
                da1.Dispose();
                con.Open();
                MySqlCommand cmd1 = null;
                for (int i = 0; i < dtexpense1.Rows.Count; i++)
                {
                    var strabc = " delete from tbl_settlement_order where id=" + dtexpense1.Rows[i]["id"];
                    cmd1 = new MySqlCommand(strabc, con);
                    cmd1.ExecuteNonQuery();
                    cmd1.Dispose();
                }

                var get_upload = " select * from tbl_settlement_upload where  tbl_seller_id = " + sellers_id + " and settlement_refernece_no =" + settlement_refernece_no + "";
                MySqlDataAdapter da2 = new MySqlDataAdapter(get_upload, con);
                DataTable dtexpense2 = new DataTable();
                da2.Fill(dtexpense2);
                da2.Dispose();
                //con.Open();
                MySqlCommand cmd2 = null;
                for (int i = 0; i < dtexpense2.Rows.Count; i++)
                {
                    string strtxt = "delete from tbl_imp_bank_transfers where tbl_settlement_upload_id=" + dtexpense2.Rows[i]["id"] + "";
                    cmd2 = new MySqlCommand(strtxt, con);
                    cmd2.ExecuteNonQuery();
                    cmd2.Dispose();
                    var strabc = " delete from tbl_settlement_upload where id=" + dtexpense2.Rows[i]["id"];
                    cmd2 = new MySqlCommand(strabc, con);
                    cmd2.ExecuteNonQuery();
                    cmd2.Dispose();
                }

                var get_history = " select * from tbl_order_history where  tbl_seller_id = " + sellers_id + " and settlement_id =" + settlement_refernece_no + "";
                MySqlDataAdapter da3 = new MySqlDataAdapter(get_history, con);
                DataTable dtexpense3 = new DataTable();
                da3.Fill(dtexpense3);
                da3.Dispose();
                //con.Open();
                MySqlCommand cmd3 = null;
                for (int i = 0; i < dtexpense3.Rows.Count; i++)
                {
                    //string strtxt = "delete from tbl_tax where tbl_seller_id=" + sellers_id + " and tbl_referneced_id=" + dtexpense3.Rows[i]["id"] + "";
                    //cmd3 = new MySqlCommand(strtxt, con);
                    //cmd3.ExecuteNonQuery();
                    //cmd3.Dispose();
                    var strabc = " delete from tbl_order_history where id=" + dtexpense3.Rows[i]["id"];
                    cmd3 = new MySqlCommand(strabc, con);
                    cmd3.ExecuteNonQuery();
                    cmd3.Dispose();
                }
                var get_expense = " select * from m_tbl_expense where  tbl_seller_id = " + sellers_id + " and reference_number =" + settlement_refernece_no + "";
                MySqlDataAdapter da = new MySqlDataAdapter(get_expense, con);
                DataTable dtexpense = new DataTable();
                da.Fill(dtexpense);
                da.Dispose();
                //con.Open();
                MySqlCommand cmd = null;
                for (int i = 0; i < dtexpense.Rows.Count; i++)
                {
                    string strtxt = "delete from tbl_tax where tbl_seller_id=" + sellers_id + " and tbl_referneced_id=" + dtexpense.Rows[i]["id"] + " and reference_type =2 or reference_type =7";
                    cmd = new MySqlCommand(strtxt, con);
                    cmd.ExecuteNonQuery();
                    cmd.Dispose();
                    var strabc = " delete from m_tbl_expense where id=" + dtexpense.Rows[i]["id"];
                    cmd = new MySqlCommand(strabc, con);
                    cmd.ExecuteNonQuery();
                    cmd.Dispose();
                    //MySqlDataAdapter dt2 = new MySqlDataAdapter(strtxt,con);
                    //DataTable dt_tax = new DataTable();
                    //dt2.Fill(dt_tax);
                    //dt2.Dispose();
                }
                con.Close();
                //return Json(JsonRequestBehavior.AllowGet);
            }
            catch(Exception ex)
            {
            }
        }


        public JsonResult GetLastOrder(int days)
        {
            cf = new comman_function();
            bool ss = cf.session_check();
            int sellerid = Convert.ToInt32(Session["SellerID"]);
            List<LastOrders> LastOrders = new List<LastOrders>();
            var GetScannedUsers = dba.tbl_sales_order.Where(a => a.tbl_sellers_id == sellerid).ToList();
            DateTime last5Date = DateTime.Now;
            for (int i = 0; i < days; i++)
            {
                DateTime PickDate = last5Date.AddDays(-i);
                LastOrders order = new LastOrders();
                order.Date = PickDate.ToString("dd-MMM");
                order.TotalOrders = GetScannedUsers.Where(a => a.purchase_date.Date == PickDate.Date).ToList().Count;
                LastOrders.Add(order);
            }
            return Json(LastOrders, JsonRequestBehavior.AllowGet);
        }
        public PartialViewResult PartialTopSeller()
        {
            //SELECT sku_no, id, tbl_sales_order_id count( * ) AS total FROM tbl_sales_order_details WHERE sku_no IS NOT NULL AND tbl_seller_id =" + sellerid + " GROUP BY sku_no ORDER BY count( * ) DESC LIMIT 5

            cf = new comman_function();
            bool ss = cf.session_check();
            List<TopProduct> lstProduct = new List<TopProduct>();
            int sellerid = Convert.ToInt32(Session["SellerID"]);


            string connectionstring = ConfigurationManager.ConnectionStrings["SellerConnection"].ConnectionString;
            MySqlConnection con = new MySqlConnection(connectionstring);

            var get_unique_count_details = "SELECT sku_no,product_name, id, tbl_sales_order_id, count( * ) AS total FROM tbl_sales_order_details WHERE sku_no IS NOT NULL AND tbl_seller_id =" + sellerid + " GROUP BY sku_no ORDER BY count( * ) DESC LIMIT 5";
            MySqlDataAdapter da1 = new MySqlDataAdapter(get_unique_count_details, con);

            DataTable dtexpense1 = new DataTable();
            da1.Fill(dtexpense1);
            da1.Dispose();
            con.Open();
            MySqlCommand cmd1 = null;
            for (int i = 0; i < dtexpense1.Rows.Count; i++)
            {
                TopProduct product = new TopProduct();
                int id =Convert.ToInt32(dtexpense1.Rows[i]["tbl_sales_order_id"]);             
                product.SKU = Convert.ToString(dtexpense1.Rows[i]["sku_no"]);// i.SKU;
                product.Quantity = Convert.ToInt16(dtexpense1.Rows[i]["total"]);
                product.Name = Convert.ToString(dtexpense1.Rows[i]["product_name"]);

                var get_sale_order = dba.tbl_sales_order.Where(a => a.id == id && a.tbl_sellers_id == sellerid).FirstOrDefault();
                if (get_sale_order != null)
                {
                    product.Price =Convert.ToDecimal(get_sale_order.bill_amount);
                }
                if (product.Quantity > 0)
                    lstProduct.Add(product);
            }
            if (lstProduct.Count > 5)
            {
                lstProduct = lstProduct.OrderByDescending(a => a.Quantity).Take(5).ToList();
            }




            //cf = new comman_function();
            //bool ss = cf.session_check();
            //List<TopProduct> lstProduct = new List<TopProduct>();
            //int sellerid = Convert.ToInt32(Session["SellerID"]);

            //var get_saleorder_details = dba.tbl_sales_order_details.Where(a => a.tbl_seller_id == sellerid).ToList();
            //if (get_saleorder_details != null)
            //{
            //    foreach (var item in get_saleorder_details)
            //    {
            //        TopProduct product = new TopProduct();
            //        string sku = item.sku_no;
            //        string unitprice = Convert.ToString(item.item_price_amount);
            //        //var 
            //        product.Name = sku;
            //        decimal amount = Convert.ToDecimal(item.item_price_amount);
            //        if (amount < 0)
            //            amount = amount * -1;
            //        product.Price = amount;
            //        product.Quantity = get_saleorder_details.Where(a => a.sku_no == item.sku_no).GroupBy(x => x.sku_no).Select(x => x.Count()).FirstOrDefault();
            //        if (product.Quantity > 0)
            //            lstProduct.Add(product);
            //    }
            //}
            //if (lstProduct.Count > 5)
            //{
            //    lstProduct = lstProduct.OrderByDescending(a => a.Quantity).Take(5).ToList();
            //}
            return PartialView(lstProduct);
        }
        public PartialViewResult PartialTopCustomer()
        {
            cf = new comman_function();
            bool ss = cf.session_check();
            List<TopProduct> lstProduct = new List<TopProduct>();
            int sellerid = Convert.ToInt32(Session["SellerID"]);

            var get_saleorder_details = dba.tbl_sales_order.Where(a => a.tbl_sellers_id == sellerid).ToList();
            var Customer = dba.tbl_customer_details.Where(a => a.tbl_seller_id == sellerid).ToList();
            foreach (var item in Customer)
            {
                TopProduct product = new TopProduct();
                product.Name = item.shipping_Buyer_Name;
                product.Quantity = get_saleorder_details.Where(a => a.tbl_Customer_Id == item.id).GroupBy(x => x.tbl_Customer_Id).Select(x => x.Count()).FirstOrDefault();
                if (product.Quantity > 0)
                    lstProduct.Add(product);
            }
            if (lstProduct.Count > 5)
            {
                lstProduct = lstProduct.OrderByDescending(a => a.Quantity).Take(5).ToList();
            }
            return PartialView(lstProduct);
        }


        public PartialViewResult PartialTopReturn()
        {
            cf = new comman_function();
            bool ss = cf.session_check();
            List<TopProduct> lstProduct = new List<TopProduct>();
            int sellerid = Convert.ToInt32(Session["SellerID"]);


            string connectionstring = ConfigurationManager.ConnectionStrings["SellerConnection"].ConnectionString;
            MySqlConnection con = new MySqlConnection(connectionstring);

            var get_unique_count_details = "SELECT SKU,Id, count(*)  AS total  FROM tbl_order_history WHERE SKU IS NOT NULL and tbl_seller_id  = " + sellerid + " GROUP BY SKU ORDER BY count(*) DESC LIMIT 5";
            MySqlDataAdapter da1 = new MySqlDataAdapter(get_unique_count_details, con);

            DataTable dtexpense1 = new DataTable();
            da1.Fill(dtexpense1);
            da1.Dispose();
            con.Open();
            MySqlCommand cmd1 = null;
            for (int i = 0; i < dtexpense1.Rows.Count; i++)
            {
                TopProduct product = new TopProduct();
                product.SKU = Convert.ToString(dtexpense1.Rows[i]["SKU"]);// i.SKU;
                product.Quantity = Convert.ToInt32(dtexpense1.Rows[i]["total"]);//get_order_history.Where(a => a.SKU == item.SKU).GroupBy(x => x.SKU).Select(x => x.Count()).FirstOrDefault();
                var get_inventory = dba.tbl_inventory.Where(a => a.sku == product.SKU && a.tbl_sellers_id == sellerid).FirstOrDefault();
                if (get_inventory != null)
                {
                    product.Name = get_inventory.item_name;
                }
                if (product.Quantity > 0)
                    lstProduct.Add(product);
            }
            if (lstProduct.Count > 5)
            {
                lstProduct = lstProduct.OrderByDescending(a => a.Quantity).Take(5).ToList();
            }

            //var get_unique_count_details = "SELECT count(DISTINCT aa.id) FROM(SELECT DISTINCT Order_Id AS id FROM `tbl_settlement_order` where tbl_seller_id = " + SellerId + " UNION ALL SELECT DISTINCT amazon_order_id AS id FROM `tbl_sales_order` where tbl_sellers_id =" + SellerId + " ) AS aa";
            //var get_order_history = dba.tbl_order_history.Where(a => a.tbl_seller_id == sellerid).ToList();
            //foreach (var item in get_order_history)
            //{
            //    TopProduct product = new TopProduct();
            //    product.SKU = item.SKU;
            //    product.Quantity = get_order_history.Where(a => a.SKU == item.SKU).GroupBy(x => x.SKU).Select(x => x.Count()).FirstOrDefault();
            //    var get_inventory = dba.tbl_inventory.Where(a => a.sku == item.SKU && a.tbl_sellers_id == sellerid).FirstOrDefault();
            //    if (get_inventory != null)
            //    {
            //        product.Name = get_inventory.item_name;
            //    }
            //    if (product.Quantity > 0)
            //        lstProduct.Add(product);
            //}
            //if (lstProduct.Count > 5)
            //{
            //    lstProduct = lstProduct.OrderByDescending(a => a.Quantity).Take(5).ToList();
            //}
            return PartialView(lstProduct);
        }

        public PartialViewResult PartialBottomSeller()
        {
            cf = new comman_function();
            bool ss = cf.session_check();
            List<TopProduct> lstProduct = new List<TopProduct>();
            int sellerid = Convert.ToInt32(Session["SellerID"]);


            string connectionstring = ConfigurationManager.ConnectionStrings["SellerConnection"].ConnectionString;
            MySqlConnection con = new MySqlConnection(connectionstring);

            var get_unique_count_details = "SELECT SKU,Id, count(*)  AS total  FROM tbl_order_history WHERE SKU IS NOT NULL and tbl_seller_id  = " + sellerid + " GROUP BY SKU ORDER BY count(*) DESC LIMIT 5";
            MySqlDataAdapter da1 = new MySqlDataAdapter(get_unique_count_details, con);

            DataTable dtexpense1 = new DataTable();
            da1.Fill(dtexpense1);
            da1.Dispose();
            con.Open();
            MySqlCommand cmd1 = null;
            for (int i = 0; i < dtexpense1.Rows.Count; i++)
            {
                TopProduct product = new TopProduct();
                product.SKU = Convert.ToString(dtexpense1.Rows[i]["SKU"]);// i.SKU;
                product.Quantity = Convert.ToInt32(dtexpense1.Rows[i]["total"]);//get_order_history.Where(a => a.SKU == item.SKU).GroupBy(x => x.SKU).Select(x => x.Count()).FirstOrDefault();
                var get_inventory = dba.tbl_inventory.Where(a => a.sku == product.SKU && a.tbl_sellers_id == sellerid).FirstOrDefault();
                if (get_inventory != null)
                {
                    product.Name = get_inventory.item_name;
                }
                if (product.Quantity > 0)
                    lstProduct.Add(product);
            }
            if (lstProduct.Count > 5)
            {
                lstProduct = lstProduct.OrderByDescending(a => a.Quantity).Take(5).ToList();
            }

           
            return PartialView(lstProduct);
        }

        #region Bargraph
        /// <summary>
        /// this is for Bragraph for dummy only
        /// </summary>
        /// <returns></returns>
        public ActionResult BarGraph()
        {
            return View(ChartData.GetData());

        }

        [HttpPost]
        public JsonResult NewChart()
        {
            int SellerId = Convert.ToInt32(Session["SellerID"]);
            comman_function cf = new comman_function();
            List<string> lstMonth = new List<string>();
            List<string> lstYear = new List<string>();
            List<string> lststartdate = new List<string>();
            List<string> lstenddate = new List<string>();

            for (int i = 0; i <= 5; i++)
            {
                lstMonth.Add(cf.GetMonth(DateTime.Now.Month - i));
                lstYear.Add(cf.GetYear(DateTime.Now.Month - i));
            }
            foreach (var item in lstMonth)
            {
                lstenddate.Add(cf.GetEndDate(item));
            }

            List<LastOrders> iData = new List<LastOrders>();
            for (int i = 0; i <= 5; i++)
            {
                //string startdate = string.Format("{0}-{1}-{2}","01",lstMonth[i], lstYear[i]);
                //string enddate = string.Format("{0}-{1}-{2}", lstenddate[i], lstMonth[i], lstYear[i]);


                string startdate = string.Format("{0}-{1}-{2}", lstYear[i], lstMonth[i], "01");
                string enddate = string.Format("{0}-{1}-{2}", lstYear[i], lstMonth[i], lstenddate[i]);

                var GetScannedUsers = dba.tbl_sales_order.Where(a => a.tbl_sellers_id == SellerId).ToList();
                var GetAllReturns = dba.tbl_order_history.Where(a => a.tbl_seller_id == SellerId).ToList();

                LastOrders order = new LastOrders();
                if (lstMonth[i] == "01")
                {
                    order.Date = "January";
                }
                else if (lstMonth[i] == "02")
                {
                    order.Date = "February";
                }
                else if (lstMonth[i] == "03")
                {
                    order.Date = "March";
                }
                else if (lstMonth[i] == "04")
                {
                    order.Date = "April";
                }
                else if (lstMonth[i] == "05")
                {
                    order.Date = "May";
                }
                else if (lstMonth[i] == "06")
                {
                    order.Date = "June";
                }
                else if (lstMonth[i] == "07")
                {
                    order.Date = "July";
                }
                else if (lstMonth[i] == "08")
                {
                    order.Date = "August";
                }
                else if (lstMonth[i] == "09")
                {
                    order.Date = "September";
                }
                else if (lstMonth[i] == "10")
                {
                    order.Date = "October";
                }
                else if (lstMonth[i] == "11")
                {
                    order.Date = "November";
                }
                else if (lstMonth[i] == "12")
                {
                    order.Date = "December";
                }
                order.TotalOrders = GetScannedUsers.Where(a => a.purchase_date.Date >= Convert.ToDateTime(startdate) && a.purchase_date.Date <= Convert.ToDateTime(enddate)).ToList().Count;

                order.TotalReturn = GetAllReturns.Where(a => Convert.ToDateTime(a.ShipmentDate).Date >= Convert.ToDateTime(startdate) && Convert.ToDateTime(a.ShipmentDate).Date <= Convert.ToDateTime(enddate)).ToList().Count;
                iData.Add(order);

            }

            return Json(iData, JsonRequestBehavior.AllowGet);

            //List<object> iData = new List<object>();

            //DateTime GetDate = new DateTime();

            ////Creating sample data  
            //DataTable dt = new DataTable();
            //dt.Columns.Add("Employee", System.Type.GetType("System.String"));
            //dt.Columns.Add("Credit", System.Type.GetType("System.Int32"));

            //DataRow dr = dt.NewRow();
            //dr["Employee"] = "Sam";
            //dr["Credit"] = 123;
            //dt.Rows.Add(dr);

            //dr = dt.NewRow();
            //dr["Employee"] = "Alex";
            //dr["Credit"] = 456;
            //dt.Rows.Add(dr);

            //dr = dt.NewRow();
            //dr["Employee"] = "Michael";
            //dr["Credit"] = 587;
            //dt.Rows.Add(dr);

            //dr = dt.NewRow();
            //dr["Employee"] = "Vineet Gahlot";
            //dr["Credit"] = 600;
            //dt.Rows.Add(dr);
            ////Looping and extracting each DataColumn to List<Object>  
            //foreach (DataColumn dc in dt.Columns)
            //{
            //    List<object> x = new List<object>();
            //    x = (from DataRow drr in dt.Rows select drr[dc.ColumnName]).ToList();
            //    iData.Add(x);
            //}
            ////Source data returned as JSON  
            //return Json(iData, JsonRequestBehavior.AllowGet);
        }


        public JsonResult GetNetRealization()
        {
            int SellerId = Convert.ToInt32(Session["SellerID"]);
            comman_function cf = new comman_function();
            List<string> lstMonth = new List<string>();
            List<string> lstYear = new List<string>();
            List<string> lststartdate = new List<string>();
            List<string> lstenddate = new List<string>();

            for (int i = 0; i <= 5; i++)
            {
                lstMonth.Add(cf.GetMonth(DateTime.Now.Month - i));
                lstYear.Add(cf.GetYear(DateTime.Now.Month - i));
            }
            foreach (var item in lstMonth)
            {
                lstenddate.Add(cf.GetEndDate(item));
            }

            List<LastOrders> iData = new List<LastOrders>();
            for (int i = 0; i <= 5; i++)
            {             
                string startdate = string.Format("{0}-{1}-{2}", lstYear[i], lstMonth[i], "01");
                string enddate = string.Format("{0}-{1}-{2}", lstYear[i], lstMonth[i], lstenddate[i]);

                var GetScannedUsers = dba.tbl_sales_order.Where(a => a.tbl_sellers_id == SellerId).ToList();
                var GetAllExpences = dba.m_tbl_expense.Where(a => a.tbl_seller_id == SellerId).ToList();

                LastOrders order = new LastOrders();
                if (lstMonth[i] == "01")
                {
                    order.Date = "January";
                }
                else if (lstMonth[i] == "02")
                {
                    order.Date = "February";
                }
                else if (lstMonth[i] == "03")
                {
                    order.Date = "March";
                }
                else if (lstMonth[i] == "04")
                {
                    order.Date = "April";
                }
                else if (lstMonth[i] == "05")
                {
                    order.Date = "May";
                }
                else if (lstMonth[i] == "06")
                {
                    order.Date = "June";
                }
                else if (lstMonth[i] == "07")
                {
                    order.Date = "July";
                }
                else if (lstMonth[i] == "08")
                {
                    order.Date = "August";
                }
                else if (lstMonth[i] == "09")
                {
                    order.Date = "September";
                }
                else if (lstMonth[i] == "10")
                {
                    order.Date = "October";
                }
                else if (lstMonth[i] == "11")
                {
                    order.Date = "November";
                }
                else if (lstMonth[i] == "12")
                {
                    order.Date = "December";
                }
                decimal expenseamount = 0, expenseamount1 = 0, netvalue = 0; ;
                   double todayamount = 0;               
                if (GetScannedUsers != null)
                {
                    var list = GetScannedUsers.Where(a => a.purchase_date.Date >= Convert.ToDateTime(startdate) && a.purchase_date.Date <= Convert.ToDateTime(enddate)).ToList();
                    foreach (var item1 in list)
                    {                       
                        todayamount = todayamount + item1.bill_amount;
                    }// end of foreach(item1)
                }// end of if(get_saleDetails)
               
                if (GetAllExpences != null)
                {
                    var explist = GetAllExpences.Where(a => a.tbl_seller_id == SellerId && a.settlement_datetime >= Convert.ToDateTime(startdate) && a.settlement_datetime <= Convert.ToDateTime(enddate)).ToList();
                    foreach (var expenseitem in explist)
                    {
                        decimal expamt = Convert.ToDecimal(expenseitem.expense_amount);
                        expenseamount1 = Convert.ToDecimal(expenseamount1 + expamt);
                         decimal result = decimal.Round(expenseamount1, 2, MidpointRounding.AwayFromZero);

                    }
                }

                decimal amount =Convert.ToDecimal(todayamount);
                decimal amtresult = decimal.Round(amount, 2, MidpointRounding.AwayFromZero);
                order.TotalAmount =Convert.ToDouble(amtresult);

                if (expenseamount1 >= 0)
                    order.TotalExpences = Convert.ToDouble(expenseamount1);
                else
                    order.TotalExpences = Convert.ToDouble(expenseamount1) *(-1);

                netvalue =Convert.ToDecimal(order.TotalAmount - order.TotalExpences);
                decimal result1 = decimal.Round(netvalue, 2, MidpointRounding.AwayFromZero);
                order.NetRealization =Convert.ToDouble(result1);
                            
                iData.Add(order);

            }

            return Json(iData, JsonRequestBehavior.AllowGet);

            
        }  
        #endregion
        /// <summary>
        /// This is for Add seller Vendor Details
        /// </summary>
        /// <returns></returns>
        public ActionResult SellerVendor()
        {
            //DeleteSettlementfile();
            cf = new comman_function();
            if (!cf.session_check())
                return RedirectToAction("Login", "Home");
            return View();
        }


        public JsonResult GetSellerVendor()
        {
            cf = new comman_function();
            bool ss = cf.session_check();

            int Sellerid = Convert.ToInt32(Session["SellerID"]);
            //var marketList = dba.tbl_seller_vendors.Where(a => a.status == 1).ToList();

            //return new JsonResult { Data = marketList, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
            var get_vendordetails = (from ob_tbl_seller_vendors in dba.tbl_seller_vendors
                                     //join ob_tbl_sellers in db.tbl_sellers on ob_tbl_seller_vendors.tbl_sellers_id
                                     //equals ob_tbl_sellers.id
                                     //join ob_tbl_country in db.tbl_country on
                                     //    ob_tbl_seller_vendors.country equals ob_tbl_country.id
                                     select new SellerUtility
                                     {
                                         //ob_tbl_sellers = ob_tbl_sellers,
                                         //ob_tbl_country = ob_tbl_country,
                                         ob_tbl_seller_vendors = ob_tbl_seller_vendors

                                     }).Where(a => a.ob_tbl_seller_vendors.status == 1 && a.ob_tbl_seller_vendors.tbl_sellersid == Sellerid).OrderByDescending(a => a.ob_tbl_seller_vendors.id).ToList();

            return new JsonResult { Data = get_vendordetails, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }


        [HttpPost]
        public JsonResult SaveSellerVendorDetails(tbl_seller_vendors objtblsellervendors)
        {
            string Message;
            bool flag = false;
            cf = new comman_function();
            bool ss = cf.session_check();
            try
            {
                objtblsellervendors.date_created = DateTime.Now;
                objtblsellervendors.status = 1;
                objtblsellervendors.tbl_sellersid = Convert.ToInt32(Session["SellerID"]);
                dba.tbl_seller_vendors.Add(objtblsellervendors);
                dba.SaveChanges();
                flag = true;
                Message = "Seller Vendor is created successfully";
            }
            catch (DbEntityValidationException e)
            {
                foreach (var eve in e.EntityValidationErrors)
                {
                    Debug.WriteLine(@"Entity of type has the following validation errors:", eve.Entry.Entity.GetType().Name, eve.Entry.State);
                    foreach (var ve in eve.ValidationErrors)
                    {
                        Debug.WriteLine("- Property: \"{0}\", Error: \"{1}\"",
                         ve.PropertyName, ve.ErrorMessage);
                    }
                }
                throw;
            }
            catch (DbUpdateException dbu)
            {
                var exception = HandleDbUpdateException(dbu);
                throw exception;
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
                throw;
            }
            return new JsonResult { Data = new { Message = Message, Status = flag } };
        }


        [HttpPost]
        public JsonResult UpdateSellerVendorDetails(tbl_seller_vendors objtblsellervendors)
        {
            string Message;
            bool flag = false;
            try
            {
                var get_details = dba.tbl_seller_vendors.Where(a => a.id == objtblsellervendors.id).FirstOrDefault();
                if (get_details != null)
                {
                    get_details.address = objtblsellervendors.address;
                    get_details.vendor_name = objtblsellervendors.vendor_name;
                    get_details.email = objtblsellervendors.email;
                    get_details.mobile = objtblsellervendors.mobile;
                    get_details.contact_person = objtblsellervendors.contact_person;
                    dba.Entry(get_details).State = EntityState.Modified;
                    dba.SaveChanges();
                    flag = true;
                    Message = "Seller  Vendor is updated successfully";
                }
                else
                {
                    flag = false;
                    Message = "some error occurred !!";
                }
            }
            catch (DbEntityValidationException e)
            {
                foreach (var eve in e.EntityValidationErrors)
                {
                    Debug.WriteLine(@"Entity of type has the following validation errors:", eve.Entry.Entity.GetType().Name, eve.Entry.State);
                    foreach (var ve in eve.ValidationErrors)
                    {
                        Debug.WriteLine("- Property: \"{0}\", Error: \"{1}\"",
                            ve.PropertyName, ve.ErrorMessage);
                    }
                }
                throw;
            }
            catch (DbUpdateException dbu)
            {
                var exception = HandleDbUpdateException(dbu);
                throw exception;
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
                throw;
            }

            return new JsonResult { Data = new { Message = Message, Status = flag } };
        }


        public JsonResult DeleteSellerVendor(string id)
        {
            try
            {
                int no = Convert.ToInt32(id);
                tbl_seller_vendors objtblsellervendors = dba.tbl_seller_vendors.Where(a => a.id == no).FirstOrDefault();
                objtblsellervendors.status = 0;
                dba.Entry(objtblsellervendors).State = EntityState.Modified;
                dba.SaveChanges();
                string Message = "Entry is deleted Successfully";
                return Json(Message, JsonRequestBehavior.AllowGet);
            }
            catch (Exception exp)
            {
                return Json("Error in getting record !", JsonRequestBehavior.AllowGet);
            }
        }


        private Exception HandleDbUpdateException(DbUpdateException dbu)
        {
            var builder = new StringBuilder("A DbUpdateException was caught while saving changes. ");
            try
            {
                foreach (var result in dbu.Entries)
                {
                    builder.AppendFormat("Type: {0} was part of the problem. ", result.Entity.GetType().Name);
                }
            }
            catch (Exception e)
            {
                builder.Append("Error parsing DbUpdateException: " + e.ToString());
            }

            string message = builder.ToString();
            return new Exception(message, dbu);
        }

        /// <summary>
        /// This is for Warehouse CRUD
        /// </summary>
        /// <returns></returns>
        public ActionResult Warehouse()
        {
            cf = new comman_function();
            if (!cf.session_check())
                return RedirectToAction("Login", "Home");
            return View();
        }
        public JsonResult GetWarehouse()
        {
            cf = new comman_function();
            bool ss = cf.session_check();
            int Sellerid = Convert.ToInt32(Session["SellerID"]);
            var get_warehousedetails = (from ob_tbl_seller_warehouses in dba.tbl_seller_warehouses
                                        select new SellerUtility
                                        {
                                            ob_tbl_seller_warehouses = ob_tbl_seller_warehouses
                                        }).Where(a => a.ob_tbl_seller_warehouses.isactive == 1 && a.ob_tbl_seller_warehouses.tbl_sellers_id == Sellerid).OrderByDescending(a => a.ob_tbl_seller_warehouses.id).ToList();

            return new JsonResult { Data = get_warehousedetails, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }
        [HttpPost]
        public JsonResult SaveWarehouseDetails(tbl_seller_warehouses objtblsellerwarehouses)
        {
            string Message;
            bool flag = false;
            cf = new comman_function();
            bool ss = cf.session_check();
            try
            {

                //update
                if (objtblsellerwarehouses.n_default_warehouse == 1)
                {
                    var get_details = dba.tbl_seller_warehouses.Where(a => a.n_default_warehouse == 1 && a.isactive == 1).FirstOrDefault();
                    if (get_details != null)
                    {
                        get_details.n_default_warehouse = 0;
                        dba.Entry(get_details).State = EntityState.Modified;
                        dba.SaveChanges();
                    }
                }
                objtblsellerwarehouses.date_created = DateTime.Now;
                objtblsellerwarehouses.isactive = 1;
                objtblsellerwarehouses.tbl_sellers_id = Convert.ToInt32(Session["SellerID"]);
                objtblsellerwarehouses.created_by = Convert.ToInt32(Session["SellerID"]);
                dba.tbl_seller_warehouses.Add(objtblsellerwarehouses);
                dba.SaveChanges();





                flag = true;
                Message = "Warehouse is created successfully";
            }
            catch (DbEntityValidationException e)
            {
                foreach (var eve in e.EntityValidationErrors)
                {
                    Debug.WriteLine(@"Entity of type has the following validation errors:", eve.Entry.Entity.GetType().Name, eve.Entry.State);
                    foreach (var ve in eve.ValidationErrors)
                    {
                        Debug.WriteLine("- Property: \"{0}\", Error: \"{1}\"",
                         ve.PropertyName, ve.ErrorMessage);
                    }
                }
                throw;
            }
            catch (DbUpdateException dbu)
            {
                var exception = HandleDbUpdateException(dbu);
                throw exception;
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
                throw;
            }
            return new JsonResult { Data = new { Message = Message, Status = flag } };
        }

        [HttpPost]
        public JsonResult UpdateWarehouseDetails(tbl_seller_warehouses objtblsellerwarehouses)
        {
            string Message;
            bool flag = false;
            cf = new comman_function();
            bool ss = cf.session_check();
            try
            {
                var get_details = dba.tbl_seller_warehouses.Where(a => a.id == objtblsellerwarehouses.id).FirstOrDefault();
                if (get_details != null)
                {
                    get_details.address = objtblsellerwarehouses.address;
                    get_details.warehouse_name = objtblsellerwarehouses.warehouse_name;
                    get_details.email = objtblsellerwarehouses.email;
                    get_details.mobile = objtblsellerwarehouses.mobile;
                    get_details.contact_person = objtblsellerwarehouses.contact_person;
                    dba.Entry(get_details).State = EntityState.Modified;
                    dba.SaveChanges();
                    flag = true;
                    Message = "Warehouse is updated successfully";
                }
                else
                {
                    flag = false;
                    Message = "some error occurred !!";
                }
            }
            catch (DbEntityValidationException e)
            {
                foreach (var eve in e.EntityValidationErrors)
                {
                    Debug.WriteLine(@"Entity of type has the following validation errors:", eve.Entry.Entity.GetType().Name, eve.Entry.State);
                    foreach (var ve in eve.ValidationErrors)
                    {
                        Debug.WriteLine("- Property: \"{0}\", Error: \"{1}\"",
                            ve.PropertyName, ve.ErrorMessage);
                    }
                }
                throw;
            }
            catch (DbUpdateException dbu)
            {
                var exception = HandleDbUpdateException(dbu);
                throw exception;
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
                throw;
            }

            return new JsonResult { Data = new { Message = Message, Status = flag } };
        }


        public JsonResult DeleteWarehouse(string id)
        {
            try
            {
                int no = Convert.ToInt32(id);
                tbl_seller_warehouses objtblWarehouse = dba.tbl_seller_warehouses.Where(a => a.id == no).FirstOrDefault();
                objtblWarehouse.isactive = 0;
                dba.Entry(objtblWarehouse).State = EntityState.Modified;
                dba.SaveChanges();
                string Message = "Entry is deleted Successfully";
                return Json(Message, JsonRequestBehavior.AllowGet);
            }
            catch (Exception exp)
            {
                return Json("Error in getting record !", JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// This is for Purchase  CRUD Opertaion
        /// </summary>
        /// <returns></returns>
        /// 
        public JsonResult FillWareHouseForPurchase()
        {
            cf = new comman_function();
            bool ss = cf.session_check();
            int SellerId = Convert.ToInt32(Session["SellerID"]);
            var GetWarehouseDetails = dba.tbl_seller_warehouses.Where(a => a.isactive == 1 && a.tbl_sellers_id == SellerId).ToList();
            return new JsonResult { Data = GetWarehouseDetails, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        public JsonResult FillVendorForPurchase()
        {
            cf = new comman_function();
            bool ss = cf.session_check();
            int SellerId = Convert.ToInt32(Session["SellerID"]);
            var GetVendorDetails = dba.tbl_seller_vendors.Where(a => a.status == 1 && a.tbl_sellersid == SellerId).ToList();
            return new JsonResult { Data = GetVendorDetails, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        public ActionResult Purchase()
        {
            cf = new comman_function();
            if (!cf.session_check())
                return RedirectToAction("Login", "Home");
            return View();
        }
        public JsonResult GetPurchase()
        {
            cf = new comman_function();
            bool ss = cf.session_check();
            int SellerId = Convert.ToInt32(Session["SellerID"]);
            var get_purchasedetails = (from ob_tbl_purchase in dba.tbl_purchase
                                       join ob_tbl_seller_warehouses in dba.tbl_seller_warehouses on ob_tbl_purchase.tbl_seller_warehouses_id
                                       equals ob_tbl_seller_warehouses.id
                                       join ob_tbl_seller_vendors in dba.tbl_seller_vendors on
                                       ob_tbl_purchase.tbl_seller_vendors_id equals ob_tbl_seller_vendors.id
                                       select new SellerUtility
                                       {
                                           ob_tbl_purchase = ob_tbl_purchase,
                                           ob_tbl_seller_warehouses = ob_tbl_seller_warehouses,
                                           ob_tbl_seller_vendors = ob_tbl_seller_vendors
                                       }).Where(a => a.ob_tbl_purchase.isactive == 1 && a.ob_tbl_purchase.tbl_sellers_id == SellerId).OrderByDescending(a => a.ob_tbl_purchase.id).ToList();

            return new JsonResult { Data = get_purchasedetails, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        [HttpPost]
        public JsonResult SavePurchaseDetails(tbl_purchase objtblpurchase)
        {
            cf = new comman_function();
            bool ss = cf.session_check();
            string Message, fileName, actualFileName;
            Message = fileName = actualFileName = string.Empty;
            bool flag = false;
            if (objtblpurchase != null)
            {

                if (Request.Files != null)
                {
                    if (Request.Files.Count > 0)
                    {
                        var file = Request.Files[0];
                        actualFileName = file.FileName;
                        string[] getfileName = actualFileName.Split('.');
                        actualFileName = getfileName[0];
                        fileName = actualFileName + DateTime.Now.ToString("yyyyMMddHHmmssfff") + Path.GetExtension(file.FileName);
                        var path = "";
                        path = Path.Combine(Server.MapPath("~/Upload/PurchaseInvoice"), fileName);
                        string getpath = Url.Content(Path.Combine("~/Upload/PurchaseInvoice", fileName));
                        file.SaveAs(path);
                        objtblpurchase.invoice_photo_path = getpath;
                    }

                    try
                    {
                        objtblpurchase.isactive = 1;
                        objtblpurchase.date_created = DateTime.Now;
                        // objtblpurchase.date_invoice = DateTime.Now;
                        objtblpurchase.tbl_sellers_id = Convert.ToInt32(Session["SellerID"]);
                        objtblpurchase.created_by = Convert.ToInt32(Session["SellerID"]);
                        dba.tbl_purchase.Add(objtblpurchase);
                        dba.SaveChanges();
                        flag = true;
                        Message = "Addition of Purchase sucessfull !";
                    }

                    catch (DbUpdateException dbu)
                    {
                        var exception = HandleDbUpdateException(dbu);
                        Message = "Addition of Purchase Unsucessfull !";
                    }

                }
            }
            else
            {
                Message = "Addition of Purchase unsucessfull !";
            }
            return new JsonResult { Data = new { Message = Message, Status = flag } };
        }

        public JsonResult DeletePurchase(string id)
        {
            try
            {
                int no = Convert.ToInt32(id);
                tbl_purchase objtblPurchase = dba.tbl_purchase.Where(a => a.id == no).FirstOrDefault();
                objtblPurchase.isactive = 0;
                dba.Entry(objtblPurchase).State = EntityState.Modified;
                dba.SaveChanges();
                string Message = "Entry is deleted Successfully";
                return Json(Message, JsonRequestBehavior.AllowGet);
            }
            catch (Exception exp)
            {
                return Json("Error in getting record !", JsonRequestBehavior.AllowGet);
            }
        }

        public string UpdatePurchase(tbl_purchase Eve)
        {
            string Message, fileName, actualFileName;
            Message = fileName = actualFileName = string.Empty;
            bool flag = false;
            if (Eve != null)
            {
                var getpurchaseDetails = dba.tbl_purchase.Where(a => a.id == Eve.id).FirstOrDefault();
                if (getpurchaseDetails != null)
                {
                    if (Request.Files != null)
                    {
                        if (Request.Files.Count > 0)
                        {
                            var file = Request.Files[0];
                            actualFileName = file.FileName;
                            string[] getfileName = actualFileName.Split('.');
                            actualFileName = getfileName[0];
                            fileName = actualFileName + DateTime.Now.ToString("yyyyMMddHHmmssfff") + Path.GetExtension(file.FileName);
                            var path = "";
                            path = Path.Combine(Server.MapPath("~/Upload/PurchaseInvoice"), fileName);
                            string getpath = Url.Content(Path.Combine("~/Upload/PurchaseInvoice", fileName));
                            file.SaveAs(path);
                            getpurchaseDetails.invoice_photo_path = getpath;
                        }
                        try
                        {
                            getpurchaseDetails.invoice_amount = Eve.invoice_amount;
                            getpurchaseDetails.invoice_no = Eve.invoice_no;
                            getpurchaseDetails.date_created = DateTime.Now;
                            dba.Entry(getpurchaseDetails).State = EntityState.Modified;
                            dba.SaveChanges();
                            flag = true;
                            Message = "Purchase Update sucessfull !";
                        }

                        catch (Exception e)
                        {
                            Debug.WriteLine(e.Message);
                            Message = "Purchase Update Unsucessfull !";
                        }
                    }
                }
                Message = "Purchase Update sucessfull !";
            }
            else
            {
                Message = "Purchase Update unsucessfull !";
            }
            return Message;
        }


        /// <summary>
        /// This is for Colour Crud Operation
        /// </summary>
        /// <returns></returns>
        public ActionResult Colour()
        {
            cf = new comman_function();
            if (!cf.session_check())
                return RedirectToAction("Login", "Home");
            return View();
        }

        public JsonResult GetColor()
        {
            var get_colordetails = (from ob_m_color in dba.m_color
                                    select new SellerUtility
                                    {
                                        ob_m_color = ob_m_color
                                    }).Where(a => a.ob_m_color.isactive == 1).OrderByDescending(a => a.ob_m_color.id).ToList();

            return new JsonResult { Data = get_colordetails, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        [HttpPost]
        public JsonResult SaveColorDetails(m_color objm_color)
        {
            string Message;
            bool flag = false;
            try
            {
                objm_color.isactive = 1;
                dba.m_color.Add(objm_color);
                dba.SaveChanges();
                flag = true;
                Message = "Color is created successfully";
            }
            catch (DbEntityValidationException e)
            {
                foreach (var eve in e.EntityValidationErrors)
                {
                    Debug.WriteLine(@"Entity of type has the following validation errors:", eve.Entry.Entity.GetType().Name, eve.Entry.State);
                    foreach (var ve in eve.ValidationErrors)
                    {
                        Debug.WriteLine("- Property: \"{0}\", Error: \"{1}\"",
                         ve.PropertyName, ve.ErrorMessage);
                    }
                }
                throw;
            }
            catch (DbUpdateException dbu)
            {
                var exception = HandleDbUpdateException(dbu);
                throw exception;
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
                throw;
            }
            return new JsonResult { Data = new { Message = Message, Status = flag } };
        }

        [HttpPost]
        public JsonResult UpdateColorDetails(m_color objm_color)
        {
            string Message;
            bool flag = false;
            try
            {
                var get_details = dba.m_color.Where(a => a.id == objm_color.id).FirstOrDefault();
                if (get_details != null)
                {
                    get_details.color_code = objm_color.color_code;
                    get_details.color_name = objm_color.color_name;

                    dba.Entry(get_details).State = EntityState.Modified;
                    dba.SaveChanges();
                    flag = true;
                    Message = "Color is updated successfully";
                }
                else
                {
                    flag = false;
                    Message = "some error occurred !!";
                }
            }
            catch (DbEntityValidationException e)
            {
                foreach (var eve in e.EntityValidationErrors)
                {
                    Debug.WriteLine(@"Entity of type has the following validation errors:", eve.Entry.Entity.GetType().Name, eve.Entry.State);
                    foreach (var ve in eve.ValidationErrors)
                    {
                        Debug.WriteLine("- Property: \"{0}\", Error: \"{1}\"",
                            ve.PropertyName, ve.ErrorMessage);
                    }
                }
                throw;
            }
            catch (DbUpdateException dbu)
            {
                var exception = HandleDbUpdateException(dbu);
                throw exception;
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
                throw;
            }

            return new JsonResult { Data = new { Message = Message, Status = flag } };
        }

        public JsonResult DeleteColor(string id)
        {
            try
            {
                int no = Convert.ToInt32(id);
                m_color objtblColor = dba.m_color.Where(a => a.id == no).FirstOrDefault();
                objtblColor.isactive = 0;
                dba.Entry(objtblColor).State = EntityState.Modified;
                dba.SaveChanges();
                string Message = "Entry is deleted Successfully";
                return Json(Message, JsonRequestBehavior.AllowGet);
            }
            catch (Exception exp)
            {
                return Json("Error in getting record !", JsonRequestBehavior.AllowGet);
            }
        }

        /// <summary>
        /// This is for Inventory CRUD Operation
        /// </summary>
        /// <returns></returns>
        public ActionResult Inventory()
        {
            cf = new comman_function();
            if (!cf.session_check())
                return RedirectToAction("Login", "Home");
            return View();
        }

        public JsonResult FillColor()
        {
            var GetColorDetails = dba.m_color.Where(a => a.isactive == 1).ToList();
            return new JsonResult { Data = GetColorDetails, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        public JsonResult FillFullfilled()
        {
            var GetFullfilledBy = dba.m_fullfilled.ToList();
            return new JsonResult { Data = GetFullfilledBy, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        public JsonResult FillItemDetailsBy()
        {
            var GetDetails = dba.tbl_details_items.ToList();
            return new JsonResult { Data = GetDetails, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }
        public JsonResult FillCategory()
        {
            cf = new comman_function();
            bool ss = cf.session_check();
            int SellerId = Convert.ToInt32(Session["SellerID"]);
            var GetCategoryDetails = dba.tbl_item_category.Where(a => a.isactive == 1 && a.tbl_sellers_id == SellerId).ToList();
            return new JsonResult { Data = GetCategoryDetails, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }
        public JsonResult FillSubCategory(int categoryId)
        {
            var GetSubCategoryDetails = dba.tbl_item_subcategory.Where(a => a.tbl_item_category_id == categoryId).ToList();
            return new JsonResult { Data = GetSubCategoryDetails, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        //public JsonResult GetInventory()
        //{
        //    cf = new comman_function();
        //    bool ss = cf.session_check();
        //    int SellerId = Convert.ToInt32(Session["SellerID"]);
        //    var get_inventorydetails = (from ob_tbl_inventory in dba.tbl_inventory

        //                                join ob_m_color in dba.m_color on ob_tbl_inventory.m_item_color_id
        //                                equals ob_m_color.id
        //                                into JoinedEmpDept
        //                                from proj in JoinedEmpDept.DefaultIfEmpty()
        //                                // join ob_m_item_category in db.m_item_category on ob_tbl_inventory.tbl_item_category_id
        //                                // equals ob_m_item_category.id
        //                                // join ob_m_item_subcategory in db.m_item_subcategory on ob_tbl_inventory.tbl_item_subcategory_id
        //                                //equals ob_m_item_subcategory.id


        //                                select new SellerUtility
        //                                {
        //                                    ob_tbl_inventory = ob_tbl_inventory,
        //                                    ob_m_color = proj
        //                                    //ob_m_item_category = ob_m_item_category,
        //                                    //ob_m_item_subcategory = ob_m_item_subcategory
        //                                }).Where(a => a.ob_tbl_inventory.isactive == 1 && a.ob_tbl_inventory.tbl_sellers_id == SellerId).OrderByDescending(a => a.ob_tbl_inventory.id).ToList();

        //    return new JsonResult { Data = get_inventorydetails, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        //}
        public JsonResult GetInventory()
        {
            cf = new comman_function();
            bool ss = cf.session_check();
            int SellerId = Convert.ToInt32(Session["SellerID"]);

            var get_inventorydetails = (from ob_tbl_inventory in dba.tbl_inventory
                                        join ob_item_category in dba.tbl_item_category on ob_tbl_inventory.tbl_item_category_id equals ob_item_category.id
                                        into JoinedEmpDept1
                                        from proj1 in JoinedEmpDept1.DefaultIfEmpty()
                                        join ob_m_item_tax in dba.tbl_category_slabs on proj1.id equals ob_m_item_tax.m_category_id
                                        into JoinedEmpDept2
                                        from proj2 in JoinedEmpDept2.DefaultIfEmpty()
                                        select new
                                        {
                                            ob_tbl_inventory = ob_tbl_inventory,
                                            //ob_m_color = proj,
                                            ob_item_category = proj1,
                                            ob_m_item_tax = proj2
                                        }).Where(a => a.ob_tbl_inventory.isactive == 1 && a.ob_tbl_inventory.tbl_sellers_id == SellerId).OrderByDescending(a => a.ob_tbl_inventory.id).ToList();
            return new JsonResult { Data = get_inventorydetails, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        [HttpPost]
        public JsonResult SaveInventoryDetails(tbl_inventory Res, List<tbl_effectiveprice> price_slabs)
        {
            cf = new comman_function();
            bool ss = cf.session_check();
            string Message, fileName, actualFileName;
            Message = fileName = actualFileName = string.Empty;
            bool flag = false;
            if (Res != null)
            {
                if (Request.Files != null)
                {
                    for (int i = 0; i < Request.Files.Count; i++)
                    {
                        var file = Request.Files[i];
                        actualFileName = file.FileName;
                        string[] getfileName = actualFileName.Split('.');
                        actualFileName = getfileName[0];
                        fileName = actualFileName + DateTime.Now.ToString("yyyyMMddHHmmssfff") + Path.GetExtension(file.FileName);
                        var path = "";
                        string getpath = "";
                        switch (i)
                        {
                            case 0:
                                path = Path.Combine(Server.MapPath("~/Upload/InventoryManage"), fileName);
                                getpath = Url.Content(Path.Combine("~/Upload/InventoryManage", fileName));
                                Res.item_photo1_path = getpath;
                                break;
                            case 1:
                                path = Path.Combine(Server.MapPath("~/Upload/InventoryManage"), fileName);
                                getpath = Url.Content(Path.Combine("~/Upload/InventoryManage", fileName));
                                Res.item_photo2_path = getpath;
                                break;
                            case 2:
                                path = Path.Combine(Server.MapPath("~/Upload/InventoryManage"), fileName);
                                getpath = Url.Content(Path.Combine("~/Upload/InventoryManage", fileName));
                                Res.item_photo3_path = getpath;
                                break;
                            default:
                                break;
                        }
                        file.SaveAs(path);
                    }
                }
                try
                {

                    Res.isactive = 1;
                    Res.lastupdated = DateTime.Now;
                    Res.tbl_sellers_id = Convert.ToInt32(Session["SellerID"]);
                    Res.item_No = 0;
                    Res.t_virtualItemCount = 0;
                    dba.tbl_inventory.Add(Res);
                    dba.SaveChanges();
                    if (price_slabs != null)
                    {
                        foreach (var item in price_slabs)
                        {
                            tbl_inventory_effectiveprice temp = new tbl_inventory_effectiveprice();
                            temp.tbl_sellerid = Convert.ToInt32(Session["SellerID"]);
                            temp.tbl_inventory_id = Res.id;
                            temp.Effecive_date = Convert.ToDateTime(item.Effecive_date);
                            temp.Effective_price = item.Effective_price;
                            dba.tbl_inventory_effectiveprice.Add(temp);
                            dba.SaveChanges();

                            var get_details = dba.tbl_inventory.Where(a => a.id == Res.id).FirstOrDefault();
                            if (get_details != null)
                            {
                                get_details.t_effectiveBought_price = item.Effective_price;
                                dba.Entry(get_details).State = EntityState.Modified;
                                dba.SaveChanges();

                            }
                        }
                    }

                    flag = true;
                    Message = "Addition of Inventory sucessfull !";
                }
                catch (DbUpdateException dbu)
                {
                    var exception = HandleDbUpdateException(dbu);
                    Message = "Addition of Inventory Unsucessfull !";
                }
                catch (Exception e)
                {
                    Debug.WriteLine(e.Message);
                    Message = "Addition of Inventory Unsucessfull !";
                }
            }
            return new JsonResult { Data = new { Message = Message, Status = flag } };
        }

        public JsonResult DeleteInventory(string id)
        {
            try
            {
                int no = Convert.ToInt32(id);
                tbl_inventory objtblinventory = dba.tbl_inventory.Where(a => a.id == no).FirstOrDefault();
                objtblinventory.isactive = 0;
                dba.Entry(objtblinventory).State = EntityState.Modified;
                dba.SaveChanges();
                string Message = "Entry is deleted Successfully";
                return Json(Message, JsonRequestBehavior.AllowGet);
            }
            catch (Exception exp)
            {
                return Json("Error in getting record !", JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public JsonResult UpdateInventoryDetails(tbl_inventory objtbl_inventory, List<tbl_effectiveprice> price_slabs)
        {
            cf = new comman_function();
            string Message, fileName, actualFileName;
            Message = fileName = actualFileName = string.Empty;
            bool flag = false;
            if (objtbl_inventory != null)
            {
                if (Request.Files != null)
                {
                    for (int i = 0; i < Request.Files.Count; i++)
                    {
                        var file = Request.Files[i];
                        actualFileName = file.FileName;
                        string[] getfileName = actualFileName.Split('.');
                        actualFileName = getfileName[0];
                        fileName = actualFileName + DateTime.Now.ToString("yyyyMMddHHmmssfff") + Path.GetExtension(file.FileName);
                        var path = "";
                        string getpath = "";
                        switch (i)
                        {
                            case 0:
                                path = Path.Combine(Server.MapPath("~/Upload/InventoryManage"), fileName);
                                getpath = Url.Content(Path.Combine("~/Upload/InventoryManage", fileName));
                                objtbl_inventory.item_photo1_path = getpath;
                                break;
                            case 1:
                                path = Path.Combine(Server.MapPath("~/Upload/InventoryManage"), fileName);
                                getpath = Url.Content(Path.Combine("~/Upload/InventoryManage", fileName));
                                objtbl_inventory.item_photo2_path = getpath;
                                break;
                            case 2:
                                path = Path.Combine(Server.MapPath("~/Upload/InventoryManage"), fileName);
                                getpath = Url.Content(Path.Combine("~/Upload/InventoryManage", fileName));
                                objtbl_inventory.item_photo3_path = getpath;
                                break;
                            default:
                                break;
                        }
                        file.SaveAs(path);
                    }
                }

                try
                {
                    var get_details = dba.tbl_inventory.Where(a => a.id == objtbl_inventory.id).FirstOrDefault();
                    if (get_details != null)
                    {
                        get_details.brand = objtbl_inventory.brand;
                        get_details.hsn_code = objtbl_inventory.hsn_code;
                        get_details.item_code = objtbl_inventory.item_code;
                        get_details.item_description = objtbl_inventory.item_description;
                        get_details.item_dimension = objtbl_inventory.item_dimension;

                        get_details.item_name = objtbl_inventory.item_name;

                        get_details.remarks = objtbl_inventory.remarks;
                        get_details.packed_dimension = objtbl_inventory.packed_dimension;
                        get_details.sku = objtbl_inventory.sku;
                        get_details.item_count = objtbl_inventory.item_count;
                        get_details.mrp = objtbl_inventory.mrp;
                        get_details.packed_weight = objtbl_inventory.packed_weight;
                        get_details.selling_price = objtbl_inventory.selling_price;
                        get_details.item_weight = objtbl_inventory.item_weight;
                        get_details.transfer_price = objtbl_inventory.transfer_price;

                        get_details.m_item_color_id = objtbl_inventory.m_item_color_id;
                        get_details.tbl_item_category_id = objtbl_inventory.tbl_item_category_id;
                        get_details.tbl_item_subcategory_id = objtbl_inventory.tbl_item_subcategory_id;
                        get_details.lead_time_to_ship = objtbl_inventory.lead_time_to_ship;

                        get_details.n_fullfilled_id = objtbl_inventory.n_fullfilled_id;
                        get_details.tax_update = 1;


                        if (!string.IsNullOrEmpty(objtbl_inventory.item_photo1_path))
                        {
                            get_details.item_photo1_path = objtbl_inventory.item_photo1_path;
                        }
                        if (!string.IsNullOrEmpty(objtbl_inventory.item_photo2_path))
                        {
                            get_details.item_photo2_path = objtbl_inventory.item_photo2_path;
                        }
                        if (!string.IsNullOrEmpty(objtbl_inventory.item_photo3_path))
                        {
                            get_details.item_photo3_path = objtbl_inventory.item_photo3_path;
                        }
                        get_details.lastupdated = DateTime.Now;
                        dba.Entry(get_details).State = EntityState.Modified;
                        dba.SaveChanges();
                        flag = true;

                        //var categorydetails = dba.tbl_item_category.Where(a => a.id == get_details.tbl_item_category_id).FirstOrDefault();
                        //if (categorydetails != null)
                        //{
                        //    double taxrate = 0;
                        //    double itemtax = 0, cgst_tax = 0, sgst_tax = 0, igst_tax = 0;
                        //    double itemprice = 0, cgst_amount = 0, sgst_amount = 0, igst_amount = 0;
                        //    string customerstate = "";
                        //    var get_saleorder_details = dba.tbl_sales_order_details.Where(a => a.sku_no.ToLower() == get_details.sku.ToLower()).ToList();
                        //    if (get_saleorder_details != null)
                        //    {
                        //        foreach (var item in get_saleorder_details)
                        //        {
                        //            itemtax = item.item_tax_amount;
                        //            itemprice = item.item_price_amount;                                   
                        //            var category_slabs = dba.tbl_category_slabs.Where(a => a.m_category_id == categorydetails.id).ToList();
                        //            if (category_slabs != null)
                        //            {
                        //                foreach (var slab in category_slabs)//if (category_slabs != null)
                        //                {
                        //                    if (slab.from_rs < itemprice && slab.to_rs > itemprice)
                        //                    {
                        //                        taxrate = Convert.ToDouble(slab.tax_rate);
                        //                    }
                        //                    continue;
                        //                }
                        //            }

                        //            if (taxrate != 0 && taxrate != null)
                        //            {
                        //                var seller_details = db.tbl_sellers.Where(a => a.id == get_details.tbl_sellers_id).FirstOrDefault();
                        //                if (seller_details != null)
                        //                {
                        //                    var get_saleorder = dba.tbl_sales_order.Where(a => a.id == item.tbl_sales_order_id).FirstOrDefault();// to get sale order details for getting customer details
                        //                    var getcustomerdetails = dba.tbl_customer_details.Where(a => a.id == get_saleorder.tbl_Customer_Id).FirstOrDefault();// to get customer details from tbl customerdetails in seller admin db.
                        //                    if (getcustomerdetails != null)
                        //                    {
                        //                        customerstate = getcustomerdetails.State_Region.ToLower(); ;
                        //                    }
                        //                    var getcountrydetails = db.tbl_country.Where(a => a.status == 1 && a.countrylevel == 0 && a.id == seller_details.country).FirstOrDefault();// to get country name from country table in admin db.
                        //                    var getstatedetails = db.tbl_country.Where(m => m.id == seller_details.state && m.countrylevel == 1 && m.status == 1).FirstOrDefault();//to get state name from country table in admin db.
                        //                    string sellerstate = getstatedetails.countryname.ToLower();


                        //                    if (sellerstate == customerstate)
                        //                    {
                        //                        cgst_tax = Convert.ToDouble(taxrate) / 2;
                        //                        sgst_tax = Convert.ToDouble(taxrate) - cgst_tax;

                        //                        cgst_amount = (itemtax) / 2; //(objsaledetails.item_price_amount * 100) / (100 + Convert.ToDouble(cgst_tax));
                        //                        sgst_amount = (itemtax - cgst_amount); //(objsaledetails.item_price_amount * 100) / (100 + Convert.ToDouble(sgst_tax));                                         
                        //                    }
                        //                    else
                        //                    {
                        //                        igst_tax = Convert.ToDouble(taxrate);
                        //                        igst_amount = (itemtax); //(objsaledetails.item_price_amount * 100) / (100 + Convert.ToDouble(igst_tax));

                        //                    }
                        //                    var get_taxdetails = dba.tbl_tax.Where(a => a.tbl_referneced_id == item.id).FirstOrDefault();
                        //                    if (get_taxdetails == null)
                        //                    {
                        //                        tbl_tax objtax = new tbl_tax();
                        //                        objtax.tbl_seller_id = item.tbl_seller_id;
                        //                        objtax.tbl_referneced_id = item.id;
                        //                        objtax.reference_type = 3;
                        //                        objtax.isactive = 1;
                        //                        if (cgst_tax != null && cgst_tax != 0.0 && sgst_tax != null && sgst_tax != 0.0)
                        //                        {
                        //                            objtax.cgst_tax = cgst_tax;
                        //                            objtax.sgst_tax = sgst_tax;
                        //                            objtax.CGST_amount = cgst_amount;
                        //                            objtax.sgst_amount = sgst_amount;
                        //                        }
                        //                        else
                        //                        {
                        //                            objtax.igst_tax = igst_tax;
                        //                            objtax.Igst_amount = igst_amount;
                        //                        }
                        //                        dba.tbl_tax.Add(objtax);
                        //                        dba.SaveChanges();

                        //                    }// end of if(get_taxdetails)
                        //                    else
                        //                    {
                        //                        if (cgst_tax != null && cgst_tax != 0.0 && sgst_tax != null && sgst_tax != 0.0)
                        //                        {
                        //                            get_taxdetails.cgst_tax = cgst_tax;
                        //                            get_taxdetails.sgst_tax = sgst_tax;
                        //                            get_taxdetails.CGST_amount = cgst_amount;
                        //                            get_taxdetails.sgst_amount = sgst_amount;
                        //                        }
                        //                        else
                        //                        {
                        //                            get_taxdetails.igst_tax = igst_tax;
                        //                            get_taxdetails.Igst_amount = igst_amount;
                        //                        }
                        //                        dba.Entry(get_taxdetails).State = EntityState.Modified;
                        //                        dba.SaveChanges();
                        //                    }
                        //                }
                        //            }
                        //        }// end of foreach
                        //    }// end of if(get_saleorder_details) 
                        //}


                        if (price_slabs != null)
                        {
                            foreach (var item in price_slabs)
                            {
                                if (item.Id != null && item.Id != 0)
                                {
                                    var get_slabsdetails = dba.tbl_inventory_effectiveprice.Where(a => a.tbl_inventory_id == item.tbl_inventory_id && a.Id == item.Id).FirstOrDefault();
                                    if (get_slabsdetails != null)
                                    {
                                        get_slabsdetails.Effective_price = item.Effective_price;
                                        get_slabsdetails.Item_Tax = item.Item_Tax;
                                        get_slabsdetails.Gross_price = item.Gross_price;

                                        //string aaa = item.Effecive_date.Replace("-", "/");
                                        //string date = Convert.ToDateTime(aaa).ToString("MM/dd/yyyy");
                                        string datetime2 = item.Effecive_date.TrimEnd();

                                        DateTime ddd = cf.MyDateTimeConverter(datetime2);
                                        // get_slabsdetails.Effecive_date =DateTime.Parse(item.Effecive_date);
                                        //dba.Entry(get_slabsdetails).State = EntityState.Modified;
                                        //dba.SaveChanges();
                                    }
                                }
                                else
                                {
                                    tbl_inventory_effectiveprice temp = new tbl_inventory_effectiveprice();
                                    temp.tbl_sellerid = Convert.ToInt32(Session["SellerID"]);
                                    temp.tbl_inventory_id = objtbl_inventory.id;
                                    temp.Effecive_date = Convert.ToDateTime(item.Effecive_date);
                                    temp.Effective_price = item.Effective_price;
                                    temp.Item_Tax = item.Item_Tax;
                                    temp.Gross_price = item.Gross_price;
                                    dba.tbl_inventory_effectiveprice.Add(temp);
                                    dba.SaveChanges();

                                    var get_savedetails = dba.tbl_inventory.Where(a => a.id == objtbl_inventory.id).FirstOrDefault();
                                    if (get_savedetails != null)
                                    {
                                        get_savedetails.t_effectiveBought_price = item.Gross_price;
                                        dba.Entry(get_savedetails).State = EntityState.Modified;
                                        dba.SaveChanges();
                                    }
                                }
                            }
                        }
                        Message = "Inventory is updated successfully";
                    }
                    else
                    {
                        flag = false;
                        Message = "some error occurred !!";
                    }
                }
                catch (DbEntityValidationException e)
                {
                    foreach (var eve in e.EntityValidationErrors)
                    {
                        Debug.WriteLine(@"Entity of type has the following validation errors:", eve.Entry.Entity.GetType().Name, eve.Entry.State);
                        foreach (var ve in eve.ValidationErrors)
                        {
                            Debug.WriteLine("- Property: \"{0}\", Error: \"{1}\"",
                                ve.PropertyName, ve.ErrorMessage);
                        }
                    }
                    throw;
                }
                catch (DbUpdateException dbu)
                {
                    var exception = HandleDbUpdateException(dbu);
                    throw exception;
                }
                catch (Exception e)
                {
                    Debug.WriteLine(e.Message);
                    throw;
                }
            }
            return new JsonResult { Data = new { Message = Message, Status = flag } };
        }


        // [HttpPost]
        //public JsonResult AddCategory1(tbl_item_category Eve)
        //{
        //    cf = new comman_function();
        //    bool ss = cf.session_check();
        //    temp_item_category temp = new temp_item_category();
        //    string Message;
        //    bool flag = false;
        //    try
        //    {
        //        Eve.isactive = 1;
        //        Eve.tbl_sellers_id = Convert.ToInt32(Session["SellerID"]);
        //        Eve.updated_by = Convert.ToInt32(Session["SellerID"]);
        //        Eve.date_tax_updated = DateTime.Now;
        //        dba.tbl_item_category.Add(Eve);
        //        dba.SaveChanges();
        //        flag = true;
        //        temp.t_category_name = Eve.category_name;
        //        temp.tax_rate = Eve.tax_rate;
        //        temp.isactive = 1;
        //        temp.tbl_sellers_id = Eve.tbl_sellers_id;
        //        temp.updated_by = Eve.updated_by;
        //        temp.d_date_tax_updated = Eve.date_tax_updated;
        //        temp.m_item_category_id = Eve.id;
        //        db.temp_item_category.Add(temp);
        //        db.SaveChanges();


        //        Message = "Category is created successfully";
        //    }
        //    catch (DbEntityValidationException e)
        //    {
        //        foreach (var eve in e.EntityValidationErrors)
        //        {
        //            Debug.WriteLine(@"Entity of type has the following validation errors:", eve.Entry.Entity.GetType().Name, eve.Entry.State);
        //            foreach (var ve in eve.ValidationErrors)
        //            {
        //                Debug.WriteLine("- Property: \"{0}\", Error: \"{1}\"",
        //                 ve.PropertyName, ve.ErrorMessage);
        //            }
        //        }
        //        throw;
        //    }
        //    catch (DbUpdateException dbu)
        //    {
        //        var exception = HandleDbUpdateException(dbu);
        //        throw exception;
        //    }
        //    catch (Exception e)
        //    {
        //        Debug.WriteLine(e.Message);
        //        throw;
        //    }
        //    return new JsonResult { Data = new { Message = Message, Status = flag } };
        //}
        [HttpGet]
        public bool IsUserNameAvailable(string userName)
        {
            int SellerId = Convert.ToInt32(Session["SellerID"]);
            var result = dba.tbl_item_category.Where(a => a.tbl_sellers_id == SellerId && a.category_name == userName).ToList();

            if (result.Count() > 0)
            {
                return false;
            }
            else
            {
                return true;
            }

        }



        [HttpPost]
        public JsonResult AddCategory(tbl_item_category Eve, List<tbl_category_slabs> category_slabs)
        {
            int SellerId = Convert.ToInt32(Session["SellerID"]);
            cf = new comman_function();
            bool ss = cf.session_check();
            tbl_item_category obj = new tbl_item_category();
            string Message;
            bool flag = false;
            try
            {
                obj.isactive = 1;
                obj.category_name = Eve.category_name;
                obj.hsn_code = Eve.hsn_code;
                obj.tbl_sellers_id = SellerId;
                obj.updated_by = SellerId;
                obj.date_tax_updated = DateTime.Now;
                dba.tbl_item_category.Add(obj);
                dba.SaveChanges();
                flag = true;
                if (category_slabs != null)
                {
                    foreach (var item in category_slabs)
                    {
                        tbl_category_slabs temp = new tbl_category_slabs();
                        temp.m_category_id = obj.id;
                        temp.tbl_seller_id = obj.tbl_sellers_id;
                        temp.from_rs = item.from_rs;
                        temp.to_rs = item.to_rs;
                        temp.tax_rate = item.tax_rate;
                        dba.tbl_category_slabs.Add(temp);
                        dba.SaveChanges();
                    }
                }
                Message = "Category is created successfully";
            }
            catch (DbEntityValidationException e)
            {
                foreach (var eve in e.EntityValidationErrors)
                {
                    Debug.WriteLine(@"Entity of type has the following validation errors:", eve.Entry.Entity.GetType().Name, eve.Entry.State);
                    foreach (var ve in eve.ValidationErrors)
                    {
                        Debug.WriteLine("- Property: \"{0}\", Error: \"{1}\"",
                         ve.PropertyName, ve.ErrorMessage);
                    }
                }
                throw;
            }
            catch (DbUpdateException dbu)
            {
                var exception = HandleDbUpdateException(dbu);
                throw exception;
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
                throw;
            }
            return new JsonResult { Data = new { Message = Message, Status = flag } };
        }

        public JsonResult UpdateCategoryDetails(tbl_category_slabs objtblslabs)
        {
            string Message;
            bool flag = false;
            try
            {
                var get_details = dba.tbl_category_slabs.Where(a => a.id == objtblslabs.id).FirstOrDefault();
                if (get_details != null)
                {
                    get_details.to_rs = objtblslabs.to_rs;
                    get_details.from_rs = objtblslabs.from_rs;
                    get_details.tax_rate = objtblslabs.tax_rate;
                    dba.Entry(get_details).State = EntityState.Modified;
                    dba.SaveChanges();
                    flag = true;
                    Message = "Category is updated successfully";
                }
                else
                {
                    flag = false;
                    Message = "some error occurred !!";
                }
            }
            catch (DbEntityValidationException e)
            {
                foreach (var eve in e.EntityValidationErrors)
                {
                    Debug.WriteLine(@"Entity of type has the following validation errors:", eve.Entry.Entity.GetType().Name, eve.Entry.State);
                    foreach (var ve in eve.ValidationErrors)
                    {
                        Debug.WriteLine("- Property: \"{0}\", Error: \"{1}\"",
                            ve.PropertyName, ve.ErrorMessage);
                    }
                }
                throw;
            }
            catch (DbUpdateException dbu)
            {
                var exception = HandleDbUpdateException(dbu);
                throw exception;
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
                throw;
            }

            return new JsonResult { Data = new { Message = Message, Status = flag } };
        }

        public JsonResult ViewTaxDetails(int? id)
        {
            int SellerId = Convert.ToInt32(Session["SellerID"]);
            List<category_slabs> lst = new List<category_slabs>();
            if (id != null)
            {
                var get_details = dba.tbl_item_category.Where(a => a.tbl_sellers_id == SellerId && a.id == id).FirstOrDefault();
                if (get_details != null)
                {
                    var details = dba.tbl_category_slabs.Where(a => a.m_category_id == get_details.id).ToList();

                    category_slabs obj_category = new category_slabs();
                    obj_category.id = get_details.id;
                    obj_category.category_name = get_details.category_name;
                    obj_category.hsn_code = get_details.hsn_code;


                    List<partial_slabs> obj_slabs = new List<partial_slabs>();
                    foreach (var item in details)
                    {
                        partial_slabs obj = new partial_slabs();
                        obj.from_rs = item.from_rs;
                        obj.to_rs = item.to_rs;
                        obj.tax_rate = item.tax_rate;
                        obj.id = item.id;
                        obj_slabs.Add(obj);
                    }
                    obj_category.obj_partial_slabs = obj_slabs;
                    lst.Add(obj_category);

                }
            }
            return new JsonResult { Data = lst, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
            //return Json("",JsonRequestBehavior.AllowGet);
        }


        public JsonResult ViewPriceSlabDetails(int? id)
        {
            int SellerId = Convert.ToInt32(Session["SellerID"]);
            List<tbl_effectiveprice> lst = new List<tbl_effectiveprice>();
            if (id != null)
            {
                var get_details = dba.tbl_inventory_effectiveprice.Where(a => a.tbl_sellerid == SellerId && a.tbl_inventory_id == id).ToList();
                if (get_details != null)
                {
                    foreach (var item in get_details)
                    {
                        //tbl_inventory_effectiveprice obj_category = new tbl_inventory_effectiveprice();
                        tbl_effectiveprice obj_category = new tbl_effectiveprice();
                        obj_category.Id = item.Id;
                        obj_category.tbl_inventory_id = item.tbl_inventory_id;
                        obj_category.tbl_sellerid = item.tbl_sellerid;
                        // obj_category.Effecive_date = Convert.ToDateTime(item.Effecive_date).ToString("dd/MM/yyyy");
                        obj_category.Effecive_date = Convert.ToDateTime(item.Effecive_date).ToString("dd/MM/yyyy").Replace("-", "/");                        
                        obj_category.Effective_price = item.Effective_price;
                        obj_category.Gross_price = item.Gross_price;
                        obj_category.Item_Tax = item.Item_Tax;
                        lst.Add(obj_category);
                    }
                }
            }
            return new JsonResult { Data = lst.OrderBy(a => a.Id), JsonRequestBehavior = JsonRequestBehavior.AllowGet };
            //return Json("",JsonRequestBehavior.AllowGet);
        }



        [HttpPost]
        public JsonResult AddSubCategory(tbl_item_subcategory Eve)
        {
            cf = new comman_function();
            bool ss = cf.session_check();
            temp_item_subcategory tempsub = new temp_item_subcategory();
            string Message;
            bool flag = false;
            try
            {
                Eve.isactive = 1;
                Eve.updated_by = Convert.ToInt32(Session["SellerID"]);
                Eve.tbl_sellers_id = Convert.ToInt32(Session["SellerID"]);
                Eve.updated_on = DateTime.Now;
                dba.tbl_item_subcategory.Add(Eve);
                dba.SaveChanges();
                flag = true;

                tempsub.isactive = Eve.isactive;
                tempsub.updated_by = Eve.updated_by;
                tempsub.tbl_sellers_id = Eve.tbl_sellers_id;
                tempsub.updated_on = Eve.updated_on;
                tempsub.tbl_item_category_id = Eve.tbl_item_category_id;
                tempsub.t_subcategory_name = Eve.subcategory_name;
                tempsub.t_hsn_code = Eve.hsn_code;
                tempsub.t_tax_rate = Eve.tax_rate;
                tempsub.tbl_sub_category_id = Eve.id;
                db.temp_item_subcategory.Add(tempsub);
                db.SaveChanges();
                Message = "Sub Category is created successfully";
            }
            catch (DbEntityValidationException e)
            {
                foreach (var eve in e.EntityValidationErrors)
                {
                    Debug.WriteLine(@"Entity of type has the following validation errors:", eve.Entry.Entity.GetType().Name, eve.Entry.State);
                    foreach (var ve in eve.ValidationErrors)
                    {
                        Debug.WriteLine("- Property: \"{0}\", Error: \"{1}\"",
                         ve.PropertyName, ve.ErrorMessage);
                    }
                }
                throw;
            }
            catch (DbUpdateException dbu)
            {
                var exception = HandleDbUpdateException(dbu);
                throw exception;
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
                throw;
            }
            return new JsonResult { Data = new { Message = Message, Status = flag } };
        }

        //public ActionResult MyInventory1()
        //{
        //    return View();
        //}

        //public JsonResult GetMyInventory(int? categoryID, int? subcategoryID)
        //{
        //    int SellerId = Convert.ToInt32(Session["SellerID"]);
        //    var get_inventorydetails = (from ob_tbl_inventory in dba.tbl_inventory
        //                                join ob_m_color in dba.m_color on ob_tbl_inventory.m_item_color_id
        //                                equals ob_m_color.id
        //                                join ob_tbl_item_category in dba.tbl_item_category on ob_tbl_inventory.tbl_item_category_id
        //                                equals ob_tbl_item_category.id
        //                                join ob_tbl_item_subcategory in dba.tbl_item_subcategory on ob_tbl_inventory.tbl_item_subcategory_id
        //                                equals ob_tbl_item_subcategory.id


        //                                select new SellerUtility
        //                                {
        //                                    ob_tbl_inventory = ob_tbl_inventory,
        //                                    ob_m_color = ob_m_color,
        //                                    ob_tbl_item_category = ob_tbl_item_category,
        //                                    ob_tbl_item_subcategory = ob_tbl_item_subcategory
        //                                }).Where(a => a.ob_tbl_inventory.isactive == 1 && a.ob_tbl_inventory.tbl_sellers_id == SellerId).OrderByDescending(a => a.ob_tbl_inventory.id).ToList();
        //    if (get_inventorydetails != null)
        //    {
        //        if (categoryID != 0 && categoryID != null)
        //            get_inventorydetails = get_inventorydetails.Where(a => a.ob_tbl_item_category.id == categoryID).ToList();
        //    }
        //    if (get_inventorydetails != null)
        //    {
        //        if (subcategoryID != 0 && subcategoryID != null)
        //            get_inventorydetails = get_inventorydetails.Where(a => a.ob_tbl_item_subcategory.id == subcategoryID).ToList();
        //    }

        //    return new JsonResult { Data = get_inventorydetails, JsonRequestBehavior = JsonRequestBehavior.AllowGet };








        //}
        [HttpPost]
        public JsonResult UpdateMyInventoryDetails(tbl_inventory ob)
        {
            string Message;
            bool flag = false;
            try
            {
                var get_details = dba.tbl_inventory.Where(a => a.id == ob.id).FirstOrDefault();
                if (get_details != null)
                {
                    get_details.selling_price = ob.selling_price;
                    get_details.item_count = ob.item_count;

                    dba.Entry(get_details).State = EntityState.Modified;
                    dba.SaveChanges();
                    flag = true;
                    Message = "Inventory is updated successfully";
                }
                else
                {
                    flag = false;
                    Message = "some error occurred !!";
                }
            }

            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
                throw;
            }

            return new JsonResult { Data = new { Message = Message, Status = flag } };
        }




        public ActionResult InventoryDetailList(int? id)
        {
            cf = new comman_function();
            if (!cf.session_check())
                return RedirectToAction("Login", "Home");
            return View();
        }

        public JsonResult InventoryDetailList1(int? id)
        {
            cf = new comman_function();
            bool ss = cf.session_check();
            int SellerId = Convert.ToInt32(Session["SellerID"]);
            string Name = Convert.ToString(Session["UserName"]);

            var GetPurchaseOrder = (from ob_tbl_inventory in dba.tbl_inventory.Where(a => a.id == id && a.isactive == 1 && a.tbl_sellers_id == SellerId)
                                    join ob_tbl_inventory_details in dba.tbl_inventory_details on ob_tbl_inventory.id equals (ob_tbl_inventory_details.tbl_inventory_id ?? 0)
                                    into JoinedEmpDept
                                    from proj in JoinedEmpDept.DefaultIfEmpty()

                                    join ob_tbl_purchase_details in dba.tbl_purchase_details on ob_tbl_inventory.id equals (ob_tbl_purchase_details.tbl_inventory_id ?? 0)
                                    into JoinedEmpDept1
                                    from proj1 in JoinedEmpDept1.Where(a => a.isactive == 1).DefaultIfEmpty()

                                    join ob_tbl_purchase in dba.tbl_purchase on proj1.tbl_purchase_id equals (ob_tbl_purchase.id)
                                    into JoinedEmpDept2
                                    from proj2 in JoinedEmpDept2.Where(a => a.isactive == 1).DefaultIfEmpty()

                                    join ob_tbl_seller_vendors in dba.tbl_seller_vendors on proj2.tbl_seller_vendors_id equals (ob_tbl_seller_vendors.id)
                                    into JoinedEmpDept3
                                    from proj3 in JoinedEmpDept3.Where(a => a.status == 1).DefaultIfEmpty()

                                    join ob_tbl_seller_warehouses in dba.tbl_seller_warehouses on proj.tbl_inventory_warehouse_transfers_id equals (ob_tbl_seller_warehouses.id)
                                    into JoinedEmpDept4
                                    from proj4 in JoinedEmpDept4.Where(a => a.isactive == 1).DefaultIfEmpty()

                                    join ob_m_color in dba.m_color on ob_tbl_inventory.m_item_color_id equals (ob_m_color.id)
                                    into JoinedEmpDept5
                                    from proj5 in JoinedEmpDept5.Where(a => a.isactive == 1).DefaultIfEmpty()

                                    join ob_m_item_status in dba.m_item_status on proj.m_item_status_id equals (ob_m_item_status.id)
                                    into JoinedEmpDept6
                                    from proj6 in JoinedEmpDept6.DefaultIfEmpty()


                                    select new
                                    {
                                        ob_tbl_inventory = ob_tbl_inventory,
                                        ob_tbl_inventory_details = proj,
                                        ob_tbl_purchase_details = proj1,
                                        ob_tbl_purchase = proj2,
                                        ob_tbl_seller_vendors = proj3,
                                        ob_tbl_seller_warehouses = proj4,
                                        ob_m_color = proj5,
                                        ob_m_item_status = proj6

                                    }).ToList();

            return new JsonResult { Data = GetPurchaseOrder, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
            // return View(GetPurchaseOrder);

        }

        public ActionResult MyInventoryList()
        {
            cf = new comman_function();
            if (!cf.session_check())
                return RedirectToAction("Login", "Home");
            return View();
        }

        public JsonResult GetMyInventory(int? categoryID, int? subcategoryID)
        {
            cf = new comman_function();
            bool ss = cf.session_check();
            int SellerId = Convert.ToInt32(Session["SellerID"]);
           
                var get_inventorydetails = (from ob_tbl_inventory in dba.tbl_inventory

                                            join ob_m_color in dba.m_color on ob_tbl_inventory.m_item_color_id equals (ob_m_color.id)
                                            into JoinedEmpDept5
                                            from ob_m_color in JoinedEmpDept5

                                            join ob_tbl_item_category in dba.tbl_item_category on ob_tbl_inventory.tbl_item_category_id equals (ob_tbl_item_category.id)
                                            into JoinedEmpDept6
                                            from ob_tbl_item_category in JoinedEmpDept6

                                            join ob_tbl_item_subcategory in dba.tbl_item_subcategory on ob_tbl_inventory.tbl_item_subcategory_id equals (ob_tbl_item_subcategory.id)
                                            into JoinedEmpDept7
                                            from ob_tbl_item_subcategory in JoinedEmpDept7
                                           

                                            select new SellerUtility
                                            {
                                                ob_tbl_inventory = ob_tbl_inventory,
                                                ob_m_color = ob_m_color,
                                                ob_tbl_item_category = ob_tbl_item_category,
                                                ob_tbl_item_subcategory = ob_tbl_item_subcategory
                                            }).Where(a => a.ob_tbl_inventory.isactive == 1 && a.ob_tbl_inventory.tbl_sellers_id == SellerId).OrderByDescending(a => a.ob_tbl_inventory.id).ToList();


                if (get_inventorydetails != null)
                {
                    if (categoryID != 0 && categoryID != null)
                        get_inventorydetails = get_inventorydetails.Where(a => a.ob_tbl_item_category.id == categoryID).ToList();

                    if (subcategoryID != 0 && subcategoryID != null && categoryID != 0 && categoryID != null)
                        get_inventorydetails = get_inventorydetails.Where(a => a.ob_tbl_item_subcategory.id == subcategoryID && a.ob_tbl_item_category.id == categoryID).ToList();
                }

                return new JsonResult { Data = get_inventorydetails, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
                   
        }

        /// <summary>
        /// This is for Manage Vendor Payment
        /// </summary>
        /// <returns></returns>
        public ActionResult VendorPayment()
        {
            cf = new comman_function();
            if (!cf.session_check())
                return RedirectToAction("Login", "Home");
            return View();
        }

        public JsonResult FillVendorForPO()
        {
            cf = new comman_function();
            bool ss = cf.session_check();
            int SellerId = Convert.ToInt32(Session["SellerID"]);
            var GetPOVendorDetails = dba.tbl_seller_vendors.Where(a => a.status == 1 && a.tbl_sellersid == SellerId).ToList();
            return new JsonResult { Data = GetPOVendorDetails, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        public JsonResult GetAllPONumber()
        {
            cf = new comman_function();
            bool ss = cf.session_check();
            int SellerId = Convert.ToInt32(Session["SellerID"]);
            var get_podetails = (from ob_tbl_purchase in dba.tbl_purchase
                                 join ob_tbl_seller_vendors in dba.tbl_seller_vendors on ob_tbl_purchase.tbl_seller_vendors_id
                                        equals ob_tbl_seller_vendors.id
                                        into JoinedEmpDept
                                 from proj in JoinedEmpDept.DefaultIfEmpty()
                                 join ob_tbl_vendor_payment in dba.tbl_vendor_payment on ob_tbl_purchase.id
                                         equals ob_tbl_vendor_payment.tbl_purchase_id
                                         into EmpDept
                                 from proj1 in EmpDept.DefaultIfEmpty()
                                 select new SellerUtility
                                 {
                                     ob_tbl_purchase = ob_tbl_purchase,
                                     ob_tbl_seller_vendors = proj,
                                     ob_tbl_vendor_payment = proj1

                                 }).Where(a => a.ob_tbl_purchase.isactive == 1 && a.ob_tbl_purchase.tbl_sellers_id == SellerId && a.ob_tbl_purchase.po_number != null).OrderByDescending(a => a.ob_tbl_purchase.id).ToList();



            return new JsonResult { Data = get_podetails, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }



        [HttpPost]
        public JsonResult AddVendorPayment(tbl_vendor_payment Eve)
        {
            cf = new comman_function();
            bool ss = cf.session_check();
            string Message;
            bool flag = false;
            try
            {
                Eve.isactive = 1;
                Eve.tbl_seller_id = Convert.ToInt32(Session["SellerID"]);
                Eve.n_paymentBy = Convert.ToInt32(Session["SellerID"]);
                Eve.d_date_of_payment = DateTime.Now;
                dba.tbl_vendor_payment.Add(Eve);
                dba.SaveChanges();
                Message = "Payment is created successfully";
            }
            catch (DbEntityValidationException e)
            {
                foreach (var eve in e.EntityValidationErrors)
                {
                    Debug.WriteLine(@"Entity of type has the following validation errors:", eve.Entry.Entity.GetType().Name, eve.Entry.State);
                    foreach (var ve in eve.ValidationErrors)
                    {
                        Debug.WriteLine("- Property: \"{0}\", Error: \"{1}\"",
                         ve.PropertyName, ve.ErrorMessage);
                    }
                }
                throw;
            }

            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
                throw;
            }
            return new JsonResult { Data = new { Message = Message, Status = flag } };
        }
        /// <summary>
        /// This is for Item Transfer from Warehose to Warehouse
        /// </summary>
        /// <returns></returns>
        /// 


        public JsonResult FillInventoryforItemName()
        {
            cf = new comman_function();
            bool ss = cf.session_check();
            int SellerId = Convert.ToInt32(Session["SellerID"]);
            var GetInventoryDetails = dba.tbl_inventory.Where(a => a.isactive == 1 && a.tbl_sellers_id == SellerId).ToList();
            return new JsonResult { Data = GetInventoryDetails, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }


        public JsonResult FillItemStatus()
        {
            cf = new comman_function();
            bool ss = cf.session_check();
            int SellerId = Convert.ToInt32(Session["SellerID"]);
            var GetStatusDetails = dba.m_item_status.ToList();
            return new JsonResult { Data = GetStatusDetails, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }



        public JsonResult FillInventoryforItemCount(int? id)
        {
            cf = new comman_function();
            bool ss = cf.session_check();
            int SellerId = Convert.ToInt32(Session["SellerID"]);
            string Name = Convert.ToString(Session["UserName"]);

            var GetPurchaseOrder = (from ob_tbl_inventory in dba.tbl_inventory.Where(a => a.id == id && a.isactive == 1 && a.tbl_sellers_id == SellerId)

                                    join ob_tbl_purchase_details in dba.tbl_purchase_details on ob_tbl_inventory.id equals (ob_tbl_purchase_details.tbl_inventory_id ?? 0)
                                    into JoinedEmpDept1
                                    from proj1 in JoinedEmpDept1.Where(a => a.isactive == 1).DefaultIfEmpty()

                                    join ob_tbl_purchase in dba.tbl_purchase on proj1.tbl_purchase_id equals (ob_tbl_purchase.id)
                                    into JoinedEmpDept2
                                    from proj2 in JoinedEmpDept2.Where(a => a.isactive == 1).DefaultIfEmpty()

                                    join ob_tbl_seller_warehouses in dba.tbl_seller_warehouses on proj2.tbl_seller_warehouses_id equals (ob_tbl_seller_warehouses.id)
                                    into JoinedEmpDept4
                                    from proj4 in JoinedEmpDept4.Where(a => a.isactive == 1).DefaultIfEmpty()

                                    select new
                                    {
                                        ob_tbl_inventory = ob_tbl_inventory,
                                        ob_tbl_purchase_details = proj1,
                                        ob_tbl_purchase = proj2,
                                        ob_tbl_seller_warehouses = proj4,
                                    }).ToList();

            return new JsonResult { Data = GetPurchaseOrder, JsonRequestBehavior = JsonRequestBehavior.AllowGet };


        }



        /// <summary>
        ///  this is for item transfer from warehouse to warehouse
        /// </summary>
        /// <param name="Eve"></param>
        /// <returns></returns>
        [HttpPost]
        //public JsonResult AddItemTransfertoWarehouse(tbl_warehouse_transfer Eve)
        //{

        //    tbl_inventory_details objdetails = new tbl_inventory_details();
        //    string Message;
        //    bool flag = false;
        //    try
        //    {
        //        Eve.is_active = 1;
        //        Eve.tbl_seller_id = Convert.ToInt32(Session["SellerID"]);            
        //        Eve.created_on = DateTime.Now;
        //        dba.tbl_warehouse_transfer.Add(Eve);
        //        dba.SaveChanges();

        //        var get_inventory_details = dba.tbl_inventory_details.Where(a => a.tbl_inventory_warehouse_transfers_id == Eve.from_warehouse_id && a.tbl_inventory_id == Eve.tbl_inventory_Id && a.tbl_sellers_id ==Eve.tbl_seller_id).ToList();
        //        if (get_inventory_details != null)
        //        {
        //            int mycount = 0;
        //            foreach(var item in get_inventory_details)
        //            {
        //                if (mycount < Eve.Item_Transfer_Count)
        //                {
        //                    item.tbl_inventory_warehouse_transfers_id = Eve.to_warehouse_id;

        //                    dba.Entry(item).State = EntityState.Modified;
        //                    dba.SaveChanges();
        //                    mycount++;
        //                }
        //                else {
        //                    break;
        //                }                      
        //           }                                  
        //       }

        //        var get_inventory = dba.tbl_inventory.Where(a => a.id == Eve.tbl_inventory_Id).FirstOrDefault();
        //        if (get_inventory != null)
        //        {
        //            int item  = Convert.ToInt16(get_inventory.item_count - Eve.Item_Transfer_Count);
        //            get_inventory.item_count = item;
        //            dba.Entry(get_inventory).State = EntityState.Modified;
        //            dba.SaveChanges();
        //        }

        //        Message = "Item transfer is created successfully";
        //    }


        //    catch (Exception e)
        //    {
        //        Debug.WriteLine(e.Message);
        //        throw;
        //    }
        //    return new JsonResult { Data = new { Message = Message, Status = flag } };
        //}

        public JsonResult AddItemTransfertoWarehouse(tbl_warehouse_transfer Eve)
        {
            cf = new comman_function();
            bool ss = cf.session_check();
            tbl_inventory_details objdetails = new tbl_inventory_details();
            string Message;
            bool flag = false;
            try
            {
                Eve.is_active = 1;
                Eve.tbl_seller_id = Convert.ToInt32(Session["SellerID"]);
                Eve.created_on = DateTime.Now;
                dba.tbl_warehouse_transfer.Add(Eve);
                dba.SaveChanges();
                var get_inventory_details = dba.tbl_inventory_details.Where(a => a.tbl_inventory_warehouse_transfers_id == Eve.from_warehouse_id && a.tbl_inventory_id == Eve.tbl_inventory_Id && a.tbl_sellers_id == Eve.tbl_seller_id).ToList();
                if (get_inventory_details != null)
                {
                    int mycount = 0;
                    foreach (var item in get_inventory_details)
                    {
                        if (mycount < Eve.Item_Transfer_Count)
                        {
                            if (Eve.to_warehouse_id != 0)
                            {
                                item.tbl_inventory_warehouse_transfers_id = Eve.to_warehouse_id;
                            }
                            else if (Eve.Marketplace_id != null)
                            {
                                item.m_marketplace_id = Eve.Marketplace_id;
                                item.tbl_inventory_warehouse_transfers_id = 0;
                            }
                            dba.Entry(item).State = EntityState.Modified;
                            dba.SaveChanges();
                            mycount++;
                        }
                        else
                        {
                            break;
                        }
                    }
                }
                var get_inventory = dba.tbl_inventory.Where(a => a.id == Eve.tbl_inventory_Id).FirstOrDefault();
                if (get_inventory != null)
                {
                    int item = Convert.ToInt16(get_inventory.item_count - Eve.Item_Transfer_Count);
                    get_inventory.item_count = item;
                    dba.Entry(get_inventory).State = EntityState.Modified;
                    dba.SaveChanges();
                }
                Message = "Item transfer is created successfully";
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
                throw;
            }
            return new JsonResult { Data = new { Message = Message, Status = flag } };
        }










        [HttpPost]
        public JsonResult UpdateMyInventoryDetailsByStatus(tbl_inventory_details ob)
        {
            string Message;
            bool flag = false;
            try
            {
                var get_details = dba.tbl_inventory_details.Where(a => a.id == ob.id).FirstOrDefault();
                if (get_details != null)
                {
                    get_details.m_item_status_id = ob.m_item_status_id;
                    get_details.item_serial_No = ob.item_serial_No;


                    dba.Entry(get_details).State = EntityState.Modified;
                    dba.SaveChanges();
                    flag = true;
                    Message = "Item Status is updated successfully";
                }
                else
                {
                    flag = false;
                    Message = "some error occurred !!";
                }
            }

            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
                throw;
            }

            return new JsonResult { Data = new { Message = Message, Status = flag } };
        }
        /// <summary>
        /// add Virtual quantity and add also in inventory details table
        /// </summary>
        /// <param name="ob"></param>
        /// <returns></returns>
        [HttpPost]
        public JsonResult AddVirtualQuantity(tbl_inventory ob)
        {
            cf = new comman_function();
            bool ss = cf.session_check();
            string Message;
            bool flag = false;
            try
            {
                int sellers_id = Convert.ToInt32(Session["SellerID"]);
                var get_details = dba.tbl_inventory.Where(a => a.id == ob.id).FirstOrDefault();
                if (get_details != null)
                {
                    int itemcount = Convert.ToInt16(get_details.item_count);
                    get_details.item_count = itemcount + ob.t_virtualItemCount;
                    get_details.t_virtualItemCount = ob.t_virtualItemCount;
                    dba.Entry(get_details).State = EntityState.Modified;
                    dba.SaveChanges();
                    flag = true;

                    if (ob.t_virtualItemCount > 0)
                    {
                        for (var item1 = 0; item1 < ob.t_virtualItemCount; item1++)
                        {
                            tbl_inventory_details objinventorydetails = new tbl_inventory_details();
                            objinventorydetails.tbl_inventory_id = ob.id;
                            objinventorydetails.created_on = DateTime.Now;
                            objinventorydetails.n_virtualStatus = 0;
                            objinventorydetails.isactive = 1;
                            objinventorydetails.tbl_sellers_id = sellers_id;
                            dba.tbl_inventory_details.Add(objinventorydetails);
                            dba.SaveChanges();
                        }
                    }

                    Message = "Virtual Quantity is Added successfully";
                }
                else
                {
                    flag = false;
                    Message = "some error occurred !!";
                }
            }

            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
                throw;
            }

            return new JsonResult { Data = new { Message = Message, Status = flag } };
        }

        public ActionResult SellerMarketPlace()
        {
            cf = new comman_function();
            if (!cf.session_check())
                return RedirectToAction("Login", "Home");
            return View();
        }

        public JsonResult FillMarketPlaceID()
        {
            cf = new comman_function();
            bool ss = cf.session_check();
            int sellerId = Convert.ToInt16(Session["SellerID"]);
            var Marketplace_details = db.m_marketplace.Where(a => a.isactive == 1).ToList();
            return new JsonResult { Data = Marketplace_details, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        public JsonResult GetSellermarketDetails()
        {
            cf = new comman_function();
            bool ss = cf.session_check();
            int sellers_id = Convert.ToInt32(Session["SellerID"]);
            var get_details = (from ob_tbl_sellermarketplace in dba.tbl_sellermarketplace
                               select new SellerUtility
                               {
                                   ob_tbl_sellermarketplace = ob_tbl_sellermarketplace

                               }).Where(a => a.ob_tbl_sellermarketplace.isactive == 1 && a.ob_tbl_sellermarketplace.tbl_seller_id == sellers_id).OrderByDescending(a => a.ob_tbl_sellermarketplace.id).ToList();




            List<viewsellermarketplace> lst_sellermarketplace = new List<viewsellermarketplace>();
            foreach (var item in get_details)
            {
                viewsellermarketplace sellermarketplace = new viewsellermarketplace();
                var abc = db.m_marketplace.Where(a => a.isactive == 1 && a.id == item.ob_tbl_sellermarketplace.m_marketplace_id).FirstOrDefault();
                if (abc != null)
                {
                    sellermarketplace.m_marketplace_id = abc.id;
                    sellermarketplace.MarketplaceName = abc.name;
                    sellermarketplace.ImagePath = abc.logo_path;
                }
                sellermarketplace.my_unique_id = item.ob_tbl_sellermarketplace.my_unique_id;
                sellermarketplace.t_loginName = item.ob_tbl_sellermarketplace.t_loginName;
                sellermarketplace.t_password = item.ob_tbl_sellermarketplace.t_password;

                sellermarketplace.t_secret_Key = item.ob_tbl_sellermarketplace.t_secret_Key;
                sellermarketplace.t_auth_token = item.ob_tbl_sellermarketplace.t_auth_token;
                sellermarketplace.t_access_Key_id = item.ob_tbl_sellermarketplace.t_access_Key_id;


                sellermarketplace.id = item.ob_tbl_sellermarketplace.id;
                sellermarketplace.market_palce_id = item.ob_tbl_sellermarketplace.market_palce_id;
                //sellermarketplace.GSTN_No = item.ob_tbl_sellermarketplace.GSTN_No;



                lst_sellermarketplace.Add(sellermarketplace);
            }
            return new JsonResult { Data = lst_sellermarketplace, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        public JsonResult DeleteSellerMarket(string id)
        {
            try
            {
                int no = Convert.ToInt32(id);
                tbl_sellermarketplace objtbl = dba.tbl_sellermarketplace.Where(a => a.id == no).FirstOrDefault();
                objtbl.isactive = 0;
                dba.Entry(objtbl).State = EntityState.Modified;
                dba.SaveChanges();
                string Message = "Entry is deleted Successfully";
                return Json(Message, JsonRequestBehavior.AllowGet);
            }
            catch (Exception exp)
            {
                return Json("Error in getting record !", JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public JsonResult SaveMarketPlaceDetails(tbl_sellermarketplace obj)
        {
            cf = new comman_function();
            bool ss = cf.session_check();
            int sellers_id = Convert.ToInt32(Session["SellerID"]);

            bool flag = false;
            try
            {
                //string Message;
                var get_details = dba.tbl_sellermarketplace.Where(a => a.m_marketplace_id == obj.m_marketplace_id && a.tbl_seller_id == sellers_id && a.isactive == 1).FirstOrDefault();
                if (get_details == null)
                {
                   var get_marketplace = db.m_marketplace.Where(a => a.id == obj.m_marketplace_id && a.isactive == 1).FirstOrDefault();
                    if(get_marketplace != null)
                    {
                        obj.MarketPlaceName = get_marketplace.name;
                    }
                    obj.isactive = 1;
                    obj.tbl_seller_id = sellers_id;
                    obj.createdon = DateTime.Now;
                    dba.tbl_sellermarketplace.Add(obj);
                    dba.SaveChanges();
                    flag = true;
                    //Message = "Seller Market Details is created successfully";
                }
                else
                {
                    return new JsonResult { Data = new { Message = "Already Added", Status = flag } };
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
                throw;
            }
            return new JsonResult { Data = new { Message = "Seller Market Details is created successfully", Status = flag } };
        }


        [HttpPost]
        public JsonResult UpdateDetails(viewsellermarketplace objm)
        {
            string Message;
            bool flag = false;
            try
            {
                var get_details = dba.tbl_sellermarketplace.Where(a => a.id == objm.id).FirstOrDefault();
                if (get_details != null)
                {
                    get_details.m_marketplace_id = objm.m_marketplace_id;
                    get_details.my_unique_id = objm.my_unique_id;
                    get_details.t_loginName = objm.t_loginName;
                    get_details.t_password = objm.t_password;
                    get_details.t_access_Key_id = objm.t_access_Key_id;
                    get_details.t_auth_token = objm.t_auth_token;
                    get_details.t_secret_Key = objm.t_secret_Key;
                    get_details.market_palce_id = objm.market_palce_id;
                    //get_details.GSTN_No = objm.GSTN_No;
                    dba.Entry(get_details).State = EntityState.Modified;
                    dba.SaveChanges();
                    flag = true;
                    Message = "Seller Market Place is updated successfully";
                }
                else
                {
                    flag = false;
                    Message = "some error occurred !!";
                }
            }

            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
                throw;
            }

            return new JsonResult { Data = new { Message = Message, Status = flag } };
        }

        /// <summary>
        /// This is for Add Sales SKU code in tbl item_sales_Association
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>

        public JsonResult GetManageSaleAssociation(int? id)
        {
            cf = new comman_function();
            bool ss = cf.session_check();
            model_sales_Association objtblsale = new model_sales_Association();
            int SellerId = Convert.ToInt32(Session["SellerID"]);
            var itemname = dba.tbl_inventory.Where(a => a.id == id && a.isactive == 1 && a.tbl_sellers_id == SellerId).FirstOrDefault();
            objtblsale.ddlInventoryList = new SelectList((dba.tbl_inventory.Where(m => m.isactive == 1 && m.tbl_sellers_id == SellerId && m.id == id).Select(p => new { p.id, p.item_name })).ToList(), "id", "item_name");
            var marketplacedetails = dba.tbl_sellermarketplace.Where(a => a.tbl_seller_id == SellerId && a.isactive == 1).ToList();

            //objtblsale.ddlMarketPlaceList = new SelectList((db.m_marketplace.Where(m => m.isactive == 1).Select(p => new { p.id, p.name })).ToList(), "id", "name");
            objtblsale.tbl_inventory_id = itemname.id;
            objtblsale.ItemName = itemname.item_name;
            string tempdata = itemname.item_photo1_path;
            string[] arrdata = Regex.Split(tempdata, "/");
            string aa = arrdata[1].Replace("/", "");
            string bb = arrdata[2].Replace("/", "");
            string cc = arrdata[3].Replace("/", "");
            objtblsale.Image = cc;
            List<salesDetails> lstsalesdetails = new List<salesDetails>();
            foreach (var item in marketplacedetails)
            {
                if (item != null)
                {
                    var ddd = db.m_marketplace.Where(m => m.isactive == 1 && m.id == item.m_marketplace_id).Select(p => new { p.id, p.name }).FirstOrDefault();
                    salesDetails details = new salesDetails();
                    details.MarketPlaceName = ddd.name;
                    details.m_marketplace_id = ddd.id;
                    details.tbl_inventory_id = objtblsale.tbl_inventory_id;
                    details.ItemName = objtblsale.ItemName;
                    details.ImagePAth = objtblsale.Image;
                    lstsalesdetails.Add(details);
                }
            }
            ///Upload/InventoryManage/Hydrangeas20170916115448752.jpg
            return Json(lstsalesdetails, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult AddSAlesSKUCODE(salesDetails Eve)
        {
            tbl_item_sale_association obj = new tbl_item_sale_association();
            cf = new comman_function();
            bool ss = cf.session_check();
            string Message;
            bool flag = false;
            try
            {
                if (Eve != null)
                {
                    obj.m_marketplace_id = Eve.m_marketplace_id;
                    obj.tbl_inventory_id = Eve.tbl_inventory_id;
                    obj.model_seller_code = Eve.model_seller_code;
                    obj.is_active = 1;
                    obj.tbl_seller_id = Convert.ToInt32(Session["SellerID"]);
                    obj.created_on = DateTime.Now;
                    dba.tbl_item_sale_association.Add(obj);
                    dba.SaveChanges();
                }
                Message = "Item transfer is created successfully";
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
                throw;
            }
            return new JsonResult { Data = new { Message = Message, Status = flag } };
        }

        /// <summary>
        /// This is for Publish Item Code
        /// </summary>
        /// <param name="id"></param>
        /// <returns></returns>
        public JsonResult GetPublishItem(int? id)
        {
            cf = new comman_function();
            bool ss = cf.session_check();
            int SellerId = Convert.ToInt32(Session["SellerID"]);
            var itemname = dba.tbl_inventory.Where(a => a.id == id && a.isactive == 1 && a.tbl_sellers_id == SellerId).FirstOrDefault();
            var marketplacedetails = dba.tbl_sellermarketplace.Where(a => a.tbl_seller_id == SellerId && a.isactive == 1).ToList();
            List<publishDetails> lstdetails = new List<publishDetails>();
            foreach (var item in marketplacedetails)
            {
                if (item != null)
                {
                    var ddd = db.m_marketplace.Where(m => m.isactive == 1 && m.id == item.m_marketplace_id).Select(p => new { p.id, p.name }).FirstOrDefault();
                    var publishlist = dba.tbl_publish_item.Where(a => a.tbl_inventory_id == id && a.t_MarketPlace_id == item.m_marketplace_id && a.is_active == 1 && a.tbl_seller_id == SellerId).FirstOrDefault();
                    publishDetails details = new publishDetails();
                    details.MarketPlaceName = ddd.name;
                    details.m_marketplace_id = ddd.id;
                    details.tbl_inventory_id = itemname.id;
                    details.ItemName = itemname.item_name;
                    details.ImagePAth = itemname.item_photo1_path;
                    details.total_count = Convert.ToInt16(itemname.item_count);
                    if (publishlist != null)
                    {
                        details.CurrentItem = publishlist.t_current_item;
                    }
                    lstdetails.Add(details);
                }
            }
            ///Upload/InventoryManage/Hydrangeas20170916115448752.jpg
            return Json(lstdetails, JsonRequestBehavior.AllowGet);
        }

        [HttpPost]
        public JsonResult AddPublishItem(publishDetails Eve)
        {
            cf = new comman_function();
            bool ss = cf.session_check();
            tbl_publish_item obj = new tbl_publish_item();
            string Message;
            bool flag = false;
            try
            {
                if (Eve != null)
                {
                    int SellerId = Convert.ToInt32(Session["SellerID"]);
                    var get_publishdetails = dba.tbl_publish_item.Where(a => a.t_MarketPlace_id == Eve.m_marketplace_id && a.tbl_inventory_id == Eve.tbl_inventory_id && a.tbl_seller_id == SellerId).FirstOrDefault();

                    var get_inventorydetails = dba.tbl_inventory.Where(a => a.id == Eve.tbl_inventory_id).FirstOrDefault();
                    if (get_publishdetails != null)
                    {
                        get_publishdetails.t_current_item = Eve.NewItem;
                        get_publishdetails.t_New_ItemCount = Eve.NewItem;
                        dba.Entry(get_publishdetails).State = EntityState.Modified;
                        dba.SaveChanges();
                        flag = true;

                        if (get_inventorydetails != null)
                        {
                            int itemcount = Convert.ToInt16(get_inventorydetails.item_count);
                            int publishcount = Eve.total_count;
                            int ftotal = publishcount - itemcount;
                            get_inventorydetails.t_virtualItemCount = ftotal;
                            dba.Entry(get_inventorydetails).State = EntityState.Modified;
                            dba.SaveChanges();
                        }
                    }

               //if (Eve != null)
                    //{
                    else
                    {
                        obj.t_MarketPlace_id = Eve.m_marketplace_id;
                        obj.tbl_inventory_id = Eve.tbl_inventory_id;
                        obj.t_current_item = Eve.NewItem;
                        obj.t_New_ItemCount = Eve.NewItem;
                        obj.t_Total_Item = Eve.TotalItem;
                        obj.is_active = 1;
                        obj.tbl_seller_id = Convert.ToInt32(Session["SellerID"]);
                        obj.created_on = DateTime.Now;
                        dba.tbl_publish_item.Add(obj);
                        dba.SaveChanges();
                    }
                }
                Message = "Item transfer is created successfully";
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
                throw;
            }
            return new JsonResult { Data = new { Message = Message, Status = flag } };
        }

        /// <summary>
        /// This is for Add/Update/Delete for Courier Company Data
        /// </summary>
        /// <returns></returns>
        public ActionResult CourierComapny()
        {
            cf = new comman_function();
            if (!cf.session_check())
                return RedirectToAction("Login", "Home");
            return View();
        }
        public JsonResult GetCourierDetails()
        {
            cf = new comman_function();
            bool ss = cf.session_check();
            int sellers_id = Convert.ToInt32(Session["SellerID"]);
            var get_details = (from ob_tbl_courier_comapny in dba.tbl_courier_comapny
                               select new SellerUtility
                               {
                                   ob_tbl_courier_comapny = ob_tbl_courier_comapny

                               }).Where(a => a.ob_tbl_courier_comapny.is_active == 0 && a.ob_tbl_courier_comapny.tbl_seller_id == sellers_id).OrderByDescending(a => a.ob_tbl_courier_comapny.id).ToList();

            return new JsonResult { Data = get_details, JsonRequestBehavior = JsonRequestBehavior.AllowGet };
        }

        public JsonResult DeleteCourier(string id)
        {
            try
            {
                int no = Convert.ToInt32(id);
                tbl_courier_comapny objtbl = dba.tbl_courier_comapny.Where(a => a.id == no).FirstOrDefault();
                objtbl.is_active = 1;
                dba.Entry(objtbl).State = EntityState.Modified;
                dba.SaveChanges();
                string Message = "Entry is deleted Successfully";
                return Json(Message, JsonRequestBehavior.AllowGet);
            }
            catch (Exception exp)
            {
                return Json("Error in getting record !", JsonRequestBehavior.AllowGet);
            }
        }

        [HttpPost]
        public JsonResult SaveCourierDetails(tbl_courier_comapny obj)
        {
            cf = new comman_function();
            bool ss = cf.session_check();
            int sellers_id = Convert.ToInt32(Session["SellerID"]);
            string Message;
            bool flag = false;
            try
            {
                obj.is_active = 0;
                obj.tbl_seller_id = sellers_id;
                obj.created_on = DateTime.Now;
                dba.tbl_courier_comapny.Add(obj);
                dba.SaveChanges();
                flag = true;
                Message = "Courier Company is created successfully";
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
                throw;
            }
            return new JsonResult { Data = new { Message = Message, Status = flag } };
        }

        [HttpPost]
        public JsonResult UpdateCourierDetails(tbl_courier_comapny objm)
        {
            string Message;
            bool flag = false;
            try //AddCategory
            {
                var get_details = dba.tbl_courier_comapny.Where(a => a.id == objm.id).FirstOrDefault();
                if (get_details != null)
                {
                    get_details.courier_company_name = objm.courier_company_name;
                    dba.Entry(get_details).State = EntityState.Modified;
                    dba.SaveChanges();
                    flag = true;
                    Message = "Courier Company is updated successfully";
                }
                else
                {
                    flag = false;
                    Message = "some error occurred !!";
                }
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
                throw;
            }
            return new JsonResult { Data = new { Message = Message, Status = flag } };
        }
        //-----------------------------------Medhvani Code-------------------------------

        public ActionResult ManageCategory()
        {
            return View();
        }
        public JsonResult UpdateCategoryList(tbl_category_slabs obj)
        {
            string Message;
            bool flag = false;
            cf = new comman_function();
            bool ss = cf.session_check();
            try
            {
                var get_details = dba.tbl_category_slabs.FirstOrDefault();
                if (get_details != null)
                {
                    get_details.m_category_id = obj.m_category_id;
                    //get_details.tax_rate = obj.tax_rate;

                    dba.Entry(get_details).State = EntityState.Modified;
                    dba.SaveChanges();
                    flag = true;
                    Message = "Category is updated successfully";
                }
                else
                {
                    flag = false;
                    Message = "some error occurred !!";
                }
            }

            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
                throw;
            }

            return new JsonResult { Data = new { Message = Message, Status = flag } };
        }

        public JsonResult ManageCategoryList()
        {
            int SellerId = Convert.ToInt32(Session["SellerID"]);
            List<ViewCategoryList> objlist = new List<ViewCategoryList>();
            var get_category = dba.tbl_item_category.Where(a => a.tbl_sellers_id == SellerId).OrderByDescending(a =>a.id).ToList();
            if (get_category != null)
            {
                ViewCategoryList obj = null;
                foreach (var item in get_category)
                {
                    obj = new ViewCategoryList();
                    obj.Id = item.id;
                    obj.CategoryName = item.category_name;
                    var get_taxdetails = dba.tbl_category_slabs.Where(a => a.tbl_seller_id == SellerId && a.m_category_id == item.id).OrderByDescending(a => a.id).FirstOrDefault();
                    if (get_taxdetails != null)
                    {

                        if (get_taxdetails.tax_rate != null)
                        {
                            obj.CategoryTax = get_taxdetails.tax_rate;
                        }
                    }
                    objlist.Add(obj);
                }

            }
            return Json(objlist, JsonRequestBehavior.AllowGet);
        }

        public JsonResult SelectedItem(int? id, int? category_id)
        {
            string message = "Success";
            try
            {
                int SellerId = Convert.ToInt32(Session["SellerID"]);
                var getdetails = dba.tbl_inventory.Where(a => a.id == id && a.tbl_sellers_id == SellerId).FirstOrDefault();

                if (getdetails != null)
                {                  
                    getdetails.tbl_item_category_id = category_id;
                    dba.Entry(getdetails).State = EntityState.Modified;
                    dba.SaveChanges();
                }
            }
            catch (Exception ex)
            {
            }
            return Json(message, JsonRequestBehavior.AllowGet);
        }


        public JsonResult UpdateCategoryWithSlab(tbl_item_category Eve, List<tbl_category_slabs> category_slabs)
        {
            string Message="";
            bool flag = false;
            try
            {
                var get_details = dba.tbl_item_category.Where(a => a.id == Eve.id).FirstOrDefault();
                if (get_details != null)
                {
                    get_details.hsn_code = Eve.hsn_code;
                    get_details.category_name = Eve.category_name;                  
                    dba.Entry(get_details).State = EntityState.Modified;
                    dba.SaveChanges();
                    flag = true;
                    Message = "Category is updated successfully";
                    if(category_slabs != null)
                    {
                        foreach(var item in category_slabs )
                        {
                            var get_slabdetails = dba.tbl_category_slabs.Where(a => a.id == item.id).FirstOrDefault();
                            if (get_slabdetails != null)
                            {
                                get_slabdetails.tax_rate = item.tax_rate;
                                get_slabdetails.to_rs = item.to_rs;
                                get_slabdetails.from_rs = item.from_rs;
                                dba.Entry(get_slabdetails).State = EntityState.Modified;
                                dba.SaveChanges();
                            }
                            else
                            {
                                tbl_category_slabs obj_slab = new tbl_category_slabs();
                                obj_slab.from_rs = item.from_rs;
                                obj_slab.to_rs = item.to_rs;
                                obj_slab.tax_rate = item.tax_rate;
                                obj_slab.tbl_seller_id = get_details.tbl_sellers_id;
                                obj_slab.m_category_id = get_details.id;
                                dba.tbl_category_slabs.Add(obj_slab);
                                dba.SaveChanges();
                            }
                        }
                    }
                    
                }
                else
                {
                    flag = false;
                    Message = "some error occurred !!";
                }
            }
            catch (DbEntityValidationException e)
            {
                foreach (var eve in e.EntityValidationErrors)
                {
                    Debug.WriteLine(@"Entity of type has the following validation errors:", eve.Entry.Entity.GetType().Name, eve.Entry.State);
                    foreach (var ve in eve.ValidationErrors)
                    {
                        Debug.WriteLine("- Property: \"{0}\", Error: \"{1}\"",
                            ve.PropertyName, ve.ErrorMessage);
                    }
                }
                throw;
            }
            catch (DbUpdateException dbu)
            {
                var exception = HandleDbUpdateException(dbu);
                throw exception;
            }
            catch (Exception e)
            {
                Debug.WriteLine(e.Message);
                throw;
            }

            return new JsonResult { Data = new { Message = Message, Status = flag } };
        }


        //-----------------------------------End----------------------------------------//


    }
}
