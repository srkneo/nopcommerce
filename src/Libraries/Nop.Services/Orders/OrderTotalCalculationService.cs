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
using System.Globalization;
using System.Linq;
using System.Text;
using Nop.Core.Domain.Customers;
using Nop.Core.Domain.Discounts;
using Nop.Core.Domain.Orders;
using Nop.Core;
using Nop.Core.Domain.Tax;
using Nop.Services.Catalog;
using Nop.Services.Customers;
using Nop.Services.Tax;
using Nop.Services.Discounts;
using Nop.Services.Shipping;
using Nop.Services.Payments;
using Nop.Core.Domain.Shipping;
using Nop.Core.Domain.Common;

namespace Nop.Services.Orders
{
    /// <summary>
    /// Order service
    /// </summary>
    public partial class OrderTotalCalculationService : IOrderTotalCalculationService
    {
        #region Fields

        private readonly IWorkContext _workContext;
        private readonly IPriceCalculationService _priceCalculationService;
        private readonly ITaxService _taxService;
        private readonly IShippingService _shippingService;
        private readonly IPaymentService _paymentService;
        private readonly ICheckoutAttributeParser _checkoutAttributeParser;
        private readonly IDiscountService _discountService;
        private readonly IGiftCardService _giftCardService;
        private readonly TaxSettings _taxSettings;
        private readonly RewardPointsSettings _rewardPointsSettings;
        #endregion

        #region Ctor

        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="workContext">Work context</param>
        /// <param name="priceCalculationService">Price calculation service</param>
        /// <param name="taxService">Tax service</param>
        /// <param name="shippingService">Shipping service</param>
        /// <param name="paymentService">Payment service</param>
        /// <param name="checkoutAttributeParser">Checkout attribute parser</param>
        /// <param name="discountService">Discount service</param>
        /// <param name="giftCardService">Gift card service</param>
        /// <param name="taxSettings">Tax settings</param>
        /// <param name="rewardPointsSettings">Reward points settings</param>
        public OrderTotalCalculationService(IWorkContext workContext,
            IPriceCalculationService priceCalculationService,
            ITaxService taxService,
            IShippingService shippingService,
            IPaymentService paymentService,
            ICheckoutAttributeParser checkoutAttributeParser,
            IDiscountService discountService,
            IGiftCardService giftCardService,
            TaxSettings taxSettings,
            RewardPointsSettings rewardPointsSettings)
        {
            this._workContext = workContext;
            this._priceCalculationService = priceCalculationService;
            this._taxService = taxService;
            this._shippingService = shippingService;
            this._paymentService = paymentService;
            this._checkoutAttributeParser = checkoutAttributeParser;
            this._discountService = discountService;
            this._giftCardService = giftCardService;
            this._taxSettings = taxSettings;
            this._rewardPointsSettings = rewardPointsSettings;
        }

        #endregion
        
        #region Methods

        /// <summary>
        /// Gets shopping cart subtotal
        /// </summary>
        /// <param name="cart">Cart</param>
        /// <param name="discountAmount">Applied discount amount</param>
        /// <param name="appliedDiscount">Applied discount</param>
        /// <param name="subTotalWithoutDiscount">Sub total (without discount)</param>
        /// <param name="subTotalWithDiscount">Sub total (with discount)</param>
        public void GetShoppingCartSubTotal(IList<ShoppingCartItem> cart,
            out decimal discountAmount, out Discount appliedDiscount,
            out decimal subTotalWithoutDiscount, out decimal subTotalWithDiscount)
        {
            bool includingTax = false;
            switch (_workContext.TaxDisplayType)
            {
                case TaxDisplayType.ExcludingTax:
                    includingTax = false;
                    break;
                case TaxDisplayType.IncludingTax:
                    includingTax = true;
                    break;
            }
            GetShoppingCartSubTotal(cart, includingTax,
                out discountAmount, out appliedDiscount,
                out subTotalWithoutDiscount, out subTotalWithDiscount);
        }

