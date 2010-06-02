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

using NopSolutions.NopCommerce.BusinessLogic.Caching;
using NopSolutions.NopCommerce.BusinessLogic.Configuration.Settings;
using NopSolutions.NopCommerce.DataAccess;
using NopSolutions.NopCommerce.DataAccess.Products.Specs;
using System.Data;
using System.Xml;

namespace NopSolutions.NopCommerce.BusinessLogic.Products.Specs
{
    /// <summary>
    /// Specification attribute manager
    /// </summary>
    public partial class SpecificationAttributeManager
    {
        #region Constants
        private const string SPECIFICATIONATTRIBUTE_BY_ID_KEY = "Nop.specificationattributes.id-{0}-{1}";
        private const string SPECIFICATIONATTRIBUTEOPTION_BY_ID_KEY = "Nop.specificationattributeoptions.id-{0}-{1}";
        private const string PRODUCTSPECIFICATIONATTRIBUTE_ALLBYPRODUCTID_KEY = "Nop.productspecificationattribute.allbyproductid-{0}-{1}-{2}";
        private const string SPECIFICATIONATTRIBUTE_PATTERN_KEY = "Nop.specificationattributes.";
        private const string SPECIFICATIONATTRIBUTEOPTION_PATTERN_KEY = "Nop.specificationattributeoptions.";
        private const string PRODUCTSPECIFICATIONATTRIBUTE_PATTERN_KEY = "Nop.productspecificationattribute.";
        #endregion

        #region Utilities

        /// <summary>
        /// Maps a DBSpecificationAttributeCollection to a SpecificationAttributeCollection
        /// </summary>
        /// <param name="dbCollection">DBSpecificationAttributeCollection</param>
        /// <returns>SpecificationAttributeCollection</returns>
        private static SpecificationAttributeCollection DBMapping(DBSpecificationAttributeCollection dbCollection)
        {
            if (dbCollection == null)
                return null;

            var collection = new SpecificationAttributeCollection();
            foreach (var dbItem in dbCollection)
            {
                var item = DBMapping(dbItem);
                collection.Add(item);
            }

            return collection;
        }

        /// <summary>
        /// Maps a DBSpecificationAttribute to a SpecificationAttribute
        /// </summary>
        /// <param name="dbItem">DBSpecificationAttribute</param>
        /// <returns>SpecificationAttribute</returns>
        private static SpecificationAttribute DBMapping(DBSpecificationAttribute dbItem)
        {
            if (dbItem == null)
                return null;

            var item = new SpecificationAttribute();
            item.SpecificationAttributeId = dbItem.SpecificationAttributeId;
            item.Name = dbItem.Name;
            item.DisplayOrder = dbItem.DisplayOrder;

            return item;
        }

        private static SpecificationAttributeLocalized DBMapping(DBSpecificationAttributeLocalized dbItem)
        {
            if (dbItem == null)
                return null;

            var item = new SpecificationAttributeLocalized();
            item.SpecificationAttributeLocalizedId = dbItem.SpecificationAttributeLocalizedId;
            item.SpecificationAttributeId = dbItem.SpecificationAttributeId;
            item.LanguageId = dbItem.LanguageId;
            item.Name = dbItem.Name;

            return item;
        }

        /// <summary>
        /// Maps a DBSpecificationAttributeOptionCollection to a SpecificationAttributeOptionCollections
        /// </summary>
        /// <param name="dbCollection">DBSpecificationAttributeOptionCollection</param>
        /// <returns>SpecificationAttributeOptionCollection</returns>
        private static SpecificationAttributeOptionCollection DBMapping(DBSpecificationAttributeOptionCollection dbCollection)
        {
            if (dbCollection == null)
                return null;

            var collection = new SpecificationAttributeOptionCollection();
            foreach (var dbItem in dbCollection)
            {
                var item = DBMapping(dbItem);
                collection.Add(item);
            }

            return collection;
        }

        /// <summary>
        /// Maps a DBSpecificationAttributeOption to a SpecificationAttributeOption
        /// </summary>
        /// <param name="dbItem">DBSpecificationAttributeOption</param>
        /// <returns>SpecificationAttributeOption</returns>
        private static SpecificationAttributeOption DBMapping(DBSpecificationAttributeOption dbItem)
        {
            if (dbItem == null)
                return null;

            var item = new SpecificationAttributeOption();
            item.SpecificationAttributeOptionId = dbItem.SpecificationAttributeOptionId;
            item.SpecificationAttributeId = dbItem.SpecificationAttributeId;
            item.Name = dbItem.Name;
            item.DisplayOrder = dbItem.DisplayOrder;

            return item;
        }

