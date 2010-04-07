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
using NopSolutions.NopCommerce.DataAccess.Localization;
using System.Xml;

namespace NopSolutions.NopCommerce.BusinessLogic.Localization
{
    /// <summary>
    /// Locale string resource manager
    /// </summary>
    public partial class LocaleStringResourceManager
    {
        #region Constants
        private const string LOCALSTRINGRESOURCES_ALL_KEY = "Nop.localestringresource.all-{0}";
        private const string LOCALSTRINGRESOURCES_PATTERN_KEY = "Nop.localestringresource.";
        #endregion

        #region Utilities
        private static LocaleStringResourceDictionary DBMapping(DBLocaleStringResourceCollection dbCollection)
        {
            if (dbCollection == null)
                return null;

            var dictionary = new LocaleStringResourceDictionary();
            foreach (var dbItem in dbCollection)
            {
                var item = DBMapping(dbItem);
                dictionary.Add(item.ResourceName.ToLowerInvariant(), item);
            }

            return dictionary;
        }

        private static LocaleStringResource DBMapping(DBLocaleStringResource dbItem)
        {
            if (dbItem == null)
                return null;

            var item = new LocaleStringResource();
            item.LocaleStringResourceID = dbItem.LocaleStringResourceID;
            item.LanguageID = dbItem.LanguageID;
            item.ResourceName = dbItem.ResourceName;
            item.ResourceValue = dbItem.ResourceValue;

            return item;
        }
        #endregion

        #region Methods
        /// <summary>
        /// Deletes a locale string resource
        /// </summary>
        /// <param name="LocaleStringResourceID">Locale string resource identifier</param>
        public static void DeleteLocaleStringResource(int LocaleStringResourceID)
        {
            DBProviderManager<DBLocaleStringResourceProvider>.Provider.DeleteLocaleStringResource(LocaleStringResourceID);
            if (LocaleStringResourceManager.CacheEnabled)
            {
                NopCache.RemoveByPattern(LOCALSTRINGRESOURCES_PATTERN_KEY);
            }
        }

        /// <summary>
        /// Gets a locale string resource
        /// </summary>
        /// <param name="LocaleStringResourceID">Locale string resource identifier</param>
        /// <returns>Locale string resource</returns>
        public static LocaleStringResource GetLocaleStringResourceByID(int LocaleStringResourceID)
        {
            if (LocaleStringResourceID == 0)
                return null;

            var dbItem = DBProviderManager<DBLocaleStringResourceProvider>.Provider.GetLocaleStringResourceByID(LocaleStringResourceID);
            var localeStringResource = DBMapping(dbItem);
            return localeStringResource;
        }

        /// <summary>
        /// Gets all locale string resources by language identifier
        /// </summary>
        /// <param name="LanguageID">Language identifier</param>
        /// <returns>Locale string resource collection</returns>
        public static LocaleStringResourceDictionary GetAllResourcesByLanguageID(int LanguageID)
        {
            string key = string.Format(LOCALSTRINGRESOURCES_ALL_KEY, LanguageID);
            object obj2 = NopCache.Get(key);
            if (LocaleStringResourceManager.CacheEnabled && (obj2 != null))
            {
                return (LocaleStringResourceDictionary)obj2;
            }

            var dbCollection = DBProviderManager<DBLocaleStringResourceProvider>.Provider.GetAllResourcesByLanguageID(LanguageID);
            var localeStringResourceDictionary = DBMapping(dbCollection);

            if (LocaleStringResourceManager.CacheEnabled)
            {
                NopCache.Max(key, localeStringResourceDictionary);
            }
            return localeStringResourceDictionary;
        }

        /// <summary>
        /// Inserts a locale string resource
        /// </summary>
        /// <param name="LanguageID">The language identifier</param>
        /// <param name="ResourceName">The resource name</param>
        /// <param name="ResourceValue">The resource value</param>
        /// <returns>Locale string resource</returns>
        public static LocaleStringResource InsertLocaleStringResource(int LanguageID, string ResourceName, string ResourceValue)
        {
            var dbItem = DBProviderManager<DBLocaleStringResourceProvider>.Provider.InsertLocaleStringResource(LanguageID, ResourceName, ResourceValue);
            var localeStringResource = DBMapping(dbItem);
            if (LocaleStringResourceManager.CacheEnabled)
            {
                NopCache.RemoveByPattern(LOCALSTRINGRESOURCES_PATTERN_KEY);
            }
            return localeStringResource;
        }

        /// <summary>
        /// Gets all locale string resources as XML
        /// </summary>
        /// <param name="LanguageID">The Language identifier</param>
        /// <returns>XML</returns>
        public static string GetAllLocaleStringResourcesAsXML(int LanguageID)
        {
            return DBProviderManager<DBLocaleStringResourceProvider>.Provider.GetAllLocaleStringResourcesAsXML(LanguageID);
        }

        /// <summary>
        /// Inserts all locale string resources from XML
        /// </summary>
        /// <param name="LanguageID">The Language identifier</param>
        /// <param name="xml">The XML package</param>
        public static void InsertAllLocaleStringResourcesFromXML(int LanguageID, string xml)
        {
            DBProviderManager<DBLocaleStringResourceProvider>.Provider.InsertAllLocaleStringResourcesFromXML(LanguageID, xml);
            if (LocaleStringResourceManager.CacheEnabled)
            {
                NopCache.RemoveByPattern(LOCALSTRINGRESOURCES_PATTERN_KEY);
            }
        }


        /// <summary>
        /// Updates the locale string resource
        /// </summary>
        /// <param name="LocaleStringResourceID">The locale string resource identifier</param>
        /// <param name="LanguageID">The language identifier</param>
        /// <param name="ResourceName">The resource name</param>
        /// <param name="ResourceValue">The resource value</param>
        /// <returns>Locale string resource</returns>
        public static LocaleStringResource UpdateLocaleStringResource(int LocaleStringResourceID, int LanguageID, string ResourceName, string ResourceValue)
        {
            var dbItem = DBProviderManager<DBLocaleStringResourceProvider>.Provider.UpdateLocaleStringResource(LocaleStringResourceID, LanguageID, ResourceName, ResourceValue);
            var localeStringResource = DBMapping(dbItem);
            if (LocaleStringResourceManager.CacheEnabled)
            {
                NopCache.RemoveByPattern(LOCALSTRINGRESOURCES_PATTERN_KEY);
            }
            return localeStringResource;
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
                return SettingManager.GetSettingValueBoolean("Cache.LocaleStringResourceManager.CacheEnabled");
            }
        }
        #endregion
    }
}
