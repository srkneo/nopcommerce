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
using System.Configuration.Provider;
using System.Data;
using System.Data.Common;
using System.Globalization;
using System.Text;
using Microsoft.Practices.EnterpriseLibrary.Data;

namespace NopSolutions.NopCommerce.DataAccess.Orders
{
    /// <summary>
    /// Order provider for SQL Server
    /// </summary>
    public partial class SqlOrderProvider : DBOrderProvider
    {
        #region Fields
        private string _sqlConnectionString;
        #endregion

        #region Utilities
        private DBOrder GetOrderFromReader(IDataReader dataReader)
        {
            var item = new DBOrder();
            item.OrderId = NopSqlDataHelper.GetInt(dataReader, "OrderID");
            item.OrderGuid = NopSqlDataHelper.GetGuid(dataReader, "OrderGUID");
            item.CustomerId = NopSqlDataHelper.GetInt(dataReader, "CustomerID");
            item.CustomerLanguageId = NopSqlDataHelper.GetInt(dataReader, "CustomerLanguageID");
            item.CustomerTaxDisplayTypeId = NopSqlDataHelper.GetInt(dataReader, "CustomerTaxDisplayTypeID");
            item.CustomerIP = NopSqlDataHelper.GetString(dataReader, "CustomerIP");
            item.OrderSubtotalInclTax = NopSqlDataHelper.GetDecimal(dataReader, "OrderSubtotalInclTax");
            item.OrderSubtotalExclTax = NopSqlDataHelper.GetDecimal(dataReader, "OrderSubtotalExclTax");
            item.OrderShippingInclTax = NopSqlDataHelper.GetDecimal(dataReader, "OrderShippingInclTax");
            item.OrderShippingExclTax = NopSqlDataHelper.GetDecimal(dataReader, "OrderShippingExclTax");
            item.PaymentMethodAdditionalFeeInclTax = NopSqlDataHelper.GetDecimal(dataReader, "PaymentMethodAdditionalFeeInclTax");
            item.PaymentMethodAdditionalFeeExclTax = NopSqlDataHelper.GetDecimal(dataReader, "PaymentMethodAdditionalFeeExclTax");
            item.OrderTax = NopSqlDataHelper.GetDecimal(dataReader, "OrderTax");
            item.OrderTotal = NopSqlDataHelper.GetDecimal(dataReader, "OrderTotal");
            item.OrderDiscount = NopSqlDataHelper.GetDecimal(dataReader, "OrderDiscount");
            item.OrderSubtotalInclTaxInCustomerCurrency = NopSqlDataHelper.GetDecimal(dataReader, "OrderSubtotalInclTaxInCustomerCurrency");
            item.OrderSubtotalExclTaxInCustomerCurrency = NopSqlDataHelper.GetDecimal(dataReader, "OrderSubtotalExclTaxInCustomerCurrency");
            item.OrderShippingInclTaxInCustomerCurrency = NopSqlDataHelper.GetDecimal(dataReader, "OrderShippingInclTaxInCustomerCurrency");
            item.OrderShippingExclTaxInCustomerCurrency = NopSqlDataHelper.GetDecimal(dataReader, "OrderShippingExclTaxInCustomerCurrency");
            item.PaymentMethodAdditionalFeeInclTaxInCustomerCurrency = NopSqlDataHelper.GetDecimal(dataReader, "PaymentMethodAdditionalFeeInclTaxInCustomerCurrency");
            item.PaymentMethodAdditionalFeeExclTaxInCustomerCurrency = NopSqlDataHelper.GetDecimal(dataReader, "PaymentMethodAdditionalFeeExclTaxInCustomerCurrency");
            item.OrderTaxInCustomerCurrency = NopSqlDataHelper.GetDecimal(dataReader, "OrderTaxInCustomerCurrency");
            item.OrderTotalInCustomerCurrency = NopSqlDataHelper.GetDecimal(dataReader, "OrderTotalInCustomerCurrency");
            item.OrderDiscountInCustomerCurrency = NopSqlDataHelper.GetDecimal(dataReader, "OrderDiscountInCustomerCurrency");
            item.CheckoutAttributeDescription = NopSqlDataHelper.GetString(dataReader, "CheckoutAttributeDescription");
            item.CheckoutAttributesXml = NopSqlDataHelper.GetString(dataReader, "CheckoutAttributesXML");
            item.CustomerCurrencyCode = NopSqlDataHelper.GetString(dataReader, "CustomerCurrencyCode");
            item.OrderWeight = NopSqlDataHelper.GetDecimal(dataReader, "OrderWeight");
            item.AffiliateId = NopSqlDataHelper.GetInt(dataReader, "AffiliateID");
            item.OrderStatusId = NopSqlDataHelper.GetInt(dataReader, "OrderStatusID");
            item.AllowStoringCreditCardNumber = NopSqlDataHelper.GetBoolean(dataReader, "AllowStoringCreditCardNumber");
            item.CardType = NopSqlDataHelper.GetString(dataReader, "CardType");
            item.CardName = NopSqlDataHelper.GetString(dataReader, "CardName");
            item.CardNumber = NopSqlDataHelper.GetString(dataReader, "CardNumber");
            item.MaskedCreditCardNumber = NopSqlDataHelper.GetString(dataReader, "MaskedCreditCardNumber");
            item.CardCvv2 = NopSqlDataHelper.GetString(dataReader, "CardCVV2");
            item.CardExpirationMonth = NopSqlDataHelper.GetString(dataReader, "CardExpirationMonth");
            item.CardExpirationYear = NopSqlDataHelper.GetString(dataReader, "CardExpirationYear");
            item.PaymentMethodId = NopSqlDataHelper.GetInt(dataReader, "PaymentMethodID");
            item.PaymentMethodName = NopSqlDataHelper.GetString(dataReader, "PaymentMethodName");
            item.AuthorizationTransactionId = NopSqlDataHelper.GetString(dataReader, "AuthorizationTransactionID");
            item.AuthorizationTransactionCode = NopSqlDataHelper.GetString(dataReader, "AuthorizationTransactionCode");
            item.AuthorizationTransactionResult = NopSqlDataHelper.GetString(dataReader, "AuthorizationTransactionResult");
            item.CaptureTransactionId = NopSqlDataHelper.GetString(dataReader, "CaptureTransactionID");
            item.CaptureTransactionResult = NopSqlDataHelper.GetString(dataReader, "CaptureTransactionResult");
            item.SubscriptionTransactionId = NopSqlDataHelper.GetString(dataReader, "SubscriptionTransactionID");
            item.PurchaseOrderNumber = NopSqlDataHelper.GetString(dataReader, "PurchaseOrderNumber");
            item.PaymentStatusId = NopSqlDataHelper.GetInt(dataReader, "PaymentStatusID");
            item.PaidDate = NopSqlDataHelper.GetNullableUtcDateTime(dataReader, "PaidDate");
            item.BillingFirstName = NopSqlDataHelper.GetString(dataReader, "BillingFirstName");
            item.BillingLastName = NopSqlDataHelper.GetString(dataReader, "BillingLastName");
            item.BillingPhoneNumber = NopSqlDataHelper.GetString(dataReader, "BillingPhoneNumber");
            item.BillingEmail = NopSqlDataHelper.GetString(dataReader, "BillingEmail");
            item.BillingFaxNumber = NopSqlDataHelper.GetString(dataReader, "BillingFaxNumber");
            item.BillingCompany = NopSqlDataHelper.GetString(dataReader, "BillingCompany");
            item.BillingAddress1 = NopSqlDataHelper.GetString(dataReader, "BillingAddress1");
            item.BillingAddress2 = NopSqlDataHelper.GetString(dataReader, "BillingAddress2");
            item.BillingCity = NopSqlDataHelper.GetString(dataReader, "BillingCity");
            item.BillingStateProvince = NopSqlDataHelper.GetString(dataReader, "BillingStateProvince");
            item.BillingStateProvinceId = NopSqlDataHelper.GetInt(dataReader, "BillingStateProvinceID");
            item.BillingZipPostalCode = NopSqlDataHelper.GetString(dataReader, "BillingZipPostalCode");
            item.BillingCountry = NopSqlDataHelper.GetString(dataReader, "BillingCountry");
            item.BillingCountryId = NopSqlDataHelper.GetInt(dataReader, "BillingCountryID");
            item.ShippingStatusId = NopSqlDataHelper.GetInt(dataReader, "ShippingStatusID");
            item.ShippingFirstName = NopSqlDataHelper.GetString(dataReader, "ShippingFirstName");
            item.ShippingLastName = NopSqlDataHelper.GetString(dataReader, "ShippingLastName");
            item.ShippingPhoneNumber = NopSqlDataHelper.GetString(dataReader, "ShippingPhoneNumber");
            item.ShippingEmail = NopSqlDataHelper.GetString(dataReader, "ShippingEmail");
            item.ShippingFaxNumber = NopSqlDataHelper.GetString(dataReader, "ShippingFaxNumber");
            item.ShippingCompany = NopSqlDataHelper.GetString(dataReader, "ShippingCompany");
            item.ShippingAddress1 = NopSqlDataHelper.GetString(dataReader, "ShippingAddress1");
            item.ShippingAddress2 = NopSqlDataHelper.GetString(dataReader, "ShippingAddress2");
            item.ShippingCity = NopSqlDataHelper.GetString(dataReader, "ShippingCity");
            item.ShippingStateProvince = NopSqlDataHelper.GetString(dataReader, "ShippingStateProvince");
            item.ShippingStateProvinceId = NopSqlDataHelper.GetInt(dataReader, "ShippingStateProvinceID");
            item.ShippingZipPostalCode = NopSqlDataHelper.GetString(dataReader, "ShippingZipPostalCode");
            item.ShippingCountry = NopSqlDataHelper.GetString(dataReader, "ShippingCountry");
            item.ShippingCountryId = NopSqlDataHelper.GetInt(dataReader, "ShippingCountryID");
            item.ShippingMethod = NopSqlDataHelper.GetString(dataReader, "ShippingMethod");
            item.ShippingRateComputationMethodId = NopSqlDataHelper.GetInt(dataReader, "ShippingRateComputationMethodID");
            item.ShippedDate = NopSqlDataHelper.GetNullableUtcDateTime(dataReader, "ShippedDate");
            item.DeliveryDate = NopSqlDataHelper.GetNullableUtcDateTime(dataReader, "DeliveryDate");
            item.TrackingNumber = NopSqlDataHelper.GetString(dataReader, "TrackingNumber");
            item.Deleted = NopSqlDataHelper.GetBoolean(dataReader, "Deleted");
            item.CreatedOn = NopSqlDataHelper.GetUtcDateTime(dataReader, "CreatedOn");
            return item;
        }

