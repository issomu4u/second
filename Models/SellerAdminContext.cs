using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace SellerVendor.Models
{
    public class SellerAdminContext : DbContext
    {
        public SellerAdminContext()
            : base("SellerAdminConnection")
        {
        }
       
        public DbSet<tbl_sellers> tbl_sellers { get; set; }
        public DbSet<tbl_country> tbl_country { get; set; }
        public DbSet<m_source_of_joining> m_source_of_joining { get; set; }
        public DbSet<m_database> m_database { get; set; }
        public DbSet<tbl_exception_history> tbl_exception_history { get; set; }
        public DbSet<m_item_category> m_item_category { get; set; }
        public DbSet<m_item_subcategory> m_item_subcategory { get; set; }
        public DbSet<m_marketplace> m_marketplace { get; set; }
        public DbSet<tbl_user_login> tbl_user_login { get; set; }
        public DbSet<temp_item_category> temp_item_category { get; set; }
        public DbSet<temp_item_subcategory> temp_item_subcategory { get; set; }
        public DbSet<tbl_m_plan> tbl_m_plan { get; set; }
        public DbSet<tbl_seller_type> tbl_seller_type { get; set; }
        public DbSet<tbl_user_payment> tbl_user_payment { get; set; }
       
    }
}