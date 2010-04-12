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
    public partial class SQLOrderProvider : DBOrderProvider
    {
        #region Fields
        private string _sqlConnectionString;
        #endregion

        #region Utilities
        private DBOrder GetOrderFromReader(IDataReader dataReader)
        {
            DBOrder order = new DBOrder();
            order.OrderID = NopSqlDataHelper.GetInt(dataReader, "OrderID");
            order.OrderGUID = NopSqlDataHelper.GetGuid(dataReader, "OrderGUID");
            order.CustomerID = NopSqlDataHelper.GetInt(dataReader, "CustomerID");
            order.CustomerLanguageID = NopSqlDataHelper.GetInt(dataReader, "CustomerLanguageID");
            order.CustomerTaxDisplayTypeID = NopSqlDataHelper.GetInt(dataReader, "CustomerTaxDisplayTypeID");
            order.CustomerIP = NopSqlDataHelper.GetString(dataReader, "CustomerIP");
            order.OrderSubtotalInclTax = NopSqlDataHelper.GetDecimal(dataReader, "OrderSubtotalInclTax");
            order.OrderSubtotalExclTax = NopSqlDataHelper.GetDecimal(dataReader, "OrderSubtotalExclTax");
            order.OrderShippingInclTax = NopSqlDataHelper.GetDecimal(dataReader, "OrderShippingInclTax");
            order.OrderShippingExclTax = NopSqlDataHelper.GetDecimal(dataReader, "OrderShippingExclTax");
            order.PaymentMethodAdditionalFeeInclTax = NopSqlDataHelper.GetDecimal(dataReader, "PaymentMethodAdditionalFeeInclTax");
            order.PaymentMethodAdditionalFeeExclTax = NopSqlDataHelper.GetDecimal(dataReader, "PaymentMethodAdditionalFeeExclTax");
            order.OrderTax = NopSqlDataHelper.GetDecimal(dataReader, "OrderTax");
            order.OrderTotal = NopSqlDataHelper.GetDecimal(dataReader, "OrderTotal");
            order.OrderDiscount = NopSqlDataHelper.GetDecimal(dataReader, "OrderDiscount");
            order.OrderSubtotalInclTaxInCustomerCurrency = NopSqlDataHelper.GetDecimal(dataReader, "OrderSubtotalInclTaxInCustomerCurrency");
            order.OrderSubtotalExclTaxInCustomerCurrency = NopSqlDataHelper.GetDecimal(dataReader, "OrderSubtotalExclTaxInCustomerCurrency");
            order.OrderShippingInclTaxInCustomerCurrency = NopSqlDataHelper.GetDecimal(dataReader, "OrderShippingInclTaxInCustomerCurrency");
            order.OrderShippingExclTaxInCustomerCurrency = NopSqlDataHelper.GetDecimal(dataReader, "OrderShippingExclTaxInCustomerCurrency");
            order.PaymentMethodAdditionalFeeInclTaxInCustomerCurrency = NopSqlDataHelper.GetDecimal(dataReader, "PaymentMethodAdditionalFeeInclTaxInCustomerCurrency");
            order.PaymentMethodAdditionalFeeExclTaxInCustomerCurrency = NopSqlDataHelper.GetDecimal(dataReader, "PaymentMethodAdditionalFeeExclTaxInCustomerCurrency");
            order.OrderTaxInCustomerCurrency = NopSqlDataHelper.GetDecimal(dataReader, "OrderTaxInCustomerCurrency");
            order.OrderTotalInCustomerCurrency = NopSqlDataHelper.GetDecimal(dataReader, "OrderTotalInCustomerCurrency");
            order.OrderDiscountInCustomerCurrency = NopSqlDataHelper.GetDecimal(dataReader, "OrderDiscountInCustomerCurrency");
            order.CustomerCurrencyCode = NopSqlDataHelper.GetString(dataReader, "CustomerCurrencyCode");
            order.OrderWeight = NopSqlDataHelper.GetDecimal(dataReader, "OrderWeight");
            order.AffiliateID = NopSqlDataHelper.GetInt(dataReader, "AffiliateID");
            order.OrderStatusID = NopSqlDataHelper.GetInt(dataReader, "OrderStatusID");
            order.AllowStoringCreditCardNumber = NopSqlDataHelper.GetBoolean(dataReader, "AllowStoringCreditCardNumber");
            order.CardType = NopSqlDataHelper.GetString(dataReader, "CardType");
            order.CardName = NopSqlDataHelper.GetString(dataReader, "CardName");
            order.CardNumber = NopSqlDataHelper.GetString(dataReader, "CardNumber");
            order.MaskedCreditCardNumber = NopSqlDataHelper.GetString(dataReader, "MaskedCreditCardNumber");
            order.CardCVV2 = NopSqlDataHelper.GetString(dataReader, "CardCVV2");
            order.CardExpirationMonth = NopSqlDataHelper.GetString(dataReader, "CardExpirationMonth");
            order.CardExpirationYear = NopSqlDataHelper.GetString(dataReader, "CardExpirationYear");
            order.PaymentMethodID = NopSqlDataHelper.GetInt(dataReader, "PaymentMethodID");
            order.PaymentMethodName = NopSqlDataHelper.GetString(dataReader, "PaymentMethodName");
            order.AuthorizationTransactionID = NopSqlDataHelper.GetString(dataReader, "AuthorizationTransactionID");
            order.AuthorizationTransactionCode = NopSqlDataHelper.GetString(dataReader, "AuthorizationTransactionCode");
            order.AuthorizationTransactionResult = NopSqlDataHelper.GetString(dataReader, "AuthorizationTransactionResult");
            order.CaptureTransactionID = NopSqlDataHelper.GetString(dataReader, "CaptureTransactionID");
            order.CaptureTransactionResult = NopSqlDataHelper.GetString(dataReader, "CaptureTransactionResult");
            order.SubscriptionTransactionID = NopSqlDataHelper.GetString(dataReader, "SubscriptionTransactionID");
            order.PurchaseOrderNumber = NopSqlDataHelper.GetString(dataReader, "PurchaseOrderNumber");
            order.PaymentStatusID = NopSqlDataHelper.GetInt(dataReader, "PaymentStatusID");
            order.PaidDate = NopSqlDataHelper.GetNullableUtcDateTime(dataReader, "PaidDate");
            order.BillingFirstName = NopSqlDataHelper.GetString(dataReader, "BillingFirstName");
            order.BillingLastName = NopSqlDataHelper.GetString(dataReader, "BillingLastName");
            order.BillingPhoneNumber = NopSqlDataHelper.GetString(dataReader, "BillingPhoneNumber");
            order.BillingEmail = NopSqlDataHelper.GetString(dataReader, "BillingEmail");
            order.BillingFaxNumber = NopSqlDataHelper.GetString(dataReader, "BillingFaxNumber");
            order.BillingCompany = NopSqlDataHelper.GetString(dataReader, "BillingCompany");
            order.BillingAddress1 = NopSqlDataHelper.GetString(dataReader, "BillingAddress1");
            order.BillingAddress2 = NopSqlDataHelper.GetString(dataReader, "BillingAddress2");
            order.BillingCity = NopSqlDataHelper.GetString(dataReader, "BillingCity");
            order.BillingStateProvince = NopSqlDataHelper.GetString(dataReader, "BillingStateProvince");
            order.BillingStateProvinceID = NopSqlDataHelper.GetInt(dataReader, "BillingStateProvinceID");
            order.BillingZipPostalCode = NopSqlDataHelper.GetString(dataReader, "BillingZipPostalCode");
            order.BillingCountry = NopSqlDataHelper.GetString(dataReader, "BillingCountry");
            order.BillingCountryID = NopSqlDataHelper.GetInt(dataReader, "BillingCountryID");
            order.ShippingStatusID = NopSqlDataHelper.GetInt(dataReader, "ShippingStatusID");
            order.ShippingFirstName = NopSqlDataHelper.GetString(dataReader, "ShippingFirstName");
            order.ShippingLastName = NopSqlDataHelper.GetString(dataReader, "ShippingLastName");
            order.ShippingPhoneNumber = NopSqlDataHelper.GetString(dataReader, "ShippingPhoneNumber");
            order.ShippingEmail = NopSqlDataHelper.GetString(dataReader, "ShippingEmail");
            order.ShippingFaxNumber = NopSqlDataHelper.GetString(dataReader, "ShippingFaxNumber");
            order.ShippingCompany = NopSqlDataHelper.GetString(dataReader, "ShippingCompany");
            order.ShippingAddress1 = NopSqlDataHelper.GetString(dataReader, "ShippingAddress1");
            order.ShippingAddress2 = NopSqlDataHelper.GetString(dataReader, "ShippingAddress2");
            order.ShippingCity = NopSqlDataHelper.GetString(dataReader, "ShippingCity");
            order.ShippingStateProvince = NopSqlDataHelper.GetString(dataReader, "ShippingStateProvince");
            order.ShippingStateProvinceID = NopSqlDataHelper.GetInt(dataReader, "ShippingStateProvinceID");
            order.ShippingZipPostalCode = NopSqlDataHelper.GetString(dataReader, "ShippingZipPostalCode");
            order.ShippingCountry = NopSqlDataHelper.GetString(dataReader, "ShippingCountry");
            order.ShippingCountryID = NopSqlDataHelper.GetInt(dataReader, "ShippingCountryID");
            order.ShippingMethod = NopSqlDataHelper.GetString(dataReader, "ShippingMethod");
            order.ShippingRateComputationMethodID = NopSqlDataHelper.GetInt(dataReader, "ShippingRateComputationMethodID");
            order.ShippedDate = NopSqlDataHelper.GetNullableUtcDateTime(dataReader, "ShippedDate");
            order.TrackingNumber = NopSqlDataHelper.GetString(dataReader, "TrackingNumber");
            order.Deleted = NopSqlDataHelper.GetBoolean(dataReader, "Deleted");
            order.CreatedOn = NopSqlDataHelper.GetUtcDateTime(dataReader, "CreatedOn");
            return order;
        }

        private DBOrderNote GetOrderNoteFromReader(IDataReader dataReader)
        {
            DBOrderNote orderNote = new DBOrderNote();
            orderNote.OrderNoteID = NopSqlDataHelper.GetInt(dataReader, "OrderNoteID");
            orderNote.OrderID = NopSqlDataHelper.GetInt(dataReader, "OrderID");
            orderNote.Note = NopSqlDataHelper.GetString(dataReader, "Note");
            orderNote.DisplayToCustomer = NopSqlDataHelper.GetBoolean(dataReader, "DisplayToCustomer");
            orderNote.CreatedOn = NopSqlDataHelper.GetUtcDateTime(dataReader, "CreatedOn");
            return orderNote;
        }

        private DBOrderProductVariant GetOrderProductVariantFromReader(IDataReader dataReader)
        {
            DBOrderProductVariant orderProductVariant = new DBOrderProductVariant();
            orderProductVariant.OrderProductVariantID = NopSqlDataHelper.GetInt(dataReader, "OrderProductVariantID");
            orderProductVariant.OrderProductVariantGUID = NopSqlDataHelper.GetGuid(dataReader, "OrderProductVariantGUID");
            orderProductVariant.OrderID = NopSqlDataHelper.GetInt(dataReader, "OrderID");
            orderProductVariant.ProductVariantID = NopSqlDataHelper.GetInt(dataReader, "ProductVariantID");
            orderProductVariant.UnitPriceInclTax = NopSqlDataHelper.GetDecimal(dataReader, "UnitPriceInclTax");
            orderProductVariant.UnitPriceExclTax = NopSqlDataHelper.GetDecimal(dataReader, "UnitPriceExclTax");
            orderProductVariant.PriceInclTax = NopSqlDataHelper.GetDecimal(dataReader, "PriceInclTax");
            orderProductVariant.PriceExclTax = NopSqlDataHelper.GetDecimal(dataReader, "PriceExclTax");
            orderProductVariant.UnitPriceInclTaxInCustomerCurrency = NopSqlDataHelper.GetDecimal(dataReader, "UnitPriceInclTaxInCustomerCurrency");
            orderProductVariant.UnitPriceExclTaxInCustomerCurrency = NopSqlDataHelper.GetDecimal(dataReader, "UnitPriceExclTaxInCustomerCurrency");
            orderProductVariant.PriceInclTaxInCustomerCurrency = NopSqlDataHelper.GetDecimal(dataReader, "PriceInclTaxInCustomerCurrency");
            orderProductVariant.PriceExclTaxInCustomerCurrency = NopSqlDataHelper.GetDecimal(dataReader, "PriceExclTaxInCustomerCurrency");
            orderProductVariant.AttributeDescription = NopSqlDataHelper.GetString(dataReader, "AttributeDescription");
            orderProductVariant.AttributesXML = NopSqlDataHelper.GetString(dataReader, "AttributesXML");
            orderProductVariant.Quantity = NopSqlDataHelper.GetInt(dataReader, "Quantity");
            orderProductVariant.DiscountAmountInclTax = NopSqlDataHelper.GetDecimal(dataReader, "DiscountAmountInclTax");
            orderProductVariant.DiscountAmountExclTax = NopSqlDataHelper.GetDecimal(dataReader, "DiscountAmountExclTax");
            orderProductVariant.DownloadCount = NopSqlDataHelper.GetInt(dataReader, "DownloadCount");
            orderProductVariant.IsDownloadActivated = NopSqlDataHelper.GetBoolean(dataReader, "IsDownloadActivated");
            orderProductVariant.LicenseDownloadID = NopSqlDataHelper.GetInt(dataReader, "LicenseDownloadID");
            return orderProductVariant;
        }

        private DBOrderStatus GetOrderStatusFromReader(IDataReader dataReader)
        {
            DBOrderStatus orderStatus = new DBOrderStatus();
            orderStatus.OrderStatusID = NopSqlDataHelper.GetInt(dataReader, "OrderStatusID");
            orderStatus.Name = NopSqlDataHelper.GetString(dataReader, "Name");
            return orderStatus;
        }

        private DBOrderAverageReportLine GetOrderAverageReportLineFromReader(IDataReader dataReader)
        {
            DBOrderAverageReportLine orderAverageReportLine = new DBOrderAverageReportLine();
            orderAverageReportLine.SumOrders = NopSqlDataHelper.GetDecimal(dataReader, "SumOrders");
            orderAverageReportLine.CountOrders = NopSqlDataHelper.GetDecimal(dataReader, "CountOrders");
            return orderAverageReportLine;
        }

        private DBBestSellersReportLine GetBestSellersReportLineFromReader(IDataReader dataReader)
        {
            DBBestSellersReportLine bestSellersReportLine = new DBBestSellersReportLine();
            bestSellersReportLine.ProductVariantID = NopSqlDataHelper.GetInt(dataReader, "ProductVariantID");
            bestSellersReportLine.SalesTotalCount = NopSqlDataHelper.GetDecimal(dataReader, "SalesTotalCount");
            bestSellersReportLine.SalesTotalAmount = NopSqlDataHelper.GetDecimal(dataReader, "SalesTotalAmount");
            return bestSellersReportLine;
        }

        private DBRecurringPayment GetRecurringPaymentFromReader(IDataReader dataReader)
        {
            DBRecurringPayment recurringPayment = new DBRecurringPayment();
            recurringPayment.RecurringPaymentID = NopSqlDataHelper.GetInt(dataReader, "RecurringPaymentID");
            recurringPayment.InitialOrderID = NopSqlDataHelper.GetInt(dataReader, "InitialOrderID");
            recurringPayment.CycleLength = NopSqlDataHelper.GetInt(dataReader, "CycleLength");
            recurringPayment.CyclePeriod = NopSqlDataHelper.GetInt(dataReader, "CyclePeriod");
            recurringPayment.TotalCycles = NopSqlDataHelper.GetInt(dataReader, "TotalCycles");
            recurringPayment.StartDate = NopSqlDataHelper.GetUtcDateTime(dataReader, "StartDate");
            recurringPayment.IsActive = NopSqlDataHelper.GetBoolean(dataReader, "IsActive");
            recurringPayment.Deleted = NopSqlDataHelper.GetBoolean(dataReader, "Deleted");
            recurringPayment.CreatedOn = NopSqlDataHelper.GetUtcDateTime(dataReader, "CreatedOn");
            return recurringPayment;
        }

        private DBRecurringPaymentHistory GetRecurringPaymentHistoryFromReader(IDataReader dataReader)
        {
            DBRecurringPaymentHistory recurringPaymentHistory = new DBRecurringPaymentHistory();
            recurringPaymentHistory.RecurringPaymentHistoryID = NopSqlDataHelper.GetInt(dataReader, "RecurringPaymentHistoryID");
            recurringPaymentHistory.RecurringPaymentID = NopSqlDataHelper.GetInt(dataReader, "RecurringPaymentID");
            recurringPaymentHistory.OrderID = NopSqlDataHelper.GetInt(dataReader, "OrderID");
            recurringPaymentHistory.CreatedOn = NopSqlDataHelper.GetUtcDateTime(dataReader, "CreatedOn");
            return recurringPaymentHistory;
        }

        private DBGiftCard GetGiftCardFromReader(IDataReader dataReader)
        {
            DBGiftCard giftCard = new DBGiftCard();
            giftCard.GiftCardID = NopSqlDataHelper.GetInt(dataReader, "GiftCardID");
            giftCard.PurchasedOrderProductVariantID = NopSqlDataHelper.GetInt(dataReader, "PurchasedOrderProductVariantID");
            giftCard.Amount = NopSqlDataHelper.GetDecimal(dataReader, "Amount");
            giftCard.IsGiftCardActivated = NopSqlDataHelper.GetBoolean(dataReader, "IsGiftCardActivated");
            giftCard.GiftCardCouponCode = NopSqlDataHelper.GetString(dataReader, "GiftCardCouponCode");
            giftCard.RecipientName = NopSqlDataHelper.GetString(dataReader, "RecipientName");
            giftCard.RecipientEmail = NopSqlDataHelper.GetString(dataReader, "RecipientEmail");
            giftCard.SenderName = NopSqlDataHelper.GetString(dataReader, "SenderName");
            giftCard.SenderEmail = NopSqlDataHelper.GetString(dataReader, "SenderEmail");
            giftCard.Message = NopSqlDataHelper.GetString(dataReader, "Message");
            giftCard.IsSenderNotified = NopSqlDataHelper.GetBoolean(dataReader, "IsSenderNotified");
            giftCard.CreatedOn = NopSqlDataHelper.GetUtcDateTime(dataReader, "CreatedOn");
            return giftCard;
        }

        private DBGiftCardUsageHistory GetGiftCardUsageHistoryFromReader(IDataReader dataReader)
        {
            DBGiftCardUsageHistory giftCardUsageHistory = new DBGiftCardUsageHistory();
            giftCardUsageHistory.GiftCardUsageHistoryID = NopSqlDataHelper.GetInt(dataReader, "GiftCardUsageHistoryID");
            giftCardUsageHistory.GiftCardID = NopSqlDataHelper.GetInt(dataReader, "GiftCardID");
            giftCardUsageHistory.CustomerID = NopSqlDataHelper.GetInt(dataReader, "CustomerID");
            giftCardUsageHistory.OrderID = NopSqlDataHelper.GetInt(dataReader, "OrderID");
            giftCardUsageHistory.UsedValue = NopSqlDataHelper.GetDecimal(dataReader, "UsedValue");
            giftCardUsageHistory.UsedValueInCustomerCurrency = NopSqlDataHelper.GetDecimal(dataReader, "UsedValueInCustomerCurrency");
            giftCardUsageHistory.CreatedOn = NopSqlDataHelper.GetUtcDateTime(dataReader, "CreatedOn");
            return giftCardUsageHistory;
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
        /// <param name="OrderID">The order identifier</param>
        /// <returns>Order</returns>
        public override DBOrder GetOrderByID(int OrderID)
        {
            DBOrder order = null;
            if (OrderID == 0)
                return order;
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_OrderLoadByPrimaryKey");
            db.AddInParameter(dbCommand, "OrderID", DbType.Int32, OrderID);
            using (IDataReader dataReader = db.ExecuteReader(dbCommand))
            {
                if (dataReader.Read())
                {
                    order = GetOrderFromReader(dataReader);
                }
            }
            return order;
        }

        /// <summary>
        /// Gets an order
        /// </summary>
        /// <param name="OrderGUID">The order identifier</param>
        /// <returns>Order</returns>
        public override DBOrder GetOrderByGUID(Guid OrderGUID)
        {
            DBOrder order = null;
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_OrderLoadByGuid");
            db.AddInParameter(dbCommand, "OrderGUID", DbType.Guid, OrderGUID);
            using (IDataReader dataReader = db.ExecuteReader(dbCommand))
            {
                if (dataReader.Read())
                {
                    order = GetOrderFromReader(dataReader);
                }
            }
            return order;
        }

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
        public override DBOrderCollection SearchOrders(DateTime? StartTime, DateTime? EndTime, string CustomerEmail, int? OrderStatusID, int? PaymentStatusID, int? ShippingStatusID)
        {
            var result = new DBOrderCollection();

            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_OrderSearch");
            if (StartTime.HasValue)
                db.AddInParameter(dbCommand, "StartTime", DbType.DateTime, StartTime.Value);
            else
                db.AddInParameter(dbCommand, "StartTime", DbType.DateTime, null);
            if (EndTime.HasValue)
                db.AddInParameter(dbCommand, "EndTime", DbType.DateTime, EndTime.Value);
            else
                db.AddInParameter(dbCommand, "EndTime", DbType.DateTime, null);
            db.AddInParameter(dbCommand, "CustomerEmail", DbType.String, CustomerEmail);
            if (OrderStatusID.HasValue)
                db.AddInParameter(dbCommand, "OrderStatusID", DbType.Int32, OrderStatusID.Value);
            else
                db.AddInParameter(dbCommand, "OrderStatusID", DbType.Int32, null);
            if (PaymentStatusID.HasValue)
                db.AddInParameter(dbCommand, "PaymentStatusID", DbType.Int32, PaymentStatusID.Value);
            else
                db.AddInParameter(dbCommand, "PaymentStatusID", DbType.Int32, null);

            if (ShippingStatusID.HasValue)
                db.AddInParameter(dbCommand, "ShippingStatusID", DbType.Int32, ShippingStatusID);
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
        /// <param name="StartTime">Order start time; null to load all</param>
        /// <param name="EndTime">Order end time; null to load all</param>
        /// <param name="OrderStatusID">Order status identifier; null to load all orders</param>
        /// <param name="PaymentStatusID">Order payment status identifier; null to load all orders</param>
        /// <returns>Result</returns>
        public override IDataReader OrderProductVariantReport(DateTime? StartTime, DateTime? EndTime, int? OrderStatusID, int? PaymentStatusID)
        {
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_OrderProductVariantReport");
            if (StartTime.HasValue)
                db.AddInParameter(dbCommand, "StartTime", DbType.DateTime, StartTime.Value);
            else
                db.AddInParameter(dbCommand, "StartTime", DbType.DateTime, null);
            if (EndTime.HasValue)
                db.AddInParameter(dbCommand, "EndTime", DbType.DateTime, EndTime.Value);
            else
                db.AddInParameter(dbCommand, "EndTime", DbType.DateTime, null);
            if (OrderStatusID.HasValue)
                db.AddInParameter(dbCommand, "OrderStatusID", DbType.Int32, OrderStatusID.Value);
            else
                db.AddInParameter(dbCommand, "OrderStatusID", DbType.Int32, null);
            if (PaymentStatusID.HasValue)
                db.AddInParameter(dbCommand, "PaymentStatusID", DbType.Int32, PaymentStatusID.Value);
            else
                db.AddInParameter(dbCommand, "PaymentStatusID", DbType.Int32, null);
            return db.ExecuteReader(dbCommand);
        }

        /// <summary>
        /// Get the bests sellers report
        /// </summary>
        /// <param name="LastDays">Last number of days</param>
        /// <param name="RecordsToReturn">Number of products to return</param>
        /// <param name="OrderBy">1 - order by total count, 2 - Order by total amount</param>
        /// <returns>Result</returns>
        public override List<DBBestSellersReportLine> BestSellersReport(int LastDays, int RecordsToReturn, int OrderBy)
        {
            List<DBBestSellersReportLine> result = new List<DBBestSellersReportLine>();
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_SalesBestSellersReport");
            db.AddInParameter(dbCommand, "LastDays", DbType.Int32, LastDays);
            db.AddInParameter(dbCommand, "RecordsToReturn", DbType.Int32, RecordsToReturn);
            db.AddInParameter(dbCommand, "OrderBy", DbType.Int32, OrderBy);
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
        /// <param name="OrderStatusID">Order status identifier</param>
        /// <param name="startTime">Start date</param>
        /// <param name="endTime">End date</param>
        /// <returns>Result</returns>
        public override DBOrderAverageReportLine OrderAverageReport(int OrderStatusID, DateTime? startTime, DateTime? endTime)
        {
            DBOrderAverageReportLine item = null;
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_OrderAverageReport");
            db.AddInParameter(dbCommand, "OrderStatusID", DbType.Int32, OrderStatusID);
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
        /// <param name="CustomerID">Customer identifier</param>
        /// <returns>Order collection</returns>
        public override DBOrderCollection GetOrdersByCustomerID(int CustomerID)
        {
            var result = new DBOrderCollection();
            if (CustomerID == 0)
                return result;
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_OrderLoadByCustomerID");
            db.AddInParameter(dbCommand, "CustomerID", DbType.Int32, CustomerID);
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
        /// <param name="AuthorizationTransactionID">Authorization transaction identifier</param>
        /// <param name="PaymentMethodID">Payment method identifier</param>
        /// <returns>Order</returns>
        public override DBOrder GetOrderByAuthorizationTransactionIDAndPaymentMethodID(string AuthorizationTransactionID, int PaymentMethodID)
        {
            DBOrder order = null;
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_OrderLoadByAuthorizationTransactionIDAndPaymentMethodID");
            db.AddInParameter(dbCommand, "AuthorizationTransactionID", DbType.String, AuthorizationTransactionID);
            db.AddInParameter(dbCommand, "PaymentMethodID", DbType.Int32, PaymentMethodID);
            using (IDataReader dataReader = db.ExecuteReader(dbCommand))
            {
                if (dataReader.Read())
                {
                    order = GetOrderFromReader(dataReader);
                }
            }
            return order;
        }

        /// <summary>
        /// Gets all orders by affiliate identifier
        /// </summary>
        /// <param name="AffiliateID">Affiliate identifier</param>
        /// <returns>Order collection</returns>
        public override DBOrderCollection GetOrdersByAffiliateID(int AffiliateID)
        {
            var result = new DBOrderCollection();
            if (AffiliateID == 0)
                return result;
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_OrderLoadByAffiliateID");
            db.AddInParameter(dbCommand, "AffiliateID", DbType.Int32, AffiliateID);
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
        public override DBOrder InsertOrder(Guid OrderGUID, int CustomerID, int CustomerLanguageID,
            int CustomerTaxDisplayTypeID, string CustomerIP, decimal OrderSubtotalInclTax, decimal OrderSubtotalExclTax,
            decimal OrderShippingInclTax, decimal OrderShippingExclTax,
            decimal PaymentMethodAdditionalFeeInclTax, decimal PaymentMethodAdditionalFeeExclTax,
            decimal OrderTax, decimal OrderTotal, decimal OrderDiscount,
            decimal OrderSubtotalInclTaxInCustomerCurrency, decimal OrderSubtotalExclTaxInCustomerCurrency,
            decimal OrderShippingInclTaxInCustomerCurrency, decimal OrderShippingExclTaxInCustomerCurrency,
            decimal PaymentMethodAdditionalFeeInclTaxInCustomerCurrency, decimal PaymentMethodAdditionalFeeExclTaxInCustomerCurrency,
            decimal OrderTaxInCustomerCurrency, decimal OrderTotalInCustomerCurrency,
            decimal OrderDiscountInCustomerCurrency, string CustomerCurrencyCode, decimal OrderWeight,
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
            string TrackingNumber, bool Deleted, DateTime CreatedOn)
        {
            DBOrder order = null;
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_OrderInsert");
            db.AddOutParameter(dbCommand, "OrderID", DbType.Int32, 0);
            db.AddInParameter(dbCommand, "OrderGUID", DbType.Guid, OrderGUID);
            db.AddInParameter(dbCommand, "CustomerID", DbType.Int32, CustomerID);
            db.AddInParameter(dbCommand, "CustomerLanguageID", DbType.Int32, CustomerLanguageID);
            db.AddInParameter(dbCommand, "CustomerTaxDisplayTypeID", DbType.Int32, CustomerTaxDisplayTypeID);
            db.AddInParameter(dbCommand, "CustomerIP", DbType.String, CustomerIP);
            db.AddInParameter(dbCommand, "OrderSubtotalInclTax", DbType.Decimal, OrderSubtotalInclTax);
            db.AddInParameter(dbCommand, "OrderSubtotalExclTax", DbType.Decimal, OrderSubtotalExclTax);
            db.AddInParameter(dbCommand, "OrderShippingInclTax", DbType.Decimal, OrderShippingInclTax);
            db.AddInParameter(dbCommand, "OrderShippingExclTax", DbType.Decimal, OrderShippingExclTax);
            db.AddInParameter(dbCommand, "PaymentMethodAdditionalFeeInclTax", DbType.Decimal, PaymentMethodAdditionalFeeInclTax);
            db.AddInParameter(dbCommand, "PaymentMethodAdditionalFeeExclTax", DbType.Decimal, PaymentMethodAdditionalFeeExclTax);
            db.AddInParameter(dbCommand, "OrderTax", DbType.Decimal, OrderTax);
            db.AddInParameter(dbCommand, "OrderTotal", DbType.Decimal, OrderTotal);
            db.AddInParameter(dbCommand, "OrderDiscount", DbType.Decimal, OrderDiscount);
            db.AddInParameter(dbCommand, "OrderSubtotalInclTaxInCustomerCurrency", DbType.Decimal, OrderSubtotalInclTaxInCustomerCurrency);
            db.AddInParameter(dbCommand, "OrderSubtotalExclTaxInCustomerCurrency", DbType.Decimal, OrderSubtotalExclTaxInCustomerCurrency);
            db.AddInParameter(dbCommand, "OrderShippingInclTaxInCustomerCurrency", DbType.Decimal, OrderShippingInclTaxInCustomerCurrency);
            db.AddInParameter(dbCommand, "OrderShippingExclTaxInCustomerCurrency", DbType.Decimal, OrderShippingExclTaxInCustomerCurrency);
            db.AddInParameter(dbCommand, "PaymentMethodAdditionalFeeInclTaxInCustomerCurrency", DbType.Decimal, PaymentMethodAdditionalFeeInclTaxInCustomerCurrency);
            db.AddInParameter(dbCommand, "PaymentMethodAdditionalFeeExclTaxInCustomerCurrency", DbType.Decimal, PaymentMethodAdditionalFeeExclTaxInCustomerCurrency);
            db.AddInParameter(dbCommand, "OrderTaxInCustomerCurrency", DbType.Decimal, OrderTaxInCustomerCurrency);
            db.AddInParameter(dbCommand, "OrderTotalInCustomerCurrency", DbType.Decimal, OrderTotalInCustomerCurrency);
            db.AddInParameter(dbCommand, "OrderDiscountInCustomerCurrency", DbType.Decimal, OrderDiscountInCustomerCurrency);
            db.AddInParameter(dbCommand, "CustomerCurrencyCode", DbType.String, CustomerCurrencyCode);
            db.AddInParameter(dbCommand, "OrderWeight", DbType.Decimal, OrderWeight);
            db.AddInParameter(dbCommand, "AffiliateID", DbType.Int32, AffiliateID);
            db.AddInParameter(dbCommand, "OrderStatusID", DbType.Int32, OrderStatusID);
            db.AddInParameter(dbCommand, "AllowStoringCreditCardNumber", DbType.Boolean, AllowStoringCreditCardNumber);
            db.AddInParameter(dbCommand, "CardType", DbType.String, CardType);
            db.AddInParameter(dbCommand, "CardName", DbType.String, CardName);
            db.AddInParameter(dbCommand, "CardNumber", DbType.String, CardNumber);
            db.AddInParameter(dbCommand, "MaskedCreditCardNumber", DbType.String, MaskedCreditCardNumber);
            db.AddInParameter(dbCommand, "CardCVV2", DbType.String, CardCVV2);
            db.AddInParameter(dbCommand, "CardExpirationMonth", DbType.String, CardExpirationMonth);
            db.AddInParameter(dbCommand, "CardExpirationYear", DbType.String, CardExpirationYear);
            db.AddInParameter(dbCommand, "PaymentMethodID", DbType.Int32, PaymentMethodID);
            db.AddInParameter(dbCommand, "PaymentMethodName", DbType.String, PaymentMethodName);
            db.AddInParameter(dbCommand, "AuthorizationTransactionID", DbType.String, AuthorizationTransactionID);
            db.AddInParameter(dbCommand, "AuthorizationTransactionCode", DbType.String, AuthorizationTransactionCode);
            db.AddInParameter(dbCommand, "AuthorizationTransactionResult", DbType.String, AuthorizationTransactionResult);
            db.AddInParameter(dbCommand, "CaptureTransactionID", DbType.String, CaptureTransactionID);
            db.AddInParameter(dbCommand, "CaptureTransactionResult", DbType.String, CaptureTransactionResult);
            db.AddInParameter(dbCommand, "SubscriptionTransactionID", DbType.String, SubscriptionTransactionID);
            db.AddInParameter(dbCommand, "PurchaseOrderNumber", DbType.String, PurchaseOrderNumber);
            db.AddInParameter(dbCommand, "PaymentStatusID", DbType.Int32, PaymentStatusID);
            if (PaidDate.HasValue)
                db.AddInParameter(dbCommand, "PaidDate", DbType.DateTime, PaidDate.Value);
            else
                db.AddInParameter(dbCommand, "PaidDate", DbType.DateTime, DBNull.Value);
            db.AddInParameter(dbCommand, "BillingFirstName", DbType.String, BillingFirstName);
            db.AddInParameter(dbCommand, "BillingLastName", DbType.String, BillingLastName);
            db.AddInParameter(dbCommand, "BillingPhoneNumber", DbType.String, BillingPhoneNumber);
            db.AddInParameter(dbCommand, "BillingEmail", DbType.String, BillingEmail);
            db.AddInParameter(dbCommand, "BillingFaxNumber", DbType.String, BillingFaxNumber);
            db.AddInParameter(dbCommand, "BillingCompany", DbType.String, BillingCompany);
            db.AddInParameter(dbCommand, "BillingAddress1", DbType.String, BillingAddress1);
            db.AddInParameter(dbCommand, "BillingAddress2", DbType.String, BillingAddress2);
            db.AddInParameter(dbCommand, "BillingCity", DbType.String, BillingCity);
            db.AddInParameter(dbCommand, "BillingStateProvince", DbType.String, BillingStateProvince);
            db.AddInParameter(dbCommand, "BillingStateProvinceID", DbType.Int32, BillingStateProvinceID);
            db.AddInParameter(dbCommand, "BillingZipPostalCode", DbType.String, BillingZipPostalCode);
            db.AddInParameter(dbCommand, "BillingCountry", DbType.String, BillingCountry);
            db.AddInParameter(dbCommand, "BillingCountryID", DbType.Int32, BillingCountryID);
            db.AddInParameter(dbCommand, "ShippingStatusID", DbType.Int32, ShippingStatusID);
            db.AddInParameter(dbCommand, "ShippingFirstName", DbType.String, ShippingFirstName);
            db.AddInParameter(dbCommand, "ShippingLastName", DbType.String, ShippingLastName);
            db.AddInParameter(dbCommand, "ShippingPhoneNumber", DbType.String, ShippingPhoneNumber);
            db.AddInParameter(dbCommand, "ShippingEmail", DbType.String, ShippingEmail);
            db.AddInParameter(dbCommand, "ShippingFaxNumber", DbType.String, ShippingFaxNumber);
            db.AddInParameter(dbCommand, "ShippingCompany", DbType.String, ShippingCompany);
            db.AddInParameter(dbCommand, "ShippingAddress1", DbType.String, ShippingAddress1);
            db.AddInParameter(dbCommand, "ShippingAddress2", DbType.String, ShippingAddress2);
            db.AddInParameter(dbCommand, "ShippingCity", DbType.String, ShippingCity);
            db.AddInParameter(dbCommand, "ShippingStateProvince", DbType.String, ShippingStateProvince);
            db.AddInParameter(dbCommand, "ShippingStateProvinceID", DbType.Int32, ShippingStateProvinceID);
            db.AddInParameter(dbCommand, "ShippingZipPostalCode", DbType.String, ShippingZipPostalCode);
            db.AddInParameter(dbCommand, "ShippingCountry", DbType.String, ShippingCountry);
            db.AddInParameter(dbCommand, "ShippingCountryID", DbType.Int32, ShippingCountryID);
            db.AddInParameter(dbCommand, "ShippingMethod", DbType.String, ShippingMethod);
            db.AddInParameter(dbCommand, "ShippingRateComputationMethodID", DbType.Int32, ShippingRateComputationMethodID);
            if (ShippedDate.HasValue)
                db.AddInParameter(dbCommand, "ShippedDate", DbType.DateTime, ShippedDate.Value);
            else
                db.AddInParameter(dbCommand, "ShippedDate", DbType.DateTime, DBNull.Value);
            db.AddInParameter(dbCommand, "TrackingNumber", DbType.String, TrackingNumber);
            db.AddInParameter(dbCommand, "Deleted", DbType.Boolean, Deleted);
            db.AddInParameter(dbCommand, "CreatedOn", DbType.DateTime, CreatedOn);
            if (db.ExecuteNonQuery(dbCommand) > 0)
            {
                int OrderID = Convert.ToInt32(db.GetParameterValue(dbCommand, "@OrderID"));
                order = GetOrderByID(OrderID);
            }
            return order;
        }

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
        public override DBOrder UpdateOrder(int OrderID, Guid OrderGUID, int CustomerID, int CustomerLanguageID,
            int CustomerTaxDisplayTypeID, string CustomerIP, decimal OrderSubtotalInclTax, decimal OrderSubtotalExclTax,
            decimal OrderShippingInclTax, decimal OrderShippingExclTax,
            decimal PaymentMethodAdditionalFeeInclTax, decimal PaymentMethodAdditionalFeeExclTax,
            decimal OrderTax, decimal OrderTotal, decimal OrderDiscount,
            decimal OrderSubtotalInclTaxInCustomerCurrency, decimal OrderSubtotalExclTaxInCustomerCurrency,
            decimal OrderShippingInclTaxInCustomerCurrency, decimal OrderShippingExclTaxInCustomerCurrency,
            decimal PaymentMethodAdditionalFeeInclTaxInCustomerCurrency, decimal PaymentMethodAdditionalFeeExclTaxInCustomerCurrency,
            decimal OrderTaxInCustomerCurrency, decimal OrderTotalInCustomerCurrency,
            decimal OrderDiscountInCustomerCurrency, string CustomerCurrencyCode, decimal OrderWeight,
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
            string TrackingNumber, bool Deleted, DateTime CreatedOn)
        {
            DBOrder order = null;
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_OrderUpdate");
            db.AddInParameter(dbCommand, "OrderID", DbType.Int32, OrderID);
            db.AddInParameter(dbCommand, "OrderGUID", DbType.Guid, OrderGUID);
            db.AddInParameter(dbCommand, "CustomerID", DbType.Int32, CustomerID);
            db.AddInParameter(dbCommand, "CustomerLanguageID", DbType.Int32, CustomerLanguageID);
            db.AddInParameter(dbCommand, "CustomerTaxDisplayTypeID", DbType.Int32, CustomerTaxDisplayTypeID);
            db.AddInParameter(dbCommand, "CustomerIP", DbType.String, CustomerIP);
            db.AddInParameter(dbCommand, "OrderSubtotalInclTax", DbType.Decimal, OrderSubtotalInclTax);
            db.AddInParameter(dbCommand, "OrderSubtotalExclTax", DbType.Decimal, OrderSubtotalExclTax);
            db.AddInParameter(dbCommand, "OrderShippingInclTax", DbType.Decimal, OrderShippingInclTax);
            db.AddInParameter(dbCommand, "OrderShippingExclTax", DbType.Decimal, OrderShippingExclTax);
            db.AddInParameter(dbCommand, "PaymentMethodAdditionalFeeInclTax", DbType.Decimal, PaymentMethodAdditionalFeeInclTax);
            db.AddInParameter(dbCommand, "PaymentMethodAdditionalFeeExclTax", DbType.Decimal, PaymentMethodAdditionalFeeExclTax);
            db.AddInParameter(dbCommand, "OrderTax", DbType.Decimal, OrderTax);
            db.AddInParameter(dbCommand, "OrderTotal", DbType.Decimal, OrderTotal);
            db.AddInParameter(dbCommand, "OrderDiscount", DbType.Decimal, OrderDiscount);
            db.AddInParameter(dbCommand, "OrderSubtotalInclTaxInCustomerCurrency", DbType.Decimal, OrderSubtotalInclTaxInCustomerCurrency);
            db.AddInParameter(dbCommand, "OrderSubtotalExclTaxInCustomerCurrency", DbType.Decimal, OrderSubtotalExclTaxInCustomerCurrency);
            db.AddInParameter(dbCommand, "OrderShippingInclTaxInCustomerCurrency", DbType.Decimal, OrderShippingInclTaxInCustomerCurrency);
            db.AddInParameter(dbCommand, "OrderShippingExclTaxInCustomerCurrency", DbType.Decimal, OrderShippingExclTaxInCustomerCurrency);
            db.AddInParameter(dbCommand, "PaymentMethodAdditionalFeeInclTaxInCustomerCurrency", DbType.Decimal, PaymentMethodAdditionalFeeInclTaxInCustomerCurrency);
            db.AddInParameter(dbCommand, "PaymentMethodAdditionalFeeExclTaxInCustomerCurrency", DbType.Decimal, PaymentMethodAdditionalFeeExclTaxInCustomerCurrency);
            db.AddInParameter(dbCommand, "OrderTaxInCustomerCurrency", DbType.Decimal, OrderTaxInCustomerCurrency);
            db.AddInParameter(dbCommand, "OrderTotalInCustomerCurrency", DbType.Decimal, OrderTotalInCustomerCurrency);
            db.AddInParameter(dbCommand, "OrderDiscountInCustomerCurrency", DbType.Decimal, OrderDiscountInCustomerCurrency);
            db.AddInParameter(dbCommand, "CustomerCurrencyCode", DbType.String, CustomerCurrencyCode);
            db.AddInParameter(dbCommand, "OrderWeight", DbType.Decimal, OrderWeight);
            db.AddInParameter(dbCommand, "AffiliateID", DbType.Int32, AffiliateID);
            db.AddInParameter(dbCommand, "OrderStatusID", DbType.Int32, OrderStatusID);
            db.AddInParameter(dbCommand, "AllowStoringCreditCardNumber", DbType.Boolean, AllowStoringCreditCardNumber);
            db.AddInParameter(dbCommand, "CardType", DbType.String, CardType);
            db.AddInParameter(dbCommand, "CardName", DbType.String, CardName);
            db.AddInParameter(dbCommand, "CardNumber", DbType.String, CardNumber);
            db.AddInParameter(dbCommand, "MaskedCreditCardNumber", DbType.String, MaskedCreditCardNumber);
            db.AddInParameter(dbCommand, "CardCVV2", DbType.String, CardCVV2);
            db.AddInParameter(dbCommand, "CardExpirationMonth", DbType.String, CardExpirationMonth);
            db.AddInParameter(dbCommand, "CardExpirationYear", DbType.String, CardExpirationYear);
            db.AddInParameter(dbCommand, "PaymentMethodID", DbType.Int32, PaymentMethodID);
            db.AddInParameter(dbCommand, "PaymentMethodName", DbType.String, PaymentMethodName);
            db.AddInParameter(dbCommand, "AuthorizationTransactionID", DbType.String, AuthorizationTransactionID);
            db.AddInParameter(dbCommand, "AuthorizationTransactionCode", DbType.String, AuthorizationTransactionCode);
            db.AddInParameter(dbCommand, "AuthorizationTransactionResult", DbType.String, AuthorizationTransactionResult);
            db.AddInParameter(dbCommand, "CaptureTransactionID", DbType.String, CaptureTransactionID);
            db.AddInParameter(dbCommand, "CaptureTransactionResult", DbType.String, CaptureTransactionResult);
            db.AddInParameter(dbCommand, "SubscriptionTransactionID", DbType.String, SubscriptionTransactionID);
            db.AddInParameter(dbCommand, "PurchaseOrderNumber", DbType.String, PurchaseOrderNumber);
            db.AddInParameter(dbCommand, "PaymentStatusID", DbType.Int32, PaymentStatusID);
            if (PaidDate.HasValue)
                db.AddInParameter(dbCommand, "PaidDate", DbType.DateTime, PaidDate.Value);
            else
                db.AddInParameter(dbCommand, "PaidDate", DbType.DateTime, DBNull.Value);
            db.AddInParameter(dbCommand, "BillingFirstName", DbType.String, BillingFirstName);
            db.AddInParameter(dbCommand, "BillingLastName", DbType.String, BillingLastName);
            db.AddInParameter(dbCommand, "BillingPhoneNumber", DbType.String, BillingPhoneNumber);
            db.AddInParameter(dbCommand, "BillingEmail", DbType.String, BillingEmail);
            db.AddInParameter(dbCommand, "BillingFaxNumber", DbType.String, BillingFaxNumber);
            db.AddInParameter(dbCommand, "BillingCompany", DbType.String, BillingCompany);
            db.AddInParameter(dbCommand, "BillingAddress1", DbType.String, BillingAddress1);
            db.AddInParameter(dbCommand, "BillingAddress2", DbType.String, BillingAddress2);
            db.AddInParameter(dbCommand, "BillingCity", DbType.String, BillingCity);
            db.AddInParameter(dbCommand, "BillingStateProvince", DbType.String, BillingStateProvince);
            db.AddInParameter(dbCommand, "BillingStateProvinceID", DbType.Int32, BillingStateProvinceID);
            db.AddInParameter(dbCommand, "BillingZipPostalCode", DbType.String, BillingZipPostalCode);
            db.AddInParameter(dbCommand, "BillingCountry", DbType.String, BillingCountry);
            db.AddInParameter(dbCommand, "BillingCountryID", DbType.Int32, BillingCountryID);
            db.AddInParameter(dbCommand, "ShippingStatusID", DbType.Int32, ShippingStatusID);
            db.AddInParameter(dbCommand, "ShippingFirstName", DbType.String, ShippingFirstName);
            db.AddInParameter(dbCommand, "ShippingLastName", DbType.String, ShippingLastName);
            db.AddInParameter(dbCommand, "ShippingPhoneNumber", DbType.String, ShippingPhoneNumber);
            db.AddInParameter(dbCommand, "ShippingEmail", DbType.String, ShippingEmail);
            db.AddInParameter(dbCommand, "ShippingFaxNumber", DbType.String, ShippingFaxNumber);
            db.AddInParameter(dbCommand, "ShippingCompany", DbType.String, ShippingCompany);
            db.AddInParameter(dbCommand, "ShippingAddress1", DbType.String, ShippingAddress1);
            db.AddInParameter(dbCommand, "ShippingAddress2", DbType.String, ShippingAddress2);
            db.AddInParameter(dbCommand, "ShippingCity", DbType.String, ShippingCity);
            db.AddInParameter(dbCommand, "ShippingStateProvince", DbType.String, ShippingStateProvince);
            db.AddInParameter(dbCommand, "ShippingStateProvinceID", DbType.Int32, ShippingStateProvinceID);
            db.AddInParameter(dbCommand, "ShippingZipPostalCode", DbType.String, ShippingZipPostalCode);
            db.AddInParameter(dbCommand, "ShippingCountry", DbType.String, ShippingCountry);
            db.AddInParameter(dbCommand, "ShippingCountryID", DbType.Int32, ShippingCountryID);
            db.AddInParameter(dbCommand, "ShippingMethod", DbType.String, ShippingMethod);
            db.AddInParameter(dbCommand, "ShippingRateComputationMethodID", DbType.Int32, ShippingRateComputationMethodID);
            if (ShippedDate.HasValue)
                db.AddInParameter(dbCommand, "ShippedDate", DbType.DateTime, ShippedDate.Value);
            else
                db.AddInParameter(dbCommand, "ShippedDate", DbType.DateTime, DBNull.Value);
            db.AddInParameter(dbCommand, "TrackingNumber", DbType.String, TrackingNumber);
            db.AddInParameter(dbCommand, "Deleted", DbType.Boolean, Deleted);
            db.AddInParameter(dbCommand, "CreatedOn", DbType.DateTime, CreatedOn);
            if (db.ExecuteNonQuery(dbCommand) > 0)
                order = GetOrderByID(OrderID);

            return order;
        }

        /// <summary>
        /// Gets an order note
        /// </summary>
        /// <param name="OrderNoteID">Order note identifier</param>
        /// <returns>Order note</returns>
        public override DBOrderNote GetOrderNoteByID(int OrderNoteID)
        {
            DBOrderNote orderNote = null;
            if (OrderNoteID == 0)
                return orderNote;
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_OrderNoteLoadByPrimaryKey");
            db.AddInParameter(dbCommand, "OrderNoteID", DbType.Int32, OrderNoteID);
            using (IDataReader dataReader = db.ExecuteReader(dbCommand))
            {
                if (dataReader.Read())
                {
                    orderNote = GetOrderNoteFromReader(dataReader);
                }
            }
            return orderNote;
        }

        /// <summary>
        /// Gets an order notes by order identifier
        /// </summary>
        /// <param name="OrderID">Order identifier</param>
        /// <param name="showHidden">A value indicating whether to show hidden records</param>
        /// <returns>Order note collection</returns>
        public override DBOrderNoteCollection GetOrderNoteByOrderID(int OrderID, bool showHidden)
        {
            var result = new DBOrderNoteCollection();
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_OrderNoteLoadByOrderID");
            db.AddInParameter(dbCommand, "OrderID", DbType.Int32, OrderID);
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
        /// <param name="OrderNoteID">Order note identifier</param>
        public override void DeleteOrderNote(int OrderNoteID)
        {
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_OrderNoteDelete");
            db.AddInParameter(dbCommand, "OrderNoteID", DbType.Int32, OrderNoteID);
            int retValue = db.ExecuteNonQuery(dbCommand);
        }

        /// <summary>
        /// Inserts an order note
        /// </summary>
        /// <param name="OrderID">The order identifier</param>
        /// <param name="Note">The note</param>
        /// <param name="DisplayToCustomer">Value indicating whether the customer can see a note</param>
        /// <param name="CreatedOn">The date and time of order note creation</param>
        /// <returns>Order note</returns>
        public override DBOrderNote InsertOrderNote(int OrderID, string Note, bool DisplayToCustomer, DateTime CreatedOn)
        {
            DBOrderNote orderNote = null;
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_OrderNoteInsert");
            db.AddOutParameter(dbCommand, "OrderNoteID", DbType.Int32, 0);
            db.AddInParameter(dbCommand, "OrderID", DbType.Int32, OrderID);
            db.AddInParameter(dbCommand, "Note", DbType.String, Note);
            db.AddInParameter(dbCommand, "DisplayToCustomer", DbType.Boolean, DisplayToCustomer);
            db.AddInParameter(dbCommand, "CreatedOn", DbType.DateTime, CreatedOn);
            if (db.ExecuteNonQuery(dbCommand) > 0)
            {
                int OrderNoteID = Convert.ToInt32(db.GetParameterValue(dbCommand, "@OrderNoteID"));
                orderNote = GetOrderNoteByID(OrderNoteID);
            }
            return orderNote;
        }

        /// <summary>
        /// Updates the order note
        /// </summary>
        /// <param name="OrderNoteID">The order note identifier</param>
        /// <param name="OrderID">The order identifier</param>
        /// <param name="DisplayToCustomer">Value indicating whether the customer can see a note</param>
        /// <param name="Note">The note</param>
        /// <param name="CreatedOn">The date and time of order note creation</param>
        /// <returns>Order note</returns>
        public override DBOrderNote UpdateOrderNote(int OrderNoteID, int OrderID, string Note, bool DisplayToCustomer, DateTime CreatedOn)
        {
            DBOrderNote orderNote = null;
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_OrderNoteUpdate");
            db.AddInParameter(dbCommand, "OrderNoteID", DbType.Int32, OrderNoteID);
            db.AddInParameter(dbCommand, "OrderID", DbType.Int32, OrderID);
            db.AddInParameter(dbCommand, "Note", DbType.String, Note);
            db.AddInParameter(dbCommand, "DisplayToCustomer", DbType.Boolean, DisplayToCustomer);
            db.AddInParameter(dbCommand, "CreatedOn", DbType.DateTime, CreatedOn);
            if (db.ExecuteNonQuery(dbCommand) > 0)
                orderNote = GetOrderNoteByID(OrderNoteID);

            return orderNote;
        }

        /// <summary>
        /// Gets an order product variant
        /// </summary>
        /// <param name="OrderProductVariantID">Order product variant identifier</param>
        /// <returns>Order product variant</returns>
        public override DBOrderProductVariant GetOrderProductVariantByID(int OrderProductVariantID)
        {
            DBOrderProductVariant orderProductVariant = null;
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_OrderProductVariantLoadByPrimaryKey");
            db.AddInParameter(dbCommand, "OrderProductVariantID", DbType.Int32, OrderProductVariantID);
            using (IDataReader dataReader = db.ExecuteReader(dbCommand))
            {
                if (dataReader.Read())
                {
                    orderProductVariant = GetOrderProductVariantFromReader(dataReader);
                }
            }
            return orderProductVariant;
        }

        /// <summary>
        /// Gets an order product variant
        /// </summary>
        /// <param name="OrderProductVariantGUID">Order product variant identifier</param>
        /// <returns>Order product variant</returns>
        public override DBOrderProductVariant GetOrderProductVariantByGUID(Guid OrderProductVariantGUID)
        {
            DBOrderProductVariant orderProductVariant = null;
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_OrderProductVariantLoadByGUID");
            db.AddInParameter(dbCommand, "OrderProductVariantGUID", DbType.Guid, OrderProductVariantGUID);
            using (IDataReader dataReader = db.ExecuteReader(dbCommand))
            {
                if (dataReader.Read())
                {
                    orderProductVariant = GetOrderProductVariantFromReader(dataReader);
                }
            }
            return orderProductVariant;
        }

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
        public override DBOrderProductVariantCollection GetAllOrderProductVariants(int? OrderID,
            int? CustomerID, DateTime? StartTime, DateTime? EndTime,
            int? OrderStatusID, int? PaymentStatusID, int? ShippingStatusID,
            bool LoadDownloableProductsOnly)
        {
            var result = new DBOrderProductVariantCollection();
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_OrderProductVariantLoadAll");
            if (OrderID.HasValue)
                db.AddInParameter(dbCommand, "OrderID", DbType.Int32, OrderID.Value);
            else
                db.AddInParameter(dbCommand, "OrderID", DbType.Int32, null);
            if (CustomerID.HasValue)
                db.AddInParameter(dbCommand, "CustomerID", DbType.Int32, CustomerID.Value);
            else
                db.AddInParameter(dbCommand, "CustomerID", DbType.Int32, null);
            if (StartTime.HasValue)
                db.AddInParameter(dbCommand, "StartTime", DbType.DateTime, StartTime.Value);
            else
                db.AddInParameter(dbCommand, "StartTime", DbType.DateTime, null);
            if (EndTime.HasValue)
                db.AddInParameter(dbCommand, "EndTime", DbType.DateTime, EndTime.Value);
            else
                db.AddInParameter(dbCommand, "EndTime", DbType.DateTime, null);            
            if (OrderStatusID.HasValue)
                db.AddInParameter(dbCommand, "OrderStatusID", DbType.Int32, OrderStatusID.Value);
            else
                db.AddInParameter(dbCommand, "OrderStatusID", DbType.Int32, null);
            if (PaymentStatusID.HasValue)
                db.AddInParameter(dbCommand, "PaymentStatusID", DbType.Int32, PaymentStatusID.Value);
            else
                db.AddInParameter(dbCommand, "PaymentStatusID", DbType.Int32, null);
            if (ShippingStatusID.HasValue)
                db.AddInParameter(dbCommand, "ShippingStatusID", DbType.Int32, ShippingStatusID);
            else
                db.AddInParameter(dbCommand, "ShippingStatusID", DbType.Int32, null);
            db.AddInParameter(dbCommand, "LoadDownloableProductsOnly", DbType.Boolean, LoadDownloableProductsOnly);
            
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
        public override DBOrderProductVariant InsertOrderProductVariant(Guid OrderProductVariantGUID, 
            int OrderID, int ProductVariantID, decimal UnitPriceInclTax, 
            decimal UnitPriceExclTax, decimal PriceInclTax, decimal PriceExclTax,
            decimal UnitPriceInclTaxInCustomerCurrency, decimal UnitPriceExclTaxInCustomerCurrency,
            decimal PriceInclTaxInCustomerCurrency, decimal PriceExclTaxInCustomerCurrency,
            string AttributeDescription, string AttributesXML, int Quantity,
            decimal DiscountAmountInclTax, decimal DiscountAmountExclTax,
            int DownloadCount, bool IsDownloadActivated, int LicenseDownloadID)
        {
            DBOrderProductVariant orderProductVariant = null;
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_OrderProductVariantInsert");
            db.AddOutParameter(dbCommand, "OrderProductVariantID", DbType.Int32, 0);
            db.AddInParameter(dbCommand, "OrderProductVariantGUID", DbType.Guid, OrderProductVariantGUID);
            db.AddInParameter(dbCommand, "OrderID", DbType.Int32, OrderID);
            db.AddInParameter(dbCommand, "ProductVariantID", DbType.Int32, ProductVariantID);
            db.AddInParameter(dbCommand, "UnitPriceInclTax", DbType.Decimal, UnitPriceInclTax);
            db.AddInParameter(dbCommand, "UnitPriceExclTax", DbType.Decimal, UnitPriceExclTax);
            db.AddInParameter(dbCommand, "PriceInclTax", DbType.Decimal, PriceInclTax);
            db.AddInParameter(dbCommand, "PriceExclTax", DbType.Decimal, PriceExclTax);
            db.AddInParameter(dbCommand, "UnitPriceInclTaxInCustomerCurrency", DbType.Decimal, UnitPriceInclTaxInCustomerCurrency);
            db.AddInParameter(dbCommand, "UnitPriceExclTaxInCustomerCurrency", DbType.Decimal, UnitPriceExclTaxInCustomerCurrency);
            db.AddInParameter(dbCommand, "PriceInclTaxInCustomerCurrency", DbType.Decimal, PriceInclTaxInCustomerCurrency);
            db.AddInParameter(dbCommand, "PriceExclTaxInCustomerCurrency", DbType.Decimal, PriceExclTaxInCustomerCurrency);
            db.AddInParameter(dbCommand, "AttributeDescription", DbType.String, AttributeDescription);
            db.AddInParameter(dbCommand, "AttributesXML", DbType.Xml, AttributesXML);
            db.AddInParameter(dbCommand, "Quantity", DbType.Int32, Quantity);
            db.AddInParameter(dbCommand, "DiscountAmountInclTax", DbType.Decimal, DiscountAmountInclTax);
            db.AddInParameter(dbCommand, "DiscountAmountExclTax", DbType.Decimal, DiscountAmountExclTax);
            db.AddInParameter(dbCommand, "DownloadCount", DbType.Int32, DownloadCount);
            db.AddInParameter(dbCommand, "IsDownloadActivated", DbType.Boolean, IsDownloadActivated);
            db.AddInParameter(dbCommand, "LicenseDownloadID", DbType.Int32, LicenseDownloadID);
            if (db.ExecuteNonQuery(dbCommand) > 0)
            {
                int OrderProductVariantID = Convert.ToInt32(db.GetParameterValue(dbCommand, "@OrderProductVariantID"));
                orderProductVariant = GetOrderProductVariantByID(OrderProductVariantID);
            }
            return orderProductVariant;
        }

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
        public override DBOrderProductVariant UpdateOrderProductVariant(int OrderProductVariantID,
            Guid OrderProductVariantGUID, int OrderID, int ProductVariantID,
            decimal UnitPriceInclTax, decimal UnitPriceExclTax, decimal PriceInclTax, decimal PriceExclTax,
            decimal UnitPriceInclTaxInCustomerCurrency, decimal UnitPriceExclTaxInCustomerCurrency,
            decimal PriceInclTaxInCustomerCurrency, decimal PriceExclTaxInCustomerCurrency,
            string AttributeDescription, string AttributesXML, int Quantity,
            decimal DiscountAmountInclTax, decimal DiscountAmountExclTax,
            int DownloadCount, bool IsDownloadActivated, int LicenseDownloadID)
        {
            DBOrderProductVariant orderProductVariant = null;
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_OrderProductVariantUpdate");
            db.AddInParameter(dbCommand, "OrderProductVariantID", DbType.Int32, OrderProductVariantID);
            db.AddInParameter(dbCommand, "OrderProductVariantGUID", DbType.Guid, OrderProductVariantGUID);
            db.AddInParameter(dbCommand, "OrderID", DbType.Int32, OrderID);
            db.AddInParameter(dbCommand, "ProductVariantID", DbType.Int32, ProductVariantID);
            db.AddInParameter(dbCommand, "UnitPriceInclTax", DbType.Decimal, UnitPriceInclTax);
            db.AddInParameter(dbCommand, "UnitPriceExclTax", DbType.Decimal, UnitPriceExclTax);
            db.AddInParameter(dbCommand, "PriceInclTax", DbType.Decimal, PriceInclTax);
            db.AddInParameter(dbCommand, "PriceExclTax", DbType.Decimal, PriceExclTax);
            db.AddInParameter(dbCommand, "UnitPriceInclTaxInCustomerCurrency", DbType.Decimal, UnitPriceInclTaxInCustomerCurrency);
            db.AddInParameter(dbCommand, "UnitPriceExclTaxInCustomerCurrency", DbType.Decimal, UnitPriceExclTaxInCustomerCurrency);
            db.AddInParameter(dbCommand, "PriceInclTaxInCustomerCurrency", DbType.Decimal, PriceInclTaxInCustomerCurrency);
            db.AddInParameter(dbCommand, "PriceExclTaxInCustomerCurrency", DbType.Decimal, PriceExclTaxInCustomerCurrency);
            db.AddInParameter(dbCommand, "AttributeDescription", DbType.String, AttributeDescription);
            db.AddInParameter(dbCommand, "AttributesXML", DbType.Xml, AttributesXML);
            db.AddInParameter(dbCommand, "Quantity", DbType.Int32, Quantity);
            db.AddInParameter(dbCommand, "DiscountAmountInclTax", DbType.Decimal, DiscountAmountInclTax);
            db.AddInParameter(dbCommand, "DiscountAmountExclTax", DbType.Decimal, DiscountAmountExclTax);
            db.AddInParameter(dbCommand, "DownloadCount", DbType.Int32, DownloadCount);
            db.AddInParameter(dbCommand, "IsDownloadActivated", DbType.Boolean, IsDownloadActivated);
            db.AddInParameter(dbCommand, "LicenseDownloadID", DbType.Int32, LicenseDownloadID); 
            if (db.ExecuteNonQuery(dbCommand) > 0)
                orderProductVariant = GetOrderProductVariantByID(OrderProductVariantID);

            return orderProductVariant;
        }

        /// <summary>
        /// Gets an order status by ID
        /// </summary>
        /// <param name="OrderStatusID">Order status identifier</param>
        /// <returns>Order status</returns>
        public override DBOrderStatus GetOrderStatusByID(int OrderStatusID)
        {

            DBOrderStatus orderStatus = null;
            if (OrderStatusID == 0)
                return orderStatus;
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_OrderStatusLoadByPrimaryKey");
            db.AddInParameter(dbCommand, "OrderStatusID", DbType.Int32, OrderStatusID);
            using (IDataReader dataReader = db.ExecuteReader(dbCommand))
            {
                if (dataReader.Read())
                {
                    orderStatus = GetOrderStatusFromReader(dataReader);
                    orderStatus.Name = NopSqlDataHelper.GetString(dataReader, "Name");
                }
            }
            return orderStatus;
        }

        /// <summary>
        /// Gets all order statuses
        /// </summary>
        /// <returns>Order status collection</returns>
        public override DBOrderStatusCollection GetAllOrderStatuses()
        {
            var result = new DBOrderStatusCollection();
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_OrderStatusLoadAll");
            using (IDataReader dataReader = db.ExecuteReader(dbCommand))
            {
                while (dataReader.Read())
                {
                    var item = GetOrderStatusFromReader(dataReader);
                    result.Add(item);
                }
            }

            return result;
        }

        /// <summary>
        /// Gets an order report
        /// </summary>
        /// <param name="OrderStatusID">Order status identifier; null to load all orders</param>
        /// <param name="PaymentStatusID">Order payment status identifier; null to load all orders</param>
        /// <param name="ShippingStatusID">Order shipping status identifier; null to load all orders</param>
        /// <returns>DataTable</returns>
        public override IDataReader GetOrderReport(int? OrderStatusID, int? PaymentStatusID, int? ShippingStatusID)
        {
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_OrderIncompleteReport");
            if (OrderStatusID.HasValue)
                db.AddInParameter(dbCommand, "OrderStatusID", DbType.Int32, OrderStatusID.Value);
            else
                db.AddInParameter(dbCommand, "OrderStatusID", DbType.Int32, null);
            if (PaymentStatusID.HasValue)
                db.AddInParameter(dbCommand, "PaymentStatusID", DbType.Int32, PaymentStatusID.Value);
            else
                db.AddInParameter(dbCommand, "PaymentStatusID", DbType.Int32, null);

            if (ShippingStatusID.HasValue)
                db.AddInParameter(dbCommand, "ShippingStatusID", DbType.Int32, ShippingStatusID);
            else
                db.AddInParameter(dbCommand, "ShippingStatusID", DbType.Int32, null);

            return db.ExecuteReader(dbCommand);
        }

        /// <summary>
        /// Gets a recurring payment
        /// </summary>
        /// <param name="RecurringPaymentID">The recurring payment identifier</param>
        /// <returns>Recurring payment</returns>
        public override DBRecurringPayment GetRecurringPaymentByID(int RecurringPaymentID)
        {
            DBRecurringPayment recurringPayment = null;
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_RecurringPaymentByPrimaryKey");
            db.AddInParameter(dbCommand, "RecurringPaymentID", DbType.Int32, RecurringPaymentID);
            using (IDataReader dataReader = db.ExecuteReader(dbCommand))
            {
                if (dataReader.Read())
                {
                    recurringPayment = GetRecurringPaymentFromReader(dataReader);
                }
            }
            return recurringPayment;
        }

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
        public override DBRecurringPayment InsertRecurringPayment(int InitialOrderID,
            int CycleLength, int CyclePeriod, int TotalCycles,
            DateTime StartDate, bool IsActive, bool Deleted, DateTime CreatedOn)
        {
            DBRecurringPayment recurringPayment = null;
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_RecurringPaymentInsert");
            db.AddOutParameter(dbCommand, "RecurringPaymentID", DbType.Int32, 0);
            db.AddInParameter(dbCommand, "InitialOrderID", DbType.Int32, InitialOrderID);
            db.AddInParameter(dbCommand, "CycleLength", DbType.Int32, CycleLength);
            db.AddInParameter(dbCommand, "CyclePeriod", DbType.Int32, CyclePeriod);
            db.AddInParameter(dbCommand, "TotalCycles", DbType.Int32, TotalCycles);
            db.AddInParameter(dbCommand, "StartDate", DbType.DateTime, StartDate);
            db.AddInParameter(dbCommand, "IsActive", DbType.Boolean, IsActive);
            db.AddInParameter(dbCommand, "Deleted", DbType.Boolean, Deleted);
            db.AddInParameter(dbCommand, "CreatedOn", DbType.DateTime, CreatedOn);
            if (db.ExecuteNonQuery(dbCommand) > 0)
            {
                int RecurringPaymentID = Convert.ToInt32(db.GetParameterValue(dbCommand, "@RecurringPaymentID"));
                recurringPayment = GetRecurringPaymentByID(RecurringPaymentID);
            }
            return recurringPayment;
        }

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
        public override DBRecurringPayment UpdateRecurringPayment(int RecurringPaymentID,
            int InitialOrderID, int CycleLength, int CyclePeriod, int TotalCycles,
            DateTime StartDate, bool IsActive, bool Deleted, DateTime CreatedOn)
        {
            DBRecurringPayment recurringPayment = null;
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_RecurringPaymentUpdate");
            db.AddInParameter(dbCommand, "RecurringPaymentID", DbType.Int32, RecurringPaymentID);
            db.AddInParameter(dbCommand, "InitialOrderID", DbType.Int32, InitialOrderID);
            db.AddInParameter(dbCommand, "CycleLength", DbType.Int32, CycleLength);
            db.AddInParameter(dbCommand, "CyclePeriod", DbType.Int32, CyclePeriod);
            db.AddInParameter(dbCommand, "TotalCycles", DbType.Int32, TotalCycles);
            db.AddInParameter(dbCommand, "StartDate", DbType.DateTime, StartDate);
            db.AddInParameter(dbCommand, "IsActive", DbType.Boolean, IsActive);
            db.AddInParameter(dbCommand, "Deleted", DbType.Boolean, Deleted);
            db.AddInParameter(dbCommand, "CreatedOn", DbType.DateTime, CreatedOn);
            if (db.ExecuteNonQuery(dbCommand) > 0)
                recurringPayment = GetRecurringPaymentByID(RecurringPaymentID);

            return recurringPayment;
        }

        /// <summary>
        /// Search recurring payments
        /// </summary>
        /// <param name="showHidden">A value indicating whether to show hidden records</param>
        /// <param name="CustomerID">The customer identifier; 0 to load all records</param>
        /// <param name="InitialOrderID">The initial order identifier; 0 to load all records</param>
        /// <param name="InitialOrderStatusID">Initial order status identifier; null to load all records</param>
        /// <returns>Recurring payment collection</returns>
        public override DBRecurringPaymentCollection SearchRecurringPayments(bool showHidden,
            int CustomerID, int InitialOrderID, int? InitialOrderStatusID)
        {
            var result = new DBRecurringPaymentCollection();

            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_RecurringPaymentLoadAll");
            db.AddInParameter(dbCommand, "ShowHidden", DbType.Boolean, showHidden);
            db.AddInParameter(dbCommand, "CustomerID", DbType.Int32, CustomerID);
            db.AddInParameter(dbCommand, "InitialOrderID", DbType.Int32, InitialOrderID);
            if (InitialOrderStatusID.HasValue)
                db.AddInParameter(dbCommand, "InitialOrderStatusID", DbType.Int32, InitialOrderStatusID.Value);
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
        /// <param name="RecurringPaymentHistoryID">Recurring payment history identifier</param>
        public override void DeleteRecurringPaymentHistory(int RecurringPaymentHistoryID)
        {
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_RecurringPaymentHistoryDelete");
            db.AddInParameter(dbCommand, "RecurringPaymentHistoryID", DbType.Int32, RecurringPaymentHistoryID);
            int retValue = db.ExecuteNonQuery(dbCommand);
        }

        /// <summary>
        /// Gets a recurring payment history
        /// </summary>
        /// <param name="RecurringPaymentHistoryID">The recurring payment history identifier</param>
        /// <returns>Recurring payment history</returns>
        public override DBRecurringPaymentHistory GetRecurringPaymentHistoryByID(int RecurringPaymentHistoryID)
        {
            DBRecurringPaymentHistory recurringPaymentHistory = null;
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_RecurringPaymentHistoryLoadByPrimaryKey");
            db.AddInParameter(dbCommand, "RecurringPaymentHistoryID", DbType.Int32, RecurringPaymentHistoryID);
            using (IDataReader dataReader = db.ExecuteReader(dbCommand))
            {
                if (dataReader.Read())
                {
                    recurringPaymentHistory = GetRecurringPaymentHistoryFromReader(dataReader);
                }
            }
            return recurringPaymentHistory;
        }

        /// <summary>
        /// Inserts a recurring payment history
        /// </summary>
        /// <param name="RecurringPaymentID">The recurring payment identifier</param>
        /// <param name="OrderID">The order identifier</param>
        /// <param name="CreatedOn">The date and time of payment creation</param>
        /// <returns>Recurring payment history</returns>
        public override DBRecurringPaymentHistory InsertRecurringPaymentHistory(int RecurringPaymentID,
            int OrderID, DateTime CreatedOn)
        {
            DBRecurringPaymentHistory recurringPaymentHistory = null;
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_RecurringPaymentHistoryInsert");
            db.AddOutParameter(dbCommand, "RecurringPaymentHistoryID", DbType.Int32, 0);
            db.AddInParameter(dbCommand, "RecurringPaymentID", DbType.Int32, RecurringPaymentID);
            db.AddInParameter(dbCommand, "OrderID", DbType.Int32, OrderID);
            db.AddInParameter(dbCommand, "CreatedOn", DbType.DateTime, CreatedOn);
            if (db.ExecuteNonQuery(dbCommand) > 0)
            {
                int RecurringPaymentHistoryID = Convert.ToInt32(db.GetParameterValue(dbCommand, "@RecurringPaymentHistoryID"));
                recurringPaymentHistory = GetRecurringPaymentHistoryByID(RecurringPaymentHistoryID);
            }
            return recurringPaymentHistory;
        }

        /// <summary>
        /// Updates the recurring payment history
        /// </summary>
        /// <param name="RecurringPaymentHistoryID">The recurring payment history identifier</param>
        /// <param name="RecurringPaymentID">The recurring payment identifier</param>
        /// <param name="OrderID">The order identifier</param>
        /// <param name="CreatedOn">The date and time of payment creation</param>
        /// <returns>Recurring payment history</returns>
        public override DBRecurringPaymentHistory UpdateRecurringPaymentHistory(int RecurringPaymentHistoryID,
            int RecurringPaymentID, int OrderID, DateTime CreatedOn)
        {
            DBRecurringPaymentHistory recurringPaymentHistory = null;
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_RecurringPaymentHistoryUpdate");
            db.AddInParameter(dbCommand, "RecurringPaymentHistoryID", DbType.Int32, RecurringPaymentHistoryID);
            db.AddInParameter(dbCommand, "RecurringPaymentID", DbType.Int32, RecurringPaymentID);
            db.AddInParameter(dbCommand, "OrderID", DbType.Int32, OrderID);
            db.AddInParameter(dbCommand, "CreatedOn", DbType.DateTime, CreatedOn);
            if (db.ExecuteNonQuery(dbCommand) > 0)
                recurringPaymentHistory = GetRecurringPaymentHistoryByID(RecurringPaymentHistoryID);

            return recurringPaymentHistory;
        }

        /// <summary>
        /// Search recurring payment history
        /// </summary>
        /// <param name="RecurringPaymentID">The recurring payment identifier; 0 to load all records</param>
        /// <param name="OrderID">The order identifier; 0 to load all records</param>
        /// <returns>Recurring payment history collection</returns>
        public override DBRecurringPaymentHistoryCollection SearchRecurringPaymentHistory(int RecurringPaymentID, int OrderID)
        {
            var result = new DBRecurringPaymentHistoryCollection();

            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_RecurringPaymentHistoryLoadAll");
            db.AddInParameter(dbCommand, "RecurringPaymentID", DbType.Int32, RecurringPaymentID);
            db.AddInParameter(dbCommand, "OrderID", DbType.Int32, OrderID);

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
        /// <param name="GiftCardID">Gift card identifier</param>
        public override void DeleteGiftCard(int GiftCardID)
        {
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_GiftCardDelete");
            db.AddInParameter(dbCommand, "GiftCardID", DbType.Int32, GiftCardID);
            int retValue = db.ExecuteNonQuery(dbCommand);
        }

        /// <summary>
        /// Gets a gift card
        /// </summary>
        /// <param name="GiftCardID">Gift card identifier</param>
        /// <returns>Gift card entry</returns>
        public override DBGiftCard GetGiftCardByID(int GiftCardID)
        {
            DBGiftCard giftCard = null;
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_GiftCardLoadByPrimaryKey");
            db.AddInParameter(dbCommand, "GiftCardID", DbType.Int32, GiftCardID);
            using (IDataReader dataReader = db.ExecuteReader(dbCommand))
            {
                if (dataReader.Read())
                {
                    giftCard = GetGiftCardFromReader(dataReader);
                }
            }
            return giftCard;
        }

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
        public override DBGiftCardCollection GetAllGiftCards(int? OrderID,
            int? CustomerID, DateTime? StartTime, DateTime? EndTime,
            int? OrderStatusID, int? PaymentStatusID, int? ShippingStatusID,
            bool? IsGiftCardActivated, string GiftCardCouponCode)
        {
            var result = new DBGiftCardCollection();
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_GiftCardLoadAll");
            if (OrderID.HasValue)
                db.AddInParameter(dbCommand, "OrderID", DbType.Int32, OrderID.Value);
            else
                db.AddInParameter(dbCommand, "OrderID", DbType.Int32, null);
            if (CustomerID.HasValue)
                db.AddInParameter(dbCommand, "CustomerID", DbType.Int32, CustomerID.Value);
            else
                db.AddInParameter(dbCommand, "CustomerID", DbType.Int32, null);
            if (StartTime.HasValue)
                db.AddInParameter(dbCommand, "StartTime", DbType.DateTime, StartTime.Value);
            else
                db.AddInParameter(dbCommand, "StartTime", DbType.DateTime, null);
            if (EndTime.HasValue)
                db.AddInParameter(dbCommand, "EndTime", DbType.DateTime, EndTime.Value);
            else
                db.AddInParameter(dbCommand, "EndTime", DbType.DateTime, null);
            if (OrderStatusID.HasValue)
                db.AddInParameter(dbCommand, "OrderStatusID", DbType.Int32, OrderStatusID.Value);
            else
                db.AddInParameter(dbCommand, "OrderStatusID", DbType.Int32, null);
            if (PaymentStatusID.HasValue)
                db.AddInParameter(dbCommand, "PaymentStatusID", DbType.Int32, PaymentStatusID.Value);
            else
                db.AddInParameter(dbCommand, "PaymentStatusID", DbType.Int32, null);
            if (ShippingStatusID.HasValue)
                db.AddInParameter(dbCommand, "ShippingStatusID", DbType.Int32, ShippingStatusID);
            else
                db.AddInParameter(dbCommand, "ShippingStatusID", DbType.Int32, null);
            if (IsGiftCardActivated.HasValue)
                db.AddInParameter(dbCommand, "IsGiftCardActivated", DbType.Boolean, IsGiftCardActivated);
            else
                db.AddInParameter(dbCommand, "IsGiftCardActivated", DbType.Boolean, null);
            if (!String.IsNullOrEmpty(GiftCardCouponCode))
                db.AddInParameter(dbCommand, "GiftCardCouponCode", DbType.String, GiftCardCouponCode);
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
        public override DBGiftCard InsertGiftCard(int PurchasedOrderProductVariantID,
            decimal Amount, bool IsGiftCardActivated, string GiftCardCouponCode,
            string RecipientName, string RecipientEmail,
            string SenderName, string SenderEmail, string Message,
            bool IsSenderNotified, DateTime CreatedOn)
        {
            DBGiftCard giftCard = null;
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_GiftCardInsert");
            db.AddOutParameter(dbCommand, "GiftCardID", DbType.Int32, 0);
            db.AddInParameter(dbCommand, "PurchasedOrderProductVariantID", DbType.Int32, PurchasedOrderProductVariantID);
            db.AddInParameter(dbCommand, "Amount", DbType.Decimal, Amount);
            db.AddInParameter(dbCommand, "IsGiftCardActivated", DbType.Boolean, IsGiftCardActivated);
            db.AddInParameter(dbCommand, "GiftCardCouponCode", DbType.String, GiftCardCouponCode);
            db.AddInParameter(dbCommand, "RecipientName", DbType.String, RecipientName);
            db.AddInParameter(dbCommand, "RecipientEmail", DbType.String, RecipientEmail);
            db.AddInParameter(dbCommand, "SenderName", DbType.String, SenderName);
            db.AddInParameter(dbCommand, "SenderEmail", DbType.String, SenderEmail);
            db.AddInParameter(dbCommand, "Message", DbType.String, Message);
            db.AddInParameter(dbCommand, "IsSenderNotified", DbType.Boolean, IsSenderNotified);
            db.AddInParameter(dbCommand, "CreatedOn", DbType.DateTime, CreatedOn);
            if (db.ExecuteNonQuery(dbCommand) > 0)
            {
                int GiftCardID = Convert.ToInt32(db.GetParameterValue(dbCommand, "@GiftCardID"));
                giftCard = GetGiftCardByID(GiftCardID);
            }
            return giftCard;
        }

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
        public override DBGiftCard UpdateGiftCard(int GiftCardID,
            int PurchasedOrderProductVariantID, decimal Amount,
            bool IsGiftCardActivated, string GiftCardCouponCode,
            string RecipientName, string RecipientEmail,
            string SenderName, string SenderEmail, string Message,
            bool IsSenderNotified, DateTime CreatedOn)
        {
            DBGiftCard giftCard = null;
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_GiftCardUpdate");
            db.AddInParameter(dbCommand, "GiftCardID", DbType.Int32, GiftCardID);
            db.AddInParameter(dbCommand, "PurchasedOrderProductVariantID", DbType.Int32, PurchasedOrderProductVariantID);
            db.AddInParameter(dbCommand, "Amount", DbType.Decimal, Amount);
            db.AddInParameter(dbCommand, "IsGiftCardActivated", DbType.Boolean, IsGiftCardActivated);
            db.AddInParameter(dbCommand, "GiftCardCouponCode", DbType.String, GiftCardCouponCode);
            db.AddInParameter(dbCommand, "RecipientName", DbType.String, RecipientName);
            db.AddInParameter(dbCommand, "RecipientEmail", DbType.String, RecipientEmail);
            db.AddInParameter(dbCommand, "SenderName", DbType.String, SenderName);
            db.AddInParameter(dbCommand, "SenderEmail", DbType.String, SenderEmail);
            db.AddInParameter(dbCommand, "Message", DbType.String, Message);
            db.AddInParameter(dbCommand, "IsSenderNotified", DbType.Boolean, IsSenderNotified);
            db.AddInParameter(dbCommand, "CreatedOn", DbType.DateTime, CreatedOn);
            if (db.ExecuteNonQuery(dbCommand) > 0)
                giftCard = GetGiftCardByID(GiftCardID);

            return giftCard;
        }

        /// <summary>
        /// Deletes a gift card usage history entry
        /// </summary>
        /// <param name="GiftCardUsageHistoryID">Gift card usage history entry identifier</param>
        public override void DeleteGiftCardUsageHistory(int GiftCardUsageHistoryID)
        {
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_GiftCardUsageHistoryDelete");
            db.AddInParameter(dbCommand, "GiftCardUsageHistoryID", DbType.Int32, GiftCardUsageHistoryID);
            int retValue = db.ExecuteNonQuery(dbCommand);
        }

        /// <summary>
        /// Gets a gift card usage history entry
        /// </summary>
        /// <param name="GiftCardUsageHistoryID">Gift card usage history entry identifier</param>
        /// <returns>Gift card usage history entry</returns>
        public override DBGiftCardUsageHistory GetGiftCardUsageHistoryByID(int GiftCardUsageHistoryID)
        {
            DBGiftCardUsageHistory giftCardUsageHistory = null;
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_GiftCardUsageHistoryLoadByPrimaryKey");
            db.AddInParameter(dbCommand, "GiftCardUsageHistoryID", DbType.Int32, GiftCardUsageHistoryID);
            using (IDataReader dataReader = db.ExecuteReader(dbCommand))
            {
                if (dataReader.Read())
                {
                    giftCardUsageHistory = GetGiftCardUsageHistoryFromReader(dataReader);
                }
            }
            return giftCardUsageHistory;
        }

        /// <summary>
        /// Gets all gift card usage history entries
        /// </summary>
        /// <param name="GiftCardID">Gift card identifier identifier; null to load all records</param>
        /// <param name="CustomerID">Customer identifier; null to load all records</param>
        /// <param name="OrderID">Order identifier; null to load all records</param>
        /// <returns>Gift card usage history entries</returns>
        public override DBGiftCardUsageHistoryCollection GetAllGiftCardUsageHistoryEntries(int? GiftCardID,
            int? CustomerID, int? OrderID)
        {
            var result = new DBGiftCardUsageHistoryCollection();
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_GiftCardUsageHistoryLoadAll");
            if (GiftCardID.HasValue)
                db.AddInParameter(dbCommand, "GiftCardID", DbType.Int32, GiftCardID.Value);
            else
                db.AddInParameter(dbCommand, "GiftCardID", DbType.Int32, null);
            if (CustomerID.HasValue)
                db.AddInParameter(dbCommand, "CustomerID", DbType.Int32, CustomerID.Value);
            else
                db.AddInParameter(dbCommand, "CustomerID", DbType.Int32, null);
            if (OrderID.HasValue)
                db.AddInParameter(dbCommand, "OrderID", DbType.Int32, OrderID.Value);
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
        /// <param name="GiftCardID">Gift card identifier</param>
        /// <param name="CustomerID">Customer identifier</param>
        /// <param name="OrderID">Order identifier</param>
        /// <param name="UsedValue">Used value</param>
        /// <param name="UsedValueInCustomerCurrency">Used value (customer currency)</param>
        /// <param name="CreatedOn">A date and time of instance creation</param>
        /// <returns>Gift card usage history entry</returns>
        public override DBGiftCardUsageHistory InsertGiftCardUsageHistory(int GiftCardID,
            int CustomerID, int OrderID, decimal UsedValue,
            decimal UsedValueInCustomerCurrency, DateTime CreatedOn)
        {
            DBGiftCardUsageHistory giftCardUsageHistory = null;
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_GiftCardUsageHistoryInsert");
            db.AddOutParameter(dbCommand, "GiftCardUsageHistoryID", DbType.Int32, 0);
            db.AddInParameter(dbCommand, "GiftCardID", DbType.Int32, GiftCardID);
            db.AddInParameter(dbCommand, "CustomerID", DbType.Int32, CustomerID);
            db.AddInParameter(dbCommand, "OrderID", DbType.Int32, OrderID);
            db.AddInParameter(dbCommand, "UsedValue", DbType.Decimal, UsedValue);
            db.AddInParameter(dbCommand, "UsedValueInCustomerCurrency", DbType.Decimal, UsedValueInCustomerCurrency);
            db.AddInParameter(dbCommand, "CreatedOn", DbType.DateTime, CreatedOn);
            if (db.ExecuteNonQuery(dbCommand) > 0)
            {
                int GiftCardUsageHistoryID = Convert.ToInt32(db.GetParameterValue(dbCommand, "@GiftCardUsageHistoryID"));
                giftCardUsageHistory = GetGiftCardUsageHistoryByID(GiftCardUsageHistoryID);
            }
            return giftCardUsageHistory;
        }

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
        public override DBGiftCardUsageHistory UpdateGiftCardUsageHistory(int GiftCardUsageHistoryID,
            int GiftCardID, int CustomerID, int OrderID, decimal UsedValue,
            decimal UsedValueInCustomerCurrency, DateTime CreatedOn)
        {
            DBGiftCardUsageHistory giftCardUsageHistory = null;
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_GiftCardUsageHistoryUpdate");
            db.AddInParameter(dbCommand, "GiftCardUsageHistoryID", DbType.Int32, GiftCardUsageHistoryID);
            db.AddInParameter(dbCommand, "GiftCardID", DbType.Int32, GiftCardID);
            db.AddInParameter(dbCommand, "CustomerID", DbType.Int32, CustomerID);
            db.AddInParameter(dbCommand, "OrderID", DbType.Int32, OrderID);
            db.AddInParameter(dbCommand, "UsedValue", DbType.Decimal, UsedValue);
            db.AddInParameter(dbCommand, "UsedValueInCustomerCurrency", DbType.Decimal, UsedValueInCustomerCurrency);
            db.AddInParameter(dbCommand, "CreatedOn", DbType.DateTime, CreatedOn);
            if (db.ExecuteNonQuery(dbCommand) > 0)
                giftCardUsageHistory = GetGiftCardUsageHistoryByID(GiftCardUsageHistoryID);

            return giftCardUsageHistory;
        }
        #endregion
    }
}
