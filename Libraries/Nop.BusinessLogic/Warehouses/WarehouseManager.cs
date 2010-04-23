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
using NopSolutions.NopCommerce.BusinessLogic.Caching;
using NopSolutions.NopCommerce.BusinessLogic.Profile;
using NopSolutions.NopCommerce.DataAccess;
using NopSolutions.NopCommerce.DataAccess.Warehouses;

namespace NopSolutions.NopCommerce.BusinessLogic.Warehouses
{
    /// <summary>
    /// Warehouse manager
    /// </summary>
    public partial class WarehouseManager
    {
        #region Utilities
        private static WarehouseCollection DBMapping(DBWarehouseCollection dbCollection)
        {
            if (dbCollection == null)
                return null;

            var collection = new WarehouseCollection();
            foreach (var dbItem in dbCollection)
            {
                var item = DBMapping(dbItem);
                collection.Add(item);
            }

            return collection;
        }

        private static Warehouse DBMapping(DBWarehouse dbItem)
        {
            if (dbItem == null)
                return null;

            var item = new Warehouse();
            item.WarehouseId = dbItem.WarehouseId;
            item.Name = dbItem.Name;
            item.PhoneNumber = dbItem.PhoneNumber;
            item.Email = dbItem.Email;
            item.FaxNumber = dbItem.FaxNumber;
            item.Address1 = dbItem.Address1;
            item.Address2 = dbItem.Address2;
            item.City = dbItem.City;
            item.StateProvince = dbItem.StateProvince;
            item.ZipPostalCode = dbItem.ZipPostalCode;
            item.CountryId = dbItem.CountryId;
            item.Deleted = dbItem.Deleted;
            item.CreatedOn = dbItem.CreatedOn;
            item.UpdatedOn = dbItem.UpdatedOn;

            return item;
        }
        #endregion

        #region Methods
        /// <summary>
        /// Marks a warehouse as deleted
        /// </summary>
        /// <param name="warehouseId">The warehouse identifier</param>
        public static void MarkWarehouseAsDeleted(int warehouseId)
        {
            var warehouse = GetWarehouseById(warehouseId);
            if (warehouse != null)
            {
                UpdateWarehouse(warehouse.WarehouseId, warehouse.Name, warehouse.PhoneNumber,
                    warehouse.Email, warehouse.FaxNumber, warehouse.Address1, warehouse.Address2, warehouse.City,
                    warehouse.StateProvince, warehouse.ZipPostalCode, warehouse.CountryId, true, warehouse.CreatedOn, warehouse.UpdatedOn);
            }
        }

        /// <summary>
        /// Gets all warehouses
        /// </summary>
        /// <returns>Warehouse collection</returns>
        public static WarehouseCollection GetAllWarehouses()
        {
            var dbCollection = DBProviderManager<DBWarehouseProvider>.Provider.GetAllWarehouses();
            var warehouses = DBMapping(dbCollection);
            return warehouses;
        }

        /// <summary>
        /// Gets a warehouse
        /// </summary>
        /// <param name="warehouseId">The warehouse identifier</param>
        /// <returns>Warehouse</returns>
        public static Warehouse GetWarehouseById(int warehouseId)
        {
            if (warehouseId == 0)
                return null;

            var dbItem = DBProviderManager<DBWarehouseProvider>.Provider.GetWarehouseById(warehouseId);
            var warehouse = DBMapping(dbItem);
            return warehouse;
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
        public static Warehouse InsertWarehouse(string name, string phoneNumber,
            string email, string faxNumber, string address1, string address2,
            string city, string stateProvince, string zipPostalCode, int countryId,
            bool deleted, DateTime createdOn, DateTime updatedOn)
        {
            createdOn = DateTimeHelper.ConvertToUtcTime(createdOn);
            updatedOn = DateTimeHelper.ConvertToUtcTime(updatedOn);

            var dbItem = DBProviderManager<DBWarehouseProvider>.Provider.InsertWarehouse(name, 
                phoneNumber, email, faxNumber, address1, address2, city, stateProvince,
                zipPostalCode, countryId, deleted, createdOn, updatedOn);
            var warehouse = DBMapping(dbItem);
            return warehouse;
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
        public static Warehouse UpdateWarehouse(int warehouseId,
            string name, string phoneNumber, string email, string faxNumber,
            string address1, string address2, string city, string stateProvince,
            string zipPostalCode, int countryId, bool deleted,
            DateTime createdOn, DateTime updatedOn)
        {
            createdOn = DateTimeHelper.ConvertToUtcTime(createdOn);
            updatedOn = DateTimeHelper.ConvertToUtcTime(updatedOn);

            var dbItem = DBProviderManager<DBWarehouseProvider>.Provider.UpdateWarehouse(warehouseId, 
                name, phoneNumber, email, faxNumber, address1, address2, city, stateProvince,
                zipPostalCode, countryId, deleted, createdOn, updatedOn);
            var warehouse = DBMapping(dbItem);
            return warehouse;
        }
        #endregion
    }
}
