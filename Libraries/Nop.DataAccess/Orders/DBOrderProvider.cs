//------------------------------------------------------------------------------
// The contents of this file are subject to the nopCommerce Public License Version 1.0 ("License"); you may not use this file except in compliance with the License.
// You may obtain a copy of the License at  http://www.nopCommerce.com/License.aspx. 
// 
// Software distributed under the License is distributed on an "AS IS" basis, WITHOUT WARRANTY OF ANY KIND, either express or implied. 
// See the License for the specific language governing rights and limitations under the License.
// 
// The Original Code is nopCommerce.
// The Initial Developer of the Original Code is NopSolutions.
// All Rights Reserved.
// 
// Contributor(s): _______. 
//------------------------------------------------------------------------------

using System;
using System.Collections.Generic;
using System.Collections.Specialized;
using System.Configuration;
using System.Configuration.Provider;
using System.Data;
using System.Data.Common;
using System.Globalization;
using System.Text;
using System.Web.Hosting;
using System.Web.Configuration;

namespace NopSolutions.NopCommerce.DataAccess.Orders
{
    /// <summary>
    /// Acts as a base class for deriving custom order provider
    /// </summary>
    [DBProviderSectionName("nopDataProviders/OrderProvider")]
    public abstract partial class DBOrderProvider : BaseDBProvider
    {
        #region Methods
        /// <summary>
        /// Gets an order
        /// </summary>
        /// <param name="OrderID">The order identifier</param>
        /// <returns>Order</returns>
        public abstract DBOrder GetOrderByID(int OrderID);

        /// <summary>
        /// Gets an order
        /// </summary>
        /// <param name="OrderGUID">The order identifier</param>
        /// <returns>Order</returns>
        public abstract DBOrder GetOrderByGUID(Guid OrderGUID);

        /// <summary>
        /// Search orders
        /// </summary>
        /// <param name="StartTime">Order start time; null to load all orders</param>
        /// <param name="EndTime">Order end time; null to load all orders</param>
        /// <param name="CustomerEmail">Customer email</param>
        /// <param name="OrderStatusID">Order status identifier; null to load all orders</param>
        /// <param name="PaymentStatusID">Order payment status identifier; null to load all orders</param>
        /// <param name="ShippingStatusID">Order shipping status identifier; null to load all orders</param>
        /// <returns>Order collection</returns>
        public abstract DBOrderCollection SearchOrders(DateTime? StartTime, DateTime? EndTime, string CustomerEmail, int? OrderStatusID, int? PaymentStatusID, int? ShippingStatusID);

        /// <summary>
        /// Get order product variant sales report
        /// </summary>
        /// <param name="StartTime">Order start time; null to load all</param>
        /// <param name="EndTime">Order end time; null to load all</param>
        /// <param name="OrderStatusID">Order status identifier; null to load all orders</param>
        /// <param name="PaymentStatusID">Order payment status identifier; null to load all orders</param>
        /// <returns>Result</returns>
        public abstract IDataReader OrderProductVariantReport(DateTime? StartTime, DateTime? EndTime, int? OrderStatusID, int? PaymentStatusID);

        /// <summary>
        /// Get the bests sellers report
        /// </summary>
        /// <param name="LastDays">Last number of days</param>
        /// <param name="RecordsToReturn">Number of products to return</param>
        /// <param name="OrderBy">1 - order by total count, 2 - Order by total amount</param>
        /// <returns>Result</returns>
        public abstract List<DBBestSellersReportLine> BestSellersReport(int LastDays, int RecordsToReturn, int OrderBy);

        /// <summary>
        /// Get order average report
        /// </summary>
        /// <param name="OrderStatusID">Order status identifier</param>
        /// <param name="startTime">Start date</param>
        /// <param name="endTime">End date</param>
        /// <returns>Result</returns>
        public abstract DBOrderAverageReportLine OrderAverageReport(int OrderStatusID, DateTime? startTime, DateTime? endTime);

        /// <summary>
        /// Gets all orders by customer identifier
        /// </summary>
        /// <param name="CustomerID">Customer identifier</param>
        /// <returns>Order collection</returns>
        public abstract DBOrderCollection GetOrdersByCustomerID(int CustomerID);

        /// <summary>
        /// Gets an order by authorization transaction identifier
        /// </summary>
        /// <param name="AuthorizationTransactionID">Authorization transaction identifier</param>
        /// <param name="PaymentMethodID">Payment method identifier</param>
        /// <returns>Order</returns>
        public abstract DBOrder GetOrderByAuthorizationTransactionIDAndPaymentMethodID(string AuthorizationTransactionID, int PaymentMethodID);

        /// <summary>
        /// Gets all orders by affiliate identifier
        /// </summary>
        /// <param name="AffiliateID">Affiliate identifier</param>
        /// <returns>Order collection</returns>
        public abstract DBOrderCollection GetOrdersByAffiliateID(int AffiliateID);