        private DBOrderProductVariant GetOrderProductVariantFromReader(IDataReader dataReader)
        {
            var item = new DBOrderProductVariant();
            item.OrderProductVariantId = NopSqlDataHelper.GetInt(dataReader, "OrderProductVariantID");
            item.OrderProductVariantGuid = NopSqlDataHelper.GetGuid(dataReader, "OrderProductVariantGUID");
            item.OrderId = NopSqlDataHelper.GetInt(dataReader, "OrderID");
            item.ProductVariantId = NopSqlDataHelper.GetInt(dataReader, "ProductVariantID");
            item.UnitPriceInclTax = NopSqlDataHelper.GetDecimal(dataReader, "UnitPriceInclTax");
            item.UnitPriceExclTax = NopSqlDataHelper.GetDecimal(dataReader, "UnitPriceExclTax");
            item.PriceInclTax = NopSqlDataHelper.GetDecimal(dataReader, "PriceInclTax");
            item.PriceExclTax = NopSqlDataHelper.GetDecimal(dataReader, "PriceExclTax");
            item.UnitPriceInclTaxInCustomerCurrency = NopSqlDataHelper.GetDecimal(dataReader, "UnitPriceInclTaxInCustomerCurrency");
            item.UnitPriceExclTaxInCustomerCurrency = NopSqlDataHelper.GetDecimal(dataReader, "UnitPriceExclTaxInCustomerCurrency");
            item.PriceInclTaxInCustomerCurrency = NopSqlDataHelper.GetDecimal(dataReader, "PriceInclTaxInCustomerCurrency");
            item.PriceExclTaxInCustomerCurrency = NopSqlDataHelper.GetDecimal(dataReader, "PriceExclTaxInCustomerCurrency");
            item.AttributeDescription = NopSqlDataHelper.GetString(dataReader, "AttributeDescription");
            item.AttributesXml = NopSqlDataHelper.GetString(dataReader, "AttributesXML");
            item.Quantity = NopSqlDataHelper.GetInt(dataReader, "Quantity");
            item.DiscountAmountInclTax = NopSqlDataHelper.GetDecimal(dataReader, "DiscountAmountInclTax");
            item.DiscountAmountExclTax = NopSqlDataHelper.GetDecimal(dataReader, "DiscountAmountExclTax");
            item.DownloadCount = NopSqlDataHelper.GetInt(dataReader, "DownloadCount");
            item.IsDownloadActivated = NopSqlDataHelper.GetBoolean(dataReader, "IsDownloadActivated");
            item.LicenseDownloadId = NopSqlDataHelper.GetInt(dataReader, "LicenseDownloadID");
            return item;
        }

