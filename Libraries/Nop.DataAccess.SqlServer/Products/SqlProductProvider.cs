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
using System.Xml;

namespace NopSolutions.NopCommerce.DataAccess.Products
{
    /// <summary>
    /// Product provider for SQL Server
    /// </summary>
    public partial class SqlProductProvider : DBProductProvider
    {
        #region Fields
        private string _sqlConnectionString;
        #endregion

        #region Utilities
        private DBProduct GetProductFromReader(IDataReader dataReader)
        {
            var item = new DBProduct();
            item.ProductId = NopSqlDataHelper.GetInt(dataReader, "ProductID");
            item.Name = NopSqlDataHelper.GetString(dataReader, "Name");
            item.ShortDescription = NopSqlDataHelper.GetString(dataReader, "ShortDescription");
            item.FullDescription = NopSqlDataHelper.GetString(dataReader, "FullDescription");
            item.AdminComment = NopSqlDataHelper.GetString(dataReader, "AdminComment");
            item.ProductTypeId = NopSqlDataHelper.GetInt(dataReader, "ProductTypeID");
            item.TemplateId = NopSqlDataHelper.GetInt(dataReader, "TemplateID");
            item.ShowOnHomePage = NopSqlDataHelper.GetBoolean(dataReader, "ShowOnHomePage");
            item.MetaKeywords = NopSqlDataHelper.GetString(dataReader, "MetaKeywords");
            item.MetaDescription = NopSqlDataHelper.GetString(dataReader, "MetaDescription");
            item.MetaTitle = NopSqlDataHelper.GetString(dataReader, "MetaTitle");
            item.SEName = NopSqlDataHelper.GetString(dataReader, "SEName");
            item.AllowCustomerReviews = NopSqlDataHelper.GetBoolean(dataReader, "AllowCustomerReviews");
            item.AllowCustomerRatings = NopSqlDataHelper.GetBoolean(dataReader, "AllowCustomerRatings");
            item.RatingSum = NopSqlDataHelper.GetInt(dataReader, "RatingSum");
            item.TotalRatingVotes = NopSqlDataHelper.GetInt(dataReader, "TotalRatingVotes");
            item.Published = NopSqlDataHelper.GetBoolean(dataReader, "Published");
            item.Deleted = NopSqlDataHelper.GetBoolean(dataReader, "Deleted");
            item.CreatedOn = NopSqlDataHelper.GetUtcDateTime(dataReader, "CreatedOn");
            item.UpdatedOn = NopSqlDataHelper.GetUtcDateTime(dataReader, "UpdatedOn");
            return item;
        }

        private DBProductVariant GetProductVariantFromReader(IDataReader dataReader)
        {
            var item = new DBProductVariant();
            item.ProductVariantId = NopSqlDataHelper.GetInt(dataReader, "ProductVariantID");
            item.ProductId = NopSqlDataHelper.GetInt(dataReader, "ProductID");
            item.Name = NopSqlDataHelper.GetString(dataReader, "Name");
            item.SKU = NopSqlDataHelper.GetString(dataReader, "SKU");
            item.Description = NopSqlDataHelper.GetString(dataReader, "Description");
            item.AdminComment = NopSqlDataHelper.GetString(dataReader, "AdminComment");
            item.ManufacturerPartNumber = NopSqlDataHelper.GetString(dataReader, "ManufacturerPartNumber");
            item.IsGiftCard = NopSqlDataHelper.GetBoolean(dataReader, "IsGiftCard");
            item.IsDownload = NopSqlDataHelper.GetBoolean(dataReader, "IsDownload");
            item.DownloadId = NopSqlDataHelper.GetInt(dataReader, "DownloadID");
            item.UnlimitedDownloads = NopSqlDataHelper.GetBoolean(dataReader, "UnlimitedDownloads");
            item.MaxNumberOfDownloads = NopSqlDataHelper.GetInt(dataReader, "MaxNumberOfDownloads");
            item.DownloadExpirationDays = NopSqlDataHelper.GetNullableInt(dataReader, "DownloadExpirationDays");
            item.DownloadActivationType = NopSqlDataHelper.GetInt(dataReader, "DownloadActivationType");
            item.HasSampleDownload = NopSqlDataHelper.GetBoolean(dataReader, "HasSampleDownload");
            item.SampleDownloadId = NopSqlDataHelper.GetInt(dataReader, "SampleDownloadID");
            item.HasUserAgreement = NopSqlDataHelper.GetBoolean(dataReader, "HasUserAgreement");
            item.UserAgreementText = NopSqlDataHelper.GetString(dataReader, "UserAgreementText");
            item.IsRecurring = NopSqlDataHelper.GetBoolean(dataReader, "IsRecurring");
            item.CycleLength = NopSqlDataHelper.GetInt(dataReader, "CycleLength");
            item.CyclePeriod = NopSqlDataHelper.GetInt(dataReader, "CyclePeriod");
            item.TotalCycles = NopSqlDataHelper.GetInt(dataReader, "TotalCycles");
            item.IsShipEnabled = NopSqlDataHelper.GetBoolean(dataReader, "IsShipEnabled");
            item.IsFreeShipping = NopSqlDataHelper.GetBoolean(dataReader, "IsFreeShipping");
            item.AdditionalShippingCharge = NopSqlDataHelper.GetDecimal(dataReader, "AdditionalShippingCharge");
            item.IsTaxExempt = NopSqlDataHelper.GetBoolean(dataReader, "IsTaxExempt");
            item.TaxCategoryId = NopSqlDataHelper.GetInt(dataReader, "TaxCategoryID");
            item.ManageInventory = NopSqlDataHelper.GetInt(dataReader, "ManageInventory");
            item.StockQuantity = NopSqlDataHelper.GetInt(dataReader, "StockQuantity");
            item.DisplayStockAvailability = NopSqlDataHelper.GetBoolean(dataReader, "DisplayStockAvailability");
            item.MinStockQuantity = NopSqlDataHelper.GetInt(dataReader, "MinStockQuantity");
            item.LowStockActivityId = NopSqlDataHelper.GetInt(dataReader, "LowStockActivityID");
            item.NotifyAdminForQuantityBelow = NopSqlDataHelper.GetInt(dataReader, "NotifyAdminForQuantityBelow");
            item.AllowOutOfStockOrders = NopSqlDataHelper.GetBoolean(dataReader, "AllowOutOfStockOrders");
            item.OrderMinimumQuantity = NopSqlDataHelper.GetInt(dataReader, "OrderMinimumQuantity");
            item.OrderMaximumQuantity = NopSqlDataHelper.GetInt(dataReader, "OrderMaximumQuantity");
            item.WarehouseId = NopSqlDataHelper.GetInt(dataReader, "WarehouseId");
            item.DisableBuyButton = NopSqlDataHelper.GetBoolean(dataReader, "DisableBuyButton");
            item.Price = NopSqlDataHelper.GetDecimal(dataReader, "Price");
            item.OldPrice = NopSqlDataHelper.GetDecimal(dataReader, "OldPrice");
            item.ProductCost = NopSqlDataHelper.GetDecimal(dataReader, "ProductCost");
            item.CustomerEntersPrice = NopSqlDataHelper.GetBoolean(dataReader, "CustomerEntersPrice");
            item.MinimumCustomerEnteredPrice = NopSqlDataHelper.GetDecimal(dataReader, "MinimumCustomerEnteredPrice");
            item.MaximumCustomerEnteredPrice = NopSqlDataHelper.GetDecimal(dataReader, "MaximumCustomerEnteredPrice");
            item.Weight = NopSqlDataHelper.GetDecimal(dataReader, "Weight");
            item.Length = NopSqlDataHelper.GetDecimal(dataReader, "Length");
            item.Width = NopSqlDataHelper.GetDecimal(dataReader, "Width");
            item.Height = NopSqlDataHelper.GetDecimal(dataReader, "Height");
            item.PictureId = NopSqlDataHelper.GetInt(dataReader, "PictureID");
            item.AvailableStartDateTime = NopSqlDataHelper.GetNullableUtcDateTime(dataReader, "AvailableStartDateTime");
            item.AvailableEndDateTime = NopSqlDataHelper.GetNullableUtcDateTime(dataReader, "AvailableEndDateTime");
            item.Published = NopSqlDataHelper.GetBoolean(dataReader, "Published");
            item.Deleted = NopSqlDataHelper.GetBoolean(dataReader, "Deleted");
            item.DisplayOrder = NopSqlDataHelper.GetInt(dataReader, "DisplayOrder");
            item.CreatedOn = NopSqlDataHelper.GetUtcDateTime(dataReader, "CreatedOn");
            item.UpdatedOn = NopSqlDataHelper.GetUtcDateTime(dataReader, "UpdatedOn");
            return item;
        }

        private DBRelatedProduct GetRelatedProductFromReader(IDataReader dataReader)
        {
            var item = new DBRelatedProduct();
            item.RelatedProductId = NopSqlDataHelper.GetInt(dataReader, "RelatedProductID");
            item.ProductId1 = NopSqlDataHelper.GetInt(dataReader, "ProductID1");
            item.ProductId2 = NopSqlDataHelper.GetInt(dataReader, "ProductID2");
            item.DisplayOrder = NopSqlDataHelper.GetInt(dataReader, "DisplayOrder");
            return item;
        }

        private DBProductVariantPricelist GetProductVariantPricelistFromReader(IDataReader dataReader)
        {
            var item = new DBProductVariantPricelist();
            item.ProductVariantPricelistId = NopSqlDataHelper.GetInt(dataReader, "ProductVariantPricelistID");
            item.ProductVariantId = NopSqlDataHelper.GetInt(dataReader, "ProductVariantID");
            item.PricelistId = NopSqlDataHelper.GetInt(dataReader, "PricelistID");
            item.PriceAdjustmentTypeId = NopSqlDataHelper.GetInt(dataReader, "PriceAdjustmentTypeID");
            item.PriceAdjustment = NopSqlDataHelper.GetDecimal(dataReader, "PriceAdjustment");
            item.UpdatedOn = NopSqlDataHelper.GetUtcDateTime(dataReader, "UpdatedOn");
            return item;
        }

