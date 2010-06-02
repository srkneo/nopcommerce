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
using System.Configuration.Provider;
using System.Data;
using System.Data.Common;
using System.Globalization;
using System.Text;
using Microsoft.Practices.EnterpriseLibrary.Data;

namespace NopSolutions.NopCommerce.DataAccess.Categories
{
    /// <summary>
    /// Category provider for SQL Server
    /// </summary>
    public partial class SqlCategoryProvider : DBCategoryProvider
    {
        #region Fields
        private string _sqlConnectionString;
        #endregion

        #region Utilities
        private DBCategory GetCategoryFromReader(IDataReader dataReader)
        {
            var item = new DBCategory();
            item.CategoryId = NopSqlDataHelper.GetInt(dataReader, "CategoryID");
            item.Name = NopSqlDataHelper.GetString(dataReader, "Name");
            item.Description = NopSqlDataHelper.GetString(dataReader, "Description");
            item.TemplateId = NopSqlDataHelper.GetInt(dataReader, "TemplateID");
            item.MetaKeywords = NopSqlDataHelper.GetString(dataReader, "MetaKeywords");
            item.MetaDescription = NopSqlDataHelper.GetString(dataReader, "MetaDescription");
            item.MetaTitle = NopSqlDataHelper.GetString(dataReader, "MetaTitle");
            item.SEName = NopSqlDataHelper.GetString(dataReader, "SEName");
            item.ParentCategoryId = NopSqlDataHelper.GetInt(dataReader, "ParentCategoryID");
            item.PictureId = NopSqlDataHelper.GetInt(dataReader, "PictureID");
            item.PageSize = NopSqlDataHelper.GetInt(dataReader, "PageSize");
            item.PriceRanges = NopSqlDataHelper.GetString(dataReader, "PriceRanges");
            item.ShowOnHomePage = NopSqlDataHelper.GetBoolean(dataReader, "ShowOnHomePage");
            item.Published = NopSqlDataHelper.GetBoolean(dataReader, "Published");
            item.Deleted = NopSqlDataHelper.GetBoolean(dataReader, "Deleted");
            item.DisplayOrder = NopSqlDataHelper.GetInt(dataReader, "DisplayOrder");
            item.CreatedOn = NopSqlDataHelper.GetUtcDateTime(dataReader, "CreatedOn");
            item.UpdatedOn = NopSqlDataHelper.GetUtcDateTime(dataReader, "UpdatedOn");
            return item;
        }

        private DBCategoryLocalized GetCategoryLocalizedFromReader(IDataReader dataReader)
        {
            var item = new DBCategoryLocalized();
            item.CategoryLocalizedId = NopSqlDataHelper.GetInt(dataReader, "CategoryLocalizedID");
            item.CategoryId = NopSqlDataHelper.GetInt(dataReader, "CategoryID");
            item.LanguageId = NopSqlDataHelper.GetInt(dataReader, "LanguageID");
            item.Name = NopSqlDataHelper.GetString(dataReader, "Name");
            item.Description = NopSqlDataHelper.GetString(dataReader, "Description");
            item.MetaKeywords = NopSqlDataHelper.GetString(dataReader, "MetaKeywords");
            item.MetaDescription = NopSqlDataHelper.GetString(dataReader, "MetaDescription");
            item.MetaTitle = NopSqlDataHelper.GetString(dataReader, "MetaTitle");
            item.SEName = NopSqlDataHelper.GetString(dataReader, "SEName");
            return item;
        }

        private DBProductCategory GetProductCategoryFromReader(IDataReader dataReader)
        {
            var item = new DBProductCategory();
            item.ProductCategoryId = NopSqlDataHelper.GetInt(dataReader, "ProductCategoryID");
            item.ProductId = NopSqlDataHelper.GetInt(dataReader, "ProductID");
            item.CategoryId = NopSqlDataHelper.GetInt(dataReader, "CategoryID");
            item.IsFeaturedProduct = NopSqlDataHelper.GetBoolean(dataReader, "IsFeaturedProduct");
            item.DisplayOrder = NopSqlDataHelper.GetInt(dataReader, "DisplayOrder");
            return item;
        }
        #endregion

        #region Methods