        private DBOrderAverageReportLine GetOrderAverageReportLineFromReader(IDataReader dataReader)
        {
            var item = new DBOrderAverageReportLine();
            item.SumOrders = NopSqlDataHelper.GetDecimal(dataReader, "SumOrders");
            item.CountOrders = NopSqlDataHelper.GetDecimal(dataReader, "CountOrders");
            return item;
        }

        private DBBestSellersReportLine GetBestSellersReportLineFromReader(IDataReader dataReader)
        {
            var item = new DBBestSellersReportLine();
            item.ProductVariantId = NopSqlDataHelper.GetInt(dataReader, "ProductVariantID");
            item.SalesTotalCount = NopSqlDataHelper.GetDecimal(dataReader, "SalesTotalCount");
            item.SalesTotalAmount = NopSqlDataHelper.GetDecimal(dataReader, "SalesTotalAmount");
            return item;
        }

        private DBRecurringPayment GetRecurringPaymentFromReader(IDataReader dataReader)
        {
            var item = new DBRecurringPayment();
            item.RecurringPaymentId = NopSqlDataHelper.GetInt(dataReader, "RecurringPaymentID");
            item.InitialOrderId = NopSqlDataHelper.GetInt(dataReader, "InitialOrderID");
            item.CycleLength = NopSqlDataHelper.GetInt(dataReader, "CycleLength");
            item.CyclePeriod = NopSqlDataHelper.GetInt(dataReader, "CyclePeriod");
            item.TotalCycles = NopSqlDataHelper.GetInt(dataReader, "TotalCycles");
            item.StartDate = NopSqlDataHelper.GetUtcDateTime(dataReader, "StartDate");
            item.IsActive = NopSqlDataHelper.GetBoolean(dataReader, "IsActive");
            item.Deleted = NopSqlDataHelper.GetBoolean(dataReader, "Deleted");
            item.CreatedOn = NopSqlDataHelper.GetUtcDateTime(dataReader, "CreatedOn");
            return item;
        }

