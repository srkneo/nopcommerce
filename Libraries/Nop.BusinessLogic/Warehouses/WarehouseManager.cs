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
using System.Linq;
using System.Text;
using NopSolutions.NopCommerce.BusinessLogic.Caching;
using NopSolutions.NopCommerce.BusinessLogic.Data;
using NopSolutions.NopCommerce.BusinessLogic.Profile;

namespace NopSolutions.NopCommerce.BusinessLogic.Warehouses
{
    /// <summary>
    /// Warehouse manager
    /// </summary>
    public partial class WarehouseManager
    {
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
        public static List<Warehouse> GetAllWarehouses()
        {
            var context = ObjectContextHelper.CurrentObjectContext;
            var query = from w in context.Warehouses
                        orderby w.Name
                        where !w.Deleted
                        select w;
            var warehouses = query.ToList();

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

            var context = ObjectContextHelper.CurrentObjectContext;
            var query = from w in context.Warehouses
                        where w.WarehouseId == warehouseId
                        select w;
            var warehouse = query.SingleOrDefault();
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

            var warehouse = new Warehouse();
            warehouse.Name = name;
            warehouse.PhoneNumber = phoneNumber;
            warehouse.Email = email;
            warehouse.FaxNumber = faxNumber;
            warehouse.Address1 = address1;
            warehouse.Address2 = address2;
            warehouse.City = city;
            warehouse.StateProvince = stateProvince;
            warehouse.ZipPostalCode = zipPostalCode;
            warehouse.CountryId = countryId;
            warehouse.Deleted = deleted;
            warehouse.CreatedOn = createdOn;
            warehouse.UpdatedOn = updatedOn;

            var context = ObjectContextHelper.CurrentObjectContext;
            context.Warehouses.AddObject(warehouse);
            context.SaveChanges();

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

            var warehouse = GetWarehouseById(warehouseId);

            var context = ObjectContextHelper.CurrentObjectContext;
            context.Warehouses.Attach(warehouse);

            warehouse.Name = name;
            warehouse.PhoneNumber = phoneNumber;
            warehouse.Email = email;
            warehouse.FaxNumber = faxNumber;
            warehouse.Address1 = address1;
            warehouse.Address2 = address2;
            warehouse.City = city;
            warehouse.StateProvince = stateProvince;
            warehouse.ZipPostalCode = zipPostalCode;
            warehouse.CountryId = countryId;
            warehouse.Deleted = deleted;
            warehouse.CreatedOn = createdOn;
            warehouse.UpdatedOn = updatedOn;
            context.SaveChanges();
            return warehouse;
        }
        #endregion
    }
}