        private static SpecificationAttributeOptionLocalized DBMapping(DBSpecificationAttributeOptionLocalized dbItem)
        {
            if (dbItem == null)
                return null;

            var item = new SpecificationAttributeOptionLocalized();
            item.SpecificationAttributeOptionLocalizedId = dbItem.SpecificationAttributeOptionLocalizedId;
            item.SpecificationAttributeOptionId = dbItem.SpecificationAttributeOptionId;
            item.LanguageId = dbItem.LanguageId;
            item.Name = dbItem.Name;

            return item;
        }

        /// <summary>
        /// Maps a DBProductSpecificationAttributeCollection to a ProductSpecificationAttributeCollection
        /// </summary>
        /// <param name="dbCollection">DBProductSpecificationAttributeCollection</param>
        /// <returns>ProductSpecificationAttributeCollection</returns>
        private static ProductSpecificationAttributeCollection DBMapping(DBProductSpecificationAttributeCollection dbCollection)
        {
            if (dbCollection == null)
                return null;

            var collection = new ProductSpecificationAttributeCollection();
            foreach (var dbItem in dbCollection)
            {
                var item = DBMapping(dbItem);
                collection.Add(item);
            }

            return collection;
        }

        /// <summary>
        /// Maps a DBProductSpecificationAttribute to a ProductSpecificationAttribute
        /// </summary>
        /// <param name="dbItem">DBProductSpecificationAttribute</param>
        /// <returns>ProductSpecificationAttribute</returns>
        private static ProductSpecificationAttribute DBMapping(DBProductSpecificationAttribute dbItem)
        {
            if (dbItem == null)
                return null;

            var item = new ProductSpecificationAttribute();
            item.ProductSpecificationAttributeId = dbItem.ProductSpecificationAttributeId;
            item.ProductId = dbItem.ProductId;
            item.SpecificationAttributeOptionId = dbItem.SpecificationAttributeOptionId;
            item.AllowFiltering = dbItem.AllowFiltering;
            item.ShowOnProductPage = dbItem.ShowOnProductPage;
            item.DisplayOrder = dbItem.DisplayOrder;

            return item;
        }

        /// <summary>
        /// Maps a DBSpecificationAttributeOptionFilter to a SpecificationAttributeOptionFilter
        /// </summary>
        /// <param name="dbItem">DBSpecificationAttributeOptionFilter</param>
        /// <returns>SpecificationAttributeOptionFilter</returns>
        private static SpecificationAttributeOptionFilter DBMapping(DBSpecificationAttributeOptionFilter dbItem)
        {
            if (dbItem == null)
                return null;

            var item = new SpecificationAttributeOptionFilter();
            item.SpecificationAttributeId = dbItem.SpecificationAttributeId;
            item.SpecificationAttributeName = dbItem.SpecificationAttributeName;
            item.DisplayOrder = dbItem.DisplayOrder;
            item.SpecificationAttributeOptionId = dbItem.SpecificationAttributeOptionId;
            item.SpecificationAttributeOptionName = dbItem.SpecificationAttributeOptionName;
            return item;
        }

        /// <summary>
        /// Maps a DBSpecificationAttributeOptionFilterCollection to a SpecificationAttributeOptionFilterCollection
        /// </summary>
        /// <param name="dbCol">DBSpecificationAttributeOptionFilterCollection</param>
        /// <returns>SpecificationAttributeOptionFilterCollection</returns>
        private static SpecificationAttributeOptionFilterCollection DBMapping(DBSpecificationAttributeOptionFilterCollection dbCol)
        {
            if (dbCol == null)
                return null;

            var col = new SpecificationAttributeOptionFilterCollection();
            foreach (var dbItem in dbCol)
            {
                var item = DBMapping(dbItem);
                col.Add(item);
            }
            return col;
        }
        #endregion

        #region Methods

        #region Specification attribute

        /// <summary>
        /// Gets a specification attribute
        /// </summary>
        /// <param name="specificationAttributeId">The specification attribute identifier</param>
        /// <returns>Specification attribute</returns>
        public static SpecificationAttribute GetSpecificationAttributeById(int specificationAttributeId)
        {
            int languageId = 0;
            if (NopContext.Current != null)
                languageId = NopContext.Current.WorkingLanguage.LanguageId;
            return GetSpecificationAttributeById(specificationAttributeId, languageId);
        }

