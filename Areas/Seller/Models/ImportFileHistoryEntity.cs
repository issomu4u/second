using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;
using System;

namespace SellerVendor.Areas.Seller.Models
{
    [Table("ImportFileHistory")]
    public class ImportFileHistoryEntity
    {
        [Key]
        public int Id { get; set; }
        public int SellerId { get; set; }
        public byte MarketPlaceId { get; set; }
        public string FileName { get; set; }
        public string FilePath { get; set; }
        public string Message { get; set; }
        public byte FileType { get; set; }
        public byte FileStatus { get; set; }
        public DateTime CreatedOnUtc { get; set; }
        public DateTime? LastUpdatedOnUtc { get; set; }
    }
}