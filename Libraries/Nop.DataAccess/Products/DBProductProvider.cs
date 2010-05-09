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
using System.Xml;

namespace NopSolutions.NopCommerce.DataAccess.Products
{
    /// <summary>
    /// Acts as a base class for deriving custom product provider
    /// </summary>
    [DBProviderSectionName("nopDataProviders/ProductProvider")]
    public abstract partial class DBProductProvider : BaseDBProvider
    {
        #region Methods

        /// <summary>
        /// Gets all products
        /// </summary>
        /// <param name="showHidden">A value indicating whether to show hidden records</param>
        /// <param name="languageId">Language identifier</param>
        /// <returns>Product collection</returns>
        public abstract DBProductCollection GetAllProducts(bool showHidden, int languageId);

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
        /// <param name="showHidden">A value indicating whether to show hidden records</param>
        /// <param name="totalRecords">Total records</param>
        /// <returns>Product collection</returns>
        public abstract DBProductCollection GetAllProducts(int categoryId, int manufacturerId,
            bool? featuredProducts, decimal? priceMin, decimal? priceMax, 
            string keywords, bool searchDescriptions,
            int pageSize, int pageIndex, List<int> filteredSpecs, 
            int languageId, bool showHidden, out int totalRecords);

        /// <summary>
        /// Gets all products displayed on the home page
        /// </summary>
        /// <param name="showHidden">A value indicating whether to show hidden records</param>
        /// <param name="languageId">Language identifier</param>
        /// <returns>Product collection</returns>
        public abstract DBProductCollection GetAllProductsDisplayedOnHomePage(bool showHidden,
            int languageId);

        /// <summary>
        /// Gets a product
        /// </summary>
        /// <param name="productId">Product identifier</param>
        /// <param name="languageId">Language identifier</param>
        /// <returns>Product</returns>
        public abstract DBProduct GetProductById(int productId, int languageId);

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
        public abstract DBProduct InsertProduct(string name, string shortDescription, 
            string fullDescription, string adminComment, int productTypeId, 
            int templateId, bool showOnHomePage,
            string metaKeywords, string metaDescription, string metaTitle,
            string seName, bool allowCustomerReviews, bool allowCustomerRatings, 
            int ratingSum,  int totalRatingVotes, bool published, 
            bool deleted, DateTime createdOn, DateTime updatedOn);

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
        public abstract DBProduct UpdateProduct(int productId, 
            string name, string shortDescription,
            string fullDescription, string adminComment, int productTypeId,
            int templateId, bool showOnHomePage,
            string metaKeywords, string metaDescription, string metaTitle,
            string seName, bool allowCustomerReviews, bool allowCustomerRatings,
            int ratingSum, int totalRatingVotes, bool published,
            bool deleted, DateTime createdOn, DateTime updatedOn);

        /// <summary>
        /// Gets localized product by id
        /// </summary>
        /// <param name="productLocalizedId">Localized product identifier</param>
        /// <returns>Product content</returns>
        public abstract DBProductLocalized GetProductLocalizedById(int productLocalizedId);

        /// <summary>
        /// Gets localized product by product id and language id
        /// </summary>
        /// <param name="productId">Product identifier</param>
        /// <param name="languageId">Language identifier</param>
        /// <returns>Product content</returns>
        public abstract DBProductLocalized GetProductLocalizedByProductIdAndLanguageId(int productId, int languageId);

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
        public abstract DBProductLocalized InsertProductLocalized(int productId,
            int languageId, string name, string shortDescription, string fullDescription,
            string metaKeywords, string metaDescription, string metaTitle,
            string seName);

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
        public abstract DBProductLocalized UpdateProductLocalized(int productLocalizedId, 
            int productId, int languageId, string name, string shortDescription, 
            string fullDescription, string metaKeywords, string metaDescription, 
            string metaTitle, string seName);

        /// <summary>
        /// Gets localized product variant by id
        /// </summary>
        /// <param name="productVariantLocalizedId">Localized product variant identifier</param>
        /// <returns>Product variant content</returns>
        public abstract DBProductVariantLocalized GetProductVariantLocalizedById(int productVariantLocalizedId);

        /// <summary>
        /// Gets localized product variant by product variant id and language id
        /// </summary>
        /// <param name="productVariantId">Product variant identifier</param>
        /// <param name="languageId">Language identifier</param>
        /// <returns>Product variant content</returns>
        public abstract DBProductVariantLocalized GetProductVariantLocalizedByProductVariantIdAndLanguageId(int productVariantId, int languageId);
        
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
        public abstract DBProductVariantCollection GetAllProductVariants(int categoryId,
            int manufacturerId, string keywords,bool showHidden,
            int pageSize, int pageIndex, out int totalRecords);

