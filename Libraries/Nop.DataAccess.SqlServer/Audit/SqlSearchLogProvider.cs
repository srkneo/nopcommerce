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
using Microsoft.Practices.EnterpriseLibrary.Data;
using System.Web;
using System.Collections.Specialized;
using System.Configuration.Provider;

namespace NopSolutions.NopCommerce.DataAccess.Audit
{
    /// <summary>
    /// Search log provider for SQL Server
    /// </summary>
    public partial class SqlSearchLogProvider : DBSearchLogProvider
    {
        #region Fields
        private string _sqlConnectionString;
        #endregion

        #region Utilities
        private DBSearchLog GetSearchLogFromReader(IDataReader dataReader)
        {
            var item = new DBSearchLog();
            item.SearchLogId = NopSqlDataHelper.GetInt(dataReader, "SearchLogID");
            item.SearchTerm = NopSqlDataHelper.GetString(dataReader, "SearchTerm");
            item.CustomerId = NopSqlDataHelper.GetInt(dataReader, "CustomerID");
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
        /// Get order product variant sales report
        /// </summary>
        /// <param name="startTime">Order start time; null to load all</param>
        /// <param name="endTime">Order end time; null to load all</param>
        /// <param name="count">Item count. 0 if you want to get all items</param>
        /// <returns>Result</returns>
        public override IDataReader SearchTermReport(DateTime? startTime,
            DateTime? endTime, int count)
        {
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_SearchTermReport");
            if (startTime.HasValue)
                db.AddInParameter(dbCommand, "StartTime", DbType.DateTime, startTime.Value);
            else
                db.AddInParameter(dbCommand, "StartTime", DbType.DateTime, null);
            if (endTime.HasValue)
                db.AddInParameter(dbCommand, "EndTime", DbType.DateTime, endTime.Value);
            else
                db.AddInParameter(dbCommand, "EndTime", DbType.DateTime, null);

            db.AddInParameter(dbCommand, "Count", DbType.Int32, count);
            return db.ExecuteReader(dbCommand);
        }

        /// <summary>
        /// Gets all search log items
        /// </summary>
        /// <returns>Search log collection</returns>
        public override DBSearchLogCollection GetAllSearchLogs()
        {
            var result = new DBSearchLogCollection();
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_SearchLogLoadAll");
            using (IDataReader dataReader = db.ExecuteReader(dbCommand))
            {
                while (dataReader.Read())
                {
                    var item = GetSearchLogFromReader(dataReader);
                    result.Add(item);
                }
            }

            return result;
        }

        /// <summary>
        /// Gets a search log item
        /// </summary>
        /// <param name="searchLogId">The search log item identifier</param>
        /// <returns>Search log item</returns>
        public override DBSearchLog GetSearchLogById(int searchLogId)
        {
            DBSearchLog item = null;
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_SearchLogLoadByPrimaryKey");
            db.AddInParameter(dbCommand, "SearchLogID", DbType.Int32, searchLogId);
            using (IDataReader dataReader = db.ExecuteReader(dbCommand))
            {
                if (dataReader.Read())
                {
                    item = GetSearchLogFromReader(dataReader);
                }
            }
            return item;
        }

        /// <summary>
        /// Inserts a search log item
        /// </summary>
        /// <param name="searchTerm">The search term</param>
        /// <param name="customerId">The customer identifier</param>
        /// <param name="createdOn">The date and time of instance creation</param>
        /// <returns>Search log item</returns>
        public override DBSearchLog InsertSearchLog(string searchTerm,
            int customerId, DateTime createdOn)
        {
            DBSearchLog item = null;
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_SearchLogInsert");
            db.AddOutParameter(dbCommand, "SearchLogID", DbType.Int32, 0);
            db.AddInParameter(dbCommand, "SearchTerm", DbType.String, searchTerm);
            db.AddInParameter(dbCommand, "CustomerID", DbType.Int32, customerId);
            db.AddInParameter(dbCommand, "CreatedOn", DbType.DateTime, createdOn);
            if (db.ExecuteNonQuery(dbCommand) > 0)
            {
                int searchLogId = Convert.ToInt32(db.GetParameterValue(dbCommand, "@SearchLogID"));
                item = GetSearchLogById(searchLogId);
            }

            return item;
        }

        /// <summary>
        /// Clear search log
        /// </summary>
        public override void ClearSearchLog()
        {
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_SearchLogClear");
            db.ExecuteNonQuery(dbCommand);
        }
        #endregion
    }
}
