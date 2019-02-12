using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SellerVendor.Areas.Seller.Models
{
    public class m_report_color
    {
        public int ID { get; set; }
        public string HeaderRow_ForColor { get; set; }
        public string HeaderRow_BackColor { get; set; }
        public string RowStyle_BackColor { get; set; }
        public short? m_Status_ID { get; set; }
        public DateTime? Date_of_Creation { get; set; }
    }
}