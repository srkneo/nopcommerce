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

        #region Orders

        /// <summary>
        /// Gets an order
        /// </summary>
        /// <param name="orderId">The order identifier</param>
        /// <returns>Order</returns>
        public abstract DBOrder GetOrderById(int orderId);

        /// <summary>
        /// Gets an order
        /// </summary>
        /// <param name="orderGuid">The order identifier</param>
        /// <returns>Order</returns>
        public abstract DBOrder GetOrderByGuid(Guid orderGuid);

        /// <summary>
        /// Search orders
        /// </summary>
        /// <param name="startTime">Order start time; null to load all orders</param>
        /// <param name="endTime">Order end time; null to load all orders</param>
        /// <param name="customerEmail">Customer email</param>
        /// <param name="orderStatusId">Order status identifier; null to load all orders</param>
        /// <param name="paymentStatusId">Order payment status identifier; null to load all orders</param>
        /// <param name="shippingStatusId">Order shipping status identifier; null to load all orders</param>
        /// <returns>Order collection</returns>
        public abstract DBOrderCollection SearchOrders(DateTime? startTime, 
            DateTime? endTime, string customerEmail, int? orderStatusId, 
            int? paymentStatusId, int? shippingStatusId);

        /// <summary>
        /// Gets all orders by customer identifier
        /// </summary>
        /// <param name="customerId">Customer identifier</param>
        /// <returns>Order collection</returns>
        public abstract DBOrderCollection GetOrdersByCustomerId(int customerId);

        /// <summary>
        /// Gets an order by authorization transaction identifier
        /// </summary>
        /// <param name="authorizationTransactionId">Authorization transaction identifier</param>
        /// <param name="paymentMethodId">Payment method identifier</param>
        /// <returns>Order</returns>
        public abstract DBOrder GetOrderByAuthorizationTransactionIdAndPaymentMethodId(string authorizationTransactionId, int paymentMethodId);

        /// <summary>
        /// Gets all orders by affiliate identifier
        /// </summary>
        /// <param name="affiliateId">Affiliate identifier</param>
        /// <returns>Order collection</returns>
        public abstract DBOrderCollection GetOrdersByAffiliateId(int affiliateId);

        /// <summary>
        /// Inserts an order
        /// </summary>
        /// <param name="orderGuid">The order identifier</param>
        /// <param name="customerId">The customer identifier</param>
        /// <param name="customerLanguageId">The customer language identifier</param>
        /// <param name="customerTaxDisplayTypeId">The customer tax display type identifier</param>
        /// <param name="customerIP">The customer IP address</param>
        /// <param name="orderSubtotalInclTax">The order subtotal (incl tax)</param>
        /// <param name="orderSubtotalExclTax">The order subtotal (excl tax)</param>
        /// <param name="orderShippingInclTax">The order shipping (incl tax)</param>
        /// <param name="orderShippingExclTax">The order shipping (excl tax)</param>
        /// <param name="paymentMethodAdditionalFeeInclTax">The payment method additional fee (incl tax)</param>
        /// <param name="paymentMethodAdditionalFeeExclTax">The payment method additional fee (excl tax)</param>
        /// <param name="orderTax">The order tax</param>
        /// <param name="orderTotal">The order total</param>
        /// <param name="orderDiscount">The order discount</param>
        /// <param name="orderSubtotalInclTaxInCustomerCurrency">The order subtotal incl tax (customer currency)</param>
        /// <param name="orderSubtotalExclTaxInCustomerCurrency">The order subtotal excl tax (customer currency)</param>
        /// <param name="orderShippingInclTaxInCustomerCurrency">The order shipping incl tax (customer currency)</param>
        /// <param name="orderShippingExclTaxInCustomerCurrency">The order shipping excl tax (customer currency)</param>
        /// <param name="paymentMethodAdditionalFeeInclTaxInCustomerCurrency">The payment method additional fee incl tax (customer currency)</param>
        /// <param name="paymentMethodAdditionalFeeExclTaxInCustomerCurrency">The payment method additional fee excl tax (customer currency)</param>
        /// <param name="orderTaxInCustomerCurrency">The order tax (customer currency)</param>
        /// <param name="orderTotalInCustomerCurrency">The order total (customer currency)</param>
        /// <param name="orderDiscountInCustomerCurrency">The order discount (customer currency)</param>
        /// <param name="checkoutAttributeDescription">The checkout attribute description</param>
        /// <param name="checkoutAttributesXml">The checkout attributes in XML format</param>
        /// <param name="customerCurrencyCode">The customer currency code</param>
        /// <param name="orderWeight">The order weight</param>
        /// <param name="affiliateId">The affiliate identifier</param>
        /// <param name="orderStatusId">The order status identifier</param>
        /// <param name="allowStoringCreditCardNumber">The value indicating whether storing of credit card number is allowed</param>
        /// <param name="cardType">The card type</param>
        /// <param name="cardName">The card name</param>
        /// <param name="cardNumber">The card number</param>
        /// <param name="maskedCreditCardNumber">The masked credit card number</param>
        /// <param name="cardCvv2">The card CVV2</param>
        /// <param name="cardExpirationMonth">The card expiration month</param>
        /// <param name="cardExpirationYear">The card expiration year</param>
        /// <param name="paymentMethodId">The payment method identifier</param>
        /// <param name="paymentMethodName">The payment method name</param>
        /// <param name="authorizationTransactionId">The authorization transaction identifier</param>
        /// <param name="authorizationTransactionCode">The authorization transaction code</param>
        /// <param name="authorizationTransactionResult">The authorization transaction result</param>
        /// <param name="captureTransactionId">The capture transaction identifier</param>
        /// <param name="captureTransactionResult">The capture transaction result</param>
        /// <param name="subscriptionTransactionId">The subscription transaction identifier</param>
        /// <param name="purchaseOrderNumber">The purchase order number</param>
        /// <param name="paymentStatusId">The payment status identifier</param>
        /// <param name="paidDate">The paid date and time</param>
        /// <param name="billingFirstName">The billing first name</param>
        /// <param name="billingLastName">The billing last name</param>
        /// <param name="billingPhoneNumber">he billing phone number</param>
        /// <param name="billingEmail">The billing email</param>
        /// <param name="billingFaxNumber">The billing fax number</param>
        /// <param name="billingCompany">The billing company</param>
        /// <param name="billingAddress1">The billing address 1</param>
        /// <param name="billingAddress2">The billing address 2</param>
        /// <param name="billingCity">The billing city</param>
        /// <param name="billingStateProvince">The billing state/province</param>
        /// <param name="billingStateProvinceId">The billing state/province identifier</param>
        /// <param name="billingZipPostalCode">The billing zip/postal code</param>
        /// <param name="billingCountry">The billing country</param>
        /// <param name="billingCountryId">The billing country identifier</param>
        /// <param name="shippingStatusId">The shipping status identifier</param>
        /// <param name="shippingFirstName">The shipping first name</param>
        /// <param name="shippingLastName">The shipping last name</param>
        /// <param name="shippingPhoneNumber">The shipping phone number</param>
        /// <param name="shippingEmail">The shipping email</param>
        /// <param name="shippingFaxNumber">The shipping fax number</param>
        /// <param name="shippingCompany">The shipping  company</param>
        /// <param name="shippingAddress1">The shipping address 1</param>
        /// <param name="shippingAddress2">The shipping address 2</param>
        /// <param name="shippingCity">The shipping city</param>
        /// <param name="shippingStateProvince">The shipping state/province</param>
        /// <param name="shippingStateProvinceId">The shipping state/province identifier</param>
        /// <param name="shippingZipPostalCode">The shipping zip/postal code</param>
        /// <param name="shippingCountry">The shipping country</param>
        /// <param name="shippingCountryId">The shipping country identifier</param>
        /// <param name="shippingMethod">The shipping method</param>
        /// <param name="shippingRateComputationMethodId">The shipping rate computation method identifier</param>
        /// <param name="shippedDate">The shipped date and time</param>
        /// <param name="deliveryDate">The delivery date and time</param>
        /// <param name="trackingNumber">The tracking number of order</param>
        /// <param name="deleted">A value indicating whether the entity has been deleted</param>
        /// <param name="createdOn">The date and time of order creation</param>
        /// <returns>Order</returns>
        public abstract DBOrder InsertOrder(Guid orderGuid, 
            int customerId, 
            int customerLanguageId,
            int customerTaxDisplayTypeId, 
            string customerIP, 
            decimal orderSubtotalInclTax, 
            decimal orderSubtotalExclTax,
            decimal orderShippingInclTax, 
            decimal orderShippingExclTax, 
            decimal paymentMethodAdditionalFeeInclTax, 
            decimal paymentMethodAdditionalFeeExclTax,
            decimal orderTax, 
            decimal orderTotal,
            decimal orderDiscount,
            decimal orderSubtotalInclTaxInCustomerCurrency, 
            decimal orderSubtotalExclTaxInCustomerCurrency,
            decimal orderShippingInclTaxInCustomerCurrency, 
            decimal orderShippingExclTaxInCustomerCurrency,
            decimal paymentMethodAdditionalFeeInclTaxInCustomerCurrency, 
            decimal paymentMethodAdditionalFeeExclTaxInCustomerCurrency,
            decimal orderTaxInCustomerCurrency,
            decimal orderTotalInCustomerCurrency,
            decimal orderDiscountInCustomerCurrency, 
            string checkoutAttributeDescription,
            string checkoutAttributesXml,
            string customerCurrencyCode, 
            decimal orderWeight,
            int affiliateId, 
            int orderStatusId,
            bool allowStoringCreditCardNumber, 
            string cardType, 
            string cardName, 
            string cardNumber, 
            string maskedCreditCardNumber,
            string cardCvv2,
            string cardExpirationMonth, 
            string cardExpirationYear, 
            int paymentMethodId,
            string paymentMethodName, 
            string authorizationTransactionId, 
            string authorizationTransactionCode,
            string authorizationTransactionResult,
            string captureTransactionId,
            string captureTransactionResult,
            string subscriptionTransactionId, 
            string purchaseOrderNumber, 
            int paymentStatusId,
            DateTime? paidDate, 
            string billingFirstName,
            string billingLastName,
            string billingPhoneNumber,
            string billingEmail, 
            string billingFaxNumber, 
            string billingCompany,
            string billingAddress1,
            string billingAddress2, 
            string billingCity, 
            string billingStateProvince,
            int billingStateProvinceId, 
            string billingZipPostalCode, 
            string billingCountry,
            int billingCountryId, 
            int shippingStatusId, 
            string shippingFirstName,
            string shippingLastName, 
            string shippingPhoneNumber, 
            string shippingEmail,
            string shippingFaxNumber, 
            string shippingCompany, 
            string shippingAddress1,
            string shippingAddress2, 
            string shippingCity, 
            string shippingStateProvince,
            int shippingStateProvinceId, 
            string shippingZipPostalCode,
            string shippingCountry, 
            int shippingCountryId, 
            string shippingMethod,
            int shippingRateComputationMethodId,
            DateTime? shippedDate,
            DateTime? deliveryDate,
            string trackingNumber, 
            bool deleted, 
            DateTime createdOn);

        /// <summary>
        /// Updates the order
        /// </summary>
        /// <param name="orderId">The order identifier</param>
        /// <param name="orderGuid">The order identifier</param>
        /// <param name="customerId">The customer identifier</param>
        /// <param name="customerLanguageId">The customer language identifier</param>
        /// <param name="customerTaxDisplayTypeId">The customer tax display type identifier</param>
        /// <param name="customerIP">The customer IP address</param>
        /// <param name="orderSubtotalInclTax">The order subtotal (incl tax)</param>
        /// <param name="orderSubtotalExclTax">The order subtotal (excl tax)</param>
        /// <param name="orderShippingInclTax">The order shipping (incl tax)</param>
        /// <param name="orderShippingExclTax">The order shipping (excl tax)</param>
        /// <param name="paymentMethodAdditionalFeeInclTax">The payment method additional fee (incl tax)</param>
        /// <param name="paymentMethodAdditionalFeeExclTax">The payment method additional fee (excl tax)</param>
        /// <param name="orderTax">The order tax</param>
        /// <param name="orderTotal">The order total</param>
        /// <param name="orderDiscount">The order discount</param>
        /// <param name="orderSubtotalInclTaxInCustomerCurrency">The order subtotal incl tax (customer currency)</param>
        /// <param name="orderSubtotalExclTaxInCustomerCurrency">The order subtotal excl tax (customer currency)</param>
        /// <param name="orderShippingInclTaxInCustomerCurrency">The order shipping incl tax (customer currency)</param>
        /// <param name="orderShippingExclTaxInCustomerCurrency">The order shipping excl tax (customer currency)</param>
        /// <param name="paymentMethodAdditionalFeeInclTaxInCustomerCurrency">The payment method additional fee incl tax (customer currency)</param>
        /// <param name="paymentMethodAdditionalFeeExclTaxInCustomerCurrency">The payment method additional fee excl tax (customer currency)</param>
        /// <param name="orderTaxInCustomerCurrency">The order tax (customer currency)</param>
        /// <param name="orderTotalInCustomerCurrency">The order total (customer currency)</param>
        /// <param name="orderDiscountInCustomerCurrency">The order discount (customer currency)</param>
        /// <param name="checkoutAttributeDescription">The checkout attribute description</param>
        /// <param name="checkoutAttributesXml">The checkout attributes in XML format</param>
        /// <param name="customerCurrencyCode">The customer currency code</param>
        /// <param name="orderWeight">The order weight</param>
        /// <param name="affiliateId">The affiliate identifier</param>
        /// <param name="orderStatusId">The order status identifier</param>
        /// <param name="allowStoringCreditCardNumber">The value indicating whether storing of credit card number is allowed</param>
        /// <param name="cardType">The card type</param>
        /// <param name="cardName">The card name</param>
        /// <param name="cardNumber">The card number</param>
        /// <param name="maskedCreditCardNumber">The masked credit card number</param>
        /// <param name="cardCvv2">The card CVV2</param>
        /// <param name="cardExpirationMonth">The card expiration month</param>
        /// <param name="cardExpirationYear">The card expiration year</param>
        /// <param name="paymentMethodId">The payment method identifier</param>
        /// <param name="paymentMethodName">The payment method name</param>
        /// <param name="authorizationTransactionId">The authorization transaction identifier</param>
        /// <param name="authorizationTransactionCode">The authorization transaction code</param>
        /// <param name="authorizationTransactionResult">The authorization transaction result</param>
        /// <param name="captureTransactionId">The capture transaction identifier</param>
        /// <param name="captureTransactionResult">The capture transaction result</param>
        /// <param name="subscriptionTransactionId">The subscription transaction identifier</param>
        /// <param name="purchaseOrderNumber">The purchase order number</param>
        /// <param name="paymentStatusId">The payment status identifier</param>
        /// <param name="paidDate">The paid date and time</param>
        /// <param name="billingFirstName">The billing first name</param>
        /// <param name="billingLastName">The billing last name</param>
        /// <param name="billingPhoneNumber">he billing phone number</param>
        /// <param name="billingEmail">The billing email</param>
        /// <param name="billingFaxNumber">The billing fax number</param>
        /// <param name="billingCompany">The billing company</param>
        /// <param name="billingAddress1">The billing address 1</param>
        /// <param name="billingAddress2">The billing address 2</param>
        /// <param name="billingCity">The billing city</param>
        /// <param name="billingStateProvince">The billing state/province</param>
        /// <param name="billingStateProvinceId">The billing state/province identifier</param>
        /// <param name="billingZipPostalCode">The billing zip/postal code</param>
        /// <param name="billingCountry">The billing country</param>
        /// <param name="billingCountryId">The billing country identifier</param>
        /// <param name="shippingStatusId">The shipping status identifier</param>
        /// <param name="shippingFirstName">The shipping first name</param>
        /// <param name="shippingLastName">The shipping last name</param>
        /// <param name="shippingPhoneNumber">The shipping phone number</param>
        /// <param name="shippingEmail">The shipping email</param>
        /// <param name="shippingFaxNumber">The shipping fax number</param>
        /// <param name="shippingCompany">The shipping  company</param>
        /// <param name="shippingAddress1">The shipping address 1</param>
        /// <param name="shippingAddress2">The shipping address 2</param>
        /// <param name="shippingCity">The shipping city</param>
        /// <param name="shippingStateProvince">The shipping state/province</param>
        /// <param name="shippingStateProvinceId">The shipping state/province identifier</param>
        /// <param name="shippingZipPostalCode">The shipping zip/postal code</param>
        /// <param name="shippingCountry">The shipping country</param>
        /// <param name="shippingCountryId">The shipping country identifier</param>
        /// <param name="shippingMethod">The shipping method</param>
        /// <param name="shippingRateComputationMethodId">The shipping rate computation method identifier</param>
        /// <param name="shippedDate">The shipped date and time</param>
        /// <param name="deliveryDate">The delivery date and time</param>
        /// <param name="trackingNumber">The tracking number of order</param>
        /// <param name="deleted">A value indicating whether the entity has been deleted</param>
        /// <param name="createdOn">The date and time of order creation</param>
        /// <returns>Order</returns>
        public abstract DBOrder UpdateOrder(int orderId,
            Guid orderGuid,
            int customerId,
            int customerLanguageId,
            int customerTaxDisplayTypeId,
            string customerIP,
            decimal orderSubtotalInclTax,
            decimal orderSubtotalExclTax,
            decimal orderShippingInclTax,
            decimal orderShippingExclTax,
            decimal paymentMethodAdditionalFeeInclTax,
            decimal paymentMethodAdditionalFeeExclTax,
            decimal orderTax,
            decimal orderTotal,
            decimal orderDiscount,
            decimal orderSubtotalInclTaxInCustomerCurrency,
            decimal orderSubtotalExclTaxInCustomerCurrency,
            decimal orderShippingInclTaxInCustomerCurrency,
            decimal orderShippingExclTaxInCustomerCurrency,
            decimal paymentMethodAdditionalFeeInclTaxInCustomerCurrency,
            decimal paymentMethodAdditionalFeeExclTaxInCustomerCurrency,
            decimal orderTaxInCustomerCurrency,
            decimal orderTotalInCustomerCurrency,
            decimal orderDiscountInCustomerCurrency,
            string checkoutAttributeDescription,
            string checkoutAttributesXml,
            string customerCurrencyCode,
            decimal orderWeight,
            int affiliateId,
            int orderStatusId,
            bool allowStoringCreditCardNumber,
            string cardType,
            string cardName,
            string cardNumber,
            string maskedCreditCardNumber,
            string cardCvv2,
            string cardExpirationMonth,
            string cardExpirationYear,
            int paymentMethodId,
            string paymentMethodName,
            string authorizationTransactionId,
            string authorizationTransactionCode,
            string authorizationTransactionResult,
            string captureTransactionId,
            string captureTransactionResult,
            string subscriptionTransactionId,
            string purchaseOrderNumber,
            int paymentStatusId,
            DateTime? paidDate,
            string billingFirstName,
            string billingLastName,
            string billingPhoneNumber,
            string billingEmail,
            string billingFaxNumber,
            string billingCompany,
            string billingAddress1,
            string billingAddress2,
            string billingCity,
            string billingStateProvince,
            int billingStateProvinceId,
            string billingZipPostalCode,
            string billingCountry,
            int billingCountryId,
            int shippingStatusId,
            string shippingFirstName,
            string shippingLastName,
            string shippingPhoneNumber,
            string shippingEmail,
            string shippingFaxNumber,
            string shippingCompany,
            string shippingAddress1,
            string shippingAddress2,
            string shippingCity,
            string shippingStateProvince,
            int shippingStateProvinceId,
            string shippingZipPostalCode,
            string shippingCountry,
            int shippingCountryId,
            string shippingMethod,
            int shippingRateComputationMethodId,
            DateTime? shippedDate,
            DateTime? deliveryDate,
            string trackingNumber,
            bool deleted,
            DateTime createdOn);

        #endregion

        #region Orders product variants


        /// <summary>
        /// Gets an order product variant
        /// </summary>
        /// <param name="orderProductVariantId">Order product variant identifier</param>
        /// <returns>Order product variant</returns>
        public abstract DBOrderProductVariant GetOrderProductVariantById(int orderProductVariantId);

        /// <summary>
        /// Gets an order product variant
        /// </summary>
        /// <param name="orderProductVariantGuid">Order product variant identifier</param>
        /// <returns>Order product variant</returns>
        public abstract DBOrderProductVariant GetOrderProductVariantByGuid(Guid orderProductVariantGuid);

        /// <summary>
        /// Gets all order product variants
        /// </summary>
        /// <param name="orderId">Order identifier; null to load all records</param>
        /// <param name="customerId">Customer identifier; null to load all records</param>
        /// <param name="startTime">Order start time; null to load all records</param>
        /// <param name="endTime">Order end time; null to load all records</param>
        /// <param name="orderStatusId">Order status identifier; null to load all records</param>
        /// <param name="paymentStatusId">Order payment status identifier; null to load all records</param>
        /// <param name="shippingStatusId">Order shipping status identifier; null to load all records</param>
        /// <param name="loadDownloableProductsOnly">Value indicating whether to load downloadable products only</param>
        /// <returns>Order collection</returns>
        public abstract DBOrderProductVariantCollection GetAllOrderProductVariants(int? orderId,
            int? customerId, DateTime? startTime, DateTime? endTime,
            int? orderStatusId, int? paymentStatusId, int? shippingStatusId,
            bool loadDownloableProductsOnly);

        /// <summary>
        /// Inserts a order product variant
        /// </summary>
        /// <param name="orderProductVariantGuid">The order product variant identifier</param>
        /// <param name="orderId">The order identifier</param>
        /// <param name="productVariantId">The product variant identifier</param>
        /// <param name="unitPriceInclTax">The unit price in primary store currency (incl tax)</param>
        /// <param name="unitPriceExclTax">The unit price in primary store currency (excl tax)</param>
        /// <param name="priceInclTax">The price in primary store currency (incl tax)</param>
        /// <param name="priceExclTax">The price in primary store currency (excl tax)</param>
        /// <param name="unitPriceInclTaxInCustomerCurrency">The unit price in primary store currency (incl tax)</param>
        /// <param name="unitPriceExclTaxInCustomerCurrency">The unit price in customer currency (excl tax)</param>
        /// <param name="priceInclTaxInCustomerCurrency">The price in primary store currency (incl tax)</param>
        /// <param name="priceExclTaxInCustomerCurrency">The price in customer currency (excl tax)</param>
        /// <param name="attributeDescription">The attribute description</param>
        /// <param name="attributesXml">The attribute description in XML format</param>
        /// <param name="quantity">The quantity</param>
        /// <param name="discountAmountInclTax">The discount amount (incl tax)</param>
        /// <param name="discountAmountExclTax">The discount amount (excl tax)</param>
        /// <param name="downloadCount">The download count</param>
        /// <param name="isDownloadActivated">The value indicating whether download is activated</param>
        /// <param name="licenseDownloadId">A license download identifier (in case this is a downloadable product)</param>
        /// <returns>Order product variant</returns>
        public abstract DBOrderProductVariant InsertOrderProductVariant(Guid orderProductVariantGuid,
            int orderId,
            int productVariantId,
            decimal unitPriceInclTax,
            decimal unitPriceExclTax,
            decimal priceInclTax,
            decimal priceExclTax,
            decimal unitPriceInclTaxInCustomerCurrency,
            decimal unitPriceExclTaxInCustomerCurrency,
            decimal priceInclTaxInCustomerCurrency,
            decimal priceExclTaxInCustomerCurrency,
            string attributeDescription,
            string attributesXml,
            int quantity,
            decimal discountAmountInclTax,
            decimal discountAmountExclTax,
            int downloadCount,
            bool isDownloadActivated,
            int licenseDownloadId);

        /// <summary>
        /// Updates the order product variant
        /// </summary>
        /// <param name="orderProductVariantId">The order product variant identifier</param>
        /// <param name="orderProductVariantGuid">The order product variant identifier</param>
        /// <param name="orderId">The order identifier</param>
        /// <param name="productVariantId">The product variant identifier</param>
        /// <param name="unitPriceInclTax">The unit price in primary store currency (incl tax)</param>
        /// <param name="unitPriceExclTax">The unit price in primary store currency (excl tax)</param>
        /// <param name="priceInclTax">The price in primary store currency (incl tax)</param>
        /// <param name="priceExclTax">The price in primary store currency (excl tax)</param>
        /// <param name="unitPriceInclTaxInCustomerCurrency">The unit price in primary store currency (incl tax)</param>
        /// <param name="unitPriceExclTaxInCustomerCurrency">The unit price in customer currency (excl tax)</param>
        /// <param name="priceInclTaxInCustomerCurrency">The price in primary store currency (incl tax)</param>
        /// <param name="priceExclTaxInCustomerCurrency">The price in customer currency (excl tax)</param>
        /// <param name="attributeDescription">The attribute description</param>
        /// <param name="attributesXml">The attribute description in XML format</param>
        /// <param name="quantity">The quantity</param>
        /// <param name="discountAmountInclTax">The discount amount (incl tax)</param>
        /// <param name="discountAmountExclTax">The discount amount (excl tax)</param>
        /// <param name="downloadCount">The download count</param>
        /// <param name="isDownloadActivated">The value indicating whether download is activated</param>
        /// <param name="licenseDownloadId">A license download identifier (in case this is a downloadable product)</param>
        /// <returns>Order product variant</returns>
        public abstract DBOrderProductVariant UpdateOrderProductVariant(int orderProductVariantId,
            Guid orderProductVariantGuid,
            int orderId,
            int productVariantId,
            decimal unitPriceInclTax,
            decimal unitPriceExclTax,
            decimal priceInclTax,
            decimal priceExclTax,
            decimal unitPriceInclTaxInCustomerCurrency,
            decimal unitPriceExclTaxInCustomerCurrency,
            decimal priceInclTaxInCustomerCurrency,
            decimal priceExclTaxInCustomerCurrency,
            string attributeDescription,
            string attributesXml,
            int quantity,
            decimal discountAmountInclTax,
            decimal discountAmountExclTax,
            int downloadCount,
            bool isDownloadActivated,
            int licenseDownloadId);
        
        #endregion

        #region Order notes

        /// <summary>
        /// Gets an order note
        /// </summary>
        /// <param name="orderNoteId">Order note identifier</param>
        /// <returns>Order note</returns>
        public abstract DBOrderNote GetOrderNoteById(int orderNoteId);

        /// <summary>
        /// Gets an order notes by order identifier
        /// </summary>
        /// <param name="orderId">Order identifier</param>
        /// <param name="showHidden">A value indicating whether all orders should be loaded</param>
        /// <returns>Order note collection</returns>
        public abstract DBOrderNoteCollection GetOrderNoteByOrderId(int orderId, bool showHidden);

        /// <summary>
        /// Deletes an order note
        /// </summary>
        /// <param name="orderNoteId">Order note identifier</param>
        public abstract void DeleteOrderNote(int orderNoteId);

        /// <summary>
        /// Inserts an order note
        /// </summary>
        /// <param name="orderId">The order identifier</param>
        /// <param name="note">The note</param>
        /// <param name="displayToCustomer">A value indicating whether the customer can see a note</param>
        /// <param name="createdOn">The date and time of order note creation</param>
        /// <returns>Order note</returns>
        public abstract DBOrderNote InsertOrderNote(int orderId, string note,
            bool displayToCustomer, DateTime createdOn);

        /// <summary>
        /// Updates the order note
        /// </summary>
        /// <param name="orderNoteId">The order note identifier</param>
        /// <param name="orderId">The order identifier</param>
        /// <param name="note">The note</param>
        /// <param name="displayToCustomer">A value indicating whether the customer can see a note</param>
        /// <param name="createdOn">The date and time of order note creation</param>
        /// <returns>Order note</returns>
        public abstract DBOrderNote UpdateOrderNote(int orderNoteId, int orderId,
            string note, bool displayToCustomer, DateTime createdOn);

        #endregion

        #region Order statuses

        /// <summary>
        /// Gets an order status by Id
        /// </summary>
        /// <param name="orderStatusId">Order status identifier</param>
        /// <returns>Order status</returns>
        public abstract DBOrderStatus GetOrderStatusById(int orderStatusId);

        /// <summary>
        /// Gets all order statuses
        /// </summary>
        /// <returns>Order status collection</returns>
        public abstract DBOrderStatusCollection GetAllOrderStatuses();

        #endregion

        #region Reports

        /// <summary>
        /// Gets an order report
        /// </summary>
        /// <param name="orderStatusId">Order status identifier; null to load all orders</param>
        /// <param name="paymentStatusId">Order payment status identifier; null to load all orders</param>
        /// <param name="shippingStatusId">Order shipping status identifier; null to load all orders</param>
        /// <returns>IdataReader</returns>
        public abstract IDataReader GetOrderReport(int? orderStatusId,
            int? paymentStatusId, int? shippingStatusId);

        /// <summary>
        /// Get order product variant sales report
        /// </summary>
        /// <param name="startTime">Order start time; null to load all</param>
        /// <param name="endTime">Order end time; null to load all</param>
        /// <param name="orderStatusId">Order status identifier; null to load all records</param>
        /// <param name="paymentStatusId">Order payment status identifier; null to load all records</param>
        /// <param name="billingCountryId">Billing country identifier; null to load all records</param>
        /// <returns>Result</returns>
        public abstract IDataReader OrderProductVariantReport(DateTime? startTime,
            DateTime? endTime, int? orderStatusId, int? paymentStatusId,
            int? billingCountryId);

        /// <summary>
        /// Get the bests sellers report
        /// </summary>
        /// <param name="lastDays">Last number of days</param>
        /// <param name="recordsToReturn">Number of products to return</param>
        /// <param name="orderBy">1 - order by total count, 2 - Order by total amount</param>
        /// <returns>Result</returns>
        public abstract List<DBBestSellersReportLine> BestSellersReport(int lastDays,
            int recordsToReturn, int orderBy);

        /// <summary>
        /// Get order average report
        /// </summary>
        /// <param name="orderStatusId">Order status identifier</param>
        /// <param name="startTime">Start date</param>
        /// <param name="endTime">End date</param>
        /// <returns>Result</returns>
        public abstract DBOrderAverageReportLine OrderAverageReport(int orderStatusId,
            DateTime? startTime, DateTime? endTime);


        #endregion

        #region Recurring payments

        /// <summary>
        /// Gets a recurring payment
        /// </summary>
        /// <param name="recurringPaymentId">The recurring payment identifier</param>
        /// <returns>Recurring payment</returns>
        public abstract DBRecurringPayment GetRecurringPaymentById(int recurringPaymentId);

        /// <summary>
        /// Inserts a recurring payment
        /// </summary>
        /// <param name="initialOrderId">The initial order identifier</param>
        /// <param name="cycleLength">The cycle length</param>
        /// <param name="cyclePeriod">The cycle period</param>
        /// <param name="totalCycles">The total cycles</param>
        /// <param name="startDate">The start date</param>
        /// <param name="isActive">The value indicating whether the payment is active</param>
        /// <param name="deleted">The value indicating whether the entity has been deleted</param>
        /// <param name="createdOn">The date and time of payment creation</param>
        /// <returns>Recurring payment</returns>
        public abstract DBRecurringPayment InsertRecurringPayment(int initialOrderId,
            int cycleLength, int cyclePeriod, int totalCycles,
            DateTime startDate, bool isActive, bool deleted, DateTime createdOn);

        /// <summary>
        /// Updates the recurring payment
        /// </summary>
        /// <param name="recurringPaymentId">The recurring payment identifier</param>
        /// <param name="initialOrderId">The initial order identifier</param>
        /// <param name="cycleLength">The cycle length</param>
        /// <param name="cyclePeriod">The cycle period</param>
        /// <param name="totalCycles">The total cycles</param>
        /// <param name="startDate">The start date</param>
        /// <param name="isActive">The value indicating whether the payment is active</param>
        /// <param name="deleted">The value indicating whether the entity has been deleted</param>
        /// <param name="createdOn">The date and time of payment creation</param>
        /// <returns>Recurring payment</returns>
        public abstract DBRecurringPayment UpdateRecurringPayment(int recurringPaymentId,
            int initialOrderId, int cycleLength, int cyclePeriod, int totalCycles,
            DateTime startDate, bool isActive, bool deleted, DateTime createdOn);

        /// <summary>
        /// Search recurring payments
        /// </summary>
        /// <param name="showHidden">A value indicating whether to show hidden records</param>
        /// <param name="customerId">The customer identifier; 0 to load all records</param>
        /// <param name="initialOrderId">The initial order identifier; 0 to load all records</param>
        /// <param name="initialOrderStatusId">Initial order status identifier; null to load all records</param>
        /// <returns>Recurring payment collection</returns>
        public abstract DBRecurringPaymentCollection SearchRecurringPayments(bool showHidden,
            int customerId, int initialOrderId, int? initialOrderStatusId);

        /// <summary>
        /// Deletes a recurring payment history
        /// </summary>
        /// <param name="recurringPaymentHistoryId">Recurring payment history identifier</param>
        public abstract void DeleteRecurringPaymentHistory(int recurringPaymentHistoryId);

        /// <summary>
        /// Gets a recurring payment history
        /// </summary>
        /// <param name="recurringPaymentHistoryId">The recurring payment history identifier</param>
        /// <returns>Recurring payment history</returns>
        public abstract DBRecurringPaymentHistory GetRecurringPaymentHistoryById(int recurringPaymentHistoryId);

        /// <summary>
        /// Inserts a recurring payment history
        /// </summary>
        /// <param name="recurringPaymentId">The recurring payment identifier</param>
        /// <param name="orderId">The order identifier</param>
        /// <param name="createdOn">The date and time of payment creation</param>
        /// <returns>Recurring payment history</returns>
        public abstract DBRecurringPaymentHistory InsertRecurringPaymentHistory(int recurringPaymentId,
            int orderId, DateTime createdOn);

        /// <summary>
        /// Updates the recurring payment history
        /// </summary>
        /// <param name="recurringPaymentHistoryId">The recurring payment history identifier</param>
        /// <param name="recurringPaymentId">The recurring payment identifier</param>
        /// <param name="orderId">The order identifier</param>
        /// <param name="createdOn">The date and time of payment creation</param>
        /// <returns>Recurring payment history</returns>
        public abstract DBRecurringPaymentHistory UpdateRecurringPaymentHistory(int recurringPaymentHistoryId,
            int recurringPaymentId, int orderId, DateTime createdOn);

        /// <summary>
        /// Search recurring payment history
        /// </summary>
        /// <param name="recurringPaymentId">The recurring payment identifier; 0 to load all records</param>
        /// <param name="orderId">The order identifier; 0 to load all records</param>
        /// <returns>Recurring payment history collection</returns>
        public abstract DBRecurringPaymentHistoryCollection SearchRecurringPaymentHistory(int recurringPaymentId,
            int orderId);

        #endregion

        #region Gift Cards

        /// <summary>
        /// Deletes a gift card
        /// </summary>
        /// <param name="giftCardId">Gift card identifier</param>
        public abstract void DeleteGiftCard(int giftCardId);

        /// <summary>
        /// Gets a gift card
        /// </summary>
        /// <param name="giftCardId">Gift card identifier</param>
        /// <returns>Gift card entry</returns>
        public abstract DBGiftCard GetGiftCardById(int giftCardId);

        /// <summary>
        /// Gets all gift cards
        /// </summary>
        /// <param name="orderId">Order identifier; null to load all records</param>
        /// <param name="customerId">Customer identifier; null to load all records</param>
        /// <param name="startTime">Order start time; null to load all records</param>
        /// <param name="endTime">Order end time; null to load all records</param>
        /// <param name="orderStatusId">Order status identifier; null to load all records</param>
        /// <param name="paymentStatusId">Order payment status identifier; null to load all records</param>
        /// <param name="shippingStatusId">Order shipping status identifier; null to load all records</param>
        /// <param name="isGiftCardActivated">Value indicating whether gift card is activated; null to load all records</param>
        /// <param name="giftCardCouponCode">Gift card coupon code; null or string.empty to load all records</param>
        /// <returns>Gift cards</returns>
        public abstract DBGiftCardCollection GetAllGiftCards(int? orderId,
            int? customerId, DateTime? startTime, DateTime? endTime,
            int? orderStatusId, int? paymentStatusId, int? shippingStatusId,
            bool? isGiftCardActivated, string giftCardCouponCode);

        /// <summary>
        /// Inserts a gift card
        /// </summary>
        /// <param name="purchasedOrderProductVariantId">Purchased order product variant identifier</param>
        /// <param name="amount">Amount</param>
        /// <param name="isGiftCardActivated">Value indicating whether gift card is activated</param>
        /// <param name="giftCardCouponCode">Gift card coupon code</param>
        /// <param name="recipientName">Recipient name</param>
        /// <param name="recipientEmail">Recipient email</param>
        /// <param name="senderName">Sender name</param>
        /// <param name="senderEmail">Sender email</param>
        /// <param name="message">Message</param>
        /// <param name="isRecipientNotified">Value indicating whether recipient is notified</param>
        /// <param name="createdOn">A date and time of instance creation</param>
        /// <returns>Gift card</returns>
        public abstract DBGiftCard InsertGiftCard(int purchasedOrderProductVariantId,
            decimal amount, bool isGiftCardActivated, string giftCardCouponCode,
            string recipientName, string recipientEmail,
            string senderName, string senderEmail, string message,
            bool isRecipientNotified, DateTime createdOn);

        /// <summary>
        /// Updates the gift card
        /// </summary>
        /// <param name="giftCardId">Gift card identifier</param>
        /// <param name="purchasedOrderProductVariantId">Purchased order product variant identifier</param>
        /// <param name="amount">Amount</param>
        /// <param name="isGiftCardActivated">Value indicating whether gift card is activated</param>
        /// <param name="giftCardCouponCode">Gift card coupon code</param>
        /// <param name="recipientName">Recipient name</param>
        /// <param name="recipientEmail">Recipient email</param>
        /// <param name="senderName">Sender name</param>
        /// <param name="senderEmail">Sender email</param>
        /// <param name="message">Message</param>
        /// <param name="isRecipientNotified">Value indicating whether recipient is notified</param>
        /// <param name="createdOn">A date and time of instance creation</param>
        /// <returns>Gift card</returns>
        public abstract DBGiftCard UpdateGiftCard(int giftCardId, 
            int purchasedOrderProductVariantId,
            decimal amount, bool isGiftCardActivated, string giftCardCouponCode,
            string recipientName, string recipientEmail,
            string senderName, string senderEmail, string message,
            bool isRecipientNotified, DateTime createdOn);
        
        /// <summary>
        /// Deletes a gift card usage history entry
        /// </summary>
        /// <param name="giftCardUsageHistoryId">Gift card usage history entry identifier</param>
        public abstract void DeleteGiftCardUsageHistory(int giftCardUsageHistoryId);

        /// <summary>
        /// Gets a gift card usage history entry
        /// </summary>
        /// <param name="giftCardUsageHistoryId">Gift card usage history entry identifier</param>
        /// <returns>Gift card usage history entry</returns>
        public abstract DBGiftCardUsageHistory GetGiftCardUsageHistoryById(int giftCardUsageHistoryId);

        /// <summary>
        /// Gets all gift card usage history entries
        /// </summary>
        /// <param name="giftCardId">Gift card identifier identifier; null to load all records</param>
        /// <param name="customerId">Customer identifier; null to load all records</param>
        /// <param name="orderId">Order identifier; null to load all records</param>
        /// <returns>Gift card usage history entries</returns>
        public abstract DBGiftCardUsageHistoryCollection GetAllGiftCardUsageHistoryEntries(int? giftCardId,
            int? customerId, int? orderId);

        /// <summary>
        /// Inserts a gift card usage history entry
        /// </summary>
        /// <param name="giftCardId">Gift card identifier</param>
        /// <param name="customerId">Customer identifier</param>
        /// <param name="orderId">Order identifier</param>
        /// <param name="usedValue">Used value</param>
        /// <param name="usedValueInCustomerCurrency">Used value (customer currency)</param>
        /// <param name="createdOn">A date and time of instance creation</param>
        /// <returns>Gift card usage history entry</returns>
        public abstract DBGiftCardUsageHistory InsertGiftCardUsageHistory(int giftCardId,
            int customerId, int orderId, decimal usedValue, 
            decimal usedValueInCustomerCurrency, DateTime createdOn);

        /// <summary>
        /// Updates the gift card usage history entry
        /// </summary>
        /// <param name="giftCardUsageHistoryId">Gift card usage history entry identifier</param>
        /// <param name="giftCardId">Gift card identifier</param>
        /// <param name="customerId">Customer identifier</param>
        /// <param name="orderId">Order identifier</param>
        /// <param name="usedValue">Used value</param>
        /// <param name="usedValueInCustomerCurrency">Used value (customer currency)</param>
        /// <param name="createdOn">A date and time of instance creation</param>
        /// <returns>Gift card usage history entry</returns>
        public abstract DBGiftCardUsageHistory UpdateGiftCardUsageHistory(int giftCardUsageHistoryId,
            int giftCardId, int customerId, int orderId, decimal usedValue,
            decimal usedValueInCustomerCurrency, DateTime createdOn);
        
        #endregion

        #region Reward points

        /// <summary>
        /// Deletes a reward point history entry
        /// </summary>
        /// <param name="rewardPointsHistoryId">Reward point history entry identifier</param>
        public abstract void DeleteRewardPointsHistory(int rewardPointsHistoryId);

        /// <summary>
        /// Gets a reward point history entry
        /// </summary>
        /// <param name="rewardPointsHistoryId">Reward point history entry identifier</param>
        /// <returns>Reward point history entry</returns>
        public abstract DBRewardPointsHistory GetRewardPointsHistoryById(int rewardPointsHistoryId);

        /// <summary>
        /// Gets all reward point history entries
        /// </summary>
        /// <param name="customerId">Customer identifier; null to load all records</param>
        /// <param name="orderId">Order identifier; null to load all records</param>
        /// <param name="pageSize">Page size</param>
        /// <param name="pageIndex">Page index</param>
        /// <param name="totalRecords">Total records</param>
        /// <returns>Reward point history entries</returns>
        public abstract DBRewardPointsHistoryCollection GetAllRewardPointsHistoryEntries(int? customerId,
            int? orderId, int pageSize, int pageIndex, out int totalRecords);

        /// <summary>
        /// Inserts a reward point history entry
        /// </summary>
        /// <param name="customerId">Customer identifier</param>
        /// <param name="orderId">Order identifier</param>
        /// <param name="points">Points redeemed/added</param>
        /// <param name="pointsBalance">Points balance</param>
        /// <param name="usedAmount">Used amount</param>
        /// <param name="usedAmountInCustomerCurrency">Used amount (customer currency)</param>
        /// <param name="customerCurrencyCode">Customer currency code</param>
        /// <param name="message">Customer currency code</param>
        /// <param name="createdOn">A date and time of instance creation</param>
        /// <returns>Reward point history entry</returns>
        public abstract DBRewardPointsHistory InsertRewardPointsHistory(int customerId,
            int orderId, int points, int pointsBalance, decimal usedAmount,
            decimal usedAmountInCustomerCurrency, string customerCurrencyCode,
            string message, DateTime createdOn);

        /// <summary>
        /// Updates a reward point history entry
        /// </summary>
        /// <param name="rewardPointsHistoryId">Reward point history entry identifier</param>
        /// <param name="customerId">Customer identifier</param>
        /// <param name="orderId">Order identifier</param>
        /// <param name="points">Points redeemed/added</param>
        /// <param name="pointsBalance">Points balance</param>
        /// <param name="usedAmount">Used amount</param>
        /// <param name="usedAmountInCustomerCurrency">Used amount (customer currency)</param>
        /// <param name="customerCurrencyCode">Customer currency code</param>
        /// <param name="message">Customer currency code</param>
        /// <param name="createdOn">A date and time of instance creation</param>
        /// <returns>Reward point history entry</returns>
        public abstract DBRewardPointsHistory UpdateRewardPointsHistory(int rewardPointsHistoryId,
            int customerId, int orderId, int points, int pointsBalance, decimal usedAmount,
            decimal usedAmountInCustomerCurrency, string customerCurrencyCode,
            string message, DateTime createdOn);
        
        #endregion

        #endregion
    }
}
