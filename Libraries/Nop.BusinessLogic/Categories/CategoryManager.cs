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
using NopSolutions.NopCommerce.DataAccess.Categories;

namespace NopSolutions.NopCommerce.BusinessLogic.Categories
{
    /// <summary>
    /// Category manager
    /// </summary>
    public partial class CategoryManager
    {
        #region Constants
        private const string CATEGORIES_ALL_KEY = "Nop.category.all-{0}-{1}-{2}";
        private const string CATEGORIES_BY_ID_KEY = "Nop.category.id-{0}-{1}";
        private const string PRODUCTCATEGORIES_ALLBYCATEGORYID_KEY = "Nop.productcategory.allbycategoryid-{0}-{1}";
        private const string PRODUCTCATEGORIES_ALLBYPRODUCTID_KEY = "Nop.productcategory.allbyproductid-{0}-{1}";
        private const string PRODUCTCATEGORIES_BY_ID_KEY = "Nop.productcategory.id-{0}";
        private const string CATEGORIES_PATTERN_KEY = "Nop.category.";
        private const string PRODUCTCATEGORIES_PATTERN_KEY = "Nop.productcategory.";

        #endregion

        #region Utilities

        private static CategoryCollection DBMapping(DBCategoryCollection dbCollection)
        {
            if (dbCollection == null)
                return null;

            var collection = new CategoryCollection();
            foreach (var dbItem in dbCollection)
            {
                var item = DBMapping(dbItem);
                collection.Add(item);
            }

            return collection;
        }

        private static Category DBMapping(DBCategory dbItem)
        {
            if (dbItem == null)
                return null;

            var item = new Category();
            item.CategoryId = dbItem.CategoryId;
            item.Name = dbItem.Name;
            item.Description = dbItem.Description;
            item.TemplateId = dbItem.TemplateId;
            item.MetaKeywords = dbItem.MetaKeywords;
            item.MetaDescription = dbItem.MetaDescription;
            item.MetaTitle = dbItem.MetaTitle;
            item.SEName = dbItem.SEName;
            item.ParentCategoryId = dbItem.ParentCategoryId;
            item.PictureId = dbItem.PictureId;
            item.PageSize = dbItem.PageSize;
            item.PriceRanges = dbItem.PriceRanges;
            item.ShowOnHomePage = dbItem.ShowOnHomePage;
            item.Published = dbItem.Published;
            item.Deleted = dbItem.Deleted;
            item.DisplayOrder = dbItem.DisplayOrder;
            item.CreatedOn = dbItem.CreatedOn;
            item.UpdatedOn = dbItem.UpdatedOn;

            return item;
        }
        
        private static CategoryLocalized DBMapping(DBCategoryLocalized dbItem)
        {
            if (dbItem == null)
                return null;

            var item = new CategoryLocalized();
            item.CategoryLocalizedId = dbItem.CategoryLocalizedId;
            item.CategoryId = dbItem.CategoryId;
            item.LanguageId = dbItem.LanguageId;
            item.Name = dbItem.Name;
            item.Description = dbItem.Description;
            item.MetaKeywords = dbItem.MetaKeywords;
            item.MetaDescription = dbItem.MetaDescription;
            item.MetaTitle = dbItem.MetaTitle;
            item.SEName = dbItem.SEName;

            return item;
        }

        private static ProductCategoryCollection DBMapping(DBProductCategoryCollection dbCollection)
        {
            if (dbCollection == null)
                return null;

            var collection = new ProductCategoryCollection();
            foreach (var dbItem in dbCollection)
            {
                var item = DBMapping(dbItem);
                collection.Add(item);
            }

            return collection;
        }

        private static ProductCategory DBMapping(DBProductCategory dbItem)
        {
            if (dbItem == null)
                return null;

            var item = new ProductCategory();
            item.ProductCategoryId = dbItem.ProductCategoryId;
            item.ProductId = dbItem.ProductId;
            item.CategoryId = dbItem.CategoryId;
            item.IsFeaturedProduct = dbItem.IsFeaturedProduct;
            item.DisplayOrder = dbItem.DisplayOrder;

            return item;
        }

        #endregion

        #region Methods
        /// <summary>
        /// Marks category as deleted
        /// </summary>
        /// <param name="categoryId">Category identifier</param>
        public static void MarkCategoryAsDeleted(int categoryId)
        {
            var category = GetCategoryById(categoryId, 0);
            if (category != null)
            {
                category = UpdateCategory(category.CategoryId, category.Name, 
                    category.Description, category.TemplateId, category.MetaKeywords,
                     category.MetaDescription, category.MetaTitle, category.SEName, category.ParentCategoryId,
                     category.PictureId, category.PageSize, category.PriceRanges, category.ShowOnHomePage, 
                     category.Published, true, category.DisplayOrder,
                     category.CreatedOn, category.UpdatedOn);
            }
        }

