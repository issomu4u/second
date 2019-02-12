using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Xml.Serialization;

namespace SellerVendor.Areas.Seller.Models
{
    public class XMLDummy
    {
       
        public string AmazonOrderID { get; set; }
        public string MerchantID { get; set; }
        public string PurchaseDate { get; set; }
        public string LastUpdatedDate { get; set; }
        public string OrderStatus { get; set; }
        public string SalesChannel { get; set; }
        public string BusinessOrder { get; set; }

        public string FulfillmentChannel { get; set; }
        public string ShipServiceLevel { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string PostalCode { get; set; }
        public string Country { get; set; }
    }




    public class ChartData
    {
        public static List<ChartData> GetData()
        {
            var data = new List<ChartData>();

            data.Add(new ChartData("A", 46, 78));
            data.Add(new ChartData("B", 35, 72));
            data.Add(new ChartData("C", 68, 86));
            data.Add(new ChartData("D", 30, 23));
            data.Add(new ChartData("E", 27, 70));
            data.Add(new ChartData("F", 85, 60));
            data.Add(new ChartData("D", 43, 88));
            data.Add(new ChartData("H", 29, 22));

            return data;
        }

        public static List<ChartData> GetLineAreaChartData()
        {
            var data = new List<ChartData>();

            data.Add(new ChartData("A", 56, 62));
            data.Add(new ChartData("B", 30, 70));
            data.Add(new ChartData("C", 58, 68));
            data.Add(new ChartData("D", 65, 54));
            data.Add(new ChartData("E", 40, 52));
            data.Add(new ChartData("F", 36, 60));
            data.Add(new ChartData("D", 70, 48));

            return data;
        }

        public static List<ChartData> GetMultipleAxesSampleData()
        {
            var data = new List<ChartData>();

            data.Add(new ChartData("A", 1, 40));
            data.Add(new ChartData("B", 4, 60));
            data.Add(new ChartData("C", 3, 62));
            data.Add(new ChartData("D", 5, 52));
            data.Add(new ChartData("E", 2, 70));
            data.Add(new ChartData("F", 1, 75));

            return data;
        }

        public static List<ChartData> GetPieChartData()
        {
            var data = new List<ChartData>();

            data.Add(new ChartData("United States", 65));
            data.Add(new ChartData("United Kingdom", 58));
            data.Add(new ChartData("Germany", 30));
            data.Add(new ChartData("India", 60));
            data.Add(new ChartData("Russia", 65));
            data.Add(new ChartData("China", 75));

            return data;
        }

        public static List<ChartData> GetFunnelChartData()
        {
            var data = new List<ChartData>();

            data.Add(new ChartData("United States", 650));
            data.Add(new ChartData("United Kingdom", 530));
            data.Add(new ChartData("Germany", 440));
            data.Add(new ChartData("India", 280));
            data.Add(new ChartData("Russia", 150));
            data.Add(new ChartData("China", 75));

            return data;
        }

        public static List<ChartData> GetPyramidChartData()
        {
            var data = new List<ChartData>();

            data.Add(new ChartData("United States", 200));
            data.Add(new ChartData("United Kingdom", 220));
            data.Add(new ChartData("Germany", 260));
            data.Add(new ChartData("India", 280));
            data.Add(new ChartData("Russia", 450));
            data.Add(new ChartData("China", 600));

            return data;
        }

        public static List<ChartData> GetCategoryAxisSampleChartData()
        {
            var data = new List<ChartData>();

            data.Add(new ChartData("Category 1", 70));
            data.Add(new ChartData("Category 2", 40));
            data.Add(new ChartData("Category 3", 85));
            data.Add(new ChartData("Category 4", 50));
            data.Add(new ChartData("Category 5", 25));
            data.Add(new ChartData("Category 6", 40));

            return data;
        }

        public static List<ChartData> GetLogarithmicSampleChartData()
        {
            var data = new List<ChartData>();

            data.Add(new ChartData("A", 5));
            data.Add(new ChartData("B", 50));
            data.Add(new ChartData("C", 500));
            data.Add(new ChartData("D", 5000));
            data.Add(new ChartData("E", 50000));

            return data;
        }

        public ChartData(string label, double value1)
        {
            this.Label = label;
            this.Value1 = value1;
        }

        public ChartData(string label, double value1, double value2)
        {
            this.Label = label;
            this.Value1 = value1;
            this.Value2 = value2;
        }

        public string Label { get; set; }
        public double Value1 { get; set; }
        public double Value2 { get; set; }
    }

}