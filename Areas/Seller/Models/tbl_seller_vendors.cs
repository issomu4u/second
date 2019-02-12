using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SellerVendor.Areas.Seller.Models
{
    public class tbl_seller_vendors
    {
        public int id { get; set; }

        public int tbl_sellersid { get; set; }
        public string vendor_name { get; set; }
        public string mobile { get; set; }
        public string telephone { get; set; }
        public string email { get; set; }
        public string address { get; set; }

        public string contact_person { get; set; }
        public int country { get; set; }
        public int state { get; set; }
        public string city { get; set; }
        public string pincode { get; set; }
        public DateTime date_created { get; set; }

        public string gstin { get; set; }
        public string pan { get; set; }
        public int status { get; set; }
    }
}