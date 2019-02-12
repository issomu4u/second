using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SellerVendor.Areas.Seller.Models
{
    public class api_parameter
    {
        public int id { get; set; }
        public string Seller_ID { get; set; }
        public string Secret_Key  { get; set; }
        public string AWS_Access_Key_ID  { get; set; }
        public string marketplaceid { get; set; }
        public DateTime createafter { get; set; }
        public DateTime createbefore { get; set; }
        public string auth_token { get; set; }
        public int m_marketplace_id { get; set; }
    }
    public class seller_parameter_list
    {
        public string id { get; set; }
        public string my_unique_id { get; set; }
        public string my_seller_id { get; set; }
        public string t_loginName { get; set; }
        public string t_password { get; set; }
        public string t_access_Key_id { get; set; }
        public string t_auth_token { get; set; }
        public string t_secret_Key { get; set; }
        public string market_palce_id { get; set; }
        public string m_marketplace_id { get; set; }
    
    }
}