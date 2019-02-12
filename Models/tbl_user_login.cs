using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SellerVendor.Models
{
    public class tbl_user_login
    {    
        public int id { get; set; }
        public string password { get; set; }
        //public DateTime? lastlogin { get; set; }
        public int m_user_role_id { get; set; }
        public DateTime? last_login { get; set; }
        public int tbl_sellers_id { get; set; }
        public string login_id { get; set; }
        public int isactive { get; set; }
        public int created_by { get; set; }
        public DateTime date_created { get; set; }
        public string Email { get; set; }

        public double? wallet_balance { get; set; }
        public decimal? applied_plan_rate { get; set; }
        public int? total_orders { get; set; }
    }
}