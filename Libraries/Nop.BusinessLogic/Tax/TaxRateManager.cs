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
using NopSolutions.NopCommerce.DataAccess.Tax;
using NopSolutions.NopCommerce.DataAccess;
using NopSolutions.NopCommerce.BusinessLogic.Configuration.Settings;

namespace NopSolutions.NopCommerce.BusinessLogic.Tax
{
    /// <summary>
    /// Tax rate manager
    /// </summary>
    public partial class TaxRateManager
    {
        #region Constants
        private const string TAXRATE_ALL_KEY = "Nop.taxrate.all";
        private const string TAXRATE_BY_ID_KEY = "Nop.taxrate.id-{0}";
        private const string TAXRATE_PATTERN_KEY = "Nop.taxrate.";
        #endregion

        #region Utilities
        private static TaxRateCollection DBMapping(DBTaxRateCollection dbCollection)
        {
            if (dbCollection == null)
                return null;

            var collection = new TaxRateCollection();
            foreach (var dbItem in dbCollection)
            {
                var item = DBMapping(dbItem);
                collection.Add(item);
            }

            return collection;
        }

        private static TaxRate DBMapping(DBTaxRate dbItem)
        {
            if (dbItem == null)
                return null;

            var item = new TaxRate();
            item.TaxRateId = dbItem.TaxRateId;
            item.TaxCategoryId = dbItem.TaxCategoryId;
            item.CountryId = dbItem.CountryId;
            item.StateProvinceId = dbItem.StateProvinceId;
            item.Zip = dbItem.Zip;
            item.Percentage = dbItem.Percentage;

            return item;
        }
        #endregion

        #region Methods
        /// <summary>
        /// Deletes a tax rate
        /// </summary>
        /// <param name="taxRateId">Tax rate identifier</param>
        public static void DeleteTaxRate(int taxRateId)
        {
            DBProviderManager<DBTaxRateProvider>.Provider.DeleteTaxRate(taxRateId);
            if (TaxRateManager.CacheEnabled)
            {
                NopCache.RemoveByPattern(TAXRATE_PATTERN_KEY);
            }
        }

        /// <summary>
        /// Gets a tax rate
        /// </summary>
        /// <param name="taxRateId">Tax rate identifier</param>
        /// <returns>Tax rate</returns>
        public static TaxRate GetTaxRateById(int taxRateId)
        {
            if (taxRateId == 0)
                return null;

            string key = string.Format(TAXRATE_BY_ID_KEY, taxRateId);
            object obj2 = NopCache.Get(key);
            if (TaxRateManager.CacheEnabled && (obj2 != null))
            {
                return (TaxRate)obj2;
            }

            var dbItem = DBProviderManager<DBTaxRateProvider>.Provider.GetTaxRateById(taxRateId);
            var taxRate = DBMapping(dbItem);

            if (TaxRateManager.CacheEnabled)
            {
                NopCache.Max(key, taxRate);
            }
            return taxRate;
        }

        /// <summary>
        /// Gets all tax rates
        /// </summary>
        /// <returns>Tax rate collection</returns>
        public static TaxRateCollection GetAllTaxRates()
        {
            string key = TAXRATE_ALL_KEY;
            object obj2 = NopCache.Get(key);
            if (TaxRateManager.CacheEnabled && (obj2 != null))
            {
                return (TaxRateCollection)obj2;
            }

            var dbCollection = DBProviderManager<DBTaxRateProvider>.Provider.GetAllTaxRates();
            var collection = DBMapping(dbCollection);

            if (TaxRateManager.CacheEnabled)
            {
                NopCache.Max(key, collection);
            } 
            
            return collection;
        }

