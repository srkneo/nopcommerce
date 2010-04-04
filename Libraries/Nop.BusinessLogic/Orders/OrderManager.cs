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
using System.Text;
using System.Web;
using NopSolutions.NopCommerce.BusinessLogic.Audit;
using NopSolutions.NopCommerce.BusinessLogic.Caching;
using NopSolutions.NopCommerce.BusinessLogic.Configuration.Settings;
using NopSolutions.NopCommerce.BusinessLogic.CustomerManagement;
using NopSolutions.NopCommerce.BusinessLogic.Directory;
using NopSolutions.NopCommerce.BusinessLogic.Localization;
using NopSolutions.NopCommerce.BusinessLogic.Messages;
using NopSolutions.NopCommerce.BusinessLogic.Payment;
using NopSolutions.NopCommerce.BusinessLogic.Products;
using NopSolutions.NopCommerce.BusinessLogic.Products.Attributes;
using NopSolutions.NopCommerce.BusinessLogic.Profile;
using NopSolutions.NopCommerce.BusinessLogic.Promo.Discounts;
using NopSolutions.NopCommerce.BusinessLogic.Security;
using NopSolutions.NopCommerce.BusinessLogic.Shipping;
using NopSolutions.NopCommerce.BusinessLogic.Tax;
using NopSolutions.NopCommerce.BusinessLogic.Utils;
using NopSolutions.NopCommerce.BusinessLogic.Utils.Html;
using NopSolutions.NopCommerce.Common;
using NopSolutions.NopCommerce.Common.Utils;
using NopSolutions.NopCommerce.Common.Utils.Html;
using NopSolutions.NopCommerce.DataAccess;
using NopSolutions.NopCommerce.DataAccess.Orders;

namespace NopSolutions.NopCommerce.BusinessLogic.Orders
{
    /// <summary>
    /// Order manager
    /// </summary>
    public partial class OrderManager
    {
        #region Constants
        private const string ORDERSTATUSES_ALL_KEY = "Nop.orderstatus.all";
        private const string ORDERSTATUSES_BY_ID_KEY = "Nop.orderstatus.id-{0}";
        private const string ORDERSTATUSES_PATTERN_KEY = "Nop.orderstatus.";
        #endregion

        #region Utilities

        private static OrderCollection DBMapping(DBOrderCollection dbCollection)
        {
            if (dbCollection == null)
                return null;

            var collection = new OrderCollection();
            foreach (var dbItem in dbCollection)
            {
                var item = DBMapping(dbItem);
                collection.Add(item);
            }

            return collection;
        }

        private static Order DBMapping(DBOrder dbItem)
        {
            if (dbItem == null)
                return null;

            var item = new Order();
            item.OrderID = dbItem.OrderID;
            item.OrderGUID = dbItem.OrderGUID;
            item.CustomerID = dbItem.CustomerID;
            item.CustomerLanguageID = dbItem.CustomerLanguageID;
            item.CustomerTaxDisplayTypeID = dbItem.CustomerTaxDisplayTypeID;
            item.CustomerIP = dbItem.CustomerIP;
            item.OrderSubtotalInclTax = dbItem.OrderSubtotalInclTax;
            item.OrderSubtotalExclTax = dbItem.OrderSubtotalExclTax;
            item.OrderShippingInclTax = dbItem.OrderShippingInclTax;
            item.OrderShippingExclTax = dbItem.OrderShippingExclTax;
            item.PaymentMethodAdditionalFeeInclTax = dbItem.PaymentMethodAdditionalFeeInclTax;
            item.PaymentMethodAdditionalFeeExclTax = dbItem.PaymentMethodAdditionalFeeExclTax;
            item.OrderTax = dbItem.OrderTax;
            item.OrderTotal = dbItem.OrderTotal;
            item.OrderDiscount = dbItem.OrderDiscount;
            item.OrderSubtotalInclTaxInCustomerCurrency = dbItem.OrderSubtotalInclTaxInCustomerCurrency;
            item.OrderSubtotalExclTaxInCustomerCurrency = dbItem.OrderSubtotalExclTaxInCustomerCurrency;
            item.OrderShippingInclTaxInCustomerCurrency = dbItem.OrderShippingInclTaxInCustomerCurrency;
            item.OrderShippingExclTaxInCustomerCurrency = dbItem.OrderShippingExclTaxInCustomerCurrency;
            item.PaymentMethodAdditionalFeeInclTaxInCustomerCurrency = dbItem.PaymentMethodAdditionalFeeInclTaxInCustomerCurrency;
            item.PaymentMethodAdditionalFeeExclTaxInCustomerCurrency = dbItem.PaymentMethodAdditionalFeeExclTaxInCustomerCurrency;
            item.OrderTaxInCustomerCurrency = dbItem.OrderTaxInCustomerCurrency;
            item.OrderTotalInCustomerCurrency = dbItem.OrderTotalInCustomerCurrency;
            item.OrderDiscountInCustomerCurrency = dbItem.OrderDiscountInCustomerCurrency;
            item.CustomerCurrencyCode = dbItem.CustomerCurrencyCode;
            item.OrderWeight = dbItem.OrderWeight;
            item.AffiliateID = dbItem.AffiliateID;
            item.OrderStatusID = dbItem.OrderStatusID;
            item.AllowStoringCreditCardNumber = dbItem.AllowStoringCreditCardNumber;
            item.CardType = dbItem.CardType;
            item.CardName = dbItem.CardName;
            item.CardNumber = dbItem.CardNumber;
            item.MaskedCreditCardNumber = dbItem.MaskedCreditCardNumber;
            item.CardCVV2 = dbItem.CardCVV2;
            item.CardExpirationMonth = dbItem.CardExpirationMonth;
            item.CardExpirationYear = dbItem.CardExpirationYear;
            item.PaymentMethodID = dbItem.PaymentMethodID;
            item.PaymentMethodName = dbItem.PaymentMethodName;
            item.AuthorizationTransactionID = dbItem.AuthorizationTransactionID;
            item.AuthorizationTransactionCode = dbItem.AuthorizationTransactionCode;
            item.AuthorizationTransactionResult = dbItem.AuthorizationTransactionResult;
            item.CaptureTransactionID = dbItem.CaptureTransactionID;
            item.CaptureTransactionResult = dbItem.CaptureTransactionResult;
            item.SubscriptionTransactionID = dbItem.SubscriptionTransactionID;
            item.PurchaseOrderNumber = dbItem.PurchaseOrderNumber;
            item.PaymentStatusID = dbItem.PaymentStatusID;
            item.PaidDate = dbItem.PaidDate;
            item.BillingFirstName = dbItem.BillingFirstName;
            item.BillingLastName = dbItem.BillingLastName;
            item.BillingPhoneNumber = dbItem.BillingPhoneNumber;
            item.BillingEmail = dbItem.BillingEmail;
            item.BillingFaxNumber = dbItem.BillingFaxNumber;
            item.BillingCompany = dbItem.BillingCompany;
            item.BillingAddress1 = dbItem.BillingAddress1;
            item.BillingAddress2 = dbItem.BillingAddress2;
            item.BillingCity = dbItem.BillingCity;
            item.BillingStateProvince = dbItem.BillingStateProvince;
            item.BillingStateProvinceID = dbItem.BillingStateProvinceID;
            item.BillingZipPostalCode = dbItem.BillingZipPostalCode;
            item.BillingCountry = dbItem.BillingCountry;
            item.BillingCountryID = dbItem.BillingCountryID;
            item.ShippingStatusID = dbItem.ShippingStatusID;
            item.ShippingFirstName = dbItem.ShippingFirstName;
            item.ShippingLastName = dbItem.ShippingLastName;
            item.ShippingPhoneNumber = dbItem.ShippingPhoneNumber;
            item.ShippingEmail = dbItem.ShippingEmail;
            item.ShippingFaxNumber = dbItem.ShippingFaxNumber;
            item.ShippingCompany = dbItem.ShippingCompany;
            item.ShippingAddress1 = dbItem.ShippingAddress1;
            item.ShippingAddress2 = dbItem.ShippingAddress2;
            item.ShippingCity = dbItem.ShippingCity;
            item.ShippingStateProvince = dbItem.ShippingStateProvince;
            item.ShippingStateProvinceID = dbItem.ShippingStateProvinceID;
            item.ShippingZipPostalCode = dbItem.ShippingZipPostalCode;
            item.ShippingCountry = dbItem.ShippingCountry;
            item.ShippingCountryID = dbItem.ShippingCountryID;
            item.ShippingMethod = dbItem.ShippingMethod;
            item.ShippingRateComputationMethodID = dbItem.ShippingRateComputationMethodID;
            item.ShippedDate = dbItem.ShippedDate;
            item.TrackingNumber = dbItem.TrackingNumber;
            item.Deleted = dbItem.Deleted;
            item.CreatedOn = dbItem.CreatedOn;

            return item;
        }

        private static OrderNoteCollection DBMapping(DBOrderNoteCollection dbCollection)
        {
            if (dbCollection == null)
                return null;

            var collection = new OrderNoteCollection();
            foreach (var dbItem in dbCollection)
            {
                var item = DBMapping(dbItem);
                collection.Add(item);
            }

            return collection;
        }

        private static OrderNote DBMapping(DBOrderNote dbItem)
        {
            if (dbItem == null)
                return null;

            var item = new OrderNote();
            item.OrderNoteID = dbItem.OrderNoteID;
            item.OrderID = dbItem.OrderID;
            item.Note = dbItem.Note;
            item.DisplayToCustomer = dbItem.DisplayToCustomer;
            item.CreatedOn = dbItem.CreatedOn;

            return item;
        }

        private static OrderProductVariantCollection DBMapping(DBOrderProductVariantCollection dbCollection)
        {
            if (dbCollection == null)
                return null;

            var collection = new OrderProductVariantCollection();
            foreach (var dbItem in dbCollection)
            {
                var item = DBMapping(dbItem);
                collection.Add(item);
            }

            return collection;
        }

        private static OrderProductVariant DBMapping(DBOrderProductVariant dbItem)
        {
            if (dbItem == null)
                return null;

            var item = new OrderProductVariant();
            item.OrderProductVariantID = dbItem.OrderProductVariantID;
            item.OrderProductVariantGUID = dbItem.OrderProductVariantGUID;
            item.OrderID = dbItem.OrderID;
            item.ProductVariantID = dbItem.ProductVariantID;
            item.UnitPriceInclTax = dbItem.UnitPriceInclTax;
            item.UnitPriceExclTax = dbItem.UnitPriceExclTax;
            item.PriceInclTax = dbItem.PriceInclTax;
            item.PriceExclTax = dbItem.PriceExclTax;
            item.UnitPriceInclTaxInCustomerCurrency = dbItem.UnitPriceInclTaxInCustomerCurrency;
            item.UnitPriceExclTaxInCustomerCurrency = dbItem.UnitPriceExclTaxInCustomerCurrency;
            item.PriceInclTaxInCustomerCurrency = dbItem.PriceInclTaxInCustomerCurrency;
            item.PriceExclTaxInCustomerCurrency = dbItem.PriceExclTaxInCustomerCurrency;
            item.AttributeDescription = dbItem.AttributeDescription;
            item.AttributesXML = dbItem.AttributesXML;
            item.Quantity = dbItem.Quantity;
            item.DiscountAmountInclTax = dbItem.DiscountAmountInclTax;
            item.DiscountAmountExclTax = dbItem.DiscountAmountExclTax;
            item.DownloadCount = dbItem.DownloadCount;
            item.IsDownloadActivated = dbItem.IsDownloadActivated;
            item.LicenseDownloadID = dbItem.LicenseDownloadID;

            return item;
        }

        private static OrderStatusCollection DBMapping(DBOrderStatusCollection dbCollection)
        {
            if (dbCollection == null)
                return null;

            var collection = new OrderStatusCollection();
            foreach (var dbItem in dbCollection)
            {
                var item = DBMapping(dbItem);
                collection.Add(item);
            }

            return collection;
        }

        private static OrderStatus DBMapping(DBOrderStatus dbItem)
        {
            if (dbItem == null)
                return null;

            var item = new OrderStatus();
            item.OrderStatusID = dbItem.OrderStatusID;
            item.Name = dbItem.Name;

            return item;
        }

        private static OrderAverageReportLine DBMapping(DBOrderAverageReportLine dbItem)
        {
            if (dbItem == null)
                return null;

            var item = new OrderAverageReportLine();
            item.SumTodayOrders = dbItem.SumTodayOrders;
            item.CountTodayOrders = dbItem.CountTodayOrders;
            item.SumThisWeekOrders = dbItem.SumThisWeekOrders;
            item.CountThisWeekOrders = dbItem.CountThisWeekOrders;
            item.SumThisMonthOrders = dbItem.SumThisMonthOrders;
            item.CountThisMonthOrders = dbItem.CountThisMonthOrders;
            item.SumThisYearOrders = dbItem.SumThisYearOrders;
            item.CountThisYearOrders = dbItem.CountThisYearOrders;
            item.SumAllTimeOrders = dbItem.SumAllTimeOrders;
            item.CountAllTimeOrders = dbItem.CountAllTimeOrders;

            return item;
        }

        private static List<BestSellersReportLine> DBMapping(List<DBBestSellersReportLine> dbCollection)
        {
            if (dbCollection == null)
                return null;

            var collection = new List<BestSellersReportLine>();
            foreach (var dbItem in dbCollection)
            {
                var item = DBMapping(dbItem);
                collection.Add(item);
            }

            return collection;
        }

        private static BestSellersReportLine DBMapping(DBBestSellersReportLine dbItem)
        {
            if (dbItem == null)
                return null;

            var item = new BestSellersReportLine();
            item.ProductVariantID = dbItem.ProductVariantID;
            item.SalesTotalCount = dbItem.SalesTotalCount;
            item.SalesTotalAmount = dbItem.SalesTotalAmount;

            return item;
        }

        private static RecurringPaymentCollection DBMapping(DBRecurringPaymentCollection dbCollection)
        {
            if (dbCollection == null)
                return null;

            var collection = new RecurringPaymentCollection();
            foreach (var dbItem in dbCollection)
            {
                var item = DBMapping(dbItem);
                collection.Add(item);
            }

            return collection;
        }

        private static RecurringPayment DBMapping(DBRecurringPayment dbItem)
        {
            if (dbItem == null)
                return null;

            var item = new RecurringPayment();
            item.RecurringPaymentID = dbItem.RecurringPaymentID;
            item.InitialOrderID = dbItem.InitialOrderID;
            item.CycleLength = dbItem.CycleLength;
            item.CyclePeriod = dbItem.CyclePeriod;
            item.TotalCycles = dbItem.TotalCycles;
            item.StartDate = dbItem.StartDate;
            item.IsActive = dbItem.IsActive;
            item.Deleted = dbItem.Deleted;
            item.CreatedOn = dbItem.CreatedOn;

            return item;
        }

        private static RecurringPaymentHistoryCollection DBMapping(DBRecurringPaymentHistoryCollection dbCollection)
        {
            if (dbCollection == null)
                return null;

            var collection = new RecurringPaymentHistoryCollection();
            foreach (var dbItem in dbCollection)
            {
                var item = DBMapping(dbItem);
                collection.Add(item);
            }

            return collection;
        }

        private static RecurringPaymentHistory DBMapping(DBRecurringPaymentHistory dbItem)
        {
            if (dbItem == null)
                return null;

            var item = new RecurringPaymentHistory();
            item.RecurringPaymentHistoryID = dbItem.RecurringPaymentHistoryID;
            item.RecurringPaymentID = dbItem.RecurringPaymentID;
            item.OrderID = dbItem.OrderID;
            item.CreatedOn = dbItem.CreatedOn;

            return item;
        }

        private static GiftCardCollection DBMapping(DBGiftCardCollection dbCollection)
        {
            if (dbCollection == null)
                return null;

            var collection = new GiftCardCollection();
            foreach (var dbItem in dbCollection)
            {
                var item = DBMapping(dbItem);
                collection.Add(item);
            }

            return collection;
        }

        private static GiftCard DBMapping(DBGiftCard dbItem)
        {
            if (dbItem == null)
                return null;

            var item = new GiftCard();
            item.GiftCardID = dbItem.GiftCardID;
            item.PurchasedOrderProductVariantID = dbItem.PurchasedOrderProductVariantID;
            item.Amount = dbItem.Amount;
            item.IsGiftCardActivated = dbItem.IsGiftCardActivated;
            item.GiftCardCouponCode = dbItem.GiftCardCouponCode;
            item.RecipientName = dbItem.RecipientName;
            item.RecipientEmail = dbItem.RecipientEmail;
            item.SenderName = dbItem.SenderName;
            item.SenderEmail = dbItem.SenderEmail;
            item.Message = dbItem.Message;
            item.IsSenderNotified = dbItem.IsSenderNotified;
            item.CreatedOn = dbItem.CreatedOn;

            return item;
        }

        private static GiftCardUsageHistoryCollection DBMapping(DBGiftCardUsageHistoryCollection dbCollection)
        {
            if (dbCollection == null)
                return null;

            var collection = new GiftCardUsageHistoryCollection();
            foreach (var dbItem in dbCollection)
            {
                var item = DBMapping(dbItem);
                collection.Add(item);
            }

            return collection;
        }

        private static GiftCardUsageHistory DBMapping(DBGiftCardUsageHistory dbItem)
        {
            if (dbItem == null)
                return null;

            var item = new GiftCardUsageHistory();
            item.GiftCardUsageHistoryID = dbItem.GiftCardUsageHistoryID;
            item.GiftCardID = dbItem.GiftCardID;
            item.CustomerID = dbItem.CustomerID;
            item.OrderID = dbItem.OrderID;
            item.UsedValue = dbItem.UsedValue;
            item.UsedValueInCustomerCurrency = dbItem.UsedValueInCustomerCurrency;
            item.CreatedOn = dbItem.CreatedOn;

            return item;
        }

        /// <summary>
        /// Sets an order status
        /// </summary>
        /// <param name="OrderID">Order identifier</param>
        /// <param name="OS">New order status</param>
        /// <param name="notifyCustomer">True to notify customer</param>
        /// <returns>Order</returns>
        protected static Order SetOrderStatus(int OrderID, OrderStatusEnum OS, bool notifyCustomer)
        {
            var order = GetOrderByID(OrderID);
            if (order != null)
            {
                if (order.OrderStatus == OS)
                    return order;

                var updatedOrder = UpdateOrder(order.OrderID, order.OrderGUID, order.CustomerID, order.CustomerLanguageID,
                    order.CustomerTaxDisplayType, order.CustomerIP, order.OrderSubtotalInclTax, order.OrderSubtotalExclTax, order.OrderShippingInclTax,
                    order.OrderShippingExclTax, order.PaymentMethodAdditionalFeeInclTax, order.PaymentMethodAdditionalFeeExclTax,
                    order.OrderTax, order.OrderTotal, order.OrderDiscount,
                    order.OrderSubtotalInclTaxInCustomerCurrency, order.OrderSubtotalExclTaxInCustomerCurrency,
                    order.OrderShippingInclTaxInCustomerCurrency, order.OrderShippingExclTaxInCustomerCurrency,
                    order.PaymentMethodAdditionalFeeInclTaxInCustomerCurrency, order.PaymentMethodAdditionalFeeExclTaxInCustomerCurrency,
                    order.OrderTaxInCustomerCurrency, order.OrderTotalInCustomerCurrency,
                    order.OrderDiscountInCustomerCurrency, order.CustomerCurrencyCode, order.OrderWeight,
                    order.AffiliateID, OS, order.AllowStoringCreditCardNumber, order.CardType,
                    order.CardName, order.CardNumber, order.MaskedCreditCardNumber,
                    order.CardCVV2, order.CardExpirationMonth, order.CardExpirationYear,
                    order.PaymentMethodID, order.PaymentMethodName,
                    order.AuthorizationTransactionID,
                    order.AuthorizationTransactionCode, order.AuthorizationTransactionResult,
                    order.CaptureTransactionID, order.CaptureTransactionResult,
                    order.SubscriptionTransactionID, order.PurchaseOrderNumber, order.PaymentStatus, order.PaidDate,
                    order.BillingFirstName, order.BillingLastName, order.BillingPhoneNumber,
                    order.BillingEmail, order.BillingFaxNumber, order.BillingCompany, order.BillingAddress1,
                    order.BillingAddress2, order.BillingCity,
                    order.BillingStateProvince, order.BillingStateProvinceID, order.BillingZipPostalCode,
                    order.BillingCountry, order.BillingCountryID, order.ShippingStatus,
                    order.ShippingFirstName, order.ShippingLastName, order.ShippingPhoneNumber,
                    order.ShippingEmail, order.ShippingFaxNumber, order.ShippingCompany,
                    order.ShippingAddress1, order.ShippingAddress2, order.ShippingCity,
                    order.ShippingStateProvince, order.ShippingStateProvinceID, order.ShippingZipPostalCode,
                    order.ShippingCountry, order.ShippingCountryID,
                    order.ShippingMethod, order.ShippingRateComputationMethodID, order.ShippedDate,
                    order.TrackingNumber, order.Deleted, order.CreatedOn);

                InsertOrderNote(OrderID, string.Format("Order status has been changed to {0}", OS.ToString()), false, DateTime.Now);

                if (order.OrderStatus != OrderStatusEnum.Complete &&
                    OS == OrderStatusEnum.Complete
                    && notifyCustomer)
                {
                    int orderCompletedCustomerNotificationQueuedEmailID = MessageManager.SendOrderCompletedCustomerNotification(updatedOrder, updatedOrder.CustomerLanguageID);
                    InsertOrderNote(OrderID, string.Format("\"Order completed\" email (to customer) has been queued. Queued email identifier: {0}.", orderCompletedCustomerNotificationQueuedEmailID), false, DateTime.Now);
                }

                if (order.OrderStatus != OrderStatusEnum.Cancelled &&
                    OS == OrderStatusEnum.Cancelled
                    && notifyCustomer)
                {
                    int orderCancelledCustomerNotificationQueuedEmailID = MessageManager.SendOrderCancelledCustomerNotification(updatedOrder, updatedOrder.CustomerLanguageID);
                    InsertOrderNote(OrderID, string.Format("\"Order cancelled\" email (to customer) has been queued. Queued email identifier: {0}.", orderCancelledCustomerNotificationQueuedEmailID), false, DateTime.Now);
                }

                return updatedOrder;
            }
            return null;
        }

        /// <summary>
        /// Checks order status
        /// </summary>
        /// <param name="OrderID">Order identifier</param>
        /// <returns>Validated order</returns>
        protected static Order CheckOrderStatus(int OrderID)
        {
            var order = GetOrderByID(OrderID);
            if (order == null)
                return null;

            if (order.OrderStatus == OrderStatusEnum.Pending)
            {
                if (order.PaymentStatus == PaymentStatusEnum.Authorized || order.PaymentStatus == PaymentStatusEnum.Paid)
                {
                    order = SetOrderStatus(OrderID, OrderStatusEnum.Processing, false);
                }
            }

            if (order.OrderStatus == OrderStatusEnum.Pending)
            {
                if (order.ShippingStatus == ShippingStatusEnum.Shipped)
                {
                    order = SetOrderStatus(OrderID, OrderStatusEnum.Processing, false);
                }
            }

            if (order.OrderStatus != OrderStatusEnum.Cancelled && order.OrderStatus != OrderStatusEnum.Complete)
            {
                if (order.PaymentStatus == PaymentStatusEnum.Paid)
                {
                    if (!CanShip(order))
                    {
                        order = SetOrderStatus(OrderID, OrderStatusEnum.Complete, true);
                    }
                }
            }

            return order;
        }

