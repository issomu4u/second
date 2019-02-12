using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace SellerVendor.Areas.Seller.Models
{
    public class Seller_Market : DbContext
    {
        public Seller_Market()
            : base("SellerConnection")
        {
        }

        public DbSet<tbl_sellermarketplace> tbl_sellermarketplace { get; set; }
        public DbSet<tbl_seller_api_plan> tbl_seller_api_plan { get; set; }
    }
}