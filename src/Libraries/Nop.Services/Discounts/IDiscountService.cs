﻿using System.Collections.Generic;
using Nop.Core;
using Nop.Core.Domain.Customers;
using Nop.Core.Domain.Discounts;

namespace Nop.Services.Discounts
{
    /// <summary>
    /// Discount service interface
    /// </summary>
    public partial interface IDiscountService
    {
        /// <summary>
        /// Delete discount
        /// </summary>
        /// <param name="discount">Discount</param>
        void DeleteDiscount(Discount discount);

        /// <summary>
        /// Gets a discount
        /// </summary>
        /// <param name="discountId">Discount identifier</param>
        /// <returns>Discount</returns>
        Discount GetDiscountById(int discountId);

        /// <summary>
        /// Gets all discounts
        /// </summary>
        /// <param name="discountType">Discount type; null to load all discount</param>
        /// <param name="showHidden">A value indicating whether to show hidden records</param>
        /// <returns>Discount collection</returns>
        IList<Discount> GetAllDiscounts(DiscountType? discountType, bool showHidden = false);

        /// <summary>
        /// Inserts a discount
        /// </summary>
        /// <param name="discount">Discount</param>
        void InsertDiscount(Discount discount);

        /// <summary>
        /// Updates the discount
        /// </summary>
        /// <param name="discount">Discount</param>
        void UpdateDiscount(Discount discount);

        /// <summary>
        /// Delete discount requirement
        /// </summary>
        /// <param name="discountRequirement">Discount requirement</param>
        void DeleteDiscountRequirement(DiscountRequirement discountRequirement);

        /// <summary>
        /// Load discount requirement rule by system name
        /// </summary>
        /// <param name="systemName">System name</param>
        /// <returns>Found discount requirement rule</returns>
        IDiscountRequirementRule LoadDiscountRequirementRuleBySystemName(string systemName);

        /// <summary>
        /// Load all discount requirement rules
        /// </summary>
        /// <returns>Discount requirement rules</returns>
        IList<IDiscountRequirementRule> LoadAllDiscountRequirementRules();


        /// <summary>
        /// Get discount by coupon code
        /// </summary>
        /// <param name="couponCode">CouponCode</param>
        /// <param name="showHidden">A value indicating whether to show hidden records</param>
        /// <returns>Discount</returns>
        Discount GetDiscountByCouponCode(string couponCode, bool showHidden = false);

        /// <summary>
        /// Check discount requirements
        /// </summary>
        /// <param name="discount">Discount</param>
        /// <param name="customer">Customer</param>
        /// <returns>true - requirement is met; otherwise, false</returns>
        bool IsDiscountValid(Discount discount, Customer customer);

        /// <summary>
        /// Check discount requirements
        /// </summary>
        /// <param name="discount">Discount</param>
        /// <param name="customer">Customer</param>
        /// <param name="couponCodeToValidate">Coupon code to validate</param>
        /// <returns>true - requirement is met; otherwise, false</returns>
        bool IsDiscountValid(Discount discount, Customer customer, string couponCodeToValidate);

        /// <summary>
        /// Gets a discount usage history record
        /// </summary>
        /// <param name="discountUsageHistoryId">Discount usage history record identifier</param>
        /// <returns>Discount usage history</returns>
        DiscountUsageHistory GetDiscountUsageHistoryById(int discountUsageHistoryId);
        
        /// <summary>
        /// Gets all discount usage history records
        /// </summary>
        /// <param name="discountId">Discount identifier</param>
        /// <param name="customerId">Customer identifier</param>
        /// <param name="pageIndex">Page index</param>
        /// <param name="pageSize">Page size</param>
        /// <returns>Discount usage history records</returns>
        IPagedList<DiscountUsageHistory> GetAllDiscountUsageHistory(int? discountId, 
            int? customerId, int pageIndex, int pageSize);

        /// <summary>
        /// Insert discount usage history record
        /// </summary>
        /// <param name="discountUsageHistory">Discount usage history record</param>
        void InsertDiscountUsageHistory(DiscountUsageHistory discountUsageHistory);
        
        /// <summary>
        /// Update discount usage history record
        /// </summary>
        /// <param name="discountUsageHistory">Discount usage history record</param>
        void UpdateDiscountUsageHistory(DiscountUsageHistory discountUsageHistory);

        /// <summary>
        /// Delete discount usage history record
        /// </summary>
        /// <param name="discountUsageHistory">Discount usage history record</param>
        void DeleteDiscountUsageHistory(DiscountUsageHistory discountUsageHistory);

    }
}
