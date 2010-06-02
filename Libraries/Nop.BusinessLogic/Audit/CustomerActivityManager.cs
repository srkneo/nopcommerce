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
using System.Linq;
using System.Text;
using NopSolutions.NopCommerce.DataAccess.Audit;
using NopSolutions.NopCommerce.DataAccess;
using NopSolutions.NopCommerce.BusinessLogic.Caching;
using NopSolutions.NopCommerce.BusinessLogic.Profile;

namespace NopSolutions.NopCommerce.BusinessLogic.Audit
{
    /// <summary>
    /// Customer activity manager
    /// </summary>
    public class CustomerActivityManager
    {
        #region Constants
        private const string ACTIVITYTYPE_ALL_KEY = "Nop.activitytype.all";
        private const string ACTIVITYTYPE_BY_ID_KEY = "Nop.activitytype.id-{0}";
        private const string ACTIVITYTYPE_PATTERN_KEY = "Nop.activitytype.";
        #endregion

        #region Utilities
        private static ActivityLogTypeCollection DBMapping(DBActivityLogTypeCollection dbCollection)
        {
            if (dbCollection == null)
                return null;

            var collection = new ActivityLogTypeCollection();
            foreach (var dbItem in dbCollection)
            {
                var item = DBMapping(dbItem);
                collection.Add(item);
            }

            return collection;
        }
        
        private static ActivityLogType DBMapping(DBActivityLogType dbItem)
        {
            if (dbItem == null)
                return null;

            var item = new ActivityLogType();
            item.ActivityLogTypeId = dbItem.ActivityLogTypeId;
            item.SystemKeyword = dbItem.SystemKeyword;
            item.Name = dbItem.Name;
            item.Enabled = dbItem.Enabled;

            return item;
        }

        private static ActivityLogCollection DBMapping(DBActivityLogCollection dbCollection)
        {
            if (dbCollection == null)
                return null;

            var collection = new ActivityLogCollection();
            foreach (var dbItem in dbCollection)
            {
                var item = DBMapping(dbItem);
                collection.Add(item);
            }

            return collection;
        }
        
        private static ActivityLog DBMapping(DBActivityLog dbItem)
        {
            if (dbItem == null)
                return null;

            var item = new ActivityLog();
            item.ActivityLogId = dbItem.ActivityLogId;
            item.ActivityLogTypeId = dbItem.ActivityLogTypeId;
            item.CustomerId = dbItem.CustomerId;
            item.Comment = dbItem.Comment;
            item.CreatedOn = dbItem.CreatedOn;

            return item;
        }
        #endregion

        #region Methods
        /// <summary>
        /// Inserts an activity log type item
        /// </summary>
        /// <param name="systemKeyword">The system keyword</param>
        /// <param name="name">The display name</param>
        /// <param name="enabled">Value indicating whether the activity log type is enabled</param>
        /// <returns>Activity log type item</returns>
        public static ActivityLogType InsertActivityType(string systemKeyword,
            string name, bool enabled)
        {
            var dbItem = DBProviderManager<DBCustomerActivityProvider>.Provider.InsertActivityType(systemKeyword, name, enabled);
            var activityType = DBMapping(dbItem);

            if (NopCache.IsEnabled)
                NopCache.RemoveByPattern(ACTIVITYTYPE_PATTERN_KEY);
            
            return activityType;
        }

        /// <summary>
        /// Updates an activity log type item
        /// </summary>
        /// <param name="activityLogTypeId">Activity log type identifier</param>
        /// <param name="systemKeyword">The system keyword</param>
        /// <param name="name">The display name</param>
        /// <param name="enabled">Value indicating whether the activity log type is enabled</param>
        /// <returns>Activity log type item</returns>
        public static ActivityLogType UpdateActivityType(int activityLogTypeId,
            string systemKeyword, string name, bool enabled)
        {
            var dbItem = DBProviderManager<DBCustomerActivityProvider>.Provider.UpdateActivityType(activityLogTypeId, 
                systemKeyword, name, enabled);
            var activityType = DBMapping(dbItem);

            if (NopCache.IsEnabled)
                NopCache.RemoveByPattern(ACTIVITYTYPE_PATTERN_KEY);
            
            return activityType;
        }

        /// <summary>
        /// Updates an activity log type item
        /// </summary>
        /// <param name="activityLogTypeId">Activity log type identifier</param>
        /// <param name="enabled">Value indicating whether the activity log type is enabled</param>
        /// <returns>Activity log type item</returns>
        public static ActivityLogType UpdateActivityType(int activityLogTypeId, bool enabled)
        {
            var activityType = GetActivityTypeById(activityLogTypeId);
            if (activityType == null || activityType.Enabled == enabled)
                return activityType;

            return UpdateActivityType(activityType.ActivityLogTypeId, 
                activityType.SystemKeyword, activityType.Name, enabled);
        }
        
        /// <summary>
        /// Deletes an activity log type item
        /// </summary>
        /// <param name="activityLogTypeId">Activity log type identifier</param>
        public static void DeleteActivityType(int activityLogTypeId)
        {
            if (NopCache.IsEnabled)
                NopCache.RemoveByPattern(ACTIVITYTYPE_PATTERN_KEY);
            
            DBProviderManager<DBCustomerActivityProvider>.Provider.DeleteActivityType(activityLogTypeId);
        }
        
