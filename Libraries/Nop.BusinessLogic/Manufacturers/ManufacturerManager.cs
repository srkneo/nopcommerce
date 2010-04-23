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
using NopSolutions.NopCommerce.BusinessLogic.Profile;
using NopSolutions.NopCommerce.DataAccess;
using NopSolutions.NopCommerce.DataAccess.Manufacturers;

namespace NopSolutions.NopCommerce.BusinessLogic.Manufacturers
{
    /// <summary>
    /// Manufacturer manager
    /// </summary>
    public partial class ManufacturerManager
    {
        #region Constants
        private const string MANUFACTURERS_ALL_KEY = "Nop.manufacturer.all-{0}-{1}";
        private const string MANUFACTURERS_BY_ID_KEY = "Nop.manufacturer.id-{0}-{1}";
        private const string PRODUCTMANUFACTURERS_ALLBYMANUFACTURERID_KEY = "Nop.productmanufacturer.allbymanufacturerid-{0}-{1}";
        private const string PRODUCTMANUFACTURERS_ALLBYPRODUCTID_KEY = "Nop.productmanufacturer.allbyproductid-{0}-{1}";
        private const string PRODUCTMANUFACTURERS_BY_ID_KEY = "Nop.productmanufacturer.id-{0}";
        private const string MANUFACTURERS_PATTERN_KEY = "Nop.manufacturer.";
        private const string PRODUCTMANUFACTURERS_PATTERN_KEY = "Nop.productmanufacturer.";
        #endregion

        #region Utilities
        private static ManufacturerCollection DBMapping(DBManufacturerCollection dbCollection)
        {
            if (dbCollection == null)
                return null;

            var collection = new ManufacturerCollection();
            foreach (var dbItem in dbCollection)
            {
                var item = DBMapping(dbItem);
                collection.Add(item);
            }

            return collection;
        }

        private static Manufacturer DBMapping(DBManufacturer dbItem)
        {
            if (dbItem == null)
                return null;

            var item = new Manufacturer();
            item.ManufacturerId = dbItem.ManufacturerId;
            item.Name = dbItem.Name;
            item.Description = dbItem.Description;
            item.TemplateId = dbItem.TemplateId;
            item.MetaKeywords = dbItem.MetaKeywords;
            item.MetaDescription = dbItem.MetaDescription;
            item.MetaTitle = dbItem.MetaTitle;
            item.SEName = dbItem.SEName;
            item.PictureId = dbItem.PictureId;
            item.PageSize = dbItem.PageSize;
            item.PriceRanges = dbItem.PriceRanges;
            item.Published = dbItem.Published;
            item.Deleted = dbItem.Deleted;
            item.DisplayOrder = dbItem.DisplayOrder;
            item.CreatedOn = dbItem.CreatedOn;
            item.UpdatedOn = dbItem.UpdatedOn;

            return item;
        }

        private static ManufacturerLocalized DBMapping(DBManufacturerLocalized dbItem)
        {
            if (dbItem == null)
                return null;

            var item = new ManufacturerLocalized();
            item.ManufacturerLocalizedId = dbItem.ManufacturerLocalizedId;
            item.ManufacturerId = dbItem.ManufacturerId;
            item.LanguageId = dbItem.LanguageId;
            item.Name = dbItem.Name;
            item.Description = dbItem.Description;
            item.MetaKeywords = dbItem.MetaKeywords;
            item.MetaDescription = dbItem.MetaDescription;
            item.MetaTitle = dbItem.MetaTitle;
            item.SEName = dbItem.SEName;

            return item;
        }

        private static ProductManufacturerCollection DBMapping(DBProductManufacturerCollection dbCollection)
        {
            if (dbCollection == null)
                return null;

            var collection = new ProductManufacturerCollection();
            foreach (var dbItem in dbCollection)
            {
                var item = DBMapping(dbItem);
                collection.Add(item);
            }

            return collection;
        }

