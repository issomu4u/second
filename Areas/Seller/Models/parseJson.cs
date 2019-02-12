using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SellerVendor.Areas.Seller.Models
{
    public class parseJson
    {
        public string hasMore { get; set; }
        public List<orderItems> orderItems { get; set; }
       
        
    }
    public class orderItems
    {
        public string orderItemId { get; set; }
        public string orderId { get; set; }
        public string hsn { get; set; }
        public string status { get; set; }
        public string hold { get; set; }
        public string orderDate { get; set; }
        public string dispatchAfterDate { get; set; }
        public string dispatchByDate { get; set; }
        public string updatedAt { get; set; }
        public string sla { get; set; }
        public int quantity { get; set; }
        public string title { get; set; }
        public string listingId { get; set; }
        public string fsn { get; set; }
        public string sku { get; set; }
        public decimal price { get; set; }
        public decimal shippingFee { get; set; }
        public string shippingPincode { get; set; }
        public string paymentType { get; set; }
        public  priceComponents priceComponents{ get; set; }
        public decimal sellingPrice { get; set; }
        public decimal customerPrice { get; set; }
        public decimal shippingCharge { get; set; }
        public decimal totalPrice { get; set; }
        public List<shipments> shipmentDetails { get; set; }

      //  public List<priceComponents> priceComponents { get; set; }
       
    }
    public class priceComponents
    {
        public decimal sellingPrice { get; set; }
        public decimal customerPrice { get; set; }
        public decimal shippingCharge { get; set; }
        public decimal totalPrice { get; set; }
    }

    


    /// <summary>
    /// it is used for shipment details 
    /// </summary>
    public class shipmentDetails
    {
        public List<shipments> shipments { get; set; }
    }

    public class shipments
    {
        public string orderId { get; set; }
        public string shipmentId { get; set; }
        public string returnAddress { get; set; }
        public string weighingRequired { get; set; }

        public deliveryAddress deliveryAddress { get; set; }
        public billingAddress billingAddress { get; set; }
        public buyerDetails buyerDetails { get; set; }
        public sellerAddress sellerAddress { get; set; }
        public List<SellerVendor.Areas.Seller.Common.orderItems> orderItems { get; set; }
    }

    

   
    public class deliveryAddress
    {
        public string firstName { get; set; }
        public string lastName { get; set; }
        public string addressLine1 { get; set; }
        public string addressLine2 { get; set; }
        public string city { get; set; }
        public string state { get; set; }
        public string pincode { get; set; }
        public string stateCode { get; set; }
        public string stateName { get; set; }
        public string landmark { get; set; }
        public string contactNumber { get; set; }
    }
    public class billingAddress
    {
        public string firstName { get; set; }
        public string lastName { get; set; }
        public string addressLine1 { get; set; }
        public string addressLine2 { get; set; }
        public string city { get; set; }
        public string state { get; set; }
        public string pincode { get; set; }
        public string stateCode { get; set; }
        public string stateName { get; set; }
        public string landmark { get; set; }
        public string contactNumber { get; set; }
    }
    public class buyerDetails
    {
        public string firstName { get; set; }
        public string lastName { get; set; }
        public string contactNumber { get; set; }
        public string primaryEmail { get; set; }
    }
    public class sellerAddress
    {
        public string firstName { get; set; }
        public string lastName { get; set; }
        public string addressLine1 { get; set; }
        public string addressLine2 { get; set; }
        public string city { get; set; }
        public string state { get; set; }
        public string pincode { get; set; }
        public string stateCode { get; set; }
        public string stateName { get; set; }
        public string landmark { get; set; }
        public string contactNumber { get; set; }
    }


    public class SalesOrderRequest
    {

        public filter filter { get; set; }
    }
    public class filter
    {
        public orderDate orderDate { get; set; }
    }
    public class orderDate
    {
        public string fromDate { get; set; }
        public string toDate { get; set; }
    }

#region for Flipkartorder csv file

    public class Flipkartcsv
    {
        public string OrderedOn { get; set; }
        public string ShipmentID { get; set; }
        public string ORDERITEMID { get; set; }
        public string OrderId { get; set; }
        public string HSNCODE { get; set; }
        public string OrderState { get; set; }
        public string OrderType { get; set; }
        public string FSN { get; set; }
        public string SKU { get; set; }
        public string Product { get; set; }
        public string InvoiceNo { get; set; }
        public string CGST { get; set; }
        public string IGST { get; set; }
        public string SGST { get; set; }
        public string InvoiceDate { get; set; }
        public string InvoiceAmount { get; set; }
        public string SellingPricePerItem { get; set; }
        public string ShippingChargeperitem { get; set; }
        public string Quantity { get; set; }
        public string Price_inc { get; set; }
        public string Buyername { get; set; }
        public string Shiptoname { get; set; }
        public string AddressLine1 { get; set; }
        public string AddressLine2 { get; set; }
        public string City { get; set; }
        public string State { get; set; }
        public string PINCode { get; set; }
        public string ProcSLA { get; set; }
        public string DispatchAfterdate { get; set; }
        public string Dispatchbydate { get; set; }
        public string TrackingID { get; set; }
        public string PackageLength { get; set; }
        public string PackageBreadth  { get; set; }
        public string PackageHeight { get; set; }
        public string PackageWeight { get; set; }
        public double item_price { get; set; }
        public double itemtax_price { get; set; }
        public double shipping_price { get; set; }
        public double shipping_taxprice { get; set; }
        public double Igsttaxprice { get; set; }
        public double cgsttaxprice { get; set; }
        public double sgsttaxprice { get; set; }
        
    }
#endregion

}