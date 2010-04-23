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
using System.Linq;
using System.Text;
using System.Web;
using System.Xml;
using NopSolutions.NopCommerce.BusinessLogic.Caching;
using NopSolutions.NopCommerce.BusinessLogic.Configuration.Settings;
using NopSolutions.NopCommerce.BusinessLogic.Localization;
using NopSolutions.NopCommerce.BusinessLogic.Messages;
using NopSolutions.NopCommerce.BusinessLogic.Orders;
using NopSolutions.NopCommerce.BusinessLogic.Products.Attributes;
using NopSolutions.NopCommerce.BusinessLogic.Profile;
using NopSolutions.NopCommerce.BusinessLogic.Utils.Html;
using NopSolutions.NopCommerce.Common.Utils.Html;
using NopSolutions.NopCommerce.DataAccess;
using NopSolutions.NopCommerce.DataAccess.Products;
using NopSolutions.NopCommerce.BusinessLogic.CustomerManagement;
using NopSolutions.NopCommerce.BusinessLogic.Media;
using NopSolutions.NopCommerce.BusinessLogic.Categories;
using NopSolutions.NopCommerce.BusinessLogic.Manufacturers;
using NopSolutions.NopCommerce.BusinessLogic.Promo.Discounts;
using NopSolutions.NopCommerce.BusinessLogic.Products.Specs;
using NopSolutions.NopCommerce.BusinessLogic.Audit;
using NopSolutions.NopCommerce.Common;
using NopSolutions.NopCommerce.BusinessLogic.Directory;

namespace NopSolutions.NopCommerce.BusinessLogic.Products
{
    /// <summary>
    /// Product manager
    /// </summary>
    public partial class ProductManager
    {
        #region Constants
        private const string PRODUCTS_BY_ID_KEY = "Nop.product.id-{0}-{1}";
        private const string PRODUCTVARIANTS_ALL_KEY = "Nop.productvariant.all-{0}-{1}-{2}";
        private const string PRODUCTVARIANTS_BY_ID_KEY = "Nop.productvariant.id-{0}-{1}";
        private const string TIERPRICES_ALLBYPRODUCTVARIANTID_KEY = "Nop.tierprice.allbyproductvariantid-{0}";
        private const string PRODUCTS_PATTERN_KEY = "Nop.product.";
        private const string PRODUCTVARIANTS_PATTERN_KEY = "Nop.productvariant.";
        private const string TIERPRICES_PATTERN_KEY = "Nop.tierprice.";
        #endregion

        #region Utilities

        private static ProductCollection DBMapping(DBProductCollection dbCollection)
        {
            if (dbCollection == null)
                return null;

            var collection = new ProductCollection();
            foreach (var dbItem in dbCollection)
            {
                var item = DBMapping(dbItem);
                collection.Add(item);
            }

            return collection;
        }

        private static Product DBMapping(DBProduct dbItem)
        {
            if (dbItem == null)
                return null;

            var item = new Product();
            item.ProductId = dbItem.ProductId;
            item.Name = dbItem.Name;
            item.ShortDescription = dbItem.ShortDescription;
            item.FullDescription = dbItem.FullDescription;
            item.AdminComment = dbItem.AdminComment;
            item.ProductTypeId = dbItem.ProductTypeId;
            item.TemplateId = dbItem.TemplateId;
            item.ShowOnHomePage = dbItem.ShowOnHomePage;
            item.MetaKeywords = dbItem.MetaKeywords;
            item.MetaDescription = dbItem.MetaDescription;
            item.MetaTitle = dbItem.MetaTitle;
            item.SEName = dbItem.SEName;
            item.AllowCustomerReviews = dbItem.AllowCustomerReviews;
            item.AllowCustomerRatings = dbItem.AllowCustomerRatings;
            item.RatingSum = dbItem.RatingSum;
            item.TotalRatingVotes = dbItem.TotalRatingVotes;
            item.Published = dbItem.Published;
            item.Deleted = dbItem.Deleted;
            item.CreatedOn = dbItem.CreatedOn;
            item.UpdatedOn = dbItem.UpdatedOn;

            return item;
        }

        private static ProductPictureCollection DBMapping(DBProductPictureCollection dbCollection)
        {
            if (dbCollection == null)
                return null;

            var collection = new ProductPictureCollection();
            foreach (var dbItem in dbCollection)
            {
                var item = DBMapping(dbItem);
                collection.Add(item);
            }

            return collection;
        }

        private static ProductPicture DBMapping(DBProductPicture dbItem)
        {
            if (dbItem == null)
                return null;

            var item = new ProductPicture();
            item.ProductPictureId = dbItem.ProductPictureId;
            item.ProductId = dbItem.ProductId;
            item.PictureId = dbItem.PictureId;
            item.DisplayOrder = dbItem.DisplayOrder;

            return item;
        }

        private static ProductReviewCollection DBMapping(DBProductReviewCollection dbCollection)
        {
            if (dbCollection == null)
                return null;

            var collection = new ProductReviewCollection();
            foreach (var dbItem in dbCollection)
            {
                var item = DBMapping(dbItem);
                collection.Add(item);
            }

            return collection;
        }

        private static ProductReview DBMapping(DBProductReview dbItem)
        {
            if (dbItem == null)
                return null;

            var item = new ProductReview();
            item.ProductReviewId = dbItem.ProductReviewId;
            item.ProductId = dbItem.ProductId;
            item.CustomerId = dbItem.CustomerId;
            item.Title = dbItem.Title;
            item.ReviewText = dbItem.ReviewText;
            item.Rating = dbItem.Rating;
            item.HelpfulYesTotal = dbItem.HelpfulYesTotal;
            item.HelpfulNoTotal = dbItem.HelpfulNoTotal;
            item.IsApproved = dbItem.IsApproved;
            item.CreatedOn = dbItem.CreatedOn;

            return item;
        }

        private static ProductTypeCollection DBMapping(DBProductTypeCollection dbCollection)
        {
            if (dbCollection == null)
                return null;

            var collection = new ProductTypeCollection();
            foreach (var dbItem in dbCollection)
            {
                var item = DBMapping(dbItem);
                collection.Add(item);
            }

            return collection;
        }

        private static ProductType DBMapping(DBProductType dbItem)
        {
            if (dbItem == null)
                return null;

            var item = new ProductType();
            item.ProductTypeId = dbItem.ProductTypeId;
            item.Name = dbItem.Name;
            item.DisplayOrder = dbItem.DisplayOrder;
            item.CreatedOn = dbItem.CreatedOn;
            item.UpdatedOn = dbItem.UpdatedOn;

            return item;
        }

        private static ProductVariantCollection DBMapping(DBProductVariantCollection dbCollection)
        {
            if (dbCollection == null)
                return null;

            var collection = new ProductVariantCollection();
            foreach (var dbItem in dbCollection)
            {
                var item = DBMapping(dbItem);
                collection.Add(item);
            }

            return collection;
        }

        private static ProductVariant DBMapping(DBProductVariant dbItem)
        {
            if (dbItem == null)
                return null;

            var item = new ProductVariant();
            item.ProductVariantId = dbItem.ProductVariantId;
            item.ProductId = dbItem.ProductId;
            item.Name = dbItem.Name;
            item.SKU = dbItem.SKU;
            item.Description = dbItem.Description;
            item.AdminComment = dbItem.AdminComment;
            item.ManufacturerPartNumber = dbItem.ManufacturerPartNumber;
            item.IsGiftCard = dbItem.IsGiftCard;
            item.IsDownload = dbItem.IsDownload;
            item.DownloadId = dbItem.DownloadId;
            item.UnlimitedDownloads = dbItem.UnlimitedDownloads;
            item.MaxNumberOfDownloads = dbItem.MaxNumberOfDownloads;
            item.DownloadExpirationDays = dbItem.DownloadExpirationDays;
            item.DownloadActivationType = dbItem.DownloadActivationType;
            item.HasSampleDownload = dbItem.HasSampleDownload;
            item.SampleDownloadId = dbItem.SampleDownloadId;
            item.HasUserAgreement = dbItem.HasUserAgreement;
            item.UserAgreementText = dbItem.UserAgreementText;
            item.IsRecurring = dbItem.IsRecurring;
            item.CycleLength = dbItem.CycleLength;
            item.CyclePeriod = dbItem.CyclePeriod;
            item.TotalCycles = dbItem.TotalCycles;
            item.IsShipEnabled = dbItem.IsShipEnabled;
            item.IsFreeShipping = dbItem.IsFreeShipping;
            item.AdditionalShippingCharge = dbItem.AdditionalShippingCharge;
            item.IsTaxExempt = dbItem.IsTaxExempt;
            item.TaxCategoryId = dbItem.TaxCategoryId;
            item.ManageInventory = dbItem.ManageInventory;
            item.StockQuantity = dbItem.StockQuantity;
            item.DisplayStockAvailability = dbItem.DisplayStockAvailability;
            item.MinStockQuantity = dbItem.MinStockQuantity;
            item.LowStockActivityId = dbItem.LowStockActivityId;
            item.NotifyAdminForQuantityBelow = dbItem.NotifyAdminForQuantityBelow;
            item.AllowOutOfStockOrders = dbItem.AllowOutOfStockOrders;
            item.OrderMinimumQuantity = dbItem.OrderMinimumQuantity;
            item.OrderMaximumQuantity = dbItem.OrderMaximumQuantity;
            item.WarehouseId = dbItem.WarehouseId;
            item.DisableBuyButton = dbItem.DisableBuyButton;
            item.Price = dbItem.Price;
            item.OldPrice = dbItem.OldPrice;
            item.ProductCost = dbItem.ProductCost;
            item.CustomerEntersPrice = dbItem.CustomerEntersPrice;
            item.MinimumCustomerEnteredPrice = dbItem.MinimumCustomerEnteredPrice;
            item.MaximumCustomerEnteredPrice = dbItem.MaximumCustomerEnteredPrice;
            item.Weight = dbItem.Weight;
            item.Length = dbItem.Length;
            item.Width = dbItem.Width;
            item.Height = dbItem.Height;
            item.PictureId = dbItem.PictureId;
            item.AvailableStartDateTime = dbItem.AvailableStartDateTime;
            item.AvailableEndDateTime = dbItem.AvailableEndDateTime;
            item.Published = dbItem.Published;
            item.Deleted = dbItem.Deleted;
            item.DisplayOrder = dbItem.DisplayOrder;
            item.CreatedOn = dbItem.CreatedOn;
            item.UpdatedOn = dbItem.UpdatedOn;
            return item;
        }

        private static RelatedProductCollection DBMapping(DBRelatedProductCollection dbCollection)
        {
            if (dbCollection == null)
                return null;

            var collection = new RelatedProductCollection();
            foreach (var dbItem in dbCollection)
            {
                var item = DBMapping(dbItem);
                collection.Add(item);
            }

            return collection;
        }

        private static RelatedProduct DBMapping(DBRelatedProduct dbItem)
        {
            if (dbItem == null)
                return null;

            var item = new RelatedProduct();
            item.RelatedProductId = dbItem.RelatedProductId;
            item.ProductId1 = dbItem.ProductId1;
            item.ProductId2 = dbItem.ProductId2;
            item.DisplayOrder = dbItem.DisplayOrder;

            return item;
        }

        private static PricelistCollection DBMapping(DBPricelistCollection dbCollection)
        {
            if (dbCollection == null)
                return null;

            var collection = new PricelistCollection();
            foreach (var dbItem in dbCollection)
            {
                var item = DBMapping(dbItem);
                collection.Add(item);
            }

            return collection;
        }

        private static Pricelist DBMapping(DBPricelist dbItem)
        {
            if (dbItem == null)
                return null;

            var item = new Pricelist();
            item.PricelistId = dbItem.PricelistId;
            item.ExportModeId = dbItem.ExportModeId;
            item.ExportTypeId = dbItem.ExportTypeId;
            item.AffiliateId = dbItem.AffiliateId;
            item.DisplayName = dbItem.DisplayName;
            item.ShortName = dbItem.ShortName;
            item.PricelistGuid = dbItem.PricelistGuid;
            item.CacheTime = dbItem.CacheTime;
            item.FormatLocalization = dbItem.FormatLocalization;
            item.Description = dbItem.Description;
            item.AdminNotes = dbItem.AdminNotes;
            item.Header = dbItem.Header;
            item.Body = dbItem.Body;
            item.Footer = dbItem.Footer;
            item.PriceAdjustment = dbItem.PriceAdjustment;
            item.PriceAdjustmentTypeId = dbItem.PriceAdjustmentTypeId;
            item.OverrideIndivAdjustment = dbItem.OverrideIndivAdjustment;
            item.CreatedOn = dbItem.CreatedOn;
            item.UpdatedOn = dbItem.UpdatedOn;

            return item;
        }

        private static ProductVariantPricelistCollection DBMapping(DBProductVariantPricelistCollection dbCollection)
        {
            if (dbCollection == null)
                return null;

            var collection = new ProductVariantPricelistCollection();
            foreach (var dbItem in dbCollection)
            {
                var item = DBMapping(dbItem);
                collection.Add(item);
            }

            return collection;
        }

        private static ProductVariantPricelist DBMapping(DBProductVariantPricelist dbItem)
        {
            if (dbItem == null)
                return null;

            var item = new ProductVariantPricelist();
            item.ProductVariantPricelistId = dbItem.ProductVariantPricelistId;
            item.ProductVariantId = dbItem.ProductVariantId;
            item.PricelistId = dbItem.PricelistId;
            item.PriceAdjustmentTypeId = dbItem.PriceAdjustmentTypeId;
            item.PriceAdjustment = dbItem.PriceAdjustment;
            item.UpdatedOn = dbItem.UpdatedOn;

            return item;
        }

        private static TierPriceCollection DBMapping(DBTierPriceCollection dbCollection)
        {
            if (dbCollection == null)
                return null;

            var collection = new TierPriceCollection();
            foreach (var dbItem in dbCollection)
            {
                var item = DBMapping(dbItem);
                collection.Add(item);
            }

            return collection;
        }

        private static TierPrice DBMapping(DBTierPrice dbItem)
        {
            if (dbItem == null)
                return null;

            var item = new TierPrice();
            item.TierPriceId = dbItem.TierPriceId;
            item.ProductVariantId = dbItem.ProductVariantId;
            item.Quantity = dbItem.Quantity;
            item.Price = dbItem.Price;

            return item;
        }

        private static ProductLocalized DBMapping(DBProductLocalized dbItem)
        {
            if (dbItem == null)
                return null;

            var item = new ProductLocalized();
            item.ProductLocalizedId = dbItem.ProductLocalizedId;
            item.ProductId = dbItem.ProductId;
            item.LanguageId = dbItem.LanguageId;
            item.Name = dbItem.Name;
            item.ShortDescription = dbItem.ShortDescription;
            item.FullDescription = dbItem.FullDescription;
            item.MetaKeywords = dbItem.MetaKeywords;
            item.MetaDescription = dbItem.MetaDescription;
            item.MetaTitle = dbItem.MetaTitle;
            item.SEName = dbItem.SEName;

            return item;
        }