        private DBRecurringPaymentHistory GetRecurringPaymentHistoryFromReader(IDataReader dataReader)
        {
            var item = new DBRecurringPaymentHistory();
            item.RecurringPaymentHistoryId = NopSqlDataHelper.GetInt(dataReader, "RecurringPaymentHistoryID");
            item.RecurringPaymentId = NopSqlDataHelper.GetInt(dataReader, "RecurringPaymentID");
            item.OrderId = NopSqlDataHelper.GetInt(dataReader, "OrderID");
            item.CreatedOn = NopSqlDataHelper.GetUtcDateTime(dataReader, "CreatedOn");
            return item;
        }

        private DBGiftCard GetGiftCardFromReader(IDataReader dataReader)
        {
            var item = new DBGiftCard();
            item.GiftCardId = NopSqlDataHelper.GetInt(dataReader, "GiftCardID");
            item.PurchasedOrderProductVariantId = NopSqlDataHelper.GetInt(dataReader, "PurchasedOrderProductVariantID");
            item.Amount = NopSqlDataHelper.GetDecimal(dataReader, "Amount");
            item.IsGiftCardActivated = NopSqlDataHelper.GetBoolean(dataReader, "IsGiftCardActivated");
            item.GiftCardCouponCode = NopSqlDataHelper.GetString(dataReader, "GiftCardCouponCode");
            item.RecipientName = NopSqlDataHelper.GetString(dataReader, "RecipientName");
            item.RecipientEmail = NopSqlDataHelper.GetString(dataReader, "RecipientEmail");
            item.SenderName = NopSqlDataHelper.GetString(dataReader, "SenderName");
            item.SenderEmail = NopSqlDataHelper.GetString(dataReader, "SenderEmail");
            item.Message = NopSqlDataHelper.GetString(dataReader, "Message");
            item.IsRecipientNotified = NopSqlDataHelper.GetBoolean(dataReader, "IsRecipientNotified");
            item.CreatedOn = NopSqlDataHelper.GetUtcDateTime(dataReader, "CreatedOn");
            return item;
        }

        private DBGiftCardUsageHistory GetGiftCardUsageHistoryFromReader(IDataReader dataReader)
        {
            var item = new DBGiftCardUsageHistory();
            item.GiftCardUsageHistoryId = NopSqlDataHelper.GetInt(dataReader, "GiftCardUsageHistoryID");
            item.GiftCardId = NopSqlDataHelper.GetInt(dataReader, "GiftCardID");
            item.CustomerId = NopSqlDataHelper.GetInt(dataReader, "CustomerID");
            item.OrderId = NopSqlDataHelper.GetInt(dataReader, "OrderID");
            item.UsedValue = NopSqlDataHelper.GetDecimal(dataReader, "UsedValue");
            item.UsedValueInCustomerCurrency = NopSqlDataHelper.GetDecimal(dataReader, "UsedValueInCustomerCurrency");
            item.CreatedOn = NopSqlDataHelper.GetUtcDateTime(dataReader, "CreatedOn");
            return item;
        }

