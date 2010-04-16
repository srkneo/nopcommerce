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
using NopSolutions.NopCommerce.BusinessLogic.Configuration.Settings;
using NopSolutions.NopCommerce.BusinessLogic.CustomerManagement;
using NopSolutions.NopCommerce.BusinessLogic.Orders;
using NopSolutions.NopCommerce.BusinessLogic.Products;
using NopSolutions.NopCommerce.BusinessLogic.Products.Attributes;
using NopSolutions.NopCommerce.BusinessLogic.Promo.Discounts;
using NopSolutions.NopCommerce.BusinessLogic.Tax;
using NopSolutions.NopCommerce.Common;


namespace NopSolutions.NopCommerce.BusinessLogic.Shipping
{
    /// <summary>
    /// Shipping manager
    /// </summary>
    public partial class ShippingManager
    {
        #region Utilities

        /// <summary>
        /// Gets a value indicating whether shipping is free
        /// </summary>
        /// <param name="Cart">Cart</param>
        /// <param name="customer">Customer</param>
        /// <returns>A value indicating whether shipping is free</returns>
        protected static bool IsFreeShipping(ShoppingCart Cart, Customer customer)
        {
            if (customer != null)
            {
                var customerRoles = customer.CustomerRoles;
                foreach (var customerRole in customerRoles)
                    if (customerRole.FreeShipping)
                        return true;
            }

            bool shoppingCartRequiresShipping = ShoppingCartRequiresShipping(Cart);
            if (!shoppingCartRequiresShipping)
                return true;

            decimal subTotalDiscountBase = decimal.Zero;
            Discount appliedDiscount = null;
            List<AppliedGiftCard> appliedGiftCards = null;
            decimal subtotalBaseWithoutPromo = decimal.Zero;
            decimal subtotalBaseWithPromo = decimal.Zero;
            string SubTotalError = ShoppingCartManager.GetShoppingCartSubTotal(Cart,
                customer, out subTotalDiscountBase,
                out appliedDiscount, out appliedGiftCards,
                out subtotalBaseWithoutPromo, out subtotalBaseWithPromo);
            if (SettingManager.GetSettingValueBoolean("Shipping.FreeShippingOverX.Enabled"))
            {
                decimal freeShippingOverX = SettingManager.GetSettingValueDecimalNative("Shipping.FreeShippingOverX.Value");
                if (subtotalBaseWithPromo > freeShippingOverX)
                    return true;
            }

            bool allItemsAreFreeShipping = true;
            foreach (var sc in Cart)
                if (!sc.IsFreeShipping)
                {
                    allItemsAreFreeShipping = false;
                    break;
                }
            if (allItemsAreFreeShipping)
                return true;

            return false;
        }

        /// <summary>
        /// Create shipment package from shopping cart
        /// </summary>
        /// <param name="Cart">Shopping cart</param>
        /// <param name="customer">Customer</param>
        /// <param name="ShippingAddress">Shipping address</param>
        /// <returns>Shipment package</returns>
        protected static ShipmentPackage CreateShipmentPackage(ShoppingCart Cart, Customer customer, Address ShippingAddress)
        {
            var shipmentPackage = new ShipmentPackage();
            shipmentPackage.Customer = customer;
            shipmentPackage.Items = new ShoppingCart();
            foreach (var sc in Cart)
                if (sc.IsShipEnabled)
                    shipmentPackage.Items.Add(sc);
            shipmentPackage.ShippingAddress = ShippingAddress;
            //TODO set values from warehouses or shipping origin
            shipmentPackage.CountryFrom = null;
            shipmentPackage.StateProvinceFrom = null;
            shipmentPackage.ZipPostalCodeFrom = string.Empty;
            return shipmentPackage;

        }

        #endregion

        #region Methods
        /// <summary>
        /// Gets shopping cart weight
        /// </summary>
        /// <param name="Cart">Cart</param>
        /// <param name="customer">Customer</param>
        /// <returns>Shopping cart weight</returns>
        public static decimal GetShoppingCartTotalWeigth(ShoppingCart Cart, Customer customer)
        {
            decimal totalWeight = decimal.Zero;
            //shopping cart items
            foreach (var shoppingCartItem in Cart)
                totalWeight += shoppingCartItem.TotalWeigth;

            //checkout attributes
            if (customer != null)
            {
                var caValues = CheckoutAttributeHelper.ParseCheckoutAttributeValues(customer.CheckoutAttributes);
                foreach (var caValue in caValues)
                    totalWeight += caValue.WeightAdjustment;
            }
            return totalWeight;
        }