        private static ProductManufacturer DBMapping(DBProductManufacturer dbItem)
        {
            if (dbItem == null)
                return null;

            var item = new ProductManufacturer();
            item.ProductManufacturerId = dbItem.ProductManufacturerId;
            item.ProductId = dbItem.ProductId;
            item.ManufacturerId = dbItem.ManufacturerId;
            item.IsFeaturedProduct = dbItem.IsFeaturedProduct;
            item.DisplayOrder = dbItem.DisplayOrder;

            return item;
        }
        #endregion

        #region Methods
        /// <summary>
        /// Marks a manufacturer as deleted
        /// </summary>
        /// <param name="manufacturerId">Manufacturer identifer</param>
        public static void MarkManufacturerAsDeleted(int manufacturerId)
        {
            var manufacturer = GetManufacturerById(manufacturerId);
            if (manufacturer != null)
            {
                manufacturer = UpdateManufacturer(manufacturer.ManufacturerId, manufacturer.Name, manufacturer.Description,
                    manufacturer.TemplateId, manufacturer.MetaKeywords,
                    manufacturer.MetaDescription, manufacturer.MetaTitle,
                    manufacturer.SEName, manufacturer.PictureId, manufacturer.PageSize,
                    manufacturer.PriceRanges, manufacturer.Published,
                    true, manufacturer.DisplayOrder, manufacturer.CreatedOn, manufacturer.UpdatedOn);
            }
        }

        /// <summary>
        /// Removes a manufacturer picture
        /// </summary>
        /// <param name="manufacturerId">Manufacturer identifier</param>
        public static void RemoveManufacturerPicture(int manufacturerId)
        {
            var manufacturer = GetManufacturerById(manufacturerId);
            if (manufacturer != null)
            {
                UpdateManufacturer(manufacturer.ManufacturerId, manufacturer.Name, manufacturer.Description,
                    manufacturer.TemplateId, manufacturer.MetaKeywords,
                    manufacturer.MetaDescription, manufacturer.MetaTitle,
                    manufacturer.SEName, 0, manufacturer.PageSize, manufacturer.PriceRanges,
                    manufacturer.Published, manufacturer.Deleted, manufacturer.DisplayOrder, 
                    manufacturer.CreatedOn, manufacturer.UpdatedOn);
            }
        }

        /// <summary>
        /// Gets all manufacturers
        /// </summary>
        /// <returns>Manufacturer collection</returns>
        public static ManufacturerCollection GetAllManufacturers()
        {
            bool showHidden = NopContext.Current.IsAdmin;
            return GetAllManufacturers(showHidden);
        }

        /// <summary>
        /// Gets all manufacturers
        /// </summary>
        /// <param name="showHidden">A value indicating whether to show hidden records</param>
        /// <returns>Manufacturer collection</returns>
        public static ManufacturerCollection GetAllManufacturers(bool showHidden)
        {
            int languageId = 0;
            if (NopContext.Current != null)
                languageId = NopContext.Current.WorkingLanguage.LanguageId;
            return GetAllManufacturers(showHidden, languageId);
        }

        /// <summary>
        /// Gets all manufacturers
        /// </summary>
        /// <param name="showHidden">A value indicating whether to show hidden records</param>
        /// <param name="languageId">Language identifier</param>
        /// <returns>Manufacturer collection</returns>
        public static ManufacturerCollection GetAllManufacturers(bool showHidden, int languageId)
        {
            string key = string.Format(MANUFACTURERS_ALL_KEY, showHidden, languageId);
            object obj2 = NopCache.Get(key);
            if (ManufacturerManager.ManufacturersCacheEnabled && (obj2 != null))
            {
                return (ManufacturerCollection)obj2;
            }

            var dbCollection = DBProviderManager<DBManufacturerProvider>.Provider.GetAllManufacturers(showHidden, languageId);
            var manufacturerCollection = DBMapping(dbCollection);

            if (ManufacturerManager.ManufacturersCacheEnabled)
            {
                NopCache.Max(key, manufacturerCollection);
            }
            return manufacturerCollection;
        }

        /// <summary>
        /// Gets a manufacturer
        /// </summary>
        /// <param name="manufacturerId">Manufacturer identifier</param>
        /// <returns>Manufacturer</returns>
        public static Manufacturer GetManufacturerById(int manufacturerId)
        {
            int languageId = 0;
            if (NopContext.Current != null)
                languageId = NopContext.Current.WorkingLanguage.LanguageId;
            return GetManufacturerById(manufacturerId, languageId);
        }
        
