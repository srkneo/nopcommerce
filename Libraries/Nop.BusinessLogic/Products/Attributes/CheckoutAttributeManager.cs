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
    /// Checkout attribute manager
    /// </summary>
    public partial class CheckoutAttributeManager
    {
        #region Constants
        private const string CHECKOUTATTRIBUTES_ALL_KEY = "Nop.checkoutattribute.all-{0}-{1}";
        private const string CHECKOUTATTRIBUTES_BY_ID_KEY = "Nop.checkoutattribute.id-{0}-{1}";
        private const string CHECKOUTATTRIBUTEVALUES_ALL_KEY = "Nop.checkoutattributevalue.all-{0}-{1}";
        private const string CHECKOUTATTRIBUTEVALUES_BY_ID_KEY = "Nop.checkoutattributevalue.id-{0}-{1}";
        private const string CHECKOUTATTRIBUTES_PATTERN_KEY = "Nop.checkoutattribute.";
        private const string CHECKOUTATTRIBUTEVALUES_PATTERN_KEY = "Nop.checkoutattributevalue.";
        #endregion

        #region Utilities
        private static CheckoutAttributeCollection DBMapping(DBCheckoutAttributeCollection dbCollection)
        {
            if (dbCollection == null)
                return null;

            var collection = new CheckoutAttributeCollection();
            foreach (var dbItem in dbCollection)
            {
                var item = DBMapping(dbItem);
                collection.Add(item);
            }

            return collection;
        }

        private static CheckoutAttribute DBMapping(DBCheckoutAttribute dbItem)
        {
            if (dbItem == null)
                return null;

            var item = new CheckoutAttribute();
            item.CheckoutAttributeId = dbItem.CheckoutAttributeId;
            item.Name = dbItem.Name;
            item.TextPrompt = dbItem.TextPrompt;
            item.IsRequired = dbItem.IsRequired;
            item.ShippableProductRequired = dbItem.ShippableProductRequired;
            item.IsTaxExempt = dbItem.IsTaxExempt;
            item.TaxCategoryId = dbItem.TaxCategoryId;
            item.AttributeControlTypeId = dbItem.AttributeControlTypeId;
            item.DisplayOrder = dbItem.DisplayOrder;

            return item;
        }

        private static CheckoutAttributeLocalized DBMapping(DBCheckoutAttributeLocalized dbItem)
        {
            if (dbItem == null)
                return null;

            var item = new CheckoutAttributeLocalized();
            item.CheckoutAttributeLocalizedId = dbItem.CheckoutAttributeLocalizedId;
            item.CheckoutAttributeId = dbItem.CheckoutAttributeId;
            item.LanguageId = dbItem.LanguageId;
            item.Name = dbItem.Name;
            item.TextPrompt = dbItem.TextPrompt;

            return item;
        }

        private static CheckoutAttributeValueCollection DBMapping(DBCheckoutAttributeValueCollection dbCollection)
        {
            if (dbCollection == null)
                return null;

            var collection = new CheckoutAttributeValueCollection();
            foreach (var dbItem in dbCollection)
            {
                var item = DBMapping(dbItem);
                collection.Add(item);
            }

            return collection;
        }

        private static CheckoutAttributeValue DBMapping(DBCheckoutAttributeValue dbItem)
        {
            if (dbItem == null)
                return null;

            var item = new CheckoutAttributeValue();
            item.CheckoutAttributeValueId = dbItem.CheckoutAttributeValueId;
            item.CheckoutAttributeId = dbItem.CheckoutAttributeId;
            item.Name = dbItem.Name;
            item.PriceAdjustment = dbItem.PriceAdjustment;
            item.WeightAdjustment = dbItem.WeightAdjustment;
            item.IsPreSelected = dbItem.IsPreSelected;
            item.DisplayOrder = dbItem.DisplayOrder;

            return item;
        }

        private static CheckoutAttributeValueLocalized DBMapping(DBCheckoutAttributeValueLocalized dbItem)
        {
            if (dbItem == null)
                return null;

            var item = new CheckoutAttributeValueLocalized();
            item.CheckoutAttributeValueLocalizedId = dbItem.CheckoutAttributeValueLocalizedId;
            item.CheckoutAttributeValueId = dbItem.CheckoutAttributeValueId;
            item.LanguageId = dbItem.LanguageId;
            item.Name = dbItem.Name;

            return item;
        }

        #endregion

        #region Methods

        #region Checkout attributes

        /// <summary>
        /// Deletes a checkout attribute
        /// </summary>
        /// <param name="checkoutAttributeId">Checkout attribute identifier</param>
        public static void DeleteCheckoutAttribute(int checkoutAttributeId)
        {
            DBProviderManager<DBCheckoutAttributeProvider>.Provider.DeleteCheckoutAttribute(checkoutAttributeId);
            if (CheckoutAttributeManager.CacheEnabled)
            {
                NopCache.RemoveByPattern(CHECKOUTATTRIBUTES_PATTERN_KEY);
                NopCache.RemoveByPattern(CHECKOUTATTRIBUTEVALUES_PATTERN_KEY);
            }
        }

        /// <summary>
        /// Gets all checkout attributes
        /// </summary>
        /// <param name="dontLoadShippableProductRequired">Value indicating whether to do not load attributes for checkout attibutes which require shippable products</param>
        /// <returns>Checkout attribute collection</returns>
        public static CheckoutAttributeCollection GetAllCheckoutAttributes(bool dontLoadShippableProductRequired)
        {
            int languageId = 0;
            if (NopContext.Current != null)
                languageId = NopContext.Current.WorkingLanguage.LanguageId;
            return GetAllCheckoutAttributes(languageId, dontLoadShippableProductRequired);
        }

        /// <summary>
        /// Gets all checkout attributes
        /// </summary>
        /// <param name="languageId">Language identifier</param>
        /// <param name="dontLoadShippableProductRequired">Value indicating whether to do not load attributes for checkout attibutes which require shippable products</param>
        /// <returns>Checkout attribute collection</returns>
        public static CheckoutAttributeCollection GetAllCheckoutAttributes(int languageId,
            bool dontLoadShippableProductRequired)
        {
            string key = string.Format(CHECKOUTATTRIBUTES_ALL_KEY, languageId, dontLoadShippableProductRequired);
            object obj2 = NopCache.Get(key);
            if (CheckoutAttributeManager.CacheEnabled && (obj2 != null))
            {
                return (CheckoutAttributeCollection)obj2;
            }

            var dbCollection = DBProviderManager<DBCheckoutAttributeProvider>.Provider.GetAllCheckoutAttributes(languageId, dontLoadShippableProductRequired);
            var checkoutAttributes = DBMapping(dbCollection);

            if (CheckoutAttributeManager.CacheEnabled)
            {
                NopCache.Max(key, checkoutAttributes);
            }
            return checkoutAttributes;
        }

        /// <summary>
        /// Gets a checkout attribute 
        /// </summary>
        /// <param name="checkoutAttributeId">Checkout attribute identifier</param>
        /// <returns>Checkout attribute</returns>
        public static CheckoutAttribute GetCheckoutAttributeById(int checkoutAttributeId)
        {
            int languageId = 0;
            if (NopContext.Current != null)
                languageId = NopContext.Current.WorkingLanguage.LanguageId;
            return GetCheckoutAttributeById(checkoutAttributeId, languageId);
        }

        /// <summary>
        /// Gets a checkout attribute 
        /// </summary>
        /// <param name="checkoutAttributeId">Checkout attribute identifier</param>
        /// <param name="languageId">Language identifier</param>
        /// <returns>Checkout attribute</returns>
        public static CheckoutAttribute GetCheckoutAttributeById(int checkoutAttributeId, 
            int languageId)
        {
            if (checkoutAttributeId == 0)
                return null;

            string key = string.Format(CHECKOUTATTRIBUTES_BY_ID_KEY, checkoutAttributeId, languageId);
            object obj2 = NopCache.Get(key);
            if (CheckoutAttributeManager.CacheEnabled && (obj2 != null))
            {
                return (CheckoutAttribute)obj2;
            }

            var dbItem = DBProviderManager<DBCheckoutAttributeProvider>.Provider.GetCheckoutAttributeById(checkoutAttributeId, languageId);
            var checkoutAttribute = DBMapping(dbItem);

            if (CheckoutAttributeManager.CacheEnabled)
            {
                NopCache.Max(key, checkoutAttribute);
            }
            return checkoutAttribute;
        }

        /// <summary>
        /// Inserts a checkout attribute
        /// </summary>
        /// <param name="name">Name</param>
        /// <param name="textPrompt">Text prompt</param>
        /// <param name="isRequired">Value indicating whether the entity is required</param>
        /// <param name="shippableProductRequired">Value indicating whether shippable products are required in order to display this attribute</param>
        /// <param name="isTaxExempt">Value indicating whether the attribute is marked as tax exempt</param>
        /// <param name="taxCategoryId">Tax category identifier</param>
        /// <param name="attributeControlTypeId">Attribute control type identifier</param>
        /// <param name="displayOrder">Display order</param>
        /// <returns>Checkout attribute</returns>
        public static CheckoutAttribute InsertCheckoutAttribute(string name,
            string textPrompt, bool isRequired, bool shippableProductRequired,
            bool isTaxExempt, int taxCategoryId, int attributeControlTypeId,
            int displayOrder)
        {
            var dbItem = DBProviderManager<DBCheckoutAttributeProvider>.Provider.InsertCheckoutAttribute(name,
                textPrompt, isRequired, shippableProductRequired,
                isTaxExempt, taxCategoryId, attributeControlTypeId, displayOrder);
            var checkoutAttribute = DBMapping(dbItem);
            if (CheckoutAttributeManager.CacheEnabled)
            {
                NopCache.RemoveByPattern(CHECKOUTATTRIBUTES_PATTERN_KEY);
                NopCache.RemoveByPattern(CHECKOUTATTRIBUTEVALUES_PATTERN_KEY);
            }
            return checkoutAttribute;
        }

        /// <summary>
        /// Updates the checkout attribute
        /// </summary>
        /// <param name="checkoutAttributeId">Checkout attribute identifier</param>
        /// <param name="name">Name</param>
        /// <param name="textPrompt">Text prompt</param>
        /// <param name="isRequired">Value indicating whether the entity is required</param>
        /// <param name="shippableProductRequired">Value indicating whether shippable products are required in order to display this attribute</param>
        /// <param name="isTaxExempt">Value indicating whether the attribute is marked as tax exempt</param>
        /// <param name="taxCategoryId">Tax category identifier</param>
        /// <param name="attributeControlTypeId">Attribute control type identifier</param>
        /// <param name="displayOrder">Display order</param>
        /// <returns>Checkout attribute</returns>
        public static CheckoutAttribute UpdateCheckoutAttribute(int checkoutAttributeId,
            string name, string textPrompt, bool isRequired, bool shippableProductRequired,
            bool isTaxExempt, int taxCategoryId, int attributeControlTypeId,
            int displayOrder)
        {
            var dbItem = DBProviderManager<DBCheckoutAttributeProvider>.Provider.UpdateCheckoutAttribute(checkoutAttributeId,
                name, textPrompt, isRequired, shippableProductRequired,
                isTaxExempt, taxCategoryId, attributeControlTypeId, displayOrder);
            var checkoutAttribute = DBMapping(dbItem);
            if (CheckoutAttributeManager.CacheEnabled)
            {
                NopCache.RemoveByPattern(CHECKOUTATTRIBUTES_PATTERN_KEY);
                NopCache.RemoveByPattern(CHECKOUTATTRIBUTEVALUES_PATTERN_KEY);
            }

            return checkoutAttribute;
        }

        /// <summary>
        /// Gets localized checkout attribute by id
        /// </summary>
        /// <param name="checkoutAttributeLocalizedId">Localized checkout attribute identifier</param>
        /// <returns>Checkout attribute content</returns>
        public static CheckoutAttributeLocalized GetCheckoutAttributeLocalizedById(int checkoutAttributeLocalizedId)
        {
            if (checkoutAttributeLocalizedId == 0)
                return null;

            var dbItem = DBProviderManager<DBCheckoutAttributeProvider>.Provider.GetCheckoutAttributeLocalizedById(checkoutAttributeLocalizedId);
            var item = DBMapping(dbItem);
            return item;
        }

        /// <summary>
        /// Gets localized checkout attribute by checkout attribute id and language id
        /// </summary>
        /// <param name="checkoutAttributeId">Checkout attribute identifier</param>
        /// <param name="languageId">Language identifier</param>
        /// <returns>Checkout attribute content</returns>
        public static CheckoutAttributeLocalized GetCheckoutAttributeLocalizedByCheckoutAttributeIdAndLanguageId(int checkoutAttributeId, int languageId)
        {
            if (checkoutAttributeId == 0 || languageId == 0)
                return null;

            var dbItem = DBProviderManager<DBCheckoutAttributeProvider>.Provider.GetCheckoutAttributeLocalizedByCheckoutAttributeIdAndLanguageId(checkoutAttributeId, languageId);
            var item = DBMapping(dbItem);
            return item;
        }

        /// <summary>
        /// Inserts a localized checkout attribute
        /// </summary>
        /// <param name="checkoutAttributeId">Checkout attribute identifier</param>
        /// <param name="languageId">Language identifier</param>
        /// <param name="name">Name text</param>
        /// <param name="textPrompt">Text prompt</param>
        /// <returns>Checkout attribute content</returns>
        public static CheckoutAttributeLocalized InsertCheckoutAttributeLocalized(int checkoutAttributeId,
            int languageId, string name, string textPrompt)
        {
            var dbItem = DBProviderManager<DBCheckoutAttributeProvider>.Provider.InsertCheckoutAttributeLocalized(checkoutAttributeId,
                languageId, name, textPrompt);
            var item = DBMapping(dbItem);

            if (CheckoutAttributeManager.CacheEnabled)
            {
                NopCache.RemoveByPattern(CHECKOUTATTRIBUTES_PATTERN_KEY);
                NopCache.RemoveByPattern(CHECKOUTATTRIBUTEVALUES_PATTERN_KEY);
            }

            return item;
        }

        /// <summary>
        /// Update a localized checkout attribute
        /// </summary>
        /// <param name="checkoutAttributeLocalizedId">Localized checkout attribute identifier</param>
        /// <param name="checkoutAttributeId">Checkout attribute identifier</param>
        /// <param name="languageId">Language identifier</param>
        /// <param name="name">Name text</param>
        /// <param name="textPrompt">Text prompt</param>
        /// <returns>Checkout attribute content</returns>
        public static CheckoutAttributeLocalized UpdateCheckoutAttributeLocalized(int checkoutAttributeLocalizedId,
            int checkoutAttributeId, int languageId, string name, string textPrompt)
        {
            var dbItem = DBProviderManager<DBCheckoutAttributeProvider>.Provider.UpdateCheckoutAttributeLocalized(checkoutAttributeLocalizedId,
                checkoutAttributeId, languageId, name, textPrompt);
            var item = DBMapping(dbItem);

            if (CheckoutAttributeManager.CacheEnabled)
            {
                NopCache.RemoveByPattern(CHECKOUTATTRIBUTES_PATTERN_KEY);
                NopCache.RemoveByPattern(CHECKOUTATTRIBUTEVALUES_PATTERN_KEY);
            }

            return item;
        }
        
        #endregion

        #region Checkout variant attribute values

        /// <summary>
        /// Deletes a checkout attribute value
        /// </summary>
        /// <param name="checkoutAttributeValueId">Checkout attribute value identifier</param>
        public static void DeleteCheckoutAttributeValue(int checkoutAttributeValueId)
        {
            DBProviderManager<DBCheckoutAttributeProvider>.Provider.DeleteCheckoutAttributeValue(checkoutAttributeValueId);
            if (CheckoutAttributeManager.CacheEnabled)
            {
                NopCache.RemoveByPattern(CHECKOUTATTRIBUTES_PATTERN_KEY);
                NopCache.RemoveByPattern(CHECKOUTATTRIBUTEVALUES_PATTERN_KEY);
            }
        }

        /// <summary>
        /// Gets checkout attribute values by checkout attribute identifier
        /// </summary>
        /// <param name="checkoutAttributeId">The checkout attribute identifier</param>
        /// <returns>Checkout attribute value collection</returns>
        public static CheckoutAttributeValueCollection GetCheckoutAttributeValues(int checkoutAttributeId)
        {
            int languageId = 0;
            if (NopContext.Current != null)
                languageId = NopContext.Current.WorkingLanguage.LanguageId;
            return GetCheckoutAttributeValues(checkoutAttributeId, languageId);
        }

        /// <summary>
        /// Gets checkout attribute values by checkout attribute identifier
        /// </summary>
        /// <param name="checkoutAttributeId">The checkout attribute identifier</param>
        /// <param name="languageId">Language identifier</param>
        /// <returns>Checkout attribute value collection</returns>
        public static CheckoutAttributeValueCollection GetCheckoutAttributeValues(int checkoutAttributeId, int languageId)
        {
            string key = string.Format(CHECKOUTATTRIBUTEVALUES_ALL_KEY, checkoutAttributeId, languageId);
            object obj2 = NopCache.Get(key);
            if (CheckoutAttributeManager.CacheEnabled && (obj2 != null))
            {
                return (CheckoutAttributeValueCollection)obj2;
            }

            var dbCollection = DBProviderManager<DBCheckoutAttributeProvider>.Provider.GetCheckoutAttributeValues(checkoutAttributeId, languageId);
            var checkoutAttributeValues = DBMapping(dbCollection);

            if (CheckoutAttributeManager.CacheEnabled)
            {
                NopCache.Max(key, checkoutAttributeValues);
            }
            return checkoutAttributeValues;
        }

        /// <summary>
        /// Gets a checkout attribute value
        /// </summary>
        /// <param name="checkoutAttributeValueId">Checkout attribute value identifier</param>
        /// <returns>Checkout attribute value</returns>
        public static CheckoutAttributeValue GetCheckoutAttributeValueById(int checkoutAttributeValueId)
        {
            int languageId = 0;
            if (NopContext.Current != null)
                languageId = NopContext.Current.WorkingLanguage.LanguageId;
            return GetCheckoutAttributeValueById(checkoutAttributeValueId, languageId);
        }

        /// <summary>
        /// Gets a checkout attribute value
        /// </summary>
        /// <param name="checkoutAttributeValueId">Checkout attribute value identifier</param>
        /// <param name="languageId">Language identifier</param>
        /// <returns>Checkout attribute value</returns>
        public static CheckoutAttributeValue GetCheckoutAttributeValueById(int checkoutAttributeValueId, int languageId)
        {
            if (checkoutAttributeValueId == 0)
                return null;

            string key = string.Format(CHECKOUTATTRIBUTEVALUES_BY_ID_KEY, checkoutAttributeValueId, languageId);
            object obj2 = NopCache.Get(key);
            if (CheckoutAttributeManager.CacheEnabled && (obj2 != null))
            {
                return (CheckoutAttributeValue)obj2;
            }

            var dbItem = DBProviderManager<DBCheckoutAttributeProvider>.Provider.GetCheckoutAttributeValueById(checkoutAttributeValueId, languageId);
            var checkoutAttributeValue = DBMapping(dbItem);
            if (CheckoutAttributeManager.CacheEnabled)
            {
                NopCache.Max(key, checkoutAttributeValue);
            }
            return checkoutAttributeValue;
        }

        /// <summary>
        /// Inserts a checkout attribute value
        /// </summary>
        /// <param name="checkoutAttributeId">The checkout attribute identifier</param>
        /// <param name="name">The checkout attribute name</param>
        /// <param name="priceAdjustment">The price adjustment</param>
        /// <param name="weightAdjustment">The weight adjustment</param>
        /// <param name="isPreSelected">The value indicating whether the value is pre-selected</param>
        /// <param name="displayOrder">The display order</param>
        /// <returns>Checkout attribute value</returns>
        public static CheckoutAttributeValue InsertCheckoutAttributeValue(int checkoutAttributeId,
            string name, decimal priceAdjustment, decimal weightAdjustment,
            bool isPreSelected, int displayOrder)
        {
            var dbItem = DBProviderManager<DBCheckoutAttributeProvider>.Provider.InsertCheckoutAttributeValue(checkoutAttributeId,
                name, priceAdjustment, weightAdjustment, isPreSelected, displayOrder);
            var checkoutAttributeValue = DBMapping(dbItem);

            if (CheckoutAttributeManager.CacheEnabled)
            {
                NopCache.RemoveByPattern(CHECKOUTATTRIBUTES_PATTERN_KEY);
                NopCache.RemoveByPattern(CHECKOUTATTRIBUTEVALUES_PATTERN_KEY);
            }

            return checkoutAttributeValue;
        }

        /// <summary>
        /// Updates the checkout attribute value
        /// </summary>
        /// <param name="checkoutAttributeValueId">The checkout attribute value identifier</param>
        /// <param name="checkoutAttributeId">The checkout attribute identifier</param>
        /// <param name="name">The checkout attribute name</param>
        /// <param name="priceAdjustment">The price adjustment</param>
        /// <param name="weightAdjustment">The weight adjustment</param>
        /// <param name="isPreSelected">The value indicating whether the value is pre-selected</param>
        /// <param name="displayOrder">The display order</param>
        /// <returns>Checkout attribute value</returns>
        public static CheckoutAttributeValue UpdateCheckoutAttributeValue(int checkoutAttributeValueId,
            int checkoutAttributeId, string name, decimal priceAdjustment, decimal weightAdjustment,
            bool isPreSelected, int displayOrder)
        {
            var dbItem = DBProviderManager<DBCheckoutAttributeProvider>.Provider.UpdateCheckoutAttributeValue(checkoutAttributeValueId,
                checkoutAttributeId, name, priceAdjustment, weightAdjustment, isPreSelected, displayOrder);
            var checkoutAttributeValue = DBMapping(dbItem);

            if (CheckoutAttributeManager.CacheEnabled)
            {
                NopCache.RemoveByPattern(CHECKOUTATTRIBUTES_PATTERN_KEY);
                NopCache.RemoveByPattern(CHECKOUTATTRIBUTEVALUES_PATTERN_KEY);
            }

            return checkoutAttributeValue;
        }

        /// <summary>
        /// Gets localized checkout attribute value by id
        /// </summary>
        /// <param name="checkoutAttributeValueLocalizedId">Localized checkout attribute value identifier</param>
        /// <returns>Localized checkout attribute value</returns>
        public static CheckoutAttributeValueLocalized GetCheckoutAttributeValueLocalizedById(int checkoutAttributeValueLocalizedId)
        {
            if (checkoutAttributeValueLocalizedId == 0)
                return null;

            var dbItem = DBProviderManager<DBCheckoutAttributeProvider>.Provider.GetCheckoutAttributeValueLocalizedById(checkoutAttributeValueLocalizedId);
            var item = DBMapping(dbItem);
            return item;
        }

        /// <summary>
        /// Gets localized checkout attribute value by checkout attribute value id and language id
        /// </summary>
        /// <param name="checkoutAttributeValueId">Checkout attribute value identifier</param>
        /// <param name="languageId">Language identifier</param>
        /// <returns>Localized checkout attribute value</returns>
        public static CheckoutAttributeValueLocalized GetCheckoutAttributeValueLocalizedByCheckoutAttributeValueIdAndLanguageId(int checkoutAttributeValueId, int languageId)
        {
            if (checkoutAttributeValueId == 0 || languageId == 0)
                return null;

            var dbItem = DBProviderManager<DBCheckoutAttributeProvider>.Provider.GetCheckoutAttributeValueLocalizedByCheckoutAttributeValueIdAndLanguageId(checkoutAttributeValueId, languageId);
            var item = DBMapping(dbItem);
            return item;
        }

        /// <summary>
        /// Inserts a localized checkout attribute value
        /// </summary>
        /// <param name="checkoutAttributeValueId">Checkout attribute value identifier</param>
        /// <param name="languageId">Language identifier</param>
        /// <param name="name">Name text</param>
        /// <returns>Localized checkout attribute value</returns>
        public static CheckoutAttributeValueLocalized InsertCheckoutAttributeValueLocalized(int checkoutAttributeValueId,
            int languageId, string name)
        {
            var dbItem = DBProviderManager<DBCheckoutAttributeProvider>.Provider.InsertCheckoutAttributeValueLocalized(checkoutAttributeValueId,
                languageId, name);
            var item = DBMapping(dbItem);

            if (CheckoutAttributeManager.CacheEnabled)
            {
                NopCache.RemoveByPattern(CHECKOUTATTRIBUTES_PATTERN_KEY);
                NopCache.RemoveByPattern(CHECKOUTATTRIBUTEVALUES_PATTERN_KEY);
            }

            return item;
        }

        /// <summary>
        /// Update a localized checkout attribute value
        /// </summary>
        /// <param name="checkoutAttributeValueLocalizedId">Localized checkout attribute value identifier</param>
        /// <param name="checkoutAttributeValueId">Checkout attribute value identifier</param>
        /// <param name="languageId">Language identifier</param>
        /// <param name="name">Name text</param>
        /// <returns>Localized checkout attribute value</returns>
        public static CheckoutAttributeValueLocalized UpdateCheckoutAttributeValueLocalized(int checkoutAttributeValueLocalizedId,
            int checkoutAttributeValueId, int languageId, string name)
        {
            var dbItem = DBProviderManager<DBCheckoutAttributeProvider>.Provider.UpdateCheckoutAttributeValueLocalized(checkoutAttributeValueLocalizedId,
                checkoutAttributeValueId, languageId, name);
            var item = DBMapping(dbItem);

            if (CheckoutAttributeManager.CacheEnabled)
            {
                NopCache.RemoveByPattern(CHECKOUTATTRIBUTES_PATTERN_KEY);
                NopCache.RemoveByPattern(CHECKOUTATTRIBUTEVALUES_PATTERN_KEY);
            }

            return item;
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
                return SettingManager.GetSettingValueBoolean("Cache.CheckoutAttributeManager.CacheEnabled");
            }
        }
        #endregion
    }
}
