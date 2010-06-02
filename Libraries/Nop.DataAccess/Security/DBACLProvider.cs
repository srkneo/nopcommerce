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

namespace NopSolutions.NopCommerce.DataAccess.Security
{
    /// <summary>
    /// Acts as a base class for deriving custom ACL provider
    /// </summary>
    [DBProviderSectionName("nopDataProviders/ACLProvider")]
    public abstract partial class DBACLProvider : BaseDBProvider
    {
        #region Methods

        /// <summary>
        /// Deletes a customer action
        /// </summary>
        /// <param name="customerActionId">Customer action identifier</param>
        public abstract void DeleteCustomerAction(int customerActionId);

        /// <summary>
        /// Gets a customer action by identifier
        /// </summary>
        /// <param name="customerActionId">Customer action identifier</param>
        /// <returns>Customer action</returns>
        public abstract DBCustomerAction GetCustomerActionById(int customerActionId);

        /// <summary>
        /// Gets all customer actions
        /// </summary>
        /// <returns>Customer action collection</returns>
        public abstract DBCustomerActionCollection GetAllCustomerActions();

        /// <summary>
        /// Inserts a customer action
        /// </summary>
        /// <param name="name">The name</param>
        /// <param name="systemKeyword">The system keyword</param>
        /// <param name="comment">The comment</param>
        /// <param name="displayOrder">The display order</param>
        /// <returns>A customer action</returns>
        public abstract DBCustomerAction InsertCustomerAction(string name,
            string systemKeyword, string comment, string displayOrder);

        /// <summary>
        /// Updates the customer action
        /// </summary>
        /// <param name="customerActionId">The customer action identifier</param>
        /// <param name="name">The name</param>
        /// <param name="systemKeyword">The system keyword</param>
        /// <param name="comment">The comment</param>
        /// <param name="displayOrder">The display order</param>
        /// <returns>A customer action</returns>
        public abstract DBCustomerAction UpdateCustomerAction(int customerActionId, 
            string name, string systemKeyword, string comment, string displayOrder);
        


        /// <summary>
        /// Deletes an ACL
        /// </summary>
        /// <param name="aclId">ACL identifier</param>
        public abstract void DeleteAcl(int aclId);

        /// <summary>
        /// Gets an ACL by identifier
        /// </summary>
        /// <param name="aclId">ACL identifier</param>
        /// <returns>ACL</returns>
        public abstract DBACL GetAclById(int aclId);

        /// <summary>
        /// Gets all ACL
        /// </summary>
        /// <param name="customerActionId">Customer action identifier; 0 to load all records</param>
        /// <param name="customerRoleId">Customer role identifier; 0 to load all records</param>
        /// <param name="allow">Value indicating whether action is allowed; null to load all records</param>
        /// <returns>ACL collection</returns>
        public abstract DBACLCollection GetAllAcl(int customerActionId,
            int customerRoleId, bool? allow);

        /// <summary>
        /// Inserts an ACL
        /// </summary>
        /// <param name="customerActionId">The customer action identifier</param>
        /// <param name="customerRoleId">The customer role identifier</param>
        /// <param name="allow">The value indicating whether action is allowed</param>
        /// <returns>An ACL</returns>
        public abstract DBACL InsertAcl(int customerActionId,
            int customerRoleId, bool allow);

        /// <summary>
        /// Updates the ACL
        /// </summary>
        /// <param name="aclId">The ACL identifier</param>
        /// <param name="customerActionId">The customer action identifier</param>
        /// <param name="customerRoleId">The customer role identifier</param>
        /// <param name="allow">The value indicating whether action is allowed</param>
        /// <returns>An ACL</returns>
        public abstract DBACL UpdateAcl(int aclId, int customerActionId,
            int customerRoleId, bool allow);
        
        /// <summary>
        /// Indicates whether action is allowed
        /// </summary>
        /// <param name="customerId">Customer identifer</param>
        /// <param name="actionSystemKeyword">Action system keyword</param>
        /// <returns>Result</returns>
        public abstract bool IsActionAllowed(int customerId, string actionSystemKeyword);
        #endregion
    }
}