        /// <summary>
        /// Gets a manufacturer
        /// </summary>
        /// <param name="manufacturerId">Manufacturer identifier</param>
        /// <param name="languageId">Language identifier</param>
        /// <returns>Manufacturer</returns>
        public static Manufacturer GetManufacturerById(int manufacturerId, int languageId)
        {
            if (manufacturerId == 0)
                return null;

            string key = string.Format(MANUFACTURERS_BY_ID_KEY, manufacturerId, languageId);
            object obj2 = NopCache.Get(key);
            if (ManufacturerManager.ManufacturersCacheEnabled && (obj2 != null))
            {
                return (Manufacturer)obj2;
            }

            var dbItem = DBProviderManager<DBManufacturerProvider>.Provider.GetManufacturerById(manufacturerId, languageId);
            var manufacturer = DBMapping(dbItem);

            if (ManufacturerManager.ManufacturersCacheEnabled)
            {
                NopCache.Max(key, manufacturer);
            }
            return manufacturer;
        }

        /// <summary>
        /// Inserts a manufacturer
        /// </summary>
        /// <param name="name">The name</param>
        /// <param name="description">The description</param>
        /// <param name="templateId">The template identifier</param>
        /// <param name="metaKeywords">The meta keywords</param>
        /// <param name="metaDescription">The meta description</param>
        /// <param name="metaTitle">The meta title</param>
        /// <param name="seName">The search-engine name</param>
        /// <param name="pictureId">The parent picture identifier</param>
        /// <param name="pageSize">The page size</param>
        /// <param name="priceRanges">The price ranges</param>
        /// <param name="published">A value indicating whether the entity is published</param>
        /// <param name="deleted">A value indicating whether the entity has been deleted</param>
        /// <param name="displayOrder">The display order</param>
        /// <param name="createdOn">The date and time of instance creation</param>
        /// <param name="updatedOn">The date and time of instance update</param>
        /// <returns>Manufacturer</returns>
        public static Manufacturer InsertManufacturer(string name, string description,
            int templateId, string metaKeywords, string metaDescription, string metaTitle,
            string seName, int pictureId, int pageSize, string priceRanges,
            bool published, bool deleted, int displayOrder,
            DateTime createdOn, DateTime updatedOn)
        {
            createdOn = DateTimeHelper.ConvertToUtcTime(createdOn);
            updatedOn = DateTimeHelper.ConvertToUtcTime(updatedOn);

            var dbItem = DBProviderManager<DBManufacturerProvider>.Provider.InsertManufacturer(name, 
                description, templateId, metaKeywords, metaDescription, metaTitle,
                seName, pictureId, pageSize, priceRanges, published, deleted,
                displayOrder, createdOn, updatedOn);
            var manufacturer = DBMapping(dbItem);

            if (ManufacturerManager.ManufacturersCacheEnabled || ManufacturerManager.MappingsCacheEnabled)
            {
                NopCache.RemoveByPattern(MANUFACTURERS_PATTERN_KEY);
                NopCache.RemoveByPattern(PRODUCTMANUFACTURERS_PATTERN_KEY);
            }

            return manufacturer;
        }