        #endregion

        #region Methods

        #region Repository methods

        #region Orders

        /// <summary>
        /// Gets an order
        /// </summary>
        /// <param name="OrderID">The order identifier</param>
        /// <returns>Order</returns>
        public static Order GetOrderByID(int OrderID)
        {
            if (OrderID == 0)
                return null;

            var dbItem = DBProviderManager<DBOrderProvider>.Provider.GetOrderByID(OrderID);
            var order = DBMapping(dbItem);
            return order;
        }

        /// <summary>
        /// Gets an order
        /// </summary>
        /// <param name="OrderGUID">The order identifier</param>
        /// <returns>Order</returns>
        public static Order GetOrderByGUID(Guid OrderGUID)
        {
            if (OrderGUID == Guid.Empty)
                return null;

            var dbItem = DBProviderManager<DBOrderProvider>.Provider.GetOrderByGUID(OrderGUID);
            var order = DBMapping(dbItem);
            return order;
        }

        /// <summary>
        /// Marks an order as deleted
        /// </summary>
        /// <param name="OrderID">The order identifier</param>
        public static void MarkOrderAsDeleted(int OrderID)
        {
            var order = GetOrderByID(OrderID);
            if (order != null)
            {
                UpdateOrder(order.OrderID, order.OrderGUID, order.CustomerID, order.CustomerLanguageID,
                    order.CustomerTaxDisplayType, order.CustomerIP, order.OrderSubtotalInclTax, order.OrderSubtotalExclTax, order.OrderShippingInclTax,
                   order.OrderShippingExclTax, order.PaymentMethodAdditionalFeeInclTax, order.PaymentMethodAdditionalFeeExclTax,
                   order.OrderTax, order.OrderTotal, order.OrderDiscount,
                   order.OrderSubtotalInclTaxInCustomerCurrency, order.OrderSubtotalExclTaxInCustomerCurrency,
                   order.OrderShippingInclTaxInCustomerCurrency, order.OrderShippingExclTaxInCustomerCurrency,
                   order.PaymentMethodAdditionalFeeInclTaxInCustomerCurrency, order.PaymentMethodAdditionalFeeExclTaxInCustomerCurrency,
                   order.OrderTaxInCustomerCurrency, order.OrderTotalInCustomerCurrency,
                   order.OrderDiscountInCustomerCurrency, order.CustomerCurrencyCode, order.OrderWeight,
                   order.AffiliateID, order.OrderStatus, order.AllowStoringCreditCardNumber, order.CardType,
                   order.CardName, order.CardNumber, order.MaskedCreditCardNumber,
                    order.CardCVV2, order.CardExpirationMonth, order.CardExpirationYear,
                    order.PaymentMethodID, order.PaymentMethodName, order.AuthorizationTransactionID,
                    order.AuthorizationTransactionCode, order.AuthorizationTransactionResult,
                    order.CaptureTransactionID, order.CaptureTransactionResult,
                    order.SubscriptionTransactionID, order.PurchaseOrderNumber, order.PaymentStatus, order.PaidDate,
                    order.BillingFirstName, order.BillingLastName, order.BillingPhoneNumber,
                    order.BillingEmail, order.BillingFaxNumber, order.BillingCompany, order.BillingAddress1,
                    order.BillingAddress2, order.BillingCity, order.BillingStateProvince,
                    order.BillingStateProvinceID, order.BillingZipPostalCode, order.BillingCountry,
                    order.BillingCountryID, order.ShippingStatus,
                    order.ShippingFirstName, order.ShippingLastName, order.ShippingPhoneNumber,
                    order.ShippingEmail, order.ShippingFaxNumber, order.ShippingCompany,
                    order.ShippingAddress1, order.ShippingAddress2, order.ShippingCity,
                    order.ShippingStateProvince, order.ShippingStateProvinceID, order.ShippingZipPostalCode,
                    order.ShippingCountry, order.ShippingCountryID,
                    order.ShippingMethod, order.ShippingRateComputationMethodID, order.ShippedDate, order.TrackingNumber, true,
                    order.CreatedOn);
            }
        }

        /// <summary>
        /// Search orders
        /// </summary>
        /// <param name="StartTime">Order start time; null to load all orders</param>
        /// <param name="EndTime">Order end time; null to load all orders</param>
        /// <param name="CustomerEmail">Customer email</param>
        /// <param name="OS">Order status; null to load all orders</param>
        /// <param name="PS">Order payment status; null to load all orders</param>
        /// <param name="SS">Order shippment status; null to load all orders</param>
        /// <returns>Order collection</returns>
        public static OrderCollection SearchOrders(DateTime? StartTime, DateTime? EndTime,
            string CustomerEmail, OrderStatusEnum? OS, PaymentStatusEnum? PS, 
            ShippingStatusEnum? SS)
        {
            int? orderStatusID = null;
            if (OS.HasValue)
                orderStatusID = (int)OS.Value;

            int? paymentStatusID = null;
            if (PS.HasValue)
                paymentStatusID = (int)PS.Value;

            int? shippingStatusID = null;
            if (SS.HasValue)
                shippingStatusID = (int)SS.Value;

            var dbCollection = DBProviderManager<DBOrderProvider>.Provider.SearchOrders(StartTime, EndTime, CustomerEmail, orderStatusID, paymentStatusID, shippingStatusID);
            var orders = DBMapping(dbCollection);
            return orders;
        }

        /// <summary>
        /// Load all orders
        /// </summary>
        /// <returns>Order collection</returns>
        public static OrderCollection LoadAllOrders()
        {
            return SearchOrders(null, null, string.Empty, null, null, null);
        }

        /// <summary>
        /// Gets all orders by customer identifier
        /// </summary>
        /// <param name="CustomerID">Customer identifier</param>
        /// <returns>Order collection</returns>
        public static OrderCollection GetOrdersByCustomerID(int CustomerID)
        {
            var dbCollection = DBProviderManager<DBOrderProvider>.Provider.GetOrdersByCustomerID(CustomerID);
            var orders = DBMapping(dbCollection);
            return orders;
        }

        /// <summary>
        /// Gets an order by authorization transaction identifier
        /// </summary>
        /// <param name="AuthorizationTransactionID">Authorization transaction identifier</param>
        /// <param name="PaymentMethodID">Payment method identifier</param>
        /// <returns>Order</returns>
        public static Order GetOrderByAuthorizationTransactionIDAndPaymentMethodID(string AuthorizationTransactionID, int PaymentMethodID)
        {
            var dbItem = DBProviderManager<DBOrderProvider>.Provider.GetOrderByAuthorizationTransactionIDAndPaymentMethodID(AuthorizationTransactionID, PaymentMethodID);
            var order = DBMapping(dbItem);
            return order;
        }

        /// <summary>
        /// Gets all orders by affiliate identifier
        /// </summary>
        /// <param name="AffiliateID">Affiliate identifier</param>
        /// <returns>Order collection</returns>
        public static OrderCollection GetOrdersByAffiliateID(int AffiliateID)
        {
            var dbCollection = DBProviderManager<DBOrderProvider>.Provider.GetOrdersByAffiliateID(AffiliateID);
            var orders = DBMapping(dbCollection);
            return orders;
        }

        /// <summary>
        /// Inserts an order
        /// </summary>
        /// <param name="OrderGUID">The order identifier</param>
        /// <param name="CustomerID">The customer identifier</param>
        /// <param name="CustomerLanguageID">The customer language identifier</param>
        /// <param name="CustomerTaxDisplayType">The customer tax display type</param>
        /// <param name="CustomerIP">The customer IP address</param>
        /// <param name="OrderSubtotalInclTax">The order subtotal (incl tax)</param>
        /// <param name="OrderSubtotalExclTax">The order subtotal (excl tax)</param>
        /// <param name="OrderShippingInclTax">The order shipping (incl tax)</param>
        /// <param name="OrderShippingExclTax">The order shipping (excl tax)</param>
        /// <param name="PaymentMethodAdditionalFeeInclTax">The payment method additional fee (incl tax)</param>
        /// <param name="PaymentMethodAdditionalFeeExclTax">The payment method additional fee (excl tax)</param>
        /// <param name="OrderTax">The order tax</param>
        /// <param name="OrderTotal">The order total</param>
        /// <param name="OrderDiscount">The order discount</param>
        /// <param name="OrderSubtotalInclTaxInCustomerCurrency">The order subtotal incl tax (customer currency)</param>
        /// <param name="OrderSubtotalExclTaxInCustomerCurrency">The order subtotal excl tax (customer currency)</param>
        /// <param name="OrderShippingInclTaxInCustomerCurrency">The order shipping incl tax (customer currency)</param>
        /// <param name="OrderShippingExclTaxInCustomerCurrency">The order shipping excl tax (customer currency)</param>
        /// <param name="PaymentMethodAdditionalFeeInclTaxInCustomerCurrency">The payment method additional fee incl tax (customer currency)</param>
        /// <param name="PaymentMethodAdditionalFeeExclTaxInCustomerCurrency">The payment method additional fee excl tax (customer currency)</param>
        /// <param name="OrderTaxInCustomerCurrency">The order tax (customer currency)</param>
        /// <param name="OrderTotalInCustomerCurrency">The order total (customer currency)</param>
        /// <param name="OrderDiscountInCustomerCurrency">The order discount (customer currency)</param>
        /// <param name="CustomerCurrencyCode">The customer currency code</param>
        /// <param name="OrderWeight">The order weight</param>
        /// <param name="AffiliateID">The affiliate identifier</param>
        /// <param name="OrderStatus">The order status</param>
        /// <param name="AllowStoringCreditCardNumber">The value indicating whether storing of credit card number is allowed</param>
        /// <param name="CardType">The card type</param>
        /// <param name="CardName">The card name</param>
        /// <param name="CardNumber">The card number</param>
        /// <param name="MaskedCreditCardNumber">The masked credit card number</param>
        /// <param name="CardCVV2">The card CVV2</param>
        /// <param name="CardExpirationMonth">The card expiration month</param>
        /// <param name="CardExpirationYear">The card expiration year</param>
        /// <param name="PaymentMethodID">The payment method identifier</param>
        /// <param name="PaymentMethodName">The payment method name</param>
        /// <param name="AuthorizationTransactionID">The authorization transaction ID</param>
        /// <param name="AuthorizationTransactionCode">The authorization transaction code</param>
        /// <param name="AuthorizationTransactionResult">The authorization transaction result</param>
        /// <param name="CaptureTransactionID">The capture transaction ID</param>
        /// <param name="CaptureTransactionResult">The capture transaction result</param>
        /// <param name="SubscriptionTransactionID">The subscription transaction ID</param>
        /// <param name="PurchaseOrderNumber">The purchase order number</param>
        /// <param name="PaymentStatus">The payment status</param>
        /// <param name="PaidDate">The paid date and time</param>
        /// <param name="BillingFirstName">The billing first name</param>
        /// <param name="BillingLastName">The billing last name</param>
        /// <param name="BillingPhoneNumber">he billing phone number</param>
        /// <param name="BillingEmail">The billing email</param>
        /// <param name="BillingFaxNumber">The billing fax number</param>
        /// <param name="BillingCompany">The billing company</param>
        /// <param name="BillingAddress1">The billing address 1</param>
        /// <param name="BillingAddress2">The billing address 2</param>
        /// <param name="BillingCity">The billing city</param>
        /// <param name="BillingStateProvince">The billing state/province</param>
        /// <param name="BillingStateProvinceID">The billing state/province identifier</param>
        /// <param name="BillingZipPostalCode">The billing zip/postal code</param>
        /// <param name="BillingCountry">The billing country</param>
        /// <param name="BillingCountryID">The billing country identifier</param>
        /// <param name="ShippingStatus">The shipping status</param>
        /// <param name="ShippingFirstName">The shipping first name</param>
        /// <param name="ShippingLastName">The shipping last name</param>
        /// <param name="ShippingPhoneNumber">The shipping phone number</param>
        /// <param name="ShippingEmail">The shipping email</param>
        /// <param name="ShippingFaxNumber">The shipping fax number</param>
        /// <param name="ShippingCompany">The shipping  company</param>
        /// <param name="ShippingAddress1">The shipping address 1</param>
        /// <param name="ShippingAddress2">The shipping address 2</param>
        /// <param name="ShippingCity">The shipping city</param>
        /// <param name="ShippingStateProvince">The shipping state/province</param>
        /// <param name="ShippingStateProvinceID">The shipping state/province identifier</param>
        /// <param name="ShippingZipPostalCode">The shipping zip/postal code</param>
        /// <param name="ShippingCountry">The shipping country</param>
        /// <param name="ShippingCountryID">The shipping country identifier</param>
        /// <param name="ShippingMethod">The shipping method</param>
        /// <param name="ShippingRateComputationMethodID">The shipping rate computation method identifier</param>
        /// <param name="ShippedDate">The shipped date and time</param>
        /// <param name="TrackingNumber">The tracking number of order</param>
        /// <param name="Deleted">A value indicating whether the entity has been deleted</param>
        /// <param name="CreatedOn">The date and time of order creation</param>
        /// <returns>Order</returns>
        public static Order InsertOrder(Guid OrderGUID, int CustomerID, int CustomerLanguageID,
            TaxDisplayTypeEnum CustomerTaxDisplayType, string CustomerIP, decimal OrderSubtotalInclTax, decimal OrderSubtotalExclTax,
            decimal OrderShippingInclTax, decimal OrderShippingExclTax,
            decimal PaymentMethodAdditionalFeeInclTax, decimal PaymentMethodAdditionalFeeExclTax,
            decimal OrderTax, decimal OrderTotal, decimal OrderDiscount,
            decimal OrderSubtotalInclTaxInCustomerCurrency, decimal OrderSubtotalExclTaxInCustomerCurrency,
            decimal OrderShippingInclTaxInCustomerCurrency, decimal OrderShippingExclTaxInCustomerCurrency,
            decimal PaymentMethodAdditionalFeeInclTaxInCustomerCurrency, decimal PaymentMethodAdditionalFeeExclTaxInCustomerCurrency,
            decimal OrderTaxInCustomerCurrency, decimal OrderTotalInCustomerCurrency,
            decimal OrderDiscountInCustomerCurrency, string CustomerCurrencyCode, decimal OrderWeight,
            int AffiliateID, OrderStatusEnum OrderStatus, bool AllowStoringCreditCardNumber, string CardType,
            string CardName, string CardNumber, string MaskedCreditCardNumber, string CardCVV2,
            string CardExpirationMonth, string CardExpirationYear, int PaymentMethodID,
            string PaymentMethodName, string AuthorizationTransactionID, string AuthorizationTransactionCode,
            string AuthorizationTransactionResult, string CaptureTransactionID, string CaptureTransactionResult,
            string SubscriptionTransactionID, string PurchaseOrderNumber, PaymentStatusEnum PaymentStatus, DateTime? PaidDate,
            string BillingFirstName, string BillingLastName,
            string BillingPhoneNumber, string BillingEmail, string BillingFaxNumber, string BillingCompany,
            string BillingAddress1, string BillingAddress2, string BillingCity, string BillingStateProvince,
            int BillingStateProvinceID, string BillingZipPostalCode, string BillingCountry,
            int BillingCountryID, ShippingStatusEnum ShippingStatus, string ShippingFirstName,
            string ShippingLastName, string ShippingPhoneNumber, string ShippingEmail,
            string ShippingFaxNumber, string ShippingCompany, string ShippingAddress1,
            string ShippingAddress2, string ShippingCity, string ShippingStateProvince,
            int ShippingStateProvinceID, string ShippingZipPostalCode,
            string ShippingCountry, int ShippingCountryID, string ShippingMethod, int ShippingRateComputationMethodID, DateTime? ShippedDate,
            string TrackingNumber, bool Deleted, DateTime CreatedOn)
        {
            if (CustomerIP == null)
                CustomerIP = string.Empty;
            if (CustomerCurrencyCode == null)
                CustomerCurrencyCode = string.Empty;
            if (CardType == null)
                CardType = string.Empty;
            if (CardName == null)
                CardName = string.Empty;
            if (CardNumber == null)
                CardNumber = string.Empty;
            if (CardCVV2 == null)
                CardCVV2 = string.Empty;
            if (CardExpirationMonth == null)
                CardExpirationMonth = string.Empty;
            if (CardExpirationYear == null)
                CardExpirationYear = string.Empty;
            if (PaymentMethodName == null)
                PaymentMethodName = string.Empty;
            if (AuthorizationTransactionID == null)
                AuthorizationTransactionID = string.Empty;
            if (AuthorizationTransactionCode == null)
                AuthorizationTransactionCode = string.Empty;
            if (AuthorizationTransactionResult == null)
                AuthorizationTransactionResult = string.Empty;
            if (CaptureTransactionID == null)
                CaptureTransactionID = string.Empty;
            if (CaptureTransactionResult == null)
                CaptureTransactionResult = string.Empty;
            if (SubscriptionTransactionID == null)
                SubscriptionTransactionID = string.Empty;
            if (PurchaseOrderNumber == null)
                PurchaseOrderNumber = string.Empty;
            if (BillingFirstName == null)
                BillingFirstName = string.Empty;
            if (BillingLastName == null)
                BillingLastName = string.Empty;
            if (BillingPhoneNumber == null)
                BillingPhoneNumber = string.Empty;
            if (BillingEmail == null)
                BillingEmail = string.Empty;
            if (BillingFaxNumber == null)
                BillingFaxNumber = string.Empty;
            if (BillingCompany == null)
                BillingCompany = string.Empty;
            if (BillingZipPostalCode == null)
                BillingZipPostalCode = string.Empty;
            if (BillingCountry == null)
                BillingCountry = string.Empty;
            if (ShippingLastName == null)
                ShippingLastName = string.Empty;
            if (ShippingPhoneNumber == null)
                ShippingPhoneNumber = string.Empty;
            if (ShippingEmail == null)
                ShippingEmail = string.Empty;
            if (ShippingFaxNumber == null)
                ShippingFaxNumber = string.Empty;
            if (ShippingCompany == null)
                ShippingCompany = string.Empty;
            if (ShippingAddress1 == null)
                ShippingAddress1 = string.Empty;
            if (ShippingAddress2 == null)
                ShippingAddress2 = string.Empty;
            if (ShippingCity == null)
                ShippingCity = string.Empty;
            if (ShippingStateProvince == null)
                ShippingStateProvince = string.Empty;
            if (ShippingZipPostalCode == null)
                ShippingZipPostalCode = string.Empty;
            if (ShippingCountry == null)
                ShippingCountry = string.Empty;
            if (ShippingMethod == null)
                ShippingMethod = string.Empty;
            if (PaidDate.HasValue)
                PaidDate = DateTimeHelper.ConvertToUtcTime(PaidDate.Value);
            if (ShippedDate.HasValue)
                ShippedDate = DateTimeHelper.ConvertToUtcTime(ShippedDate.Value);
            if (TrackingNumber == null)
                TrackingNumber = string.Empty;
            CreatedOn = DateTimeHelper.ConvertToUtcTime(CreatedOn);

            var dbItem = DBProviderManager<DBOrderProvider>.Provider.InsertOrder(OrderGUID, CustomerID, CustomerLanguageID,
             (int)CustomerTaxDisplayType, CustomerIP, OrderSubtotalInclTax, OrderSubtotalExclTax,
             OrderShippingInclTax, OrderShippingExclTax,
             PaymentMethodAdditionalFeeInclTax, PaymentMethodAdditionalFeeExclTax,
             OrderTax, OrderTotal, OrderDiscount,
             OrderSubtotalInclTaxInCustomerCurrency, OrderSubtotalExclTaxInCustomerCurrency,
             OrderShippingInclTaxInCustomerCurrency, OrderShippingExclTaxInCustomerCurrency,
             PaymentMethodAdditionalFeeInclTaxInCustomerCurrency, PaymentMethodAdditionalFeeExclTaxInCustomerCurrency,
             OrderTaxInCustomerCurrency, OrderTotalInCustomerCurrency, 
             OrderDiscountInCustomerCurrency, CustomerCurrencyCode, OrderWeight,
             AffiliateID, (int)OrderStatus, AllowStoringCreditCardNumber,
             CardType, CardName, CardNumber, MaskedCreditCardNumber, CardCVV2,
             CardExpirationMonth, CardExpirationYear, PaymentMethodID,
             PaymentMethodName, AuthorizationTransactionID, AuthorizationTransactionCode,
             AuthorizationTransactionResult, CaptureTransactionID, CaptureTransactionResult,
             SubscriptionTransactionID, PurchaseOrderNumber,
             (int)PaymentStatus, PaidDate, BillingFirstName, BillingLastName,
             BillingPhoneNumber, BillingEmail, BillingFaxNumber, BillingCompany,
             BillingAddress1, BillingAddress2, BillingCity, BillingStateProvince, BillingStateProvinceID,
             BillingZipPostalCode, BillingCountry, BillingCountryID, (int)ShippingStatus, ShippingFirstName, ShippingLastName,
             ShippingPhoneNumber, ShippingEmail,
             ShippingFaxNumber, ShippingCompany, ShippingAddress1,
             ShippingAddress2, ShippingCity, ShippingStateProvince, ShippingStateProvinceID, ShippingZipPostalCode,
             ShippingCountry, ShippingCountryID, ShippingMethod, ShippingRateComputationMethodID, ShippedDate,
             TrackingNumber, Deleted, CreatedOn);

            var order = DBMapping(dbItem);
            return order;
        }

