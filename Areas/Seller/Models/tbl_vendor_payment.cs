using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SellerVendor.Areas.Seller.Models
{
    public class tbl_vendor_payment
    {
     
        public int id { get; set; }
        public int isactive { get; set; }
        public int n_paymentBy { get; set; }
        public int n_paymentMode { get; set; }
        public int tbl_purchase_id { get; set; }
        public int tbl_seller_id { get; set; }
        public int tbl_vendor_id { get; set; }
        public string t_BankName { get; set; }
        public string t_chequeno { get; set; }
        public string t_PurchaseAmount { get; set; }
        public string po_number { get; set; }
        public string t_Remark { get; set; }
        public DateTime d_date_of_payment { get; set; }
        public double po_invoiceAmount { get; set; }
        public Nullable<DateTime> po_date { get; set; }
        public int t_Payment_status { get; set; }
    }
}