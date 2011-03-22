using System;
using System.Collections.Generic;
using System.Globalization;
using System.Linq;
using System.Text;
using Nop.Core;
using Nop.Core.Domain.Catalog;
using Nop.Core.Domain.Customers;
using Nop.Core.Domain.Discounts;
using Nop.Core.Domain.Localization;
using Nop.Core.Domain.Logging;
using Nop.Core.Domain.Orders;
using Nop.Core.Domain.Payments;
using Nop.Core.Domain.Shipping;
using Nop.Core.Domain.Tax;
using Nop.Data;
using Nop.Services.Catalog;
using Nop.Services.Customers;
using Nop.Services.Discounts;
using Nop.Services.Localization;
using Nop.Services.Logging;
using Nop.Services.Payments;
using Nop.Services.Shipping;
using Nop.Services.Tax;
using Nop.Core.Domain.Common;
using Nop.Services.Security;

namespace Nop.Services.Orders
{
    /// <summary>
    /// Order processing service
    /// </summary>
    public partial class OrderProcessingService : IOrderProcessingService
    {
        #region Fields
        
        private readonly IOrderService _orderService;
        private readonly IWebHelper _webHelper;
        private readonly ILocalizationService _localizationService;
        private readonly ILanguageService _languageService;
        private readonly IProductService _productService;
        private readonly IPaymentService _paymentService;
        private readonly ILogger _logger;
        private readonly IOrderTotalCalculationService _orderTotalCalculationService;
        private readonly IPriceCalculationService _priceCalculationService;
        private readonly IPriceFormatter _priceFormatter;
        private readonly IProductAttributeParser _productAttributeParser;
        private readonly IProductAttributeFormatter _productAttributeFormatter;
        private readonly IGiftCardService _giftCardService;
        private readonly IShoppingCartService _shoppingCartService;
        private readonly ICheckoutAttributeFormatter _checkoutAttributeFormatter;
        private readonly IShippingService _shippingService;
        private readonly ITaxService _taxService;
        private readonly ICustomerService _customerService;
        private readonly IDiscountService _discountService;
        private readonly IEncryptionService _encryptionService;

        private readonly IWorkContext _workContext;

        private readonly RewardPointsSettings _rewardPointsSettings;
        private readonly OrderSettings _orderSettings;
        private readonly TaxSettings _taxSettings;

        #endregion

        #region Ctor

        /// <summary>
        /// Ctor
        /// </summary>
        /// <param name="orderService">Order service</param>
        /// <param name="webHelper">Web helper</param>
        /// <param name="localizationService">Localization service</param>
        /// <param name="languageService">Language service</param>
        /// <param name="productService">Product service</param>
        /// <param name="paymentService">Payment service</param>
        /// <param name="logger">Logger</param>
        /// <param name="orderTotalCalculationService">Order total calculationservice</param>
        /// <param name="priceCalculationService">Price calculation service</param>
        /// <param name="priceFormatter">Price formatter</param>
        /// <param name="productAttributeParser">Product attribute parser</param>
        /// <param name="productAttributeFormatter">Product attribute formatter</param>
        /// <param name="giftCardService">Gift card service</param>
        /// <param name="shoppingCartService">Shopping cart service</param>
        /// <param name="checkoutAttributeFormatter">Checkout attribute service</param>
        /// <param name="shippingService">Shipping service</param>
        /// <param name="taxService">Tax service</param>
        /// <param name="customerService">Customer service</param>
        /// <param name="discountService">Discount service</param>
        /// <param name="encryptionService">Encryption service</param>
        /// <param name="workContext">Work context</param>
        /// <param name="rewardPointsSettings">Reward points settings</param>
        /// <param name="orderSettings">Order settings</param>
        /// <param name="taxSettings">Tax settings</param>
        public OrderProcessingService(IOrderService orderService,
            IWebHelper webHelper,
            ILocalizationService localizationService,
            ILanguageService languageService,
            IProductService productService,
            IPaymentService paymentService,
            ILogger logger,
            IOrderTotalCalculationService orderTotalCalculationService,
            IPriceCalculationService priceCalculationService,
            IPriceFormatter priceFormatter,
            IProductAttributeParser productAttributeParser,
            IProductAttributeFormatter productAttributeFormatter,
            IGiftCardService giftCardService,
            IShoppingCartService shoppingCartService,
            ICheckoutAttributeFormatter checkoutAttributeFormatter,
            IShippingService shippingService,
            ITaxService taxService,
            ICustomerService customerService,
            IDiscountService discountService,
            IEncryptionService encryptionService,
            IWorkContext workContext,
            RewardPointsSettings rewardPointsSettings,
            OrderSettings orderSettings,
            TaxSettings taxSettings)
        {
            this._orderService = orderService;
            this._webHelper = webHelper;
            this._localizationService = localizationService;
            this._languageService = languageService;
            this._productService = productService;
            this._paymentService = paymentService;
            this._logger = logger;
            this._orderTotalCalculationService = orderTotalCalculationService;
            this._priceCalculationService = priceCalculationService;
            this._priceFormatter = priceFormatter;
            this._productAttributeParser = productAttributeParser;
            this._productAttributeFormatter = productAttributeFormatter;
            this._giftCardService = giftCardService;
            this._shoppingCartService = shoppingCartService;
            this._checkoutAttributeFormatter = checkoutAttributeFormatter;
            this._workContext = workContext;
            this._shippingService = shippingService;
            this._taxService = taxService;
            this._customerService = customerService;
            this._discountService = discountService;
            this._encryptionService = encryptionService;
            this._rewardPointsSettings = rewardPointsSettings;
            this._orderSettings = orderSettings;
            this._taxSettings = taxSettings;
        }

        #endregion

        #region Utilities

        /// <summary>
        /// Sets an order status
        /// </summary>
        /// <param name="order">Order</param>
        /// <param name="os">New order status</param>
        /// <param name="notifyCustomer">True to notify customer</param>
        protected void SetOrderStatus(Order order,
            OrderStatus os, bool notifyCustomer)
        {
            if (order == null)
                throw new ArgumentNullException("order");

            OrderStatus prevOrderStatus = order.OrderStatus;
            if (prevOrderStatus == os)
                return;

            //set and save new order status
            order.OrderStatusId = (int)os;
            _orderService.UpdateOrder(order);

            //order notes, notifications
            order.OrderNotes.Add(new OrderNote()
                {
                    Note = string.Format("Order status has been changed to {0}", os.ToString()),
                    DisplayToCustomer = false,
                    CreatedOnUtc = DateTime.UtcNow
                });
            _orderService.UpdateOrder(order);


            if (prevOrderStatus != OrderStatus.Complete &&
                os == OrderStatus.Complete
                && notifyCustomer)
            {
                //TODO implement notification
                //int orderCompletedCustomerNotificationQueuedEmailId = _messageService.SendOrderCompletedCustomerNotification(order, order.CustomerLanguageId);
                //if (orderCompletedCustomerNotificationQueuedEmailId > 0)
                //{
                //    order.OrderNotes.Add(new OrderNote()
                //    {
                //        Note = string.Format("\"Order completed\" email (to customer) has been queued. Queued email identifier: {0}.", orderCompletedCustomerNotificationQueuedEmailId),
                //        DisplayToCustomer = false,
                //        CreatedOnUtc = DateTime.UtcNow
                //    });
                //    _orderService.UpdateOrder(order);
                //}
            }

            if (prevOrderStatus != OrderStatus.Cancelled &&
                os == OrderStatus.Cancelled
                && notifyCustomer)
            {
                //TODO implement notification
                //int orderCancelledCustomerNotificationQueuedEmailId = _messageService.SendOrderCancelledCustomerNotification(order, order.CustomerLanguageId);
                //if (orderCancelledCustomerNotificationQueuedEmailId > 0)
                //{
                //    order.OrderNotes.Add(new OrderNote()
                //    {
                //        Note = string.Format("\"Order cancelled\" email (to customer) has been queued. Queued email identifier: {0}.", orderCancelledCustomerNotificationQueuedEmailId),
                //        DisplayToCustomer = false,
                //        CreatedOnUtc = DateTime.UtcNow
                //    });
                //    _orderService.UpdateOrder(order);
                //}
            }

            //reward points
            if (_rewardPointsSettings.Enabled)
            {
                if (_rewardPointsSettings.PointsForPurchases_Amount > decimal.Zero)
                {
                    //Ensure that reward points are applied only to registered users
                    if (order.Customer != null)
                    {
                        //UNDONE ensure customer is not guest !order.Customer.IsGuest
                        int points = (int)Math.Truncate(order.OrderTotal / _rewardPointsSettings.PointsForPurchases_Amount * _rewardPointsSettings.PointsForPurchases_Points);
                        if (points != 0)
                        {
                            if (_rewardPointsSettings.PointsForPurchases_Awarded == order.OrderStatus)
                            {
                                order.Customer.AddRewardPointsHistoryEntry(points,
                                    string.Format(_localizationService.GetResource("RewardPoints.Message.EarnedForOrder"), order.Id));
                                _orderService.UpdateOrder(order);
                            }


                            if (_rewardPointsSettings.PointsForPurchases_Canceled == order.OrderStatus)
                            {
                                order.Customer.AddRewardPointsHistoryEntry(-points,
                                       string.Format(_localizationService.GetResource("RewardPoints.Message.ReducedForOrder"), order.Id));
                                _orderService.UpdateOrder(order);
                            }
                        }
                    }
                }
            }

            //TODO implement gift cards activation
            //if (this.GiftCards_Activated.HasValue &&
            //   this.GiftCards_Activated.Value == order.OrderStatus)
            //{
            //    var giftCards = GetAllGiftCards(order.OrderId, null, null, null, null, null, null, false, string.Empty);
            //    foreach (var gc in giftCards)
            //    {
            //        bool isRecipientNotified = gc.IsRecipientNotified;
            //        switch (gc.PurchasedOrderProductVariant.ProductVariant.GiftCardType)
            //        {
            //            case (int)GiftCardType.Virtual:
            //                {
            //                    //send email for virtual gift card
            //                    if (!String.IsNullOrEmpty(gc.RecipientEmail) &&
            //                        !String.IsNullOrEmpty(gc.SenderEmail))
            //                    {
            //                        var customerLang = _languageService.GetLanguageById(order.CustomerLanguageId);
            //                        if (customerLang == null)
            //                            customerLang = NopContext.Current.WorkingLanguage;
            //                        int queuedEmailId = _messageService.SendGiftCardNotification(gc, customerLang.LanguageId);
            //                        if (queuedEmailId > 0)
            //                        {
            //                            isRecipientNotified = true;
            //                        }
            //                    }
            //                }
            //                break;
            //            case (int)GiftCardTypeEnum.Physical:
            //                {
            //                }
            //                break;
            //            default:
            //                break;
            //        }

            //        gc.IsGiftCardActivated = true;
            //        gc.IsRecipientNotified = isRecipientNotified;
            //        this.UpdateGiftCard(gc);
            //    }
            //}

            //TODO implement gift cards deactivation
            //if (this.GiftCards_Deactivated.HasValue &&
            //   this.GiftCards_Deactivated.Value == order.OrderStatus)
            //{
            //    var giftCards = GetAllGiftCards(order.OrderId,
            //        null, null, null, null, null, null, true, string.Empty);
            //    foreach (var gc in giftCards)
            //    {
            //        gc.IsGiftCardActivated = false;
            //        this.UpdateGiftCard(gc);
            //    }
            //}
        }

