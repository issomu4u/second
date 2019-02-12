using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SellerVendor.Areas.Seller.Models
{
    public class tbl_sales_order_status
    {
        public int id { get; set; }
        public int is_active { get; set; }
        public string sales_order_status { get; set; }      
    }
}