        /// <summary>
        /// Inserts an order
        /// </summary>
        /// <param name="OrderGUID">The order identifier</param>
        /// <param name="CustomerID">The customer identifier</param>
        /// <param name="CustomerLanguageID">The customer language identifier</param>
        /// <param name="CustomerTaxDisplayTypeID">The customer tax display type identifier</param>
        /// <param name="CustomerIP">The customer IP address</param>
        /// <param name="OrderSubtotalInclTax">The order subtotal (incl tax)</param>
        /// <param name="OrderSubtotalExclTax">The order subtotal (excl tax)</param>
        /// <param name="OrderShippingInclTax">The order shipping (incl tax)</param>
        /// <param name="OrderShippingExclTax">The order shipping (excl tax)</param>
        /// <param name="PaymentMethodAdditionalFeeInclTax">The payment method additional fee (incl tax)</param>
        /// <param name="PaymentMethodAdditionalFeeExclTax">The payment method additional fee (excl tax)</param>
        /// <param name="OrderTax">The order tax</param>
        /// <param name="OrderTotal">The order total</param>
        /// <param name="OrderDiscount">The order discount</param>
        /// <param name="OrderSubtotalInclTaxInCustomerCurrency">The order subtotal incl tax (customer currency)</param>
        /// <param name="OrderSubtotalExclTaxInCustomerCurrency">The order subtotal excl tax (customer currency)</param>
        /// <param name="OrderShippingInclTaxInCustomerCurrency">The order shipping incl tax (customer currency)</param>
        /// <param name="OrderShippingExclTaxInCustomerCurrency">The order shipping excl tax (customer currency)</param>
        /// <param name="PaymentMethodAdditionalFeeInclTaxInCustomerCurrency">The payment method additional fee incl tax (customer currency)</param>
        /// <param name="PaymentMethodAdditionalFeeExclTaxInCustomerCurrency">The payment method additional fee excl tax (customer currency)</param>
        /// <param name="OrderTaxInCustomerCurrency">The order tax (customer currency)</param>
        /// <param name="OrderTotalInCustomerCurrency">The order total (customer currency)</param>
        /// <param name="OrderDiscountInCustomerCurrency">The order discount (customer currency)</param>
        /// <param name="CheckoutAttributeDescription">The checkout attribute description</param>
        /// <param name="CheckoutAttributesXML">The checkout attributes in XML format</param>
        /// <param name="CustomerCurrencyCode">The customer currency code</param>
        /// <param name="OrderWeight">The order weight</param>
        /// <param name="AffiliateID">The affiliate identifier</param>
        /// <param name="OrderStatusID">The order status identifier</param>
        /// <param name="AllowStoringCreditCardNumber">The value indicating whether storing of credit card number is allowed</param>
        /// <param name="CardType">The card type</param>
        /// <param name="CardName">The card name</param>
        /// <param name="CardNumber">The card number</param>
        /// <param name="MaskedCreditCardNumber">The masked credit card number</param>
        /// <param name="CardCVV2">The card CVV2</param>
        /// <param name="CardExpirationMonth">The card expiration month</param>
        /// <param name="CardExpirationYear">The card expiration year</param>
        /// <param name="PaymentMethodID">The payment method identifier</param>
        /// <param name="PaymentMethodName">The payment method name</param>
        /// <param name="AuthorizationTransactionID">The authorization transaction ID</param>
        /// <param name="AuthorizationTransactionCode">The authorization transaction code</param>
        /// <param name="AuthorizationTransactionResult">The authorization transaction result</param>
        /// <param name="CaptureTransactionID">The capture transaction ID</param>
        /// <param name="CaptureTransactionResult">The capture transaction result</param>
        /// <param name="SubscriptionTransactionID">The subscription transaction ID</param>
        /// <param name="PurchaseOrderNumber">The purchase order number</param>
        /// <param name="PaymentStatusID">The payment status identifier</param>
        /// <param name="PaidDate">The paid date and time</param>
        /// <param name="BillingFirstName">The billing first name</param>
        /// <param name="BillingLastName">The billing last name</param>
        /// <param name="BillingPhoneNumber">he billing phone number</param>
        /// <param name="BillingEmail">The billing email</param>
        /// <param name="BillingFaxNumber">The billing fax number</param>
        /// <param name="BillingCompany">The billing company</param>
        /// <param name="BillingAddress1">The billing address 1</param>
        /// <param name="BillingAddress2">The billing address 2</param>
        /// <param name="BillingCity">The billing city</param>
        /// <param name="BillingStateProvince">The billing state/province</param>
        /// <param name="BillingStateProvinceID">The billing state/province identifier</param>
        /// <param name="BillingZipPostalCode">The billing zip/postal code</param>
        /// <param name="BillingCountry">The billing country</param>
        /// <param name="BillingCountryID">The billing country identifier</param>
        /// <param name="ShippingStatusID">The shipping status identifier</param>
        /// <param name="ShippingFirstName">The shipping first name</param>
        /// <param name="ShippingLastName">The shipping last name</param>
        /// <param name="ShippingPhoneNumber">The shipping phone number</param>
        /// <param name="ShippingEmail">The shipping email</param>
        /// <param name="ShippingFaxNumber">The shipping fax number</param>
        /// <param name="ShippingCompany">The shipping  company</param>
        /// <param name="ShippingAddress1">The shipping address 1</param>
        /// <param name="ShippingAddress2">The shipping address 2</param>
        /// <param name="ShippingCity">The shipping city</param>
        /// <param name="ShippingStateProvince">The shipping state/province</param>
        /// <param name="ShippingStateProvinceID">The shipping state/province identifier</param>
        /// <param name="ShippingZipPostalCode">The shipping zip/postal code</param>
        /// <param name="ShippingCountry">The shipping country</param>
        /// <param name="ShippingCountryID">The shipping country identifier</param>
        /// <param name="ShippingMethod">The shipping method</param>
        /// <param name="ShippingRateComputationMethodID">The shipping rate computation method identifier</param>
        /// <param name="ShippedDate">The shipped date and time</param>
        /// <param name="TrackingNumber">The tracking number of order</param>
        /// <param name="Deleted">A value indicating whether the entity has been deleted</param>
        /// <param name="CreatedOn">The date and time of order creation</param>
        /// <returns>Order</returns>
        public abstract DBOrder InsertOrder(Guid OrderGUID, int CustomerID, int CustomerLanguageID,
            int CustomerTaxDisplayTypeID, string CustomerIP, decimal OrderSubtotalInclTax, decimal OrderSubtotalExclTax,
            decimal OrderShippingInclTax, decimal OrderShippingExclTax, 
            decimal PaymentMethodAdditionalFeeInclTax, decimal PaymentMethodAdditionalFeeExclTax,
            decimal OrderTax, decimal OrderTotal, decimal OrderDiscount,
            decimal OrderSubtotalInclTaxInCustomerCurrency, decimal OrderSubtotalExclTaxInCustomerCurrency,
            decimal OrderShippingInclTaxInCustomerCurrency, decimal OrderShippingExclTaxInCustomerCurrency,
            decimal PaymentMethodAdditionalFeeInclTaxInCustomerCurrency, decimal PaymentMethodAdditionalFeeExclTaxInCustomerCurrency,
            decimal OrderTaxInCustomerCurrency, decimal OrderTotalInCustomerCurrency,
            decimal OrderDiscountInCustomerCurrency, 
            string CheckoutAttributeDescription, string CheckoutAttributesXML,
            string CustomerCurrencyCode, decimal OrderWeight,
            int AffiliateID, int OrderStatusID, bool AllowStoringCreditCardNumber, string CardType, 
            string CardName, string CardNumber, string MaskedCreditCardNumber, string CardCVV2,
            string CardExpirationMonth, string CardExpirationYear, int PaymentMethodID,
            string PaymentMethodName, string AuthorizationTransactionID, string AuthorizationTransactionCode,
            string AuthorizationTransactionResult, string CaptureTransactionID, string CaptureTransactionResult,
            string SubscriptionTransactionID, string PurchaseOrderNumber, int PaymentStatusID, DateTime? PaidDate, 
            string BillingFirstName, string BillingLastName,
            string BillingPhoneNumber, string BillingEmail, string BillingFaxNumber, string BillingCompany,
            string BillingAddress1, string BillingAddress2, string BillingCity, string BillingStateProvince,
            int BillingStateProvinceID, string BillingZipPostalCode, string BillingCountry,
            int BillingCountryID, int ShippingStatusID, string ShippingFirstName,
            string ShippingLastName, string ShippingPhoneNumber, string ShippingEmail,
            string ShippingFaxNumber, string ShippingCompany, string ShippingAddress1,
            string ShippingAddress2, string ShippingCity, string ShippingStateProvince,
            int ShippingStateProvinceID, string ShippingZipPostalCode,
            string ShippingCountry, int ShippingCountryID, string ShippingMethod, int ShippingRateComputationMethodID, DateTime? ShippedDate,
            string TrackingNumber, bool Deleted, DateTime CreatedOn);

