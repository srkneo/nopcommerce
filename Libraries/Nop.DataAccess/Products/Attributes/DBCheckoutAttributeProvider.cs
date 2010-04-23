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
using System.Collections.Specialized;
using System.Configuration;
using System.Configuration.Provider;
using System.Data;
using System.Data.Common;
using System.Globalization;
using System.Text;
using System.Web.Configuration;
using System.Web.Hosting;

namespace NopSolutions.NopCommerce.DataAccess.Products.Attributes
{
    /// <summary>
    /// Acts as a base class for deriving custom checkout attribute provider
    /// </summary>
    [DBProviderSectionName("nopDataProviders/CheckoutAttributeProvider")]
    public abstract partial class DBCheckoutAttributeProvider : BaseDBProvider
    {
        #region Methods

        /// <summary>
        /// Deletes a checkout attribute
        /// </summary>
        /// <param name="checkoutAttributeId">Checkout attribute identifier</param>
        public abstract void DeleteCheckoutAttribute(int checkoutAttributeId);

        /// <summary>
        /// Gets all checkout attributes
        /// </summary>
        /// <param name="languageId">Language identifier</param>
        /// <param name="dontLoadShippableProductRequired">Value indicating whether to do not load attributes for checkout attibutes which require shippable products</param>
        /// <returns>Checkout attribute collection</returns>
        public abstract DBCheckoutAttributeCollection GetAllCheckoutAttributes(int languageId, bool dontLoadShippableProductRequired);

        /// <summary>
        /// Gets a checkout attribute 
        /// </summary>
        /// <param name="checkoutAttributeId">Checkout attribute identifier</param>
        /// <param name="languageId">Language identifier</param>
        /// <returns>Checkout attribute</returns>
        public abstract DBCheckoutAttribute GetCheckoutAttributeById(int checkoutAttributeId, int languageId);

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
        public abstract DBCheckoutAttribute InsertCheckoutAttribute(string name,
            string textPrompt, bool isRequired, bool shippableProductRequired,
            bool isTaxExempt, int taxCategoryId, int attributeControlTypeId,
            int displayOrder);

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
        public abstract DBCheckoutAttribute UpdateCheckoutAttribute(int checkoutAttributeId,
            string name, string textPrompt, bool isRequired, bool shippableProductRequired,
            bool isTaxExempt, int taxCategoryId, int attributeControlTypeId,
            int displayOrder);
        
        /// <summary>
        /// Gets localized checkout attribute by id
        /// </summary>
        /// <param name="checkoutAttributeLocalizedId">Localized checkout attribute identifier</param>
        /// <returns>Checkout attribute content</returns>
        public abstract DBCheckoutAttributeLocalized GetCheckoutAttributeLocalizedById(int checkoutAttributeLocalizedId);

        /// <summary>
        /// Gets localized checkout attribute by checkout attribute id and language id
        /// </summary>
        /// <param name="checkoutAttributeId">Checkout attribute identifier</param>
        /// <param name="languageId">Language identifier</param>
        /// <returns>Checkout attribute content</returns>
        public abstract DBCheckoutAttributeLocalized GetCheckoutAttributeLocalizedByCheckoutAttributeIdAndLanguageId(int checkoutAttributeId, int languageId);

        /// <summary>
        /// Inserts a localized checkout attribute
        /// </summary>
        /// <param name="checkoutAttributeId">Checkout attribute identifier</param>
        /// <param name="languageId">Language identifier</param>
        /// <param name="name">Name text</param>
        /// <param name="textPrompt">Text prompt</param>
        /// <returns>Checkout attribute content</returns>
        public abstract DBCheckoutAttributeLocalized InsertCheckoutAttributeLocalized(int checkoutAttributeId,
            int languageId, string name, string textPrompt);

