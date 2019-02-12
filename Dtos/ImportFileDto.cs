using SellerVendor.Enums;
using System.Collections.Generic;

namespace SellerVendor.Dtos
{
    public class ImportFileDto
    {
        public int FileHistoryId { get; set; }
        public MarketPlace MarketPlace { get; set; }
        public string FileName { get; set; }
        public string Location { get; set; }
        public FileType FileType { get; set; }
        public bool IsUploaded { get; set; }
        public bool IsDumpedIntoDb { get; set; }
        public bool IsProcessed { get; set; }
        public List<string> Messages { get; set; }
        public string FullPath { get { return Location + FileName; } }

        public ImportFileDto()
        { Messages = new List<string>(); }
    }
}