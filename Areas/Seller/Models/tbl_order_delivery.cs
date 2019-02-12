using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SellerVendor.Areas.Seller.Models
{
    public class tbl_order_delivery
    {
        public DateTime created_on { get; set; }
        public int id { get; set; }
        public int is_active { get; set; }
        public int n_courier_id { get; set; }
        public int n_sale_order_status { get; set; }
        public int tbl_seller_id { get; set; }
        public string t_awb_number { get; set; }
        public int tbl_sales_order_id { get; set; }
        public DateTime dispatch_date { get; set; }
        public DateTime receive_date { get; set; }
        public string t_Remarks { get; set; }
        public int tbl_sale_orderdetails_id { get; set; }
        public double? t_shipping_price { get; set; }
        public double? t_shipping_tax { get; set; }
        public int? item_serialNo { get; set; }
    }
}