        /// <summary>
        /// Update a localized checkout attribute
        /// </summary>
        /// <param name="checkoutAttributeLocalizedId">Localized checkout attribute identifier</param>
        /// <param name="checkoutAttributeId">Checkout attribute identifier</param>
        /// <param name="languageId">Language identifier</param>
        /// <param name="name">Name text</param>
        /// <param name="textPrompt">Text prompt</param>
        /// <returns>Checkout attribute content</returns>
        public abstract DBCheckoutAttributeLocalized UpdateCheckoutAttributeLocalized(int checkoutAttributeLocalizedId,
            int checkoutAttributeId, int languageId, string name, string textPrompt);

        /// <summary>
        /// Deletes a checkout attribute value
        /// </summary>
        /// <param name="checkoutAttributeValueId">Checkout attribute value identifier</param>
        public abstract void DeleteCheckoutAttributeValue(int checkoutAttributeValueId);

        /// <summary>
        /// Gets checkout attribute values by checkout attribute identifier
        /// </summary>
        /// <param name="checkoutAttributeId">The checkout attribute identifier</param>
        /// <param name="languageId">Language identifier</param>
        /// <returns>Checkout attribute value collection</returns>
        public abstract DBCheckoutAttributeValueCollection GetCheckoutAttributeValues(int checkoutAttributeId, int languageId);

        /// <summary>
        /// Gets a checkout attribute value
        /// </summary>
        /// <param name="checkoutAttributeValueId">Checkout attribute value identifier</param>
        /// <param name="languageId">Language identifier</param>
        /// <returns>Checkout attribute value</returns>
        public abstract DBCheckoutAttributeValue GetCheckoutAttributeValueById(int checkoutAttributeValueId, int languageId);

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
        public abstract DBCheckoutAttributeValue InsertCheckoutAttributeValue(int checkoutAttributeId,
            string name, decimal priceAdjustment, decimal weightAdjustment,
            bool isPreSelected, int displayOrder);

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
        public abstract DBCheckoutAttributeValue UpdateCheckoutAttributeValue(int checkoutAttributeValueId,
            int checkoutAttributeId, string name, decimal priceAdjustment, decimal weightAdjustment,
            bool isPreSelected, int displayOrder);

        /// <summary>
        /// Gets localized checkout attribute value by id
        /// </summary>
        /// <param name="checkoutAttributeValueLocalizedId">Localized checkout attribute value identifier</param>
        /// <returns>Localized checkout attribute value</returns>
        public abstract DBCheckoutAttributeValueLocalized GetCheckoutAttributeValueLocalizedById(int checkoutAttributeValueLocalizedId);

        /// <summary>
        /// Gets localized checkout attribute value by checkout attribute value id and language id
        /// </summary>
        /// <param name="checkoutAttributeValueId">Checkout attribute value identifier</param>
        /// <param name="languageId">Language identifier</param>
        /// <returns>Localized checkout attribute value</returns>
        public abstract DBCheckoutAttributeValueLocalized GetCheckoutAttributeValueLocalizedByCheckoutAttributeValueIdAndLanguageId(int checkoutAttributeValueId, int languageId);

        /// <summary>
        /// Inserts a localized checkout attribute value
        /// </summary>
        /// <param name="checkoutAttributeValueId">Checkout attribute value identifier</param>
        /// <param name="languageId">Language identifier</param>
        /// <param name="name">Name text</param>
        /// <returns>Localized checkout attribute value</returns>
        public abstract DBCheckoutAttributeValueLocalized InsertCheckoutAttributeValueLocalized(int checkoutAttributeValueId,
            int languageId, string name);

        /// <summary>
        /// Update a localized checkout attribute value
        /// </summary>
        /// <param name="checkoutAttributeValueLocalizedId">Localized checkout attribute value identifier</param>
        /// <param name="checkoutAttributeValueId">Checkout attribute value identifier</param>
        /// <param name="languageId">Language identifier</param>
        /// <param name="name">Name text</param>
        /// <returns>Localized checkout attribute value</returns>
        public abstract DBCheckoutAttributeValueLocalized UpdateCheckoutAttributeValueLocalized(int checkoutAttributeValueLocalizedId,
            int checkoutAttributeValueId, int languageId, string name);

        #endregion
    } 
}
