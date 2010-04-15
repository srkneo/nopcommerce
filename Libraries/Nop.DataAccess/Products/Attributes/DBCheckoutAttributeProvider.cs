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
        /// <param name="CheckoutAttributeID">Checkout attribute identifier</param>
        public abstract void DeleteCheckoutAttribute(int CheckoutAttributeID);

        /// <summary>
        /// Gets all checkout attributes
        /// </summary>
        /// <param name="LanguageID">Language identifier</param>
        /// <param name="DontLoadShippableProductRequired">Value indicating whether to do not load attributes for checkout attibutes which require shippable products</param>
        /// <returns>Checkout attribute collection</returns>
        public abstract DBCheckoutAttributeCollection GetAllCheckoutAttributes(int LanguageID, bool DontLoadShippableProductRequired);

        /// <summary>
        /// Gets a checkout attribute 
        /// </summary>
        /// <param name="CheckoutAttributeID">Checkout attribute identifier</param>
        /// <param name="LanguageID">Language identifier</param>
        /// <returns>Checkout attribute</returns>
        public abstract DBCheckoutAttribute GetCheckoutAttributeByID(int CheckoutAttributeID, int LanguageID);

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
        public abstract DBCheckoutAttribute InsertCheckoutAttribute(string Name,
            string TextPrompt, bool IsRequired, bool ShippableProductRequired,
            bool IsTaxExempt, int TaxCategoryID, int AttributeControlTypeID,
            int DisplayOrder);

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
        public abstract DBCheckoutAttribute UpdateCheckoutAttribute(int CheckoutAttributeID,
            string Name, string TextPrompt, bool IsRequired, bool ShippableProductRequired,
            bool IsTaxExempt, int TaxCategoryID, int AttributeControlTypeID,
            int DisplayOrder);
        
        /// <summary>
        /// Gets localized checkout attribute by id
        /// </summary>
        /// <param name="CheckoutAttributeLocalizedID">Localized checkout attribute identifier</param>
        /// <returns>Checkout attribute content</returns>
        public abstract DBCheckoutAttributeLocalized GetCheckoutAttributeLocalizedByID(int CheckoutAttributeLocalizedID);

        /// <summary>
        /// Gets localized checkout attribute by checkout attribute id and language id
        /// </summary>
        /// <param name="CheckoutAttributeID">Checkout attribute identifier</param>
        /// <param name="LanguageID">Language identifier</param>
        /// <returns>Checkout attribute content</returns>
        public abstract DBCheckoutAttributeLocalized GetCheckoutAttributeLocalizedByCheckoutAttributeIDAndLanguageID(int CheckoutAttributeID, int LanguageID);

        /// <summary>
        /// Inserts a localized checkout attribute
        /// </summary>
        /// <param name="CheckoutAttributeID">Checkout attribute identifier</param>
        /// <param name="LanguageID">Language identifier</param>
        /// <param name="Name">Name text</param>
        /// <param name="TextPrompt">Text prompt</param>
        /// <returns>Checkout attribute content</returns>
        public abstract DBCheckoutAttributeLocalized InsertCheckoutAttributeLocalized(int CheckoutAttributeID,
            int LanguageID, string Name, string TextPrompt);

        /// <summary>
        /// Update a localized checkout attribute
        /// </summary>
        /// <param name="CheckoutAttributeLocalizedID">Localized checkout attribute identifier</param>
        /// <param name="CheckoutAttributeID">Checkout attribute identifier</param>
        /// <param name="LanguageID">Language identifier</param>
        /// <param name="Name">Name text</param>
        /// <param name="TextPrompt">Text prompt</param>
        /// <returns>Checkout attribute content</returns>
        public abstract DBCheckoutAttributeLocalized UpdateCheckoutAttributeLocalized(int CheckoutAttributeLocalizedID,
            int CheckoutAttributeID, int LanguageID, string Name, string TextPrompt);

        /// <summary>
        /// Deletes a checkout attribute value
        /// </summary>
        /// <param name="CheckoutAttributeValueID">Checkout attribute value identifier</param>
        public abstract void DeleteCheckoutAttributeValue(int CheckoutAttributeValueID);

        /// <summary>
        /// Gets checkout attribute values by checkout attribute identifier
        /// </summary>
        /// <param name="CheckoutAttributeID">The checkout attribute identifier</param>
        /// <param name="LanguageID">Language identifier</param>
        /// <returns>Checkout attribute value collection</returns>
        public abstract DBCheckoutAttributeValueCollection GetCheckoutAttributeValues(int CheckoutAttributeID, int LanguageID);

        /// <summary>
        /// Gets a checkout attribute value
        /// </summary>
        /// <param name="CheckoutAttributeValueID">Checkout attribute value identifier</param>
        /// <param name="LanguageID">Language identifier</param>
        /// <returns>Checkout attribute value</returns>
        public abstract DBCheckoutAttributeValue GetCheckoutAttributeValueByID(int CheckoutAttributeValueID, int LanguageID);

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
        public abstract DBCheckoutAttributeValue InsertCheckoutAttributeValue(int CheckoutAttributeID,
            string Name, decimal PriceAdjustment, decimal WeightAdjustment,
            bool IsPreSelected, int DisplayOrder);

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
        public abstract DBCheckoutAttributeValue UpdateCheckoutAttributeValue(int CheckoutAttributeValueID,
            int CheckoutAttributeID, string Name, decimal PriceAdjustment, 
            decimal WeightAdjustment, bool IsPreSelected, int DisplayOrder);

        /// <summary>
        /// Gets localized checkout attribute value by id
        /// </summary>
        /// <param name="CheckoutAttributeValueLocalizedID">Localized checkout attribute value identifier</param>
        /// <returns>Localized checkout attribute value</returns>
        public abstract DBCheckoutAttributeValueLocalized GetCheckoutAttributeValueLocalizedByID(int CheckoutAttributeValueLocalizedID);

        /// <summary>
        /// Gets localized checkout attribute value by checkout attribute value id and language id
        /// </summary>
        /// <param name="CheckoutAttributeValueID">Checkout attribute value identifier</param>
        /// <param name="LanguageID">Language identifier</param>
        /// <returns>Localized checkout attribute value</returns>
        public abstract DBCheckoutAttributeValueLocalized GetCheckoutAttributeValueLocalizedByCheckoutAttributeValueIDAndLanguageID(int CheckoutAttributeValueID, int LanguageID);

        /// <summary>
        /// Inserts a localized checkout attribute value
        /// </summary>
        /// <param name="CheckoutAttributeValueID">Checkout attribute value identifier</param>
        /// <param name="LanguageID">Language identifier</param>
        /// <param name="Name">Name text</param>
        /// <returns>Localized checkout attribute value</returns>
        public abstract DBCheckoutAttributeValueLocalized InsertCheckoutAttributeValueLocalized(int CheckoutAttributeValueID,
            int LanguageID, string Name);

        /// <summary>
        /// Update a localized checkout attribute value
        /// </summary>
        /// <param name="CheckoutAttributeValueLocalizedID">Localized checkout attribute value identifier</param>
        /// <param name="CheckoutAttributeValueID">Checkout attribute value identifier</param>
        /// <param name="LanguageID">Language identifier</param>
        /// <param name="Name">Name text</param>
        /// <returns>Localized checkout attribute value</returns>
        public abstract DBCheckoutAttributeValueLocalized UpdateCheckoutAttributeValueLocalized(int CheckoutAttributeValueLocalizedID,
            int CheckoutAttributeValueID, int LanguageID, string Name);

        #endregion
    } 
}
