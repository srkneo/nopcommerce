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
using System.Web.Hosting;
using System.Web.Configuration;
using System.Configuration;
using System.Collections.Specialized;

namespace NopSolutions.NopCommerce.DataAccess.Audit
{
    /// <summary>
    /// Acts as a base class for deriving custom customer activity provider
    /// </summary>
    [DBProviderSectionName("nopDataProviders/CustomerActivityProvider")]
    public abstract partial class DBCustomerActivityProvider : BaseDBProvider
    {
        #region Methods
        /// <summary>
        /// Inserts an activity log type item
        /// </summary>
        /// <param name="systemKeyword">The system keyword</param>
        /// <param name="name">The display name</param>
        /// <param name="enabled">Value indicating whether the activity log type is enabled</param>
        /// <returns>Activity log type item</returns>
        public abstract DBActivityLogType InsertActivityType(string systemKeyword, 
            string name, bool enabled);
       
        /// <summary>
        /// Updates an activity log type item
        /// </summary>
        /// <param name="activityLogTypeId">Activity log type identifier</param>
        /// <param name="systemKeyword">The system keyword</param>
        /// <param name="name">The display name</param>
        /// <param name="enabled">Value indicating whether the activity log type is enabled</param>
        /// <returns>Activity log type item</returns>
        public abstract DBActivityLogType UpdateActivityType(int activityLogTypeId, 
            string systemKeyword, string name, bool enabled);
        
        /// <summary>
        /// Deletes an activity log type item
        /// </summary>
        /// <param name="activityLogTypeId">Activity log type identifier</param>
        public abstract void DeleteActivityType(int activityLogTypeId);
        
        /// <summary>
        /// Gets all activity log type items
        /// </summary>
        /// <returns>Activity log type collection</returns>
        public abstract DBActivityLogTypeCollection GetAllActivityTypes();
        
        /// <summary>
        /// Gets an activity log type item
        /// </summary>
        /// <param name="activityLogTypeId">Activity log type identifier</param>
        /// <returns>Activity log type item</returns>
        public abstract DBActivityLogType GetActivityTypeById(int activityLogTypeId);

        /// <summary>
        /// Inserts an activity log item
        /// </summary>
        /// <param name="activityLogTypeId">Activity log type identifier</param>
        /// <param name="customerId">The customer identifier</param>
        /// <param name="comment">The activity comment</param>
        /// <param name="createdOn">The date and time of instance creation</param>
        /// <returns>Activity log item</returns>
        public abstract DBActivityLog InsertActivity(int activityLogTypeId, 
            int customerId, string comment, DateTime createdOn);
        
        /// <summary>
        /// Updates an activity log 
        /// </summary>
        /// <param name="activityLogId">Activity log identifier</param>
        /// <param name="activityLogTypeId">Activity log type identifier</param>
        /// <param name="customerId">The customer identifier</param>
        /// <param name="comment">The activity comment</param>
        /// <param name="createdOn">The date and time of instance creation</param>
        /// <returns>Activity log item</returns>
        public abstract DBActivityLog UpdateActivity(int activityLogId, 
            int activityLogTypeId, int customerId, string comment, DateTime createdOn);
        
        /// <summary>
        /// Deletes an activity log item
        /// </summary>
        /// <param name="activityLogId">Activity log type identifier</param>
        public abstract void DeleteActivity(int activityLogId);
        
        /// <summary>
        /// Gets all activity log items
        /// </summary>
        /// <param name="createdOnFrom">Log item creation from; null to load all customers</param>
        /// <param name="createdOnTo">Log item creation to; null to load all customers</param>
        /// <param name="email">Customer Email</param>
        /// <param name="username">Customer username</param>
        /// <param name="activityLogTypeId">Activity log type identifier</param>
        /// <param name="pageSize">Page size</param>
        /// <param name="pageIndex">Page index</param>
        /// <param name="totalRecords">Total records</param>
        /// <returns>Activity log collection</returns>
        public abstract DBActivityLogCollection GetAllActivities(DateTime? createdOnFrom, 
            DateTime? createdOnTo, string email, string username, int activityLogTypeId, 
            int pageSize, int pageIndex, out int totalRecords);
        
        /// <summary>
        /// Gets an activity log item
        /// </summary>
        /// <param name="activityLogId">Activity log identifier</param>
        /// <returns>Activity log item</returns>
        public abstract DBActivityLog GetActivityById(int activityLogId);

        /// <summary>
        /// Clears activity log
        /// </summary>
        public abstract void ClearAllActivities();
        #endregion
    }
}
