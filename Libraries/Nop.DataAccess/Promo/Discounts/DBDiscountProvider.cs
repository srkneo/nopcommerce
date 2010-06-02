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

namespace NopSolutions.NopCommerce.DataAccess.Promo.Discounts
{
    /// <summary>
    /// Acts as a base class for deriving custom discount provider
    /// </summary>
    [DBProviderSectionName("nopDataProviders/DiscountProvider")]
    public abstract partial class DBDiscountProvider : BaseDBProvider
    {
        #region Methods

        #region Discounts

        /// <summary>
        /// Gets a discount
        /// </summary>
        /// <param name="discountId">Discount identifier</param>
        /// <returns>Discount</returns>
        public abstract DBDiscount GetDiscountById(int discountId);

        /// <summary>
        /// Gets all discounts
        /// </summary>
        /// <param name="showHidden">A value indicating whether to show hidden records</param>
        /// <param name="discountTypeId">Discount type identifier; null to load all discount</param>
        /// <returns>Discount collection</returns>
        public abstract DBDiscountCollection GetAllDiscounts(bool showHidden, int? discountTypeId);

        /// <summary>
        /// Inserts a discount
        /// </summary>
        /// <param name="discountTypeId">The discount type identifier</param>
        /// <param name="discountRequirementId">The discount requirement identifier</param>
        /// <param name="discountLimitationId">The discount limitation identifier</param>
        /// <param name="name">The name</param>
        /// <param name="usePercentage">A value indicating whether to use percentage</param>
        /// <param name="discountPercentage">The discount percentage</param>
        /// <param name="discountAmount">The discount amount</param>
        /// <param name="startDate">The discount start date and time</param>
        /// <param name="endDate">The discount end date and time</param>
        /// <param name="requiresCouponCode">The value indicating whether discount requires coupon code</param>
        /// <param name="couponCode">The coupon code</param>
        /// <param name="deleted">A value indicating whether the entity has been deleted</param>
        /// <returns>Discount</returns>
        public abstract DBDiscount InsertDiscount(int discountTypeId, 
            int discountRequirementId, int discountLimitationId, 
            string name, bool usePercentage, decimal discountPercentage, 
            decimal discountAmount, DateTime startDate, DateTime endDate, 
            bool requiresCouponCode, string couponCode, bool deleted);

        /// <summary>
        /// Updates the discount
        /// </summary>
        /// <param name="discountId">Discount identifier</param>
        /// <param name="discountTypeId">The discount type identifier</param>
        /// <param name="discountRequirementId">The discount requirement identifier</param>
        /// <param name="discountLimitationId">The discount limitation identifier</param>
        /// <param name="name">The name</param>
        /// <param name="usePercentage">A value indicating whether to use percentage</param>
        /// <param name="discountPercentage">The discount percentage</param>
        /// <param name="discountAmount">The discount amount</param>
        /// <param name="startDate">The discount start date and time</param>
        /// <param name="endDate">The discount end date and time</param>
        /// <param name="requiresCouponCode">The value indicating whether discount requires coupon code</param>
        /// <param name="couponCode">The coupon code</param>
        /// <param name="deleted">A value indicating whether the entity has been deleted</param>
        /// <returns>Discount</returns>
        public abstract DBDiscount UpdateDiscount(int discountId, int discountTypeId,
            int discountRequirementId, int discountLimitationId,
            string name, bool usePercentage, decimal discountPercentage,
            decimal discountAmount, DateTime startDate, DateTime endDate,
            bool requiresCouponCode, string couponCode, bool deleted);

        /// <summary>
        /// Adds a discount to a product variant
        /// </summary>
        /// <param name="productVariantId">Product variant identifier</param>
        /// <param name="discountId">Discount identifier</param>
        public abstract void AddDiscountToProductVariant(int productVariantId, int discountId);

        /// <summary>
        /// Removes a discount from a product variant
        /// </summary>
        /// <param name="productVariantId">Product variant identifier</param>
        /// <param name="discountId">Discount identifier</param>
        public abstract void RemoveDiscountFromProductVariant(int productVariantId, int discountId);

        /// <summary>
        /// Gets a discount collection of a product variant
        /// </summary>
        /// <param name="productVariantId">Product variant identifier</param>
        /// <param name="showHidden">A value indicating whether to show hidden records</param>
        /// <returns>Discount collection</returns>
        public abstract DBDiscountCollection GetDiscountsByProductVariantId(int productVariantId, bool showHidden);

