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
using NopSolutions.NopCommerce.DataAccess.Products.Attributes;

namespace NopSolutions.NopCommerce.BusinessLogic.Products.Attributes
{
    /// <summary>
    /// Product attribute manager
    /// </summary>
    public partial class ProductAttributeManager
    {
        #region Constants
        private const string PRODUCTATTRIBUTES_ALL_KEY = "Nop.productattribute.all-{0}";
        private const string PRODUCTATTRIBUTES_BY_ID_KEY = "Nop.productattribute.id-{0}-{1}";
        private const string PRODUCTVARIANTATTRIBUTES_ALL_KEY = "Nop.productvariantattribute.all-{0}";
        private const string PRODUCTVARIANTATTRIBUTES_BY_ID_KEY = "Nop.productvariantattribute.id-{0}";
        private const string PRODUCTVARIANTATTRIBUTEVALUES_ALL_KEY = "Nop.productvariantattributevalue.all-{0}-{1}";
        private const string PRODUCTVARIANTATTRIBUTEVALUES_BY_ID_KEY = "Nop.productvariantattributevalue.id-{0}-{1}";
        private const string PRODUCTATTRIBUTES_PATTERN_KEY = "Nop.productattribute.";
        private const string PRODUCTVARIANTATTRIBUTES_PATTERN_KEY = "Nop.productvariantattribute.";
        private const string PRODUCTVARIANTATTRIBUTEVALUES_PATTERN_KEY = "Nop.productvariantattributevalue.";
        #endregion

        #region Utilities
        private static ProductAttributeCollection DBMapping(DBProductAttributeCollection dbCollection)
        {
            if (dbCollection == null)
                return null;

            var collection = new ProductAttributeCollection();
            foreach (var dbItem in dbCollection)
            {
                var item = DBMapping(dbItem);
                collection.Add(item);
            }

            return collection;
        }

        private static ProductAttribute DBMapping(DBProductAttribute dbItem)
        {
            if (dbItem == null)
                return null;

            var item = new ProductAttribute();
            item.ProductAttributeID = dbItem.ProductAttributeID;
            item.Name = dbItem.Name;
            item.Description = dbItem.Description;

            return item;
        }

        private static ProductAttributeLocalized DBMapping(DBProductAttributeLocalized dbItem)
        {
            if (dbItem == null)
                return null;

            var item = new ProductAttributeLocalized();
            item.ProductAttributeLocalizedID = dbItem.ProductAttributeLocalizedID;
            item.ProductAttributeID = dbItem.ProductAttributeID;
            item.LanguageID = dbItem.LanguageID;
            item.Name = dbItem.Name;
            item.Description = dbItem.Description;

            return item;
        }

        private static ProductVariantAttributeCollection DBMapping(DBProductVariantAttributeCollection dbCollection)
        {
            if (dbCollection == null)
                return null;

            var collection = new ProductVariantAttributeCollection();
            foreach (var dbItem in dbCollection)
            {
                var item = DBMapping(dbItem);
                collection.Add(item);
            }

            return collection;
        }

        private static ProductVariantAttribute DBMapping(DBProductVariantAttribute dbItem)
        {
            if (dbItem == null)
                return null;

            var item = new ProductVariantAttribute();
            item.ProductVariantAttributeID = dbItem.ProductVariantAttributeID;
            item.ProductVariantID = dbItem.ProductVariantID;
            item.ProductAttributeID = dbItem.ProductAttributeID;
            item.TextPrompt = dbItem.TextPrompt;
            item.IsRequired = dbItem.IsRequired;
            item.AttributeControlTypeID = dbItem.AttributeControlTypeID;
            item.DisplayOrder = dbItem.DisplayOrder;

            return item;
        }

        private static ProductVariantAttributeValueCollection DBMapping(DBProductVariantAttributeValueCollection dbCollection)
        {
            if (dbCollection == null)
                return null;

            var collection = new ProductVariantAttributeValueCollection();
            foreach (var dbItem in dbCollection)
            {
                var item = DBMapping(dbItem);
                collection.Add(item);
            }

            return collection;
        }

        private static ProductVariantAttributeValue DBMapping(DBProductVariantAttributeValue dbItem)
        {
            if (dbItem == null)
                return null;

            var item = new ProductVariantAttributeValue();
            item.ProductVariantAttributeValueID = dbItem.ProductVariantAttributeValueID;
            item.ProductVariantAttributeID = dbItem.ProductVariantAttributeID;
            item.Name = dbItem.Name;
            item.PriceAdjustment = dbItem.PriceAdjustment;
            item.WeightAdjustment = dbItem.WeightAdjustment;
            item.IsPreSelected = dbItem.IsPreSelected;
            item.DisplayOrder = dbItem.DisplayOrder;

            return item;
        }

