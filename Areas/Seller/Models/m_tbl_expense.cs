using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SellerVendor.Areas.Seller.Models
{
    public class m_tbl_expense
    {
        public int id { get; set; }
        public DateTime? date_created { get; set; }
        public double? expense_amount { get; set; }
        public int? expense_type_id { get; set; }
        public string reference_number { get; set; }
        public int? tbl_seller_id { get; set; }
        public DateTime? settlement_datetime { get; set; }
        public string Original_order_id { get; set; }
        public string settlement_order_id { get; set; }
        public string sku_no { get; set; }
        public string adjustment_id { get; set; }
        public string promotion_id { get; set; }
        public short? quantity_purchased { get; set; }
        public short? t_transactionType_id { get; set; }
        public int? tbl_order_historyid { get; set; }

    }
}