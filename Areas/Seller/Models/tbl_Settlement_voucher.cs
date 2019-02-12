using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SellerVendor.Areas.Seller.Models
{
    public class tbl_Settlement_voucher
    {
        public int Id { get; set; }
        public double? principal_price { get; set; }
        public double? product_tax { get; set; }
        public double? shipping_price { get; set; }
        public double? shipping_tax { get; set; }
        public double? giftwrap_price { get; set; }
        public double? giftwarp_tax { get; set; }
        public double? shipping_discount { get; set; }
        public double? shipping_tax_discount { get; set; }

        public string unique_order_id { get; set; }
        public string Order_Id { get; set; }
        public double? INCORRECT_FEES_ITEMS { get; set; }
        public double? SAFE_T_Reimbursement { get; set; }

        public string return_fee { get; set; }
        public short? t_transactionType_id { get; set; }
        public int? expense_type_id { get; set; }
        public double? expense_amount { get; set; }
        public Nullable<double> CGST_amount { get; set; }
        public Nullable<double> Igst_amount { get; set; }
        public Nullable<double> sgst_amount { get; set; }
    }
}