        /// <summary>
        /// Gets shopping cart subtotal
        /// </summary>
        /// <param name="cart">Cart</param>
        /// <param name="includingTax">A value indicating whether calculated price should include tax</param>
        /// <param name="discountAmount">Applied discount amount</param>
        /// <param name="appliedDiscount">Applied discount</param>
        /// <param name="subTotalWithoutDiscount">Sub total (without discount)</param>
        /// <param name="subTotalWithDiscount">Sub total (with discount)</param>
        public void GetShoppingCartSubTotal(IList<ShoppingCartItem> cart, 
            bool includingTax,
            out decimal discountAmount, out Discount appliedDiscount,
            out decimal subTotalWithoutDiscount, out decimal subTotalWithDiscount)
        {
            SortedDictionary<decimal, decimal> taxRates = null;
            GetShoppingCartSubTotal(cart, includingTax, 
                out discountAmount, out appliedDiscount,
                out subTotalWithoutDiscount, out subTotalWithDiscount, out taxRates);
        }

        /// <summary>
        /// Gets shopping cart subtotal
        /// </summary>
        /// <param name="cart">Cart</param>
        /// <param name="includingTax">A value indicating whether calculated price should include tax</param>
        /// <param name="discountAmount">Applied discount amount</param>
        /// <param name="appliedDiscount">Applied discount</param>
        /// <param name="subTotalWithoutDiscount">Sub total (without discount)</param>
        /// <param name="subTotalWithDiscount">Sub total (with discount)</param>
        /// <param name="taxRates">Tax rates (of order sub total)</param>
        public void GetShoppingCartSubTotal(IList<ShoppingCartItem> cart,
            bool includingTax,
            out decimal discountAmount, out Discount appliedDiscount,
            out decimal subTotalWithoutDiscount, out decimal subTotalWithDiscount,
            out SortedDictionary<decimal, decimal> taxRates)
        {
            discountAmount = decimal.Zero;
            appliedDiscount = null;
            subTotalWithoutDiscount = decimal.Zero;
            subTotalWithDiscount = decimal.Zero;
            taxRates = new SortedDictionary<decimal, decimal>();

            if (cart.Count == 0)
                return;

            //get the customer 
            Customer customer = cart.GetCustomer();
            
            //sub totals
            decimal subTotalExclTaxWithoutDiscount = decimal.Zero;
            decimal subTotalInclTaxWithoutDiscount = decimal.Zero;
            foreach (var shoppingCartItem in cart)
            {
                decimal taxRate = decimal.Zero;
                decimal sciSubTotal = _priceCalculationService.GetSubTotal(shoppingCartItem, true);

                decimal sciExclTax = _taxService.GetProductPrice(shoppingCartItem.ProductVariant, sciSubTotal, false, customer, out taxRate);
                decimal sciInclTax = _taxService.GetProductPrice(shoppingCartItem.ProductVariant, sciSubTotal, true, customer, out taxRate);
                subTotalExclTaxWithoutDiscount += sciExclTax;
                subTotalInclTaxWithoutDiscount += sciInclTax;
                
                //tax rates
                decimal sciTax = sciInclTax - sciExclTax;
                if (taxRate > decimal.Zero && sciTax > decimal.Zero)
                {
                    if (!taxRates.ContainsKey(taxRate))
                    {
                        taxRates.Add(taxRate, sciTax);
                    }
                    else
                    {
                        taxRates[taxRate] = taxRates[taxRate] + sciTax;
                    }
                }
            }

            //checkout attributes
            if (customer != null)
            {
                var caValues = _checkoutAttributeParser.ParseCheckoutAttributeValues(customer.CheckoutAttributes);
                foreach (var caValue in caValues)
                {
                    decimal taxRate = decimal.Zero;

                    decimal caExclTax = _taxService.GetCheckoutAttributePrice(caValue, false, customer, out taxRate);
                    decimal caInclTax = _taxService.GetCheckoutAttributePrice(caValue, true, customer, out taxRate);
                    subTotalExclTaxWithoutDiscount += caExclTax;
                    subTotalInclTaxWithoutDiscount += caInclTax;
                    
                    //tax rates
                    decimal caTax = caInclTax - caExclTax;
                    if (taxRate > decimal.Zero && caTax > decimal.Zero)
                    {
                        if (!taxRates.ContainsKey(taxRate))
                        {
                            taxRates.Add(taxRate, caTax);
                        }
                        else
                        {
                            taxRates[taxRate] = taxRates[taxRate] + caTax;
                        }
                    }
                }
            }

            //subtotal without discount
            if (includingTax)
                subTotalWithoutDiscount = subTotalInclTaxWithoutDiscount;
            else
                subTotalWithoutDiscount = subTotalExclTaxWithoutDiscount;
            if (subTotalWithoutDiscount < decimal.Zero)
                subTotalWithoutDiscount = decimal.Zero;
            //subTotalWithoutDiscount = Math.Round(subTotalWithoutDiscount, 2);

            /*We calculate discount amount on order subtotal excl tax (discount first)*/
            //calculate discount amount ('Applied to order subtotal' discount)
            decimal discountAmountExclTax = GetOrderSubtotalDiscount(customer, subTotalExclTaxWithoutDiscount, out appliedDiscount);
            if (subTotalExclTaxWithoutDiscount < discountAmountExclTax)
                discountAmountExclTax = subTotalExclTaxWithoutDiscount;
            decimal discountAmountInclTax = discountAmountExclTax;
            //subtotal with discount (excl tax)
            decimal subTotalExclTaxWithDiscount = subTotalExclTaxWithoutDiscount - discountAmountExclTax;
            decimal subTotalInclTaxWithDiscount = subTotalExclTaxWithDiscount;

            //add tax for shopping items & checkout attributes
            Dictionary<decimal, decimal> tempTaxRates = new Dictionary<decimal, decimal>(taxRates);
            foreach (KeyValuePair<decimal, decimal> kvp in tempTaxRates)
            {
                decimal taxRate = kvp.Key;
                decimal taxValue = kvp.Value;

                if (taxValue != decimal.Zero)
                {
                    //discount the tax amount that applies to subtotal items
                    if (subTotalExclTaxWithoutDiscount > decimal.Zero)
                    {
                        decimal discountTax = taxRates[taxRate] * (discountAmountExclTax / subTotalExclTaxWithoutDiscount);
                        discountAmountInclTax += discountTax;
                        taxValue = taxRates[taxRate] - discountTax;
                        //taxValue = Math.Round(taxValue, 2);
                        taxRates[taxRate] = taxValue;
                    }

                    //subtotal with discount (incl tax)
                    subTotalInclTaxWithDiscount += taxValue;
                }
            }
            //discountAmountInclTax = Math.Round(discountAmountInclTax, 2);

            if (includingTax)
            {
                subTotalWithDiscount = subTotalInclTaxWithDiscount;
                discountAmount = discountAmountInclTax;
            }
            else
            {
                subTotalWithDiscount = subTotalExclTaxWithDiscount;
                discountAmount = discountAmountExclTax;
            }

            //round
            if (subTotalWithDiscount < decimal.Zero)
                subTotalWithDiscount = decimal.Zero;
            //subTotalWithDiscount = Math.Round(subTotalWithDiscount, 2);
        }

