using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SellerVendor.Models
{
    public class temp_item_category
    {
        public DateTime d_date_tax_updated { get; set; }
        public int id { get; set; }
        public int isactive { get; set; }
        public int m_item_category_id { get; set; }
        public double tax_rate { get; set; }
        public int tbl_sellers_id { get; set; }
        public string t_category_name { get; set; }
        public int updated_by { get; set; }
    }
}