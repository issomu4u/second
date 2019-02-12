using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using SellerVendor.Models;

namespace SellerVendor.Controllers
{
    public class EntitiesWrapper
    {
        public tbl_sellers  ob_tbl_sellers{ get; set; }
        public tbl_country ob_tbl_country { get; set; }
        public m_source_of_joining ob_m_source_of_joining { get; set; }
        public m_item_category ob_m_item_category { get; set; }
        public m_item_subcategory ob_m_item_subcategory { get; set; }
       
    }

    public class categoryTreeView
    {
        public List<parentCategory> list_parent_category { get; set; }
    }

    public class parentCategory
    {
        public int id { get; set; }
        public string category_name { get; set; }
        public double tax_rate { get; set; }
        public DateTime updated_on { get; set; }
        public List<parentSubCategory> list_sub_category { get; set; }
    }
    public class parentSubCategory
    {
        public int id { get; set; }
        public int m_item_category_id { get; set; }
        public string subcategory_name { get; set; }
        public string hsn_code { get; set; }
        public double tax_rate { get; set; }
    }
    public class LoginData
    {
        public string Email { get; set; }
        public string Password { get; set; }
    }
}