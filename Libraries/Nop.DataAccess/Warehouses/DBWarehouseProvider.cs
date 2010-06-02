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
using System.Configuration;
using System.Configuration.Provider;
using System.Data;
using System.Data.Common;
using System.Globalization;
using System.Text;
using System.Web.Configuration;
using System.Web.Hosting;

namespace NopSolutions.NopCommerce.DataAccess.Warehouses
{
    /// <summary>
    /// Acts as a base class for deriving custom warehouse provider
    /// </summary>
    [DBProviderSectionName("nopDataProviders/WarehouseProvider")]
    public abstract partial class DBWarehouseProvider : BaseDBProvider
    {
        #region Methods

        /// <summary>
        /// Gets all warehouses
        /// </summary>
        /// <returns>Warehouse collection</returns>
        public abstract DBWarehouseCollection GetAllWarehouses();

        /// <summary>
        /// Gets a warehouse
        /// </summary>
        /// <param name="warehouseId">The warehouse identifier</param>
        /// <returns>Warehouse</returns>
        public abstract DBWarehouse GetWarehouseById(int warehouseId);

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
        public abstract DBWarehouse InsertWarehouse(string name, string phoneNumber,
            string email, string faxNumber, string address1, string address2,
            string city, string stateProvince, string zipPostalCode, int countryId, 
            bool deleted, DateTime createdOn, DateTime updatedOn);

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
        public abstract DBWarehouse UpdateWarehouse(int warehouseId, 
            string name, string phoneNumber, string email, string faxNumber, 
            string address1, string address2, string city, string stateProvince,
            string zipPostalCode, int countryId, bool deleted, 
            DateTime createdOn, DateTime updatedOn);

        #endregion
    }
}
