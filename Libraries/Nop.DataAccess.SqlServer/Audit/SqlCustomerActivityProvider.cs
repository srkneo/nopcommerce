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

namespace NopSolutions.NopCommerce.DataAccess.Audit
{
    /// <summary>
    /// Customer activity provider for SQL Server
    /// </summary>
    public partial class SqlCustomerActivityProvider : DBCustomerActivityProvider
    {
        #region Fields
        private string _sqlConnectionString;
        #endregion

        #region Utilities

        private DBActivityLog GetActivityLogFromReader(IDataReader dataReader)
        {
            var item = new DBActivityLog();
            item.ActivityLogId = NopSqlDataHelper.GetInt(dataReader, "ActivityLogID");
            item.ActivityLogTypeId = NopSqlDataHelper.GetInt(dataReader, "ActivityLogTypeID");
            item.CustomerId = NopSqlDataHelper.GetInt(dataReader, "CustomerID");
            item.Comment = NopSqlDataHelper.GetString(dataReader, "Comment");
            item.CreatedOn = NopSqlDataHelper.GetUtcDateTime(dataReader, "CreatedOn");
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
        public override DBActivityLogCollection GetAllActivities(DateTime? createdOnFrom,
            DateTime? createdOnTo, string email, string username, int activityLogTypeId,
            int pageSize, int pageIndex, out int totalRecords)
        {
            totalRecords = 0;
            var result = new DBActivityLogCollection();
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_ActivityLogLoadAll");
            if (createdOnFrom.HasValue)
                db.AddInParameter(dbCommand, "CreatedOnFrom", DbType.DateTime, createdOnFrom.Value);
            else
                db.AddInParameter(dbCommand, "CreatedOnFrom", DbType.DateTime, null);
            if (createdOnTo.HasValue)
                db.AddInParameter(dbCommand, "CreatedOnTo", DbType.DateTime, createdOnTo.Value);
            else
                db.AddInParameter(dbCommand, "CreatedOnTo", DbType.DateTime, null);
            db.AddInParameter(dbCommand, "Email", DbType.String, email);
            db.AddInParameter(dbCommand, "Username", DbType.String, username);
            if (activityLogTypeId > 0)
                db.AddInParameter(dbCommand, "ActivityLogTypeID", DbType.Int32, activityLogTypeId);
            else
                db.AddInParameter(dbCommand, "ActivityLogTypeID", DbType.Int32, null);
            db.AddInParameter(dbCommand, "PageSize", DbType.Int32, pageSize);
            db.AddInParameter(dbCommand, "PageIndex", DbType.Int32, pageIndex);
            db.AddOutParameter(dbCommand, "TotalRecords", DbType.Int32, 0);
            using (IDataReader dataReader = db.ExecuteReader(dbCommand))
            {
                while (dataReader.Read())
                {
                    var item = GetActivityLogFromReader(dataReader);
                    result.Add(item);
                }
            }
            totalRecords = Convert.ToInt32(db.GetParameterValue(dbCommand, "@TotalRecords"));

            return result;
        }
        
        /// <summary>
        /// Clears activity log
        /// </summary>
        public override void ClearAllActivities()
        {
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_ActivityLogClearAll");
            db.ExecuteNonQuery(dbCommand);
        }
        #endregion
    }
}