        /// <summary>
        /// Gets an order discount (applied to order subtotal)
        /// </summary>
        /// <param name="customer">Customer</param>
        /// <param name="orderSubTotal">Order subtotal</param>
        /// <param name="appliedDiscount">Applied discount</param>
        /// <returns>Order discount</returns>
        public decimal GetOrderSubtotalDiscount(Customer customer,
            decimal orderSubTotal, out Discount appliedDiscount)
        {
            decimal discountAmount = decimal.Zero;

            var allDiscounts = _discountService.GetAllDiscounts(DiscountType.AssignedToOrderSubTotal);
            var allowedDiscounts = new List<Discount>();
            foreach (var discount in allDiscounts)
            {
                if (_discountService.IsDiscountValid(discount, customer) &&
                           discount.DiscountType == DiscountType.AssignedToOrderSubTotal &&
                           !allowedDiscounts.Contains(discount))
                {
                    allowedDiscounts.Add(discount);
                }
            }

            appliedDiscount = _discountService.GetPreferredDiscount(allowedDiscounts, orderSubTotal);
            if (appliedDiscount != null)
                discountAmount = appliedDiscount.GetDiscountAmount(orderSubTotal);

            if (discountAmount < decimal.Zero)
                discountAmount = decimal.Zero;

            return discountAmount;
        }





