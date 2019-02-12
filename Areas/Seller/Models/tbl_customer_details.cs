using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SellerVendor.Areas.Seller.Models
{
    public class tbl_customer_details
    {
        public int id { get; set; }
        public string Address_1 { get; set; }
        public string Address_2 { get; set; }
        public string shipping_Buyer_Name { get; set; }
        public string City { get; set; }
        public string Country_Code { get; set; }

        public string Postal_Code { get; set; }
        public string Shipping_Name { get; set; }
        public string State_Region { get; set; }
        public int tbl_Sales_OrderId { get; set; }
        public string last_name { get; set; }
        public string state_code { get; set; }
        public string landmark { get; set; }
        public string contact_no { get; set; }
        public string state { get; set; }
        public string customer_email { get; set; }
        public int? customer_count { get; set; }
        public int? tbl_seller_id { get; set; }
    }
}