using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SellerVendor.Areas.Seller.Models
{
    public class PurchaseDetailsviewmodel
    {
        public double sold_price { get; set; }
        public int tax_paid { get; set; }

        public double cgst_tax { get; set; }       
        public double igst_tax { get; set; }    
        public decimal rate_of_tax { get; set; }
        public double sgst_tax { get; set; }

        public Nullable<double> CGST_amount { get; set; }
        public Nullable<double> Igst_amount { get; set; }
        public Nullable<double> rateoftax_amount { get; set; }
        public Nullable<double> sgst_amount { get; set; }
        public Nullable<double> t_totaltax_amount { get; set; }
       

        public Nullable<double> base_amount { get; set; }
        public Nullable<int> item_count { get; set; }
        public Nullable<int> tbl_inventory_id { get; set; }
        public int tbl_purchase_id { get; set; }
        public int PurchaseDetailsid { get; set; }
        public string InventoryName { get; set; }
    }
}