        /// <summary>
        /// Checks order status
        /// </summary>
        /// <param name="order">Order</param>
        /// <returns>Validated order</returns>
        protected void CheckOrderStatus(Order order)
        {
            if (order == null)
                throw new ArgumentNullException("order");

            if (order.OrderStatus == OrderStatus.Pending)
            {
                if (order.PaymentStatus == PaymentStatus.Authorized ||
                    order.PaymentStatus == PaymentStatus.Paid)
                {
                    SetOrderStatus(order, OrderStatus.Processing, false);
                }
            }

            if (order.OrderStatus == OrderStatus.Pending)
            {
                if (order.ShippingStatus == ShippingStatus.Shipped ||
                    order.ShippingStatus == ShippingStatus.Delivered)
                {
                    SetOrderStatus(order, OrderStatus.Processing, false);
                }
            }

            if (order.OrderStatus != OrderStatus.Cancelled &&
                order.OrderStatus != OrderStatus.Complete)
            {
                if (order.PaymentStatus == PaymentStatus.Paid)
                {
                    if (!CanShip(order) && !CanDeliver(order))
                    {
                        SetOrderStatus(order, OrderStatus.Complete, true);
                    }
                }
            }

            if (order.PaymentStatus == PaymentStatus.Paid && !order.PaidDateUtc.HasValue)
            {
                //ensure that paid date is set
                order.PaidDateUtc = DateTime.UtcNow;
                _orderService.UpdateOrder(order);
            }
        }

        #endregion

        #region Methods

