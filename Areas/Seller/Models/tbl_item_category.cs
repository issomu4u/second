using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SellerVendor.Areas.Seller.Models
{
    public class tbl_item_category
    {
        public int id { get; set; }
        public string category_name { get; set; }
        public Nullable<int> m_item_category_id { get; set; }
        public double tax_rate { get; set; }
        public int tbl_sellers_id { get; set; }
        public int updated_by { get; set; }
        public DateTime date_tax_updated { get; set; }
        public int isactive { get; set; }
        public string hsn_code { get; set; }
    }
}