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
        /// Gets a list of products purchased by other customers who purchased the above
        /// </summary>
        /// <param name="productId">Product identifier</param>
        /// <param name="showHidden">A value indicating whether to show hidden records</param>
        /// <param name="pageSize">Page size</param>
        /// <param name="pageIndex">Page index</param>
        /// <param name="totalRecords">Total records</param>
        /// <returns>Product collection</returns>
        public override DBProductCollection GetProductsAlsoPurchasedById(int productId,
            bool showHidden, int pageSize, int pageIndex, out int totalRecords)
        {
            totalRecords = 0;
            var result = new DBProductCollection();
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_ProductAlsoPurchasedLoadByProductID");
            db.AddInParameter(dbCommand, "ProductID", DbType.Int32, productId);
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
        /// <param name="showHidden">A value indicating whether to show hidden records</param>
        /// <returns>Recently added products list</returns>
        public override DBProductCollection GetRecentlyAddedProducts(int number, bool showHidden)
        {
            var result = new DBProductCollection();
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_ProductLoadRecentlyAdded");
            db.AddInParameter(dbCommand, "Number", DbType.Int32, number);
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