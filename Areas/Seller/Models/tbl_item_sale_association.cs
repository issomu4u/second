using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.Mvc;

namespace SellerVendor.Areas.Seller.Models
{
    public class tbl_item_sale_association
    {
        public int id { get; set; }
        public string model_seller_code { get; set; }
        public int m_marketplace_id { get; set; }
        public int tbl_inventory_id { get; set; }
        public int tbl_seller_id { get; set; }
        public DateTime created_on { get; set; }
        public int is_active { get; set; }
    }

    public partial class model_sales_Association 
    {
        public int id { get; set; }
        public string model_seller_code { get; set; }
        public int m_marketplace_id { get; set; }
        public int tbl_inventory_id { get; set; }
        public int tbl_seller_id { get; set; }
        public DateTime created_on { get; set; }
        public int is_active { get; set; }
        public string XmlSalesDetails { get; set; }
        public SelectList ddlInventoryList { get; set; }
        public List<SelectListItem> ddlMarketPlaceList { get; set; }
        public List<model_sales_Association> ddlsalesassociation { get; set; }
        public string ItemName { get; set; }
        public string Image { get; set; }
        
    }

    public partial class salesDetails
    {
        public int tbl_inventory_id { get; set; }
        public int m_marketplace_id { get; set; }
        public string model_seller_code { get; set; }
        public string MarketPlaceName { get; set; }
        public string ItemName { get; set; }
        public string ImagePAth { get; set; }
    }
}