        private DBProductLocalized GetProductLocalizedFromReader(IDataReader dataReader)
        {
            var item = new DBProductLocalized();
            item.ProductLocalizedId = NopSqlDataHelper.GetInt(dataReader, "ProductLocalizedID");
            item.ProductId = NopSqlDataHelper.GetInt(dataReader, "ProductID");
            item.LanguageId = NopSqlDataHelper.GetInt(dataReader, "LanguageID");
            item.Name = NopSqlDataHelper.GetString(dataReader, "Name");
            item.ShortDescription = NopSqlDataHelper.GetString(dataReader, "ShortDescription");
            item.FullDescription = NopSqlDataHelper.GetString(dataReader, "FullDescription");
            item.MetaKeywords = NopSqlDataHelper.GetString(dataReader, "MetaKeywords");
            item.MetaDescription = NopSqlDataHelper.GetString(dataReader, "MetaDescription");
            item.MetaTitle = NopSqlDataHelper.GetString(dataReader, "MetaTitle");
            item.SEName = NopSqlDataHelper.GetString(dataReader, "SEName");
            return item;
        }

        private DBProductVariantLocalized GetProductVariantLocalizedFromReader(IDataReader dataReader)
        {
            var item = new DBProductVariantLocalized();
            item.ProductVariantLocalizedId = NopSqlDataHelper.GetInt(dataReader, "ProductVariantLocalizedID");
            item.ProductVariantId = NopSqlDataHelper.GetInt(dataReader, "ProductVariantID");
            item.LanguageId = NopSqlDataHelper.GetInt(dataReader, "LanguageID");
            item.Name = NopSqlDataHelper.GetString(dataReader, "Name");
            item.Description = NopSqlDataHelper.GetString(dataReader, "Description");
            return item;
        }

