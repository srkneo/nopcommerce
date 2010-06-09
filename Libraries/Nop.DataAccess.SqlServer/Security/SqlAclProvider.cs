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
    public partial class SqlAclProvider : DBACLProvider
    {
        #region Fields
        private string _sqlConnectionString;
        #endregion

        #region Utilities

        private DBACL GetAclFromReader(IDataReader dataReader)
        {
            var item = new DBACL();
            item.AclId = NopSqlDataHelper.GetInt(dataReader, "ACLID");
            item.CustomerActionId = NopSqlDataHelper.GetInt(dataReader, "CustomerActionID");
            item.CustomerRoleId = NopSqlDataHelper.GetInt(dataReader, "CustomerRoleID");
            item.Allow = NopSqlDataHelper.GetBoolean(dataReader, "Allow");
            return item;
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
        /// Deletes an ACL
        /// </summary>
        /// <param name="aclId">ACL identifier</param>
        public override void DeleteAcl(int aclId)
        {
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_ACLDelete");
            db.AddInParameter(dbCommand, "ACLID", DbType.Int32, aclId);
            db.ExecuteNonQuery(dbCommand);
        }

        /// <summary>
        /// Gets an ACL by identifier
        /// </summary>
        /// <param name="aclId">ACL identifier</param>
        /// <returns>ACL</returns>
        public override DBACL GetAclById(int aclId)
        {
            DBACL item = null;
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_ACLLoadByPrimaryKey");
            db.AddInParameter(dbCommand, "ACLID", DbType.Int32, aclId);
            using (IDataReader dataReader = db.ExecuteReader(dbCommand))
            {
                if (dataReader.Read())
                {
                    item = GetAclFromReader(dataReader);
                }
            }
            return item;
        }


        /// <summary>
        /// Gets all ACL
        /// </summary>
        /// <param name="customerActionId">Customer action identifier; 0 to load all records</param>
        /// <param name="customerRoleId">Customer role identifier; 0 to load all records</param>
        /// <param name="allow">Value indicating whether action is allowed; null to load all records</param>
        /// <returns>ACL collection</returns>
        public override DBACLCollection GetAllAcl(int customerActionId,
            int customerRoleId, bool? allow)
        {
            var result = new DBACLCollection();
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_ACLLoadAll");
            db.AddInParameter(dbCommand, "CustomerActionID", DbType.Int32, customerActionId);
            db.AddInParameter(dbCommand, "CustomerRoleID", DbType.Int32, customerRoleId);
            if (allow.HasValue)
                db.AddInParameter(dbCommand, "Allow", DbType.Boolean, allow.Value);
            else
                db.AddInParameter(dbCommand, "Allow", DbType.Boolean, null);
            
            using (IDataReader dataReader = db.ExecuteReader(dbCommand))
            {
                while (dataReader.Read())
                {
                    var item = GetAclFromReader(dataReader);
                    result.Add(item);
                }
            }

            return result;
        }

        /// <summary>
        /// Inserts an ACL
        /// </summary>
        /// <param name="customerActionId">The customer action identifier</param>
        /// <param name="customerRoleId">The customer role identifier</param>
        /// <param name="allow">The value indicating whether action is allowed</param>
        /// <returns>An ACL</returns>
        public override DBACL InsertAcl(int customerActionId,
            int customerRoleId, bool allow)
        {
            DBACL item = null;
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_ACLInsert");
            db.AddOutParameter(dbCommand, "ACLID", DbType.Int32, 0);
            db.AddInParameter(dbCommand, "CustomerActionID", DbType.Int32, customerActionId);
            db.AddInParameter(dbCommand, "CustomerRoleID", DbType.Int32, customerRoleId);
            db.AddInParameter(dbCommand, "Allow", DbType.Boolean, allow);
            if (db.ExecuteNonQuery(dbCommand) > 0)
            {
                int aclId = Convert.ToInt32(db.GetParameterValue(dbCommand, "@ACLID"));
                item = GetAclById(aclId);
            }
            return item;
        }

        /// <summary>
        /// Updates the ACL
        /// </summary>
        /// <param name="aclId">The ACL identifier</param>
        /// <param name="customerActionId">The customer action identifier</param>
        /// <param name="customerRoleId">The customer role identifier</param>
        /// <param name="allow">The value indicating whether action is allowed</param>
        /// <returns>An ACL</returns>
        public override DBACL UpdateAcl(int aclId, int customerActionId,
            int customerRoleId, bool allow)
        {
            DBACL item = null;
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_ACLUpdate");
            db.AddInParameter(dbCommand, "ACLID", DbType.Int32, aclId);
            db.AddInParameter(dbCommand, "CustomerActionID", DbType.Int32, customerActionId);
            db.AddInParameter(dbCommand, "CustomerRoleID", DbType.Int32, customerRoleId);
            db.AddInParameter(dbCommand, "Allow", DbType.Boolean, allow);
            if (db.ExecuteNonQuery(dbCommand) > 0)
                item = GetAclById(aclId);

            return item;
        }

        /// <summary>
        /// Indicates whether action is allowed
        /// </summary>
        /// <param name="customerId">Customer identifer</param>
        /// <param name="actionSystemKeyword">Action system keyword</param>
        /// <returns>Result</returns>
        public override bool IsActionAllowed(int customerId, string actionSystemKeyword)
        {
            bool result = false;
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_ACLIsAllowed");
            db.AddInParameter(dbCommand, "CustomerID", DbType.Int32, customerId);
            db.AddInParameter(dbCommand, "ActionSystemKeyword", DbType.String, actionSystemKeyword);

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
