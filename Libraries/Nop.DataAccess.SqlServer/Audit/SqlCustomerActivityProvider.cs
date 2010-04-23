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
    public partial class SQLCustomerActivityProvider : DBCustomerActivityProvider
    {
        #region Fields
        private string _sqlConnectionString;
        #endregion

        #region Utilities
        private DBActivityLogType GetActivityLogTypeFromReader(IDataReader dataReader)
        {
            DBActivityLogType activityType = new DBActivityLogType();
            activityType.ActivityLogTypeID = NopSqlDataHelper.GetInt(dataReader, "ActivityLogTypeID");
            activityType.SystemKeyword = NopSqlDataHelper.GetString(dataReader, "SystemKeyword");
            activityType.Name = NopSqlDataHelper.GetString(dataReader, "Name");
            activityType.Enabled = NopSqlDataHelper.GetBoolean(dataReader, "Enabled");
            return activityType;
        }
        
        private DBActivityLog GetActivityLogFromReader(IDataReader dataReader)
        {
            DBActivityLog activity = new DBActivityLog();
            activity.ActivityLogID = NopSqlDataHelper.GetInt(dataReader, "ActivityLogID");
            activity.ActivityLogTypeID = NopSqlDataHelper.GetInt(dataReader, "ActivityLogTypeID");
            activity.CustomerID = NopSqlDataHelper.GetInt(dataReader, "CustomerID");
            activity.Comment = NopSqlDataHelper.GetString(dataReader, "Comment");
            activity.CreatedOn = NopSqlDataHelper.GetUtcDateTime(dataReader, "CreatedOn");
            return activity;
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
        /// <param name="SystemKeyword">The system keyword</param>
        /// <param name="Name">The display name</param>
        /// <param name="Enabled">Value indicating whether the activity log type is enabled</param>
        /// <returns>Activity log type item</returns>
        public override DBActivityLogType InsertActivityType(string SystemKeyword, string Name, bool Enabled)
        {
            DBActivityLogType activityType = null;
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_ActivityLogTypeInsert");
            db.AddOutParameter(dbCommand, "ActivityLogTypeID", DbType.Int32, 0);
            db.AddInParameter(dbCommand, "SystemKeyword", DbType.String, SystemKeyword);
            db.AddInParameter(dbCommand, "Name", DbType.String, Name);
            db.AddInParameter(dbCommand, "Enabled", DbType.Boolean, Enabled);
            if (db.ExecuteNonQuery(dbCommand) > 0)
            {
                int ActivityLogTypeID = Convert.ToInt32(db.GetParameterValue(dbCommand, "@ActivityLogTypeID"));
                activityType = GetActivityTypeByID(ActivityLogTypeID);
            }
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
        public override DBActivityLogType UpdateActivityType(int ActivityLogTypeID, string SystemKeyword, string Name, bool Enabled)
        {
            DBActivityLogType activityType = null;
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_ActivityLogTypeUpdate");
            db.AddInParameter(dbCommand, "ActivityLogTypeID", DbType.Int32, ActivityLogTypeID);
            db.AddInParameter(dbCommand, "SystemKeyword", DbType.String, SystemKeyword);
            db.AddInParameter(dbCommand, "Name", DbType.String, Name);
            db.AddInParameter(dbCommand, "Enabled", DbType.Boolean, Enabled);
            if (db.ExecuteNonQuery(dbCommand) > 0)
                activityType = GetActivityTypeByID(ActivityLogTypeID);
            return activityType;
        }
        
        /// <summary>
        /// Deletes an activity log type item
        /// </summary>
        /// <param name="ActivityLogTypeID">Activity log type identifier</param>
        public override void DeleteActivityType(int ActivityLogTypeID)
        {
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_ActivityLogTypeDelete");
            db.AddInParameter(dbCommand, "ActivityLogTypeID", DbType.Int32, ActivityLogTypeID);
            int retValue = db.ExecuteNonQuery(dbCommand);
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
        /// <param name="ActivityLogTypeID">Activity log type identifier</param>
        /// <returns>Activity log type item</returns>
        public override DBActivityLogType GetActivityTypeByID(int ActivityLogTypeID)
        {
            DBActivityLogType activityType = null;
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_ActivityLogTypeLoadByPrimaryKey");
            db.AddInParameter(dbCommand, "ActivityLogTypeID", DbType.Int32, ActivityLogTypeID);
            using (IDataReader dataReader = db.ExecuteReader(dbCommand))
            {
                if (dataReader.Read())
                    activityType = GetActivityLogTypeFromReader(dataReader);
            }
            return activityType;
        }

        /// <summary>
        /// Inserts an activity log item
        /// </summary>
        /// <param name="ActivityLogTypeID">Activity log type identifier</param>
        /// <param name="CustomerID">The customer identifier</param>
        /// <param name="Comment">The activity comment</param>
        /// <param name="CreatedOn">The date and time of instance creation</param>
        /// <returns>Activity log item</returns>
        public override DBActivityLog InsertActivity(int ActivityLogTypeID, int CustomerID, string Comment, DateTime CreatedOn)
        {
            DBActivityLog activity = null;
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_ActivityLogInsert");
            db.AddOutParameter(dbCommand, "ActivityLogID", DbType.Int32, 0);
            db.AddInParameter(dbCommand, "ActivityLogTypeID", DbType.Int32, ActivityLogTypeID);
            db.AddInParameter(dbCommand, "CustomerID", DbType.Int32, CustomerID);
            db.AddInParameter(dbCommand, "Comment", DbType.String, Comment);
            db.AddInParameter(dbCommand, "CreatedOn", DbType.DateTime, CreatedOn);
            if (db.ExecuteNonQuery(dbCommand) > 0)
            {
                int ActivityLogID = Convert.ToInt32(db.GetParameterValue(dbCommand, "@ActivityLogID"));
                activity = GetActivityByID(ActivityLogID);
            }
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
        public override DBActivityLog UpdateActivity(int ActivityLogID, int ActivityLogTypeID, int CustomerID, string Comment, DateTime CreatedOn)
        {
            DBActivityLog activity = null;
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_ActivityLogUpdate");
            db.AddInParameter(dbCommand, "ActivityLogID", DbType.Int32, ActivityLogID);
            db.AddInParameter(dbCommand, "ActivityLogTypeID", DbType.Int32, ActivityLogTypeID);
            db.AddInParameter(dbCommand, "CustomerID", DbType.Int32, CustomerID);
            db.AddInParameter(dbCommand, "Comment", DbType.String, Comment);
            db.AddInParameter(dbCommand, "CreatedOn", DbType.DateTime, CreatedOn);
            if (db.ExecuteNonQuery(dbCommand) > 0)
                activity = GetActivityByID(ActivityLogID);
            return activity;
        }
        
        /// <summary>
        /// Deletes an activity log item
        /// </summary>
        /// <param name="ActivityLogID">Activity log type identifier</param>
        public override void DeleteActivity(int ActivityLogID)
        {
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_ActivityLogDelete");
            db.AddInParameter(dbCommand, "ActivityLogID", DbType.Int32, ActivityLogID);
            int retValue = db.ExecuteNonQuery(dbCommand);
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
        public override DBActivityLogCollection GetAllActivities(DateTime? CreatedOnFrom, DateTime? CreatedOnTo,
            string Email, string Username, int ActivityLogTypeID,
            int PageSize, int PageIndex, out int TotalRecords)
        {
            TotalRecords = 0;
            var result = new DBActivityLogCollection();
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_ActivityLogLoadAll");
            if (CreatedOnFrom.HasValue)
                db.AddInParameter(dbCommand, "CreatedOnFrom", DbType.DateTime, CreatedOnFrom.Value);
            else
                db.AddInParameter(dbCommand, "CreatedOnFrom", DbType.DateTime, null);
            if (CreatedOnTo.HasValue)
                db.AddInParameter(dbCommand, "CreatedOnTo", DbType.DateTime, CreatedOnTo.Value);
            else
                db.AddInParameter(dbCommand, "CreatedOnTo", DbType.DateTime, null);
            db.AddInParameter(dbCommand, "Email", DbType.String, Email);
            db.AddInParameter(dbCommand, "Username", DbType.String, Username);
            if (ActivityLogTypeID > 0)
                db.AddInParameter(dbCommand, "ActivityLogTypeID", DbType.Int32, ActivityLogTypeID);
            else
                db.AddInParameter(dbCommand, "ActivityLogTypeID", DbType.Int32, null);
            db.AddInParameter(dbCommand, "PageSize", DbType.Int32, PageSize);
            db.AddInParameter(dbCommand, "PageIndex", DbType.Int32, PageIndex);
            db.AddOutParameter(dbCommand, "TotalRecords", DbType.Int32, 0);
            using (IDataReader dataReader = db.ExecuteReader(dbCommand))
            {
                while (dataReader.Read())
                {
                    var item = GetActivityLogFromReader(dataReader);
                    result.Add(item);
                }
            }
            TotalRecords = Convert.ToInt32(db.GetParameterValue(dbCommand, "@TotalRecords"));

            return result;
        }
        
        /// <summary>
        /// Gets an activity log item
        /// </summary>
        /// <param name="ActivityLogID">Activity log identifier</param>
        /// <returns>Activity log item</returns>
        public override DBActivityLog GetActivityByID(int ActivityLogID)
        {
            DBActivityLog activity = null;
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_ActivityLogLoadByPrimaryKey");
            db.AddInParameter(dbCommand, "ActivityLogID", DbType.Int32, ActivityLogID);
            using (IDataReader dataReader = db.ExecuteReader(dbCommand))
            {
                if (dataReader.Read())
                    activity = GetActivityLogFromReader(dataReader);
            }
            return activity;
        }

        /// <summary>
        /// Clears activity log
        /// </summary>
        public override void ClearAllActivities()
        {
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_ActivityLogClearAll");
            int retValue = db.ExecuteNonQuery(dbCommand);
        }
        #endregion
    }
}