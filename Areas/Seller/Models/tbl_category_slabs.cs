using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SellerVendor.Areas.Seller.Models
{
    public class tbl_category_slabs
    {
        public int id { get; set; }
        public int? m_category_id { get; set; }
        public int? tbl_seller_id { get; set; }
        public int? from_rs { get; set; }
        public int? to_rs { get; set; }
        public int? tax_rate { get; set; }
    }
    public class category_slabs
    {
        public int id { get; set; }
        public string category_name { get; set; }
        public string hsn_code { get; set; }
        public List<partial_slabs> obj_partial_slabs { get; set; }
    }
    public partial class partial_slabs
    {
        public int id { get; set; }
        public int? from_rs { get; set; }
        public int? to_rs { get; set; }
        public int? tax_rate { get; set; }

    }
}