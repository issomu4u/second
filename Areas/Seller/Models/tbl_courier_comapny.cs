using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SellerVendor.Areas.Seller.Models
{
    public class tbl_courier_comapny
    {
        public string courier_company_name { get; set; }
        public int id { get; set; }
        public DateTime created_on { get; set; }
        public int is_active { get; set; }
        public int tbl_seller_id { get; set; }
    }
}