        /// <summary>
        /// Gets shopping cart shipping total
        /// </summary>
        /// <param name="cart">Cart</param>
        /// <returns>Shipping total</returns>
        public decimal? GetShoppingCartShippingTotal(IList<ShoppingCartItem> cart)
        {
            bool includingTax = false;
            switch (_workContext.TaxDisplayType)
            {
                case TaxDisplayType.ExcludingTax:
                    includingTax = false;
                    break;
                case TaxDisplayType.IncludingTax:
                    includingTax = true;
                    break;
            }
            return GetShoppingCartShippingTotal(cart, includingTax);
        }

        /// <summary>
        /// Gets shopping cart shipping total
        /// </summary>
        /// <param name="cart">Cart</param>
        /// <param name="includingTax">A value indicating whether calculated price should include tax</param>
        /// <returns>Shipping total</returns>
        public decimal? GetShoppingCartShippingTotal(IList<ShoppingCartItem> cart, bool includingTax)
        {
            decimal taxRate = decimal.Zero;
            return GetShoppingCartShippingTotal(cart, includingTax, out taxRate);
        }

        /// <summary>
        /// Gets shopping cart shipping total
        /// </summary>
        /// <param name="cart">Cart</param>
        /// <param name="includingTax">A value indicating whether calculated price should include tax</param>
        /// <param name="taxRate">Applied tax rate</param>
        /// <returns>Shipping total</returns>
        public decimal? GetShoppingCartShippingTotal(IList<ShoppingCartItem> cart, bool includingTax,
            out decimal taxRate)
        {
            Discount appliedDiscount = null;
            return GetShoppingCartShippingTotal(cart, includingTax, out taxRate, out appliedDiscount);
        }

        /// <summary>
        /// Gets shopping cart shipping total
        /// </summary>
        /// <param name="cart">Cart</param>
        /// <param name="includingTax">A value indicating whether calculated price should include tax</param>
        /// <param name="taxRate">Applied tax rate</param>
        /// <param name="appliedDiscount">Applied discount</param>
        /// <returns>Shipping total</returns>
        public decimal? GetShoppingCartShippingTotal(IList<ShoppingCartItem> cart, bool includingTax,
            out decimal taxRate, out Discount appliedDiscount)
        {
            decimal? shippingTotalWithoutDiscount = null;
            decimal? shippingTotalWithDiscount = null;
            decimal? shippingTotalWithDiscountTaxed = null;
            appliedDiscount = null;
            taxRate = decimal.Zero;

            var customer = cart.GetCustomer();

            bool isFreeShipping = _shippingService.IsFreeShipping(cart);
            if (isFreeShipping)
                return decimal.Zero;

            ShippingOption lastShippingOption = null;
            if (customer != null)
                lastShippingOption = customer.GetAttribute<ShippingOption>("LastShippingOption");

            if (lastShippingOption != null)
            {
                //use last shipping option (get from cache)
                //we have already discounted cache value
                shippingTotalWithoutDiscount = lastShippingOption.Rate;

                //discount
                decimal discountAmount = GetShippingDiscount(customer,
                    shippingTotalWithoutDiscount.Value, out appliedDiscount);
                shippingTotalWithDiscount = shippingTotalWithoutDiscount - discountAmount;
                if (shippingTotalWithDiscount < decimal.Zero)
                    shippingTotalWithDiscount = decimal.Zero;
                //shippingTotalWithDiscount = Math.Round(shippingTotalWithDiscount.Value, 2);
            }
            else
            {
                //use fixed rate (if possible)
                Address shippingAddress = null;
                if (customer != null)
                    shippingAddress = customer.ShippingAddress;

                var shippingRateComputationMethods = _shippingService.LoadActiveShippingRateComputationMethods();
                if (shippingRateComputationMethods.Count == 0)
                    throw new NopException("Shipping rate computation method could not be loaded");

                if (shippingRateComputationMethods.Count == 1)
                {
                    var getShippingOptionRequest = _shippingService.CreateShippingOptionRequest(cart, shippingAddress);

                    var shippingRateComputationMethod = shippingRateComputationMethods[0];
                    decimal? fixedRate = shippingRateComputationMethod.GetFixedRate(getShippingOptionRequest);
                    if (fixedRate.HasValue)
                    {
                        decimal additionalShippingCharge = _shippingService.GetShoppingCartAdditionalShippingCharge(cart);
                        shippingTotalWithoutDiscount = fixedRate.Value + additionalShippingCharge;
                        //shippingTotalWithoutDiscount = Math.Round(shippingTotalWithoutDiscount.Value, 2);
                        decimal shippingTotalDiscount = GetShippingDiscount(customer, shippingTotalWithoutDiscount.Value, out appliedDiscount);
                        shippingTotalWithDiscount = shippingTotalWithoutDiscount.Value - shippingTotalDiscount;
                        if (shippingTotalWithDiscount.Value < decimal.Zero)
                            shippingTotalWithDiscount = decimal.Zero;
                    }
                }
            }

            if (shippingTotalWithDiscount.HasValue)
            {
                shippingTotalWithDiscountTaxed = _taxService.GetShippingPrice(shippingTotalWithDiscount.Value,
                    includingTax,
                    customer,
                    out taxRate);

                //shippingTotalWithDiscountTaxed = Math.Round(shippingTotalWithDiscountTaxed.Value, 2);
            }

            return shippingTotalWithDiscountTaxed;
        }

