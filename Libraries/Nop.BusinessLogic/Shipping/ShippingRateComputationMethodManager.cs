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

namespace NopSolutions.NopCommerce.BusinessLogic.Shipping
{
    /// <summary>
    /// Shipping rate computation method manager
    /// </summary>
    public partial class ShippingRateComputationMethodManager
    {
        #region Constants
        private const string SHIPPINGRATECOMPUTATIONMETHODS_ALL_KEY = "Nop.shippingratecomputationmethod.all-{0}";
        private const string SHIPPINGRATECOMPUTATIONMETHODS_BY_ID_KEY = "Nop.shippingratecomputationmethod.id-{0}";
        private const string SHIPPINGRATECOMPUTATIONMETHODS_PATTERN_KEY = "Nop.shippingratecomputationmethod.";
        #endregion

        #region Utilities

        private static ShippingRateComputationMethodCollection DBMapping(DBShippingRateComputationMethodCollection dbCollection)
        {
            if (dbCollection == null)
                return null;

            var collection = new ShippingRateComputationMethodCollection();
            foreach (var dbItem in dbCollection)
            {
                var item = DBMapping(dbItem);
                collection.Add(item);
            }

            return collection;
        }

        private static ShippingRateComputationMethod DBMapping(DBShippingRateComputationMethod dbItem)
        {
            if (dbItem == null)
                return null;

            var item = new ShippingRateComputationMethod();
            item.ShippingRateComputationMethodId = dbItem.ShippingRateComputationMethodId;
            item.Name = dbItem.Name;
            item.Description = dbItem.Description;
            item.ConfigureTemplatePath = dbItem.ConfigureTemplatePath;
            item.ClassName = dbItem.ClassName;
            item.IsActive = dbItem.IsActive;
            item.DisplayOrder = dbItem.DisplayOrder;

            return item;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Deletes a shipping rate computation method
        /// </summary>
        /// <param name="shippingRateComputationMethodId">Shipping rate computation method identifier</param>
        public static void DeleteShippingRateComputationMethod(int shippingRateComputationMethodId)
        {
            DBProviderManager<DBShippingRateComputationMethodProvider>.Provider.DeleteShippingRateComputationMethod(shippingRateComputationMethodId);
            if (ShippingRateComputationMethodManager.CacheEnabled)
            {
                NopCache.RemoveByPattern(SHIPPINGRATECOMPUTATIONMETHODS_PATTERN_KEY);
            }
        }

        /// <summary>
        /// Gets a shipping rate computation method
        /// </summary>
        /// <param name="shippingRateComputationMethodId">Shipping rate computation method identifier</param>
        /// <returns>Shipping rate computation method</returns>
        public static ShippingRateComputationMethod GetShippingRateComputationMethodById(int shippingRateComputationMethodId)
        {
            if (shippingRateComputationMethodId == 0)
                return null;

            string key = string.Format(SHIPPINGRATECOMPUTATIONMETHODS_BY_ID_KEY, shippingRateComputationMethodId);
            object obj2 = NopCache.Get(key);
            if (ShippingRateComputationMethodManager.CacheEnabled && (obj2 != null))
            {
                return (ShippingRateComputationMethod)obj2;
            }

            var dbItem = DBProviderManager<DBShippingRateComputationMethodProvider>.Provider.GetShippingRateComputationMethodById(shippingRateComputationMethodId);
            var shippingRateComputationMethod = DBMapping(dbItem);

            if (ShippingRateComputationMethodManager.CacheEnabled)
            {
                NopCache.Max(key, shippingRateComputationMethod);
            }
            return shippingRateComputationMethod;
        }

        /// <summary>
        /// Gets all shipping rate computation methods
        /// </summary>
        /// <returns>Shipping rate computation method collection</returns>
        public static ShippingRateComputationMethodCollection GetAllShippingRateComputationMethods()
        {
            bool showHidden = NopContext.Current.IsAdmin;
            return GetAllShippingRateComputationMethods(showHidden);
        }

        /// <summary>
        /// Gets all shipping rate computation methods
        /// </summary>
        /// <param name="showHidden">A value indicating whether to show hidden records</param>
        /// <returns>Shipping rate computation method collection</returns>
        public static ShippingRateComputationMethodCollection GetAllShippingRateComputationMethods(bool showHidden)
        {
            string key = string.Format(SHIPPINGRATECOMPUTATIONMETHODS_ALL_KEY, showHidden);
            object obj2 = NopCache.Get(key);
            if (ShippingRateComputationMethodManager.CacheEnabled && (obj2 != null))
            {
                return (ShippingRateComputationMethodCollection)obj2;
            }

            var dbCollection = DBProviderManager<DBShippingRateComputationMethodProvider>.Provider.GetAllShippingRateComputationMethods(showHidden);
            var shippingRateComputationMethods = DBMapping(dbCollection);

            if (ShippingRateComputationMethodManager.CacheEnabled)
            {
                NopCache.Max(key, shippingRateComputationMethods);
            }
            return shippingRateComputationMethods;
        }

        /// <summary>
        /// Inserts a shipping rate computation method
        /// </summary>
        /// <param name="name">The name</param>
        /// <param name="description">The description</param>
        /// <param name="configureTemplatePath">The configure template path</param>
        /// <param name="className">The class name</param>
        /// <param name="isActive">The value indicating whether the method is active</param>
        /// <param name="displayOrder">The display order</param>
        /// <returns>Shipping rate computation method</returns>
        public static ShippingRateComputationMethod InsertShippingRateComputationMethod(string name,
            string description, string configureTemplatePath, string className,
            bool isActive, int displayOrder)
        {
            DBShippingRateComputationMethod dbItem = DBProviderManager<DBShippingRateComputationMethodProvider>.Provider.InsertShippingRateComputationMethod(name,
                description, configureTemplatePath, className, isActive, displayOrder);
            ShippingRateComputationMethod shippingRateComputationMethod = DBMapping(dbItem);

            if (ShippingRateComputationMethodManager.CacheEnabled)
            {
                NopCache.RemoveByPattern(SHIPPINGRATECOMPUTATIONMETHODS_PATTERN_KEY);
            }
            return shippingRateComputationMethod;
        }

        /// <summary>
        /// Updates the shipping rate computation method
        /// </summary>
        /// <param name="shippingRateComputationMethodId">The shipping rate computation method identifier</param>
        /// <param name="name">The name</param>
        /// <param name="description">The description</param>
        /// <param name="configureTemplatePath">The configure template path</param>
        /// <param name="className">The class name</param>
        /// <param name="isActive">The value indicating whether the method is active</param>
        /// <param name="displayOrder">The display order</param>
        /// <returns>Shipping rate computation method</returns>
        public static ShippingRateComputationMethod UpdateShippingRateComputationMethod(int shippingRateComputationMethodId,
            string name, string description, string configureTemplatePath, string className,
            bool isActive, int displayOrder)
        {
            var dbItem = DBProviderManager<DBShippingRateComputationMethodProvider>.Provider.UpdateShippingRateComputationMethod(shippingRateComputationMethodId,
                name, description, configureTemplatePath, className,
                isActive, displayOrder);
            var shippingRateComputationMethod = DBMapping(dbItem);

            if (ShippingRateComputationMethodManager.CacheEnabled)
            {
                NopCache.RemoveByPattern(SHIPPINGRATECOMPUTATIONMETHODS_PATTERN_KEY);
            }
            return shippingRateComputationMethod;
        }
        
        /// <summary>
        /// Gets a shipping rate computation method type
        /// </summary>
        /// <param name="shippingRateComputationMethodId">The shipping rate computation method identifier</param>
        /// <returns>A shipping rate computation method type</returns>
        public static ShippingRateComputationMethodTypeEnum GetShippingRateComputationMethodTypeEnum(int shippingRateComputationMethodId)
        {
            var method = GetShippingRateComputationMethodById(shippingRateComputationMethodId);
            if (method == null)
                return ShippingRateComputationMethodTypeEnum.Unknown;
            var iMethod = Activator.CreateInstance(Type.GetType(method.ClassName)) as IShippingRateComputationMethod;
            return iMethod.ShippingRateComputationMethodType;
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
                return SettingManager.GetSettingValueBoolean("Cache.ShippingRateComputationMethodManager.CacheEnabled");
            }
        }
        #endregion
    }
}