        /// <summary>
        /// Gets a specification attribute
        /// </summary>
        /// <param name="specificationAttributeId">The specification attribute identifier</param>
        /// <param name="languageId">Language identifier</param>
        /// <returns>Specification attribute</returns>
        public static SpecificationAttribute GetSpecificationAttributeById(int specificationAttributeId, int languageId)
        {
            if (specificationAttributeId == 0)
                return null;

            string key = string.Format(SPECIFICATIONATTRIBUTE_BY_ID_KEY, specificationAttributeId, languageId);
            object obj2 = NopCache.Get(key);
            if (SpecificationAttributeManager.CacheEnabled && (obj2 != null))
            {
                return (SpecificationAttribute)obj2;
            }

            var dbItem = DBProviderManager<DBSpecificationAttributeProvider>.Provider.GetSpecificationAttributeById(specificationAttributeId, languageId);
            var specificationAttribute = DBMapping(dbItem);

            if (SpecificationAttributeManager.CacheEnabled)
            {
                NopCache.Max(key, specificationAttribute);
            }
            return specificationAttribute;
        }

        /// <summary>
        /// Gets specification attribute collection
        /// </summary>
        /// <returns>Specification attribute collection</returns>
        public static SpecificationAttributeCollection GetSpecificationAttributes()
        {
            int languageId = 0;
            if (NopContext.Current != null)
                languageId = NopContext.Current.WorkingLanguage.LanguageId;
            return GetSpecificationAttributes(languageId);
        }

        /// <summary>
        /// Gets specification attribute collection
        /// </summary>
        /// <param name="languageId">Language identifier</param>
        /// <returns>Specification attribute collection</returns>
        public static SpecificationAttributeCollection GetSpecificationAttributes(int languageId)
        {
            var dbCollection = DBProviderManager<DBSpecificationAttributeProvider>.Provider.GetSpecificationAttributes(languageId);
            var specificationAttributes = DBMapping(dbCollection);
            return specificationAttributes;
        }

        /// <summary>
        /// Deletes a specification attribute
        /// </summary>
        /// <param name="specificationAttributeId">The specification attribute identifier</param>
        public static void DeleteSpecificationAttribute(int specificationAttributeId)
        {
            DBProviderManager<DBSpecificationAttributeProvider>.Provider.DeleteSpecificationAttribute(specificationAttributeId);
            if (SpecificationAttributeManager.CacheEnabled)
            {
                NopCache.RemoveByPattern(SPECIFICATIONATTRIBUTE_PATTERN_KEY);
                NopCache.RemoveByPattern(SPECIFICATIONATTRIBUTEOPTION_PATTERN_KEY);
                NopCache.RemoveByPattern(PRODUCTSPECIFICATIONATTRIBUTE_PATTERN_KEY);
            }
        }

        /// <summary>
        /// Inserts a specification attribute
        /// </summary>
        /// <param name="name">The name</param>
        /// <param name="displayOrder">Display order</param>
        /// <returns>Specification attribute</returns>
        public static SpecificationAttribute InsertSpecificationAttribute(string name, int displayOrder)
        {
            var dbItem = DBProviderManager<DBSpecificationAttributeProvider>.Provider.InsertSpecificationAttribute(name, displayOrder);
            var specificationAttribute = DBMapping(dbItem);

            if (SpecificationAttributeManager.CacheEnabled)
            {
                NopCache.RemoveByPattern(SPECIFICATIONATTRIBUTE_PATTERN_KEY);
                NopCache.RemoveByPattern(SPECIFICATIONATTRIBUTEOPTION_PATTERN_KEY);
                NopCache.RemoveByPattern(PRODUCTSPECIFICATIONATTRIBUTE_PATTERN_KEY);
            }

            return specificationAttribute;
        }

        /// <summary>
        /// Updates the specification attribute
        /// </summary>
        /// <param name="specificationAttributeId">The specification attribute identifier</param>
        /// <param name="name">The name</param>
        /// <param name="displayOrder">Display order</param>
        /// <returns>Specification attribute</returns>
        public static SpecificationAttribute UpdateSpecificationAttribute(int specificationAttributeId, string name, int displayOrder)
        {
            var dbItem = DBProviderManager<DBSpecificationAttributeProvider>.Provider.UpdateSpecificationAttribute(specificationAttributeId, name, displayOrder);
            var specificationAttribute = DBMapping(dbItem);
            if (SpecificationAttributeManager.CacheEnabled)
            {
                NopCache.RemoveByPattern(SPECIFICATIONATTRIBUTE_PATTERN_KEY);
                NopCache.RemoveByPattern(SPECIFICATIONATTRIBUTEOPTION_PATTERN_KEY);
                NopCache.RemoveByPattern(PRODUCTSPECIFICATIONATTRIBUTE_PATTERN_KEY);
            }

            return specificationAttribute;
        }

