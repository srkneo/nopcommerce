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
using System.Collections;
using System.Configuration;
using System.Data;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using NopSolutions.NopCommerce.BusinessLogic;
using NopSolutions.NopCommerce.BusinessLogic.CustomerManagement;
using NopSolutions.NopCommerce.BusinessLogic.Directory;
using NopSolutions.NopCommerce.BusinessLogic.Localization;
using NopSolutions.NopCommerce.BusinessLogic.Measures;
using NopSolutions.NopCommerce.BusinessLogic.Media;
using NopSolutions.NopCommerce.BusinessLogic.Orders;
using NopSolutions.NopCommerce.BusinessLogic.Payment;
using NopSolutions.NopCommerce.BusinessLogic.Products;
using NopSolutions.NopCommerce.BusinessLogic.Profile;
using NopSolutions.NopCommerce.BusinessLogic.Promo.Affiliates;
using NopSolutions.NopCommerce.BusinessLogic.Security;
using NopSolutions.NopCommerce.BusinessLogic.Shipping;
using NopSolutions.NopCommerce.BusinessLogic.Tax;
using NopSolutions.NopCommerce.BusinessLogic.Utils;
using NopSolutions.NopCommerce.BusinessLogic.Utils.Html;
using NopSolutions.NopCommerce.Common.Utils;
using System.IO;


namespace NopSolutions.NopCommerce.Web.Administration.Modules
{
    public partial class OrderDetailsControl : BaseNopAdministrationUserControl
    {
        private void BindData()
        {
            Order order = OrderManager.GetOrderById(this.OrderId);
            if (order != null && !order.Deleted)
            {
                this.lblOrderStatus.Text = OrderManager.GetOrderStatusName(order.OrderStatusId);
                this.CancelOrderButton.Visible = OrderManager.CanCancelOrder(order);
                this.lblOrderId.Text = order.OrderId.ToString();
                this.lblOrderGuid.Text = order.OrderGuid.ToString();

                Customer customer = order.Customer;
                if (customer != null)
                {
                    if (customer.IsGuest)
                    {
                        this.lblCustomer.Text = string.Format("<a href=\"CustomerDetails.aspx?CustomerID={0}\">{1}</a>", customer.CustomerId, GetLocaleResourceString("Admin.OrderDetails.Guest"));
                    }
                    else
                    {
                        this.lblCustomer.Text = string.Format("<a href=\"CustomerDetails.aspx?CustomerID={0}\">{1}</a>", customer.CustomerId, Server.HtmlEncode(customer.Email));
                    }
                }
                else
                {
                    this.lblCustomer.Text = "Customer was deleted";
                }

                if(!String.IsNullOrEmpty(order.CustomerIP))
                {
                    lblCustomerIP.Text = order.CustomerIP;
                }
                else
                {
                    btnBanByCustomerIP.Enabled = false;
                }

                Affiliate affiliate = order.Affiliate;
                if (affiliate != null)
                {
                    this.lblAffiliate.Text = string.Format("<a href=\"AffiliateDetails.aspx?AffiliateID={0}\">{1}</a>", affiliate.AffiliateId, Server.HtmlEncode(affiliate.FullName));
                    this.divAffiliate.Visible = true;
                }
                else
                    this.divAffiliate.Visible = false;

                this.lblCreatedOn.Text = DateTimeHelper.ConvertToUserTime(order.CreatedOn).ToString();

                BindOrderTotals(order);
                BindPaymentInfo(order);
                BindBillingInfo(order);
                BindShippingInfo(order);
                BindProductInfo(order);
                BindOrderNotes(order);
            }
            else
                Response.Redirect("Orders.aspx");
        }

        private void BindPaymentInfo(Order order)
        {
            if (order == null)
                return;

            if (order.AllowStoringCreditCardNumber)
            {
                //card type
                string cardTypeDecrypted = SecurityHelper.Decrypt(order.CardType);
                if (!String.IsNullOrEmpty(cardTypeDecrypted))
                {
                    this.lblCardType.Text = Server.HtmlEncode(cardTypeDecrypted);
                }
                else
                {
                    pnlCartType.Visible = false;
                }

                //cardholder name
                string cardNameDecrypted = SecurityHelper.Decrypt(order.CardName);
                if (!String.IsNullOrEmpty(cardNameDecrypted))
                {
                    this.lblCardName.Text = Server.HtmlEncode(cardNameDecrypted);
                }
                else
                {
                    pnlCardName.Visible = false;
                }

                //card number
                string cardNumberDecrypted = SecurityHelper.Decrypt(order.CardNumber);
                if (!String.IsNullOrEmpty(cardNumberDecrypted))
                {
                    this.lblCardNumber.Text = Server.HtmlEncode(cardNumberDecrypted);
                }
                else
                {
                    pnlCardNumber.Visible = false;
                }

                //cvv
                string cardCVV2Decrypted = SecurityHelper.Decrypt(order.CardCvv2);
                this.lblCardCVV2.Text = Server.HtmlEncode(cardCVV2Decrypted);

                //expiry date
                string cardExpirationMonthDecrypted = SecurityHelper.Decrypt(order.CardExpirationMonth);
                if (!String.IsNullOrEmpty(cardExpirationMonthDecrypted) && cardExpirationMonthDecrypted != "0")
                {
                    this.lblCardExpirationMonth.Text = cardExpirationMonthDecrypted;
                }
                else
                {
                    pnlCardExpiryMonth.Visible = false;
                }
                string cardExpirationYearDecrypted = SecurityHelper.Decrypt(order.CardExpirationYear);
                if (!String.IsNullOrEmpty(cardExpirationYearDecrypted) && cardExpirationYearDecrypted != "0")
                {
                    this.lblCardExpirationYear.Text = cardExpirationYearDecrypted;
                }
                else
                {
                    pnlCardExpiryYear.Visible = false;
                }
            }
            else
            {
                pnlCartType.Visible = false;
                pnlCardName.Visible = false;

                string maskedCreditCardNumberDecrypted = SecurityHelper.Decrypt(order.MaskedCreditCardNumber);
                if (!String.IsNullOrEmpty(maskedCreditCardNumberDecrypted))
                {
                    this.lblCardNumber.Text = Server.HtmlEncode(maskedCreditCardNumberDecrypted);
                }
                else
                {
                    pnlCardNumber.Visible = false;
                }

                pnlCardCVV2.Visible = false;
                pnlCardExpiryMonth.Visible = false;
                pnlCardExpiryYear.Visible = false;
            }

            PaymentMethod pm = PaymentMethodManager.GetPaymentMethodById(order.PaymentMethodId);
            if (pm != null && pm.SystemKeyword == "PURCHASEORDER")
            {
                this.lblPONumber.Text = Server.HtmlEncode(order.PurchaseOrderNumber);
            }
            else
            {
                pnlPONumber.Visible = false;
            }
            this.lblPaymentMethodName.Text = Server.HtmlEncode(order.PaymentMethodName);
            this.lblPaymentStatus.Text = PaymentStatusManager.GetPaymentStatusName(order.PaymentStatusId);
            this.btnCapture.Visible = OrderManager.CanCapture(order);
            this.btnMarkAsPaid.Visible = OrderManager.CanMarkOrderAsPaid(order);
            this.btnRefund.Visible = OrderManager.CanRefund(order);
            this.btnRefundOffline.Visible = OrderManager.CanRefundOffline(order);
            this.btnVoid.Visible = OrderManager.CanVoid(order);
            this.btnVoidOffline.Visible = OrderManager.CanVoidOffline(order);
        }

