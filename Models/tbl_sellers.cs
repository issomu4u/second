using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SellerVendor.Models
{
    public class tbl_sellers
    {
        public int id { get; set; }
        public int db_name { get; set; }
        public string db_pwd { get; set; }
        public string address { get; set; }
        public int country { get; set; }
        public int state { get; set; }
        public string city { get; set; }
        public string pincode { get; set; }
        public string business_name { get; set; }
        public string mobile { get; set; }
        public string email { get; set; }


        public string pan { get; set; }
        public string contact_person { get; set; }
        public string referred_by { get; set; }

        public int created_by { get; set; }
        public DateTime date_created { get; set; }
        public DateTime date_of_signup { get; set; }



        public string gstin { get; set; }

        public int isactive { get; set; }

        public int m_source_of_joining_id { get; set; }
        public Nullable<int> tbl_type_id { get; set; }
        public Nullable<int> t_seller_typeid { get; set; }
    }


    public class partial_tbl_sellers
    {
        public int id { get; set; }
        public int db_name { get; set; }
        public string db_pwd { get; set; }
        public string address { get; set; }
        public int country { get; set; }
        public int state { get; set; }
        public string city { get; set; }
        public string pincode { get; set; }
        public string business_name { get; set; }
        public string mobile { get; set; }
        public string email { get; set; }
        public decimal? applied_plan_rate { get; set; }

        public string pan { get; set; }
        public string contact_person { get; set; }
        public string referred_by { get; set; }

        public int created_by { get; set; }
        public DateTime date_created { get; set; }
        public DateTime date_of_signup { get; set; }

        public double? wallet_balance { get; set; }

        public string gstin { get; set; }

        public int isactive { get; set; }

        public int m_source_of_joining_id { get; set; }


        public Nullable<int> tbl_type_id { get; set; }
        public Nullable<int> t_seller_typeid { get; set; }
    }
}