        /// <summary>
        /// Gets localized specification attribute by id
        /// </summary>
        /// <param name="specificationAttributeLocalizedId">Localized specification identifier</param>
        /// <returns>Specification attribute content</returns>
        public static SpecificationAttributeLocalized GetSpecificationAttributeLocalizedById(int specificationAttributeLocalizedId)
        {
            if (specificationAttributeLocalizedId == 0)
                return null;

            var dbItem = DBProviderManager<DBSpecificationAttributeProvider>.Provider.GetSpecificationAttributeLocalizedById(specificationAttributeLocalizedId);
            var item = DBMapping(dbItem);
            return item;
        }

        /// <summary>
        /// Gets localized specification attribute by specification attribute id and language id
        /// </summary>
        /// <param name="specificationAttributeId">Specification attribute identifier</param>
        /// <param name="languageId">Language identifier</param>
        /// <returns>Specification attribute content</returns>
        public static SpecificationAttributeLocalized GetSpecificationAttributeLocalizedBySpecificationAttributeIdAndLanguageId(int specificationAttributeId, int languageId)
        {
            if (specificationAttributeId == 0 || languageId == 0)
                return null;

            var dbItem = DBProviderManager<DBSpecificationAttributeProvider>.Provider.GetSpecificationAttributeLocalizedBySpecificationAttributeIdAndLanguageId(specificationAttributeId, languageId);
            var item = DBMapping(dbItem);
            return item;
        }

        /// <summary>
        /// Inserts a localized specification attribute
        /// </summary>
        /// <param name="specificationAttributeId">Specification attribute identifier</param>
        /// <param name="languageId">Language identifier</param>
        /// <param name="name">Name text</param>
        /// <returns>Specification attribute content</returns>
        public static SpecificationAttributeLocalized InsertSpecificationAttributeLocalized(int specificationAttributeId,
            int languageId, string name)
        {
            var dbItem = DBProviderManager<DBSpecificationAttributeProvider>.Provider.InsertSpecificationAttributeLocalized(specificationAttributeId,
                languageId, name);
            var item = DBMapping(dbItem);

            if (SpecificationAttributeManager.CacheEnabled)
            {
                NopCache.RemoveByPattern(SPECIFICATIONATTRIBUTE_PATTERN_KEY);
                NopCache.RemoveByPattern(SPECIFICATIONATTRIBUTEOPTION_PATTERN_KEY);
                NopCache.RemoveByPattern(PRODUCTSPECIFICATIONATTRIBUTE_PATTERN_KEY);
            }

            return item;
        }

        /// <summary>
        /// Update a localized specification attribute
        /// </summary>
        /// <param name="specificationAttributeLocalizedId">Localized specification attribute identifier</param>
        /// <param name="specificationAttributeId">Specification attribute identifier</param>
        /// <param name="languageId">Language identifier</param>
        /// <param name="name">Name text</param>
        /// <returns>Specification attribute content</returns>
        public static SpecificationAttributeLocalized UpdateSpecificationAttributeLocalized(int specificationAttributeLocalizedId,
            int specificationAttributeId, int languageId, string name)
        {
            var dbItem = DBProviderManager<DBSpecificationAttributeProvider>.Provider.UpdateSpecificationAttributeLocalized(specificationAttributeLocalizedId,
                specificationAttributeId, languageId, name);
            var item = DBMapping(dbItem);

            if (SpecificationAttributeManager.CacheEnabled)
            {
                NopCache.RemoveByPattern(SPECIFICATIONATTRIBUTE_PATTERN_KEY);
                NopCache.RemoveByPattern(SPECIFICATIONATTRIBUTEOPTION_PATTERN_KEY);
                NopCache.RemoveByPattern(PRODUCTSPECIFICATIONATTRIBUTE_PATTERN_KEY);
            }

            return item;
        }
        
        #endregion

        #region Specification attribute option

        /// <summary>
        /// Gets a specification attribute option
        /// </summary>
        /// <param name="specificationAttributeOptionId">The specification attribute option identifier</param>
        /// <returns>Specification attribute option</returns>
        public static SpecificationAttributeOption GetSpecificationAttributeOptionById(int specificationAttributeOptionId)
        {
            int languageId = 0;
            if (NopContext.Current != null)
                languageId = NopContext.Current.WorkingLanguage.LanguageId;
            return GetSpecificationAttributeOptionById(specificationAttributeOptionId, languageId);
        }