        /// <summary>
        /// Gets shopping cart shipping total
        /// </summary>
        /// <param name="Cart">Cart</param>
        /// <returns>Shipping total</returns>
        public static decimal? GetShoppingCartShippingTotal(ShoppingCart Cart)
        {
            string Error = string.Empty;
            return GetShoppingCartShippingTotal(Cart, ref Error);
        }

        /// <summary>
        /// Gets shopping cart shipping total
        /// </summary>
        /// <param name="Cart">Cart</param>
        /// <param name="Error">Error</param>
        /// <returns>Shipping total</returns>
        public static decimal? GetShoppingCartShippingTotal(ShoppingCart Cart, ref string Error)
        {
            Customer customer = NopContext.Current.User;
            return GetShoppingCartShippingTotal(Cart, customer, ref Error);
        }

        /// <summary>
        /// Gets shopping cart shipping total
        /// </summary>
        /// <param name="Cart">Cart</param>
        /// <param name="customer">Customer</param>
        /// <returns>Shipping total</returns>
        public static decimal? GetShoppingCartShippingTotal(ShoppingCart Cart, Customer customer)
        {
            string Error = string.Empty;
            return GetShoppingCartShippingTotal(Cart, customer, ref Error);
        }

        /// <summary>
        /// Gets shopping cart shipping total
        /// </summary>
        /// <param name="Cart">Cart</param>
        /// <param name="customer">Customer</param>
        /// <param name="Error">Error</param>
        /// <returns>Shipping total</returns>
        public static decimal? GetShoppingCartShippingTotal(ShoppingCart Cart,
            Customer customer, ref string Error)
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
            return GetShoppingCartShippingTotal(Cart, customer, includingTax, ref Error);
        }

        /// <summary>
        /// Gets shopping cart shipping total
        /// </summary>
        /// <param name="Cart">Cart</param>
        /// <param name="customer">Customer</param>
        /// <param name="includingTax">A value indicating whether calculated price should include tax</param>
        /// <returns>Shipping total</returns>
        public static decimal? GetShoppingCartShippingTotal(ShoppingCart Cart,
            Customer customer, bool includingTax)
        {
            string Error = string.Empty;
            return GetShoppingCartShippingTotal(Cart, customer, includingTax, ref Error);
        }

        /// <summary>
        /// Gets shopping cart shipping total
        /// </summary>
        /// <param name="Cart">Cart</param>
        /// <param name="customer">Customer</param>
        /// <param name="includingTax">A value indicating whether calculated price should include tax</param>
        /// <param name="Error">Error</param>
        /// <returns>Shipping total</returns>
        public static decimal? GetShoppingCartShippingTotal(ShoppingCart Cart, 
            Customer customer, bool includingTax, ref string Error)
        {
            Discount appliedDiscount = null;
            return GetShoppingCartShippingTotal(Cart, customer, includingTax,
                out appliedDiscount, ref Error);
        }

