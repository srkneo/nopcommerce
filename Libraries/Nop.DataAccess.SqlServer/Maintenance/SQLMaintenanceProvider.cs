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
using System.Data.SqlClient;

namespace NopSolutions.NopCommerce.DataAccess.Maintenance
{
    /// <summary>
    /// Maintenance provider for SQL Server
    /// </summary>
    public partial class SQLMaintenanceProvider : DBMaintenanceProvider
    {
        #region Fields
        private string _sqlConnectionString;
        #endregion

        #region Utilities


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
        /// Reindex
        /// </summary>
        public override void Reindex()
        {
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_Maintenance_ReindexTables");
            db.ExecuteNonQuery(dbCommand);
        }
        /// <summary>
        /// Backup database
        /// </summary>
        /// <param name="fileName">Destination file name</param>
        public override void Backup(string fileName)
        {
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);

            string dbName = NopSqlDataHelper.GetDatabaseName(_sqlConnectionString);
            string commandText = string.Format(
                "BACKUP DATABASE [{0}] TO DISK = '{1}' WITH FORMAT",
                dbName,
                fileName);

            DbCommand dbCommand = db.GetSqlStringCommand(commandText);
            db.ExecuteNonQuery(dbCommand);
        }
        /// <summary>
        /// Restore database
        /// </summary>
        /// <param name="fileName">Target file name</param>
        public override void RestoreBackup(string fileName)
        {
            string masterConnectionString = NopSqlDataHelper.GetMasterConnectionString(_sqlConnectionString);
            Database db = NopSqlDataHelper.CreateConnection(masterConnectionString);

            string dbName = NopSqlDataHelper.GetDatabaseName(_sqlConnectionString);
            string commandText = string.Format(
                "DECLARE @ErrorMessage NVARCHAR(4000)\n" +
                "ALTER DATABASE [{0}] SET SINGLE_USER WITH ROLLBACK IMMEDIATE\n" +
                "BEGIN TRY\n" +
                    "RESTORE DATABASE [{0}] FROM DISK = '{1}' WITH REPLACE\n" +
                "END TRY\n" +
                "BEGIN CATCH\n" +
                    "SET @ErrorMessage = ERROR_MESSAGE()\n" +
                "END CATCH\n" +
                "ALTER DATABASE [{0}] SET MULTI_USER WITH ROLLBACK IMMEDIATE\n" +
                "IF (@ErrorMessage is not NULL)\n" +
                "BEGIN\n" +
                    "RAISERROR (@ErrorMessage, 16, 1)\n" +
                "END",
                dbName,
                fileName);

            DbCommand dbCommand = db.GetSqlStringCommand(commandText);
            db.ExecuteNonQuery(dbCommand);

            //clear all pools
            SqlConnection.ClearAllPools();
        }
        #endregion
    }
}