        private static ProductVariantAttributeValueLocalized DBMapping(DBProductVariantAttributeValueLocalized dbItem)
        {
            if (dbItem == null)
                return null;

            var item = new ProductVariantAttributeValueLocalized();
            item.ProductVariantAttributeValueLocalizedID = dbItem.ProductVariantAttributeValueLocalizedID;
            item.ProductVariantAttributeValueID = dbItem.ProductVariantAttributeValueID;
            item.LanguageID = dbItem.LanguageID;
            item.Name = dbItem.Name;

            return item;
        }

        private static ProductVariantAttributeCombinationCollection DBMapping(DBProductVariantAttributeCombinationCollection dbCollection)
        {
            if (dbCollection == null)
                return null;

            var collection = new ProductVariantAttributeCombinationCollection();
            foreach (var dbItem in dbCollection)
            {
                var item = DBMapping(dbItem);
                collection.Add(item);
            }

            return collection;
        }

        private static ProductVariantAttributeCombination DBMapping(DBProductVariantAttributeCombination dbItem)
        {
            if (dbItem == null)
                return null;

            var item = new ProductVariantAttributeCombination();
            item.ProductVariantAttributeCombinationID = dbItem.ProductVariantAttributeCombinationID;
            item.ProductVariantID = dbItem.ProductVariantID;
            item.AttributesXML = dbItem.AttributesXML;
            item.StockQuantity = dbItem.StockQuantity;
            item.AllowOutOfStockOrders = dbItem.AllowOutOfStockOrders;

            return item;
        }
       
        #endregion

        #region Methods

        #region Product attributes

        /// <summary>
        /// Deletes a product attribute
        /// </summary>
        /// <param name="ProductAttributeID">Product attribute identifier</param>
        public static void DeleteProductAttribute(int ProductAttributeID)
        {
            DBProviderManager<DBProductAttributeProvider>.Provider.DeleteProductAttribute(ProductAttributeID);
            if (ProductAttributeManager.CacheEnabled)
            {
                NopCache.RemoveByPattern(PRODUCTATTRIBUTES_PATTERN_KEY);
                NopCache.RemoveByPattern(PRODUCTVARIANTATTRIBUTES_PATTERN_KEY);
                NopCache.RemoveByPattern(PRODUCTVARIANTATTRIBUTEVALUES_PATTERN_KEY);
            }
        }

        /// <summary>
        /// Gets all product attributes
        /// </summary>
        /// <returns>Product attribute collection</returns>
        public static ProductAttributeCollection GetAllProductAttributes()
        {
            int languageId = 0;
            if (NopContext.Current != null)
                languageId = NopContext.Current.WorkingLanguage.LanguageID;
            return GetAllProductAttributes(languageId);
        }

        /// <summary>
        /// Gets all product attributes
        /// </summary>
        /// <param name="LanguageID">Language identifier</param>
        /// <returns>Product attribute collection</returns>
        public static ProductAttributeCollection GetAllProductAttributes(int LanguageID)
        {
            string key = string.Format(PRODUCTATTRIBUTES_ALL_KEY, LanguageID);
            object obj2 = NopCache.Get(key);
            if (ProductAttributeManager.CacheEnabled && (obj2 != null))
            {
                return (ProductAttributeCollection)obj2;
            }

            var dbCollection = DBProviderManager<DBProductAttributeProvider>.Provider.GetAllProductAttributes(LanguageID);
            var productAttributes = DBMapping(dbCollection);

            if (ProductAttributeManager.CacheEnabled)
            {
                NopCache.Max(key, productAttributes);
            }
            return productAttributes;
        }

        /// <summary>
        /// Gets a product attribute 
        /// </summary>
        /// <param name="ProductAttributeID">Product attribute identifier</param>
        /// <returns>Product attribute </returns>
        public static ProductAttribute GetProductAttributeByID(int ProductAttributeID)
        {
            int languageId = 0;
            if (NopContext.Current != null)
                languageId = NopContext.Current.WorkingLanguage.LanguageID;
            return GetProductAttributeByID(ProductAttributeID, languageId);
        }