        /// <summary>
        /// Removes category picture
        /// </summary>
        /// <param name="categoryId">Category identifier</param>
        public static void RemoveCategoryPicture(int categoryId)
        {
            var category = GetCategoryById(categoryId, 0);
            if (category != null)
            {
                UpdateCategory(category.CategoryId, category.Name, category.Description,
                    category.TemplateId, category.MetaKeywords,
                    category.MetaDescription, category.MetaTitle, category.SEName,
                    category.ParentCategoryId, 0, category.PageSize, category.PriceRanges,
                    category.ShowOnHomePage, category.Published, category.Deleted, category.DisplayOrder,
                    category.CreatedOn, category.UpdatedOn);
            }
        }

        /// <summary>
        /// Gets all categories
        /// </summary>
        /// <param name="parentCategoryId">Parent category identifier</param>
        /// <returns>Category collection</returns>
        public static CategoryCollection GetAllCategories(int parentCategoryId)
        {
            bool showHidden = NopContext.Current.IsAdmin;
            return GetAllCategories(parentCategoryId, showHidden);
        }

        /// <summary>
        /// Gets all categories
        /// </summary>
        /// <param name="parentCategoryId">Parent category identifier</param>
        /// <param name="showHidden">A value indicating whether to show hidden records</param>
        /// <returns>Category collection</returns>
        public static CategoryCollection GetAllCategories(int parentCategoryId, bool showHidden)
        {
            int languageId = 0;
            if (NopContext.Current != null)
                languageId = NopContext.Current.WorkingLanguage.LanguageId;
            return GetAllCategories(parentCategoryId, showHidden, languageId);
        }

        /// <summary>
        /// Gets all categories
        /// </summary>
        /// <param name="parentCategoryId">Parent category identifier</param>
        /// <param name="showHidden">A value indicating whether to show hidden records</param>
        /// <param name="languageId">Language identifier</param>
        /// <returns>Category collection</returns>
        public static CategoryCollection GetAllCategories(int parentCategoryId,
            bool showHidden, int languageId)
        {
            string key = string.Format(CATEGORIES_ALL_KEY, showHidden, parentCategoryId, languageId);
            object obj2 = NopCache.Get(key);
            if (CategoryManager.CategoriesCacheEnabled && (obj2 != null))
            {
                return (CategoryCollection)obj2;
            }
            var dbCollection = DBProviderManager<DBCategoryProvider>.Provider.GetAllCategories(parentCategoryId, 
                showHidden, languageId);
            var categoryCollection = DBMapping(dbCollection);

            if (CategoryManager.CategoriesCacheEnabled)
            {
                NopCache.Max(key, categoryCollection);
            }
            return categoryCollection;
        }

        /// <summary>
        /// Gets all categories displayed on the home page
        /// </summary>
        /// <returns>Category collection</returns>
        public static CategoryCollection GetAllCategoriesDisplayedOnHomePage()
        {
            return GetAllCategoriesDisplayedOnHomePage(NopContext.Current.IsAdmin, NopContext.Current.WorkingLanguage.LanguageId);
        }

        /// <summary>
        /// Gets all categories displayed on the home page
        /// </summary>
        /// <param name="showHidden">A value indicating whether to show hidden records</param>
        /// <param name="languageId">Language identifier</param>
        /// <returns>Category collection</returns>
        public static CategoryCollection GetAllCategoriesDisplayedOnHomePage(bool showHidden, int languageId)
        {
            var dbCollection = DBProviderManager<DBCategoryProvider>.Provider.GetAllCategoriesDisplayedOnHomePage(showHidden, languageId);
            var categoryCollection = DBMapping(dbCollection);
            return categoryCollection;
        }

        /// <summary>
        /// Gets a category
        /// </summary>
        /// <param name="categoryId">Category identifier</param>
        /// <returns>Category</returns>
        public static Category GetCategoryById(int categoryId)
        {
            int languageId = 0;
            if (NopContext.Current != null)
                languageId = NopContext.Current.WorkingLanguage.LanguageId;
            return GetCategoryById(categoryId, languageId);
        }
        
