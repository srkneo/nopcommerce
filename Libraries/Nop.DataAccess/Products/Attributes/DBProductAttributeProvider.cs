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
    /// Acts as a base class for deriving custom product attribute provider
    /// </summary>
    [DBProviderSectionName("nopDataProviders/ProductAttributeProvider")]
    public abstract partial class DBProductAttributeProvider : BaseDBProvider
    {
        #region Methods

        /// <summary>
        /// Deletes a product attribute
        /// </summary>
        /// <param name="productAttributeId">Product attribute identifier</param>
        public abstract void DeleteProductAttribute(int productAttributeId);

        /// <summary>
        /// Gets all product attributes
        /// </summary>
        /// <param name="languageId">Language identifier</param>
        /// <returns>Product attribute collection</returns>
        public abstract DBProductAttributeCollection GetAllProductAttributes(int languageId);

        /// <summary>
        /// Gets a product attribute 
        /// </summary>
        /// <param name="productAttributeId">Product attribute identifier</param>
        /// <param name="languageId">Language identifier</param>
        /// <returns>Product attribute </returns>
        public abstract DBProductAttribute GetProductAttributeById(int productAttributeId, int languageId);

        /// <summary>
        /// Inserts a product attribute
        /// </summary>
        /// <param name="name">The name</param>
        /// <param name="description">The description</param>
        /// <returns>Product attribute </returns>
        public abstract DBProductAttribute InsertProductAttribute(string name,
            string description);

        /// <summary>
        /// Updates the product attribute
        /// </summary>
        /// <param name="productAttributeId">Product attribute identifier</param>
        /// <param name="name">The name</param>
        /// <param name="description">The description</param>
        /// <returns>Product attribute </returns>
        public abstract DBProductAttribute UpdateProductAttribute(int productAttributeId,
            string name, string description);
        
        /// <summary>
        /// Gets localized product attribute by id
        /// </summary>
        /// <param name="productAttributeLocalizedId">Localized product attribute identifier</param>
        /// <returns>Product attribute content</returns>
        public abstract DBProductAttributeLocalized GetProductAttributeLocalizedById(int productAttributeLocalizedId);

        /// <summary>
        /// Gets localized product attribute by product attribute id and language id
        /// </summary>
        /// <param name="productAttributeId">Product attribute identifier</param>
        /// <param name="languageId">Language identifier</param>
        /// <returns>Product attribute content</returns>
        public abstract DBProductAttributeLocalized GetProductAttributeLocalizedByProductAttributeIdAndLanguageId(int productAttributeId, int languageId);

        /// <summary>
        /// Inserts a localized product attribute
        /// </summary>
        /// <param name="productAttributeId">Product attribute identifier</param>
        /// <param name="languageId">Language identifier</param>
        /// <param name="name">Name text</param>
        /// <param name="description">Description text</param>
        /// <returns>Product attribute content</returns>
        public abstract DBProductAttributeLocalized InsertProductAttributeLocalized(int productAttributeId,
            int languageId, string name, string description);

        /// <summary>
        /// Update a localized product attribute
        /// </summary>
        /// <param name="productAttributeLocalizedId">Localized product attribute identifier</param>
        /// <param name="productAttributeId">Product attribute identifier</param>
        /// <param name="languageId">Language identifier</param>
        /// <param name="name">Name text</param>
        /// <param name="description">Description text</param>
        /// <returns>Product attribute content</returns>
        public abstract DBProductAttributeLocalized UpdateProductAttributeLocalized(int productAttributeLocalizedId,
            int productAttributeId, int languageId, string name, string description);

        /// <summary>
        /// Deletes a product variant attribute value
        /// </summary>
        /// <param name="productVariantAttributeValueId">Product variant attribute value identifier</param>
        public abstract void DeleteProductVariantAttributeValue(int productVariantAttributeValueId);

        /// <summary>
        /// Gets product variant attribute values by product identifier
        /// </summary>
        /// <param name="productVariantAttributeId">The product variant attribute mapping identifier</param>
        /// <param name="languageId">Language identifier</param>
        /// <returns>Product variant attribute mapping collection</returns>
        public abstract DBProductVariantAttributeValueCollection GetProductVariantAttributeValues(int productVariantAttributeId, int languageId);

        /// <summary>
        /// Gets a product variant attribute value
        /// </summary>
        /// <param name="productVariantAttributeValueId">Product variant attribute value identifier</param>
        /// <param name="languageId">Language identifier</param>
        /// <returns>Product variant attribute value</returns>
        public abstract DBProductVariantAttributeValue GetProductVariantAttributeValueById(int productVariantAttributeValueId, int languageId);

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
        public abstract DBProductVariantAttributeValue InsertProductVariantAttributeValue(int productVariantAttributeId,
            string name, decimal priceAdjustment, decimal weightAdjustment,
            bool isPreSelected, int displayOrder);

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
        public abstract DBProductVariantAttributeValue UpdateProductVariantAttributeValue(int productVariantAttributeValueId,
            int productVariantAttributeId, string name,
            decimal priceAdjustment, decimal weightAdjustment,
            bool isPreSelected, int displayOrder);

        /// <summary>
        /// Gets localized product variant attribute value by id
        /// </summary>
        /// <param name="productVariantAttributeValueLocalizedId">Localized product variant attribute value identifier</param>
        /// <returns>Localized product variant attribute value</returns>
        public abstract DBProductVariantAttributeValueLocalized GetProductVariantAttributeValueLocalizedById(int productVariantAttributeValueLocalizedId);

        /// <summary>
        /// Gets localized product variant attribute value by product variant attribute value id and language id
        /// </summary>
        /// <param name="productVariantAttributeValueId">Product variant attribute value identifier</param>
        /// <param name="languageId">Language identifier</param>
        /// <returns>Localized product variant attribute value</returns>
        public abstract DBProductVariantAttributeValueLocalized GetProductVariantAttributeValueLocalizedByProductVariantAttributeValueIdAndLanguageId(int productVariantAttributeValueId, int languageId);

        /// <summary>
        /// Inserts a localized product variant attribute value
        /// </summary>
        /// <param name="productVariantAttributeValueId">Product variant attribute value identifier</param>
        /// <param name="languageId">Language identifier</param>
        /// <param name="name">Name text</param>
        /// <returns>Localized product variant attribute value</returns>
        public abstract DBProductVariantAttributeValueLocalized InsertProductVariantAttributeValueLocalized(int productVariantAttributeValueId,
            int languageId, string name);

        /// <summary>
        /// Update a localized product variant attribute value
        /// </summary>
        /// <param name="productVariantAttributeValueLocalizedId">Localized product variant attribute value identifier</param>
        /// <param name="productVariantAttributeValueId">Product variant attribute value identifier</param>
        /// <param name="languageId">Language identifier</param>
        /// <param name="name">Name text</param>
        /// <returns>Localized product variant attribute value</returns>
        public abstract DBProductVariantAttributeValueLocalized UpdateProductVariantAttributeValueLocalized(int productVariantAttributeValueLocalizedId,
            int productVariantAttributeValueId, int languageId, string name);
        
        #endregion
    } 
}