        /// <summary>
        /// Updates the order
        /// </summary>
        /// <param name="OrderID">The order identifier</param>
        /// <param name="OrderGUID">The order identifier</param>
        /// <param name="CustomerID">The customer identifier</param>
        /// <param name="CustomerLanguageID">The customer language identifier</param>
        /// <param name="CustomerTaxDisplayTypeID">The customer tax display type identifier</param>
        /// <param name="CustomerIP">The customer IP address</param>
        /// <param name="OrderSubtotalInclTax">The order subtotal (incl tax)</param>
        /// <param name="OrderSubtotalExclTax">The order subtotal (excl tax)</param>
        /// <param name="OrderShippingInclTax">The order shipping (incl tax)</param>
        /// <param name="OrderShippingExclTax">The order shipping (excl tax)</param>
        /// <param name="PaymentMethodAdditionalFeeInclTax">The payment method additional fee (incl tax)</param>
        /// <param name="PaymentMethodAdditionalFeeExclTax">The payment method additional fee (excl tax)</param>
        /// <param name="OrderTax">The order tax</param>
        /// <param name="OrderTotal">The order total</param>
        /// <param name="OrderDiscount">The order discount</param>
        /// <param name="OrderSubtotalInclTaxInCustomerCurrency">The order subtotal incl tax (customer currency)</param>
        /// <param name="OrderSubtotalExclTaxInCustomerCurrency">The order subtotal excl tax (customer currency)</param>
        /// <param name="OrderShippingInclTaxInCustomerCurrency">The order shipping incl tax (customer currency)</param>
        /// <param name="OrderShippingExclTaxInCustomerCurrency">The order shipping excl tax (customer currency)</param>
        /// <param name="PaymentMethodAdditionalFeeInclTaxInCustomerCurrency">The payment method additional fee incl tax (customer currency)</param>
        /// <param name="PaymentMethodAdditionalFeeExclTaxInCustomerCurrency">The payment method additional fee excl tax (customer currency)</param>
        /// <param name="OrderTaxInCustomerCurrency">The order tax (customer currency)</param>
        /// <param name="OrderTotalInCustomerCurrency">The order total (customer currency)</param>
        /// <param name="OrderDiscountInCustomerCurrency">The order discount (customer currency)</param>
        /// <param name="CheckoutAttributeDescription">The checkout attribute description</param>
        /// <param name="CheckoutAttributesXML">The checkout attributes in XML format</param>
        /// <param name="CustomerCurrencyCode">The customer currency code</param>
        /// <param name="OrderWeight">The order weight</param>
        /// <param name="AffiliateID">The affiliate identifier</param>
        /// <param name="OrderStatusID">The order status identifier</param>
        /// <param name="AllowStoringCreditCardNumber">The value indicating whether storing of credit card number is allowed</param>
        /// <param name="CardType">The card type</param>
        /// <param name="CardName">The card name</param>
        /// <param name="CardNumber">The card number</param>
        /// <param name="MaskedCreditCardNumber">The masked credit card number</param>
        /// <param name="CardCVV2">The card CVV2</param>
        /// <param name="CardExpirationMonth">The card expiration month</param>
        /// <param name="CardExpirationYear">The card expiration year</param>
        /// <param name="PaymentMethodID">The payment method identifier</param>
        /// <param name="PaymentMethodName">The payment method name</param>
        /// <param name="AuthorizationTransactionID">The authorization transaction ID</param>
        /// <param name="AuthorizationTransactionCode">The authorization transaction code</param>
        /// <param name="AuthorizationTransactionResult">The authorization transaction result</param>
        /// <param name="CaptureTransactionID">The capture transaction ID</param>
        /// <param name="CaptureTransactionResult">The capture transaction result</param>
        /// <param name="SubscriptionTransactionID">The subscription transaction ID</param>
        /// <param name="PurchaseOrderNumber">The purchase order number</param>
        /// <param name="PaymentStatusID">The payment status identifier</param>
        /// <param name="PaidDate">The paid date and time</param>
        /// <param name="BillingFirstName">The billing first name</param>
        /// <param name="BillingLastName">The billing last name</param>
        /// <param name="BillingPhoneNumber">he billing phone number</param>
        /// <param name="BillingEmail">The billing email</param>
        /// <param name="BillingFaxNumber">The billing fax number</param>
        /// <param name="BillingCompany">The billing company</param>
        /// <param name="BillingAddress1">The billing address 1</param>
        /// <param name="BillingAddress2">The billing address 2</param>
        /// <param name="BillingCity">The billing city</param>
        /// <param name="BillingStateProvince">The billing state/province</param>
        /// <param name="BillingStateProvinceID">The billing state/province identifier</param>
        /// <param name="BillingZipPostalCode">The billing zip/postal code</param>
        /// <param name="BillingCountry">The billing country</param>
        /// <param name="BillingCountryID">The billing country identifier</param>
        /// <param name="ShippingStatusID">The shipping status identifier</param>
        /// <param name="ShippingFirstName">The shipping first name</param>
        /// <param name="ShippingLastName">The shipping last name</param>
        /// <param name="ShippingPhoneNumber">The shipping phone number</param>
        /// <param name="ShippingEmail">The shipping email</param>
        /// <param name="ShippingFaxNumber">The shipping fax number</param>
        /// <param name="ShippingCompany">The shipping  company</param>
        /// <param name="ShippingAddress1">The shipping address 1</param>
        /// <param name="ShippingAddress2">The shipping address 2</param>
        /// <param name="ShippingCity">The shipping city</param>
        /// <param name="ShippingStateProvince">The shipping state/province</param>
        /// <param name="ShippingStateProvinceID">The shipping state/province identifier</param>
        /// <param name="ShippingZipPostalCode">The shipping zip/postal code</param>
        /// <param name="ShippingCountry">The shipping country</param>
        /// <param name="ShippingCountryID">The shipping country identifier</param>
        /// <param name="ShippingMethod">The shipping method</param>
        /// <param name="ShippingRateComputationMethodID">The shipping rate computation method identifier</param>
        /// <param name="ShippedDate">The shipped date and time</param>
        /// <param name="TrackingNumber">The tracking number of order</param>
        /// <param name="Deleted">A value indicating whether the entity has been deleted</param>
        /// <param name="CreatedOn">The date and time of order creation</param>
        /// <returns>Order</returns>
        public abstract DBOrder UpdateOrder(int OrderID, Guid OrderGUID, int CustomerID, int CustomerLanguageID,
            int CustomerTaxDisplayTypeID, string CustomerIP, decimal OrderSubtotalInclTax, decimal OrderSubtotalExclTax,
            decimal OrderShippingInclTax, decimal OrderShippingExclTax,
            decimal PaymentMethodAdditionalFeeInclTax, decimal PaymentMethodAdditionalFeeExclTax, 
            decimal OrderTax, decimal OrderTotal, decimal OrderDiscount,
            decimal OrderSubtotalInclTaxInCustomerCurrency, decimal OrderSubtotalExclTaxInCustomerCurrency,
            decimal OrderShippingInclTaxInCustomerCurrency, decimal OrderShippingExclTaxInCustomerCurrency,
            decimal PaymentMethodAdditionalFeeInclTaxInCustomerCurrency, decimal PaymentMethodAdditionalFeeExclTaxInCustomerCurrency,
            decimal OrderTaxInCustomerCurrency, decimal OrderTotalInCustomerCurrency,
            decimal OrderDiscountInCustomerCurrency,
            string CheckoutAttributeDescription, string CheckoutAttributesXML, 
            string CustomerCurrencyCode, decimal OrderWeight,
            int AffiliateID, int OrderStatusID, bool AllowStoringCreditCardNumber, string CardType,
            string CardName, string CardNumber, string MaskedCreditCardNumber, string CardCVV2,
            string CardExpirationMonth, string CardExpirationYear, int PaymentMethodID,
            string PaymentMethodName, string AuthorizationTransactionID, string AuthorizationTransactionCode,
            string AuthorizationTransactionResult, string CaptureTransactionID, string CaptureTransactionResult,
            string SubscriptionTransactionID, string PurchaseOrderNumber, int PaymentStatusID, DateTime? PaidDate, 
            string BillingFirstName, string BillingLastName,
            string BillingPhoneNumber, string BillingEmail, string BillingFaxNumber, string BillingCompany,
            string BillingAddress1, string BillingAddress2, string BillingCity, string BillingStateProvince,
            int BillingStateProvinceID, string BillingZipPostalCode, string BillingCountry,
            int BillingCountryID, int ShippingStatusID, string ShippingFirstName,
            string ShippingLastName, string ShippingPhoneNumber, string ShippingEmail,
            string ShippingFaxNumber, string ShippingCompany, string ShippingAddress1,
            string ShippingAddress2, string ShippingCity, string ShippingStateProvince,
            int ShippingStateProvinceID, string ShippingZipPostalCode,
            string ShippingCountry, int ShippingCountryID, string ShippingMethod, int ShippingRateComputationMethodID, DateTime? ShippedDate,
            string TrackingNumber, bool Deleted, DateTime CreatedOn);