        /// <summary>
        /// Gets a product attribute 
        /// </summary>
        /// <param name="ProductAttributeID">Product attribute identifier</param>
        /// <param name="LanguageID">Language identifier</param>
        /// <returns>Product attribute </returns>
        public static ProductAttribute GetProductAttributeByID(int ProductAttributeID, int LanguageID)
        {
            if (ProductAttributeID == 0)
                return null;

            string key = string.Format(PRODUCTATTRIBUTES_BY_ID_KEY, ProductAttributeID, LanguageID);
            object obj2 = NopCache.Get(key);
            if (ProductAttributeManager.CacheEnabled && (obj2 != null))
            {
                return (ProductAttribute)obj2;
            }

            var dbItem = DBProviderManager<DBProductAttributeProvider>.Provider.GetProductAttributeByID(ProductAttributeID, LanguageID);
            var productAttribute = DBMapping(dbItem);

            if (ProductAttributeManager.CacheEnabled)
            {
                NopCache.Max(key, productAttribute);
            }
            return productAttribute;
        }

        /// <summary>
        /// Inserts a product attribute
        /// </summary>
        /// <param name="Name">The name</param>
        /// <param name="Description">The description</param>
        /// <returns>Product attribute </returns>
        public static ProductAttribute InsertProductAttribute(string Name, string Description)
        {
            var dbItem = DBProviderManager<DBProductAttributeProvider>.Provider.InsertProductAttribute(Name, Description);
            var productAttribute = DBMapping(dbItem);
            if (ProductAttributeManager.CacheEnabled)
            {
                NopCache.RemoveByPattern(PRODUCTATTRIBUTES_PATTERN_KEY);
                NopCache.RemoveByPattern(PRODUCTVARIANTATTRIBUTES_PATTERN_KEY);
                NopCache.RemoveByPattern(PRODUCTVARIANTATTRIBUTEVALUES_PATTERN_KEY);
            }
            return productAttribute;
        }

        /// <summary>
        /// Updates the product attribute
        /// </summary>
        /// <param name="ProductAttributeID">Product attribute identifier</param>
        /// <param name="Name">The name</param>
        /// <param name="Description">The description</param>
        /// <returns>Product attribute </returns>
        public static ProductAttribute UpdateProductAttribute(int ProductAttributeID, string Name,
            string Description)
        {
            var dbItem = DBProviderManager<DBProductAttributeProvider>.Provider.UpdateProductAttribute(ProductAttributeID,
                Name, Description);
            var productAttribute = DBMapping(dbItem);
            if (ProductAttributeManager.CacheEnabled)
            {
                NopCache.RemoveByPattern(PRODUCTATTRIBUTES_PATTERN_KEY);
                NopCache.RemoveByPattern(PRODUCTVARIANTATTRIBUTES_PATTERN_KEY);
                NopCache.RemoveByPattern(PRODUCTVARIANTATTRIBUTEVALUES_PATTERN_KEY);
            }

            return productAttribute;
        }

        /// <summary>
        /// Gets localized product attribute by id
        /// </summary>
        /// <param name="ProductAttributeLocalizedID">Localized product attribute identifier</param>
        /// <returns>Product attribute content</returns>
        public static ProductAttributeLocalized GetProductAttributeLocalizedByID(int ProductAttributeLocalizedID)
        {
            if (ProductAttributeLocalizedID == 0)
                return null;

            var dbItem = DBProviderManager<DBProductAttributeProvider>.Provider.GetProductAttributeLocalizedByID(ProductAttributeLocalizedID);
            var item = DBMapping(dbItem);
            return item;
        }

        /// <summary>
        /// Gets localized product attribute by product attribute id and language id
        /// </summary>
        /// <param name="ProductAttributeID">Product attribute identifier</param>
        /// <param name="LanguageID">Language identifier</param>
        /// <returns>Product attribute content</returns>
        public static ProductAttributeLocalized GetProductAttributeLocalizedByProductAttributeIDAndLanguageID(int ProductAttributeID, int LanguageID)
        {
            if (ProductAttributeID == 0 || LanguageID == 0)
                return null;

            var dbItem = DBProviderManager<DBProductAttributeProvider>.Provider.GetProductAttributeLocalizedByProductAttributeIDAndLanguageID(ProductAttributeID, LanguageID);
            var item = DBMapping(dbItem);
            return item;
        }

        /// <summary>
        /// Inserts a localized product attribute
        /// </summary>
        /// <param name="ProductAttributeID">Product attribute identifier</param>
        /// <param name="LanguageID">Language identifier</param>
        /// <param name="Name">Name text</param>
        /// <param name="Description">Description text</param>
        /// <returns>Localized product attribute</returns>
        public static ProductAttributeLocalized InsertProductAttributeLocalized(int ProductAttributeID,
            int LanguageID, string Name, string Description)
        {
            var dbItem = DBProviderManager<DBProductAttributeProvider>.Provider.InsertProductAttributeLocalized(ProductAttributeID,
            LanguageID, Name, Description);
            var item = DBMapping(dbItem);
            return item;
        }

