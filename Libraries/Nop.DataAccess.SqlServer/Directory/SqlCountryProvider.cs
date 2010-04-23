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


namespace NopSolutions.NopCommerce.DataAccess.Directory
{
    /// <summary>
    /// Country provider for SQL Server
    /// </summary>
    public partial class SqlCountryProvider : DBCountryProvider
    {
        #region Fields
        private string _sqlConnectionString;
        #endregion

        #region Utilities
        private DBCountry GetCountryFromReader(IDataReader dataReader)
        {
            var item = new DBCountry();
            item.CountryId = NopSqlDataHelper.GetInt(dataReader, "CountryID");
            item.Name = NopSqlDataHelper.GetString(dataReader, "Name");
            item.AllowsRegistration = NopSqlDataHelper.GetBoolean(dataReader, "AllowsRegistration");
            item.AllowsBilling = NopSqlDataHelper.GetBoolean(dataReader, "AllowsBilling");
            item.AllowsShipping = NopSqlDataHelper.GetBoolean(dataReader, "AllowsShipping");
            item.TwoLetterIsoCode = NopSqlDataHelper.GetString(dataReader, "TwoLetterISOCode");
            item.ThreeLetterIsoCode = NopSqlDataHelper.GetString(dataReader, "ThreeLetterISOCode");
            item.NumericIsoCode = NopSqlDataHelper.GetInt(dataReader, "NumericISOCode");
            item.Published = NopSqlDataHelper.GetBoolean(dataReader, "Published");
            item.DisplayOrder = NopSqlDataHelper.GetInt(dataReader, "DisplayOrder");
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
        /// Deletes a country
        /// </summary>
        /// <param name="countryId">Country identifier</param>
        public override void DeleteCountry(int countryId)
        {
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_CountryDelete");
            db.AddInParameter(dbCommand, "CountryID", DbType.Int32, countryId);
            db.ExecuteNonQuery(dbCommand);
        }

        /// <summary>
        /// Gets all countries
        /// </summary>
        /// <returns>Country collection</returns>
        public override DBCountryCollection GetAllCountries(bool showHidden)
        {
            var result = new DBCountryCollection();
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_CountryLoadAll");
            db.AddInParameter(dbCommand, "ShowHidden", DbType.Boolean, showHidden);
            using (IDataReader dataReader = db.ExecuteReader(dbCommand))
            {
                while (dataReader.Read())
                {
                    var item = GetCountryFromReader(dataReader);
                    result.Add(item);
                }
            }
            return result;
        }

        /// <summary>
        /// Gets all countries that allow registration
        /// </summary>
        /// <returns>Country collection</returns>
        public override DBCountryCollection GetAllCountriesForRegistration(bool showHidden)
        {
            var result = new DBCountryCollection();
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_CountryLoadAllForRegistration");
            db.AddInParameter(dbCommand, "ShowHidden", DbType.Boolean, showHidden);
            using (IDataReader dataReader = db.ExecuteReader(dbCommand))
            {
                while (dataReader.Read())
                {
                    var item = GetCountryFromReader(dataReader);
                    result.Add(item);
                }
            }
            return result;
        }

        /// <summary>
        /// Gets all countries that allow billing
        /// </summary>
        /// <returns>Country collection</returns>
        public override DBCountryCollection GetAllCountriesForBilling(bool showHidden)
        {
            var result = new DBCountryCollection();
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_CountryLoadAllForBilling");
            db.AddInParameter(dbCommand, "ShowHidden", DbType.Boolean, showHidden);
            using (IDataReader dataReader = db.ExecuteReader(dbCommand))
            {
                while (dataReader.Read())
                {
                    var item = GetCountryFromReader(dataReader);
                    result.Add(item);
                }
            }
            return result;
        }

        /// <summary>
        /// Gets all countries that allow shipping
        /// </summary>
        /// <returns>Country collection</returns>
        public override DBCountryCollection GetAllCountriesForShipping(bool showHidden)
        {
            var result = new DBCountryCollection();
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_CountryLoadAllForShipping");
            db.AddInParameter(dbCommand, "ShowHidden", DbType.Boolean, showHidden);
            using (IDataReader dataReader = db.ExecuteReader(dbCommand))
            {
                while (dataReader.Read())
                {
                    var item = GetCountryFromReader(dataReader);
                    result.Add(item);
                }
            }
            return result;
        }

        /// <summary>
        /// Gets a country 
        /// </summary>
        /// <param name="countryId">Country identifier</param>
        /// <returns>Country</returns>
        public override DBCountry GetCountryById(int countryId)
        {
            DBCountry item = null;
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_CountryLoadByPrimaryKey");
            db.AddInParameter(dbCommand, "CountryID", DbType.Int32, countryId);
            using (IDataReader dataReader = db.ExecuteReader(dbCommand))
            {
                if (dataReader.Read())
                {
                    item = GetCountryFromReader(dataReader);
                }
            }
            return item;
        }

        /// <summary>
        /// Gets a country by two letter ISO code
        /// </summary>
        /// <param name="twoLetterIsoCode">Country two letter ISO code</param>
        /// <returns>Country</returns>
        public override DBCountry GetCountryByTwoLetterIsoCode(string twoLetterIsoCode)
        {
            DBCountry item = null;
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_CountryLoadByTwoLetterISOCode");
            db.AddInParameter(dbCommand, "TwoLetterISOCode", DbType.String, twoLetterIsoCode);
            using (IDataReader dataReader = db.ExecuteReader(dbCommand))
            {
                if (dataReader.Read())
                {
                    item = GetCountryFromReader(dataReader);
                }
            }
            return item;
        }

        /// <summary>
        /// Gets a country by three letter ISO code
        /// </summary>
        /// <param name="threeLetterIsoCode">Country three letter ISO code</param>
        /// <returns>Country</returns>
        public override DBCountry GetCountryByThreeLetterIsoCode(string threeLetterIsoCode)
        {
            DBCountry item = null;
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_CountryLoadByThreeLetterISOCode");
            db.AddInParameter(dbCommand, "ThreeLetterISOCode", DbType.String, threeLetterIsoCode);
            using (IDataReader dataReader = db.ExecuteReader(dbCommand))
            {
                if (dataReader.Read())
                {
                    item = GetCountryFromReader(dataReader);
                }
            }
            return item;
        }

        /// <summary>
        /// Inserts a country
        /// </summary>
        /// <param name="name">The name</param>
        /// <param name="allowsRegistration">A value indicating whether registration is allowed to this country</param>
        /// <param name="allowsBilling">A value indicating whether billing is allowed to this country</param>
        /// <param name="allowsShipping">A value indicating whether shipping is allowed to this country</param>
        /// <param name="twoLetterIsoCode">The two letter ISO code</param>
        /// <param name="threeLetterIsoCode">The three letter ISO code</param>
        /// <param name="numericIsoCode">The numeric ISO code</param>
        /// <param name="published">A value indicating whether the entity is published</param>
        /// <param name="displayOrder">The display order</param>
        /// <returns>Country</returns>
        public override DBCountry InsertCountry(string name,
            bool allowsRegistration, bool allowsBilling, bool allowsShipping,
            string twoLetterIsoCode, string threeLetterIsoCode, int numericIsoCode,
            bool published, int displayOrder)
        {
            DBCountry item = null;
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_CountryInsert");
            db.AddOutParameter(dbCommand, "CountryID", DbType.Int32, 0);
            db.AddInParameter(dbCommand, "Name", DbType.String, name);
            db.AddInParameter(dbCommand, "AllowsRegistration", DbType.Boolean, allowsRegistration);
            db.AddInParameter(dbCommand, "AllowsBilling", DbType.Boolean, allowsBilling);
            db.AddInParameter(dbCommand, "AllowsShipping", DbType.Boolean, allowsShipping);
            db.AddInParameter(dbCommand, "TwoLetterISOCode", DbType.String, twoLetterIsoCode);
            db.AddInParameter(dbCommand, "ThreeLetterISOCode", DbType.String, threeLetterIsoCode);
            db.AddInParameter(dbCommand, "NumericISOCode", DbType.Int32, numericIsoCode);
            db.AddInParameter(dbCommand, "Published", DbType.Boolean, published);
            db.AddInParameter(dbCommand, "DisplayOrder", DbType.Int32, displayOrder);
            if (db.ExecuteNonQuery(dbCommand) > 0)
            {
                int countryId = Convert.ToInt32(db.GetParameterValue(dbCommand, "@CountryID"));
                item = GetCountryById(countryId);
            }
            return item;
        }

        /// <summary>
        /// Updates the country
        /// </summary>
        /// <param name="countryId">The country identifier</param>
        /// <param name="name">The name</param>
        /// <param name="allowsRegistration">A value indicating whether registration is allowed to this country</param>
        /// <param name="allowsBilling">A value indicating whether billing is allowed to this country</param>
        /// <param name="allowsShipping">A value indicating whether shipping is allowed to this country</param>
        /// <param name="twoLetterIsoCode">The two letter ISO code</param>
        /// <param name="threeLetterIsoCode">The three letter ISO code</param>
        /// <param name="numericIsoCode">The numeric ISO code</param>
        /// <param name="published">A value indicating whether the entity is published</param>
        /// <param name="displayOrder">The display order</param>
        /// <returns>Country</returns>
        public override DBCountry UpdateCountry(int countryId, string name,
            bool allowsRegistration, bool allowsBilling, bool allowsShipping,
            string twoLetterIsoCode, string threeLetterIsoCode, int numericIsoCode,
            bool published, int displayOrder)
        {
            DBCountry item = null;
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_CountryUpdate");
            db.AddInParameter(dbCommand, "CountryID", DbType.Int32, countryId);
            db.AddInParameter(dbCommand, "Name", DbType.String, name);
            db.AddInParameter(dbCommand, "AllowsRegistration", DbType.Boolean, allowsRegistration);
            db.AddInParameter(dbCommand, "AllowsBilling", DbType.Boolean, allowsBilling);
            db.AddInParameter(dbCommand, "AllowsShipping", DbType.Boolean, allowsShipping);
            db.AddInParameter(dbCommand, "TwoLetterISOCode", DbType.String, twoLetterIsoCode);
            db.AddInParameter(dbCommand, "ThreeLetterISOCode", DbType.String, threeLetterIsoCode);
            db.AddInParameter(dbCommand, "NumericISOCode", DbType.Int32, numericIsoCode);
            db.AddInParameter(dbCommand, "Published", DbType.Boolean, published);
            db.AddInParameter(dbCommand, "DisplayOrder", DbType.Int32, displayOrder);
            if (db.ExecuteNonQuery(dbCommand) > 0)
                item = GetCountryById(countryId);
            return item;
        }
        #endregion
    }
}