        /// <summary>
        /// Updates the order
        /// </summary>
        /// <param name="OrderID">he order identifier</param>
        /// <param name="OrderGUID">The order identifier</param>
        /// <param name="CustomerID">The customer identifier</param>
        /// <param name="CustomerLanguageID">The customer language identifier</param>
        /// <param name="CustomerTaxDisplayType">The customer tax display type</param>
        /// <param name="CustomerIP">The customer IP address</param>
        /// <param name="OrderSubtotalInclTax">The order subtotal (incl tax)</param>
        /// <param name="OrderSubtotalExclTax">The order subtotal (excl tax)</param>
        /// <param name="OrderShippingInclTax">The order shipping (incl tax)</param>
        /// <param name="OrderShippingExclTax">The order shipping (excl tax)</param>
        /// <param name="PaymentMethodAdditionalFeeInclTax">The payment method additional fee (incl tax)</param>
        /// <param name="PaymentMethodAdditionalFeeExclTax">The payment method additional fee (excl tax)</param>
        /// <param name="OrderTax">The order tax</param>
        /// <param name="OrderTotal">The order total</param>
        /// <param name="OrderDiscount">The order discount</param>
        /// <param name="OrderSubtotalInclTaxInCustomerCurrency">The order subtotal incl tax (customer currency)</param>
        /// <param name="OrderSubtotalExclTaxInCustomerCurrency">The order subtotal excl tax (customer currency)</param>
        /// <param name="OrderShippingInclTaxInCustomerCurrency">The order shipping incl tax (customer currency)</param>
        /// <param name="OrderShippingExclTaxInCustomerCurrency">The order shipping excl tax (customer currency)</param>
        /// <param name="PaymentMethodAdditionalFeeInclTaxInCustomerCurrency">The payment method additional fee incl tax (customer currency)</param>
        /// <param name="PaymentMethodAdditionalFeeExclTaxInCustomerCurrency">The payment method additional fee excl tax (customer currency)</param>
        /// <param name="OrderTaxInCustomerCurrency">The order tax (customer currency)</param>
        /// <param name="OrderTotalInCustomerCurrency">The order total (customer currency)</param>
        /// <param name="OrderDiscountInCustomerCurrency">The order discount (customer currency)</param>
        /// <param name="CustomerCurrencyCode">The customer currency code</param>
        /// <param name="OrderWeight">The order weight</param>
        /// <param name="AffiliateID">The affiliate identifier</param>
        /// <param name="OrderStatus">The order status</param>
        /// <param name="AllowStoringCreditCardNumber">The value indicating whether storing of credit card number is allowed</param>
        /// <param name="CardType">The card type</param>
        /// <param name="CardName">The card name</param>
        /// <param name="CardNumber">The card number</param>
        /// <param name="MaskedCreditCardNumber">The masked credit card number</param>
        /// <param name="CardCVV2">The card CVV2</param>
        /// <param name="CardExpirationMonth">The card expiration month</param>
        /// <param name="CardExpirationYear">The card expiration year</param>
        /// <param name="PaymentMethodID">The payment method identifier</param>
        /// <param name="PaymentMethodName">The payment method name</param>
        /// <param name="AuthorizationTransactionID">The authorization transaction ID</param>
        /// <param name="AuthorizationTransactionCode">The authorization transaction code</param>
        /// <param name="AuthorizationTransactionResult">The authorization transaction result</param>
        /// <param name="CaptureTransactionID">The capture transaction ID</param>
        /// <param name="CaptureTransactionResult">The capture transaction result</param>
        /// <param name="SubscriptionTransactionID">The subscription transaction ID</param>
        /// <param name="PurchaseOrderNumber">The purchase order number</param>
        /// <param name="PaymentStatus">The payment status</param>
        /// <param name="PaidDate">The paid date and time</param>
        /// <param name="BillingFirstName">The billing first name</param>
        /// <param name="BillingLastName">The billing last name</param>
        /// <param name="BillingPhoneNumber">he billing phone number</param>
        /// <param name="BillingEmail">The billing email</param>
        /// <param name="BillingFaxNumber">The billing fax number</param>
        /// <param name="BillingCompany">The billing company</param>
        /// <param name="BillingAddress1">The billing address 1</param>
        /// <param name="BillingAddress2">The billing address 2</param>
        /// <param name="BillingCity">The billing city</param>
        /// <param name="BillingStateProvince">The billing state/province</param>
        /// <param name="BillingStateProvinceID">The billing state/province identifier</param>
        /// <param name="BillingZipPostalCode">The billing zip/postal code</param>
        /// <param name="BillingCountry">The billing country</param>
        /// <param name="BillingCountryID">The billing country identifier</param>
        /// <param name="ShippingStatus">The shipping status</param>
        /// <param name="ShippingFirstName">The shipping first name</param>
        /// <param name="ShippingLastName">The shipping last name</param>
        /// <param name="ShippingPhoneNumber">The shipping phone number</param>
        /// <param name="ShippingEmail">The shipping email</param>
        /// <param name="ShippingFaxNumber">The shipping fax number</param>
        /// <param name="ShippingCompany">The shipping  company</param>
        /// <param name="ShippingAddress1">The shipping address 1</param>
        /// <param name="ShippingAddress2">The shipping address 2</param>
        /// <param name="ShippingCity">The shipping city</param>
        /// <param name="ShippingStateProvince">The shipping state/province</param>
        /// <param name="ShippingStateProvinceID">The shipping state/province identifier</param>
        /// <param name="ShippingZipPostalCode">The shipping zip/postal code</param>
        /// <param name="ShippingCountry">The shipping country</param>
        /// <param name="ShippingCountryID">The shipping country identifier</param>
        /// <param name="ShippingMethod">The shipping method</param>
        /// <param name="ShippingRateComputationMethodID">The shipping rate computation method identifier</param>
        /// <param name="ShippedDate">The shipped date and time</param>
        /// <param name="TrackingNumber">The tracking number of order</param>
        /// <param name="Deleted">A value indicating whether the entity has been deleted</param>
        /// <param name="CreatedOn">The date and time of order creation</param>
        /// <returns>Order</returns>
        public static Order UpdateOrder(int OrderID, Guid OrderGUID, int CustomerID, int CustomerLanguageID,
            TaxDisplayTypeEnum CustomerTaxDisplayType, string CustomerIP, decimal OrderSubtotalInclTax, decimal OrderSubtotalExclTax,
            decimal OrderShippingInclTax, decimal OrderShippingExclTax,
            decimal PaymentMethodAdditionalFeeInclTax, decimal PaymentMethodAdditionalFeeExclTax,
            decimal OrderTax, decimal OrderTotal, decimal OrderDiscount,
            decimal OrderSubtotalInclTaxInCustomerCurrency, decimal OrderSubtotalExclTaxInCustomerCurrency,
            decimal OrderShippingInclTaxInCustomerCurrency, decimal OrderShippingExclTaxInCustomerCurrency,
            decimal PaymentMethodAdditionalFeeInclTaxInCustomerCurrency, decimal PaymentMethodAdditionalFeeExclTaxInCustomerCurrency,
            decimal OrderTaxInCustomerCurrency, decimal OrderTotalInCustomerCurrency,
            decimal OrderDiscountInCustomerCurrency, string CustomerCurrencyCode, decimal OrderWeight,
            int AffiliateID, OrderStatusEnum OrderStatus, bool AllowStoringCreditCardNumber,
            string CardType, string CardName, string CardNumber, string MaskedCreditCardNumber, string CardCVV2,
            string CardExpirationMonth, string CardExpirationYear, int PaymentMethodID,
            string PaymentMethodName, string AuthorizationTransactionID, string AuthorizationTransactionCode,
            string AuthorizationTransactionResult, string CaptureTransactionID, string CaptureTransactionResult,
            string SubscriptionTransactionID, string PurchaseOrderNumber, PaymentStatusEnum PaymentStatus, DateTime? PaidDate,
            string BillingFirstName, string BillingLastName,
            string BillingPhoneNumber, string BillingEmail, string BillingFaxNumber, string BillingCompany,
            string BillingAddress1, string BillingAddress2, string BillingCity, string BillingStateProvince,
            int BillingStateProvinceID, string BillingZipPostalCode, string BillingCountry,
            int BillingCountryID, ShippingStatusEnum ShippingStatus, string ShippingFirstName,
            string ShippingLastName, string ShippingPhoneNumber, string ShippingEmail,
            string ShippingFaxNumber, string ShippingCompany, string ShippingAddress1,
            string ShippingAddress2, string ShippingCity, string ShippingStateProvince,
            int ShippingStateProvinceID, string ShippingZipPostalCode,
            string ShippingCountry, int ShippingCountryID, string ShippingMethod, int ShippingRateComputationMethodID, DateTime? ShippedDate,
            string TrackingNumber, bool Deleted, DateTime CreatedOn)
        {
            if (PaidDate.HasValue)
                PaidDate = DateTimeHelper.ConvertToUtcTime(PaidDate.Value);
            if (ShippedDate.HasValue)
                ShippedDate = DateTimeHelper.ConvertToUtcTime(ShippedDate.Value);
            if (TrackingNumber == null)
                TrackingNumber = string.Empty;
            CreatedOn = DateTimeHelper.ConvertToUtcTime(CreatedOn);

            var dbItem = DBProviderManager<DBOrderProvider>.Provider.UpdateOrder(OrderID, OrderGUID, CustomerID, CustomerLanguageID,
                (int)CustomerTaxDisplayType, CustomerIP, OrderSubtotalInclTax, OrderSubtotalExclTax,
                OrderShippingInclTax, OrderShippingExclTax,
                PaymentMethodAdditionalFeeInclTax, PaymentMethodAdditionalFeeExclTax,
                OrderTax, OrderTotal, OrderDiscount,
                OrderSubtotalInclTaxInCustomerCurrency, OrderSubtotalExclTaxInCustomerCurrency,
                OrderShippingInclTaxInCustomerCurrency, OrderShippingExclTaxInCustomerCurrency,
                PaymentMethodAdditionalFeeInclTaxInCustomerCurrency, PaymentMethodAdditionalFeeExclTaxInCustomerCurrency,
                OrderTaxInCustomerCurrency, OrderTotalInCustomerCurrency, 
                OrderDiscountInCustomerCurrency, CustomerCurrencyCode, OrderWeight,
                AffiliateID, (int)OrderStatus, AllowStoringCreditCardNumber,
                CardType, CardName, CardNumber, MaskedCreditCardNumber, CardCVV2,
                CardExpirationMonth, CardExpirationYear, PaymentMethodID,
                PaymentMethodName, AuthorizationTransactionID, AuthorizationTransactionCode,
                AuthorizationTransactionResult, CaptureTransactionID, CaptureTransactionResult,
                SubscriptionTransactionID, PurchaseOrderNumber,
                (int)PaymentStatus, PaidDate, BillingFirstName, BillingLastName,
                BillingPhoneNumber, BillingEmail, BillingFaxNumber, BillingCompany,
                BillingAddress1, BillingAddress2, BillingCity, BillingStateProvince, BillingStateProvinceID,
                BillingZipPostalCode, BillingCountry, BillingCountryID, (int)ShippingStatus, ShippingFirstName, ShippingLastName,
                ShippingPhoneNumber, ShippingEmail,
                ShippingFaxNumber, ShippingCompany, ShippingAddress1,
                ShippingAddress2, ShippingCity, ShippingStateProvince, ShippingStateProvinceID, ShippingZipPostalCode,
                ShippingCountry, ShippingCountryID, ShippingMethod, ShippingRateComputationMethodID, ShippedDate,
                TrackingNumber, Deleted, CreatedOn);

            var order = DBMapping(dbItem);
            return order;
        }

        /// <summary>
        /// Set tracking number of order
        /// </summary>
        /// <param name="OrderID">Order note identifier</param>
        /// <param name="TrackingNumber">The tracking number of order</param>
        public static void SetOrderTrackingNumber(int OrderID, string TrackingNumber)
        {
            var order = GetOrderByID(OrderID);
            if (order != null)
            {
                UpdateOrder(
                   order.OrderID, order.OrderGUID, order.CustomerID, order.CustomerLanguageID,
                   order.CustomerTaxDisplayType, order.CustomerIP, order.OrderSubtotalInclTax, order.OrderSubtotalExclTax, order.OrderShippingInclTax,
                   order.OrderShippingExclTax, order.PaymentMethodAdditionalFeeInclTax, order.PaymentMethodAdditionalFeeExclTax,
                   order.OrderTax, order.OrderTotal, order.OrderDiscount,
                   order.OrderSubtotalInclTaxInCustomerCurrency, order.OrderSubtotalExclTaxInCustomerCurrency,
                   order.OrderShippingInclTaxInCustomerCurrency, order.OrderShippingExclTaxInCustomerCurrency,
                   order.PaymentMethodAdditionalFeeInclTaxInCustomerCurrency, order.PaymentMethodAdditionalFeeExclTaxInCustomerCurrency,
                   order.OrderTaxInCustomerCurrency, order.OrderTotalInCustomerCurrency,
                   order.OrderDiscountInCustomerCurrency, order.CustomerCurrencyCode, order.OrderWeight,
                   order.AffiliateID, order.OrderStatus, order.AllowStoringCreditCardNumber, order.CardType,
                   order.CardName, order.CardNumber, order.MaskedCreditCardNumber,
                   order.CardCVV2, order.CardExpirationMonth, order.CardExpirationYear,
                   order.PaymentMethodID, order.PaymentMethodName, order.AuthorizationTransactionID,
                   order.AuthorizationTransactionCode, order.AuthorizationTransactionResult,
                   order.CaptureTransactionID, order.CaptureTransactionResult,
                   order.SubscriptionTransactionID, order.PurchaseOrderNumber, order.PaymentStatus, order.PaidDate,
                   order.BillingFirstName, order.BillingLastName, order.BillingPhoneNumber,
                   order.BillingEmail, order.BillingFaxNumber, order.BillingCompany, order.BillingAddress1,
                   order.BillingAddress2, order.BillingCity, order.BillingStateProvince,
                   order.BillingStateProvinceID, order.BillingZipPostalCode, order.BillingCountry,
                   order.BillingCountryID, order.ShippingStatus,
                   order.ShippingFirstName, order.ShippingLastName, order.ShippingPhoneNumber,
                   order.ShippingEmail, order.ShippingFaxNumber, order.ShippingCompany,
                   order.ShippingAddress1, order.ShippingAddress2, order.ShippingCity,
                   order.ShippingStateProvince, order.ShippingStateProvinceID, order.ShippingZipPostalCode,
                   order.ShippingCountry, order.ShippingCountryID,
                   order.ShippingMethod, order.ShippingRateComputationMethodID, order.ShippedDate,
                   TrackingNumber, order.Deleted, order.CreatedOn);
            }
        }

        #endregion
        
        #region Orders product variants

        /// <summary>
        /// Gets an order product variant
        /// </summary>
        /// <param name="OrderProductVariantID">Order product variant identifier</param>
        /// <returns>Order product variant</returns>
        public static OrderProductVariant GetOrderProductVariantByID(int OrderProductVariantID)
        {
            if (OrderProductVariantID == 0)
                return null;

            var dbItem = DBProviderManager<DBOrderProvider>.Provider.GetOrderProductVariantByID(OrderProductVariantID);
            var orderProductVariant = DBMapping(dbItem);
            return orderProductVariant;
        }

        /// <summary>
        /// Gets an order product variant
        /// </summary>
        /// <param name="OrderProductVariantGUID">Order product variant identifier</param>
        /// <returns>Order product variant</returns>
        public static OrderProductVariant GetOrderProductVariantByGUID(Guid OrderProductVariantGUID)
        {
            if (OrderProductVariantGUID == Guid.Empty)
                return null;

            var dbItem = DBProviderManager<DBOrderProvider>.Provider.GetOrderProductVariantByGUID(OrderProductVariantGUID);
            var orderProductVariant = DBMapping(dbItem);
            return orderProductVariant;
        }

        /// <summary>
        /// Gets all order product variants
        /// </summary>
        /// <param name="OrderID">Order identifier; null to load all records</param>
        /// <param name="CustomerID">Customer identifier; null to load all records</param>
        /// <param name="StartTime">Order start time; null to load all records</param>
        /// <param name="EndTime">Order end time; null to load all records</param>
        /// <param name="OS">Order status; null to load all records</param>
        /// <param name="PS">Order payment status; null to load all records</param>
        /// <param name="SS">Order shippment status; null to load all records</param>
        /// <returns>Order collection</returns>
        public static OrderProductVariantCollection GetAllOrderProductVariants(int? OrderID,
            int? CustomerID, DateTime? StartTime, DateTime? EndTime,
            OrderStatusEnum? OS, PaymentStatusEnum? PS, ShippingStatusEnum? SS)
        {
            return GetAllOrderProductVariants(OrderID, CustomerID, StartTime, EndTime, OS, PS, SS, false);
        }

        /// <summary>
        /// Gets all order product variants
        /// </summary>
        /// <param name="OrderID">Order identifier; null to load all records</param>
        /// <param name="CustomerID">Customer identifier; null to load all records</param>
        /// <param name="StartTime">Order start time; null to load all records</param>
        /// <param name="EndTime">Order end time; null to load all records</param>
        /// <param name="OS">Order status; null to load all records</param>
        /// <param name="PS">Order payment status; null to load all records</param>
        /// <param name="SS">Order shippment status; null to load all records</param>
        /// <param name="LoadDownloableProductsOnly">Value indicating whether to load downloadable products only</param>
        /// <returns>Order collection</returns>
        public static OrderProductVariantCollection GetAllOrderProductVariants(int? OrderID,
            int? CustomerID, DateTime? StartTime, DateTime? EndTime,
            OrderStatusEnum? OS, PaymentStatusEnum? PS, ShippingStatusEnum? SS,
            bool LoadDownloableProductsOnly)
        {
            int? orderStatusID = null;
            if (OS.HasValue)
                orderStatusID = (int)OS.Value;

            int? paymentStatusID = null;
            if (PS.HasValue)
                paymentStatusID = (int)PS.Value;

            int? shippingStatusID = null;
            if (SS.HasValue)
                shippingStatusID = (int)SS.Value;

            var dbCollection = DBProviderManager<DBOrderProvider>.Provider.GetAllOrderProductVariants(OrderID,
                CustomerID, StartTime, EndTime, orderStatusID, paymentStatusID, shippingStatusID,
                LoadDownloableProductsOnly);
            var orderProductVariants = DBMapping(dbCollection);
            return orderProductVariants;
        }

        /// <summary>
        /// Gets an order product variants by the order identifier
        /// </summary>
        /// <param name="OrderID">The order identifier</param>
        /// <returns>Order product variant collection</returns>
        public static OrderProductVariantCollection GetOrderProductVariantsByOrderID(int OrderID)
        {
            return GetAllOrderProductVariants(OrderID, null, null, null, null, null, null);
        }

        /// <summary>
        /// Inserts a order product variant
        /// </summary>
        /// <param name="OrderProductVariantGUID">The order product variant identifier</param>
        /// <param name="OrderID">The order identifier</param>
        /// <param name="ProductVariantID">The product variant identifier</param>
        /// <param name="UnitPriceInclTax">The unit price in primary store currency (incl tax)</param>
        /// <param name="UnitPriceExclTax">The unit price in primary store currency (excl tax)</param>
        /// <param name="PriceInclTax">The price in primary store currency (incl tax)</param>
        /// <param name="PriceExclTax">The price in primary store currency (excl tax)</param>
        /// <param name="UnitPriceInclTaxInCustomerCurrency">The unit price in primary store currency (incl tax)</param>
        /// <param name="UnitPriceExclTaxInCustomerCurrency">The unit price in customer currency (excl tax)</param>
        /// <param name="PriceInclTaxInCustomerCurrency">The price in primary store currency (incl tax)</param>
        /// <param name="PriceExclTaxInCustomerCurrency">The price in customer currency (excl tax)</param>
        /// <param name="AttributeDescription">The attribute description</param>
        /// <param name="AttributesXML">The attribute description in XML format</param>
        /// <param name="Quantity">The quantity</param>
        /// <param name="DiscountAmountInclTax">The discount amount (incl tax)</param>
        /// <param name="DiscountAmountExclTax">The discount amount (excl tax)</param>
        /// <param name="DownloadCount">The download count</param>
        /// <param name="IsDownloadActivated">The value indicating whether download is activated</param>
        /// <param name="LicenseDownloadID">A license download identifier (in case this is a downloadable product)</param>
        /// <returns>Order product variant</returns>
        public static OrderProductVariant InsertOrderProductVariant(Guid OrderProductVariantGUID,
            int OrderID, int ProductVariantID,
            decimal UnitPriceInclTax, decimal UnitPriceExclTax, decimal PriceInclTax, decimal PriceExclTax,
            decimal UnitPriceInclTaxInCustomerCurrency, decimal UnitPriceExclTaxInCustomerCurrency,
            decimal PriceInclTaxInCustomerCurrency, decimal PriceExclTaxInCustomerCurrency,
            string AttributeDescription, string AttributesXML, int Quantity,
            decimal DiscountAmountInclTax, decimal DiscountAmountExclTax,
            int DownloadCount, bool IsDownloadActivated, int LicenseDownloadID)
        {
            if (AttributeDescription == null)
                AttributeDescription = string.Empty;

            var dbItem = DBProviderManager<DBOrderProvider>.Provider.InsertOrderProductVariant(
                OrderProductVariantGUID, OrderID, ProductVariantID,
                UnitPriceInclTax, UnitPriceExclTax, PriceInclTax, PriceExclTax,
                UnitPriceInclTaxInCustomerCurrency, UnitPriceExclTaxInCustomerCurrency,
                PriceInclTaxInCustomerCurrency, PriceExclTaxInCustomerCurrency,
                AttributeDescription, AttributesXML, Quantity, DiscountAmountInclTax,
                DiscountAmountExclTax, DownloadCount, IsDownloadActivated, LicenseDownloadID);
            var log = DBMapping(dbItem);
            return log;
        }

        /// <summary>
        /// Updates the order product variant
        /// </summary>
        /// <param name="OrderProductVariantID">The order product variant identifier</param>
        /// <param name="OrderProductVariantGUID">The order product variant identifier</param>
        /// <param name="OrderID">The order identifier</param>
        /// <param name="ProductVariantID">The product variant identifier</param>
        /// <param name="UnitPriceInclTax">The unit price in primary store currency (incl tax)</param>
        /// <param name="UnitPriceExclTax">The unit price in primary store currency (excl tax)</param>
        /// <param name="PriceInclTax">The price in primary store currency (incl tax)</param>
        /// <param name="PriceExclTax">The price in primary store currency (excl tax)</param>
        /// <param name="UnitPriceInclTaxInCustomerCurrency">The unit price in primary store currency (incl tax)</param>
        /// <param name="UnitPriceExclTaxInCustomerCurrency">The unit price in customer currency (excl tax)</param>
        /// <param name="PriceInclTaxInCustomerCurrency">The price in primary store currency (incl tax)</param>
        /// <param name="PriceExclTaxInCustomerCurrency">The price in customer currency (excl tax)</param>
        /// <param name="AttributeDescription">The attribute description</param>
        /// <param name="AttributesXML">The attribute description in XML format</param>
        /// <param name="Quantity">The quantity</param>
        /// <param name="DiscountAmountInclTax">The discount amount (incl tax)</param>
        /// <param name="DiscountAmountExclTax">The discount amount (excl tax)</param>
        /// <param name="DownloadCount">The downloads count</param>
        /// <param name="IsDownloadActivated">The value indicating whether download is activated</param>
        /// <param name="LicenseDownloadID">A license download identifier (in case this is a downloadable product)</param>
        /// <returns>Order product variant</returns>
        public static OrderProductVariant UpdateOrderProductVariant(int OrderProductVariantID,
            Guid OrderProductVariantGUID, int OrderID, int ProductVariantID,
            decimal UnitPriceInclTax, decimal UnitPriceExclTax, decimal PriceInclTax, decimal PriceExclTax,
            decimal UnitPriceInclTaxInCustomerCurrency, decimal UnitPriceExclTaxInCustomerCurrency,
            decimal PriceInclTaxInCustomerCurrency, decimal PriceExclTaxInCustomerCurrency,
            string AttributeDescription, string AttributesXML, int Quantity,
            decimal DiscountAmountInclTax, decimal DiscountAmountExclTax,
            int DownloadCount, bool IsDownloadActivated, int LicenseDownloadID)
        {
            if (AttributeDescription == null)
                AttributeDescription = string.Empty;

            var dbItem = DBProviderManager<DBOrderProvider>.Provider.UpdateOrderProductVariant(
                OrderProductVariantID, OrderProductVariantGUID, OrderID,
                ProductVariantID, UnitPriceInclTax, UnitPriceExclTax, PriceInclTax, PriceExclTax,
                UnitPriceInclTaxInCustomerCurrency, UnitPriceExclTaxInCustomerCurrency,
                PriceInclTaxInCustomerCurrency, PriceExclTaxInCustomerCurrency,
                AttributeDescription, AttributesXML, Quantity, DiscountAmountInclTax,
                DiscountAmountExclTax, DownloadCount, IsDownloadActivated, LicenseDownloadID);
            var log = DBMapping(dbItem);
            return log;
        }

