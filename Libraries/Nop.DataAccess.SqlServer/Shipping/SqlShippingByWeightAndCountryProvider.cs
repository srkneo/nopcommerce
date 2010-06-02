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
    /// ShippingByWeightAndCountry provider for SQL Server
    /// </summary>
    public partial class SqlShippingByWeightAndCountryProvider : DBShippingByWeightAndCountryProvider
    {
        #region Fields
        private string _sqlConnectionString;
        #endregion

        #region Utilities
        private DBShippingByWeightAndCountry GetShippingByWeightAndCountryFromReader(IDataReader dataReader)
        {
            var item = new DBShippingByWeightAndCountry();
            item.ShippingByWeightAndCountryId = NopSqlDataHelper.GetInt(dataReader, "ShippingByWeightAndCountryID");
            item.ShippingMethodId = NopSqlDataHelper.GetInt(dataReader, "ShippingMethodID");
            item.CountryId = NopSqlDataHelper.GetInt(dataReader, "CountryID");
            item.From = NopSqlDataHelper.GetDecimal(dataReader, "From");
            item.To = NopSqlDataHelper.GetDecimal(dataReader, "To");
            item.UsePercentage = NopSqlDataHelper.GetBoolean(dataReader, "UsePercentage");
            item.ShippingChargePercentage = NopSqlDataHelper.GetDecimal(dataReader, "ShippingChargePercentage");
            item.ShippingChargeAmount = NopSqlDataHelper.GetDecimal(dataReader, "ShippingChargeAmount");
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
        /// Gets a ShippingByWeightAndCountry
        /// </summary>
        /// <param name="shippingByWeightAndCountryId">ShippingByWeightAndCountry identifier</param>
        /// <returns>ShippingByWeightAndCountry</returns>
        public override DBShippingByWeightAndCountry GetById(int shippingByWeightAndCountryId)
        {
            DBShippingByWeightAndCountry item = null;
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_ShippingByWeightAndCountryLoadByPrimaryKey");
            db.AddInParameter(dbCommand, "ShippingByWeightAndCountryID", DbType.Int32, shippingByWeightAndCountryId);
            using (IDataReader dataReader = db.ExecuteReader(dbCommand))
            {
                if (dataReader.Read())
                {
                    item = GetShippingByWeightAndCountryFromReader(dataReader);
                }
            }
            return item;
        }

        /// <summary>
        /// Deletes a ShippingByWeightAndCountry
        /// </summary>
        /// <param name="shippingByWeightAndCountryId">ShippingByWeightAndCountry identifier</param>
        public override void DeleteShippingByWeightAndCountry(int shippingByWeightAndCountryId)
        {
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_ShippingByWeightAndCountryDelete");
            db.AddInParameter(dbCommand, "ShippingByWeightAndCountryID", DbType.Int32, shippingByWeightAndCountryId);
            db.ExecuteNonQuery(dbCommand);
        }

        /// <summary>
        /// Gets all ShippingByWeightAndCountrys
        /// </summary>
        /// <returns>ShippingByWeightAndCountry collection</returns>
        public override DBShippingByWeightAndCountryCollection GetAll()
        {
            var result = new DBShippingByWeightAndCountryCollection();
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_ShippingByWeightAndCountryLoadAll");
            using (IDataReader dataReader = db.ExecuteReader(dbCommand))
            {
                while (dataReader.Read())
                {
                    var item = GetShippingByWeightAndCountryFromReader(dataReader);
                    result.Add(item);
                }
            }

            return result;
        }

        /// <summary>
        /// Inserts a ShippingByWeightAndCountry
        /// </summary>
        /// <param name="shippingMethodId">The shipping method identifier</param>
        /// <param name="countryId">The country identifier</param>
        /// <param name="from">The "from" value</param>
        /// <param name="to">The "to" value</param>
        /// <param name="usePercentage">A value indicating whether to use percentage</param>
        /// <param name="shippingChargePercentage">The shipping charge percentage</param>
        /// <param name="shippingChargeAmount">The shipping charge amount</param>
        /// <returns>ShippingByWeightAndCountry</returns>
        public override DBShippingByWeightAndCountry InsertShippingByWeightAndCountry(int shippingMethodId,
            int countryId, decimal from, decimal to, bool usePercentage,
            decimal shippingChargePercentage, decimal shippingChargeAmount)
        {
            DBShippingByWeightAndCountry item = null;
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_ShippingByWeightAndCountryInsert");
            db.AddOutParameter(dbCommand, "ShippingByWeightAndCountryID", DbType.Int32, 0);
            db.AddInParameter(dbCommand, "ShippingMethodID", DbType.Int32, shippingMethodId);
            db.AddInParameter(dbCommand, "CountryID", DbType.Int32, countryId);
            db.AddInParameter(dbCommand, "From", DbType.Decimal, from);
            db.AddInParameter(dbCommand, "To", DbType.Decimal, to);
            db.AddInParameter(dbCommand, "UsePercentage", DbType.Boolean, usePercentage);
            db.AddInParameter(dbCommand, "ShippingChargePercentage", DbType.Decimal, shippingChargePercentage);
            db.AddInParameter(dbCommand, "ShippingChargeAmount", DbType.Decimal, shippingChargeAmount);
            if (db.ExecuteNonQuery(dbCommand) > 0)
            {
                int shippingByWeightAndCountryId = Convert.ToInt32(db.GetParameterValue(dbCommand, "@ShippingByWeightAndCountryID"));
                item = GetById(shippingByWeightAndCountryId);
            }
            return item;
        }

        /// <summary>
        /// Updates the ShippingByWeightAndCountry
        /// </summary>
        /// <param name="shippingByWeightAndCountryId">The ShippingByWeightAndCountry identifier</param>
        /// <param name="shippingMethodId">The shipping method identifier</param>
        /// <param name="countryId">The country identifier</param>
        /// <param name="from">The "from" value</param>
        /// <param name="to">The "to" value</param>
        /// <param name="usePercentage">A value indicating whether to use percentage</param>
        /// <param name="shippingChargePercentage">The shipping charge percentage</param>
        /// <param name="shippingChargeAmount">The shipping charge amount</param>
        /// <returns>ShippingByWeightAndCountry</returns>
        public override DBShippingByWeightAndCountry UpdateShippingByWeightAndCountry(int shippingByWeightAndCountryId,
            int shippingMethodId, int countryId, decimal from, decimal to, bool usePercentage,
            decimal shippingChargePercentage, decimal shippingChargeAmount)
        {
            DBShippingByWeightAndCountry item = null;
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_ShippingByWeightAndCountryUpdate");
            db.AddInParameter(dbCommand, "ShippingByWeightAndCountryID", DbType.Int32, shippingByWeightAndCountryId);
            db.AddInParameter(dbCommand, "ShippingMethodID", DbType.Int32, shippingMethodId);
            db.AddInParameter(dbCommand, "CountryID", DbType.Int32, countryId);
            db.AddInParameter(dbCommand, "From", DbType.Decimal, from);
            db.AddInParameter(dbCommand, "To", DbType.Decimal, to);
            db.AddInParameter(dbCommand, "UsePercentage", DbType.Boolean, usePercentage);
            db.AddInParameter(dbCommand, "ShippingChargePercentage", DbType.Decimal, shippingChargePercentage);
            db.AddInParameter(dbCommand, "ShippingChargeAmount", DbType.Decimal, shippingChargeAmount);
            if (db.ExecuteNonQuery(dbCommand) > 0)
                item = GetById(shippingByWeightAndCountryId);

            return item;
        }

        /// <summary>
        /// Gets all ShippingByWeightAndCountrys by shipping method identifier
        /// </summary>
        /// <param name="shippingMethodId">The shipping method identifier</param>
        /// <param name="countryId">The country identifier</param>
        /// <returns>ShippingByWeightAndCountry collection</returns>
        public override DBShippingByWeightAndCountryCollection GetAllByShippingMethodIdAndCountryId(int shippingMethodId, int countryId)
        {
            var result = new DBShippingByWeightAndCountryCollection();
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_ShippingByWeightAndCountryLoadByShippingMethodIDAndCountryID");
            db.AddInParameter(dbCommand, "ShippingMethodID", DbType.Int32, shippingMethodId);
            db.AddInParameter(dbCommand, "CountryID", DbType.Int32, countryId);
            using (IDataReader dataReader = db.ExecuteReader(dbCommand))
            {
                while (dataReader.Read())
                {
                    var item = GetShippingByWeightAndCountryFromReader(dataReader);
                    result.Add(item);
                }
            }

            return result;
        }
        #endregion
    }
}