        /// <summary>
        /// Gets a specification attribute option
        /// </summary>
        /// <param name="specificationAttributeOptionId">The specification attribute option identifier</param>
        /// <param name="languageId">Language identifier</param>
        /// <returns>Specification attribute option</returns>
        public static SpecificationAttributeOption GetSpecificationAttributeOptionById(int specificationAttributeOptionId, int languageId)
        {
            if (specificationAttributeOptionId == 0)
                return null;

            string key = string.Format(SPECIFICATIONATTRIBUTEOPTION_BY_ID_KEY, specificationAttributeOptionId, languageId);
            object obj2 = NopCache.Get(key);
            if (SpecificationAttributeManager.CacheEnabled && (obj2 != null))
            {
                return (SpecificationAttributeOption)obj2;
            }

            var dbItem = DBProviderManager<DBSpecificationAttributeProvider>.Provider.GetSpecificationAttributeOptionById(specificationAttributeOptionId, languageId);
            var specificationAttribute = DBMapping(dbItem);

            if (SpecificationAttributeManager.CacheEnabled)
            {
                NopCache.Max(key, specificationAttribute);
            }
            return specificationAttribute;
        }

        /// <summary>
        /// Gets a specification attribute option by specification attribute id
        /// </summary>
        /// <param name="specificationAttributeId">The specification attribute identifier</param>
        /// <returns>Specification attribute option</returns>
        public static SpecificationAttributeOptionCollection GetSpecificationAttributeOptionsBySpecificationAttribute(int specificationAttributeId)
        {
            int languageId = 0;
            if (NopContext.Current != null)
                languageId = NopContext.Current.WorkingLanguage.LanguageId;
            return GetSpecificationAttributeOptionsBySpecificationAttribute(specificationAttributeId, languageId);
        }

        /// <summary>
        /// Gets a specification attribute option by specification attribute id
        /// </summary>
        /// <param name="specificationAttributeId">The specification attribute identifier</param>
        /// <param name="languageId">Language identifier</param>
        /// <returns>Specification attribute option</returns>
        public static SpecificationAttributeOptionCollection GetSpecificationAttributeOptionsBySpecificationAttribute(int specificationAttributeId, int languageId)
        {
            var dbCollection = DBProviderManager<DBSpecificationAttributeProvider>.Provider.GetSpecificationAttributeOptionsBySpecificationAttributeId(specificationAttributeId, languageId);
            var specificationAttributeOptions = DBMapping(dbCollection);
            return specificationAttributeOptions;
        }

        /// <summary>
        /// Gets specification attribute option collection
        /// </summary>
        /// <returns>Specification attribute option collection</returns>
        public static SpecificationAttributeOptionCollection GetSpecificationAttributeOptions()
        {
            int languageId = 0;
            if (NopContext.Current != null)
                languageId = NopContext.Current.WorkingLanguage.LanguageId;
            return GetSpecificationAttributeOptions(languageId);
        }

        /// <summary>
        /// Gets specification attribute option collection
        /// </summary>
        /// <param name="languageId">Language identifier</param>
        /// <returns>Specification attribute option collection</returns>
        public static SpecificationAttributeOptionCollection GetSpecificationAttributeOptions(int languageId)
        {
            var dbCollection = DBProviderManager<DBSpecificationAttributeProvider>.Provider.GetSpecificationAttributeOptions(languageId);
            var specificationAttributeOptions = DBMapping(dbCollection);
            return specificationAttributeOptions;
        }

        /// <summary>
        /// Deletes a specification attribute option
        /// </summary>
        /// <param name="specificationAttributeOptionId">The specification attribute option identifier</param>
        public static void DeleteSpecificationAttributeOption(int specificationAttributeOptionId)
        {
            DBProviderManager<DBSpecificationAttributeProvider>.Provider.DeleteSpecificationAttributeOption(specificationAttributeOptionId);
            if (SpecificationAttributeManager.CacheEnabled)
            {
                NopCache.RemoveByPattern(SPECIFICATIONATTRIBUTEOPTION_PATTERN_KEY);
                NopCache.RemoveByPattern(SPECIFICATIONATTRIBUTEOPTION_PATTERN_KEY);
                NopCache.RemoveByPattern(PRODUCTSPECIFICATIONATTRIBUTE_PATTERN_KEY);
            }
        }

        /// <summary>
        /// Inserts a specification attribute option
        /// </summary>
        /// <param name="specificationAttributeId">The specification attribute identifier</param>
        /// <param name="name">The name</param>
        /// <param name="displayOrder">Display order</param>
        /// <returns>Specification attribute option</returns>
        public static SpecificationAttributeOption InsertSpecificationAttributeOption(int specificationAttributeId, 
            string name, int displayOrder)
        {
            var dbItem = DBProviderManager<DBSpecificationAttributeProvider>.Provider.InsertSpecificationAttributeOption(specificationAttributeId, name, displayOrder);
            var specificationAttribute = DBMapping(dbItem);

            if (SpecificationAttributeManager.CacheEnabled)
            {
                NopCache.RemoveByPattern(SPECIFICATIONATTRIBUTEOPTION_PATTERN_KEY);
                NopCache.RemoveByPattern(SPECIFICATIONATTRIBUTEOPTION_PATTERN_KEY);
                NopCache.RemoveByPattern(PRODUCTSPECIFICATIONATTRIBUTE_PATTERN_KEY);
            }

            return specificationAttribute;
        }