        /// <summary>
        /// Places an order
        /// </summary>
        /// <param name="processPaymentRequest">Process payment request</param>
        /// <returns>Place order result</returns>
        public PlaceOrderResult PlaceOrder(ProcessPaymentRequest processPaymentRequest)
        {
            //think about moving functionality of processing recurring orders (after the initial order was placed) to ProcessNextRecurringPayment() method
            if (processPaymentRequest == null)
                throw new ArgumentNullException("processPaymentRequest");

            var result = new PlaceOrderResult();
            try
            {
                #region Order details (customer, addresses, totals)

                //Recurring orders. Load initial order
                Order initialOrder = processPaymentRequest.InitialOrder;
                if (processPaymentRequest.IsRecurringPayment)
                {
                    if (initialOrder == null)
                        throw new ArgumentException("Initial order is not set for recurring payment");

                    processPaymentRequest.PaymentMethodSystemName = initialOrder.PaymentMethodSystemName;
                }

                //customer
                var customer = processPaymentRequest.Customer;
                if (customer == null)
                    throw new ArgumentException("Customer is not set");

                //customer currency
                var customerCurrency = customer.Currency ?? _workContext.WorkingCurrency;
                string customerCurrencyCode = "";
                if (!processPaymentRequest.IsRecurringPayment)
                    customerCurrencyCode = customerCurrency.CurrencyCode;
                else
                    customerCurrencyCode = initialOrder.CustomerCurrencyCode;

                //customer language
                Language customerLanguage = null;
                if (!processPaymentRequest.IsRecurringPayment)
                    customerLanguage = customer.Language;
                else
                    customerLanguage = _languageService.GetLanguageById(initialOrder.CustomerLanguageId);
                if (customerLanguage == null)
                    customerLanguage = _workContext.WorkingLanguage;

                //UNDONE check whether customer is guest
                //if (customer.IsGuest && !_orderSettings.AnonymousCheckoutAllowed)
                //    throw new NopException("Anonymous checkout is not allowed");

                //billing address
                Address billingAddress = null;
                if (!processPaymentRequest.IsRecurringPayment)
                {
                    if (customer.BillingAddress == null)
                        throw new NopException("Billing address is not provided");

                    if (!CommonHelper.IsValidEmail(customer.BillingAddress.Email))
                        throw new NopException("Email is not valid");

                    //clone billing address
                    billingAddress = (Address)customer.BillingAddress.Clone();
                    if (billingAddress.Country != null && !billingAddress.Country.AllowsBilling)
                        throw new NopException(string.Format("Country '{0}' is not allowed for billing", billingAddress.Country.Name));
                }
                else
                {
                    if (initialOrder.BillingAddress == null)
                        throw new NopException("Billing address is not available");

                    //clone billing address
                    billingAddress = (Address)initialOrder.BillingAddress.Clone();
                    if (billingAddress.Country != null && !billingAddress.Country.AllowsBilling)
                        throw new NopException(string.Format("Country '{0}' is not allowed for billing", billingAddress.Country.Name));
                }
                
                //load and validate customer shopping cart
                IList<ShoppingCartItem> cart = null;
                if (!processPaymentRequest.IsRecurringPayment)
                {
                    //load shopping cart
                    cart = customer.ShoppingCartItems.Where(sci => sci.ShoppingCartType == ShoppingCartType.ShoppingCart).ToList();

                    //validate the entire shopping cart
                    var warnings = _shoppingCartService.GetShoppingCartWarnings(cart, customer.CheckoutAttributes, true);
                    if (warnings.Count > 0)
                    {
                        var warningsSb = new StringBuilder();
                        foreach (string warning in warnings)
                        {
                            warningsSb.Append(warning);
                            warningsSb.Append(";");
                        }
                        throw new NopException(warningsSb.ToString());
                    }

                    //validate individual cart items
                    foreach (var sci in cart)
                    {
                        var sciWarnings = _shoppingCartService.GetShoppingCartItemWarnings(sci.ShoppingCartType,
                            sci.ProductVariant, sci.AttributesXml,
                            sci.CustomerEnteredPrice, sci.Quantity);
                        if (sciWarnings.Count > 0)
                        {
                            var warningsSb = new StringBuilder();
                            foreach (string warning in sciWarnings)
                            {
                                warningsSb.Append(warning);
                                warningsSb.Append(";");
                            }
                            throw new NopException(warningsSb.ToString());
                        }
                    }
                }

                //tax display type
                var customerTaxDisplayType = TaxDisplayType.IncludingTax;
                if (!processPaymentRequest.IsRecurringPayment)
                {
                    if (_taxSettings.AllowCustomersToSelectTaxDisplayType)
                        customerTaxDisplayType = customer.TaxDisplayType;
                    else
                        customerTaxDisplayType = _taxSettings.TaxDisplayType;
                }
                else
                {
                    customerTaxDisplayType = initialOrder.CustomerTaxDisplayType;
                }

                //checkout attributes
                string checkoutAttributeDescription, checkoutAttributesXml;
                if (!processPaymentRequest.IsRecurringPayment)
                {
                    checkoutAttributeDescription = _checkoutAttributeFormatter.FormatAttributes(customer.CheckoutAttributes, customer);
                    checkoutAttributesXml = customer.CheckoutAttributes;
                }
                else
                {
                    checkoutAttributeDescription = initialOrder.CheckoutAttributeDescription;
                    checkoutAttributesXml = initialOrder.CheckoutAttributesXml;
                }

                //applied discount (used to store discount usage history)
                var appliedDiscounts = new List<Discount>();

                //sub total
                decimal orderSubTotalInclTax, orderSubTotalExclTax;
                decimal orderSubTotalDiscountInclTax = 0, orderSubTotalDiscountExclTax = 0;
                if (!processPaymentRequest.IsRecurringPayment)
                {
                    //sub total (incl tax)
                    decimal orderSubTotalDiscountAmount1 = decimal.Zero;
                    Discount orderSubTotalAppliedDiscount1 = null;
                    decimal subTotalWithoutDiscountBase1 = decimal.Zero;
                    decimal subTotalWithDiscountBase1 = decimal.Zero;
                    _orderTotalCalculationService.GetShoppingCartSubTotal(cart,
                        true, out orderSubTotalDiscountAmount1, out orderSubTotalAppliedDiscount1,
                        out subTotalWithoutDiscountBase1, out subTotalWithDiscountBase1);
                    orderSubTotalInclTax = subTotalWithoutDiscountBase1;
                    orderSubTotalDiscountInclTax = orderSubTotalDiscountAmount1;

                    //discount history
                    if (orderSubTotalAppliedDiscount1 != null && !appliedDiscounts.Contains(orderSubTotalAppliedDiscount1))
                        appliedDiscounts.Add(orderSubTotalAppliedDiscount1);

                    //sub total (excl tax)
                    decimal orderSubTotalDiscountAmount2 = decimal.Zero;
                    Discount orderSubTotalAppliedDiscount2 = null;
                    decimal subTotalWithoutDiscountBase2 = decimal.Zero;
                    decimal subTotalWithDiscountBase2 = decimal.Zero;
                    _orderTotalCalculationService.GetShoppingCartSubTotal(cart,
                        false, out orderSubTotalDiscountAmount2, out orderSubTotalAppliedDiscount2,
                        out subTotalWithoutDiscountBase2, out subTotalWithDiscountBase2);
                    orderSubTotalExclTax = subTotalWithoutDiscountBase2;
                    orderSubTotalDiscountExclTax = orderSubTotalDiscountAmount2;
                }
                else
                {
                    orderSubTotalInclTax = initialOrder.OrderSubtotalInclTax;
                    orderSubTotalExclTax = initialOrder.OrderSubtotalExclTax;
                }


                //shipping info
                decimal orderWeight = decimal.Zero;
                bool shoppingCartRequiresShipping = false;
                if (!processPaymentRequest.IsRecurringPayment)
                {
                    orderWeight = _shippingService.GetShoppingCartTotalWeight(cart);
                    shoppingCartRequiresShipping = cart.RequiresShipping();
                }
                else
                {
                    orderWeight = initialOrder.OrderWeight;
                    if (initialOrder.ShippingStatus != ShippingStatus.ShippingNotRequired)
                        shoppingCartRequiresShipping = true;
                }
                Address shippingAddress = null;
                string shippingMethodName = "", shippingRateComputationMethodSystemName = "";
                if (shoppingCartRequiresShipping)
                {
                    if (!processPaymentRequest.IsRecurringPayment)
                    {
                        if (customer.ShippingAddress == null)
                            throw new NopException("Shipping address is not provided");

                        if (!CommonHelper.IsValidEmail(customer.ShippingAddress.Email))
                            throw new NopException("Email is not valid");

                        //clone shipping address
                        shippingAddress = (Address)customer.ShippingAddress.Clone();
                        if (shippingAddress.Country != null && !shippingAddress.Country.AllowsShipping)
                            throw new NopException(string.Format("Country '{0}' is not allowed for shipping", shippingAddress.Country.Name));

                        var shippingOption = customer.GetAttribute<ShippingOption>("LastShippingOption");
                        if (shippingOption != null)
                        {
                            shippingMethodName = shippingOption.Name;
                            shippingRateComputationMethodSystemName = shippingOption.ShippingRateComputationMethodSystemName;
                        }
                    }
                    else
                    {
                        if (initialOrder.ShippingAddress == null)
                            throw new NopException("Shipping address is not available");

                        //clone billing address
                        shippingAddress = (Address)initialOrder.ShippingAddress.Clone();
                        if (shippingAddress.Country != null && !shippingAddress.Country.AllowsShipping)
                            throw new NopException(string.Format("Country '{0}' is not allowed for shipping", shippingAddress.Country.Name));

                        shippingMethodName = initialOrder.ShippingMethod;
                        shippingRateComputationMethodSystemName = initialOrder.ShippingRateComputationMethodSystemName;
                    }
                }


                //shipping total
                decimal? orderShippingTotalInclTax, orderShippingTotalExclTax = null;
                if (!processPaymentRequest.IsRecurringPayment)
                {
                    decimal taxRate = decimal.Zero;
                    Discount shippingTotalDiscount = null;
                    orderShippingTotalInclTax = _orderTotalCalculationService.GetShoppingCartShippingTotal(cart, true, out taxRate, out shippingTotalDiscount);
                    orderShippingTotalExclTax = _orderTotalCalculationService.GetShoppingCartShippingTotal(cart, false);
                    if (!orderShippingTotalInclTax.HasValue || !orderShippingTotalExclTax.HasValue)
                        throw new NopException("Shipping total couldn't be calculated");

                    if (shippingTotalDiscount != null && !appliedDiscounts.Contains(shippingTotalDiscount))
                        appliedDiscounts.Add(shippingTotalDiscount);
                }
                else
                {
                    orderShippingTotalInclTax = initialOrder.OrderShippingInclTax;
                    orderShippingTotalExclTax = initialOrder.OrderShippingExclTax;
                }


                //payment total
                decimal paymentAdditionalFeeInclTax, paymentAdditionalFeeExclTax;
                if (!processPaymentRequest.IsRecurringPayment)
                {
                    decimal paymentAdditionalFee = _paymentService.GetAdditionalHandlingFee(processPaymentRequest.PaymentMethodSystemName);
                    paymentAdditionalFeeInclTax = _taxService.GetPaymentMethodAdditionalFee(paymentAdditionalFee, true, customer);
                    paymentAdditionalFeeExclTax = _taxService.GetPaymentMethodAdditionalFee(paymentAdditionalFee, false, customer);
                }
                else
                {
                    paymentAdditionalFeeInclTax = initialOrder.PaymentMethodAdditionalFeeInclTax;
                    paymentAdditionalFeeExclTax = initialOrder.PaymentMethodAdditionalFeeExclTax;
                }


                //tax total
                decimal orderTaxTotal = decimal.Zero;
                string vatNumber = "", taxRates = "";
                if (!processPaymentRequest.IsRecurringPayment)
                {
                    //tax amount
                    SortedDictionary<decimal, decimal> taxRatesDictionary = null;
                    orderTaxTotal = _orderTotalCalculationService.GetTaxTotal(cart, out taxRatesDictionary);

                    //VAT number
                    if (_taxSettings.EuVatEnabled && customer.VatNumberStatus == VatNumberStatus.Valid)
                        vatNumber = customer.VatNumber;

                    //tax rates
                    foreach (var kvp in taxRatesDictionary)
                    {
                        var taxRate = kvp.Key;
                        var taxValue = kvp.Value;
                        taxRates += string.Format("{0}:{1};   ", taxRate.ToString(CultureInfo.InvariantCulture), taxValue.ToString(CultureInfo.InvariantCulture));
                    }
                }
                else
                {
                    orderTaxTotal = initialOrder.OrderTax;
                    //VAT number
                    //TODO: Possible BUG: VAT number status may have changed since original order was placed, probably best to recalculate tax or do some checks?
                    vatNumber = initialOrder.VatNumber;
                }


                //order total (and applied discounts, gift cards, reward points)
                decimal? orderTotal = null;
                //decimal orderTotalInCustomerCurrency = decimal.Zero;
                decimal orderDiscountAmount = decimal.Zero;
                //decimal orderDiscountInCustomerCurrency = decimal.Zero;
                List<AppliedGiftCard> appliedGiftCards = null;
                int redeemedRewardPoints = 0;
                decimal redeemedRewardPointsAmount = decimal.Zero;
                if (!processPaymentRequest.IsRecurringPayment)
                {
                    Discount orderAppliedDiscount = null;
                    orderTotal = _orderTotalCalculationService.GetShoppingCartTotal(cart,
                        out orderDiscountAmount, out orderAppliedDiscount, out appliedGiftCards,
                        out redeemedRewardPoints, out redeemedRewardPointsAmount);
                    if (!orderTotal.HasValue)
                        throw new NopException("Order total couldn't be calculated");

                    //discount history
                    if (orderAppliedDiscount != null && !appliedDiscounts.Contains(orderAppliedDiscount))
                        appliedDiscounts.Add(orderAppliedDiscount);
                }
                else
                {
                    orderDiscountAmount = initialOrder.OrderDiscount;
                    orderTotal = initialOrder.OrderTotal;
                }
                processPaymentRequest.OrderTotal = orderTotal.Value;

                #endregion

                #region Payment workflow

                //skip payment workflow if order total equals zero
                bool skipPaymentWorkflow = false;
                if (orderTotal.Value == decimal.Zero)
                    skipPaymentWorkflow = true;

                //payment workflow
                IPaymentMethod paymentMethod = null;
                if (!skipPaymentWorkflow)
                {
                    paymentMethod = _paymentService.LoadPaymentMethodBySystemName(processPaymentRequest.PaymentMethodSystemName);
                    if (paymentMethod == null)
                        throw new NopException("Payment method couldn't be loaded");

                    //TODO ensure that payment method is active (commented)
                    //if (!paymentMethod.IsActive)
                    //    throw new NopException("Payment method is not active");
                }
                else
                    processPaymentRequest.PaymentMethodSystemName = "";

                //recurring or standard shopping cart?
                bool isRecurringShoppingCart = false;
                if (!processPaymentRequest.IsRecurringPayment)
                {
                    isRecurringShoppingCart = cart.IsRecurring();
                    if (isRecurringShoppingCart)
                    {
                        int recurringCycleLength = 0;
                        RecurringProductCyclePeriod recurringCyclePeriod;
                        int recurringTotalCycles = 0;
                        string recurringCyclesError = cart.GetReccuringCycleInfo(out recurringCycleLength, out recurringCyclePeriod, out recurringTotalCycles);
                        if (!string.IsNullOrEmpty(recurringCyclesError))
                            throw new NopException(recurringCyclesError);
                        processPaymentRequest.RecurringCycleLength = recurringCycleLength;
                        processPaymentRequest.RecurringCyclePeriod = recurringCyclePeriod;
                        processPaymentRequest.RecurringTotalCycles = recurringTotalCycles;
                    }
                }
                else
                    isRecurringShoppingCart = true;


                //process payment
                ProcessPaymentResult processPaymentResult = null;
                if (!skipPaymentWorkflow)
                {
                    if (!processPaymentRequest.IsRecurringPayment)
                    {
                        if (isRecurringShoppingCart)
                        {
                            //recurring cart
                            var recurringPaymentType = _paymentService.GetRecurringPaymentType(processPaymentRequest.PaymentMethodSystemName);
                            switch (recurringPaymentType)
                            {
                                case RecurringPaymentType.NotSupported:
                                    throw new NopException("Recurring payments are not supported by selected payment method");
                                case RecurringPaymentType.Manual:
                                case RecurringPaymentType.Automatic:
                                    processPaymentResult = _paymentService.ProcessRecurringPayment(processPaymentRequest);
                                    break;
                                default:
                                    throw new NopException("Not supported recurring payment type");
                            }
                        }
                        else
                        {
                            //standard cart
                            processPaymentResult = _paymentService.ProcessPayment(processPaymentRequest);
                        }
                    }
                    else
                    {
                        if (isRecurringShoppingCart)
                        {
                            var recurringPaymentType = _paymentService.GetRecurringPaymentType(processPaymentRequest.PaymentMethodSystemName);
                            switch (recurringPaymentType)
                            {
                                case RecurringPaymentType.NotSupported:
                                    throw new NopException("Recurring payments are not supported by selected payment method");
                                case RecurringPaymentType.Manual:
                                    processPaymentResult = _paymentService.ProcessRecurringPayment(processPaymentRequest);
                                    break;
                                case RecurringPaymentType.Automatic:
                                    //payment is processed on payment gateway site
                                    break;
                                default:
                                    throw new NopException("Not supported recurring payment type");
                            }
                        }
                        else
                        {
                            throw new NopException("No recurring products");
                        }
                    }
                }
                else
                {
                    //payment is not required
                    if (processPaymentResult == null)
                        processPaymentResult = new ProcessPaymentResult();
                    processPaymentResult.NewPaymentStatus = PaymentStatus.Paid;
                }

                if (processPaymentResult == null)
                    throw new NopException("processPaymentResult is not available");

                #endregion

                if (processPaymentResult.Success)
                {

                    //save order in data storage
                    //uncomment this line to support transactions
                    //using (var scope = new System.Transactions.TransactionScope())
                    {
                        #region Save order details

                        var shippingStatus = ShippingStatus.NotYetShipped;
                        if (!shoppingCartRequiresShipping)
                            shippingStatus = ShippingStatus.ShippingNotRequired;

                        var order = new Order()
                        {
                            OrderGuid = processPaymentRequest.OrderGuid,
                            CustomerId = customer.Id,
                            CustomerLanguageId = customerLanguage.Id,
                            CustomerTaxDisplayType = customerTaxDisplayType,
                            CustomerIp = _webHelper.GetCurrentIpAddress(),
                            OrderSubtotalInclTax = orderSubTotalInclTax,
                            OrderSubtotalExclTax = orderSubTotalExclTax,
                            OrderSubTotalDiscountInclTax = orderSubTotalDiscountInclTax,
                            OrderSubTotalDiscountExclTax = orderSubTotalDiscountExclTax,
                            OrderShippingInclTax = orderShippingTotalInclTax.Value,
                            OrderShippingExclTax = orderShippingTotalExclTax.Value,
                            PaymentMethodAdditionalFeeInclTax = paymentAdditionalFeeInclTax,
                            PaymentMethodAdditionalFeeExclTax = paymentAdditionalFeeExclTax,
                            TaxRates = taxRates,
                            OrderTax = orderTaxTotal,
                            OrderTotal = orderTotal.Value,
                            RefundedAmount = decimal.Zero,
                            OrderDiscount = orderDiscountAmount,
                            CheckoutAttributeDescription = checkoutAttributeDescription,
                            CheckoutAttributesXml = checkoutAttributesXml,
                            CustomerCurrencyCode = customerCurrencyCode,
                            OrderWeight = orderWeight,
                            AffiliateId = customer.AffiliateId,
                            OrderStatus = OrderStatus.Pending,
                            AllowStoringCreditCardNumber = processPaymentResult.AllowStoringCreditCardNumber,
                            CardType = processPaymentResult.AllowStoringCreditCardNumber ? _encryptionService.EncryptText(processPaymentRequest.CreditCardType) : string.Empty,
                            CardName = processPaymentResult.AllowStoringCreditCardNumber ? _encryptionService.EncryptText(processPaymentRequest.CreditCardName) : string.Empty,
                            CardNumber = processPaymentResult.AllowStoringCreditCardNumber ? _encryptionService.EncryptText(processPaymentRequest.CreditCardNumber) : string.Empty,
                            MaskedCreditCardNumber = _encryptionService.EncryptText(_paymentService.GetMaskedCreditCardNumber(processPaymentRequest.CreditCardNumber)),
                            CardCvv2 = processPaymentResult.AllowStoringCreditCardNumber ? _encryptionService.EncryptText(processPaymentRequest.CreditCardCvv2) : string.Empty,
                            CardExpirationMonth = processPaymentResult.AllowStoringCreditCardNumber ? _encryptionService.EncryptText(processPaymentRequest.CreditCardExpireMonth.ToString()) : string.Empty,
                            CardExpirationYear = processPaymentResult.AllowStoringCreditCardNumber ? _encryptionService.EncryptText(processPaymentRequest.CreditCardExpireYear.ToString()) : string.Empty,
                            PaymentMethodSystemName = processPaymentRequest.PaymentMethodSystemName,
                            AuthorizationTransactionId = processPaymentResult.AuthorizationTransactionId,
                            AuthorizationTransactionCode = processPaymentResult.AuthorizationTransactionCode,
                            AuthorizationTransactionResult = processPaymentResult.AuthorizationTransactionResult,
                            CaptureTransactionId = processPaymentResult.CaptureTransactionId,
                            CaptureTransactionResult = processPaymentResult.CaptureTransactionResult,
                            SubscriptionTransactionId = processPaymentResult.SubscriptionTransactionId,
                            PurchaseOrderNumber = processPaymentRequest.PurchaseOrderNumber,
                            PaymentStatus = processPaymentResult.NewPaymentStatus,
                            PaidDateUtc = null,
                            BillingAddress = billingAddress,
                            ShippingAddress = shippingAddress,
                            ShippingStatus = shippingStatus,
                            ShippingMethod = shippingMethodName,
                            ShippingRateComputationMethodSystemName = shippingRateComputationMethodSystemName,
                            ShippedDateUtc = null,
                            DeliveryDateUtc = null,
                            TrackingNumber = "",
                            VatNumber = vatNumber,
                            Deleted = false,
                            CreatedOnUtc = DateTime.UtcNow
                        };
                        _orderService.InsertOrder(order);

                        result.PlacedOrder = order;

                        if (!processPaymentRequest.IsRecurringPayment)
                        {
                            //move shopping cart items to order product variants
                            foreach (var sc in cart)
                            {
                                //prices
                                decimal taxRate = decimal.Zero;
                                decimal scUnitPrice = _priceCalculationService.GetUnitPrice(sc, true);
                                decimal scSubTotal = _priceCalculationService.GetSubTotal(sc, true);
                                decimal scUnitPriceInclTax = _taxService.GetProductPrice(sc.ProductVariant, scUnitPrice, true, customer, out taxRate);
                                decimal scUnitPriceExclTax = _taxService.GetProductPrice(sc.ProductVariant, scUnitPrice, false, customer, out taxRate);
                                decimal scSubTotalInclTax = _taxService.GetProductPrice(sc.ProductVariant, scSubTotal, true, customer, out taxRate);
                                decimal scSubTotalExclTax = _taxService.GetProductPrice(sc.ProductVariant, scSubTotal, false, customer, out taxRate);

                                //discounts
                                Discount scDiscount = null;
                                decimal discountAmount = _priceCalculationService.GetDiscountAmount(sc, out scDiscount);
                                decimal discountAmountInclTax = _taxService.GetProductPrice(sc.ProductVariant, discountAmount, true, customer, out taxRate);
                                decimal discountAmountExclTax = _taxService.GetProductPrice(sc.ProductVariant, discountAmount, false, customer, out taxRate);
                                if (scDiscount != null && !appliedDiscounts.Contains(scDiscount))
                                    appliedDiscounts.Add(scDiscount);

                                //attributes
                                string attributeDescription = _productAttributeFormatter.FormatAttributes(sc.ProductVariant, sc.AttributesXml, customer);

                                //save order item
                                var opv = new OrderProductVariant()
                                {
                                    OrderProductVariantGuid = Guid.NewGuid(),
                                    Order = order,
                                    ProductVariantId = sc.ProductVariantId,
                                    UnitPriceInclTax = scUnitPriceInclTax,
                                    UnitPriceExclTax = scUnitPriceExclTax,
                                    PriceInclTax = scSubTotalInclTax,
                                    PriceExclTax = scSubTotalExclTax,
                                    AttributeDescription = attributeDescription,
                                    AttributesXml = sc.AttributesXml,
                                    Quantity = sc.Quantity,
                                    DiscountAmountInclTax = discountAmountInclTax,
                                    DiscountAmountExclTax = discountAmountExclTax,
                                    DownloadCount = 0,
                                    IsDownloadActivated = false,
                                    LicenseDownloadId = 0
                                };
                                order.OrderProductVariants.Add(opv);
                                _orderService.UpdateOrder(order);

                                //gift cards
                                if (sc.ProductVariant.IsGiftCard)
                                {
                                    string giftCardRecipientName, giftCardRecipientEmail,
                                        giftCardSenderName, giftCardSenderEmail, giftCardMessage;
                                    _productAttributeParser.GetGiftCardAttribute(sc.AttributesXml,
                                        out giftCardRecipientName, out giftCardRecipientEmail,
                                        out giftCardSenderName, out giftCardSenderEmail, out giftCardMessage);

                                    for (int i = 0; i < sc.Quantity; i++)
                                    {
                                        var gc = new GiftCard()
                                        {
                                            //PurchasedOrderProductVariantId = opv.OrderProductVariantId,
                                            Amount = scUnitPriceExclTax,
                                            IsGiftCardActivated = false,
                                            GiftCardCouponCode = _giftCardService.GenerateGiftCardCode(),
                                            RecipientName = giftCardRecipientName,
                                            RecipientEmail = giftCardRecipientEmail,
                                            SenderName = giftCardSenderName,
                                            SenderEmail = giftCardSenderEmail,
                                            Message = giftCardMessage,
                                            IsRecipientNotified = false,
                                            CreatedOnUtc = DateTime.UtcNow
                                        };
                                        _giftCardService.InsertGiftCard(gc);
                                    }
                                }

                                //inventory
                                _productService.AdjustInventory(sc.ProductVariant, true, sc.Quantity, sc.AttributesXml);
                            }

                            //clear shopping cart
                            customer.ShoppingCartItems.Clear();
                            _customerService.UpdateCustomer(customer);
                            //TODO reset checkout data
                        }
                        else
                        {
                            //recurring payment
                            var initialOrderProductVariants = initialOrder.OrderProductVariants;
                            foreach (var opv in initialOrderProductVariants)
                            {
                                //save item
                                var newOpv = new OrderProductVariant()
                                {
                                    OrderProductVariantGuid = Guid.NewGuid(),
                                    Order = order,
                                    ProductVariantId = opv.ProductVariantId,
                                    UnitPriceInclTax = opv.UnitPriceInclTax,
                                    UnitPriceExclTax = opv.UnitPriceExclTax,
                                    PriceInclTax = opv.PriceInclTax,
                                    PriceExclTax = opv.PriceExclTax,
                                    AttributeDescription = opv.AttributeDescription,
                                    AttributesXml = opv.AttributesXml,
                                    Quantity = opv.Quantity,
                                    DiscountAmountInclTax = opv.DiscountAmountInclTax,
                                    DiscountAmountExclTax = opv.DiscountAmountExclTax,
                                    DownloadCount = 0,
                                    IsDownloadActivated = false,
                                    LicenseDownloadId = 0
                                };
                                order.OrderProductVariants.Add(opv);
                                _orderService.UpdateOrder(order);

                                //gift cards
                                if (opv.ProductVariant.IsGiftCard)
                                {
                                    string giftCardRecipientName, giftCardRecipientEmail,
                                        giftCardSenderName, giftCardSenderEmail, giftCardMessage;
                                    _productAttributeParser.GetGiftCardAttribute(opv.AttributesXml,
                                        out giftCardRecipientName, out giftCardRecipientEmail,
                                        out giftCardSenderName, out giftCardSenderEmail, out giftCardMessage);

                                    for (int i = 0; i < opv.Quantity; i++)
                                    {
                                        var gc = new GiftCard()
                                        {
                                            //PurchasedOrderProductVariantId = newOpv.OrderProductVariantId,
                                            Amount = opv.UnitPriceExclTax,
                                            IsGiftCardActivated = false,
                                            GiftCardCouponCode = _giftCardService.GenerateGiftCardCode(),
                                            RecipientName = giftCardRecipientName,
                                            RecipientEmail = giftCardRecipientEmail,
                                            SenderName = giftCardSenderName,
                                            SenderEmail = giftCardSenderEmail,
                                            Message = giftCardMessage,
                                            IsRecipientNotified = false,
                                            CreatedOnUtc = DateTime.UtcNow
                                        };
                                        _giftCardService.InsertGiftCard(gc);
                                    }
                                }

                                //inventory
                                _productService.AdjustInventory(opv.ProductVariant, true, opv.Quantity, opv.AttributesXml);
                            }
                        }

                        //discount usage history
                        if (!processPaymentRequest.IsRecurringPayment)
                            foreach (var discount in appliedDiscounts)
                            {
                                var duh = new DiscountUsageHistory()
                                {
                                    Discount = discount,
                                    Order = order,
                                    CreatedOnUtc = DateTime.UtcNow
                                };
                                discount.DiscountUsageHistory.Add(duh);
                                _discountService.UpdateDiscount(discount);
                            }

                        //gift card usage history
                        if (!processPaymentRequest.IsRecurringPayment)
                            if (appliedGiftCards != null)
                                foreach (var agc in appliedGiftCards)
                                {
                                    decimal amountUsed = agc.AmountCanBeUsed;
                                    var gcuh = new GiftCardUsageHistory()
                                    {
                                        GiftCard = agc.GiftCard,
                                        UsedWithOrder = order,
                                        UsedValue = amountUsed,
                                        CreatedOnUtc = DateTime.UtcNow
                                    };
                                    agc.GiftCard.GiftCardUsageHistory.Add(gcuh);
                                    _giftCardService.UpdateGiftCard(agc.GiftCard);
                                }

                        //reward points history
                        if (redeemedRewardPointsAmount > decimal.Zero)
                        {
                            customer.AddRewardPointsHistoryEntry(-redeemedRewardPoints,
                                string.Format(_localizationService.GetResource("RewardPoints.Message.RedeemedForOrder", order.CustomerLanguageId), order.Id),
                                order,
                                redeemedRewardPointsAmount);
                            _customerService.UpdateCustomer(customer);
                        }

                        //recurring orders
                        if (!processPaymentRequest.IsRecurringPayment && isRecurringShoppingCart)
                        {
                            //create recurring payment (the first payment)
                            var rp = new RecurringPayment()
                            {
                                CycleLength = processPaymentRequest.RecurringCycleLength,
                                CyclePeriod = processPaymentRequest.RecurringCyclePeriod,
                                TotalCycles = processPaymentRequest.RecurringTotalCycles,
                                StartDateUtc = DateTime.UtcNow,
                                IsActive = true,
                                Deleted = false,
                                CreatedOnUtc = DateTime.UtcNow,
                                InitialOrder = order,
                            };
                            _orderService.InsertRecurringPayment(rp);


                            var recurringPaymentType = _paymentService.GetRecurringPaymentType(processPaymentRequest.PaymentMethodSystemName);
                            switch (recurringPaymentType)
                            {
                                case RecurringPaymentType.NotSupported:
                                    {
                                        //not supported
                                    }
                                    break;
                                case RecurringPaymentType.Manual:
                                    {
                                        //first payment
                                        var rph = new RecurringPaymentHistory()
                                        {
                                            RecurringPayment = rp,
                                            CreatedOnUtc = DateTime.UtcNow,
                                            Order = order,
                                        };
                                        rp.RecurringPaymentHistory.Add(rph);
                                        _orderService.UpdateRecurringPayment(rp);
                                    }
                                    break;
                                case RecurringPaymentType.Automatic:
                                    {
                                        //will be created later (process is automated)
                                    }
                                    break;
                                default:
                                    break;
                            }
                        }

                        #endregion

                        #region Notifications & notes

                        //notes, messages
                        order.OrderNotes.Add(new OrderNote()
                            {
                                Note = "Order placed",
                                DisplayToCustomer = false,
                                CreatedOnUtc = DateTime.UtcNow
                            });
                        _orderService.UpdateOrder(order);

                        //UNDONE send email notifications
                        //int orderPlacedStoreOwnerNotificationQueuedEmailId = _messageService.SendOrderPlacedStoreOwnerNotification(order, localizationManager.DefaultAdminLanguage.LanguageId);
                        //if (orderPlacedStoreOwnerNotificationQueuedEmailId > 0)
                        //{
                        //    order.OrderNotes.Add(new OrderNote()
                        //    {
                        //        Note = string.Format("\"Order placed\" email (to store owner) has been queued. Queued email identifier: {0}.", orderPlacedStoreOwnerNotificationQueuedEmailId),
                        //        DisplayToCustomer = false,
                        //        CreatedOnUtc = DateTime.UtcNow
                        //    });
                        //    _orderService.UpdateOrder(order);
                        //}

                        //int orderPlacedCustomerNotificationQueuedEmailId = messageService.SendOrderPlacedCustomerNotification(order, order.CustomerLanguageId);
                        //if (orderPlacedCustomerNotificationQueuedEmailId > 0)
                        //{
                        //    order.OrderNotes.Add(new OrderNote()
                        //    {
                        //        Note = string.Format("\"Order placed\" email (to customer) has been queued. Queued email identifier: {0}.", orderPlacedCustomerNotificationQueuedEmailId),
                        //        DisplayToCustomer = false,
                        //        CreatedOnUtc = DateTime.UtcNow
                        //    });
                        //    _orderService.UpdateOrder(order);
                        //}

                        //UNDONE send SMS
                        //_smsService.SendOrderPlacedNotification(order);

                        //check order status
                        CheckOrderStatus(order);

                        //TODO reset checkout data
                        //if (!processPaymentRequest.IsRecurringPayment)
                        //    customerService.ResetCheckoutData(customer.CustomerId, true);

                        //TODO insert activity
                        //if (!processPaymentRequest.IsRecurringPayment)
                        //{
                        //    customerActivityService.InsertActivity(
                        //        "PlaceOrder",
                        //        _localizationManager.GetLocaleResourceString("ActivityLog.PlaceOrder"),
                        //        order.Id);
                        //}

                        //uncomment this line to support transactions
                        //scope.Complete();


                        //TODO raise event             
                        //EventContext.Current.OnOrderPlaced(null, new OrderEventArgs() { Order = order });

                        //TODO raise event         
                        //if (order.PaymentStatus == PaymentStatus.Paid)
                        //    EventContext.Current.OnOrderPaid(null, new OrderEventArgs() { Order = order });

                        #endregion
                    }
                }
            }
            catch (Exception exc)
            {
                if (result == null)
                    result = new PlaceOrderResult();
                result.AddError(string.Format("Error: {0}. Full exception: {1}", exc.Message, exc.ToString()));
            }

            #region Process errors

            string error = "";
            for (int i = 0; i < result.Errors.Count; i++)
            {
                error += string.Format("Error {0}: {1}", i, result.Errors[i]);
                if (i != result.Errors.Count - 1)
                    error += ". ";
            }
            if (!String.IsNullOrEmpty(error))
            {
                //log it
                string logError = string.Format("Error while placing order. {0}", error);
                _logger.InsertLog(LogLevel.Error, logError, logError);
            }

            #endregion

            return result;
        }