        /// <summary>
        /// Gets a shipping discount
        /// </summary>
        /// <param name="customer">Customer</param>
        /// <param name="shippingTotal">Shipping total</param>
        /// <param name="appliedDiscount">Applied discount</param>
        /// <returns>Shipping discount</returns>
        public decimal GetShippingDiscount(Customer customer, decimal shippingTotal, out Discount appliedDiscount)
        {
            decimal shippingDiscountAmount = decimal.Zero;

            var allDiscounts = _discountService.GetAllDiscounts(DiscountType.AssignedToShipping);
            var allowedDiscounts = new List<Discount>();
            foreach (var discount in allDiscounts)
            {
                if (_discountService.IsDiscountValid(discount, customer) &&
                           discount.DiscountType == DiscountType.AssignedToShipping &&
                           !allowedDiscounts.Contains(discount))
                {
                    allowedDiscounts.Add(discount);
                }
            }

            appliedDiscount = _discountService.GetPreferredDiscount(allowedDiscounts, shippingTotal);
            if (appliedDiscount != null)
            {
                shippingDiscountAmount = appliedDiscount.GetDiscountAmount(shippingTotal);
            }

            if (shippingDiscountAmount < decimal.Zero)
                shippingDiscountAmount = decimal.Zero;

            //shippingDiscountAmount = Math.Round(shippingDiscountAmount, 2);

            return shippingDiscountAmount;
        }
        





        /// <summary>
        /// Gets tax
        /// </summary>
        /// <param name="cart">Shopping cart</param>
        /// <param name="paymentMethodSystemName">Payment method identifier</param>
        /// <returns>Tax total</returns>
        public decimal GetTaxTotal(IList<ShoppingCartItem> cart, string paymentMethodSystemName = "")
        {
            //TODO don't pass 'paymentMethodSystemName' param (we can get it from 'customer' entity)
            SortedDictionary<decimal, decimal> taxRates = null;
            return GetTaxTotal(cart, paymentMethodSystemName, out taxRates);
        }

