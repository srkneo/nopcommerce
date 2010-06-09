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

namespace NopSolutions.NopCommerce.DataAccess.Shipping
{
    /// <summary>
    /// Shipping method provider for SQL Server
    /// </summary>
    public partial class SqlShippingMethodProvider : DBShippingMethodProvider
    {
        #region Fields
        private string _sqlConnectionString;
        #endregion

        #region Utilities
        private DBShippingMethod GetShippingMethodFromReader(IDataReader dataReader)
        {
            var item = new DBShippingMethod();
            item.ShippingMethodId = NopSqlDataHelper.GetInt(dataReader, "ShippingMethodID");
            item.Name = NopSqlDataHelper.GetString(dataReader, "Name");
            item.Description = NopSqlDataHelper.GetString(dataReader, "Description");
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
        /// Gets all shipping methods
        /// </summary>
        /// <param name="filterByCountryId">The country indentifier</param>
        /// <returns>Shipping method collection</returns>
        public override DBShippingMethodCollection GetAllShippingMethods(int? filterByCountryId)
        {
            var result = new DBShippingMethodCollection();
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_ShippingMethodLoadAll");
            if(filterByCountryId.HasValue)
            {
                db.AddInParameter(dbCommand, "FilterByCountryID", DbType.Int32, filterByCountryId.Value);
            }
            using (IDataReader dataReader = db.ExecuteReader(dbCommand))
            {
                while (dataReader.Read())
                {
                    var item = GetShippingMethodFromReader(dataReader);
                    result.Add(item);
                }
            }
            return result;
        }

        /// <summary>
        /// Inserts shipping method country mapping
        /// </summary>
        /// <param name="shippingMethodId">The shipping method identifier</param>
        /// <param name="countryId">The country identifier</param>
        public override void InsertShippingMethodCountryMapping(int shippingMethodId, int countryId)
        {
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_ShippingMethod_RestrictedCountriesInsert");
            db.AddInParameter(dbCommand, "ShippingMethodID", DbType.Int32, shippingMethodId);
            db.AddInParameter(dbCommand, "CountryID", DbType.Int32, countryId);
            db.ExecuteNonQuery(dbCommand);
        }

        /// <summary>
        /// Checking whether the shipping method country mapping exists
        /// </summary>
        /// <param name="shippingMethodId">The shipping method identifier</param>
        /// <param name="countryId">The country identifier</param>
        /// <returns>True if mapping exist, otherwise false</returns>
        public override bool DoesShippingMethodCountryMappingExist(int shippingMethodId, int countryId)
        {
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_ShippingMethod_RestrictedCountriesContains");
            db.AddInParameter(dbCommand, "ShippingMethodID", DbType.Int32, shippingMethodId);
            db.AddInParameter(dbCommand, "CountryID", DbType.Int32, countryId);
            db.AddOutParameter(dbCommand, "Result", DbType.Boolean, 0);
            db.ExecuteNonQuery(dbCommand);            
            return Convert.ToBoolean(db.GetParameterValue(dbCommand, "@Result"));
        }

        /// <summary>
        /// Deletes shipping method country mapping
        /// </summary>
        /// <param name="shippingMethodId">The shipping method identifier</param>
        /// <param name="countryId">The country identifier</param>
        public override void DeleteShippingMethodCountryMapping(int shippingMethodId, int countryId)
        {
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_ShippingMethod_RestrictedCountriesDelete");
            db.AddInParameter(dbCommand, "ShippingMethodID", DbType.Int32, shippingMethodId);
            db.AddInParameter(dbCommand, "CountryID", DbType.Int32, countryId);
            db.ExecuteNonQuery(dbCommand);
        }
        #endregion
    }
}