        /// <summary>
        /// Process next recurring psayment
        /// </summary>
        /// <param name="recurringPayment">Recurring payment</param>
        public void ProcessNextRecurringPayment(RecurringPayment recurringPayment)
        {
            if (recurringPayment == null)
                throw new ArgumentNullException("recurringPayment");
            try
            {
                if (!recurringPayment.IsActive)
                    throw new NopException("Recurring payment is not active");

                var initialOrder = recurringPayment.InitialOrder;
                if (initialOrder == null)
                    throw new NopException("Initial order could not be loaded");

                var customer = initialOrder.Customer;
                if (customer == null)
                    throw new NopException("Customer could not be loaded");

                var nextPaymentDate = recurringPayment.NextPaymentDate;
                if (!nextPaymentDate.HasValue)
                    throw new NopException("Next payment date could not be calculated");

                //payment info
                var paymentInfo = new ProcessPaymentRequest()
                {
                    Customer = customer,
                    OrderGuid = Guid.NewGuid(),
                    IsRecurringPayment = true,
                    InitialOrder = initialOrder,
                    RecurringCycleLength = recurringPayment.CycleLength,
                    RecurringCyclePeriod = recurringPayment.CyclePeriod,
                    RecurringTotalCycles = recurringPayment.TotalCycles,
                };

                //place a new order
                var result = this.PlaceOrder(paymentInfo);
                if (result.Success)
                {
                    if (result.PlacedOrder == null)
                        throw new NopException("Placed order could not be loaded");

                    var rph = new RecurringPaymentHistory()
                    {
                        RecurringPayment = recurringPayment,
                        CreatedOnUtc = DateTime.UtcNow,
                        Order = result.PlacedOrder,
                    };
                    _orderService.UpdateRecurringPayment(recurringPayment);
                }
                else
                {
                    string error = "";
                    for (int i = 0; i < result.Errors.Count; i++)
                    {
                        error += string.Format("Error {0}: {1}", i, result.Errors[i]);
                        if (i != result.Errors.Count - 1)
                            error += ". ";
                    }
                    throw new NopException(error);
                }
            }
            catch (Exception exc)
            {
                _logger.InsertLog(LogLevel.Error, string.Format("Error while processing recurring order. {0}", exc.Message), exc);
                throw;
            }
        }

