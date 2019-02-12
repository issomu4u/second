using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SellerVendor.Models
{
    public class tbl_user_payment
    {
        public int id { get; set; }
        public int? user_id { get; set; }
        public DateTime date_of_creation { get; set; }
        public double? amount_added { get; set; }
        public int? type { get; set; }
        public string description { get; set; }
    }
}