        /// <summary>
        /// Update a localized product attribute
        /// </summary>
        /// <param name="ProductAttributeLocalizedID">Localized product attribute identifier</param>
        /// <param name="ProductAttributeID">Product attribute identifier</param>
        /// <param name="LanguageID">Language identifier</param>
        /// <param name="Name">Name text</param>
        /// <param name="Description">Description text</param>
        /// <returns>DBProductAttributeLocalized</returns>
        public static ProductAttributeLocalized UpdateProductAttributeLocalized(int ProductAttributeLocalizedID,
            int ProductAttributeID, int LanguageID, string Name, string Description)
        {
            var dbItem = DBProviderManager<DBProductAttributeProvider>.Provider.UpdateProductAttributeLocalized(ProductAttributeLocalizedID,
                ProductAttributeID, LanguageID, Name, Description);
            var item = DBMapping(dbItem);
            return item;
        }
        
        #endregion

        #region Product variant attributes mappings (ProductVariantAttribute)

        /// <summary>
        /// Deletes a product variant attribute mapping
        /// </summary>
        /// <param name="ProductVariantAttributeID">Product variant attribute mapping identifier</param>
        public static void DeleteProductVariantAttribute(int ProductVariantAttributeID)
        {
            DBProviderManager<DBProductAttributeProvider>.Provider.DeleteProductVariantAttribute(ProductVariantAttributeID);
            if (ProductAttributeManager.CacheEnabled)
            {
                NopCache.RemoveByPattern(PRODUCTATTRIBUTES_PATTERN_KEY);
                NopCache.RemoveByPattern(PRODUCTVARIANTATTRIBUTES_PATTERN_KEY);
                NopCache.RemoveByPattern(PRODUCTVARIANTATTRIBUTEVALUES_PATTERN_KEY);
            }
        }

        /// <summary>
        /// Gets product variant attribute mappings by product identifier
        /// </summary>
        /// <param name="ProductVariantID">The product variant identifier</param>
        /// <returns>Product variant attribute mapping collection</returns>
        public static ProductVariantAttributeCollection GetProductVariantAttributesByProductVariantID(int ProductVariantID)
        {
            string key = string.Format(PRODUCTVARIANTATTRIBUTES_ALL_KEY, ProductVariantID);
            object obj2 = NopCache.Get(key);
            if (ProductAttributeManager.CacheEnabled && (obj2 != null))
            {
                return (ProductVariantAttributeCollection)obj2;
            }

            var dbCollection = DBProviderManager<DBProductAttributeProvider>.Provider.GetProductVariantAttributesByProductVariantID(ProductVariantID);
            var productVariantAttributes = DBMapping(dbCollection);

            if (ProductAttributeManager.CacheEnabled)
            {
                NopCache.Max(key, productVariantAttributes);
            }
            return productVariantAttributes;
        }

        /// <summary>
        /// Gets a product variant attribute mapping
        /// </summary>
        /// <param name="ProductVariantAttributeID">Product variant attribute mapping identifier</param>
        /// <returns>Product variant attribute mapping</returns>
        public static ProductVariantAttribute GetProductVariantAttributeByID(int ProductVariantAttributeID)
        {
            if (ProductVariantAttributeID == 0)
                return null;

            string key = string.Format(PRODUCTVARIANTATTRIBUTES_BY_ID_KEY, ProductVariantAttributeID);
            object obj2 = NopCache.Get(key);
            if (ProductAttributeManager.CacheEnabled && (obj2 != null))
            {
                return (ProductVariantAttribute)obj2;
            }

            var dbItem = DBProviderManager<DBProductAttributeProvider>.Provider.GetProductVariantAttributeByID(ProductVariantAttributeID);
            var productVariantAttribute = DBMapping(dbItem);

            if (ProductAttributeManager.CacheEnabled)
            {
                NopCache.Max(key, productVariantAttribute);
            }
            return productVariantAttribute;
        }