        /// <summary>
        /// Gets a category
        /// </summary>
        /// <param name="categoryId">Category identifier</param>
        /// <param name="languageId">Language identifier</param>
        /// <returns>Category</returns>
        public static Category GetCategoryById(int categoryId, int languageId)
        {
            if (categoryId == 0)
                return null;

            string key = string.Format(CATEGORIES_BY_ID_KEY, categoryId, languageId);
            object obj2 = NopCache.Get(key);
            if (CategoryManager.CategoriesCacheEnabled && (obj2 != null))
            {
                return (Category)obj2;
            }
            var dbItem = DBProviderManager<DBCategoryProvider>.Provider.GetCategoryById(categoryId, languageId);
            var category = DBMapping(dbItem);

            if (CategoryManager.CategoriesCacheEnabled)
            {
                NopCache.Max(key, category);
            }
            return category;
        }

        /// <summary>
        /// Gets a category breadcrumb
        /// </summary>
        /// <param name="categoryId">Category identifier</param>
        /// <returns>Category</returns>
        public static CategoryCollection GetBreadCrumb(int categoryId)
        {
            var breadCrumb = new CategoryCollection();
            var category = GetCategoryById(categoryId);
            while (category != null && !category.Deleted && category.Published)
            {
                breadCrumb.Add(category);
                category = category.ParentCategory;
            }
            breadCrumb.Reverse();
            return breadCrumb;
        }

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
        public static Category InsertCategory(string name, string description,
            int templateId, string metaKeywords, string metaDescription, string metaTitle,
            string seName, int parentCategoryId, int pictureId,
            int pageSize, string priceRanges, bool showOnHomePage, bool published, bool deleted,
            int displayOrder, DateTime createdOn, DateTime updatedOn)
        {
            createdOn = DateTimeHelper.ConvertToUtcTime(createdOn);
            updatedOn = DateTimeHelper.ConvertToUtcTime(updatedOn);

            var dbItem = DBProviderManager<DBCategoryProvider>.Provider.InsertCategory(name, 
                description, templateId, metaKeywords, metaDescription, metaTitle,
                seName, parentCategoryId, pictureId, pageSize, priceRanges, showOnHomePage, published, deleted,
                displayOrder, createdOn, updatedOn);

            var category = DBMapping(dbItem);

            if (CategoryManager.CategoriesCacheEnabled || CategoryManager.MappingsCacheEnabled)
            {
                NopCache.RemoveByPattern(CATEGORIES_PATTERN_KEY);
                NopCache.RemoveByPattern(PRODUCTCATEGORIES_PATTERN_KEY);
            }

            return category;
        }

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
        /// <param name="published">A value indicating whether the entity is published</param>
        /// <param name="showOnHomePage">A value indicating whether the category will be shown on home page</param>
        /// <param name="deleted">A value indicating whether the entity has been deleted</param>
        /// <param name="displayOrder">The display order</param>
        /// <param name="createdOn">The date and time of instance creation</param>
        /// <param name="updatedOn">The date and time of instance update</param>
        /// <returns>Category</returns>
        public static Category UpdateCategory(int categoryId, string name, string description,
            int templateId, string metaKeywords, string metaDescription, string metaTitle,
            string seName, int parentCategoryId, int pictureId,
            int pageSize, string priceRanges, bool showOnHomePage, bool published, bool deleted,
            int displayOrder, DateTime createdOn, DateTime updatedOn)
        {
            createdOn = DateTimeHelper.ConvertToUtcTime(createdOn);
            updatedOn = DateTimeHelper.ConvertToUtcTime(updatedOn);

            //validate category hierarchy
            var parentCategory = GetCategoryById(parentCategoryId, 0);
            while (parentCategory != null)
            {
                if (categoryId == parentCategory.CategoryId)
                {
                    parentCategoryId = 0;
                    break;
                }
                parentCategory = GetCategoryById(parentCategory.ParentCategoryId, 0);
            }

            var dbItem = DBProviderManager<DBCategoryProvider>.Provider.UpdateCategory(categoryId,
                name, description, templateId, metaKeywords, metaDescription, metaTitle,
                seName, parentCategoryId, pictureId, pageSize, priceRanges, showOnHomePage, published, deleted,
                displayOrder, createdOn, updatedOn);

            var category = DBMapping(dbItem);
            if (CategoryManager.CategoriesCacheEnabled || CategoryManager.MappingsCacheEnabled)
            {
                NopCache.RemoveByPattern(CATEGORIES_PATTERN_KEY);
                NopCache.RemoveByPattern(PRODUCTCATEGORIES_PATTERN_KEY);
            }

            return category;
        }