        /// <summary>
        /// Cancels a recurring payment
        /// </summary>
        /// <param name="recurringPayment">Recurring payment</param>
        public IList<string> CancelRecurringPayment(RecurringPayment recurringPayment)
        {
            if (recurringPayment == null)
                throw new ArgumentNullException("recurringPayment");

            var initialOrder = recurringPayment.InitialOrder;
            if (initialOrder == null)
                return new List<string>() { "Initial order could not be loaded" };


            var request = new CancelRecurringPaymentRequest();
            CancelRecurringPaymentResult result = null;
            try
            {
                request.Order = initialOrder;
                result = _paymentService.CancelRecurringPayment(request);
                if (result.Success)
                {
                    //update recurring payment
                    recurringPayment.IsActive = false;
                    _orderService.UpdateRecurringPayment(recurringPayment);


                    //add a note
                    initialOrder.OrderNotes.Add(new OrderNote()
                    {
                        Note = "Recurring payment has been cancelled",
                        DisplayToCustomer = false,
                        CreatedOnUtc = DateTime.UtcNow
                    });
                    _orderService.UpdateOrder(initialOrder);
                }
            }
            catch (Exception exc)
            {
                if (result == null)
                    result = new CancelRecurringPaymentResult();
                result.AddError(string.Format("Error: {0}. Full exception: {1}", exc.Message, exc.ToString()));
            }


            //process errors
            string error = "";
            for (int i = 0; i < result.Errors.Count; i++)
            {
                error += string.Format("Error {0}: {1}", i, result.Errors[i]);
                if (i != result.Errors.Count - 1)
                    error += ". ";
            }
            if (!String.IsNullOrEmpty(error))
            {
                //add a note
                initialOrder.OrderNotes.Add(new OrderNote()
                {
                    Note = string.Format("Unable to cancel recurring payment. {0}", error),
                    DisplayToCustomer = false,
                    CreatedOnUtc = DateTime.UtcNow
                });
                _orderService.UpdateOrder(initialOrder);

                //log it
                string logError = string.Format("Error cancelling recurring payment. Order #{0}. Error: {1}", initialOrder.Id, error);
                _logger.InsertLog(LogLevel.Error, logError, logError);
            }
            return result.Errors;
        }

