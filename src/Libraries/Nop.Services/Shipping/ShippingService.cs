using System;
using System.Collections.Generic;
using System.Linq;
using Nop.Core;
using Nop.Core.Caching;
using Nop.Core.Data;
using Nop.Core.Domain.Common;
using Nop.Core.Domain.Customers;
using Nop.Core.Domain.Orders;
using Nop.Core.Domain.Shipping;
using Nop.Core.Plugins;
using Nop.Services.Catalog;
using Nop.Services.Logging;
using Nop.Services.Orders;

namespace Nop.Services.Shipping
{
    /// <summary>
    /// Shipping service
    /// </summary>
    public partial class ShippingService : IShippingService
    {
        #region Constants

        private const string SHIPPINGMETHODS_BY_ID_KEY = "Nop.shippingMethod.id-{0}";
        private const string SHIPPINGMETHODS_PATTERN_KEY = "Nop.shippingMethod.";

        #endregion

        #region Fields

        private readonly IRepository<ShippingMethod> _shippingMethodRepository;
        private readonly ICacheManager _cacheManager;
        private readonly ILogger _logger;
        private readonly IProductAttributeParser _productAttributeParser;
        private readonly ICheckoutAttributeParser _checkoutAttributeParser;
        private readonly ShippingSettings _shippingSettings;
        private readonly IPluginFinder _pluginFinder;
        
        #endregion

        #region Ctor

        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="cacheManager">Cache manager</param>
        /// <param name="shippingMethodRepository">Shipping method repository</param>
        /// <param name="logger">Logger</param>
        /// <param name="productAttributeParser">Product attribute parser</param>
        /// <param name="checkoutAttributeParser">Checkout attribute parser</param>
        /// <param name="shippingSettings">Shipping settings</param>
        /// <param name="pluginFinder">Plugin finder</param>
        public ShippingService(ICacheManager cacheManager, 
            IRepository<ShippingMethod> shippingMethodRepository,
            ILogger logger,
            IProductAttributeParser productAttributeParser,
            ICheckoutAttributeParser checkoutAttributeParser,
            ShippingSettings shippingSettings,
            IPluginFinder pluginFinder)
        {
            this._cacheManager = cacheManager;
            this._shippingMethodRepository = shippingMethodRepository;
            this._logger = logger;
            this._productAttributeParser = productAttributeParser;
            this._checkoutAttributeParser = checkoutAttributeParser;
            this._shippingSettings = shippingSettings;
            this._pluginFinder = pluginFinder;
        }

        #endregion
        
        #region Methods

        #region Shipping rate computation methods

        /// <summary>
        /// Load active shipping rate computation methods
        /// </summary>
        /// <returns>Shipping rate computation methods</returns>
        public virtual IList<IShippingRateComputationMethod> LoadActiveShippingRateComputationMethods()
        {
            return LoadAllShippingRateComputationMethods()
                   .Where(provider => _shippingSettings.ActiveShippingRateComputationMethodSystemNames.Contains(provider.PluginDescriptor.SystemName, StringComparer.InvariantCultureIgnoreCase))
                   .ToList();
        }

        /// <summary>
        /// Load shipping rate computation method by system name
        /// </summary>
        /// <param name="systemName">System name</param>
        /// <returns>Found Shipping rate computation method</returns>
        public virtual IShippingRateComputationMethod LoadShippingRateComputationMethodBySystemName(string systemName)
        {
            var providers = LoadAllShippingRateComputationMethods();
            var provider = providers.SingleOrDefault(p => p.PluginDescriptor.SystemName.Equals(systemName, StringComparison.InvariantCultureIgnoreCase));
            return provider;
        }

        /// <summary>
        /// Load all shipping rate computation methods
        /// </summary>
        /// <returns>Shipping rate computation methods</returns>
        public virtual IList<IShippingRateComputationMethod> LoadAllShippingRateComputationMethods()
        {
            //TODO Do not return plugin instances, return a list of PluginDescriptor
            var providers = _pluginFinder.GetPlugins<IShippingRateComputationMethod>();
            return providers.ToList();
        }

        #endregion

        #region Shipping methods


        /// <summary>
        /// Deletes a shipping method
        /// </summary>
        /// <param name="shippingMethod">The shipping method</param>
        public virtual void DeleteShippingMethod(ShippingMethod shippingMethod)
        {
            if (shippingMethod == null)
                throw new ArgumentNullException("shippingMethod");

            _shippingMethodRepository.Delete(shippingMethod);
            _cacheManager.RemoveByPattern(SHIPPINGMETHODS_PATTERN_KEY);
        }