        /// <summary>
        /// Increase an order product variant download count
        /// </summary>
        /// <param name="OrderProductVariantID">Order product variant identifier</param>
        /// <returns>Order product variant</returns>
        public static OrderProductVariant IncreaseOrderProductDownloadCount(int OrderProductVariantID)
        {
            var orderProductVariant = GetOrderProductVariantByID(OrderProductVariantID);
            if (orderProductVariant == null)
                throw new NopException("Order product variant could not be loaded");

            int newDownloadCount = orderProductVariant.DownloadCount + 1;

            orderProductVariant = UpdateOrderProductVariant(orderProductVariant.OrderProductVariantID,
                orderProductVariant.OrderProductVariantGUID, orderProductVariant.OrderID,
                orderProductVariant.ProductVariantID,
                orderProductVariant.UnitPriceInclTax, orderProductVariant.UnitPriceExclTax,
                orderProductVariant.PriceInclTax, orderProductVariant.PriceExclTax,
                orderProductVariant.UnitPriceInclTaxInCustomerCurrency, orderProductVariant.UnitPriceExclTaxInCustomerCurrency,
                orderProductVariant.PriceInclTaxInCustomerCurrency, orderProductVariant.PriceExclTaxInCustomerCurrency,
                orderProductVariant.AttributeDescription, orderProductVariant.AttributesXML,
                orderProductVariant.Quantity,
                orderProductVariant.DiscountAmountInclTax, orderProductVariant.DiscountAmountExclTax,
                newDownloadCount, orderProductVariant.IsDownloadActivated,
                orderProductVariant.LicenseDownloadID);

            return orderProductVariant;
        }

        #endregion

        #region Order notes

        /// <summary>
        /// Gets an order note
        /// </summary>
        /// <param name="OrderNoteID">Order note identifier</param>
        /// <returns>Order note</returns>
        public static OrderNote GetOrderNoteByID(int OrderNoteID)
        {
            if (OrderNoteID == 0)
                return null;

            var dbItem = DBProviderManager<DBOrderProvider>.Provider.GetOrderNoteByID(OrderNoteID);
            var orderNote = DBMapping(dbItem);
            return orderNote;
        }

        /// <summary>
        /// Gets an order notes by order identifier
        /// </summary>
        /// <param name="OrderID">Order identifier</param>
        /// <returns>Order note collection</returns>
        public static OrderNoteCollection GetOrderNoteByOrderID(int OrderID)
        {
            return GetOrderNoteByOrderID(OrderID, NopContext.Current.IsAdmin);
        }

        /// <summary>
        /// Gets an order notes by order identifier
        /// </summary>
        /// <param name="OrderID">Order identifier</param>
        /// <param name="ShowHidden">A value indicating whether all orders should be loaded</param>
        /// <returns>Order note collection</returns>
        public static OrderNoteCollection GetOrderNoteByOrderID(int OrderID, bool ShowHidden)
        {
            var dbCollection = DBProviderManager<DBOrderProvider>.Provider.GetOrderNoteByOrderID(OrderID, ShowHidden);
            var orderNotes = DBMapping(dbCollection);
            return orderNotes;
        }

        /// <summary>
        /// Deletes an order note
        /// </summary>
        /// <param name="OrderNoteID">Order note identifier</param>
        public static void DeleteOrderNote(int OrderNoteID)
        {
            DBProviderManager<DBOrderProvider>.Provider.DeleteOrderNote(OrderNoteID);
        }

        /// <summary>
        /// Inserts an order note
        /// </summary>
        /// <param name="OrderID">The order identifier</param>
        /// <param name="Note">The note</param>
        /// <param name="CreatedOn">The date and time of order note creation</param>
        /// <returns>Order note</returns>
        public static OrderNote InsertOrderNote(int OrderID, string Note, DateTime CreatedOn)
        {
            return InsertOrderNote(OrderID, Note, false, CreatedOn);
        }

        /// <summary>
        /// Inserts an order note
        /// </summary>
        /// <param name="OrderID">The order identifier</param>
        /// <param name="Note">The note</param>
        /// <param name="DisplayToCustomer">A value indicating whether the customer can see a note</param>
        /// <param name="CreatedOn">The date and time of order note creation</param>
        /// <returns>Order note</returns>
        public static OrderNote InsertOrderNote(int OrderID, string Note, bool DisplayToCustomer, DateTime CreatedOn)
        {
            CreatedOn = DateTimeHelper.ConvertToUtcTime(CreatedOn);

            var dbItem = DBProviderManager<DBOrderProvider>.Provider.InsertOrderNote(OrderID, Note, DisplayToCustomer, CreatedOn);
            var orderNote = DBMapping(dbItem);
            return orderNote;
        }

        /// <summary>
        /// Updates the order note
        /// </summary>
        /// <param name="OrderNoteID">The order note identifier</param>
        /// <param name="OrderID">The order identifier</param>
        /// <param name="Note">The note</param>
        /// <param name="DisplayToCustomer">A value indicating whether the customer can see a note</param>
        /// <param name="CreatedOn">The date and time of order note creation</param>
        /// <returns>Order note</returns>
        public static OrderNote UpdateOrderNote(int OrderNoteID, int OrderID, string Note, bool DisplayToCustomer, DateTime CreatedOn)
        {
            CreatedOn = DateTimeHelper.ConvertToUtcTime(CreatedOn);

            var dbItem = DBProviderManager<DBOrderProvider>.Provider.UpdateOrderNote(OrderNoteID, OrderID, Note, DisplayToCustomer, CreatedOn);
            var orderNote = DBMapping(dbItem);
            return orderNote;
        }

        #endregion

        #region Order statuses

        /// <summary>
        /// Gets an order status full name
        /// </summary>
        /// <param name="OrderStatusID">Order status identifier</param>
        /// <returns>Order status name</returns>
        public static string GetOrderStatusName(int OrderStatusID)
        {
            var orderStatus = GetOrderStatusByID(OrderStatusID);
            if (orderStatus != null)
                return orderStatus.Name;
            else
                return ((OrderStatusEnum)OrderStatusID).ToString();
        }

        /// <summary>
        /// Gets an order status by ID
        /// </summary>
        /// <param name="OrderStatusID">Order status identifier</param>
        /// <returns>Order status</returns>
        public static OrderStatus GetOrderStatusByID(int OrderStatusID)
        {
            if (OrderStatusID == 0)
                return null;

            string key = string.Format(ORDERSTATUSES_BY_ID_KEY, OrderStatusID);
            object obj2 = NopCache.Get(key);
            if (OrderManager.CacheEnabled && (obj2 != null))
            {
                return (OrderStatus)obj2;
            }

            var dbItem = DBProviderManager<DBOrderProvider>.Provider.GetOrderStatusByID(OrderStatusID);
            var orderStatus = DBMapping(dbItem);

            if (OrderManager.CacheEnabled)
            {
                NopCache.Max(key, orderStatus);
            }
            return orderStatus;
        }

        /// <summary>
        /// Gets all order statuses
        /// </summary>
        /// <returns>Order status collection</returns>
        public static OrderStatusCollection GetAllOrderStatuses()
        {
            string key = string.Format(ORDERSTATUSES_ALL_KEY);
            object obj2 = NopCache.Get(key);
            if (OrderManager.CacheEnabled && (obj2 != null))
            {
                return (OrderStatusCollection)obj2;
            }

            var dbCollection = DBProviderManager<DBOrderProvider>.Provider.GetAllOrderStatuses();
            var orderStatusCollection = DBMapping(dbCollection);

            if (OrderManager.CacheEnabled)
            {
                NopCache.Max(key, orderStatusCollection);
            }
            return orderStatusCollection;
        }

        #endregion

        #region Reports

        /// <summary>
        /// Get order product variant sales report
        /// </summary>
        /// <param name="StartTime">Order start time; null to load all</param>
        /// <param name="EndTime">Order end time; null to load all</param>
        /// <param name="OS">Order status; null to load all orders</param>
        /// <param name="PS">Order payment status; null to load all orders</param>
        /// <returns>Result</returns>
        public static IDataReader OrderProductVariantReport(DateTime? StartTime, DateTime? EndTime,
            OrderStatusEnum? OS, PaymentStatusEnum? PS)
        {
            int? orderStatusID = null;
            if (OS.HasValue)
                orderStatusID = (int)OS.Value;

            int? paymentStatusID = null;
            if (PS.HasValue)
                paymentStatusID = (int)PS.Value;

            return DBProviderManager<DBOrderProvider>.Provider.OrderProductVariantReport(StartTime, EndTime, orderStatusID, paymentStatusID);
        }

        /// <summary>
        /// Get the bests sellers report
        /// </summary>
        /// <param name="LastDays">Last number of days</param>
        /// <param name="RecordsToReturn">Number of products to return</param>
        /// <param name="OrderBy">1 - order by total count, 2 - Order by total amount</param>
        /// <returns>Result</returns>
        public static List<BestSellersReportLine> BestSellersReport(int LastDays, int RecordsToReturn, int OrderBy)
        {
            var dbCollection = DBProviderManager<DBOrderProvider>.Provider.BestSellersReport(LastDays, RecordsToReturn, OrderBy);
            var report = DBMapping(dbCollection);
            return report;
        }

        /// <summary>
        /// Get order average report
        /// </summary>
        /// <param name="OS">Order status; null to load all orders</param>
        /// <returns>Result</returns>
        public static OrderAverageReportLine OrderAverageReport(OrderStatusEnum OS)
        {
            int orderStatusID = (int)OS;
            var dbItem = DBProviderManager<DBOrderProvider>.Provider.OrderAverageReport(orderStatusID);
            var orderAverageReportLine = DBMapping(dbItem);
            orderAverageReportLine.OrderStatus = OS;
            return orderAverageReportLine;
        }

        /// <summary>
        /// Gets an order report
        /// </summary>
        /// <param name="OS">Order status; null to load all orders</param>
        /// <param name="PS">Order payment status; null to load all orders</param>
        /// <param name="SS">Order shippment status; null to load all orders</param>
        /// <returns>IDataReader</returns>
        public static IDataReader GetOrderReport(OrderStatusEnum? OS, PaymentStatusEnum? PS, ShippingStatusEnum? SS)
        {
            int? orderStatusID = null;
            if (OS.HasValue)
                orderStatusID = (int)OS.Value;

            int? paymentStatusID = null;
            if (PS.HasValue)
                paymentStatusID = (int)PS.Value;

            int? shippmentStatusID = null;
            if (SS.HasValue)
                shippmentStatusID = (int)SS.Value;

            return DBProviderManager<DBOrderProvider>.Provider.GetOrderReport(orderStatusID, paymentStatusID, shippmentStatusID);
        }
       
        #endregion

        #region Recurring payments

        /// <summary>
        /// Gets a recurring payment
        /// </summary>
        /// <param name="RecurringPaymentID">The recurring payment identifier</param>
        /// <returns>Recurring payment</returns>
        public static RecurringPayment GetRecurringPaymentByID(int RecurringPaymentID)
        {
            if (RecurringPaymentID == 0)
                return null;

            var dbItem = DBProviderManager<DBOrderProvider>.Provider.GetRecurringPaymentByID(RecurringPaymentID);
            var recurringPayment = DBMapping(dbItem);
            return recurringPayment;
        }

        /// <summary>
        /// Deletes a recurring payment
        /// </summary>
        /// <param name="RecurringPaymentID">Recurring payment identifier</param>
        public static void DeleteRecurringPayment(int RecurringPaymentID)
        {
            var recurringPayment = GetRecurringPaymentByID(RecurringPaymentID);
            if (recurringPayment != null)
            {
                UpdateRecurringPayment(recurringPayment.RecurringPaymentID, recurringPayment.InitialOrderID,
                    recurringPayment.CycleLength, recurringPayment.CyclePeriod,
                    recurringPayment.TotalCycles, recurringPayment.StartDate,
                    recurringPayment.IsActive, true, recurringPayment.CreatedOn);
            }
        }

        /// <summary>
        /// Inserts a recurring payment
        /// </summary>
        /// <param name="InitialOrderID">The initial order identifier</param>
        /// <param name="CycleLength">The cycle length</param>
        /// <param name="CyclePeriod">The cycle period</param>
        /// <param name="TotalCycles">The total cycles</param>
        /// <param name="StartDate">The start date</param>
        /// <param name="IsActive">The value indicating whether the payment is active</param>
        /// <param name="Deleted">The value indicating whether the entity has been deleted</param>
        /// <param name="CreatedOn">The date and time of payment creation</param>
        /// <returns>Recurring payment</returns>
        public static RecurringPayment InsertRecurringPayment(int InitialOrderID,
            int CycleLength, int CyclePeriod, int TotalCycles,
            DateTime StartDate, bool IsActive, bool Deleted, DateTime CreatedOn)
        {
            StartDate = DateTimeHelper.ConvertToUtcTime(StartDate);
            CreatedOn = DateTimeHelper.ConvertToUtcTime(CreatedOn);

            var dbItem = DBProviderManager<DBOrderProvider>.Provider.InsertRecurringPayment(InitialOrderID,
                CycleLength, CyclePeriod, TotalCycles, StartDate, IsActive, Deleted, CreatedOn);
            var recurringPayment = DBMapping(dbItem);
            return recurringPayment;
        }

        /// <summary>
        /// Updates the recurring payment
        /// </summary>
        /// <param name="RecurringPaymentID">The recurring payment identifier</param>
        /// <param name="InitialOrderID">The initial order identifier</param>
        /// <param name="CycleLength">The cycle length</param>
        /// <param name="CyclePeriod">The cycle period</param>
        /// <param name="TotalCycles">The total cycles</param>
        /// <param name="StartDate">The start date</param>
        /// <param name="IsActive">The value indicating whether the payment is active</param>
        /// <param name="Deleted">The value indicating whether the entity has been deleted</param>
        /// <param name="CreatedOn">The date and time of payment creation</param>
        /// <returns>Recurring payment</returns>
        public static RecurringPayment UpdateRecurringPayment(int RecurringPaymentID,
            int InitialOrderID, int CycleLength, int CyclePeriod, int TotalCycles,
            DateTime StartDate, bool IsActive, bool Deleted, DateTime CreatedOn)
        {
            StartDate = DateTimeHelper.ConvertToUtcTime(StartDate);
            CreatedOn = DateTimeHelper.ConvertToUtcTime(CreatedOn);

            var dbItem = DBProviderManager<DBOrderProvider>.Provider.UpdateRecurringPayment(RecurringPaymentID,
                InitialOrderID, CycleLength, CyclePeriod, TotalCycles,
                StartDate, IsActive, Deleted, CreatedOn);
            var recurringPayment = DBMapping(dbItem);
            return recurringPayment;
        }

        /// <summary>
        /// Search recurring payments
        /// </summary>
        /// <param name="CustomerID">The customer identifier; 0 to load all records</param>
        /// <param name="InitialOrderID">The initial order identifier; 0 to load all records</param>
        /// <param name="InitialOrderStatus">Initial order status identifier; null to load all records</param>
        /// <returns>Recurring payment collection</returns>
        public static RecurringPaymentCollection SearchRecurringPayments(int CustomerID, 
            int InitialOrderID, OrderStatusEnum? InitialOrderStatus)
        {
            bool showHidden = NopContext.Current.IsAdmin;
            return SearchRecurringPayments(showHidden, CustomerID, InitialOrderID, InitialOrderStatus);
        }

        /// <summary>
        /// Search recurring payments
        /// </summary>
        /// <param name="showHidden">A value indicating whether to show hidden records</param>
        /// <param name="CustomerID">The customer identifier; 0 to load all records</param>
        /// <param name="InitialOrderID">The initial order identifier; 0 to load all records</param>
        /// <param name="InitialOrderStatus">Initial order status identifier; null to load all records</param>
        /// <returns>Recurring payment collection</returns>
        public static RecurringPaymentCollection SearchRecurringPayments(bool showHidden,
            int CustomerID, int InitialOrderID, OrderStatusEnum? InitialOrderStatus)
        {
            int? initialOrderStatusID = null;
            if (InitialOrderStatus.HasValue)
                initialOrderStatusID = (int)InitialOrderStatus.Value;

            var dbCollection = DBProviderManager<DBOrderProvider>.Provider.SearchRecurringPayments(showHidden, 
                CustomerID, InitialOrderID, initialOrderStatusID);
            var recurringPayments = DBMapping(dbCollection);
            return recurringPayments;
        }

        /// <summary>
        /// Deletes a recurring payment history
        /// </summary>
        /// <param name="RecurringPaymentHistoryID">Recurring payment history identifier</param>
        public static void DeleteRecurringPaymentHistory(int RecurringPaymentHistoryID)
        {
            DBProviderManager<DBOrderProvider>.Provider.DeleteRecurringPaymentHistory(RecurringPaymentHistoryID);
        }

        /// <summary>
        /// Gets a recurring payment history
        /// </summary>
        /// <param name="RecurringPaymentHistoryID">The recurring payment history identifier</param>
        /// <returns>Recurring payment history</returns>
        public static RecurringPaymentHistory GetRecurringPaymentHistoryByID(int RecurringPaymentHistoryID)
        {
            if (RecurringPaymentHistoryID == 0)
                return null;

            var dbItem = DBProviderManager<DBOrderProvider>.Provider.GetRecurringPaymentHistoryByID(RecurringPaymentHistoryID);
            var recurringPaymentHistory = DBMapping(dbItem);
            return recurringPaymentHistory;
        }

        /// <summary>
        /// Inserts a recurring payment history
        /// </summary>
        /// <param name="RecurringPaymentID">The recurring payment identifier</param>
        /// <param name="OrderID">The order identifier</param>
        /// <param name="CreatedOn">The date and time of payment creation</param>
        /// <returns>Recurring payment history</returns>
        public static RecurringPaymentHistory InsertRecurringPaymentHistory(int RecurringPaymentID,
            int OrderID, DateTime CreatedOn)
        {
            CreatedOn = DateTimeHelper.ConvertToUtcTime(CreatedOn);

            var dbItem = DBProviderManager<DBOrderProvider>.Provider.InsertRecurringPaymentHistory(RecurringPaymentID,
                OrderID, CreatedOn);
            var recurringPaymentHistory = DBMapping(dbItem);
            return recurringPaymentHistory;
        }

        /// <summary>
        /// Updates the recurring payment history
        /// </summary>
        /// <param name="RecurringPaymentHistoryID">The recurring payment history identifier</param>
        /// <param name="RecurringPaymentID">The recurring payment identifier</param>
        /// <param name="OrderID">The order identifier</param>
        /// <param name="CreatedOn">The date and time of payment creation</param>
        /// <returns>Recurring payment history</returns>
        public static RecurringPaymentHistory UpdateRecurringPaymentHistory(int RecurringPaymentHistoryID,
            int RecurringPaymentID, int OrderID, DateTime CreatedOn)
        {
            CreatedOn = DateTimeHelper.ConvertToUtcTime(CreatedOn);

            var dbItem = DBProviderManager<DBOrderProvider>.Provider.UpdateRecurringPaymentHistory(RecurringPaymentHistoryID,
                RecurringPaymentID, OrderID, CreatedOn);
            var recurringPaymentHistory = DBMapping(dbItem);
            return recurringPaymentHistory;
        }

        /// <summary>
        /// Search recurring payment history
        /// </summary>
        /// <param name="RecurringPaymentID">The recurring payment identifier; 0 to load all records</param>
        /// <param name="OrderID">The order identifier; 0 to load all records</param>
        /// <returns>Recurring payment history collection</returns>
        public static RecurringPaymentHistoryCollection SearchRecurringPaymentHistory(int RecurringPaymentID, int OrderID)
        {
            var dbCollection = DBProviderManager<DBOrderProvider>.Provider.SearchRecurringPaymentHistory(RecurringPaymentID, OrderID);
            var recurringPaymentHistoryCollection = DBMapping(dbCollection);
            return recurringPaymentHistoryCollection;
        }

        #endregion

        #region Gift Cards

        /// <summary>
        /// Deletes a gift card
        /// </summary>
        /// <param name="GiftCardID">Gift card identifier</param>
        public static void DeleteGiftCard(int GiftCardID)
        {
            DBProviderManager<DBOrderProvider>.Provider.DeleteGiftCard(GiftCardID);
        }

        /// <summary>
        /// Gets a gift card
        /// </summary>
        /// <param name="GiftCardID">Gift card identifier</param>
        /// <returns>Gift card entry</returns>
        public static GiftCard GetGiftCardByID(int GiftCardID)
        {
            if (GiftCardID == 0)
                return null;

            var dbItem = DBProviderManager<DBOrderProvider>.Provider.GetGiftCardByID(GiftCardID);
            var giftCard = DBMapping(dbItem);
            return giftCard;
        }

        /// <summary>
        /// Gets all gift cards
        /// </summary>
        /// <param name="OrderID">Order identifier; null to load all records</param>
        /// <param name="CustomerID">Customer identifier; null to load all records</param>
        /// <param name="StartTime">Order start time; null to load all records</param>
        /// <param name="EndTime">Order end time; null to load all records</param>
        /// <param name="OS">Order status; null to load all records</param>
        /// <param name="PS">Order payment status; null to load all records</param>
        /// <param name="SS">Order shippment status; null to load all records</param>
        /// <param name="IsGiftCardActivated">Value indicating whether gift card is activated; null to load all records</param>
        /// <param name="GiftCardCouponCode">Gift card coupon code; null or string.empty to load all records</param>
        /// <returns>Gift cards</returns>
        public static GiftCardCollection GetAllGiftCards(int? OrderID,
            int? CustomerID, DateTime? StartTime, DateTime? EndTime,
            OrderStatusEnum? OS, PaymentStatusEnum? PS, ShippingStatusEnum? SS,
            bool? IsGiftCardActivated, string GiftCardCouponCode)
        {
            int? orderStatusID = null;
            if (OS.HasValue)
                orderStatusID = (int)OS.Value;

            int? paymentStatusID = null;
            if (PS.HasValue)
                paymentStatusID = (int)PS.Value;

            int? shippingStatusID = null;
            if (SS.HasValue)
                shippingStatusID = (int)SS.Value;

            if (GiftCardCouponCode != null)
                GiftCardCouponCode = GiftCardCouponCode.Trim();

            var dbCollection = DBProviderManager<DBOrderProvider>.Provider.GetAllGiftCards(OrderID,
                CustomerID, StartTime, EndTime, orderStatusID, paymentStatusID, shippingStatusID,
                IsGiftCardActivated, GiftCardCouponCode);
            var giftCards = DBMapping(dbCollection);
            return giftCards;
        }

