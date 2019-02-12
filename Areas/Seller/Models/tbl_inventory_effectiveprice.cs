using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SellerVendor.Areas.Seller.Models
{
    public class tbl_inventory_effectiveprice
    {
        public int Id { get; set; }
        public int? tbl_inventory_id { get; set; }
        public int? tbl_sellerid { get; set; }
        public DateTime? Effecive_date { get; set; }
        public double? Effective_price { get; set; }
        public double? Item_Tax { get; set; }
        public double? Gross_price { get; set; }
    }

    public class  tbl_effectiveprice
    {
        public int Id { get; set; }
        public int? tbl_inventory_id { get; set; }
        public int? tbl_sellerid { get; set; }
        public string Effecive_date { get; set; }
        public double? Effective_price { get; set; }
        public double? Item_Tax { get; set; }
        public double? Gross_price { get; set; }
    }
}