        /// <summary>
        /// Gets an order note
        /// </summary>
        /// <param name="OrderNoteID">Order note identifier</param>
        /// <returns>Order note</returns>
        public abstract DBOrderNote GetOrderNoteByID(int OrderNoteID);

        /// <summary>
        /// Gets an order notes by order identifier
        /// </summary>
        /// <param name="OrderID">Order identifier</param>
        /// <param name="ShowHidden">A value indicating whether all orders should be loaded</param>
        /// <returns>Order note collection</returns>
        public abstract DBOrderNoteCollection GetOrderNoteByOrderID(int OrderID, bool ShowHidden);

        /// <summary>
        /// Deletes an order note
        /// </summary>
        /// <param name="OrderNoteID">Order note identifier</param>
        public abstract void DeleteOrderNote(int OrderNoteID);

        /// <summary>
        /// Inserts an order note
        /// </summary>
        /// <param name="OrderID">The order identifier</param>
        /// <param name="Note">The note</param>
        /// <param name="DisplayToCustomer">A value indicating whether the customer can see a note</param>
        /// <param name="CreatedOn">The date and time of order note creation</param>
        /// <returns>Order note</returns>
        public abstract DBOrderNote InsertOrderNote(int OrderID, string Note, bool DisplayToCustomer, DateTime CreatedOn);

