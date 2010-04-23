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
using System.Configuration.Provider;
using System.Data;
using System.Data.Common;
using System.Globalization;
using System.Text;
using Microsoft.Practices.EnterpriseLibrary.Data;

namespace NopSolutions.NopCommerce.DataAccess.Security
{
    /// <summary>
    /// ACL provider for SQL Server
    /// </summary>
    public partial class SQLACLProvider : DBACLProvider
    {
        #region Fields
        private string _sqlConnectionString;
        #endregion

        #region Utilities
        private DBCustomerAction GetCustomerActionFromReader(IDataReader dataReader)
        {
            DBCustomerAction customerAction = new DBCustomerAction();
            customerAction.CustomerActionID = NopSqlDataHelper.GetInt(dataReader, "CustomerActionID");
            customerAction.Name = NopSqlDataHelper.GetString(dataReader, "Name");
            customerAction.SystemKeyword = NopSqlDataHelper.GetString(dataReader, "SystemKeyword");
            customerAction.Comment = NopSqlDataHelper.GetString(dataReader, "Comment");
            customerAction.DisplayOrder = NopSqlDataHelper.GetInt(dataReader, "DisplayOrder");
            return customerAction;
        }

        private DBACL GetACLFromReader(IDataReader dataReader)
        {
            DBACL acl = new DBACL();
            acl.ACLID = NopSqlDataHelper.GetInt(dataReader, "ACLID");
            acl.CustomerActionID = NopSqlDataHelper.GetInt(dataReader, "CustomerActionID");
            acl.CustomerRoleID = NopSqlDataHelper.GetInt(dataReader, "CustomerRoleID");
            acl.Allow = NopSqlDataHelper.GetBoolean(dataReader, "Allow");
            return acl;
        }
        #endregion

        #region Methods

        /// <summary>
        /// Initializes the provider with the property values specified in the application's configuration file. This method is not intended to be used directly from your code
        /// </summary>
        /// <param name="name">The name of the provider instance to initialize</param>
        /// <param name="config">A NameValueCollection that contains the names and values of configuration options for the provider.</param>
        public override void Initialize(string name, NameValueCollection config)
        {
            if (config == null)
            {
                throw new ArgumentNullException("config");
            }

            base.Initialize(name, config);

            string connectionStringName = config["connectionStringName"];
            if (String.IsNullOrEmpty(connectionStringName))
                throw new ProviderException("Connection name not specified");
            this._sqlConnectionString = NopSqlDataHelper.GetConnectionString(connectionStringName);
            if ((this._sqlConnectionString == null) || (this._sqlConnectionString.Length < 1))
            {
                throw new ProviderException(string.Format("Connection string not found. {0}", connectionStringName));
            }
            config.Remove("connectionStringName");

            if (config.Count > 0)
            {
                string key = config.GetKey(0);
                if (!string.IsNullOrEmpty(key))
                {
                    throw new ProviderException(string.Format("Provider unrecognized attribute. {0}", new object[] { key }));
                }
            }
        }

        /// <summary>
        /// Deletes a customer action
        /// </summary>
        /// <param name="CustomerActionID">Customer action identifier</param>
        public override void DeleteCustomerAction(int CustomerActionID)
        {
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_CustomerActionDelete");
            db.AddInParameter(dbCommand, "CustomerActionID", DbType.Int32, CustomerActionID);
            int retValue = db.ExecuteNonQuery(dbCommand);
        }

        /// <summary>
        /// Gets a customer action by identifier
        /// </summary>
        /// <param name="CustomerActionID">Customer action identifier</param>
        /// <returns>Customer action</returns>
        public override DBCustomerAction GetCustomerActionByID(int CustomerActionID)
        {
            DBCustomerAction customerAction = null;
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_CustomerActionLoadByPrimaryKey");
            db.AddInParameter(dbCommand, "CustomerActionID", DbType.Int32, CustomerActionID);
            using (IDataReader dataReader = db.ExecuteReader(dbCommand))
            {
                if (dataReader.Read())
                {
                    customerAction = GetCustomerActionFromReader(dataReader);
                }
            }
            return customerAction;
        }
        
