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
using NopSolutions.NopCommerce.DataAccess.Directory;

namespace NopSolutions.NopCommerce.BusinessLogic.Directory
{
    /// <summary>
    /// State province manager
    /// </summary>
    public partial class StateProvinceManager
    {
        #region Constants
        private const string STATEPROVINCES_ALL_KEY = "Nop.stateprovince.all-{0}";
        private const string STATEPROVINCES_BY_ID_KEY = "Nop.stateprovince.id-{0}";
        private const string STATEPROVINCES_PATTERN_KEY = "Nop.stateprovince.";
        #endregion

        #region Utilities
        private static StateProvinceCollection DBMapping(DBStateProvinceCollection dbCollection)
        {
            if (dbCollection == null)
                return null;

            var collection = new StateProvinceCollection();
            foreach (var dbItem in dbCollection)
            {
                var item = DBMapping(dbItem);
                collection.Add(item);
            }

            return collection;
        }

        private static StateProvince DBMapping(DBStateProvince dbItem)
        {
            if (dbItem == null)
                return null;

            var item = new StateProvince();
            item.StateProvinceId = dbItem.StateProvinceId;
            item.CountryId = dbItem.CountryId;
            item.Name = dbItem.Name;
            item.Abbreviation = dbItem.Abbreviation;
            item.DisplayOrder = dbItem.DisplayOrder;

            return item;
        }
        #endregion

        #region Methods
        /// <summary>
        /// Deletes a state/province
        /// </summary>
        /// <param name="stateProvinceId">The state/province identifier</param>
        public static void DeleteStateProvince(int stateProvinceId)
        {
            DBProviderManager<DBStateProvinceProvider>.Provider.DeleteStateProvince(stateProvinceId);
            if (StateProvinceManager.CacheEnabled)
            {
                NopCache.RemoveByPattern(STATEPROVINCES_PATTERN_KEY);
            }
        }

        /// <summary>
        /// Gets a state/province
        /// </summary>
        /// <param name="stateProvinceId">The state/province identifier</param>
        /// <returns>State/province</returns>
        public static StateProvince GetStateProvinceById(int stateProvinceId)
        {
            if (stateProvinceId == 0)
                return null;

            string key = string.Format(STATEPROVINCES_BY_ID_KEY, stateProvinceId);
            object obj2 = NopCache.Get(key);
            if (StateProvinceManager.CacheEnabled && (obj2 != null))
            {
                return (StateProvince)obj2;
            }

            var dbItem = DBProviderManager<DBStateProvinceProvider>.Provider.GetStateProvinceById(stateProvinceId);
            var stateProvince = DBMapping(dbItem);

            if (StateProvinceManager.CacheEnabled)
            {
                NopCache.Max(key, stateProvince);
            }
            return stateProvince;
        }

        /// <summary>
        /// Gets a state/province 
        /// </summary>
        /// <param name="abbreviation">The state/province abbreviation</param>
        /// <returns>State/province</returns>
        public static StateProvince GetStateProvinceByAbbreviation(string abbreviation)
        {
            var dbItem = DBProviderManager<DBStateProvinceProvider>.Provider.GetStateProvinceByAbbreviation(abbreviation);
            var stateProvince = DBMapping(dbItem);
            return stateProvince;
        }
        
        /// <summary>
        /// Gets a state/province collection by country identifier
        /// </summary>
        /// <param name="countryId">Country identifier</param>
        /// <returns>State/province collection</returns>
        public static StateProvinceCollection GetStateProvincesByCountryId(int countryId)
        {
            string key = string.Format(STATEPROVINCES_ALL_KEY, countryId);
            object obj2 = NopCache.Get(key);
            if (StateProvinceManager.CacheEnabled && (obj2 != null))
            {
                return (StateProvinceCollection)obj2;
            }

            var dbCollection = DBProviderManager<DBStateProvinceProvider>.Provider.GetStateProvincesByCountryId(countryId);
            var stateProvinceCollection = DBMapping(dbCollection);

            if (StateProvinceManager.CacheEnabled)
            {
                NopCache.Max(key, stateProvinceCollection);
            }
            return stateProvinceCollection;
        }

        /// <summary>
        /// Inserts a state/province
        /// </summary>
        /// <param name="countryId">The country identifier</param>
        /// <param name="name">The name</param>
        /// <param name="abbreviation">The abbreviation</param>
        /// <param name="displayOrder">The display order</param>
        /// <returns>State/province</returns>
        public static StateProvince InsertStateProvince(int countryId,
            string name, string abbreviation, int displayOrder)
        {
            var dbItem = DBProviderManager<DBStateProvinceProvider>.Provider.InsertStateProvince(countryId, 
                name, abbreviation, displayOrder);
            var stateProvince = DBMapping(dbItem);

            if (StateProvinceManager.CacheEnabled)
            {
                NopCache.RemoveByPattern(STATEPROVINCES_PATTERN_KEY);
            }

            return stateProvince;
        }

        /// <summary>
        /// Updates a state/province
        /// </summary>
        /// <param name="stateProvinceId">The state/province identifier</param>
        /// <param name="countryId">The country identifier</param>
        /// <param name="name">The name</param>
        /// <param name="abbreviation">The abbreviation</param>
        /// <param name="displayOrder">The display order</param>
        /// <returns>State/province</returns>
        public static StateProvince UpdateStateProvince(int stateProvinceId,
            int countryId, string name, string abbreviation, int displayOrder)
        {
            var dbItem = DBProviderManager<DBStateProvinceProvider>.Provider.UpdateStateProvince(stateProvinceId, 
                countryId, name, abbreviation, displayOrder);
            var stateProvince = DBMapping(dbItem);

            if (StateProvinceManager.CacheEnabled)
            {
                NopCache.RemoveByPattern(STATEPROVINCES_PATTERN_KEY);
            }

            return stateProvince;
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
                return SettingManager.GetSettingValueBoolean("Cache.StateProvinceManager.CacheEnabled");
            }
        }
        #endregion
    }
}
