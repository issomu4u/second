using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SellerVendor.Models
{
    public class m_item_subcategory
    {
        public int id { get; set; }        
        public int m_item_category_id { get; set; }
        public string subcategory_name { get; set; }
        public string hsn_code { get; set; } 
        public double tax_rate { get; set; }
        public int isactive { get; set; }
        public DateTime updated_on { get; set; }
    }
}