        /// <summary>
        /// Inserts a localized product variant
        /// </summary>
        /// <param name="productVariantId">Product variant identifier</param>
        /// <param name="languageId">Language identifier</param>
        /// <param name="name">Name text</param>
        /// <param name="description">Description text</param>
        /// <returns>Product variant content</returns>
        public abstract DBProductVariantLocalized InsertProductVariantLocalized(int productVariantId,
            int languageId, string name, string description);

        /// <summary>
        /// Update a localized product variant
        /// </summary>
        /// <param name="productVariantLocalizedId">Localized product variant identifier</param>
        /// <param name="productVariantId">Product variant identifier</param>
        /// <param name="languageId">Language identifier</param>
        /// <param name="name">Name text</param>
        /// <param name="description">Description text</param>
        /// <returns>Product variant content</returns>
        public abstract DBProductVariantLocalized UpdateProductVariantLocalized(int productVariantLocalizedId,
            int productVariantId, int languageId, string name, string description);

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
        public abstract DBProductCollection GetProductsAlsoPurchasedById(int productId, 
            int languageId, bool showHidden, int pageSize, int pageIndex, out int totalRecords);

        /// <summary>
        /// Sets a product rating
        /// </summary>
        /// <param name="productId">Product identifer</param>
        /// <param name="customerId">Customer identifer</param>
        /// <param name="rating">Rating</param>
        /// <param name="ratedOn">Rating was created on</param>
        public abstract void SetProductRating(int productId, int customerId, 
            int rating, DateTime ratedOn);

        /// <summary>
        /// Gets a recently added products list
        /// </summary>
        /// <param name="number">Number of products to load</param>
        /// <param name="languageId">Language identifier</param>
        /// <param name="showHidden">A value indicating whether to show hidden records</param>
        /// <returns>Recently added products list</returns>
        public abstract DBProductCollection GetRecentlyAddedProducts(int number,
            int languageId, bool showHidden);

        /// <summary>
        /// Deletes a product picture mapping
        /// </summary>
        /// <param name="productPictureId">Product picture mapping identifier</param>
        public abstract void DeleteProductPicture(int productPictureId);

        /// <summary>
        /// Gets a product picture mapping
        /// </summary>
        /// <param name="productPictureId">Product picture mapping identifier</param>
        /// <returns>Product picture mapping</returns>
        public abstract DBProductPicture GetProductPictureById(int productPictureId);

        /// <summary>
        /// Inserts a product picture mapping
        /// </summary>
        /// <param name="productId">Product identifier</param>
        /// <param name="pictureId">Picture identifier</param>
        /// <param name="displayOrder">The display order</param>
        /// <returns>Product picture mapping</returns>
        public abstract DBProductPicture InsertProductPicture(int productId,
          int pictureId, int displayOrder);

        /// <summary>
        /// Updates the product picture mapping
        /// </summary>
        /// <param name="productPictureId">Product picture mapping identifier</param>
        /// <param name="productId">Product identifier</param>
        /// <param name="pictureId">Picture identifier</param>
        /// <param name="displayOrder">The display order</param>
        /// <returns>Product picture mapping</returns>
        public abstract DBProductPicture UpdateProductPicture(int productPictureId,
            int productId, int pictureId, int displayOrder);

        /// <summary>
        /// Gets all product picture mappings by product identifier
        /// </summary>
        /// <param name="productId">Product identifier</param>
        /// <returns>Product picture mapping collection</returns>
        public abstract DBProductPictureCollection GetProductPicturesByProductId(int productId);

        /// <summary>
        /// Gets a product review
        /// </summary>
        /// <param name="productReviewId">Product review identifier</param>
        /// <returns>Product review</returns>
        public abstract DBProductReview GetProductReviewById(int productReviewId);

        /// <summary>
        /// Gets a product review collection by product identifier
        /// </summary>
        /// <param name="productId">Product identifier</param>
        /// <param name="showHidden">A value indicating whether to show hidden records</param>
        /// <returns>Product review collection</returns>
        public abstract DBProductReviewCollection GetProductReviewByProductId(int productId, bool showHidden);

        /// <summary>
        /// Deletes a product review
        /// </summary>
        /// <param name="productReviewId">Product review identifier</param>
        public abstract void DeleteProductReview(int productReviewId);

