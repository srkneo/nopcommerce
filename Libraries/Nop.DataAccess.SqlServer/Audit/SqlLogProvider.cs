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
    /// Log provider for SQL Server
    /// </summary>
    public partial class SqlLogProvider : DBLogProvider
    {
        #region Fields
        private string _sqlConnectionString;
        #endregion

        #region Utilities
        private DBLog GetLogFromReader(IDataReader dataReader)
        {
            var item = new DBLog();
            item.LogId = NopSqlDataHelper.GetInt(dataReader, "LogID");
            item.LogTypeId = NopSqlDataHelper.GetInt(dataReader, "LogTypeID");
            item.Severity = NopSqlDataHelper.GetInt(dataReader, "Severity");
            item.Message = NopSqlDataHelper.GetString(dataReader, "Message");
            item.Exception = NopSqlDataHelper.GetString(dataReader, "Exception");
            item.IPAddress = NopSqlDataHelper.GetString(dataReader, "IPAddress");
            item.CustomerId = NopSqlDataHelper.GetInt(dataReader, "CustomerID");
            item.PageUrl = NopSqlDataHelper.GetString(dataReader, "PageURL");
            item.ReferrerUrl = NopSqlDataHelper.GetString(dataReader, "ReferrerURL");
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
        /// Deletes a log item
        /// </summary>
        /// <param name="logId">Log item identifier</param>
        public override void DeleteLog(int logId)
        {
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_LogDelete");
            db.AddInParameter(dbCommand, "LogID", DbType.Int32, logId);
            db.ExecuteNonQuery(dbCommand);
        }

        /// <summary>
        /// Clears a log
        /// </summary>
        public override void ClearLog()
        {
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_LogClear");
            db.ExecuteNonQuery(dbCommand);
        }

        /// <summary>
        /// Gets all log items
        /// </summary>
        /// <returns>Log item collection</returns>
        public override DBLogCollection GetAllLogs()
        {
            var result = new DBLogCollection();
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_LogLoadAll");
            using (IDataReader dataReader = db.ExecuteReader(dbCommand))
            {
                while (dataReader.Read())
                {
                    var item = GetLogFromReader(dataReader);
                    result.Add(item);
                }
            }

            return result;
        }

        /// <summary>
        /// Gets a log item
        /// </summary>
        /// <param name="logId">Log item identifier</param>
        /// <returns>Log item</returns>
        public override DBLog GetLogById(int logId)
        {
            DBLog item = null;
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_LogLoadByPrimaryKey");
            db.AddInParameter(dbCommand, "LogID", DbType.Int32, logId);
            using (IDataReader dataReader = db.ExecuteReader(dbCommand))
            {
                if (dataReader.Read())
                {
                    item = GetLogFromReader(dataReader);
                }
            }
            return item;
        }

        /// <summary>
        /// Inserts a log item
        /// </summary>
        /// <param name="logTypeId">Log item type identifier</param>
        /// <param name="severity">The severity</param>
        /// <param name="message">The short message</param>
        /// <param name="exception">The full exception</param>
        /// <param name="ipAddress">The IP address</param>
        /// <param name="customerId">The customer identifier</param>
        /// <param name="pageUrl">The page URL</param>
        /// <param name="referrerUrl">The referrer URL</param>
        /// <param name="createdOn">The date and time of instance creationL</param>
        /// <returns>Log item</returns>
        public override DBLog InsertLog(int logTypeId, int severity, string message,
            string exception, string ipAddress, int customerId,
            string pageUrl, string referrerUrl, DateTime createdOn)
        {
            DBLog item = null;
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_LogInsert");
            db.AddOutParameter(dbCommand, "LogID", DbType.Int32, 0);
            db.AddInParameter(dbCommand, "LogTypeID", DbType.Int32, logTypeId);
            db.AddInParameter(dbCommand, "Severity", DbType.Int32, severity);
            db.AddInParameter(dbCommand, "Message", DbType.String, message);
            db.AddInParameter(dbCommand, "Exception", DbType.String, exception);
            db.AddInParameter(dbCommand, "IPAddress", DbType.String, ipAddress);
            db.AddInParameter(dbCommand, "CustomerID", DbType.Int32, customerId);
            db.AddInParameter(dbCommand, "PageURL", DbType.String, pageUrl);
            db.AddInParameter(dbCommand, "ReferrerURL", DbType.String, referrerUrl);
            db.AddInParameter(dbCommand, "CreatedOn", DbType.DateTime, createdOn);
            if (db.ExecuteNonQuery(dbCommand) > 0)
            {
                int logId = Convert.ToInt32(db.GetParameterValue(dbCommand, "@LogID"));
                item = GetLogById(logId);
            }
            return item;
        }
        #endregion
    }
}
