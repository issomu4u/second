using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SellerVendor.Areas.Seller.Models
{
    public class tbl_imp_bank_transfers
    {
        public int Id { get; set; }
        public int? tbl_settlement_upload_id { get; set; }
        public decimal? amount { get; set; }
        public short? verifystatus { get; set; }
        public string remarks { get; set; }
        public DateTime? verified_on { get; set; }
    }
}