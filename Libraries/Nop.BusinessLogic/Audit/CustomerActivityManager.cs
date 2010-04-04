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
            item.ActivityLogTypeID = dbItem.ActivityLogTypeID;
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
            item.ActivityLogID = dbItem.ActivityLogID;
            item.ActivityLogTypeID = dbItem.ActivityLogTypeID;
            item.CustomerID = dbItem.CustomerID;
            item.Comment = dbItem.Comment;
            item.CreatedOn = dbItem.CreatedOn;

            return item;
        }
        #endregion

        #region Methods
        /// <summary>
        /// Inserts an activity log type item
        /// </summary>
        /// <param name="SystemKeyword">The system keyword</param>
        /// <param name="Name">The display name</param>
        /// <param name="Enabled">Value indicating whether the activity log type is enabled</param>
        /// <returns>Activity log type item</returns>
        public static ActivityLogType InsertActivityType(string SystemKeyword, string Name, bool Enabled)
        {
            var dbItem = DBProviderManager<DBCustomerActivityProvider>.Provider.InsertActivityType(SystemKeyword, Name, Enabled);
            var activityType = DBMapping(dbItem);

            if (NopCache.IsEnabled)
                NopCache.RemoveByPattern(ACTIVITYTYPE_PATTERN_KEY);
            
            return activityType;
        }
        
        /// <summary>
        /// Updates an activity log type item
        /// </summary>
        /// <param name="ActivityLogTypeID">Activity log type identifier</param>
        /// <param name="SystemKeyword">The system keyword</param>
        /// <param name="Name">The display name</param>
        /// <param name="Enabled">Value indicating whether the activity log type is enabled</param>
        /// <returns>Activity log type item</returns>
        public static ActivityLogType UpdateActivityType(int ActivityLogTypeID, string SystemKeyword, string Name, bool Enabled)
        {
            var dbItem = DBProviderManager<DBCustomerActivityProvider>.Provider.UpdateActivityType(ActivityLogTypeID, SystemKeyword, Name, Enabled);
            var activityType = DBMapping(dbItem);

            if (NopCache.IsEnabled)
                NopCache.RemoveByPattern(ACTIVITYTYPE_PATTERN_KEY);
            
            return activityType;
        }

        /// <summary>
        /// Updates an activity log type item
        /// </summary>
        /// <param name="ActivityLogTypeID">Activity log type identifier</param>
        /// <param name="Enabled">Value indicating whether the activity log type is enabled</param>
        /// <returns>Activity log type item</returns>
        public static ActivityLogType UpdateActivityType(int ActivityLogTypeID, bool Enabled)
        {
            var activityType = GetActivityTypeByID(ActivityLogTypeID);
            if (activityType == null || activityType.Enabled == Enabled)
                return activityType;

            var dbItem = DBProviderManager<DBCustomerActivityProvider>.Provider.UpdateActivityType(
                activityType.ActivityLogTypeID, activityType.SystemKeyword,
                activityType.Name, Enabled);
            activityType = DBMapping(dbItem);

            if (NopCache.IsEnabled)
                NopCache.RemoveByPattern(ACTIVITYTYPE_PATTERN_KEY);

            return activityType;
        }
        
        /// <summary>
        /// Deletes an activity log type item
        /// </summary>
        /// <param name="ActivityLogTypeID">Activity log type identifier</param>
        public static void DeleteActivityType(int ActivityLogTypeID)
        {
            if (NopCache.IsEnabled)
                NopCache.RemoveByPattern(ACTIVITYTYPE_PATTERN_KEY);
            
            DBProviderManager<DBCustomerActivityProvider>.Provider.DeleteActivityType(ActivityLogTypeID);
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
        /// <param name="ActivityLogTypeID">Activity log type identifier</param>
        /// <returns>Activity log type item</returns>
        public static ActivityLogType GetActivityTypeByID(int ActivityLogTypeID)
        {
            if (ActivityLogTypeID == 0)
                return null;

            string key = string.Format(ACTIVITYTYPE_BY_ID_KEY, ActivityLogTypeID);
            if (NopCache.IsEnabled)
            {
                object cache = NopCache.Get(key);
                if (cache != null)
                    return (ActivityLogType)cache;
            }

            var dbItem = DBProviderManager<DBCustomerActivityProvider>.Provider.GetActivityTypeByID(ActivityLogTypeID);
            var activityLogType = DBMapping(dbItem);

            if (NopCache.IsEnabled)
                NopCache.Max(key, activityLogType);
            
            return activityLogType;
        }

        /// <summary>
        /// Inserts an activity log item
        /// </summary>
        /// <param name="SystemKeyword">The system keyword</param>
        /// <param name="Comment">The activity comment</param>
        /// <returns>Activity log item</returns>
        public static ActivityLog InsertActivity(string SystemKeyword, string Comment)
        {
            return InsertActivity(SystemKeyword, Comment, new object[0]);
        }

        /// <summary>
        /// Inserts an activity log item
        /// </summary>
        /// <param name="SystemKeyword">The system keyword</param>
        /// <param name="Comment">The activity comment</param>
        /// <param name="CommentParams">The activity comment parameters for string.Format() function.</param>
        /// <returns>Activity log item</returns>
        public static ActivityLog InsertActivity(string SystemKeyword, string Comment, params object[] CommentParams)
        {
            if (NopContext.Current == null || NopContext.Current.User == null || NopContext.Current.User.IsGuest)
                return null;

            var activityTypes = GetAllActivityTypes();
            var activityType = activityTypes.FindBySystemKeyword(SystemKeyword);
            if (activityType == null || !activityType.Enabled)
                return null;

            int CustomerID = NopContext.Current.User.CustomerID;
            DateTime CreatedOn = DateTimeHelper.ConvertToUtcTime(DateTime.Now);
            Comment = string.Format(Comment, CommentParams);

            var dbItem = DBProviderManager<DBCustomerActivityProvider>.Provider.InsertActivity(activityType.ActivityLogTypeID, CustomerID, Comment, CreatedOn);
            var activity = DBMapping(dbItem);
            return activity;
        }
        
        /// <summary>
        /// Updates an activity log 
        /// </summary>
        /// <param name="ActivityLogID">Activity log identifier</param>
        /// <param name="ActivityLogTypeID">Activity log type identifier</param>
        /// <param name="CustomerID">The customer identifier</param>
        /// <param name="Comment">The activity comment</param>
        /// <param name="CreatedOn">The date and time of instance creation</param>
        /// <returns>Activity log item</returns>
        public static ActivityLog UpdateActivity(int ActivityLogID, int ActivityLogTypeID, int CustomerID, string Comment, DateTime CreatedOn)
        {
            var dbItem = DBProviderManager<DBCustomerActivityProvider>.Provider.UpdateActivity(ActivityLogID, ActivityLogTypeID, CustomerID, Comment, CreatedOn);
            var activity = DBMapping(dbItem);
            return activity;
        }
        
        /// <summary>
        /// Deletes an activity log item
        /// </summary>
        /// <param name="ActivityLogID">Activity log type identifier</param>
        public static void DeleteActivity(int ActivityLogID)
        {
            DBProviderManager<DBCustomerActivityProvider>.Provider.DeleteActivity(ActivityLogID);
        }
        
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
        public static ActivityLogCollection GetAllActivities(DateTime? CreatedOnFrom, DateTime? CreatedOnTo,
            string Email, string Username, int ActivityLogTypeID,
            int PageSize, int PageIndex, out int TotalRecords)
        {
            if (PageSize <= 0)
                PageSize = 10;
            if (PageSize == int.MaxValue)
                PageSize = int.MaxValue - 1;

            if (PageIndex < 0)
                PageIndex = 0;
            if (PageIndex == int.MaxValue)
                PageIndex = int.MaxValue - 1;

            var dbCollection = DBProviderManager<DBCustomerActivityProvider>.Provider.GetAllActivities(
                CreatedOnFrom, CreatedOnTo, Email, Username, ActivityLogTypeID, PageSize, PageIndex, out TotalRecords);
            var collection = DBMapping(dbCollection);
            return collection;
        }
        
        /// <summary>
        /// Gets an activity log item
        /// </summary>
        /// <param name="ActivityLogID">Activity log identifier</param>
        /// <returns>Activity log item</returns>
        public static ActivityLog GetActivityByID(int ActivityLogID)
        {
            if (ActivityLogID == 0)
                return null;

            var dbItem = DBProviderManager<DBCustomerActivityProvider>.Provider.GetActivityByID(ActivityLogID);
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
