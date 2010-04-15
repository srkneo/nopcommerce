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
            item.CheckoutAttributeID = dbItem.CheckoutAttributeID;
            item.Name = dbItem.Name;
            item.TextPrompt = dbItem.TextPrompt;
            item.IsRequired = dbItem.IsRequired;
            item.ShippableProductRequired = dbItem.ShippableProductRequired;
            item.IsTaxExempt = dbItem.IsTaxExempt;
            item.TaxCategoryID = dbItem.TaxCategoryID;
            item.AttributeControlTypeID = dbItem.AttributeControlTypeID;
            item.DisplayOrder = dbItem.DisplayOrder;

            return item;
        }

        private static CheckoutAttributeLocalized DBMapping(DBCheckoutAttributeLocalized dbItem)
        {
            if (dbItem == null)
                return null;

            var item = new CheckoutAttributeLocalized();
            item.CheckoutAttributeLocalizedID = dbItem.CheckoutAttributeLocalizedID;
            item.CheckoutAttributeID = dbItem.CheckoutAttributeID;
            item.LanguageID = dbItem.LanguageID;
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
            item.CheckoutAttributeValueID = dbItem.CheckoutAttributeValueID;
            item.CheckoutAttributeID = dbItem.CheckoutAttributeID;
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
            item.CheckoutAttributeValueLocalizedID = dbItem.CheckoutAttributeValueLocalizedID;
            item.CheckoutAttributeValueID = dbItem.CheckoutAttributeValueID;
            item.LanguageID = dbItem.LanguageID;
            item.Name = dbItem.Name;

            return item;
        }

        #endregion

        #region Methods

        #region Checkout attributes

        /// <summary>
        /// Deletes a checkout attribute
        /// </summary>
        /// <param name="CheckoutAttributeID">Checkout attribute identifier</param>
        public static void DeleteCheckoutAttribute(int CheckoutAttributeID)
        {
            DBProviderManager<DBCheckoutAttributeProvider>.Provider.DeleteCheckoutAttribute(CheckoutAttributeID);
            if (CheckoutAttributeManager.CacheEnabled)
            {
                NopCache.RemoveByPattern(CHECKOUTATTRIBUTES_PATTERN_KEY);
                NopCache.RemoveByPattern(CHECKOUTATTRIBUTEVALUES_PATTERN_KEY);
            }
        }

        /// <summary>
        /// Gets all checkout attributes
        /// </summary>
        /// <param name="DontLoadShippableProductRequired">Value indicating whether to do not load attributes for checkout attibutes which require shippable products</param>
        /// <returns>Checkout attribute collection</returns>
        public static CheckoutAttributeCollection GetAllCheckoutAttributes(bool DontLoadShippableProductRequired)
        {
            int languageId = 0;
            if (NopContext.Current != null)
                languageId = NopContext.Current.WorkingLanguage.LanguageID;
            return GetAllCheckoutAttributes(languageId, DontLoadShippableProductRequired);
        }

        /// <summary>
        /// Gets all checkout attributes
        /// </summary>
        /// <param name="LanguageID">Language identifier</param>
        /// <param name="DontLoadShippableProductRequired">Value indicating whether to do not load attributes for checkout attibutes which require shippable products</param>
        /// <returns>Checkout attribute collection</returns>
        public static CheckoutAttributeCollection GetAllCheckoutAttributes(int LanguageID, bool DontLoadShippableProductRequired)
        {
            string key = string.Format(CHECKOUTATTRIBUTES_ALL_KEY, LanguageID, DontLoadShippableProductRequired);
            object obj2 = NopCache.Get(key);
            if (CheckoutAttributeManager.CacheEnabled && (obj2 != null))
            {
                return (CheckoutAttributeCollection)obj2;
            }

            var dbCollection = DBProviderManager<DBCheckoutAttributeProvider>.Provider.GetAllCheckoutAttributes(LanguageID, DontLoadShippableProductRequired);
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
        /// <param name="CheckoutAttributeID">Checkout attribute identifier</param>
        /// <returns>Checkout attribute</returns>
        public static CheckoutAttribute GetCheckoutAttributeByID(int CheckoutAttributeID)
        {
            int languageId = 0;
            if (NopContext.Current != null)
                languageId = NopContext.Current.WorkingLanguage.LanguageID;
            return GetCheckoutAttributeByID(CheckoutAttributeID, languageId);
        }

        /// <summary>
        /// Gets a checkout attribute 
        /// </summary>
        /// <param name="CheckoutAttributeID">Checkout attribute identifier</param>
        /// <param name="LanguageID">Language identifier</param>
        /// <returns>Checkout attribute</returns>
        public static CheckoutAttribute GetCheckoutAttributeByID(int CheckoutAttributeID, int LanguageID)
        {
            if (CheckoutAttributeID == 0)
                return null;

            string key = string.Format(CHECKOUTATTRIBUTES_BY_ID_KEY, CheckoutAttributeID, LanguageID);
            object obj2 = NopCache.Get(key);
            if (CheckoutAttributeManager.CacheEnabled && (obj2 != null))
            {
                return (CheckoutAttribute)obj2;
            }

            var dbItem = DBProviderManager<DBCheckoutAttributeProvider>.Provider.GetCheckoutAttributeByID(CheckoutAttributeID, LanguageID);
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
        /// <param name="Name">Name</param>
        /// <param name="TextPrompt">Text prompt</param>
        /// <param name="IsRequired">Value indicating whether the entity is required</param>
        /// <param name="ShippableProductRequired">Value indicating whether shippable products are required in order to display this attribute</param>
        /// <param name="IsTaxExempt">Value indicating whether the attribute is marked as tax exempt</param>
        /// <param name="TaxCategoryID">Tax category identifier</param>
        /// <param name="AttributeControlTypeID">Attribute control type identifier</param>
        /// <param name="DisplayOrder">Display order</param>
        /// <returns>Checkout attribute</returns>
        public static CheckoutAttribute InsertCheckoutAttribute(string Name,
            string TextPrompt, bool IsRequired, bool ShippableProductRequired,
            bool IsTaxExempt, int TaxCategoryID, int AttributeControlTypeID,
            int DisplayOrder)
        {
            var dbItem = DBProviderManager<DBCheckoutAttributeProvider>.Provider.InsertCheckoutAttribute(Name,
                TextPrompt, IsRequired, ShippableProductRequired,
                IsTaxExempt, TaxCategoryID, AttributeControlTypeID, DisplayOrder);
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
        /// <param name="CheckoutAttributeID">Checkout attribute identifier</param>
        /// <param name="Name">Name</param>
        /// <param name="TextPrompt">Text prompt</param>
        /// <param name="IsRequired">Value indicating whether the entity is required</param>
        /// <param name="ShippableProductRequired">Value indicating whether shippable products are required in order to display this attribute</param>
        /// <param name="IsTaxExempt">Value indicating whether the attribute is marked as tax exempt</param>
        /// <param name="TaxCategoryID">Tax category identifier</param>
        /// <param name="AttributeControlTypeID">Attribute control type identifier</param>
        /// <param name="DisplayOrder">Display order</param>
        /// <returns>Checkout attribute</returns>
        public static CheckoutAttribute UpdateCheckoutAttribute(int CheckoutAttributeID,
            string Name, string TextPrompt, bool IsRequired, bool ShippableProductRequired,
            bool IsTaxExempt, int TaxCategoryID, int AttributeControlTypeID,
            int DisplayOrder)
        {
            var dbItem = DBProviderManager<DBCheckoutAttributeProvider>.Provider.UpdateCheckoutAttribute(CheckoutAttributeID,
                Name, TextPrompt, IsRequired, ShippableProductRequired,
                IsTaxExempt, TaxCategoryID, AttributeControlTypeID, DisplayOrder);
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
        /// <param name="CheckoutAttributeLocalizedID">Localized checkout attribute identifier</param>
        /// <returns>Checkout attribute content</returns>
        public static CheckoutAttributeLocalized GetCheckoutAttributeLocalizedByID(int CheckoutAttributeLocalizedID)
        {
            if (CheckoutAttributeLocalizedID == 0)
                return null;

            var dbItem = DBProviderManager<DBCheckoutAttributeProvider>.Provider.GetCheckoutAttributeLocalizedByID(CheckoutAttributeLocalizedID);
            var item = DBMapping(dbItem);
            return item;
        }

        /// <summary>
        /// Gets localized checkout attribute by checkout attribute id and language id
        /// </summary>
        /// <param name="CheckoutAttributeID">Checkout attribute identifier</param>
        /// <param name="LanguageID">Language identifier</param>
        /// <returns>Checkout attribute content</returns>
        public static CheckoutAttributeLocalized GetCheckoutAttributeLocalizedByCheckoutAttributeIDAndLanguageID(int CheckoutAttributeID, int LanguageID)
        {
            if (CheckoutAttributeID == 0 || LanguageID == 0)
                return null;

            var dbItem = DBProviderManager<DBCheckoutAttributeProvider>.Provider.GetCheckoutAttributeLocalizedByCheckoutAttributeIDAndLanguageID(CheckoutAttributeID, LanguageID);
            var item = DBMapping(dbItem);
            return item;
        }

        /// <summary>
        /// Inserts a localized checkout attribute
        /// </summary>
        /// <param name="CheckoutAttributeID">Checkout attribute identifier</param>
        /// <param name="LanguageID">Language identifier</param>
        /// <param name="Name">Name text</param>
        /// <param name="TextPrompt">Text prompt</param>
        /// <returns>Checkout attribute content</returns>
        public static CheckoutAttributeLocalized InsertCheckoutAttributeLocalized(int CheckoutAttributeID,
            int LanguageID, string Name, string TextPrompt)
        {
            var dbItem = DBProviderManager<DBCheckoutAttributeProvider>.Provider.InsertCheckoutAttributeLocalized(CheckoutAttributeID,
                LanguageID, Name, TextPrompt);
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
        /// <param name="CheckoutAttributeLocalizedID">Localized checkout attribute identifier</param>
        /// <param name="CheckoutAttributeID">Checkout attribute identifier</param>
        /// <param name="LanguageID">Language identifier</param>
        /// <param name="Name">Name text</param>
        /// <param name="TextPrompt">Text prompt</param>
        /// <returns>Checkout attribute content</returns>
        public static CheckoutAttributeLocalized UpdateCheckoutAttributeLocalized(int CheckoutAttributeLocalizedID,
            int CheckoutAttributeID, int LanguageID, string Name, string TextPrompt)
        {
            var dbItem = DBProviderManager<DBCheckoutAttributeProvider>.Provider.UpdateCheckoutAttributeLocalized(CheckoutAttributeLocalizedID,
                CheckoutAttributeID, LanguageID, Name, TextPrompt);
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
        /// <param name="CheckoutAttributeValueID">Checkout attribute value identifier</param>
        public static void DeleteCheckoutAttributeValue(int CheckoutAttributeValueID)
        {
            DBProviderManager<DBCheckoutAttributeProvider>.Provider.DeleteCheckoutAttributeValue(CheckoutAttributeValueID);
            if (CheckoutAttributeManager.CacheEnabled)
            {
                NopCache.RemoveByPattern(CHECKOUTATTRIBUTES_PATTERN_KEY);
                NopCache.RemoveByPattern(CHECKOUTATTRIBUTEVALUES_PATTERN_KEY);
            }
        }

        /// <summary>
        /// Gets checkout attribute values by checkout attribute identifier
        /// </summary>
        /// <param name="CheckoutAttributeID">The checkout attribute identifier</param>
        /// <param name="LanguageID">Language identifier</param>
        /// <returns>Checkout attribute value collection</returns>
        public static CheckoutAttributeValueCollection GetCheckoutAttributeValues(int CheckoutAttributeID)
        {
            int languageId = 0;
            if (NopContext.Current != null)
                languageId = NopContext.Current.WorkingLanguage.LanguageID;
            return GetCheckoutAttributeValues(CheckoutAttributeID, languageId);
        }

        /// <summary>
        /// Gets checkout attribute values by checkout attribute identifier
        /// </summary>
        /// <param name="CheckoutAttributeID">The checkout attribute identifier</param>
        /// <param name="LanguageID">Language identifier</param>
        /// <returns>Checkout attribute value collection</returns>
        public static CheckoutAttributeValueCollection GetCheckoutAttributeValues(int CheckoutAttributeID, int LanguageID)
        {
            string key = string.Format(CHECKOUTATTRIBUTEVALUES_ALL_KEY, CheckoutAttributeID, LanguageID);
            object obj2 = NopCache.Get(key);
            if (CheckoutAttributeManager.CacheEnabled && (obj2 != null))
            {
                return (CheckoutAttributeValueCollection)obj2;
            }

            var dbCollection = DBProviderManager<DBCheckoutAttributeProvider>.Provider.GetCheckoutAttributeValues(CheckoutAttributeID, LanguageID);
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
        /// <param name="CheckoutAttributeValueID">Checkout attribute value identifier</param>
        /// <returns>Checkout attribute value</returns>
        public static CheckoutAttributeValue GetCheckoutAttributeValueByID(int CheckoutAttributeValueID)
        {
            int languageId = 0;
            if (NopContext.Current != null)
                languageId = NopContext.Current.WorkingLanguage.LanguageID;
            return GetCheckoutAttributeValueByID(CheckoutAttributeValueID, languageId);
        }

        /// <summary>
        /// Gets a checkout attribute value
        /// </summary>
        /// <param name="CheckoutAttributeValueID">Checkout attribute value identifier</param>
        /// <param name="LanguageID">Language identifier</param>
        /// <returns>Checkout attribute value</returns>
        public static CheckoutAttributeValue GetCheckoutAttributeValueByID(int CheckoutAttributeValueID, int LanguageID)
        {
            if (CheckoutAttributeValueID == 0)
                return null;

            string key = string.Format(CHECKOUTATTRIBUTEVALUES_BY_ID_KEY, CheckoutAttributeValueID, LanguageID);
            object obj2 = NopCache.Get(key);
            if (CheckoutAttributeManager.CacheEnabled && (obj2 != null))
            {
                return (CheckoutAttributeValue)obj2;
            }

            var dbItem = DBProviderManager<DBCheckoutAttributeProvider>.Provider.GetCheckoutAttributeValueByID(CheckoutAttributeValueID, LanguageID);
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
        /// <param name="CheckoutAttributeID">The checkout attribute identifier</param>
        /// <param name="Name">The checkout attribute name</param>
        /// <param name="PriceAdjustment">The price adjustment</param>
        /// <param name="WeightAdjustment">The weight adjustment</param>
        /// <param name="IsPreSelected">The value indicating whether the value is pre-selected</param>
        /// <param name="DisplayOrder">The display order</param>
        /// <returns>Checkout attribute value</returns>
        public static CheckoutAttributeValue InsertCheckoutAttributeValue(int CheckoutAttributeID,
            string Name, decimal PriceAdjustment, decimal WeightAdjustment,
            bool IsPreSelected, int DisplayOrder)
        {
            var dbItem = DBProviderManager<DBCheckoutAttributeProvider>.Provider.InsertCheckoutAttributeValue(CheckoutAttributeID,
                Name, PriceAdjustment, WeightAdjustment, IsPreSelected, DisplayOrder);
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
        /// <param name="CheckoutAttributeValueID">The checkout attribute value identifier</param>
        /// <param name="CheckoutAttributeID">The checkout attribute identifier</param>
        /// <param name="Name">The checkout attribute name</param>
        /// <param name="PriceAdjustment">The price adjustment</param>
        /// <param name="WeightAdjustment">The weight adjustment</param>
        /// <param name="IsPreSelected">The value indicating whether the value is pre-selected</param>
        /// <param name="DisplayOrder">The display order</param>
        /// <returns>Checkout attribute value</returns>
        public static CheckoutAttributeValue UpdateCheckoutAttributeValue(int CheckoutAttributeValueID,
            int CheckoutAttributeID, string Name, decimal PriceAdjustment,
            decimal WeightAdjustment, bool IsPreSelected, int DisplayOrder)
        {
            var dbItem = DBProviderManager<DBCheckoutAttributeProvider>.Provider.UpdateCheckoutAttributeValue(CheckoutAttributeValueID,
                CheckoutAttributeID, Name, PriceAdjustment, WeightAdjustment, IsPreSelected, DisplayOrder);
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
        /// <param name="CheckoutAttributeValueLocalizedID">Localized checkout attribute value identifier</param>
        /// <returns>Localized checkout attribute value</returns>
        public static CheckoutAttributeValueLocalized GetCheckoutAttributeValueLocalizedByID(int CheckoutAttributeValueLocalizedID)
        {
            if (CheckoutAttributeValueLocalizedID == 0)
                return null;

            var dbItem = DBProviderManager<DBCheckoutAttributeProvider>.Provider.GetCheckoutAttributeValueLocalizedByID(CheckoutAttributeValueLocalizedID);
            var item = DBMapping(dbItem);
            return item;
        }

        /// <summary>
        /// Gets localized checkout attribute value by checkout attribute value id and language id
        /// </summary>
        /// <param name="CheckoutAttributeValueID">Checkout attribute value identifier</param>
        /// <param name="LanguageID">Language identifier</param>
        /// <returns>Localized checkout attribute value</returns>
        public static CheckoutAttributeValueLocalized GetCheckoutAttributeValueLocalizedByCheckoutAttributeValueIDAndLanguageID(int CheckoutAttributeValueID, int LanguageID)
        {
            if (CheckoutAttributeValueID == 0 || LanguageID == 0)
                return null;

            var dbItem = DBProviderManager<DBCheckoutAttributeProvider>.Provider.GetCheckoutAttributeValueLocalizedByCheckoutAttributeValueIDAndLanguageID(CheckoutAttributeValueID, LanguageID);
            var item = DBMapping(dbItem);
            return item;
        }

        /// <summary>
        /// Inserts a localized checkout attribute value
        /// </summary>
        /// <param name="CheckoutAttributeValueID">Checkout attribute value identifier</param>
        /// <param name="LanguageID">Language identifier</param>
        /// <param name="Name">Name text</param>
        /// <returns>Localized checkout attribute value</returns>
        public static CheckoutAttributeValueLocalized InsertCheckoutAttributeValueLocalized(int CheckoutAttributeValueID,
            int LanguageID, string Name)
        {
            var dbItem = DBProviderManager<DBCheckoutAttributeProvider>.Provider.InsertCheckoutAttributeValueLocalized(CheckoutAttributeValueID,
                LanguageID, Name);
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
        /// <param name="CheckoutAttributeValueLocalizedID">Localized checkout attribute value identifier</param>
        /// <param name="CheckoutAttributeValueID">Checkout attribute value identifier</param>
        /// <param name="LanguageID">Language identifier</param>
        /// <param name="Name">Name text</param>
        /// <returns>Localized checkout attribute value</returns>
        public static CheckoutAttributeValueLocalized UpdateCheckoutAttributeValueLocalized(int CheckoutAttributeValueLocalizedID,
            int CheckoutAttributeValueID, int LanguageID, string Name)
        {
            var dbItem = DBProviderManager<DBCheckoutAttributeProvider>.Provider.UpdateCheckoutAttributeValueLocalized(CheckoutAttributeValueLocalizedID,
                CheckoutAttributeValueID, LanguageID, Name);
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
