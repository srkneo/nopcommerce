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


namespace NopSolutions.NopCommerce.DataAccess.Categories
{
    /// <summary>
    /// Acts as a base class for deriving custom category provider
    /// </summary>
    [DBProviderSectionName("nopDataProviders/CategoryProvider")]
    public abstract partial class DBCategoryProvider : BaseDBProvider
    {
        #region Methods
        /// <summary>
        /// Gets all categories
        /// </summary>
        /// <param name="parentCategoryId">Parent category identifier</param>
        /// <param name="showHidden">A value indicating whether to show hidden records</param>
        /// <param name="languageId">Language identifier</param>
        /// <returns>Category collection</returns>
        public abstract DBCategoryCollection GetAllCategories(int parentCategoryId, 
            bool showHidden, int languageId);

        /// <summary>
        /// Gets all categories displayed on the home page
        /// </summary>
        /// <param name="showHidden">A value indicating whether to show hidden records</param>
        /// <param name="languageId">Language identifier</param>
        /// <returns>Category collection</returns>
        public abstract DBCategoryCollection GetAllCategoriesDisplayedOnHomePage(bool showHidden, int languageId);

        /// <summary>
        /// Gets a category
        /// </summary>
        /// <param name="categoryId">Category identifier</param>
        /// <param name="languageId">Language identifier</param>
        /// <returns>Category</returns>
        public abstract DBCategory GetCategoryById(int categoryId, int languageId);
        
        /// <summary>
        /// Inserts category
        /// </summary>
        /// <param name="name">The category name</param>
        /// <param name="description">The description</param>
        /// <param name="templateId">The template identifier</param>
        /// <param name="metaKeywords">The meta keywords</param>
        /// <param name="metaDescription">The meta description</param>
        /// <param name="metaTitle">The meta title</param>
        /// <param name="seName">The search-engine name</param>
        /// <param name="parentCategoryId">The parent category identifier</param>
        /// <param name="pictureId">The picture identifier</param>
        /// <param name="pageSize">The page size</param>
        /// <param name="priceRanges">The price ranges</param>
        /// <param name="showOnHomePage">A value indicating whether the category will be shown on home page</param>
        /// <param name="published">A value indicating whether the entity is published</param>
        /// <param name="deleted">A value indicating whether the entity has been deleted</param>
        /// <param name="displayOrder">The display order</param>
        /// <param name="createdOn">The date and time of instance creation</param>
        /// <param name="updatedOn">The date and time of instance update</param>
        /// <returns>Category</returns>
        public abstract DBCategory InsertCategory(string name, string description,
            int templateId, string metaKeywords, string metaDescription, string metaTitle,
            string seName, int parentCategoryId, int pictureId,
            int pageSize, string priceRanges, bool showOnHomePage, bool published, bool deleted,
            int displayOrder, DateTime createdOn, DateTime updatedOn);

        /// <summary>
        /// Updates the category
        /// </summary>
        /// <param name="categoryId">Category identifier</param>
        /// <param name="name">The category name</param>
        /// <param name="description">The description</param>
        /// <param name="templateId">The template identifier</param>
        /// <param name="metaKeywords">The meta keywords</param>
        /// <param name="metaDescription">The meta description</param>
        /// <param name="metaTitle">The meta title</param>
        /// <param name="seName">The search-engine name</param>
        /// <param name="parentCategoryId">The parent category identifier</param>
        /// <param name="pictureId">The picture identifier</param>
        /// <param name="pageSize">The page size</param>
        /// <param name="priceRanges">The price ranges</param>
        /// <param name="showOnHomePage">A value indicating whether the category will be shown on home page</param>
        /// <param name="published">A value indicating whether the entity is published</param>
        /// <param name="deleted">A value indicating whether the entity has been deleted</param>
        /// <param name="displayOrder">The display order</param>
        /// <param name="createdOn">The date and time of instance creation</param>
        /// <param name="updatedOn">The date and time of instance update</param>
        /// <returns>Category</returns>
        public abstract DBCategory UpdateCategory(int categoryId, string name, string description,
            int templateId, string metaKeywords, string metaDescription, string metaTitle,
            string seName, int parentCategoryId, int pictureId,
            int pageSize, string priceRanges, bool showOnHomePage, bool published, bool deleted,
            int displayOrder, DateTime createdOn, DateTime updatedOn);

        /// <summary>
        /// Gets localized category by id
        /// </summary>
        /// <param name="categoryLocalizedId">Localized category identifier</param>
        /// <returns>Category content</returns>
        public abstract DBCategoryLocalized GetCategoryLocalizedById(int categoryLocalizedId);

        /// <summary>
        /// Gets localized category by category id and language id
        /// </summary>
        /// <param name="categoryId">Category identifier</param>
        /// <param name="languageId">Language identifier</param>
        /// <returns>Category content</returns>
        public abstract DBCategoryLocalized GetCategoryLocalizedByCategoryIdAndLanguageId(int categoryId, int languageId);

        /// <summary>
        /// Inserts a localized category
        /// </summary>
        /// <param name="categoryId">Category identifier</param>
        /// <param name="languageId">Language identifier</param>
        /// <param name="name">Name text</param>
        /// <param name="description">Description text</param>
        /// <param name="metaKeywords">Meta keywords text</param>
        /// <param name="metaDescription">Meta descriptions text</param>
        /// <param name="metaTitle">Metat title text</param>
        /// <param name="seName">Se Name text</param>
        /// <returns>Category content</returns>
        public abstract DBCategoryLocalized InsertCategoryLocalized(int categoryId, 
            int languageId, string name, string description, 
            string metaKeywords, string metaDescription, string metaTitle, 
            string seName);

        /// <summary>
        /// Update a localized category
        /// </summary>
        /// <param name="categoryLocalizedId">Localized category identifier</param>
        /// <param name="categoryId">Category identifier</param>
        /// <param name="languageId">Language identifier</param>
        /// <param name="name">Name text</param>
        /// <param name="description">Description text</param>
        /// <param name="metaKeywords">Meta keywords text</param>
        /// <param name="metaDescription">Meta descriptions text</param>
        /// <param name="metaTitle">Metat title text</param>
        /// <param name="seName">Se Name text</param>
        /// <returns>Category content</returns>
        public abstract DBCategoryLocalized UpdateCategoryLocalized(int categoryLocalizedId,
            int categoryId, int languageId, string name, string description,
            string metaKeywords, string metaDescription, string metaTitle,
            string seName);
      
        /// <summary>
        /// Gets product category mapping collection
        /// </summary>
        /// <param name="categoryId">Category identifier</param>
        /// <param name="showHidden">A value indicating whether to show hidden records</param>
        /// <returns>Product a category mapping collection</returns>
        public abstract DBProductCategoryCollection GetProductCategoriesByCategoryId(int categoryId, bool showHidden);

        /// <summary>
        /// Gets a product category mapping collection
        /// </summary>
        /// <param name="productId">Product identifier</param>
        /// <param name="showHidden">A value indicating whether to show hidden records</param>
        /// <returns>Product category mapping collection</returns>
        public abstract DBProductCategoryCollection GetProductCategoriesByProductId(int productId, bool showHidden);

        #endregion
    }
}
