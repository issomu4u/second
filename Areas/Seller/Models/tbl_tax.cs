using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SellerVendor.Areas.Seller.Models
{
    public class tbl_tax
    {
        public double? cgst_tax { get; set; }
        public int id { get; set; }
        public double? igst_tax { get; set; }
        public int isactive { get; set; }
        public double? rate_of_tax { get; set; }
        public double? sgst_tax { get; set; }
        public int? tbl_referneced_id { get; set; }
        public int tbl_seller_id { get; set; }
        public Nullable<double> CGST_amount { get; set; }
        public Nullable<double> Igst_amount { get; set; }
        public Nullable<double> rateoftax_amount { get; set; }
        public Nullable<double> sgst_amount { get; set; }
        public Nullable<double> t_totaltax_amount { get; set; }
        public Nullable<double> tax_paid { get; set; }
        public int? reference_type { get; set; }
        public double? giftwarp_tax { get; set; }
        public double? shippint_tax_amount { get; set; }
        public double? product_tax { get; set; }
        public int? tbl_history_id { get; set; }
    }
}