        /// <summary>
        /// Gets a shipping method
        /// </summary>
        /// <param name="shippingMethodId">The shipping method identifier</param>
        /// <returns>Shipping method</returns>
        public virtual ShippingMethod GetShippingMethodById(int shippingMethodId)
        {
            if (shippingMethodId == 0)
                return null;

            string key = string.Format(SHIPPINGMETHODS_BY_ID_KEY, shippingMethodId);
            return _cacheManager.Get(key, () =>
            {
                var shippingMethod = _shippingMethodRepository.GetById(shippingMethodId);
                return shippingMethod;
            });
        }
        
        /// <summary>
        /// Gets all shipping methods
        /// </summary>
        /// <param name="filterByCountryId">The country indentifier to filter by</param>
        /// <returns>Shipping method collection</returns>
        public virtual IList<ShippingMethod> GetAllShippingMethods(int? filterByCountryId = null)
        {
            if (filterByCountryId.HasValue && filterByCountryId.Value > 0)
            {
                var query1 = from sm in _shippingMethodRepository.Table
                             where
                             sm.RestrictedCountries.Select(c => c.Id).Contains(filterByCountryId.Value)
                             select sm.Id;

                var query2 = from sm in _shippingMethodRepository.Table
                             where !query1.Contains(sm.Id)
                             orderby sm.DisplayOrder
                             select sm;

                var shippingMethods = query2.ToList();
                return shippingMethods;
            }
            else
            {
                var query = from sm in _shippingMethodRepository.Table
                            orderby sm.DisplayOrder
                            select sm;
                var shippingMethods = query.ToList();
                return shippingMethods;
            }
        }

        /// <summary>
        /// Inserts a shipping method
        /// </summary>
        /// <param name="shippingMethod">Shipping method</param>
        public virtual void InsertShippingMethod(ShippingMethod shippingMethod)
        {
            if (shippingMethod == null)
                throw new ArgumentNullException("shippingMethod");

            _shippingMethodRepository.Insert(shippingMethod);
            
            _cacheManager.RemoveByPattern(SHIPPINGMETHODS_PATTERN_KEY);
        }

        /// <summary>
        /// Updates the shipping method
        /// </summary>
        /// <param name="shippingMethod">Shipping method</param>
        public virtual void UpdateShippingMethod(ShippingMethod shippingMethod)
        {
            if (shippingMethod == null)
                throw new ArgumentNullException("shippingMethod");

            _shippingMethodRepository.Update(shippingMethod);

            _cacheManager.RemoveByPattern(SHIPPINGMETHODS_PATTERN_KEY);
        }

        #endregion

        #region Workflow

        /// <summary>
        /// Gets shopping cart item total weight
        /// </summary>
        /// <param name="shoppingCartItem">Shopping cart item</param>
        /// <returns>Shopping cart item weight</returns>
        public virtual decimal GetShoppingCartItemTotalWeight(ShoppingCartItem shoppingCartItem)
        {
            if (shoppingCartItem == null)
                throw new ArgumentNullException("shoppingCartItem");
            decimal totalWeight = decimal.Zero;
            if (shoppingCartItem.ProductVariant != null)
            {
                decimal attributesTotalWeight = decimal.Zero;

                if (!String.IsNullOrEmpty(shoppingCartItem.AttributesXml))
                {
                    var pvaValues = _productAttributeParser.ParseProductVariantAttributeValues(shoppingCartItem.AttributesXml);
                    foreach (var pvaValue in pvaValues)
                        attributesTotalWeight += pvaValue.WeightAdjustment;
                }
                decimal unitWeight = shoppingCartItem.ProductVariant.Weight + attributesTotalWeight;
                totalWeight = unitWeight * shoppingCartItem.Quantity;
            }
            return totalWeight;
        }

        /// <summary>
        /// Gets shopping cart weight
        /// </summary>
        /// <param name="cart">Cart</param>
        /// <returns>Shopping cart weight</returns>
        public virtual decimal GetShoppingCartTotalWeight(IList<ShoppingCartItem> cart)
        {
            Customer customer = cart.GetCustomer();

            decimal totalWeight = decimal.Zero;
            //shopping cart items
            foreach (var shoppingCartItem in cart)
                totalWeight += GetShoppingCartItemTotalWeight(shoppingCartItem);

            //checkout attributes
            if (customer != null)
            {
                if (!String.IsNullOrEmpty(customer.CheckoutAttributes))
                {
                    var caValues = _checkoutAttributeParser.ParseCheckoutAttributeValues(customer.CheckoutAttributes);
                    foreach (var caValue in caValues)
                        totalWeight += caValue.WeightAdjustment;
                }
            }
            return totalWeight;
        }

        /// <summary>
        /// Gets shopping cart additional shipping charge
        /// </summary>
        /// <param name="cart">Cart</param>
        /// <returns>Additional shipping charge</returns>
        public virtual decimal GetShoppingCartAdditionalShippingCharge(IList<ShoppingCartItem> cart)
        {
            decimal additionalShippingCharge = decimal.Zero;
            
            bool isFreeShipping = IsFreeShipping(cart);
            if (isFreeShipping)
                return decimal.Zero;

            foreach (var sci in cart)
                if (sci.IsShipEnabled && !sci.IsFreeShipping)
                    additionalShippingCharge += sci.AdditionalShippingCharge;

            return additionalShippingCharge;
        }

