using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SellerVendor.Areas.Seller.Models
{
    public class tbl_order_upload
    {
        public int id { get; set; }
        public string filename { get; set; }
        public short? type { get; set; }
        public int? voucher_running_no { get; set; }
        public DateTime? date_uploaded { get; set; }
        public int? tbl_seller_id { get; set; }
        public int? new_order_uploaded { get; set; }
        public int? tbl_Marketplace_id { get; set; }
        public DateTime? from_date { get; set; }
        public DateTime? to_date { get; set; }
        public short? source { get; set; }
        public string status { get; set; }
        public short? checkstatus { get; set; }
        public DateTime? processing_datetime { get; set; }
        public DateTime? completed_datetime { get; set; }

    }


    public partial class vieworderUpload
    {
        public int id { get; set; }
        public string MarketplaceName { get; set; }
        public string FileName { get; set; }
        public string OrderCount { get; set; }
        public string ImagePath { get; set; }
        public string FromDate { get; set; }
        public string ToDate { get; set; }
        public string UploadedDate { get; set; }
        public string DepositDate { get; set; }
        public string SourceName { get; set; }
        public string Filestatus { get; set; }
        public decimal suspenseamt { get; set; }
        public string ReferenceNo { get; set; }
        public int Sellerid { get; set; }
        public int BalanceTaxCount { get; set; }

       
    }
}