        /// <summary>
        /// Gets localized category by id
        /// </summary>
        /// <param name="categoryLocalizedId">Localized category identifier</param>
        /// <returns>Category content</returns>
        public static CategoryLocalized GetCategoryLocalizedById(int categoryLocalizedId)
        {
            if (categoryLocalizedId == 0)
                return null;

            var dbItem = DBProviderManager<DBCategoryProvider>.Provider.GetCategoryLocalizedById(categoryLocalizedId);
            var item = DBMapping(dbItem);
            return item;
        }

        /// <summary>
        /// Gets localized category by category id and language id
        /// </summary>
        /// <param name="categoryId">Category identifier</param>
        /// <param name="languageId">Language identifier</param>
        /// <returns>Category content</returns>
        public static CategoryLocalized GetCategoryLocalizedByCategoryIdAndLanguageId(int categoryId, int languageId)
        {
            if (categoryId == 0 || languageId == 0)
                return null;

            var dbItem = DBProviderManager<DBCategoryProvider>.Provider.GetCategoryLocalizedByCategoryIdAndLanguageId(categoryId, languageId);
            var item = DBMapping(dbItem);
            return item;
        }

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
        public static CategoryLocalized InsertCategoryLocalized(int categoryId,
            int languageId, string name, string description,
            string metaKeywords, string metaDescription, string metaTitle,
            string seName)
        {
            var dbItem = DBProviderManager<DBCategoryProvider>.Provider.InsertCategoryLocalized(categoryId,
                languageId, name, description, metaKeywords, 
                metaDescription, metaTitle, seName);
            var item = DBMapping(dbItem);

            if (CategoryManager.CategoriesCacheEnabled)
            {
                NopCache.RemoveByPattern(CATEGORIES_PATTERN_KEY);
            }

            return item;
        }

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
        public static CategoryLocalized UpdateCategoryLocalized(int categoryLocalizedId,
            int categoryId, int languageId, string name, string description,
            string metaKeywords, string metaDescription, string metaTitle,
            string seName)
        {
            var dbItem = DBProviderManager<DBCategoryProvider>.Provider.UpdateCategoryLocalized(categoryLocalizedId,
                categoryId, languageId, name, description, metaKeywords, 
                metaDescription, metaTitle, seName);
            var item = DBMapping(dbItem);

            if (CategoryManager.CategoriesCacheEnabled)
            {
                NopCache.RemoveByPattern(CATEGORIES_PATTERN_KEY);
            }

            return item;
        }
        
        /// <summary>
        /// Deletes a product category mapping
        /// </summary>
        /// <param name="productCategoryId">Product category identifier</param>
        public static void DeleteProductCategory(int productCategoryId)
        {
            if (productCategoryId == 0)
                return;

            DBProviderManager<DBCategoryProvider>.Provider.DeleteProductCategory(productCategoryId);

            if (CategoryManager.CategoriesCacheEnabled || CategoryManager.MappingsCacheEnabled)
            {
                NopCache.RemoveByPattern(CATEGORIES_PATTERN_KEY);
                NopCache.RemoveByPattern(PRODUCTCATEGORIES_PATTERN_KEY);
            }
        }

        /// <summary>
        /// Gets product category mapping collection
        /// </summary>
        /// <param name="categoryId">Category identifier</param>
        /// <returns>Product a category mapping collection</returns>
        public static ProductCategoryCollection GetProductCategoriesByCategoryId(int categoryId)
        {
            if (categoryId == 0)
                return new ProductCategoryCollection();

            bool showHidden = NopContext.Current.IsAdmin;
            string key = string.Format(PRODUCTCATEGORIES_ALLBYCATEGORYID_KEY, showHidden, categoryId);
            object obj2 = NopCache.Get(key);
            if (CategoryManager.MappingsCacheEnabled && (obj2 != null))
            {
                return (ProductCategoryCollection)obj2;
            }

            var dbCollection = DBProviderManager<DBCategoryProvider>.Provider.GetProductCategoriesByCategoryId(categoryId, showHidden);
            var productCategoryCollection = DBMapping(dbCollection);

            if (CategoryManager.MappingsCacheEnabled)
            {
                NopCache.Max(key, productCategoryCollection);
            }
            return productCategoryCollection;
        }

