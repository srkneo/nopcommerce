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
using NopSolutions.NopCommerce.BusinessLogic.Caching;
using NopSolutions.NopCommerce.BusinessLogic.Profile;
using NopSolutions.NopCommerce.DataAccess;
using NopSolutions.NopCommerce.DataAccess.Audit;

namespace NopSolutions.NopCommerce.BusinessLogic.Audit
{
    /// <summary>
    /// Search log manager
    /// </summary>
    public partial class SearchLogManager
    {
        #region Utilities
        private static SearchLogCollection DBMapping(DBSearchLogCollection dbCollection)
        {
            if (dbCollection == null)
                return null;

            var collection = new SearchLogCollection();
            foreach (var dbItem in dbCollection)
            {
                var item = DBMapping(dbItem);
                collection.Add(item);
            }

            return collection;
        }

        private static SearchLog DBMapping(DBSearchLog dbItem)
        {
            if (dbItem == null)
                return null;

            var item = new SearchLog();
            item.SearchLogId = dbItem.SearchLogId;
            item.SearchTerm = dbItem.SearchTerm;
            item.CustomerId = dbItem.CustomerId;
            item.CreatedOn = dbItem.CreatedOn;

            return item;
        }
        #endregion

        #region Methods

        /// <summary>
        /// Get search term stats
        /// </summary>
        /// <param name="startTime">Start time; null to load all</param>
        /// <param name="endTime">End time; null to load all</param>
        /// <param name="count">Item count. 0 if you want to get all items</param>
        /// <returns>Result</returns>
        public static IDataReader SearchTermReport(DateTime? startTime, 
            DateTime? endTime, int count)
        {
            return DBProviderManager<DBSearchLogProvider>.Provider.SearchTermReport(startTime,
                endTime, count);
        }

        /// <summary>
        /// Gets all search log items
        /// </summary>
        /// <returns>Search log collection</returns>
        public static SearchLogCollection GetAllSearchLogs()
        {
            var dbCollection = DBProviderManager<DBSearchLogProvider>.Provider.GetAllSearchLogs();
            var collection = DBMapping(dbCollection);
            return collection;
        }

        /// <summary>
        /// Gets a search log item
        /// </summary>
        /// <param name="searchLogId">The search log item identifier</param>
        /// <returns>Search log item</returns>
        public static SearchLog GetSearchLogById(int searchLogId)
        {
            if (searchLogId == 0)
                return null;

            var dbItem = DBProviderManager<DBSearchLogProvider>.Provider.GetSearchLogById(searchLogId);
            var item = DBMapping(dbItem);
            return item;
        }

        /// <summary>
        /// Inserts a search log item
        /// </summary>
        /// <param name="searchTerm">The search term</param>
        /// <param name="customerId">The customer identifier</param>
        /// <param name="createdOn">The date and time of instance creation</param>
        /// <returns>Search log item</returns>
        public static SearchLog InsertSearchLog(string searchTerm,
            int customerId, DateTime createdOn)
        {
            createdOn = DateTimeHelper.ConvertToUtcTime(createdOn);

            var dbItem = DBProviderManager<DBSearchLogProvider>.Provider.InsertSearchLog(searchTerm,
                customerId, createdOn);
            var item = DBMapping(dbItem);
            return item;
        }
        /// <summary>
        /// Clear search log
        /// </summary>
        public static void ClearSearchLog()
        {
            DBProviderManager<DBSearchLogProvider>.Provider.ClearSearchLog();
        }
        #endregion
    }
}
