using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SellerVendor.Areas.Seller.Models
{
    public class tbl_product_upload
    {
        public int Id { get; set; }
        public short? source { get; set; }
        public DateTime? uploaded_datetime { get; set; }
        public DateTime? processing_datetime { get; set; }
        public DateTime? completed_datetime { get; set; }
        public DateTime? from_date { get; set; }
        public DateTime? to_datetime { get; set; }
        public int? tbl_seller_id { get; set; }
        public int? tbl_marketplace_id { get; set; }
        public string status { get; set; }
    }
}