        protected void BindBillingInfo(Order order)
        {
            if (order == null)
                return;

            this.lBillingFirstName.Text = Server.HtmlEncode(order.BillingFirstName);
            this.lBillingLastName.Text = Server.HtmlEncode(order.BillingLastName);
            this.lBillingPhoneNumber.Text = Server.HtmlEncode(order.BillingPhoneNumber);
            this.lBillingEmail.Text = Server.HtmlEncode(order.BillingEmail);
            this.lBillingFaxNumber.Text = Server.HtmlEncode(order.BillingFaxNumber);
            if (!String.IsNullOrEmpty(order.BillingCompany))
                this.lBillingCompany.Text = Server.HtmlEncode(order.BillingCompany);
            else
                this.pnlBillingCompany.Visible = false;
            this.lBillingAddress1.Text = Server.HtmlEncode(order.BillingAddress1);
            if (!String.IsNullOrEmpty(order.BillingAddress2))
                this.lBillingAddress2.Text = Server.HtmlEncode(order.BillingAddress2);
            else
                this.pnlBillingAddress2.Visible = false;
            this.lBillingCity.Text = Server.HtmlEncode(order.BillingCity);
            if (!String.IsNullOrEmpty(order.BillingCountry))
                this.lBillingCountry.Text = Server.HtmlEncode(order.BillingCountry);
            else
                this.pnlBillingCountry.Visible = false;
            if (!String.IsNullOrEmpty(order.BillingStateProvince))
                this.lBillingStateProvince.Text = Server.HtmlEncode(order.BillingStateProvince);
            this.lBillingZipPostalCode.Text = Server.HtmlEncode(order.BillingZipPostalCode);
        }

        protected void BindShippingInfo(Order order)
        {
            if (order == null)
                return;

            if (order.ShippingStatus != ShippingStatusEnum.ShippingNotRequired)
            {
                this.lShippingFirstName.Text = Server.HtmlEncode(order.ShippingFirstName);
                this.lShippingLastName.Text = Server.HtmlEncode(order.ShippingLastName);
                this.lShippingPhoneNumber.Text = Server.HtmlEncode(order.ShippingPhoneNumber);
                this.lShippingEmail.Text = Server.HtmlEncode(order.ShippingEmail);
                this.lShippingFaxNumber.Text = Server.HtmlEncode(order.ShippingFaxNumber);
                if (!String.IsNullOrEmpty(order.ShippingCompany))
                    this.lShippingCompany.Text = Server.HtmlEncode(order.ShippingCompany);
                else
                    this.pnlShippingCompany.Visible = false;
                this.lShippingAddress1.Text = Server.HtmlEncode(order.ShippingAddress1);
                if (!String.IsNullOrEmpty(order.ShippingAddress2))
                    this.lShippingAddress2.Text = Server.HtmlEncode(order.ShippingAddress2);
                else
                    this.pnlShippingAddress2.Visible = false;
                this.lShippingCity.Text = Server.HtmlEncode(order.ShippingCity);
                if (!String.IsNullOrEmpty(order.ShippingCountry))
                    this.lShippingCountry.Text = Server.HtmlEncode(order.ShippingCountry);
                else
                    this.pnlShippingCountry.Visible = false;
                if (!String.IsNullOrEmpty(order.ShippingStateProvince))
                    this.lShippingStateProvince.Text = Server.HtmlEncode(order.ShippingStateProvince);
                this.lShippingZipPostalCode.Text = Server.HtmlEncode(order.ShippingZipPostalCode);

                this.lShippingAddressGoogle.NavigateUrl = string.Format("http://maps.google.com/maps?f=q&hl=en&ie=UTF8&oe=UTF8&geocode=&q={0}", Server.UrlEncode(order.ShippingAddress1 + " " + order.ShippingZipPostalCode + " " + order.ShippingCity + " " + order.ShippingCountry));

                this.lblShippingMethod.Text = Server.HtmlEncode(order.ShippingMethod);

                this.txtTrackingNumber.Text = order.TrackingNumber;
                
                this.btnSetAsShipped.Visible = OrderManager.CanShip(order);

                if (order.ShippedDate.HasValue)
                {
                    this.lblShippedDate.Text = DateTimeHelper.ConvertToUserTime(order.ShippedDate.Value).ToString();
                }

                this.lblOrderWeight.Text = string.Format("{0} [{1}]", order.OrderWeight, MeasureManager.BaseWeightIn.Name);

                this.divShippingNotRequired.Visible = false;
                this.divShippingAddress.Visible = true;
                this.divShippingWeight.Visible = true;
                this.divShippingMethod.Visible = true;
                this.divTrackingNumber.Visible = true;
                this.divShippedDate.Visible = true;
            }
            else
            {
                this.divShippingNotRequired.Visible = true;
                this.divShippingAddress.Visible = false;
                this.divShippingWeight.Visible = false;
                this.divShippingMethod.Visible = false;
                this.divTrackingNumber.Visible = false;
                this.divShippedDate.Visible = false;
            }
        }