        /// <summary>
        /// Gets a value indicating whether a customer can cancel recurring payment
        /// </summary>
        /// <param name="customerToValidate">Customer</param>
        /// <param name="recurringPayment">Recurring Payment</param>
        /// <returns>value indicating whether a customer can cancel recurring payment</returns>
        public bool CanCancelRecurringPayment(Customer customerToValidate, RecurringPayment recurringPayment)
        {
            if (recurringPayment == null)
                return false;

            if (customerToValidate == null)
                return false;

            var initialOrder = recurringPayment.InitialOrder;
            if (initialOrder == null)
                return false;

            var customer = recurringPayment.InitialOrder.Customer;
            if (customer == null)
                return false;

            if (initialOrder.OrderStatus == OrderStatus.Cancelled)
                return false;

            //TODO check whether customer is admin
            //if (!customerToValidate.IsAdmin)
            {
                if (customer.Id != customerToValidate.Id)
                    return false;
            }

            if (!recurringPayment.NextPaymentDate.HasValue)
                return false;

            return true;
        }



        /// <summary>
        /// Gets a value indicating whether shipping is allowed
        /// </summary>
        /// <param name="order">Order</param>
        /// <returns>A value indicating whether shipping is allowed</returns>
        public bool CanShip(Order order)
        {
            if (order == null)
                return false;

            if (order.OrderStatus == OrderStatus.Cancelled)
                return false;

            if (order.ShippingStatus == ShippingStatus.NotYetShipped)
                return true;

            return false;
        }

        /// <summary>
        /// Ships order
        /// </summary>
        /// <param name="order">Order</param>
        /// <param name="notifyCustomer">True to notify customer</param>
        public void Ship(Order order, bool notifyCustomer)
        {
            if (order == null)
                throw new ArgumentNullException("order");

            if (!CanShip(order))
                throw new NopException("Can not do shipment for order.");

            order.ShippedDateUtc = DateTime.UtcNow;
            order.ShippingStatusId = (int)ShippingStatus.Shipped;
            _orderService.UpdateOrder(order);

            //add a note
            order.OrderNotes.Add(new OrderNote()
                {
                    Note = "Order has been shipped",
                    DisplayToCustomer = false,
                    CreatedOnUtc = DateTime.UtcNow
                });
            _orderService.UpdateOrder(order);

            if (notifyCustomer)
            {
                //TODO notify customer
                //int orderShippedCustomerNotificationQueuedEmailId = _messageService.SendOrderShippedCustomerNotification(order, order.CustomerLanguageId);
                //if (orderShippedCustomerNotificationQueuedEmailId > 0)
                //{
                //    order.OrderNotes.Add(new OrderNote()
                //    {
                //        Note = string.Format("\"Shipped\" email (to customer) has been queued. Queued email identifier: {0}.", orderShippedCustomerNotificationQueuedEmailId),
                //        DisplayToCustomer = false,
                //        CreatedOnUtc = DateTime.UtcNow
                //    });
                //    _orderService.UpdateOrder(order);
                //}
            }

            //check order status
            CheckOrderStatus(order);
        }
        
        /// <summary>
        /// Gets a value indicating whether order is delivered
        /// </summary>
        /// <param name="order">Order</param>
        /// <returns>A value indicating whether shipping is delivered</returns>
        public bool CanDeliver(Order order)
        {
            if (order == null)
                return false;

            if (order.OrderStatus == OrderStatus.Cancelled)
                return false;

            if (order.ShippingStatus == ShippingStatus.Shipped)
                return true;

            return false;
        }

        /// <summary>
        /// Marks order status as delivered
        /// </summary>
        /// <param name="order">Order</param>
        /// <param name="notifyCustomer">True to notify customer</param>
        public void Deliver(Order order, bool notifyCustomer)
        {
            if (order == null)
                throw new ArgumentNullException("order");

            if (!CanDeliver(order))
                throw new NopException("Can not do delivery for order.");

            order.DeliveryDateUtc = DateTime.UtcNow;
            order.ShippingStatusId = (int)ShippingStatus.Delivered;
            _orderService.UpdateOrder(order);

            //add a note
            order.OrderNotes.Add(new OrderNote()
            {
                Note = "Order has been delivered",
                DisplayToCustomer = false,
                CreatedOnUtc = DateTime.UtcNow
            });
            _orderService.UpdateOrder(order);

            if (notifyCustomer)
            {
                //TODO send email notification
                //int orderDeliveredCustomerNotificationQueuedEmailId = _messageService.SendOrderDeliveredCustomerNotification(order, order.CustomerLanguageId);
                //if (orderDeliveredCustomerNotificationQueuedEmailId > 0)
                //{
                //    order.OrderNotes.Add(new OrderNote()
                //    {
                //        Note = string.Format("\"Delivered\" email (to customer) has been queued. Queued email identifier: {0}.", orderDeliveredCustomerNotificationQueuedEmailId),
                //        DisplayToCustomer = false,
                //        CreatedOnUtc = DateTime.UtcNow
                //    });
                //    _orderService.UpdateOrder(order);
                //}
            }

            //check order status
            CheckOrderStatus(order);
        }



        /// <summary>
        /// Gets a value indicating whether cancel is allowed
        /// </summary>
        /// <param name="order">Order</param>
        /// <returns>A value indicating whether cancel is allowed</returns>
        public bool CanCancelOrder(Order order)
        {
            if (order == null)
                return false;

            if (order.OrderStatus == OrderStatus.Cancelled)
                return false;

            return true;
        }

        /// <summary>
        /// Cancels order
        /// </summary>
        /// <param name="order">Order</param>
        /// <param name="notifyCustomer">True to notify customer</param>
        public void CancelOrder(Order order, bool notifyCustomer)
        {
            if (order == null)
                throw new ArgumentNullException("order");

            if (!CanCancelOrder(order))
                throw new NopException("Can not do cancel for order.");

            //Cancel order
            SetOrderStatus(order, OrderStatus.Cancelled, notifyCustomer);

            //add a note
            order.OrderNotes.Add(new OrderNote()
            {
                Note = "Order has been cancelled",
                DisplayToCustomer = false,
                CreatedOnUtc = DateTime.UtcNow
            });
            _orderService.UpdateOrder(order);

            //cancel recurring payments
            var recurringPayments = _orderService.SearchRecurringPayments(0, order.Id, null);
            foreach (var rp in recurringPayments)
            {
                var errors = CancelRecurringPayment(rp);
            }

            //Adjust inventory
            foreach (var opv in order.OrderProductVariants)
                _productService.AdjustInventory(opv.ProductVariant, false, opv.Quantity, opv.AttributesXml);
        }



        /// <summary>
        /// Gets a value indicating whether order can be marked as authorized
        /// </summary>
        /// <param name="order">Order</param>
        /// <returns>A value indicating whether order can be marked as authorized</returns>
        public bool CanMarkOrderAsAuthorized(Order order)
        {
            if (order == null)
                return false;

            if (order.OrderStatus == OrderStatus.Cancelled)
                return false;

            if (order.PaymentStatus == PaymentStatus.Pending)
                return true;

            return false;
        }

        /// <summary>
        /// Marks order as authorized
        /// </summary>
        /// <param name="order">Order</param>
        public void MarkAsAuthorized(Order order)
        {
            if (order == null)
                throw new ArgumentNullException("order");

            order.PaymentStatusId = (int)PaymentStatus.Authorized;
            _orderService.UpdateOrder(order);

            //add a note
            order.OrderNotes.Add(new OrderNote()
            {
                Note = "Order has been marked as authorized",
                DisplayToCustomer = false,
                CreatedOnUtc = DateTime.UtcNow
            });
            _orderService.UpdateOrder(order);

            //check order status
            CheckOrderStatus(order);
        }



        /// <summary>
        /// Gets a value indicating whether capture from admin panel is allowed
        /// </summary>
        /// <param name="order">Order</param>
        /// <returns>A value indicating whether capture from admin panel is allowed</returns>
        public bool CanCapture(Order order)
        {
            if (order == null)
                return false;

            if (order.OrderStatus == OrderStatus.Cancelled ||
                order.OrderStatus == OrderStatus.Pending)
                return false;

            if (order.PaymentStatus == PaymentStatus.Authorized &&
                _paymentService.SupportCapture(order.PaymentMethodSystemName))
                return true;

            return false;
        }

        /// <summary>
        /// Capture an order (from admin panel)
        /// </summary>
        /// <param name="order">Order</param>
        /// <returns>A list of errors; empty list if no errors</returns>
        public IList<string> Capture(Order order)
        {
            if (order == null)
                throw new ArgumentNullException("order");

            if (!CanCapture(order))
                throw new NopException("Can not do capture for order.");

            var request = new CapturePaymentRequest();
            CapturePaymentResult result = null;
            try
            {
                //old info from placing order
                request.Order = order;
                result = _paymentService.Capture(request);

                if (result.Success)
                {
                    var paidDate = order.PaidDateUtc;
                    if (result.NewPaymentStatus == PaymentStatus.Paid)
                        paidDate = DateTime.UtcNow;

                    order.CaptureTransactionId = result.CaptureTransactionId;
                    order.CaptureTransactionResult = result.CaptureTransactionResult;
                    order.PaymentStatus = result.NewPaymentStatus;
                    order.PaidDateUtc = paidDate;
                    _orderService.UpdateOrder(order);

                    //add a note
                    order.OrderNotes.Add(new OrderNote()
                    {
                        Note = "Order has been captured",
                        DisplayToCustomer = false,
                        CreatedOnUtc = DateTime.UtcNow
                    });
                    _orderService.UpdateOrder(order);

                    CheckOrderStatus(order);

                    //TODO raise event         
                    //if (order.PaymentStatus == PaymentStatus.Paid)
                    //{
                    //    EventContext.Current.OnOrderPaid(null,
                    //        new OrderEventArgs() { Order = order });
                    //}
                }
            }
            catch (Exception exc)
            {
                if (result == null)
                    result = new CapturePaymentResult();
                result.AddError(string.Format("Error: {0}. Full exception: {1}", exc.Message, exc.ToString()));
            }


            //process errors
            string error = "";
            for (int i = 0; i < result.Errors.Count; i++)
            {
                error += string.Format("Error {0}: {1}", i, result.Errors[i]);
                if (i != result.Errors.Count - 1)
                    error += ". ";
            }
            if (!String.IsNullOrEmpty(error))
            {
                //add a note
                order.OrderNotes.Add(new OrderNote()
                {
                    Note = string.Format("Unable to capture order. {0}", error),
                    DisplayToCustomer = false,
                    CreatedOnUtc = DateTime.UtcNow
                });
                _orderService.UpdateOrder(order);

                //log it
                string logError = string.Format("Error capturing order #{0}. Error: {1}", order.Id, error);
                _logger.InsertLog(LogLevel.Error, logError, logError);
            }
            return result.Errors;
        }

