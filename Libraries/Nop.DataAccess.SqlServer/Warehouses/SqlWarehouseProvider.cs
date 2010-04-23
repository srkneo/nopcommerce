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

namespace NopSolutions.NopCommerce.DataAccess.Warehouses
{
    /// <summary>
    /// Warehouse provider for SQL Server
    /// </summary>
    public partial class SqlWarehouseProvider : DBWarehouseProvider
    {
        #region Fields
        private string _sqlConnectionString;
        #endregion

        #region Utilities
        private DBWarehouse GetWarehouseFromReader(IDataReader dataReader)
        {
            var item = new DBWarehouse();
            item.WarehouseId = NopSqlDataHelper.GetInt(dataReader, "WarehouseID");
            item.Name = NopSqlDataHelper.GetString(dataReader, "Name");
            item.PhoneNumber = NopSqlDataHelper.GetString(dataReader, "PhoneNumber");
            item.Email = NopSqlDataHelper.GetString(dataReader, "Email");
            item.FaxNumber = NopSqlDataHelper.GetString(dataReader, "FaxNumber");
            item.Address1 = NopSqlDataHelper.GetString(dataReader, "Address1");
            item.Address2 = NopSqlDataHelper.GetString(dataReader, "Address2");
            item.City = NopSqlDataHelper.GetString(dataReader, "City");
            item.StateProvince = NopSqlDataHelper.GetString(dataReader, "StateProvince");
            item.ZipPostalCode = NopSqlDataHelper.GetString(dataReader, "ZipPostalCode");
            item.CountryId = NopSqlDataHelper.GetInt(dataReader, "CountryID");
            item.Deleted = NopSqlDataHelper.GetBoolean(dataReader, "Deleted");
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
        /// Gets all warehouses
        /// </summary>
        /// <returns>Warehouse collection</returns>
        public override DBWarehouseCollection GetAllWarehouses()
        {
            var result = new DBWarehouseCollection();
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_WarehouseLoadAll");
            using (IDataReader dataReader = db.ExecuteReader(dbCommand))
            {
                while (dataReader.Read())
                {
                    var item = GetWarehouseFromReader(dataReader);
                    result.Add(item);
                }
            }

            return result;
        }

        /// <summary>
        /// Gets a warehouse
        /// </summary>
        /// <param name="warehouseId">The warehouse identifier</param>
        /// <returns>Warehouse</returns>
        public override DBWarehouse GetWarehouseById(int warehouseId)
        {
            DBWarehouse item = null;
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_WarehouseLoadByPrimaryKey");
            db.AddInParameter(dbCommand, "WarehouseID", DbType.Int32, warehouseId);
            using (IDataReader dataReader = db.ExecuteReader(dbCommand))
            {
                if (dataReader.Read())
                {
                    item = GetWarehouseFromReader(dataReader);
                }
            }
            return item;
        }

        /// <summary>
        /// Inserts a warehouse
        /// </summary>
        /// <param name="name">The name</param>
        /// <param name="phoneNumber">The phone number</param>
        /// <param name="email">The email</param>
        /// <param name="faxNumber">The fax number</param>
        /// <param name="address1">The address 1</param>
        /// <param name="address2">The address 2</param>
        /// <param name="city">The city</param>
        /// <param name="stateProvince">The state/province</param>
        /// <param name="zipPostalCode">The zip/postal code</param>
        /// <param name="countryId">The country identifier</param>
        /// <param name="deleted">A value indicating whether the entity has been deleted</param>
        /// <param name="createdOn">The date and time of instance creation</param>
        /// <param name="updatedOn">The date and time of instance update</param>
        /// <returns>Warehouse</returns>
        public override DBWarehouse InsertWarehouse(string name, string phoneNumber,
            string email, string faxNumber, string address1, string address2,
            string city, string stateProvince, string zipPostalCode, int countryId,
            bool deleted, DateTime createdOn, DateTime updatedOn)
        {
            DBWarehouse item = null;
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_WarehouseInsert");
            db.AddOutParameter(dbCommand, "WarehouseID", DbType.Int32, 0);
            db.AddInParameter(dbCommand, "Name", DbType.String, name);
            db.AddInParameter(dbCommand, "PhoneNumber", DbType.String, phoneNumber);
            db.AddInParameter(dbCommand, "Email", DbType.String, email);
            db.AddInParameter(dbCommand, "FaxNumber", DbType.String, faxNumber);
            db.AddInParameter(dbCommand, "Address1", DbType.String, address1);
            db.AddInParameter(dbCommand, "Address2", DbType.String, address2);
            db.AddInParameter(dbCommand, "City", DbType.String, city);
            db.AddInParameter(dbCommand, "StateProvince", DbType.String, stateProvince);
            db.AddInParameter(dbCommand, "ZipPostalCode", DbType.String, zipPostalCode);
            db.AddInParameter(dbCommand, "CountryID", DbType.Int32, countryId);
            db.AddInParameter(dbCommand, "Deleted", DbType.Boolean, deleted);
            db.AddInParameter(dbCommand, "CreatedOn", DbType.DateTime, createdOn);
            db.AddInParameter(dbCommand, "UpdatedOn", DbType.DateTime, updatedOn);
            if (db.ExecuteNonQuery(dbCommand) > 0)
            {
                int warehouseId = Convert.ToInt32(db.GetParameterValue(dbCommand, "@WarehouseID"));
                item = GetWarehouseById(warehouseId);
            }

            return item;
        }

        /// <summary>
        /// Updates the warehouse
        /// </summary>
        /// <param name="warehouseId">The warehouse identifier</param>
        /// <param name="name">The name</param>
        /// <param name="phoneNumber">The phone number</param>
        /// <param name="email">The email</param>
        /// <param name="faxNumber">The fax number</param>
        /// <param name="address1">The address 1</param>
        /// <param name="address2">The address 2</param>
        /// <param name="city">The city</param>
        /// <param name="stateProvince">The state/province</param>
        /// <param name="zipPostalCode">The zip/postal code</param>
        /// <param name="countryId">The country identifier</param>
        /// <param name="deleted">A value indicating whether the entity has been deleted</param>
        /// <param name="createdOn">The date and time of instance creation</param>
        /// <param name="updatedOn">The date and time of instance update</param>
        /// <returns>Warehouse</returns>
        public override DBWarehouse UpdateWarehouse(int warehouseId,
            string name, string phoneNumber, string email, string faxNumber,
            string address1, string address2, string city, string stateProvince,
            string zipPostalCode, int countryId, bool deleted,
            DateTime createdOn, DateTime updatedOn)
        {
            DBWarehouse item = null;
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_WarehouseUpdate");
            db.AddInParameter(dbCommand, "WarehouseID", DbType.Int32, warehouseId);
            db.AddInParameter(dbCommand, "Name", DbType.String, name);
            db.AddInParameter(dbCommand, "PhoneNumber", DbType.String, phoneNumber);
            db.AddInParameter(dbCommand, "Email", DbType.String, email);
            db.AddInParameter(dbCommand, "FaxNumber", DbType.String, faxNumber);
            db.AddInParameter(dbCommand, "Address1", DbType.String, address1);
            db.AddInParameter(dbCommand, "Address2", DbType.String, address2);
            db.AddInParameter(dbCommand, "City", DbType.String, city);
            db.AddInParameter(dbCommand, "StateProvince", DbType.String, stateProvince);
            db.AddInParameter(dbCommand, "ZipPostalCode", DbType.String, zipPostalCode);
            db.AddInParameter(dbCommand, "CountryID", DbType.Int32, countryId);
            db.AddInParameter(dbCommand, "Deleted", DbType.Boolean, deleted);
            db.AddInParameter(dbCommand, "CreatedOn", DbType.DateTime, createdOn);
            db.AddInParameter(dbCommand, "UpdatedOn", DbType.DateTime, updatedOn);
            if (db.ExecuteNonQuery(dbCommand) > 0)
                item = GetWarehouseById(warehouseId);

            return item;
        }
        #endregion
    }
}