        /// <summary>
        /// Gets all product reviews
        /// </summary>
        /// <param name="showHidden">A value indicating whether to show hidden records</param>
        /// <returns>Product review collection</returns>
        public abstract DBProductReviewCollection GetAllProductReviews(bool showHidden);

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
        public abstract DBProductReview InsertProductReview(int productId, int customerId, string title,
            string reviewText, int rating, int helpfulYesTotal,
            int helpfulNoTotal, bool isApproved, DateTime createdOn);

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
        public abstract DBProductReview UpdateProductReview(int productReviewId, int productId, int customerId, string title,
            string reviewText, int rating, int helpfulYesTotal,
            int helpfulNoTotal, bool isApproved, DateTime createdOn);

        /// <summary>
        /// Sets a product rating helpfulness
        /// </summary>
        /// <param name="productReviewId">Product review identifer</param>
        /// <param name="customerId">Customer identifer</param>
        /// <param name="wasHelpful">A value indicating whether the product review was helpful or not </param>
        public abstract void SetProductRatingHelpfulness(int productReviewId,
            int customerId, bool wasHelpful);

        /// <summary>
        /// Gets a product variant
        /// </summary>
        /// <param name="productVariantId">Product variant identifier</param>
        /// <param name="languageId">Language identifier</param>
        /// <returns>Product variant</returns>
        public abstract DBProductVariant GetProductVariantById(int productVariantId, 
            int languageId);

        /// <summary>
        /// Gets a product variant by SKU
        /// </summary>
        /// <param name="sku">SKU</param>
        /// <returns>Product variant</returns>
        public abstract DBProductVariant GetProductVariantBySKU(string sku);

        /// <summary>
        /// Get low stock product variants
        /// </summary>
        /// <returns>Result</returns>
        public abstract DBProductVariantCollection GetLowStockProductVariants();

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
        public abstract DBProductVariant InsertProductVariant(int productId,
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
            DateTime createdOn, DateTime updatedOn);

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
        public abstract DBProductVariant UpdateProductVariant(int productVariantId, 
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
            DateTime createdOn, DateTime updatedOn);

        /// <summary>
        /// Gets product variants by product identifier
        /// </summary>
        /// <param name="productId">The product identifier</param>
        /// <param name="languageId">Language identifier</param>
        /// <param name="showHidden">A value indicating whether to show hidden records</param>
        /// <returns>Product variant collection</returns>
        public abstract DBProductVariantCollection GetProductVariantsByProductId(int productId,
            int languageId, bool showHidden);

        /// <summary>
        /// Gets restricted product variants by discount identifier
        /// </summary>
        /// <param name="discountId">The discount identifier</param>
        /// <returns>Product variant collection</returns>
        public abstract DBProductVariantCollection GetProductVariantsRestrictedByDiscountId(int discountId);

        /// <summary>
        /// Deletes a related product
        /// </summary>
        /// <param name="relatedProductId">Related product identifer</param>
        public abstract void DeleteRelatedProduct(int relatedProductId);

        /// <summary>
        /// Gets a related product collection by product identifier
        /// </summary>
        /// <param name="productId1">The first product identifier</param>
        /// <param name="showHidden">A value indicating whether to show hidden records</param>
        /// <returns>Related product collection</returns>
        public abstract DBRelatedProductCollection GetRelatedProductsByProductId1(int productId1, bool showHidden);

        /// <summary>
        /// Gets a related product
        /// </summary>
        /// <param name="relatedProductId">Related product identifer</param>
        /// <returns></returns>
        public abstract DBRelatedProduct GetRelatedProductById(int relatedProductId);

        /// <summary>
        /// Inserts a related product
        /// </summary>
        /// <param name="productId1">The first product identifier</param>
        /// <param name="productId2">The second product identifier</param>
        /// <param name="displayOrder">The display order</param>
        /// <returns>Related product</returns>
        public abstract DBRelatedProduct InsertRelatedProduct(int productId1, int productId2, int displayOrder);

        /// <summary>
        /// Updates a related product
        /// </summary>
        /// <param name="relatedProductId">The related product identifier</param>
        /// <param name="productId1">The first product identifier</param>
        /// <param name="productId2">The second product identifier</param>
        /// <param name="displayOrder">The display order</param>
        /// <returns>Related product</returns>
        public abstract DBRelatedProduct UpdateRelatedProduct(int relatedProductId, 
            int productId1, int productId2, int displayOrder);

        /// <summary>
        /// Gets all product types
        /// </summary>
        /// <returns>Product type collection</returns>
        public abstract DBProductTypeCollection GetAllProductTypes();

