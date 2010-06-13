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
using System.Configuration;
using System.Configuration.Provider;
using System.Web.Hosting;
using System.Web.Configuration;
using System.Collections.Specialized;

namespace NopSolutions.NopCommerce.DataAccess.Manufacturers
{
    /// <summary>
    /// Acts as a base class for deriving custom manufacturer provider
    /// </summary>
    [DBProviderSectionName("nopDataProviders/ManufacturerProvider")]
    public abstract partial class DBManufacturerProvider : BaseDBProvider
    {
        #region Methods

        /// <summary>
        /// Gets all manufacturers
        /// </summary>
        /// <param name="showHidden">A value indicating whether to show hidden records</param>
        /// <param name="languageId">Language identifier</param>
        /// <returns>Manufacturer collection</returns>
        public abstract DBManufacturerCollection GetAllManufacturers(bool showHidden, int languageId);

        /// <summary>
        /// Gets a manufacturer
        /// </summary>
        /// <param name="manufacturerId">Manufacturer identifier</param>
        /// <param name="languageId">Language identifier</param>
        /// <returns>Manufacturer</returns>
        public abstract DBManufacturer GetManufacturerById(int manufacturerId, int languageId);

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
        public abstract DBManufacturer InsertManufacturer(string name, string description,
            int templateId, string metaKeywords, string metaDescription, string metaTitle,
            string seName, int pictureId, int pageSize, string priceRanges, 
            bool published, bool deleted, int displayOrder, 
            DateTime createdOn, DateTime updatedOn);

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
        public abstract DBManufacturer UpdateManufacturer(int manufacturerId,
            string name, string description,
            int templateId, string metaKeywords, string metaDescription, string metaTitle,
            string seName, int pictureId, int pageSize, string priceRanges,
            bool published, bool deleted, int displayOrder,
            DateTime createdOn, DateTime updatedOn);

        /// <summary>
        /// Gets localized manufacturer by id
        /// </summary>
        /// <param name="manufacturerLocalizedId">Localized manufacturer identifier</param>
        /// <returns>Manufacturer content</returns>
        public abstract DBManufacturerLocalized GetManufacturerLocalizedById(int manufacturerLocalizedId);

        /// <summary>
        /// Gets localized manufacturer by manufacturer id and language id
        /// </summary>
        /// <param name="manufacturerId">Manufacturer identifier</param>
        /// <param name="languageId">Language identifier</param>
        /// <returns>Manufacturer content</returns>
        public abstract DBManufacturerLocalized GetManufacturerLocalizedByManufacturerIdAndLanguageId(int manufacturerId, int languageId);

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
        public abstract DBManufacturerLocalized InsertManufacturerLocalized(int manufacturerId,
            int languageId, string name, string description,
            string metaKeywords, string metaDescription, string metaTitle, string seName);

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
        public abstract DBManufacturerLocalized UpdateManufacturerLocalized(int manufacturerLocalizedId,
            int manufacturerId, int languageId, string name, string description,
            string metaKeywords, string metaDescription, string metaTitle, string seName);
      
        /// <summary>
        /// Gets product product manufacturer collection
        /// </summary>
        /// <param name="manufacturerId">Manufacturer identifier</param>
        /// <param name="showHidden">A value indicating whether to show hidden records</param>
        /// <returns>Product manufacturer collection</returns>
        public abstract DBProductManufacturerCollection GetProductManufacturersByManufacturerId(int manufacturerId, bool showHidden);

        /// <summary>
        /// Gets a product manufacturer mapping collection
        /// </summary>
        /// <param name="productId">Product identifier</param>
        /// <param name="showHidden">A value indicating whether to show hidden records</param>
        /// <returns>Product manufacturer mapping collection</returns>
        public abstract DBProductManufacturerCollection GetProductManufacturersByProductId(int productId, bool showHidden);

        #endregion
    }
}
