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
using NopSolutions.NopCommerce.BusinessLogic.Configuration.Settings;
using NopSolutions.NopCommerce.DataAccess;
using NopSolutions.NopCommerce.DataAccess.Shipping;
using NopSolutions.NopCommerce.BusinessLogic.Directory;
using NopSolutions.NopCommerce.BusinessLogic.Data;

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
        private static List<ShippingMethod> DBMapping(DBShippingMethodCollection dbCollection)
        {
            if (dbCollection == null)
                return null;

            var collection = new List<ShippingMethod>();
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
            item.ShippingMethodId = dbItem.ShippingMethodId;
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
        /// <param name="shippingMethodId">The shipping method identifier</param>
        public static void DeleteShippingMethod(int shippingMethodId)
        {
            var shippingMethod = GetShippingMethodById(shippingMethodId);
            if (shippingMethod == null)
                return;

            var context = ObjectContextHelper.CurrentObjectContext;
            if (!context.IsAttached(shippingMethod))
                context.ShippingMethods.Attach(shippingMethod);
            context.DeleteObject(shippingMethod);
            context.SaveChanges();

            if (ShippingMethodManager.CacheEnabled)
            {
                NopCache.RemoveByPattern(SHIPPINGMETHODS_PATTERN_KEY);
            }
        }

        /// <summary>
        /// Gets a shipping method
        /// </summary>
        /// <param name="shippingMethodId">The shipping method identifier</param>
        /// <returns>Shipping method</returns>
        public static ShippingMethod GetShippingMethodById(int shippingMethodId)
        {
            if (shippingMethodId == 0)
                return null;

            string key = string.Format(SHIPPINGMETHODS_BY_ID_KEY, shippingMethodId);
            object obj2 = NopCache.Get(key);
            if (ShippingMethodManager.CacheEnabled && (obj2 != null))
            {
                return (ShippingMethod)obj2;
            }

            var context = ObjectContextHelper.CurrentObjectContext;
            var query = from sm in context.ShippingMethods
                        where sm.ShippingMethodId == shippingMethodId
                        select sm;
            var shippingMethod = query.SingleOrDefault();

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
        public static List<ShippingMethod> GetAllShippingMethods()
        {
            return GetAllShippingMethods(null);
        }

        /// <summary>
        /// Gets all shipping methods
        /// </summary>
        /// <param name="filterByCountryId">The country indentifier</param>
        /// <returns>Shipping method collection</returns>
        public static List<ShippingMethod> GetAllShippingMethods(int? filterByCountryId)
        {
            var dbCollection = DBProviderManager<DBShippingMethodProvider>.Provider.GetAllShippingMethods(filterByCountryId);
            var shippingMethods = DBMapping(dbCollection);
            return shippingMethods;
        }

        /// <summary>
        /// Inserts a shipping method
        /// </summary>
        /// <param name="name">The name</param>
        /// <param name="description">The description</param>
        /// <param name="displayOrder">The display order</param>
        /// <returns>Shipping method</returns>
        public static ShippingMethod InsertShippingMethod(string name,
            string description, int displayOrder)
        {
            var shippingMethod = new ShippingMethod();
            shippingMethod.Name = name;
            shippingMethod.Description = description;
            shippingMethod.DisplayOrder = displayOrder;

            var context = ObjectContextHelper.CurrentObjectContext;
            context.ShippingMethods.AddObject(shippingMethod);
            context.SaveChanges();

            if (ShippingMethodManager.CacheEnabled)
            {
                NopCache.RemoveByPattern(SHIPPINGMETHODS_PATTERN_KEY);
            }
            return shippingMethod;
        }

        /// <summary>
        /// Updates the shipping method
        /// </summary>
        /// <param name="shippingMethodId">The shipping method identifier</param>
        /// <param name="name">The name</param>
        /// <param name="description">The description</param>
        /// <param name="displayOrder">The display order</param>
        /// <returns>Shipping method</returns>
        public static ShippingMethod UpdateShippingMethod(int shippingMethodId,
            string name, string description, int displayOrder)
        {
            var shippingMethod = GetShippingMethodById(shippingMethodId);
            if (shippingMethod == null)
                return null;

            var context = ObjectContextHelper.CurrentObjectContext;
            if (!context.IsAttached(shippingMethod))
                context.ShippingMethods.Attach(shippingMethod);

            shippingMethod.Name = name;
            shippingMethod.Description = description;
            shippingMethod.DisplayOrder = displayOrder;
            context.SaveChanges();

            if (ShippingMethodManager.CacheEnabled)
            {
                NopCache.RemoveByPattern(SHIPPINGMETHODS_PATTERN_KEY);
            }

            return shippingMethod;
        }

        /// <summary>
        /// Creates the shipping method country mapping
        /// </summary>
        /// <param name="shippingMethodId">The shipping method identifier</param>
        /// <param name="countryId">The country identifier</param>
        public static void CreateShippingMethodCountryMapping(int shippingMethodId, int countryId)
        {
            DBProviderManager<DBShippingMethodProvider>.Provider.InsertShippingMethodCountryMapping(shippingMethodId, countryId);
        }

        /// <summary>
        /// Checking whether the shipping method country mapping exists
        /// </summary>
        /// <param name="shippingMethodId">The shipping method identifier</param>
        /// <param name="countryId">The country identifier</param>
        /// <returns>True if mapping exist, otherwise false</returns>
        public static bool DoesShippingMethodCountryMappingExist(int shippingMethodId, int countryId)
        {
            return DBProviderManager<DBShippingMethodProvider>.Provider.DoesShippingMethodCountryMappingExist(shippingMethodId, countryId);
        }

        /// <summary>
        /// Deletes the shipping method country mapping
        /// </summary>
        /// <param name="shippingMethodId">The shipping method identifier</param>
        /// <param name="countryId">The country identifier</param>
        public static void DeleteShippingMethodCountryMapping(int shippingMethodId, int countryId)
        {
            DBProviderManager<DBShippingMethodProvider>.Provider.DeleteShippingMethodCountryMapping(shippingMethodId, countryId);
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
