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
using System.Collections.Specialized;
using System.Configuration.Provider;

namespace NopSolutions.NopCommerce.DataAccess.Directory
{
    /// <summary>
    /// Currency provider for SQL Server
    /// </summary>
    public partial class SqlCurrencyProvider : DBCurrencyProvider
    {
        #region Fields
        private string _sqlConnectionString;
        #endregion

        #region Utilities
        private DBCurrency GetCurrencyFromReader(IDataReader dataReader)
        {
            var item = new DBCurrency();
            item.CurrencyId = NopSqlDataHelper.GetInt(dataReader, "CurrencyID");
            item.Name = NopSqlDataHelper.GetString(dataReader, "Name");
            item.CurrencyCode = NopSqlDataHelper.GetString(dataReader, "CurrencyCode");
            item.Rate = NopSqlDataHelper.GetDecimal(dataReader, "Rate");
            item.DisplayLocale = NopSqlDataHelper.GetString(dataReader, "DisplayLocale");
            item.CustomFormatting = NopSqlDataHelper.GetString(dataReader, "CustomFormatting");
            item.Published = NopSqlDataHelper.GetBoolean(dataReader, "Published");
            item.DisplayOrder = NopSqlDataHelper.GetInt(dataReader, "DisplayOrder");
            item.CreatedOn = NopSqlDataHelper.GetUtcDateTime(dataReader, "CreatedOn");
            item.UpdatedOn = NopSqlDataHelper.GetUtcDateTime(dataReader, "UpdatedOn");
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
        /// Deletes currency
        /// </summary>
        /// <param name="currencyId">Currency identifier</param>
        public override void DeleteCurrency(int currencyId)
        {
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_CurrencyDelete");
            db.AddInParameter(dbCommand, "CurrencyID", DbType.Int32, currencyId);
            db.ExecuteNonQuery(dbCommand);
        }

        /// <summary>
        /// Gets a currency
        /// </summary>
        /// <param name="currencyId">Currency identifier</param>
        /// <returns>Currency</returns>
        public override DBCurrency GetCurrencyById(int currencyId)
        {
            DBCurrency item = null;
            if (currencyId == 0)
                return item;
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_CurrencyLoadByPrimaryKey");
            db.AddInParameter(dbCommand, "CurrencyID", DbType.Int32, currencyId);
            using (IDataReader dataReader = db.ExecuteReader(dbCommand))
            {
                if (dataReader.Read())
                {
                    item = GetCurrencyFromReader(dataReader);
                }
            }
            return item;
        }

        /// <summary>
        /// Gets all currencies
        /// </summary>
        /// <returns>Currency collection</returns>
        public override DBCurrencyCollection GetAllCurrencies(bool showHidden)
        {
            var result = new DBCurrencyCollection();
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_CurrencyLoadAll");
            db.AddInParameter(dbCommand, "ShowHidden", DbType.Boolean, showHidden);
            using (IDataReader dataReader = db.ExecuteReader(dbCommand))
            {
                while (dataReader.Read())
                {
                    var item = GetCurrencyFromReader(dataReader);
                    result.Add(item);
                }
            }

            return result;
        }

        /// <summary>
        /// Inserts a currency
        /// </summary>
        /// <param name="name">The name</param>
        /// <param name="currencyCode">The currency code</param>
        /// <param name="rate">The rate</param>
        /// <param name="displayLocale">The display locale</param>
        /// <param name="customFormatting">The custom formatting</param>
        /// <param name="published">A value indicating whether the entity is published</param>
        /// <param name="displayOrder">The display order</param>
        /// <param name="createdOn">The date and time of instance creation</param>
        /// <param name="updatedOn">The date and time of instance update</param>
        /// <returns>A currency</returns>
        public override DBCurrency InsertCurrency(string name,
            string currencyCode, decimal rate, string displayLocale,
            string customFormatting, bool published, int displayOrder,
            DateTime createdOn, DateTime updatedOn)
        {
            DBCurrency item = null;
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_CurrencyInsert");
            db.AddOutParameter(dbCommand, "CurrencyID", DbType.Int32, 0);
            db.AddInParameter(dbCommand, "Name", DbType.String, name);
            db.AddInParameter(dbCommand, "CurrencyCode", DbType.String, currencyCode);
            db.AddInParameter(dbCommand, "Rate", DbType.Decimal, rate);
            db.AddInParameter(dbCommand, "DisplayLocale", DbType.String, displayLocale);
            db.AddInParameter(dbCommand, "CustomFormatting", DbType.String, customFormatting);
            db.AddInParameter(dbCommand, "Published", DbType.Boolean, published);
            db.AddInParameter(dbCommand, "DisplayOrder", DbType.Int32, displayOrder);
            db.AddInParameter(dbCommand, "CreatedOn", DbType.DateTime, createdOn);
            db.AddInParameter(dbCommand, "UpdatedOn", DbType.DateTime, updatedOn);
            if (db.ExecuteNonQuery(dbCommand) > 0)
            {
                int currencyId = Convert.ToInt32(db.GetParameterValue(dbCommand, "@CurrencyID"));
                item = GetCurrencyById(currencyId);
            }
            return item;
        }

        /// <summary>
        /// Updates the currency
        /// </summary>
        /// <param name="currencyId">Currency identifier</param>
        /// <param name="name">The name</param>
        /// <param name="currencyCode">The currency code</param>
        /// <param name="rate">The rate</param>
        /// <param name="displayLocale">The display locale</param>
        /// <param name="customFormatting">The custom formatting</param>
        /// <param name="published">A value indicating whether the entity is published</param>
        /// <param name="displayOrder">The display order</param>
        /// <param name="createdOn">The date and time of instance creation</param>
        /// <param name="updatedOn">The date and time of instance update</param>
        /// <returns>A currency</returns>
        public override DBCurrency UpdateCurrency(int currencyId, string name,
            string currencyCode, decimal rate, string displayLocale,
            string customFormatting, bool published, int displayOrder,
            DateTime createdOn, DateTime updatedOn)
        {
            DBCurrency item = null;
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_CurrencyUpdate");
            db.AddInParameter(dbCommand, "CurrencyID", DbType.Int32, currencyId);
            db.AddInParameter(dbCommand, "Name", DbType.String, name);
            db.AddInParameter(dbCommand, "CurrencyCode", DbType.String, currencyCode);
            db.AddInParameter(dbCommand, "Rate", DbType.Decimal, rate);
            db.AddInParameter(dbCommand, "DisplayLocale", DbType.String, displayLocale);
            db.AddInParameter(dbCommand, "CustomFormatting", DbType.String, customFormatting);
            db.AddInParameter(dbCommand, "Published", DbType.Boolean, published);
            db.AddInParameter(dbCommand, "DisplayOrder", DbType.Int32, displayOrder);
            db.AddInParameter(dbCommand, "CreatedOn", DbType.DateTime, createdOn);
            db.AddInParameter(dbCommand, "UpdatedOn", DbType.DateTime, updatedOn);
            if (db.ExecuteNonQuery(dbCommand) > 0)
                item = GetCurrencyById(currencyId);

            return item;
        }
        #endregion
    }
}