        /// <summary>
        /// Updates the manufacturer
        /// </summary>
        /// <param name="manufacturerId">Manufacturer identifier</param>
        /// <param name="name">The name</param>
        /// <param name="description">The description</param>
        /// <param name="templateId">The template identifier</param>
        /// <param name="metaKeywords">The meta keywords</param>
        /// <param name="metaDescription">The meta description</param>
        /// <param name="metaTitle">The meta title</param>
        /// <param name="seName">The search-engine name</param>
        /// <param name="pictureId">The parent picture identifier</param>
        /// <param name="pageSize">The page size</param>
        /// <param name="priceRanges">The price ranges</param>
        /// <param name="published">A value indicating whether the entity is published</param>
        /// <param name="deleted">A value indicating whether the entity has been deleted</param>
        /// <param name="displayOrder">The display order</param>
        /// <param name="createdOn">The date and time of instance creation</param>
        /// <param name="updatedOn">The date and time of instance update</param>
        /// <returns>Manufacturer</returns>
        public static Manufacturer UpdateManufacturer(int manufacturerId,
            string name, string description,
            int templateId, string metaKeywords, string metaDescription, string metaTitle,
            string seName, int pictureId, int pageSize, string priceRanges,
            bool published, bool deleted, int displayOrder,
            DateTime createdOn, DateTime updatedOn)
        {
            createdOn = DateTimeHelper.ConvertToUtcTime(createdOn);
            updatedOn = DateTimeHelper.ConvertToUtcTime(updatedOn);

            var dbItem = DBProviderManager<DBManufacturerProvider>.Provider.UpdateManufacturer(manufacturerId,
                name, description, templateId, metaKeywords, metaDescription, metaTitle,
                seName, pictureId, pageSize, priceRanges, published, deleted,
                displayOrder, createdOn, updatedOn);
            var manufacturer = DBMapping(dbItem);

            if (ManufacturerManager.ManufacturersCacheEnabled || ManufacturerManager.MappingsCacheEnabled)
            {
                NopCache.RemoveByPattern(MANUFACTURERS_PATTERN_KEY);
                NopCache.RemoveByPattern(PRODUCTMANUFACTURERS_PATTERN_KEY);
            }

            return manufacturer;
        }

        /// <summary>
        /// Gets localized manufacturer by id
        /// </summary>
        /// <param name="manufacturerLocalizedId">Localized manufacturer identifier</param>
        /// <returns>Manufacturer content</returns>
        public static ManufacturerLocalized GetManufacturerLocalizedById(int manufacturerLocalizedId)
        {
            if (manufacturerLocalizedId == 0)
                return null;

            var dbItem = DBProviderManager<DBManufacturerProvider>.Provider.GetManufacturerLocalizedById(manufacturerLocalizedId);
            var item = DBMapping(dbItem);
            return item;
        }

        /// <summary>
        /// Gets localized manufacturer by manufacturer id and language id
        /// </summary>
        /// <param name="manufacturerId">Manufacturer identifier</param>
        /// <param name="languageId">Language identifier</param>
        /// <returns>Manufacturer content</returns>
        public static ManufacturerLocalized GetManufacturerLocalizedByManufacturerIdAndLanguageId(int manufacturerId, int languageId)
        {
            if (manufacturerId == 0 || languageId == 0)
                return null;

            var dbItem = DBProviderManager<DBManufacturerProvider>.Provider.GetManufacturerLocalizedByManufacturerIdAndLanguageId(manufacturerId, languageId);
            var item = DBMapping(dbItem);
            return item;
        }

        /// <summary>
        /// Inserts a localized manufacturer
        /// </summary>
        /// <param name="manufacturerId">Manufacturer identifier</param>
        /// <param name="languageId">Language identifier</param>
        /// <param name="name">Name text</param>
        /// <param name="description">Description text</param>
        /// <param name="metaKeywords">Meta keywords text</param>
        /// <param name="metaDescription">Meta descriptions text</param>
        /// <param name="metaTitle">Metat title text</param>
        /// <param name="seName">Se name text</param>
        /// <returns>Manufacturer content</returns>
        public static ManufacturerLocalized InsertManufacturerLocalized(int manufacturerId,
            int languageId, string name, string description,
            string metaKeywords, string metaDescription, string metaTitle, string seName)
        {
            var dbItem = DBProviderManager<DBManufacturerProvider>.Provider.InsertManufacturerLocalized(manufacturerId,
                languageId, name, description, metaKeywords, metaDescription, metaTitle, seName);
            var item = DBMapping(dbItem);

            if (ManufacturerManager.ManufacturersCacheEnabled)
            {
                NopCache.RemoveByPattern(MANUFACTURERS_PATTERN_KEY);
            }

            return item;
        }

