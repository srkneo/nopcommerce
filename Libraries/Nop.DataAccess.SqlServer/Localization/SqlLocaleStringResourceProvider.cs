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
using Microsoft.Practices.EnterpriseLibrary.Data.Sql;
using System.Xml;
using System.Data.SqlClient;

namespace NopSolutions.NopCommerce.DataAccess.Localization
{
    /// <summary>
    /// Locale string resource provider for SQL Server
    /// </summary>
    public partial class SqlLocaleStringResourceProvider : DBLocaleStringResourceProvider
    {
        #region Fields
        private string _sqlConnectionString;
        #endregion

        #region Utilities
        private DBLocaleStringResource GetLocaleStringResourceFromReader(IDataReader dataReader)
        {
            var item = new DBLocaleStringResource();
            item.LocaleStringResourceId = NopSqlDataHelper.GetInt(dataReader, "LocaleStringResourceID");
            item.LanguageId = NopSqlDataHelper.GetInt(dataReader, "LanguageID");
            item.ResourceName = NopSqlDataHelper.GetString(dataReader, "ResourceName");
            item.ResourceValue = NopSqlDataHelper.GetString(dataReader, "ResourceValue");
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
        /// Deletes a locale string resource
        /// </summary>
        /// <param name="localeStringResourceId">Locale string resource identifier</param>
        public override void DeleteLocaleStringResource(int localeStringResourceId)
        {
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_LocaleStringResourceDelete");
            db.AddInParameter(dbCommand, "LocaleStringResourceID", DbType.Int32, localeStringResourceId);
            db.ExecuteNonQuery(dbCommand);
        }

        /// <summary>
        /// Gets a locale string resource
        /// </summary>
        /// <param name="localeStringResourceId">Locale string resource identifier</param>
        /// <returns>Locale string resource</returns>
        public override DBLocaleStringResource GetLocaleStringResourceById(int localeStringResourceId)
        {
            DBLocaleStringResource item = null;
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_LocaleStringResourceLoadByPrimaryKey");
            db.AddInParameter(dbCommand, "LocaleStringResourceID", DbType.Int32, localeStringResourceId);
            using (IDataReader dataReader = db.ExecuteReader(dbCommand))
            {
                if (dataReader.Read())
                {
                    item = GetLocaleStringResourceFromReader(dataReader);
                }
            }
            return item;
        }

        /// <summary>
        /// Gets all locale string resources by language identifier
        /// </summary>
        /// <param name="languageId">Language identifier</param>
        /// <returns>Locale string resource collection</returns>
        public override DBLocaleStringResourceCollection GetAllResourcesByLanguageId(int languageId)
        {
            var result = new DBLocaleStringResourceCollection();
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_LocaleStringResourceLoadAllByLanguageID");
            db.AddInParameter(dbCommand, "LanguageID", DbType.Int32, languageId);
            using (IDataReader dataReader = db.ExecuteReader(dbCommand))
            {
                while (dataReader.Read())
                {
                    var item = GetLocaleStringResourceFromReader(dataReader);
                    result.Add(item);
                }
            }

            return result;
        }

        /// <summary>
        /// Inserts a locale string resource
        /// </summary>
        /// <param name="languageId">The language identifier</param>
        /// <param name="resourceName">The resource name</param>
        /// <param name="resourceValue">The resource value</param>
        /// <returns>Locale string resource</returns>
        public override DBLocaleStringResource InsertLocaleStringResource(int languageId,
            string resourceName, string resourceValue)
        {
            DBLocaleStringResource item = null;
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_LocaleStringResourceInsert");
            db.AddOutParameter(dbCommand, "LocaleStringResourceID", DbType.Int32, 0);
            db.AddInParameter(dbCommand, "LanguageID", DbType.Int32, languageId);
            db.AddInParameter(dbCommand, "ResourceName", DbType.String, resourceName);
            db.AddInParameter(dbCommand, "ResourceValue", DbType.String, resourceValue);
            if (db.ExecuteNonQuery(dbCommand) > 0)
            {
                int localeStringResourceId = Convert.ToInt32(db.GetParameterValue(dbCommand, "@LocaleStringResourceID"));
                item = GetLocaleStringResourceById(localeStringResourceId);
            }
            return item;
        }

        /// <summary>
        /// Updates the locale string resource
        /// </summary>
        /// <param name="localeStringResourceId">The locale string resource identifier</param>
        /// <param name="languageId">The language identifier</param>
        /// <param name="resourceName">The resource name</param>
        /// <param name="resourceValue">The resource value</param>
        /// <returns>Locale string resource</returns>
        public override DBLocaleStringResource UpdateLocaleStringResource(int localeStringResourceId,
            int languageId, string resourceName, string resourceValue)
        {
            DBLocaleStringResource item = null;
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_LocaleStringResourceUpdate");
            db.AddInParameter(dbCommand, "LocaleStringResourceID", DbType.Int32, localeStringResourceId);
            db.AddInParameter(dbCommand, "LanguageID", DbType.Int32, languageId);
            db.AddInParameter(dbCommand, "ResourceName", DbType.String, resourceName);
            db.AddInParameter(dbCommand, "ResourceValue", DbType.String, resourceValue);
            if (db.ExecuteNonQuery(dbCommand) > 0)
                item = GetLocaleStringResourceById(localeStringResourceId);

            return item;
        }

        /// <summary>
        /// Loads all locale string resources as XML
        /// </summary>
        /// <param name="languageId">The Language identifier</param>
        /// <returns>XML</returns>
        public override string GetAllLocaleStringResourcesAsXml(int languageId)
        {
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString) as SqlDatabase;
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_LanguagePackExport");
            db.AddInParameter(dbCommand, "LanguageID", DbType.Int32, languageId);
            db.AddOutParameter(dbCommand, "XmlPackage", DbType.Xml, Int32.MaxValue);
            db.ExecuteNonQuery(dbCommand);
            return Convert.ToString(db.GetParameterValue(dbCommand, "@XmlPackage"));
        }

        /// <summary>
        /// Inserts all locale string resources from XML
        /// </summary>
        /// <param name="languageId">The Language identifier</param>
        /// <param name="xml">The XML package</param>
        public override void InsertAllLocaleStringResourcesFromXml(int languageId, string xml)
        {
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_LanguagePackImport");
            //little hack here
            dbCommand.CommandTimeout = 600;
            db.AddInParameter(dbCommand, "LanguageID", DbType.Int32, languageId);
            db.AddInParameter(dbCommand, "XmlPackage", DbType.Xml, xml);

            db.ExecuteNonQuery(dbCommand);
        }
        #endregion
    }
}