        /// <summary>
        /// Gets shopping cart shipping total
        /// </summary>
        /// <param name="Cart">Cart</param>
        /// <param name="customer">Customer</param>
        /// <param name="includingTax">A value indicating whether calculated price should include tax</param>
        /// <param name="appliedDiscount">Applied discount</param>
        /// <param name="Error">Error</param>
        /// <returns>Shipping total</returns>
        public static decimal? GetShoppingCartShippingTotal(ShoppingCart Cart,
            Customer customer, bool includingTax, out Discount appliedDiscount, ref string Error)
        {
            decimal? shippingTotalWithoutDiscount = null;
            decimal? shippingTotalWithDiscount = null;
            decimal? shippingTotalWithDiscountTaxed = null;
            appliedDiscount = null;

            bool isFreeShipping = IsFreeShipping(Cart, customer);
            if (isFreeShipping)
                return decimal.Zero;

            ShippingOption lastShippingOption = null;
            if (customer != null)
            {
                lastShippingOption = customer.LastShippingOption;
            }

            if (lastShippingOption != null)
            {
                //use last shipping option (get from cache)
                //we have already discounted cache value
                shippingTotalWithDiscount = lastShippingOption.Rate;
                appliedDiscount = DiscountManager.GetDiscountByID(lastShippingOption.AppliedDiscountID);
            }
            else
            {
                //use fixed rate (if possible)
                Address shippingAddress = null;
                if (customer != null)
                {
                    shippingAddress = customer.ShippingAddress;
                }
                var ShipmentPackage = CreateShipmentPackage(Cart, customer, shippingAddress);
                var shippingRateComputationMethods = ShippingRateComputationMethodManager.GetAllShippingRateComputationMethods(false);
                if (shippingRateComputationMethods.Count == 0)
                    throw new NopException("Shipping rate computation method could not be loaded");

                if (shippingRateComputationMethods.Count == 1)
                {
                    var shippingRateComputationMethod = shippingRateComputationMethods[0];
                    var iShippingRateComputationMethod = Activator.CreateInstance(Type.GetType(shippingRateComputationMethod.ClassName)) as IShippingRateComputationMethod;

                    decimal? fixedRate = iShippingRateComputationMethod.GetFixedRate(ShipmentPackage);
                    if (fixedRate.HasValue)
                    {
                        decimal additionalShippingCharge = GetShoppingCartAdditionalShippingCharge(Cart, customer);
                        shippingTotalWithoutDiscount = fixedRate.Value + additionalShippingCharge;
                        shippingTotalWithoutDiscount = Math.Round(shippingTotalWithoutDiscount.Value, 2);
                        decimal shippingTotalDiscount = GetShippingDiscount(customer, shippingTotalWithoutDiscount.Value, out appliedDiscount);
                        shippingTotalWithDiscount = shippingTotalWithoutDiscount.Value - shippingTotalDiscount;
                        if (shippingTotalWithDiscount.Value < decimal.Zero)
                            shippingTotalWithDiscount = decimal.Zero;
                    }
                }
            }

            if (!shippingTotalWithDiscount.HasValue)
            {
                Error = "Shipping total could not be calculated";
            }
            else
            {
                shippingTotalWithDiscountTaxed = TaxManager.GetShippingPrice(shippingTotalWithDiscount.Value,
                    includingTax,
                    customer,
                    ref Error);

                shippingTotalWithDiscountTaxed = Math.Round(shippingTotalWithDiscountTaxed.Value, 2);
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
        public static decimal GetShippingDiscount(Customer customer, 
            decimal shippingTotal, out Discount appliedDiscount)
        {
            decimal shippingDiscountAmount = decimal.Zero;

            string customerCouponCode = string.Empty;
            if (customer != null)
                customerCouponCode = customer.LastAppliedCouponCode;

            var allDiscounts = DiscountManager.GetAllDiscounts(DiscountTypeEnum.AssignedToShipping);
            var allowedDiscounts = new DiscountCollection();
            foreach (var _discount in allDiscounts)
            {
                if (_discount.IsActive(customerCouponCode) &&
                    _discount.DiscountType == DiscountTypeEnum.AssignedToShipping &&
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

            appliedDiscount = DiscountManager.GetPreferredDiscount(allowedDiscounts, shippingTotal);
            if (appliedDiscount != null)
            {
                shippingDiscountAmount = appliedDiscount.GetDiscountAmount(shippingTotal);
            }

            if (shippingDiscountAmount < decimal.Zero)
                shippingDiscountAmount = decimal.Zero;

            shippingDiscountAmount = Math.Round(shippingDiscountAmount, 2);

            return shippingDiscountAmount;
        }
        
        /// <summary>
        /// Indicates whether the shopping cart requires shipping
        /// </summary>
        /// <param name="Cart">Cart</param>
        /// <returns>True if the shopping cart requires shipping; otherwise, false.</returns>
        public static bool ShoppingCartRequiresShipping(ShoppingCart Cart)
        {
            foreach (var shoppingCartItem in Cart)
                if (shoppingCartItem.IsShipEnabled)
                    return true;
            return false;
        }

        /// <summary>
        /// Gets shopping cart additional shipping charge
        /// </summary>
        /// <param name="Cart">Cart</param>
        /// <param name="customer">Customer</param>
        /// <returns>Additional shipping charge</returns>
        public static decimal GetShoppingCartAdditionalShippingCharge(ShoppingCart Cart, Customer customer)
        {
            decimal additionalShippingCharge = decimal.Zero;

            bool isFreeShipping = IsFreeShipping(Cart, customer);
            if (isFreeShipping)
                return decimal.Zero;

            foreach (var shoppingCartItem in Cart)
                additionalShippingCharge += shoppingCartItem.AdditionalShippingCharge;

            return additionalShippingCharge;
        }

        /// <summary>
        ///  Gets available shipping options
        /// </summary>
        /// <param name="Cart">Shopping cart</param>
        /// <param name="customer">Customer</param>
        /// <param name="ShippingAddress">Shipping address</param>
        /// <param name="Error">Error</param>
        /// <returns>Shipping options</returns>
        public static ShippingOptionCollection GetShippingOptions(ShoppingCart Cart, Customer customer, Address ShippingAddress, ref string Error)
        {
            return GetShippingOptions(Cart, customer, ShippingAddress, null, ref Error);
        }

        /// <summary>
        ///  Gets available shipping options
        /// </summary>
        /// <param name="Cart">Shopping cart</param>
        /// <param name="customer">Customer</param>
        /// <param name="ShippingAddress">Shipping address</param>
        /// <param name="AllowedShippingRateComputationMethodID">Allowed shipping rate computation method identifier; null to load shipping options of all methods</param>
        /// <param name="Error">Error</param>
        /// <returns>Shipping options</returns>
        public static ShippingOptionCollection GetShippingOptions(ShoppingCart Cart, Customer customer, Address ShippingAddress, 
            int? AllowedShippingRateComputationMethodID, ref string Error)
        {
            if (Cart == null)
                throw new ArgumentNullException("Cart");

            var shippingOptions = new ShippingOptionCollection();

            bool isFreeShipping = IsFreeShipping(Cart, customer);

            var ShipmentPackage = CreateShipmentPackage(Cart, customer, ShippingAddress);
            var shippingRateComputationMethods = ShippingRateComputationMethodManager.GetAllShippingRateComputationMethods(false);
            if (shippingRateComputationMethods.Count == 0)
                throw new NopException("Shipping rate computation method could not be loaded");

            foreach (var srcm in shippingRateComputationMethods)
            {
                if (AllowedShippingRateComputationMethodID.HasValue &&
                    AllowedShippingRateComputationMethodID.Value > 0 &&
                    AllowedShippingRateComputationMethodID.Value != srcm.ShippingRateComputationMethodID)
                    continue;

                var iShippingRateComputationMethod = Activator.CreateInstance(Type.GetType(srcm.ClassName)) as IShippingRateComputationMethod;

                var shippingOptions2 = iShippingRateComputationMethod.GetShippingOptions(ShipmentPackage, ref Error);
                if (shippingOptions2 != null)
                {
                    foreach (var so2 in shippingOptions2)
                    {
                        so2.ShippingRateComputationMethodID = srcm.ShippingRateComputationMethodID;
                        shippingOptions.Add(so2);
                    }
                }
            }

            //additional shipping charges
            decimal additionalShippingCharge = GetShoppingCartAdditionalShippingCharge(Cart, customer);
            shippingOptions.ForEach(so => so.Rate += additionalShippingCharge);

            //discounts
            foreach (var so in shippingOptions)
            {
                decimal rateWithoutDiscount = Math.Round(so.Rate, 2);
                decimal rateWithDiscount = decimal.Zero;

                Discount rateDiscount = null;
                decimal rateDiscountAmount = GetShippingDiscount(customer, rateWithoutDiscount, out rateDiscount);
                rateWithDiscount = rateWithoutDiscount - rateDiscountAmount;
                if (rateWithDiscount < decimal.Zero)
                    rateWithDiscount = decimal.Zero;

                rateWithDiscount = Math.Round(rateWithDiscount, 2);

                so.Rate = rateWithDiscount;
                if (rateDiscount != null)
                    so.AppliedDiscountID = rateDiscount.DiscountID;
            }

            //free shipping
            if (isFreeShipping)
            {
                shippingOptions.ForEach(so => so.Rate = decimal.Zero);
            }

            return shippingOptions;
        }

        #endregion

        #region Properties
        
        /// <summary>
        /// Gets or sets a default shipping origin address
        /// </summary>
        public static Address ShippingOrigin
        {
            get
            {
                int countryID = SettingManager.GetSettingValueInteger("Shipping.ShippingOrigin.CountryID");
                int stateProvinceID = SettingManager.GetSettingValueInteger("Shipping.ShippingOrigin.StateProvinceID");
                string zipPostalCode = SettingManager.GetSettingValue("Shipping.ShippingOrigin.ZipPostalCode");
                var address = new Address();
                address.CountryID = countryID;
                address.StateProvinceID = stateProvinceID;
                address.ZipPostalCode = zipPostalCode;
                return address;
            }
            set
            {
                int countryID = 0;
                int stateProvinceID = 0;
                string zipPostalCode = string.Empty;

                if (value != null)
                {
                    countryID = value.CountryID;
                    stateProvinceID = value.StateProvinceID;
                    zipPostalCode = value.ZipPostalCode;
                }

                SettingManager.SetParam("Shipping.ShippingOrigin.CountryID", countryID.ToString());
                SettingManager.SetParam("Shipping.ShippingOrigin.StateProvinceID", stateProvinceID.ToString());
                SettingManager.SetParam("Shipping.ShippingOrigin.ZipPostalCode", zipPostalCode);
            }
        }
        #endregion
    }
}