        /// <summary>
        /// Inserts a product variant attribute mapping
        /// </summary>
        /// <param name="ProductVariantID">The product variant identifier</param>
        /// <param name="ProductAttributeID">The product attribute identifier</param>
        /// <param name="TextPrompt">The text prompt</param>
        /// <param name="IsRequired">The value indicating whether the entity is required</param>
        /// <param name="AttributeControlType">The attribute control type</param>
        /// <param name="DisplayOrder">The display order</param>
        /// <returns>Product variant attribute mapping</returns>
        public static ProductVariantAttribute InsertProductVariantAttribute(int ProductVariantID,
            int ProductAttributeID, string TextPrompt, bool IsRequired, AttributeControlTypeEnum AttributeControlType, int DisplayOrder)
        {
            var dbItem = DBProviderManager<DBProductAttributeProvider>.Provider.InsertProductVariantAttribute(ProductVariantID,
                ProductAttributeID, TextPrompt, IsRequired, (int)AttributeControlType, DisplayOrder);
            var productVariantAttribute = DBMapping(dbItem);

            if (ProductAttributeManager.CacheEnabled)
            {
                NopCache.RemoveByPattern(PRODUCTATTRIBUTES_PATTERN_KEY);
                NopCache.RemoveByPattern(PRODUCTVARIANTATTRIBUTES_PATTERN_KEY);
                NopCache.RemoveByPattern(PRODUCTVARIANTATTRIBUTEVALUES_PATTERN_KEY);
            }

            return productVariantAttribute;
        }

        /// <summary>
        /// Updates the product variant attribute mapping
        /// </summary>
        /// <param name="ProductVariantAttributeID">The product variant attribute mapping identifier</param>
        /// <param name="ProductVariantID">The product variant identifier</param>
        /// <param name="ProductAttributeID">The product attribute identifier</param>
        /// <param name="TextPrompt">The text prompt</param>
        /// <param name="IsRequired">The value indicating whether the entity is required</param>
        /// <param name="AttributeControlType">The attribute control type</param>
        /// <param name="DisplayOrder">The display order</param>
        /// <returns>Product variant attribute mapping</returns>
        public static ProductVariantAttribute UpdateProductVariantAttribute(int ProductVariantAttributeID, int ProductVariantID,
            int ProductAttributeID, string TextPrompt, bool IsRequired, AttributeControlTypeEnum AttributeControlType, int DisplayOrder)
        {
            var dbItem = DBProviderManager<DBProductAttributeProvider>.Provider.UpdateProductVariantAttribute(ProductVariantAttributeID,
                ProductVariantID, ProductAttributeID, TextPrompt, IsRequired, (int)AttributeControlType, DisplayOrder);
            var productVariantAttribute = DBMapping(dbItem);

            if (ProductAttributeManager.CacheEnabled)
            {
                NopCache.RemoveByPattern(PRODUCTATTRIBUTES_PATTERN_KEY);
                NopCache.RemoveByPattern(PRODUCTVARIANTATTRIBUTES_PATTERN_KEY);
                NopCache.RemoveByPattern(PRODUCTVARIANTATTRIBUTEVALUES_PATTERN_KEY);
            }

            return productVariantAttribute;
        }

        #endregion

        #region Product variant attribute values  (ProductVariantAttributeValue)

        /// <summary>
        /// Deletes a product variant attribute value
        /// </summary>
        /// <param name="ProductVariantAttributeValueID">Product variant attribute value identifier</param>
        public static void DeleteProductVariantAttributeValue(int ProductVariantAttributeValueID)
        {
            DBProviderManager<DBProductAttributeProvider>.Provider.DeleteProductVariantAttributeValue(ProductVariantAttributeValueID);
            if (ProductAttributeManager.CacheEnabled)
            {
                NopCache.RemoveByPattern(PRODUCTATTRIBUTES_PATTERN_KEY);
                NopCache.RemoveByPattern(PRODUCTVARIANTATTRIBUTES_PATTERN_KEY);
                NopCache.RemoveByPattern(PRODUCTVARIANTATTRIBUTEVALUES_PATTERN_KEY);
            }
        }

        /// <summary>
        /// Gets product variant attribute values by product identifier
        /// </summary>
        /// <param name="ProductVariantAttributeID">The product variant attribute mapping identifier</param>
        /// <returns>Product variant attribute mapping collection</returns>
        public static ProductVariantAttributeValueCollection GetProductVariantAttributeValues(int ProductVariantAttributeID)
        {
            int languageId = 0;
            if (NopContext.Current != null)
                languageId = NopContext.Current.WorkingLanguage.LanguageID;
            return GetProductVariantAttributeValues(ProductVariantAttributeID, languageId);
        }

        /// <summary>
        /// Gets product variant attribute values by product identifier
        /// </summary>
        /// <param name="ProductVariantAttributeID">The product variant attribute mapping identifier</param>
        /// <param name="LanguageID">Language identifier</param>
        /// <returns>Product variant attribute mapping collection</returns>
        public static ProductVariantAttributeValueCollection GetProductVariantAttributeValues(int ProductVariantAttributeID, int LanguageID)
        {
            string key = string.Format(PRODUCTVARIANTATTRIBUTEVALUES_ALL_KEY, ProductVariantAttributeID, LanguageID);
            object obj2 = NopCache.Get(key);
            if (ProductAttributeManager.CacheEnabled && (obj2 != null))
            {
                return (ProductVariantAttributeValueCollection)obj2;
            }

            var dbCollection = DBProviderManager<DBProductAttributeProvider>.Provider.GetProductVariantAttributeValues(ProductVariantAttributeID, LanguageID);
            var productVariantAttributeValues = DBMapping(dbCollection);

            if (ProductAttributeManager.CacheEnabled)
            {
                NopCache.Max(key, productVariantAttributeValues);
            }
            return productVariantAttributeValues;
        }

