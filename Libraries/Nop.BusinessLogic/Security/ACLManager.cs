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
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Security;
using NopSolutions.NopCommerce.BusinessLogic.Caching;
using NopSolutions.NopCommerce.BusinessLogic.Configuration.Settings;
using NopSolutions.NopCommerce.BusinessLogic.Utils;
using NopSolutions.NopCommerce.Common;
using NopSolutions.NopCommerce.Common.Utils;
using NopSolutions.NopCommerce.DataAccess;
using NopSolutions.NopCommerce.DataAccess.Security;


namespace NopSolutions.NopCommerce.BusinessLogic.Security
{
    /// <summary>
    /// ACL manager
    /// </summary>
    public partial class ACLManager
    {
        #region Constants
        private const string CUSTOMERACTIONS_ALL_KEY = "Nop.customeraction.all";
        private const string CUSTOMERACTIONS_BY_ID_KEY = "Nop.customeraction.id-{0}";
        private const string CUSTOMERACTIONS_PATTERN_KEY = "Nop.customeraction.";
        #endregion

        #region Utilities

        private static ACLCollection DBMapping(DBACLCollection dbCollection)
        {
            if (dbCollection == null)
                return null;

            var collection = new ACLCollection();
            foreach (var dbItem in dbCollection)
            {
                var item = DBMapping(dbItem);
                collection.Add(item);
            }

            return collection;
        }

        private static ACL DBMapping(DBACL dbItem)
        {
            if (dbItem == null)
                return null;

            var item = new ACL();
            item.ACLID = dbItem.ACLID;
            item.CustomerActionID = dbItem.CustomerActionID;
            item.CustomerRoleID = dbItem.CustomerRoleID;
            item.Allow = dbItem.Allow;

            return item;
        }

        private static CustomerActionCollection DBMapping(DBCustomerActionCollection dbCollection)
        {
            if (dbCollection == null)
                return null;

            var collection = new CustomerActionCollection();
            foreach (var dbItem in dbCollection)
            {
                var item = DBMapping(dbItem);
                collection.Add(item);
            }

            return collection;
        }

        private static CustomerAction DBMapping(DBCustomerAction dbItem)
        {
            if (dbItem == null)
                return null;

            var item = new CustomerAction();
            item.CustomerActionID = dbItem.CustomerActionID;
            item.Name = dbItem.Name;
            item.SystemKeyword = dbItem.SystemKeyword;
            item.Comment = dbItem.Comment;
            item.DisplayOrder = dbItem.DisplayOrder;

            return item;
        }
        
        #endregion

        #region Methods

        #region Repository methods
        /// <summary>
        /// Deletes a customer action
        /// </summary>
        /// <param name="CustomerActionID">Customer action identifier</param>
        public static void DeleteCustomerAction(int CustomerActionID)
        {
            DBProviderManager<DBACLProvider>.Provider.DeleteCustomerAction(CustomerActionID);
            if (ACLManager.CacheEnabled)
            {
                NopCache.RemoveByPattern(CUSTOMERACTIONS_PATTERN_KEY);
            }
        }

        /// <summary>
        /// Gets a customer action by identifier
        /// </summary>
        /// <param name="CustomerActionID">Customer action identifier</param>
        /// <returns>Customer action</returns>
        public static CustomerAction GetCustomerActionByID(int CustomerActionID)
        {
            if (CustomerActionID == 0)
                return null;

            string key = string.Format(CUSTOMERACTIONS_BY_ID_KEY, CustomerActionID);
            object obj2 = NopCache.Get(key);
            if (ACLManager.CacheEnabled && (obj2 != null))
            {
                return (CustomerAction)obj2;
            }

            var dbItem = DBProviderManager<DBACLProvider>.Provider.GetCustomerActionByID(CustomerActionID);
            var customerAction = DBMapping(dbItem);

            if (ACLManager.CacheEnabled)
            {
                NopCache.Max(key, customerAction);
            }
            return customerAction;
        }

        /// <summary>
        /// Gets all customer actions
        /// </summary>
        /// <returns>Customer action collection</returns>
        public static CustomerActionCollection GetAllCustomerActions()
        {
            string key = string.Format(CUSTOMERACTIONS_ALL_KEY);
            object obj2 = NopCache.Get(key);
            if (ACLManager.CacheEnabled && (obj2 != null))
            {
                return (CustomerActionCollection)obj2;
            }

            var dbCollection = DBProviderManager<DBACLProvider>.Provider.GetAllCustomerActions();
            var customerActions = DBMapping(dbCollection);
            if (ACLManager.CacheEnabled)
            {
                NopCache.Max(key, customerActions);
            }
            return customerActions;
        }

        /// <summary>
        /// Inserts a customer action
        /// </summary>
        /// <param name="Name">The name</param>
        /// <param name="SystemKeyword">The system keyword</param>
        /// <param name="Comment">The comment</param>
        /// <param name="DisplayOrder">The display order</param>
        /// <returns>A customer action</returns>
        public static CustomerAction InsertCustomerAction(string Name,
            string SystemKeyword, string Comment, string DisplayOrder)
        {
            var dbItem = DBProviderManager<DBACLProvider>.Provider.InsertCustomerAction(Name,
                SystemKeyword, Comment, DisplayOrder);
            var customerAction = DBMapping(dbItem);

            if (ACLManager.CacheEnabled)
            {
                NopCache.RemoveByPattern(CUSTOMERACTIONS_PATTERN_KEY);
            }

            return customerAction;
        }