        /// <summary>
        /// Updates the order note
        /// </summary>
        /// <param name="OrderNoteID">The order note identifier</param>
        /// <param name="OrderID">The order identifier</param>
        /// <param name="Note">The note</param>
        /// <param name="DisplayToCustomer">A value indicating whether the customer can see a note</param>
        /// <param name="CreatedOn">The date and time of order note creation</param>
        /// <returns>Order note</returns>
        public abstract DBOrderNote UpdateOrderNote(int OrderNoteID, int OrderID, string Note, bool DisplayToCustomer, DateTime CreatedOn);

        /// <summary>
        /// Gets an order product variant
        /// </summary>
        /// <param name="OrderProductVariantID">Order product variant identifier</param>
        /// <returns>Order product variant</returns>
        public abstract DBOrderProductVariant GetOrderProductVariantByID(int OrderProductVariantID);

        /// <summary>
        /// Gets an order product variant
        /// </summary>
        /// <param name="OrderProductVariantGUID">Order product variant identifier</param>
        /// <returns>Order product variant</returns>
        public abstract DBOrderProductVariant GetOrderProductVariantByGUID(Guid OrderProductVariantGUID);

        /// <summary>
        /// Gets all order product variants
        /// </summary>
        /// <param name="OrderID">Order identifier; null to load all records</param>
        /// <param name="CustomerID">Customer identifier; null to load all records</param>
        /// <param name="StartTime">Order start time; null to load all records</param>
        /// <param name="EndTime">Order end time; null to load all records</param>
        /// <param name="OrderStatusID">Order status identifier; null to load all records</param>
        /// <param name="PaymentStatusID">Order payment status identifier; null to load all records</param>
        /// <param name="ShippingStatusID">Order shipping status identifier; null to load all records</param>
        /// <param name="LoadDownloableProductsOnly">Value indicating whether to load downloadable products only</param>
        /// <returns>Order collection</returns>
        public abstract DBOrderProductVariantCollection GetAllOrderProductVariants(int? OrderID, 
            int? CustomerID, DateTime? StartTime, DateTime? EndTime,
            int? OrderStatusID, int? PaymentStatusID, int? ShippingStatusID,
            bool LoadDownloableProductsOnly);