        /// <summary>
        /// Gets a product variant attribute value
        /// </summary>
        /// <param name="ProductVariantAttributeValueID">Product variant attribute value identifier</param>
        /// <returns>Product variant attribute value</returns>
        public static ProductVariantAttributeValue GetProductVariantAttributeValueByID(int ProductVariantAttributeValueID)
        {
            int languageId = 0;
            if (NopContext.Current != null)
                languageId = NopContext.Current.WorkingLanguage.LanguageID;
            return GetProductVariantAttributeValueByID(ProductVariantAttributeValueID, languageId);
        }

        /// <summary>
        /// Gets a product variant attribute value
        /// </summary>
        /// <param name="ProductVariantAttributeValueID">Product variant attribute value identifier</param>
        /// <param name="LanguageID">Language identifier</param>
        /// <returns>Product variant attribute value</returns>
        public static ProductVariantAttributeValue GetProductVariantAttributeValueByID(int ProductVariantAttributeValueID, int LanguageID)
        {
            if (ProductVariantAttributeValueID == 0)
                return null;

            string key = string.Format(PRODUCTVARIANTATTRIBUTEVALUES_BY_ID_KEY, ProductVariantAttributeValueID ,LanguageID);
            object obj2 = NopCache.Get(key);
            if (ProductAttributeManager.CacheEnabled && (obj2 != null))
            {
                return (ProductVariantAttributeValue)obj2;
            }

            var dbItem = DBProviderManager<DBProductAttributeProvider>.Provider.GetProductVariantAttributeValueByID(ProductVariantAttributeValueID, LanguageID);
            var productVariantAttributeValue = DBMapping(dbItem);
            if (ProductAttributeManager.CacheEnabled)
            {
                NopCache.Max(key, productVariantAttributeValue);
            }
            return productVariantAttributeValue;
        }

        /// <summary>
        /// Inserts a product variant attribute value
        /// </summary>
        /// <param name="ProductVariantAttributeID">The product variant attribute mapping identifier</param>
        /// <param name="Name">The product variant attribute name</param>
        /// <param name="PriceAdjustment">The price adjustment</param>
        /// <param name="WeightAdjustment">The weight adjustment</param>
        /// <param name="IsPreSelected">The value indicating whether the value is pre-selected</param>
        /// <param name="DisplayOrder">The display order</param>
        /// <returns>Product variant attribute value</returns>
        public static ProductVariantAttributeValue InsertProductVariantAttributeValue(int ProductVariantAttributeID,
            string Name, decimal PriceAdjustment, decimal WeightAdjustment,
            bool IsPreSelected, int DisplayOrder)
        {
            var dbItem = DBProviderManager<DBProductAttributeProvider>.Provider.InsertProductVariantAttributeValue(ProductVariantAttributeID,
                Name, PriceAdjustment, WeightAdjustment, IsPreSelected, DisplayOrder);
            var productVariantAttributeValue = DBMapping(dbItem);

            if (ProductAttributeManager.CacheEnabled)
            {
                NopCache.RemoveByPattern(PRODUCTATTRIBUTES_PATTERN_KEY);
                NopCache.RemoveByPattern(PRODUCTVARIANTATTRIBUTES_PATTERN_KEY);
                NopCache.RemoveByPattern(PRODUCTVARIANTATTRIBUTEVALUES_PATTERN_KEY);
            }

            return productVariantAttributeValue;
        }