        /// <summary>
        /// Updates the specification attribute
        /// </summary>
        /// <param name="specificationAttributeOptionId">The specification attribute option identifier</param>
        /// <param name="specificationAttributeId">The specification attribute identifier</param>
        /// <param name="name">The name</param>
        /// <param name="displayOrder">Display order</param>
        /// <returns>Specification attribute option</returns>
        public static SpecificationAttributeOption UpdateSpecificationAttributeOptions(int specificationAttributeOptionId, 
            int specificationAttributeId, string name, int displayOrder)
        {
            var dbItem = DBProviderManager<DBSpecificationAttributeProvider>.Provider.UpdateSpecificationAttributeOption(specificationAttributeOptionId, specificationAttributeId, name, displayOrder);
            var specificationAttributeOption = DBMapping(dbItem);
            if (SpecificationAttributeManager.CacheEnabled)
            {
                NopCache.RemoveByPattern(SPECIFICATIONATTRIBUTEOPTION_PATTERN_KEY);
                NopCache.RemoveByPattern(SPECIFICATIONATTRIBUTEOPTION_PATTERN_KEY);
                NopCache.RemoveByPattern(PRODUCTSPECIFICATIONATTRIBUTE_PATTERN_KEY);
            }

            return specificationAttributeOption;
        }

        /// <summary>
        /// Gets localized specification attribute option by id
        /// </summary>
        /// <param name="specificationAttributeOptionLocalizedId">Localized specification attribute option identifier</param>
        /// <returns>Localized specification attribute option</returns>
        public static SpecificationAttributeOptionLocalized GetSpecificationAttributeOptionLocalizedById(int specificationAttributeOptionLocalizedId)
        {
            if (specificationAttributeOptionLocalizedId == 0)
                return null;

            var dbItem = DBProviderManager<DBSpecificationAttributeProvider>.Provider.GetSpecificationAttributeOptionLocalizedById(specificationAttributeOptionLocalizedId);
            var item = DBMapping(dbItem);
            return item;
        }

        /// <summary>
        /// Gets localized specification attribute option by specification attribute option id and language id
        /// </summary>
        /// <param name="specificationAttributeOptionId">Specification attribute option identifier</param>
        /// <param name="languageId">Language identifier</param>
        /// <returns>Localized specification attribute option</returns>
        public static SpecificationAttributeOptionLocalized GetSpecificationAttributeOptionLocalizedBySpecificationAttributeOptionIdAndLanguageId(int specificationAttributeOptionId, int languageId)
        {
            if (specificationAttributeOptionId == 0 || languageId == 0)
                return null;

            var dbItem = DBProviderManager<DBSpecificationAttributeProvider>.Provider.GetSpecificationAttributeOptionLocalizedBySpecificationAttributeOptionIdAndLanguageId(specificationAttributeOptionId, languageId);
            var item = DBMapping(dbItem);
            return item;
        }

        /// <summary>
        /// Inserts a localized specification attribute option
        /// </summary>
        /// <param name="specificationAttributeOptionId">Specification attribute option identifier</param>
        /// <param name="languageId">Language identifier</param>
        /// <param name="name">Name text</param>
        /// <returns>Localized specification attribute option</returns>
        public static SpecificationAttributeOptionLocalized InsertSpecificationAttributeOptionLocalized(int specificationAttributeOptionId,
            int languageId, string name)
        {
            var dbItem = DBProviderManager<DBSpecificationAttributeProvider>.Provider.InsertSpecificationAttributeOptionLocalized(specificationAttributeOptionId,
                languageId, name);
            var item = DBMapping(dbItem);

            if (SpecificationAttributeManager.CacheEnabled)
            {
                NopCache.RemoveByPattern(SPECIFICATIONATTRIBUTE_PATTERN_KEY);
                NopCache.RemoveByPattern(SPECIFICATIONATTRIBUTEOPTION_PATTERN_KEY);
                NopCache.RemoveByPattern(PRODUCTSPECIFICATIONATTRIBUTE_PATTERN_KEY);
            }

            return item;
        }

