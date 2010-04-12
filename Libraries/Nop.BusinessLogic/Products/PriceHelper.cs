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
using System.Text;
using System.Web.Compilation;
using NopSolutions.NopCommerce.BusinessLogic.Categories;
using NopSolutions.NopCommerce.BusinessLogic.Configuration.Settings;
using NopSolutions.NopCommerce.BusinessLogic.CustomerManagement;
using NopSolutions.NopCommerce.BusinessLogic.Directory;
using NopSolutions.NopCommerce.BusinessLogic.Localization;
using NopSolutions.NopCommerce.BusinessLogic.Orders;
using NopSolutions.NopCommerce.BusinessLogic.Products;
using NopSolutions.NopCommerce.BusinessLogic.Products.Attributes;
using NopSolutions.NopCommerce.BusinessLogic.Promo.Discounts;
using NopSolutions.NopCommerce.BusinessLogic.Shipping;
using NopSolutions.NopCommerce.BusinessLogic.Tax;

namespace NopSolutions.NopCommerce.BusinessLogic.Products
{
    /// <summary>
    /// Price helper
    /// </summary>
    public partial class PriceHelper
    {
        #region Utilities

        /// <summary>
        /// Gets allowed discounts
        /// </summary>
        /// <param name="productVariant">Product variant</param>
        /// <param name="customer">Customer</param>
        /// <returns>Discounts</returns>
        protected static DiscountCollection GetAllowedDiscounts(ProductVariant productVariant, Customer customer)
        {
            var allowedDiscounts = new DiscountCollection();

            string customerCouponCode = string.Empty;
            if (customer != null)
                customerCouponCode = customer.LastAppliedCouponCode;

            foreach (var _discount in productVariant.AllDiscounts)
            {
                if (_discount.IsActive(customerCouponCode) &&
                    _discount.DiscountType == DiscountTypeEnum.AssignedToSKUs &&
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

            var productCategories = CategoryManager.GetProductCategoriesByProductID(productVariant.ProductID);
            foreach (var _productCategory in productCategories)
            {
                var category = _productCategory.Category;
                if (category != null)
                {
                    foreach (var _discount in category.Discounts)
                    {
                        if (_discount.IsActive(customerCouponCode) &&
                            _discount.DiscountType == DiscountTypeEnum.AssignedToSKUs &&
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
                }
            }
            return allowedDiscounts;
        }

        /// <summary>
        /// Gets a preferred discount
        /// </summary>
        /// <param name="productVariant">Product variant</param>
        /// <param name="customer">Customer</param>
        /// <returns>Preferred discount</returns>
        protected static Discount GetPreferredDiscount(ProductVariant productVariant, Customer customer)
        {
            return GetPreferredDiscount(productVariant, customer, decimal.Zero);
        }

        /// <summary>
        /// Gets a preferred discount
        /// </summary>
        /// <param name="productVariant">Product variant</param>
        /// <param name="customer">Customer</param>
        /// <param name="AdditionalCharge">Additional charge</param>
        /// <returns>Preferred discount</returns>
        protected static Discount GetPreferredDiscount(ProductVariant productVariant, Customer customer, decimal AdditionalCharge)
        {
            var allowedDiscounts = GetAllowedDiscounts(productVariant, customer);
            decimal finalPriceWithoutDiscount = GetFinalPrice(productVariant, customer, AdditionalCharge, false);
            var preferredDiscount = DiscountManager.GetPreferredDiscount(allowedDiscounts, finalPriceWithoutDiscount);
            return preferredDiscount;
        }
      
        /// <summary>
        /// Gets a tier price
        /// </summary>
        /// <param name="productVariant">Product variant</param>
        /// <param name="Quantity">Quantity</param>
        /// <returns>Price</returns>
        protected static decimal GetTierPrice(ProductVariant productVariant, int Quantity)
        {
            var tierPrices = productVariant.TierPrices;

            int previousQty = 1;
            decimal previousPrice = productVariant.Price;            
            foreach (TierPrice tierPrice in tierPrices)
            {
                if (Quantity < tierPrice.Quantity)
                    continue;

                if (tierPrice.Quantity < previousQty)
                    continue;

                previousPrice = tierPrice.Price; 
                previousQty = tierPrice.Quantity;
            }

            return  previousPrice;
        }

        /// <summary>
        /// Gets a price by customer role (if defined)
        /// </summary>
        /// <param name="productVariant">Product variant</param>
        /// <param name="customer">Customer</param>
        /// <returns>Price</returns>
        protected static decimal? GetCustomPriceByCustomerRole(ProductVariant productVariant, Customer customer)
        {
            if (productVariant == null)
                return null;
            if (customer == null)
                return null;

            decimal? result = null;
            var customerRoles = customer.CustomerRoles;
            var crppCollection = productVariant.CustomerRoleProductPrices;
            foreach (var crpp in crppCollection)
            {
                foreach (var cr in customerRoles)
                {
                    if (cr.CustomerRoleID == crpp.CustomerRoleID)
                    {
                        if (result.HasValue)
                        {
                            if (result.Value > crpp.Price)
                            {
                                result = crpp.Price;
                            }
                        }
                        else
                        {
                            result = crpp.Price;
                        }
                    }
                }
            }
            return result;
        }

        #endregion

        #region Methods

        #region Calculation methods

        /// <summary>
        /// Gets the final price
        /// </summary>
        /// <param name="productVariant">Product variant</param>
        /// <param name="includeDiscounts">A value indicating whether include discounts or not for final price computation</param>
        /// <returns>Final price</returns>
        public static decimal GetFinalPrice(ProductVariant productVariant, 
            bool includeDiscounts)
        {
            var customer = NopContext.Current.User;
            return GetFinalPrice(productVariant, customer, includeDiscounts);
        }

        /// <summary>
        /// Gets the final price
        /// </summary>
        /// <param name="productVariant">Product variant</param>
        /// <param name="customer">The customer</param>
        /// <param name="includeDiscounts">A value indicating whether include discounts or not for final price computation</param>
        /// <returns>Final price</returns>
        public static decimal GetFinalPrice(ProductVariant productVariant, Customer customer, 
            bool includeDiscounts)
        {
            return GetFinalPrice(productVariant, customer, decimal.Zero, includeDiscounts);
        }

        /// <summary>
        /// Gets the final price
        /// </summary>
        /// <param name="productVariant">Product variant</param>
        /// <param name="customer">The customer</param>
        /// <param name="AdditionalCharge">Additional charge</param>
        /// <param name="includeDiscounts">A value indicating whether include discounts or not for final price computation</param>
        /// <returns>Final price</returns>
        public static decimal GetFinalPrice(ProductVariant productVariant, Customer customer, 
            decimal AdditionalCharge, bool includeDiscounts)
        {
            decimal result = decimal.Zero;

            decimal initialPrice = productVariant.Price;

            //price by customer role
            decimal? cpcc = GetCustomPriceByCustomerRole(productVariant, customer);            
            if (cpcc.HasValue)
            {
                initialPrice = cpcc.Value;
            }
            
            if (includeDiscounts)
            {
                decimal discountAmount = GetDiscountAmount(productVariant, customer, AdditionalCharge);
                result = initialPrice + AdditionalCharge - discountAmount;
            }
            else
            {
                result = initialPrice + AdditionalCharge;
            }
            if (result < decimal.Zero)
                result = decimal.Zero;
            return result;
        }

        /// <summary>
        /// Gets the shopping cart item sub total
        /// </summary>
        /// <param name="shoppingCartItem">The shopping cart item</param>
        /// <param name="includeDiscounts">A value indicating whether include discounts or not for price computation</param>
        /// <returns>Shopping cart item sub total</returns>
        public static decimal GetSubTotal(ShoppingCartItem shoppingCartItem, bool includeDiscounts)
        {
            var customer = NopContext.Current.User;
            return GetSubTotal(shoppingCartItem, customer, includeDiscounts);
        }

        /// <summary>
        /// Gets the shopping cart item sub total
        /// </summary>
        /// <param name="shoppingCartItem">The shopping cart item</param>
        /// <param name="customer">The customer</param>
        /// <param name="includeDiscounts">A value indicating whether include discounts or not for price computation</param>
        /// <returns>Shopping cart item sub total</returns>
        public static decimal GetSubTotal(ShoppingCartItem shoppingCartItem, Customer customer, 
            bool includeDiscounts)
        {
            return GetUnitPrice(shoppingCartItem, customer, includeDiscounts) * shoppingCartItem.Quantity;
        }

        /// <summary>
        /// Gets the shopping cart unit price (one item)
        /// </summary>
        /// <param name="shoppingCartItem">The shopping cart item</param>
        /// <param name="includeDiscounts">A value indicating whether include discounts or not for price computation</param>
        /// <returns>Shopping cart unit price (one item)</returns>
        public static decimal GetUnitPrice(ShoppingCartItem shoppingCartItem, bool includeDiscounts)
        {
            var customer = NopContext.Current.User;
            return GetUnitPrice(shoppingCartItem, customer, includeDiscounts);
        }

        /// <summary>
        /// Gets the shopping cart unit price (one item)
        /// </summary>
        /// <param name="shoppingCartItem">The shopping cart item</param>
        /// <param name="customer">The customer</param>
        /// <param name="includeDiscounts">A value indicating whether include discounts or not for price computation</param>
        /// <returns>Shopping cart unit price (one item)</returns>
        public static decimal GetUnitPrice(ShoppingCartItem shoppingCartItem, Customer customer,
            bool includeDiscounts)
        {
            decimal finalPrice = decimal.Zero;
            var productVariant = shoppingCartItem.ProductVariant;
            if (productVariant != null)
            {
                decimal attributesTotalPrice = decimal.Zero;

                var pvaValues = ProductAttributeHelper.ParseProductVariantAttributeValues(shoppingCartItem.AttributesXML);
                foreach (var pvaValue in pvaValues)
                {
                    attributesTotalPrice += pvaValue.PriceAdjustment;
                }

                if (productVariant.CustomerEntersPrice)
                {
                    finalPrice = shoppingCartItem.CustomerEnteredPrice;
                }
                else
                {
                    finalPrice = GetFinalPrice(productVariant, customer, attributesTotalPrice, includeDiscounts);

                    if (productVariant.TierPrices.Count > 0)
                    {
                        decimal tierPrice = GetTierPrice(productVariant, shoppingCartItem.Quantity);
                        finalPrice = Math.Min(finalPrice, tierPrice);
                    }
                }
            }

            finalPrice = Math.Round(finalPrice, 2);

            return finalPrice;
        }



        /// <summary>
        /// Gets discount amount
        /// </summary>
        /// <param name="productVariant">Product variant</param>
        /// <returns>Discount amount</returns>
        public static decimal GetDiscountAmount(ProductVariant productVariant)
        {
            var customer = NopContext.Current.User;
            return GetDiscountAmount(productVariant, customer, decimal.Zero);
        }

        /// <summary>
        /// Gets discount amount
        /// </summary>
        /// <param name="productVariant">Product variant</param>
        /// <param name="customer">The customer</param>
        /// <returns>Discount amount</returns>
        public static decimal GetDiscountAmount(ProductVariant productVariant, Customer customer)
        {
            return GetDiscountAmount(productVariant, customer, decimal.Zero);
        }

        /// <summary>
        /// Gets discount amount
        /// </summary>
        /// <param name="productVariant">Product variant</param>
        /// <param name="customer">The customer</param>
        /// <param name="AdditionalCharge">Additional charge</param>
        /// <returns>Discount amount</returns>
        public static decimal GetDiscountAmount(ProductVariant productVariant, Customer customer, 
            decimal AdditionalCharge)
        {
            Discount appliedDiscount = null;
            return GetDiscountAmount(productVariant, customer, AdditionalCharge, out appliedDiscount);
        }

        /// <summary>
        /// Gets discount amount
        /// </summary>
        /// <param name="productVariant">Product variant</param>
        /// <param name="customer">The customer</param>
        /// <param name="AdditionalCharge">Additional charge</param>
        /// <param name="appliedDiscount">Applied discount</param>
        /// <returns>Discount amount</returns>
        public static decimal GetDiscountAmount(ProductVariant productVariant, Customer customer,
            decimal AdditionalCharge, out Discount appliedDiscount)
        {
            decimal appliedDiscountAmount = decimal.Zero;

            //we don't apply discounts to products with price entered by a customer
            if (productVariant.CustomerEntersPrice)
            {
                appliedDiscount = null;
                return appliedDiscountAmount;
            }

            appliedDiscount = GetPreferredDiscount(productVariant, customer, AdditionalCharge);
            if (appliedDiscount != null)
            {
                decimal finalPriceWithoutDiscount = GetFinalPrice(productVariant, customer, AdditionalCharge, false);
                appliedDiscountAmount = appliedDiscount.GetDiscountAmount(finalPriceWithoutDiscount);
            }

            return appliedDiscountAmount;
        }



        /// <summary>
        /// Gets discount amount
        /// </summary>
        /// <param name="shoppingCartItem">The shopping cart item</param>
        /// <returns>Discount amount</returns>
        public static decimal GetDiscountAmount(ShoppingCartItem shoppingCartItem)
        {
            var customer = NopContext.Current.User;
            return GetDiscountAmount(shoppingCartItem, customer);
        }

        /// <summary>
        /// Gets discount amount
        /// </summary>
        /// <param name="shoppingCartItem">The shopping cart item</param>
        /// <param name="customer">The customer</param>
        /// <returns>Discount amount</returns>
        public static decimal GetDiscountAmount(ShoppingCartItem shoppingCartItem, Customer customer)
        {
            Discount appliedDiscount = null;
            return GetDiscountAmount(shoppingCartItem, customer, out appliedDiscount);
        }

        /// <summary>
        /// Gets discount amount
        /// </summary>
        /// <param name="shoppingCartItem">The shopping cart item</param>
        /// <param name="customer">The customer</param>
        /// <param name="appliedDiscount">Applied discount</param>
        /// <returns>Discount amount</returns>
        public static decimal GetDiscountAmount(ShoppingCartItem shoppingCartItem, Customer customer,
            out Discount appliedDiscount)
        {
            appliedDiscount = null;
            decimal discountAmount = decimal.Zero;
            var productVariant = shoppingCartItem.ProductVariant;
            if (productVariant != null)
            {
                decimal attributesTotalPrice = decimal.Zero;

                var pvaValues = ProductAttributeHelper.ParseProductVariantAttributeValues(shoppingCartItem.AttributesXML);
                foreach (var pvaValue in pvaValues)
                {
                    attributesTotalPrice += pvaValue.PriceAdjustment;
                }

                decimal productVariantDiscountAmount = GetDiscountAmount(productVariant, customer, attributesTotalPrice, out appliedDiscount);
                discountAmount = productVariantDiscountAmount * shoppingCartItem.Quantity;
            }

            discountAmount = Math.Round(discountAmount, 2);
            return discountAmount;
        }
        
        #endregion
        
        #region Formatting

        /// <summary>
        /// Formats the price
        /// </summary>
        /// <param name="Price">Price</param>
        /// <returns>Price</returns>
        public static string FormatPrice(decimal Price)
        {
            bool ShowCurrency = true;
            var TargetCurrency = NopContext.Current.WorkingCurrency;
            return FormatPrice(Price, ShowCurrency, TargetCurrency);
        }

        /// <summary>
        /// Formats the price
        /// </summary>
        /// <param name="Price">Price</param>
        /// <param name="ShowCurrency">A value indicating whether to show a currency</param>
        /// <param name="TargetCurrency">Target currency</param>
        /// <returns>Price</returns>
        public static string FormatPrice(decimal Price, bool ShowCurrency, Currency TargetCurrency)
        {
            var Language = NopContext.Current.WorkingLanguage;
            bool priceIncludesTax = false;
            switch (NopContext.Current.TaxDisplayType)
            {
                case TaxDisplayTypeEnum.ExcludingTax:
                    priceIncludesTax = false;
                    break;
                case TaxDisplayTypeEnum.IncludingTax:
                    priceIncludesTax = true;
                    break;
            }
            return FormatPrice(Price, ShowCurrency, TargetCurrency, Language, priceIncludesTax);
        }

        /// <summary>
        /// Formats the price
        /// </summary>
        /// <param name="Price">Price</param>
        /// <param name="ShowCurrency">A value indicating whether to show a currency</param>
        /// <param name="ShowTax">A value indicating whether to show tax suffix</param>
        /// <returns>Price</returns>
        public static string FormatPrice(decimal Price, bool ShowCurrency, bool ShowTax)
        {
            var TargetCurrency = NopContext.Current.WorkingCurrency;
            var Language = NopContext.Current.WorkingLanguage;
            bool priceIncludesTax = false;
            switch (NopContext.Current.TaxDisplayType)
            {
                case TaxDisplayTypeEnum.ExcludingTax:
                    priceIncludesTax = false;
                    break;
                case TaxDisplayTypeEnum.IncludingTax:
                    priceIncludesTax = true;
                    break;
            }
            return FormatPrice(Price, ShowCurrency, TargetCurrency, Language, priceIncludesTax, ShowTax);
        }

        /// <summary>
        /// Formats the price
        /// </summary>
        /// <param name="Price">Price</param>
        /// <param name="ShowCurrency">A value indicating whether to show a currency</param>
        /// <param name="CurrencyCode">Currency code</param>
        /// <param name="ShowTax">A value indicating whether to show tax suffix</param>
        /// <returns>Price</returns>
        public static string FormatPrice(decimal Price, bool ShowCurrency, string CurrencyCode,
            bool ShowTax)
        {
            var currency = CurrencyManager.GetCurrencyByCode(CurrencyCode);
            if (currency == null)
            {
                currency = new Currency();
                currency.CurrencyCode = CurrencyCode;
            }
            var Language = NopContext.Current.WorkingLanguage;
            bool priceIncludesTax = false;
            switch (NopContext.Current.TaxDisplayType)
            {
                case TaxDisplayTypeEnum.ExcludingTax:
                    priceIncludesTax = false;
                    break;
                case TaxDisplayTypeEnum.IncludingTax:
                    priceIncludesTax = true;
                    break;
            }

            return FormatPrice(Price, ShowCurrency, currency, Language, priceIncludesTax, ShowTax);
        }

        /// <summary>
        /// Formats the price
        /// </summary>
        /// <param name="Price">Price</param>
        /// <param name="ShowCurrency">A value indicating whether to show a currency</param>
        /// <param name="TargetCurrency">Target currency</param>
        /// <param name="Language">Language</param>
        /// <param name="priceIncludesTax">A value indicating whether price includes tax</param>
        /// <returns>Price</returns>
        public static string FormatPrice(decimal Price, bool ShowCurrency, Currency TargetCurrency,
            Language Language, bool priceIncludesTax)
        {
            bool ShowTax = TaxManager.DisplayTaxSuffix;
            return FormatPrice(Price, ShowCurrency, TargetCurrency, Language, priceIncludesTax, ShowTax);
        }

        /// <summary>
        /// Formats the price
        /// </summary>
        /// <param name="Price">Price</param>
        /// <param name="ShowCurrency">A value indicating whether to show a currency</param>
        /// <param name="TargetCurrency">Target currency</param>
        /// <param name="Language">Language</param>
        /// <param name="priceIncludesTax">A value indicating whether price includes tax</param>
        /// <param name="ShowTax">A value indicating whether to show tax suffix</param>
        /// <returns>Price</returns>
        public static string FormatPrice(decimal Price, bool ShowCurrency, Currency TargetCurrency,
            Language Language, bool priceIncludesTax, bool ShowTax)
        {
            string currencyString = LocalizationManager.GetCurrencyString(Price, ShowCurrency, TargetCurrency);

            if (ShowTax)
            {
                string formatStr = string.Empty;
                if (priceIncludesTax)
                {
                    formatStr = LocalizationManager.GetLocaleResourceString("Products.InclTaxSuffix", Language.LanguageID, false);
                    if (String.IsNullOrEmpty(formatStr))
                    {
                        formatStr = "{0} incl tax";
                    }
                }
                else
                {
                    formatStr = LocalizationManager.GetLocaleResourceString("Products.ExclTaxSuffix", Language.LanguageID, false);
                    if (String.IsNullOrEmpty(formatStr))
                    {
                        formatStr = "{0} excl tax";
                    }
                }
                string taxString = string.Format(formatStr, currencyString);
                return taxString;
            }
            else
            {
                return currencyString;
            }
        }

        /// <summary>
        /// Formats the price
        /// </summary>
        /// <param name="Price">Price</param>
        /// <param name="ShowCurrency">A value indicating whether to show a currency</param>
        /// <param name="CurrencyCode">Currency code</param>
        /// <param name="Language">Language</param>
        /// <param name="priceIncludesTax">A value indicating whether price includes tax</param>
        /// <returns>Price</returns>
        public static string FormatPrice(decimal Price, bool ShowCurrency, string CurrencyCode, 
            Language Language, bool priceIncludesTax)
        {
            var currency = CurrencyManager.GetCurrencyByCode(CurrencyCode);
            if (currency == null)
            {
                currency = new Currency();
                currency.CurrencyCode = CurrencyCode;
            }
            return FormatPrice(Price, ShowCurrency, currency, Language, priceIncludesTax);
        }



        /// <summary>
        /// Formats the shipping price
        /// </summary>
        /// <param name="Price">Price</param>
        /// <param name="ShowCurrency">A value indicating whether to show a currency</param>
        /// <returns>Price</returns>
        public static string FormatShippingPrice(decimal Price, bool ShowCurrency)
        {
            var TargetCurrency = NopContext.Current.WorkingCurrency;
            var Language = NopContext.Current.WorkingLanguage;
            bool priceIncludesTax = false;
            switch (NopContext.Current.TaxDisplayType)
            {
                case TaxDisplayTypeEnum.ExcludingTax:
                    priceIncludesTax = false;
                    break;
                case TaxDisplayTypeEnum.IncludingTax:
                    priceIncludesTax = true;
                    break;
            }
            return FormatShippingPrice(Price, ShowCurrency, TargetCurrency, Language, priceIncludesTax);
        }

        /// <summary>
        /// Formats the shipping price
        /// </summary>
        /// <param name="Price">Price</param>
        /// <param name="ShowCurrency">A value indicating whether to show a currency</param>
        /// <param name="TargetCurrency">Target currency</param>
        /// <param name="Language">Language</param>
        /// <param name="priceIncludesTax">A value indicating whether price includes tax</param>
        /// <returns>Price</returns>
        public static string FormatShippingPrice(decimal Price, bool ShowCurrency, Currency TargetCurrency,
            Language Language, bool priceIncludesTax)
        {
            bool ShowTax = TaxManager.ShippingIsTaxable && TaxManager.DisplayTaxSuffix;
            return FormatShippingPrice(Price, ShowCurrency, TargetCurrency, Language, priceIncludesTax, ShowTax);
        }

        /// <summary>
        /// Formats the shipping price
        /// </summary>
        /// <param name="Price">Price</param>
        /// <param name="ShowCurrency">A value indicating whether to show a currency</param>
        /// <param name="TargetCurrency">Target currency</param>
        /// <param name="Language">Language</param>
        /// <param name="priceIncludesTax">A value indicating whether price includes tax</param>
        /// <param name="ShowTax">A value indicating whether to show tax suffix</param>
        /// <returns>Price</returns>
        public static string FormatShippingPrice(decimal Price, bool ShowCurrency, Currency TargetCurrency,
            Language Language, bool priceIncludesTax, bool ShowTax)
        {
            return FormatPrice(Price, ShowCurrency, TargetCurrency, Language, priceIncludesTax, ShowTax);
        }
        
        /// <summary>
        /// Formats the shipping price
        /// </summary>
        /// <param name="Price">Price</param>
        /// <param name="ShowCurrency">A value indicating whether to show a currency</param>
        /// <param name="CurrencyCode">Currency code</param>
        /// <param name="Language">Language</param>
        /// <param name="priceIncludesTax">A value indicating whether price includes tax</param>
        /// <returns>Price</returns>
        public static string FormatShippingPrice(decimal Price, bool ShowCurrency, string CurrencyCode, 
            Language Language, bool priceIncludesTax)
        {
            var currency = CurrencyManager.GetCurrencyByCode(CurrencyCode);
            if (currency == null)
            {
                currency = new Currency();
                currency.CurrencyCode = CurrencyCode;
            }
            return FormatShippingPrice(Price, ShowCurrency, currency, Language, priceIncludesTax);
        }

        /// <summary>
        /// Formats the payment method additional fee
        /// </summary>
        /// <param name="Price">Price</param>
        /// <param name="ShowCurrency">A value indicating whether to show a currency</param>
        /// <returns>Price</returns>
        public static string FormatPaymentMethodAdditionalFee(decimal Price, bool ShowCurrency)
        {
            var TargetCurrency = NopContext.Current.WorkingCurrency;
            var Language = NopContext.Current.WorkingLanguage;
            bool priceIncludesTax = false;
            switch (NopContext.Current.TaxDisplayType)
            {
                case TaxDisplayTypeEnum.ExcludingTax:
                    priceIncludesTax = false;
                    break;
                case TaxDisplayTypeEnum.IncludingTax:
                    priceIncludesTax = true;
                    break;
            }
            return FormatPaymentMethodAdditionalFee(Price, ShowCurrency, TargetCurrency, Language, priceIncludesTax);
        }

        /// <summary>
        /// Formats the payment method additional fee
        /// </summary>
        /// <param name="Price">Price</param>
        /// <param name="ShowCurrency">A value indicating whether to show a currency</param>
        /// <param name="TargetCurrency">Target currency</param>
        /// <param name="Language">Language</param>
        /// <param name="priceIncludesTax">A value indicating whether price includes tax</param>
        /// <returns>Price</returns>
        public static string FormatPaymentMethodAdditionalFee(decimal Price, bool ShowCurrency,
            Currency TargetCurrency, Language Language, bool priceIncludesTax)
        {
            bool ShowTax = TaxManager.PaymentMethodAdditionalFeeIsTaxable && TaxManager.DisplayTaxSuffix;
            return FormatPaymentMethodAdditionalFee(Price, ShowCurrency, TargetCurrency, Language, priceIncludesTax, ShowTax);
        }

        /// <summary>
        /// Formats the payment method additional fee
        /// </summary>
        /// <param name="Price">Price</param>
        /// <param name="ShowCurrency">A value indicating whether to show a currency</param>
        /// <param name="TargetCurrency">Target currency</param>
        /// <param name="Language">Language</param>
        /// <param name="priceIncludesTax">A value indicating whether price includes tax</param>
        /// <param name="ShowTax">A value indicating whether to show tax suffix</param>
        /// <returns>Price</returns>
        public static string FormatPaymentMethodAdditionalFee(decimal Price, bool ShowCurrency, 
            Currency TargetCurrency, Language Language, bool priceIncludesTax, bool ShowTax)
        {
            return FormatPrice(Price, ShowCurrency, TargetCurrency, Language, priceIncludesTax, ShowTax);
        }

        /// <summary>
        /// Formats the payment method additional fee
        /// </summary>
        /// <param name="Price">Price</param>
        /// <param name="ShowCurrency">A value indicating whether to show a currency</param>
        /// <param name="CurrencyCode">Currency code</param>
        /// <param name="Language">Language</param>
        /// <param name="priceIncludesTax">A value indicating whether price includes tax</param>
        /// <returns>Price</returns>
        public static string FormatPaymentMethodAdditionalFee(decimal Price, bool ShowCurrency, 
            string CurrencyCode, Language Language, bool priceIncludesTax)
        {
            var currency = CurrencyManager.GetCurrencyByCode(CurrencyCode);
            if (currency == null)
            {
                currency = new Currency();
                currency.CurrencyCode = CurrencyCode;
            }
            return FormatPaymentMethodAdditionalFee(Price, ShowCurrency, currency, Language, priceIncludesTax);
        }

        #endregion

        #endregion
    }
}