        /// <summary>
        /// Gets a value indicating whether order can be marked as paid
        /// </summary>
        /// <param name="order">Order</param>
        /// <returns>A value indicating whether order can be marked as paid</returns>
        public bool CanMarkOrderAsPaid(Order order)
        {
            if (order == null)
                return false;

            if (order.OrderStatus == OrderStatus.Cancelled)
                return false;

            if (order.PaymentStatus == PaymentStatus.Paid ||
                order.PaymentStatus == PaymentStatus.Refunded ||
                order.PaymentStatus == PaymentStatus.Voided)
                return false;

            return true;
        }

        /// <summary>
        /// Marks order as paid
        /// </summary>
        /// <param name="order">Order</param>
        public void MarkOrderAsPaid(Order order)
        {
            if (order == null)
                throw new ArgumentNullException("order");

            if (!CanMarkOrderAsPaid(order))
                throw new NopException("You can't mark this order as paid");

            order.PaymentStatusId = (int)PaymentStatus.Paid;
            order.PaidDateUtc = DateTime.UtcNow;
            _orderService.UpdateOrder(order);

            //add a note
            order.OrderNotes.Add(new OrderNote()
            {
                Note = "Order has been marked as paid",
                DisplayToCustomer = false,
                CreatedOnUtc = DateTime.UtcNow
            });
            _orderService.UpdateOrder(order);

            CheckOrderStatus(order);

            //TODO raise event         
            //if (order.PaymentStatus == PaymentStatus.Paid)
            //{
            //    EventContext.Current.OnOrderPaid(null,
            //        new OrderEventArgs() { Order = order });
            //}
        }



        /// <summary>
        /// Gets a value indicating whether refund from admin panel is allowed
        /// </summary>
        /// <param name="order">Order</param>
        /// <returns>A value indicating whether refund from admin panel is allowed</returns>
        public bool CanRefund(Order order)
        {
            if (order == null)
                return false;

            if (order.OrderTotal == decimal.Zero)
                return false;

            if (order.OrderStatus == OrderStatus.Cancelled)
                return false;

            if (order.PaymentStatus == PaymentStatus.Paid &&
                _paymentService.SupportRefund(order.PaymentMethodSystemName))
                return true;

            return false;
        }
        
        /// <summary>
        /// Refunds an order (from admin panel)
        /// </summary>
        /// <param name="order">Order</param>
        /// <returns>A list of errors; empty list if no errors</returns>
        public IList<string> Refund(Order order)
        {
            if (order == null)
                throw new ArgumentNullException("order");

            if (!CanRefund(order))
                throw new NopException("Can not do refund for order.");

            var request = new RefundPaymentRequest();
            RefundPaymentResult result = null;
            try
            {
                request.Order = order;
                request.AmountToRefund = order.OrderTotal;
                request.IsPartialRefund = false;
                result = _paymentService.Refund(request);
                if (result.Success)
                {
                    //total amount refunded
                    decimal totalAmountRefunded = order.RefundedAmount + request.AmountToRefund;

                    //update order info
                    order.RefundedAmount = totalAmountRefunded;
                    order.PaymentStatus = result.NewPaymentStatus;
                    _orderService.UpdateOrder(order);

                    //add a note
                    order.OrderNotes.Add(new OrderNote()
                    {
                        Note = string.Format("Order has been refunded. Amount = {0}", _priceFormatter.FormatPrice(request.AmountToRefund, true, false)),
                        DisplayToCustomer = false,
                        CreatedOnUtc = DateTime.UtcNow
                    });
                    _orderService.UpdateOrder(order);

                    //check order status
                    CheckOrderStatus(order);
                }

            }
            catch (Exception exc)
            {
                if (result == null)
                    result = new RefundPaymentResult();
                result.AddError(string.Format("Error: {0}. Full exception: {1}", exc.Message, exc.ToString()));
            }

            //process errors
            string error = "";
            for (int i = 0; i < result.Errors.Count; i++)
            {
                error += string.Format("Error {0}: {1}", i, result.Errors[i]);
                if (i != result.Errors.Count - 1)
                    error += ". ";
            }
            if (!String.IsNullOrEmpty(error))
            {
                //add a note
                order.OrderNotes.Add(new OrderNote()
                {
                    Note = string.Format("Unable to refund order. {0}", error),
                    DisplayToCustomer = false,
                    CreatedOnUtc = DateTime.UtcNow
                });
                _orderService.UpdateOrder(order);

                //log it
                string logError = string.Format("Error refunding order #{0}. Error: {1}", order.Id, error);
                _logger.InsertLog(LogLevel.Error, logError, logError);
            }
            return result.Errors;
        }

        /// <summary>
        /// Gets a value indicating whether order can be marked as refunded
        /// </summary>
        /// <param name="order">Order</param>
        /// <returns>A value indicating whether order can be marked as refunded</returns>
        public bool CanRefundOffline(Order order)
        {
            if (order == null)
                return false;

            if (order.OrderTotal == decimal.Zero)
                return false;

            if (order.OrderStatus == OrderStatus.Cancelled)
                return false;

            if (order.PaymentStatus == PaymentStatus.Paid)
                return true;

            return false;
        }

        /// <summary>
        /// Refunds an order (offline)
        /// </summary>
        /// <param name="order">Order</param>
        public void RefundOffline(Order order)
        {
            if (order == null)
                throw new ArgumentNullException("order");

            if (!CanRefundOffline(order))
                throw new NopException("You can't refund this order");

            //amout to refund
            decimal amountToRefund = order.OrderTotal;

            //total amount refunded
            decimal totalAmountRefunded = order.RefundedAmount + amountToRefund;

            //update order info
            order.RefundedAmount = totalAmountRefunded;
            order.PaymentStatus = PaymentStatus.Refunded;
            _orderService.UpdateOrder(order);

            //add a note
            order.OrderNotes.Add(new OrderNote()
            {
                Note = string.Format("Order has been marked as refunded. Amount = {0}", _priceFormatter.FormatPrice(amountToRefund, true, false)),
                DisplayToCustomer = false,
                CreatedOnUtc = DateTime.UtcNow
            });
            _orderService.UpdateOrder(order);

            //check order status
            CheckOrderStatus(order);
        }

        /// <summary>
        /// Gets a value indicating whether partial refund from admin panel is allowed
        /// </summary>
        /// <param name="order">Order</param>
        /// <param name="amountToRefund">Amount to refund</param>
        /// <returns>A value indicating whether refund from admin panel is allowed</returns>
        public bool CanPartiallyRefund(Order order, decimal amountToRefund)
        {
            if (order == null)
                return false;

            if (order.OrderTotal == decimal.Zero)
                return false;

            if (order.OrderStatus == OrderStatus.Cancelled)
                return false;

            decimal canBeRefunded = order.OrderTotal - order.RefundedAmount;
            if (canBeRefunded <= decimal.Zero)
                return false;

            if (amountToRefund > canBeRefunded)
                return false;

            if ((order.PaymentStatus == PaymentStatus.Paid ||
                order.PaymentStatus == PaymentStatus.PartiallyRefunded) &&
                _paymentService.SupportPartiallyRefund(order.PaymentMethodSystemName))
                return true;

            return false;
        }

        /// <summary>
        /// Partially refunds an order (from admin panel)
        /// </summary>
        /// <param name="order">Order</param>
        /// <param name="amountToRefund">Amount to refund</param>
        /// <returns>A list of errors; empty list if no errors</returns>
        public IList<string> PartiallyRefund(Order order, decimal amountToRefund)
        {
            if (order == null)
                throw new ArgumentNullException("order");

            amountToRefund = Math.Round(amountToRefund, 2);

            if (!CanPartiallyRefund(order, amountToRefund))
                throw new NopException("Can not do partial refund for order.");

            var request = new RefundPaymentRequest();
            RefundPaymentResult result = null;
            try
            {
                request.Order = order;
                request.AmountToRefund = amountToRefund;
                request.IsPartialRefund = true;

                result = _paymentService.Refund(request);

                if (result.Success)
                {
                    //total amount refunded
                    decimal totalAmountRefunded = order.RefundedAmount + amountToRefund;

                    //update order info
                    order.RefundedAmount = totalAmountRefunded;
                    order.PaymentStatus = result.NewPaymentStatus;
                    _orderService.UpdateOrder(order);


                    //add a note
                    order.OrderNotes.Add(new OrderNote()
                    {
                        Note = string.Format("Order has been partially refunded. Amount = {0}", _priceFormatter.FormatPrice(amountToRefund, true, false)),
                        DisplayToCustomer = false,
                        CreatedOnUtc = DateTime.UtcNow
                    });
                    _orderService.UpdateOrder(order);

                    //check order status
                    CheckOrderStatus(order);
                }
            }
            catch (Exception exc)
            {
                if (result == null)
                    result = new RefundPaymentResult();
                result.AddError(string.Format("Error: {0}. Full exception: {1}", exc.Message, exc.ToString()));
            }

            //process errors
            string error = "";
            for (int i = 0; i < result.Errors.Count; i++)
            {
                error += string.Format("Error {0}: {1}", i, result.Errors[i]);
                if (i != result.Errors.Count - 1)
                    error += ". ";
            }
            if (!String.IsNullOrEmpty(error))
            {
                //add a note
                order.OrderNotes.Add(new OrderNote()
                {
                    Note = string.Format("Unable to partially refund order. {0}", error),
                    DisplayToCustomer = false,
                    CreatedOnUtc = DateTime.UtcNow
                });
                _orderService.UpdateOrder(order);

                //log it
                string logError = string.Format("Error refunding order #{0}. Error: {1}", order.Id, error);
                _logger.InsertLog(LogLevel.Error, logError, logError);
            }
            return result.Errors;
        }