        /// <summary>
        /// Update a localized specification attribute option
        /// </summary>
        /// <param name="specificationAttributeOptionLocalizedId">Localized specification attribute option identifier</param>
        /// <param name="specificationAttributeOptionId">Specification attribute option identifier</param>
        /// <param name="languageId">Language identifier</param>
        /// <param name="name">Name text</param>
        /// <returns>Localized specification attribute option</returns>
        public static SpecificationAttributeOptionLocalized UpdateSpecificationAttributeOptionLocalized(int specificationAttributeOptionLocalizedId,
            int specificationAttributeOptionId, int languageId, string name)
        {
            var dbItem = DBProviderManager<DBSpecificationAttributeProvider>.Provider.UpdateSpecificationAttributeOptionLocalized(specificationAttributeOptionLocalizedId,
                specificationAttributeOptionId, languageId, name);
            var item = DBMapping(dbItem);

            if (SpecificationAttributeManager.CacheEnabled)
            {
                NopCache.RemoveByPattern(SPECIFICATIONATTRIBUTE_PATTERN_KEY);
                NopCache.RemoveByPattern(SPECIFICATIONATTRIBUTEOPTION_PATTERN_KEY);
                NopCache.RemoveByPattern(PRODUCTSPECIFICATIONATTRIBUTE_PATTERN_KEY);
            }

            return item;
        }
        
        #endregion

        #region Product specification attribute

        /// <summary>
        /// Deletes a product specification attribute mapping
        /// </summary>
        /// <param name="productSpecificationAttributeId">Product specification attribute identifier</param>
        public static void DeleteProductSpecificationAttribute(int productSpecificationAttributeId)
        {
            DBProviderManager<DBSpecificationAttributeProvider>.Provider.DeleteProductSpecificationAttribute(productSpecificationAttributeId);

            if (SpecificationAttributeManager.CacheEnabled)
            {
                NopCache.RemoveByPattern(SPECIFICATIONATTRIBUTEOPTION_PATTERN_KEY);
                NopCache.RemoveByPattern(SPECIFICATIONATTRIBUTEOPTION_PATTERN_KEY);
                NopCache.RemoveByPattern(PRODUCTSPECIFICATIONATTRIBUTE_PATTERN_KEY);
            }
        }

        /// <summary>
        /// Gets a product specification attribute mapping collection
        /// </summary>
        /// <param name="productId">Product identifier</param>
        /// <returns>Product specification attribute mapping collection</returns>
        public static ProductSpecificationAttributeCollection GetProductSpecificationAttributesByProductId(int productId)
        {
            return GetProductSpecificationAttributesByProductId(productId, null, null);
        }

        /// <summary>
        /// Gets a product specification attribute mapping collection
        /// </summary>
        /// <param name="productId">Product identifier</param>
        /// <param name="allowFiltering">0 to load attributes with AllowFiltering set to false, 0 to load attributes with AllowFiltering set to true, null to load all attributes</param>
        /// <param name="showOnProductPage">0 to load attributes with ShowOnProductPage set to false, 0 to load attributes with ShowOnProductPage set to true, null to load all attributes</param>
        /// <returns>Product specification attribute mapping collection</returns>
        public static ProductSpecificationAttributeCollection GetProductSpecificationAttributesByProductId(int productId, 
            bool? allowFiltering, bool? showOnProductPage)
        {
            string allowFilteringCacheStr = "null";
            if (allowFiltering.HasValue)
                allowFilteringCacheStr = allowFiltering.ToString();
            string showOnProductPageCacheStr = "null";
            if (showOnProductPage.HasValue)
                showOnProductPageCacheStr = showOnProductPage.ToString();
            string key = string.Format(PRODUCTSPECIFICATIONATTRIBUTE_ALLBYPRODUCTID_KEY, productId, allowFilteringCacheStr, showOnProductPageCacheStr);
            object obj2 = NopCache.Get(key);
            if (SpecificationAttributeManager.CacheEnabled && (obj2 != null))
            {
                return (ProductSpecificationAttributeCollection)obj2;
            }

            var dbCollection = DBProviderManager<DBSpecificationAttributeProvider>.Provider.GetProductSpecificationAttributesByProductId(productId, 
                allowFiltering, showOnProductPage);
            var productSpecificationAttributes = DBMapping(dbCollection);

            if (SpecificationAttributeManager.CacheEnabled)
            {
                NopCache.Max(key, productSpecificationAttributes);
            }
            return productSpecificationAttributes;
        }

        /// <summary>
        /// Gets a product specification attribute mapping 
        /// </summary>
        /// <param name="productSpecificationAttributeId">Product specification attribute mapping identifier</param>
        /// <returns>Product specification attribute mapping</returns>
        public static ProductSpecificationAttribute GetProductSpecificationAttributeById(int productSpecificationAttributeId)
        {
            if (productSpecificationAttributeId == 0)
                return null;

            var dbItem = DBProviderManager<DBSpecificationAttributeProvider>.Provider.GetProductSpecificationAttributeById(productSpecificationAttributeId);
            var productSpecificationAttribute = DBMapping(dbItem);
            return productSpecificationAttribute;
        }

