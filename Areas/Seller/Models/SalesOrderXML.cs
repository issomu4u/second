using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml.Serialization;

namespace SellerVendor.Areas.Seller.Models
{
    public class SalesOrderXML
    {
        [Serializable, XmlRoot("Message"), XmlType("Message")]
        public class Message
        {
            public Order Oorder { get; set; }
         }

        [XmlType("Order")]
        public class Order
        {
            [XmlElement("AmazonOrderID")]
            public string AmazonOrderID { get; set; }
            [XmlElement("MerchantOrderID")]
            public string MerchantOrderID { get; set; }
            [XmlElement("PurchaseDate")]
            public string PurchaseDate { get; set; }
            [XmlElement("LastUpdatedDate")]
            public string LastUpdatedDate { get; set; }
            [XmlElement("OrderStatus")]
            public string OrderStatus { get; set; }
            [XmlElement("SalesChannel")]
            public string SalesChannel { get; set; }
            [XmlElement("IsBusinessOrder")]
            public string IsBusinessOrder { get; set; }

            List<OrderItem> listorderitem { get; set; }
        }


        [XmlType("OrderItem")]
        public partial class OrderItem
        {         
            [XmlElement("ASIN")]
            public string ASIN { get; set; }
            [XmlElement("SKU")]
            public string SKU { get; set; }
            [XmlElement("ItemStatus")]
            public string ItemStatus { get; set; }
            [XmlElement("ProductName")]
            public string ProductName { get; set; }
            [XmlElement("Quantity")]
            public int Quantity { get; set; }

            List<ItemPrice> listitemprice { get; set; }

        }

        [XmlType("ItemPrice")]
        public class ItemPrice
        {
            List<Component> lstcomponent = new List<Component>();
            List<Promotion> lstpromotion = new List<Promotion>();
        }

        [XmlType("Component")]
        public class Component
        {
            [XmlElement("Type")]
            public string Type { get; set; }
            [XmlElement("Amount")]
            public decimal Amount { get; set; }           
        }

        [XmlType("Promotion")]
        public class Promotion
        {
            [XmlElement("PromotionIDs")]
            public string PromotionIDs { get; set; }
            [XmlElement("ShipPromotionDiscount")]
            public decimal ShipPromotionDiscount { get; set; }
        }


        [XmlType("FulfillmentData")]
        public class FulfillmentData
        {
            [XmlElement("FulfillmentChannel")]
            public string FulfillmentChannel { get; set; }
            [XmlElement("ShipServiceLevel")]
            public decimal ShipServiceLevel { get; set; }
        }

        [XmlType("Address")]
        public class Address
        {
            [XmlElement("City")]
            public string City { get; set; }
            [XmlElement("State")]
            public string State { get; set; }
            [XmlElement("PostalCode")]
            public string PostalCode { get; set; }
            [XmlElement("Country")]
            public string Country { get; set; }
        }


    }
}