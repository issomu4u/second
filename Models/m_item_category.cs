using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SellerVendor.Models
{
    public class m_item_category
    {
        public int id { get; set; }
        public string category_name { get; set; }       
        public double tax_rate { get; set; }
        public DateTime updated_on { get; set; }
        public int isactive { get; set; }
    }
}