        /// <summary>
        /// Gets tax
        /// </summary>
        /// <param name="cart">Shopping cart</param>
        /// <param name="paymentMethodSystemName">Payment method identifier</param>
        /// <param name="taxRates">Tax rates</param>
        /// <returns>Tax total</returns>
        public decimal GetTaxTotal(IList<ShoppingCartItem> cart, string paymentMethodSystemName,
            out SortedDictionary<decimal, decimal> taxRates)
        {
            //TODO don't pass 'paymentMethodSystemName' param (we can get it from 'customer' entity)
            taxRates = new SortedDictionary<decimal, decimal>();

            var customer = cart.GetCustomer();

            //order sub total (items + checkout attributes)
            decimal subTotalTaxTotal = decimal.Zero;
            decimal orderSubTotalDiscountAmount = decimal.Zero;
            Discount orderSubTotalAppliedDiscount = null;
            decimal subTotalWithoutDiscountBase = decimal.Zero;
            decimal subTotalWithDiscountBase = decimal.Zero;
            SortedDictionary<decimal, decimal> orderSubTotalTaxRates = null;
            GetShoppingCartSubTotal(cart,  false, 
                out orderSubTotalDiscountAmount, out orderSubTotalAppliedDiscount,
                out subTotalWithoutDiscountBase, out subTotalWithDiscountBase,
                out orderSubTotalTaxRates);
            foreach (KeyValuePair<decimal, decimal> kvp in orderSubTotalTaxRates)
            {
                decimal taxRate = kvp.Key;
                decimal taxValue = kvp.Value;
                subTotalTaxTotal += taxValue;

                if (taxRate > decimal.Zero && taxValue > decimal.Zero)
                {
                    if (!taxRates.ContainsKey(taxRate))
                        taxRates.Add(taxRate, taxValue);
                    else
                        taxRates[taxRate] = taxRates[taxRate] + taxValue;
                }
            }

            //shipping
            decimal shippingTax = decimal.Zero;
            if (_taxSettings.ShippingIsTaxable)
            {
                decimal taxRate = decimal.Zero;
                decimal? shippingExclTax = GetShoppingCartShippingTotal(cart, false, out taxRate);
                decimal? shippingInclTax = GetShoppingCartShippingTotal(cart, true, out taxRate);
                if (shippingExclTax.HasValue && shippingInclTax.HasValue)
                {
                    shippingTax = shippingInclTax.Value - shippingExclTax.Value;

                    //tax rates
                    if (taxRate > decimal.Zero && shippingTax > decimal.Zero)
                    {
                        if (!taxRates.ContainsKey(taxRate))
                            taxRates.Add(taxRate, shippingTax);
                        else
                            taxRates[taxRate] = taxRates[taxRate] + shippingTax;
                    }
                }
            }

            //payment method additional fee
            decimal paymentMethodAdditionalFeeTax = decimal.Zero;
            if (_taxSettings.PaymentMethodAdditionalFeeIsTaxable)
            {
                decimal taxRate = decimal.Zero;

                decimal paymentMethodAdditionalFee = _paymentService.GetAdditionalHandlingFee(paymentMethodSystemName);
                decimal paymentMethodAdditionalFeeExclTax = _taxService.GetPaymentMethodAdditionalFee(paymentMethodAdditionalFee, false, customer, out taxRate);
                decimal paymentMethodAdditionalFeeInclTax = _taxService.GetPaymentMethodAdditionalFee(paymentMethodAdditionalFee, true, customer, out taxRate);
                
                paymentMethodAdditionalFeeTax = paymentMethodAdditionalFeeInclTax - paymentMethodAdditionalFeeExclTax;

                //tax rates
                if (taxRate > decimal.Zero && paymentMethodAdditionalFeeTax > decimal.Zero)
                {
                    if (!taxRates.ContainsKey(taxRate))
                        taxRates.Add(taxRate, paymentMethodAdditionalFeeTax);
                    else
                        taxRates[taxRate] = taxRates[taxRate] + paymentMethodAdditionalFeeTax;
                }
            }

            //add at least one tax rate (0%)
            if (taxRates.Count == 0)
                taxRates.Add(decimal.Zero, decimal.Zero);

            //summarize taxes
            decimal taxTotal = subTotalTaxTotal + shippingTax + paymentMethodAdditionalFeeTax;
            //taxTotal = Math.Round(taxTotal, 2);
            return taxTotal;
        }





        /// <summary>
        /// Gets shopping cart total
        /// </summary>
        /// <param name="cart">Cart</param>
        /// <param name="paymentMethodSystemName">Payment method identifier</param>
        /// <returns>Shopping cart total;Null if shopping cart total couldn't be calculated now</returns>
        public decimal? GetShoppingCartTotal(IList<ShoppingCartItem> cart,
            string paymentMethodSystemName)
        {
            decimal discountAmount = decimal.Zero;
            Discount appliedDiscount = null;
            
            int redeemedRewardPoints = 0;
            decimal redeemedRewardPointsAmount = decimal.Zero;
            List<AppliedGiftCard> appliedGiftCards = null;
            return GetShoppingCartTotal(cart, paymentMethodSystemName,
                out discountAmount, out appliedDiscount,
                out appliedGiftCards,
                out redeemedRewardPoints, out redeemedRewardPointsAmount);
        }