        /// <summary>
        /// Updates the customer action
        /// </summary>
        /// <param name="CustomerActionID">The customer action identifier</param>
        /// <param name="Name">The name</param>
        /// <param name="SystemKeyword">The system keyword</param>
        /// <param name="Comment">The comment</param>
        /// <param name="DisplayOrder">The display order</param>
        /// <returns>A customer action</returns>
        public static CustomerAction UpdateCustomerAction(int CustomerActionID,
            string Name, string SystemKeyword, string Comment, string DisplayOrder)
        {
            var dbItem = DBProviderManager<DBACLProvider>.Provider.UpdateCustomerAction(CustomerActionID,
                Name, SystemKeyword, Comment, DisplayOrder);
            var customerAction = DBMapping(dbItem);

            if (ACLManager.CacheEnabled)
            {
                NopCache.RemoveByPattern(CUSTOMERACTIONS_PATTERN_KEY);
            }

            return customerAction;
        }



        /// <summary>
        /// Deletes an ACL
        /// </summary>
        /// <param name="ACLID">ACL identifier</param>
        public static void DeleteACL(int ACLID)
        {
            DBProviderManager<DBACLProvider>.Provider.DeleteACL(ACLID);
        }

        /// <summary>
        /// Gets an ACL by identifier
        /// </summary>
        /// <param name="ACLID">ACL identifier</param>
        /// <returns>ACL</returns>
        public static ACL GetACLByID(int ACLID)
        {
            if (ACLID == 0)
                return null;

            var dbItem = DBProviderManager<DBACLProvider>.Provider.GetACLByID(ACLID);
            var acl = DBMapping(dbItem);
            return acl;
        }

        /// <summary>
        /// Gets all ACL
        /// </summary>
        /// <param name="CustomerActionID">Customer action identifier; 0 to load all records</param>
        /// <param name="CustomerRoleID">Customer role identifier; 0 to load all records</param>
        /// <param name="Allow">Value indicating whether action is allowed; null to load all records</param>
        /// <returns>ACL collection</returns>
        public static ACLCollection GetAllACL(int CustomerActionID,
            int CustomerRoleID, bool? Allow)
        {
            var dbCollection = DBProviderManager<DBACLProvider>.Provider.GetAllACL(CustomerActionID,
                CustomerRoleID, Allow);
            var aclCollection = DBMapping(dbCollection);
            return aclCollection;
        }

        /// <summary>
        /// Inserts an ACL
        /// </summary>
        /// <param name="CustomerActionID">The customer action identifier</param>
        /// <param name="CustomerRoleID">The customer role identifier</param>
        /// <param name="Allow">The value indicating whether action is allowed</param>
        /// <returns>An ACL</returns>
        public static ACL InsertACL(int CustomerActionID,
            int CustomerRoleID, bool Allow)
        {
            var dbItem = DBProviderManager<DBACLProvider>.Provider.InsertACL(CustomerActionID,
                CustomerRoleID, Allow);
            var acl = DBMapping(dbItem);
            return acl;
        }

        /// <summary>
        /// Updates the ACL
        /// </summary>
        /// <param name="ACLID">The ACL identifier</param>
        /// <param name="CustomerActionID">The customer action identifier</param>
        /// <param name="CustomerRoleID">The customer role identifier</param>
        /// <param name="Allow">The value indicating whether action is allowed</param>
        /// <returns>An ACL</returns>
        public static ACL UpdateACL(int ACLID, int CustomerActionID,
            int CustomerRoleID, bool Allow)
        {
            var dbItem = DBProviderManager<DBACLProvider>.Provider.UpdateACL(ACLID,
                CustomerActionID, CustomerRoleID, Allow);
            var acl = DBMapping(dbItem);
            return acl;
        }
        #endregion

        #region Helper methods
        /// <summary>
        /// Indicates whether action is allowed
        /// </summary>
        /// <param name="ActionSystemKeyword">Action system keyword</param>
        /// <returns>Result</returns>
        public static bool IsActionAllowed(string ActionSystemKeyword)
        {
            int userID = 0;
            if (NopContext.Current != null &&
                NopContext.Current.User != null)
                userID = NopContext.Current.User.CustomerID;
            return IsActionAllowed(userID, ActionSystemKeyword);
        }

        /// <summary>
        /// Indicates whether action is allowed
        /// </summary>
        /// <param name="CustomerID">Customer identifer</param>
        /// <param name="ActionSystemKeyword">Action system keyword</param>
        /// <returns>Result</returns>
        public static bool IsActionAllowed(int CustomerID, string ActionSystemKeyword)
        {
            if (!ACLManager.Enabled)
                return true;

            bool result = DBProviderManager<DBACLProvider>.Provider.IsActionAllowed(CustomerID,
                ActionSystemKeyword);

            return result;
        }
        #endregion

        #endregion

        #region Property
        /// <summary>
        /// Gets a value indicating ACL feature is enabled
        /// </summary>
        public static bool Enabled
        {
            get
            {
                return SettingManager.GetSettingValueBoolean("ACL.Enabled");
            }
        }

        /// <summary>
        /// Gets a value indicating whether cache is enabled
        /// </summary>
        public static bool CacheEnabled
        {
            get
            {
                return SettingManager.GetSettingValueBoolean("Cache.ACLManager.CacheEnabled");
            }
        }

        #endregion
    }
}