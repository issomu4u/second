using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SellerVendor.Areas.Seller.Models
{
    public class tbl_purchase_details
    {
        public Nullable<double> base_amount { get; set; }
       
        public int id { get; set; }
        public int tbl_purchase_id { get; set; }
        public int tbl_sellers_id { get; set; }
        public Nullable<int> isactive { get; set; }
        public Nullable<int> item_count { get; set; }
        public Nullable<int> tbl_inventory_id { get; set; }

        //public Nullable<double> igst_tax { get; set; }
        //public Nullable<decimal> rate_of_tax { get; set; }
        //public Nullable<double> sgst_tax { get; set; }       
        //public Nullable<double> cgst_tax { get; set; }
    }
}