        /// <summary>
        /// Gets a product type
        /// </summary>
        /// <param name="productTypeId">Product type identifier</param>
        /// <returns>Product type</returns>
        public abstract DBProductType GetProductTypeById(int productTypeId);

        /// <summary>
        /// Gets all product variants directly assigned to a pricelist
        /// </summary>
        /// <param name="pricelistId">Pricelist identifier</param>
        /// <returns>Product variants</returns>
        public abstract DBProductVariantCollection GetProductVariantsByPricelistId(int pricelistId);

        /// <summary>
        /// Gets a collection of all available pricelists
        /// </summary>
        /// <returns>Collection of pricelists</returns>
        public abstract DBPricelistCollection GetAllPricelists();

        /// <summary>
        /// Gets a pricelist
        /// </summary>
        /// <param name="pricelistId">Pricelist identifier</param>
        /// <returns>Pricelist</returns>
        public abstract DBPricelist GetPricelistById(int pricelistId);

        /// <summary>
        /// Gets a pricelist
        /// </summary>
        /// <param name="pricelistGuid">Pricelist GUID</param>
        /// <returns>Pricelist</returns>
        public abstract DBPricelist GetPricelistByGuid(string pricelistGuid);

        /// <summary>
        /// Inserts a pricelist
        /// </summary>
        /// <param name="exportModeId">Mode of list creation identifier</param>
        /// <param name="exportTypeId">Export type identifier</param>
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
        /// <param name="priceAdjustmentTypeId">Type of price adjustment identifier</param>
        /// <param name="priceAdjustment">price will be adjusted by this amount</param>
        /// <param name="overrideIndivAdjustment">Use individual adjustment, if available, or override</param>
        /// <param name="createdOn">When was this record originally created</param>
        /// <param name="updatedOn">Last time this record was updated</param>
        /// <returns>Pricelist</returns>
        public abstract DBPricelist InsertPricelist(int exportModeId,
            int exportTypeId, int? affiliateId, string displayName, 
            string shortName, string pricelistGuid, int cacheTime,
            string formatLocalization, string description, string adminNotes,
            string header, string body, string footer,
            int priceAdjustmentTypeId, decimal priceAdjustment, bool overrideIndivAdjustment,
            DateTime createdOn, DateTime updatedOn);

        /// <summary>
        /// Updates the Pricelist
        /// </summary>
        /// <param name="pricelistId">Unique Identifier</param>
        /// <param name="exportModeId">Mode of list creation identifier</param>
        /// <param name="exportTypeId">Export type identifier</param>
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
        /// <param name="priceAdjustmentTypeId">Type of price adjustment identifier</param>
        /// <param name="priceAdjustment">price will be adjusted by this amount</param>
        /// <param name="overrideIndivAdjustment">Use individual adjustment, if available, or override</param>
        /// <param name="createdOn">When was this record originally created</param>
        /// <param name="updatedOn">Last time this record was updated</param>
        /// <returns>Pricelist</returns>
        public abstract DBPricelist UpdatePricelist(int pricelistId, int exportModeId,
            int exportTypeId, int? affiliateId, string displayName,
            string shortName, string pricelistGuid, int cacheTime,
            string formatLocalization, string description, string adminNotes,
            string header, string body, string footer,
            int priceAdjustmentTypeId, decimal priceAdjustment, bool overrideIndivAdjustment,
            DateTime createdOn, DateTime updatedOn);

        /// <summary>
        /// Deletes a pricelist
        /// </summary>
        /// <param name="pricelistId">Pricelist identifier</param>
        public abstract void DeletePricelist(int pricelistId);

        /// <summary>
        /// Deletes a product variant pricelist
        /// </summary>
        /// <param name="productVariantPricelistId">ProductVariantPricelist identifier</param>
        public abstract void DeleteProductVariantPricelist(int productVariantPricelistId);

        /// <summary>
        /// Gets a ProductVariantPricelist
        /// </summary>
        /// <param name="productVariantPricelistId">ProductVariantPricelist identifier</param>
        /// <returns>ProductVariantPricelist</returns>
        public abstract DBProductVariantPricelist GetProductVariantPricelistById(int productVariantPricelistId);

        /// <summary>
        /// Gets ProductVariantPricelist
        /// </summary>
        /// <param name="productVariantId">ProductVariant identifier</param>
        /// <param name="pricelistId">Pricelist identifier</param>
        /// <returns>ProductVariantPricelist</returns>
        public abstract DBProductVariantPricelist GetProductVariantPricelist(int productVariantId, int pricelistId);

