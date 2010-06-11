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
using NopSolutions.NopCommerce.BusinessLogic.Data;
using NopSolutions.NopCommerce.DataAccess;
using NopSolutions.NopCommerce.DataAccess.Tax;

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
        private static List<TaxRate> DBMapping(DBTaxRateCollection dbCollection)
        {
            if (dbCollection == null)
                return null;

            var collection = new List<TaxRate>();
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
            var taxRate = GetTaxRateById(taxRateId);

            var context = ObjectContextHelper.CurrentObjectContext;
            if (!context.IsAttached(taxRate))
                context.TaxRates.Attach(taxRate);
            context.DeleteObject(taxRate);
            context.SaveChanges();
            
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

            var context = ObjectContextHelper.CurrentObjectContext;
            var query = from tr in context.TaxRates
                        where tr.TaxRateId == taxRateId
                        select tr;
            var taxRate = query.SingleOrDefault();

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
        public static List<TaxRate> GetAllTaxRates()
        {
            string key = TAXRATE_ALL_KEY;
            object obj2 = NopCache.Get(key);
            if (TaxRateManager.CacheEnabled && (obj2 != null))
            {
                return (List<TaxRate>)obj2;
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
        public static List<TaxRate> GetAllTaxRates(int taxCategoryId, int countryId,
            int stateProvinceId, string zip)
        {
            if (zip == null)
                zip = string.Empty;
            if (!String.IsNullOrEmpty(zip))
                zip = zip.Trim();

            var existingRates = GetAllTaxRates().FindTaxRates(countryId, taxCategoryId);

            //filter by state/province
            var matchedByStateProvince = new List<TaxRate>();
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
            var matchedByZip = new List<TaxRate>();
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

            var taxRate = new TaxRate();
            taxRate.TaxCategoryId = taxCategoryId;
            taxRate.CountryId = countryId;
            taxRate.StateProvinceId = stateProvinceId;
            taxRate.Zip = zip;
            taxRate.Percentage = percentage;

            var context = ObjectContextHelper.CurrentObjectContext;
            context.TaxRates.AddObject(taxRate);
            context.SaveChanges();
            
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

            var taxRate = GetTaxRateById(taxRateId);

            var context = ObjectContextHelper.CurrentObjectContext;
            if (!context.IsAttached(taxRate))
                context.TaxRates.Attach(taxRate);

            taxRate.TaxCategoryId = taxCategoryId;
            taxRate.CountryId = countryId;
            taxRate.StateProvinceId = stateProvinceId;
            taxRate.Zip = zip;
            taxRate.Percentage = percentage;
            context.SaveChanges();
            
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
