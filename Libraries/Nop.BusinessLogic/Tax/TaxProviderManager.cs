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
using NopSolutions.NopCommerce.DataAccess.Tax;

namespace NopSolutions.NopCommerce.BusinessLogic.Tax
{
    /// <summary>
    /// Tax provider manager
    /// </summary>
    public partial class TaxProviderManager
    {
        #region Constants
        private const string TAXPROVIDERS_ALL_KEY = "Nop.taxprovider.all";
        private const string TAXPROVIDERS_BY_ID_KEY = "Nop.taxprovider.id-{0}";
        private const string TAXPROVIDERS_PATTERN_KEY = "Nop.taxprovider.";
        #endregion

        #region Utilities
        private static TaxProviderCollection DBMapping(DBTaxProviderCollection dbCollection)
        {
            if (dbCollection == null)
                return null;

            var collection = new TaxProviderCollection();
            foreach (var dbItem in dbCollection)
            {
                var item = DBMapping(dbItem);
                collection.Add(item);
            }

            return collection;
        }

        private static TaxProvider DBMapping(DBTaxProvider dbItem)
        {
            if (dbItem == null)
                return null;

            var item = new TaxProvider();
            item.TaxProviderId = dbItem.TaxProviderId;
            item.Name = dbItem.Name;
            item.Description = dbItem.Description;
            item.ConfigureTemplatePath = dbItem.ConfigureTemplatePath;
            item.ClassName = dbItem.ClassName;
            item.DisplayOrder = dbItem.DisplayOrder;

            return item;
        }
        #endregion

        #region Methods
        /// <summary>
        /// Deletes a tax provider
        /// </summary>
        /// <param name="taxProviderId">Tax provider identifier</param>
        public static void DeleteTaxProvider(int taxProviderId)
        {
            DBProviderManager<DBTaxProviderProvider>.Provider.DeleteTaxProvider(taxProviderId);
            if (TaxProviderManager.CacheEnabled)
            {
                NopCache.RemoveByPattern(TAXPROVIDERS_PATTERN_KEY);
            }
        }

        /// <summary>
        /// Gets a tax provider
        /// </summary>
        /// <param name="taxProviderId">Tax provider identifier</param>
        /// <returns>Tax provider</returns>
        public static TaxProvider GetTaxProviderById(int taxProviderId)
        {
            if (taxProviderId == 0)
                return null;

            string key = string.Format(TAXPROVIDERS_BY_ID_KEY, taxProviderId);
            object obj2 = NopCache.Get(key);
            if (TaxProviderManager.CacheEnabled && (obj2 != null))
            {
                return (TaxProvider)obj2;
            }

            var dbItem = DBProviderManager<DBTaxProviderProvider>.Provider.GetTaxProviderById(taxProviderId);
            var taxProvider = DBMapping(dbItem);

            if (TaxProviderManager.CacheEnabled)
            {
                NopCache.Max(key, taxProvider);
            }
            return taxProvider;
        }

        /// <summary>
        /// Gets all tax providers
        /// </summary>
        /// <returns>Shipping rate computation method collection</returns>
        public static TaxProviderCollection GetAllTaxProviders()
        {
            string key = string.Format(TAXPROVIDERS_ALL_KEY);
            object obj2 = NopCache.Get(key);
            if (TaxProviderManager.CacheEnabled && (obj2 != null))
            {
                return (TaxProviderCollection)obj2;
            }

            var dbCollection = DBProviderManager<DBTaxProviderProvider>.Provider.GetAllTaxProviders();
            var taxProviderCollection = DBMapping(dbCollection);

            if (TaxProviderManager.CacheEnabled)
            {
                NopCache.Max(key, taxProviderCollection);
            }
            return taxProviderCollection;
        }

        /// <summary>
        /// Inserts a tax provider
        /// </summary>
        /// <param name="name">The name</param>
        /// <param name="description">The description</param>
        /// <param name="configureTemplatePath">The configure template path</param>
        /// <param name="className">The class name</param>
        /// <param name="displayOrder">The display order</param>
        /// <returns>Tax provider</returns>
        public static TaxProvider InsertTaxProvider(string name, string description,
           string configureTemplatePath, string className, int displayOrder)
        {
            var dbItem = DBProviderManager<DBTaxProviderProvider>.Provider.InsertTaxProvider(name, 
                description, configureTemplatePath, className, displayOrder);
            var taxProvider = DBMapping(dbItem);

            if (TaxProviderManager.CacheEnabled)
            {
                NopCache.RemoveByPattern(TAXPROVIDERS_PATTERN_KEY);
            }
            return taxProvider;
        }

        /// <summary>
        /// Updates the tax provider
        /// </summary>
        /// <param name="taxProviderId">The tax provider identifier</param>
        /// <param name="name">The name</param>
        /// <param name="description">The description</param>
        /// <param name="configureTemplatePath">The configure template path</param>
        /// <param name="className">The class name</param>
        /// <param name="displayOrder">The display order</param>
        /// <returns>Tax provider</returns>
        public static TaxProvider UpdateTaxProvider(int taxProviderId,
            string name, string description, string configureTemplatePath,
            string className, int displayOrder)
        {
            var dbItem = DBProviderManager<DBTaxProviderProvider>.Provider.UpdateTaxProvider(taxProviderId, name, 
                description, configureTemplatePath, className, displayOrder);
            var taxProvider = DBMapping(dbItem);

            if (TaxProviderManager.CacheEnabled)
            {
                NopCache.RemoveByPattern(TAXPROVIDERS_PATTERN_KEY);
            }
            return taxProvider;
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
                return SettingManager.GetSettingValueBoolean("Cache.TaxProviderManager.CacheEnabled");
            }
        }
        #endregion
    }
}