        /// <summary>
        /// Inserts a product specification attribute mapping
        /// </summary>
        /// <param name="productId">Product identifier</param>
        /// <param name="specificationAttributeOptionId">Specification attribute option identifier</param>
        /// <param name="allowFiltering">Allow product filtering by this attribute</param>
        /// <param name="showOnProductPage">Show the attribute on the product page</param>
        /// <param name="displayOrder">The display order</param>
        /// <returns>Product specification attribute mapping</returns>
        public static ProductSpecificationAttribute InsertProductSpecificationAttribute(int productId, int specificationAttributeOptionId,
           bool allowFiltering, bool showOnProductPage, int displayOrder)
        {
            var dbItem = DBProviderManager<DBSpecificationAttributeProvider>.Provider.InsertProductSpecificationAttribute(productId,
                specificationAttributeOptionId, allowFiltering, showOnProductPage, displayOrder);
            var productSpecificationAttribute = DBMapping(dbItem);
            if (SpecificationAttributeManager.CacheEnabled)
            {
                NopCache.RemoveByPattern(SPECIFICATIONATTRIBUTEOPTION_PATTERN_KEY);
                NopCache.RemoveByPattern(SPECIFICATIONATTRIBUTEOPTION_PATTERN_KEY);
                NopCache.RemoveByPattern(PRODUCTSPECIFICATIONATTRIBUTE_PATTERN_KEY);
            }
            return productSpecificationAttribute;
        }

        /// <summary>
        /// Updates the product specification attribute mapping
        /// </summary>
        /// <param name="productSpecificationAttributeId">product specification attribute mapping identifier</param>
        /// <param name="productId">Product identifier</param>
        /// <param name="specificationAttributeOptionId">Specification attribute identifier</param>
        /// <param name="allowFiltering">Allow product filtering by this attribute</param>
        /// <param name="showOnProductPage">Show the attribute on the product page</param>
        /// <param name="displayOrder">The display order</param>
        /// <returns>Product specification attribute mapping</returns>
        public static ProductSpecificationAttribute UpdateProductSpecificationAttribute(int productSpecificationAttributeId,
            int productId, int specificationAttributeOptionId, bool allowFiltering, bool showOnProductPage, int displayOrder)
        {
            var dbItem = DBProviderManager<DBSpecificationAttributeProvider>.Provider.UpdateProductSpecificationAttribute(productSpecificationAttributeId,
                productId, specificationAttributeOptionId, allowFiltering, showOnProductPage, displayOrder);
            var productSpecificationAttribute = DBMapping(dbItem);
            if (SpecificationAttributeManager.CacheEnabled)
            {
                NopCache.RemoveByPattern(SPECIFICATIONATTRIBUTEOPTION_PATTERN_KEY);
                NopCache.RemoveByPattern(SPECIFICATIONATTRIBUTEOPTION_PATTERN_KEY);
                NopCache.RemoveByPattern(PRODUCTSPECIFICATIONATTRIBUTE_PATTERN_KEY);
            }
            return productSpecificationAttribute;
        }

        #endregion

        #region Specification attribute option filter


        /// <summary>
        /// Gets a filtered product specification attribute mapping collection by category id
        /// </summary>
        /// <param name="categoryId">Category identifier</param>
        /// <returns>Product specification attribute mapping collection</returns>
        public static SpecificationAttributeOptionFilterCollection GetSpecificationAttributeOptionFilter(int categoryId)
        {
            int languageId = 0;
            if (NopContext.Current != null)
                languageId = NopContext.Current.WorkingLanguage.LanguageId;
            return GetSpecificationAttributeOptionFilter(categoryId, languageId);
        }

        /// <summary>
        /// Gets a filtered product specification attribute mapping collection by category id
        /// </summary>
        /// <param name="categoryId">Category identifier</param>
        /// <param name="languageId">Language identifier</param>
        /// <returns>Product specification attribute mapping collection</returns>
        public static SpecificationAttributeOptionFilterCollection GetSpecificationAttributeOptionFilter(int categoryId, int languageId)
        {
            var dbCol = DBProviderManager<DBSpecificationAttributeProvider>.Provider.GetSpecificationAttributeOptionFilterByCategoryId(categoryId, languageId);
            var col = DBMapping(dbCol);
            return col;
        }


        #endregion

        #endregion

        #region Property
        /// <summary>
        /// Gets a value indicating whether cache is enabled
        /// </summary>
        public static bool CacheEnabled
        {
            get
            {
                return SettingManager.GetSettingValueBoolean("Cache.SpecificationAttributeManager.CacheEnabled");
            }
        }
        #endregion
    }
}