        /// <summary>
        /// Adds a discount to a category
        /// </summary>
        /// <param name="categoryId">Category identifier</param>
        /// <param name="discountId">Discount identifier</param>
        public abstract void AddDiscountToCategory(int categoryId, int discountId);

        /// <summary>
        /// Removes a discount from a category
        /// </summary>
        /// <param name="categoryId">Category identifier</param>
        /// <param name="discountId">Discount identifier</param>
        public abstract void RemoveDiscountFromCategory(int categoryId, int discountId);

        /// <summary>
        /// Gets a discount collection of a category
        /// </summary>
        /// <param name="categoryId">Category identifier</param>
        /// <param name="showHidden">A value indicating whether to show hidden records</param>
        /// <returns>Discount collection</returns>
        public abstract DBDiscountCollection GetDiscountsByCategoryId(int categoryId, bool showHidden);

        /// <summary>
        /// Adds a discount requirement
        /// </summary>
        /// <param name="productVariantId">Product variant identifier</param>
        /// <param name="discountId">Discount identifier</param>
        public abstract void AddDiscountRestriction(int productVariantId, int discountId);

        /// <summary>
        /// Removes discount requirement
        /// </summary>
        /// <param name="productVariantId">Product variant identifier</param>
        /// <param name="discountId">Discount identifier</param>
        public abstract void RemoveDiscountRestriction(int productVariantId, int discountId);

        #endregion

        #region Etc

        /// <summary>
        /// Gets all discount requirements
        /// </summary>
        /// <returns>Discount requirement collection</returns>
        public abstract DBDiscountRequirementCollection GetAllDiscountRequirements();

        /// <summary>
        /// Gets all discount types
        /// </summary>
        /// <returns>Discount type collection</returns>
        public abstract DBDiscountTypeCollection GetAllDiscountTypes();

        /// <summary>
        /// Gets all discount limitations
        /// </summary>
        /// <returns>Discount limitation collection</returns>
        public abstract DBDiscountLimitationCollection GetAllDiscountLimitations();

        #endregion

        #region Discount History

        /// <summary>
        /// Deletes a discount usage history entry
        /// </summary>
        /// <param name="discountUsageHistoryId">Discount usage history entry identifier</param>
        public abstract void DeleteDiscountUsageHistory(int discountUsageHistoryId);

        /// <summary>
        /// Gets a discount usage history entry
        /// </summary>
        /// <param name="discountUsageHistoryId">Discount usage history entry identifier</param>
        /// <returns>Discount usage history entry</returns>
        public abstract DBDiscountUsageHistory GetDiscountUsageHistoryById(int discountUsageHistoryId);

        /// <summary>
        /// Gets all discount usage history entries
        /// </summary>
        /// <param name="discountId">Discount type identifier; null to load all</param>
        /// <param name="customerId">Customer identifier; null to load all</param>
        /// <param name="orderId">Order identifier; null to load all</param>
        /// <returns>Discount usage history entries</returns>
        public abstract DBDiscountUsageHistoryCollection GetAllDiscountUsageHistoryEntries(int? discountId,
            int? customerId, int? orderId);

        /// <summary>
        /// Inserts a discount usage history entry
        /// </summary>
        /// <param name="discountId">Discount type identifier</param>
        /// <param name="customerId">Customer identifier</param>
        /// <param name="orderId">Order identifier</param>
        /// <param name="createdOn">A date and time of instance creation</param>
        /// <returns>Discount usage history entry</returns>
        public abstract DBDiscountUsageHistory InsertDiscountUsageHistory(int discountId,
            int customerId, int orderId, DateTime createdOn);

        /// <summary>
        /// Updates the discount usage history entry
        /// </summary>
        /// <param name="discountUsageHistoryId">discount usage history entry identifier</param>
        /// <param name="discountId">Discount type identifier</param>
        /// <param name="customerId">Customer identifier</param>
        /// <param name="orderId">Order identifier</param>
        /// <param name="createdOn">A date and time of instance creation</param>
        /// <returns>Discount usage history entry</returns>
        public abstract DBDiscountUsageHistory UpdateDiscountUsageHistory(int discountUsageHistoryId,
            int discountId, int customerId, int orderId, DateTime createdOn);
        
        #endregion

        #endregion
    }
}
