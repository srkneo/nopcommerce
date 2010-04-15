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
            item.SpecificationAttributeID = dbItem.SpecificationAttributeID;
            item.Name = dbItem.Name;
            item.DisplayOrder = dbItem.DisplayOrder;

            return item;
        }

        private static SpecificationAttributeLocalized DBMapping(DBSpecificationAttributeLocalized dbItem)
        {
            if (dbItem == null)
                return null;

            var item = new SpecificationAttributeLocalized();
            item.SpecificationAttributeLocalizedID = dbItem.SpecificationAttributeLocalizedID;
            item.SpecificationAttributeID = dbItem.SpecificationAttributeID;
            item.LanguageID = dbItem.LanguageID;
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
            item.SpecificationAttributeOptionID = dbItem.SpecificationAttributeOptionID;
            item.SpecificationAttributeID = dbItem.SpecificationAttributeID;
            item.Name = dbItem.Name;
            item.DisplayOrder = dbItem.DisplayOrder;

            return item;
        }

        private static SpecificationAttributeOptionLocalized DBMapping(DBSpecificationAttributeOptionLocalized dbItem)
        {
            if (dbItem == null)
                return null;

            var item = new SpecificationAttributeOptionLocalized();
            item.SpecificationAttributeOptionLocalizedID = dbItem.SpecificationAttributeOptionLocalizedID;
            item.SpecificationAttributeOptionID = dbItem.SpecificationAttributeOptionID;
            item.LanguageID = dbItem.LanguageID;
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
            item.ProductSpecificationAttributeID = dbItem.ProductSpecificationAttributeID;
            item.ProductID = dbItem.ProductID;
            item.SpecificationAttributeOptionID = dbItem.SpecificationAttributeOptionID;
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
            item.SpecificationAttributeID = dbItem.SpecificationAttributeID;
            item.SpecificationAttributeName = dbItem.SpecificationAttributeName;
            item.DisplayOrder = dbItem.DisplayOrder;
            item.SpecificationAttributeOptionID = dbItem.SpecificationAttributeOptionID;
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

        #region SpecificationAttribute

        /// <summary>
        /// Gets a specification attribute
        /// </summary>
        /// <param name="SpecificationAttributeID">The specification attribute identifier</param>
        /// <returns>Specification attribute</returns>
        public static SpecificationAttribute GetSpecificationAttributeByID(int SpecificationAttributeID)
        {
            int languageId = 0;
            if (NopContext.Current != null)
                languageId = NopContext.Current.WorkingLanguage.LanguageID;
            return GetSpecificationAttributeByID(SpecificationAttributeID, languageId);
        }

        /// <summary>
        /// Gets a specification attribute
        /// </summary>
        /// <param name="SpecificationAttributeID">The specification attribute identifier</param>
        /// <param name="LanguageID">Language identifier</param>
        /// <returns>Specification attribute</returns>
        public static SpecificationAttribute GetSpecificationAttributeByID(int SpecificationAttributeID, int LanguageID)
        {
            if (SpecificationAttributeID == 0)
                return null;

            string key = string.Format(SPECIFICATIONATTRIBUTE_BY_ID_KEY, SpecificationAttributeID, LanguageID);
            object obj2 = NopCache.Get(key);
            if (SpecificationAttributeManager.CacheEnabled && (obj2 != null))
            {
                return (SpecificationAttribute)obj2;
            }

            var dbItem = DBProviderManager<DBSpecificationAttributeProvider>.Provider.GetSpecificationAttributeByID(SpecificationAttributeID, LanguageID);
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
                languageId = NopContext.Current.WorkingLanguage.LanguageID;
            return GetSpecificationAttributes(languageId);
        }

        /// <summary>
        /// Gets specification attribute collection
        /// </summary>
        /// <param name="LanguageID">Language identifier</param>
        /// <returns>Specification attribute collection</returns>
        public static SpecificationAttributeCollection GetSpecificationAttributes(int LanguageID)
        {
            var dbCollection = DBProviderManager<DBSpecificationAttributeProvider>.Provider.GetSpecificationAttributes(LanguageID);
            var specificationAttributes = DBMapping(dbCollection);
            return specificationAttributes;
        }

        /// <summary>
        /// Deletes a specification attribute
        /// </summary>
        /// <param name="SpecificationAttributeID">The specification attribute identifier</param>
        public static void DeleteSpecificationAttribute(int SpecificationAttributeID)
        {
            DBProviderManager<DBSpecificationAttributeProvider>.Provider.DeleteSpecificationAttribute(SpecificationAttributeID);
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
        /// <param name="specificationAttributeID">The specification attribute identifier</param>
        /// <param name="name">The name</param>
        /// <param name="displayOrder">Display order</param>
        /// <returns>Specification attribute</returns>
        public static SpecificationAttribute UpdateSpecificationAttribute(int specificationAttributeID, string name, int displayOrder)
        {
            var dbItem = DBProviderManager<DBSpecificationAttributeProvider>.Provider.UpdateSpecificationAttribute(specificationAttributeID, name, displayOrder);
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
        /// <param name="SpecificationAttributeLocalizedID">Localized specification identifier</param>
        /// <returns>Specification attribute content</returns>
        public static SpecificationAttributeLocalized GetSpecificationAttributeLocalizedByID(int SpecificationAttributeLocalizedID)
        {
            if (SpecificationAttributeLocalizedID == 0)
                return null;

            var dbItem = DBProviderManager<DBSpecificationAttributeProvider>.Provider.GetSpecificationAttributeLocalizedByID(SpecificationAttributeLocalizedID);
            var item = DBMapping(dbItem);
            return item;
        }

        /// <summary>
        /// Gets localized specification attribute by specification attribute id and language id
        /// </summary>
        /// <param name="SpecificationAttributeID">Specification attribute identifier</param>
        /// <param name="LanguageID">Language identifier</param>
        /// <returns>Specification attribute content</returns>
        public static SpecificationAttributeLocalized GetSpecificationAttributeLocalizedBySpecificationAttributeIDAndLanguageID(int SpecificationAttributeID, int LanguageID)
        {
            if (SpecificationAttributeID == 0 || LanguageID == 0)
                return null;

            var dbItem = DBProviderManager<DBSpecificationAttributeProvider>.Provider.GetSpecificationAttributeLocalizedBySpecificationAttributeIDAndLanguageID(SpecificationAttributeID, LanguageID);
            var item = DBMapping(dbItem);
            return item;
        }

        /// <summary>
        /// Inserts a localized specification attribute
        /// </summary>
        /// <param name="SpecificationAttributeID">Specification attribute identifier</param>
        /// <param name="LanguageID">Language identifier</param>
        /// <param name="Name">Name text</param>
        /// <returns>Specification attribute content</returns>
        public static SpecificationAttributeLocalized InsertSpecificationAttributeLocalized(int SpecificationAttributeID,
            int LanguageID, string Name)
        {
            var dbItem = DBProviderManager<DBSpecificationAttributeProvider>.Provider.InsertSpecificationAttributeLocalized(SpecificationAttributeID,
                LanguageID, Name);
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
        /// <param name="SpecificationAttributeLocalizedID">Localized specification attribute identifier</param>
        /// <param name="SpecificationAttributeID">Specification attribute identifier</param>
        /// <param name="LanguageID">Language identifier</param>
        /// <param name="Name">Name text</param>
        /// <returns>Specification attribute content</returns>
        public static SpecificationAttributeLocalized UpdateSpecificationAttributeLocalized(int SpecificationAttributeLocalizedID,
            int SpecificationAttributeID, int LanguageID, string Name)
        {
            var dbItem = DBProviderManager<DBSpecificationAttributeProvider>.Provider.UpdateSpecificationAttributeLocalized(SpecificationAttributeLocalizedID,
                SpecificationAttributeID, LanguageID, Name);
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

        #region SpecificationAttributeOption

        /// <summary>
        /// Gets a specification attribute option
        /// </summary>
        /// <param name="specificationAttributeOptionID">The specification attribute option identifier</param>
        /// <returns>Specification attribute option</returns>
        public static SpecificationAttributeOption GetSpecificationAttributeOptionByID(int specificationAttributeOptionID)
        {
            int languageId = 0;
            if (NopContext.Current != null)
                languageId = NopContext.Current.WorkingLanguage.LanguageID;
            return GetSpecificationAttributeOptionByID(specificationAttributeOptionID, languageId);
        }

        /// <summary>
        /// Gets a specification attribute option
        /// </summary>
        /// <param name="specificationAttributeOptionID">The specification attribute option identifier</param>
        /// <param name="LanguageID">Language identifier</param>
        /// <returns>Specification attribute option</returns>
        public static SpecificationAttributeOption GetSpecificationAttributeOptionByID(int specificationAttributeOptionID, int LanguageID)
        {
            if (specificationAttributeOptionID == 0)
                return null;

            string key = string.Format(SPECIFICATIONATTRIBUTEOPTION_BY_ID_KEY, specificationAttributeOptionID, LanguageID);
            object obj2 = NopCache.Get(key);
            if (SpecificationAttributeManager.CacheEnabled && (obj2 != null))
            {
                return (SpecificationAttributeOption)obj2;
            }

            var dbItem = DBProviderManager<DBSpecificationAttributeProvider>.Provider.GetSpecificationAttributeOptionByID(specificationAttributeOptionID, LanguageID);
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
        /// <param name="specificationAttributeID">The specification attribute identifier</param>
        /// <returns>Specification attribute option</returns>
        public static SpecificationAttributeOptionCollection GetSpecificationAttributeOptionsBySpecificationAttribute(int specificationAttributeID)
        {
            int languageId = 0;
            if (NopContext.Current != null)
                languageId = NopContext.Current.WorkingLanguage.LanguageID;
            return GetSpecificationAttributeOptionsBySpecificationAttribute(specificationAttributeID, languageId);
        }

        /// <summary>
        /// Gets a specification attribute option by specification attribute id
        /// </summary>
        /// <param name="specificationAttributeID">The specification attribute identifier</param>
        /// <param name="LanguageID">Language identifier</param>
        /// <returns>Specification attribute option</returns>
        public static SpecificationAttributeOptionCollection GetSpecificationAttributeOptionsBySpecificationAttribute(int specificationAttributeID, int LanguageID)
        {
            var dbCollection = DBProviderManager<DBSpecificationAttributeProvider>.Provider.GetSpecificationAttributeOptionsBySpecificationAttributeID(specificationAttributeID, LanguageID);
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
                languageId = NopContext.Current.WorkingLanguage.LanguageID;
            return GetSpecificationAttributeOptions(languageId);
        }

        /// <summary>
        /// Gets specification attribute option collection
        /// </summary>
        /// <param name="LanguageID">Language identifier</param>
        /// <returns>Specification attribute option collection</returns>
        public static SpecificationAttributeOptionCollection GetSpecificationAttributeOptions(int LanguageID)
        {
            var dbCollection = DBProviderManager<DBSpecificationAttributeProvider>.Provider.GetSpecificationAttributeOptions(LanguageID);
            var specificationAttributeOptions = DBMapping(dbCollection);
            return specificationAttributeOptions;
        }

        /// <summary>
        /// Deletes a specification attribute option
        /// </summary>
        /// <param name="specificationAttributeOptionID">The specification attribute option identifier</param>
        public static void DeleteSpecificationAttributeOption(int specificationAttributeOptionID)
        {
            DBProviderManager<DBSpecificationAttributeProvider>.Provider.DeleteSpecificationAttributeOption(specificationAttributeOptionID);
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
        /// <param name="specificationAttributeID">The specification attribute identifier</param>
        /// <param name="name">The name</param>
        /// <param name="displayOrder">Display order</param>
        /// <returns>Specification attribute option</returns>
        public static SpecificationAttributeOption InsertSpecificationAttributeOption(int specificationAttributeID, string name, int displayOrder)
        {
            var dbItem = DBProviderManager<DBSpecificationAttributeProvider>.Provider.InsertSpecificationAttributeOption(specificationAttributeID, name, displayOrder);
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
        /// <param name="specificationAttributeOptionID">The specification attribute option identifier</param>
        /// <param name="specificationAttributeID">The specification attribute identifier</param>
        /// <param name="name">The name</param>
        /// <param name="displayOrder">Display order</param>
        /// <returns>Specification attribute option</returns>
        public static SpecificationAttributeOption UpdateSpecificationAttributeOptions(int specificationAttributeOptionID, int specificationAttributeID, string name, int displayOrder)
        {
            var dbItem = DBProviderManager<DBSpecificationAttributeProvider>.Provider.UpdateSpecificationAttributeOption(specificationAttributeOptionID, specificationAttributeID, name, displayOrder);
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
        /// <param name="SpecificationAttributeOptionLocalizedID">Localized specification attribute option identifier</param>
        /// <returns>Localized specification attribute option</returns>
        public static SpecificationAttributeOptionLocalized GetSpecificationAttributeOptionLocalizedByID(int SpecificationAttributeOptionLocalizedID)
        {
            if (SpecificationAttributeOptionLocalizedID == 0)
                return null;

            var dbItem = DBProviderManager<DBSpecificationAttributeProvider>.Provider.GetSpecificationAttributeOptionLocalizedByID(SpecificationAttributeOptionLocalizedID);
            var item = DBMapping(dbItem);
            return item;
        }

        /// <summary>
        /// Gets localized specification attribute option by specification attribute option id and language id
        /// </summary>
        /// <param name="SpecificationAttributeOptionID">Specification attribute option identifier</param>
        /// <param name="LanguageID">Language identifier</param>
        /// <returns>Localized specification attribute option</returns>
        public static SpecificationAttributeOptionLocalized GetSpecificationAttributeOptionLocalizedBySpecificationAttributeOptionIDAndLanguageID(int SpecificationAttributeOptionID, int LanguageID)
        {
            if (SpecificationAttributeOptionID == 0 || LanguageID == 0)
                return null;

            var dbItem = DBProviderManager<DBSpecificationAttributeProvider>.Provider.GetSpecificationAttributeOptionLocalizedBySpecificationAttributeOptionIDAndLanguageID(SpecificationAttributeOptionID, LanguageID);
            var item = DBMapping(dbItem);
            return item;
        }

        /// <summary>
        /// Inserts a localized specification attribute option
        /// </summary>
        /// <param name="SpecificationAttributeOptionID">Specification attribute option identifier</param>
        /// <param name="LanguageID">Language identifier</param>
        /// <param name="Name">Name text</param>
        /// <returns>Localized specification attribute option</returns>
        public static SpecificationAttributeOptionLocalized InsertSpecificationAttributeOptionLocalized(int SpecificationAttributeOptionID,
            int LanguageID, string Name)
        {
            var dbItem = DBProviderManager<DBSpecificationAttributeProvider>.Provider.InsertSpecificationAttributeOptionLocalized(SpecificationAttributeOptionID,
                LanguageID, Name);
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
        /// <param name="SpecificationAttributeOptionLocalizedID">Localized specification attribute option identifier</param>
        /// <param name="SpecificationAttributeOptionID">Specification attribute option identifier</param>
        /// <param name="LanguageID">Language identifier</param>
        /// <param name="Name">Name text</param>
        /// <returns>Localized specification attribute option</returns>
        public static SpecificationAttributeOptionLocalized UpdateSpecificationAttributeOptionLocalized(int SpecificationAttributeOptionLocalizedID,
            int SpecificationAttributeOptionID, int LanguageID, string Name)
        {
            var dbItem = DBProviderManager<DBSpecificationAttributeProvider>.Provider.UpdateSpecificationAttributeOptionLocalized(SpecificationAttributeOptionLocalizedID,
                SpecificationAttributeOptionID, LanguageID, Name);
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

        #region ProductSpecificationAttribute

        /// <summary>
        /// Deletes a product specification attribute mapping
        /// </summary>
        /// <param name="ProductSpecificationAttributeID">Product specification attribute identifier</param>
        public static void DeleteProductSpecificationAttribute(int ProductSpecificationAttributeID)
        {
            DBProviderManager<DBSpecificationAttributeProvider>.Provider.DeleteProductSpecificationAttribute(ProductSpecificationAttributeID);

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
        /// <param name="ProductID">Product identifier</param>
        /// <returns>Product specification attribute mapping collection</returns>
        public static ProductSpecificationAttributeCollection GetProductSpecificationAttributesByProductID(int ProductID)
        {
            return GetProductSpecificationAttributesByProductID(ProductID, null, null);
        }

        /// <summary>
        /// Gets a product specification attribute mapping collection
        /// </summary>
        /// <param name="ProductID">Product identifier</param>
        /// <param name="AllowFiltering">0 to load attributes with AllowFiltering set to false, 0 to load attributes with AllowFiltering set to true, null to load all attributes</param>
        /// <param name="ShowOnProductPage">0 to load attributes with ShowOnProductPage set to false, 0 to load attributes with ShowOnProductPage set to true, null to load all attributes</param>
        /// <returns>Product specification attribute mapping collection</returns>
        public static ProductSpecificationAttributeCollection GetProductSpecificationAttributesByProductID(int ProductID, bool? AllowFiltering, bool? ShowOnProductPage)
        {
            string allowFilteringCacheStr = "null";
            if (AllowFiltering.HasValue)
                allowFilteringCacheStr = AllowFiltering.ToString();
            string showOnProductPageCacheStr = "null";
            if (ShowOnProductPage.HasValue)
                showOnProductPageCacheStr = ShowOnProductPage.ToString();
            string key = string.Format(PRODUCTSPECIFICATIONATTRIBUTE_ALLBYPRODUCTID_KEY, ProductID, allowFilteringCacheStr, showOnProductPageCacheStr);
            object obj2 = NopCache.Get(key);
            if (SpecificationAttributeManager.CacheEnabled && (obj2 != null))
            {
                return (ProductSpecificationAttributeCollection)obj2;
            }

            var dbCollection = DBProviderManager<DBSpecificationAttributeProvider>.Provider.GetProductSpecificationAttributesByProductID(ProductID, AllowFiltering, ShowOnProductPage);
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
        /// <param name="ProductSpecificationAttributeID">Product specification attribute mapping identifier</param>
        /// <returns>Product specification attribute mapping</returns>
        public static ProductSpecificationAttribute GetProductSpecificationAttributeByID(int ProductSpecificationAttributeID)
        {
            if (ProductSpecificationAttributeID == 0)
                return null;

            var dbItem = DBProviderManager<DBSpecificationAttributeProvider>.Provider.GetProductSpecificationAttributeByID(ProductSpecificationAttributeID);
            var productSpecificationAttribute = DBMapping(dbItem);
            return productSpecificationAttribute;
        }

        /// <summary>
        /// Gets a filtered product specification attribute mapping collection by category id
        /// </summary>
        /// <param name="CategoryID">Category identifier</param>
        /// <returns>Product specification attribute mapping collection</returns>
        public static SpecificationAttributeOptionFilterCollection GetSpecificationAttributeOptionFilter(int CategoryID)
        {
            int languageId = 0;
            if (NopContext.Current != null)
                languageId = NopContext.Current.WorkingLanguage.LanguageID;
            return GetSpecificationAttributeOptionFilter(CategoryID, languageId);
        }

        /// <summary>
        /// Gets a filtered product specification attribute mapping collection by category id
        /// </summary>
        /// <param name="CategoryID">Category identifier</param>
        /// <param name="LanguageID">Language identifier</param>
        /// <returns>Product specification attribute mapping collection</returns>
        public static SpecificationAttributeOptionFilterCollection GetSpecificationAttributeOptionFilter(int CategoryID, int LanguageID)
        {
            var dbCol = DBProviderManager<DBSpecificationAttributeProvider>.Provider.GetSpecificationAttributeOptionFilterByCategoryID(CategoryID, LanguageID);
            var col = DBMapping(dbCol);
            return col;
        }

        /// <summary>
        /// Inserts a product specification attribute mapping
        /// </summary>
        /// <param name="productID">Product identifier</param>
        /// <param name="specificationAttributeOptionID">Specification attribute option identifier</param>
        /// <param name="allowFiltering">Allow product filtering by this attribute</param>
        /// <param name="showOnProductPage">Show the attribute on the product page</param>
        /// <param name="displayOrder">The display order</param>
        /// <returns>Product specification attribute mapping</returns>
        public static ProductSpecificationAttribute InsertProductSpecificationAttribute(int productID, int specificationAttributeOptionID,
           bool allowFiltering, bool showOnProductPage, int displayOrder)
        {
            var dbItem = DBProviderManager<DBSpecificationAttributeProvider>.Provider.InsertProductSpecificationAttribute(productID,
                specificationAttributeOptionID, allowFiltering, showOnProductPage, displayOrder);
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
        /// <param name="productSpecificationAttributeID">product specification attribute mapping identifier</param>
        /// <param name="productID">Product identifier</param>
        /// <param name="specificationAttributeOptionID">Specification attribute identifier</param>
        /// <param name="allowFiltering">Allow product filtering by this attribute</param>
        /// <param name="showOnProductPage">Show the attribute on the product page</param>
        /// <param name="displayOrder">The display order</param>
        /// <returns>Product specification attribute mapping</returns>
        public static ProductSpecificationAttribute UpdateProductSpecificationAttribute(int productSpecificationAttributeID,
            int productID, int specificationAttributeOptionID, bool allowFiltering, bool showOnProductPage, int displayOrder)
        {
            var dbItem = DBProviderManager<DBSpecificationAttributeProvider>.Provider.UpdateProductSpecificationAttribute(productSpecificationAttributeID,
                productID, specificationAttributeOptionID, allowFiltering, showOnProductPage, displayOrder);
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
