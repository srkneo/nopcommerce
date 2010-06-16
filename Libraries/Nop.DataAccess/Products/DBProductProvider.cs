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
        public abstract DBProductCollection GetAllProducts(int categoryId,
            int manufacturerId, int productTagId, 
            bool? featuredProducts, decimal? priceMin, decimal? priceMax, 
            string keywords, bool searchDescriptions,
            int pageSize, int pageIndex, List<int> filteredSpecs,
            int languageId, int orderBy, bool showHidden, out int totalRecords);

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
        /// Gets a list of products purchased by other customers who purchased the above
        /// </summary>
        /// <param name="productId">Product identifier</param>
        /// <param name="showHidden">A value indicating whether to show hidden records</param>
        /// <param name="pageSize">Page size</param>
        /// <param name="pageIndex">Page index</param>
        /// <param name="totalRecords">Total records</param>
        /// <returns>Product collection</returns>
        public abstract DBProductCollection GetProductsAlsoPurchasedById(int productId, 
            bool showHidden, int pageSize, int pageIndex, out int totalRecords);

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
        /// Gets all product tags
        /// </summary>
        /// <param name="productId">Product identifier</param>
        /// <param name="name">Product tag name or empty string to load all records</param>
        /// <returns>Product tag collection</returns>
        public abstract DBProductTagCollection GetAllProductTags(int productId, 
            string name);

        /// <summary>
        /// Adds a discount tag mapping
        /// </summary>
        /// <param name="productId">Product identifier</param>
        /// <param name="productTagId">Product tag identifier</param>
        public abstract void AddProductTagMapping(int productId, int productTagId);

        /// <summary>
        /// Removes a discount tag mapping
        /// </summary>
        /// <param name="productId">Product identifier</param>
        /// <param name="productTagId">Product tag identifier</param>
        public abstract void RemoveProductTagMapping(int productId, int productTagId);

        #endregion
    }
}