        /// <summary>
        /// Gets all customer actions
        /// </summary>
        /// <returns>Customer action collection</returns>
        public override DBCustomerActionCollection GetAllCustomerActions()
        {
            var result = new DBCustomerActionCollection();
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_CustomerActionLoadAll");
            
            using (IDataReader dataReader = db.ExecuteReader(dbCommand))
            {
                while (dataReader.Read())
                {
                    var item = GetCustomerActionFromReader(dataReader);
                    result.Add(item);
                }
            }

            return result;
        }

        /// <summary>
        /// Inserts a customer action
        /// </summary>
        /// <param name="Name">The name</param>
        /// <param name="SystemKeyword">The system keyword</param>
        /// <param name="Comment">The comment</param>
        /// <param name="DisplayOrder">The display order</param>
        /// <returns>A customer action</returns>
        public override DBCustomerAction InsertCustomerAction(string Name,
            string SystemKeyword, string Comment, string DisplayOrder)
        {
            DBCustomerAction customerAction = null;
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_CustomerActionInsert");
            db.AddOutParameter(dbCommand, "CustomerActionID", DbType.Int32, 0);
            db.AddInParameter(dbCommand, "Name", DbType.String, Name);
            db.AddInParameter(dbCommand, "SystemKeyword", DbType.String, SystemKeyword);
            db.AddInParameter(dbCommand, "Comment", DbType.String, Comment);
            db.AddInParameter(dbCommand, "DisplayOrder", DbType.Int32, DisplayOrder);
            if (db.ExecuteNonQuery(dbCommand) > 0)
            {
                int CustomerActionID = Convert.ToInt32(db.GetParameterValue(dbCommand, "@CustomerActionID"));
                customerAction = GetCustomerActionByID(CustomerActionID);
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
        public override DBCustomerAction UpdateCustomerAction(int CustomerActionID,
            string Name, string SystemKeyword, string Comment, string DisplayOrder)
        {
            DBCustomerAction customerAction = null;
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_CustomerActionUpdate");
            db.AddInParameter(dbCommand, "CustomerActionID", DbType.Int32, CustomerActionID);
            db.AddInParameter(dbCommand, "Name", DbType.String, Name);
            db.AddInParameter(dbCommand, "SystemKeyword", DbType.String, SystemKeyword);
            db.AddInParameter(dbCommand, "Comment", DbType.String, Comment);
            db.AddInParameter(dbCommand, "DisplayOrder", DbType.Int32, DisplayOrder);
            if (db.ExecuteNonQuery(dbCommand) > 0)
                customerAction = GetCustomerActionByID(CustomerActionID);

            return customerAction;
        }



        /// <summary>
        /// Deletes an ACL
        /// </summary>
        /// <param name="ACLID">ACL identifier</param>
        public override void DeleteACL(int ACLID)
        {
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_ACLDelete");
            db.AddInParameter(dbCommand, "ACLID", DbType.Int32, ACLID);
            int retValue = db.ExecuteNonQuery(dbCommand);
        }

        /// <summary>
        /// Gets an ACL by identifier
        /// </summary>
        /// <param name="ACLID">ACL identifier</param>
        /// <returns>ACL</returns>
        public override DBACL GetACLByID(int ACLID)
        {
            DBACL acl = null;
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_ACLLoadByPrimaryKey");
            db.AddInParameter(dbCommand, "ACLID", DbType.Int32, ACLID);
            using (IDataReader dataReader = db.ExecuteReader(dbCommand))
            {
                if (dataReader.Read())
                {
                    acl = GetACLFromReader(dataReader);
                }
            }
            return acl;
        }


        /// <summary>
        /// Gets all ACL
        /// </summary>
        /// <param name="CustomerActionID">Customer action identifier; 0 to load all records</param>
        /// <param name="CustomerRoleID">Customer role identifier; 0 to load all records</param>
        /// <param name="Allow">Value indicating whether action is allowed; null to load all records</param>
        /// <returns>ACL collection</returns>
        public override DBACLCollection GetAllACL(int CustomerActionID,
            int CustomerRoleID, bool? Allow)
        {
            var result = new DBACLCollection();
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_ACLLoadAll");
            db.AddInParameter(dbCommand, "CustomerActionID", DbType.Int32, CustomerActionID);
            db.AddInParameter(dbCommand, "CustomerRoleID", DbType.Int32, CustomerRoleID);
            if (Allow.HasValue)
                db.AddInParameter(dbCommand, "Allow", DbType.Boolean, Allow.Value);
            else
                db.AddInParameter(dbCommand, "Allow", DbType.Boolean, null);
            
            using (IDataReader dataReader = db.ExecuteReader(dbCommand))
            {
                while (dataReader.Read())
                {
                    var item = GetACLFromReader(dataReader);
                    result.Add(item);
                }
            }

            return result;
        }

        /// <summary>
        /// Inserts an ACL
        /// </summary>
        /// <param name="CustomerActionID">The customer action identifier</param>
        /// <param name="CustomerRoleID">The customer role identifier</param>
        /// <param name="Allow">The value indicating whether action is allowed</param>
        /// <returns>An ACL</returns>
        public override DBACL InsertACL(int CustomerActionID,
            int CustomerRoleID, bool Allow)
        {
            DBACL acl = null;
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_ACLInsert");
            db.AddOutParameter(dbCommand, "ACLID", DbType.Int32, 0);
            db.AddInParameter(dbCommand, "CustomerActionID", DbType.Int32, CustomerActionID);
            db.AddInParameter(dbCommand, "CustomerRoleID", DbType.Int32, CustomerRoleID);
            db.AddInParameter(dbCommand, "Allow", DbType.Boolean, Allow);
            if (db.ExecuteNonQuery(dbCommand) > 0)
            {
                int ACLID = Convert.ToInt32(db.GetParameterValue(dbCommand, "@ACLID"));
                acl = GetACLByID(ACLID);
            }
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
        public override DBACL UpdateACL(int ACLID, int CustomerActionID,
            int CustomerRoleID, bool Allow)
        {
            DBACL acl = null;
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_ACLUpdate");
            db.AddInParameter(dbCommand, "ACLID", DbType.Int32, ACLID);
            db.AddInParameter(dbCommand, "CustomerActionID", DbType.Int32, CustomerActionID);
            db.AddInParameter(dbCommand, "CustomerRoleID", DbType.Int32, CustomerRoleID);
            db.AddInParameter(dbCommand, "Allow", DbType.Boolean, Allow);
            if (db.ExecuteNonQuery(dbCommand) > 0)
                acl = GetACLByID(ACLID);

            return acl;
        }

        /// <summary>
        /// Indicates whether action is allowed
        /// </summary>
        /// <param name="CustomerID">Customer identifer</param>
        /// <param name="ActionSystemKeyword">Action system keyword</param>
        /// <returns>Result</returns>
        public override bool IsActionAllowed(int CustomerID, string ActionSystemKeyword)
        {
            bool result = false;
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_ACLIsAllowed");
            db.AddInParameter(dbCommand, "CustomerID", DbType.Int32, CustomerID);
            db.AddInParameter(dbCommand, "ActionSystemKeyword", DbType.String, ActionSystemKeyword);

            using (IDataReader dataReader = db.ExecuteReader(dbCommand))
            {
                if (dataReader.Read())
                {
                    result = NopSqlDataHelper.GetBoolean(dataReader, "result");
                }
            }

            return result;
        }

        #endregion
    }
}
