using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SellerVendor.Areas.Seller.Models
{
    public class tbl_seller_warehouses
    {
        public int id { get; set; }
        public string warehouse_name { get; set; }
        public string contact_person { get; set; }
        public string email { get; set; }
        public string mobile { get; set; }
        public int tbl_sellers_id { get; set; }
        public string address { get; set; }      
        public int country { get; set; }
        public int state { get; set; }
        public string city { get; set; }     
        public int isactive { get; set; }      
        public int created_by { get; set; }
        public DateTime date_created { get; set; }
        public Nullable<int> n_default_warehouse { get; set; }
    }
}