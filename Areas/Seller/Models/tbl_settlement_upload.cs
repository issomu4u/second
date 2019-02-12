using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SellerVendor.Areas.Seller.Models
{
    public class RetMessage
    {
        public int status { get; set; }
        public string message { get; set; }
    }
    public class tbl_settlement_upload
    {
        public int Id { get; set; }
        public int? tbl_seller_id { get; set; }
        public int? market_place_id { get; set; }
        public string settlement_refernece_no { get; set; }
        public DateTime? uploaded_on { get; set; }
        public short? settlement_type { get; set; }
        public DateTime? settlement_from { get; set; }
        public DateTime? settlement_to { get; set; }
        public int? uploaded_by { get; set; }
        public int? settlement_id { get; set; }
        public DateTime? deposit_date { get; set; }
        public decimal? previous_reserve_amount { get; set; }
        public decimal? current_reserve_amount { get; set; }
        public decimal? Nonsubscription_feeadj { get; set; }
        public decimal? INCORRECT_FEES_ITEMS { get; set; }
        public decimal? Cost_of_Advertising { get; set; }
        public decimal? FBAInboundTransportationFee { get; set; }       
        public decimal? Storage_Fee { get; set; }
        public decimal? BalanceAdjustment { get; set; }
        public decimal? Payable_to_Amazon { get; set; }
        public decimal? suspense_amt { get; set; }
        public int? status { get; set; }
        public int? voucher_running_no { get; set; }
        public string file_name { get; set; }
        public int? new_order_uploaded { get; set; }
        public short? Source { get; set; }
        public string file_status { get; set; }

        public DateTime? processing_datetime { get; set; }
        public DateTime? completed_datetime { get; set; }
        public DateTime? request_fromdate { get; set; }
        public DateTime? request_date_to { get; set; }

        public DateTime? LastUpdatedDateUTC { get; set; }
    }
}