        /// <summary>
        /// Initializes the provider with the property values specified in the application's configuration file. This method is not intended to be used directly from your code
        /// </summary>
        /// <param name="name">The name of the provider instance to initialize</param>
        /// <param name="config">A NameValueCollection that contains the names and values of configuration options for the provider.</param>
        public override void Initialize(string name, NameValueCollection config)
        {
            if (config == null)
            {
                throw new ArgumentNullException("config");
            }

            base.Initialize(name, config);

            string connectionStringName = config["connectionStringName"];
            if (String.IsNullOrEmpty(connectionStringName))
                throw new ProviderException("Connection name not specified");
            this._sqlConnectionString = NopSqlDataHelper.GetConnectionString(connectionStringName);
            if ((this._sqlConnectionString == null) || (this._sqlConnectionString.Length < 1))
            {
                throw new ProviderException(string.Format("Connection string not found. {0}", connectionStringName));
            }
            config.Remove("connectionStringName");

            if (config.Count > 0)
            {
                string key = config.GetKey(0);
                if (!string.IsNullOrEmpty(key))
                {
                    throw new ProviderException(string.Format("Provider unrecognized attribute. {0}", new object[] { key }));
                }
            }
        }

        /// <summary>
        /// Gets all categories
        /// </summary>
        /// <param name="parentCategoryId">Parent category identifier</param>
        /// <param name="showHidden">A value indicating whether to show hidden records</param>
        /// <param name="languageId">Language identifier</param>
        /// <returns>Category collection</returns>
        public override DBCategoryCollection GetAllCategories(int parentCategoryId,
            bool showHidden, int languageId)
        {
            var result = new DBCategoryCollection();
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_CategoryLoadAll");
            db.AddInParameter(dbCommand, "ShowHidden", DbType.Boolean, showHidden);
            db.AddInParameter(dbCommand, "ParentCategoryID", DbType.Int32, parentCategoryId);
            db.AddInParameter(dbCommand, "LanguageID", DbType.Int32, languageId);
            using (IDataReader dataReader = db.ExecuteReader(dbCommand))
            {
                while (dataReader.Read())
                {
                    var item = GetCategoryFromReader(dataReader);
                    result.Add(item);
                }
            }

            return result;
        }

        /// <summary>
        /// Gets all categories displayed on the home page
        /// </summary>
        /// <param name="showHidden">A value indicating whether to show hidden records</param>
        /// <param name="languageId">Language identifier</param>
        /// <returns>Category collection</returns>
        public override DBCategoryCollection GetAllCategoriesDisplayedOnHomePage(bool showHidden, int languageId)
        {
            var result = new DBCategoryCollection();
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_CategoryLoadDisplayedOnHomePage");
            db.AddInParameter(dbCommand, "ShowHidden", DbType.Boolean, showHidden);
            db.AddInParameter(dbCommand, "LanguageID", DbType.Int32, languageId);
            using(IDataReader dataReader = db.ExecuteReader(dbCommand))
            {
                while(dataReader.Read())
                {
                    var item = GetCategoryFromReader(dataReader);
                    result.Add(item);
                }
            }
            return result;
        }

        /// <summary>
        /// Gets a category
        /// </summary>
        /// <param name="categoryId">Category identifier</param>
        /// <param name="languageId">Language identifier</param>
        /// <returns>Category</returns>
        public override DBCategory GetCategoryById(int categoryId, int languageId)
        {
            DBCategory item = null;
            if (categoryId == 0)
                return item;
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_CategoryLoadByPrimaryKey");
            db.AddInParameter(dbCommand, "CategoryID", DbType.Int32, categoryId);
            db.AddInParameter(dbCommand, "LanguageID", DbType.Int32, languageId);
            using (IDataReader dataReader = db.ExecuteReader(dbCommand))
            {
                if (dataReader.Read())
                {
                    item = GetCategoryFromReader(dataReader);
                }
            }
            return item;
        }

