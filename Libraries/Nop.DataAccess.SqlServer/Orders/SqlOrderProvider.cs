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

        private DBOrderNote GetOrderNoteFromReader(IDataReader dataReader)
        {
            var item = new DBOrderNote();
            item.OrderNoteId = NopSqlDataHelper.GetInt(dataReader, "OrderNoteID");
            item.OrderId = NopSqlDataHelper.GetInt(dataReader, "OrderID");
            item.Note = NopSqlDataHelper.GetString(dataReader, "Note");
            item.DisplayToCustomer = NopSqlDataHelper.GetBoolean(dataReader, "DisplayToCustomer");
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
        /// Gets an order
        /// </summary>
        /// <param name="orderId">The order identifier</param>
        /// <returns>Order</returns>
        public override DBOrder GetOrderById(int orderId)
        {
            DBOrder item = null;
            if (orderId == 0)
                return item;
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_OrderLoadByPrimaryKey");
            db.AddInParameter(dbCommand, "OrderID", DbType.Int32, orderId);
            using (IDataReader dataReader = db.ExecuteReader(dbCommand))
            {
                if (dataReader.Read())
                {
                    item = GetOrderFromReader(dataReader);
                }
            }
            return item;
        }

        /// <summary>
        /// Gets an order
        /// </summary>
        /// <param name="orderGuid">The order identifier</param>
        /// <returns>Order</returns>
        public override DBOrder GetOrderByGuid(Guid orderGuid)
        {
            DBOrder item = null;
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_OrderLoadByGuid");
            db.AddInParameter(dbCommand, "OrderGUID", DbType.Guid, orderGuid);
            using (IDataReader dataReader = db.ExecuteReader(dbCommand))
            {
                if (dataReader.Read())
                {
                    item = GetOrderFromReader(dataReader);
                }
            }
            return item;
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
        /// Gets all orders by customer identifier
        /// </summary>
        /// <param name="customerId">Customer identifier</param>
        /// <returns>Order collection</returns>
        public override DBOrderCollection GetOrdersByCustomerId(int customerId)
        {
            var result = new DBOrderCollection();
            if (customerId == 0)
                return result;
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_OrderLoadByCustomerID");
            db.AddInParameter(dbCommand, "CustomerID", DbType.Int32, customerId);
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
        /// Gets an order by authorization transaction identifier
        /// </summary>
        /// <param name="authorizationTransactionId">Authorization transaction identifier</param>
        /// <param name="paymentMethodId">Payment method identifier</param>
        /// <returns>Order</returns>
        public override DBOrder GetOrderByAuthorizationTransactionIdAndPaymentMethodId(string authorizationTransactionId, int paymentMethodId)
        {
            DBOrder item = null;
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_OrderLoadByAuthorizationTransactionIDAndPaymentMethodID");
            db.AddInParameter(dbCommand, "AuthorizationTransactionID", DbType.String, authorizationTransactionId);
            db.AddInParameter(dbCommand, "PaymentMethodID", DbType.Int32, paymentMethodId);
            using (IDataReader dataReader = db.ExecuteReader(dbCommand))
            {
                if (dataReader.Read())
                {
                    item = GetOrderFromReader(dataReader);
                }
            }
            return item;
        }

        /// <summary>
        /// Gets all orders by affiliate identifier
        /// </summary>
        /// <param name="affiliateId">Affiliate identifier</param>
        /// <returns>Order collection</returns>
        public override DBOrderCollection GetOrdersByAffiliateId(int affiliateId)
        {
            var result = new DBOrderCollection();
            if (affiliateId == 0)
                return result;
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_OrderLoadByAffiliateID");
            db.AddInParameter(dbCommand, "AffiliateID", DbType.Int32, affiliateId);
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
        public override DBOrder InsertOrder(Guid orderGuid,
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
            DateTime createdOn)
        {
            DBOrder item = null;
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_OrderInsert");
            db.AddOutParameter(dbCommand, "OrderID", DbType.Int32, 0);
            db.AddInParameter(dbCommand, "OrderGUID", DbType.Guid, orderGuid);
            db.AddInParameter(dbCommand, "CustomerID", DbType.Int32, customerId);
            db.AddInParameter(dbCommand, "CustomerLanguageID", DbType.Int32, customerLanguageId);
            db.AddInParameter(dbCommand, "CustomerTaxDisplayTypeID", DbType.Int32, customerTaxDisplayTypeId);
            db.AddInParameter(dbCommand, "CustomerIP", DbType.String, customerIP);
            db.AddInParameter(dbCommand, "OrderSubtotalInclTax", DbType.Decimal, orderSubtotalInclTax);
            db.AddInParameter(dbCommand, "OrderSubtotalExclTax", DbType.Decimal, orderSubtotalExclTax);
            db.AddInParameter(dbCommand, "OrderShippingInclTax", DbType.Decimal, orderShippingInclTax);
            db.AddInParameter(dbCommand, "OrderShippingExclTax", DbType.Decimal, orderShippingExclTax);
            db.AddInParameter(dbCommand, "PaymentMethodAdditionalFeeInclTax", DbType.Decimal, paymentMethodAdditionalFeeInclTax);
            db.AddInParameter(dbCommand, "PaymentMethodAdditionalFeeExclTax", DbType.Decimal, paymentMethodAdditionalFeeExclTax);
            db.AddInParameter(dbCommand, "OrderTax", DbType.Decimal, orderTax);
            db.AddInParameter(dbCommand, "OrderTotal", DbType.Decimal, orderTotal);
            db.AddInParameter(dbCommand, "OrderDiscount", DbType.Decimal, orderDiscount);
            db.AddInParameter(dbCommand, "OrderSubtotalInclTaxInCustomerCurrency", DbType.Decimal, orderSubtotalInclTaxInCustomerCurrency);
            db.AddInParameter(dbCommand, "OrderSubtotalExclTaxInCustomerCurrency", DbType.Decimal, orderSubtotalExclTaxInCustomerCurrency);
            db.AddInParameter(dbCommand, "OrderShippingInclTaxInCustomerCurrency", DbType.Decimal, orderShippingInclTaxInCustomerCurrency);
            db.AddInParameter(dbCommand, "OrderShippingExclTaxInCustomerCurrency", DbType.Decimal, orderShippingExclTaxInCustomerCurrency);
            db.AddInParameter(dbCommand, "PaymentMethodAdditionalFeeInclTaxInCustomerCurrency", DbType.Decimal, paymentMethodAdditionalFeeInclTaxInCustomerCurrency);
            db.AddInParameter(dbCommand, "PaymentMethodAdditionalFeeExclTaxInCustomerCurrency", DbType.Decimal, paymentMethodAdditionalFeeExclTaxInCustomerCurrency);
            db.AddInParameter(dbCommand, "OrderTaxInCustomerCurrency", DbType.Decimal, orderTaxInCustomerCurrency);
            db.AddInParameter(dbCommand, "OrderTotalInCustomerCurrency", DbType.Decimal, orderTotalInCustomerCurrency);
            db.AddInParameter(dbCommand, "OrderDiscountInCustomerCurrency", DbType.Decimal, orderDiscountInCustomerCurrency);
            db.AddInParameter(dbCommand, "CheckoutAttributeDescription", DbType.String, checkoutAttributeDescription);
            db.AddInParameter(dbCommand, "CheckoutAttributesXML", DbType.Xml, checkoutAttributesXml);
            db.AddInParameter(dbCommand, "CustomerCurrencyCode", DbType.String, customerCurrencyCode);
            db.AddInParameter(dbCommand, "OrderWeight", DbType.Decimal, orderWeight);
            db.AddInParameter(dbCommand, "AffiliateID", DbType.Int32, affiliateId);
            db.AddInParameter(dbCommand, "OrderStatusID", DbType.Int32, orderStatusId);
            db.AddInParameter(dbCommand, "AllowStoringCreditCardNumber", DbType.Boolean, allowStoringCreditCardNumber);
            db.AddInParameter(dbCommand, "CardType", DbType.String, cardType);
            db.AddInParameter(dbCommand, "CardName", DbType.String, cardName);
            db.AddInParameter(dbCommand, "CardNumber", DbType.String, cardNumber);
            db.AddInParameter(dbCommand, "MaskedCreditCardNumber", DbType.String, maskedCreditCardNumber);
            db.AddInParameter(dbCommand, "CardCVV2", DbType.String, cardCvv2);
            db.AddInParameter(dbCommand, "CardExpirationMonth", DbType.String, cardExpirationMonth);
            db.AddInParameter(dbCommand, "CardExpirationYear", DbType.String, cardExpirationYear);
            db.AddInParameter(dbCommand, "PaymentMethodID", DbType.Int32, paymentMethodId);
            db.AddInParameter(dbCommand, "PaymentMethodName", DbType.String, paymentMethodName);
            db.AddInParameter(dbCommand, "AuthorizationTransactionID", DbType.String, authorizationTransactionId);
            db.AddInParameter(dbCommand, "AuthorizationTransactionCode", DbType.String, authorizationTransactionCode);
            db.AddInParameter(dbCommand, "AuthorizationTransactionResult", DbType.String, authorizationTransactionResult);
            db.AddInParameter(dbCommand, "CaptureTransactionID", DbType.String, captureTransactionId);
            db.AddInParameter(dbCommand, "CaptureTransactionResult", DbType.String, captureTransactionResult);
            db.AddInParameter(dbCommand, "SubscriptionTransactionID", DbType.String, subscriptionTransactionId);
            db.AddInParameter(dbCommand, "PurchaseOrderNumber", DbType.String, purchaseOrderNumber);
            db.AddInParameter(dbCommand, "PaymentStatusID", DbType.Int32, paymentStatusId);
            if (paidDate.HasValue)
                db.AddInParameter(dbCommand, "PaidDate", DbType.DateTime, paidDate.Value);
            else
                db.AddInParameter(dbCommand, "PaidDate", DbType.DateTime, DBNull.Value);
            db.AddInParameter(dbCommand, "BillingFirstName", DbType.String, billingFirstName);
            db.AddInParameter(dbCommand, "BillingLastName", DbType.String, billingLastName);
            db.AddInParameter(dbCommand, "BillingPhoneNumber", DbType.String, billingPhoneNumber);
            db.AddInParameter(dbCommand, "BillingEmail", DbType.String, billingEmail);
            db.AddInParameter(dbCommand, "BillingFaxNumber", DbType.String, billingFaxNumber);
            db.AddInParameter(dbCommand, "BillingCompany", DbType.String, billingCompany);
            db.AddInParameter(dbCommand, "BillingAddress1", DbType.String, billingAddress1);
            db.AddInParameter(dbCommand, "BillingAddress2", DbType.String, billingAddress2);
            db.AddInParameter(dbCommand, "BillingCity", DbType.String, billingCity);
            db.AddInParameter(dbCommand, "BillingStateProvince", DbType.String, billingStateProvince);
            db.AddInParameter(dbCommand, "BillingStateProvinceID", DbType.Int32, billingStateProvinceId);
            db.AddInParameter(dbCommand, "BillingZipPostalCode", DbType.String, billingZipPostalCode);
            db.AddInParameter(dbCommand, "BillingCountry", DbType.String, billingCountry);
            db.AddInParameter(dbCommand, "BillingCountryID", DbType.Int32, billingCountryId);
            db.AddInParameter(dbCommand, "ShippingStatusID", DbType.Int32, shippingStatusId);
            db.AddInParameter(dbCommand, "ShippingFirstName", DbType.String, shippingFirstName);
            db.AddInParameter(dbCommand, "ShippingLastName", DbType.String, shippingLastName);
            db.AddInParameter(dbCommand, "ShippingPhoneNumber", DbType.String, shippingPhoneNumber);
            db.AddInParameter(dbCommand, "ShippingEmail", DbType.String, shippingEmail);
            db.AddInParameter(dbCommand, "ShippingFaxNumber", DbType.String, shippingFaxNumber);
            db.AddInParameter(dbCommand, "ShippingCompany", DbType.String, shippingCompany);
            db.AddInParameter(dbCommand, "ShippingAddress1", DbType.String, shippingAddress1);
            db.AddInParameter(dbCommand, "ShippingAddress2", DbType.String, shippingAddress2);
            db.AddInParameter(dbCommand, "ShippingCity", DbType.String, shippingCity);
            db.AddInParameter(dbCommand, "ShippingStateProvince", DbType.String, shippingStateProvince);
            db.AddInParameter(dbCommand, "ShippingStateProvinceID", DbType.Int32, shippingStateProvinceId);
            db.AddInParameter(dbCommand, "ShippingZipPostalCode", DbType.String, shippingZipPostalCode);
            db.AddInParameter(dbCommand, "ShippingCountry", DbType.String, shippingCountry);
            db.AddInParameter(dbCommand, "ShippingCountryID", DbType.Int32, shippingCountryId);
            db.AddInParameter(dbCommand, "ShippingMethod", DbType.String, shippingMethod);
            db.AddInParameter(dbCommand, "ShippingRateComputationMethodID", DbType.Int32, shippingRateComputationMethodId);
            if (shippedDate.HasValue)
                db.AddInParameter(dbCommand, "ShippedDate", DbType.DateTime, shippedDate.Value);
            else
                db.AddInParameter(dbCommand, "ShippedDate", DbType.DateTime, DBNull.Value);
            if (deliveryDate.HasValue)
                db.AddInParameter(dbCommand, "DeliveryDate", DbType.DateTime, deliveryDate.Value);
            else
                db.AddInParameter(dbCommand, "DeliveryDate", DbType.DateTime, DBNull.Value);
            db.AddInParameter(dbCommand, "TrackingNumber", DbType.String, trackingNumber);
            db.AddInParameter(dbCommand, "Deleted", DbType.Boolean, deleted);
            db.AddInParameter(dbCommand, "CreatedOn", DbType.DateTime, createdOn);
            if (db.ExecuteNonQuery(dbCommand) > 0)
            {
                int orderId = Convert.ToInt32(db.GetParameterValue(dbCommand, "@OrderID"));
                item = GetOrderById(orderId);
            }
            return item;
        }

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
        public override DBOrder UpdateOrder(int orderId,
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
            DateTime createdOn)
        {
            DBOrder item = null;
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_OrderUpdate");
            db.AddInParameter(dbCommand, "OrderID", DbType.Int32, orderId);
            db.AddInParameter(dbCommand, "OrderGUID", DbType.Guid, orderGuid);
            db.AddInParameter(dbCommand, "CustomerID", DbType.Int32, customerId);
            db.AddInParameter(dbCommand, "CustomerLanguageID", DbType.Int32, customerLanguageId);
            db.AddInParameter(dbCommand, "CustomerTaxDisplayTypeID", DbType.Int32, customerTaxDisplayTypeId);
            db.AddInParameter(dbCommand, "CustomerIP", DbType.String, customerIP);
            db.AddInParameter(dbCommand, "OrderSubtotalInclTax", DbType.Decimal, orderSubtotalInclTax);
            db.AddInParameter(dbCommand, "OrderSubtotalExclTax", DbType.Decimal, orderSubtotalExclTax);
            db.AddInParameter(dbCommand, "OrderShippingInclTax", DbType.Decimal, orderShippingInclTax);
            db.AddInParameter(dbCommand, "OrderShippingExclTax", DbType.Decimal, orderShippingExclTax);
            db.AddInParameter(dbCommand, "PaymentMethodAdditionalFeeInclTax", DbType.Decimal, paymentMethodAdditionalFeeInclTax);
            db.AddInParameter(dbCommand, "PaymentMethodAdditionalFeeExclTax", DbType.Decimal, paymentMethodAdditionalFeeExclTax);
            db.AddInParameter(dbCommand, "OrderTax", DbType.Decimal, orderTax);
            db.AddInParameter(dbCommand, "OrderTotal", DbType.Decimal, orderTotal);
            db.AddInParameter(dbCommand, "OrderDiscount", DbType.Decimal, orderDiscount);
            db.AddInParameter(dbCommand, "OrderSubtotalInclTaxInCustomerCurrency", DbType.Decimal, orderSubtotalInclTaxInCustomerCurrency);
            db.AddInParameter(dbCommand, "OrderSubtotalExclTaxInCustomerCurrency", DbType.Decimal, orderSubtotalExclTaxInCustomerCurrency);
            db.AddInParameter(dbCommand, "OrderShippingInclTaxInCustomerCurrency", DbType.Decimal, orderShippingInclTaxInCustomerCurrency);
            db.AddInParameter(dbCommand, "OrderShippingExclTaxInCustomerCurrency", DbType.Decimal, orderShippingExclTaxInCustomerCurrency);
            db.AddInParameter(dbCommand, "PaymentMethodAdditionalFeeInclTaxInCustomerCurrency", DbType.Decimal, paymentMethodAdditionalFeeInclTaxInCustomerCurrency);
            db.AddInParameter(dbCommand, "PaymentMethodAdditionalFeeExclTaxInCustomerCurrency", DbType.Decimal, paymentMethodAdditionalFeeExclTaxInCustomerCurrency);
            db.AddInParameter(dbCommand, "OrderTaxInCustomerCurrency", DbType.Decimal, orderTaxInCustomerCurrency);
            db.AddInParameter(dbCommand, "OrderTotalInCustomerCurrency", DbType.Decimal, orderTotalInCustomerCurrency);
            db.AddInParameter(dbCommand, "OrderDiscountInCustomerCurrency", DbType.Decimal, orderDiscountInCustomerCurrency);
            db.AddInParameter(dbCommand, "CheckoutAttributeDescription", DbType.String, checkoutAttributeDescription);
            db.AddInParameter(dbCommand, "CheckoutAttributesXML", DbType.Xml, checkoutAttributesXml);
            db.AddInParameter(dbCommand, "CustomerCurrencyCode", DbType.String, customerCurrencyCode);
            db.AddInParameter(dbCommand, "OrderWeight", DbType.Decimal, orderWeight);
            db.AddInParameter(dbCommand, "AffiliateID", DbType.Int32, affiliateId);
            db.AddInParameter(dbCommand, "OrderStatusID", DbType.Int32, orderStatusId);
            db.AddInParameter(dbCommand, "AllowStoringCreditCardNumber", DbType.Boolean, allowStoringCreditCardNumber);
            db.AddInParameter(dbCommand, "CardType", DbType.String, cardType);
            db.AddInParameter(dbCommand, "CardName", DbType.String, cardName);
            db.AddInParameter(dbCommand, "CardNumber", DbType.String, cardNumber);
            db.AddInParameter(dbCommand, "MaskedCreditCardNumber", DbType.String, maskedCreditCardNumber);
            db.AddInParameter(dbCommand, "CardCVV2", DbType.String, cardCvv2);
            db.AddInParameter(dbCommand, "CardExpirationMonth", DbType.String, cardExpirationMonth);
            db.AddInParameter(dbCommand, "CardExpirationYear", DbType.String, cardExpirationYear);
            db.AddInParameter(dbCommand, "PaymentMethodID", DbType.Int32, paymentMethodId);
            db.AddInParameter(dbCommand, "PaymentMethodName", DbType.String, paymentMethodName);
            db.AddInParameter(dbCommand, "AuthorizationTransactionID", DbType.String, authorizationTransactionId);
            db.AddInParameter(dbCommand, "AuthorizationTransactionCode", DbType.String, authorizationTransactionCode);
            db.AddInParameter(dbCommand, "AuthorizationTransactionResult", DbType.String, authorizationTransactionResult);
            db.AddInParameter(dbCommand, "CaptureTransactionID", DbType.String, captureTransactionId);
            db.AddInParameter(dbCommand, "CaptureTransactionResult", DbType.String, captureTransactionResult);
            db.AddInParameter(dbCommand, "SubscriptionTransactionID", DbType.String, subscriptionTransactionId);
            db.AddInParameter(dbCommand, "PurchaseOrderNumber", DbType.String, purchaseOrderNumber);
            db.AddInParameter(dbCommand, "PaymentStatusID", DbType.Int32, paymentStatusId);
            if (paidDate.HasValue)
                db.AddInParameter(dbCommand, "PaidDate", DbType.DateTime, paidDate.Value);
            else
                db.AddInParameter(dbCommand, "PaidDate", DbType.DateTime, DBNull.Value);
            db.AddInParameter(dbCommand, "BillingFirstName", DbType.String, billingFirstName);
            db.AddInParameter(dbCommand, "BillingLastName", DbType.String, billingLastName);
            db.AddInParameter(dbCommand, "BillingPhoneNumber", DbType.String, billingPhoneNumber);
            db.AddInParameter(dbCommand, "BillingEmail", DbType.String, billingEmail);
            db.AddInParameter(dbCommand, "BillingFaxNumber", DbType.String, billingFaxNumber);
            db.AddInParameter(dbCommand, "BillingCompany", DbType.String, billingCompany);
            db.AddInParameter(dbCommand, "BillingAddress1", DbType.String, billingAddress1);
            db.AddInParameter(dbCommand, "BillingAddress2", DbType.String, billingAddress2);
            db.AddInParameter(dbCommand, "BillingCity", DbType.String, billingCity);
            db.AddInParameter(dbCommand, "BillingStateProvince", DbType.String, billingStateProvince);
            db.AddInParameter(dbCommand, "BillingStateProvinceID", DbType.Int32, billingStateProvinceId);
            db.AddInParameter(dbCommand, "BillingZipPostalCode", DbType.String, billingZipPostalCode);
            db.AddInParameter(dbCommand, "BillingCountry", DbType.String, billingCountry);
            db.AddInParameter(dbCommand, "BillingCountryID", DbType.Int32, billingCountryId);
            db.AddInParameter(dbCommand, "ShippingStatusID", DbType.Int32, shippingStatusId);
            db.AddInParameter(dbCommand, "ShippingFirstName", DbType.String, shippingFirstName);
            db.AddInParameter(dbCommand, "ShippingLastName", DbType.String, shippingLastName);
            db.AddInParameter(dbCommand, "ShippingPhoneNumber", DbType.String, shippingPhoneNumber);
            db.AddInParameter(dbCommand, "ShippingEmail", DbType.String, shippingEmail);
            db.AddInParameter(dbCommand, "ShippingFaxNumber", DbType.String, shippingFaxNumber);
            db.AddInParameter(dbCommand, "ShippingCompany", DbType.String, shippingCompany);
            db.AddInParameter(dbCommand, "ShippingAddress1", DbType.String, shippingAddress1);
            db.AddInParameter(dbCommand, "ShippingAddress2", DbType.String, shippingAddress2);
            db.AddInParameter(dbCommand, "ShippingCity", DbType.String, shippingCity);
            db.AddInParameter(dbCommand, "ShippingStateProvince", DbType.String, shippingStateProvince);
            db.AddInParameter(dbCommand, "ShippingStateProvinceID", DbType.Int32, shippingStateProvinceId);
            db.AddInParameter(dbCommand, "ShippingZipPostalCode", DbType.String, shippingZipPostalCode);
            db.AddInParameter(dbCommand, "ShippingCountry", DbType.String, shippingCountry);
            db.AddInParameter(dbCommand, "ShippingCountryID", DbType.Int32, shippingCountryId);
            db.AddInParameter(dbCommand, "ShippingMethod", DbType.String, shippingMethod);
            db.AddInParameter(dbCommand, "ShippingRateComputationMethodID", DbType.Int32, shippingRateComputationMethodId);
            if (shippedDate.HasValue)
                db.AddInParameter(dbCommand, "ShippedDate", DbType.DateTime, shippedDate.Value);
            else
                db.AddInParameter(dbCommand, "ShippedDate", DbType.DateTime, DBNull.Value);
            if (deliveryDate.HasValue)
                db.AddInParameter(dbCommand, "DeliveryDate", DbType.DateTime, deliveryDate.Value);
            else
                db.AddInParameter(dbCommand, "DeliveryDate", DbType.DateTime, DBNull.Value);
            db.AddInParameter(dbCommand, "TrackingNumber", DbType.String, trackingNumber);
            db.AddInParameter(dbCommand, "Deleted", DbType.Boolean, deleted);
            db.AddInParameter(dbCommand, "CreatedOn", DbType.DateTime, createdOn);
            if (db.ExecuteNonQuery(dbCommand) > 0)
                item = GetOrderById(orderId);

            return item;
        }

        /// <summary>
        /// Gets an order note
        /// </summary>
        /// <param name="orderNoteId">Order note identifier</param>
        /// <returns>Order note</returns>
        public override DBOrderNote GetOrderNoteById(int orderNoteId)
        {
            DBOrderNote item = null;
            if (orderNoteId == 0)
                return item;
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_OrderNoteLoadByPrimaryKey");
            db.AddInParameter(dbCommand, "OrderNoteID", DbType.Int32, orderNoteId);
            using (IDataReader dataReader = db.ExecuteReader(dbCommand))
            {
                if (dataReader.Read())
                {
                    item = GetOrderNoteFromReader(dataReader);
                }
            }
            return item;
        }

        /// <summary>
        /// Gets an order notes by order identifier
        /// </summary>
        /// <param name="orderId">Order identifier</param>
        /// <param name="showHidden">A value indicating whether all orders should be loaded</param>
        /// <returns>Order note collection</returns>
        public override DBOrderNoteCollection GetOrderNoteByOrderId(int orderId, bool showHidden)
        {
            var result = new DBOrderNoteCollection();
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_OrderNoteLoadByOrderID");
            db.AddInParameter(dbCommand, "OrderID", DbType.Int32, orderId);
            db.AddInParameter(dbCommand, "ShowHidden", DbType.Boolean, showHidden);
            using (IDataReader dataReader = db.ExecuteReader(dbCommand))
            {
                while (dataReader.Read())
                {
                    var item = GetOrderNoteFromReader(dataReader);
                    result.Add(item);
                }
            }

            return result;
        }

        /// <summary>
        /// Deletes an order note
        /// </summary>
        /// <param name="orderNoteId">Order note identifier</param>
        public override void DeleteOrderNote(int orderNoteId)
        {
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_OrderNoteDelete");
            db.AddInParameter(dbCommand, "OrderNoteID", DbType.Int32, orderNoteId);
            db.ExecuteNonQuery(dbCommand);
        }

        /// <summary>
        /// Inserts an order note
        /// </summary>
        /// <param name="orderId">The order identifier</param>
        /// <param name="note">The note</param>
        /// <param name="displayToCustomer">A value indicating whether the customer can see a note</param>
        /// <param name="createdOn">The date and time of order note creation</param>
        /// <returns>Order note</returns>
        public override DBOrderNote InsertOrderNote(int orderId, string note,
            bool displayToCustomer, DateTime createdOn)
        {
            DBOrderNote item = null;
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_OrderNoteInsert");
            db.AddOutParameter(dbCommand, "OrderNoteID", DbType.Int32, 0);
            db.AddInParameter(dbCommand, "OrderID", DbType.Int32, orderId);
            db.AddInParameter(dbCommand, "Note", DbType.String, note);
            db.AddInParameter(dbCommand, "DisplayToCustomer", DbType.Boolean, displayToCustomer);
            db.AddInParameter(dbCommand, "CreatedOn", DbType.DateTime, createdOn);
            if (db.ExecuteNonQuery(dbCommand) > 0)
            {
                int orderNoteId = Convert.ToInt32(db.GetParameterValue(dbCommand, "@OrderNoteID"));
                item = GetOrderNoteById(orderNoteId);
            }
            return item;
        }

        /// <summary>
        /// Updates the order note
        /// </summary>
        /// <param name="orderNoteId">The order note identifier</param>
        /// <param name="orderId">The order identifier</param>
        /// <param name="note">The note</param>
        /// <param name="displayToCustomer">A value indicating whether the customer can see a note</param>
        /// <param name="createdOn">The date and time of order note creation</param>
        /// <returns>Order note</returns>
        public override DBOrderNote UpdateOrderNote(int orderNoteId, int orderId,
            string note, bool displayToCustomer, DateTime createdOn)
        {
            DBOrderNote item = null;
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_OrderNoteUpdate");
            db.AddInParameter(dbCommand, "OrderNoteID", DbType.Int32, orderNoteId);
            db.AddInParameter(dbCommand, "OrderID", DbType.Int32, orderId);
            db.AddInParameter(dbCommand, "Note", DbType.String, note);
            db.AddInParameter(dbCommand, "DisplayToCustomer", DbType.Boolean, displayToCustomer);
            db.AddInParameter(dbCommand, "CreatedOn", DbType.DateTime, createdOn);
            if (db.ExecuteNonQuery(dbCommand) > 0)
                item = GetOrderNoteById(orderNoteId);

            return item;
        }

        /// <summary>
        /// Gets an order product variant
        /// </summary>
        /// <param name="orderProductVariantId">Order product variant identifier</param>
        /// <returns>Order product variant</returns>
        public override DBOrderProductVariant GetOrderProductVariantById(int orderProductVariantId)
        {
            DBOrderProductVariant item = null;
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_OrderProductVariantLoadByPrimaryKey");
            db.AddInParameter(dbCommand, "OrderProductVariantID", DbType.Int32, orderProductVariantId);
            using (IDataReader dataReader = db.ExecuteReader(dbCommand))
            {
                if (dataReader.Read())
                {
                    item = GetOrderProductVariantFromReader(dataReader);
                }
            }
            return item;
        }

        /// <summary>
        /// Delete an order product variant
        /// </summary>
        /// <param name="orderProductVariantId">Order product variant identifier</param>
        public override void DeleteOrderProductVariant(int orderProductVariantId)
        {
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_OrderProductVariantDelete");
            db.AddInParameter(dbCommand, "OrderProductVariantID", DbType.Int32, orderProductVariantId);
            db.ExecuteNonQuery(dbCommand);
        }

        /// <summary>
        /// Gets an order product variant
        /// </summary>
        /// <param name="orderProductVariantGuid">Order product variant identifier</param>
        /// <returns>Order product variant</returns>
        public override DBOrderProductVariant GetOrderProductVariantByGuid(Guid orderProductVariantGuid)
        {
            DBOrderProductVariant item = null;
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_OrderProductVariantLoadByGUID");
            db.AddInParameter(dbCommand, "OrderProductVariantGUID", DbType.Guid, orderProductVariantGuid);
            using (IDataReader dataReader = db.ExecuteReader(dbCommand))
            {
                if (dataReader.Read())
                {
                    item = GetOrderProductVariantFromReader(dataReader);
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
        public override DBOrderProductVariant InsertOrderProductVariant(Guid orderProductVariantGuid,
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
            int licenseDownloadId)
        {
            DBOrderProductVariant item = null;
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_OrderProductVariantInsert");
            db.AddOutParameter(dbCommand, "OrderProductVariantID", DbType.Int32, 0);
            db.AddInParameter(dbCommand, "OrderProductVariantGUID", DbType.Guid, orderProductVariantGuid);
            db.AddInParameter(dbCommand, "OrderID", DbType.Int32, orderId);
            db.AddInParameter(dbCommand, "ProductVariantID", DbType.Int32, productVariantId);
            db.AddInParameter(dbCommand, "UnitPriceInclTax", DbType.Decimal, unitPriceInclTax);
            db.AddInParameter(dbCommand, "UnitPriceExclTax", DbType.Decimal, unitPriceExclTax);
            db.AddInParameter(dbCommand, "PriceInclTax", DbType.Decimal, priceInclTax);
            db.AddInParameter(dbCommand, "PriceExclTax", DbType.Decimal, priceExclTax);
            db.AddInParameter(dbCommand, "UnitPriceInclTaxInCustomerCurrency", DbType.Decimal, unitPriceInclTaxInCustomerCurrency);
            db.AddInParameter(dbCommand, "UnitPriceExclTaxInCustomerCurrency", DbType.Decimal, unitPriceExclTaxInCustomerCurrency);
            db.AddInParameter(dbCommand, "PriceInclTaxInCustomerCurrency", DbType.Decimal, priceInclTaxInCustomerCurrency);
            db.AddInParameter(dbCommand, "PriceExclTaxInCustomerCurrency", DbType.Decimal, priceExclTaxInCustomerCurrency);
            db.AddInParameter(dbCommand, "AttributeDescription", DbType.String, attributeDescription);
            db.AddInParameter(dbCommand, "AttributesXML", DbType.Xml, attributesXml);
            db.AddInParameter(dbCommand, "Quantity", DbType.Int32, quantity);
            db.AddInParameter(dbCommand, "DiscountAmountInclTax", DbType.Decimal, discountAmountInclTax);
            db.AddInParameter(dbCommand, "DiscountAmountExclTax", DbType.Decimal, discountAmountExclTax);
            db.AddInParameter(dbCommand, "DownloadCount", DbType.Int32, downloadCount);
            db.AddInParameter(dbCommand, "IsDownloadActivated", DbType.Boolean, isDownloadActivated);
            db.AddInParameter(dbCommand, "LicenseDownloadID", DbType.Int32, licenseDownloadId);
            if (db.ExecuteNonQuery(dbCommand) > 0)
            {
                int orderProductVariantId = Convert.ToInt32(db.GetParameterValue(dbCommand, "@OrderProductVariantID"));
                item = GetOrderProductVariantById(orderProductVariantId);
            }
            return item;
        }

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
        public override DBOrderProductVariant UpdateOrderProductVariant(int orderProductVariantId,
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
            int licenseDownloadId)
        {
            DBOrderProductVariant item = null;
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_OrderProductVariantUpdate");
            db.AddInParameter(dbCommand, "OrderProductVariantID", DbType.Int32, orderProductVariantId);
            db.AddInParameter(dbCommand, "OrderProductVariantGUID", DbType.Guid, orderProductVariantGuid);
            db.AddInParameter(dbCommand, "OrderID", DbType.Int32, orderId);
            db.AddInParameter(dbCommand, "ProductVariantID", DbType.Int32, productVariantId);
            db.AddInParameter(dbCommand, "UnitPriceInclTax", DbType.Decimal, unitPriceInclTax);
            db.AddInParameter(dbCommand, "UnitPriceExclTax", DbType.Decimal, unitPriceExclTax);
            db.AddInParameter(dbCommand, "PriceInclTax", DbType.Decimal, priceInclTax);
            db.AddInParameter(dbCommand, "PriceExclTax", DbType.Decimal, priceExclTax);
            db.AddInParameter(dbCommand, "UnitPriceInclTaxInCustomerCurrency", DbType.Decimal, unitPriceInclTaxInCustomerCurrency);
            db.AddInParameter(dbCommand, "UnitPriceExclTaxInCustomerCurrency", DbType.Decimal, unitPriceExclTaxInCustomerCurrency);
            db.AddInParameter(dbCommand, "PriceInclTaxInCustomerCurrency", DbType.Decimal, priceInclTaxInCustomerCurrency);
            db.AddInParameter(dbCommand, "PriceExclTaxInCustomerCurrency", DbType.Decimal, priceExclTaxInCustomerCurrency);
            db.AddInParameter(dbCommand, "AttributeDescription", DbType.String, attributeDescription);
            db.AddInParameter(dbCommand, "AttributesXML", DbType.Xml, attributesXml);
            db.AddInParameter(dbCommand, "Quantity", DbType.Int32, quantity);
            db.AddInParameter(dbCommand, "DiscountAmountInclTax", DbType.Decimal, discountAmountInclTax);
            db.AddInParameter(dbCommand, "DiscountAmountExclTax", DbType.Decimal, discountAmountExclTax);
            db.AddInParameter(dbCommand, "DownloadCount", DbType.Int32, downloadCount);
            db.AddInParameter(dbCommand, "IsDownloadActivated", DbType.Boolean, isDownloadActivated);
            db.AddInParameter(dbCommand, "LicenseDownloadID", DbType.Int32, licenseDownloadId); 
            if (db.ExecuteNonQuery(dbCommand) > 0)
                item = GetOrderProductVariantById(orderProductVariantId);

            return item;
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
        /// Gets a recurring payment
        /// </summary>
        /// <param name="recurringPaymentId">The recurring payment identifier</param>
        /// <returns>Recurring payment</returns>
        public override DBRecurringPayment GetRecurringPaymentById(int recurringPaymentId)
        {
            DBRecurringPayment item = null;
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_RecurringPaymentByPrimaryKey");
            db.AddInParameter(dbCommand, "RecurringPaymentID", DbType.Int32, recurringPaymentId);
            using (IDataReader dataReader = db.ExecuteReader(dbCommand))
            {
                if (dataReader.Read())
                {
                    item = GetRecurringPaymentFromReader(dataReader);
                }
            }
            return item;
        }

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
        public override DBRecurringPayment InsertRecurringPayment(int initialOrderId,
            int cycleLength, int cyclePeriod, int totalCycles,
            DateTime startDate, bool isActive, bool deleted, DateTime createdOn)
        {
            DBRecurringPayment item = null;
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_RecurringPaymentInsert");
            db.AddOutParameter(dbCommand, "RecurringPaymentID", DbType.Int32, 0);
            db.AddInParameter(dbCommand, "InitialOrderID", DbType.Int32, initialOrderId);
            db.AddInParameter(dbCommand, "CycleLength", DbType.Int32, cycleLength);
            db.AddInParameter(dbCommand, "CyclePeriod", DbType.Int32, cyclePeriod);
            db.AddInParameter(dbCommand, "TotalCycles", DbType.Int32, totalCycles);
            db.AddInParameter(dbCommand, "StartDate", DbType.DateTime, startDate);
            db.AddInParameter(dbCommand, "IsActive", DbType.Boolean, isActive);
            db.AddInParameter(dbCommand, "Deleted", DbType.Boolean, deleted);
            db.AddInParameter(dbCommand, "CreatedOn", DbType.DateTime, createdOn);
            if (db.ExecuteNonQuery(dbCommand) > 0)
            {
                int recurringPaymentId = Convert.ToInt32(db.GetParameterValue(dbCommand, "@RecurringPaymentID"));
                item = GetRecurringPaymentById(recurringPaymentId);
            }
            return item;
        }

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
        public override DBRecurringPayment UpdateRecurringPayment(int recurringPaymentId,
            int initialOrderId, int cycleLength, int cyclePeriod, int totalCycles,
            DateTime startDate, bool isActive, bool deleted, DateTime createdOn)
        {
            DBRecurringPayment item = null;
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_RecurringPaymentUpdate");
            db.AddInParameter(dbCommand, "RecurringPaymentID", DbType.Int32, recurringPaymentId);
            db.AddInParameter(dbCommand, "InitialOrderID", DbType.Int32, initialOrderId);
            db.AddInParameter(dbCommand, "CycleLength", DbType.Int32, cycleLength);
            db.AddInParameter(dbCommand, "CyclePeriod", DbType.Int32, cyclePeriod);
            db.AddInParameter(dbCommand, "TotalCycles", DbType.Int32, totalCycles);
            db.AddInParameter(dbCommand, "StartDate", DbType.DateTime, startDate);
            db.AddInParameter(dbCommand, "IsActive", DbType.Boolean, isActive);
            db.AddInParameter(dbCommand, "Deleted", DbType.Boolean, deleted);
            db.AddInParameter(dbCommand, "CreatedOn", DbType.DateTime, createdOn);
            if (db.ExecuteNonQuery(dbCommand) > 0)
                item = GetRecurringPaymentById(recurringPaymentId);

            return item;
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
        /// Deletes a recurring payment history
        /// </summary>
        /// <param name="recurringPaymentHistoryId">Recurring payment history identifier</param>
        public override void DeleteRecurringPaymentHistory(int recurringPaymentHistoryId)
        {
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_RecurringPaymentHistoryDelete");
            db.AddInParameter(dbCommand, "RecurringPaymentHistoryID", DbType.Int32, recurringPaymentHistoryId);
            db.ExecuteNonQuery(dbCommand);
        }

        /// <summary>
        /// Gets a recurring payment history
        /// </summary>
        /// <param name="recurringPaymentHistoryId">The recurring payment history identifier</param>
        /// <returns>Recurring payment history</returns>
        public override DBRecurringPaymentHistory GetRecurringPaymentHistoryById(int recurringPaymentHistoryId)
        {
            DBRecurringPaymentHistory item = null;
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_RecurringPaymentHistoryLoadByPrimaryKey");
            db.AddInParameter(dbCommand, "RecurringPaymentHistoryID", DbType.Int32, recurringPaymentHistoryId);
            using (IDataReader dataReader = db.ExecuteReader(dbCommand))
            {
                if (dataReader.Read())
                {
                    item = GetRecurringPaymentHistoryFromReader(dataReader);
                }
            }
            return item;
        }

        /// <summary>
        /// Inserts a recurring payment history
        /// </summary>
        /// <param name="recurringPaymentId">The recurring payment identifier</param>
        /// <param name="orderId">The order identifier</param>
        /// <param name="createdOn">The date and time of payment creation</param>
        /// <returns>Recurring payment history</returns>
        public override DBRecurringPaymentHistory InsertRecurringPaymentHistory(int recurringPaymentId,
            int orderId, DateTime createdOn)
        {
            DBRecurringPaymentHistory item = null;
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_RecurringPaymentHistoryInsert");
            db.AddOutParameter(dbCommand, "RecurringPaymentHistoryID", DbType.Int32, 0);
            db.AddInParameter(dbCommand, "RecurringPaymentID", DbType.Int32, recurringPaymentId);
            db.AddInParameter(dbCommand, "OrderID", DbType.Int32, orderId);
            db.AddInParameter(dbCommand, "CreatedOn", DbType.DateTime, createdOn);
            if (db.ExecuteNonQuery(dbCommand) > 0)
            {
                int recurringPaymentHistoryId = Convert.ToInt32(db.GetParameterValue(dbCommand, "@RecurringPaymentHistoryID"));
                item = GetRecurringPaymentHistoryById(recurringPaymentHistoryId);
            }
            return item;
        }

        /// <summary>
        /// Updates the recurring payment history
        /// </summary>
        /// <param name="recurringPaymentHistoryId">The recurring payment history identifier</param>
        /// <param name="recurringPaymentId">The recurring payment identifier</param>
        /// <param name="orderId">The order identifier</param>
        /// <param name="createdOn">The date and time of payment creation</param>
        /// <returns>Recurring payment history</returns>
        public override DBRecurringPaymentHistory UpdateRecurringPaymentHistory(int recurringPaymentHistoryId,
            int recurringPaymentId, int orderId, DateTime createdOn)
        {
            DBRecurringPaymentHistory item = null;
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_RecurringPaymentHistoryUpdate");
            db.AddInParameter(dbCommand, "RecurringPaymentHistoryID", DbType.Int32, recurringPaymentHistoryId);
            db.AddInParameter(dbCommand, "RecurringPaymentID", DbType.Int32, recurringPaymentId);
            db.AddInParameter(dbCommand, "OrderID", DbType.Int32, orderId);
            db.AddInParameter(dbCommand, "CreatedOn", DbType.DateTime, createdOn);
            if (db.ExecuteNonQuery(dbCommand) > 0)
                item = GetRecurringPaymentHistoryById(recurringPaymentHistoryId);

            return item;
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
        /// Deletes a gift card
        /// </summary>
        /// <param name="giftCardId">Gift card identifier</param>
        public override void DeleteGiftCard(int giftCardId)
        {
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_GiftCardDelete");
            db.AddInParameter(dbCommand, "GiftCardID", DbType.Int32, giftCardId);
            db.ExecuteNonQuery(dbCommand);
        }

        /// <summary>
        /// Gets a gift card
        /// </summary>
        /// <param name="giftCardId">Gift card identifier</param>
        /// <returns>Gift card entry</returns>
        public override DBGiftCard GetGiftCardById(int giftCardId)
        {
            DBGiftCard item = null;
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_GiftCardLoadByPrimaryKey");
            db.AddInParameter(dbCommand, "GiftCardID", DbType.Int32, giftCardId);
            using (IDataReader dataReader = db.ExecuteReader(dbCommand))
            {
                if (dataReader.Read())
                {
                    item = GetGiftCardFromReader(dataReader);
                }
            }
            return item;
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
        public override DBGiftCard InsertGiftCard(int purchasedOrderProductVariantId,
            decimal amount, bool isGiftCardActivated, string giftCardCouponCode,
            string recipientName, string recipientEmail,
            string senderName, string senderEmail, string message,
            bool isRecipientNotified, DateTime createdOn)
        {
            DBGiftCard item = null;
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_GiftCardInsert");
            db.AddOutParameter(dbCommand, "GiftCardID", DbType.Int32, 0);
            db.AddInParameter(dbCommand, "PurchasedOrderProductVariantID", DbType.Int32, purchasedOrderProductVariantId);
            db.AddInParameter(dbCommand, "Amount", DbType.Decimal, amount);
            db.AddInParameter(dbCommand, "IsGiftCardActivated", DbType.Boolean, isGiftCardActivated);
            db.AddInParameter(dbCommand, "GiftCardCouponCode", DbType.String, giftCardCouponCode);
            db.AddInParameter(dbCommand, "RecipientName", DbType.String, recipientName);
            db.AddInParameter(dbCommand, "RecipientEmail", DbType.String, recipientEmail);
            db.AddInParameter(dbCommand, "SenderName", DbType.String, senderName);
            db.AddInParameter(dbCommand, "SenderEmail", DbType.String, senderEmail);
            db.AddInParameter(dbCommand, "Message", DbType.String, message);
            db.AddInParameter(dbCommand, "IsRecipientNotified", DbType.Boolean, isRecipientNotified);
            db.AddInParameter(dbCommand, "CreatedOn", DbType.DateTime, createdOn);
            if (db.ExecuteNonQuery(dbCommand) > 0)
            {
                int giftCardId = Convert.ToInt32(db.GetParameterValue(dbCommand, "@GiftCardID"));
                item = GetGiftCardById(giftCardId);
            }
            return item;
        }

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
        public override DBGiftCard UpdateGiftCard(int giftCardId,
            int purchasedOrderProductVariantId,
            decimal amount, bool isGiftCardActivated, string giftCardCouponCode,
            string recipientName, string recipientEmail,
            string senderName, string senderEmail, string message,
            bool isRecipientNotified, DateTime createdOn)
        {
            DBGiftCard item = null;
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_GiftCardUpdate");
            db.AddInParameter(dbCommand, "GiftCardID", DbType.Int32, giftCardId);
            db.AddInParameter(dbCommand, "PurchasedOrderProductVariantID", DbType.Int32, purchasedOrderProductVariantId);
            db.AddInParameter(dbCommand, "Amount", DbType.Decimal, amount);
            db.AddInParameter(dbCommand, "IsGiftCardActivated", DbType.Boolean, isGiftCardActivated);
            db.AddInParameter(dbCommand, "GiftCardCouponCode", DbType.String, giftCardCouponCode);
            db.AddInParameter(dbCommand, "RecipientName", DbType.String, recipientName);
            db.AddInParameter(dbCommand, "RecipientEmail", DbType.String, recipientEmail);
            db.AddInParameter(dbCommand, "SenderName", DbType.String, senderName);
            db.AddInParameter(dbCommand, "SenderEmail", DbType.String, senderEmail);
            db.AddInParameter(dbCommand, "Message", DbType.String, message);
            db.AddInParameter(dbCommand, "IsRecipientNotified", DbType.Boolean, isRecipientNotified);
            db.AddInParameter(dbCommand, "CreatedOn", DbType.DateTime, createdOn);
            if (db.ExecuteNonQuery(dbCommand) > 0)
                item = GetGiftCardById(giftCardId);

            return item;
        }

        /// <summary>
        /// Deletes a gift card usage history entry
        /// </summary>
        /// <param name="giftCardUsageHistoryId">Gift card usage history entry identifier</param>
        public override void DeleteGiftCardUsageHistory(int giftCardUsageHistoryId)
        {
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_GiftCardUsageHistoryDelete");
            db.AddInParameter(dbCommand, "GiftCardUsageHistoryID", DbType.Int32, giftCardUsageHistoryId);
            db.ExecuteNonQuery(dbCommand);
        }

        /// <summary>
        /// Gets a gift card usage history entry
        /// </summary>
        /// <param name="giftCardUsageHistoryId">Gift card usage history entry identifier</param>
        /// <returns>Gift card usage history entry</returns>
        public override DBGiftCardUsageHistory GetGiftCardUsageHistoryById(int giftCardUsageHistoryId)
        {
            DBGiftCardUsageHistory item = null;
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_GiftCardUsageHistoryLoadByPrimaryKey");
            db.AddInParameter(dbCommand, "GiftCardUsageHistoryID", DbType.Int32, giftCardUsageHistoryId);
            using (IDataReader dataReader = db.ExecuteReader(dbCommand))
            {
                if (dataReader.Read())
                {
                    item = GetGiftCardUsageHistoryFromReader(dataReader);
                }
            }
            return item;
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
        /// Inserts a gift card usage history entry
        /// </summary>
        /// <param name="giftCardId">Gift card identifier</param>
        /// <param name="customerId">Customer identifier</param>
        /// <param name="orderId">Order identifier</param>
        /// <param name="usedValue">Used value</param>
        /// <param name="usedValueInCustomerCurrency">Used value (customer currency)</param>
        /// <param name="createdOn">A date and time of instance creation</param>
        /// <returns>Gift card usage history entry</returns>
        public override DBGiftCardUsageHistory InsertGiftCardUsageHistory(int giftCardId,
            int customerId, int orderId, decimal usedValue,
            decimal usedValueInCustomerCurrency, DateTime createdOn)
        {
            DBGiftCardUsageHistory item = null;
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_GiftCardUsageHistoryInsert");
            db.AddOutParameter(dbCommand, "GiftCardUsageHistoryID", DbType.Int32, 0);
            db.AddInParameter(dbCommand, "GiftCardID", DbType.Int32, giftCardId);
            db.AddInParameter(dbCommand, "CustomerID", DbType.Int32, customerId);
            db.AddInParameter(dbCommand, "OrderID", DbType.Int32, orderId);
            db.AddInParameter(dbCommand, "UsedValue", DbType.Decimal, usedValue);
            db.AddInParameter(dbCommand, "UsedValueInCustomerCurrency", DbType.Decimal, usedValueInCustomerCurrency);
            db.AddInParameter(dbCommand, "CreatedOn", DbType.DateTime, createdOn);
            if (db.ExecuteNonQuery(dbCommand) > 0)
            {
                int giftCardUsageHistoryId = Convert.ToInt32(db.GetParameterValue(dbCommand, "@GiftCardUsageHistoryID"));
                item = GetGiftCardUsageHistoryById(giftCardUsageHistoryId);
            }
            return item;
        }

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
        public override DBGiftCardUsageHistory UpdateGiftCardUsageHistory(int giftCardUsageHistoryId,
            int giftCardId, int customerId, int orderId, decimal usedValue,
            decimal usedValueInCustomerCurrency, DateTime createdOn)
        {
            DBGiftCardUsageHistory item = null;
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_GiftCardUsageHistoryUpdate");
            db.AddInParameter(dbCommand, "GiftCardUsageHistoryID", DbType.Int32, giftCardUsageHistoryId);
            db.AddInParameter(dbCommand, "GiftCardID", DbType.Int32, giftCardId);
            db.AddInParameter(dbCommand, "CustomerID", DbType.Int32, customerId);
            db.AddInParameter(dbCommand, "OrderID", DbType.Int32, orderId);
            db.AddInParameter(dbCommand, "UsedValue", DbType.Decimal, usedValue);
            db.AddInParameter(dbCommand, "UsedValueInCustomerCurrency", DbType.Decimal, usedValueInCustomerCurrency);
            db.AddInParameter(dbCommand, "CreatedOn", DbType.DateTime, createdOn);
            if (db.ExecuteNonQuery(dbCommand) > 0)
                item = GetGiftCardUsageHistoryById(giftCardUsageHistoryId);

            return item;
        }

        /// <summary>
        /// Deletes a reward point history entry
        /// </summary>
        /// <param name="rewardPointsHistoryId">Reward point history entry identifier</param>
        public override void DeleteRewardPointsHistory(int rewardPointsHistoryId)
        {
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_RewardPointsHistoryDelete");
            db.AddInParameter(dbCommand, "RewardPointsHistoryId", DbType.Int32, rewardPointsHistoryId);
            db.ExecuteNonQuery(dbCommand);
        }

        /// <summary>
        /// Gets a reward point history entry
        /// </summary>
        /// <param name="rewardPointsHistoryId">Reward point history entry identifier</param>
        /// <returns>Reward point history entry</returns>
        public override DBRewardPointsHistory GetRewardPointsHistoryById(int rewardPointsHistoryId)
        {
            DBRewardPointsHistory item = null;
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_RewardPointsHistoryLoadByPrimaryKey");
            db.AddInParameter(dbCommand, "RewardPointsHistoryId", DbType.Int32, rewardPointsHistoryId);
            using (IDataReader dataReader = db.ExecuteReader(dbCommand))
            {
                if (dataReader.Read())
                {
                    item = GetRewardPointsHistoryFromReader(dataReader);
                }
            }
            return item;
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
        public override DBRewardPointsHistory InsertRewardPointsHistory(int customerId,
            int orderId, int points, int pointsBalance, decimal usedAmount,
            decimal usedAmountInCustomerCurrency, string customerCurrencyCode,
            string message, DateTime createdOn)
        {
            DBRewardPointsHistory item = null;
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_RewardPointsHistoryInsert");
            db.AddOutParameter(dbCommand, "RewardPointsHistoryId", DbType.Int32, 0);
            db.AddInParameter(dbCommand, "CustomerId", DbType.Int32, customerId);
            db.AddInParameter(dbCommand, "OrderID", DbType.Int32, orderId);
            db.AddInParameter(dbCommand, "Points", DbType.Int32, points);
            db.AddInParameter(dbCommand, "PointsBalance", DbType.Int32, pointsBalance);
            db.AddInParameter(dbCommand, "UsedAmount", DbType.Decimal, usedAmount);
            db.AddInParameter(dbCommand, "UsedAmountInCustomerCurrency", DbType.Decimal, usedAmountInCustomerCurrency);
            db.AddInParameter(dbCommand, "CustomerCurrencyCode", DbType.String, customerCurrencyCode);
            db.AddInParameter(dbCommand, "Message", DbType.String, message);
            db.AddInParameter(dbCommand, "CreatedOn", DbType.DateTime, createdOn);
            if (db.ExecuteNonQuery(dbCommand) > 0)
            {
                int rewardPointsHistoryId = Convert.ToInt32(db.GetParameterValue(dbCommand, "@RewardPointsHistoryId"));
                item = GetRewardPointsHistoryById(rewardPointsHistoryId);
            }
            return item;
        }

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
        public override DBRewardPointsHistory UpdateRewardPointsHistory(int rewardPointsHistoryId,
            int customerId, int orderId, int points, int pointsBalance, decimal usedAmount,
            decimal usedAmountInCustomerCurrency, string customerCurrencyCode,
            string message, DateTime createdOn)
        {
            DBRewardPointsHistory item = null;
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_RewardPointsHistoryUpdate");
            db.AddInParameter(dbCommand, "RewardPointsHistoryId", DbType.Int32, rewardPointsHistoryId);
            db.AddInParameter(dbCommand, "CustomerId", DbType.Int32, customerId);
            db.AddInParameter(dbCommand, "OrderID", DbType.Int32, orderId);
            db.AddInParameter(dbCommand, "Points", DbType.Int32, points);
            db.AddInParameter(dbCommand, "PointsBalance", DbType.Int32, pointsBalance);
            db.AddInParameter(dbCommand, "UsedAmount", DbType.Decimal, usedAmount);
            db.AddInParameter(dbCommand, "UsedAmountInCustomerCurrency", DbType.Decimal, usedAmountInCustomerCurrency);
            db.AddInParameter(dbCommand, "CustomerCurrencyCode", DbType.String, customerCurrencyCode);
            db.AddInParameter(dbCommand, "Message", DbType.String, message);
            db.AddInParameter(dbCommand, "CreatedOn", DbType.DateTime, createdOn);
            if (db.ExecuteNonQuery(dbCommand) > 0)
                item = GetRewardPointsHistoryById(rewardPointsHistoryId);

            return item;
        }
        #endregion
    }
}