        /// <summary>
        /// Inserts a order product variant
        /// </summary>
        /// <param name="OrderProductVariantGUID">The order product variant identifier</param>
        /// <param name="OrderID">The order identifier</param>
        /// <param name="ProductVariantID">The product variant identifier</param>
        /// <param name="UnitPriceInclTax">The unit price in primary store currency (incl tax)</param>
        /// <param name="UnitPriceExclTax">The unit price in primary store currency (excl tax)</param>
        /// <param name="PriceInclTax">The price in primary store currency (incl tax)</param>
        /// <param name="PriceExclTax">The price in primary store currency (excl tax)</param>
        /// <param name="UnitPriceInclTaxInCustomerCurrency">The unit price in primary store currency (incl tax)</param>
        /// <param name="UnitPriceExclTaxInCustomerCurrency">The unit price in customer currency (excl tax)</param>
        /// <param name="PriceInclTaxInCustomerCurrency">The price in primary store currency (incl tax)</param>
        /// <param name="PriceExclTaxInCustomerCurrency">The price in customer currency (excl tax)</param>
        /// <param name="AttributeDescription">The attribute description</param>
        /// <param name="AttributesXML">The attribute description in XML format</param>
        /// <param name="Quantity">The quantity</param>
        /// <param name="DiscountAmountInclTax">The discount amount (incl tax)</param>
        /// <param name="DiscountAmountExclTax">The discount amount (excl tax)</param>
        /// <param name="DownloadCount">The download count</param>
        /// <param name="IsDownloadActivated">The value indicating whether download is activated</param>
        /// <param name="LicenseDownloadID">A license download identifier (in case this is a downloadable product)</param>
        /// <returns>Order product variant</returns>
        public abstract DBOrderProductVariant InsertOrderProductVariant(Guid OrderProductVariantGUID, 
            int OrderID, int ProductVariantID, decimal UnitPriceInclTax,
            decimal UnitPriceExclTax, decimal PriceInclTax, decimal PriceExclTax,
            decimal UnitPriceInclTaxInCustomerCurrency, decimal UnitPriceExclTaxInCustomerCurrency,
            decimal PriceInclTaxInCustomerCurrency, decimal PriceExclTaxInCustomerCurrency,
            string AttributeDescription, string AttributesXML, int Quantity,
            decimal DiscountAmountInclTax, decimal DiscountAmountExclTax,
            int DownloadCount, bool IsDownloadActivated, int LicenseDownloadID);

        /// <summary>
        /// Updates the order product variant
        /// </summary>
        /// <param name="OrderProductVariantID">The order product variant identifier</param>
        /// <param name="OrderProductVariantGUID">The order product variant identifier</param>
        /// <param name="OrderID">The order identifier</param>
        /// <param name="ProductVariantID">The product variant identifier</param>
        /// <param name="UnitPriceInclTax">The unit price in primary store currency (incl tax)</param>
        /// <param name="UnitPriceExclTax">The unit price in primary store currency (excl tax)</param>
        /// <param name="PriceInclTax">The price in primary store currency (incl tax)</param>
        /// <param name="PriceExclTax">The price in primary store currency (excl tax)</param>
        /// <param name="UnitPriceInclTaxInCustomerCurrency">The unit price in primary store currency (incl tax)</param>
        /// <param name="UnitPriceExclTaxInCustomerCurrency">The unit price in customer currency (excl tax)</param>
        /// <param name="PriceInclTaxInCustomerCurrency">The price in primary store currency (incl tax)</param>
        /// <param name="PriceExclTaxInCustomerCurrency">The price in customer currency (excl tax)</param>
        /// <param name="AttributeDescription">The attribute description</param>
        /// <param name="AttributesXML">The attribute description in XML format</param>
        /// <param name="Quantity">The quantity</param>
        /// <param name="DiscountAmountInclTax">The discount amount (incl tax)</param>
        /// <param name="DiscountAmountExclTax">The discount amount (excl tax)</param>
        /// <param name="DownloadCount">The download count</param>
        /// <param name="IsDownloadActivated">The value indicating whether download is activated</param>
        /// <param name="LicenseDownloadID">A license download identifier (in case this is a downloadable product)</param>
        /// <returns>Order product variant</returns>
        public abstract DBOrderProductVariant UpdateOrderProductVariant(int OrderProductVariantID,
            Guid OrderProductVariantGUID, int OrderID, int ProductVariantID,
            decimal UnitPriceInclTax, decimal UnitPriceExclTax, decimal PriceInclTax, decimal PriceExclTax,
            decimal UnitPriceInclTaxInCustomerCurrency, decimal UnitPriceExclTaxInCustomerCurrency,
            decimal PriceInclTaxInCustomerCurrency, decimal PriceExclTaxInCustomerCurrency,
            string AttributeDescription, string AttributesXML, int Quantity,
            decimal DiscountAmountInclTax, decimal DiscountAmountExclTax,
            int DownloadCount, bool IsDownloadActivated, int LicenseDownloadID);

        /// <summary>
        /// Gets an order status by ID
        /// </summary>
        /// <param name="OrderStatusID">Order status identifier</param>
        /// <returns>Order status</returns>
        public abstract DBOrderStatus GetOrderStatusByID(int OrderStatusID);

        /// <summary>
        /// Gets all order statuses
        /// </summary>
        /// <returns>Order status collection</returns>
        public abstract DBOrderStatusCollection GetAllOrderStatuses();

        /// <summary>
        /// Gets an order report
        /// </summary>
        /// <param name="OrderStatusID">Order status identifier; null to load all orders</param>
        /// <param name="PaymentStatusID">Order payment status identifier; null to load all orders</param>
        /// <param name="ShippingStatusID">Order shipping status identifier; null to load all orders</param>
        /// <returns>IDataReader</returns>
        public abstract IDataReader GetOrderReport(int? OrderStatusID, int? PaymentStatusID, int? ShippingStatusID);
        
        /// <summary>
        /// Gets a recurring payment
        /// </summary>
        /// <param name="RecurringPaymentID">The recurring payment identifier</param>
        /// <returns>Recurring payment</returns>
        public abstract DBRecurringPayment GetRecurringPaymentByID(int RecurringPaymentID);

