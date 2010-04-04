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
        /// <param name="CustomerActionID">Customer action identifier</param>
        public abstract void DeleteCustomerAction(int CustomerActionID);

        /// <summary>
        /// Gets a customer action by identifier
        /// </summary>
        /// <param name="CustomerActionID">Customer action identifier</param>
        /// <returns>Customer action</returns>
        public abstract DBCustomerAction GetCustomerActionByID(int CustomerActionID);

        /// <summary>
        /// Gets all customer actions
        /// </summary>
        /// <returns>Customer action collection</returns>
        public abstract DBCustomerActionCollection GetAllCustomerActions();

        /// <summary>
        /// Inserts a customer action
        /// </summary>
        /// <param name="Name">The name</param>
        /// <param name="SystemKeyword">The system keyword</param>
        /// <param name="Comment">The comment</param>
        /// <param name="DisplayOrder">The display order</param>
        /// <returns>A customer action</returns>
        public abstract DBCustomerAction InsertCustomerAction(string Name,
            string SystemKeyword, string Comment, string DisplayOrder);

        /// <summary>
        /// Updates the customer action
        /// </summary>
        /// <param name="CustomerActionID">The customer action identifier</param>
        /// <param name="Name">The name</param>
        /// <param name="SystemKeyword">The system keyword</param>
        /// <param name="Comment">The comment</param>
        /// <param name="DisplayOrder">The display order</param>
        /// <returns>A customer action</returns>
        public abstract DBCustomerAction UpdateCustomerAction(int CustomerActionID,
            string Name, string SystemKeyword, string Comment, string DisplayOrder);
        


        /// <summary>
        /// Deletes an ACL
        /// </summary>
        /// <param name="ACLID">ACL identifier</param>
        public abstract void DeleteACL(int ACLID);

        /// <summary>
        /// Gets an ACL by identifier
        /// </summary>
        /// <param name="ACLID">ACL identifier</param>
        /// <returns>ACL</returns>
        public abstract DBACL GetACLByID(int ACLID);

        /// <summary>
        /// Gets all ACL
        /// </summary>
        /// <param name="CustomerActionID">Customer action identifier; 0 to load all records</param>
        /// <param name="CustomerRoleID">Customer role identifier; 0 to load all records</param>
        /// <param name="Allow">Value indicating whether action is allowed; null to load all records</param>
        /// <returns>ACL collection</returns>
        public abstract DBACLCollection GetAllACL(int CustomerActionID,
            int CustomerRoleID, bool? Allow);

        /// <summary>
        /// Inserts an ACL
        /// </summary>
        /// <param name="CustomerActionID">The customer action identifier</param>
        /// <param name="CustomerRoleID">The customer role identifier</param>
        /// <param name="Allow">The value indicating whether action is allowed</param>
        /// <returns>An ACL</returns>
        public abstract DBACL InsertACL(int CustomerActionID,
            int CustomerRoleID, bool Allow);

        /// <summary>
        /// Updates the ACL
        /// </summary>
        /// <param name="ACLID">The ACL identifier</param>
        /// <param name="CustomerActionID">The customer action identifier</param>
        /// <param name="CustomerRoleID">The customer role identifier</param>
        /// <param name="Allow">The value indicating whether action is allowed</param>
        /// <returns>An ACL</returns>
        public abstract DBACL UpdateACL(int ACLID, int CustomerActionID,
            int CustomerRoleID, bool Allow);
        
        /// <summary>
        /// Indicates whether action is allowed
        /// </summary>
        /// <param name="CustomerID">Customer identifer</param>
        /// <param name="ActionSystemKeyword">Action system keyword</param>
        /// <returns>Result</returns>
        public abstract bool IsActionAllowed(int CustomerID, string ActionSystemKeyword);
        #endregion
    }
}
