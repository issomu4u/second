using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SellerVendor.Areas.Seller.Models
{
    [Table("PayTmSettlementImport")]
    public class PayTmSettlementImportEntity
    {
        [Key]
        public int Id { get; set; }
        public int ImportFileHistoryId { get; set; }
        public string OrderId { get; set; }
        public string OrderItemId { get; set; }
        public string OrderCreationDate { get; set; }
        public string ReturnDate { get; set; }
        public string ProductId { get; set; }
        public string ProductName { get; set; }
        public string MerchantSKU { get; set; }
        public string OrderItemStatus { get; set; }
        public string SettlementDate { get; set; }
        public string PaymentType { get; set; }
        public string PaymentStatus { get; set; }
        public string AdjustmentReason { get; set; }
        public string TotalPrice { get; set; }
        public string MarketplaceCommission { get; set; }
        public string LogisticsCharges { get; set; }
        public string PGCommission { get; set; }
        public string Penalty { get; set; }
        public string AdjustmentAmount { get; set; }
        public string AdjustmentTaxes { get; set; }
        public string NetAdjustments { get; set; }
        public string ServiceTax { get; set; }
        public string PayableAmount { get; set; }
        public string PayoutWallet { get; set; }
        public string PayoutPG { get; set; }
        public string PayoutCOD { get; set; }
        public string WalletUTR { get; set; }
        public string CODUTR { get; set; }
        public string OperatorReferenceNumber { get; set; }
        public string MPCommissionCGST { get; set; }
        public string MPCommissionIGST { get; set; }
        public string MPCommissionSGST { get; set; }
        public string PGCommissionCGST { get; set; }
        public string PGCommissionIGST { get; set; }
        public string PGCommissionSGST { get; set; }
        public string LogisticsCGST { get; set; }
        public string LogisticsIGST { get; set; }
        public string LogisticsSGST { get; set; }
        public string CustomerCompanyName { get; set; }
        public string CustomerBillingAddress { get; set; }
        public string CustomerGSTIN { get; set; }
        public string IGST { get; set; }
        public string CGST { get; set; }
        public string SGST { get; set; }
        public string GSTSourceState { get; set; }
        public string GSTDestinationState { get; set; }
        public string GSTSourcePincode { get; set; }
        public string GSTDestinationPincode { get; set; }
        public string InvoiceGenerationDate { get; set; }
        public string InvoiceNumber { get; set; }
    }
}