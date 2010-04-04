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
using NopSolutions.NopCommerce.BusinessLogic.Configuration.Settings;
using NopSolutions.NopCommerce.DataAccess;
using NopSolutions.NopCommerce.DataAccess.Shipping;
using NopSolutions.NopCommerce.BusinessLogic.Directory;

namespace NopSolutions.NopCommerce.BusinessLogic.Shipping
{
    /// <summary>
    /// Shipping method manager
    /// </summary>
    public partial class ShippingMethodManager
    {
        #region Constants
        private const string SHIPPINGMETHODS_BY_ID_KEY = "Nop.shippingMethod.id-{0}";
        private const string SHIPPINGMETHODS_PATTERN_KEY = "Nop.shippingMethod.";
        #endregion

        #region Utilities
        private static ShippingMethodCollection DBMapping(DBShippingMethodCollection dbCollection)
        {
            if (dbCollection == null)
                return null;

            var collection = new ShippingMethodCollection();
            foreach (var dbItem in dbCollection)
            {
                var item = DBMapping(dbItem);
                collection.Add(item);
            }

            return collection;
        }

        private static ShippingMethod DBMapping(DBShippingMethod dbItem)
        {
            if (dbItem == null)
                return null;

            var item = new ShippingMethod();
            item.ShippingMethodID = dbItem.ShippingMethodID;
            item.Name = dbItem.Name;
            item.Description = dbItem.Description;
            item.DisplayOrder = dbItem.DisplayOrder;

            return item;
        }
        #endregion

        #region Methods
        /// <summary>
        /// Deletes a shipping method
        /// </summary>
        /// <param name="ShippingMethodID">The shipping method identifier</param>
        public static void DeleteShippingMethod(int ShippingMethodID)
        {
            DBProviderManager<DBShippingMethodProvider>.Provider.DeleteShippingMethod(ShippingMethodID);
            if (ShippingMethodManager.CacheEnabled)
            {
                NopCache.RemoveByPattern(SHIPPINGMETHODS_PATTERN_KEY);
            }
        }

        /// <summary>
        /// Gets a shipping method
        /// </summary>
        /// <param name="ShippingMethodID">The shipping method identifier</param>
        /// <returns>Shipping method</returns>
        public static ShippingMethod GetShippingMethodByID(int ShippingMethodID)
        {
            if (ShippingMethodID == 0)
                return null;

            string key = string.Format(SHIPPINGMETHODS_BY_ID_KEY, ShippingMethodID);
            object obj2 = NopCache.Get(key);
            if (ShippingMethodManager.CacheEnabled && (obj2 != null))
            {
                return (ShippingMethod)obj2;
            }

            var dbItem = DBProviderManager<DBShippingMethodProvider>.Provider.GetShippingMethodByID(ShippingMethodID);
            var shippingMethod = DBMapping(dbItem);

            if (ShippingMethodManager.CacheEnabled)
            {
                NopCache.Max(key, shippingMethod);
            }
            return shippingMethod;
        }

        /// <summary>
        /// Gets all shipping methods
        /// </summary>
        /// <returns>Shipping method collection</returns>
        public static ShippingMethodCollection GetAllShippingMethods()
        {
            return GetAllShippingMethods(null);
        }

        /// <summary>
        /// Gets all shipping methods
        /// </summary>
        /// <param name="FilterByCountryID">The country indentifier</param>
        /// <returns>Shipping method collection</returns>
        public static ShippingMethodCollection GetAllShippingMethods(int? FilterByCountryID)
        {
            var dbCollection = DBProviderManager<DBShippingMethodProvider>.Provider.GetAllShippingMethods(FilterByCountryID);
            var shippingMethods = DBMapping(dbCollection);
            return shippingMethods;
        }

        /// <summary>
        /// Inserts a shipping method
        /// </summary>
        /// <param name="Name">The name</param>
        /// <param name="Description">The description</param>
        /// <param name="DisplayOrder">The display order</param>
        /// <returns>Shipping method</returns>
        public static ShippingMethod InsertShippingMethod(string Name, string Description, int DisplayOrder)
        {
            var dbItem = DBProviderManager<DBShippingMethodProvider>.Provider.InsertShippingMethod(Name, 
                Description, DisplayOrder);
            var shippingMethod = DBMapping(dbItem);

            if (ShippingMethodManager.CacheEnabled)
            {
                NopCache.RemoveByPattern(SHIPPINGMETHODS_PATTERN_KEY);
            }
            return shippingMethod;
        }

        /// <summary>
        /// Updates the shipping method
        /// </summary>
        /// <param name="ShippingMethodID">The shipping method identifier</param>
        /// <param name="Name">The name</param>
        /// <param name="Description">The description</param>
        /// <param name="DisplayOrder">The display order</param>
        /// <returns>Shipping method</returns>
        public static ShippingMethod UpdateShippingMethod(int ShippingMethodID, string Name,
            string Description, int DisplayOrder)
        {
            var dbItem = DBProviderManager<DBShippingMethodProvider>.Provider.UpdateShippingMethod(ShippingMethodID, 
                Name, Description, DisplayOrder);
            var shippingMethod = DBMapping(dbItem);

            if (ShippingMethodManager.CacheEnabled)
            {
                NopCache.RemoveByPattern(SHIPPINGMETHODS_PATTERN_KEY);
            }

            return shippingMethod;
        }

        /// <summary>
        /// Creates the shipping method country mapping
        /// </summary>
        /// <param name="shippingMethodID">The shipping method identifier</param>
        /// <param name="countryID">The country identifier</param>
        public static void CreateShippingMethodCountryMapping(int shippingMethodID, int countryID)
        {
            DBProviderManager<DBShippingMethodProvider>.Provider.InsertShippingMethodCountryMapping(shippingMethodID, countryID);
        }

        /// <summary>
        /// Checking whether the shipping method country mapping is exists
        /// </summary>
        /// <param name="shippingMethodID">The shipping method identifier</param>
        /// <param name="countryID">The country identifier</param>
        /// <returns>True if mapping exist, otherwise false</returns>
        public static bool IsShippingMethodCountryMappingExists(int shippingMethodID, int countryID)
        {
            return DBProviderManager<DBShippingMethodProvider>.Provider.IsShippingMethodCountryMappingExists(shippingMethodID, countryID);
        }

        /// <summary>
        /// Deletes the shipping method country mapping
        /// </summary>
        /// <param name="shippingMethodID">The shipping method identifier</param>
        /// <param name="countryID">The country identifier</param>
        public static void DeleteShippingMethodCountryMapping(int shippingMethodID, int countryID)
        {
            DBProviderManager<DBShippingMethodProvider>.Provider.DeleteShippingMethodCountryMapping(shippingMethodID, countryID);
        }
        #endregion

        #region Property
        /// <summary>
        /// Gets a value indicating whether cache is enabled
        /// </summary>
        public static bool CacheEnabled
        {
            get
            {
                return SettingManager.GetSettingValueBoolean("Cache.ShippingMethodManager.CacheEnabled");
            }
        }
        #endregion
    }
}