        /// <summary>
        /// Gets shopping cart total
        /// </summary>
        /// <param name="cart">Cart</param>
        /// <param name="paymentMethodSystemName">Payment method identifier</param>
        /// <param name="appliedGiftCards">Applied gift cards</param>
        /// <param name="discountAmount">Applied discount amount</param>
        /// <param name="appliedDiscount">Applied discount</param>
        /// <param name="redeemedRewardPoints">Reward points to redeem</param>
        /// <param name="redeemedRewardPointsAmount">Reward points amount in primary store currency to redeem</param>
        /// <returns>Shopping cart total;Null if shopping cart total couldn't be calculated now</returns>
        public decimal? GetShoppingCartTotal(IList<ShoppingCartItem> cart,
            string paymentMethodSystemName,
            out decimal discountAmount, out Discount appliedDiscount,
            out List<AppliedGiftCard> appliedGiftCards,
            out int redeemedRewardPoints, out decimal redeemedRewardPointsAmount)
        {
            redeemedRewardPoints = 0;
            redeemedRewardPointsAmount = decimal.Zero;

            var customer = cart.GetCustomer();

            //subtotal without tax
            decimal subtotalBase = decimal.Zero;
            decimal orderSubTotalDiscountAmount = decimal.Zero;
            Discount orderSubTotalAppliedDiscount = null;
            decimal subTotalWithoutDiscountBase = decimal.Zero;
            decimal subTotalWithDiscountBase = decimal.Zero;
            GetShoppingCartSubTotal(cart, false,
                out orderSubTotalDiscountAmount, out orderSubTotalAppliedDiscount,
                out subTotalWithoutDiscountBase, out subTotalWithDiscountBase);
            //subtotal with discount
            subtotalBase = subTotalWithDiscountBase;



            //shipping without tax
            decimal? shoppingCartShipping = GetShoppingCartShippingTotal(cart, false);



            //payment method additional fee without tax
            decimal paymentMethodAdditionalFeeWithoutTax = decimal.Zero;
            if (String.IsNullOrEmpty(paymentMethodSystemName))
            {
                decimal paymentMethodAdditionalFee = _paymentService.GetAdditionalHandlingFee(paymentMethodSystemName);
                paymentMethodAdditionalFeeWithoutTax = _taxService.GetPaymentMethodAdditionalFee(paymentMethodAdditionalFee,
                    false, customer);
            }




            //tax
            decimal shoppingCartTax = GetTaxTotal(cart, paymentMethodSystemName);




            //order total
            decimal resultTemp = decimal.Zero;
            resultTemp += subtotalBase;
            if (shoppingCartShipping.HasValue)
            {
                resultTemp += shoppingCartShipping.Value;
            }
            resultTemp += paymentMethodAdditionalFeeWithoutTax;
            resultTemp += shoppingCartTax;
            //resultTemp = Math.Round(resultTemp, 2);

            #region Order total discount

            discountAmount = GetOrderTotalDiscount(customer, resultTemp, out appliedDiscount);

            //sub totals with discount        
            if (resultTemp < discountAmount)
                discountAmount = resultTemp;

            //reduce subtotal
            resultTemp -= discountAmount;

            if (resultTemp < decimal.Zero)
                resultTemp = decimal.Zero;
            //resultTemp = Math.Round(resultTemp, 2);

            #endregion

            #region Applied gift cards

            //let's apply gift cards now (gift cards that can be used)
            appliedGiftCards = new List<AppliedGiftCard>();
            if (!cart.IsRecurring())
            {
                //we don't apply gift cards for recurring products
                var giftCards = _giftCardService.GetActiveGiftCardsAppliedByCustomer(customer);
                foreach (var gc in giftCards)
                {
                    if (resultTemp > decimal.Zero)
                    {
                        decimal remainingAmount = gc.GetGiftCardRemainingAmount();
                        decimal amountCanBeUsed = decimal.Zero;
                        if (resultTemp > remainingAmount)
                            amountCanBeUsed = remainingAmount;
                        else
                            amountCanBeUsed = resultTemp;

                        //reduce subtotal
                        resultTemp -= amountCanBeUsed;

                        var appliedGiftCard = new AppliedGiftCard();
                        appliedGiftCard.GiftCard = gc;
                        appliedGiftCard.AmountCanBeUsed = amountCanBeUsed;
                        appliedGiftCards.Add(appliedGiftCard);
                    }
                }
            }

            #endregion

            if (resultTemp < decimal.Zero)
                resultTemp = decimal.Zero;
            //resultTemp = Math.Round(resultTemp, 2);

            decimal? orderTotal = null;
            if (!shoppingCartShipping.HasValue)
            {
                //return null if we have errors
                orderTotal = null;
                return orderTotal;
            }
            else
            {
                //return result if we have no errors
                orderTotal = resultTemp;
            }

            #region Reward points

            bool useRewardPoints = false;
            if (customer != null)
                useRewardPoints = customer.UseRewardPointsDuringCheckout;
            
            if (_rewardPointsSettings.RewardPointsEnabled && useRewardPoints && customer != null)
            {
                int rewardPointsBalance = customer.GetRewardPointsBalance();
                decimal rewardPointsBalanceAmount = ConvertRewardPointsToAmount(rewardPointsBalance);
                if (orderTotal.HasValue && orderTotal.Value > decimal.Zero)
                {
                    if (orderTotal.Value > rewardPointsBalanceAmount)
                    {
                        redeemedRewardPoints = rewardPointsBalance;
                        redeemedRewardPointsAmount = rewardPointsBalanceAmount;
                    }
                    else
                    {
                        redeemedRewardPointsAmount = orderTotal.Value;
                        redeemedRewardPoints = ConvertAmountToRewardPoints(redeemedRewardPointsAmount);
                    }
                }
            }
            #endregion

            if (orderTotal.HasValue)
            {
                orderTotal = orderTotal.Value - redeemedRewardPointsAmount;
                //orderTotal = Math.Round(orderTotal.Value, 2);
                return orderTotal;
            }
            else
                return null;
        }