        /// <summary>
        /// Inserts a recurring payment
        /// </summary>
        /// <param name="InitialOrderID">The initial order identifier</param>
        /// <param name="CycleLength">The cycle length</param>
        /// <param name="CyclePeriod">The cycle period</param>
        /// <param name="TotalCycles">The total cycles</param>
        /// <param name="StartDate">The start date</param>
        /// <param name="IsActive">The value indicating whether the payment is active</param>
        /// <param name="Deleted">The value indicating whether the entity has been deleted</param>
        /// <param name="CreatedOn">The date and time of payment creation</param>
        /// <returns>Recurring payment</returns>
        public abstract DBRecurringPayment InsertRecurringPayment(int InitialOrderID, 
            int CycleLength, int CyclePeriod, int TotalCycles,
            DateTime StartDate, bool IsActive, bool Deleted, DateTime CreatedOn);

        /// <summary>
        /// Updates the recurring payment
        /// </summary>
        /// <param name="RecurringPaymentID">The recurring payment identifier</param>
        /// <param name="InitialOrderID">The initial order identifier</param>
        /// <param name="CycleLength">The cycle length</param>
        /// <param name="CyclePeriod">The cycle period</param>
        /// <param name="TotalCycles">The total cycles</param>
        /// <param name="StartDate">The start date</param>
        /// <param name="IsActive">The value indicating whether the payment is active</param>
        /// <param name="Deleted">The value indicating whether the entity has been deleted</param>
        /// <param name="CreatedOn">The date and time of payment creation</param>
        /// <returns>Recurring payment</returns>
        public abstract DBRecurringPayment UpdateRecurringPayment(int RecurringPaymentID,
            int InitialOrderID, int CycleLength, int CyclePeriod, int TotalCycles,
            DateTime StartDate, bool IsActive, bool Deleted, DateTime CreatedOn);

        /// <summary>
        /// Search recurring payments
        /// </summary>
        /// <param name="showHidden">A value indicating whether to show hidden records</param>
        /// <param name="CustomerID">The customer identifier; 0 to load all records</param>
        /// <param name="InitialOrderID">The initial order identifier; 0 to load all records</param>
        /// <param name="InitialOrderStatusID">Initial order status identifier; null to load all records</param>
        /// <returns>Recurring payment collection</returns>
        public abstract DBRecurringPaymentCollection SearchRecurringPayments(bool showHidden,
            int CustomerID, int InitialOrderID, int? InitialOrderStatusID);

        /// <summary>
        /// Deletes a recurring payment history
        /// </summary>
        /// <param name="RecurringPaymentHistoryID">Recurring payment history identifier</param>
        public abstract void DeleteRecurringPaymentHistory(int RecurringPaymentHistoryID);

        /// <summary>
        /// Gets a recurring payment history
        /// </summary>
        /// <param name="RecurringPaymentHistoryID">The recurring payment history identifier</param>
        /// <returns>Recurring payment history</returns>
        public abstract DBRecurringPaymentHistory GetRecurringPaymentHistoryByID(int RecurringPaymentHistoryID);

        /// <summary>
        /// Inserts a recurring payment history
        /// </summary>
        /// <param name="RecurringPaymentID">The recurring payment identifier</param>
        /// <param name="OrderID">The order identifier</param>
        /// <param name="CreatedOn">The date and time of payment creation</param>
        /// <returns>Recurring payment history</returns>
        public abstract DBRecurringPaymentHistory InsertRecurringPaymentHistory(int RecurringPaymentID, 
            int OrderID, DateTime CreatedOn);

        /// <summary>
        /// Updates the recurring payment history
        /// </summary>
        /// <param name="RecurringPaymentHistoryID">The recurring payment history identifier</param>
        /// <param name="RecurringPaymentID">The recurring payment identifier</param>
        /// <param name="OrderID">The order identifier</param>
        /// <param name="CreatedOn">The date and time of payment creation</param>
        /// <returns>Recurring payment history</returns>
        public abstract DBRecurringPaymentHistory UpdateRecurringPaymentHistory(int RecurringPaymentHistoryID,
            int RecurringPaymentID, int OrderID, DateTime CreatedOn);

        /// <summary>
        /// Search recurring payment history
        /// </summary>
        /// <param name="RecurringPaymentID">The recurring payment identifier; 0 to load all records</param>
        /// <param name="OrderID">The order identifier; 0 to load all records</param>
        /// <returns>Recurring payment history collection</returns>
        public abstract DBRecurringPaymentHistoryCollection SearchRecurringPaymentHistory(int RecurringPaymentID, int OrderID);

        /// <summary>
        /// Deletes a gift card
        /// </summary>
        /// <param name="GiftCardID">Gift card identifier</param>
        public abstract void DeleteGiftCard(int GiftCardID);

        /// <summary>
        /// Gets a gift card
        /// </summary>
        /// <param name="GiftCardID">Gift card identifier</param>
        /// <returns>Gift card entry</returns>
        public abstract DBGiftCard GetGiftCardByID(int GiftCardID);

        /// <summary>
        /// Gets all gift cards
        /// </summary>
        /// <param name="OrderID">Order identifier; null to load all records</param>
        /// <param name="CustomerID">Customer identifier; null to load all records</param>
        /// <param name="StartTime">Order start time; null to load all records</param>
        /// <param name="EndTime">Order end time; null to load all records</param>
        /// <param name="OrderStatusID">Order status identifier; null to load all records</param>
        /// <param name="PaymentStatusID">Order payment status identifier; null to load all records</param>
        /// <param name="ShippingStatusID">Order shipping status identifier; null to load all records</param>
        /// <param name="IsGiftCardActivated">Value indicating whether gift card is activated; null to load all records</param>
        /// <param name="GiftCardCouponCode">Gift card coupon code; null or string.empty to load all records</param>
        /// <returns>Gift cards</returns>
        public abstract DBGiftCardCollection GetAllGiftCards(int? OrderID,
            int? CustomerID, DateTime? StartTime, DateTime? EndTime,
            int? OrderStatusID, int? PaymentStatusID, int? ShippingStatusID,
            bool? IsGiftCardActivated, string GiftCardCouponCode);