        /// <summary>
        /// Inserts category identifier
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
        public override DBCategory InsertCategory(string name, string description,
            int templateId, string metaKeywords, string metaDescription, string metaTitle,
            string seName, int parentCategoryId, int pictureId,
            int pageSize, string priceRanges, bool showOnHomePage, bool published, bool deleted,
            int displayOrder, DateTime createdOn, DateTime updatedOn)
        {
            DBCategory item = null;
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_CategoryInsert");
            db.AddOutParameter(dbCommand, "CategoryID", DbType.Int32, 0);
            db.AddInParameter(dbCommand, "Name", DbType.String, name);
            db.AddInParameter(dbCommand, "Description", DbType.String, description);
            db.AddInParameter(dbCommand, "TemplateID", DbType.Int32, templateId);
            db.AddInParameter(dbCommand, "MetaKeywords", DbType.String, metaKeywords);
            db.AddInParameter(dbCommand, "MetaDescription", DbType.String, metaDescription);
            db.AddInParameter(dbCommand, "MetaTitle", DbType.String, metaTitle);
            db.AddInParameter(dbCommand, "SEName", DbType.String, seName);
            db.AddInParameter(dbCommand, "ParentCategoryID", DbType.Int32, parentCategoryId);
            db.AddInParameter(dbCommand, "PictureID", DbType.Int32, pictureId);
            db.AddInParameter(dbCommand, "PageSize", DbType.Int32, pageSize);
            db.AddInParameter(dbCommand, "PriceRanges", DbType.String, priceRanges);
            db.AddInParameter(dbCommand, "Published", DbType.Boolean, published);
            db.AddInParameter(dbCommand, "Deleted", DbType.Boolean, deleted);
            db.AddInParameter(dbCommand, "DisplayOrder", DbType.Int32, displayOrder);
            db.AddInParameter(dbCommand, "CreatedOn", DbType.DateTime, createdOn);
            db.AddInParameter(dbCommand, "UpdatedOn", DbType.DateTime, updatedOn);
            db.AddInParameter(dbCommand, "ShowOnHomePage", DbType.Boolean, showOnHomePage);
            if (db.ExecuteNonQuery(dbCommand) > 0)
            {
                int categoryId = Convert.ToInt32(db.GetParameterValue(dbCommand, "@CategoryID"));
                item = GetCategoryById(categoryId, 0);
            }
            return item;
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
        /// <param name="showOnHomePage">A value indicating whether the category will be shown on home page</param>
        /// <param name="published">A value indicating whether the entity is published</param>
        /// <param name="deleted">A value indicating whether the entity has been deleted</param>
        /// <param name="displayOrder">The display order</param>
        /// <param name="createdOn">The date and time of instance creation</param>
        /// <param name="updatedOn">The date and time of instance update</param>
        /// <returns>Category</returns>
        public override DBCategory UpdateCategory(int categoryId, string name, string description,
            int templateId, string metaKeywords, string metaDescription, string metaTitle,
            string seName, int parentCategoryId, int pictureId,
            int pageSize, string priceRanges, bool showOnHomePage, bool published, bool deleted,
            int displayOrder, DateTime createdOn, DateTime updatedOn)
        {
            DBCategory item = null;
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_CategoryUpdate");
            db.AddInParameter(dbCommand, "CategoryID", DbType.Int32, categoryId);
            db.AddInParameter(dbCommand, "Name", DbType.String, name);
            db.AddInParameter(dbCommand, "Description", DbType.String, description);
            db.AddInParameter(dbCommand, "TemplateID", DbType.Int32, templateId);
            db.AddInParameter(dbCommand, "MetaKeywords", DbType.String, metaKeywords);
            db.AddInParameter(dbCommand, "MetaDescription", DbType.String, metaDescription);
            db.AddInParameter(dbCommand, "MetaTitle", DbType.String, metaTitle);
            db.AddInParameter(dbCommand, "SEName", DbType.String, seName);
            db.AddInParameter(dbCommand, "ParentCategoryID", DbType.Int32, parentCategoryId);
            db.AddInParameter(dbCommand, "PictureID", DbType.Int32, pictureId);
            db.AddInParameter(dbCommand, "PageSize", DbType.Int32, pageSize);
            db.AddInParameter(dbCommand, "PriceRanges", DbType.String, priceRanges);
            db.AddInParameter(dbCommand, "Published", DbType.Boolean, published);
            db.AddInParameter(dbCommand, "Deleted", DbType.Boolean, deleted);
            db.AddInParameter(dbCommand, "DisplayOrder", DbType.Int32, displayOrder);
            db.AddInParameter(dbCommand, "CreatedOn", DbType.DateTime, createdOn);
            db.AddInParameter(dbCommand, "UpdatedOn", DbType.DateTime, updatedOn);
            db.AddInParameter(dbCommand, "ShowOnHomePage", DbType.Boolean, showOnHomePage);
            if (db.ExecuteNonQuery(dbCommand) > 0)
                item = GetCategoryById(categoryId, 0);

            return item;
        }

        /// <summary>
        /// Gets localized category by id
        /// </summary>
        /// <param name="categoryLocalizedId">Localized category identifier</param>
        /// <returns>Category content</returns>
        public override DBCategoryLocalized GetCategoryLocalizedById(int categoryLocalizedId)
        {
            DBCategoryLocalized item = null;
            if (categoryLocalizedId == 0)
                return item;
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_CategoryLocalizedLoadByPrimaryKey");
            db.AddInParameter(dbCommand, "CategoryLocalizedID", DbType.Int32, categoryLocalizedId);
            using (IDataReader dataReader = db.ExecuteReader(dbCommand))
            {
                if (dataReader.Read())
                {
                    item = GetCategoryLocalizedFromReader(dataReader);
                }
            }
            return item;
        }

        /// <summary>
        /// Gets localized category by category id and language id
        /// </summary>
        /// <param name="categoryId">Category identifier</param>
        /// <param name="languageId">Language identifier</param>
        /// <returns>Category content</returns>
        public override DBCategoryLocalized GetCategoryLocalizedByCategoryIdAndLanguageId(int categoryId, int languageId)
        {
            DBCategoryLocalized item = null;
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_CategoryLocalizedLoadByCategoryIDAndLanguageID");
            db.AddInParameter(dbCommand, "CategoryID", DbType.Int32, categoryId);
            db.AddInParameter(dbCommand, "LanguageID", DbType.Int32, languageId);
            using (IDataReader dataReader = db.ExecuteReader(dbCommand))
            {
                if (dataReader.Read())
                {
                    item = GetCategoryLocalizedFromReader(dataReader);
                }
            }
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
        public override DBCategoryLocalized InsertCategoryLocalized(int categoryId,
            int languageId, string name, string description,
            string metaKeywords, string metaDescription, string metaTitle,
            string seName)
        {
            DBCategoryLocalized item = null;
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_CategoryLocalizedInsert");
            db.AddOutParameter(dbCommand, "CategoryLocalizedID", DbType.Int32, 0);
            db.AddInParameter(dbCommand, "CategoryID", DbType.Int32, categoryId);
            db.AddInParameter(dbCommand, "LanguageID", DbType.Int32, languageId);
            db.AddInParameter(dbCommand, "Name", DbType.String, name);
            db.AddInParameter(dbCommand, "Description", DbType.String, description);
            db.AddInParameter(dbCommand, "MetaKeywords", DbType.String, metaKeywords);
            db.AddInParameter(dbCommand, "MetaDescription", DbType.String, metaDescription);
            db.AddInParameter(dbCommand, "MetaTitle", DbType.String, metaTitle);
            db.AddInParameter(dbCommand, "SEName", DbType.String, seName);
            if (db.ExecuteNonQuery(dbCommand) > 0)
            {
                int categoryLocalizedId = Convert.ToInt32(db.GetParameterValue(dbCommand, "@CategoryLocalizedID"));
                item = GetCategoryLocalizedById(categoryLocalizedId);
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
        public override DBCategoryLocalized UpdateCategoryLocalized(int categoryLocalizedId,
            int categoryId, int languageId, string name, string description,
            string metaKeywords, string metaDescription, string metaTitle,
            string seName)
        {
            DBCategoryLocalized item = null;
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_CategoryLocalizedUpdate");
            db.AddInParameter(dbCommand, "CategoryLocalizedID", DbType.Int32, categoryLocalizedId);
            db.AddInParameter(dbCommand, "CategoryID", DbType.Int32, categoryId);
            db.AddInParameter(dbCommand, "LanguageID", DbType.Int32, languageId);
            db.AddInParameter(dbCommand, "Name", DbType.String, name);
            db.AddInParameter(dbCommand, "Description", DbType.String, description);
            db.AddInParameter(dbCommand, "MetaKeywords", DbType.String, metaKeywords);
            db.AddInParameter(dbCommand, "MetaDescription", DbType.String, metaDescription);
            db.AddInParameter(dbCommand, "MetaTitle", DbType.String, metaTitle);
            db.AddInParameter(dbCommand, "SEName", DbType.String, seName);
            if (db.ExecuteNonQuery(dbCommand) > 0)
                item = GetCategoryLocalizedById(categoryLocalizedId);

            return item;
        }

        /// <summary>
        /// Deletes a product category mapping
        /// </summary>
        /// <param name="productCategoryId">Product category identifier</param>
        public override void DeleteProductCategory(int productCategoryId)
        {
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_Product_Category_MappingDelete");
            db.AddInParameter(dbCommand, "ProductCategoryID", DbType.Int32, productCategoryId);
            db.ExecuteNonQuery(dbCommand);
        }

        /// <summary>
        /// Gets product category mapping collection
        /// </summary>
        /// <param name="categoryId">Category identifier</param>
        /// <param name="showHidden">A value indicating whether to show hidden records</param>
        /// <returns>Product a category mapping collection</returns>
        public override DBProductCategoryCollection GetProductCategoriesByCategoryId(int categoryId, bool showHidden)
        {
            var result = new DBProductCategoryCollection();
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_Product_Category_MappingLoadByCategoryID");
            db.AddInParameter(dbCommand, "CategoryID", DbType.Int32, categoryId);
            db.AddInParameter(dbCommand, "ShowHidden", DbType.Boolean, showHidden);
            using (IDataReader dataReader = db.ExecuteReader(dbCommand))
            {
                while (dataReader.Read())
                {
                    var item = GetProductCategoryFromReader(dataReader);
                    result.Add(item);
                }
            }

            return result;
        }

        /// <summary>
        /// Gets a product category mapping collection
        /// </summary>
        /// <param name="productId">Product identifier</param>
        /// <param name="showHidden">A value indicating whether to show hidden records</param>
        /// <returns>Product category mapping collection</returns>
        public override DBProductCategoryCollection GetProductCategoriesByProductId(int productId, bool showHidden)
        {
            var result = new DBProductCategoryCollection();
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_Product_Category_MappingLoadByProductID");
            db.AddInParameter(dbCommand, "ProductID", DbType.Int32, productId);
            db.AddInParameter(dbCommand, "ShowHidden", DbType.Boolean, showHidden);
            using (IDataReader dataReader = db.ExecuteReader(dbCommand))
            {
                while (dataReader.Read())
                {
                    var item = GetProductCategoryFromReader(dataReader);
                    result.Add(item);
                }
            }

            return result;
        }

        /// <summary>
        /// Gets a product category mapping 
        /// </summary>
        /// <param name="productCategoryId">Product category mapping identifier</param>
        /// <returns>Product category mapping</returns>
        public override DBProductCategory GetProductCategoryById(int productCategoryId)
        {
            DBProductCategory item = null;
            if (productCategoryId == 0)
                return item;
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_Product_Category_MappingLoadByPrimaryKey");
            db.AddInParameter(dbCommand, "ProductCategoryID", DbType.Int32, productCategoryId);
            using (IDataReader dataReader = db.ExecuteReader(dbCommand))
            {
                if (dataReader.Read())
                {
                    item = GetProductCategoryFromReader(dataReader);
                }
            }
            return item;
        }

        /// <summary>
        /// Inserts a product category mapping
        /// </summary>
        /// <param name="productId">Product identifier</param>
        /// <param name="categoryId">Category identifier</param>
        /// <param name="isFeaturedProduct">A value indicating whether the product is featured</param>
        /// <param name="displayOrder">The display order</param>
        /// <returns>Product category mapping </returns>
        public override DBProductCategory InsertProductCategory(int productId, int categoryId,
            bool isFeaturedProduct, int displayOrder)
        {
            DBProductCategory item = null;
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_Product_Category_MappingInsert");
            db.AddOutParameter(dbCommand, "ProductCategoryID", DbType.Int32, 0);
            db.AddInParameter(dbCommand, "ProductID", DbType.Int32, productId);
            db.AddInParameter(dbCommand, "CategoryID", DbType.Int32, categoryId);
            db.AddInParameter(dbCommand, "IsFeaturedProduct", DbType.Boolean, isFeaturedProduct);
            db.AddInParameter(dbCommand, "DisplayOrder", DbType.Int32, displayOrder);
            if (db.ExecuteNonQuery(dbCommand) > 0)
            {
                int productCategoryId = Convert.ToInt32(db.GetParameterValue(dbCommand, "@ProductCategoryID"));
                item = GetProductCategoryById(productCategoryId);
            }
            return item;
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
        public override DBProductCategory UpdateProductCategory(int productCategoryId,
            int productId, int categoryId, bool isFeaturedProduct, int displayOrder)
        {
            DBProductCategory item = null;
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_Product_Category_MappingUpdate");
            db.AddInParameter(dbCommand, "ProductCategoryID", DbType.Int32, productCategoryId);
            db.AddInParameter(dbCommand, "ProductID", DbType.Int32, productId);
            db.AddInParameter(dbCommand, "CategoryID", DbType.Int32, categoryId);
            db.AddInParameter(dbCommand, "IsFeaturedProduct", DbType.Boolean, isFeaturedProduct);
            db.AddInParameter(dbCommand, "DisplayOrder", DbType.Int32, displayOrder);
            if (db.ExecuteNonQuery(dbCommand) > 0)
                item = GetProductCategoryById(productCategoryId);

            return item;
        }
        #endregion
    }
}