        /// <summary>
        /// Inserts a gift card
        /// </summary>
        /// <param name="PurchasedOrderProductVariantID">Purchased order product variant identifier</param>
        /// <param name="Amount">Amount</param>
        /// <param name="IsGiftCardActivated">Value indicating whether gift card is activated</param>
        /// <param name="GiftCardCouponCode">Gift card coupon code</param>
        /// <param name="RecipientName">Recipient name</param>
        /// <param name="RecipientEmail">Recipient email</param>
        /// <param name="SenderName">Sender name</param>
        /// <param name="SenderEmail">Sender email</param>
        /// <param name="Message">Message</param>
        /// <param name="IsSenderNotified">Value indicating whether sender is notified</param>
        /// <param name="CreatedOn">A date and time of instance creation</param>
        /// <returns>Gift card</returns>
        public static GiftCard InsertGiftCard(int PurchasedOrderProductVariantID,
            decimal Amount, bool IsGiftCardActivated, string GiftCardCouponCode,
            string RecipientName, string RecipientEmail,
            string SenderName, string SenderEmail, string Message,
            bool IsSenderNotified, DateTime CreatedOn)
        {
            CreatedOn = DateTimeHelper.ConvertToUtcTime(CreatedOn);

            var dbItem = DBProviderManager<DBOrderProvider>.Provider.InsertGiftCard(PurchasedOrderProductVariantID,
                Amount, IsGiftCardActivated, GiftCardCouponCode, RecipientName, RecipientEmail,
                SenderName, SenderEmail, Message, IsSenderNotified, CreatedOn);
            var giftCard = DBMapping(dbItem);
            return giftCard;
        }

        /// <summary>
        /// Updates the gift card
        /// </summary>
        /// <param name="GiftCardID">Gift card identifier</param>
        /// <param name="PurchasedOrderProductVariantID">Purchased order product variant identifier</param>
        /// <param name="Amount">Amount</param>
        /// <param name="IsGiftCardActivated">Value indicating whether gift card is activated</param>
        /// <param name="GiftCardCouponCode">Gift card coupon code</param>
        /// <param name="RecipientName">Recipient name</param>
        /// <param name="RecipientEmail">Recipient email</param>
        /// <param name="SenderName">Sender name</param>
        /// <param name="SenderEmail">Sender email</param>
        /// <param name="Message">Message</param>
        /// <param name="IsSenderNotified">Value indicating whether sender is notified</param>
        /// <param name="CreatedOn">A date and time of instance creation</param>
        /// <returns>Gift card</returns>
        public static GiftCard UpdateGiftCard(int GiftCardID, 
            int PurchasedOrderProductVariantID, decimal Amount, 
            bool IsGiftCardActivated, string GiftCardCouponCode,
            string RecipientName, string RecipientEmail,
            string SenderName, string SenderEmail, string Message,
            bool IsSenderNotified, DateTime CreatedOn)
        {
            CreatedOn = DateTimeHelper.ConvertToUtcTime(CreatedOn);

            var dbItem = DBProviderManager<DBOrderProvider>.Provider.UpdateGiftCard(GiftCardID,
                PurchasedOrderProductVariantID, Amount, IsGiftCardActivated, 
                GiftCardCouponCode, RecipientName, RecipientEmail,
                SenderName, SenderEmail, Message, IsSenderNotified, CreatedOn);
            var giftCard = DBMapping(dbItem);
            return giftCard;
        }

        /// <summary>
        /// Deletes a gift card usage history entry
        /// </summary>
        /// <param name="GiftCardUsageHistoryID">Gift card usage history entry identifier</param>
        public static void DeleteGiftCardUsageHistory(int GiftCardUsageHistoryID)
        {
            DBProviderManager<DBOrderProvider>.Provider.DeleteGiftCardUsageHistory(GiftCardUsageHistoryID);
        }

        /// <summary>
        /// Gets a gift card usage history entry
        /// </summary>
        /// <param name="GiftCardUsageHistoryID">Gift card usage history entry identifier</param>
        /// <returns>Gift card usage history entry</returns>
        public static GiftCardUsageHistory GetGiftCardUsageHistoryByID(int GiftCardUsageHistoryID)
        {
            if (GiftCardUsageHistoryID == 0)
                return null;

            var dbItem = DBProviderManager<DBOrderProvider>.Provider.GetGiftCardUsageHistoryByID(GiftCardUsageHistoryID);
            var giftCardUsageHistory = DBMapping(dbItem);
            return giftCardUsageHistory;
        }

        /// <summary>
        /// Gets all gift card usage history entries
        /// </summary>
        /// <param name="GiftCardID">Gift card identifier; null to load all records</param>
        /// <param name="CustomerID">Customer identifier; null to load all records</param>
        /// <param name="OrderID">Order identifier; null to load all records</param>
        /// <returns>Gift card usage history entries</returns>
        public static GiftCardUsageHistoryCollection GetAllGiftCardUsageHistoryEntries(int? GiftCardID,
            int? CustomerID, int? OrderID)
        {
            var dbCollection = DBProviderManager<DBOrderProvider>.Provider.GetAllGiftCardUsageHistoryEntries(GiftCardID, CustomerID, OrderID);
            var giftCardUsageHistoryEntries = DBMapping(dbCollection);
            return giftCardUsageHistoryEntries;
        }

        /// <summary>
        /// Inserts a gift card usage history entry
        /// </summary>
        /// <param name="GiftCardID">Gift card identifier</param>
        /// <param name="CustomerID">Customer identifier</param>
        /// <param name="OrderID">Order identifier</param>
        /// <param name="UsedValue">Used value</param>
        /// <param name="UsedValueInCustomerCurrency">Used value (customer currency)</param>
        /// <param name="CreatedOn">A date and time of instance creation</param>
        /// <returns>Gift card usage history entry</returns>
        public static GiftCardUsageHistory InsertGiftCardUsageHistory(int GiftCardID,
            int CustomerID, int OrderID, decimal UsedValue, 
            decimal UsedValueInCustomerCurrency, DateTime CreatedOn)
        {
            CreatedOn = DateTimeHelper.ConvertToUtcTime(CreatedOn);

            var dbItem = DBProviderManager<DBOrderProvider>.Provider.InsertGiftCardUsageHistory(GiftCardID, 
                CustomerID, OrderID, UsedValue, UsedValueInCustomerCurrency, CreatedOn);
            var giftCardUsageHistory = DBMapping(dbItem);
            return giftCardUsageHistory;
        }

        /// <summary>
        /// Updates the gift card usage history entry
        /// </summary>
        /// <param name="GiftCardUsageHistoryID">Gift card usage history entry identifier</param>
        /// <param name="GiftCardID">Gift card identifier</param>
        /// <param name="CustomerID">Customer identifier</param>
        /// <param name="OrderID">Order identifier</param>
        /// <param name="UsedValue">Used value</param>
        /// <param name="UsedValueInCustomerCurrency">Used value (customer currency)</param>
        /// <param name="CreatedOn">A date and time of instance creation</param>
        /// <returns>Gift card usage history entry</returns>
        public static GiftCardUsageHistory UpdateGiftCardUsageHistory(int GiftCardUsageHistoryID,
            int GiftCardID, int CustomerID, int OrderID, decimal UsedValue,
            decimal UsedValueInCustomerCurrency, DateTime CreatedOn)
        {
            CreatedOn = DateTimeHelper.ConvertToUtcTime(CreatedOn);

            var dbItem = DBProviderManager<DBOrderProvider>.Provider.UpdateGiftCardUsageHistory(GiftCardUsageHistoryID,
                GiftCardID, CustomerID, OrderID, UsedValue, UsedValueInCustomerCurrency, CreatedOn);
            var giftCardUsageHistory = DBMapping(dbItem);
            return giftCardUsageHistory;
        }

        #endregion

        #endregion

