using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SellerVendor.Models
{
    public class tbl_country
    {
        public short countryid { get; set; }
        public short countrylevel { get; set; }
        public string countryname { get; set; }
        public string countryurl { get; set; }
        public int id { get; set; }
        public short parentid { get; set; }
        public short status { get; set; }
        public string zipcode { get; set; }
    }
}