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
        private static List<ProductAttribute> DBMapping(DBProductAttributeCollection dbCollection)
        {
            if (dbCollection == null)
                return null;

            var collection = new List<ProductAttribute>();
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
            item.ProductAttributeId = dbItem.ProductAttributeId;
            item.Name = dbItem.Name;
            item.Description = dbItem.Description;

            return item;
        }

        private static ProductAttributeLocalized DBMapping(DBProductAttributeLocalized dbItem)
        {
            if (dbItem == null)
                return null;

            var item = new ProductAttributeLocalized();
            item.ProductAttributeLocalizedId = dbItem.ProductAttributeLocalizedId;
            item.ProductAttributeId = dbItem.ProductAttributeId;
            item.LanguageId = dbItem.LanguageId;
            item.Name = dbItem.Name;
            item.Description = dbItem.Description;

            return item;
        }

        private static List<ProductVariantAttributeValue> DBMapping(DBProductVariantAttributeValueCollection dbCollection)
        {
            if (dbCollection == null)
                return null;

            var collection = new List<ProductVariantAttributeValue>();
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
            item.ProductVariantAttributeValueId = dbItem.ProductVariantAttributeValueId;
            item.ProductVariantAttributeId = dbItem.ProductVariantAttributeId;
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
            item.ProductVariantAttributeValueLocalizedId = dbItem.ProductVariantAttributeValueLocalizedId;
            item.ProductVariantAttributeValueId = dbItem.ProductVariantAttributeValueId;
            item.LanguageId = dbItem.LanguageId;
            item.Name = dbItem.Name;

            return item;
        }

        #endregion

        #region Methods

        #region Product attributes

        /// <summary>
        /// Deletes a product attribute
        /// </summary>
        /// <param name="productAttributeId">Product attribute identifier</param>
        public static void DeleteProductAttribute(int productAttributeId)
        {
            DBProviderManager<DBProductAttributeProvider>.Provider.DeleteProductAttribute(productAttributeId);
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
        public static List<ProductAttribute> GetAllProductAttributes()
        {
            int languageId = 0;
            if (NopContext.Current != null)
                languageId = NopContext.Current.WorkingLanguage.LanguageId;
            return GetAllProductAttributes(languageId);
        }

        /// <summary>
        /// Gets all product attributes
        /// </summary>
        /// <param name="languageId">Language identifier</param>
        /// <returns>Product attribute collection</returns>
        public static List<ProductAttribute> GetAllProductAttributes(int languageId)
        {
            string key = string.Format(PRODUCTATTRIBUTES_ALL_KEY, languageId);
            object obj2 = NopCache.Get(key);
            if (ProductAttributeManager.CacheEnabled && (obj2 != null))
            {
                return (List<ProductAttribute>)obj2;
            }

            var dbCollection = DBProviderManager<DBProductAttributeProvider>.Provider.GetAllProductAttributes(languageId);
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
        /// <param name="productAttributeId">Product attribute identifier</param>
        /// <returns>Product attribute </returns>
        public static ProductAttribute GetProductAttributeById(int productAttributeId)
        {
            int languageId = 0;
            if (NopContext.Current != null)
                languageId = NopContext.Current.WorkingLanguage.LanguageId;
            return GetProductAttributeById(productAttributeId, languageId);
        }

        /// <summary>
        /// Gets a product attribute 
        /// </summary>
        /// <param name="productAttributeId">Product attribute identifier</param>
        /// <param name="languageId">Language identifier</param>
        /// <returns>Product attribute </returns>
        public static ProductAttribute GetProductAttributeById(int productAttributeId, int languageId)
        {
            if (productAttributeId == 0)
                return null;

            string key = string.Format(PRODUCTATTRIBUTES_BY_ID_KEY, productAttributeId, languageId);
            object obj2 = NopCache.Get(key);
            if (ProductAttributeManager.CacheEnabled && (obj2 != null))
            {
                return (ProductAttribute)obj2;
            }

            var dbItem = DBProviderManager<DBProductAttributeProvider>.Provider.GetProductAttributeById(productAttributeId, languageId);
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
        /// <param name="name">The name</param>
        /// <param name="description">The description</param>
        /// <returns>Product attribute </returns>
        public static ProductAttribute InsertProductAttribute(string name, string description)
        {
            var dbItem = DBProviderManager<DBProductAttributeProvider>.Provider.InsertProductAttribute(name, description);
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
        /// <param name="productAttributeId">Product attribute identifier</param>
        /// <param name="name">The name</param>
        /// <param name="description">The description</param>
        /// <returns>Product attribute </returns>
        public static ProductAttribute UpdateProductAttribute(int productAttributeId,
            string name, string description)
        {
            var dbItem = DBProviderManager<DBProductAttributeProvider>.Provider.UpdateProductAttribute(productAttributeId,
                name, description);
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
        /// <param name="productAttributeLocalizedId">Localized product attribute identifier</param>
        /// <returns>Product attribute content</returns>
        public static ProductAttributeLocalized GetProductAttributeLocalizedById(int productAttributeLocalizedId)
        {
            if (productAttributeLocalizedId == 0)
                return null;

            var dbItem = DBProviderManager<DBProductAttributeProvider>.Provider.GetProductAttributeLocalizedById(productAttributeLocalizedId);
            var item = DBMapping(dbItem);
            return item;
        }

        /// <summary>
        /// Gets localized product attribute by product attribute id and language id
        /// </summary>
        /// <param name="productAttributeId">Product attribute identifier</param>
        /// <param name="languageId">Language identifier</param>
        /// <returns>Product attribute content</returns>
        public static ProductAttributeLocalized GetProductAttributeLocalizedByProductAttributeIdAndLanguageId(int productAttributeId, int languageId)
        {
            if (productAttributeId == 0 || languageId == 0)
                return null;

            var dbItem = DBProviderManager<DBProductAttributeProvider>.Provider.GetProductAttributeLocalizedByProductAttributeIdAndLanguageId(productAttributeId, languageId);
            var item = DBMapping(dbItem);
            return item;
        }

        /// <summary>
        /// Inserts a localized product attribute
        /// </summary>
        /// <param name="productAttributeId">Product attribute identifier</param>
        /// <param name="languageId">Language identifier</param>
        /// <param name="name">Name text</param>
        /// <param name="description">Description text</param>
        /// <returns>Product attribute content</returns>
        public static ProductAttributeLocalized InsertProductAttributeLocalized(int productAttributeId,
            int languageId, string name, string description)
        {
            var dbItem = DBProviderManager<DBProductAttributeProvider>.Provider.InsertProductAttributeLocalized(productAttributeId,
            languageId, name, description);
            var item = DBMapping(dbItem);

            if (ProductAttributeManager.CacheEnabled)
            {
                NopCache.RemoveByPattern(PRODUCTATTRIBUTES_PATTERN_KEY);
                NopCache.RemoveByPattern(PRODUCTVARIANTATTRIBUTES_PATTERN_KEY);
                NopCache.RemoveByPattern(PRODUCTVARIANTATTRIBUTEVALUES_PATTERN_KEY);
            }

            return item;
        }

        /// <summary>
        /// Update a localized product attribute
        /// </summary>
        /// <param name="productAttributeLocalizedId">Localized product attribute identifier</param>
        /// <param name="productAttributeId">Product attribute identifier</param>
        /// <param name="languageId">Language identifier</param>
        /// <param name="name">Name text</param>
        /// <param name="description">Description text</param>
        /// <returns>Product attribute content</returns>
        public static ProductAttributeLocalized UpdateProductAttributeLocalized(int productAttributeLocalizedId,
            int productAttributeId, int languageId, string name, string description)
        {
            var dbItem = DBProviderManager<DBProductAttributeProvider>.Provider.UpdateProductAttributeLocalized(productAttributeLocalizedId,
                productAttributeId, languageId, name, description);
            var item = DBMapping(dbItem);

            if (ProductAttributeManager.CacheEnabled)
            {
                NopCache.RemoveByPattern(PRODUCTATTRIBUTES_PATTERN_KEY);
                NopCache.RemoveByPattern(PRODUCTVARIANTATTRIBUTES_PATTERN_KEY);
                NopCache.RemoveByPattern(PRODUCTVARIANTATTRIBUTEVALUES_PATTERN_KEY);
            }

            return item;
        }
        
        #endregion

        #region Product variant attributes mappings (ProductVariantAttribute)

        /// <summary>
        /// Deletes a product variant attribute mapping
        /// </summary>
        /// <param name="productVariantAttributeId">Product variant attribute mapping identifier</param>
        public static void DeleteProductVariantAttribute(int productVariantAttributeId)
        {
            var productVariantAttribute = GetProductVariantAttributeById(productVariantAttributeId);
            if (productVariantAttribute == null)
                return;

            var context = ObjectContextHelper.CurrentObjectContext;
            if (!context.IsAttached(productVariantAttribute))
                context.ProductVariantAttributes.Attach(productVariantAttribute);
            context.DeleteObject(productVariantAttribute);
            context.SaveChanges();

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
        /// <param name="productVariantId">The product variant identifier</param>
        /// <returns>Product variant attribute mapping collection</returns>
        public static List<ProductVariantAttribute> GetProductVariantAttributesByProductVariantId(int productVariantId)
        {
            string key = string.Format(PRODUCTVARIANTATTRIBUTES_ALL_KEY, productVariantId);
            object obj2 = NopCache.Get(key);
            if (ProductAttributeManager.CacheEnabled && (obj2 != null))
            {
                return (List<ProductVariantAttribute>)obj2;
            }

            var context = ObjectContextHelper.CurrentObjectContext;
            var query = from pva in context.ProductVariantAttributes
                        orderby pva.DisplayOrder
                        where pva.ProductVariantId == productVariantId
                        select pva;
            var productVariantAttributes = query.ToList();

            if (ProductAttributeManager.CacheEnabled)
            {
                NopCache.Max(key, productVariantAttributes);
            }
            return productVariantAttributes;
        }

        /// <summary>
        /// Gets a product variant attribute mapping
        /// </summary>
        /// <param name="productVariantAttributeId">Product variant attribute mapping identifier</param>
        /// <returns>Product variant attribute mapping</returns>
        public static ProductVariantAttribute GetProductVariantAttributeById(int productVariantAttributeId)
        {
            if (productVariantAttributeId == 0)
                return null;

            string key = string.Format(PRODUCTVARIANTATTRIBUTES_BY_ID_KEY, productVariantAttributeId);
            object obj2 = NopCache.Get(key);
            if (ProductAttributeManager.CacheEnabled && (obj2 != null))
            {
                return (ProductVariantAttribute)obj2;
            }

            var context = ObjectContextHelper.CurrentObjectContext;
            var query = from pva in context.ProductVariantAttributes
                        where pva.ProductVariantAttributeId == productVariantAttributeId
                        select pva;
            var productVariantAttribute = query.SingleOrDefault();

            if (ProductAttributeManager.CacheEnabled)
            {
                NopCache.Max(key, productVariantAttribute);
            }
            return productVariantAttribute;
        }

        /// <summary>
        /// Inserts a product variant attribute mapping
        /// </summary>
        /// <param name="productVariantId">The product variant identifier</param>
        /// <param name="productAttributeId">The product attribute identifier</param>
        /// <param name="textPrompt">The text prompt</param>
        /// <param name="isRequired">The value indicating whether the entity is required</param>
        /// <param name="attributeControlType">The attribute control type</param>
        /// <param name="displayOrder">The display order</param>
        /// <returns>Product variant attribute mapping</returns>
        public static ProductVariantAttribute InsertProductVariantAttribute(int productVariantId,
            int productAttributeId, string textPrompt, bool isRequired, AttributeControlTypeEnum attributeControlType, int displayOrder)
        {
            var productVariantAttribute = new ProductVariantAttribute();
            productVariantAttribute.ProductVariantId = productVariantId;
            productVariantAttribute.ProductAttributeId = productAttributeId;
            productVariantAttribute.TextPrompt = textPrompt;
            productVariantAttribute.IsRequired = isRequired;
            productVariantAttribute.AttributeControlTypeId = (int)attributeControlType;
            productVariantAttribute.DisplayOrder = displayOrder;

            var context = ObjectContextHelper.CurrentObjectContext;
            context.ProductVariantAttributes.AddObject(productVariantAttribute);
            context.SaveChanges();

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
        /// <param name="productVariantAttributeId">The product variant attribute mapping identifier</param>
        /// <param name="productVariantId">The product variant identifier</param>
        /// <param name="productAttributeId">The product attribute identifier</param>
        /// <param name="textPrompt">The text prompt</param>
        /// <param name="isRequired">The value indicating whether the entity is required</param>
        /// <param name="attributeControlType">The attribute control type</param>
        /// <param name="displayOrder">The display order</param>
        /// <returns>Product variant attribute mapping</returns>
        public static ProductVariantAttribute UpdateProductVariantAttribute(int productVariantAttributeId, 
            int productVariantId, int productAttributeId, string textPrompt, 
            bool isRequired, AttributeControlTypeEnum attributeControlType, int displayOrder)
        {
            var productVariantAttribute = GetProductVariantAttributeById(productVariantAttributeId);
            if (productVariantAttribute == null)
                return null;

            var context = ObjectContextHelper.CurrentObjectContext;
            if (!context.IsAttached(productVariantAttribute))
                context.ProductVariantAttributes.Attach(productVariantAttribute);

            productVariantAttribute.ProductVariantId = productVariantId;
            productVariantAttribute.ProductAttributeId = productAttributeId;
            productVariantAttribute.TextPrompt = textPrompt;
            productVariantAttribute.IsRequired = isRequired;
            productVariantAttribute.AttributeControlTypeId = (int)attributeControlType;
            productVariantAttribute.DisplayOrder = displayOrder;
            context.SaveChanges();

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
        /// <param name="productVariantAttributeValueId">Product variant attribute value identifier</param>
        public static void DeleteProductVariantAttributeValue(int productVariantAttributeValueId)
        {
            DBProviderManager<DBProductAttributeProvider>.Provider.DeleteProductVariantAttributeValue(productVariantAttributeValueId);
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
        /// <param name="productVariantAttributeId">The product variant attribute mapping identifier</param>
        /// <returns>Product variant attribute mapping collection</returns>
        public static List<ProductVariantAttributeValue> GetProductVariantAttributeValues(int productVariantAttributeId)
        {
            int languageId = 0;
            if (NopContext.Current != null)
                languageId = NopContext.Current.WorkingLanguage.LanguageId;
            return GetProductVariantAttributeValues(productVariantAttributeId, languageId);
        }

        /// <summary>
        /// Gets product variant attribute values by product identifier
        /// </summary>
        /// <param name="productVariantAttributeId">The product variant attribute mapping identifier</param>
        /// <param name="languageId">Language identifier</param>
        /// <returns>Product variant attribute mapping collection</returns>
        public static List<ProductVariantAttributeValue> GetProductVariantAttributeValues(int productVariantAttributeId, int languageId)
        {
            string key = string.Format(PRODUCTVARIANTATTRIBUTEVALUES_ALL_KEY, productVariantAttributeId, languageId);
            object obj2 = NopCache.Get(key);
            if (ProductAttributeManager.CacheEnabled && (obj2 != null))
            {
                return (List<ProductVariantAttributeValue>)obj2;
            }

            var dbCollection = DBProviderManager<DBProductAttributeProvider>.Provider.GetProductVariantAttributeValues(productVariantAttributeId, languageId);
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
        /// <param name="productVariantAttributeValueId">Product variant attribute value identifier</param>
        /// <returns>Product variant attribute value</returns>
        public static ProductVariantAttributeValue GetProductVariantAttributeValueById(int productVariantAttributeValueId)
        {
            int languageId = 0;
            if (NopContext.Current != null)
                languageId = NopContext.Current.WorkingLanguage.LanguageId;
            return GetProductVariantAttributeValueById(productVariantAttributeValueId, languageId);
        }

        /// <summary>
        /// Gets a product variant attribute value
        /// </summary>
        /// <param name="productVariantAttributeValueId">Product variant attribute value identifier</param>
        /// <param name="languageId">Language identifier</param>
        /// <returns>Product variant attribute value</returns>
        public static ProductVariantAttributeValue GetProductVariantAttributeValueById(int productVariantAttributeValueId, int languageId)
        {
            if (productVariantAttributeValueId == 0)
                return null;

            string key = string.Format(PRODUCTVARIANTATTRIBUTEVALUES_BY_ID_KEY, productVariantAttributeValueId ,languageId);
            object obj2 = NopCache.Get(key);
            if (ProductAttributeManager.CacheEnabled && (obj2 != null))
            {
                return (ProductVariantAttributeValue)obj2;
            }

            var dbItem = DBProviderManager<DBProductAttributeProvider>.Provider.GetProductVariantAttributeValueById(productVariantAttributeValueId, languageId);
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
        /// <param name="productVariantAttributeId">The product variant attribute mapping identifier</param>
        /// <param name="name">The product variant attribute name</param>
        /// <param name="priceAdjustment">The price adjustment</param>
        /// <param name="weightAdjustment">The weight adjustment</param>
        /// <param name="isPreSelected">The value indicating whether the value is pre-selected</param>
        /// <param name="displayOrder">The display order</param>
        /// <returns>Product variant attribute value</returns>
        public static ProductVariantAttributeValue InsertProductVariantAttributeValue(int productVariantAttributeId,
            string name, decimal priceAdjustment, decimal weightAdjustment,
            bool isPreSelected, int displayOrder)
        {
            var dbItem = DBProviderManager<DBProductAttributeProvider>.Provider.InsertProductVariantAttributeValue(productVariantAttributeId,
                name, priceAdjustment, weightAdjustment, isPreSelected, displayOrder);
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
        /// <param name="productVariantAttributeValueId">The product variant attribute value identifier</param>
        /// <param name="productVariantAttributeId">The product variant attribute mapping identifier</param>
        /// <param name="name">The product variant attribute name</param>
        /// <param name="priceAdjustment">The price adjustment</param>
        /// <param name="weightAdjustment">The weight adjustment</param>
        /// <param name="isPreSelected">The value indicating whether the value is pre-selected</param>
        /// <param name="displayOrder">The display order</param>
        /// <returns>Product variant attribute value</returns>
        public static ProductVariantAttributeValue UpdateProductVariantAttributeValue(int productVariantAttributeValueId,
            int productVariantAttributeId, string name,
            decimal priceAdjustment, decimal weightAdjustment,
            bool isPreSelected, int displayOrder)
        {
            var dbItem = DBProviderManager<DBProductAttributeProvider>.Provider.UpdateProductVariantAttributeValue(productVariantAttributeValueId,
                productVariantAttributeId, name, priceAdjustment, weightAdjustment, isPreSelected, displayOrder);
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
        /// <param name="productVariantAttributeValueLocalizedId">Localized product variant attribute value identifier</param>
        /// <returns>Localized product variant attribute value</returns>
        public static ProductVariantAttributeValueLocalized GetProductVariantAttributeValueLocalizedById(int productVariantAttributeValueLocalizedId)
        {
            if (productVariantAttributeValueLocalizedId == 0)
                return null;

            var dbItem = DBProviderManager<DBProductAttributeProvider>.Provider.GetProductVariantAttributeValueLocalizedById(productVariantAttributeValueLocalizedId);
            var item = DBMapping(dbItem);
            return item;
        }

        /// <summary>
        /// Gets localized product variant attribute value by product variant attribute value id and language id
        /// </summary>
        /// <param name="productVariantAttributeValueId">Product variant attribute value identifier</param>
        /// <param name="languageId">Language identifier</param>
        /// <returns>Localized product variant attribute value</returns>
        public static ProductVariantAttributeValueLocalized GetProductVariantAttributeValueLocalizedByProductVariantAttributeValueIdAndLanguageId(int productVariantAttributeValueId, int languageId)
        {
            if (productVariantAttributeValueId == 0 || languageId == 0)
                return null;

            var dbItem = DBProviderManager<DBProductAttributeProvider>.Provider.GetProductVariantAttributeValueLocalizedByProductVariantAttributeValueIdAndLanguageId(productVariantAttributeValueId, languageId);
            var item = DBMapping(dbItem);
            return item;
        }

        /// <summary>
        /// Inserts a localized product variant attribute value
        /// </summary>
        /// <param name="productVariantAttributeValueId">Product variant attribute value identifier</param>
        /// <param name="languageId">Language identifier</param>
        /// <param name="name">Name text</param>
        /// <returns>Localized product variant attribute value</returns>
        public static ProductVariantAttributeValueLocalized InsertProductVariantAttributeValueLocalized(int productVariantAttributeValueId,
            int languageId, string name)
        {
            var dbItem = DBProviderManager<DBProductAttributeProvider>.Provider.InsertProductVariantAttributeValueLocalized(productVariantAttributeValueId,
                languageId, name);
            var item = DBMapping(dbItem);
            
            if (ProductAttributeManager.CacheEnabled)
            {
                NopCache.RemoveByPattern(PRODUCTATTRIBUTES_PATTERN_KEY);
                NopCache.RemoveByPattern(PRODUCTVARIANTATTRIBUTES_PATTERN_KEY);
                NopCache.RemoveByPattern(PRODUCTVARIANTATTRIBUTEVALUES_PATTERN_KEY);
            }

            return item;
        }

        /// <summary>
        /// Update a localized product variant attribute value
        /// </summary>
        /// <param name="productVariantAttributeValueLocalizedId">Localized product variant attribute value identifier</param>
        /// <param name="productVariantAttributeValueId">Product variant attribute value identifier</param>
        /// <param name="languageId">Language identifier</param>
        /// <param name="name">Name text</param>
        /// <returns>Localized product variant attribute value</returns>
        public static ProductVariantAttributeValueLocalized UpdateProductVariantAttributeValueLocalized(int productVariantAttributeValueLocalizedId,
            int productVariantAttributeValueId, int languageId, string name)
        {
            var dbItem = DBProviderManager<DBProductAttributeProvider>.Provider.UpdateProductVariantAttributeValueLocalized(productVariantAttributeValueLocalizedId,
                productVariantAttributeValueId, languageId, name);
            var item = DBMapping(dbItem);

            if (ProductAttributeManager.CacheEnabled)
            {
                NopCache.RemoveByPattern(PRODUCTATTRIBUTES_PATTERN_KEY);
                NopCache.RemoveByPattern(PRODUCTVARIANTATTRIBUTES_PATTERN_KEY);
                NopCache.RemoveByPattern(PRODUCTVARIANTATTRIBUTEVALUES_PATTERN_KEY);
            }

            return item;
        }
        
        #endregion

        #region Product variant attribute compinations (ProductVariantAttributeCombination)

        /// <summary>
        /// Deletes a product variant attribute combination
        /// </summary>
        /// <param name="productVariantAttributeCombinationId">Product variant attribute combination identifier</param>
        public static void DeleteProductVariantAttributeCombination(int productVariantAttributeCombinationId)
        {
            var combination = GetProductVariantAttributeCombinationById(productVariantAttributeCombinationId);
            if (combination == null)
                return;

            var context = ObjectContextHelper.CurrentObjectContext;
            if (!context.IsAttached(combination))
                context.ProductVariantAttributeCombinations.Attach(combination);
            context.DeleteObject(combination);
            context.SaveChanges();
        }

        /// <summary>
        /// Gets all product variant attribute combinations
        /// </summary>
        /// <param name="productVariantId">Product variant identifier</param>
        /// <returns>Product variant attribute combination collection</returns>
        public static List<ProductVariantAttributeCombination> GetAllProductVariantAttributeCombinations(int productVariantId)
        {
            if (productVariantId == 0)
                return new List<ProductVariantAttributeCombination>();
            var context = ObjectContextHelper.CurrentObjectContext;
            var query = from pvac in context.ProductVariantAttributeCombinations
                        orderby pvac.ProductVariantAttributeCombinationId
                        where pvac.ProductVariantId == productVariantId
                        select pvac;
            var combinations = query.ToList();
            return combinations;
        }

        /// <summary>
        /// Gets a product variant attribute combination
        /// </summary>
        /// <param name="productVariantAttributeCombinationId">Product variant attribute combination identifier</param>
        /// <returns>Product variant attribute combination</returns>
        public static ProductVariantAttributeCombination GetProductVariantAttributeCombinationById(int productVariantAttributeCombinationId)
        {
            if (productVariantAttributeCombinationId == 0)
                return null;

            var context = ObjectContextHelper.CurrentObjectContext;
            var query = from pvac in context.ProductVariantAttributeCombinations
                        where pvac.ProductVariantAttributeCombinationId == productVariantAttributeCombinationId
                        select pvac;
            var combination = query.SingleOrDefault();

            return combination;
        }

        /// <summary>
        /// Inserts a product variant attribute combination
        /// </summary>
        /// <param name="productVariantId">The product variant identifier</param>
        /// <param name="attributesXml">The attributes</param>
        /// <param name="stockQuantity">The stock quantity</param>
        /// <param name="allowOutOfStockOrders">The value indicating whether to allow orders when out of stock</param>
        /// <returns>Product variant attribute combination</returns>
        public static ProductVariantAttributeCombination InsertProductVariantAttributeCombination(int productVariantId,
            string attributesXml,
            int stockQuantity,
            bool allowOutOfStockOrders)
        {
            var combination = new ProductVariantAttributeCombination();
            combination.ProductVariantId = productVariantId;
            combination.AttributesXml = attributesXml;
            combination.StockQuantity = stockQuantity;
            combination.AllowOutOfStockOrders = allowOutOfStockOrders;

            var context = ObjectContextHelper.CurrentObjectContext;
            context.ProductVariantAttributeCombinations.AddObject(combination);
            context.SaveChanges();

            return combination;
        }

        /// <summary>
        /// Updates a product variant attribute combination
        /// </summary>
        /// <param name="productVariantAttributeCombinationId">Product variant attribute combination identifier</param>
        /// <param name="productVariantId">The product variant identifier</param>
        /// <param name="attributesXml">The attributes</param>
        /// <param name="stockQuantity">The stock quantity</param>
        /// <param name="allowOutOfStockOrders">The value indicating whether to allow orders when out of stock</param>
        /// <returns>Product variant attribute combination</returns>
        public static ProductVariantAttributeCombination UpdateProductVariantAttributeCombination(int productVariantAttributeCombinationId,
            int productVariantId,
            string attributesXml,
            int stockQuantity,
            bool allowOutOfStockOrders)
        {
            var combination = GetProductVariantAttributeCombinationById(productVariantAttributeCombinationId);
            if (combination == null)
                return null;

            var context = ObjectContextHelper.CurrentObjectContext;
            if (!context.IsAttached(combination))
                context.ProductVariantAttributeCombinations.Attach(combination);

            combination.ProductVariantId = productVariantId;
            combination.AttributesXml = attributesXml;
            combination.StockQuantity = stockQuantity;
            combination.AllowOutOfStockOrders = allowOutOfStockOrders;
            context.SaveChanges();

            return combination;
        }

        /// <summary>
        /// Finds a product variant attribute combination by attributes stored in XML 
        /// </summary>
        /// <param name="productVariantId">Product variant identifier</param>
        /// <param name="attributesXml">Attributes in XML format</param>
        /// <returns>Found product variant attribute combination</returns>
        public static ProductVariantAttributeCombination FindProductVariantAttributeCombination(int productVariantId, string attributesXml)
        {
            //existing combinations
            var combinations = ProductAttributeManager.GetAllProductVariantAttributeCombinations(productVariantId);
            if (combinations.Count == 0)
                return null;

            foreach (var combination in combinations)
            {
                bool attributesEqual = ProductAttributeHelper.AreProductAttributesEqual(combination.AttributesXml, attributesXml);
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