        /// <summary>
        /// Gets all tax rates by params
        /// </summary>
        /// <param name="taxCategoryId">The tax category identifier</param>
        /// <param name="countryId">The country identifier</param>
        /// <param name="stateProvinceId">The state/province identifier</param>
        /// <param name="zip">The zip</param>
        /// <returns>Tax rate collection</returns>
        public static TaxRateCollection GetAllTaxRates(int taxCategoryId, int countryId,
            int stateProvinceId, string zip)
        {
            if (zip == null)
                zip = string.Empty;
            if (!String.IsNullOrEmpty(zip))
                zip = zip.Trim();

            var existingRates = GetAllTaxRates().FindTaxRates(countryId, taxCategoryId);

            //filter by state/province
            var matchedByStateProvince = new TaxRateCollection();
            foreach (var taxRate in existingRates)
            {
                if (stateProvinceId == taxRate.StateProvinceId)
                    matchedByStateProvince.Add(taxRate);
            }
            if (matchedByStateProvince.Count == 0)
            {
                foreach (var taxRate in existingRates)
                {
                    if (taxRate.StateProvinceId == 0)
                        matchedByStateProvince.Add(taxRate);
                }
            }

            //filter by zip
            var matchedByZip = new TaxRateCollection();
            foreach (var taxRate in matchedByStateProvince)
            {
                if (zip.ToLower() == taxRate.Zip.ToLower())
                    matchedByZip.Add(taxRate);
            }
            if (matchedByZip.Count == 0)
            {
                foreach (var taxRate in matchedByStateProvince)
                {
                    if (taxRate.Zip.Trim() == string.Empty)
                        matchedByZip.Add(taxRate);
                }
            }

            return matchedByZip;
        }

        /// <summary>
        /// Inserts a tax rate
        /// </summary>
        /// <param name="taxCategoryId">The tax category identifier</param>
        /// <param name="countryId">The country identifier</param>
        /// <param name="stateProvinceId">The state/province identifier</param>
        /// <param name="zip">The zip</param>
        /// <param name="percentage">The percentage</param>
        /// <returns>Tax rate</returns>
        public static TaxRate InsertTaxRate(int taxCategoryId, int countryId,
            int stateProvinceId, string zip, decimal percentage)
        {
            if (zip == null)
                zip = string.Empty;
            if (!String.IsNullOrEmpty(zip))
                zip = zip.Trim();

            var dbItem = DBProviderManager<DBTaxRateProvider>.Provider.InsertTaxRate(taxCategoryId, countryId, stateProvinceId, zip, percentage);
            var taxRate = DBMapping(dbItem);

            if (TaxRateManager.CacheEnabled)
            {
                NopCache.RemoveByPattern(TAXRATE_PATTERN_KEY);
            }

            return taxRate;
        }

        /// <summary>
        /// Updates the tax rate
        /// </summary>
        /// <param name="taxRateId">The tax rate identifier</param>
        /// <param name="taxCategoryId">The tax category identifier</param>
        /// <param name="countryId">The country identifier</param>
        /// <param name="stateProvinceId">The state/province identifier</param>
        /// <param name="zip">The zip</param>
        /// <param name="percentage">The percentage</param>
        /// <returns>Tax rate</returns>
        public static TaxRate UpdateTaxRate(int taxRateId,
            int taxCategoryId, int countryId, int stateProvinceId,
            string zip, decimal percentage)
        {
            if (zip == null)
                zip = string.Empty;
            if (!String.IsNullOrEmpty(zip))
                zip = zip.Trim();

            var dbItem = DBProviderManager<DBTaxRateProvider>.Provider.UpdateTaxRate(taxRateId, 
                taxCategoryId, countryId, stateProvinceId, zip, percentage);
            var taxRate = DBMapping(dbItem);

            if (TaxRateManager.CacheEnabled)
            {
                NopCache.RemoveByPattern(TAXRATE_PATTERN_KEY);
            }

            return taxRate;
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
                return SettingManager.GetSettingValueBoolean("Cache.TaxRateManager.CacheEnabled");
            }
        }
        #endregion
    }
}
