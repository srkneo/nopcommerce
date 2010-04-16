using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using NopSolutions.NopCommerce.BusinessLogic;
using NopSolutions.NopCommerce.BusinessLogic.Orders;
using NopSolutions.NopCommerce.BusinessLogic.SEO;
using NopSolutions.NopCommerce.Common.Utils;
using NopSolutions.NopCommerce.Payment.Methods.USAePay;

namespace NopSolutions.NopCommerce.Web
{
    public partial class USAePayEPaymentFormReturn : BaseNopPage
    {
        #region Handlers
        protected void Page_Load(object sender, EventArgs e)
        {
            if(NopContext.Current.User == null)
            {
                string loginURL = SEOHelper.GetLoginPageURL(true);
                Response.Redirect(loginURL);
            }

            CommonHelper.SetResponseNoCache(Response);

            if(!Page.IsPostBack)
            {
                if(EPaymentFormSettings.UsePIN && !EPaymentFormHelper.ValidateResponseSign(Request.Form))
                {
                    Response.Redirect(CommonHelper.GetStoreLocation());
                }
                if(!Request.Form["UMstatus"].Equals("Approved"))
                {
                    Response.Redirect(CommonHelper.GetStoreLocation());
                }

                int OrderID = 0;
                if(!Int32.TryParse(Request.Form["UMinvoice"], out OrderID))
                {
                    Response.Redirect(CommonHelper.GetStoreLocation());
                }
                Order order = OrderManager.GetOrderByID(OrderID);
                if(order == null || NopContext.Current.User.CustomerID != order.CustomerID)
                {
                    Response.Redirect(CommonHelper.GetStoreLocation());
                }

                string transactionID = Request.Form["UMrefNum"];

                if(EPaymentFormSettings.AuthorizeOnly)
                {
                    //set AuthorizationTransactionID
                    order = OrderManager.UpdateOrder(order.OrderID, order.OrderGUID, order.CustomerID, order.CustomerLanguageID,
                       order.CustomerTaxDisplayType, order.CustomerIP, order.OrderSubtotalInclTax, order.OrderSubtotalExclTax, order.OrderShippingInclTax,
                       order.OrderShippingExclTax, order.PaymentMethodAdditionalFeeInclTax, order.PaymentMethodAdditionalFeeExclTax,
                       order.OrderTax, order.OrderTotal, order.OrderDiscount,
                       order.OrderSubtotalInclTaxInCustomerCurrency, order.OrderSubtotalExclTaxInCustomerCurrency,
                       order.OrderShippingInclTaxInCustomerCurrency, order.OrderShippingExclTaxInCustomerCurrency,
                       order.PaymentMethodAdditionalFeeInclTaxInCustomerCurrency, order.PaymentMethodAdditionalFeeExclTaxInCustomerCurrency,
                       order.OrderTaxInCustomerCurrency, order.OrderTotalInCustomerCurrency,
                       order.OrderDiscountInCustomerCurrency,
                       order.CheckoutAttributeDescription, order.CheckoutAttributesXML,
                       order.CustomerCurrencyCode, order.OrderWeight,
                       order.AffiliateID, order.OrderStatus, order.AllowStoringCreditCardNumber,
                       order.CardType, order.CardName, order.CardNumber, order.MaskedCreditCardNumber,
                       order.CardCVV2, order.CardExpirationMonth, order.CardExpirationYear,
                       order.PaymentMethodID, order.PaymentMethodName,
                       transactionID,
                       order.AuthorizationTransactionCode, order.AuthorizationTransactionResult,
                       order.CaptureTransactionID, order.CaptureTransactionResult,
                       order.SubscriptionTransactionID, order.PurchaseOrderNumber,
                       order.PaymentStatus, order.PaidDate,
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

                    if(OrderManager.CanMarkOrderAsAuthorized(order))
                    {
                        OrderManager.MarkAsAuthorized(order.OrderID);
                    }
                }
                else
                {
                    //set CaptureTransactionID
                    order = OrderManager.UpdateOrder(order.OrderID, order.OrderGUID, order.CustomerID, order.CustomerLanguageID,
                       order.CustomerTaxDisplayType, order.CustomerIP, order.OrderSubtotalInclTax, order.OrderSubtotalExclTax, order.OrderShippingInclTax,
                       order.OrderShippingExclTax, order.PaymentMethodAdditionalFeeInclTax, order.PaymentMethodAdditionalFeeExclTax,
                       order.OrderTax, order.OrderTotal, order.OrderDiscount,
                       order.OrderSubtotalInclTaxInCustomerCurrency, order.OrderSubtotalExclTaxInCustomerCurrency,
                       order.OrderShippingInclTaxInCustomerCurrency, order.OrderShippingExclTaxInCustomerCurrency,
                       order.PaymentMethodAdditionalFeeInclTaxInCustomerCurrency, order.PaymentMethodAdditionalFeeExclTaxInCustomerCurrency,
                       order.OrderTaxInCustomerCurrency, order.OrderTotalInCustomerCurrency,
                       order.OrderDiscountInCustomerCurrency,
                       order.CheckoutAttributeDescription, order.CheckoutAttributesXML, 
                       order.CustomerCurrencyCode, order.OrderWeight,
                       order.AffiliateID, order.OrderStatus, order.AllowStoringCreditCardNumber,
                       order.CardType, order.CardName, order.CardNumber, order.MaskedCreditCardNumber,
                       order.CardCVV2, order.CardExpirationMonth, order.CardExpirationYear,
                       order.PaymentMethodID, order.PaymentMethodName,
                       order.AuthorizationTransactionID,
                       order.AuthorizationTransactionCode, order.AuthorizationTransactionResult,
                       transactionID, order.CaptureTransactionResult,
                       order.SubscriptionTransactionID, order.PurchaseOrderNumber,
                       order.PaymentStatus, order.PaidDate,
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

                    if(OrderManager.CanMarkOrderAsPaid(order))
                    {
                        OrderManager.MarkOrderAsPaid(order.OrderID);
                    }
                }

                Response.Redirect("~/checkoutcompleted.aspx");
            }
        }
        #endregion    
    }
}
