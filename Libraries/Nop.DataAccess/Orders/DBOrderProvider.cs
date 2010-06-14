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

        #endregion

        #region Orders product variants
        
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
        /// Gets all gift card usage history entries
        /// </summary>
        /// <param name="giftCardId">Gift card identifier identifier; null to load all records</param>
        /// <param name="customerId">Customer identifier; null to load all records</param>
        /// <param name="orderId">Order identifier; null to load all records</param>
        /// <returns>Gift card usage history entries</returns>
        public abstract DBGiftCardUsageHistoryCollection GetAllGiftCardUsageHistoryEntries(int? giftCardId,
            int? customerId, int? orderId);

        #endregion

        #region Reward points

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

        #endregion

        #endregion
    }
}