        /// <summary>
        /// Inserts a ProductVariantPricelist
        /// </summary>
        /// <param name="productVariantId">The product variant identifer</param>
        /// <param name="pricelistId">The pricelist identifier</param>
        /// <param name="priceAdjustmentTypeId">Price adjustment type identifier</param>
        /// <param name="priceAdjustment">The price will be adjusted by this amount</param>
        /// <param name="updatedOn">The date and time of instance update</param>
        /// <returns>ProductVariantPricelist</returns>
        public abstract DBProductVariantPricelist InsertProductVariantPricelist(int productVariantId,
            int pricelistId, int priceAdjustmentTypeId, decimal priceAdjustment,
            DateTime updatedOn);

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
        public abstract DBProductVariantPricelist UpdateProductVariantPricelist(int productVariantPricelistId, 
            int productVariantId, int pricelistId, int priceAdjustmentTypeId, 
            decimal priceAdjustment, DateTime updatedOn);

        /// <summary>
        /// Gets a tier price
        /// </summary>
        /// <param name="tierPriceId">Tier price identifier</param>
        /// <returns>Tier price</returns>
        public abstract DBTierPrice GetTierPriceById(int tierPriceId);

        /// <summary>
        /// Gets tier prices by product variant identifier
        /// </summary>
        /// <param name="productVariantId">Product variant identifier</param>
        /// <returns>Tier price collection</returns>
        public abstract DBTierPriceCollection GetTierPricesByProductVariantId(int productVariantId);

        /// <summary>
        /// Deletes a tier price
        /// </summary>
        /// <param name="tierPriceId">Tier price identifier</param>
        public abstract void DeleteTierPrice(int tierPriceId);

        /// <summary>
        /// Inserts a tier price
        /// </summary>
        /// <param name="productVariantId">The product variant identifier</param>
        /// <param name="quantity">The quantity</param>
        /// <param name="price">The price</param>
        /// <returns>Tier price</returns>
        public abstract DBTierPrice InsertTierPrice(int productVariantId, 
            int quantity, decimal price);

        /// <summary>
        /// Updates the tier price
        /// </summary>
        /// <param name="tierPriceId">The tier price identifier</param>
        /// <param name="productVariantId">The product variant identifier</param>
        /// <param name="quantity">The quantity</param>
        /// <param name="price">The price</param>
        /// <returns>Tier price</returns>
        public abstract DBTierPrice UpdateTierPrice(int tierPriceId,
            int productVariantId, int quantity, decimal price);

        /// <summary>
        /// Deletes a product price by customer role by identifier 
        /// </summary>
        /// <param name="customerRoleProductPriceId">The identifier</param>
        public abstract void DeleteCustomerRoleProductPrice(int customerRoleProductPriceId);

        /// <summary>
        /// Gets a product price by customer role by identifier 
        /// </summary>
        /// <param name="customerRoleProductPriceId">The identifier</param>
        /// <returns>Product price by customer role by identifier </returns>
        public abstract DBCustomerRoleProductPrice GetCustomerRoleProductPriceById(int customerRoleProductPriceId);

        /// <summary>
        /// Gets a collection of product prices by customer role
        /// </summary>
        /// <param name="productVariantId">Product variant identifier</param>
        /// <returns>A collection of product prices by customer role</returns>
        public abstract DBCustomerRoleProductPriceCollection GetAllCustomerRoleProductPrices(int productVariantId);

        /// <summary>
        /// Inserts a product price by customer role
        /// </summary>
        /// <param name="customerRoleId">The customer role identifier</param>
        /// <param name="productVariantId">The product variant identifier</param>
        /// <param name="price">The price</param>
        /// <returns>A product price by customer role</returns>
        public abstract DBCustomerRoleProductPrice InsertCustomerRoleProductPrice(int customerRoleId, 
            int productVariantId, decimal price);

        /// <summary>
        /// Updates a product price by customer role
        /// </summary>
        /// <param name="customerRoleProductPriceId">The identifier</param>
        /// <param name="customerRoleId">The customer role identifier</param>
        /// <param name="productVariantId">The product variant identifier</param>
        /// <param name="price">The price</param>
        /// <returns>A product price by customer role</returns>
        public abstract DBCustomerRoleProductPrice UpdateCustomerRoleProductPrice(int customerRoleProductPriceId,
            int customerRoleId, int productVariantId, decimal price);

        #endregion
    }
}