        private DBProductTag GetProductTagFromReader(IDataReader dataReader)
        {
            var item = new DBProductTag();
            item.ProductTagId = NopSqlDataHelper.GetInt(dataReader, "ProductTagID");
            item.Name = NopSqlDataHelper.GetString(dataReader, "Name");
            item.ProductCount = NopSqlDataHelper.GetInt(dataReader, "ProductCount");
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
        /// Gets all products
        /// </summary>
        /// <param name="showHidden">A value indicating whether to show hidden records</param>
        /// <param name="languageId">Language identifier</param>
        /// <returns>Product collection</returns>
        public override DBProductCollection GetAllProducts(bool showHidden, int languageId)
        {
            var result = new DBProductCollection();
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_ProductLoadAll");
            db.AddInParameter(dbCommand, "ShowHidden", DbType.Boolean, showHidden);
            db.AddInParameter(dbCommand, "LanguageID", DbType.Int32, languageId);
            using (IDataReader dataReader = db.ExecuteReader(dbCommand))
            {
                while (dataReader.Read())
                {
                    var item = GetProductFromReader(dataReader);
                    result.Add(item);
                }
            }

            return result;
        }

        /// <summary>
        /// Gets all products
        /// </summary>
        /// <param name="categoryId">Category identifier</param>
        /// <param name="manufacturerId">Manufacturer identifier</param>
        /// <param name="productTagId">Product tag identifier</param>
        /// <param name="featuredProducts">A value indicating whether loaded products are marked as featured (relates only to categories and manufacturers). 0 to load featured products only, 1 to load not featured products only, null to load all products</param>
        /// <param name="priceMin">Minimum price</param>
        /// <param name="priceMax">Maximum price</param>
        /// <param name="keywords">Keywords</param>
        /// <param name="searchDescriptions">A value indicating whether to search in descriptions</param>
        /// <param name="pageSize">Page size</param>
        /// <param name="pageIndex">Page index</param>
        /// <param name="filteredSpecs">Filtered product specification identifiers</param>
        /// <param name="languageId">Language identifier</param>
        /// <param name="showHidden">A value indicating whether to show hidden records</param>
        /// <param name="orderBy">Order by</param>
        /// <param name="totalRecords">Total records</param>
        /// <returns>Product collection</returns>
        public override DBProductCollection GetAllProducts(int categoryId, 
            int manufacturerId, int productTagId,
            bool? featuredProducts, decimal? priceMin, decimal? priceMax,
            string keywords, bool searchDescriptions,
            int pageSize, int pageIndex, List<int> filteredSpecs,
            int languageId, int orderBy, bool showHidden, out int totalRecords)
        {
            totalRecords = 0;
            var result = new DBProductCollection();
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_ProductLoadAllPaged");
            db.AddInParameter(dbCommand, "CategoryID", DbType.Int32, categoryId);
            db.AddInParameter(dbCommand, "ManufacturerID", DbType.Int32, manufacturerId);
            db.AddInParameter(dbCommand, "ProductTagID", DbType.Int32, productTagId);
            if (featuredProducts.HasValue)
                db.AddInParameter(dbCommand, "FeaturedProducts", DbType.Boolean, featuredProducts.Value);
            else
                db.AddInParameter(dbCommand, "FeaturedProducts", DbType.Boolean, null);
            if (priceMin.HasValue)
                db.AddInParameter(dbCommand, "PriceMin", DbType.Decimal, priceMin.Value);
            else
                db.AddInParameter(dbCommand, "PriceMin", DbType.Decimal, null);
            if (priceMax.HasValue)
                db.AddInParameter(dbCommand, "PriceMax", DbType.Decimal, priceMax.Value);
            else
                db.AddInParameter(dbCommand, "PriceMax", DbType.Decimal, null);
            db.AddInParameter(dbCommand, "Keywords", DbType.String, keywords);
            db.AddInParameter(dbCommand, "SearchDescriptions", DbType.Boolean, searchDescriptions);
            db.AddInParameter(dbCommand, "ShowHidden", DbType.Boolean, showHidden);
            db.AddInParameter(dbCommand, "PageSize", DbType.Int32, pageSize);
            db.AddInParameter(dbCommand, "PageIndex", DbType.Int32, pageIndex);

            string commaSeparatedSpecIds = string.Empty;
            if (filteredSpecs != null)
            {
                filteredSpecs.Sort();
                for (int i = 0; i < filteredSpecs.Count; i++)
                {
                    commaSeparatedSpecIds += filteredSpecs[i].ToString();
                    if (i != filteredSpecs.Count - 1)
                    {
                        commaSeparatedSpecIds += ",";
                    }
                }
            }
            db.AddInParameter(dbCommand, "FilteredSpecs", DbType.String, commaSeparatedSpecIds);
            db.AddInParameter(dbCommand, "LanguageID", DbType.Int32, languageId);
            db.AddInParameter(dbCommand, "OrderBy", DbType.Int32, orderBy);
            db.AddOutParameter(dbCommand, "TotalRecords", DbType.Int32, 0);

            using (IDataReader dataReader = db.ExecuteReader(dbCommand))
            {
                while (dataReader.Read())
                {
                    var item = GetProductFromReader(dataReader);
                    result.Add(item);
                }
            }
            totalRecords = Convert.ToInt32(db.GetParameterValue(dbCommand, "@TotalRecords"));

            return result;
        }

        /// <summary>
        /// Gets all products displayed on the home page
        /// </summary>
        /// <param name="showHidden">A value indicating whether to show hidden records</param>
        /// <param name="languageId">Language identifier</param>
        /// <returns>Product collection</returns>
        public override DBProductCollection GetAllProductsDisplayedOnHomePage(bool showHidden,
            int languageId)
        {
            var result = new DBProductCollection();
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_ProductLoadDisplayedOnHomePage");
            db.AddInParameter(dbCommand, "ShowHidden", DbType.Boolean, showHidden);
            db.AddInParameter(dbCommand, "LanguageID", DbType.Int32, languageId);
            using (IDataReader dataReader = db.ExecuteReader(dbCommand))
            {
                while (dataReader.Read())
                {
                    var item = GetProductFromReader(dataReader);
                    result.Add(item);
                }
            }

            return result;
        }

        /// <summary>
        /// Gets a product
        /// </summary>
        /// <param name="productId">Product identifier</param>
        /// <param name="languageId">Language identifier</param>
        /// <returns>Product</returns>
        public override DBProduct GetProductById(int productId, int languageId)
        {
            DBProduct item = null;
            if (productId == 0)
                return item;
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_ProductLoadByPrimaryKey");
            db.AddInParameter(dbCommand, "ProductID", DbType.Int32, productId);
            db.AddInParameter(dbCommand, "LanguageID", DbType.Int32, languageId);
            using (IDataReader dataReader = db.ExecuteReader(dbCommand))
            {
                if (dataReader.Read())
                {
                    item = GetProductFromReader(dataReader);
                }
            }
            return item;
        }

        /// <summary>
        /// Inserts a product
        /// </summary>
        /// <param name="name">The name</param>
        /// <param name="shortDescription">The short description</param>
        /// <param name="fullDescription">The full description</param>
        /// <param name="adminComment">The admin comment</param>
        /// <param name="productTypeId">The product type identifier</param>
        /// <param name="templateId">The template identifier</param>
        /// <param name="showOnHomePage">A value indicating whether to show the product on the home page</param>
        /// <param name="metaKeywords">The meta keywords</param>
        /// <param name="metaDescription">The meta description</param>
        /// <param name="metaTitle">The meta title</param>
        /// <param name="seName">The search-engine name</param>
        /// <param name="allowCustomerReviews">A value indicating whether the product allows customer reviews</param>
        /// <param name="allowCustomerRatings">A value indicating whether the product allows customer ratings</param>
        /// <param name="ratingSum">The rating sum</param>
        /// <param name="totalRatingVotes">The total rating votes</param>
        /// <param name="published">A value indicating whether the entity is published</param>
        /// <param name="deleted">A value indicating whether the entity has been deleted</param>
        /// <param name="createdOn">The date and time of product creation</param>
        /// <param name="updatedOn">The date and time of product update</param>
        /// <returns>Product</returns>
        public override DBProduct InsertProduct(string name, string shortDescription,
            string fullDescription, string adminComment, int productTypeId,
            int templateId, bool showOnHomePage,
            string metaKeywords, string metaDescription, string metaTitle,
            string seName, bool allowCustomerReviews, bool allowCustomerRatings,
            int ratingSum, int totalRatingVotes, bool published,
            bool deleted, DateTime createdOn, DateTime updatedOn)
        {
            DBProduct item = null;
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_ProductInsert");
            db.AddOutParameter(dbCommand, "ProductID", DbType.Int32, 0);
            db.AddInParameter(dbCommand, "Name", DbType.String, name);
            db.AddInParameter(dbCommand, "ShortDescription", DbType.String, shortDescription);
            db.AddInParameter(dbCommand, "FullDescription", DbType.String, fullDescription);
            db.AddInParameter(dbCommand, "AdminComment", DbType.String, adminComment);
            db.AddInParameter(dbCommand, "ProductTypeID", DbType.Int32, productTypeId);
            db.AddInParameter(dbCommand, "TemplateID", DbType.Int32, templateId);
            db.AddInParameter(dbCommand, "ShowOnHomePage", DbType.Boolean, showOnHomePage);
            db.AddInParameter(dbCommand, "MetaKeywords", DbType.String, metaKeywords);
            db.AddInParameter(dbCommand, "MetaDescription", DbType.String, metaDescription);
            db.AddInParameter(dbCommand, "MetaTitle", DbType.String, metaTitle);
            db.AddInParameter(dbCommand, "SEName", DbType.String, seName);
            db.AddInParameter(dbCommand, "AllowCustomerReviews", DbType.Boolean, allowCustomerReviews);
            db.AddInParameter(dbCommand, "AllowCustomerRatings", DbType.Boolean, allowCustomerRatings);
            db.AddInParameter(dbCommand, "RatingSum", DbType.Int32, ratingSum);
            db.AddInParameter(dbCommand, "TotalRatingVotes", DbType.Int32, totalRatingVotes);
            db.AddInParameter(dbCommand, "Published", DbType.Boolean, published);
            db.AddInParameter(dbCommand, "Deleted", DbType.Boolean, deleted);
            db.AddInParameter(dbCommand, "CreatedOn", DbType.DateTime, createdOn);
            db.AddInParameter(dbCommand, "UpdatedOn", DbType.DateTime, updatedOn);
            if (db.ExecuteNonQuery(dbCommand) > 0)
            {
                int productId = Convert.ToInt32(db.GetParameterValue(dbCommand, "@ProductID"));
                item = GetProductById(productId, 0);
            }

            return item;
        }

        /// <summary>
        /// Updates the product
        /// </summary>
        /// <param name="productId">Product identifier</param>
        /// <param name="name">The name</param>
        /// <param name="shortDescription">The short description</param>
        /// <param name="fullDescription">The full description</param>
        /// <param name="adminComment">The admin comment</param>
        /// <param name="productTypeId">The product type identifier</param>
        /// <param name="templateId">The template identifier</param>
        /// <param name="showOnHomePage">A value indicating whether to show the product on the home page</param>
        /// <param name="metaKeywords">The meta keywords</param>
        /// <param name="metaDescription">The meta description</param>
        /// <param name="metaTitle">The meta title</param>
        /// <param name="seName">The search-engine name</param>
        /// <param name="allowCustomerReviews">A value indicating whether the product allows customer reviews</param>
        /// <param name="allowCustomerRatings">A value indicating whether the product allows customer ratings</param>
        /// <param name="ratingSum">The rating sum</param>
        /// <param name="totalRatingVotes">The total rating votes</param>
        /// <param name="published">A value indicating whether the entity is published</param>
        /// <param name="deleted">A value indicating whether the entity has been deleted</param>
        /// <param name="createdOn">The date and time of product creation</param>
        /// <param name="updatedOn">The date and time of product update</param>
        /// <returns>Product</returns>
        public override DBProduct UpdateProduct(int productId,
            string name, string shortDescription,
            string fullDescription, string adminComment, int productTypeId,
            int templateId, bool showOnHomePage,
            string metaKeywords, string metaDescription, string metaTitle,
            string seName, bool allowCustomerReviews, bool allowCustomerRatings,
            int ratingSum, int totalRatingVotes, bool published,
            bool deleted, DateTime createdOn, DateTime updatedOn)
        {
            DBProduct item = null;
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_ProductUpdate");
            db.AddInParameter(dbCommand, "ProductID", DbType.Int32, productId);
            db.AddInParameter(dbCommand, "Name", DbType.String, name);
            db.AddInParameter(dbCommand, "ShortDescription", DbType.String, shortDescription);
            db.AddInParameter(dbCommand, "FullDescription", DbType.String, fullDescription);
            db.AddInParameter(dbCommand, "AdminComment", DbType.String, adminComment);
            db.AddInParameter(dbCommand, "ProductTypeID", DbType.Int32, productTypeId);
            db.AddInParameter(dbCommand, "TemplateID", DbType.Int32, templateId);
            db.AddInParameter(dbCommand, "ShowOnHomePage", DbType.Boolean, showOnHomePage);
            db.AddInParameter(dbCommand, "MetaKeywords", DbType.String, metaKeywords);
            db.AddInParameter(dbCommand, "MetaDescription", DbType.String, metaDescription);
            db.AddInParameter(dbCommand, "MetaTitle", DbType.String, metaTitle);
            db.AddInParameter(dbCommand, "SEName", DbType.String, seName);
            db.AddInParameter(dbCommand, "AllowCustomerReviews", DbType.Boolean, allowCustomerReviews);
            db.AddInParameter(dbCommand, "AllowCustomerRatings", DbType.Boolean, allowCustomerRatings);
            db.AddInParameter(dbCommand, "RatingSum", DbType.Int32, ratingSum);
            db.AddInParameter(dbCommand, "TotalRatingVotes", DbType.Int32, totalRatingVotes);
            db.AddInParameter(dbCommand, "Published", DbType.Boolean, published);
            db.AddInParameter(dbCommand, "Deleted", DbType.Boolean, deleted);
            db.AddInParameter(dbCommand, "CreatedOn", DbType.DateTime, createdOn);
            db.AddInParameter(dbCommand, "UpdatedOn", DbType.DateTime, updatedOn);
            if (db.ExecuteNonQuery(dbCommand) > 0)
                item = GetProductById(productId, 0);

            return item;
        }

        /// <summary>
        /// Gets localized product by id
        /// </summary>
        /// <param name="productLocalizedId">Localized product identifier</param>
        /// <returns>Product content</returns>
        public override DBProductLocalized GetProductLocalizedById(int productLocalizedId)
        {
            DBProductLocalized item = null;
            if (productLocalizedId == 0)
                return item;
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_ProductLocalizedLoadByPrimaryKey");
            db.AddInParameter(dbCommand, "ProductLocalizedID", DbType.Int32, productLocalizedId);
            using (IDataReader dataReader = db.ExecuteReader(dbCommand))
            {
                if (dataReader.Read())
                {
                    item = GetProductLocalizedFromReader(dataReader);
                }
            }
            return item;
        }

        /// <summary>
        /// Gets localized product by product id and language id
        /// </summary>
        /// <param name="productId">Product identifier</param>
        /// <param name="languageId">Language identifier</param>
        /// <returns>Product content</returns>
        public override DBProductLocalized GetProductLocalizedByProductIdAndLanguageId(int productId, int languageId)
        {
            DBProductLocalized item = null;
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_ProductLocalizedLoadByProductIDAndLanguageID");
            db.AddInParameter(dbCommand, "ProductID", DbType.Int32, productId);
            db.AddInParameter(dbCommand, "LanguageID", DbType.Int32, languageId);
            using (IDataReader dataReader = db.ExecuteReader(dbCommand))
            {
                if (dataReader.Read())
                {
                    item = GetProductLocalizedFromReader(dataReader);
                }
            }
            return item;
        }

        /// <summary>
        /// Gets all product variants
        /// </summary>
        /// <param name="categoryId">Category identifier</param>
        /// <param name="manufacturerId">Manufacturer identifier</param>
        /// <param name="keywords">Keywords</param>
        /// <param name="showHidden">A value indicating whether to show hidden records</param>
        /// <param name="pageSize">Page size</param>
        /// <param name="pageIndex">Page index</param>
        /// <param name="totalRecords">Total records</param>
        /// <returns>Product variants</returns>
        public override DBProductVariantCollection GetAllProductVariants(int categoryId,
            int manufacturerId, string keywords, bool showHidden,
            int pageSize, int pageIndex, out int totalRecords)
        {
            totalRecords = 0;
            var result = new DBProductVariantCollection();
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_ProductVariantLoadAll");
            db.AddInParameter(dbCommand, "CategoryID", DbType.Int32, categoryId);
            db.AddInParameter(dbCommand, "ManufacturerID", DbType.Int32, manufacturerId);
            db.AddInParameter(dbCommand, "Keywords", DbType.String, keywords);
            db.AddInParameter(dbCommand, "ShowHidden", DbType.Boolean, showHidden);
            db.AddInParameter(dbCommand, "PageSize", DbType.Int32, pageSize);
            db.AddInParameter(dbCommand, "PageIndex", DbType.Int32, pageIndex);
            db.AddOutParameter(dbCommand, "TotalRecords", DbType.Int32, 0);

            using (IDataReader dataReader = db.ExecuteReader(dbCommand))
            {
                while (dataReader.Read())
                {
                    var item = GetProductVariantFromReader(dataReader);
                    result.Add(item);
                }
            }
            totalRecords = Convert.ToInt32(db.GetParameterValue(dbCommand, "@TotalRecords"));

            return result;
        }

        /// <summary>
        /// Inserts a localized product
        /// </summary>
        /// <param name="productId">Product identifier</param>
        /// <param name="languageId">Language identifier</param>
        /// <param name="name">Name text</param>
        /// <param name="shortDescription">The short description</param>
        /// <param name="fullDescription">The full description</param>
        /// <param name="metaKeywords">Meta keywords text</param>
        /// <param name="metaDescription">Meta descriptions text</param>
        /// <param name="metaTitle">Metat title text</param>
        /// <param name="seName">Se Name text</param>
        /// <returns>Product content</returns>
        public override DBProductLocalized InsertProductLocalized(int productId,
            int languageId, string name, string shortDescription, string fullDescription,
            string metaKeywords, string metaDescription, string metaTitle,
            string seName)
        {
            DBProductLocalized item = null;
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_ProductLocalizedInsert");
            db.AddOutParameter(dbCommand, "ProductLocalizedID", DbType.Int32, 0);
            db.AddInParameter(dbCommand, "ProductID", DbType.Int32, productId);
            db.AddInParameter(dbCommand, "LanguageID", DbType.Int32, languageId);
            db.AddInParameter(dbCommand, "Name", DbType.String, name);
            db.AddInParameter(dbCommand, "ShortDescription", DbType.String, shortDescription);
            db.AddInParameter(dbCommand, "FullDescription", DbType.String, fullDescription);
            db.AddInParameter(dbCommand, "MetaKeywords", DbType.String, metaKeywords);
            db.AddInParameter(dbCommand, "MetaDescription", DbType.String, metaDescription);
            db.AddInParameter(dbCommand, "MetaTitle", DbType.String, metaTitle);
            db.AddInParameter(dbCommand, "SEName", DbType.String, seName);
            if (db.ExecuteNonQuery(dbCommand) > 0)
            {
                int productLocalizedId = Convert.ToInt32(db.GetParameterValue(dbCommand, "@ProductLocalizedID"));
                item = GetProductLocalizedById(productLocalizedId);
            }
            return item;
        }

        /// <summary>
        /// Update a localized product
        /// </summary>
        /// <param name="productLocalizedId">Localized product identifier</param>
        /// <param name="productId">Product identifier</param>
        /// <param name="languageId">Language identifier</param>
        /// <param name="name">Name text</param>
        /// <param name="shortDescription">The short description</param>
        /// <param name="fullDescription">The full description</param>
        /// <param name="metaKeywords">Meta keywords text</param>
        /// <param name="metaDescription">Meta descriptions text</param>
        /// <param name="metaTitle">Metat title text</param>
        /// <param name="seName">Se Name text</param>
        /// <returns>Product content</returns>
        public override DBProductLocalized UpdateProductLocalized(int productLocalizedId,
            int productId, int languageId, string name, string shortDescription,
            string fullDescription, string metaKeywords, string metaDescription,
            string metaTitle, string seName)
        {
            DBProductLocalized item = null;
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_ProductLocalizedUpdate");
            db.AddInParameter(dbCommand, "ProductLocalizedID", DbType.Int32, productLocalizedId);
            db.AddInParameter(dbCommand, "ProductID", DbType.Int32, productId);
            db.AddInParameter(dbCommand, "LanguageID", DbType.Int32, languageId);
            db.AddInParameter(dbCommand, "Name", DbType.String, name);
            db.AddInParameter(dbCommand, "ShortDescription", DbType.String, shortDescription);
            db.AddInParameter(dbCommand, "FullDescription", DbType.String, fullDescription);
            db.AddInParameter(dbCommand, "MetaKeywords", DbType.String, metaKeywords);
            db.AddInParameter(dbCommand, "MetaDescription", DbType.String, metaDescription);
            db.AddInParameter(dbCommand, "MetaTitle", DbType.String, metaTitle);
            db.AddInParameter(dbCommand, "SEName", DbType.String, seName);
            if (db.ExecuteNonQuery(dbCommand) > 0)
                item = GetProductLocalizedById(productLocalizedId);

            return item;
        }

        /// <summary>
        /// Gets localized product variant by id
        /// </summary>
        /// <param name="productVariantLocalizedId">Localized product variant identifier</param>
        /// <returns>Product variant content</returns>
        public override DBProductVariantLocalized GetProductVariantLocalizedById(int productVariantLocalizedId)
        {
            DBProductVariantLocalized item = null;
            if (productVariantLocalizedId == 0)
                return item;
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_ProductVariantLocalizedLoadByPrimaryKey");
            db.AddInParameter(dbCommand, "ProductVariantLocalizedID", DbType.Int32, productVariantLocalizedId);
            using (IDataReader dataReader = db.ExecuteReader(dbCommand))
            {
                if (dataReader.Read())
                {
                    item = GetProductVariantLocalizedFromReader(dataReader);
                }
            }
            return item;
        }

        /// <summary>
        /// Gets localized product variant by product variant id and language id
        /// </summary>
        /// <param name="productVariantId">Product variant identifier</param>
        /// <param name="languageId">Language identifier</param>
        /// <returns>Product variant content</returns>
        public override DBProductVariantLocalized GetProductVariantLocalizedByProductVariantIdAndLanguageId(int productVariantId, int languageId)
        {
            DBProductVariantLocalized item = null;
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_ProductVariantLocalizedLoadByProductVariantIDAndLanguageID");
            db.AddInParameter(dbCommand, "ProductVariantID", DbType.Int32, productVariantId);
            db.AddInParameter(dbCommand, "LanguageID", DbType.Int32, languageId);
            using (IDataReader dataReader = db.ExecuteReader(dbCommand))
            {
                if (dataReader.Read())
                {
                    item = GetProductVariantLocalizedFromReader(dataReader);
                }
            }
            return item;
        }

        /// <summary>
        /// Inserts a localized product variant
        /// </summary>
        /// <param name="productVariantId">Product variant identifier</param>
        /// <param name="languageId">Language identifier</param>
        /// <param name="name">Name text</param>
        /// <param name="description">Description text</param>
        /// <returns>Product variant content</returns>
        public override DBProductVariantLocalized InsertProductVariantLocalized(int productVariantId,
            int languageId, string name, string description)
        {
            DBProductVariantLocalized item = null;
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_ProductVariantLocalizedInsert");
            db.AddOutParameter(dbCommand, "ProductVariantLocalizedID", DbType.Int32, 0);
            db.AddInParameter(dbCommand, "ProductVariantID", DbType.Int32, productVariantId);
            db.AddInParameter(dbCommand, "LanguageID", DbType.Int32, languageId);
            db.AddInParameter(dbCommand, "Name", DbType.String, name);
            db.AddInParameter(dbCommand, "Description", DbType.String, description);
            if (db.ExecuteNonQuery(dbCommand) > 0)
            {
                int productVariantLocalizedId = Convert.ToInt32(db.GetParameterValue(dbCommand, "@ProductVariantLocalizedID"));
                item = GetProductVariantLocalizedById(productVariantLocalizedId);
            }
            return item;
        }

        /// <summary>
        /// Update a localized product variant
        /// </summary>
        /// <param name="productVariantLocalizedId">Localized product variant identifier</param>
        /// <param name="productVariantId">Product variant identifier</param>
        /// <param name="languageId">Language identifier</param>
        /// <param name="name">Name text</param>
        /// <param name="description">Description text</param>
        /// <returns>Product variant content</returns>
        public override DBProductVariantLocalized UpdateProductVariantLocalized(int productVariantLocalizedId,
            int productVariantId, int languageId, string name, string description)
        {
            DBProductVariantLocalized item = null;
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_ProductVariantLocalizedUpdate");
            db.AddInParameter(dbCommand, "ProductVariantLocalizedID", DbType.Int32, productVariantLocalizedId);
            db.AddInParameter(dbCommand, "ProductVariantID", DbType.Int32, productVariantId);
            db.AddInParameter(dbCommand, "LanguageID", DbType.Int32, languageId);
            db.AddInParameter(dbCommand, "Name", DbType.String, name);
            db.AddInParameter(dbCommand, "Description", DbType.String, description);
            if (db.ExecuteNonQuery(dbCommand) > 0)
                item = GetProductVariantLocalizedById(productVariantLocalizedId);

            return item;
        }

        /// <summary>
        /// Gets a list of products purchased by other customers who purchased the above
        /// </summary>
        /// <param name="productId">Product identifier</param>
        /// <param name="languageId">Language identifier</param>
        /// <param name="showHidden">A value indicating whether to show hidden records</param>
        /// <param name="pageSize">Page size</param>
        /// <param name="pageIndex">Page index</param>
        /// <param name="totalRecords">Total records</param>
        /// <returns>Product collection</returns>
        public override DBProductCollection GetProductsAlsoPurchasedById(int productId,
            int languageId, bool showHidden, int pageSize, int pageIndex, out int totalRecords)
        {
            totalRecords = 0;
            var result = new DBProductCollection();
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_ProductAlsoPurchasedLoadByProductID");
            db.AddInParameter(dbCommand, "ProductID", DbType.Int32, productId);
            db.AddInParameter(dbCommand, "LanguageID", DbType.Int32, languageId);
            db.AddInParameter(dbCommand, "ShowHidden", DbType.Boolean, showHidden);
            db.AddInParameter(dbCommand, "PageSize", DbType.Int32, pageSize);
            db.AddInParameter(dbCommand, "PageIndex", DbType.Int32, pageIndex);
            db.AddOutParameter(dbCommand, "TotalRecords", DbType.Int32, 0);
            using (IDataReader dataReader = db.ExecuteReader(dbCommand))
            {
                while (dataReader.Read())
                {
                    var item = GetProductFromReader(dataReader);
                    result.Add(item);
                }
            }
            totalRecords = Convert.ToInt32(db.GetParameterValue(dbCommand, "@TotalRecords"));

            return result;
        }

        /// <summary>
        /// Sets a product rating
        /// </summary>
        /// <param name="productId">Product identifer</param>
        /// <param name="customerId">Customer identifer</param>
        /// <param name="rating">Rating</param>
        /// <param name="ratedOn">Rating was created on</param>
        public override void SetProductRating(int productId, int customerId,
            int rating, DateTime ratedOn)
        {
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_ProductRatingCreate");
            db.AddInParameter(dbCommand, "ProductID", DbType.Int32, productId);
            db.AddInParameter(dbCommand, "CustomerID", DbType.Int32, customerId);
            db.AddInParameter(dbCommand, "Rating", DbType.Int32, rating);
            db.AddInParameter(dbCommand, "RatedOn", DbType.DateTime, ratedOn);
            db.ExecuteNonQuery(dbCommand);
        }

        /// <summary>
        /// Gets a recently added products list
        /// </summary>
        /// <param name="number">Number of products to load</param>
        /// <param name="languageId">Language identifier</param>
        /// <param name="showHidden">A value indicating whether to show hidden records</param>
        /// <returns>Recently added products list</returns>
        public override DBProductCollection GetRecentlyAddedProducts(int number,
            int languageId, bool showHidden)
        {
            var result = new DBProductCollection();
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_ProductLoadRecentlyAdded");
            db.AddInParameter(dbCommand, "Number", DbType.Int32, number);
            db.AddInParameter(dbCommand, "LanguageID", DbType.Int32, languageId);
            db.AddInParameter(dbCommand, "ShowHidden", DbType.Boolean, showHidden);
            using (IDataReader dataReader = db.ExecuteReader(dbCommand))
            {
                while (dataReader.Read())
                {
                    var item = GetProductFromReader(dataReader);
                    result.Add(item);
                }
            }

            return result;
        }

        /// <summary>
        /// Gets a product variant
        /// </summary>
        /// <param name="productVariantId">Product variant identifier</param>
        /// <param name="languageId">Language identifier</param>
        /// <returns>Product variant</returns>
        public override DBProductVariant GetProductVariantById(int productVariantId,
            int languageId)
        {
            DBProductVariant item = null;
            if (productVariantId == 0)
                return item;
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_ProductVariantLoadByPrimaryKey");
            db.AddInParameter(dbCommand, "ProductVariantID", DbType.Int32, productVariantId);
            db.AddInParameter(dbCommand, "LanguageID", DbType.Int32, languageId);
            using (IDataReader dataReader = db.ExecuteReader(dbCommand))
            {
                if (dataReader.Read())
                {
                    item = GetProductVariantFromReader(dataReader);
                }
            }
            return item;
        }

        /// <summary>
        /// Gets a product variant by SKU
        /// </summary>
        /// <param name="sku">SKU</param>
        /// <returns>Product variant</returns>
        public override DBProductVariant GetProductVariantBySKU(string sku)
        {
            DBProductVariant item = null;
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_ProductVariantLoadBySKU");
            db.AddInParameter(dbCommand, "SKU", DbType.String, sku);
            using (IDataReader dataReader = db.ExecuteReader(dbCommand))
            {
                if (dataReader.Read())
                {
                    item = GetProductVariantFromReader(dataReader);
                }
            }
            return item;
        }

        /// <summary>
        /// Get low stock product variants
        /// </summary>
        /// <returns>Result</returns>
        public override DBProductVariantCollection GetLowStockProductVariants()
        {
            var result = new DBProductVariantCollection();
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_ProductVariantLoadLowStock");
            using (IDataReader dataReader = db.ExecuteReader(dbCommand))
            {
                while (dataReader.Read())
                {
                    var item = GetProductVariantFromReader(dataReader);
                    result.Add(item);
                }
            }

            return result;
        }

        /// <summary>
        /// Inserts a product variant
        /// </summary>
        /// <param name="productId">The product identifier</param>
        /// <param name="name">The name</param>
        /// <param name="sku">The SKU</param>
        /// <param name="description">The description</param>
        /// <param name="adminComment">The admin comment</param>
        /// <param name="manufacturerPartNumber">The manufacturer part number</param>
        /// <param name="isGiftCard">A value indicating whether the product variant is gift card</param>
        /// <param name="isDownload">A value indicating whether the product variant is download</param>
        /// <param name="downloadId">The download identifier</param>
        /// <param name="unlimitedDownloads">The value indicating whether this downloadable product can be downloaded unlimited number of times</param>
        /// <param name="maxNumberOfDownloads">The maximum number of downloads</param>
        /// <param name="downloadExpirationDays">The number of days during customers keeps access to the file</param>
        /// <param name="downloadActivationType">The download activation type</param>
        /// <param name="hasSampleDownload">The value indicating whether the product variant has a sample download file</param>
        /// <param name="sampleDownloadId">The sample download identifier</param>
        /// <param name="hasUserAgreement">A value indicating whether the product variant has a user agreement</param>
        /// <param name="userAgreementText">The text of user agreement</param>
        /// <param name="isRecurring">A value indicating whether the product variant is recurring</param>
        /// <param name="cycleLength">The cycle length</param>
        /// <param name="cyclePeriod">The cycle period</param>
        /// <param name="totalCycles">The total cycles</param>
        /// <param name="isShipEnabled">A value indicating whether the entity is ship enabled</param>
        /// <param name="isFreeShipping">A value indicating whether the entity is free shipping</param>
        /// <param name="additionalShippingCharge">The additional shipping charge</param>
        /// <param name="isTaxExempt">A value indicating whether the product variant is marked as tax exempt</param>
        /// <param name="taxCategoryId">The tax category identifier</param>
        /// <param name="manageInventory">The value indicating how to manage inventory</param>
        /// <param name="stockQuantity">The stock quantity</param>
        /// <param name="displayStockAvailability">The value indicating whether to display stock availability</param>
        /// <param name="minStockQuantity">The minimum stock quantity</param>
        /// <param name="lowStockActivityId">The low stock activity identifier</param>
        /// <param name="notifyAdminForQuantityBelow">The quantity when admin should be notified</param>
        /// <param name="allowOutOfStockOrders">The value indicating whether to allow orders when out of stock</param>
        /// <param name="orderMinimumQuantity">The order minimum quantity</param>
        /// <param name="orderMaximumQuantity">The order maximum quantity</param>
        /// <param name="warehouseId">The warehouse identifier</param>
        /// <param name="disableBuyButton">A value indicating whether to disable buy button</param>
        /// <param name="price">The price</param>
        /// <param name="oldPrice">The old price</param>
        /// <param name="productCost">The product cost</param>
        /// <param name="customerEntersPrice">The value indicating whether a customer enters price</param>
        /// <param name="minimumCustomerEnteredPrice">The minimum price entered by a customer</param>
        /// <param name="maximumCustomerEnteredPrice">The maximum price entered by a customer</param>
        /// <param name="weight">The weight</param>
        /// <param name="length">The length</param>
        /// <param name="width">The width</param>
        /// <param name="height">The height</param>
        /// <param name="pictureId">The picture identifier</param>
        /// <param name="availableStartDateTime">The available start date and time</param>
        /// <param name="availableEndDateTime">The available end date and time</param>
        /// <param name="published">A value indicating whether the entity is published</param>
        /// <param name="deleted">A value indicating whether the entity has been deleted</param>
        /// <param name="displayOrder">The display order</param>
        /// <param name="createdOn">The date and time of instance creation</param>
        /// <param name="updatedOn">The date and time of instance update</param>
        /// <returns>Product variant</returns>
        public override DBProductVariant InsertProductVariant(int productId,
            string name, string sku,
            string description, string adminComment, string manufacturerPartNumber,
            bool isGiftCard, bool isDownload, int downloadId, bool unlimitedDownloads,
            int maxNumberOfDownloads, int? downloadExpirationDays,
            int downloadActivationType, bool hasSampleDownload,
            int sampleDownloadId, bool hasUserAgreement,
            string userAgreementText, bool isRecurring,
            int cycleLength, int cyclePeriod, int totalCycles,
            bool isShipEnabled, bool isFreeShipping,
            decimal additionalShippingCharge, bool isTaxExempt, int taxCategoryId,
            int manageInventory, int stockQuantity, bool displayStockAvailability,
            int minStockQuantity, int lowStockActivityId,
            int notifyAdminForQuantityBelow, bool allowOutOfStockOrders,
            int orderMinimumQuantity, int orderMaximumQuantity,
            int warehouseId, bool disableBuyButton, decimal price,
            decimal oldPrice, decimal productCost, bool customerEntersPrice,
            decimal minimumCustomerEnteredPrice, decimal maximumCustomerEnteredPrice,
            decimal weight, decimal length, decimal width, decimal height, int pictureId,
            DateTime? availableStartDateTime, DateTime? availableEndDateTime,
            bool published, bool deleted, int displayOrder,
            DateTime createdOn, DateTime updatedOn)
        {
            if (availableStartDateTime.HasValue)
            {
                if (availableStartDateTime.Value.Year < 1900)
                    availableStartDateTime = new DateTime(1900, availableStartDateTime.Value.Month, availableStartDateTime.Value.Day);
                if (availableStartDateTime.Value.Year > 2998)
                    availableStartDateTime = new DateTime(2998, availableStartDateTime.Value.Month, availableStartDateTime.Value.Day);
            }
            if (availableEndDateTime.HasValue)
            {
                if (availableEndDateTime.Value.Year < 1900)
                    availableEndDateTime = new DateTime(1900, availableEndDateTime.Value.Month, availableEndDateTime.Value.Day);
                if (availableEndDateTime.Value.Year > 2998)
                    availableEndDateTime = new DateTime(2998, availableEndDateTime.Value.Month, availableEndDateTime.Value.Day);
            }

            DBProductVariant item = null;
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_ProductVariantInsert");
            db.AddOutParameter(dbCommand, "ProductVariantID", DbType.Int32, 0);
            db.AddInParameter(dbCommand, "ProductID", DbType.Int32, productId);
            db.AddInParameter(dbCommand, "Name", DbType.String, name);
            db.AddInParameter(dbCommand, "SKU", DbType.String, sku);
            db.AddInParameter(dbCommand, "Description", DbType.String, description);
            db.AddInParameter(dbCommand, "AdminComment", DbType.String, adminComment);
            db.AddInParameter(dbCommand, "ManufacturerPartNumber", DbType.String, manufacturerPartNumber);
            db.AddInParameter(dbCommand, "IsGiftCard", DbType.Boolean, isGiftCard);
            db.AddInParameter(dbCommand, "IsDownload", DbType.Boolean, isDownload);
            db.AddInParameter(dbCommand, "DownloadID", DbType.Int32, downloadId);
            db.AddInParameter(dbCommand, "UnlimitedDownloads", DbType.Boolean, unlimitedDownloads);
            db.AddInParameter(dbCommand, "MaxNumberOfDownloads", DbType.Int32, maxNumberOfDownloads);
            if (downloadExpirationDays.HasValue)
                db.AddInParameter(dbCommand, "DownloadExpirationDays", DbType.Int32, downloadExpirationDays.Value);
            else
                db.AddInParameter(dbCommand, "DownloadExpirationDays", DbType.Int32, DBNull.Value);
            db.AddInParameter(dbCommand, "DownloadActivationType", DbType.Int32, downloadActivationType);
            db.AddInParameter(dbCommand, "HasSampleDownload", DbType.Boolean, hasSampleDownload);
            db.AddInParameter(dbCommand, "SampleDownloadID", DbType.Int32, sampleDownloadId);
            db.AddInParameter(dbCommand, "HasUserAgreement", DbType.Boolean, hasUserAgreement);
            db.AddInParameter(dbCommand, "UserAgreementText", DbType.String, userAgreementText);
            db.AddInParameter(dbCommand, "IsRecurring", DbType.Boolean, isRecurring);
            db.AddInParameter(dbCommand, "CycleLength", DbType.Int32, cycleLength);
            db.AddInParameter(dbCommand, "CyclePeriod", DbType.Int32, cyclePeriod);
            db.AddInParameter(dbCommand, "TotalCycles", DbType.Int32, totalCycles);
            db.AddInParameter(dbCommand, "IsShipEnabled", DbType.Boolean, isShipEnabled);
            db.AddInParameter(dbCommand, "IsFreeShipping", DbType.Boolean, isFreeShipping);
            db.AddInParameter(dbCommand, "AdditionalShippingCharge", DbType.Decimal, additionalShippingCharge);
            db.AddInParameter(dbCommand, "IsTaxExempt", DbType.Boolean, isTaxExempt);
            db.AddInParameter(dbCommand, "TaxCategoryID", DbType.Int32, taxCategoryId);
            db.AddInParameter(dbCommand, "ManageInventory", DbType.Int32, manageInventory);
            db.AddInParameter(dbCommand, "StockQuantity", DbType.Int32, stockQuantity);
            db.AddInParameter(dbCommand, "DisplayStockAvailability", DbType.Boolean, displayStockAvailability);
            db.AddInParameter(dbCommand, "MinStockQuantity", DbType.Int32, minStockQuantity);
            db.AddInParameter(dbCommand, "LowStockActivityID", DbType.Int32, lowStockActivityId);
            db.AddInParameter(dbCommand, "NotifyAdminForQuantityBelow", DbType.Int32, notifyAdminForQuantityBelow);
            db.AddInParameter(dbCommand, "AllowOutOfStockOrders", DbType.Boolean, allowOutOfStockOrders);
            db.AddInParameter(dbCommand, "OrderMinimumQuantity", DbType.Int32, orderMinimumQuantity);
            db.AddInParameter(dbCommand, "OrderMaximumQuantity", DbType.Int32, orderMaximumQuantity);
            db.AddInParameter(dbCommand, "WarehouseId", DbType.Int32, warehouseId);
            db.AddInParameter(dbCommand, "DisableBuyButton", DbType.Boolean, disableBuyButton);
            db.AddInParameter(dbCommand, "Price", DbType.Decimal, price);
            db.AddInParameter(dbCommand, "OldPrice", DbType.Decimal, oldPrice);
            db.AddInParameter(dbCommand, "ProductCost", DbType.Decimal, productCost);
            db.AddInParameter(dbCommand, "CustomerEntersPrice", DbType.Boolean, customerEntersPrice);
            db.AddInParameter(dbCommand, "MinimumCustomerEnteredPrice", DbType.Decimal, minimumCustomerEnteredPrice);
            db.AddInParameter(dbCommand, "MaximumCustomerEnteredPrice", DbType.Decimal, maximumCustomerEnteredPrice);
            db.AddInParameter(dbCommand, "Weight", DbType.Decimal, weight);
            db.AddInParameter(dbCommand, "Length", DbType.Decimal, length);
            db.AddInParameter(dbCommand, "Width", DbType.Decimal, width);
            db.AddInParameter(dbCommand, "Height", DbType.Decimal, height);
            db.AddInParameter(dbCommand, "PictureID", DbType.Int32, pictureId);
            if (availableStartDateTime.HasValue)
                db.AddInParameter(dbCommand, "AvailableStartDateTime", DbType.DateTime, availableStartDateTime.Value);
            else
                db.AddInParameter(dbCommand, "AvailableStartDateTime", DbType.DateTime, DBNull.Value);
            if (availableEndDateTime.HasValue)
                db.AddInParameter(dbCommand, "AvailableEndDateTime", DbType.DateTime, availableEndDateTime.Value);
            else
                db.AddInParameter(dbCommand, "AvailableEndDateTime", DbType.DateTime, DBNull.Value);
            db.AddInParameter(dbCommand, "Published", DbType.Boolean, published);
            db.AddInParameter(dbCommand, "Deleted", DbType.Boolean, deleted);
            db.AddInParameter(dbCommand, "DisplayOrder", DbType.Int32, displayOrder);
            db.AddInParameter(dbCommand, "CreatedOn", DbType.DateTime, createdOn);
            db.AddInParameter(dbCommand, "UpdatedOn", DbType.DateTime, updatedOn);
            if (db.ExecuteNonQuery(dbCommand) > 0)
            {
                int productVariantId = Convert.ToInt32(db.GetParameterValue(dbCommand, "@ProductVariantID"));
                item = GetProductVariantById(productVariantId, 0);
            }
            return item;
        }

        /// <summary>
        /// Updates the product variant
        /// </summary>
        /// <param name="productVariantId">The product variant identifier</param>
        /// <param name="productId">The product identifier</param>
        /// <param name="name">The name</param>
        /// <param name="sku">The SKU</param>
        /// <param name="description">The description</param>
        /// <param name="adminComment">The admin comment</param>
        /// <param name="manufacturerPartNumber">The manufacturer part number</param>
        /// <param name="isGiftCard">A value indicating whether the product variant is gift card</param>
        /// <param name="isDownload">A value indicating whether the product variant is download</param>
        /// <param name="downloadId">The download identifier</param>
        /// <param name="unlimitedDownloads">The value indicating whether this downloadable product can be downloaded unlimited number of times</param>
        /// <param name="maxNumberOfDownloads">The maximum number of downloads</param>
        /// <param name="downloadExpirationDays">The number of days during customers keeps access to the file</param>
        /// <param name="downloadActivationType">The download activation type</param>
        /// <param name="hasSampleDownload">The value indicating whether the product variant has a sample download file</param>
        /// <param name="sampleDownloadId">The sample download identifier</param>
        /// <param name="hasUserAgreement">A value indicating whether the product variant has a user agreement</param>
        /// <param name="userAgreementText">The text of user agreement</param>
        /// <param name="isRecurring">A value indicating whether the product variant is recurring</param>
        /// <param name="cycleLength">The cycle length</param>
        /// <param name="cyclePeriod">The cycle period</param>
        /// <param name="totalCycles">The total cycles</param>
        /// <param name="isShipEnabled">A value indicating whether the entity is ship enabled</param>
        /// <param name="isFreeShipping">A value indicating whether the entity is free shipping</param>
        /// <param name="additionalShippingCharge">The additional shipping charge</param>
        /// <param name="isTaxExempt">A value indicating whether the product variant is marked as tax exempt</param>
        /// <param name="taxCategoryId">The tax category identifier</param>
        /// <param name="manageInventory">The value indicating how to manage inventory</param>
        /// <param name="stockQuantity">The stock quantity</param>
        /// <param name="displayStockAvailability">The value indicating whether to display stock availability</param>
        /// <param name="minStockQuantity">The minimum stock quantity</param>
        /// <param name="lowStockActivityId">The low stock activity identifier</param>
        /// <param name="notifyAdminForQuantityBelow">The quantity when admin should be notified</param>
        /// <param name="allowOutOfStockOrders">The value indicating whether to allow orders when out of stock</param>
        /// <param name="orderMinimumQuantity">The order minimum quantity</param>
        /// <param name="orderMaximumQuantity">The order maximum quantity</param>
        /// <param name="warehouseId">The warehouse identifier</param>
        /// <param name="disableBuyButton">A value indicating whether to disable buy button</param>
        /// <param name="price">The price</param>
        /// <param name="oldPrice">The old price</param>
        /// <param name="productCost">The product cost</param>
        /// <param name="customerEntersPrice">The value indicating whether a customer enters price</param>
        /// <param name="minimumCustomerEnteredPrice">The minimum price entered by a customer</param>
        /// <param name="maximumCustomerEnteredPrice">The maximum price entered by a customer</param>
        /// <param name="weight">The weight</param>
        /// <param name="length">The length</param>
        /// <param name="width">The width</param>
        /// <param name="height">The height</param>
        /// <param name="pictureId">The picture identifier</param>
        /// <param name="availableStartDateTime">The available start date and time</param>
        /// <param name="availableEndDateTime">The available end date and time</param>
        /// <param name="published">A value indicating whether the entity is published</param>
        /// <param name="deleted">A value indicating whether the entity has been deleted</param>
        /// <param name="displayOrder">The display order</param>
        /// <param name="createdOn">The date and time of instance creation</param>
        /// <param name="updatedOn">The date and time of instance update</param>
        /// <returns>Product variant</returns>
        public override DBProductVariant UpdateProductVariant(int productVariantId,
            int productId, string name, string sku,
            string description, string adminComment, string manufacturerPartNumber,
            bool isGiftCard, bool isDownload, int downloadId, bool unlimitedDownloads,
            int maxNumberOfDownloads, int? downloadExpirationDays,
            int downloadActivationType, bool hasSampleDownload,
            int sampleDownloadId, bool hasUserAgreement,
            string userAgreementText, bool isRecurring,
            int cycleLength, int cyclePeriod, int totalCycles,
            bool isShipEnabled, bool isFreeShipping,
            decimal additionalShippingCharge, bool isTaxExempt, int taxCategoryId,
            int manageInventory, int stockQuantity, bool displayStockAvailability,
            int minStockQuantity, int lowStockActivityId,
            int notifyAdminForQuantityBelow, bool allowOutOfStockOrders,
            int orderMinimumQuantity, int orderMaximumQuantity,
            int warehouseId, bool disableBuyButton, decimal price,
            decimal oldPrice, decimal productCost, bool customerEntersPrice,
            decimal minimumCustomerEnteredPrice, decimal maximumCustomerEnteredPrice,
            decimal weight, decimal length, decimal width, decimal height, int pictureId,
            DateTime? availableStartDateTime, DateTime? availableEndDateTime,
            bool published, bool deleted, int displayOrder,
            DateTime createdOn, DateTime updatedOn)
        {
            if (availableStartDateTime.HasValue)
            {
                if (availableStartDateTime.Value.Year < 1900)
                    availableStartDateTime = new DateTime(1900, availableStartDateTime.Value.Month, availableStartDateTime.Value.Day);
                if (availableStartDateTime.Value.Year > 2998)
                    availableStartDateTime = new DateTime(2998, availableStartDateTime.Value.Month, availableStartDateTime.Value.Day);
            }
            if (availableEndDateTime.HasValue)
            {
                if (availableEndDateTime.Value.Year < 1900)
                    availableEndDateTime = new DateTime(1900, availableEndDateTime.Value.Month, availableEndDateTime.Value.Day);
                if (availableEndDateTime.Value.Year > 2998)
                    availableEndDateTime = new DateTime(2998, availableEndDateTime.Value.Month, availableEndDateTime.Value.Day);
            }

            DBProductVariant item = null;
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_ProductVariantUpdate");
            db.AddInParameter(dbCommand, "ProductVariantID", DbType.Int32, productVariantId);
            db.AddInParameter(dbCommand, "ProductID", DbType.Int32, productId);
            db.AddInParameter(dbCommand, "Name", DbType.String, name);
            db.AddInParameter(dbCommand, "SKU", DbType.String, sku);
            db.AddInParameter(dbCommand, "Description", DbType.String, description);
            db.AddInParameter(dbCommand, "AdminComment", DbType.String, adminComment);
            db.AddInParameter(dbCommand, "ManufacturerPartNumber", DbType.String, manufacturerPartNumber);
            db.AddInParameter(dbCommand, "IsGiftCard", DbType.Boolean, isGiftCard);
            db.AddInParameter(dbCommand, "IsDownload", DbType.Boolean, isDownload);
            db.AddInParameter(dbCommand, "DownloadID", DbType.Int32, downloadId);
            db.AddInParameter(dbCommand, "UnlimitedDownloads", DbType.Boolean, unlimitedDownloads);
            db.AddInParameter(dbCommand, "MaxNumberOfDownloads", DbType.Int32, maxNumberOfDownloads);
            if (downloadExpirationDays.HasValue)
                db.AddInParameter(dbCommand, "DownloadExpirationDays", DbType.Int32, downloadExpirationDays.Value);
            else
                db.AddInParameter(dbCommand, "DownloadExpirationDays", DbType.Int32, DBNull.Value);
            db.AddInParameter(dbCommand, "DownloadActivationType", DbType.Int32, downloadActivationType);
            db.AddInParameter(dbCommand, "HasSampleDownload", DbType.Boolean, hasSampleDownload);
            db.AddInParameter(dbCommand, "SampleDownloadID", DbType.Int32, sampleDownloadId);
            db.AddInParameter(dbCommand, "HasUserAgreement", DbType.Boolean, hasUserAgreement);
            db.AddInParameter(dbCommand, "UserAgreementText", DbType.String, userAgreementText);
            db.AddInParameter(dbCommand, "IsRecurring", DbType.Boolean, isRecurring);
            db.AddInParameter(dbCommand, "CycleLength", DbType.Int32, cycleLength);
            db.AddInParameter(dbCommand, "CyclePeriod", DbType.Int32, cyclePeriod);
            db.AddInParameter(dbCommand, "TotalCycles", DbType.Int32, totalCycles);
            db.AddInParameter(dbCommand, "IsShipEnabled", DbType.Boolean, isShipEnabled);
            db.AddInParameter(dbCommand, "IsFreeShipping", DbType.Boolean, isFreeShipping);
            db.AddInParameter(dbCommand, "AdditionalShippingCharge", DbType.Decimal, additionalShippingCharge);
            db.AddInParameter(dbCommand, "IsTaxExempt", DbType.Boolean, isTaxExempt);
            db.AddInParameter(dbCommand, "TaxCategoryID", DbType.Int32, taxCategoryId);
            db.AddInParameter(dbCommand, "ManageInventory", DbType.Int32, manageInventory);
            db.AddInParameter(dbCommand, "StockQuantity", DbType.Int32, stockQuantity);
            db.AddInParameter(dbCommand, "DisplayStockAvailability", DbType.Boolean, displayStockAvailability);
            db.AddInParameter(dbCommand, "MinStockQuantity", DbType.Int32, minStockQuantity);
            db.AddInParameter(dbCommand, "LowStockActivityID", DbType.Int32, lowStockActivityId);
            db.AddInParameter(dbCommand, "NotifyAdminForQuantityBelow", DbType.Int32, notifyAdminForQuantityBelow);
            db.AddInParameter(dbCommand, "AllowOutOfStockOrders", DbType.Boolean, allowOutOfStockOrders);
            db.AddInParameter(dbCommand, "OrderMinimumQuantity", DbType.Int32, orderMinimumQuantity);
            db.AddInParameter(dbCommand, "OrderMaximumQuantity", DbType.Int32, orderMaximumQuantity);
            db.AddInParameter(dbCommand, "WarehouseId", DbType.Int32, warehouseId);
            db.AddInParameter(dbCommand, "DisableBuyButton", DbType.Boolean, disableBuyButton);
            db.AddInParameter(dbCommand, "Price", DbType.Decimal, price);
            db.AddInParameter(dbCommand, "OldPrice", DbType.Decimal, oldPrice);
            db.AddInParameter(dbCommand, "ProductCost", DbType.Decimal, productCost);
            db.AddInParameter(dbCommand, "CustomerEntersPrice", DbType.Boolean, customerEntersPrice);
            db.AddInParameter(dbCommand, "MinimumCustomerEnteredPrice", DbType.Decimal, minimumCustomerEnteredPrice);
            db.AddInParameter(dbCommand, "MaximumCustomerEnteredPrice", DbType.Decimal, maximumCustomerEnteredPrice);
            db.AddInParameter(dbCommand, "Weight", DbType.Decimal, weight);
            db.AddInParameter(dbCommand, "Length", DbType.Decimal, length);
            db.AddInParameter(dbCommand, "Width", DbType.Decimal, width);
            db.AddInParameter(dbCommand, "Height", DbType.Decimal, height);
            db.AddInParameter(dbCommand, "PictureID", DbType.Int32, pictureId);
            if (availableStartDateTime.HasValue)
                db.AddInParameter(dbCommand, "AvailableStartDateTime", DbType.DateTime, availableStartDateTime.Value);
            else
                db.AddInParameter(dbCommand, "AvailableStartDateTime", DbType.DateTime, DBNull.Value);
            if (availableEndDateTime.HasValue)
                db.AddInParameter(dbCommand, "AvailableEndDateTime", DbType.DateTime, availableEndDateTime.Value);
            else
                db.AddInParameter(dbCommand, "AvailableEndDateTime", DbType.DateTime, DBNull.Value);
            db.AddInParameter(dbCommand, "Published", DbType.Boolean, published);
            db.AddInParameter(dbCommand, "Deleted", DbType.Boolean, deleted);
            db.AddInParameter(dbCommand, "DisplayOrder", DbType.Int32, displayOrder);
            db.AddInParameter(dbCommand, "CreatedOn", DbType.DateTime, createdOn);
            db.AddInParameter(dbCommand, "UpdatedOn", DbType.DateTime, updatedOn);
            if (db.ExecuteNonQuery(dbCommand) > 0)
                item = GetProductVariantById(productVariantId, 0);

            return item;
        }

        /// <summary>
        /// Gets product variants by product identifier
        /// </summary>
        /// <param name="productId">The product identifier</param>
        /// <param name="languageId">Language identifier</param>
        /// <param name="showHidden">A value indicating whether to show hidden records</param>
        /// <returns>Product variant collection</returns>
        public override DBProductVariantCollection GetProductVariantsByProductId(int productId,
            int languageId, bool showHidden)
        {
            var result = new DBProductVariantCollection();
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_ProductVariantLoadByProductID");
            db.AddInParameter(dbCommand, "ProductID", DbType.Int32, productId);
            db.AddInParameter(dbCommand, "LanguageID", DbType.Int32, languageId);
            db.AddInParameter(dbCommand, "ShowHidden", DbType.Boolean, showHidden);
            using (IDataReader dataReader = db.ExecuteReader(dbCommand))
            {
                while (dataReader.Read())
                {
                    var item = GetProductVariantFromReader(dataReader);
                    result.Add(item);
                }
            }

            return result;
        }

        /// <summary>
        /// Gets restricted product variants by discount identifier
        /// </summary>
        /// <param name="discountId">The discount identifier</param>
        /// <returns>Product variant collection</returns>
        public override DBProductVariantCollection GetProductVariantsRestrictedByDiscountId(int discountId)
        {
            var result = new DBProductVariantCollection();
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_ProductVariantRestrictedLoadDiscountID");
            db.AddInParameter(dbCommand, "DiscountID", DbType.Int32, discountId);
            using (IDataReader dataReader = db.ExecuteReader(dbCommand))
            {
                while (dataReader.Read())
                {
                    var item = GetProductVariantFromReader(dataReader);
                    result.Add(item);
                }
            }

            return result;
        }

        /// <summary>
        /// Gets a related product collection by product identifier
        /// </summary>
        /// <param name="productId1">The first product identifier</param>
        /// <param name="showHidden">A value indicating whether to show hidden records</param>
        /// <returns>Related product collection</returns>
        public override DBRelatedProductCollection GetRelatedProductsByProductId1(int productId1, bool showHidden)
        {
            var result = new DBRelatedProductCollection();
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_RelatedProductLoadByProductID1");
            db.AddInParameter(dbCommand, "ProductID1", DbType.Int32, productId1);
            db.AddInParameter(dbCommand, "ShowHidden", DbType.Boolean, showHidden);
            using (IDataReader dataReader = db.ExecuteReader(dbCommand))
            {
                while (dataReader.Read())
                {
                    var item = GetRelatedProductFromReader(dataReader);
                    result.Add(item);
                }
            }

            return result;
        }

        /// <summary>
        /// Gets all product variants directly assigned to a pricelist
        /// </summary>
        /// <param name="pricelistId">Pricelist identifier</param>
        /// <returns>Product variants</returns>
        public override DBProductVariantCollection GetProductVariantsByPricelistId(int pricelistId)
        {
            var result = new DBProductVariantCollection();
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_ProductVariantLoadByPricelistID");
            db.AddInParameter(dbCommand, "PricelistID", DbType.Int32, pricelistId);
            using (IDataReader dataReader = db.ExecuteReader(dbCommand))
            {
                while (dataReader.Read())
                {
                    var item = GetProductVariantFromReader(dataReader);
                    result.Add(item);
                }
            }

            return result;
        }

        /// <summary>
        /// Deletes a product variant pricelist
        /// </summary>
        /// <param name="productVariantPricelistId">ProductVariantPricelist identifier</param>
        public override void DeleteProductVariantPricelist(int productVariantPricelistId)
        {
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_ProductVariant_Pricelist_MappingDelete");
            db.AddInParameter(dbCommand, "ProductVariantPricelistID", DbType.Int32, productVariantPricelistId);
            db.ExecuteNonQuery(dbCommand);
        }

        /// <summary>
        /// Gets a ProductVariantPricelist
        /// </summary>
        /// <param name="productVariantPricelistId">ProductVariantPricelist identifier</param>
        /// <returns>ProductVariantPricelist</returns>
        public override DBProductVariantPricelist GetProductVariantPricelistById(int productVariantPricelistId)
        {
            DBProductVariantPricelist item = null;
            if (productVariantPricelistId == 0)
                return item;
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_ProductVariant_Pricelist_MappingLoadByPrimaryKey");
            db.AddInParameter(dbCommand, "ProductVariantPricelistID", DbType.Int32, productVariantPricelistId);
            using (IDataReader dataReader = db.ExecuteReader(dbCommand))
            {
                if (dataReader.Read())
                {
                    item = GetProductVariantPricelistFromReader(dataReader);
                }
            }
            return item;
        }

        /// <summary>
        /// Gets ProductVariantPricelist
        /// </summary>
        /// <param name="productVariantId">ProductVariant identifier</param>
        /// <param name="pricelistId">Pricelist identifier</param>
        /// <returns>ProductVariantPricelist</returns>
        public override DBProductVariantPricelist GetProductVariantPricelist(int productVariantId, int pricelistId)
        {
            DBProductVariantPricelist item = null;
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_ProductVariant_Pricelist_MappingLoadByProductVariantIDAndPricelistID");
            db.AddInParameter(dbCommand, "ProductVariantID", DbType.Int32, productVariantId);
            db.AddInParameter(dbCommand, "PricelistID", DbType.Int32, pricelistId);
            using (IDataReader dataReader = db.ExecuteReader(dbCommand))
            {
                if (dataReader.Read())
                {
                    item = GetProductVariantPricelistFromReader(dataReader);
                }
            }
            return item;
        }

        /// <summary>
        /// Inserts a ProductVariantPricelist
        /// </summary>
        /// <param name="productVariantId">The product variant identifer</param>
        /// <param name="pricelistId">The pricelist identifier</param>
        /// <param name="priceAdjustmentTypeId">Price adjustment type identifier</param>
        /// <param name="priceAdjustment">The price will be adjusted by this amount</param>
        /// <param name="updatedOn">The date and time of instance update</param>
        /// <returns>ProductVariantPricelist</returns>
        public override DBProductVariantPricelist InsertProductVariantPricelist(int productVariantId,
            int pricelistId, int priceAdjustmentTypeId, decimal priceAdjustment,
            DateTime updatedOn)
        {
            DBProductVariantPricelist item = null;
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_ProductVariant_Pricelist_MappingInsert");
            db.AddOutParameter(dbCommand, "ProductVariantPricelistID", DbType.Int32, 0);
            db.AddInParameter(dbCommand, "ProductVariantID", DbType.Int32, productVariantId);
            db.AddInParameter(dbCommand, "PricelistID", DbType.Int32, pricelistId);
            db.AddInParameter(dbCommand, "PriceAdjustmentTypeID", DbType.Int32, priceAdjustmentTypeId);
            db.AddInParameter(dbCommand, "PriceAdjustment", DbType.Decimal, priceAdjustment);
            db.AddInParameter(dbCommand, "UpdatedOn", DbType.DateTime, updatedOn);
            if (db.ExecuteNonQuery(dbCommand) > 0)
            {
                int productVariantPricelistId = Convert.ToInt32(db.GetParameterValue(dbCommand, "@ProductVariantPricelistID"));
                item = GetProductVariantPricelistById(productVariantPricelistId);
            }

            return item;
        }

        /// <summary>
        /// Updates the ProductVariantPricelist
        /// </summary>
        /// <param name="productVariantPricelistId">The product variant pricelist identifier</param>
        /// <param name="productVariantId">The product variant identifer</param>
        /// <param name="pricelistId">The pricelist identifier</param>
        /// <param name="priceAdjustmentTypeId">Price adjustment type identifier</param>
        /// <param name="priceAdjustment">The price will be adjusted by this amount</param>
        /// <param name="updatedOn">The date and time of instance update</param>
        /// <returns>ProductVariantPricelist</returns>
        public override DBProductVariantPricelist UpdateProductVariantPricelist(int productVariantPricelistId,
            int productVariantId, int pricelistId, int priceAdjustmentTypeId,
            decimal priceAdjustment, DateTime updatedOn)
        {
            DBProductVariantPricelist item = null;
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_ProductVariant_Pricelist_MappingUpdate");
            db.AddInParameter(dbCommand, "ProductVariantPricelistID", DbType.Int32, productVariantPricelistId);
            db.AddInParameter(dbCommand, "ProductVariantID", DbType.Int32, productVariantId);
            db.AddInParameter(dbCommand, "PricelistID", DbType.Int32, pricelistId);
            db.AddInParameter(dbCommand, "PriceAdjustmentTypeID", DbType.Int32, priceAdjustmentTypeId);
            db.AddInParameter(dbCommand, "PriceAdjustment", DbType.Decimal, priceAdjustment);
            db.AddInParameter(dbCommand, "UpdatedOn", DbType.DateTime, updatedOn);
            if (db.ExecuteNonQuery(dbCommand) > 0)
                item = GetProductVariantPricelistById(productVariantPricelistId);

            return item;
        }

        /// <summary>
        /// Gets all product tags
        /// </summary>
        /// <param name="productId">Product identifier</param>
        /// <param name="name">Product tag name or empty string to load all records</param>
        /// <returns>Product tag collection</returns>
        public override DBProductTagCollection GetAllProductTags(int productId,
            string name)
        {
            var result = new DBProductTagCollection();
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_ProductTagLoadAll");
            db.AddInParameter(dbCommand, "ProductID", DbType.Int32, productId);
            db.AddInParameter(dbCommand, "Name", DbType.String, name);
            using (IDataReader dataReader = db.ExecuteReader(dbCommand))
            {
                while (dataReader.Read())
                {
                    var item = GetProductTagFromReader(dataReader);
                    result.Add(item);
                }
            }

            return result;
        }

        /// <summary>
        /// Adds a discount tag mapping
        /// </summary>
        /// <param name="productId">Product identifier</param>
        /// <param name="productTagId">Product tag identifier</param>
        public override void AddProductTagMapping(int productId, int productTagId)
        {
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_ProductTag_Product_MappingInsert");
            db.AddInParameter(dbCommand, "ProductID", DbType.Int32, productId);
            db.AddInParameter(dbCommand, "ProductTagID", DbType.Int32, productTagId);
            db.ExecuteNonQuery(dbCommand);
        }

        /// <summary>
        /// Removes a discount tag mapping
        /// </summary>
        /// <param name="productId">Product identifier</param>
        /// <param name="productTagId">Product tag identifier</param>
        public override void RemoveProductTagMapping(int productId, int productTagId)
        {
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_ProductTag_Product_MappingDelete");
            db.AddInParameter(dbCommand, "ProductID", DbType.Int32, productId);
            db.AddInParameter(dbCommand, "ProductTagID", DbType.Int32, productTagId);
            db.ExecuteNonQuery(dbCommand);
        }


        #endregion
    }
}