        private DBRewardPointsHistory GetRewardPointsHistoryFromReader(IDataReader dataReader)
        {
            var item = new DBRewardPointsHistory();
            item.RewardPointsHistoryId = NopSqlDataHelper.GetInt(dataReader, "RewardPointsHistoryId");
            item.CustomerId = NopSqlDataHelper.GetInt(dataReader, "CustomerID");
            item.OrderId = NopSqlDataHelper.GetInt(dataReader, "OrderID");
            item.Points = NopSqlDataHelper.GetInt(dataReader, "Points");
            item.PointsBalance = NopSqlDataHelper.GetInt(dataReader, "PointsBalance");
            item.UsedAmount = NopSqlDataHelper.GetDecimal(dataReader, "UsedAmount");
            item.UsedAmountInCustomerCurrency = NopSqlDataHelper.GetDecimal(dataReader, "UsedAmountInCustomerCurrency");
            item.CustomerCurrencyCode = NopSqlDataHelper.GetString(dataReader, "CustomerCurrencyCode");
            item.Message = NopSqlDataHelper.GetString(dataReader, "Message");
            item.CreatedOn = NopSqlDataHelper.GetUtcDateTime(dataReader, "CreatedOn");
            return item;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Initializes the provider with the property values specified in the application's configuration file. This method is not intended to be used directly from your code
        /// </summary>
        /// <param name="name">The name of the provider instance to initialize</param>
        /// <param name="config">A NameValueCollection that contains the names and values of configuration options for the provider.</param>
        public override void Initialize(string name, NameValueCollection config)
        {
            if (config == null)
            {
                throw new ArgumentNullException("config");
            }

            base.Initialize(name, config);

            string connectionStringName = config["connectionStringName"];
            if (String.IsNullOrEmpty(connectionStringName))
                throw new ProviderException("Connection name not specified");
            this._sqlConnectionString = NopSqlDataHelper.GetConnectionString(connectionStringName);
            if ((this._sqlConnectionString == null) || (this._sqlConnectionString.Length < 1))
            {
                throw new ProviderException(string.Format("Connection string not found. {0}", connectionStringName));
            }
            config.Remove("connectionStringName");

            if (config.Count > 0)
            {
                string key = config.GetKey(0);
                if (!string.IsNullOrEmpty(key))
                {
                    throw new ProviderException(string.Format("Provider unrecognized attribute. {0}", new object[] { key }));
                }
            }
        }

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
        public override DBOrderCollection SearchOrders(DateTime? startTime,
            DateTime? endTime, string customerEmail, int? orderStatusId,
            int? paymentStatusId, int? shippingStatusId)
        {
            var result = new DBOrderCollection();

            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_OrderSearch");
            if (startTime.HasValue)
                db.AddInParameter(dbCommand, "StartTime", DbType.DateTime, startTime.Value);
            else
                db.AddInParameter(dbCommand, "StartTime", DbType.DateTime, null);
            if (endTime.HasValue)
                db.AddInParameter(dbCommand, "EndTime", DbType.DateTime, endTime.Value);
            else
                db.AddInParameter(dbCommand, "EndTime", DbType.DateTime, null);
            db.AddInParameter(dbCommand, "CustomerEmail", DbType.String, customerEmail);
            if (orderStatusId.HasValue)
                db.AddInParameter(dbCommand, "OrderStatusID", DbType.Int32, orderStatusId.Value);
            else
                db.AddInParameter(dbCommand, "OrderStatusID", DbType.Int32, null);
            if (paymentStatusId.HasValue)
                db.AddInParameter(dbCommand, "PaymentStatusID", DbType.Int32, paymentStatusId.Value);
            else
                db.AddInParameter(dbCommand, "PaymentStatusID", DbType.Int32, null);
            if (shippingStatusId.HasValue)
                db.AddInParameter(dbCommand, "ShippingStatusID", DbType.Int32, shippingStatusId);
            else
                db.AddInParameter(dbCommand, "ShippingStatusID", DbType.Int32, null);

            using (IDataReader dataReader = db.ExecuteReader(dbCommand))
            {
                while (dataReader.Read())
                {
                    var item = GetOrderFromReader(dataReader);
                    result.Add(item);
                }
            }
            return result;
        }

        /// <summary>
        /// Get order product variant sales report
        /// </summary>
        /// <param name="startTime">Order start time; null to load all</param>
        /// <param name="endTime">Order end time; null to load all</param>
        /// <param name="orderStatusId">Order status identifier; null to load all records</param>
        /// <param name="paymentStatusId">Order payment status identifier; null to load all records</param>
        /// <param name="billingCountryId">Billing country identifier; null to load all records</param>
        /// <returns>Result</returns>
        public override IDataReader OrderProductVariantReport(DateTime? startTime,
            DateTime? endTime, int? orderStatusId, int? paymentStatusId,
            int? billingCountryId)
        {
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_OrderProductVariantReport");
            if (startTime.HasValue)
                db.AddInParameter(dbCommand, "StartTime", DbType.DateTime, startTime.Value);
            else
                db.AddInParameter(dbCommand, "StartTime", DbType.DateTime, null);
            if (endTime.HasValue)
                db.AddInParameter(dbCommand, "EndTime", DbType.DateTime, endTime.Value);
            else
                db.AddInParameter(dbCommand, "EndTime", DbType.DateTime, null);
            if (orderStatusId.HasValue)
                db.AddInParameter(dbCommand, "OrderStatusID", DbType.Int32, orderStatusId.Value);
            else
                db.AddInParameter(dbCommand, "OrderStatusID", DbType.Int32, null);
            if (paymentStatusId.HasValue)
                db.AddInParameter(dbCommand, "PaymentStatusID", DbType.Int32, paymentStatusId.Value);
            else
                db.AddInParameter(dbCommand, "PaymentStatusID", DbType.Int32, null);
            if (billingCountryId.HasValue)
                db.AddInParameter(dbCommand, "BillingCountryId", DbType.Int32, billingCountryId.Value);
            else
                db.AddInParameter(dbCommand, "BillingCountryId", DbType.Int32, null); 
            return db.ExecuteReader(dbCommand);
        }

        /// <summary>
        /// Get the bests sellers report
        /// </summary>
        /// <param name="lastDays">Last number of days</param>
        /// <param name="recordsToReturn">Number of products to return</param>
        /// <param name="orderBy">1 - order by total count, 2 - Order by total amount</param>
        /// <returns>Result</returns>
        public override List<DBBestSellersReportLine> BestSellersReport(int lastDays,
            int recordsToReturn, int orderBy)
        {
            var result = new List<DBBestSellersReportLine>();
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_SalesBestSellersReport");
            db.AddInParameter(dbCommand, "LastDays", DbType.Int32, lastDays);
            db.AddInParameter(dbCommand, "RecordsToReturn", DbType.Int32, recordsToReturn);
            db.AddInParameter(dbCommand, "OrderBy", DbType.Int32, orderBy);
            using (IDataReader dataReader = db.ExecuteReader(dbCommand))
            {
                while (dataReader.Read())
                {
                    var item = GetBestSellersReportLineFromReader(dataReader);
                    result.Add(item);
                }
            }
            return result;
        }

        /// <summary>
        /// Get order average report
        /// </summary>
        /// <param name="orderStatusId">Order status identifier</param>
        /// <param name="startTime">Start date</param>
        /// <param name="endTime">End date</param>
        /// <returns>Result</returns>
        public override DBOrderAverageReportLine OrderAverageReport(int orderStatusId,
            DateTime? startTime, DateTime? endTime)
        {
            DBOrderAverageReportLine item = null;
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_OrderAverageReport");
            db.AddInParameter(dbCommand, "OrderStatusID", DbType.Int32, orderStatusId);
            if (startTime.HasValue)
                db.AddInParameter(dbCommand, "StartTime", DbType.DateTime, startTime.Value);
            else
                db.AddInParameter(dbCommand, "StartTime", DbType.DateTime, null);
            if (endTime.HasValue)
                db.AddInParameter(dbCommand, "EndTime", DbType.DateTime, endTime.Value);
            else
                db.AddInParameter(dbCommand, "EndTime", DbType.DateTime, null);
            using (IDataReader dataReader = db.ExecuteReader(dbCommand))
            {
                if (dataReader.Read())
                {
                    item = GetOrderAverageReportLineFromReader(dataReader);
                }
            }
            return item;
        }

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
        public override DBOrderProductVariantCollection GetAllOrderProductVariants(int? orderId,
            int? customerId, DateTime? startTime, DateTime? endTime,
            int? orderStatusId, int? paymentStatusId, int? shippingStatusId,
            bool loadDownloableProductsOnly)
        {
            var result = new DBOrderProductVariantCollection();
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_OrderProductVariantLoadAll");
            if (orderId.HasValue)
                db.AddInParameter(dbCommand, "OrderID", DbType.Int32, orderId.Value);
            else
                db.AddInParameter(dbCommand, "OrderID", DbType.Int32, null);
            if (customerId.HasValue)
                db.AddInParameter(dbCommand, "CustomerID", DbType.Int32, customerId.Value);
            else
                db.AddInParameter(dbCommand, "CustomerID", DbType.Int32, null);
            if (startTime.HasValue)
                db.AddInParameter(dbCommand, "StartTime", DbType.DateTime, startTime.Value);
            else
                db.AddInParameter(dbCommand, "StartTime", DbType.DateTime, null);
            if (endTime.HasValue)
                db.AddInParameter(dbCommand, "EndTime", DbType.DateTime, endTime.Value);
            else
                db.AddInParameter(dbCommand, "EndTime", DbType.DateTime, null);            
            if (orderStatusId.HasValue)
                db.AddInParameter(dbCommand, "OrderStatusID", DbType.Int32, orderStatusId.Value);
            else
                db.AddInParameter(dbCommand, "OrderStatusID", DbType.Int32, null);
            if (paymentStatusId.HasValue)
                db.AddInParameter(dbCommand, "PaymentStatusID", DbType.Int32, paymentStatusId.Value);
            else
                db.AddInParameter(dbCommand, "PaymentStatusID", DbType.Int32, null);
            if (shippingStatusId.HasValue)
                db.AddInParameter(dbCommand, "ShippingStatusID", DbType.Int32, shippingStatusId);
            else
                db.AddInParameter(dbCommand, "ShippingStatusID", DbType.Int32, null);
            db.AddInParameter(dbCommand, "LoadDownloableProductsOnly", DbType.Boolean, loadDownloableProductsOnly);
            
            using (IDataReader dataReader = db.ExecuteReader(dbCommand))
            {
                while (dataReader.Read())
                {
                    var item = GetOrderProductVariantFromReader(dataReader);
                    result.Add(item);
                }
            }

            return result;
        }

        /// <summary>
        /// Gets an order report
        /// </summary>
        /// <param name="orderStatusId">Order status identifier; null to load all orders</param>
        /// <param name="paymentStatusId">Order payment status identifier; null to load all orders</param>
        /// <param name="shippingStatusId">Order shipping status identifier; null to load all orders</param>
        /// <returns>IdataReader</returns>
        public override IDataReader GetOrderReport(int? orderStatusId,
            int? paymentStatusId, int? shippingStatusId)
        {
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_OrderIncompleteReport");
            if (orderStatusId.HasValue)
                db.AddInParameter(dbCommand, "OrderStatusID", DbType.Int32, orderStatusId.Value);
            else
                db.AddInParameter(dbCommand, "OrderStatusID", DbType.Int32, null);
            if (paymentStatusId.HasValue)
                db.AddInParameter(dbCommand, "PaymentStatusID", DbType.Int32, paymentStatusId.Value);
            else
                db.AddInParameter(dbCommand, "PaymentStatusID", DbType.Int32, null);

            if (shippingStatusId.HasValue)
                db.AddInParameter(dbCommand, "ShippingStatusID", DbType.Int32, shippingStatusId);
            else
                db.AddInParameter(dbCommand, "ShippingStatusID", DbType.Int32, null);

            return db.ExecuteReader(dbCommand);
        }

        /// <summary>
        /// Search recurring payments
        /// </summary>
        /// <param name="showHidden">A value indicating whether to show hidden records</param>
        /// <param name="customerId">The customer identifier; 0 to load all records</param>
        /// <param name="initialOrderId">The initial order identifier; 0 to load all records</param>
        /// <param name="initialOrderStatusId">Initial order status identifier; null to load all records</param>
        /// <returns>Recurring payment collection</returns>
        public override DBRecurringPaymentCollection SearchRecurringPayments(bool showHidden,
            int customerId, int initialOrderId, int? initialOrderStatusId)
        {
            var result = new DBRecurringPaymentCollection();

            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_RecurringPaymentLoadAll");
            db.AddInParameter(dbCommand, "ShowHidden", DbType.Boolean, showHidden);
            db.AddInParameter(dbCommand, "CustomerID", DbType.Int32, customerId);
            db.AddInParameter(dbCommand, "InitialOrderID", DbType.Int32, initialOrderId);
            if (initialOrderStatusId.HasValue)
                db.AddInParameter(dbCommand, "InitialOrderStatusID", DbType.Int32, initialOrderStatusId.Value);
            else
                db.AddInParameter(dbCommand, "InitialOrderStatusID", DbType.Int32, null);

            using (IDataReader dataReader = db.ExecuteReader(dbCommand))
            {
                while (dataReader.Read())
                {
                    var item = GetRecurringPaymentFromReader(dataReader);
                    result.Add(item);
                }
            }
            return result;
        }

        /// <summary>
        /// Search recurring payment history
        /// </summary>
        /// <param name="recurringPaymentId">The recurring payment identifier; 0 to load all records</param>
        /// <param name="orderId">The order identifier; 0 to load all records</param>
        /// <returns>Recurring payment history collection</returns>
        public override DBRecurringPaymentHistoryCollection SearchRecurringPaymentHistory(int recurringPaymentId,
            int orderId)
        {
            var result = new DBRecurringPaymentHistoryCollection();

            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_RecurringPaymentHistoryLoadAll");
            db.AddInParameter(dbCommand, "RecurringPaymentID", DbType.Int32, recurringPaymentId);
            db.AddInParameter(dbCommand, "OrderID", DbType.Int32, orderId);

            using (IDataReader dataReader = db.ExecuteReader(dbCommand))
            {
                while (dataReader.Read())
                {
                    var item = GetRecurringPaymentHistoryFromReader(dataReader);
                    result.Add(item);
                }
            }
            return result;
        }

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
        public override DBGiftCardCollection GetAllGiftCards(int? orderId,
            int? customerId, DateTime? startTime, DateTime? endTime,
            int? orderStatusId, int? paymentStatusId, int? shippingStatusId,
            bool? isGiftCardActivated, string giftCardCouponCode)
        {
            var result = new DBGiftCardCollection();
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_GiftCardLoadAll");
            if (orderId.HasValue)
                db.AddInParameter(dbCommand, "OrderID", DbType.Int32, orderId.Value);
            else
                db.AddInParameter(dbCommand, "OrderID", DbType.Int32, null);
            if (customerId.HasValue)
                db.AddInParameter(dbCommand, "CustomerID", DbType.Int32, customerId.Value);
            else
                db.AddInParameter(dbCommand, "CustomerID", DbType.Int32, null);
            if (startTime.HasValue)
                db.AddInParameter(dbCommand, "StartTime", DbType.DateTime, startTime.Value);
            else
                db.AddInParameter(dbCommand, "StartTime", DbType.DateTime, null);
            if (endTime.HasValue)
                db.AddInParameter(dbCommand, "EndTime", DbType.DateTime, endTime.Value);
            else
                db.AddInParameter(dbCommand, "EndTime", DbType.DateTime, null);
            if (orderStatusId.HasValue)
                db.AddInParameter(dbCommand, "OrderStatusID", DbType.Int32, orderStatusId.Value);
            else
                db.AddInParameter(dbCommand, "OrderStatusID", DbType.Int32, null);
            if (paymentStatusId.HasValue)
                db.AddInParameter(dbCommand, "PaymentStatusID", DbType.Int32, paymentStatusId.Value);
            else
                db.AddInParameter(dbCommand, "PaymentStatusID", DbType.Int32, null);
            if (shippingStatusId.HasValue)
                db.AddInParameter(dbCommand, "ShippingStatusID", DbType.Int32, shippingStatusId);
            else
                db.AddInParameter(dbCommand, "ShippingStatusID", DbType.Int32, null);
            if (isGiftCardActivated.HasValue)
                db.AddInParameter(dbCommand, "IsGiftCardActivated", DbType.Boolean, isGiftCardActivated);
            else
                db.AddInParameter(dbCommand, "IsGiftCardActivated", DbType.Boolean, null);
            if (!String.IsNullOrEmpty(giftCardCouponCode))
                db.AddInParameter(dbCommand, "GiftCardCouponCode", DbType.String, giftCardCouponCode);
            else
                db.AddInParameter(dbCommand, "GiftCardCouponCode", DbType.String, null);

            using (IDataReader dataReader = db.ExecuteReader(dbCommand))
            {
                while (dataReader.Read())
                {
                    var item = GetGiftCardFromReader(dataReader);
                    result.Add(item);
                }
            }

            return result;
        }

        /// <summary>
        /// Gets all gift card usage history entries
        /// </summary>
        /// <param name="giftCardId">Gift card identifier identifier; null to load all records</param>
        /// <param name="customerId">Customer identifier; null to load all records</param>
        /// <param name="orderId">Order identifier; null to load all records</param>
        /// <returns>Gift card usage history entries</returns>
        public override DBGiftCardUsageHistoryCollection GetAllGiftCardUsageHistoryEntries(int? giftCardId,
            int? customerId, int? orderId)
        {
            var result = new DBGiftCardUsageHistoryCollection();
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_GiftCardUsageHistoryLoadAll");
            if (giftCardId.HasValue)
                db.AddInParameter(dbCommand, "GiftCardID", DbType.Int32, giftCardId.Value);
            else
                db.AddInParameter(dbCommand, "GiftCardID", DbType.Int32, null);
            if (customerId.HasValue)
                db.AddInParameter(dbCommand, "CustomerID", DbType.Int32, customerId.Value);
            else
                db.AddInParameter(dbCommand, "CustomerID", DbType.Int32, null);
            if (orderId.HasValue)
                db.AddInParameter(dbCommand, "OrderID", DbType.Int32, orderId.Value);
            else
                db.AddInParameter(dbCommand, "OrderID", DbType.Int32, null);
            using (IDataReader dataReader = db.ExecuteReader(dbCommand))
            {
                while (dataReader.Read())
                {
                    var item = GetGiftCardUsageHistoryFromReader(dataReader);
                    result.Add(item);
                }
            }

            return result;
        }

        /// <summary>
        /// Gets all reward point history entries
        /// </summary>
        /// <param name="customerId">Customer identifier; null to load all records</param>
        /// <param name="orderId">Order identifier; null to load all records</param>
        /// <param name="pageSize">Page size</param>
        /// <param name="pageIndex">Page index</param>
        /// <param name="totalRecords">Total records</param>
        /// <returns>Reward point history entries</returns>
        public override DBRewardPointsHistoryCollection GetAllRewardPointsHistoryEntries(int? customerId,
            int? orderId, int pageSize, int pageIndex, out int totalRecords)
        {
            totalRecords = 0;
            var result = new DBRewardPointsHistoryCollection();
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_RewardPointsHistoryLoadAll");
            db.AddInParameter(dbCommand, "CustomerId", DbType.Int32, customerId);
            db.AddInParameter(dbCommand, "OrderId", DbType.Int32, orderId);
            db.AddInParameter(dbCommand, "PageSize", DbType.Int32, pageSize);
            db.AddInParameter(dbCommand, "PageIndex", DbType.Int32, pageIndex);
            db.AddOutParameter(dbCommand, "TotalRecords", DbType.Int32, 0);
            using (IDataReader dataReader = db.ExecuteReader(dbCommand))
            {
                while (dataReader.Read())
                {
                    var item = GetRewardPointsHistoryFromReader(dataReader);
                    result.Add(item);
                }
            }
            totalRecords = Convert.ToInt32(db.GetParameterValue(dbCommand, "@TotalRecords"));

            return result;
        }

        #endregion
    }
}
