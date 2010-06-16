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