        /// <summary>
        /// Updates the product variant attribute value
        /// </summary>
        /// <param name="ProductVariantAttributeValueID">The product variant attribute value identifier</param>
        /// <param name="ProductVariantAttributeID">The product variant attribute mapping identifier</param>
        /// <param name="Name">The product variant attribute name</param>
        /// <param name="PriceAdjustment">The price adjustment</param>
        /// <param name="WeightAdjustment">The weight adjustment</param>
        /// <param name="IsPreSelected">The value indicating whether the value is pre-selected</param>
        /// <param name="DisplayOrder">The display order</param>
        /// <returns>Product variant attribute value</returns>
        public static ProductVariantAttributeValue UpdateProductVariantAttributeValue(int ProductVariantAttributeValueID,
            int ProductVariantAttributeID, string Name, decimal PriceAdjustment,
            decimal WeightAdjustment, bool IsPreSelected, int DisplayOrder)
        {
            var dbItem = DBProviderManager<DBProductAttributeProvider>.Provider.UpdateProductVariantAttributeValue(ProductVariantAttributeValueID,
                ProductVariantAttributeID, Name, PriceAdjustment, WeightAdjustment, IsPreSelected, DisplayOrder);
            var productVariantAttributeValue = DBMapping(dbItem);

            if (ProductAttributeManager.CacheEnabled)
            {
                NopCache.RemoveByPattern(PRODUCTATTRIBUTES_PATTERN_KEY);
                NopCache.RemoveByPattern(PRODUCTVARIANTATTRIBUTES_PATTERN_KEY);
                NopCache.RemoveByPattern(PRODUCTVARIANTATTRIBUTEVALUES_PATTERN_KEY);
            }

            return productVariantAttributeValue;
        }

        /// <summary>
        /// Gets localized product variant attribute value by id
        /// </summary>
        /// <param name="ProductVariantAttributeValueLocalizedID">Localized product variant attribute value identifier</param>
        /// <returns>Localized product variant attribute value</returns>
        public static ProductVariantAttributeValueLocalized GetProductVariantAttributeValueLocalizedByID(int ProductVariantAttributeValueLocalizedID)
        {
            if (ProductVariantAttributeValueLocalizedID == 0)
                return null;

            var dbItem = DBProviderManager<DBProductAttributeProvider>.Provider.GetProductVariantAttributeValueLocalizedByID(ProductVariantAttributeValueLocalizedID);
            var item = DBMapping(dbItem);
            return item;
        }

        /// <summary>
        /// Gets localized product variant attribute value by product variant attribute value id and language id
        /// </summary>
        /// <param name="ProductVariantAttributeValueID">Product variant attribute value identifier</param>
        /// <param name="LanguageID">Language identifier</param>
        /// <returns>Localized product variant attribute value</returns>
        public static ProductVariantAttributeValueLocalized GetProductVariantAttributeValueLocalizedByProductVariantAttributeValueIDAndLanguageID(int ProductVariantAttributeValueID, int LanguageID)
        {
            if (ProductVariantAttributeValueID == 0 || LanguageID == 0)
                return null;

            var dbItem = DBProviderManager<DBProductAttributeProvider>.Provider.GetProductVariantAttributeValueLocalizedByProductVariantAttributeValueIDAndLanguageID(ProductVariantAttributeValueID, LanguageID);
            var item = DBMapping(dbItem);
            return item;
        }

        /// <summary>
        /// Inserts a localized product variant attribute value
        /// </summary>
        /// <param name="ProductVariantAttributeValueID">Product variant attribute value identifier</param>
        /// <param name="LanguageID">Language identifier</param>
        /// <param name="Name">Name text</param>
        /// <returns>Localized product variant attribute value</returns>
        public static ProductVariantAttributeValueLocalized InsertProductVariantAttributeValueLocalized(int ProductVariantAttributeValueID,
            int LanguageID, string Name)
        {
            var dbItem = DBProviderManager<DBProductAttributeProvider>.Provider.InsertProductVariantAttributeValueLocalized(ProductVariantAttributeValueID,
            LanguageID, Name);
            var item = DBMapping(dbItem);
            return item;
        }

        /// <summary>
        /// Update a localized product variant attribute value
        /// </summary>
        /// <param name="ProductVariantAttributeValueLocalizedID">Localized product variant attribute value identifier</param>
        /// <param name="ProductVariantAttributeValueID">Product variant attribute value identifier</param>
        /// <param name="LanguageID">Language identifier</param>
        /// <param name="Name">Name text</param>
        /// <returns>Localized product variant attribute value</returns>
        public static ProductVariantAttributeValueLocalized UpdateProductVariantAttributeValueLocalized(int ProductVariantAttributeValueLocalizedID,
            int ProductVariantAttributeValueID, int LanguageID, string Name)
        {
            var dbItem = DBProviderManager<DBProductAttributeProvider>.Provider.UpdateProductVariantAttributeValueLocalized(ProductVariantAttributeValueLocalizedID,
                ProductVariantAttributeValueID, LanguageID, Name);
            var item = DBMapping(dbItem);
            return item;
        }
        
        #endregion

        #region Product variant attribute compinations (ProductVariantAttributeCombination)