        private static ProductVariantLocalized DBMapping(DBProductVariantLocalized dbItem)
        {
            if (dbItem == null)
                return null;

            var item = new ProductVariantLocalized();
            item.ProductVariantLocalizedId = dbItem.ProductVariantLocalizedId;
            item.ProductVariantId = dbItem.ProductVariantId;
            item.LanguageId = dbItem.LanguageId;
            item.Name = dbItem.Name;
            item.Description = dbItem.Description;

            return item;
        }

        private static CustomerRoleProductPriceCollection DBMapping(DBCustomerRoleProductPriceCollection dbCollection)
        {
            if (dbCollection == null)
                return null;

            var collection = new CustomerRoleProductPriceCollection();
            foreach (var dbItem in dbCollection)
            {
                var item = DBMapping(dbItem);
                collection.Add(item);
            }

            return collection;
        }

        private static CustomerRoleProductPrice DBMapping(DBCustomerRoleProductPrice dbItem)
        {
            if (dbItem == null)
                return null;

            var item = new CustomerRoleProductPrice();
            item.CustomerRoleProductPriceId = dbItem.CustomerRoleProductPriceId;
            item.CustomerRoleId = dbItem.CustomerRoleId;
            item.ProductVariantId = dbItem.ProductVariantId;
            item.Price = dbItem.Price;

            return item;
        }

        #endregion

        #region Methods

        #region Products
        /// <summary>
        /// Marks a product as deleted
        /// </summary>
        /// <param name="productId">Product identifier</param>
        public static void MarkProductAsDeleted(int productId)
        {
            var product = GetProductById(productId);
            if (product != null)
            {
                product = UpdateProduct(product.ProductId, product.Name, product.ShortDescription,
                    product.FullDescription, product.AdminComment, product.ProductTypeId,
                    product.TemplateId, product.ShowOnHomePage, product.MetaKeywords, product.MetaDescription,
                    product.MetaTitle, product.SEName, product.AllowCustomerReviews, product.AllowCustomerRatings, product.RatingSum,
                    product.TotalRatingVotes, product.Published, true, product.CreatedOn, product.UpdatedOn);

                foreach (var productVariant in product.ProductVariants)
                    MarkProductVariantAsDeleted(productVariant.ProductVariantId);
            }
        }

        /// <summary>
        /// Gets all products
        /// </summary>
        /// <returns>Product collection</returns>
        public static ProductCollection GetAllProducts()
        {
            bool showHidden = NopContext.Current.IsAdmin;
            return GetAllProducts(showHidden);
        }

        /// <summary>
        /// Gets all products
        /// </summary>
        /// <param name="showHidden">A value indicating whether to show hidden records</param>
        /// <returns>Product collection</returns>
        public static ProductCollection GetAllProducts(bool showHidden)
        {
            int languageId = 0;
            if (NopContext.Current != null)
                languageId = NopContext.Current.WorkingLanguage.LanguageId;

            return GetAllProducts(showHidden, languageId);
        }

        /// <summary>
        /// Gets all products
        /// </summary>
        /// <param name="showHidden">A value indicating whether to show hidden records</param>
        /// <param name="languageId">Language identifier</param>
        /// <returns>Product collection</returns>
        public static ProductCollection GetAllProducts(bool showHidden, int languageId)
        {
            var dbCollection = DBProviderManager<DBProductProvider>.Provider.GetAllProducts(showHidden, languageId);
            var products = DBMapping(dbCollection);
            return products;
        }

        /// <summary>
        /// Gets all products
        /// </summary>
        /// <param name="pageSize">Page size</param>
        /// <param name="pageIndex">Page index</param>
        /// <param name="totalRecords">Total records</param>
        /// <returns>Product collection</returns>
        public static ProductCollection GetAllProducts(int pageSize, int pageIndex, 
            out int totalRecords)
        {
            return GetAllProducts(0, 0, null, null, null,
                string.Empty, false, pageSize, pageIndex, null, out totalRecords);
        }

        /// <summary>
        /// Gets all products
        /// </summary>
        /// <param name="categoryId">Category identifier</param>
        /// <param name="manufacturerId">Manufacturer identifier</param>
        /// <param name="featuredProducts">A value indicating whether loaded products are marked as featured (relates only to categories and manufacturers). 0 to load featured products only, 1 to load not featured products only, null to load all products</param>
        /// <param name="pageSize">Page size</param>
        /// <param name="pageIndex">Page index</param>
        /// <param name="totalRecords">Total records</param>
        /// <returns>Product collection</returns>
        public static ProductCollection GetAllProducts(int categoryId, int manufacturerId,
            bool? featuredProducts, int pageSize, int pageIndex, out int totalRecords)
        {
            return GetAllProducts(categoryId, manufacturerId, featuredProducts, null, null,
                string.Empty, false, pageSize, pageIndex, null, out totalRecords);
        }

        /// <summary>
        /// Gets all products
        /// </summary>
        /// <param name="keywords">Keywords</param>
        /// <param name="searchDescriptions">A value indicating whether to search in descriptions</param>
        /// <param name="pageSize">Page size</param>
        /// <param name="pageIndex">Page index</param>
        /// <param name="totalRecords">Total records</param>
        /// <returns>Product collection</returns>
        public static ProductCollection GetAllProducts(string keywords, 
            bool searchDescriptions, int pageSize, int pageIndex, out int totalRecords)
        {
            return GetAllProducts(0, 0, null, null, null,
                keywords, searchDescriptions, pageSize, pageIndex, null, out totalRecords);
        }

        /// <summary>
        /// Gets all products
        /// </summary>
        /// <param name="categoryId">Category identifier</param>
        /// <param name="manufacturerId">Manufacturer identifier</param>
        /// <param name="featuredProducts">A value indicating whether loaded products are marked as featured (relates only to categories and manufacturers). 0 to load featured products only, 1 to load not featured products only, null to load all products</param>
        /// <param name="keywords">Keywords</param>
        /// <param name="searchDescriptions">A value indicating whether to search in descriptions</param>
        /// <param name="pageSize">Page size</param>
        /// <param name="pageIndex">Page index</param>
        /// <param name="filteredSpecs">Filtered product specification identifiers</param>
        /// <param name="totalRecords">Total records</param>
        /// <returns>Product collection</returns>
        public static ProductCollection GetAllProducts(int categoryId, int manufacturerId,
            bool? featuredProducts, string keywords, bool searchDescriptions,
            int pageSize, int pageIndex, List<int> filteredSpecs, out int totalRecords)
        {
            return GetAllProducts(categoryId, manufacturerId, featuredProducts, null, null,
                keywords, searchDescriptions, pageSize, pageIndex, filteredSpecs, out totalRecords);
        }

        /// <summary>
        /// Gets all products
        /// </summary>
        /// <param name="categoryId">Category identifier</param>
        /// <param name="manufacturerId">Manufacturer identifier</param>
        /// <param name="featuredProducts">A value indicating whether loaded products are marked as featured (relates only to categories and manufacturers). 0 to load featured products only, 1 to load not featured products only, null to load all products</param>
        /// <param name="priceMin">Minimum price</param>
        /// <param name="priceMax">Maximum price</param>
        /// <param name="pageSize">Page size</param>
        /// <param name="pageIndex">Page index</param>
        /// <param name="filteredSpecs">Filtered product specification identifiers</param>
        /// <param name="totalRecords">Total records</param>
        /// <returns>Product collection</returns>
        public static ProductCollection GetAllProducts(int categoryId, int manufacturerId,
            bool? featuredProducts, decimal? priceMin, decimal? priceMax,
            int pageSize, int pageIndex, List<int> filteredSpecs, out int totalRecords)
        {
            return GetAllProducts(categoryId, manufacturerId, featuredProducts, 
                priceMin, priceMax, string.Empty, false, pageSize, pageIndex, 
                filteredSpecs, out totalRecords);
        }

        /// <summary>
        /// Gets all products
        /// </summary>
        /// <param name="categoryId">Category identifier</param>
        /// <param name="manufacturerId">Manufacturer identifier</param>
        /// <param name="featuredProducts">A value indicating whether loaded products are marked as featured (relates only to categories and manufacturers). 0 to load featured products only, 1 to load not featured products only, null to load all products</param>
        /// <param name="priceMin">Minimum price</param>
        /// <param name="priceMax">Maximum price</param>
        /// <param name="keywords">Keywords</param>
        /// <param name="searchDescriptions">A value indicating whether to search in descriptions</param>
        /// <param name="pageSize">Page size</param>
        /// <param name="pageIndex">Page index</param>
        /// <param name="filteredSpecs">Filtered product specification identifiers</param>
        /// <param name="totalRecords">Total records</param>
        /// <returns>Product collection</returns>
        public static ProductCollection GetAllProducts(int categoryId, int manufacturerId,
            bool? featuredProducts, decimal? priceMin, decimal? priceMax, 
            string keywords, bool searchDescriptions, int pageSize, int pageIndex,
            List<int> filteredSpecs, out int totalRecords)
        {
            int languageId = 0;
            if (NopContext.Current != null)
                languageId = NopContext.Current.WorkingLanguage.LanguageId;

            return GetAllProducts(categoryId, manufacturerId,
                featuredProducts, priceMin, priceMax, keywords, searchDescriptions,
                pageSize, pageIndex, filteredSpecs, languageId, out totalRecords);
        }

        /// <summary>
        /// Gets all products
        /// </summary>
        /// <param name="categoryId">Category identifier</param>
        /// <param name="manufacturerId">Manufacturer identifier</param>
        /// <param name="featuredProducts">A value indicating whether loaded products are marked as featured (relates only to categories and manufacturers). 0 to load featured products only, 1 to load not featured products only, null to load all products</param>
        /// <param name="priceMin">Minimum price</param>
        /// <param name="priceMax">Maximum price</param>
        /// <param name="keywords">Keywords</param>
        /// <param name="searchDescriptions">A value indicating whether to search in descriptions</param>
        /// <param name="pageSize">Page size</param>
        /// <param name="pageIndex">Page index</param>
        /// <param name="filteredSpecs">Filtered product specification identifiers</param>
        /// <param name="languageId">Language identifier</param>
        /// <param name="totalRecords">Total records</param>
        /// <returns>Product collection</returns>
        public static ProductCollection GetAllProducts(int categoryId, int manufacturerId,
            bool? featuredProducts, decimal? priceMin, decimal? priceMax, 
            string keywords, bool searchDescriptions, int pageSize, 
            int pageIndex, List<int> filteredSpecs, int languageId, out int totalRecords)
        {
            if (pageSize <= 0)
                pageSize = 10;
            if (pageSize == int.MaxValue)
                pageSize = int.MaxValue - 1;

            if (pageIndex < 0)
                pageIndex = 0;
            if (pageIndex == int.MaxValue)
                pageIndex = int.MaxValue - 1;

            bool showHidden = NopContext.Current.IsAdmin;
            var dbCollection = DBProviderManager<DBProductProvider>.Provider.GetAllProducts(categoryId,
               manufacturerId, featuredProducts, priceMin, priceMax, keywords, searchDescriptions,
               pageSize, pageIndex, filteredSpecs, languageId, showHidden, out totalRecords);
            var products = DBMapping(dbCollection);
            return products;
        }

        /// <summary>
        /// Gets all products displayed on the home page
        /// </summary>
        /// <returns>Product collection</returns>
        public static ProductCollection GetAllProductsDisplayedOnHomePage()
        {
            bool showHidden = NopContext.Current.IsAdmin;

            int languageId = 0;
            if (NopContext.Current != null)
                languageId = NopContext.Current.WorkingLanguage.LanguageId;

            var dbCollection = DBProviderManager<DBProductProvider>.Provider.GetAllProductsDisplayedOnHomePage(showHidden, languageId);
            var products = DBMapping(dbCollection);
            return products;
        }

        /// <summary>
        /// Gets product
        /// </summary>
        /// <param name="productId">Product identifier</param>
        /// <returns>Product</returns>
        public static Product GetProductById(int productId)
        {
            int languageId = 0;
            if (NopContext.Current != null)
                languageId = NopContext.Current.WorkingLanguage.LanguageId;

            return GetProductById(productId, languageId);
        }