        /// <summary>
        /// Update a localized manufacturer
        /// </summary>
        /// <param name="manufacturerLocalizedId">Localized manufacturer identifier</param>
        /// <param name="manufacturerId">Manufacturer identifier</param>
        /// <param name="languageId">Language identifier</param>
        /// <param name="name">Name text</param>
        /// <param name="description">Description text</param>
        /// <param name="metaKeywords">Meta keywords text</param>
        /// <param name="metaDescription">Meta descriptions text</param>
        /// <param name="metaTitle">Metat title text</param>
        /// <param name="seName">Se name text</param>
        /// <returns>Manufacturer content</returns>
        public static ManufacturerLocalized UpdateManufacturerLocalized(int manufacturerLocalizedId,
            int manufacturerId, int languageId, string name, string description,
            string metaKeywords, string metaDescription, string metaTitle, string seName)
        {
            var dbItem = DBProviderManager<DBManufacturerProvider>.Provider.UpdateManufacturerLocalized(manufacturerLocalizedId,
                manufacturerId, languageId, name, description, metaKeywords, 
                metaDescription, metaTitle, seName);
            var item = DBMapping(dbItem);

            if (ManufacturerManager.ManufacturersCacheEnabled)
            {
                NopCache.RemoveByPattern(MANUFACTURERS_PATTERN_KEY);
            }

            return item;
        }

        /// <summary>
        /// Deletes a product manufacturer mapping
        /// </summary>
        /// <param name="productManufacturerId">Product manufacturer mapping identifer</param>
        public static void DeleteProductManufacturer(int productManufacturerId)
        {
            if (productManufacturerId == 0)
                return;

            DBProviderManager<DBManufacturerProvider>.Provider.DeleteProductManufacturer(productManufacturerId);

            if (ManufacturerManager.ManufacturersCacheEnabled || ManufacturerManager.MappingsCacheEnabled)
            {
                NopCache.RemoveByPattern(MANUFACTURERS_PATTERN_KEY);
                NopCache.RemoveByPattern(PRODUCTMANUFACTURERS_PATTERN_KEY);
            }
        }

        /// <summary>
        /// Gets product manufacturer collection
        /// </summary>
        /// <param name="manufacturerId">Manufacturer identifier</param>
        /// <returns>Product manufacturer collection</returns>
        public static ProductManufacturerCollection GetProductManufacturersByManufacturerId(int manufacturerId)
        {
            if (manufacturerId == 0)
                return new ProductManufacturerCollection();

            bool showHidden = NopContext.Current.IsAdmin;
            string key = string.Format(PRODUCTMANUFACTURERS_ALLBYMANUFACTURERID_KEY, showHidden, manufacturerId);
            object obj2 = NopCache.Get(key);
            if (ManufacturerManager.MappingsCacheEnabled && (obj2 != null))
            {
                return (ProductManufacturerCollection)obj2;
            }
            
            var dbCollection = DBProviderManager<DBManufacturerProvider>.Provider.GetProductManufacturersByManufacturerId(manufacturerId, showHidden);
            var productManufacturerCollection = DBMapping(dbCollection);

            if (ManufacturerManager.MappingsCacheEnabled)
            {
                NopCache.Max(key, productManufacturerCollection);
            }
            return productManufacturerCollection;
        }

        /// <summary>
        /// Gets a product manufacturer mapping collection
        /// </summary>
        /// <param name="productId">Product identifier</param>
        /// <returns>Product manufacturer mapping collection</returns>
        public static ProductManufacturerCollection GetProductManufacturersByProductId(int productId)
        {
            if (productId == 0)
                return new ProductManufacturerCollection();

            bool showHidden = NopContext.Current.IsAdmin;
            string key = string.Format(PRODUCTMANUFACTURERS_ALLBYPRODUCTID_KEY, showHidden, productId);
            object obj2 = NopCache.Get(key);
            if (ManufacturerManager.MappingsCacheEnabled && (obj2 != null))
            {
                return (ProductManufacturerCollection)obj2;
            }

            var dbCollection = DBProviderManager<DBManufacturerProvider>.Provider.GetProductManufacturersByProductId(productId, showHidden);
            var productManufacturerCollection = DBMapping(dbCollection);

            if (ManufacturerManager.MappingsCacheEnabled)
            {
                NopCache.Max(key, productManufacturerCollection);
            }
            return productManufacturerCollection;
        }