        /// <summary>
        /// Inserts a gift card
        /// </summary>
        /// <param name="PurchasedOrderProductVariantID">Purchased order product variant identifier</param>
        /// <param name="Amount">Amount</param>
        /// <param name="IsGiftCardActivated">Value indicating whether gift card is activated</param>
        /// <param name="GiftCardCouponCode">Gift card coupon code</param>
        /// <param name="RecipientName">Recipient name</param>
        /// <param name="RecipientEmail">Recipient email</param>
        /// <param name="SenderName">Sender name</param>
        /// <param name="SenderEmail">Sender email</param>
        /// <param name="Message">Message</param>
        /// <param name="IsSenderNotified">Value indicating whether sender is notified</param>
        /// <param name="CreatedOn">A date and time of instance creation</param>
        /// <returns>Gift card</returns>
        public abstract DBGiftCard InsertGiftCard(int PurchasedOrderProductVariantID,
            decimal Amount, bool IsGiftCardActivated, string GiftCardCouponCode,
            string RecipientName, string RecipientEmail,
            string SenderName, string SenderEmail, string Message, 
            bool IsSenderNotified, DateTime CreatedOn);

        /// <summary>
        /// Updates the gift card
        /// </summary>
        /// <param name="GiftCardID">Gift card identifier</param>
        /// <param name="PurchasedOrderProductVariantID">Purchased order product variant identifier</param>
        /// <param name="Amount">Amount</param>
        /// <param name="IsGiftCardActivated">Value indicating whether gift card is activated</param>
        /// <param name="GiftCardCouponCode">Gift card coupon code</param>
        /// <param name="RecipientName">Recipient name</param>
        /// <param name="RecipientEmail">Recipient email</param>
        /// <param name="SenderName">Sender name</param>
        /// <param name="SenderEmail">Sender email</param>
        /// <param name="Message">Message</param>
        /// <param name="IsSenderNotified">Value indicating whether sender is notified</param>
        /// <param name="CreatedOn">A date and time of instance creation</param>
        /// <returns>Gift card</returns>
        public abstract DBGiftCard UpdateGiftCard(int GiftCardID, 
            int PurchasedOrderProductVariantID, decimal Amount, 
            bool IsGiftCardActivated, string GiftCardCouponCode,
            string RecipientName, string RecipientEmail,
            string SenderName, string SenderEmail, string Message,
            bool IsSenderNotified, DateTime CreatedOn);
        
        /// <summary>
        /// Deletes a gift card usage history entry
        /// </summary>
        /// <param name="GiftCardUsageHistoryID">Gift card usage history entry identifier</param>
        public abstract void DeleteGiftCardUsageHistory(int GiftCardUsageHistoryID);

        /// <summary>
        /// Gets a gift card usage history entry
        /// </summary>
        /// <param name="GiftCardUsageHistoryID">Gift card usage history entry identifier</param>
        /// <returns>Gift card usage history entry</returns>
        public abstract DBGiftCardUsageHistory GetGiftCardUsageHistoryByID(int GiftCardUsageHistoryID);

        /// <summary>
        /// Gets all gift card usage history entries
        /// </summary>
        /// <param name="GiftCardID">Gift card identifier identifier; null to load all records</param>
        /// <param name="CustomerID">Customer identifier; null to load all records</param>
        /// <param name="OrderID">Order identifier; null to load all records</param>
        /// <returns>Gift card usage history entries</returns>
        public abstract DBGiftCardUsageHistoryCollection GetAllGiftCardUsageHistoryEntries(int? GiftCardID,
            int? CustomerID, int? OrderID);

        /// <summary>
        /// Inserts a gift card usage history entry
        /// </summary>
        /// <param name="GiftCardID">Gift card identifier</param>
        /// <param name="CustomerID">Customer identifier</param>
        /// <param name="OrderID">Order identifier</param>
        /// <param name="UsedValue">Used value</param>
        /// <param name="UsedValueInCustomerCurrency">Used value (customer currency)</param>
        /// <param name="CreatedOn">A date and time of instance creation</param>
        /// <returns>Gift card usage history entry</returns>
        public abstract DBGiftCardUsageHistory InsertGiftCardUsageHistory(int GiftCardID,
            int CustomerID, int OrderID, decimal UsedValue, 
            decimal UsedValueInCustomerCurrency, DateTime CreatedOn);

        /// <summary>
        /// Updates the gift card usage history entry
        /// </summary>
        /// <param name="GiftCardUsageHistoryID">Gift card usage history entry identifier</param>
        /// <param name="GiftCardID">Gift card identifier</param>
        /// <param name="CustomerID">Customer identifier</param>
        /// <param name="OrderID">Order identifier</param>
        /// <param name="UsedValue">Used value</param>
        /// <param name="UsedValueInCustomerCurrency">Used value (customer currency)</param>
        /// <param name="CreatedOn">A date and time of instance creation</param>
        /// <returns>Gift card usage history entry</returns>
        public abstract DBGiftCardUsageHistory UpdateGiftCardUsageHistory(int GiftCardUsageHistoryID,
            int GiftCardID, int CustomerID, int OrderID, decimal UsedValue,
            decimal UsedValueInCustomerCurrency, DateTime CreatedOn);
        
        #endregion
    }
}
