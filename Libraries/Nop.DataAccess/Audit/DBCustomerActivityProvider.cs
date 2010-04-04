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
        /// <param name="SystemKeyword">The system keyword</param>
        /// <param name="Name">The display name</param>
        /// <param name="Enabled">Value indicating whether the activity log type is enabled</param>
        /// <returns>Activity log type item</returns>
        public abstract DBActivityLogType InsertActivityType(string SystemKeyword, string Name, bool Enabled);
       
        /// <summary>
        /// Updates an activity log type item
        /// </summary>
        /// <param name="ActivityLogTypeID">Activity log type identifier</param>
        /// <param name="SystemKeyword">The system keyword</param>
        /// <param name="Name">The display name</param>
        /// <param name="Enabled">Value indicating whether the activity log type is enabled</param>
        /// <returns>Activity log type item</returns>
        public abstract DBActivityLogType UpdateActivityType(int ActivityLogTypeID, string SystemKeyword, string Name, bool Enabled);
        
        /// <summary>
        /// Deletes an activity log type item
        /// </summary>
        /// <param name="ActivityLogTypeID">Activity log type identifier</param>
        public abstract void DeleteActivityType(int ActivityLogTypeID);
        
        /// <summary>
        /// Gets all activity log type items
        /// </summary>
        /// <returns>Activity log type collection</returns>
        public abstract DBActivityLogTypeCollection GetAllActivityTypes();
        
        /// <summary>
        /// Gets an activity log type item
        /// </summary>
        /// <param name="ActivityLogTypeID">Activity log type identifier</param>
        /// <returns>Activity log type item</returns>
        public abstract DBActivityLogType GetActivityTypeByID(int ActivityLogTypeID);

        /// <summary>
        /// Inserts an activity log item
        /// </summary>
        /// <param name="ActivityLogTypeID">Activity log type identifier</param>
        /// <param name="CustomerID">The customer identifier</param>
        /// <param name="Comment">The activity comment</param>
        /// <param name="CreatedOn">The date and time of instance creation</param>
        /// <returns>Activity log item</returns>
        public abstract DBActivityLog InsertActivity(int ActivityLogTypeID, int CustomerID, string Comment, DateTime CreatedOn);
        
        /// <summary>
        /// Updates an activity log 
        /// </summary>
        /// <param name="ActivityLogID">Activity log identifier</param>
        /// <param name="ActivityLogTypeID">Activity log type identifier</param>
        /// <param name="CustomerID">The customer identifier</param>
        /// <param name="Comment">The activity comment</param>
        /// <param name="CreatedOn">The date and time of instance creation</param>
        /// <returns>Activity log item</returns>
        public abstract DBActivityLog UpdateActivity(int ActivityLogID, int ActivityLogTypeID, int CustomerID, string Comment, DateTime CreatedOn);
        
        /// <summary>
        /// Deletes an activity log item
        /// </summary>
        /// <param name="ActivityLogID">Activity log type identifier</param>
        public abstract void DeleteActivity(int ActivityLogID);
        
        /// <summary>
        /// Gets all activity log items
        /// </summary>
        /// <param name="CreatedOnFrom">Log item creation from; null to load all customers</param>
        /// <param name="CreatedOnTo">Log item creation to; null to load all customers</param>
        /// <param name="Email">Customer Email</param>
        /// <param name="Username">Customer username</param>
        /// <param name="ActivityLogTypeID">Activity log type identifier</param>
        /// <param name="PageSize">Page size</param>
        /// <param name="PageIndex">Page index</param>
        /// <param name="TotalRecords">Total records</param>
        /// <returns>Activity log collection</returns>
        public abstract DBActivityLogCollection GetAllActivities(DateTime? CreatedOnFrom, DateTime? CreatedOnTo,
            string Email, string Username, int ActivityLogTypeID, 
            int PageSize, int PageIndex, out int TotalRecords);
        
        /// <summary>
        /// Gets an activity log item
        /// </summary>
        /// <param name="ActivityLogID">Activity log identifier</param>
        /// <returns>Activity log item</returns>
        public abstract DBActivityLog GetActivityByID(int ActivityLogID);

        /// <summary>
        /// Clears activity log
        /// </summary>
        public abstract void ClearAllActivities();
        #endregion
    }
}