        /// <summary>
        /// Gets a value indicating whether order can be marked as partially refunded
        /// </summary>
        /// <param name="order">Order</param>
        /// <param name="amountToRefund">Amount to refund</param>
        /// <returns>A value indicating whether order can be marked as partially refunded</returns>
        public bool CanPartiallyRefundOffline(Order order, decimal amountToRefund)
        {
            if (order == null)
                return false;

            if (order.OrderTotal == decimal.Zero)
                return false;

            if (order.OrderStatus == OrderStatus.Cancelled)
                return false;

            decimal canBeRefunded = order.OrderTotal - order.RefundedAmount;
            if (canBeRefunded <= decimal.Zero)
                return false;

            if (amountToRefund > canBeRefunded)
                return false;

            if (order.PaymentStatus == PaymentStatus.Paid ||
                order.PaymentStatus == PaymentStatus.PartiallyRefunded)
                return true;

            return false;
        }

        /// <summary>
        /// Partially refunds an order (offline)
        /// </summary>
        /// <param name="order">Order</param>
        /// <param name="amountToRefund">Amount to refund</param>
        public void PartiallyRefundOffline(Order order, decimal amountToRefund)
        {
            if (order == null)
                throw new ArgumentNullException("order");

            amountToRefund = Math.Round(amountToRefund, 2);

            if (!CanPartiallyRefundOffline(order, amountToRefund))
                throw new NopException("You can't partially refund (offline) this order");

            //total amount refunded
            decimal totalAmountRefunded = order.RefundedAmount + amountToRefund;

            //update order info
            order.RefundedAmount = totalAmountRefunded;
            order.PaymentStatus = PaymentStatus.PartiallyRefunded;
            _orderService.UpdateOrder(order);

            //add a note
            order.OrderNotes.Add(new OrderNote()
            {
                Note = string.Format("Order has been marked as partially refunded. Amount = {0}", _priceFormatter.FormatPrice(amountToRefund, true, false)),
                DisplayToCustomer = false,
                CreatedOnUtc = DateTime.UtcNow
            });
            _orderService.UpdateOrder(order);

            //check order status
            CheckOrderStatus(order);
        }



        /// <summary>
        /// Gets a value indicating whether void from admin panel is allowed
        /// </summary>
        /// <param name="order">Order</param>
        /// <returns>A value indicating whether void from admin panel is allowed</returns>
        public bool CanVoid(Order order)
        {
            if (order == null)
                return false;

            if (order.OrderTotal == decimal.Zero)
                return false;

            if (order.OrderStatus == OrderStatus.Cancelled)
                return false;

            if (order.PaymentStatus == PaymentStatus.Authorized &&
                _paymentService.SupportVoid(order.PaymentMethodSystemName))
                return true;

            return false;
        }

        /// <summary>
        /// Voids order (from admin panel)
        /// </summary>
        /// <param name="order">Order</param>
        /// <returns>Voided order</returns>
        public IList<string> Void(Order order)
        {
            if (order == null)
                throw new ArgumentNullException("order");

            if (!CanVoid(order))
                throw new NopException("Can not do void for order.");

            var request = new VoidPaymentRequest();
            VoidPaymentResult result = null;
            try
            {
                request.Order = order;
                result = _paymentService.Void(request);

                if (result.Success)
                {
                    //update order info
                    order.PaymentStatus = result.NewPaymentStatus;
                    _orderService.UpdateOrder(order);

                    //add a note
                    order.OrderNotes.Add(new OrderNote()
                    {
                        Note = "Order has been voided",
                        DisplayToCustomer = false,
                        CreatedOnUtc = DateTime.UtcNow
                    });
                    _orderService.UpdateOrder(order);

                    //check order status
                    CheckOrderStatus(order);
                }
            }
            catch (Exception exc)
            {
                if (result == null)
                    result = new VoidPaymentResult();
                result.AddError(string.Format("Error: {0}. Full exception: {1}", exc.Message, exc.ToString()));
            }

            //process errors
            string error = "";
            for (int i = 0; i < result.Errors.Count; i++)
            {
                error += string.Format("Error {0}: {1}", i, result.Errors[i]);
                if (i != result.Errors.Count - 1)
                    error += ". ";
            }
            if (!String.IsNullOrEmpty(error))
            {
                //add a note
                order.OrderNotes.Add(new OrderNote()
                {
                    Note = string.Format("Unable to voiding order. {0}", error),
                    DisplayToCustomer = false,
                    CreatedOnUtc = DateTime.UtcNow
                });
                _orderService.UpdateOrder(order);

                //log it
                string logError = string.Format("Error voiding order #{0}. Error: {1}", order.Id, error);
                _logger.InsertLog(LogLevel.Error, logError, logError);
            }
            return result.Errors;
        }

        /// <summary>
        /// Gets a value indicating whether order can be marked as voided
        /// </summary>
        /// <param name="order">Order</param>
        /// <returns>A value indicating whether order can be marked as voided</returns>
        public bool CanVoidOffline(Order order)
        {
            if (order == null)
                return false;

            if (order.OrderTotal == decimal.Zero)
                return false;

            if (order.OrderStatus == OrderStatus.Cancelled)
                return false;

            if (order.PaymentStatus == PaymentStatus.Authorized)
                return true;

            return false;
        }

        /// <summary>
        /// Voids order (offline)
        /// </summary>
        /// <param name="order">Order</param>
        public void VoidOffline(Order order)
        {
            if (order == null)
                throw new ArgumentNullException("order");

            if (!CanVoidOffline(order))
                throw new NopException("You can't void this order");

            order.PaymentStatusId = (int)PaymentStatus.Voided;
            _orderService.UpdateOrder(order);

            //add a note
            order.OrderNotes.Add(new OrderNote()
            {
                Note = "Order has been marked as voided",
                DisplayToCustomer = false,
                CreatedOnUtc = DateTime.UtcNow
            });
            _orderService.UpdateOrder(order);

            //check orer status
            CheckOrderStatus(order);
        }



        /// <summary>
        /// Place order items in current user shopping cart.
        /// </summary>
        /// <param name="order">The order</param>
        public void ReOrder(Order order)
        {
            if (order == null)
                throw new ArgumentNullException("order");

            foreach (var opv in order.OrderProductVariants)
            {
                _shoppingCartService.AddToCart(opv.Order.Customer, opv.ProductVariant,
                     ShoppingCartType.ShoppingCart, opv.AttributesXml,
                    opv.UnitPriceExclTax, opv.Quantity);
            }
        }

        /// <summary>
        /// Gets a value indicating whether download is allowed
        /// </summary>
        /// <param name="orderProductVariant">Order produvt variant to check</param>
        /// <returns>True if download is allowed; otherwise, false.</returns>
        public bool IsDownloadAllowed(OrderProductVariant orderProductVariant)
        {
            if (orderProductVariant == null)
                return false;

            var order = orderProductVariant.Order;
            if (order == null || order.Deleted)
                return false;

            //order status
            if (order.OrderStatus == OrderStatus.Cancelled)
                return false;

            var productVariant = orderProductVariant.ProductVariant;
            if (productVariant == null || !productVariant.IsDownload)
                return false;

            //payment status
            switch (productVariant.DownloadActivationType)
            {
                case DownloadActivationType.WhenOrderIsPaid:
                    {
                        if (order.PaymentStatus == PaymentStatus.Paid && order.PaidDateUtc.HasValue)
                        {
                            //expiration date
                            if (productVariant.DownloadExpirationDays.HasValue)
                            {
                                if (order.PaidDateUtc.Value.AddDays(productVariant.DownloadExpirationDays.Value) > DateTime.UtcNow)
                                {
                                    return true;
                                }
                            }
                            else
                            {
                                return true;
                            }
                        }
                    }
                    break;
                case DownloadActivationType.Manually:
                    {
                        if (orderProductVariant.IsDownloadActivated)
                        {
                            //expiration date
                            if (productVariant.DownloadExpirationDays.HasValue)
                            {
                                if (order.CreatedOnUtc.AddDays(productVariant.DownloadExpirationDays.Value) > DateTime.UtcNow)
                                {
                                    return true;
                                }
                            }
                            else
                            {
                                return true;
                            }
                        }
                    }
                    break;
                default:
                    break;
            }

            return false;
        }

        /// <summary>
        /// Gets a value indicating whether license download is allowed
        /// </summary>
        /// <param name="orderProductVariant">Order produvt variant to check</param>
        /// <returns>True if license download is allowed; otherwise, false.</returns>
        public bool IsLicenseDownloadAllowed(OrderProductVariant orderProductVariant)
        {
            if (orderProductVariant == null)
                return false;

            return IsDownloadAllowed(orderProductVariant) && orderProductVariant.LicenseDownloadId > 0;
        }

        

        /// <summary>
        /// Check whether return request is allowed
        /// </summary>
        /// <param name="order">Order</param>
        /// <returns>Result</returns>
        public bool IsReturnRequestAllowed(Order order)
        {
            if (!_orderSettings.ReturnRequestsEnabled)
                return false;

            if (order == null || order.Deleted)
                return false;

            return order.OrderStatus == OrderStatus.Complete;
        }
        


        /// <summary>
        /// Valdiate minimum order sub-total amount
        /// </summary>
        /// <param name="cart">Shopping cart</param>
        /// <returns>true - OK; false - minimum order sub-total amount is not reached</returns>
        public bool ValidateMinOrderSubtotalAmount(IList<ShoppingCartItem> cart)
        {
            if (cart == null)
                throw new ArgumentNullException("cart");

            bool result = true;
            //min order amount sub-total validation
            if (cart.Count > 0 && _orderSettings.MinOrderSubtotalAmount > decimal.Zero)
            {
                //subtotal
                decimal orderSubTotalDiscountAmountBase = decimal.Zero;
                Discount orderSubTotalAppliedDiscount = null;
                decimal subTotalWithoutDiscountBase = decimal.Zero;
                decimal subTotalWithDiscountBase = decimal.Zero;
                _orderTotalCalculationService.GetShoppingCartSubTotal(cart,
                    out orderSubTotalDiscountAmountBase, out orderSubTotalAppliedDiscount,
                out subTotalWithoutDiscountBase, out subTotalWithDiscountBase);

                if (subTotalWithoutDiscountBase < _orderSettings.MinOrderSubtotalAmount)
                    return false;
            }

            return true;
        }

        /// <summary>
        /// Valdiate minimum order total amount
        /// </summary>
        /// <param name="cart">Shopping cart</param>
        /// <returns>true - OK; false - minimum order total amount is not reached</returns>
        public bool ValidateMinOrderTotalAmount(IList<ShoppingCartItem> cart)
        {
            if (cart == null)
                throw new ArgumentNullException("cart");

            if (cart.Count > 0 && _orderSettings.MinOrderTotalAmount > decimal.Zero)
            {
                decimal? shoppingCartTotalBase = _orderTotalCalculationService.GetShoppingCartTotal(cart);
                if (shoppingCartTotalBase.HasValue && shoppingCartTotalBase.Value < _orderSettings.MinOrderTotalAmount)
                    return false;
            }

            return true;
        }

        #endregion
    }
}