        /// <summary>
        /// Gets all activity log type items
        /// </summary>
        /// <returns>Activity log type collection</returns>
        public static ActivityLogTypeCollection GetAllActivityTypes()
        {
            if (NopCache.IsEnabled)
            {
                object cache = NopCache.Get(ACTIVITYTYPE_ALL_KEY);
                if (cache != null)
                    return (ActivityLogTypeCollection)cache;
            }

            var dbCollection = DBProviderManager<DBCustomerActivityProvider>.Provider.GetAllActivityTypes();
            var collection = DBMapping(dbCollection);

            if (NopCache.IsEnabled)
                NopCache.Max(ACTIVITYTYPE_ALL_KEY, collection);
            
            return collection;
        }
        
        /// <summary>
        /// Gets an activity log type item
        /// </summary>
        /// <param name="activityLogTypeId">Activity log type identifier</param>
        /// <returns>Activity log type item</returns>
        public static ActivityLogType GetActivityTypeById(int activityLogTypeId)
        {
            if (activityLogTypeId == 0)
                return null;

            string key = string.Format(ACTIVITYTYPE_BY_ID_KEY, activityLogTypeId);
            if (NopCache.IsEnabled)
            {
                object cache = NopCache.Get(key);
                if (cache != null)
                    return (ActivityLogType)cache;
            }

            var dbItem = DBProviderManager<DBCustomerActivityProvider>.Provider.GetActivityTypeById(activityLogTypeId);
            var activityLogType = DBMapping(dbItem);

            if (NopCache.IsEnabled)
                NopCache.Max(key, activityLogType);
            
            return activityLogType;
        }

        /// <summary>
        /// Inserts an activity log item
        /// </summary>
        /// <param name="systemKeyword">The system keyword</param>
        /// <param name="comment">The activity comment</param>
        /// <returns>Activity log item</returns>
        public static ActivityLog InsertActivity(string systemKeyword, string comment)
        {
            return InsertActivity(systemKeyword, comment, new object[0]);
        }

        /// <summary>
        /// Inserts an activity log item
        /// </summary>
        /// <param name="systemKeyword">The system keyword</param>
        /// <param name="comment">The activity comment</param>
        /// <param name="commentParams">The activity comment parameters for string.Format() function.</param>
        /// <returns>Activity log item</returns>
        public static ActivityLog InsertActivity(string systemKeyword, 
            string comment, params object[] commentParams)
        {
            if (NopContext.Current == null || 
                NopContext.Current.User == null ||
                NopContext.Current.User.IsGuest)
                return null;

            var activityTypes = GetAllActivityTypes();
            var activityType = activityTypes.FindBySystemKeyword(systemKeyword);
            if (activityType == null || !activityType.Enabled)
                return null;

            int customerId = NopContext.Current.User.CustomerId;
            DateTime createdOn = DateTimeHelper.ConvertToUtcTime(DateTime.Now);
            comment = string.Format(comment, commentParams);

            var dbItem = DBProviderManager<DBCustomerActivityProvider>.Provider.InsertActivity(activityType.ActivityLogTypeId, 
                customerId, comment, createdOn);
            var activity = DBMapping(dbItem);
            return activity;
        }
        
        /// <summary>
        /// Updates an activity log 
        /// </summary>
        /// <param name="activityLogId">Activity log identifier</param>
        /// <param name="activityLogTypeId">Activity log type identifier</param>
        /// <param name="customerId">The customer identifier</param>
        /// <param name="comment">The activity comment</param>
        /// <param name="createdOn">The date and time of instance creation</param>
        /// <returns>Activity log item</returns>
        public static ActivityLog UpdateActivity(int activityLogId, int activityLogTypeId,
            int customerId, string comment, DateTime createdOn)
        {
            var dbItem = DBProviderManager<DBCustomerActivityProvider>.Provider.UpdateActivity(activityLogId, 
                activityLogTypeId, customerId, comment, createdOn);
            var activity = DBMapping(dbItem);
            return activity;
        }
        
        /// <summary>
        /// Deletes an activity log item
        /// </summary>
        /// <param name="activityLogId">Activity log type identifier</param>
        public static void DeleteActivity(int activityLogId)
        {
            DBProviderManager<DBCustomerActivityProvider>.Provider.DeleteActivity(activityLogId);
        }

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
        public static ActivityLogCollection GetAllActivities(DateTime? createdOnFrom,
            DateTime? createdOnTo, string email, string username, int activityLogTypeId,
            int pageSize, int pageIndex, out int totalRecords)
        {
            if (pageSize <= 0)
                pageSize = 10;
            if (pageSize == int.MaxValue)
                pageSize = int.MaxValue - 1;

            if (pageIndex < 0)
                pageIndex = 0;
            if (pageIndex == int.MaxValue)
                pageIndex = int.MaxValue - 1;

            var dbCollection = DBProviderManager<DBCustomerActivityProvider>.Provider.GetAllActivities(
                createdOnFrom, createdOnTo, email, username, activityLogTypeId, 
                pageSize, pageIndex, out totalRecords);
            var collection = DBMapping(dbCollection);
            return collection;
        }
        
        /// <summary>
        /// Gets an activity log item
        /// </summary>
        /// <param name="activityLogId">Activity log identifier</param>
        /// <returns>Activity log item</returns>
        public static ActivityLog GetActivityById(int activityLogId)
        {
            if (activityLogId == 0)
                return null;

            var dbItem = DBProviderManager<DBCustomerActivityProvider>.Provider.GetActivityById(activityLogId);
            var activityLog = DBMapping(dbItem);
            return activityLog;
        }

        /// <summary>
        /// Clears activity log
        /// </summary>
        public static void ClearAllActivities()
        {
            DBProviderManager<DBCustomerActivityProvider>.Provider.ClearAllActivities();
        }
        #endregion
    }
}
