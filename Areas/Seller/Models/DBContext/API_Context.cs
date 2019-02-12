using System;
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Web;

namespace SellerVendor.Areas.Seller.Models.DBContext
{
    public class API_Context : DbContext
    {
        public API_Context()
            : base("APIConnection")
        {
        }

        public DbSet<tbl_api_meter> tbl_api_meter { get; set; }
    }
}