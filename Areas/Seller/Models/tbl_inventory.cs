using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SellerVendor.Areas.Seller.Models
{
    public class tbl_inventory
    {
        public int id { get; set; }
        public string brand { get; set; }
        public string hsn_code { get; set; }
        public string item_code { get; set; }
        public string item_description { get; set; }
        public string item_dimension { get; set; }
        public string item_name { get; set; }
        public string remarks { get; set; }
        public string packed_dimension { get; set; }
        public string sku { get; set; }
        public string item_photo1_path { get; set; }
        public string item_photo2_path { get; set; }
        public string item_photo3_path { get; set; }


        public Nullable<int> item_count { get; set; }
        public Nullable<int> mrp { get; set; }
        public string packed_weight { get; set; }
        public Nullable<int> selling_price { get; set; }

        public Nullable<int> m_item_color_id { get; set; }
        public Nullable<int> tbl_item_category_id { get; set; }
        public Nullable<int> tbl_item_subcategory_id { get; set; }
        public Nullable<int> tbl_sellers_id { get; set; }

        public Nullable<int> isactive { get; set; }

        public string item_weight { get; set; }
        public Nullable<double> transfer_price { get; set; }

        public Nullable<DateTime> lastupdated { get; set; }
        public Nullable<int> item_No { get; set; }
        public double? t_averagebought_price { get; set; }
        public double? t_effectiveBought_price { get; set; }
        public int? t_virtualItemCount { get; set; }
        public int? lead_time_to_ship { get; set; }
        public int? n_fullfilled_id { get; set; }
        public int? tbl_details_item_id { get; set; }
        public short? tax_update { get; set; }

        public int? tbl_marketplace_id { get; set; }
        public int? tbl_product_upload_id { get; set; }
        public string asin_no { get; set; }
    }
    public partial class ViewCategoryList
    {
        public int Id { get; set; }
        public string CategoryName { get; set; }
        public decimal? CategoryTax { get; set; }
    }
}