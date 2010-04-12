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
using NopSolutions.NopCommerce.BusinessLogic.Caching;
using NopSolutions.NopCommerce.BusinessLogic.Configuration.Settings;
using NopSolutions.NopCommerce.BusinessLogic.CustomerManagement;
using NopSolutions.NopCommerce.BusinessLogic.Localization;
using NopSolutions.NopCommerce.BusinessLogic.Payment;
using NopSolutions.NopCommerce.BusinessLogic.Products;
using NopSolutions.NopCommerce.BusinessLogic.Products.Attributes;
using NopSolutions.NopCommerce.BusinessLogic.Profile;
using NopSolutions.NopCommerce.BusinessLogic.Promo.Discounts;
using NopSolutions.NopCommerce.BusinessLogic.Shipping;
using NopSolutions.NopCommerce.BusinessLogic.Tax;
using NopSolutions.NopCommerce.DataAccess;
using NopSolutions.NopCommerce.DataAccess.Orders;
using NopSolutions.NopCommerce.Common;
using NopSolutions.NopCommerce.BusinessLogic.Audit;
using NopSolutions.NopCommerce.Common.Utils;
using System.Web;

namespace NopSolutions.NopCommerce.BusinessLogic.Orders
{
    /// <summary>
    /// Shopping cart manager
    /// </summary>
    public partial class ShoppingCartManager
    {
        #region Utilities
        private static ShoppingCart DBMapping(DBShoppingCart dbCollection)
        {
            if (dbCollection == null)
                return null;

            var collection = new ShoppingCart();
            foreach (var dbItem in dbCollection)
            {
                var item = DBMapping(dbItem);
                collection.Add(item);
            }

            return collection;
        }