        /// <summary>
        /// Gets product
        /// </summary>
        /// <param name="productId">Product identifier</param>
        /// <param name="languageId">Language identifier</param>
        /// <returns>Product</returns>
        public static Product GetProductById(int productId, int languageId)
        {
            if (productId == 0)
                return null;

            string key = string.Format(PRODUCTS_BY_ID_KEY, productId, languageId);
            object obj2 = NopCache.Get(key);
            if (ProductManager.CacheEnabled && (obj2 != null))
            {
                return (Product)obj2;
            }

            var dbItem = DBProviderManager<DBProductProvider>.Provider.GetProductById(productId, languageId);
            var product = DBMapping(dbItem);

            if (ProductManager.CacheEnabled)
            {
                NopCache.Max(key, product);
            }
            return product;
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
        public static Product InsertProduct(string name, string shortDescription,
            string fullDescription, string adminComment, int productTypeId,
            int templateId, bool showOnHomePage,
            string metaKeywords, string metaDescription, string metaTitle,
            string seName, bool allowCustomerReviews, bool allowCustomerRatings,
            int ratingSum, int totalRatingVotes, bool published,
            bool deleted, DateTime createdOn, DateTime updatedOn)
        {
            createdOn = DateTimeHelper.ConvertToUtcTime(createdOn);
            updatedOn = DateTimeHelper.ConvertToUtcTime(updatedOn);

            var dbItem = DBProviderManager<DBProductProvider>.Provider.InsertProduct(name, shortDescription,
                fullDescription, adminComment, productTypeId, templateId, showOnHomePage,
                metaKeywords, metaDescription, metaTitle, seName, allowCustomerReviews,
                allowCustomerRatings, ratingSum, totalRatingVotes, published, deleted, 
                createdOn, updatedOn);

            var product = DBMapping(dbItem);

            if (ProductManager.CacheEnabled)
            {
                NopCache.RemoveByPattern(PRODUCTS_PATTERN_KEY);
                NopCache.RemoveByPattern(PRODUCTVARIANTS_PATTERN_KEY);
                NopCache.RemoveByPattern(TIERPRICES_PATTERN_KEY);
            }
            return product;
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
        public static Product UpdateProduct(int productId,
            string name, string shortDescription,
            string fullDescription, string adminComment, int productTypeId,
            int templateId, bool showOnHomePage,
            string metaKeywords, string metaDescription, string metaTitle,
            string seName, bool allowCustomerReviews, bool allowCustomerRatings,
            int ratingSum, int totalRatingVotes, bool published,
            bool deleted, DateTime createdOn, DateTime updatedOn)
        {
            createdOn = DateTimeHelper.ConvertToUtcTime(createdOn);
            updatedOn = DateTimeHelper.ConvertToUtcTime(updatedOn);

            var dbItem = DBProviderManager<DBProductProvider>.Provider.UpdateProduct(productId, 
                name, shortDescription, fullDescription, adminComment, 
                productTypeId, templateId, showOnHomePage, metaKeywords,
                metaDescription, metaTitle, seName, allowCustomerReviews, 
                allowCustomerRatings, ratingSum, totalRatingVotes,
                published, deleted, createdOn, updatedOn);

            var product = DBMapping(dbItem);

            if (ProductManager.CacheEnabled)
            {
                NopCache.RemoveByPattern(PRODUCTS_PATTERN_KEY);
                NopCache.RemoveByPattern(PRODUCTVARIANTS_PATTERN_KEY);
                NopCache.RemoveByPattern(TIERPRICES_PATTERN_KEY);
            }

            return product;
        }

        /// <summary>
        /// Gets localized product by id
        /// </summary>
        /// <param name="productLocalizedId">Localized product identifier</param>
        /// <returns>Product content</returns>
        public static ProductLocalized GetProductLocalizedById(int productLocalizedId)
        {
            var dbItem = DBProviderManager<DBProductProvider>.Provider.GetProductLocalizedById(productLocalizedId);
            var item = DBMapping(dbItem);
            return item;
        }

        /// <summary>
        /// Gets localized product by product id and language id
        /// </summary>
        /// <param name="productId">Product identifier</param>
        /// <param name="languageId">Language identifier</param>
        /// <returns>Product content</returns>
        public static ProductLocalized GetProductLocalizedByProductIdAndLanguageId(int productId, int languageId)
        {
            var dbItem = DBProviderManager<DBProductProvider>.Provider.GetProductLocalizedByProductIdAndLanguageId(productId, languageId);
            var item = DBMapping(dbItem);
            return item;
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
        public static ProductLocalized InsertProductLocalized(int productId,
            int languageId, string name, string shortDescription, string fullDescription,
            string metaKeywords, string metaDescription, string metaTitle,
            string seName)
        {
            var dbItem = DBProviderManager<DBProductProvider>.Provider.InsertProductLocalized(productId,
                languageId, name, shortDescription, fullDescription,
                metaKeywords, metaDescription, metaTitle, seName);
            var item = DBMapping(dbItem);

            if (ProductManager.CacheEnabled)
            {
                NopCache.RemoveByPattern(PRODUCTS_PATTERN_KEY);
                NopCache.RemoveByPattern(PRODUCTVARIANTS_PATTERN_KEY);
                NopCache.RemoveByPattern(TIERPRICES_PATTERN_KEY);
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
        public static ProductLocalized UpdateProductLocalized(int productLocalizedId,
            int productId, int languageId, string name, string shortDescription,
            string fullDescription, string metaKeywords, string metaDescription,
            string metaTitle, string seName)
        {
            var dbItem = DBProviderManager<DBProductProvider>.Provider.UpdateProductLocalized(productLocalizedId,
                productId, languageId, name, shortDescription, fullDescription,
                metaKeywords, metaDescription, metaTitle, seName);
            var item = DBMapping(dbItem);

            if (ProductManager.CacheEnabled)
            {
                NopCache.RemoveByPattern(PRODUCTS_PATTERN_KEY);
                NopCache.RemoveByPattern(PRODUCTVARIANTS_PATTERN_KEY);
                NopCache.RemoveByPattern(TIERPRICES_PATTERN_KEY);
            }

            return item;
        }

        /// <summary>
        /// Gets a list of products purchased by other customers who purchased the above
        /// </summary>
        /// <param name="productId">Product identifier</param>
        /// <returns>Product collection</returns>
        public static ProductCollection GetProductsAlsoPurchasedById(int productId)
        {
            int totalRecords = 0;
            var products = GetProductsAlsoPurchasedById(productId, 
                ProductManager.ProductsAlsoPurchasedNumber, 0, out totalRecords);
            return products;
        }

        /// <summary>
        /// Gets a list of products purchased by other customers who purchased the above
        /// </summary>
        /// <param name="productId">Product identifier</param>
        /// <param name="pageSize">Page size</param>
        /// <param name="pageIndex">Page index</param>
        /// <param name="totalRecords">Total records</param>
        /// <returns>Product collection</returns>
        public static ProductCollection GetProductsAlsoPurchasedById(int productId,
            int pageSize, int pageIndex, out int totalRecords)
        {
            int languageId = 0;
            if (NopContext.Current != null)
                languageId = NopContext.Current.WorkingLanguage.LanguageId;

            return GetProductsAlsoPurchasedById(productId, languageId,
                pageSize, pageIndex, out totalRecords);
        }

        /// <summary>
        /// Gets a list of products purchased by other customers who purchased the above
        /// </summary>
        /// <param name="productId">Product identifier</param>
        /// <param name="languageId">Language identifier</param>
        /// <param name="pageSize">Page size</param>
        /// <param name="pageIndex">Page index</param>
        /// <param name="totalRecords">Total records</param>
        /// <returns>Product collection</returns>
        public static ProductCollection GetProductsAlsoPurchasedById(int productId,
            int languageId, int pageSize, int pageIndex, out int totalRecords)
        {
            if (pageSize <= 0)
                pageSize = 10;
            if (pageSize == int.MaxValue)
                pageSize = int.MaxValue - 1;

            if (pageIndex < 0)
                pageIndex = 0;
            if (pageIndex == int.MaxValue)
                pageIndex = int.MaxValue - 1;

            bool showHidden = NopContext.Current.IsAdmin;

            var dbCollection = DBProviderManager<DBProductProvider>.Provider.GetProductsAlsoPurchasedById(productId,
               languageId, showHidden, pageSize, pageIndex, out totalRecords);
            var products = DBMapping(dbCollection);
            return products;
        }

        /// <summary>
        /// Sets a product rating
        /// </summary>
        /// <param name="productId">Product identifer</param>
        /// <param name="rating">Rating</param>
        public static void SetProductRating(int productId, int rating)
        {
            if (NopContext.Current.User == null)
            {
                return;
            }
            if (NopContext.Current.User.IsGuest && !CustomerManager.AllowAnonymousUsersToSetProductRatings)
            {
                return;
            }

            if (rating < 1 || rating > 5)
                rating = 1;
            var ratedOn = DateTimeHelper.ConvertToUtcTime(DateTime.Now);
            DBProviderManager<DBProductProvider>.Provider.SetProductRating(productId, NopContext.Current.User.CustomerId,
                rating, ratedOn);

            if (ProductManager.CacheEnabled)
            {
                NopCache.RemoveByPattern(PRODUCTS_PATTERN_KEY);
            }
        }

        /// <summary>
        /// Clears a "compare products" list
        /// </summary>
        public static void ClearCompareProducts()
        {
            HttpCookie compareCookie = HttpContext.Current.Request.Cookies.Get("NopCommerce.CompareProducts");
            if (compareCookie != null)
            {
                compareCookie.Values.Clear();
                compareCookie.Expires = DateTime.Now.AddYears(-1);
                HttpContext.Current.Response.Cookies.Set(compareCookie);
            }
        }

        /// <summary>
        /// Gets a "compare products" list
        /// </summary>
        /// <returns>"Compare products" list</returns>
        public static ProductCollection GetCompareProducts()
        {
            var products = new ProductCollection();
            var productIds = GetCompareProductsIds();
            foreach (int productId in productIds)
            {
                var product = GetProductById(productId);
                if (product != null && product.Published && !product.Deleted)
                    products.Add(product);
            }
            return products;
        }

        /// <summary>
        /// Gets a "compare products" identifier list
        /// </summary>
        /// <returns>"compare products" identifier list</returns>
        public static List<int> GetCompareProductsIds()
        {
            var productIds = new List<int>();
            HttpCookie compareCookie = HttpContext.Current.Request.Cookies.Get("NopCommerce.CompareProducts");
            if ((compareCookie == null) || (compareCookie.Values == null))
                return productIds;
            string[] values = compareCookie.Values.GetValues("CompareProductIds");
            if (values == null)
                return productIds;
            foreach (string productId in values)
            {
                int prodId = int.Parse(productId);
                if (!productIds.Contains(prodId))
                    productIds.Add(prodId);
            }

            return productIds;
        }

        /// <summary>
        /// Removes a product from a "compare products" list
        /// </summary>
        /// <param name="productId">Product identifer</param>
        public static void RemoveProductFromCompareList(int productId)
        {
            var oldProductIds = GetCompareProductsIds();
            var newProductIds = new List<int>();
            newProductIds.AddRange(oldProductIds);
            newProductIds.Remove(productId);

            HttpCookie compareCookie = HttpContext.Current.Request.Cookies.Get("NopCommerce.CompareProducts");
            if ((compareCookie == null) || (compareCookie.Values == null))
                return;
            compareCookie.Values.Clear();
            foreach (int newProductId in newProductIds)
                compareCookie.Values.Add("CompareProductIds", newProductId.ToString());
            compareCookie.Expires = DateTime.Now.AddDays(10.0);
            HttpContext.Current.Response.Cookies.Set(compareCookie);
        }

        /// <summary>
        /// Adds a product to a "compare products" list
        /// </summary>
        /// <param name="productId">Product identifer</param>
        public static void AddProductToCompareList(int productId)
        {
            if (!ProductManager.CompareProductsEnabled)
                return;

            var oldProductIds = GetCompareProductsIds();
            var newProductIds = new List<int>();
            newProductIds.Add(productId);
            foreach (int oldProductId in oldProductIds)
                if (oldProductId != productId)
                    newProductIds.Add(oldProductId);

            HttpCookie compareCookie = HttpContext.Current.Request.Cookies.Get("NopCommerce.CompareProducts");
            if (compareCookie == null)
                compareCookie = new HttpCookie("NopCommerce.CompareProducts");
            compareCookie.Values.Clear();
            int maxProducts = 4;
            int i = 1;
            foreach (int newProductId in newProductIds)
            {
                compareCookie.Values.Add("CompareProductIds", newProductId.ToString());
                if (i == maxProducts)
                    break;
                i++;
            }
            compareCookie.Expires = DateTime.Now.AddDays(10.0);
            HttpContext.Current.Response.Cookies.Set(compareCookie);
        }

        /// <summary>
        /// Gets a "recently viewed products" list
        /// </summary>
        /// <param name="number">Number of products to load</param>
        /// <returns>"recently viewed products" list</returns>
        public static ProductCollection GetRecentlyViewedProducts(int number)
        {
            var products = new ProductCollection();
            var productIds = GetRecentlyViewedProductsIds(number);
            foreach (int productId in productIds)
            {
                Product product = GetProductById(productId);
                if (product != null && product.Published && !product.Deleted)
                    products.Add(product);
            }
            return products;
        }

        /// <summary>
        /// Gets a "recently viewed products" identifier list
        /// </summary>
        /// <returns>"recently viewed products" list</returns>
        public static List<int> GetRecentlyViewedProductsIds()
        {
            return GetRecentlyViewedProductsIds(int.MaxValue);
        }

        /// <summary>
        /// Gets a "recently viewed products" identifier list
        /// </summary>
        /// <param name="number">Number of products to load</param>
        /// <returns>"recently viewed products" list</returns>
        public static List<int> GetRecentlyViewedProductsIds(int number)
        {
            var productIds = new List<int>();
            HttpCookie recentlyViewedCookie = HttpContext.Current.Request.Cookies.Get("NopCommerce.RecentlyViewedProducts");
            if ((recentlyViewedCookie == null) || (recentlyViewedCookie.Values == null))
                return productIds;
            string[] values = recentlyViewedCookie.Values.GetValues("RecentlyViewedProductIds");
            if (values == null)
                return productIds;
            foreach (string productId in values)
            {
                int prodId = int.Parse(productId);
                if (!productIds.Contains(prodId))
                {
                    productIds.Add(prodId);
                    if (productIds.Count >= number)
                        break;
                }

            }

            return productIds;
        }

        /// <summary>
        /// Adds a product to a recently viewed products list
        /// </summary>
        /// <param name="productId">Product identifier</param>
        public static void AddProductToRecentlyViewedList(int productId)
        {
            if (!ProductManager.RecentlyViewedProductsEnabled)
                return;

            var oldProductIds = GetRecentlyViewedProductsIds();
            var newProductIds = new List<int>();
            newProductIds.Add(productId);
            foreach (int oldProductId in oldProductIds)
                if (oldProductId != productId)
                    newProductIds.Add(oldProductId);

            HttpCookie recentlyViewedCookie = HttpContext.Current.Request.Cookies.Get("NopCommerce.RecentlyViewedProducts");
            if (recentlyViewedCookie == null)
                recentlyViewedCookie = new HttpCookie("NopCommerce.RecentlyViewedProducts");
            recentlyViewedCookie.Values.Clear();
            int maxProducts = SettingManager.GetSettingValueInteger("Display.RecentlyViewedProductCount");
            if (maxProducts <= 0)
                maxProducts = 10;
            int i = 1;
            foreach (int newProductId in newProductIds)
            {
                recentlyViewedCookie.Values.Add("RecentlyViewedProductIds", newProductId.ToString());
                if (i == maxProducts)
                    break;
                i++;
            }
            recentlyViewedCookie.Expires = DateTime.Now.AddDays(10.0);
            HttpContext.Current.Response.Cookies.Set(recentlyViewedCookie);
        }

        /// <summary>
        /// Gets a recently added products list
        /// </summary>
        /// <param name="number">Number of products to load</param>
        /// <returns>"recently added" product list</returns>
        public static ProductCollection GetRecentlyAddedProducts(int number)
        {
            int languageId = 0;
            if (NopContext.Current != null)
                languageId = NopContext.Current.WorkingLanguage.LanguageId;

            bool showHidden = NopContext.Current.IsAdmin;

            var dbCollection = DBProviderManager<DBProductProvider>.Provider.GetRecentlyAddedProducts(number,
                languageId, showHidden);
            var products = DBMapping(dbCollection);
            return products;
        }

        /// <summary>
        /// Direct add to cart allowed
        /// </summary>
        /// <param name="productId">Product identifier</param>
        /// <param name="productVariantId">Default product variant identifier for adding to cart</param>
        /// <returns>A value indicating whether direct add to cart is allowed</returns>
        public static bool DirectAddToCartAllowed(int productId, out int productVariantId)
        {
            bool result = false;
            productVariantId = 0;
            var product = GetProductById(productId);
            if (product != null)
            {
                var productVariants = product.ProductVariants;
                if (productVariants.Count == 1)
                {
                    var defaultProductVariant = productVariants[0];
                    if (!defaultProductVariant.CustomerEntersPrice)
                    {
                        var addToCartWarnings = ShoppingCartManager.GetShoppingCartItemWarnings(ShoppingCartTypeEnum.ShoppingCart,
                            defaultProductVariant.ProductVariantId, string.Empty, decimal.Zero, 1);

                        if (addToCartWarnings.Count == 0)
                        {
                            productVariantId = defaultProductVariant.ProductVariantId;
                            result = true;
                        }
                    }
                }
            }
            return result;
        }

        /// <summary>
        /// Creates a copy of product with all depended data
        /// </summary>
        /// <param name="productId">The product identifier</param>
        /// <param name="name">The name of product duplicate</param>
        /// <param name="isPublished">A value indicating whether the product duplicate should be published</param>
        /// <param name="copyImages">A value indicating whether the product images should be copied</param>
        /// <returns>Product entity</returns>
        public static Product DuplicateProduct(int productId, string name,
            bool isPublished, bool copyImages)
        {
            var product = GetProductById(productId, 0);
            if (product == null)
                return null;

            Product productCopy = null;
            //uncomment this line to support transactions
            //using (var scope = new System.Transactions.TransactionScope())
            {
                // product
                productCopy = InsertProduct(name, product.ShortDescription,
                    product.FullDescription, product.AdminComment, product.ProductTypeId,
                    product.TemplateId, product.ShowOnHomePage, product.MetaKeywords,
                    product.MetaDescription, product.MetaTitle, product.SEName,
                    product.AllowCustomerReviews, product.AllowCustomerRatings, 0, 0,
                    isPublished, product.Deleted, product.CreatedOn, product.UpdatedOn);

                if (productCopy == null)
                    return null;

                var languages = LanguageManager.GetAllLanguages(true);

                //localization
                foreach (var lang in languages)
                {
                    var productLocalized = GetProductLocalizedByProductIdAndLanguageId(product.ProductId, lang.LanguageId);
                    if (productLocalized != null)
                    {
                        var productLocalizedCopy = InsertProductLocalized(productCopy.ProductId,
                            productLocalized.LanguageId,
                            productLocalized.Name,
                            productLocalized.ShortDescription,
                            productLocalized.FullDescription,
                            productLocalized.MetaKeywords,
                            productLocalized.MetaDescription,
                            productLocalized.MetaTitle,
                            productLocalized.SEName);
                    }
                }

                // product pictures
                if (copyImages)
                {
                    foreach (var productPicture in product.ProductPictures)
                    {
                        var picture = productPicture.Picture;
                        var pictureCopy = PictureManager.InsertPicture(picture.PictureBinary,
                            picture.Extension,
                            picture.IsNew);
                        InsertProductPicture(productCopy.ProductId,
                            pictureCopy.PictureId,
                            productPicture.DisplayOrder);
                    }
                }

                // product <-> categories mappings
                foreach (var productCategory in product.ProductCategories)
                {
                    CategoryManager.InsertProductCategory(productCopy.ProductId,
                        productCategory.CategoryId,
                        productCategory.IsFeaturedProduct,
                        productCategory.DisplayOrder);
                }

                // product <-> manufacturers mappings
                foreach (var productManufacturers in product.ProductManufacturers)
                {
                    ManufacturerManager.InsertProductManufacturer(productCopy.ProductId,
                        productManufacturers.ManufacturerId,
                        productManufacturers.IsFeaturedProduct,
                        productManufacturers.DisplayOrder);
                }

                // product <-> releated products mappings
                foreach (var relatedProduct in product.RelatedProducts)
                {
                    InsertRelatedProduct(productCopy.ProductId,
                        relatedProduct.ProductId2,
                        relatedProduct.DisplayOrder);
                }

                // product specifications
                foreach (var productSpecificationAttribute in SpecificationAttributeManager.GetProductSpecificationAttributesByProductId(product.ProductId))
                {
                    SpecificationAttributeManager.InsertProductSpecificationAttribute(productCopy.ProductId,
                        productSpecificationAttribute.SpecificationAttributeOptionId,
                        productSpecificationAttribute.AllowFiltering,
                        productSpecificationAttribute.ShowOnProductPage,
                        productSpecificationAttribute.DisplayOrder);
                }

                // product variants
                var productVariants = GetProductVariantsByProductId(product.ProductId, 0, true);
                foreach (var productVariant in productVariants)
                {
                    // product variant picture
                    int pictureId = 0;
                    if (copyImages)
                    {
                        var picture = productVariant.Picture;
                        if (picture != null)
                        {
                            var pictureCopy = PictureManager.InsertPicture(picture.PictureBinary, picture.Extension, picture.IsNew);
                            pictureId = pictureCopy.PictureId;
                        }
                    }

                    // product variant download & sample download
                    int downloadId = productVariant.DownloadId;
                    int sampleDownloadId = productVariant.SampleDownloadId;
                    if (productVariant.IsDownload)
                    {
                        var download = productVariant.Download;
                        if (download != null)
                        {
                            var downloadCopy = DownloadManager.InsertDownload(download.UseDownloadUrl, download.DownloadUrl, download.DownloadBinary, download.ContentType, download.Filename, download.Extension, download.IsNew);
                            downloadId = downloadCopy.DownloadId;
                        }

                        if (productVariant.HasSampleDownload)
                        {
                            var sampleDownload = productVariant.SampleDownload;
                            if (sampleDownload != null)
                            {
                                var sampleDownloadCopy = DownloadManager.InsertDownload(sampleDownload.UseDownloadUrl, sampleDownload.DownloadUrl, sampleDownload.DownloadBinary, sampleDownload.ContentType, sampleDownload.Filename, sampleDownload.Extension, sampleDownload.IsNew);
                                sampleDownloadId = sampleDownloadCopy.DownloadId;
                            }
                        }
                    }

                    // product variant
                    var productVariantCopy = InsertProductVariant(productCopy.ProductId, productVariant.Name,
                        productVariant.SKU, productVariant.Description, productVariant.AdminComment, productVariant.ManufacturerPartNumber,
                        productVariant.IsGiftCard, productVariant.IsDownload, downloadId,
                        productVariant.UnlimitedDownloads, productVariant.MaxNumberOfDownloads,
                        productVariant.DownloadExpirationDays, (DownloadActivationTypeEnum)productVariant.DownloadActivationType,
                        productVariant.HasSampleDownload, sampleDownloadId,
                        productVariant.HasUserAgreement, productVariant.UserAgreementText,
                        productVariant.IsRecurring, productVariant.CycleLength,
                        productVariant.CyclePeriod, productVariant.TotalCycles,
                        productVariant.IsShipEnabled, productVariant.IsFreeShipping, productVariant.AdditionalShippingCharge,
                        productVariant.IsTaxExempt, productVariant.TaxCategoryId,
                        productVariant.ManageInventory, productVariant.StockQuantity,
                        productVariant.DisplayStockAvailability, productVariant.MinStockQuantity, productVariant.LowStockActivity,
                        productVariant.NotifyAdminForQuantityBelow, productVariant.AllowOutOfStockOrders,
                        productVariant.OrderMinimumQuantity, productVariant.OrderMaximumQuantity,
                        productVariant.WarehouseId, productVariant.DisableBuyButton,
                        productVariant.Price, productVariant.OldPrice, 
                        productVariant.ProductCost, productVariant.CustomerEntersPrice,
                        productVariant.MinimumCustomerEnteredPrice, productVariant.MaximumCustomerEnteredPrice,
                        productVariant.Weight, productVariant.Length, productVariant.Width, productVariant.Height, pictureId,
                        productVariant.AvailableStartDateTime, productVariant.AvailableEndDateTime,
                        productVariant.Published, productVariant.Deleted, productVariant.DisplayOrder, productVariant.CreatedOn, productVariant.UpdatedOn);

                    //localization
                    foreach (var lang in languages)
                    {
                        var productVariantLocalized = GetProductVariantLocalizedByProductVariantIdAndLanguageId(productVariant.ProductVariantId, lang.LanguageId);
                        if (productVariantLocalized != null)
                        {
                            var productVariantLocalizedCopy = InsertProductVariantLocalized(productVariantCopy.ProductVariantId,
                                productVariantLocalized.LanguageId,
                                productVariantLocalized.Name,
                                productVariantLocalized.Description);
                        }
                    }

                    // product variant <-> attributes mappings
                    foreach (var productVariantAttribute in productVariant.ProductVariantAttributes)
                    {
                        var productVariantAttributeCopy = ProductAttributeManager.InsertProductVariantAttribute(productVariantCopy.ProductVariantId, productVariantAttribute.ProductAttributeId, productVariantAttribute.TextPrompt, productVariantAttribute.IsRequired, productVariantAttribute.AttributeControlType, productVariantAttribute.DisplayOrder);

                        // product variant attribute values
                        var productVariantAttributeValues = ProductAttributeManager.GetProductVariantAttributeValues(productVariantAttribute.ProductVariantAttributeId, 0);
                        foreach (var productVariantAttributeValue in productVariantAttributeValues)
                        {
                            var pvavCopy = ProductAttributeManager.InsertProductVariantAttributeValue(productVariantAttributeCopy.ProductVariantAttributeId, productVariantAttributeValue.Name, productVariantAttributeValue.PriceAdjustment, productVariantAttributeValue.WeightAdjustment, productVariantAttributeValue.IsPreSelected, productVariantAttributeValue.DisplayOrder);

                            //localization
                            foreach (var lang in languages)
                            {
                                var pvavLocalized = ProductAttributeManager.GetProductVariantAttributeValueLocalizedByProductVariantAttributeValueIdAndLanguageId(productVariantAttributeValue.ProductVariantAttributeValueId, lang.LanguageId);
                                if (pvavLocalized != null)
                                {
                                    var pvavLocalizedCopy = ProductAttributeManager.InsertProductVariantAttributeValueLocalized(pvavCopy.ProductVariantAttributeValueId,
                                        pvavLocalized.LanguageId,
                                        pvavLocalized.Name);
                                }
                            }
                        }
                    }
                    foreach (var combination in ProductAttributeManager.GetAllProductVariantAttributeCombinations(productVariant.ProductVariantId))
                    {
                        ProductAttributeManager.InsertProductVariantAttributeCombination(productVariant.ProductVariantId,
                              combination.AttributesXml,
                              combination.StockQuantity,
                              combination.AllowOutOfStockOrders);
                    }

                    // product variant <-> discounts mapping
                    foreach (var discount in productVariant.AllDiscounts)
                    {
                        DiscountManager.AddDiscountToProductVariant(productVariantCopy.ProductVariantId, discount.DiscountId);
                    }

                    // product variant tier prices
                    foreach (var tierPrice in productVariant.TierPrices)
                    {
                        InsertTierPrice(productVariantCopy.ProductVariantId, tierPrice.Quantity, tierPrice.Price);
                    }
                }

                //uncomment this line to support transactions
                //scope.Complete();
            }

            return productCopy;
        }

        #endregion

        #region Product Variants

        /// <summary>
        /// Remove a product variant picture
        /// </summary>
        /// <param name="productVariantId">Product variant identifier</param>
        public static void RemoveProductVariantPicture(int productVariantId)
        {
            var productVariant = GetProductVariantById(productVariantId);
            if (productVariant != null)
            {
                UpdateProductVariant(productVariant.ProductVariantId, productVariant.ProductId, productVariant.Name,
                    productVariant.SKU, productVariant.Description, productVariant.AdminComment, productVariant.ManufacturerPartNumber,
                    productVariant.IsGiftCard, productVariant.IsDownload, productVariant.DownloadId,
                    productVariant.UnlimitedDownloads, productVariant.MaxNumberOfDownloads,
                    productVariant.DownloadExpirationDays, (DownloadActivationTypeEnum)productVariant.DownloadActivationType,
                    productVariant.HasSampleDownload, productVariant.SampleDownloadId,
                    productVariant.HasUserAgreement, productVariant.UserAgreementText,
                    productVariant.IsRecurring, productVariant.CycleLength,
                    productVariant.CyclePeriod, productVariant.TotalCycles,
                    productVariant.IsShipEnabled, productVariant.IsFreeShipping, productVariant.AdditionalShippingCharge,
                    productVariant.IsTaxExempt, productVariant.TaxCategoryId, productVariant.ManageInventory,
                    productVariant.StockQuantity, productVariant.DisplayStockAvailability, productVariant.MinStockQuantity,
                    productVariant.LowStockActivity, productVariant.NotifyAdminForQuantityBelow,
                    productVariant.AllowOutOfStockOrders, productVariant.OrderMinimumQuantity,
                    productVariant.OrderMaximumQuantity, productVariant.WarehouseId,
                    productVariant.DisableBuyButton, productVariant.Price,
                    productVariant.OldPrice, productVariant.ProductCost,
                    productVariant.CustomerEntersPrice,
                    productVariant.MinimumCustomerEnteredPrice, 
                    productVariant.MaximumCustomerEnteredPrice,
                    productVariant.Weight, productVariant.Length, productVariant.Width, productVariant.Height, 0,
                    productVariant.AvailableStartDateTime, productVariant.AvailableEndDateTime,
                    productVariant.Published, productVariant.Deleted,
                    productVariant.DisplayOrder, productVariant.CreatedOn, productVariant.UpdatedOn);
            }
        }

        /// <summary>
        /// Get low stock product variants
        /// </summary>
        /// <returns>Result</returns>
        public static ProductVariantCollection GetLowStockProductVariants()
        {
            var dbCollection = DBProviderManager<DBProductProvider>.Provider.GetLowStockProductVariants();
            var productVariants = DBMapping(dbCollection);
            return productVariants;
        }

        /// <summary>
        /// Remove a product variant download
        /// </summary>
        /// <param name="productVariantId">Product variant identifier</param>
        public static void RemoveProductVariantDownload(int productVariantId)
        {
            var productVariant = GetProductVariantById(productVariantId);
            if (productVariant != null)
            {
                UpdateProductVariant(productVariant.ProductVariantId, productVariant.ProductId, productVariant.Name,
                    productVariant.SKU, productVariant.Description, productVariant.AdminComment,
                    productVariant.ManufacturerPartNumber, productVariant.IsGiftCard,
                    productVariant.IsDownload, 0,
                    productVariant.UnlimitedDownloads, productVariant.MaxNumberOfDownloads,
                    productVariant.DownloadExpirationDays, (DownloadActivationTypeEnum)productVariant.DownloadActivationType,
                    productVariant.HasSampleDownload, productVariant.SampleDownloadId,
                    productVariant.HasUserAgreement, productVariant.UserAgreementText,
                    productVariant.IsRecurring, productVariant.CycleLength,
                    productVariant.CyclePeriod, productVariant.TotalCycles,
                    productVariant.IsShipEnabled, productVariant.IsFreeShipping, productVariant.AdditionalShippingCharge,
                    productVariant.IsTaxExempt, productVariant.TaxCategoryId, productVariant.ManageInventory,
                    productVariant.StockQuantity, productVariant.DisplayStockAvailability, productVariant.MinStockQuantity,
                    productVariant.LowStockActivity, productVariant.NotifyAdminForQuantityBelow,
                    productVariant.AllowOutOfStockOrders, productVariant.OrderMinimumQuantity,
                    productVariant.OrderMaximumQuantity, productVariant.WarehouseId,
                    productVariant.DisableBuyButton, productVariant.Price,
                    productVariant.OldPrice, productVariant.ProductCost,
                    productVariant.CustomerEntersPrice,
                    productVariant.MinimumCustomerEnteredPrice, 
                    productVariant.MaximumCustomerEnteredPrice,
                    productVariant.Weight, productVariant.Length, productVariant.Width, productVariant.Height,
                    productVariant.PictureId, productVariant.AvailableStartDateTime, productVariant.AvailableEndDateTime,
                    productVariant.Published, productVariant.Deleted,
                    productVariant.DisplayOrder, productVariant.CreatedOn, productVariant.UpdatedOn);
            }
        }

        /// <summary>
        /// Remove a product variant sample download
        /// </summary>
        /// <param name="productVariantId">Product variant identifier</param>
        public static void RemoveProductVariantSampleDownload(int productVariantId)
        {
            var productVariant = GetProductVariantById(productVariantId);
            if (productVariant != null)
            {
                UpdateProductVariant(productVariant.ProductVariantId, productVariant.ProductId, productVariant.Name,
                    productVariant.SKU, productVariant.Description, productVariant.AdminComment,
                    productVariant.ManufacturerPartNumber, productVariant.IsGiftCard,
                    productVariant.IsDownload, productVariant.DownloadId,
                    productVariant.UnlimitedDownloads, productVariant.MaxNumberOfDownloads,
                    productVariant.DownloadExpirationDays, (DownloadActivationTypeEnum)productVariant.DownloadActivationType,
                    productVariant.HasSampleDownload, 0,
                    productVariant.HasUserAgreement, productVariant.UserAgreementText,
                    productVariant.IsRecurring, productVariant.CycleLength,
                    productVariant.CyclePeriod, productVariant.TotalCycles,
                    productVariant.IsShipEnabled, productVariant.IsFreeShipping,
                    productVariant.AdditionalShippingCharge, productVariant.IsTaxExempt,
                    productVariant.TaxCategoryId, productVariant.ManageInventory,
                    productVariant.StockQuantity, productVariant.DisplayStockAvailability, productVariant.MinStockQuantity,
                    productVariant.LowStockActivity, productVariant.NotifyAdminForQuantityBelow,
                    productVariant.AllowOutOfStockOrders, productVariant.OrderMinimumQuantity,
                    productVariant.OrderMaximumQuantity, productVariant.WarehouseId,
                    productVariant.DisableBuyButton, productVariant.Price, productVariant.OldPrice,
                    productVariant.ProductCost, productVariant.CustomerEntersPrice,
                    productVariant.MinimumCustomerEnteredPrice, productVariant.MaximumCustomerEnteredPrice,
                    productVariant.Weight, productVariant.Length, productVariant.Width, productVariant.Height,
                    productVariant.PictureId, productVariant.AvailableStartDateTime, productVariant.AvailableEndDateTime,
                    productVariant.Published, productVariant.Deleted,
                    productVariant.DisplayOrder, productVariant.CreatedOn, productVariant.UpdatedOn);
            }
        }

        /// <summary>
        /// Gets a product variant
        /// </summary>
        /// <param name="productVariantId">Product variant identifier</param>
        /// <returns>Product variant</returns>
        public static ProductVariant GetProductVariantById(int productVariantId)
        {
            int languageId = 0;
            if (NopContext.Current != null)
                languageId = NopContext.Current.WorkingLanguage.LanguageId;

            return GetProductVariantById(productVariantId, languageId);
        }

        /// <summary>
        /// Gets a product variant
        /// </summary>
        /// <param name="productVariantId">Product variant identifier</param>
        /// <param name="languageId">Language identifier</param>
        /// <returns>Product variant</returns>
        public static ProductVariant GetProductVariantById(int productVariantId, int languageId)
        {
            if (productVariantId == 0)
                return null;

            string key = string.Format(PRODUCTVARIANTS_BY_ID_KEY, productVariantId, languageId);
            object obj2 = NopCache.Get(key);
            if (ProductManager.CacheEnabled && (obj2 != null))
            {
                return (ProductVariant)obj2;
            }

            var dbItem = DBProviderManager<DBProductProvider>.Provider.GetProductVariantById(productVariantId, languageId);
            var productVariant = DBMapping(dbItem);

            if (ProductManager.CacheEnabled)
            {
                NopCache.Max(key, productVariant);
            }
            return productVariant;
        }

        /// <summary>
        /// Gets a product variant by SKU
        /// </summary>
        /// <param name="sku">SKU</param>
        /// <returns>Product variant</returns>
        public static ProductVariant GetProductVariantBySKU(string sku)
        {
            if (String.IsNullOrEmpty(sku))
                return null;

            sku = sku.Trim();

            var dbItem = DBProviderManager<DBProductProvider>.Provider.GetProductVariantBySKU(sku);
            var productVariant = DBMapping(dbItem);
            return productVariant;
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
        /// <param name="lowStockActivity">The low stock activity</param>
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
        public static ProductVariant InsertProductVariant(int productId,
            string name, string sku,
            string description, string adminComment, string manufacturerPartNumber,
            bool isGiftCard, bool isDownload, int downloadId, bool unlimitedDownloads,
            int maxNumberOfDownloads, int? downloadExpirationDays,
            DownloadActivationTypeEnum downloadActivationType, bool hasSampleDownload,
            int sampleDownloadId, bool hasUserAgreement,
            string userAgreementText, bool isRecurring,
            int cycleLength, int cyclePeriod, int totalCycles,
            bool isShipEnabled, bool isFreeShipping,
            decimal additionalShippingCharge, bool isTaxExempt, int taxCategoryId,
            int manageInventory, int stockQuantity, bool displayStockAvailability,
            int minStockQuantity, LowStockActivityEnum lowStockActivity,
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
                availableStartDateTime = DateTimeHelper.ConvertToUtcTime(availableStartDateTime.Value);
            if (availableEndDateTime.HasValue)
                availableEndDateTime = DateTimeHelper.ConvertToUtcTime(availableEndDateTime.Value);

            sku = sku.Trim();

            createdOn = DateTimeHelper.ConvertToUtcTime(createdOn);
            updatedOn = DateTimeHelper.ConvertToUtcTime(updatedOn);

            var dbItem = DBProviderManager<DBProductProvider>.Provider.InsertProductVariant(productId,
                name, sku, description, adminComment, manufacturerPartNumber, isGiftCard, isDownload,
                downloadId, unlimitedDownloads, maxNumberOfDownloads,
                downloadExpirationDays, (int)downloadActivationType,
                hasSampleDownload, sampleDownloadId, hasUserAgreement, userAgreementText, isRecurring, cycleLength,
                cyclePeriod, totalCycles, isShipEnabled, isFreeShipping,
                additionalShippingCharge, isTaxExempt, taxCategoryId, manageInventory,
                stockQuantity, displayStockAvailability, minStockQuantity, (int)lowStockActivity,
                notifyAdminForQuantityBelow, allowOutOfStockOrders, orderMinimumQuantity,
                orderMaximumQuantity, warehouseId, disableBuyButton,
                price, oldPrice, productCost, customerEntersPrice,
                minimumCustomerEnteredPrice, maximumCustomerEnteredPrice,
                weight, length, width, height, pictureId,
                availableStartDateTime, availableEndDateTime,
                published, deleted, displayOrder, createdOn, updatedOn);
            var productVariant = DBMapping(dbItem);

            if (ProductManager.CacheEnabled)
            {
                NopCache.RemoveByPattern(PRODUCTS_PATTERN_KEY);
                NopCache.RemoveByPattern(PRODUCTVARIANTS_PATTERN_KEY);
                NopCache.RemoveByPattern(TIERPRICES_PATTERN_KEY);
            }

            return productVariant;
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
        /// <param name="lowStockActivity">The low stock activity</param>
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
        public static ProductVariant UpdateProductVariant(int productVariantId,
            int productId, string name, string sku,
            string description, string adminComment, string manufacturerPartNumber,
            bool isGiftCard, bool isDownload, int downloadId, bool unlimitedDownloads,
            int maxNumberOfDownloads, int? downloadExpirationDays,
            DownloadActivationTypeEnum downloadActivationType, bool hasSampleDownload,
            int sampleDownloadId, bool hasUserAgreement,
            string userAgreementText, bool isRecurring,
            int cycleLength, int cyclePeriod, int totalCycles,
            bool isShipEnabled, bool isFreeShipping,
            decimal additionalShippingCharge, bool isTaxExempt, int taxCategoryId,
            int manageInventory, int stockQuantity, bool displayStockAvailability,
            int minStockQuantity, LowStockActivityEnum lowStockActivity,
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
                availableStartDateTime = DateTimeHelper.ConvertToUtcTime(availableStartDateTime.Value);
            if (availableEndDateTime.HasValue)
                availableEndDateTime = DateTimeHelper.ConvertToUtcTime(availableEndDateTime.Value);

            sku = sku.Trim();

            createdOn = DateTimeHelper.ConvertToUtcTime(createdOn);
            updatedOn = DateTimeHelper.ConvertToUtcTime(updatedOn);

            var dbItem = DBProviderManager<DBProductProvider>.Provider.UpdateProductVariant(productVariantId,
                productId, name, sku, description, adminComment, manufacturerPartNumber,
                isGiftCard, isDownload, downloadId, unlimitedDownloads, maxNumberOfDownloads,
                downloadExpirationDays, (int)downloadActivationType, hasSampleDownload,
                sampleDownloadId, hasUserAgreement, userAgreementText, isRecurring, cycleLength,
                cyclePeriod, totalCycles, isShipEnabled, isFreeShipping,
                additionalShippingCharge, isTaxExempt, taxCategoryId,
                manageInventory, stockQuantity, displayStockAvailability,
                minStockQuantity, (int)lowStockActivity,
                notifyAdminForQuantityBelow, allowOutOfStockOrders,
                orderMinimumQuantity, orderMaximumQuantity, warehouseId, disableBuyButton,
                price, oldPrice, productCost, customerEntersPrice,
                minimumCustomerEnteredPrice, maximumCustomerEnteredPrice,
                weight, length, width, height, pictureId,
                availableStartDateTime, availableEndDateTime,
                published, deleted, displayOrder, createdOn, updatedOn);

            var productVariant = DBMapping(dbItem);

            if (ProductManager.CacheEnabled)
            {
                NopCache.RemoveByPattern(PRODUCTS_PATTERN_KEY);
                NopCache.RemoveByPattern(PRODUCTVARIANTS_PATTERN_KEY);
                NopCache.RemoveByPattern(TIERPRICES_PATTERN_KEY);
            }

            return productVariant;
        }

        /// <summary>
        /// Gets product variants by product identifier
        /// </summary>
        /// <param name="productId">The product identifier</param>
        /// <returns>Product variant collection</returns>
        public static ProductVariantCollection GetProductVariantsByProductId(int productId)
        {
            bool showHidden = NopContext.Current.IsAdmin;
            return GetProductVariantsByProductId(productId, showHidden);
        }

        /// <summary>
        /// Gets product variants by product identifier
        /// </summary>
        /// <param name="productId">The product identifier</param>
        /// <param name="showHidden">A value indicating whether to show hidden records</param>
        /// <returns>Product variant collection</returns>
        public static ProductVariantCollection GetProductVariantsByProductId(int productId,
            bool showHidden)
        {
            int languageId = 0;
            if (NopContext.Current != null)
                languageId = NopContext.Current.WorkingLanguage.LanguageId;

            return GetProductVariantsByProductId(productId, languageId, showHidden);
        }


        /// <summary>
        /// Gets product variants by product identifier
        /// </summary>
        /// <param name="productId">The product identifier</param>
        /// <param name="languageId">Language identifier</param>
        /// <param name="showHidden">A value indicating whether to show hidden records</param>
        /// <returns>Product variant collection</returns>
        public static ProductVariantCollection GetProductVariantsByProductId(int productId,
            int languageId, bool showHidden)
        {
            string key = string.Format(PRODUCTVARIANTS_ALL_KEY, showHidden, productId, languageId);
            object obj2 = NopCache.Get(key);
            if (ProductManager.CacheEnabled && (obj2 != null))
            {
                return (ProductVariantCollection)obj2;
            }

            var dbCollection = DBProviderManager<DBProductProvider>.Provider.GetProductVariantsByProductId(productId, languageId, showHidden);
            var productVariants = DBMapping(dbCollection);

            if (ProductManager.CacheEnabled)
            {
                NopCache.Max(key, productVariants);
            }
            return productVariants;
        }

        /// <summary>
        /// Gets restricted product variants by discount identifier
        /// </summary>
        /// <param name="discountId">The discount identifier</param>
        /// <returns>Product variant collection</returns>
        public static ProductVariantCollection GetProductVariantsRestrictedByDiscountId(int discountId)
        {
            if (discountId == 0)
                return new ProductVariantCollection();

            var dbCollection = DBProviderManager<DBProductProvider>.Provider.GetProductVariantsRestrictedByDiscountId(discountId);
            var productVariants = DBMapping(dbCollection);
            return productVariants;
        }

        /// <summary>
        /// Gets localized product variant by id
        /// </summary>
        /// <param name="productVariantLocalizedId">Localized product variant identifier</param>
        /// <returns>Product variant content</returns>
        public static ProductVariantLocalized GetProductVariantLocalizedById(int productVariantLocalizedId)
        {
            var dbItem = DBProviderManager<DBProductProvider>.Provider.GetProductVariantLocalizedById(productVariantLocalizedId);
            var item = DBMapping(dbItem);
            return item;
        }

        /// <summary>
        /// Gets localized product variant by product variant id and language id
        /// </summary>
        /// <param name="productVariantId">Product variant identifier</param>
        /// <param name="languageId">Language identifier</param>
        /// <returns>Product variant content</returns>
        public static ProductVariantLocalized GetProductVariantLocalizedByProductVariantIdAndLanguageId(int productVariantId, int languageId)
        {
            var dbItem = DBProviderManager<DBProductProvider>.Provider.GetProductVariantLocalizedByProductVariantIdAndLanguageId(productVariantId, languageId);
            var item = DBMapping(dbItem); 
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
        public static ProductVariantLocalized InsertProductVariantLocalized(int productVariantId,
            int languageId, string name, string description)
        {
            var dbItem = DBProviderManager<DBProductProvider>.Provider.InsertProductVariantLocalized(productVariantId,
                languageId, name, description);
            var item = DBMapping(dbItem);

            if (ProductManager.CacheEnabled)
            {
                NopCache.RemoveByPattern(PRODUCTS_PATTERN_KEY);
                NopCache.RemoveByPattern(PRODUCTVARIANTS_PATTERN_KEY);
                NopCache.RemoveByPattern(TIERPRICES_PATTERN_KEY);
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
        public static ProductVariantLocalized UpdateProductVariantLocalized(int productVariantLocalizedId,
            int productVariantId, int languageId, string name, string description)
        {
            var dbItem = DBProviderManager<DBProductProvider>.Provider.UpdateProductVariantLocalized(productVariantLocalizedId,
                productVariantId, languageId, name, description);
            var item = DBMapping(dbItem);

            if (ProductManager.CacheEnabled)
            {
                NopCache.RemoveByPattern(PRODUCTS_PATTERN_KEY);
                NopCache.RemoveByPattern(PRODUCTVARIANTS_PATTERN_KEY);
                NopCache.RemoveByPattern(TIERPRICES_PATTERN_KEY);
            }

            return item;
        }

        /// <summary>
        /// Marks a product variant as deleted
        /// </summary>
        /// <param name="productVariantId">Product variant identifier</param>
        public static void MarkProductVariantAsDeleted(int productVariantId)
        {
            var productVariant = GetProductVariantById(productVariantId);
            if (productVariant != null)
            {
                productVariant = UpdateProductVariant(productVariant.ProductVariantId, 
                    productVariant.ProductId, productVariant.Name,
                    productVariant.SKU, productVariant.Description, 
                    productVariant.AdminComment, productVariant.ManufacturerPartNumber,
                    productVariant.IsGiftCard, productVariant.IsDownload, 
                    productVariant.DownloadId, productVariant.UnlimitedDownloads, 
                    productVariant.MaxNumberOfDownloads,
                    productVariant.DownloadExpirationDays, (DownloadActivationTypeEnum)productVariant.DownloadActivationType,
                    productVariant.HasSampleDownload, productVariant.SampleDownloadId,
                    productVariant.HasUserAgreement, productVariant.UserAgreementText,
                    productVariant.IsRecurring, productVariant.CycleLength,
                    productVariant.CyclePeriod, productVariant.TotalCycles,
                    productVariant.IsShipEnabled, productVariant.IsFreeShipping, productVariant.AdditionalShippingCharge,
                    productVariant.IsTaxExempt, productVariant.TaxCategoryId,
                    productVariant.ManageInventory, productVariant.StockQuantity,
                    productVariant.DisplayStockAvailability,
                    productVariant.MinStockQuantity, productVariant.LowStockActivity,
                    productVariant.NotifyAdminForQuantityBelow, productVariant.AllowOutOfStockOrders,
                    productVariant.OrderMinimumQuantity, productVariant.OrderMaximumQuantity,
                    productVariant.WarehouseId, productVariant.DisableBuyButton,
                    productVariant.Price, productVariant.OldPrice,
                    productVariant.ProductCost, productVariant.CustomerEntersPrice,
                    productVariant.MinimumCustomerEnteredPrice, productVariant.MaximumCustomerEnteredPrice,
                    productVariant.Weight, productVariant.Length, productVariant.Width, productVariant.Height, productVariant.PictureId,
                    productVariant.AvailableStartDateTime, productVariant.AvailableEndDateTime,
                    productVariant.Published, true, productVariant.DisplayOrder, productVariant.CreatedOn, productVariant.UpdatedOn);
            }
        }

        /// <summary>
        /// Adjusts inventory
        /// </summary>
        /// <param name="productVariantId">Product variant identifier</param>
        /// <param name="decrease">A value indicating whether to increase or descrease product variant stock quantity</param>
        /// <param name="quantity">Quantity</param>
        /// <param name="attributesXml">Attributes in XML format</param>
        public static void AdjustInventory(int productVariantId, bool decrease,
            int quantity, string attributesXml)
        {
            var productVariant = GetProductVariantById(productVariantId);
            if (productVariant == null)
                return;

            switch ((ManageInventoryMethodEnum)productVariant.ManageInventory)
            {
                case ManageInventoryMethodEnum.DontManageStock:
                    {
                        //do nothing
                        return;
                    }
                    break;
                case ManageInventoryMethodEnum.ManageStock:
                    {
                        int newStockQuantity = 0;
                        if (decrease)
                            newStockQuantity = productVariant.StockQuantity - quantity;
                        else
                            newStockQuantity = productVariant.StockQuantity + quantity;

                        bool newPublished = productVariant.Published;
                        bool newDisableBuyButton = productVariant.DisableBuyButton;

                        //check if minimum quantity is reached
                        if (decrease)
                        {
                            if (productVariant.MinStockQuantity >= newStockQuantity)
                            {
                                switch (productVariant.LowStockActivity)
                                {
                                    case LowStockActivityEnum.DisableBuyButton:
                                        newDisableBuyButton = true;
                                        break;
                                    case LowStockActivityEnum.Unpublish:
                                        newPublished = false;
                                        break;
                                    default:
                                        break;
                                }
                            }
                        }

                        if (decrease && productVariant.NotifyAdminForQuantityBelow > newStockQuantity)
                        {
                            MessageManager.SendQuantityBelowStoreOwnerNotification(productVariant, LocalizationManager.DefaultAdminLanguage.LanguageId);
                        }

                        productVariant = UpdateProductVariant(productVariant.ProductVariantId, productVariant.ProductId, productVariant.Name,
                             productVariant.SKU, productVariant.Description, productVariant.AdminComment, productVariant.ManufacturerPartNumber,
                             productVariant.IsGiftCard, productVariant.IsDownload, productVariant.DownloadId,
                             productVariant.UnlimitedDownloads, productVariant.MaxNumberOfDownloads,
                             productVariant.DownloadExpirationDays, (DownloadActivationTypeEnum)productVariant.DownloadActivationType,
                             productVariant.HasSampleDownload, productVariant.SampleDownloadId,
                             productVariant.HasUserAgreement, productVariant.UserAgreementText,
                             productVariant.IsRecurring, productVariant.CycleLength,
                             productVariant.CyclePeriod, productVariant.TotalCycles,
                             productVariant.IsShipEnabled, productVariant.IsFreeShipping, productVariant.AdditionalShippingCharge,
                             productVariant.IsTaxExempt, productVariant.TaxCategoryId,
                             productVariant.ManageInventory, newStockQuantity, productVariant.DisplayStockAvailability,
                             productVariant.MinStockQuantity, productVariant.LowStockActivity,
                             productVariant.NotifyAdminForQuantityBelow, productVariant.AllowOutOfStockOrders,
                             productVariant.OrderMinimumQuantity, productVariant.OrderMaximumQuantity,
                             productVariant.WarehouseId, newDisableBuyButton, productVariant.Price,
                             productVariant.OldPrice, productVariant.ProductCost, 
                             productVariant.CustomerEntersPrice,
                             productVariant.MinimumCustomerEnteredPrice, 
                             productVariant.MaximumCustomerEnteredPrice,
                             productVariant.Weight, productVariant.Length, productVariant.Width,
                             productVariant.Height, productVariant.PictureId,
                             productVariant.AvailableStartDateTime, productVariant.AvailableEndDateTime,
                             newPublished, productVariant.Deleted, productVariant.DisplayOrder, productVariant.CreatedOn, productVariant.UpdatedOn);

                        if (decrease)
                        {
                            var product = productVariant.Product;
                            bool allProductVariantsUnpublished = true;
                            foreach (var pv2 in product.ProductVariants)
                            {
                                if (pv2.Published)
                                {
                                    allProductVariantsUnpublished = false;
                                    break;
                                }
                            }

                            if (allProductVariantsUnpublished)
                            {
                                UpdateProduct(product.ProductId, product.Name, product.ShortDescription,
                                    product.FullDescription, product.AdminComment, product.ProductTypeId,
                                    product.TemplateId, product.ShowOnHomePage, product.MetaKeywords, product.MetaDescription,
                                    product.MetaTitle, product.SEName, product.AllowCustomerReviews, product.AllowCustomerRatings, product.RatingSum,
                                    product.TotalRatingVotes, false, product.Deleted, product.CreatedOn, product.UpdatedOn);
                            }
                        }
                    }
                    break;
                case ManageInventoryMethodEnum.ManageStockByAttributes:
                    {
                        var combination = ProductAttributeManager.FindProductVariantAttributeCombination(productVariant.ProductVariantId, attributesXml);
                        if (combination != null)
                        {
                            int newStockQuantity = 0;
                            if (decrease)
                                newStockQuantity = combination.StockQuantity - quantity;
                            else
                                newStockQuantity = combination.StockQuantity + quantity;

                            combination = ProductAttributeManager.UpdateProductVariantAttributeCombination(combination.ProductVariantAttributeCombinationId,
                                combination.ProductVariantId, combination.AttributesXml, newStockQuantity, combination.AllowOutOfStockOrders);
                        }
                    }
                    break;
                default:
                    break;
            }
        }

        #endregion
        
        #region Product pictures

        /// <summary>
        /// Deletes a product picture mapping
        /// </summary>
        /// <param name="productPictureId">Product picture mapping identifier</param>
        public static void DeleteProductPicture(int productPictureId)
        {
            DBProviderManager<DBProductProvider>.Provider.DeleteProductPicture(productPictureId);
        }

        /// <summary>
        /// Gets a product picture mapping
        /// </summary>
        /// <param name="productPictureId">Product picture mapping identifier</param>
        /// <returns>Product picture mapping</returns>
        public static ProductPicture GetProductPictureById(int productPictureId)
        {
            if (productPictureId == 0)
                return null;

            var dbItem = DBProviderManager<DBProductProvider>.Provider.GetProductPictureById(productPictureId);
            var productPicture = DBMapping(dbItem);
            return productPicture;
        }

        /// <summary>
        /// Inserts a product picture mapping
        /// </summary>
        /// <param name="productId">Product identifier</param>
        /// <param name="pictureId">Picture identifier</param>
        /// <param name="displayOrder">The display order</param>
        /// <returns>Product picture mapping</returns>
        public static ProductPicture InsertProductPicture(int productId,
            int pictureId, int displayOrder)
        {
            var dbItem = DBProviderManager<DBProductProvider>.Provider.InsertProductPicture(productId, 
                pictureId, displayOrder);
            var productPicture = DBMapping(dbItem);
            return productPicture;
        }

        /// <summary>
        /// Updates the product picture mapping
        /// </summary>
        /// <param name="productPictureId">Product picture mapping identifier</param>
        /// <param name="productId">Product identifier</param>
        /// <param name="pictureId">Picture identifier</param>
        /// <param name="displayOrder">The display order</param>
        /// <returns>Product picture mapping</returns>
        public static ProductPicture UpdateProductPicture(int productPictureId, int productId,
            int pictureId, int displayOrder)
        {
            var dbItem = DBProviderManager<DBProductProvider>.Provider.UpdateProductPicture(productPictureId, productId,
                pictureId, displayOrder);
            var productPicture = DBMapping(dbItem);
            return productPicture;
        }

        /// <summary>
        /// Gets all product picture mappings by product identifier
        /// </summary>
        /// <param name="productId">Product identifier</param>
        /// <returns>Product picture mapping collection</returns>
        public static ProductPictureCollection GetProductPicturesByProductId(int productId)
        {
            var dbCollection = DBProviderManager<DBProductProvider>.Provider.GetProductPicturesByProductId(productId);
            var productPictures = DBMapping(dbCollection);
            return productPictures;
        }

        #endregion

        #region Product reviews

        /// <summary>
        /// Gets a product review
        /// </summary>
        /// <param name="productReviewId">Product review identifier</param>
        /// <returns>Product review</returns>
        public static ProductReview GetProductReviewById(int productReviewId)
        {
            if (productReviewId == 0)
                return null;

            var dbItem = DBProviderManager<DBProductProvider>.Provider.GetProductReviewById(productReviewId);
            var productReview = DBMapping(dbItem);
            return productReview;
        }

        /// <summary>
        /// Gets a product review collection by product identifier
        /// </summary>
        /// <param name="productId">Product identifier</param>
        /// <returns>Product review collection</returns>
        public static ProductReviewCollection GetProductReviewByProductId(int productId)
        {
            bool showHidden = NopContext.Current.IsAdmin;
            var dbCollection = DBProviderManager<DBProductProvider>.Provider.GetProductReviewByProductId(productId, showHidden);
            var productReviews = DBMapping(dbCollection);
            return productReviews;
        }

        /// <summary>
        /// Deletes a product review
        /// </summary>
        /// <param name="productReviewId">Product review identifier</param>
        public static void DeleteProductReview(int productReviewId)
        {
            DBProviderManager<DBProductProvider>.Provider.DeleteProductReview(productReviewId);
        }

        /// <summary>
        /// Gets all product reviews
        /// </summary>
        /// <returns>Product review collection</returns>
        public static ProductReviewCollection GetAllProductReviews()
        {
            bool showHidden = NopContext.Current.IsAdmin;
            var dbCollection = DBProviderManager<DBProductProvider>.Provider.GetAllProductReviews(showHidden);
            var productReviews = DBMapping(dbCollection);
            return productReviews;
        }

        /// <summary>
        /// Inserts a product review
        /// </summary>
        /// <param name="productId">The product identifier</param>
        /// <param name="customerId">The customer identifier</param>
        /// <param name="title">The review title</param>
        /// <param name="reviewText">The review text</param>
        /// <param name="rating">The review rating</param>
        /// <param name="helpfulYesTotal">Review helpful votes total</param>
        /// <param name="helpfulNoTotal">Review not helpful votes total</param>
        /// <param name="isApproved">A value indicating whether the product review is approved</param>
        /// <param name="createdOn">The date and time of instance creation</param>
        /// <returns>Product review</returns>
        public static ProductReview InsertProductReview(int productId, 
            int customerId, string title,
            string reviewText, int rating, int helpfulYesTotal,
            int helpfulNoTotal, bool isApproved, DateTime createdOn)
        {
            return InsertProductReview(productId, customerId,
             title, reviewText, rating, helpfulYesTotal,
             helpfulNoTotal, isApproved, createdOn, 
             ProductManager.NotifyAboutNewProductReviews);
        }

        /// <summary>
        /// Inserts a product review
        /// </summary>
        /// <param name="productId">The product identifier</param>
        /// <param name="customerId">The customer identifier</param>
        /// <param name="title">The review title</param>
        /// <param name="reviewText">The review text</param>
        /// <param name="rating">The review rating</param>
        /// <param name="helpfulYesTotal">Review helpful votes total</param>
        /// <param name="helpfulNoTotal">Review not helpful votes total</param>
        /// <param name="isApproved">A value indicating whether the product review is approved</param>
        /// <param name="createdOn">The date and time of instance creation</param>
        /// <param name="notify">A value indicating whether to notify the store owner</param>
        /// <returns>Product review</returns>
        public static ProductReview InsertProductReview(int productId, 
            int customerId, string title,
            string reviewText, int rating, int helpfulYesTotal,
            int helpfulNoTotal, bool isApproved, DateTime createdOn, bool notify)
        {
            if (rating < 1)
                rating = 1;
            if (rating > 5)
                rating = 5;

            createdOn = DateTimeHelper.ConvertToUtcTime(createdOn);

            var dbItem = DBProviderManager<DBProductProvider>.Provider.InsertProductReview(productId, customerId,
                title, reviewText, rating, helpfulYesTotal, helpfulNoTotal,
                isApproved, createdOn);
            var productReview = DBMapping(dbItem);
            
            //activity log
            CustomerActivityManager.InsertActivity(
                "WriteProductReview",
                LocalizationManager.GetLocaleResourceString("ActivityLog.WriteProductReview"),
                productId);

            //notify store owner
            if (notify)
            {
                MessageManager.SendProductReviewNotificationMessage(productReview, LocalizationManager.DefaultAdminLanguage.LanguageId);
            }

            return productReview;
        }

        /// <summary>
        /// Updates the product review
        /// </summary>
        /// <param name="productReviewId">The product review identifier</param>
        /// <param name="productId">The product identifier</param>
        /// <param name="customerId">The customer identifier</param>
        /// <param name="title">The review title</param>
        /// <param name="reviewText">The review text</param>
        /// <param name="rating">The review rating</param>
        /// <param name="helpfulYesTotal">Review helpful votes total</param>
        /// <param name="helpfulNoTotal">Review not helpful votes total</param>
        /// <param name="isApproved">A value indicating whether the product review is approved</param>
        /// <param name="createdOn">The date and time of instance creation</param>
        /// <returns>Product review</returns>
        public static ProductReview UpdateProductReview(int productReviewId, int productId, int customerId, string title,
            string reviewText, int rating, int helpfulYesTotal,
            int helpfulNoTotal, bool isApproved, DateTime createdOn)
        {
            createdOn = DateTimeHelper.ConvertToUtcTime(createdOn);

            var dbItem = DBProviderManager<DBProductProvider>.Provider.UpdateProductReview(productReviewId,
                productId, customerId, title, reviewText, rating,
                helpfulYesTotal, helpfulNoTotal, isApproved, createdOn);
            var productReview = DBMapping(dbItem);
            return productReview;
        }

        /// <summary>
        /// Sets a product rating helpfulness
        /// </summary>
        /// <param name="productReviewId">Product review identifer</param>
        /// <param name="wasHelpful">A value indicating whether the product review was helpful or not </param>
        public static void SetProductRatingHelpfulness(int productReviewId, bool wasHelpful)
        {
            if (NopContext.Current.User == null)
            {
                return;
            }
            if (NopContext.Current.User.IsGuest && !CustomerManager.AllowAnonymousUsersToReviewProduct)
            {
                return;
            }

            DBProviderManager<DBProductProvider>.Provider.SetProductRatingHelpfulness(productReviewId,
                NopContext.Current.User.CustomerId, wasHelpful);
        }
        
        #endregion

        #region Product types

        /// <summary>
        /// Gets all product types
        /// </summary>
        /// <returns>Product type collection</returns>
        public static ProductTypeCollection GetAllProductTypes()
        {
            var dbCollection = DBProviderManager<DBProductProvider>.Provider.GetAllProductTypes();
            var productTypes = DBMapping(dbCollection);
            return productTypes;
        }

        /// <summary>
        /// Gets a product type
        /// </summary>
        /// <param name="productTypeId">Product type identifier</param>
        /// <returns>Product type</returns>
        public static ProductType GetProductTypeById(int productTypeId)
        {
            if (productTypeId == 0)
                return null;

            var dbItem = DBProviderManager<DBProductProvider>.Provider.GetProductTypeById(productTypeId);
            var productType = DBMapping(dbItem);
            return productType;
        }

        #endregion

        #region Related products

        /// <summary>
        /// Deletes a related product
        /// </summary>
        /// <param name="relatedProductId">Related product identifer</param>
        public static void DeleteRelatedProduct(int relatedProductId)
        {
            DBProviderManager<DBProductProvider>.Provider.DeleteRelatedProduct(relatedProductId);
        }

        /// <summary>
        /// Gets a related product collection by product identifier
        /// </summary>
        /// <param name="productId1">The first product identifier</param>
        /// <returns>Related product collection</returns>
        public static RelatedProductCollection GetRelatedProductsByProductId1(int productId1)
        {
            bool showHidden = NopContext.Current.IsAdmin;
            var dbCollection = DBProviderManager<DBProductProvider>.Provider.GetRelatedProductsByProductId1(productId1, showHidden);
            var relatedProducts = DBMapping(dbCollection);
            return relatedProducts;
        }

        /// <summary>
        /// Gets a related product
        /// </summary>
        /// <param name="relatedProductId">Related product identifer</param>
        /// <returns></returns>
        public static RelatedProduct GetRelatedProductById(int relatedProductId)
        {
            if (relatedProductId == 0)
                return null;

            var dbItem = DBProviderManager<DBProductProvider>.Provider.GetRelatedProductById(relatedProductId);
            var relatedProduct = DBMapping(dbItem);
            return relatedProduct;
        }

        /// <summary>
        /// Inserts a related product
        /// </summary>
        /// <param name="productId1">The first product identifier</param>
        /// <param name="productId2">The second product identifier</param>
        /// <param name="displayOrder">The display order</param>
        /// <returns>Related product</returns>
        public static RelatedProduct InsertRelatedProduct(int productId1, 
            int productId2, int displayOrder)
        {
            var dbItem = DBProviderManager<DBProductProvider>.Provider.InsertRelatedProduct(productId1, productId2, displayOrder);
            var relatedProduct = DBMapping(dbItem);
            return relatedProduct;
        }

        /// <summary>
        /// Updates a related product
        /// </summary>
        /// <param name="relatedProductId">The related product identifier</param>
        /// <param name="productId1">The first product identifier</param>
        /// <param name="productId2">The second product identifier</param>
        /// <param name="displayOrder">The display order</param>
        /// <returns>Related product</returns>
        public static RelatedProduct UpdateRelatedProduct(int relatedProductId, 
            int productId1, int productId2, int displayOrder)
        {
            var dbItem = DBProviderManager<DBProductProvider>.Provider.UpdateRelatedProduct(relatedProductId, productId1, productId2, displayOrder);
            var relatedProduct = DBMapping(dbItem);
            return relatedProduct;
        }

        #endregion

        #region Pricelists

        /// <summary>
        /// Gets all product variants directly assigned to a pricelist
        /// </summary>
        /// <param name="pricelistId">Pricelist identifier</param>
        /// <returns>Product variants</returns>
        public static ProductVariantCollection GetProductVariantsByPricelistId(int pricelistId)
        {
            var dbItem = DBProviderManager<DBProductProvider>.Provider.GetProductVariantsByPricelistId(pricelistId);
            var newProductVariantCollection = ProductManager.DBMapping(dbItem);

            return newProductVariantCollection;
        }

        /// <summary>
        /// Deletes a pricelist
        /// </summary>
        /// <param name="pricelistId">The PricelistId of the item to be deleted</param>
        public static void DeletePricelist(int pricelistId)
        {
            DBProviderManager<DBProductProvider>.Provider.DeletePricelist(pricelistId);
        }

        /// <summary>
        /// Gets a collection of all available pricelists
        /// </summary>
        /// <returns>Collection of pricelists</returns>
        public static PricelistCollection GetAllPricelists()
        {
            var dbCollection = DBProviderManager<DBProductProvider>.Provider.GetAllPricelists();
            var pricelists = DBMapping(dbCollection);
            return pricelists;
        }

        /// <summary>
        /// Gets a pricelist
        /// </summary>
        /// <param name="pricelistId">Pricelist identifier</param>
        /// <returns>Pricelist</returns>
        public static Pricelist GetPricelistById(int pricelistId)
        {
            if (pricelistId == 0)
                return null;

            var dbItem = DBProviderManager<DBProductProvider>.Provider.GetPricelistById(pricelistId);
            var newPricelist = DBMapping(dbItem);

            return newPricelist;
        }

        /// <summary>
        /// Gets a pricelist
        /// </summary>
        /// <param name="pricelistGuid">Pricelist GUId</param>
        /// <returns>Pricelist</returns>
        public static Pricelist GetPricelistByGuid(string pricelistGuid)
        {
            var dbItem = DBProviderManager<DBProductProvider>.Provider.GetPricelistByGuid(pricelistGuid);
            var newPricelist = DBMapping(dbItem);
            return newPricelist;
        }

        /// <summary>
        /// Inserts a Pricelist
        /// </summary>
        /// <param name="exportMode">Mode of list creation (Export all, assigned only, assigned only with special price)</param>
        /// <param name="exportType">CSV or XML</param>
        /// <param name="affiliateId">Affiliate connected to this pricelist (optional), links will be created with AffiliateId</param>
        /// <param name="displayName">Displayedname</param>
        /// <param name="shortName">shortname to identify the pricelist</param>
        /// <param name="pricelistGuid">unique identifier to get pricelist "anonymous"</param>
        /// <param name="cacheTime">how long will the pricelist be in cached before new creation</param>
        /// <param name="formatLocalization">what localization will be used (numeric formats, etc.) en-US, de-DE etc.</param>
        /// <param name="description">Displayed description</param>
        /// <param name="adminNotes">Admin can put some notes here, not displayed in public</param>
        /// <param name="header">Headerline of the exported file (plain text)</param>
        /// <param name="body">template for an exportet productvariant, uses delimiters and replacement strings</param>
        /// <param name="footer">Footer line of the exportet file (plain text)</param>
        /// <param name="priceAdjustmentType">type of price adjustment (if used) (relative or absolute)</param>
        /// <param name="priceAdjustment">price will be adjusted by this amount (in accordance with PriceAdjustmentType)</param>
        /// <param name="overrideIndivAdjustment">use individual adjustment, if available, or override</param>
        /// <param name="createdOn">when was this record originally created</param>
        /// <param name="updatedOn">last time this recordset was updated</param>
        /// <returns>Pricelist</returns>
        public static Pricelist InsertPricelist(PriceListExportModeEnum exportMode, 
            PriceListExportTypeEnum exportType, int affiliateId,
            string displayName, string shortName, string pricelistGuid, 
            int cacheTime, string formatLocalization, string description,
            string adminNotes, string header, string body, string footer,
            PriceAdjustmentTypeEnum priceAdjustmentType, decimal priceAdjustment, 
            bool overrideIndivAdjustment, DateTime createdOn, DateTime updatedOn)
        {
            createdOn = DateTimeHelper.ConvertToUtcTime(createdOn);
            updatedOn = DateTimeHelper.ConvertToUtcTime(updatedOn);

            var dbItem = DBProviderManager<DBProductProvider>.Provider.InsertPricelist((int)exportMode, 
                (int)exportType, affiliateId, displayName, shortName, 
                pricelistGuid, cacheTime, formatLocalization,
                description, adminNotes, header, body, footer,
                (int)priceAdjustmentType, priceAdjustment, 
                overrideIndivAdjustment, createdOn, updatedOn);
            var newPricelist = DBMapping(dbItem);

            return newPricelist;
        }

        /// <summary>
        /// Updates the Pricelist
        /// </summary>
        /// <param name="pricelistId">Unique Identifier</param>
        /// <param name="exportMode">Mode of list creation (Export all, assigned only, assigned only with special price)</param>
        /// <param name="exportType">CSV or XML</param>
        /// <param name="affiliateId">Affiliate connected to this pricelist (optional), links will be created with AffiliateId</param>
        /// <param name="displayName">Displayedname</param>
        /// <param name="shortName">shortname to identify the pricelist</param>
        /// <param name="pricelistGuid">unique identifier to get pricelist "anonymous"</param>
        /// <param name="cacheTime">how long will the pricelist be in cached before new creation</param>
        /// <param name="formatLocalization">what localization will be used (numeric formats, etc.) en-US, de-DE etc.</param>
        /// <param name="description">Displayed description</param>
        /// <param name="adminNotes">Admin can put some notes here, not displayed in public</param>
        /// <param name="header">Headerline of the exported file (plain text)</param>
        /// <param name="body">template for an exportet productvariant, uses delimiters and replacement strings</param>
        /// <param name="footer">Footer line of the exportet file (plain text)</param>
        /// <param name="priceAdjustmentType">type of price adjustment (if used) (relative or absolute)</param>
        /// <param name="priceAdjustment">price will be adjusted by this amount (in accordance with PriceAdjustmentType)</param>
        /// <param name="overrideIndivAdjustment">use individual adjustment, if available, or override</param>
        /// <param name="createdOn">when was this record originally created</param>
        /// <param name="updatedOn">last time this recordset was updated</param>
        /// <returns>Pricelist</returns>
        public static Pricelist UpdatePricelist(int pricelistId, 
            PriceListExportModeEnum exportMode, PriceListExportTypeEnum exportType, 
            int affiliateId,  string displayName, string shortName, 
            string pricelistGuid, int cacheTime, string formatLocalization,
            string description, string adminNotes,
            string header, string body, string footer,
            PriceAdjustmentTypeEnum priceAdjustmentType, decimal priceAdjustment, 
            bool overrideIndivAdjustment, DateTime createdOn, DateTime updatedOn)
        {
            createdOn = DateTimeHelper.ConvertToUtcTime(createdOn);
            updatedOn = DateTimeHelper.ConvertToUtcTime(updatedOn);

            var dbItem = DBProviderManager<DBProductProvider>.Provider.UpdatePricelist(pricelistId, 
                (int)exportMode, (int)exportType, affiliateId, displayName, shortName, pricelistGuid,
                cacheTime, formatLocalization, description, adminNotes, header, body, footer,
                (int)priceAdjustmentType, priceAdjustment, overrideIndivAdjustment,
                createdOn, updatedOn);
            var newPricelist = DBMapping(dbItem);

            return newPricelist;
        }

        /// <summary>
        /// Deletes a ProductVariantPricelist
        /// </summary>
        /// <param name="productVariantPricelistId">ProductVariantPricelist identifier</param>
        public static void DeleteProductVariantPricelist(int productVariantPricelistId)
        {
            if (productVariantPricelistId == 0)
                return;
            DBProviderManager<DBProductProvider>.Provider.DeleteProductVariantPricelist(productVariantPricelistId);
        }

        /// <summary>
        /// Gets a ProductVariantPricelist
        /// </summary>
        /// <param name="productVariantPricelistId">ProductVariantPricelist identifier</param>
        /// <returns>ProductVariantPricelist</returns>
        public static ProductVariantPricelist GetProductVariantPricelistById(int productVariantPricelistId)
        {
            if (productVariantPricelistId == 0)
                return null;

            var dbItem = DBProviderManager<DBProductProvider>.Provider.GetProductVariantPricelistById(productVariantPricelistId);
            var productVariantPricelist = DBMapping(dbItem);
            return productVariantPricelist;
        }

        /// <summary>
        /// Gets ProductVariantPricelist
        /// </summary>
        /// <param name="productVariantId">ProductVariant identifier</param>
        /// <param name="pricelistId">Pricelist identifier</param>
        /// <returns>ProductVariantPricelist</returns>
        public static ProductVariantPricelist GetProductVariantPricelist(int productVariantId, int pricelistId)
        {
            var dbItem = DBProviderManager<DBProductProvider>.Provider.GetProductVariantPricelist(productVariantId, pricelistId);
            var productVariantPricelist = DBMapping(dbItem);
            return productVariantPricelist;
        }

        /// <summary>
        /// Inserts a ProductVariantPricelist
        /// </summary>
        /// <param name="productVariantId">The product variant identifer</param>
        /// <param name="pricelistId">The pricelist identifier</param>
        /// <param name="priceAdjustmentType">The type of price adjustment (if used) (relative or absolute)</param>
        /// <param name="priceAdjustment">The price will be adjusted by this amount</param>
        /// <param name="updatedOn">The date and time of instance update</param>
        /// <returns>ProductVariantPricelist</returns>
        public static ProductVariantPricelist InsertProductVariantPricelist(int productVariantId, 
            int pricelistId, PriceAdjustmentTypeEnum priceAdjustmentType,
            decimal priceAdjustment, DateTime updatedOn)
        {
            updatedOn = DateTimeHelper.ConvertToUtcTime(updatedOn);

            var dbItem = DBProviderManager<DBProductProvider>.Provider.InsertProductVariantPricelist(productVariantId,
                pricelistId, (int)priceAdjustmentType, priceAdjustment, updatedOn);
            var newProductVariantPricelist = DBMapping(dbItem);
            return newProductVariantPricelist;
        }

        /// <summary>
        /// Updates the ProductVariantPricelist
        /// </summary>
        /// <param name="productVariantPricelistId">The product variant pricelist identifier</param>
        /// <param name="productVariantId">The product variant identifer</param>
        /// <param name="pricelistId">The pricelist identifier</param>
        /// <param name="priceAdjustmentType">The type of price adjustment (if used) (relative or absolute)</param>
        /// <param name="priceAdjustment">The price will be adjusted by this amount</param>
        /// <param name="updatedOn">The date and time of instance update</param>
        /// <returns>ProductVariantPricelist</returns>
        public static ProductVariantPricelist UpdateProductVariantPricelist(int productVariantPricelistId, 
            int productVariantId, int pricelistId,
            PriceAdjustmentTypeEnum priceAdjustmentType, decimal priceAdjustment,
            DateTime updatedOn)
        {
            if (productVariantPricelistId == 0)
                return null;

            updatedOn = DateTimeHelper.ConvertToUtcTime(updatedOn);

            var dbItem = DBProviderManager<DBProductProvider>.Provider.UpdateProductVariantPricelist(productVariantPricelistId,
                productVariantId, pricelistId, (int)priceAdjustmentType,
                priceAdjustment, updatedOn);
            var newProductVariantPricelist = DBMapping(dbItem);

            return newProductVariantPricelist;
        }

        #endregion

        #region Tier prices

        /// <summary>
        /// Gets a tier price
        /// </summary>
        /// <param name="tierPriceId">Tier price identifier</param>
        /// <returns>Tier price</returns>
        public static TierPrice GetTierPriceById(int tierPriceId)
        {
            if (tierPriceId == 0)
                return null;

            var dbItem = DBProviderManager<DBProductProvider>.Provider.GetTierPriceById(tierPriceId);
            var tierPrice = DBMapping(dbItem);
            return tierPrice;
        }

        /// <summary>
        /// Gets tier prices by product variant identifier
        /// </summary>
        /// <param name="productVariantId">Product variant identifier</param>
        /// <returns>Tier price collection</returns>
        public static TierPriceCollection GetTierPricesByProductVariantId(int productVariantId)
        {
            if (productVariantId == 0)
                return new TierPriceCollection();

            string key = string.Format(TIERPRICES_ALLBYPRODUCTVARIANTID_KEY, productVariantId);
            object obj2 = NopCache.Get(key);
            if (ProductManager.CacheEnabled && (obj2 != null))
            {
                return (TierPriceCollection)obj2;
            }

            var dbCollection = DBProviderManager<DBProductProvider>.Provider.GetTierPricesByProductVariantId(productVariantId);
            var tierPriceCollection = DBMapping(dbCollection);

            if (ProductManager.CacheEnabled)
            {
                NopCache.Max(key, tierPriceCollection);
            }
            return tierPriceCollection;
        }

        /// <summary>
        /// Deletes a tier price
        /// </summary>
        /// <param name="tierPriceId">Tier price identifier</param>
        public static void DeleteTierPrice(int tierPriceId)
        {
            DBProviderManager<DBProductProvider>.Provider.DeleteTierPrice(tierPriceId);

            if (ProductManager.CacheEnabled)
            {
                NopCache.RemoveByPattern(PRODUCTS_PATTERN_KEY);
                NopCache.RemoveByPattern(PRODUCTVARIANTS_PATTERN_KEY);
                NopCache.RemoveByPattern(TIERPRICES_PATTERN_KEY);
            }
        }

        /// <summary>
        /// Inserts a tier price
        /// </summary>
        /// <param name="productVariantId">The product variant identifier</param>
        /// <param name="quantity">The quantity</param>
        /// <param name="price">The price</param>
        /// <returns>Tier price</returns>
        public static TierPrice InsertTierPrice(int productVariantId, 
            int quantity, decimal price)
        {
            var dbItem = DBProviderManager<DBProductProvider>.Provider.InsertTierPrice(productVariantId, quantity, price);
            var tierPrice = DBMapping(dbItem);

            if (ProductManager.CacheEnabled)
            {
                NopCache.RemoveByPattern(PRODUCTS_PATTERN_KEY);
                NopCache.RemoveByPattern(PRODUCTVARIANTS_PATTERN_KEY);
                NopCache.RemoveByPattern(TIERPRICES_PATTERN_KEY);
            }
            return tierPrice;
        }

        /// <summary>
        /// Updates the tier price
        /// </summary>
        /// <param name="tierPriceId">The tier price identifier</param>
        /// <param name="productVariantId">The product variant identifier</param>
        /// <param name="quantity">The quantity</param>
        /// <param name="price">The price</param>
        /// <returns>Tier price</returns>
        public static TierPrice UpdateTierPrice(int tierPriceId, int productVariantId, 
            int quantity, decimal price)
        {
            var dbItem = DBProviderManager<DBProductProvider>.Provider.UpdateTierPrice(tierPriceId,
                productVariantId, quantity, price);
            var tierPrice = DBMapping(dbItem);

            if (ProductManager.CacheEnabled)
            {
                NopCache.RemoveByPattern(PRODUCTS_PATTERN_KEY);
                NopCache.RemoveByPattern(PRODUCTVARIANTS_PATTERN_KEY);
                NopCache.RemoveByPattern(TIERPRICES_PATTERN_KEY);
            }

            return tierPrice;
        }

        #endregion

        #region Product prices by customer role

        /// <summary>
        /// Deletes a product price by customer role by identifier 
        /// </summary>
        /// <param name="customerRoleProductPriceId">The identifier</param>
        public static void DeleteCustomerRoleProductPrice(int customerRoleProductPriceId)
        {
            DBProviderManager<DBProductProvider>.Provider.DeleteCustomerRoleProductPrice(customerRoleProductPriceId);
        }

        /// <summary>
        /// Gets a product price by customer role by identifier 
        /// </summary>
        /// <param name="customerRoleProductPriceId">The identifier</param>
        /// <returns>Product price by customer role by identifier </returns>
        public static CustomerRoleProductPrice GetCustomerRoleProductPriceById(int customerRoleProductPriceId)
        {
            if (customerRoleProductPriceId == 0)
                return null;

            var dbItem = DBProviderManager<DBProductProvider>.Provider.GetCustomerRoleProductPriceById(customerRoleProductPriceId);
            var item = DBMapping(dbItem);
            return item;
        }

        /// <summary>
        /// Gets a collection of product prices by customer role
        /// </summary>
        /// <param name="productVariantId">Product variant identifier</param>
        /// <returns>A collection of product prices by customer role</returns>
        public static CustomerRoleProductPriceCollection GetAllCustomerRoleProductPrices(int productVariantId)
        {
            var dbCollection = DBProviderManager<DBProductProvider>.Provider.GetAllCustomerRoleProductPrices(productVariantId);
            var collection = DBMapping(dbCollection);
            return collection;
        }

        /// <summary>
        /// Inserts a product price by customer role
        /// </summary>
        /// <param name="customerRoleId">The customer role identifier</param>
        /// <param name="productVariantId">The product variant identifier</param>
        /// <param name="price">The price</param>
        /// <returns>A product price by customer role</returns>
        public static CustomerRoleProductPrice InsertCustomerRoleProductPrice(int customerRoleId,
            int productVariantId, decimal price)
        {
            var dbItem = DBProviderManager<DBProductProvider>.Provider.InsertCustomerRoleProductPrice(customerRoleId,
                productVariantId, price);
            var item = DBMapping(dbItem);
            return item;
        }

        /// <summary>
        /// Updates a product price by customer role
        /// </summary>
        /// <param name="customerRoleProductPriceId">The identifier</param>
        /// <param name="customerRoleId">The customer role identifier</param>
        /// <param name="productVariantId">The product variant identifier</param>
        /// <param name="price">The price</param>
        /// <returns>A product price by customer role</returns>
        public static CustomerRoleProductPrice UpdateCustomerRoleProductPrice(int customerRoleProductPriceId,
            int customerRoleId, int productVariantId, decimal price)
        {
            var dbItem = DBProviderManager<DBProductProvider>.Provider.UpdateCustomerRoleProductPrice(customerRoleProductPriceId,
                customerRoleId, productVariantId, price);
            var item = DBMapping(dbItem);
            return item;
        }

        #endregion

        /// <summary>
        /// Formats the text
        /// </summary>
        /// <param name="text">Text</param>
        /// <returns>Formatted text</returns>
        public static string FormatProductReviewText(string text)
        {
            if (String.IsNullOrEmpty(text))
                return string.Empty;

            text = HtmlHelper.FormatText(text, false, true, false, false, false, false);

            return text;
        }

        /// <summary>
        /// Formats the email a friend text
        /// </summary>
        /// <param name="text">Text</param>
        /// <returns>Formatted text</returns>
        public static string FormatEmailAFriendText(string text)
        {
            if (String.IsNullOrEmpty(text))
                return string.Empty;

            text = HtmlHelper.FormatText(text, false, true, false, false, false, false);
            return text;
        }

        #endregion

        #region Property
        /// <summary>
        /// Gets a value indicating whether cache is enabled
        /// </summary>
        public static bool CacheEnabled
        {
            get
            {
                return SettingManager.GetSettingValueBoolean("Cache.ProductManager.CacheEnabled");
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether "Recently viewed products" feature is enabled
        /// </summary>
        public static bool RecentlyViewedProductsEnabled
        {
            get
            {
                bool recentlyViewedProductsEnabled = SettingManager.GetSettingValueBoolean("Display.RecentlyViewedProductsEnabled");
                return recentlyViewedProductsEnabled;
            }
            set
            {
                SettingManager.SetParam("Display.RecentlyViewedProductsEnabled", value.ToString());
            }
        }

        /// <summary>
        /// Gets or sets a number of "Recently viewed products"
        /// </summary>
        public static int RecentlyViewedProductsNumber
        {
            get
            {
                int recentlyViewedProductsNumber = SettingManager.GetSettingValueInteger("Display.RecentlyViewedProductsNumber");
                return recentlyViewedProductsNumber;
            }
            set
            {
                SettingManager.SetParam("Display.RecentlyViewedProductsNumber", value.ToString());
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether "Recently added products" feature is enabled
        /// </summary>
        public static bool RecentlyAddedProductsEnabled
        {
            get
            {
                bool recentlyAddedProductsEnabled = SettingManager.GetSettingValueBoolean("Display.RecentlyAddedProductsEnabled");
                return recentlyAddedProductsEnabled;
            }
            set
            {
                SettingManager.SetParam("Display.RecentlyAddedProductsEnabled", value.ToString());
            }
        }

        /// <summary>
        /// Gets or sets a number of "Recently added products"
        /// </summary>
        public static int RecentlyAddedProductsNumber
        {
            get
            {
                int recentlyAddedProductsNumber = SettingManager.GetSettingValueInteger("Display.RecentlyAddedProductsNumber");
                return recentlyAddedProductsNumber;
            }
            set
            {
                SettingManager.SetParam("Display.RecentlyAddedProductsNumber", value.ToString());
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether "Compare products" feature is enabled
        /// </summary>
        public static bool CompareProductsEnabled
        {
            get
            {
                bool compareProductsEnabled = SettingManager.GetSettingValueBoolean("Common.EnableCompareProducts");
                return compareProductsEnabled;
            }
            set
            {
                SettingManager.SetParam("Common.EnableCompareProducts", value.ToString());
            }
        }

        /// <summary>
        /// Gets or sets "List of products purchased by other customers who purchased the above" option is enable
        /// </summary>
        public static bool ProductsAlsoPurchasedEnabled
        {
            get
            {
                bool productsAlsoPurchased = SettingManager.GetSettingValueBoolean("Display.ListOfProductsAlsoPurchasedEnabled");
                return productsAlsoPurchased;
            }
            set
            {
                SettingManager.SetParam("Display.ListOfProductsAlsoPurchasedEnabled", value.ToString());
            }
        }

        /// <summary>
        /// Gets or sets a number of products also purchased by other customers to display
        /// </summary>
        public static int ProductsAlsoPurchasedNumber
        {
            get
            {
                int productsAlsoPurchasedNumber = SettingManager.GetSettingValueInteger("Display.ListOfProductsAlsoPurchasedNumberToDisplay");
                return productsAlsoPurchasedNumber;
            }
            set
            {
                SettingManager.SetParam("Display.ListOfProductsAlsoPurchasedNumberToDisplay", value.ToString());
            }
        }


        /// <summary>
        /// Gets or sets a value indicating whether to notify about new product reviews
        /// </summary>
        public static bool NotifyAboutNewProductReviews
        {
            get
            {
                return SettingManager.GetSettingValueBoolean("Product.NotifyAboutNewProductReviews");
            }
            set
            {
                SettingManager.SetParam("Product.NotifyAboutNewProductReviews", value.ToString());
            }
        }
        #endregion
    }
}
