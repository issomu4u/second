using System.ComponentModel.DataAnnotations;
using System.ComponentModel.DataAnnotations.Schema;

namespace SellerVendor.Areas.Seller.Models
{
    [Table("ImportFileMapping")]
    public class ImportFileMappingEntity
    {
        [Key]
        public int Id { get; set; }
        public byte FileType { get; set; }
        public byte MarketPlaceId { get; set; }
        public string FileColumnName { get; set; }
        public string TableColumnName { get; set; }
    }
}