        /// <summary>
        /// Gets an order discount (applied to order total)
        /// </summary>
        /// <param name="customer">Customer</param>
        /// <param name="orderTotal">Order total</param>
        /// <param name="appliedDiscount">Applied discount</param>
        /// <returns>Order discount</returns>
        public decimal GetOrderTotalDiscount(Customer customer, decimal orderTotal, out Discount appliedDiscount)
        {
            decimal discountAmount = decimal.Zero;

            var allDiscounts = _discountService.GetAllDiscounts(DiscountType.AssignedToOrderTotal);
            var allowedDiscounts = new List<Discount>();
            foreach (var discount in allDiscounts)
            {
                if (_discountService.IsDiscountValid(discount, customer) &&
                           discount.DiscountType == DiscountType.AssignedToOrderTotal &&
                           !allowedDiscounts.Contains(discount))
                {
                    allowedDiscounts.Add(discount);
                }
            }

            appliedDiscount = _discountService.GetPreferredDiscount(allowedDiscounts, orderTotal);
            if (appliedDiscount != null)
                discountAmount = appliedDiscount.GetDiscountAmount(orderTotal);

            if (discountAmount < decimal.Zero)
                discountAmount = decimal.Zero;

            //discountAmount = Math.Round(discountAmount, 2);

            return discountAmount;
        }





        /// <summary>
        /// Converts reward points to amount primary store currency
        /// </summary>
        /// <param name="rewardPoints">Reward points</param>
        /// <returns>Converted value</returns>
        public decimal ConvertRewardPointsToAmount(int rewardPoints)
        {
            decimal result = decimal.Zero;
            if (rewardPoints <= 0)
                return decimal.Zero;

            result = rewardPoints * _rewardPointsSettings.RewardPointsExchangeRate;
            result = Math.Round(result, 2);
            return result;
        }

        /// <summary>
        /// Converts an amount in primary store currency to reward points
        /// </summary>
        /// <param name="amount">Amount</param>
        /// <returns>Converted value</returns>
        public int ConvertAmountToRewardPoints(decimal amount)
        {
            int result = 0;
            if (amount <= 0)
                return 0;

            if (_rewardPointsSettings.RewardPointsExchangeRate > 0)
                result = (int)Math.Ceiling(amount / _rewardPointsSettings.RewardPointsExchangeRate);
            return result;
        }
 
        #endregion
    }
}