        private static ShoppingCartItem DBMapping(DBShoppingCartItem dbItem)
        {
            if (dbItem == null)
                return null;

            var item = new ShoppingCartItem();
            item.ShoppingCartItemID = dbItem.ShoppingCartItemID;
            item.ShoppingCartTypeID = dbItem.ShoppingCartTypeID;
            item.CustomerSessionGUID = dbItem.CustomerSessionGUID;
            item.ProductVariantID = dbItem.ProductVariantID;
            item.AttributesXML = dbItem.AttributesXML;
            item.CustomerEnteredPrice = dbItem.CustomerEnteredPrice;
            item.Quantity = dbItem.Quantity;
            item.CreatedOn = dbItem.CreatedOn;
            item.UpdatedOn = dbItem.UpdatedOn;

            return item;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Deletes expired shopping cart items
        /// </summary>
        /// <param name="OlderThan">Older than date and time</param>
        public static void DeleteExpiredShoppingCartItems(DateTime OlderThan)
        {
            OlderThan = DateTimeHelper.ConvertToUtcTime(OlderThan);

            DBProviderManager<DBShoppingCartProvider>.Provider.DeleteExpiredShoppingCartItems(OlderThan);
        }

        /// <summary>
        /// Deletes a shopping cart item
        /// </summary>
        /// <param name="ShoppingCartItemID">The shopping cart item identifier</param>
        /// <param name="ResetCheckoutData">A value indicating whether to reset checkout data</param>
        public static void DeleteShoppingCartItem(int ShoppingCartItemID, bool ResetCheckoutData)
        {
            if (ResetCheckoutData)
            {
                if (NopContext.Current.Session != null)
                {
                    CustomerManager.ResetCheckoutData(NopContext.Current.Session.CustomerID, false);
                }
            }

            var shoppingCartItem = GetShoppingCartItemByID(ShoppingCartItemID);
            if (shoppingCartItem != null)
            {
                if (shoppingCartItem.ShoppingCartType == ShoppingCartTypeEnum.ShoppingCart)
                {
                    CustomerActivityManager.InsertActivity(
                        "RemoveFromShoppingCart",
                        LocalizationManager.GetLocaleResourceString("ActivityLog.RemoveFromShoppingCart"),
                        shoppingCartItem.ProductVariant.FullProductName);
                }

                DBProviderManager<DBShoppingCartProvider>.Provider.DeleteShoppingCartItem(ShoppingCartItemID);
            }
        }

        /// <summary>
        /// Gets a shopping cart by customer session GUID
        /// </summary>
        /// <param name="ShoppingCartType">Shopping cart type</param>
        /// <param name="CustomerSessionGUID">The customer session identifier</param>
        /// <returns>Cart</returns>
        public static ShoppingCart GetShoppingCartByCustomerSessionGUID(ShoppingCartTypeEnum ShoppingCartType, Guid CustomerSessionGUID)
        {
            var dbCollection = DBProviderManager<DBShoppingCartProvider>.Provider.GetShoppingCartByCustomerSessionGUID((int)ShoppingCartType, CustomerSessionGUID);
            var shoppingCart = DBMapping(dbCollection);
            return shoppingCart;
        }

        /// <summary>
        /// Gets a shopping cart item
        /// </summary>
        /// <param name="ShoppingCartItemID">The shopping cart item identifier</param>
        /// <returns>Shopping cart item</returns>
        public static ShoppingCartItem GetShoppingCartItemByID(int ShoppingCartItemID)
        {
            if (ShoppingCartItemID == 0)
                return null;

            var dbItem = DBProviderManager<DBShoppingCartProvider>.Provider.GetShoppingCartItemByID(ShoppingCartItemID);
            var shoppingCartItem = DBMapping(dbItem);
            return shoppingCartItem;
        }

        /// <summary>
        /// Inserts a shopping cart item
        /// </summary>
        /// <param name="ShoppingCartType">The shopping cart type</param>
        /// <param name="CustomerSessionGUID">The customer session identifier</param>
        /// <param name="ProductVariantID">The product variant identifier</param>
        /// <param name="AttributesXML">The product variant attributes</param>
        /// <param name="CustomerEnteredPrice">The price enter by a customer</param>
        /// <param name="Quantity">The quantity</param>
        /// <param name="CreatedOn">The date and time of instance creation</param>
        /// <param name="UpdatedOn">The date and time of instance update</param>
        /// <returns>Shopping cart item</returns>
        internal static ShoppingCartItem InsertShoppingCartItem(ShoppingCartTypeEnum ShoppingCartType,
            Guid CustomerSessionGUID, int ProductVariantID, string AttributesXML,
            decimal CustomerEnteredPrice, int Quantity,
            DateTime CreatedOn, DateTime UpdatedOn)
        {
            if (AttributesXML == null)
                AttributesXML = string.Empty;

            CreatedOn = DateTimeHelper.ConvertToUtcTime(CreatedOn);
            UpdatedOn = DateTimeHelper.ConvertToUtcTime(UpdatedOn);

            var dbItem = DBProviderManager<DBShoppingCartProvider>.Provider.InsertShoppingCartItem((int)ShoppingCartType,
                CustomerSessionGUID, ProductVariantID, AttributesXML,
                CustomerEnteredPrice, Quantity, CreatedOn, UpdatedOn);

            var shoppingCartItem = DBMapping(dbItem);
            if (shoppingCartItem != null)
            {
                if (shoppingCartItem.ShoppingCartType == ShoppingCartTypeEnum.ShoppingCart)
                {
                    CustomerActivityManager.InsertActivity(
                        "AddToShoppingCart",
                        LocalizationManager.GetLocaleResourceString("ActivityLog.AddToShoppingCart"),
                        shoppingCartItem.ProductVariant.FullProductName);
                }
            }

            return shoppingCartItem;
        }

        /// <summary>
        /// Updates the shopping cart item
        /// </summary>
        /// <param name="ShoppingCartItemID">The shopping cart item identifier</param>
        /// <param name="ShoppingCartType">The shopping cart type</param>
        /// <param name="CustomerSessionGUID">The customer session identifier</param>
        /// <param name="ProductVariantID">The product variant identifier</param>
        /// <param name="AttributesXML">The product variant attributes</param>
        /// <param name="CustomerEnteredPrice">The price enter by a customer</param>
        /// <param name="Quantity">The quantity</param>
        /// <param name="CreatedOn">The date and time of instance creation</param>
        /// <param name="UpdatedOn">The date and time of instance update</param>
        /// <returns>Shopping cart item</returns>
        internal static ShoppingCartItem UpdateShoppingCartItem(int ShoppingCartItemID,
            ShoppingCartTypeEnum ShoppingCartType, Guid CustomerSessionGUID,
            int ProductVariantID, string AttributesXML, decimal CustomerEnteredPrice, 
            int Quantity, DateTime CreatedOn, DateTime UpdatedOn)
        {
            if (ShoppingCartItemID == 0)
                return null;

            if (AttributesXML == null)
                AttributesXML = string.Empty;

            CreatedOn = DateTimeHelper.ConvertToUtcTime(CreatedOn);
            UpdatedOn = DateTimeHelper.ConvertToUtcTime(UpdatedOn);

            var dbItem = DBProviderManager<DBShoppingCartProvider>.Provider.UpdateShoppingCartItem(ShoppingCartItemID, 
                (int)ShoppingCartType, CustomerSessionGUID, ProductVariantID, AttributesXML,
                CustomerEnteredPrice, Quantity, CreatedOn, UpdatedOn);

            var shoppingCartItem = DBMapping(dbItem);
            return shoppingCartItem;
        }

        /// <summary>
        /// Gets current user shopping cart
        /// </summary>
        /// <param name="ShoppingCartType">Shopping cart type</param>
        /// <returns>Cart</returns>
        public static ShoppingCart GetCurrentShoppingCart(ShoppingCartTypeEnum ShoppingCartType)
        {
            if (NopContext.Current.Session == null)
                return new ShoppingCart();
            var CustomerSessionGUID = NopContext.Current.Session.CustomerSessionGUID;
            return GetShoppingCartByCustomerSessionGUID(ShoppingCartType, CustomerSessionGUID);
        }

        /// <summary>
        /// Gets shopping cart
        /// </summary>
        /// <param name="CustomerID">Customer identifier</param>
        /// <param name="ShoppingCartType">Shopping cart type</param>
        /// <returns>Cart</returns>
        public static ShoppingCart GetCustomerShoppingCart(int CustomerID, ShoppingCartTypeEnum ShoppingCartType)
        {
            var customerSession = CustomerManager.GetCustomerSessionByCustomerID(CustomerID);
            if (customerSession == null)
                return new ShoppingCart();
            var CustomerSessionGUID = customerSession.CustomerSessionGUID;
            return GetShoppingCartByCustomerSessionGUID(ShoppingCartType, CustomerSessionGUID);
        }

        /// <summary>
        /// Gets shopping cart total
        /// </summary>
        /// <param name="Cart">Cart</param>
        /// <param name="customer">Customer</param>
        /// <returns>Shopping cart total;Null if shopping cart total couldn't be calculated now</returns>
        public static decimal? GetShoppingCartTotal(ShoppingCart Cart, Customer customer)
        {
            return GetShoppingCartTotal(Cart, 0, customer);
        }

        /// <summary>
        /// Gets shopping cart total
        /// </summary>
        /// <param name="Cart">Cart</param>
        /// <param name="PaymentMethodID">Payment method identifier</param>
        /// <param name="customer">Customer</param>
        /// <returns>Shopping cart total;Null if shopping cart total couldn't be calculated now</returns>
        public static decimal? GetShoppingCartTotal(ShoppingCart Cart, int PaymentMethodID, Customer customer)
        {
            string SubTotalError = string.Empty;
            string ShippingError = string.Empty;
            string PaymentMethodAdditionalFeeError = string.Empty;
            string TaxError = string.Empty;

            //subtotal without tax
            decimal subTotalDiscountBase = decimal.Zero;
            Discount appliedDiscount = null;
            List<AppliedGiftCard> appliedGiftCards = null;
            decimal subtotalBaseWithoutPromo = decimal.Zero;
            decimal subtotalBaseWithPromo = decimal.Zero;
            SubTotalError = ShoppingCartManager.GetShoppingCartSubTotal(Cart,
                customer, out subTotalDiscountBase,
                out appliedDiscount, out appliedGiftCards, false,
                out subtotalBaseWithoutPromo, out subtotalBaseWithPromo);
            if (!String.IsNullOrEmpty(SubTotalError))
                return null;

            //shipping without tax
            decimal? shoppingCartShipping = ShippingManager.GetShoppingCartShippingTotal(Cart, customer, false, ref ShippingError);
            if (!String.IsNullOrEmpty(ShippingError))
                return null;

            //payment method additional fee without tax
            decimal paymentMethodAdditionalFeeWithoutTax = decimal.Zero;
            if (PaymentMethodID > 0)
            {
                decimal paymentMethodAdditionalFee = PaymentManager.GetAdditionalHandlingFee(PaymentMethodID);
                paymentMethodAdditionalFeeWithoutTax = TaxManager.GetPaymentMethodAdditionalFee(paymentMethodAdditionalFee, false, customer, ref PaymentMethodAdditionalFeeError);
            }

            //tax
            decimal shoppingCartTax = TaxManager.GetTaxTotal(Cart, PaymentMethodID, customer, ref TaxError);
            if (!String.IsNullOrEmpty(TaxError))
                return null;

            if (shoppingCartShipping.HasValue)
                return Math.Round(subtotalBaseWithPromo + shoppingCartShipping.Value + paymentMethodAdditionalFeeWithoutTax + shoppingCartTax, 2);
            else
                return null;
        }

        /// <summary>
        /// Gets shopping cart subtotal
        /// </summary>
        /// <param name="Cart">Cart</param>
        /// <param name="customer">Customer</param>
        /// <param name="discountAmount">Subtotal discount amount</param>
        /// <param name="appliedDiscount">Applied discount</param>
        /// <param name="appliedGiftCards">Applied gift cards</param>
        /// <param name="subtotalWithoutPromo">Sub total without promo (discounts, gift cards)</param>
        /// <param name="subtotalWithPromo">Sub total with promo (discounts, gift cards)</param>
        /// <returns>Error</returns>
        public static string GetShoppingCartSubTotal(ShoppingCart Cart, Customer customer,
            out decimal discountAmount, out Discount appliedDiscount,
            out List<AppliedGiftCard> appliedGiftCards,
            out decimal subtotalWithoutPromo, out decimal subtotalWithPromo)
        {
            bool includingTax = false;
            switch (NopContext.Current.TaxDisplayType)
            {
                case TaxDisplayTypeEnum.ExcludingTax:
                    includingTax = false;
                    break;
                case TaxDisplayTypeEnum.IncludingTax:
                    includingTax = true;
                    break;
            }
            return GetShoppingCartSubTotal(Cart, customer, out discountAmount,
                out appliedDiscount, out appliedGiftCards, includingTax,
                out subtotalWithoutPromo, out subtotalWithPromo);
        }

        /// <summary>
        /// Gets shopping cart subtotal
        /// </summary>
        /// <param name="Cart">Cart</param>
        /// <param name="customer">Customer</param>
        /// <param name="discountAmount">Subtotal discount amount</param>
        /// <param name="appliedDiscount">Applied discount</param>
        /// <param name="appliedGiftCards">Applied gift cards</param>
        /// <param name="includingTax">A value indicating whether calculated price should include tax</param>
        /// <param name="subtotalWithoutPromo">Sub total without promo (discounts, gift cards)</param>
        /// <param name="subtotalWithPromo">Sub total with promo (discounts, gift cards)</param>
        /// <returns>Error</returns>
        public static string GetShoppingCartSubTotal(ShoppingCart Cart, Customer customer,
            out decimal discountAmount, out Discount appliedDiscount, 
            out List<AppliedGiftCard> appliedGiftCards, bool includingTax,
            out decimal subtotalWithoutPromo, out decimal subtotalWithPromo)
        {
            string Error = string.Empty;

            //sub totals without discount and gift cards
            decimal subTotalExclTaxWithoutDiscount = decimal.Zero;
            decimal subTotalInclTaxWithoutDiscount = decimal.Zero;
            foreach (var shoppingCartItem in Cart)
            {
                string Error2 = string.Empty;
                decimal sciSubTotal = PriceHelper.GetSubTotal(shoppingCartItem, customer, true);
                subTotalExclTaxWithoutDiscount += TaxManager.GetPrice(shoppingCartItem.ProductVariant, sciSubTotal, false, customer, ref Error2);
                if (!String.IsNullOrEmpty(Error2))
                {
                    Error = Error2;
                }
                subTotalInclTaxWithoutDiscount += TaxManager.GetPrice(shoppingCartItem.ProductVariant, sciSubTotal, true, customer, ref Error2);
                if (!String.IsNullOrEmpty(Error2))
                {
                    Error = Error2;
                }
            }
            if (includingTax)
                subtotalWithoutPromo = subTotalInclTaxWithoutDiscount;
            else
                subtotalWithoutPromo = subTotalExclTaxWithoutDiscount;
            

            #region Discounts
            //Discount amount (excl tax)
            //We calculate discount amount on subtotal excl tax
            //This type of discounts [Assigned to whole order] is not taxable
            discountAmount = GetOrderDiscount(customer, subTotalExclTaxWithoutDiscount, out appliedDiscount);

            //sub totals with discount
            decimal subTotalWithDiscount = decimal.Zero;
            if (includingTax)
            {
                subTotalWithDiscount = subTotalInclTaxWithoutDiscount - discountAmount;
            }
            else
            {
                subTotalWithDiscount = subTotalExclTaxWithoutDiscount - discountAmount;
            }

            if (subTotalWithDiscount < decimal.Zero)
                subTotalWithDiscount = decimal.Zero;
            subTotalWithDiscount = Math.Round(subTotalWithDiscount, 2);

            #endregion

            #region Gift Cards

            //let's apply gift cards now (gift cards that can be used)
            decimal subTotalWithGiftCards = subTotalWithDiscount;
            appliedGiftCards = new List<AppliedGiftCard>();
            var giftCards = GiftCardHelper.GetActiveGiftCards(customer);
            foreach (var gc in giftCards)
            {
                if (subTotalWithGiftCards > decimal.Zero)
                {
                    decimal remainingAmount = GiftCardHelper.GetGiftCardRemainingAmount(gc);
                    decimal amountCanBeUsed = decimal.Zero;
                    if (subTotalWithGiftCards > remainingAmount)
                        amountCanBeUsed = remainingAmount;
                    else
                        amountCanBeUsed = subTotalWithGiftCards;

                    //reduce subtotal
                    subTotalWithGiftCards -= amountCanBeUsed;

                    AppliedGiftCard appliedGiftCard = new AppliedGiftCard();
                    appliedGiftCard.GiftCardID = gc.GiftCardID;
                    appliedGiftCard.AmountCanBeUsed = amountCanBeUsed;
                    appliedGiftCards.Add(appliedGiftCard);
                }
            }

            if (subTotalWithGiftCards < decimal.Zero)
                subTotalWithGiftCards = decimal.Zero;
            subTotalWithGiftCards = Math.Round(subTotalWithGiftCards, 2);

            #endregion

            subtotalWithPromo = subTotalWithGiftCards;
            return Error;
        }

        /// <summary>
        /// Gets an order discount
        /// </summary>
        /// <param name="customer">Customer</param>
        /// <param name="orderSubTotal">Order subtotal</param>
        /// <param name="appliedDiscount">Applied discount</param>
        /// <returns>Order discount</returns>
        public static decimal GetOrderDiscount(Customer customer, decimal orderSubTotal, out Discount appliedDiscount)
        {
            decimal subTotalDiscountAmount = decimal.Zero;

            string customerCouponCode = string.Empty;
            if (customer != null)
                customerCouponCode = customer.LastAppliedCouponCode;

            var allDiscounts = DiscountManager.GetAllDiscounts(DiscountTypeEnum.AssignedToWholeOrder);
            var allowedDiscounts = new DiscountCollection();
            foreach (var _discount in allDiscounts)
            {
                if (_discount.IsActive(customerCouponCode) &&
                    _discount.DiscountType == DiscountTypeEnum.AssignedToWholeOrder &&
                    !allowedDiscounts.ContainsDiscount(_discount.Name))
                {
                    //discount requirements
                    if (_discount.CheckDiscountRequirements(customer)
                        && _discount.CheckDiscountLimitations(customer))
                    {
                        allowedDiscounts.Add(_discount);
                    }
                }
            }

            appliedDiscount = DiscountManager.GetPreferredDiscount(allowedDiscounts, orderSubTotal);
            if (appliedDiscount != null)
            {
                subTotalDiscountAmount = appliedDiscount.GetDiscountAmount(orderSubTotal);
            }

            if (subTotalDiscountAmount < decimal.Zero)
                subTotalDiscountAmount = decimal.Zero;

            subTotalDiscountAmount = Math.Round(subTotalDiscountAmount, 2);

            return subTotalDiscountAmount;
        }
              
        /// <summary>
        /// Validates shopping cart item
        /// </summary>
        /// <param name="ShoppingCartType">Shopping cart type</param>
        /// <param name="ProductVariantID">Product variant identifier</param>
        /// <param name="SelectedAttributes">Selected attributes</param>
        /// <param name="CustomerEnteredPrice">Customer entered price</param>
        /// <param name="Quantity">Quantity</param>
        /// <returns>Warnings</returns>
        public static List<string> GetShoppingCartItemWarnings(ShoppingCartTypeEnum ShoppingCartType,
            int ProductVariantID, string SelectedAttributes, decimal CustomerEnteredPrice, 
            int Quantity)
        {
            var warnings = new List<string>();
            var productVariant = ProductManager.GetProductVariantByID(ProductVariantID);
            if (productVariant == null)
            {
                warnings.Add(string.Format("Product variant (ID={0}) can not be loaded", ProductVariantID));
                return warnings;
            }

            var product = productVariant.Product;
            if (product == null)
            {
                warnings.Add(string.Format("Product (ID={0}) can not be loaded", productVariant.ProductID));
                return warnings;
            }

            if (product.Deleted || productVariant.Deleted)
            {
                warnings.Add("Product is deleted");
                return warnings;
            }

            if (!product.Published || !productVariant.Published)
            {
                warnings.Add("Product is not published");
            }

            if (productVariant.DisableBuyButton)
            {
                warnings.Add("Buying is disabled");
            }

            if (productVariant.CustomerEntersPrice)
            {
                if (CustomerEnteredPrice < productVariant.MinimumCustomerEnteredPrice ||
                CustomerEnteredPrice > productVariant.MaximumCustomerEnteredPrice)
                {
                    warnings.Add(string.Format(LocalizationManager.GetLocaleResourceString("ShoppingCart.CustomerEnteredPrice.RangeError"),
                        (int)productVariant.MinimumCustomerEnteredPrice, (int)productVariant.MaximumCustomerEnteredPrice));
                }
            }

            if (Quantity < productVariant.OrderMinimumQuantity)
            {
                warnings.Add(string.Format(LocalizationManager.GetLocaleResourceString("ShoppingCart.MinimumQuantity"), productVariant.OrderMinimumQuantity));
            }

            if (Quantity > productVariant.OrderMaximumQuantity)
            {
                warnings.Add(string.Format(LocalizationManager.GetLocaleResourceString("ShoppingCart.MaximumQuantity"), productVariant.OrderMaximumQuantity));
            }

            switch ((ManageInventoryMethodEnum)productVariant.ManageInventory)
            {
                case ManageInventoryMethodEnum.DontManageStock:
                    {
                    }
                    break;
                case ManageInventoryMethodEnum.ManageStock:
                    {
                        if (!productVariant.AllowOutOfStockOrders)
                        {
                            if (productVariant.StockQuantity < Quantity)
                            {
                                int maximumQuantityCanBeAdded = productVariant.StockQuantity;
                                if (maximumQuantityCanBeAdded <= 0)
                                    warnings.Add(LocalizationManager.GetLocaleResourceString("ShoppingCart.OutOfStock"));
                                else
                                    warnings.Add(string.Format(LocalizationManager.GetLocaleResourceString("ShoppingCart.QuantityExceedsStock"), maximumQuantityCanBeAdded));
                            }
                        }
                    }
                    break;
                case ManageInventoryMethodEnum.ManageStockByAttributes:
                    {
                        var combination = ProductAttributeManager.FindProductVariantAttributeCombination(productVariant.ProductVariantID, SelectedAttributes);
                        if (combination != null)
                        {
                            if (!combination.AllowOutOfStockOrders)
                            {
                                if (combination.StockQuantity < Quantity)
                                {
                                    int maximumQuantityCanBeAdded = combination.StockQuantity;
                                    if (maximumQuantityCanBeAdded <= 0)
                                        warnings.Add(LocalizationManager.GetLocaleResourceString("ShoppingCart.OutOfStock"));
                                    else
                                        warnings.Add(string.Format(LocalizationManager.GetLocaleResourceString("ShoppingCart.QuantityExceedsStock"), maximumQuantityCanBeAdded));
                                }
                            }
                        }
                    }
                    break;
                default:
                    break;
            }

            if (productVariant.AvailableStartDateTime.HasValue)
            {
                DateTime now = DateTimeHelper.ConvertToUtcTime(DateTime.Now);
                if (productVariant.AvailableStartDateTime.Value.CompareTo(now) > 0)
                {
                    warnings.Add("Product is not available");
                }
            }
            else if (productVariant.AvailableEndDateTime.HasValue)
            {
                DateTime now = DateTimeHelper.ConvertToUtcTime(DateTime.Now);
                if (productVariant.AvailableEndDateTime.Value.CompareTo(now) < 0)
                {
                    warnings.Add("Product is not available");
                }
            }

            //selected attributes
            warnings.AddRange(GetShoppingCartItemAttributeWarnings(ShoppingCartType, ProductVariantID, SelectedAttributes, Quantity));

            //gift cards
            warnings.AddRange(GetShoppingCartItemGiftCardWarnings(ShoppingCartType, ProductVariantID, SelectedAttributes));

            return warnings;
        }

        /// <summary>
        /// Validates shopping cart item attributes
        /// </summary>
        /// <param name="ShoppingCartType">Shopping cart type</param>
        /// <param name="ProductVariantID">Product variant identifier</param>
        /// <param name="SelectedAttributes">Selected attributes</param>
        /// <param name="Quantity">Quantity</param>
        /// <returns>Warnings</returns>
        public static List<string> GetShoppingCartItemAttributeWarnings(ShoppingCartTypeEnum ShoppingCartType,
            int ProductVariantID, string SelectedAttributes, int Quantity)
        {
            return GetShoppingCartItemAttributeWarnings(ShoppingCartType,
            ProductVariantID, SelectedAttributes, Quantity, true);
        }

        /// <summary>
        /// Validates shopping cart item attributes
        /// </summary>
        /// <param name="ShoppingCartType">Shopping cart type</param>
        /// <param name="ProductVariantID">Product variant identifier</param>
        /// <param name="SelectedAttributes">Selected attributes</param>
        /// <param name="Quantity">Quantity</param>
        /// <param name="ValidateQuantity">Value indicating whether to validation quantity</param>
        /// <returns>Warnings</returns>
        public static List<string> GetShoppingCartItemAttributeWarnings(ShoppingCartTypeEnum ShoppingCartType,
            int ProductVariantID, string SelectedAttributes, int Quantity, bool ValidateQuantity)
        {
            var warnings = new List<string>();
            var productVariant = ProductManager.GetProductVariantByID(ProductVariantID);
            if (productVariant == null)
            {
                warnings.Add(string.Format("Product variant (ID={0}) can not be loaded", ProductVariantID));
                return warnings;
            }

            //selected attributes
            var pva1Collection = ProductAttributeHelper.ParseProductVariantAttributes(SelectedAttributes);
            foreach (var pva1 in pva1Collection)
            {
                var pv1 = pva1.ProductVariant;
                if (pv1 != null)
                {
                    if (pv1.ProductVariantID != productVariant.ProductVariantID)
                    {
                        warnings.Add("Attribute error");
                    }
                }
                else
                {
                    warnings.Add("Attribute error");
                    return warnings;
                }
            }

            //existing product attributes
            var pva2Collection = productVariant.ProductVariantAttributes;
            foreach (var pva2 in pva2Collection)
            {
                if (pva2.IsRequired)
                {
                    bool found = false;
                    //selected product attributes
                    foreach (var pva1 in pva1Collection)
                    {
                        if (pva1.ProductVariantAttributeID == pva2.ProductVariantAttributeID)
                        {
                            var pvaValuesStr = ProductAttributeHelper.ParseValues(SelectedAttributes, pva1.ProductVariantAttributeID);
                            foreach (string str1 in pvaValuesStr)
                            {
                                if (!String.IsNullOrEmpty(str1.Trim()))
                                {
                                    found = true;
                                    break;
                                }
                            }
                        }
                    }

                    //if not found
                    if (!found)
                    {
                        if (!string.IsNullOrEmpty(pva2.TextPrompt))
                        {
                            warnings.Add(pva2.TextPrompt);
                        }
                        else
                        {
                            warnings.Add(string.Format(LocalizationManager.GetLocaleResourceString("ShoppingCart.SelectAttribute"), pva2.ProductAttribute.Name));
                        }
                    }
                }
            }

            return warnings;
        }

        /// <summary>
        /// Validates shopping cart item (gift card)
        /// </summary>
        /// <param name="ShoppingCartType">Shopping cart type</param>
        /// <param name="ProductVariantID">Product variant identifier</param>
        /// <param name="SelectedAttributes">Selected attributes</param>
        /// <returns>Warnings</returns>
        public static List<string> GetShoppingCartItemGiftCardWarnings(ShoppingCartTypeEnum ShoppingCartType,
            int ProductVariantID, string SelectedAttributes)
        {
            var warnings = new List<string>();
            var productVariant = ProductManager.GetProductVariantByID(ProductVariantID);
            if (productVariant == null)
            {
                warnings.Add(string.Format("Product variant (ID={0}) can not be loaded", ProductVariantID));
                return warnings;
            }

            //gift cards
            if (productVariant.IsGiftCard)
            {
                string giftCardRecipientName = string.Empty;
                string giftCardRecipientEmail = string.Empty;
                string giftCardSenderName = string.Empty;
                string giftCardSenderEmail = string.Empty;
                string giftCardMessage = string.Empty;
                ProductAttributeHelper.GetGiftCardAttribute(SelectedAttributes,
                    out giftCardRecipientName, out giftCardRecipientEmail,
                    out giftCardSenderName, out giftCardSenderEmail, out giftCardMessage);

                if (String.IsNullOrEmpty(giftCardRecipientName))
                    warnings.Add(LocalizationManager.GetLocaleResourceString("ShoppingCartWarning.RecipientNameError"));

                if (String.IsNullOrEmpty(giftCardRecipientEmail) || !CommonHelper.IsValidEmail(giftCardRecipientEmail))
                    warnings.Add(LocalizationManager.GetLocaleResourceString("ShoppingCartWarning.RecipientEmailError"));

                if (String.IsNullOrEmpty(giftCardSenderName))
                    warnings.Add(LocalizationManager.GetLocaleResourceString("ShoppingCartWarning.SenderNameError"));

                if (String.IsNullOrEmpty(giftCardSenderEmail) || !CommonHelper.IsValidEmail(giftCardSenderEmail))
                    warnings.Add(LocalizationManager.GetLocaleResourceString("ShoppingCartWarning.SenderEmailError"));
            }

            return warnings;
        }

        /// <summary>
        /// Validates whether this shopping cart is valid
        /// </summary>
        /// <param name="shoppingCart">Shopping cart</param>
        /// <returns>Warnings</returns>
        public static List<string> GetShoppingCartWarnings(ShoppingCart shoppingCart)
        {
            var warnings = new List<string>();

            bool hasStandartProducts = false;
            bool hasRecurringProducts = false;
            int cycleLength = 0;
            int cyclePeriod = 0;
            int totalCycles = 0;

            foreach (var sci in shoppingCart)
            {
                var productVariant = sci.ProductVariant;
                if (productVariant == null)
                {
                    warnings.Add(string.Format("Product variant (ID={0}) can not be loaded", sci.ProductVariantID));
                    return warnings;
                }

                if (productVariant.IsRecurring)
                {
                    hasRecurringProducts = true;
                }
                else
                {
                    hasStandartProducts = true;
                }
            }

            if (hasStandartProducts && hasRecurringProducts)
            {
                warnings.Add(LocalizationManager.GetLocaleResourceString("ShoppingCart.CannotMixStandardAndAutoshipProducts"));
            }

            if (hasRecurringProducts)
            {
                string cyclesError = GetReccuringCycleInfo(shoppingCart, out cycleLength, out cyclePeriod, out totalCycles);
                if (!string.IsNullOrEmpty(cyclesError))
                {
                    warnings.Add(cyclesError);
                    return warnings;
                }
            }

            return warnings;
        }

        /// <summary>
        /// Validates whether this shopping cart is valid
        /// </summary>
        /// <param name="shoppingCart">Shopping cart</param>
        /// <param name="cycleLength">Cycle length</param>
        /// <param name="cyclePeriod">Cycle period</param>
        /// <param name="totalCycles">Toital cycles</param>
        /// <returns>Error</returns>
        public static string GetReccuringCycleInfo(ShoppingCart shoppingCart, 
            out int cycleLength, out int cyclePeriod, out int totalCycles)
        {
            string error = string.Empty;

            cycleLength = 0;
            cyclePeriod = 0;
            totalCycles = 0;

            int? _cycleLength = null;
            int? _cyclePeriod = null;
            int? _totalCycles = null;

            foreach (var sci in shoppingCart)
            {
                var productVariant = sci.ProductVariant;
                if (productVariant == null)
                {
                    throw new NopException(string.Format("Product variant (ID={0}) can not be loaded", sci.ProductVariantID));
                }

                if (productVariant.IsRecurring)
                {
                    //cycle length
                    if (_cycleLength.HasValue && _cycleLength.Value != productVariant.CycleLength)
                    {
                        error = LocalizationManager.GetLocaleResourceString("ShoppingCart.ConflictingShipmentSchedules");
                        return error;
                    }
                    else
                    {
                        _cycleLength = productVariant.CycleLength;
                    }

                    //cycle period
                    if (_cyclePeriod.HasValue && _cyclePeriod.Value != productVariant.CyclePeriod)
                    {
                        error = LocalizationManager.GetLocaleResourceString("ShoppingCart.ConflictingShipmentSchedules");
                        return error;
                    }
                    else
                    {
                        _cyclePeriod = productVariant.CyclePeriod;
                    }

                    //total cycles
                    if (_totalCycles.HasValue && _totalCycles.Value != productVariant.TotalCycles)
                    {
                        error = LocalizationManager.GetLocaleResourceString("ShoppingCart.ConflictingShipmentSchedules");
                        return error;
                    }
                    else
                    {
                        _totalCycles = productVariant.TotalCycles;
                    }
                }
            }

            if (!_cycleLength.HasValue || !_cyclePeriod.HasValue || !_totalCycles.HasValue)
            {
                error = "No recurring products";
            }
            else
            {
                cycleLength = _cycleLength.Value;
                cyclePeriod = _cyclePeriod.Value;
                totalCycles = _totalCycles.Value;
            }

            return error;
        }

        /// <summary>
        /// Add a product variant to shopping cart
        /// </summary>
        /// <param name="ShoppingCartType">Shopping cart type</param>
        /// <param name="ProductVariantID">Product variant identifier</param>
        /// <param name="SelectedAttributes">Selected attributes</param>
        /// <param name="CustomerEnteredPrice">The price enter by a customer</param>
        /// <param name="Quantity">Quantity</param>
        /// <returns>Warnings</returns>
        public static List<string> AddToCart(ShoppingCartTypeEnum ShoppingCartType, int ProductVariantID,
            string SelectedAttributes, decimal CustomerEnteredPrice, int Quantity)
        {
            var warnings = new List<string>();
            if (ShoppingCartType == ShoppingCartTypeEnum.Wishlist && !SettingManager.GetSettingValueBoolean("Common.EnableWishlist"))
                return warnings;

            if (NopContext.Current.Session == null)
                NopContext.Current.Session = NopContext.Current.GetSession(true);

            var CustomerSessionGUID = NopContext.Current.Session.CustomerSessionGUID;

            CustomerManager.ResetCheckoutData(NopContext.Current.Session.CustomerID, false);

            var Cart = GetShoppingCartByCustomerSessionGUID(ShoppingCartType, CustomerSessionGUID);
            ShoppingCartItem shoppingCartItem = null;
            
            foreach (var _shoppingCartItem in Cart)
            {
                if (_shoppingCartItem.ProductVariantID == ProductVariantID)
                {
                    //attributes
                    bool attributesEqual = ProductAttributeHelper.AreProductAttributesEqual(_shoppingCartItem.AttributesXML, SelectedAttributes);

                    //gift cards
                    bool giftCardInfoSame = true;
                    if (_shoppingCartItem.ProductVariant.IsGiftCard)
                    {
                        string giftCardRecipientName1 = string.Empty;
                        string giftCardRecipientEmail1 = string.Empty;
                        string giftCardSenderName1 = string.Empty;
                        string giftCardSenderEmail1 = string.Empty;
                        string giftCardMessage1 = string.Empty;
                        ProductAttributeHelper.GetGiftCardAttribute(SelectedAttributes,
                            out giftCardRecipientName1, out giftCardRecipientEmail1,
                            out giftCardSenderName1, out giftCardSenderEmail1, out giftCardMessage1);

                        string giftCardRecipientName2 = string.Empty;
                        string giftCardRecipientEmail2 = string.Empty;
                        string giftCardSenderName2 = string.Empty;
                        string giftCardSenderEmail2 = string.Empty;
                        string giftCardMessage2 = string.Empty;
                        ProductAttributeHelper.GetGiftCardAttribute(_shoppingCartItem.AttributesXML,
                            out giftCardRecipientName2, out giftCardRecipientEmail2,
                            out giftCardSenderName2, out giftCardSenderEmail2, out giftCardMessage2);


                        if (giftCardRecipientName1.ToLowerInvariant() != giftCardRecipientName2.ToLowerInvariant() ||
                            giftCardSenderName1.ToLowerInvariant() != giftCardSenderName2.ToLowerInvariant())
                            giftCardInfoSame = false;
                    }

                    //price is the same (for products which requires customers to enter a price)
                    bool customerEnteredPricesEqual = true;
                    if (_shoppingCartItem.ProductVariant.CustomerEntersPrice)
                    {
                        customerEnteredPricesEqual = Math.Round(_shoppingCartItem.CustomerEnteredPrice, 2) == Math.Round(CustomerEnteredPrice, 2);
                    }

                    if (attributesEqual &&
                        giftCardInfoSame &&
                        customerEnteredPricesEqual)
                        shoppingCartItem = _shoppingCartItem;
                }
            }

            DateTime now = DateTime.Now;
            if (shoppingCartItem != null)
            {
                int newQuantity = shoppingCartItem.Quantity + Quantity;
                warnings.AddRange(GetShoppingCartItemWarnings(ShoppingCartType, ProductVariantID, 
                    SelectedAttributes, CustomerEnteredPrice, newQuantity));

                if (warnings.Count == 0)
                {
                    UpdateShoppingCartItem(shoppingCartItem.ShoppingCartItemID, 
                        ShoppingCartType, 
                        CustomerSessionGUID, 
                        ProductVariantID, 
                        SelectedAttributes,
                        shoppingCartItem.CustomerEnteredPrice,
                        newQuantity,
                        shoppingCartItem.CreatedOn, 
                        now);
                }
            }
            else
            {
                warnings.AddRange(GetShoppingCartItemWarnings(ShoppingCartType, ProductVariantID, 
                    SelectedAttributes, CustomerEnteredPrice, Quantity));
                if (warnings.Count == 0)
                {
                    //maximum items validation
                    if (ShoppingCartType == ShoppingCartTypeEnum.ShoppingCart)
                    {
                        if (Cart.Count >= SettingManager.GetSettingValueInteger("Common.MaximumShoppingCartItems", 1000))
                            return warnings;
                    }
                    else if (ShoppingCartType == ShoppingCartTypeEnum.Wishlist)
                    {
                        if (Cart.Count >= SettingManager.GetSettingValueInteger("Common.MaximumWishlistItems", 1000))
                            return warnings;
                    }

                    //insert item
                    InsertShoppingCartItem(ShoppingCartType, 
                        CustomerSessionGUID, 
                        ProductVariantID, 
                        SelectedAttributes,
                        CustomerEnteredPrice,
                        Quantity, 
                        now, 
                        now);
                }
            }

            return warnings;
        }

        /// <summary>
        /// Updates the shopping cart item
        /// </summary>
        /// <param name="ShoppingCartItemID">Shopping cart item identifier</param>
        /// <param name="NewQuantity">New shopping cart item quantity</param>
        /// <param name="ResetCheckoutData">A value indicating whether to reset checkout data</param>
        /// <returns>Warnings</returns>
        public static List<string> UpdateCart(int ShoppingCartItemID, int NewQuantity,
            bool ResetCheckoutData)
        {
            var warnings = new List<string>();

            if (NopContext.Current.Session == null)
                return warnings;

            var shoppingCartItem = GetShoppingCartItemByID(ShoppingCartItemID);
            if (shoppingCartItem != null)
            {
                if (ResetCheckoutData)
                {
                    CustomerManager.ResetCheckoutData(NopContext.Current.Session.CustomerID, false);
                }
                if (NewQuantity > 0)
                {
                    warnings.AddRange(GetShoppingCartItemWarnings(shoppingCartItem.ShoppingCartType,
                        shoppingCartItem.ProductVariantID, shoppingCartItem.AttributesXML, 
                        shoppingCartItem.CustomerEnteredPrice, NewQuantity));
                    if (warnings.Count == 0)
                    {
                        UpdateShoppingCartItem(
                            shoppingCartItem.ShoppingCartItemID, 
                            shoppingCartItem.ShoppingCartType, 
                            shoppingCartItem.CustomerSessionGUID,
                            shoppingCartItem.ProductVariantID, 
                            shoppingCartItem.AttributesXML,
                            shoppingCartItem.CustomerEnteredPrice,
                            NewQuantity, 
                            shoppingCartItem.CreatedOn, 
                            DateTime.Now);
                    }
                }
                else
                {
                    DeleteShoppingCartItem(shoppingCartItem.ShoppingCartItemID, ResetCheckoutData);
                }
            }

            return warnings;
        }

        #endregion
    }
}