        protected void BindProductInfo(Order order)
        {
            if (order == null)
                return;

            OrderProductVariantCollection orderProductVariants = order.OrderProductVariants;
            bool hasDownloadableItems = false;
            foreach (OrderProductVariant orderProductVariant in orderProductVariants)
            {
                if (orderProductVariant.ProductVariant != null && orderProductVariant.ProductVariant.IsDownload)
                {
                    hasDownloadableItems = true;
                    break;
                }
            }
            gvOrderProductVariants.Columns[1].Visible = hasDownloadableItems;

            this.gvOrderProductVariants.DataSource = orderProductVariants;
            this.gvOrderProductVariants.DataBind();

            this.lCheckoutAttributes.Text = order.CheckoutAttributeDescription;
        }

        private void BindOrderTotals(Order order)
        {
            if (order == null)
                return;

            //subtotal
            string orderSubtotalInclTaxStr = PriceHelper.FormatPrice(order.OrderSubtotalInclTax, true, CurrencyManager.PrimaryStoreCurrency, NopContext.Current.WorkingLanguage, true);
            string orderSubtotalExclTaxStr = PriceHelper.FormatPrice(order.OrderSubtotalExclTax, true, CurrencyManager.PrimaryStoreCurrency, NopContext.Current.WorkingLanguage, false);
            if (TaxManager.AllowCustomersToSelectTaxDisplayType)
            {
                this.lblOrderSubtotalInclTax.Text = orderSubtotalInclTaxStr;
                this.lblOrderSubtotalExclTax.Text = orderSubtotalExclTaxStr;
                this.pnlOrderSubtotalInclTax.Visible = true;
                this.pnlOrderSubtotalExclTax.Visible = true;
            }
            else
            {
                switch (TaxManager.TaxDisplayType)
                {
                    case TaxDisplayTypeEnum.ExcludingTax:
                        {
                            this.lblOrderSubtotalExclTax.Text = orderSubtotalExclTaxStr;
                            this.pnlOrderSubtotalInclTax.Visible = false;
                            this.pnlOrderSubtotalExclTax.Visible = true;
                        }
                        break;
                    case TaxDisplayTypeEnum.IncludingTax:
                        {
                            this.lblOrderSubtotalInclTax.Text = orderSubtotalInclTaxStr;
                            this.pnlOrderSubtotalInclTax.Visible = true;
                            this.pnlOrderSubtotalExclTax.Visible = false;
                        }
                        break;
                    default:
                        {
                            this.pnlOrderSubtotalInclTax.Visible = false;
                            this.pnlOrderSubtotalExclTax.Visible = false;
                        }
                        break;
                }
            }

            //shipping
            string orderShippingInclTaxStr = PriceHelper.FormatShippingPrice(order.OrderShippingInclTax, true, CurrencyManager.PrimaryStoreCurrency, NopContext.Current.WorkingLanguage, true);
            string orderShippingExclTaxStr = PriceHelper.FormatShippingPrice(order.OrderShippingExclTax, true, CurrencyManager.PrimaryStoreCurrency, NopContext.Current.WorkingLanguage, false);
            if (TaxManager.ShippingIsTaxable)
            {
                if (TaxManager.AllowCustomersToSelectTaxDisplayType)
                {
                    this.lblOrderShippingInclTax.Text = orderShippingInclTaxStr;
                    this.lblOrderShippingExclTax.Text = orderShippingExclTaxStr;
                    this.pnlOrderShippingInclTax.Visible = true;
                    this.pnlOrderShippingExclTax.Visible = true;
                }
                else
                {
                    switch (TaxManager.TaxDisplayType)
                    {
                        case TaxDisplayTypeEnum.ExcludingTax:
                            {
                                this.lblOrderShippingExclTax.Text = orderShippingExclTaxStr;
                                this.pnlOrderShippingInclTax.Visible = false;
                                this.pnlOrderShippingExclTax.Visible = true;
                            }
                            break;
                        case TaxDisplayTypeEnum.IncludingTax:
                            {
                                this.lblOrderShippingInclTax.Text = orderShippingInclTaxStr;
                                this.pnlOrderShippingInclTax.Visible = true;
                                this.pnlOrderShippingExclTax.Visible = false;
                            }
                            break;
                        default:
                            {
                                this.pnlOrderShippingInclTax.Visible = false;
                                this.pnlOrderShippingExclTax.Visible = false;
                            }
                            break;
                    }
                }
            }
            else
            {
                switch (TaxManager.TaxDisplayType)
                {
                    case TaxDisplayTypeEnum.ExcludingTax:
                        {
                            this.lblOrderShippingExclTax.Text = orderShippingExclTaxStr;
                            this.pnlOrderShippingInclTax.Visible = false;
                            this.pnlOrderShippingExclTax.Visible = true;
                        }
                        break;
                    case TaxDisplayTypeEnum.IncludingTax:
                        {
                            this.lblOrderShippingInclTax.Text = orderShippingInclTaxStr;
                            this.pnlOrderShippingInclTax.Visible = true;
                            this.pnlOrderShippingExclTax.Visible = false;
                        }
                        break;
                    default:
                        {
                            this.pnlOrderShippingInclTax.Visible = false;
                            this.pnlOrderShippingExclTax.Visible = false;
                        }
                        break;
                }
            }

            //payment method additional fee
            string paymentMethodAdditionalFeeInclTaxStr = PriceHelper.FormatPaymentMethodAdditionalFee(order.PaymentMethodAdditionalFeeInclTax, true, CurrencyManager.PrimaryStoreCurrency, NopContext.Current.WorkingLanguage, true);
            string paymentMethodAdditionalFeeExclTaxStr = PriceHelper.FormatPaymentMethodAdditionalFee(order.PaymentMethodAdditionalFeeExclTax, true, CurrencyManager.PrimaryStoreCurrency, NopContext.Current.WorkingLanguage, false);
            if (order.PaymentMethodAdditionalFeeInclTax > decimal.Zero)
            {
                if (TaxManager.PaymentMethodAdditionalFeeIsTaxable)
                {
                    if (TaxManager.AllowCustomersToSelectTaxDisplayType)
                    {
                        this.lblPaymentMethodAdditionalFeeInclTax.Text = paymentMethodAdditionalFeeInclTaxStr;
                        this.lblPaymentMethodAdditionalFeeExclTax.Text = paymentMethodAdditionalFeeExclTaxStr;
                        this.pnlPaymentMethodAdditionalFeeInclTax.Visible = true;
                        this.pnlPaymentMethodAdditionalFeeExclTax.Visible = true;
                    }
                    else
                    {
                        switch (TaxManager.TaxDisplayType)
                        {
                            case TaxDisplayTypeEnum.ExcludingTax:
                                {
                                    this.lblPaymentMethodAdditionalFeeExclTax.Text = paymentMethodAdditionalFeeExclTaxStr;
                                    this.pnlPaymentMethodAdditionalFeeInclTax.Visible = false;
                                    this.pnlPaymentMethodAdditionalFeeExclTax.Visible = true;
                                }
                                break;
                            case TaxDisplayTypeEnum.IncludingTax:
                                {
                                    this.lblPaymentMethodAdditionalFeeInclTax.Text = paymentMethodAdditionalFeeInclTaxStr;
                                    this.pnlPaymentMethodAdditionalFeeInclTax.Visible = true;
                                    this.pnlPaymentMethodAdditionalFeeExclTax.Visible = false;
                                }
                                break;
                            default:
                                {
                                    this.pnlPaymentMethodAdditionalFeeInclTax.Visible = false;
                                    this.pnlPaymentMethodAdditionalFeeExclTax.Visible = false;
                                }
                                break;
                        }
                    }
                }
                else
                {
                    switch (TaxManager.TaxDisplayType)
                    {
                        case TaxDisplayTypeEnum.ExcludingTax:
                            {
                                this.lblPaymentMethodAdditionalFeeExclTax.Text = paymentMethodAdditionalFeeExclTaxStr;
                                this.pnlPaymentMethodAdditionalFeeInclTax.Visible = false;
                                this.pnlPaymentMethodAdditionalFeeExclTax.Visible = true;
                            }
                            break;
                        case TaxDisplayTypeEnum.IncludingTax:
                            {
                                this.lblPaymentMethodAdditionalFeeInclTax.Text = paymentMethodAdditionalFeeInclTaxStr;
                                this.pnlPaymentMethodAdditionalFeeInclTax.Visible = true;
                                this.pnlPaymentMethodAdditionalFeeExclTax.Visible = false;
                            }
                            break;
                        default:
                            {
                                this.pnlPaymentMethodAdditionalFeeInclTax.Visible = false;
                                this.pnlPaymentMethodAdditionalFeeExclTax.Visible = false;
                            }
                            break;
                    }
                }
            }
            else
            {
                this.pnlPaymentMethodAdditionalFeeInclTax.Visible = false;
                this.pnlPaymentMethodAdditionalFeeExclTax.Visible = false;
            }

            //discount
            if (order.OrderDiscount > 0)
            {
                pnlDiscount.Visible = true;
                this.lblOrderDiscount.Text = PriceHelper.FormatPrice(-order.OrderDiscount, true, false);
            }
            else
            {
                pnlDiscount.Visible = false;
            }
            
            //gift cards
            GiftCardUsageHistoryCollection gcuhC = OrderManager.GetAllGiftCardUsageHistoryEntries(null, null, order.OrderId);
            if (gcuhC.Count > 0)
            {
                rptrGiftCards.Visible = true;
                rptrGiftCards.DataSource = gcuhC;
                rptrGiftCards.DataBind();
            }
            else
            {
                rptrGiftCards.Visible = false;
            }
            
            //tax
            this.lblOrderTax.Text = PriceHelper.FormatPrice(order.OrderTax, true, false);

            //reward points
            if (order.RedeemedRewardPoints!=null)
            {
                pnlRewardPoints.Visible = true;
                lblRewardPointsTitle.Text = string.Format(GetLocaleResourceString("Admin.OrderDetails.RewardPoints"), -order.RedeemedRewardPoints.Points);
                lblRewardPointsAmount.Text = PriceHelper.FormatPrice(-order.RedeemedRewardPoints.UsedAmount, true, false);
            }
            else
            {
                pnlRewardPoints.Visible = false;
            }

            //total
            this.lblOrderTotal.Text = PriceHelper.FormatPrice(order.OrderTotal, true, false);
        }

