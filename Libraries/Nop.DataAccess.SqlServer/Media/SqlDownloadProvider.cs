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

namespace NopSolutions.NopCommerce.DataAccess.Media
{
    /// <summary>
    /// Download provider for SQL Server
    /// </summary>
    public partial class SqlDownloadProvider : DBDownloadProvider
    {
        #region Fields
        private string _sqlConnectionString;
        #endregion

        #region Utilities
        private DBDownload GetDownloadFromReader(IDataReader dataReader)
        {
            var item = new DBDownload();
            item.DownloadId = NopSqlDataHelper.GetInt(dataReader, "DownloadID");
            item.UseDownloadUrl = NopSqlDataHelper.GetBoolean(dataReader, "UseDownloadURL");
            item.DownloadUrl = NopSqlDataHelper.GetString(dataReader, "DownloadURL");
            item.DownloadBinary = NopSqlDataHelper.GetBytes(dataReader, "DownloadBinary");
            item.ContentType = NopSqlDataHelper.GetString(dataReader, "ContentType");
            item.Filename = NopSqlDataHelper.GetString(dataReader, "Filename");
            item.Extension = NopSqlDataHelper.GetString(dataReader, "Extension");
            item.IsNew = NopSqlDataHelper.GetBoolean(dataReader, "IsNew");
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
        /// Gets a download
        /// </summary>
        /// <param name="downloadId">Download identifier</param>
        /// <returns>Download</returns>
        public override DBDownload GetDownloadById(int downloadId)
        {
            DBDownload item = null;
            if (downloadId == 0)
                return item;
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_DownloadLoadByPrimaryKey");
            db.AddInParameter(dbCommand, "DownloadID", DbType.Int32, downloadId);
            using (IDataReader dataReader = db.ExecuteReader(dbCommand))
            {
                if (dataReader.Read())
                {
                    item = GetDownloadFromReader(dataReader);
                }
            }
            return item;
        }

        /// <summary>
        /// Deletes a download
        /// </summary>
        /// <param name="downloadId">Download identifier</param>
        public override void DeleteDownload(int downloadId)
        {
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_DownloadDelete");
            db.AddInParameter(dbCommand, "DownloadID", DbType.Int32, downloadId);
            db.ExecuteNonQuery(dbCommand);
        }

        /// <summary>
        /// Inserts a download
        /// </summary>
        /// <param name="useDownloadUrl">The value indicating whether DownloadURL property should be used</param>
        /// <param name="downloadUrl">The download URL</param>
        /// <param name="downloadBinary">The download binary</param>
        /// <param name="contentType">The content type</param>
        /// <param name="filename">The filename of the download</param>
        /// <param name="extension">The extension</param>
        /// <param name="isNew">A value indicating whether the download is new</param>
        /// <returns>Download</returns>
        public override DBDownload InsertDownload(bool useDownloadUrl, string downloadUrl,
            byte[] downloadBinary, string contentType, string filename,
            string extension, bool isNew)
        {
            DBDownload item = null;
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_DownloadInsert");
            db.AddOutParameter(dbCommand, "DownloadID", DbType.Int32, 0);
            db.AddInParameter(dbCommand, "UseDownloadURL", DbType.Boolean, useDownloadUrl);
            db.AddInParameter(dbCommand, "DownloadURL", DbType.String, downloadUrl);
            if (downloadBinary != null)
                db.AddInParameter(dbCommand, "DownloadBinary", DbType.Binary, downloadBinary);
            else
                db.AddInParameter(dbCommand, "DownloadBinary", DbType.Binary, null);
            db.AddInParameter(dbCommand, "ContentType", DbType.String, contentType);
            db.AddInParameter(dbCommand, "Filename", DbType.String, filename);
            db.AddInParameter(dbCommand, "Extension", DbType.String, extension);
            db.AddInParameter(dbCommand, "IsNew", DbType.Boolean, isNew);
            if (db.ExecuteNonQuery(dbCommand) > 0)
            {
                int downloadId = Convert.ToInt32(db.GetParameterValue(dbCommand, "@DownloadID"));
                item = GetDownloadById(downloadId);
            }
            return item;
        }

        /// <summary>
        /// Updates the download
        /// </summary>
        /// <param name="downloadId">The download identifier</param>
        /// <param name="useDownloadUrl">The value indicating whether DownloadURL property should be used</param>
        /// <param name="downloadUrl">The download URL</param>
        /// <param name="downloadBinary">The download binary</param>
        /// <param name="contentType">The content type</param>
        /// <param name="filename">The filename of the download</param>
        /// <param name="extension">The extension</param>
        /// <param name="isNew">A value indicating whether the download is new</param>
        /// <returns>Download</returns>
        public override DBDownload UpdateDownload(int downloadId,
            bool useDownloadUrl, string downloadUrl,
            byte[] downloadBinary, string contentType, string filename,
            string extension, bool isNew)
        {
            DBDownload item = null;
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_DownloadUpdate");
            db.AddInParameter(dbCommand, "DownloadID", DbType.Int32, downloadId);
            db.AddInParameter(dbCommand, "UseDownloadURL", DbType.Boolean, useDownloadUrl);
            db.AddInParameter(dbCommand, "DownloadURL", DbType.String, downloadUrl);
            if (downloadBinary != null)
                db.AddInParameter(dbCommand, "DownloadBinary", DbType.Binary, downloadBinary);
            else
                db.AddInParameter(dbCommand, "DownloadBinary", DbType.Binary, null);
            db.AddInParameter(dbCommand, "ContentType", DbType.String, contentType);
            db.AddInParameter(dbCommand, "Filename", DbType.String, filename);
            db.AddInParameter(dbCommand, "Extension", DbType.String, extension);
            db.AddInParameter(dbCommand, "IsNew", DbType.Boolean, isNew);
            if (db.ExecuteNonQuery(dbCommand) > 0)
                item = GetDownloadById(downloadId);

            return item;
        }
        #endregion
    }
}
