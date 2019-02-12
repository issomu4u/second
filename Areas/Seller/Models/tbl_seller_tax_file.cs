using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace SellerVendor.Areas.Seller.Models
{
    public class tbl_seller_tax_file
    {
        public string order_id { get; set; }
        public DateTime order_date { get; set; }
        public string shipment_id { get; set; }
        public DateTime shipment_date { get; set; }
        public DateTime tax_calculated_date { get; set; }
        public DateTime posted_date { get; set; }
        public string marketplace { get; set; }
        //public string merchant_id { get; set; }
        //public string buyer_id { get; set; }
        public string tax_invoice_number { get; set; }
        //public string transaction_item_id { get; set; }
        public string fulfillment { get; set; }
        public string asin { get; set; }
        public string sku { get; set; }
        public string transaction_type { get; set; }
        public string product_tax_code { get; set; }
        public int quantity { get; set; }
        public string currency { get; set; }
        public double display_price { get; set; }
        public string is_display_price_taxinclusive { get; set; }
        public double final_taxinclusive_selling_price { get; set; }
        public double taxexclusive_selling_price { get; set; }
        public double total_tax { get; set; }
        public string ship_from_city { get; set; }
        public string ship_from_state { get; set; }
        //public string ship_from_country { get; set; }
        public string ship_from_postal_code { get; set; }
        //public string ship_from_tax_location_code { get; set; }
        public string ship_to_city { get; set; }
        public string ship_to_state { get; set; }
        //public string ship_to_country { get; set; }
        public string ship_to_postal_code { get; set; }
        //public string ship_to_location_code { get; set; }
        //public string taxed_location_code { get; set; }
        public string tax_address_role { get; set; }
        public string jurisdiction_level { get; set; }
        public string jurisdiction_name { get; set; }
        public double tax_amount { get; set; }
        public double taxed_jurisdiction_tax_rate { get; set; }
        public string tax_type { get; set; }
        public string tax_calculation_reason_code { get; set; }
        public double nontaxable_amount { get; set; }
        public double taxable_amount { get; set; }
        public double promo_taxinclusive_amount { get; set; }
        public string is_display_promo_tax_inclusive { get; set; }
        public string promo_type { get; set; }
        public double promo_taxexclusive_amount { get; set; }
        public double promo_amount_tax { get; set; }
        //public string promo_rule_reason_code { get; set; }
        //public string buyer_exemption_code { get; set; }
        //public string buyer_registration_id { get; set; }
        //public string buyer_registration_jurisdiction_country { get; set; }
        //public string buyer_registration_jurisdiction_state { get; set; }
        //public string buyer_registration_type { get; set; }
        //public string is_buyer_physically_present_in_registration_country { get; set; }
        //public string seller_registration_id { get; set; }
        //public string seller_registration_jurisdiction_country { get; set; }
        //public string seller_registration_jurisdiction_state { get; set; }
        //public string seller_registration_type { get; set; }
        //public string is_seller_physically_present_in_registration_country { get; set; }
        //public string seller_registration_id_2 { get; set; }
        //public string seller_registration_jurisdiction_country_2 { get; set; }
        //public string seller_registration_jurisdiction_state_2 { get; set; }
        //public string seller_registration_type_2 { get; set; }
        //public string is_seller_physically_present_in_registration_country_2 { get; set; }


        public string tax_address_role_blank_row { get; set; }
        public string jurisdiction_level_blank_row { get; set; }
        public string jurisdiction_name_blank_row { get; set; }
        public double tax_amount_blank_row { get; set; }
        public double taxed_jurisdiction_tax_rate_blank_row { get; set; }
        public string tax_type_blank_row { get; set; }
        public string tax_calculation_reason_code_blank_row { get; set; }
        public double nontaxable_amount_blank_row { get; set; }
        public double taxable_amount_blank_row { get; set; }
        public double promo_taxinclusive_amount_blank_row { get; set; }
        public string is_display_promo_tax_inclusive_blank_row { get; set; }
        public string promo_type_blank_row { get; set; }
        public double promo_taxexclusive_amount_blank_row { get; set; }
        public double promo_amount_tax_blank_row { get; set; }
    }
}