        private void BindOrderNotes()
        {
            Order order = OrderManager.GetOrderById(this.OrderId);
            BindOrderNotes(order);
        }

        private void BindOrderNotes(Order order)
        {
            if (order == null)
                return;

            OrderNoteCollection orderNotes = order.OrderNotes;
            this.gvOrderNotes.DataSource = orderNotes;
            this.gvOrderNotes.DataBind();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                this.BindData();
            }
        }

        protected void rptrGiftCards_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.AlternatingItem || e.Item.ItemType == ListItemType.Item)
            {
                var giftCardUsageHistory = e.Item.DataItem as GiftCardUsageHistory;

                ToolTipLabelControl lblOrderGiftCardTitle = e.Item.FindControl("lblOrderGiftCardTitle") as ToolTipLabelControl;
                lblOrderGiftCardTitle.Text = String.Format(GetLocaleResourceString("Admin.OrderDetails.GiftCardInfo"), Server.HtmlEncode(giftCardUsageHistory.GiftCard.GiftCardCouponCode));

                Label lblGiftCardAmount = e.Item.FindControl("lblGiftCardAmount") as Label;
                lblGiftCardAmount.Text = PriceHelper.FormatPrice(-giftCardUsageHistory.UsedValue, true, false);
            }
        }

        protected void btnSetAsShipped_Click(object sender, EventArgs e)
        {
            try
            {
                OrderManager.Ship(this.OrderId, true);
                BindData();
            }
            catch (Exception exc)
            {
                ProcessException(exc);
            }
        }

        protected void CancelOrderButton_Click(object sender, EventArgs e)
        {
            try
            {
                OrderManager.CancelOrder(this.OrderId, true);
                BindData();
            }
            catch (Exception exc)
            {
                ProcessException(exc);
            }
        }

        protected void btnGetInvoicePDF_Click(object sender, EventArgs e)
        {
            try
            {
                Order order = OrderManager.GetOrderById(this.OrderId);

                string fileName = string.Format("order_{0}_{1}.pdf", order.OrderGuid, DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss"));
                string filePath = string.Format("{0}files\\ExportImport\\{1}", HttpContext.Current.Request.PhysicalApplicationPath, fileName);

                PDFHelper.PrintOrderToPdf(order, NopContext.Current.WorkingLanguage.LanguageId, filePath);
                CommonHelper.WriteResponsePdf(filePath, fileName);
            }
            catch (Exception ex)
            {
                ProcessException(ex);
            }
        }

        protected void DeleteButton_Click(object sender, EventArgs e)
        {
            try
            {
                OrderManager.MarkOrderAsDeleted(this.OrderId);
                Response.Redirect("Orders.aspx");
            }
            catch (Exception exc)
            {
                ProcessException(exc);
            }
        }

        protected void btnCapture_Click(object sender, EventArgs e)
        {
            try
            {
                string error = string.Empty;
                OrderManager.Capture(this.OrderId, ref error);
                if (String.IsNullOrEmpty(error))
                {
                    BindData();
                }
                else
                {
                    lblChangePaymentStatusError.Text = error;
                }
            }
            catch (Exception exc)
            {
                ProcessException(exc);
            }
        }

        protected void btnMarkAsPaid_Click(object sender, EventArgs e)
        {
            try
            {
                OrderManager.MarkOrderAsPaid(this.OrderId);
                BindData();
            }
            catch (Exception exc)
            {
                ProcessException(exc);
            }
        }

        protected void btnRefund_Click(object sender, EventArgs e)
        {
            try
            {
                string error = string.Empty;
                OrderManager.Refund(this.OrderId, ref error);
                if (String.IsNullOrEmpty(error))
                {
                    BindData();
                }
                else
                {
                    lblChangePaymentStatusError.Text = error;
                }
            }
            catch (Exception exc)
            {
                ProcessException(exc);
            }
        }

        protected void btnRefundOffline_Click(object sender, EventArgs e)
        {
            try
            {
                OrderManager.RefundOffline(this.OrderId);
                BindData();
            }
            catch (Exception exc)
            {
                ProcessException(exc);
            }
        }

        protected void btnVoid_Click(object sender, EventArgs e)
        {
            try
            {
                string error = string.Empty;
                OrderManager.Void(this.OrderId, ref error);
                if (String.IsNullOrEmpty(error))
                {
                    BindData();
                }
                else
                {
                    lblChangePaymentStatusError.Text = error;
                }
            }
            catch (Exception exc)
            {
                ProcessException(exc);
            }
        }

        protected void btnVoidOffline_Click(object sender, EventArgs e)
        {
            try
            {
                OrderManager.VoidOffline(this.OrderId);
                BindData();
            }
            catch (Exception exc)
            {
                ProcessException(exc);
            }
        }
        
        protected void btnSetTrackingNumber_Click(object sender, EventArgs e)
        {
            try
            {
                OrderManager.SetOrderTrackingNumber(this.OrderId, txtTrackingNumber.Text);
                BindData();
            }
            catch (Exception exc)
            {
                ProcessException(exc);
            }
        }

        protected void BtnPrintPdfPackagingSlip_OnClick(object sender, EventArgs e)
        {
            try
            {
                Order order = OrderManager.GetOrderById(this.OrderId);
                if(order != null)
                {
                    OrderCollection orderCollection = new OrderCollection();
                    orderCollection.Add(order);

                    string fileName = String.Format("packagingslip_{0}_{1}.pdf", DateTime.Now.ToString("yyyy-MM-dd-HH-mm-ss"), CommonHelper.GenerateRandomDigitCode(4));
                    string filePath = String.Format("{0}files\\{1}", HttpContext.Current.Request.PhysicalApplicationPath, fileName);

                    PDFHelper.PrintPackagingSlipsToPdf(orderCollection, filePath);

                    CommonHelper.WriteResponsePdf(filePath, fileName);
                }
            }
            catch(Exception ex)
            {
                ProcessException(ex);
            }
        }

        protected void BtnBanByCustomerIP_OnClick(object sender, EventArgs e)
        {
            Order order = OrderManager.GetOrderById(this.OrderId);
            if(order != null && !String.IsNullOrEmpty(order.CustomerIP))
            {
                BannedIpAddress banItem = new BannedIpAddress();
                banItem.Address = order.CustomerIP;
                if(!IpBlacklistManager.IsIpAddressBanned(banItem))
                {
                    IpBlacklistManager.InsertBannedIpAddress(order.CustomerIP, String.Empty, DateTime.Now, DateTime.Now);
                }
            }
        }

        protected void btnAddNewOrderNote_Click(object sender, EventArgs e)
        {
            try
            {
                string note = txtNewOrderNote.Text.Trim();
                if (String.IsNullOrEmpty(note))
                    return;

                bool displayToCustomer = cbNewDisplayToCustomer.Checked;

                OrderNote orderNote = OrderManager.InsertOrderNote(this.OrderId, note, displayToCustomer, DateTime.Now);
                BindData();
                txtNewOrderNote.Text = string.Empty;
            }
            catch (Exception exc)
            {
                ProcessException(exc);
            }
        }

        protected void gvOrderNotes_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            int orderNoteId = (int)gvOrderNotes.DataKeys[e.RowIndex]["OrderNoteId"];
            OrderManager.DeleteOrderNote(orderNoteId);
            BindOrderNotes();
        }

        protected void gvOrderProductVariants_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "DownloadActivation")
            {
                //download activation
                int index = Convert.ToInt32(e.CommandArgument);
                GridViewRow row = gvOrderProductVariants.Rows[index];
                HiddenField hfOrderProductVariantId = row.FindControl("hfOrderProductVariantId") as HiddenField;

                int orderProductVariantId = int.Parse(hfOrderProductVariantId.Value);
                OrderProductVariant orderProductVariant = OrderManager.GetOrderProductVariantById(orderProductVariantId);

                if (orderProductVariant != null)
                {
                    orderProductVariant = OrderManager.UpdateOrderProductVariant(orderProductVariant.OrderProductVariantId,
                        orderProductVariant.OrderProductVariantGuid, orderProductVariant.OrderId,
                        orderProductVariant.ProductVariantId,
                        orderProductVariant.UnitPriceInclTax, orderProductVariant.UnitPriceExclTax,
                        orderProductVariant.PriceInclTax, orderProductVariant.PriceExclTax,
                        orderProductVariant.UnitPriceInclTaxInCustomerCurrency, orderProductVariant.UnitPriceExclTaxInCustomerCurrency,
                        orderProductVariant.PriceInclTaxInCustomerCurrency, orderProductVariant.PriceExclTaxInCustomerCurrency,
                        orderProductVariant.AttributeDescription, orderProductVariant.AttributesXml,
                        orderProductVariant.Quantity,
                        orderProductVariant.DiscountAmountInclTax, orderProductVariant.DiscountAmountExclTax,
                        orderProductVariant.DownloadCount, !orderProductVariant.IsDownloadActivated,
                        orderProductVariant.LicenseDownloadId);
                }

            }
            else if (e.CommandName == "RemoveLicenseDownload")
            {
                //remove license download
                int index = Convert.ToInt32(e.CommandArgument);
                GridViewRow row = gvOrderProductVariants.Rows[index];
                HiddenField hfOrderProductVariantId = row.FindControl("hfOrderProductVariantId") as HiddenField;

                int orderProductVariantId = int.Parse(hfOrderProductVariantId.Value);
                OrderProductVariant orderProductVariant = OrderManager.GetOrderProductVariantById(orderProductVariantId);

                if (orderProductVariant != null)
                {
                    orderProductVariant = OrderManager.UpdateOrderProductVariant(orderProductVariant.OrderProductVariantId,
                        orderProductVariant.OrderProductVariantGuid, orderProductVariant.OrderId,
                        orderProductVariant.ProductVariantId,
                        orderProductVariant.UnitPriceInclTax, orderProductVariant.UnitPriceExclTax,
                        orderProductVariant.PriceInclTax, orderProductVariant.PriceExclTax,
                        orderProductVariant.UnitPriceInclTaxInCustomerCurrency, orderProductVariant.UnitPriceExclTaxInCustomerCurrency,
                        orderProductVariant.PriceInclTaxInCustomerCurrency, orderProductVariant.PriceExclTaxInCustomerCurrency,
                        orderProductVariant.AttributeDescription, orderProductVariant.AttributesXml, 
                        orderProductVariant.Quantity,
                        orderProductVariant.DiscountAmountInclTax, orderProductVariant.DiscountAmountExclTax,
                        orderProductVariant.DownloadCount, orderProductVariant.IsDownloadActivated, 0);
                }
            }
            else if (e.CommandName == "UploadLicenseDownload")
            {
                //upload new license
                int index = Convert.ToInt32(e.CommandArgument);
                GridViewRow row = gvOrderProductVariants.Rows[index];
                HiddenField hfOrderProductVariantId = row.FindControl("hfOrderProductVariantId") as HiddenField;

                int orderProductVariantId = int.Parse(hfOrderProductVariantId.Value);
                OrderProductVariant orderProductVariant = OrderManager.GetOrderProductVariantById(orderProductVariantId);


                FileUpload fuLicenseDownload = row.FindControl("fuLicenseDownload") as FileUpload;
                HttpPostedFile licenseDownloadFile = fuLicenseDownload.PostedFile;
                if ((licenseDownloadFile != null) && (!String.IsNullOrEmpty(licenseDownloadFile.FileName)))
                {
                    byte[] licenseDownloadBinary = DownloadManager.GetDownloadBits(licenseDownloadFile.InputStream, licenseDownloadFile.ContentLength);
                    string downloadContentType = licenseDownloadFile.ContentType;
                    string downloadFilename = Path.GetFileNameWithoutExtension(licenseDownloadFile.FileName);
                    string downloadExtension = Path.GetExtension(licenseDownloadFile.FileName);

                    Download licenseDownload = DownloadManager.InsertDownload(false, string.Empty,
                        licenseDownloadBinary, downloadContentType, downloadFilename, downloadExtension, true);

                    if (orderProductVariant != null)
                    {
                        orderProductVariant = OrderManager.UpdateOrderProductVariant(orderProductVariant.OrderProductVariantId,
                            orderProductVariant.OrderProductVariantGuid, orderProductVariant.OrderId,
                            orderProductVariant.ProductVariantId,
                            orderProductVariant.UnitPriceInclTax, orderProductVariant.UnitPriceExclTax,
                            orderProductVariant.PriceInclTax, orderProductVariant.PriceExclTax,
                            orderProductVariant.UnitPriceInclTaxInCustomerCurrency, orderProductVariant.UnitPriceExclTaxInCustomerCurrency,
                            orderProductVariant.PriceInclTaxInCustomerCurrency, orderProductVariant.PriceExclTaxInCustomerCurrency,
                            orderProductVariant.AttributeDescription, orderProductVariant.AttributesXml, 
                            orderProductVariant.Quantity,
                            orderProductVariant.DiscountAmountInclTax, orderProductVariant.DiscountAmountExclTax,
                            orderProductVariant.DownloadCount, orderProductVariant.IsDownloadActivated, licenseDownload.DownloadId);
                    }
                }
            }

            BindData();
        }

        protected void gvOrderProductVariants_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                OrderProductVariant opv = (OrderProductVariant)e.Row.DataItem;
                ProductVariant pv = opv.ProductVariant;
                if (pv == null)
                    return;

                //download
                Panel pnlDownloadActivation = e.Row.FindControl("pnlDownloadActivation") as Panel;
                if (pnlDownloadActivation != null)
                    pnlDownloadActivation.Visible = pv.DownloadActivationType == (int)DownloadActivationTypeEnum.Manually;

                Button btnActivate = e.Row.FindControl("btnActivate") as Button;
                if (btnActivate != null)
                {
                    btnActivate.CommandArgument = e.Row.RowIndex.ToString();
                    if (opv.IsDownloadActivated)
                    {
                        btnActivate.Text = GetLocaleResourceString("Admin.OrderDetails.Products.DeactivateDownload");
                    }
                    else
                    {
                        btnActivate.Text = GetLocaleResourceString("Admin.OrderDetails.Products.ActivateDownload");
                    }
                }
                
                //license
                Panel pnlLicenseDownload = e.Row.FindControl("pnlLicenseDownload") as Panel;
                HyperLink hlLicenseDownload = e.Row.FindControl("hlLicenseDownload") as HyperLink;
                Button btnRemoveLicenseDownload = e.Row.FindControl("btnRemoveLicenseDownload") as Button;
                FileUpload fuLicenseDownload = e.Row.FindControl("fuLicenseDownload") as FileUpload;
                Button btnUploadLicenseDownload = e.Row.FindControl("btnUploadLicenseDownload") as Button;
                if (pnlLicenseDownload != null)
                {
                    if (pv.IsDownload)
                    {
                        pnlLicenseDownload.Visible = true;
                        
                        Download licenseDownload = opv.LicenseDownload;
                        if (licenseDownload != null)
                        {
                            hlLicenseDownload.Visible = true;
                            btnRemoveLicenseDownload.Visible = true;
                            fuLicenseDownload.Visible = false;
                            btnUploadLicenseDownload.Visible = false;

                            hlLicenseDownload.NavigateUrl = DownloadManager.GetAdminDownloadUrl(licenseDownload);
                            btnRemoveLicenseDownload.CommandArgument = e.Row.RowIndex.ToString();
                        }
                        else
                        {
                            hlLicenseDownload.Visible = false;
                            btnRemoveLicenseDownload.Visible = false;
                            fuLicenseDownload.Visible = true;
                            btnUploadLicenseDownload.Visible = true;
                            btnUploadLicenseDownload.CommandArgument = e.Row.RowIndex.ToString();
                        }
                    }
                    else
                    {
                        pnlLicenseDownload.Visible = false;
                    }
                }
            }
        }

        public string GetProductUrl(int productVariantId)
        {
            string result = string.Empty;
            ProductVariant productVariant = ProductManager.GetProductVariantById(productVariantId);
            if (productVariant != null)
                result = "ProductVariantDetails.aspx?ProductVariantID=" + productVariant.ProductVariantId.ToString();
            else
                result = "Not available. Product variant ID=" + productVariant.ProductVariantId.ToString();
            return result;
        }

        public string GetProductVariantName(int productVariantId)
        {
            ProductVariant productVariant = ProductManager.GetProductVariantById(productVariantId);
            if (productVariant != null)
                return productVariant.FullProductName;
            return "Not available. ID=" + productVariantId.ToString();
        }

        public string GetAttributeDescription(OrderProductVariant opv)
        {
            string result = opv.AttributeDescription;
            if (!String.IsNullOrEmpty(result))
                result = "<br />" + result;
            return result;
        }

        public string GetDownloadUrl(OrderProductVariant orderProductVariant)
        {
            string result = string.Empty;
            ProductVariant productVariant = orderProductVariant.ProductVariant;
            if (productVariant != null)
            {
                if (productVariant.IsDownload)
                {
                    Download download = productVariant.Download;
                    if (download != null)
                        result = string.Format("<a href=\"{0}\" >{1}</a>", DownloadManager.GetAdminDownloadUrl(download), GetLocaleResourceString("Admin.OrderDetails.Products.Download"));
                    else
                        result = "Not available anymore";
                }
            }
            else
                result = "Not available. Product variant ID = " + orderProductVariant.ProductVariantId.ToString();
            return result;
        }

        public string GetProductVariantUnitPrice(OrderProductVariant orderProductVariant)
        {
            string result = string.Empty;

            Order order = orderProductVariant.Order;
            if (TaxManager.AllowCustomersToSelectTaxDisplayType)
            {
                string unitPriceInclTaxStr = PriceHelper.FormatPrice(orderProductVariant.UnitPriceInclTax, true, CurrencyManager.PrimaryStoreCurrency, NopContext.Current.WorkingLanguage, true, true);
                string unitPriceExclTaxStr = PriceHelper.FormatPrice(orderProductVariant.UnitPriceExclTax, true, CurrencyManager.PrimaryStoreCurrency, NopContext.Current.WorkingLanguage, false, true);

                result = unitPriceInclTaxStr;
                result += "<br />";
                result += unitPriceExclTaxStr;
            }
            else
            {
                switch (TaxManager.TaxDisplayType)
                {
                    case TaxDisplayTypeEnum.ExcludingTax:
                        {
                            string unitPriceExclTaxStr = PriceHelper.FormatPrice(orderProductVariant.UnitPriceExclTax, true, CurrencyManager.PrimaryStoreCurrency, NopContext.Current.WorkingLanguage, false);
                            result += unitPriceExclTaxStr;
                        }
                        break;
                    case TaxDisplayTypeEnum.IncludingTax:
                        {
                            string unitPriceInclTaxStr = PriceHelper.FormatPrice(orderProductVariant.UnitPriceInclTax, true, CurrencyManager.PrimaryStoreCurrency, NopContext.Current.WorkingLanguage, true);
                            result = unitPriceInclTaxStr;
                        }
                        break;
                    default:
                        break;
                }
            }

            return result;
        }

        public string GetOrderProductVariantDiscountAmount(OrderProductVariant orderProductVariant)
        {
            string result = string.Empty;

            Order order = orderProductVariant.Order;
            if (TaxManager.AllowCustomersToSelectTaxDisplayType)
            {
                string discountAmountInclTaxStr = PriceHelper.FormatPrice(orderProductVariant.DiscountAmountInclTax, true, CurrencyManager.PrimaryStoreCurrency, NopContext.Current.WorkingLanguage, true, true);
                string discountAmountExclTaxStr = PriceHelper.FormatPrice(orderProductVariant.DiscountAmountExclTax, true, CurrencyManager.PrimaryStoreCurrency, NopContext.Current.WorkingLanguage, false, true);

                result = discountAmountInclTaxStr;
                result += "<br />";
                result += discountAmountExclTaxStr;
            }
            else
            {
                switch (TaxManager.TaxDisplayType)
                {
                    case TaxDisplayTypeEnum.ExcludingTax:
                        {
                            string discountAmountExclTaxStr = PriceHelper.FormatPrice(orderProductVariant.DiscountAmountExclTax, true, CurrencyManager.PrimaryStoreCurrency, NopContext.Current.WorkingLanguage, false);
                            result += discountAmountExclTaxStr;
                        }
                        break;
                    case TaxDisplayTypeEnum.IncludingTax:
                        {
                            string discountAmountInclTaxStr = PriceHelper.FormatPrice(orderProductVariant.DiscountAmountInclTax, true, CurrencyManager.PrimaryStoreCurrency, NopContext.Current.WorkingLanguage, true);
                            result = discountAmountInclTaxStr;
                        }
                        break;
                    default:
                        break;
                }
            }

            return result;
        }

        public string GetOrderProductVariantSubTotal(OrderProductVariant orderProductVariant)
        {
            string result = string.Empty;

            Order order = orderProductVariant.Order;
            if (TaxManager.AllowCustomersToSelectTaxDisplayType)
            {
                string subTotalInclTaxStr = PriceHelper.FormatPrice(orderProductVariant.PriceInclTax, true, CurrencyManager.PrimaryStoreCurrency, NopContext.Current.WorkingLanguage, true, true);
                string subTotalExclTaxStr = PriceHelper.FormatPrice(orderProductVariant.PriceExclTax, true, CurrencyManager.PrimaryStoreCurrency, NopContext.Current.WorkingLanguage, false, true);

                result = subTotalInclTaxStr;
                result += "<br />";
                result += subTotalExclTaxStr;
            }
            else
            {
                switch (TaxManager.TaxDisplayType)
                {
                    case TaxDisplayTypeEnum.ExcludingTax:
                        {
                            string subTotalExclTaxStr = PriceHelper.FormatPrice(orderProductVariant.PriceExclTax, true, CurrencyManager.PrimaryStoreCurrency, NopContext.Current.WorkingLanguage, false);
                            result += subTotalExclTaxStr;
                        }
                        break;
                    case TaxDisplayTypeEnum.IncludingTax:
                        {
                            string subTotalInclTaxStr = PriceHelper.FormatPrice(orderProductVariant.PriceInclTax, true, CurrencyManager.PrimaryStoreCurrency, NopContext.Current.WorkingLanguage, true);
                            result = subTotalInclTaxStr;
                        }
                        break;
                    default:
                        break;
                }
            }

            return result;
        }

        public int OrderId
        {
            get
            {
                return CommonHelper.QueryStringInt("OrderId");
            }
        }
    }
}