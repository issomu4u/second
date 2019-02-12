using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SellerVendor.Models
{
    public class m_marketplace
    {
        public int id { get; set; }
        public string name { get; set; }
        public string logo_path { get; set; }
        public string api_url { get; set; }
        public DateTime date_created { get; set; }       
        public int isactive { get; set; }         
    }
}