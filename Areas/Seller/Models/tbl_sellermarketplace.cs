using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SellerVendor.Areas.Seller.Models
{
    public class tbl_sellermarketplace
    {
        public int id { get; set; }
        public DateTime createdon { get; set; }
        public int isactive { get; set; }
        public string my_unique_id { get; set; }
        public int m_marketplace_id { get; set; }
        public int tbl_seller_id { get; set; }
        public string t_loginName { get; set; }
        public string t_password { get; set; }
        public string t_access_Key_id { get; set; }
        public string t_auth_token { get; set; }
        public string t_secret_Key { get; set; }
        public string market_palce_id { get; set; }
        //public string GSTN_No { get; set; }
        public string MarketPlaceName { get; set; }
    }
    public partial class viewsellermarketplace
    {
        public int id { get; set; }
        public DateTime createdon { get; set; }
        public int isactive { get; set; }
        public string my_unique_id { get; set; }
        public int m_marketplace_id { get; set; }
        public int tbl_seller_id { get; set; }
        public string t_loginName { get; set; }
        public string t_password { get; set; }
        public string MarketplaceName { get; set; }
        public string ImagePath { get; set; }
        public string t_auth_token { get; set; }
        public string t_secret_Key { get; set; }
        public string t_access_Key_id { get; set; }
        public string market_palce_id { get; set; }
        public string GSTN_No { get; set; }
    }
}