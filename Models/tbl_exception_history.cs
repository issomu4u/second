using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SellerVendor.Models
{
    public class tbl_exception_history
    {
        public int id { get; set; }       
        public string Exception { get; set; }        
        public string Page { get; set; }
        public DateTime Date { get; set; }
    }
}