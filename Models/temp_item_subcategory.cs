using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SellerVendor.Models
{
    public class temp_item_subcategory
    {
        public int id { get; set; }
        public int isactive { get; set; }
        public int tbl_item_category_id { get; set; }
        public int tbl_sellers_id { get; set; }
        public string t_hsn_code { get; set; }
        public string t_subcategory_name { get; set; }
        public double t_tax_rate { get; set; }
        public int updated_by { get; set; }
        public DateTime updated_on { get; set; }
        public int tbl_sub_category_id { get; set; }
    }
}