        /// <summary>
        /// Gets a product category mapping collection
        /// </summary>
        /// <param name="productId">Product identifier</param>
        /// <returns>Product category mapping collection</returns>
        public static ProductCategoryCollection GetProductCategoriesByProductId(int productId)
        {
            if (productId == 0)
                return new ProductCategoryCollection();

            bool showHidden = NopContext.Current.IsAdmin;
            string key = string.Format(PRODUCTCATEGORIES_ALLBYPRODUCTID_KEY, showHidden, productId);
            object obj2 = NopCache.Get(key);
            if (CategoryManager.MappingsCacheEnabled && (obj2 != null))
            {
                return (ProductCategoryCollection)obj2;
            }

            var dbCollection = DBProviderManager<DBCategoryProvider>.Provider.GetProductCategoriesByProductId(productId, showHidden);
            var productCategoryCollection = DBMapping(dbCollection);

            if (CategoryManager.MappingsCacheEnabled)
            {
                NopCache.Max(key, productCategoryCollection);
            }
            return productCategoryCollection;
        }

        /// <summary>
        /// Gets a product category mapping 
        /// </summary>
        /// <param name="productCategoryId">Product category mapping identifier</param>
        /// <returns>Product category mapping</returns>
        public static ProductCategory GetProductCategoryById(int productCategoryId)
        {
            if (productCategoryId == 0)
                return null;

            string key = string.Format(PRODUCTCATEGORIES_BY_ID_KEY, productCategoryId);
            object obj2 = NopCache.Get(key);
            if (CategoryManager.MappingsCacheEnabled && (obj2 != null))
            {
                return (ProductCategory)obj2;
            }

            var dbItem = DBProviderManager<DBCategoryProvider>.Provider.GetProductCategoryById(productCategoryId);
            var productCategory = DBMapping(dbItem);

            if (CategoryManager.MappingsCacheEnabled)
            {
                NopCache.Max(key, productCategory);
            }
            return productCategory;
        }

        /// <summary>
        /// Inserts a product category mapping
        /// </summary>
        /// <param name="productId">Product identifier</param>
        /// <param name="categoryId">Category identifier</param>
        /// <param name="isFeaturedProduct">A value indicating whether the product is featured</param>
        /// <param name="displayOrder">The display order</param>
        /// <returns>Product category mapping </returns>
        public static ProductCategory InsertProductCategory(int productId, int categoryId,
           bool isFeaturedProduct, int displayOrder)
        {
            var dbItem = DBProviderManager<DBCategoryProvider>.Provider.InsertProductCategory(productId, 
                categoryId, isFeaturedProduct, displayOrder);

            var productCategory = DBMapping(dbItem);
            if (CategoryManager.CategoriesCacheEnabled || CategoryManager.MappingsCacheEnabled)
            {
                NopCache.RemoveByPattern(CATEGORIES_PATTERN_KEY);
                NopCache.RemoveByPattern(PRODUCTCATEGORIES_PATTERN_KEY);
            }
            return productCategory;
        }

        /// <summary>
        /// Updates the product category mapping 
        /// </summary>
        /// <param name="productCategoryId">Product category mapping  identifier</param>
        /// <param name="productId">Product identifier</param>
        /// <param name="categoryId">Category identifier</param>
        /// <param name="isFeaturedProduct">A value indicating whether the product is featured</param>
        /// <param name="displayOrder">The display order</param>
        /// <returns>Product category mapping </returns>
        public static ProductCategory UpdateProductCategory(int productCategoryId,
            int productId, int categoryId, bool isFeaturedProduct, int displayOrder)
        {
            var dbItem = DBProviderManager<DBCategoryProvider>.Provider.UpdateProductCategory(productCategoryId, 
                productId, categoryId, isFeaturedProduct, displayOrder);
            var productCategory = DBMapping(dbItem);

            if (CategoryManager.CategoriesCacheEnabled || CategoryManager.MappingsCacheEnabled)
            {
                NopCache.RemoveByPattern(CATEGORIES_PATTERN_KEY);
                NopCache.RemoveByPattern(PRODUCTCATEGORIES_PATTERN_KEY);
            }
            return productCategory;
        }
        #endregion
        
        #region Property
        /// <summary>
        /// Gets a value indicating whether categories cache is enabled
        /// </summary>
        public static bool CategoriesCacheEnabled
        {
            get
            {
                return SettingManager.GetSettingValueBoolean("Cache.CategoryManager.CategoriesCacheEnabled");
            }
        }

        /// <summary>
        /// Gets a value indicating whether mappings cache is enabled
        /// </summary>
        public static bool MappingsCacheEnabled
        {
            get
            {
                return SettingManager.GetSettingValueBoolean("Cache.CategoryManager.MappingsCacheEnabled");
            }
        }
        #endregion
    }
}
