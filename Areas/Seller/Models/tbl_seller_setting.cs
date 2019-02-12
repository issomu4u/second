using System;
using System.Collections.Generic;
using System.ComponentModel.DataAnnotations;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SellerVendor.Areas.Seller.Models
{
    public class tbl_seller_setting
    {
        public DateTime created_on { get; set; }
        public int id { get; set; }
        public int is_active { get; set; }
        public int n_financial_status { get; set; }
        public int tbl_seller_id { get; set; }
        public string t_financial_year { get; set; }
        public string t_seller_prefix_code { get; set; }
        public int t_running_no { get; set; }
        public int? type { get; set; }
        public int? current_running_no { get; set; }
    }

    public partial class model_tbl_seller_setting
    {
        public int id { get; set; }
        public DateTime created_on { get; set; }
        public int is_active { get; set; }
        public int n_financial_status { get; set; }
        public int tbl_seller_id { get; set; }
        public string t_financial_year { get; set; }
        public string t_seller_prefix_code { get; set; }
        //public int t_seller_prefix_no { get; set; }
        public string t_Mode { get; set; }
        public string financialyear { get; set; }
        public int t_running_no { get; set; }
        public string t_oldPassword { get; set; }
        public string t_NewPassword { get; set; }
        public string t_ConfirmPassword { get; set; }
        public string Msg { get; set; }
        public string description { get; set; }
        public double? amount_added { get; set; }
        public DateTime date_of_creation { get; set; }
        public string sett_prefix_code { get; set; }
        public int sett_current_running_no { get; set; }
        public string sales_prefix_code { get; set; }
        public int sales_current_running_no { get; set; }
        public string return_prefix_code { get; set; }
        public int return_current_running_no { get; set; }
        public int invoice_current_running_no { get; set; }

        public int accounting_sft_id { get; set; }
       
        public string address { get; set; }
        public int country { get; set; }
        public int state { get; set; }
        public string city { get; set; }
        public string pincode { get; set; }
        public string business_name { get; set; }

            
        public string mobile { get; set; }
        public string email { get; set; }
        public SelectList ddl_country { get; set; }
        public SelectList ddl_state { get; set; }
        public string pan { get; set; }
        public string contact_person { get; set; }
        public int ProfileID { get; set; }        
        public string gstin { get; set; }
    }

    public partial class DashboardData
    {
        public double PreviousAmount { get; set; }
        public double CurrentAmount { get; set; }
        public int previousItem { get; set; }
        public int currentItem { get; set; }
        public int pendingorder { get; set; }
        public int TotalOrders { get; set; }
        public int TotalSettlementOrders { get; set; }
        public int TotalCancelledOrders { get; set; }
        public int TotalRefundOrders { get; set; }
        public int TotalReturnOrders { get; set; }
        public int UserAmount { get; set; }
    }
    public class LastOrders
    {
        public string Date { get; set; }
        public int TotalOrders { get; set; }
        public int TotalReturn { get; set; }
        public double TotalAmount { get; set; }
        public double TotalExpences { get; set; }
        public double NetRealization { get; set; }
    }
    public class TopProduct
    {
        public string Name { get; set; }
        public decimal Price { get; set; }
        public int Quantity { get; set; }
        public string SKU { get; set; }
    }
}