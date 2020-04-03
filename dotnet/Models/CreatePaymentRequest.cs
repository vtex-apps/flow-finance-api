namespace FlowFinance.Models
{
    using System.Collections.Generic;

    public class CreatePaymentRequest
    {
        /// <summary>
        /// Merchant's order reference
        /// </summary>
        public string reference { get; set; }

        /// <summary>
        /// VTEX transaction ID related to this payment
        /// </summary>
        public string transactionId { get; set; }

        /// <summary>
        /// VTEX payment ID that can be use as an unique identifier
        /// </summary>
        public string paymentId { get; set; }

        /// <summary>
        /// One of the available payment methods
        /// </summary>
        public string paymentMethod { get; set; }

        /// <summary>
        /// VTEX merchant name that will receive the payment
        /// </summary>
        public string merchantName { get; set; }

        /// <summary>
        /// Value amount of the payment
        /// </summary>
        public decimal value { get; set; }

        /// <summary>
        /// ISO 4217 "Alpha-3" currency code
        /// </summary>
        public string currency { get; set; }

        /// <summary>
        /// Number of installments
        /// </summary>
        public int installments { get; set; }

        /// <summary>
        /// A hash that represents the device used to initiate the payment
        /// </summary>
        public string deviceFingerprint { get; set; }

        /// <summary>
        /// Card
        /// </summary>
        public Card card { get; set; }

        /// <summary>
        /// Cart
        /// </summary>
        public MiniCart miniCart { get; set; }

        /// <summary>
        /// The order URL from merchant's backoffice
        /// </summary>
        public string url { get; set; }

        /// <summary>
        /// The URL you need to call to send the callbacks/notifications of payment status changes
        /// </summary>
        public string callbackUrl { get; set; }

        /// <summary>
        /// The URL you need to redirect the end user back to merchant's store when using the redirect flow
        /// </summary>
        public string returnUrl { get; set; }


        public string inboundRequestsUrl { get; set; }

        public string orderId { get; set; }
    }

    public class Expiration
    {
        /// <summary>
        /// Card expiration month (2-digits)
        /// </summary>
        public string month { get; set; }

        /// <summary>
        /// Card expiration year (4-digits)
        /// </summary>
        public string year { get; set; }
    }

    public class Card
    {
        /// <summary>
        /// Card holder name
        /// </summary>
        public string holder { get; set; }

        /// <summary>
        /// Card number
        /// </summary>
        public string number { get; set; }

        /// <summary>
        /// Card security code
        /// </summary>
        public string csc { get; set; }

        /// <summary>
        /// Card expiration
        /// </summary>
        public Expiration expiration { get; set; }
    }

    public class Buyer
    {
        /// <summary>
        /// Buyer's unique identifier
        /// </summary>
        public string id { get; set; }

        /// <summary>
        /// Buyer's first name
        /// </summary>
        public string firstName { get; set; }

        /// <summary>
        /// Buyer's last name
        /// </summary>
        public string lastName { get; set; }

        /// <summary>
        /// Buyer's document number
        /// </summary>
        public string document { get; set; }

        /// <summary>
        /// Buyer's document type
        /// </summary>
        public string documentType { get; set; }

        /// <summary>
        /// Buyer's email
        /// </summary>
        public string email { get; set; }

        /// <summary>
        /// Buyer's phone number
        /// </summary>
        public string phone { get; set; }
    }

    public class ShippingAddress
    {
        /// <summary>
        /// Shipping address: country
        /// </summary>
        public string country { get; set; }

        /// <summary>
        /// Shipping address: street
        /// </summary>
        public string street { get; set; }

        /// <summary>
        /// Shipping address: number
        /// </summary>
        public string number { get; set; }

        /// <summary>
        /// Shipping address: complement
        /// </summary>
        public string complement { get; set; }

        /// <summary>
        /// Shipping address: neighborhood
        /// </summary>
        public string neighborhood { get; set; }

        /// <summary>
        /// Shipping address: postal code
        /// </summary>
        public string postalCode { get; set; }

        /// <summary>
        /// Shipping address: city
        /// </summary>
        public string city { get; set; }

        /// <summary>
        /// Shipping address: state/province
        /// </summary>
        public string state { get; set; }
    }

    public class BillingAddress
    {
        /// <summary>
        /// Billing address: country
        /// </summary>
        public string country { get; set; }

        /// <summary>
        /// Billing address: street
        /// </summary>
        public string street { get; set; }

        /// <summary>
        /// Billing address: number
        /// </summary>
        public string number { get; set; }

        /// <summary>
        /// Billing address: complement
        /// </summary>
        public string complement { get; set; }

        /// <summary>
        /// Billing address: neighborhood
        /// </summary>
        public string neighborhood { get; set; }

        /// <summary>
        /// Billing address: postal code
        /// </summary>
        public string postalCode { get; set; }

        /// <summary>
        /// Billing address: city
        /// </summary>
        public string city { get; set; }

        /// <summary>
        /// Billing address: state/province
        /// </summary>
        public string state { get; set; }
    }

    public class Item
    {
        /// <summary>
        /// Item identifier
        /// </summary>
        public string id { get; set; }

        /// <summary>
        /// Item name
        /// </summary>
        public string name { get; set; }

        /// <summary>
        /// Item price per unit
        /// </summary>
        public decimal price { get; set; }

        /// <summary>
        /// 	Item quantity
        /// </summary>
        public int quantity { get; set; }

        /// <summary>
        /// Discount received for the item
        /// </summary>
        public decimal discount { get; set; }
    }

    public class MiniCart
    {
        /// <summary>
        /// Total shipping value
        /// </summary>
        public decimal shippingValue { get; set; }

        /// <summary>
        /// Total tax value
        /// </summary>
        public decimal taxValue { get; set; }

        /// <summary>
        /// Buyer infromation
        /// </summary>
        public Buyer buyer { get; set; }

        /// <summary>
        /// Shipping address
        /// </summary>
        public ShippingAddress shippingAddress { get; set; }

        /// <summary>
        /// Billing address
        /// </summary>
        public BillingAddress billingAddress { get; set; }

        /// <summary>
        /// Items
        /// </summary>
        public List<Item> items { get; set; }
    }
}
