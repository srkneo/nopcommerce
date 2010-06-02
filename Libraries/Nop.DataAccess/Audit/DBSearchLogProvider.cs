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
using System.Data;
using System.Data.Common;
using System.Globalization;
using System.Text;
using System.Configuration.Provider;
using System.Configuration;
using System.Web.Hosting;
using System.Web.Configuration;
using System.Collections.Specialized;

namespace NopSolutions.NopCommerce.DataAccess.Audit
{
    /// <summary>
    /// Acts as a base class for deriving custom search log provider
    /// </summary>
    [DBProviderSectionName("nopDataProviders/SearchLogProvider")]
    public abstract partial class DBSearchLogProvider : BaseDBProvider
    {
        #region Methods
        /// <summary>
        /// Get order product variant sales report
        /// </summary>
        /// <param name="startTime">Order start time; null to load all</param>
        /// <param name="endTime">Order end time; null to load all</param>
        /// <param name="count">Item count. 0 if you want to get all items</param>
        /// <returns>Result</returns>
        public abstract IDataReader SearchTermReport(DateTime? startTime, 
            DateTime? endTime, int count);
       
        /// <summary>
        /// Gets all search log items
        /// </summary>
        /// <returns>Search log collection</returns>
        public abstract DBSearchLogCollection GetAllSearchLogs();

        /// <summary>
        /// Gets a search log item
        /// </summary>
        /// <param name="searchLogId">The search log item identifier</param>
        /// <returns>Search log item</returns>
        public abstract DBSearchLog GetSearchLogById(int searchLogId);

        /// <summary>
        /// Inserts a search log item
        /// </summary>
        /// <param name="searchTerm">The search term</param>
        /// <param name="customerId">The customer identifier</param>
        /// <param name="createdOn">The date and time of instance creation</param>
        /// <returns>Search log item</returns>
        public abstract DBSearchLog InsertSearchLog(string searchTerm, 
            int customerId, DateTime createdOn);

        /// <summary>
        /// Clear search log
        /// </summary>
        public abstract void ClearSearchLog();
        #endregion
    }
}
