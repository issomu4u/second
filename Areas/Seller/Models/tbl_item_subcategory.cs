using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SellerVendor.Areas.Seller.Models
{
    public class tbl_item_subcategory
    {        
        public int id { get; set; }
        public string subcategory_name { get; set; }
        public string hsn_code { get; set; }
        public double tax_rate { get; set; }
        public int tbl_item_category_id { get; set; }
        public int updated_by { get; set; }
        public DateTime updated_on { get; set; }
        public int isactive { get; set; }
        public int tbl_sellers_id { get; set; }
    }
}