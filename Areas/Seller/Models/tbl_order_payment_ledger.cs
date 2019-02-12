using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SellerVendor.Areas.Seller.Models
{
    public class tbl_order_payment_ledger
    {
        public int id { get; set; }
        public DateTime? created_date { get; set; }
        public string is_Reconciled { get; set; }
        public DateTime? reconciled_on { get; set; }
        public int? tbl_order_id { get; set; }
        public int? tbl_seller_id { get; set; }
        public decimal? t_Amount { get; set; }
        public string t_payment_type { get; set; }
        public string t_remarks { get; set; }
        public int? tblorder_details_id { get; set; }
    }
}