        /// <summary>
        /// Deletes a product variant attribute combination
        /// </summary>
        /// <param name="ProductVariantAttributeCombinationID">Product variant attribute combination identifier</param>
        public static void DeleteProductVariantAttributeCombination(int ProductVariantAttributeCombinationID)
        {
            DBProviderManager<DBProductAttributeProvider>.Provider.DeleteProductVariantAttributeCombination(ProductVariantAttributeCombinationID);
        }

        /// <summary>
        /// Gets all product variant attribute combinations
        /// </summary>
        /// <param name="ProductVariantID">Product variant identifier</param>
        /// <returns>Product variant attribute combination collection</returns>
        public static ProductVariantAttributeCombinationCollection GetAllProductVariantAttributeCombinations(int ProductVariantID)
        {
            if (ProductVariantID == 0)
                return new ProductVariantAttributeCombinationCollection();

            var dbCollection = DBProviderManager<DBProductAttributeProvider>.Provider.GetAllProductVariantAttributeCombinations(ProductVariantID);
            var combination = DBMapping(dbCollection);
            return combination;
        }

        /// <summary>
        /// Gets a product variant attribute combination
        /// </summary>
        /// <param name="ProductVariantAttributeCombinationID">Product variant attribute combination identifier</param>
        /// <returns>Product variant attribute combination</returns>
        public static ProductVariantAttributeCombination GetProductVariantAttributeCombinationByID(int ProductVariantAttributeCombinationID)
        {
            if (ProductVariantAttributeCombinationID == 0)
                return null;

            var dbItem = DBProviderManager<DBProductAttributeProvider>.Provider.GetProductVariantAttributeCombinationByID(ProductVariantAttributeCombinationID);
            var item = DBMapping(dbItem);
            return item;
        }

        /// <summary>
        /// Inserts a product variant attribute combination
        /// </summary>
        /// <param name="ProductVariantID">The product variant identifier</param>
        /// <param name="AttributesXML">The attributes</param>
        /// <param name="StockQuantity">The stock quantity</param>
        /// <param name="AllowOutOfStockOrders">The value indicating whether to allow orders when out of stock</param>
        /// <returns>Product variant attribute combination</returns>
        public static ProductVariantAttributeCombination InsertProductVariantAttributeCombination(int ProductVariantID,
            string AttributesXML,
            int StockQuantity,
            bool AllowOutOfStockOrders)
        {
            var dbItem = DBProviderManager<DBProductAttributeProvider>.Provider.InsertProductVariantAttributeCombination(ProductVariantID,
                AttributesXML, StockQuantity, AllowOutOfStockOrders);
            var item = DBMapping(dbItem);
            return item;
        }

        /// <summary>
        /// Updates a product variant attribute combination
        /// </summary>
        /// <param name="ProductVariantAttributeCombinationID">Product variant attribute combination identifier</param>
        /// <param name="ProductVariantID">The product variant identifier</param>
        /// <param name="AttributesXML">The attributes</param>
        /// <param name="StockQuantity">The stock quantity</param>
        /// <param name="AllowOutOfStockOrders">The value indicating whether to allow orders when out of stock</param>
        /// <returns>Product variant attribute combination</returns>
        public static ProductVariantAttributeCombination UpdateProductVariantAttributeCombination(int ProductVariantAttributeCombinationID,
            int ProductVariantID,
            string AttributesXML,
            int StockQuantity,
            bool AllowOutOfStockOrders)
        {
            var dbItem = DBProviderManager<DBProductAttributeProvider>.Provider.UpdateProductVariantAttributeCombination(ProductVariantAttributeCombinationID,
                 ProductVariantID, AttributesXML, StockQuantity, AllowOutOfStockOrders);
            var item = DBMapping(dbItem);
            return item;
        }

        /// <summary>
        /// Finds a product variant attribute combination by attributes stored in XML 
        /// </summary>
        /// <param name="ProductVariantID">Product variant identifier</param>
        /// <param name="AttributesXML">Attributes in XML format</param>
        /// <returns>Found product variant attribute combination</returns>
        public static ProductVariantAttributeCombination FindProductVariantAttributeCombination(int ProductVariantID, string AttributesXML)
        {
            //existing combinations
            var combinations = ProductAttributeManager.GetAllProductVariantAttributeCombinations(ProductVariantID);
            if (combinations.Count == 0)
                return null;

            foreach (var combination in combinations)
            {
                bool attributesEqual = ProductAttributeHelper.AreProductAttributesEqual(combination.AttributesXML, AttributesXML);
                if (attributesEqual)
                {
                    return combination;
                }
            }

            return null;
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
                return SettingManager.GetSettingValueBoolean("Cache.ProductAttributeManager.CacheEnabled");
            }
        }
        #endregion
    }
}