        /// <summary>
        /// Gets a value indicating whether shipping is free
        /// </summary>
        /// <param name="cart">Cart</param>
        /// <returns>A value indicating whether shipping is free</returns>
        public virtual bool IsFreeShipping(IList<ShoppingCartItem> cart)
        {
            Customer customer = cart.GetCustomer();
            if (customer != null)
            {
                //check whether customer is in a customer role with free shipping applied
                var customerRoles = customer.CustomerRoles.Where(cr => cr.Active);
                foreach (var customerRole in customerRoles)
                    if (customerRole.FreeShipping)
                        return true;
            }

            bool shoppingCartRequiresShipping = cart.RequiresShipping();
            if (!shoppingCartRequiresShipping)
                return true;

            //check whether all shopping cart items are marked as free shipping
            bool allItemsAreFreeShipping = true;
            foreach (var sc in cart)
            {
                if (sc.IsShipEnabled && !sc.IsFreeShipping)
                {
                    allItemsAreFreeShipping = false;
                    break;
                }
            }
            if (allItemsAreFreeShipping)
                return true;

            //otherwise, return false
            return false;
        }

        /// <summary>
        /// Create shipment package from shopping cart
        /// </summary>
        /// <param name="cart">Shopping cart</param>
        /// <param name="shippingAddress">Shipping address</param>
        /// <returns>Shipment package</returns>
        public virtual GetShippingOptionRequest CreateShippingOptionRequest(IList<ShoppingCartItem> cart,
            Address shippingAddress)
        {
            var request = new GetShippingOptionRequest();
            request.Customer = cart.GetCustomer();
            request.Items = new List<ShoppingCartItem>();
            foreach (var sc in cart)
                if (sc.IsShipEnabled)
                    request.Items.Add(sc);
            request.ShippingAddress = shippingAddress;
            //TODO set values from warehouses or shipping origin
            request.CountryFrom = null;
            request.StateProvinceFrom = null;
            request.ZipPostalCodeFrom = string.Empty;
            return request;

        }

        /// <summary>
        ///  Gets available shipping options
        /// </summary>
        /// <param name="cart">Shopping cart</param>
        /// <param name="shippingAddress">Shipping address</param>
        /// <param name="allowedShippingRateComputationMethodSystemName">Filter by shipping rate computation method identifier; null to load shipping options of all shipping rate computation methods</param>
        /// <returns>Shipping options</returns>
        public virtual GetShippingOptionResponse GetShippingOptions(IList<ShoppingCartItem> cart,
            Address shippingAddress, string allowedShippingRateComputationMethodSystemName = "")
        {
            if (cart == null)
                throw new ArgumentNullException("cart");

            var result = new GetShippingOptionResponse();
            
            bool isFreeShipping = IsFreeShipping(cart);

            //create a package
            var getShippingOptionRequest = CreateShippingOptionRequest(cart, shippingAddress);
            var shippingRateComputationMethods = LoadActiveShippingRateComputationMethods();
            if (shippingRateComputationMethods.Count == 0)
                throw new NopException("Shipping rate computation method could not be loaded");

            //get shipping options
            foreach (var srcm in shippingRateComputationMethods)
            {
                if (!String.IsNullOrWhiteSpace(allowedShippingRateComputationMethodSystemName) &&
                   !allowedShippingRateComputationMethodSystemName.Equals(srcm.PluginDescriptor.SystemName, StringComparison.InvariantCultureIgnoreCase))
                    continue;

                var getShippingOptionResponse = srcm.GetShippingOptions(getShippingOptionRequest);
                foreach (var so2 in getShippingOptionResponse.ShippingOptions)
                {
                    so2.ShippingRateComputationMethodSystemName = srcm.PluginDescriptor.SystemName;
                    result.ShippingOptions.Add(so2);
                }

                //log errors
                if (!getShippingOptionResponse.Success)
                {
                    foreach (string error in getShippingOptionResponse.Errors)
                    {
                        result.AddError(error);
                        _logger.Warning(string.Format("Shipping ({0}). {1}", srcm.PluginDescriptor.FriendlyName, error));
                    }
                }
            }


            //no shipping options loaded
            if (result.ShippingOptions.Count == 0 && result.Errors.Count == 0)
                result.Errors.Add("Shipping options could not be loaded");

            //additional shipping charges
            decimal additionalShippingCharge = GetShoppingCartAdditionalShippingCharge(cart);
            foreach (var so in result.ShippingOptions)
            {
                so.Rate += additionalShippingCharge;

                //free shipping
                if (isFreeShipping)
                    so.Rate = decimal.Zero;

            }
            
            return result;
        }

        #endregion

        #endregion
    }
}
