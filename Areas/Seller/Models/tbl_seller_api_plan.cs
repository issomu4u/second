using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SellerVendor.Areas.Seller.Models
{
    public class tbl_seller_api_plan
    {
        public int id { get; set; }
        public int seller_id { get; set; }
        public int api_execution_hours { get; set; }
        public DateTime last_execution_date { get; set; }
    }
}