        /// <summary>
        /// Gets a product manufacturer mapping 
        /// </summary>
        /// <param name="productManufacturerId">Product manufacturer mapping identifier</param>
        /// <returns>Product manufacturer mapping</returns>
        public static ProductManufacturer GetProductManufacturerById(int productManufacturerId)
        {
            if (productManufacturerId == 0)
                return null;

            string key = string.Format(PRODUCTMANUFACTURERS_BY_ID_KEY, productManufacturerId);
            object obj2 = NopCache.Get(key);
            if (ManufacturerManager.MappingsCacheEnabled && (obj2 != null))
            {
                return (ProductManufacturer)obj2;
            }

            var dbItem = DBProviderManager<DBManufacturerProvider>.Provider.GetProductManufacturerById(productManufacturerId);
            var productManufacturer = DBMapping(dbItem);

            if (ManufacturerManager.MappingsCacheEnabled)
            {
                NopCache.Max(key, productManufacturer);
            }
            return productManufacturer;
        }

        /// <summary>
        /// Inserts a product manufacturer mapping
        /// </summary>
        /// <param name="productId">Product identifier</param>
        /// <param name="manufacturerId">Manufacturer identifier</param>
        /// <param name="isFeaturedProduct">A value indicating whether the product is featured</param>
        /// <param name="displayOrder">The display order</param>
        /// <returns>Product manufacturer mapping </returns>
        public static ProductManufacturer InsertProductManufacturer(int productId, 
            int manufacturerId, bool isFeaturedProduct, int displayOrder)
        {
            var dbItem = DBProviderManager<DBManufacturerProvider>.Provider.InsertProductManufacturer(productId,
                manufacturerId, isFeaturedProduct, displayOrder);
            var productManufacturer = DBMapping(dbItem);

            if (ManufacturerManager.ManufacturersCacheEnabled || ManufacturerManager.MappingsCacheEnabled)
            {
                NopCache.RemoveByPattern(MANUFACTURERS_PATTERN_KEY);
                NopCache.RemoveByPattern(PRODUCTMANUFACTURERS_PATTERN_KEY);
            }

            return productManufacturer;
        }

        /// <summary>
        /// Updates the product manufacturer mapping
        /// </summary>
        /// <param name="productManufacturerId">Product manufacturer mapping identifier</param>
        /// <param name="productId">Product identifier</param>
        /// <param name="manufacturerId">Manufacturer identifier</param>
        /// <param name="isFeaturedProduct">A value indicating whether the product is featured</param>
        /// <param name="displayOrder">The display order</param>
        /// <returns>Product manufacturer mapping </returns>
        public static ProductManufacturer UpdateProductManufacturer(int productManufacturerId,
            int productId, int manufacturerId, bool isFeaturedProduct, int displayOrder)
        {
            var dbItem = DBProviderManager<DBManufacturerProvider>.Provider.UpdateProductManufacturer(productManufacturerId,
                productId, manufacturerId, isFeaturedProduct, displayOrder);
            var productManufacturer = DBMapping(dbItem);

            if (ManufacturerManager.ManufacturersCacheEnabled || ManufacturerManager.MappingsCacheEnabled)
            {
                NopCache.RemoveByPattern(MANUFACTURERS_PATTERN_KEY);
                NopCache.RemoveByPattern(PRODUCTMANUFACTURERS_PATTERN_KEY);
            }

            return productManufacturer;
        }
        #endregion

        #region Property

        /// <summary>
        /// Gets a value indicating whether manufacturers cache is enabled
        /// </summary>
        public static bool ManufacturersCacheEnabled
        {
            get
            {
                return SettingManager.GetSettingValueBoolean("Cache.ManufacturerManager.ManufacturersCacheEnabled");
            }
        }

        /// <summary>
        /// Gets a value indicating whether mappings cache is enabled
        /// </summary>
        public static bool MappingsCacheEnabled
        {
            get
            {
                return SettingManager.GetSettingValueBoolean("Cache.ManufacturerManager.MappingsCacheEnabled");
            }
        }
        #endregion
    }
}
