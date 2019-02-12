using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SellerVendor.Models
{
    public class m_database
    {
        public int id { get; set; }
        public string database_name { get; set; }
        public string ipaddress { get; set; }      
        public int current_sellers { get; set; }
        public int max_sellers { get; set; }              
        public DateTime created_on { get; set; }
    }
}