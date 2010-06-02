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
        private DBActivityLogType GetActivityLogTypeFromReader(IDataReader dataReader)
        {
            var item = new DBActivityLogType();
            item.ActivityLogTypeId = NopSqlDataHelper.GetInt(dataReader, "ActivityLogTypeID");
            item.SystemKeyword = NopSqlDataHelper.GetString(dataReader, "SystemKeyword");
            item.Name = NopSqlDataHelper.GetString(dataReader, "Name");
            item.Enabled = NopSqlDataHelper.GetBoolean(dataReader, "Enabled");
            return item;
        }
        
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
        /// Inserts an activity log type item
        /// </summary>
        /// <param name="systemKeyword">The system keyword</param>
        /// <param name="name">The display name</param>
        /// <param name="enabled">Value indicating whether the activity log type is enabled</param>
        /// <returns>Activity log type item</returns>
        public override DBActivityLogType InsertActivityType(string systemKeyword,
            string name, bool enabled)
        {
            DBActivityLogType item = null;
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_ActivityLogTypeInsert");
            db.AddOutParameter(dbCommand, "ActivityLogTypeID", DbType.Int32, 0);
            db.AddInParameter(dbCommand, "SystemKeyword", DbType.String, systemKeyword);
            db.AddInParameter(dbCommand, "Name", DbType.String, name);
            db.AddInParameter(dbCommand, "Enabled", DbType.Boolean, enabled);
            if (db.ExecuteNonQuery(dbCommand) > 0)
            {
                int activityLogTypeId = Convert.ToInt32(db.GetParameterValue(dbCommand, "@ActivityLogTypeID"));
                item = GetActivityTypeById(activityLogTypeId);
            }
            return item;
        }

        /// <summary>
        /// Updates an activity log type item
        /// </summary>
        /// <param name="activityLogTypeId">Activity log type identifier</param>
        /// <param name="systemKeyword">The system keyword</param>
        /// <param name="name">The display name</param>
        /// <param name="enabled">Value indicating whether the activity log type is enabled</param>
        /// <returns>Activity log type item</returns>
        public override DBActivityLogType UpdateActivityType(int activityLogTypeId,
            string systemKeyword, string name, bool enabled)
        {
            DBActivityLogType item = null;
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_ActivityLogTypeUpdate");
            db.AddInParameter(dbCommand, "ActivityLogTypeID", DbType.Int32, activityLogTypeId);
            db.AddInParameter(dbCommand, "SystemKeyword", DbType.String, systemKeyword);
            db.AddInParameter(dbCommand, "Name", DbType.String, name);
            db.AddInParameter(dbCommand, "Enabled", DbType.Boolean, enabled);
            if (db.ExecuteNonQuery(dbCommand) > 0)
                item = GetActivityTypeById(activityLogTypeId);
            return item;
        }

        /// <summary>
        /// Deletes an activity log type item
        /// </summary>
        /// <param name="activityLogTypeId">Activity log type identifier</param>
        public override void DeleteActivityType(int activityLogTypeId)
        {
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_ActivityLogTypeDelete");
            db.AddInParameter(dbCommand, "ActivityLogTypeID", DbType.Int32, activityLogTypeId);
            db.ExecuteNonQuery(dbCommand);
        }

        /// <summary>
        /// Gets all activity log type items
        /// </summary>
        /// <returns>Activity log type collection</returns>
        public override DBActivityLogTypeCollection GetAllActivityTypes()
        {
            var result = new DBActivityLogTypeCollection();
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_ActivityLogTypeLoadAll");
            using (IDataReader dataReader = db.ExecuteReader(dbCommand))
            {
                while (dataReader.Read())
                {
                    var item = GetActivityLogTypeFromReader(dataReader);
                    result.Add(item);
                }
            }

            return result;
        }

        /// <summary>
        /// Gets an activity log type item
        /// </summary>
        /// <param name="activityLogTypeId">Activity log type identifier</param>
        /// <returns>Activity log type item</returns>
        public override DBActivityLogType GetActivityTypeById(int activityLogTypeId)
        {
            DBActivityLogType item = null;
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_ActivityLogTypeLoadByPrimaryKey");
            db.AddInParameter(dbCommand, "ActivityLogTypeID", DbType.Int32, activityLogTypeId);
            using (IDataReader dataReader = db.ExecuteReader(dbCommand))
            {
                if (dataReader.Read())
                    item = GetActivityLogTypeFromReader(dataReader);
            }
            return item;
        }

        /// <summary>
        /// Inserts an activity log item
        /// </summary>
        /// <param name="activityLogTypeId">Activity log type identifier</param>
        /// <param name="customerId">The customer identifier</param>
        /// <param name="comment">The activity comment</param>
        /// <param name="createdOn">The date and time of instance creation</param>
        /// <returns>Activity log item</returns>
        public override DBActivityLog InsertActivity(int activityLogTypeId,
            int customerId, string comment, DateTime createdOn)
        {
            DBActivityLog item = null;
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_ActivityLogInsert");
            db.AddOutParameter(dbCommand, "ActivityLogID", DbType.Int32, 0);
            db.AddInParameter(dbCommand, "ActivityLogTypeID", DbType.Int32, activityLogTypeId);
            db.AddInParameter(dbCommand, "CustomerID", DbType.Int32, customerId);
            db.AddInParameter(dbCommand, "Comment", DbType.String, comment);
            db.AddInParameter(dbCommand, "CreatedOn", DbType.DateTime, createdOn);
            if (db.ExecuteNonQuery(dbCommand) > 0)
            {
                int activityLogId = Convert.ToInt32(db.GetParameterValue(dbCommand, "@ActivityLogID"));
                item = GetActivityById(activityLogId);
            }
            return item;
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
        public override DBActivityLog UpdateActivity(int activityLogId,
            int activityLogTypeId, int customerId, string comment, DateTime createdOn)
        {
            DBActivityLog item = null;
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_ActivityLogUpdate");
            db.AddInParameter(dbCommand, "ActivityLogID", DbType.Int32, activityLogId);
            db.AddInParameter(dbCommand, "ActivityLogTypeID", DbType.Int32, activityLogTypeId);
            db.AddInParameter(dbCommand, "CustomerID", DbType.Int32, customerId);
            db.AddInParameter(dbCommand, "Comment", DbType.String, comment);
            db.AddInParameter(dbCommand, "CreatedOn", DbType.DateTime, createdOn);
            if (db.ExecuteNonQuery(dbCommand) > 0)
                item = GetActivityById(activityLogId);
            return item;
        }

        /// <summary>
        /// Deletes an activity log item
        /// </summary>
        /// <param name="activityLogId">Activity log type identifier</param>
        public override void DeleteActivity(int activityLogId)
        {
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_ActivityLogDelete");
            db.AddInParameter(dbCommand, "ActivityLogID", DbType.Int32, activityLogId);
            db.ExecuteNonQuery(dbCommand);
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
        /// Gets an activity log item
        /// </summary>
        /// <param name="activityLogId">Activity log identifier</param>
        /// <returns>Activity log item</returns>
        public override DBActivityLog GetActivityById(int activityLogId)
        {
            DBActivityLog item = null;
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_ActivityLogLoadByPrimaryKey");
            db.AddInParameter(dbCommand, "ActivityLogID", DbType.Int32, activityLogId);
            using (IDataReader dataReader = db.ExecuteReader(dbCommand))
            {
                if (dataReader.Read())
                    item = GetActivityLogFromReader(dataReader);
            }
            return item;
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