        #region Helper methods
        /// <summary>
        /// Gets a value indicating whether download is allowed
        /// </summary>
        /// <param name="orderProductVariant">Order produvt variant to check</param>
        /// <returns>True if download is allowed; otherwise, false.</returns>
        public static bool IsDownloadAllowed(OrderProductVariant orderProductVariant)
        {
            if (orderProductVariant == null)
                return false;

            var order = orderProductVariant.Order;
            if (order == null || order.Deleted)
                return false;

            //order status
            if (order.OrderStatus == OrderStatusEnum.Cancelled)
                return false;

            var productVariant = orderProductVariant.ProductVariant;
            if (productVariant == null || !productVariant.IsDownload)
                return false;

            //payment status
            switch (productVariant.DownloadActivationType)
            {
                case (int)DownloadActivationTypeEnum.WhenOrderIsPaid:
                    {
                        if (order.PaymentStatus == PaymentStatusEnum.Paid && order.PaidDate.HasValue)
                        {
                            //expiration date
                            if (productVariant.DownloadExpirationDays.HasValue)
                            {
                                if (order.PaidDate.Value.AddDays(productVariant.DownloadExpirationDays.Value) > DateTime.UtcNow)
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
                case (int)DownloadActivationTypeEnum.Manually:
                    {
                        if (orderProductVariant.IsDownloadActivated)
                        {
                            //expiration date
                            if (productVariant.DownloadExpirationDays.HasValue)
                            {
                                if (order.CreatedOn.AddDays(productVariant.DownloadExpirationDays.Value) > DateTime.UtcNow)
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
        public static bool IsLicenseDownloadAllowed(OrderProductVariant orderProductVariant)
        {
            if (orderProductVariant == null)
                return false;

            return IsDownloadAllowed(orderProductVariant) && orderProductVariant.LicenseDownloadID > 0;
        }

        /// <summary>
        /// Formats the order note text
        /// </summary>
        /// <param name="Text">Text</param>
        /// <returns>Formatted text</returns>
        public static string FormatOrderNoteText(string Text)
        {
            if (String.IsNullOrEmpty(Text))
                return string.Empty;

            Text = HtmlHelper.FormatText(Text, false, true, false, false, false, false);
            return Text;
        }

        #endregion

        #region Order workflow

        /// <summary>
        /// Places an order
        /// </summary>
        /// <param name="paymentInfo">Payment info</param>
        /// <param name="customer">Customer</param>
        /// <param name="OrderID">Order identifier</param>
        /// <returns>The error status, or String.Empty if no errors</returns>
        public static string PlaceOrder(PaymentInfo paymentInfo, Customer customer, 
            out int OrderID)
        {
            var OrderGuid = Guid.NewGuid();
            return PlaceOrder(paymentInfo, customer, OrderGuid, out OrderID);
        }

        /// <summary>
        /// Places an order
        /// </summary>
        /// <param name="paymentInfo">Payment info</param>
        /// <param name="customer">Customer</param>
        /// <param name="OrderGuid">Order GUID to use</param>
        /// <param name="OrderID">Order identifier</param>
        /// <returns>The error status, or String.Empty if no errors</returns>
        public static string PlaceOrder(PaymentInfo paymentInfo, Customer customer, 
            Guid OrderGuid, out int OrderID)
        {
            OrderID = 0;
            var processPaymentResult = new ProcessPaymentResult();
            try
            {
                if (customer == null)
                    throw new ArgumentNullException("customer");

                if (customer.IsGuest && !CustomerManager.AnonymousCheckoutAllowed)
                    throw new NopException("Anonymous checkout is not allowed");

                if (!CommonHelper.IsValidEmail(customer.Email))
                {
                    throw new NopException("Email is not valid");
                }

                if (paymentInfo == null)
                    throw new ArgumentNullException("paymentInfo");

                Order initialOrder = null;
                if (paymentInfo.IsRecurringPayment)
                {
                    initialOrder = GetOrderByID(paymentInfo.InitialOrderID);
                    if (initialOrder == null)
                        throw new NopException("Initial order could not be loaded");
                }

                if (!paymentInfo.IsRecurringPayment)
                {
                    if (paymentInfo.BillingAddress == null)
                        throw new NopException("Billing address not provided");

                    if (!CommonHelper.IsValidEmail(paymentInfo.BillingAddress.Email))
                    {
                        throw new NopException("Email is not valid");
                    }
                }

                if (paymentInfo.IsRecurringPayment)
                {
                    paymentInfo.PaymentMethodID = initialOrder.PaymentMethodID;
                }

                var paymentMethod = PaymentMethodManager.GetPaymentMethodByID(paymentInfo.PaymentMethodID);
                if (paymentMethod == null)
                    throw new NopException("Payment method couldn't be loaded");

                if (!paymentMethod.IsActive)
                    throw new NopException("Payment method is not active");

                if (paymentInfo.CreditCardCVV2 == null)
                    paymentInfo.CreditCardCVV2 = string.Empty;

                if (paymentInfo.CreditCardName == null)
                    paymentInfo.CreditCardName = string.Empty;

                if (paymentInfo.CreditCardNumber == null)
                    paymentInfo.CreditCardNumber = string.Empty;

                if (paymentInfo.CreditCardType == null)
                    paymentInfo.CreditCardType = string.Empty;

                if (paymentInfo.PurchaseOrderNumber == null)
                    paymentInfo.PurchaseOrderNumber = string.Empty;

                ShoppingCart cart = null;
                if (!paymentInfo.IsRecurringPayment)
                {
                    cart = ShoppingCartManager.GetCustomerShoppingCart(customer.CustomerID, ShoppingCartTypeEnum.ShoppingCart);

                    //validate cart
                    var warnings = ShoppingCartManager.GetShoppingCartWarnings(cart);
                    if (warnings.Count > 0)
                    {
                        StringBuilder warningsSb = new StringBuilder();
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
                        var sciWarnings = ShoppingCartManager.GetShoppingCartItemWarnings(sci.ShoppingCartType,
                            sci.ProductVariantID, sci.AttributesXML, sci.Quantity);

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

                //tax type
                var customerTaxDisplayType = TaxDisplayTypeEnum.IncludingTax;
                if (!paymentInfo.IsRecurringPayment)
                {
                    if (TaxManager.AllowCustomersToSelectTaxDisplayType)
                        customerTaxDisplayType = customer.TaxDisplayType;
                    else
                        customerTaxDisplayType = TaxManager.TaxDisplayType;
                }
                else
                {
                    customerTaxDisplayType = initialOrder.CustomerTaxDisplayType;
                }

                //discount usage history
                var appliedDiscounts = new DiscountCollection();


                //sub total
                decimal orderSubTotalInclTax = decimal.Zero;
                decimal orderSubTotalExclTax = decimal.Zero;
                decimal orderSubTotalDiscountAmount = decimal.Zero;
                decimal orderSubtotalInclTaxInCustomerCurrency = decimal.Zero;
                decimal orderSubtotalExclTaxInCustomerCurrency = decimal.Zero;
                decimal orderSubtotalDiscountInCustomerCurrency = decimal.Zero;
                List<AppliedGiftCard> appliedGiftCards = null;
                if (!paymentInfo.IsRecurringPayment)
                {
                    Discount subTotalAppliedDiscountInclTax = null;
                    decimal subtotalBaseWithPromoInclTax = decimal.Zero;
                    string SubTotalError1 = ShoppingCartManager.GetShoppingCartSubTotal(cart, customer,
                        out orderSubTotalDiscountAmount, out subTotalAppliedDiscountInclTax,
                        out appliedGiftCards, true,
                        out orderSubTotalInclTax, out subtotalBaseWithPromoInclTax);
                    

                    Discount subTotalAppliedDiscountExclTax = null;
                    decimal subtotalBaseWithPromoExclTax = decimal.Zero;
                    string SubTotalError2 = ShoppingCartManager.GetShoppingCartSubTotal(cart, customer,
                        out orderSubTotalDiscountAmount, out subTotalAppliedDiscountExclTax,
                        out appliedGiftCards, false,
                        out orderSubTotalExclTax, out subtotalBaseWithPromoExclTax);
                    
                    if (!String.IsNullOrEmpty(SubTotalError1) || !String.IsNullOrEmpty(SubTotalError2))
                        throw new NopException("Sub total couldn't be calculated");

                    if (subTotalAppliedDiscountInclTax != null && !appliedDiscounts.ContainsDiscount(subTotalAppliedDiscountInclTax.Name))
                        appliedDiscounts.Add(subTotalAppliedDiscountInclTax);
                    if (subTotalAppliedDiscountExclTax != null && !appliedDiscounts.ContainsDiscount(subTotalAppliedDiscountExclTax.Name))
                        appliedDiscounts.Add(subTotalAppliedDiscountExclTax);

                    //in customer currency
                    orderSubtotalInclTaxInCustomerCurrency = CurrencyManager.ConvertCurrency(orderSubTotalInclTax, CurrencyManager.PrimaryStoreCurrency, paymentInfo.CustomerCurrency);
                    orderSubtotalExclTaxInCustomerCurrency = CurrencyManager.ConvertCurrency(orderSubTotalExclTax, CurrencyManager.PrimaryStoreCurrency, paymentInfo.CustomerCurrency);
                    orderSubtotalDiscountInCustomerCurrency = CurrencyManager.ConvertCurrency(orderSubTotalDiscountAmount, CurrencyManager.PrimaryStoreCurrency, paymentInfo.CustomerCurrency);
                }
                else
                {
                    orderSubTotalInclTax = initialOrder.OrderSubtotalInclTax;
                    orderSubTotalExclTax = initialOrder.OrderSubtotalExclTax;
                    orderSubTotalDiscountAmount = initialOrder.OrderDiscount;

                    //in customer currency
                    orderSubtotalInclTaxInCustomerCurrency = initialOrder.OrderSubtotalInclTaxInCustomerCurrency;
                    orderSubtotalExclTaxInCustomerCurrency = initialOrder.OrderSubtotalExclTaxInCustomerCurrency;
                    orderSubtotalDiscountInCustomerCurrency = initialOrder.OrderDiscountInCustomerCurrency;
                }


                //shipping info
                decimal orderWeight = decimal.Zero;
                bool shoppingCartRequiresShipping = false;
                if (!paymentInfo.IsRecurringPayment)
                {
                    orderWeight = ShippingManager.GetShoppingCartTotalWeigth(cart);
                    shoppingCartRequiresShipping = ShippingManager.ShoppingCartRequiresShipping(cart);
                    if (shoppingCartRequiresShipping)
                    {
                        if (paymentInfo.ShippingAddress == null)
                            throw new NopException("Shipping address is not provided");

                        if (!CommonHelper.IsValidEmail(paymentInfo.ShippingAddress.Email))
                        {
                            throw new NopException("Email is not valid");
                        }
                    }
                }
                else
                {
                    orderWeight = initialOrder.OrderWeight;
                    if (initialOrder.ShippingStatus != ShippingStatusEnum.ShippingNotRequired)
                        shoppingCartRequiresShipping = true;
                }


                //shipping total
                decimal? orderShippingTotalInclTax = null;
                decimal? orderShippingTotalExclTax = null;
                decimal orderShippingInclTaxInCustomerCurrency = decimal.Zero;
                decimal orderShippingExclTaxInCustomerCurrency = decimal.Zero;
                if (!paymentInfo.IsRecurringPayment)
                {
                    string ShippingTotalError1 = string.Empty;
                    string ShippingTotalError2 = string.Empty;
                    Discount shippingTotalDiscount = null;
                    orderShippingTotalInclTax = ShippingManager.GetShoppingCartShippingTotal(cart, customer, true, out shippingTotalDiscount, ref ShippingTotalError1);
                    orderShippingTotalExclTax = ShippingManager.GetShoppingCartShippingTotal(cart, customer, false, ref ShippingTotalError2);
                    if (!orderShippingTotalInclTax.HasValue || !orderShippingTotalExclTax.HasValue)
                        throw new NopException("Shipping total couldn't be calculated");
                    if (shippingTotalDiscount != null && !appliedDiscounts.ContainsDiscount(shippingTotalDiscount.Name))
                        appliedDiscounts.Add(shippingTotalDiscount);

                    //in customer currency
                    orderShippingInclTaxInCustomerCurrency = CurrencyManager.ConvertCurrency(orderShippingTotalInclTax.Value, CurrencyManager.PrimaryStoreCurrency, paymentInfo.CustomerCurrency);
                    orderShippingExclTaxInCustomerCurrency = CurrencyManager.ConvertCurrency(orderShippingTotalExclTax.Value, CurrencyManager.PrimaryStoreCurrency, paymentInfo.CustomerCurrency);

                }
                else
                {
                    orderShippingTotalInclTax = initialOrder.OrderShippingInclTax;
                    orderShippingTotalExclTax = initialOrder.OrderShippingExclTax;
                    orderShippingInclTaxInCustomerCurrency = initialOrder.OrderShippingInclTaxInCustomerCurrency;
                    orderShippingExclTaxInCustomerCurrency = initialOrder.OrderShippingExclTaxInCustomerCurrency;
                }


                //payment total
                decimal paymentAdditionalFeeInclTax = decimal.Zero;
                decimal paymentAdditionalFeeExclTax = decimal.Zero;
                decimal paymentAdditionalFeeInclTaxInCustomerCurrency = decimal.Zero;
                decimal paymentAdditionalFeeExclTaxInCustomerCurrency = decimal.Zero;
                if (!paymentInfo.IsRecurringPayment)
                {
                    string PaymentAdditionalFeeError1 = string.Empty;
                    string PaymentAdditionalFeeError2 = string.Empty;
                    decimal paymentAdditionalFee = PaymentManager.GetAdditionalHandlingFee(paymentInfo.PaymentMethodID);
                    paymentAdditionalFeeInclTax = TaxManager.GetPaymentMethodAdditionalFee(paymentAdditionalFee, true, customer, ref PaymentAdditionalFeeError1);
                    paymentAdditionalFeeExclTax = TaxManager.GetPaymentMethodAdditionalFee(paymentAdditionalFee, false, customer, ref PaymentAdditionalFeeError2);
                    if (!String.IsNullOrEmpty(PaymentAdditionalFeeError1))
                        throw new NopException("Payment method fee couldn't be calculated");
                    if (!String.IsNullOrEmpty(PaymentAdditionalFeeError2))
                        throw new NopException("Payment method fee couldn't be calculated");

                    //in customer currency
                    paymentAdditionalFeeInclTaxInCustomerCurrency = CurrencyManager.ConvertCurrency(paymentAdditionalFeeInclTax, CurrencyManager.PrimaryStoreCurrency, paymentInfo.CustomerCurrency);
                    paymentAdditionalFeeExclTaxInCustomerCurrency = CurrencyManager.ConvertCurrency(paymentAdditionalFeeExclTax, CurrencyManager.PrimaryStoreCurrency, paymentInfo.CustomerCurrency);
                }
                else
                {
                    paymentAdditionalFeeInclTax = initialOrder.PaymentMethodAdditionalFeeInclTax;
                    paymentAdditionalFeeExclTax = initialOrder.PaymentMethodAdditionalFeeExclTax;
                    paymentAdditionalFeeInclTaxInCustomerCurrency = initialOrder.PaymentMethodAdditionalFeeInclTaxInCustomerCurrency;
                    paymentAdditionalFeeExclTaxInCustomerCurrency = initialOrder.PaymentMethodAdditionalFeeExclTaxInCustomerCurrency;
                }


                //tax total
                decimal orderTaxTotal = decimal.Zero;
                decimal orderTaxInCustomerCurrency = decimal.Zero;
                if (!paymentInfo.IsRecurringPayment)
                {
                    string TaxError = string.Empty;
                    orderTaxTotal = TaxManager.GetTaxTotal(cart, paymentInfo.PaymentMethodID, customer, ref TaxError);
                    if (!String.IsNullOrEmpty(TaxError))
                        throw new NopException("Tax total couldn't be calculated");

                    //in customer currency
                    orderTaxInCustomerCurrency = CurrencyManager.ConvertCurrency(orderTaxTotal, CurrencyManager.PrimaryStoreCurrency, paymentInfo.CustomerCurrency);
                }
                else
                {
                    orderTaxTotal = initialOrder.OrderTax;
                    orderTaxInCustomerCurrency = initialOrder.OrderTaxInCustomerCurrency;
                }


                //order total
                decimal? orderTotal = null;
                decimal orderTotalInCustomerCurrency = decimal.Zero;
                if (!paymentInfo.IsRecurringPayment)
                {
                    orderTotal = ShoppingCartManager.GetShoppingCartTotal(cart, paymentInfo.PaymentMethodID, customer);
                    if (!orderTotal.HasValue)
                        throw new NopException("Order total couldn't be calculated");

                    //in customer currency
                    orderTotalInCustomerCurrency = CurrencyManager.ConvertCurrency(orderTotal.Value, CurrencyManager.PrimaryStoreCurrency, paymentInfo.CustomerCurrency);
                }
                else
                {
                    orderTotal = initialOrder.OrderTotal;
                    orderTotalInCustomerCurrency = initialOrder.OrderTotalInCustomerCurrency;
                }
                paymentInfo.OrderTotal = orderTotal.Value;

                string customerCurrencyCode = string.Empty;
                if (!paymentInfo.IsRecurringPayment)
                {
                    customerCurrencyCode = paymentInfo.CustomerCurrency.CurrencyCode;
                }
                else
                {
                    customerCurrencyCode = initialOrder.CustomerCurrencyCode;
                }

                //billing info
                string billingFirstName = string.Empty;
                string billingLastName = string.Empty;
                string billingPhoneNumber = string.Empty;
                string billingEmail = string.Empty;
                string billingFaxNumber = string.Empty;
                string billingCompany = string.Empty;
                string billingAddress1 = string.Empty;
                string billingAddress2 = string.Empty;
                string billingCity = string.Empty;
                string billingStateProvince = string.Empty;
                int billingStateProvinceID = 0;
                string billingZipPostalCode = string.Empty;
                string billingCountry = string.Empty;
                int billingCountryID = 0;
                if (!paymentInfo.IsRecurringPayment)
                {
                    var billingAddress = paymentInfo.BillingAddress;
                    billingFirstName = billingAddress.FirstName;
                    billingLastName = billingAddress.LastName;
                    billingPhoneNumber = billingAddress.PhoneNumber;
                    billingEmail = billingAddress.Email;
                    billingFaxNumber = billingAddress.FaxNumber;
                    billingCompany = billingAddress.Company;
                    billingAddress1 = billingAddress.Address1;
                    billingAddress2 = billingAddress.Address2;
                    billingCity = billingAddress.City;
                    if (billingAddress.StateProvince != null)
                    {
                        billingStateProvince = billingAddress.StateProvince.Name;
                        billingStateProvinceID = billingAddress.StateProvince.StateProvinceID;
                    }
                    billingZipPostalCode = billingAddress.ZipPostalCode;
                    if (billingAddress.Country != null)
                    {
                        billingCountry = billingAddress.Country.Name;
                        billingCountryID = billingAddress.Country.CountryID;

                        if (!billingAddress.Country.AllowsBilling)
                        {
                            throw new NopException(string.Format("{0} is not allowed for billing", billingCountry));
                        }
                    }
                }
                else
                {
                    billingFirstName = initialOrder.BillingFirstName;
                    billingLastName = initialOrder.BillingLastName;
                    billingPhoneNumber = initialOrder.BillingPhoneNumber;
                    billingEmail = initialOrder.BillingEmail;
                    billingFaxNumber = initialOrder.BillingFaxNumber;
                    billingCompany = initialOrder.BillingCompany;
                    billingAddress1 = initialOrder.BillingAddress1;
                    billingAddress2 = initialOrder.BillingAddress2;
                    billingCity = initialOrder.BillingCity;
                    billingStateProvince = initialOrder.BillingStateProvince;
                    billingStateProvinceID = initialOrder.BillingStateProvinceID;
                    billingZipPostalCode = initialOrder.BillingZipPostalCode;
                    billingCountry = initialOrder.BillingCountry;
                    billingCountryID = initialOrder.BillingCountryID;
                }

                //shipping info
                string shippingFirstName = string.Empty;
                string shippingLastName = string.Empty;
                string shippingPhoneNumber = string.Empty;
                string shippingEmail = string.Empty;
                string shippingFaxNumber = string.Empty;
                string shippingCompany = string.Empty;
                string shippingAddress1 = string.Empty;
                string shippingAddress2 = string.Empty;
                string shippingCity = string.Empty;
                string shippingStateProvince = string.Empty;
                int shippingStateProvinceID = 0;
                string shippingZipPostalCode = string.Empty;
                string shippingCountry = string.Empty;
                int shippingCountryID = 0;
                string shippingMethodName = string.Empty;
                int shippingRateComputationMethodID = 0;
                if (shoppingCartRequiresShipping)
                {
                    if (!paymentInfo.IsRecurringPayment)
                    {
                        var shippingAddress = paymentInfo.ShippingAddress;
                        if (shippingAddress != null)
                        {
                            shippingFirstName = shippingAddress.FirstName;
                            shippingLastName = shippingAddress.LastName;
                            shippingPhoneNumber = shippingAddress.PhoneNumber;
                            shippingEmail = shippingAddress.Email;
                            shippingFaxNumber = shippingAddress.FaxNumber;
                            shippingCompany = shippingAddress.Company;
                            shippingAddress1 = shippingAddress.Address1;
                            shippingAddress2 = shippingAddress.Address2;
                            shippingCity = shippingAddress.City;
                            if (shippingAddress.StateProvince != null)
                            {
                                shippingStateProvince = shippingAddress.StateProvince.Name;
                                shippingStateProvinceID = shippingAddress.StateProvince.StateProvinceID;
                            }
                            shippingZipPostalCode = shippingAddress.ZipPostalCode;
                            if (shippingAddress.Country != null)
                            {
                                shippingCountry = shippingAddress.Country.Name;
                                shippingCountryID = shippingAddress.Country.CountryID;

                                if (!shippingAddress.Country.AllowsShipping)
                                {
                                    throw new NopException(string.Format("{0} is not allowed for shipping", shippingCountry));
                                }
                            }
                            shippingMethodName = string.Empty;
                            var shippingOption = customer.LastShippingOption;
                            if (shippingOption != null)
                            {
                                shippingMethodName = shippingOption.Name;
                                shippingRateComputationMethodID = shippingOption.ShippingRateComputationMethodID;
                            }
                        }
                    }
                    else
                    {
                        shippingFirstName = initialOrder.ShippingFirstName;
                        shippingLastName = initialOrder.ShippingLastName;
                        shippingPhoneNumber = initialOrder.ShippingPhoneNumber;
                        shippingEmail = initialOrder.ShippingEmail;
                        shippingFaxNumber = initialOrder.ShippingFaxNumber;
                        shippingCompany = initialOrder.ShippingCompany;
                        shippingAddress1 = initialOrder.ShippingAddress1;
                        shippingAddress2 = initialOrder.ShippingAddress2;
                        shippingCity = initialOrder.ShippingCity;
                        shippingStateProvince = initialOrder.ShippingStateProvince;
                        shippingStateProvinceID = initialOrder.ShippingStateProvinceID;
                        shippingZipPostalCode = initialOrder.ShippingZipPostalCode;
                        shippingCountry = initialOrder.ShippingCountry;
                        shippingCountryID = initialOrder.ShippingCountryID;
                        shippingMethodName = initialOrder.ShippingMethod;
                        shippingRateComputationMethodID = initialOrder.ShippingRateComputationMethodID;
                    }
                }

                //customer language
                int customerLanguageID = 0;
                if (!paymentInfo.IsRecurringPayment)
                {
                    customerLanguageID = paymentInfo.CustomerLanguage.LanguageID;
                }
                else
                {
                    customerLanguageID = initialOrder.CustomerLanguageID;
                }

                //recurring or standard shopping cart
                bool isRecurringShoppingCart = false;
                int recurringCycleLength = 0;
                int recurringCyclePeriod = 0;
                int recurringTotalCycles = 0;
                if (!paymentInfo.IsRecurringPayment)
                {
                    isRecurringShoppingCart = cart.IsRecurring;
                    if (isRecurringShoppingCart)
                    {
                        string recurringCyclesError = ShoppingCartManager.GetReccuringCycleInfo(cart, out recurringCycleLength, out recurringCyclePeriod, out recurringTotalCycles);
                        if (!string.IsNullOrEmpty(recurringCyclesError))
                        {
                            throw new NopException(recurringCyclesError);
                        }
                        paymentInfo.RecurringCycleLength = recurringCycleLength;
                        paymentInfo.RecurringCyclePeriod = recurringCyclePeriod;
                        paymentInfo.RecurringTotalCycles = recurringTotalCycles;
                    }
                }
                else
                {
                    isRecurringShoppingCart = true;
                }
                
                //process payment
                if (!paymentInfo.IsRecurringPayment)
                {
                    if (isRecurringShoppingCart)
                    {
                        //recurring cart
                        var recurringPaymentType = PaymentManager.SupportRecurringPayments(paymentMethod.PaymentMethodID);
                        switch (recurringPaymentType)
                        {
                            case RecurringPaymentTypeEnum.NotSupported:
                                throw new NopException("Recurring payments are not supported by selected payment method");
                                break;
                            case RecurringPaymentTypeEnum.Manual:
                            case RecurringPaymentTypeEnum.Automatic:
                                PaymentManager.ProcessRecurringPayment(paymentInfo, customer, OrderGuid, ref processPaymentResult);
                                break;
                            default:
                                throw new NopException("Not supported recurring payment type");
                                break;
                        }
                    }
                    else
                    {
                        //standard cart
                        PaymentManager.ProcessPayment(paymentInfo, customer, OrderGuid, ref processPaymentResult);
                    }
                }
                else
                {
                    if (isRecurringShoppingCart)
                    {
                        var recurringPaymentType = PaymentManager.SupportRecurringPayments(paymentMethod.PaymentMethodID);
                        switch (recurringPaymentType)
                        {
                            case RecurringPaymentTypeEnum.NotSupported:
                                throw new NopException("Recurring payments are not supported by selected payment method");
                                break;
                            case RecurringPaymentTypeEnum.Manual:
                                PaymentManager.ProcessRecurringPayment(paymentInfo, customer, OrderGuid, ref processPaymentResult);
                                break;
                            case RecurringPaymentTypeEnum.Automatic:
                                //payment is processed on payment gateway site
                                break;
                            default:
                                throw new NopException("Not supported recurring payment type");
                                break;
                        }
                    }
                    else
                    {
                        throw new NopException("No recurring products");
                    }
                }

                //process order
                if (String.IsNullOrEmpty(processPaymentResult.Error))
                {
                    var shippingStatusEnum = ShippingStatusEnum.NotYetShipped;
                    if (!shoppingCartRequiresShipping)
                        shippingStatusEnum = ShippingStatusEnum.ShippingNotRequired;

                    //save order in data storage
                    //uncomment this line to support transactions
                    //using (var scope = new System.Transactions.TransactionScope())
                    {

                        var order = InsertOrder(OrderGuid,
                             customer.CustomerID,
                             customerLanguageID,
                             customerTaxDisplayType,
                             NopContext.Current.UserHostAddress,
                             orderSubTotalInclTax,
                             orderSubTotalExclTax,
                             orderShippingTotalInclTax.Value,
                             orderShippingTotalExclTax.Value,
                             paymentAdditionalFeeInclTax,
                             paymentAdditionalFeeExclTax,
                             orderTaxTotal,
                             orderTotal.Value,
                             orderSubTotalDiscountAmount,
                             orderSubtotalInclTaxInCustomerCurrency,
                             orderSubtotalExclTaxInCustomerCurrency,
                             orderShippingInclTaxInCustomerCurrency,
                             orderShippingExclTaxInCustomerCurrency,
                             paymentAdditionalFeeInclTaxInCustomerCurrency,
                             paymentAdditionalFeeExclTaxInCustomerCurrency,
                             orderTaxInCustomerCurrency,
                             orderTotalInCustomerCurrency,
                             orderSubtotalDiscountInCustomerCurrency,
                             customerCurrencyCode,
                             orderWeight,
                             customer.AffiliateID,
                             OrderStatusEnum.Pending,
                             processPaymentResult.AllowStoringCreditCardNumber,
                             processPaymentResult.AllowStoringCreditCardNumber ? SecurityHelper.Encrypt(paymentInfo.CreditCardType) : string.Empty,
                             processPaymentResult.AllowStoringCreditCardNumber ? SecurityHelper.Encrypt(paymentInfo.CreditCardName) : string.Empty,
                             processPaymentResult.AllowStoringCreditCardNumber ? SecurityHelper.Encrypt(paymentInfo.CreditCardNumber) : string.Empty,
                             SecurityHelper.Encrypt(PaymentManager.GetMaskedCreditCardNumber(paymentInfo.CreditCardNumber)),
                             processPaymentResult.AllowStoringCreditCardNumber ? SecurityHelper.Encrypt(paymentInfo.CreditCardCVV2) : string.Empty,
                             processPaymentResult.AllowStoringCreditCardNumber ? SecurityHelper.Encrypt(paymentInfo.CreditCardExpireMonth.ToString()) : string.Empty,
                             processPaymentResult.AllowStoringCreditCardNumber ? SecurityHelper.Encrypt(paymentInfo.CreditCardExpireYear.ToString()) : string.Empty,
                             paymentMethod.PaymentMethodID,
                             paymentMethod.Name,
                             processPaymentResult.AuthorizationTransactionID,
                             processPaymentResult.AuthorizationTransactionCode,
                             processPaymentResult.AuthorizationTransactionResult,
                             processPaymentResult.CaptureTransactionID,
                             processPaymentResult.CaptureTransactionResult,
                             processPaymentResult.SubscriptionTransactionID,
                             paymentInfo.PurchaseOrderNumber,
                             processPaymentResult.PaymentStatus,
                             null,
                             billingFirstName,
                             billingLastName,
                             billingPhoneNumber,
                             billingEmail,
                             billingFaxNumber,
                             billingCompany,
                             billingAddress1,
                             billingAddress2,
                             billingCity,
                             billingStateProvince,
                             billingStateProvinceID,
                             billingZipPostalCode,
                             billingCountry,
                             billingCountryID,
                             shippingStatusEnum,
                             shippingFirstName,
                             shippingLastName,
                             shippingPhoneNumber,
                             shippingEmail,
                             shippingFaxNumber,
                             shippingCompany,
                             shippingAddress1,
                             shippingAddress2,
                             shippingCity,
                             shippingStateProvince,
                             shippingStateProvinceID,
                             shippingZipPostalCode,
                             shippingCountry,
                             shippingCountryID,
                             shippingMethodName,
                             shippingRateComputationMethodID,
                             null,
                             null,
                             false,
                             DateTime.Now);

                        OrderID = order.OrderID;

                        if (!paymentInfo.IsRecurringPayment)
                        {
                            //move shopping cart items to order product variants
                            foreach (var sc in cart)
                            {
                                //prices
                                decimal scUnitPrice = PriceHelper.GetUnitPrice(sc, customer, true);
                                decimal scSubTotal = PriceHelper.GetSubTotal(sc, customer, true);
                                decimal scUnitPriceInclTax = TaxManager.GetPrice(sc.ProductVariant, scUnitPrice, true, customer);
                                decimal scUnitPriceExclTax = TaxManager.GetPrice(sc.ProductVariant, scUnitPrice, false, customer);
                                decimal scSubTotalInclTax = TaxManager.GetPrice(sc.ProductVariant, scSubTotal, true, customer);
                                decimal scSubTotalExclTax = TaxManager.GetPrice(sc.ProductVariant, scSubTotal, false, customer);
                                decimal scUnitPriceInclTaxInCustomerCurrency = CurrencyManager.ConvertCurrency(scUnitPriceInclTax, CurrencyManager.PrimaryStoreCurrency, paymentInfo.CustomerCurrency);
                                decimal scUnitPriceExclTaxInCustomerCurrency = CurrencyManager.ConvertCurrency(scUnitPriceExclTax, CurrencyManager.PrimaryStoreCurrency, paymentInfo.CustomerCurrency);
                                decimal scSubTotalInclTaxInCustomerCurrency = CurrencyManager.ConvertCurrency(scSubTotalInclTax, CurrencyManager.PrimaryStoreCurrency, paymentInfo.CustomerCurrency);
                                decimal scSubTotalExclTaxInCustomerCurrency = CurrencyManager.ConvertCurrency(scSubTotalExclTax, CurrencyManager.PrimaryStoreCurrency, paymentInfo.CustomerCurrency);

                                //discounts
                                Discount scDiscount = null;
                                decimal discountAmount = PriceHelper.GetDiscountAmount(sc, customer, out scDiscount);
                                decimal discountAmountInclTax = TaxManager.GetPrice(sc.ProductVariant, discountAmount, true, customer);
                                decimal discountAmountExclTax = TaxManager.GetPrice(sc.ProductVariant, discountAmount, false, customer);
                                if (scDiscount != null && !appliedDiscounts.ContainsDiscount(scDiscount.Name))
                                    appliedDiscounts.Add(scDiscount);

                                //attributes
                                string attributeDescription = ProductAttributeHelper.FormatAttributes(sc.ProductVariant, sc.AttributesXML);

                                //save item
                                var opv = InsertOrderProductVariant(Guid.NewGuid(), order.OrderID,
                                    sc.ProductVariantID, scUnitPriceInclTax, scUnitPriceExclTax, scSubTotalInclTax, scSubTotalExclTax,
                                    scUnitPriceInclTaxInCustomerCurrency, scUnitPriceExclTaxInCustomerCurrency,
                                    scSubTotalInclTaxInCustomerCurrency, scSubTotalExclTaxInCustomerCurrency,
                                    attributeDescription, sc.AttributesXML, sc.Quantity, discountAmountInclTax,
                                    discountAmountExclTax, 0, false, 0);

                                //gift cards
                                if (sc.ProductVariant.IsGiftCard)
                                {
                                    string giftCardRecipientName = string.Empty;
                                    string giftCardRecipientEmail = string.Empty;
                                    string giftCardSenderName = string.Empty;
                                    string giftCardSenderEmail = string.Empty;
                                    string giftCardMessage = string.Empty;
                                    ProductAttributeHelper.GetGiftCardAttribute(sc.AttributesXML,
                                        out giftCardRecipientName, out giftCardRecipientEmail,
                                        out giftCardSenderName, out giftCardSenderEmail, out giftCardMessage);

                                    for (int i = 0; i < sc.Quantity; i++)
                                    {
                                        var gc = InsertGiftCard(opv.OrderProductVariantID, scUnitPriceExclTax,
                                            false, GiftCardHelper.GenerateGiftCardCode(),
                                            giftCardRecipientName, giftCardRecipientEmail,
                                           giftCardSenderName, giftCardSenderEmail,
                                           giftCardMessage, false, DateTime.Now);
                                    }
                                }

                                ShoppingCartManager.DeleteShoppingCartItem(sc.ShoppingCartItemID, false);

                                //inventory
                                ProductManager.AdjustInventory(sc.ProductVariantID, true, sc.Quantity, sc.AttributesXML);
                            }
                        }
                        else
                        {
                            var initialOrderProductVariants = initialOrder.OrderProductVariants;
                            foreach (var opv in initialOrderProductVariants)
                            {
                                InsertOrderProductVariant(Guid.NewGuid(), order.OrderID,
                                    opv.ProductVariantID, opv.UnitPriceInclTax, opv.UnitPriceExclTax,
                                    opv.PriceInclTax, opv.PriceExclTax,
                                    opv.UnitPriceInclTaxInCustomerCurrency, opv.UnitPriceExclTaxInCustomerCurrency,
                                    opv.PriceInclTaxInCustomerCurrency, opv.PriceExclTaxInCustomerCurrency,
                                    opv.AttributeDescription, opv.AttributesXML, opv.Quantity, opv.DiscountAmountInclTax,
                                    opv.DiscountAmountExclTax, 0, false, 0);

                                //UNDONE gift cards are not supported in recurring products
                                //if (opv.ProductVariant.IsGiftCard)
                                //{
                                //    for (int i = 0; i < opv.Quantity; i++)
                                //    {
                                //        GiftCard gc = InsertGiftCard(opv.OrderProductVariantID, opv.UnitPriceExclTax,
                                //            false, GiftCardHelper.GenerateGiftCardCode(), string.Empty, string.Empty,
                                //            string.Empty, string.Empty, string.Empty, false, DateTime.Now);
                                //    }
                                //}

                                //inventory
                                ProductManager.AdjustInventory(opv.ProductVariantID, true, opv.Quantity, opv.AttributesXML);
                            }
                        }

                        //discount usage history
                        if (!paymentInfo.IsRecurringPayment)
                        {
                            foreach (var discount in appliedDiscounts)
                            {
                                var duh = DiscountManager.InsertDiscountUsageHistory(discount.DiscountID,
                                    customer.CustomerID, order.OrderID, DateTime.Now);
                            }
                        }

                        //gift card usage history
                        if (!paymentInfo.IsRecurringPayment)
                        {
                            if (appliedGiftCards != null)
                            {
                                foreach (var agc in appliedGiftCards)
                                {
                                    decimal amountUsed = agc.AmountCanBeUsed;
                                    decimal amountUsedInCustomerCurrency = CurrencyManager.ConvertCurrency(amountUsed, CurrencyManager.PrimaryStoreCurrency, paymentInfo.CustomerCurrency);
                                    var gcuh = InsertGiftCardUsageHistory(agc.GiftCardID,
                                        customer.CustomerID, order.OrderID,
                                        amountUsed, amountUsedInCustomerCurrency, DateTime.Now);
                                }
                            }
                        }

                        //recurring orders
                        if (!paymentInfo.IsRecurringPayment)
                        {
                            if (isRecurringShoppingCart)
                            {
                                //create recurring payment
                                var rp = InsertRecurringPayment(order.OrderID,
                                    paymentInfo.RecurringCycleLength, paymentInfo.RecurringCyclePeriod,
                                    paymentInfo.RecurringTotalCycles, DateTime.Now,
                                    true, false, DateTime.Now);


                                var recurringPaymentType = PaymentManager.SupportRecurringPayments(paymentMethod.PaymentMethodID);
                                switch (recurringPaymentType)
                                {
                                    case RecurringPaymentTypeEnum.NotSupported:
                                        {
                                            //not supported
                                        }
                                        break;
                                    case RecurringPaymentTypeEnum.Manual:
                                        {
                                            //first payment
                                            RecurringPaymentHistory rph = InsertRecurringPaymentHistory(rp.RecurringPaymentID,
                                                order.OrderID, DateTime.Now);
                                        }
                                        break;
                                    case RecurringPaymentTypeEnum.Automatic:
                                        {
                                            //will be created later (process is automated)
                                        }
                                        break;
                                    default:
                                        break;
                                }
                            }
                        }


                        //notes, messages
                        InsertOrderNote(order.OrderID, string.Format("Order placed"), false, DateTime.Now);

                        int orderPlacedStoreOwnerNotificationQueuedEmailID = MessageManager.SendOrderPlacedStoreOwnerNotification(order, LocalizationManager.DefaultAdminLanguage.LanguageID);
                        InsertOrderNote(order.OrderID, string.Format("\"Order placed\" email (to store owner) has been queued. Queued email identifier: {0}.", orderPlacedStoreOwnerNotificationQueuedEmailID), false, DateTime.Now);

                        int orderPlacedCustomerNotificationQueuedEmailID = MessageManager.SendOrderPlacedCustomerNotification(order, order.CustomerLanguageID);
                        InsertOrderNote(order.OrderID, string.Format("\"Order placed\" email (to customer) has been queued. Queued email identifier: {0}.", orderPlacedCustomerNotificationQueuedEmailID), false, DateTime.Now);

                        if (SMSManager.IsSMSAlertsEnabled && SMSManager.SendOrderPlacedNotification(order))
                        {
                            InsertOrderNote(order.OrderID, "\"Order placed\" SMS alert (to store owner) has been sent", false, DateTime.Now);
                        }

                        //order status
                        order = CheckOrderStatus(order.OrderID);

                        //reset checkout data
                        if (!paymentInfo.IsRecurringPayment)
                        {
                            CustomerManager.ResetCheckoutData(customer.CustomerID, true);
                        }

                        //log
                        if (!paymentInfo.IsRecurringPayment)
                        {
                            CustomerActivityManager.InsertActivity(
                                "PlaceOrder",
                                LocalizationManager.GetLocaleResourceString("ActivityLog.PlaceOrder"),
                                order.OrderID);
                        }

                        //uncomment this line to support transactions
                        //scope.Complete();
                    }
                }
            }
            catch (Exception exc)
            {
                processPaymentResult.Error = exc.Message;
                processPaymentResult.FullError = exc.ToString();
            }

            if (!String.IsNullOrEmpty(processPaymentResult.Error))
            {
                LogManager.InsertLog(LogTypeEnum.OrderError, string.Format("Error while placing order. {0}", processPaymentResult.Error), processPaymentResult.FullError);
            }
            return processPaymentResult.Error;
        }

        /// <summary>
        /// Place order items in current user shopping cart.
        /// </summary>
        /// <param name="OrderID">The order identifier</param>
        public static void ReOrder(int OrderID)
        {
            var order = GetOrderByID(OrderID);
            if(order != null)
            {
                foreach (var orderProductVariant in order.OrderProductVariants)
                {
                    ShoppingCartManager.AddToCart(ShoppingCartTypeEnum.ShoppingCart, orderProductVariant.ProductVariantID, orderProductVariant.AttributesXML, orderProductVariant.Quantity);
                }
            }
        }

        /// <summary>
        /// Process next recurring psayment
        /// </summary>
        /// <param name="RecurringPaymentID">Recurring payment identifier</param>
        public static void ProcessNextRecurringPayment(int RecurringPaymentID)
        {
            try
            {
                var rp = GetRecurringPaymentByID(RecurringPaymentID);
                if (rp == null)
                    throw new NopException("Recurring payment could not be loaded");

                if (!rp.IsActive)
                    throw new NopException("Recurring payment is not active");

                var initialOrder = rp.InitialOrder;
                if (initialOrder == null)
                    throw new NopException("Initial order could not be loaded");

                var customer = initialOrder.Customer;
                if (customer == null)
                    throw new NopException("Customer could not be loaded");

                var nextPaymentDate = rp.NextPaymentDate;
                if (!nextPaymentDate.HasValue)
                    throw new NopException("Next payment date could not be calculated");

                //payment info
                var paymentInfo = new PaymentInfo();
                paymentInfo.IsRecurringPayment = true;
                paymentInfo.InitialOrderID = initialOrder.OrderID;
                paymentInfo.RecurringCycleLength = rp.CycleLength;
                paymentInfo.RecurringCyclePeriod = rp.CyclePeriod;
                paymentInfo.RecurringTotalCycles = rp.TotalCycles;

                //place new order
                int newOrderID = 0;
                string result = OrderManager.PlaceOrder(paymentInfo, customer,
                    Guid.NewGuid(), out newOrderID);
                if (!String.IsNullOrEmpty(result))
                {
                    throw new NopException(result);
                }
                else
                {
                    InsertRecurringPaymentHistory(rp.RecurringPaymentID, newOrderID, DateTime.Now);
                }
            }
            catch (Exception exc)
            {
                LogManager.InsertLog(LogTypeEnum.OrderError, string.Format("Error while processing recurring order. {0}", exc.Message), exc);
                throw;
            }
        }

        /// <summary>
        /// Cancels a recurring payment
        /// </summary>
        /// <param name="RecurringPaymentID">Recurring payment identifier</param>
        public static RecurringPayment CancelRecurringPayment(int RecurringPaymentID)
        {
            return CancelRecurringPayment(RecurringPaymentID, true);
        }

        /// <summary>
        /// Cancels a recurring payment
        /// </summary>
        /// <param name="RecurringPaymentID">Recurring payment identifier</param>
        /// <param name="throwException">A value indicating whether to throw the exception after an error has occupied.</param>
        public static RecurringPayment CancelRecurringPayment(int RecurringPaymentID, bool throwException)
        {
            var recurringPayment = GetRecurringPaymentByID(RecurringPaymentID);
            try
            {
                if (recurringPayment != null)
                {
                    //update recurring payment
                    UpdateRecurringPayment(recurringPayment.RecurringPaymentID, recurringPayment.InitialOrderID,
                        recurringPayment.CycleLength, recurringPayment.CyclePeriod,
                        recurringPayment.TotalCycles, recurringPayment.StartDate,
                        false, recurringPayment.Deleted, recurringPayment.CreatedOn);

                    var initialOrder = recurringPayment.InitialOrder;
                    if (initialOrder == null)
                        return recurringPayment;

                    //old info from placing order
                    var cancelPaymentResult = new CancelPaymentResult();                    
                    cancelPaymentResult.AuthorizationTransactionID = initialOrder.AuthorizationTransactionID;
                    cancelPaymentResult.CaptureTransactionID = initialOrder.CaptureTransactionID;
                    cancelPaymentResult.SubscriptionTransactionID = initialOrder.SubscriptionTransactionID;
                    cancelPaymentResult.Amount = initialOrder.OrderTotal;
                    PaymentManager.CancelRecurringPayment(initialOrder, ref cancelPaymentResult);
                    if (String.IsNullOrEmpty(cancelPaymentResult.Error))
                    {
                        InsertOrderNote(initialOrder.OrderID, string.Format("Recurring payment has been cancelled"), false, DateTime.Now);
                    }
                    else
                    {
                        InsertOrderNote(initialOrder.OrderID, string.Format("Error cancelling recurring payment. Error: {0}", cancelPaymentResult.Error), false, DateTime.Now);
                    }
                }
            }
            catch (Exception exc)
            {
                LogManager.InsertLog(LogTypeEnum.OrderError, "Error cancelling recurring payment", exc);
                if (throwException)
                    throw;
            }
            return recurringPayment;
        }

        /// <summary>
        /// Gets a value indicating whether a customer can cancel recurring payment
        /// </summary>
        /// <param name="customerToValidate">Customer</param>
        /// <param name="recurringPayment">Recurring Payment</param>
        /// <returns>value indicating whether a customer can cancel recurring payment</returns>
        public static bool CanCancelRecurringPayment(Customer customerToValidate, RecurringPayment recurringPayment)
        {
            if (recurringPayment == null)
                return false;

            if (customerToValidate == null)
                return false;

            var initialOrder = recurringPayment.InitialOrder;
            if (initialOrder == null)
                return false;

            var customer = recurringPayment.Customer;
            if (customer == null)
                return false;

            if (initialOrder.OrderStatus == OrderStatusEnum.Cancelled)
                return false;

            if (!customerToValidate.IsAdmin)
            {
                if (customer.CustomerID != customerToValidate.CustomerID)
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
        public static bool CanShip(Order order)
        {
            if (order == null)
                return false;

            if (order.OrderStatus == OrderStatusEnum.Cancelled)
                return false;

            if (order.ShippingStatus == ShippingStatusEnum.NotYetShipped)
                return true;

            return false;
        }

        /// <summary>
        /// Ships order
        /// </summary>
        /// <param name="OrderID">Order identifier</param>
        /// <param name="notifyCustomer">True to notify customer</param>
        /// <returns>Updated order</returns>
        public static Order Ship(int OrderID, bool notifyCustomer)
        {
            var order = GetOrderByID(OrderID);
            if (order == null)
                return order;

            if (!CanShip(order))
                throw new NopException("Can not do shipment for order.");

            var ShippedDate = DateTimeHelper.ConvertToUtcTime(DateTime.Now);
            order = UpdateOrder(order.OrderID, order.OrderGUID, order.CustomerID, order.CustomerLanguageID,
                order.CustomerTaxDisplayType, order.CustomerIP, order.OrderSubtotalInclTax, order.OrderSubtotalExclTax, order.OrderShippingInclTax,
                order.OrderShippingExclTax, order.PaymentMethodAdditionalFeeInclTax, order.PaymentMethodAdditionalFeeExclTax,
                order.OrderTax, order.OrderTotal, order.OrderDiscount,
                order.OrderSubtotalInclTaxInCustomerCurrency, order.OrderSubtotalExclTaxInCustomerCurrency,
                order.OrderShippingInclTaxInCustomerCurrency, order.OrderShippingExclTaxInCustomerCurrency,
                order.PaymentMethodAdditionalFeeInclTaxInCustomerCurrency, order.PaymentMethodAdditionalFeeExclTaxInCustomerCurrency,
                order.OrderTaxInCustomerCurrency, order.OrderTotalInCustomerCurrency,
                order.OrderDiscountInCustomerCurrency, order.CustomerCurrencyCode, order.OrderWeight,
                order.AffiliateID, order.OrderStatus, order.AllowStoringCreditCardNumber, order.CardType,
                order.CardName, order.CardNumber, order.MaskedCreditCardNumber,
                order.CardCVV2, order.CardExpirationMonth, order.CardExpirationYear,
                order.PaymentMethodID, order.PaymentMethodName,
                order.AuthorizationTransactionID,
                order.AuthorizationTransactionCode, order.AuthorizationTransactionResult,
                order.CaptureTransactionID, order.CaptureTransactionResult,
                order.SubscriptionTransactionID, order.PurchaseOrderNumber, order.PaymentStatus, order.PaidDate,
                order.BillingFirstName, order.BillingLastName, order.BillingPhoneNumber,
                order.BillingEmail, order.BillingFaxNumber, order.BillingCompany, order.BillingAddress1,
                order.BillingAddress2, order.BillingCity,
                order.BillingStateProvince, order.BillingStateProvinceID, order.BillingZipPostalCode,
                order.BillingCountry, order.BillingCountryID, ShippingStatusEnum.Shipped,
                order.ShippingFirstName, order.ShippingLastName, order.ShippingPhoneNumber,
                order.ShippingEmail, order.ShippingFaxNumber, order.ShippingCompany,
                order.ShippingAddress1, order.ShippingAddress2, order.ShippingCity,
                order.ShippingStateProvince, order.ShippingStateProvinceID, order.ShippingZipPostalCode,
                order.ShippingCountry, order.ShippingCountryID,
                order.ShippingMethod, order.ShippingRateComputationMethodID, ShippedDate,
                order.TrackingNumber, order.Deleted, order.CreatedOn);

            InsertOrderNote(order.OrderID, string.Format("Order has been shipped"), false, DateTime.Now);

            if (notifyCustomer)
            {
                int orderShippedCustomerNotificationQueuedEmailID = MessageManager.SendOrderShippedCustomerNotification(order, order.CustomerLanguageID);
                InsertOrderNote(order.OrderID, string.Format("\"Shipped\" email (to customer) has been queued. Queued email identifier: {0}.", orderShippedCustomerNotificationQueuedEmailID), false, DateTime.Now);
            }

            order = CheckOrderStatus(order.OrderID);

            return order;
        }

        /// <summary>
        /// Gets a value indicating whether cancel is allowed
        /// </summary>
        /// <param name="order">Order</param>
        /// <returns>A value indicating whether cancel is allowed</returns>
        public static bool CanCancelOrder(Order order)
        {
            if (order == null)
                return false;

            if (order.OrderStatus == OrderStatusEnum.Cancelled)
                return false;

            return true;
        }

        /// <summary>
        /// Cancels order
        /// </summary>
        /// <param name="OrderID">Order identifier</param>
        /// <param name="notifyCustomer">True to notify customer</param>
        /// <returns>Cancelled order</returns>
        public static Order CancelOrder(int OrderID, bool notifyCustomer)
        {
            var order = GetOrderByID(OrderID);
            if (order == null)
                return order;

            if (!CanCancelOrder(order))
                throw new NopException("Can not do cancel for order.");
            
            //Cancel order
            order = SetOrderStatus(order.OrderID, OrderStatusEnum.Cancelled, notifyCustomer);

            InsertOrderNote(order.OrderID, string.Format("Order has been cancelled"), false, DateTime.Now);
            
            //cancel recurring payments
            var recurringPayments = SearchRecurringPayments(0, order.OrderID, null);
            foreach (var rp in recurringPayments)
            {
                CancelRecurringPayment(rp.RecurringPaymentID, false);
            }
                
            //Adjust inventory
            foreach (var opv in order.OrderProductVariants)
                ProductManager.AdjustInventory(opv.ProductVariantID, false, opv.Quantity, opv.AttributesXML);

            return order;
        }

        /// <summary>
        /// Gets a value indicating whether order can be marked as authorized
        /// </summary>
        /// <param name="order">Order</param>
        /// <returns>A value indicating whether order can be marked as authorized</returns>
        public static bool CanMarkOrderAsAuthorized(Order order)
        {
            if (order == null)
                return false;

            if (order.OrderStatus == OrderStatusEnum.Cancelled)
                return false;

            if (order.PaymentStatus == PaymentStatusEnum.Pending)
                return true;

            return false;
        }

        /// <summary>
        /// Marks order as authorized
        /// </summary>
        /// <param name="OrderID">Order identifier</param>
        /// <returns>Authorized order</returns>
        public static Order MarkAsAuthorized(int OrderID)
        {
            var order = GetOrderByID(OrderID);
            if (order == null)
                return order;

            order = UpdateOrder(order.OrderID, order.OrderGUID, order.CustomerID, order.CustomerLanguageID,
                order.CustomerTaxDisplayType, order.CustomerIP, order.OrderSubtotalInclTax, order.OrderSubtotalExclTax, order.OrderShippingInclTax,
                   order.OrderShippingExclTax, order.PaymentMethodAdditionalFeeInclTax, order.PaymentMethodAdditionalFeeExclTax,
                   order.OrderTax, order.OrderTotal, order.OrderDiscount,
                   order.OrderSubtotalInclTaxInCustomerCurrency, order.OrderSubtotalExclTaxInCustomerCurrency,
                   order.OrderShippingInclTaxInCustomerCurrency, order.OrderShippingExclTaxInCustomerCurrency,
                   order.PaymentMethodAdditionalFeeInclTaxInCustomerCurrency, order.PaymentMethodAdditionalFeeExclTaxInCustomerCurrency,
                   order.OrderTaxInCustomerCurrency, order.OrderTotalInCustomerCurrency,
                   order.OrderDiscountInCustomerCurrency, order.CustomerCurrencyCode, order.OrderWeight,
                   order.AffiliateID, order.OrderStatus, order.AllowStoringCreditCardNumber,
                   order.CardType, order.CardName, order.CardNumber, order.MaskedCreditCardNumber,
                   order.CardCVV2, order.CardExpirationMonth, order.CardExpirationYear,
                   order.PaymentMethodID, order.PaymentMethodName,
                   order.AuthorizationTransactionID,
                   order.AuthorizationTransactionCode, order.AuthorizationTransactionResult,
                   order.CaptureTransactionID, order.CaptureTransactionResult,
                   order.SubscriptionTransactionID, order.PurchaseOrderNumber, 
                   PaymentStatusEnum.Authorized, order.PaidDate,
                   order.BillingFirstName, order.BillingLastName, order.BillingPhoneNumber,
                   order.BillingEmail, order.BillingFaxNumber, order.BillingCompany, order.BillingAddress1,
                   order.BillingAddress2, order.BillingCity,
                   order.BillingStateProvince, order.BillingStateProvinceID, order.BillingZipPostalCode,
                   order.BillingCountry, order.BillingCountryID, order.ShippingStatus,
                   order.ShippingFirstName, order.ShippingLastName, order.ShippingPhoneNumber,
                   order.ShippingEmail, order.ShippingFaxNumber, order.ShippingCompany,
                   order.ShippingAddress1, order.ShippingAddress2, order.ShippingCity,
                   order.ShippingStateProvince, order.ShippingStateProvinceID, order.ShippingZipPostalCode,
                   order.ShippingCountry, order.ShippingCountryID,
                   order.ShippingMethod, order.ShippingRateComputationMethodID,
                   order.ShippedDate, order.TrackingNumber, order.Deleted, order.CreatedOn);

            InsertOrderNote(order.OrderID, string.Format("Order has been marked as authorized"), false, DateTime.Now);

            order = CheckOrderStatus(order.OrderID);

            return order;
        }

        /// <summary>
        /// Gets a value indicating whether capture from admin panel is allowed
        /// </summary>
        /// <param name="order">Order</param>
        /// <returns>A value indicating whether capture from admin panel is allowed</returns>
        public static bool CanCapture(Order order)
        {
            if (order == null)
                return false;

            if (order.OrderStatus == OrderStatusEnum.Cancelled ||
                order.OrderStatus == OrderStatusEnum.Pending)
                return false;

            if (order.PaymentStatus == PaymentStatusEnum.Authorized &&
                PaymentManager.CanCapture(order.PaymentMethodID))
                return true;

            return false;
        }

        /// <summary>
        /// Captures order (from admin panel)
        /// </summary>
        /// <param name="OrderID">Order identifier</param>
        /// <param name="Error">Error</param>
        /// <returns>Captured order</returns>
        public static Order Capture(int OrderID, ref string Error)
        {
            var order = GetOrderByID(OrderID);
            if (order == null)
                return order;

            if (!CanCapture(order))
                throw new NopException("Can not do capture for order.");

            var processPaymentResult = new ProcessPaymentResult();
            try
            {
                //old info from placing order
                processPaymentResult.AuthorizationTransactionID = order.AuthorizationTransactionID;
                processPaymentResult.AuthorizationTransactionCode = order.AuthorizationTransactionCode;
                processPaymentResult.AuthorizationTransactionResult = order.AuthorizationTransactionResult;
                processPaymentResult.CaptureTransactionID = order.CaptureTransactionID;
                processPaymentResult.CaptureTransactionResult = order.CaptureTransactionResult;
                processPaymentResult.SubscriptionTransactionID = order.SubscriptionTransactionID;
                processPaymentResult.PaymentStatus = order.PaymentStatus;

                PaymentManager.Capture(order, ref processPaymentResult);

                if (String.IsNullOrEmpty(processPaymentResult.Error))
                {
                    var paidDate = order.PaidDate;
                    var paymentStatus = processPaymentResult.PaymentStatus;
                    if (paymentStatus == PaymentStatusEnum.Paid)
                        paidDate = DateTime.Now;
                    order = UpdateOrder(order.OrderID, order.OrderGUID, order.CustomerID, order.CustomerLanguageID,
                        order.CustomerTaxDisplayType, order.CustomerIP, order.OrderSubtotalInclTax, order.OrderSubtotalExclTax, order.OrderShippingInclTax,
                        order.OrderShippingExclTax, order.PaymentMethodAdditionalFeeInclTax, order.PaymentMethodAdditionalFeeExclTax,
                        order.OrderTax, order.OrderTotal, order.OrderDiscount,
                        order.OrderSubtotalInclTaxInCustomerCurrency, order.OrderSubtotalExclTaxInCustomerCurrency,
                        order.OrderShippingInclTaxInCustomerCurrency, order.OrderShippingExclTaxInCustomerCurrency,
                        order.PaymentMethodAdditionalFeeInclTaxInCustomerCurrency, order.PaymentMethodAdditionalFeeExclTaxInCustomerCurrency,
                        order.OrderTaxInCustomerCurrency, order.OrderTotalInCustomerCurrency,
                        order.OrderDiscountInCustomerCurrency, order.CustomerCurrencyCode, order.OrderWeight,
                        order.AffiliateID, order.OrderStatus, order.AllowStoringCreditCardNumber,
                        order.CardType, order.CardName, order.CardNumber, order.MaskedCreditCardNumber,
                        order.CardCVV2, order.CardExpirationMonth, order.CardExpirationYear,
                        order.PaymentMethodID, order.PaymentMethodName,
                        processPaymentResult.AuthorizationTransactionID,
                        processPaymentResult.AuthorizationTransactionCode,
                        processPaymentResult.AuthorizationTransactionResult,
                        processPaymentResult.CaptureTransactionID,
                        processPaymentResult.CaptureTransactionResult,
                        processPaymentResult.SubscriptionTransactionID,
                        order.PurchaseOrderNumber, paymentStatus, paidDate,
                        order.BillingFirstName, order.BillingLastName, order.BillingPhoneNumber,
                        order.BillingEmail, order.BillingFaxNumber, order.BillingCompany, order.BillingAddress1,
                        order.BillingAddress2, order.BillingCity,
                        order.BillingStateProvince, order.BillingStateProvinceID, order.BillingZipPostalCode,
                        order.BillingCountry, order.BillingCountryID, order.ShippingStatus,
                        order.ShippingFirstName, order.ShippingLastName, order.ShippingPhoneNumber,
                        order.ShippingEmail, order.ShippingFaxNumber, order.ShippingCompany,
                        order.ShippingAddress1, order.ShippingAddress2, order.ShippingCity,
                        order.ShippingStateProvince, order.ShippingStateProvinceID, order.ShippingZipPostalCode,
                        order.ShippingCountry, order.ShippingCountryID,
                        order.ShippingMethod, order.ShippingRateComputationMethodID, order.ShippedDate,
                        order.TrackingNumber, order.Deleted, order.CreatedOn);

                    InsertOrderNote(order.OrderID, string.Format("Order has been captured"), false, DateTime.Now);

                }
                else
                {
                    InsertOrderNote(order.OrderID, string.Format("Unable to capture order. Error: {0}", processPaymentResult.Error), false, DateTime.Now);

                }
                order = CheckOrderStatus(order.OrderID);
            }
            catch (Exception exc)
            {
                processPaymentResult.Error = exc.Message;
                processPaymentResult.FullError = exc.ToString();
            }

            if (!String.IsNullOrEmpty(processPaymentResult.Error))
            {
                Error = processPaymentResult.Error;
                LogManager.InsertLog(LogTypeEnum.OrderError, string.Format("Error capturing order. {0}", processPaymentResult.Error), processPaymentResult.FullError);
            }
            return order;
        }

        /// <summary>
        /// Gets a value indicating whether order can be marked as paid
        /// </summary>
        /// <param name="order">Order</param>
        /// <returns>A value indicating whether order can be marked as paid</returns>
        public static bool CanMarkOrderAsPaid(Order order)
        {
            if (order == null)
                return false;

            if (order.OrderStatus == OrderStatusEnum.Cancelled)
                return false;

            if (order.PaymentStatus == PaymentStatusEnum.Paid || 
                order.PaymentStatus == PaymentStatusEnum.Refunded || 
                order.PaymentStatus == PaymentStatusEnum.Voided)
                return false;

            return true;
        }

        /// <summary>
        /// Marks order as paid
        /// </summary>
        /// <param name="OrderID">Order identifier</param>
        /// <returns>Updated order</returns>
        public static Order MarkOrderAsPaid(int OrderID)
        {
            var order = GetOrderByID(OrderID);
            if (order == null)
                return order;

            if (!CanMarkOrderAsPaid(order))
                throw new NopException("You can't mark this order as paid");

            order = UpdateOrder(order.OrderID, order.OrderGUID, order.CustomerID, order.CustomerLanguageID,
                    order.CustomerTaxDisplayType, order.CustomerIP, order.OrderSubtotalInclTax, order.OrderSubtotalExclTax, order.OrderShippingInclTax,
                    order.OrderShippingExclTax, order.PaymentMethodAdditionalFeeInclTax, order.PaymentMethodAdditionalFeeExclTax,
                    order.OrderTax, order.OrderTotal, order.OrderDiscount,
                    order.OrderSubtotalInclTaxInCustomerCurrency, order.OrderSubtotalExclTaxInCustomerCurrency,
                    order.OrderShippingInclTaxInCustomerCurrency, order.OrderShippingExclTaxInCustomerCurrency,
                    order.PaymentMethodAdditionalFeeInclTaxInCustomerCurrency, order.PaymentMethodAdditionalFeeExclTaxInCustomerCurrency,
                    order.OrderTaxInCustomerCurrency, order.OrderTotalInCustomerCurrency,
                    order.OrderDiscountInCustomerCurrency, order.CustomerCurrencyCode, order.OrderWeight,
                    order.AffiliateID, order.OrderStatus, order.AllowStoringCreditCardNumber, order.CardType,
                    order.CardName, order.CardNumber, order.MaskedCreditCardNumber,
                    order.CardCVV2, order.CardExpirationMonth, order.CardExpirationYear,
                    order.PaymentMethodID, order.PaymentMethodName,
                    order.AuthorizationTransactionID,
                    order.AuthorizationTransactionCode, order.AuthorizationTransactionResult,
                    order.CaptureTransactionID, order.CaptureTransactionResult,
                    order.SubscriptionTransactionID, order.PurchaseOrderNumber, PaymentStatusEnum.Paid, DateTime.Now,
                    order.BillingFirstName, order.BillingLastName, order.BillingPhoneNumber,
                    order.BillingEmail, order.BillingFaxNumber, order.BillingCompany, order.BillingAddress1,
                    order.BillingAddress2, order.BillingCity,
                    order.BillingStateProvince, order.BillingStateProvinceID, order.BillingZipPostalCode,
                    order.BillingCountry, order.BillingCountryID, order.ShippingStatus,
                    order.ShippingFirstName, order.ShippingLastName, order.ShippingPhoneNumber,
                    order.ShippingEmail, order.ShippingFaxNumber, order.ShippingCompany,
                    order.ShippingAddress1, order.ShippingAddress2, order.ShippingCity,
                    order.ShippingStateProvince, order.ShippingStateProvinceID, order.ShippingZipPostalCode,
                    order.ShippingCountry, order.ShippingCountryID,
                    order.ShippingMethod, order.ShippingRateComputationMethodID, order.ShippedDate,
                    order.TrackingNumber, order.Deleted, order.CreatedOn);

            InsertOrderNote(order.OrderID, string.Format("Order has been marked as paid"), false, DateTime.Now);

            order = CheckOrderStatus(order.OrderID);

            return order;
        }

        /// <summary>
        /// Gets a value indicating whether refund from admin panel is allowed
        /// </summary>
        /// <param name="order">Order</param>
        /// <returns>A value indicating whether refund from admin panel is allowed</returns>
        public static bool CanRefund(Order order)
        {
            if (order == null)
                return false;

            if (order.OrderStatus == OrderStatusEnum.Cancelled)
                return false;

            if (order.PaymentStatus == PaymentStatusEnum.Paid &&
                PaymentManager.CanRefund(order.PaymentMethodID))
                return true;

            return false;
        }

        /// <summary>
        /// Refunds order (from admin panel)
        /// </summary>
        /// <param name="OrderID">Order identifier</param>
        /// <param name="Error">Error</param>
        /// <returns>Refunded order</returns>
        public static Order Refund(int OrderID, ref string Error)
        {
            var order = GetOrderByID(OrderID);
            if (order == null)
                return order;

            if (!CanRefund(order))
                throw new NopException("Can not do refund for order.");

            var cancelPaymentResult = new CancelPaymentResult();
            try
            {
                //old info from placing order
                cancelPaymentResult.AuthorizationTransactionID = order.AuthorizationTransactionID;
                cancelPaymentResult.CaptureTransactionID = order.CaptureTransactionID;
                cancelPaymentResult.SubscriptionTransactionID = order.SubscriptionTransactionID;
                cancelPaymentResult.Amount = order.OrderTotal;
                cancelPaymentResult.PaymentStatus = order.PaymentStatus;

                PaymentManager.Refund(order, ref cancelPaymentResult);

                if (String.IsNullOrEmpty(cancelPaymentResult.Error))
                {
                    order = UpdateOrder(order.OrderID, order.OrderGUID, order.CustomerID, order.CustomerLanguageID,
                        order.CustomerTaxDisplayType, order.CustomerIP, order.OrderSubtotalInclTax, order.OrderSubtotalExclTax, order.OrderShippingInclTax,
                        order.OrderShippingExclTax, order.PaymentMethodAdditionalFeeInclTax, order.PaymentMethodAdditionalFeeExclTax,
                        order.OrderTax, order.OrderTotal, order.OrderDiscount,
                        order.OrderSubtotalInclTaxInCustomerCurrency, order.OrderSubtotalExclTaxInCustomerCurrency,
                        order.OrderShippingInclTaxInCustomerCurrency, order.OrderShippingExclTaxInCustomerCurrency,
                        order.PaymentMethodAdditionalFeeInclTaxInCustomerCurrency, order.PaymentMethodAdditionalFeeExclTaxInCustomerCurrency,
                        order.OrderTaxInCustomerCurrency, order.OrderTotalInCustomerCurrency,
                        order.OrderDiscountInCustomerCurrency, order.CustomerCurrencyCode, order.OrderWeight,
                        order.AffiliateID, order.OrderStatus, order.AllowStoringCreditCardNumber,
                        order.CardType, order.CardName, order.CardNumber, order.MaskedCreditCardNumber,
                        order.CardCVV2, order.CardExpirationMonth, order.CardExpirationYear,
                        order.PaymentMethodID, order.PaymentMethodName,
                        cancelPaymentResult.AuthorizationTransactionID,
                        order.AuthorizationTransactionCode,
                        order.AuthorizationTransactionResult,
                        cancelPaymentResult.CaptureTransactionID,
                        order.CaptureTransactionResult,
                        order.SubscriptionTransactionID, order.PurchaseOrderNumber, cancelPaymentResult.PaymentStatus, order.PaidDate,
                        order.BillingFirstName, order.BillingLastName, order.BillingPhoneNumber,
                        order.BillingEmail, order.BillingFaxNumber, order.BillingCompany, order.BillingAddress1,
                        order.BillingAddress2, order.BillingCity,
                        order.BillingStateProvince, order.BillingStateProvinceID, order.BillingZipPostalCode,
                        order.BillingCountry, order.BillingCountryID, order.ShippingStatus,
                        order.ShippingFirstName, order.ShippingLastName, order.ShippingPhoneNumber,
                        order.ShippingEmail, order.ShippingFaxNumber, order.ShippingCompany,
                        order.ShippingAddress1, order.ShippingAddress2, order.ShippingCity,
                        order.ShippingStateProvince, order.ShippingStateProvinceID, order.ShippingZipPostalCode,
                        order.ShippingCountry, order.ShippingCountryID,
                        order.ShippingMethod, order.ShippingRateComputationMethodID, order.ShippedDate,
                        order.TrackingNumber, order.Deleted, order.CreatedOn);

                    InsertOrderNote(order.OrderID, string.Format("Order has been refunded"), false, DateTime.Now);

                }
                else
                {
                    InsertOrderNote(order.OrderID, string.Format("Unable to refund order. Error: {0}", cancelPaymentResult.Error), false, DateTime.Now);

                }
                order = CheckOrderStatus(order.OrderID);
            }
            catch (Exception exc)
            {
                cancelPaymentResult.Error = exc.Message;
                cancelPaymentResult.FullError = exc.ToString();
            }

            if (!String.IsNullOrEmpty(cancelPaymentResult.Error))
            {
                Error = cancelPaymentResult.Error;
                LogManager.InsertLog(LogTypeEnum.OrderError, string.Format("Error refunding order. {0}", cancelPaymentResult.Error), cancelPaymentResult.FullError);
            }
            return order;
        }

        /// <summary>
        /// Gets a value indicating whether order can be marked as refunded
        /// </summary>
        /// <param name="order">Order</param>
        /// <returns>A value indicating whether order can be marked as refunded</returns>
        public static bool CanRefundOffline(Order order)
        {
            if (order == null)
                return false;

            if (order.OrderStatus == OrderStatusEnum.Cancelled)
                return false;

            if (order.PaymentStatus == PaymentStatusEnum.Paid)
                return true;

            return false;
        }

        /// <summary>
        /// Refunds order (offline)
        /// </summary>
        /// <param name="OrderID">Order identifier</param>
        /// <returns>Updated order</returns>
        public static Order RefundOffline(int OrderID)
        {
            var order = GetOrderByID(OrderID);
            if (order == null)
                return order;

            if (!CanRefundOffline(order))
                throw new NopException("You can't refund this order");

            order = UpdateOrder(order.OrderID, order.OrderGUID, order.CustomerID, order.CustomerLanguageID,
                order.CustomerTaxDisplayType, order.CustomerIP, order.OrderSubtotalInclTax, order.OrderSubtotalExclTax, order.OrderShippingInclTax,
                   order.OrderShippingExclTax, order.PaymentMethodAdditionalFeeInclTax, order.PaymentMethodAdditionalFeeExclTax,
                   order.OrderTax, order.OrderTotal, order.OrderDiscount,
                   order.OrderSubtotalInclTaxInCustomerCurrency, order.OrderSubtotalExclTaxInCustomerCurrency,
                   order.OrderShippingInclTaxInCustomerCurrency, order.OrderShippingExclTaxInCustomerCurrency,
                   order.PaymentMethodAdditionalFeeInclTaxInCustomerCurrency, order.PaymentMethodAdditionalFeeExclTaxInCustomerCurrency,
                   order.OrderTaxInCustomerCurrency, order.OrderTotalInCustomerCurrency,
                   order.OrderDiscountInCustomerCurrency, order.CustomerCurrencyCode, order.OrderWeight,
                   order.AffiliateID, order.OrderStatus, order.AllowStoringCreditCardNumber, order.CardType,
                   order.CardName, order.CardNumber, order.MaskedCreditCardNumber,
                   order.CardCVV2, order.CardExpirationMonth, order.CardExpirationYear,
                   order.PaymentMethodID, order.PaymentMethodName,
                   order.AuthorizationTransactionID,
                   order.AuthorizationTransactionCode, order.AuthorizationTransactionResult,
                   order.CaptureTransactionID, order.CaptureTransactionResult,
                   order.SubscriptionTransactionID, order.PurchaseOrderNumber, PaymentStatusEnum.Refunded, order.PaidDate,
                   order.BillingFirstName, order.BillingLastName, order.BillingPhoneNumber,
                   order.BillingEmail, order.BillingFaxNumber, order.BillingCompany, order.BillingAddress1,
                   order.BillingAddress2, order.BillingCity,
                   order.BillingStateProvince, order.BillingStateProvinceID, order.BillingZipPostalCode,
                   order.BillingCountry, order.BillingCountryID, order.ShippingStatus,
                   order.ShippingFirstName, order.ShippingLastName, order.ShippingPhoneNumber,
                   order.ShippingEmail, order.ShippingFaxNumber, order.ShippingCompany,
                   order.ShippingAddress1, order.ShippingAddress2, order.ShippingCity,
                   order.ShippingStateProvince, order.ShippingStateProvinceID, order.ShippingZipPostalCode,
                   order.ShippingCountry, order.ShippingCountryID,
                   order.ShippingMethod, order.ShippingRateComputationMethodID, order.ShippedDate,
                   order.TrackingNumber, order.Deleted, order.CreatedOn);

            InsertOrderNote(order.OrderID, string.Format("Order has been marked as refunded"), false, DateTime.Now);

            order = CheckOrderStatus(order.OrderID);

            return order;
        }

        /// <summary>
        /// Gets a value indicating whether void from admin panel is allowed
        /// </summary>
        /// <param name="order">Order</param>
        /// <returns>A value indicating whether void from admin panel is allowed</returns>
        public static bool CanVoid(Order order)
        {
            if (order == null)
                return false;

            if (order.OrderStatus == OrderStatusEnum.Cancelled)
                return false;

            if (order.PaymentStatus == PaymentStatusEnum.Authorized &&
                PaymentManager.CanVoid(order.PaymentMethodID))
                return true;

            return false;
        }

        /// <summary>
        /// Voids order (from admin panel)
        /// </summary>
        /// <param name="OrderID">Order identifier</param>
        /// <param name="Error">Error</param>
        /// <returns>Voided order</returns>
        public static Order Void(int OrderID, ref string Error)
        {
            var order = GetOrderByID(OrderID);
            if (order == null)
                return order;

            if (!CanVoid(order))
                throw new NopException("Can not do void for order.");

            var cancelPaymentResult = new CancelPaymentResult();
            try
            {
                //old info from placing order
                cancelPaymentResult.AuthorizationTransactionID = order.AuthorizationTransactionID;
                cancelPaymentResult.CaptureTransactionID = order.CaptureTransactionID;
                cancelPaymentResult.Amount = order.OrderTotal;
                cancelPaymentResult.PaymentStatus = order.PaymentStatus;

                PaymentManager.Void(order, ref cancelPaymentResult);

                if (String.IsNullOrEmpty(cancelPaymentResult.Error))
                {
                    order = UpdateOrder(order.OrderID, order.OrderGUID, order.CustomerID, order.CustomerLanguageID,
                        order.CustomerTaxDisplayType, order.CustomerIP, order.OrderSubtotalInclTax, order.OrderSubtotalExclTax, order.OrderShippingInclTax,
                        order.OrderShippingExclTax, order.PaymentMethodAdditionalFeeInclTax, order.PaymentMethodAdditionalFeeExclTax,
                        order.OrderTax, order.OrderTotal, order.OrderDiscount,
                        order.OrderSubtotalInclTaxInCustomerCurrency, order.OrderSubtotalExclTaxInCustomerCurrency,
                        order.OrderShippingInclTaxInCustomerCurrency, order.OrderShippingExclTaxInCustomerCurrency,
                        order.PaymentMethodAdditionalFeeInclTaxInCustomerCurrency, order.PaymentMethodAdditionalFeeExclTaxInCustomerCurrency,
                        order.OrderTaxInCustomerCurrency, order.OrderTotalInCustomerCurrency,
                        order.OrderDiscountInCustomerCurrency, order.CustomerCurrencyCode, order.OrderWeight,
                        order.AffiliateID, order.OrderStatus, order.AllowStoringCreditCardNumber,
                        order.CardType, order.CardName, order.CardNumber, order.MaskedCreditCardNumber,
                        order.CardCVV2, order.CardExpirationMonth, order.CardExpirationYear,
                        order.PaymentMethodID, order.PaymentMethodName,
                        cancelPaymentResult.AuthorizationTransactionID,
                        order.AuthorizationTransactionCode,
                        order.AuthorizationTransactionResult,
                        cancelPaymentResult.CaptureTransactionID,
                        order.CaptureTransactionResult,
                        order.SubscriptionTransactionID, order.PurchaseOrderNumber,
                        cancelPaymentResult.PaymentStatus, order.PaidDate,
                        order.BillingFirstName, order.BillingLastName, order.BillingPhoneNumber,
                        order.BillingEmail, order.BillingFaxNumber, order.BillingCompany, order.BillingAddress1,
                        order.BillingAddress2, order.BillingCity,
                        order.BillingStateProvince, order.BillingStateProvinceID, order.BillingZipPostalCode,
                        order.BillingCountry, order.BillingCountryID, order.ShippingStatus,
                        order.ShippingFirstName, order.ShippingLastName, order.ShippingPhoneNumber,
                        order.ShippingEmail, order.ShippingFaxNumber, order.ShippingCompany,
                        order.ShippingAddress1, order.ShippingAddress2, order.ShippingCity,
                        order.ShippingStateProvince, order.ShippingStateProvinceID, order.ShippingZipPostalCode,
                        order.ShippingCountry, order.ShippingCountryID,
                        order.ShippingMethod, order.ShippingRateComputationMethodID, order.ShippedDate,
                        order.TrackingNumber, order.Deleted, order.CreatedOn);

                    InsertOrderNote(order.OrderID, string.Format("Order has been voided"), false, DateTime.Now);

                }
                else
                {
                    InsertOrderNote(order.OrderID, string.Format("Unable to void order. Error: {0}", cancelPaymentResult.Error), false, DateTime.Now);

                }
                order = CheckOrderStatus(order.OrderID);
            }
            catch (Exception exc)
            {
                cancelPaymentResult.Error = exc.Message;
                cancelPaymentResult.FullError = exc.ToString();
            }

            if (!String.IsNullOrEmpty(cancelPaymentResult.Error))
            {
                Error = cancelPaymentResult.Error;
                LogManager.InsertLog(LogTypeEnum.OrderError, string.Format("Error voiding order. {0}", cancelPaymentResult.Error), cancelPaymentResult.FullError);
            }
            return order;
        }

        /// <summary>
        /// Gets a value indicating whether order can be marked as voided
        /// </summary>
        /// <param name="order">Order</param>
        /// <returns>A value indicating whether order can be marked as voided</returns>
        public static bool CanVoidOffline(Order order)
        {
            if (order == null)
                return false;

            if (order.OrderStatus == OrderStatusEnum.Cancelled)
                return false;

            if (order.PaymentStatus == PaymentStatusEnum.Authorized)
                return true;

            return false;
        }

        /// <summary>
        /// Voids order (offline)
        /// </summary>
        /// <param name="OrderID">Order identifier</param>
        /// <returns>Updated order</returns>
        public static Order VoidOffline(int OrderID)
        {
            var order = GetOrderByID(OrderID);
            if (order == null)
                return order;

            if (!CanVoidOffline(order))
                throw new NopException("You can't void this order");

            order = UpdateOrder(order.OrderID, order.OrderGUID, order.CustomerID, order.CustomerLanguageID,
                order.CustomerTaxDisplayType, order.CustomerIP, order.OrderSubtotalInclTax, order.OrderSubtotalExclTax, order.OrderShippingInclTax,
                   order.OrderShippingExclTax, order.PaymentMethodAdditionalFeeInclTax, order.PaymentMethodAdditionalFeeExclTax,
                   order.OrderTax, order.OrderTotal, order.OrderDiscount,
                   order.OrderSubtotalInclTaxInCustomerCurrency, order.OrderSubtotalExclTaxInCustomerCurrency,
                   order.OrderShippingInclTaxInCustomerCurrency, order.OrderShippingExclTaxInCustomerCurrency,
                   order.PaymentMethodAdditionalFeeInclTaxInCustomerCurrency, order.PaymentMethodAdditionalFeeExclTaxInCustomerCurrency,
                   order.OrderTaxInCustomerCurrency, order.OrderTotalInCustomerCurrency,
                   order.OrderDiscountInCustomerCurrency, order.CustomerCurrencyCode, order.OrderWeight,
                   order.AffiliateID, order.OrderStatus, order.AllowStoringCreditCardNumber, order.CardType,
                   order.CardName, order.CardNumber, order.MaskedCreditCardNumber,
                   order.CardCVV2, order.CardExpirationMonth, order.CardExpirationYear,
                   order.PaymentMethodID, order.PaymentMethodName,
                   order.AuthorizationTransactionID,
                   order.AuthorizationTransactionCode, order.AuthorizationTransactionResult,
                   order.CaptureTransactionID, order.CaptureTransactionResult,
                   order.SubscriptionTransactionID, order.PurchaseOrderNumber, PaymentStatusEnum.Voided, order.PaidDate,
                   order.BillingFirstName, order.BillingLastName, order.BillingPhoneNumber,
                   order.BillingEmail, order.BillingFaxNumber, order.BillingCompany, order.BillingAddress1,
                   order.BillingAddress2, order.BillingCity,
                   order.BillingStateProvince, order.BillingStateProvinceID, order.BillingZipPostalCode,
                   order.BillingCountry, order.BillingCountryID, order.ShippingStatus,
                   order.ShippingFirstName, order.ShippingLastName, order.ShippingPhoneNumber,
                   order.ShippingEmail, order.ShippingFaxNumber, order.ShippingCompany,
                   order.ShippingAddress1, order.ShippingAddress2, order.ShippingCity,
                   order.ShippingStateProvince, order.ShippingStateProvinceID, order.ShippingZipPostalCode,
                   order.ShippingCountry, order.ShippingCountryID,
                   order.ShippingMethod, order.ShippingRateComputationMethodID, order.ShippedDate,
                   order.TrackingNumber, order.Deleted, order.CreatedOn);

            InsertOrderNote(order.OrderID, string.Format("Order has been marked as voided"), false, DateTime.Now);

            order = CheckOrderStatus(order.OrderID);

            return order;
        }

        #endregion

        #endregion

        #region Property
        /// <summary>
        /// Gets a value indicating whether cache is enabled
        /// </summary>
        public static bool CacheEnabled
        {
            get
            {
                return SettingManager.GetSettingValueBoolean("Cache.OrderManager.CacheEnabled");
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether customer can make re-order
        /// </summary>
        public static bool IsReOrderAllowed
        {
            get
            {
                return SettingManager.GetSettingValueBoolean("Order.IsReOrderAllowed", true);
            }
            set
            {
                SettingManager.SetParam("Order.IsReOrderAllowed", value.ToString());
            }
        }
        #endregion
    }
}
