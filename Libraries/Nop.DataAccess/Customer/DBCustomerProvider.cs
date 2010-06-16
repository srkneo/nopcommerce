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
using System.Web.Configuration;
using System.Web.Hosting;


namespace NopSolutions.NopCommerce.DataAccess.CustomerManagement
{
    /// <summary>
    /// Acts as a base class for deriving custom customer provider
    /// </summary>
    [DBProviderSectionName("nopDataProviders/CustomerProvider")]
    public abstract partial class DBCustomerProvider : BaseDBProvider
    {
        #region Methods

        /// <summary>
        /// Gets all customers
        /// </summary>
        /// <param name="registrationFrom">Customer registration from; null to load all customers</param>
        /// <param name="registrationTo">Customer registration to; null to load all customers</param>
        /// <param name="email">Customer Email</param>
        /// <param name="username">Customer username</param>
        /// <param name="dontLoadGuestCustomers">A value indicating whether to don't load guest customers</param>
        /// <param name="pageSize">Page size</param>
        /// <param name="pageIndex">Page index</param>
        /// <param name="totalRecords">Total records</param>
        /// <returns>Customer collection</returns>
        public abstract DBCustomerCollection GetAllCustomers(DateTime? registrationFrom, 
            DateTime? registrationTo, string email, string username,
            bool dontLoadGuestCustomers, int pageSize, int pageIndex, out int totalRecords);

        /// <summary>
        /// Get best customers
        /// </summary>
        /// <param name="startTime">Order start time; null to load all</param>
        /// <param name="endTime">Order end time; null to load all</param>
        /// <param name="orderStatusId">Order status identifier; null to load all records</param>
        /// <param name="paymentStatusId">Order payment status identifier; null to load all records</param>
        /// <param name="shippingStatusId">Order shipping status identifier; null to load all records</param>
        /// <param name="orderBy">1 - order by order total, 2 - order by number of orders</param>
        /// <returns>Report</returns>
        public abstract IDataReader GetBestCustomersReport(DateTime? startTime,
            DateTime? endTime, int? orderStatusId, int? paymentStatusId, 
            int? shippingStatusId, int orderBy);

        /// <summary>
        /// Get customer report by language
        /// </summary>
        /// <returns>Report</returns>
        public abstract IDataReader GetCustomerReportByLanguage();

        /// <summary>
        /// Get customer report by attribute key
        /// </summary>
        /// <param name="customerAttributeKey">Customer attribute key</param>
        /// <returns>Report</returns>
        public abstract IDataReader GetCustomerReportByAttributeKey(string customerAttributeKey);

        /// <summary>
        /// Deletes all expired customer sessions
        /// </summary>
        /// <param name="olderThan">Older than date and time</param>
        public abstract void DeleteExpiredCustomerSessions(DateTime olderThan);

        #endregion
    }
}
