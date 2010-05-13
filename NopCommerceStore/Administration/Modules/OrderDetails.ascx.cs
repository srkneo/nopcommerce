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

            this.lblBillingFirstName.Text = Server.HtmlEncode(order.BillingFirstName);
            this.txtBillingFirstName.Text = order.BillingFirstName;
            this.lblBillingLastName.Text = Server.HtmlEncode(order.BillingLastName);
            this.txtBillingLastName.Text = order.BillingLastName;
            this.lblBillingPhoneNumber.Text = Server.HtmlEncode(order.BillingPhoneNumber);
            this.txtBillingPhoneNumber.Text = order.BillingPhoneNumber;
            this.lblBillingEmail.Text = Server.HtmlEncode(order.BillingEmail);
            this.txtBillingEmail.Text = order.BillingEmail;
            this.lblBillingFaxNumber.Text = Server.HtmlEncode(order.BillingFaxNumber);
            this.txtBillingFaxNumber.Text = order.BillingFaxNumber;
            this.lblBillingCompany.Text = Server.HtmlEncode(order.BillingCompany);
            this.txtBillingCompany.Text = order.BillingCompany;
            this.lblBillingAddress1.Text = Server.HtmlEncode(order.BillingAddress1);
            this.txtBillingAddress1.Text = order.BillingAddress1;
            this.lblBillingAddress2.Text = Server.HtmlEncode(order.BillingAddress2);
            this.txtBillingAddress2.Text = order.BillingAddress2;
            this.lblBillingCity.Text = Server.HtmlEncode(order.BillingCity);
            this.txtBillingCity.Text = order.BillingCity;
            this.lblBillingCountry.Text = Server.HtmlEncode(order.BillingCountry);
            this.FillBillingCountryDropDowns(order);
            CommonHelper.SelectListItem(this.ddlBillingCountry, order.BillingCountryId);
            this.lblBillingStateProvince.Text = Server.HtmlEncode(order.BillingStateProvince);
            this.FillBillingStateProvinceDropDowns();
            CommonHelper.SelectListItem(this.ddlBillingStateProvince, order.BillingStateProvinceId);
            this.lblBillingZipPostalCode.Text = Server.HtmlEncode(order.BillingZipPostalCode);
            this.txtBillingZipPostalCode.Text = order.BillingZipPostalCode;
        }

        protected void BindShippingInfo(Order order)
        {
            if (order == null)
                return;

            if (order.ShippingStatus != ShippingStatusEnum.ShippingNotRequired)
            {
                this.lblShippingFirstName.Text = Server.HtmlEncode(order.ShippingFirstName);
                this.txtShippingFirstName.Text = order.ShippingFirstName;
                this.lblShippingLastName.Text = Server.HtmlEncode(order.ShippingLastName);
                this.txtShippingLastName.Text = order.ShippingLastName;
                this.lblShippingPhoneNumber.Text = Server.HtmlEncode(order.ShippingPhoneNumber);
                this.txtShippingPhoneNumber.Text = order.ShippingPhoneNumber;
                this.lblShippingEmail.Text = Server.HtmlEncode(order.ShippingEmail);
                this.txtShippingEmail.Text = order.ShippingEmail;
                this.lblShippingFaxNumber.Text = Server.HtmlEncode(order.ShippingFaxNumber);
                this.txtShippingFaxNumber.Text = order.ShippingFaxNumber;
                this.lblShippingCompany.Text = Server.HtmlEncode(order.ShippingCompany);
                this.txtShippingCompany.Text = order.ShippingCompany;
                this.lblShippingAddress1.Text = Server.HtmlEncode(order.ShippingAddress1);
                this.txtShippingAddress1.Text = order.ShippingAddress1;
                this.lblShippingAddress2.Text = Server.HtmlEncode(order.ShippingAddress2);
                this.txtShippingAddress2.Text = order.ShippingAddress2;
                this.lblShippingCity.Text = Server.HtmlEncode(order.ShippingCity);
                this.txtShippingCity.Text = order.ShippingCity;
                this.lblShippingCountry.Text = Server.HtmlEncode(order.ShippingCountry);
                this.FillShippingCountryDropDowns(order);
                CommonHelper.SelectListItem(this.ddlShippingCountry, order.ShippingCountryId);
                this.lblShippingStateProvince.Text = Server.HtmlEncode(order.ShippingStateProvince);
                this.FillShippingStateProvinceDropDowns();
                CommonHelper.SelectListItem(this.ddlShippingStateProvince, order.ShippingStateProvinceId);
                this.lblShippingZipPostalCode.Text = Server.HtmlEncode(order.ShippingZipPostalCode);
                this.txtShippingZipPostalCode.Text = order.ShippingZipPostalCode;

                this.hlShippingAddressGoogle.NavigateUrl = string.Format("http://maps.google.com/maps?f=q&hl=en&ie=UTF8&oe=UTF8&geocode=&q={0}", Server.UrlEncode(order.ShippingAddress1 + " " + order.ShippingZipPostalCode + " " + order.ShippingCity + " " + order.ShippingCountry));

                this.lblShippingMethod.Text = Server.HtmlEncode(order.ShippingMethod);

                this.txtTrackingNumber.Text = order.TrackingNumber;

                this.btnSetAsShipped.Visible = OrderManager.CanShip(order);
                if (order.ShippedDate.HasValue)
                {
                    this.lblShippedDate.Text = DateTimeHelper.ConvertToUserTime(order.ShippedDate.Value).ToString();
                }
                else
                {
                    this.lblShippedDate.Text = GetLocaleResourceString("Admin.OrderDetails.ShippedDate.NotYet");
                }

                this.btnSetAsDelivered.Visible = OrderManager.CanDeliver(order);
                if (order.DeliveryDate.HasValue)
                {
                    this.lblDeliveryDate.Text = DateTimeHelper.ConvertToUserTime(order.DeliveryDate.Value).ToString();
                }
                else
                {
                    this.lblDeliveryDate.Text = GetLocaleResourceString("Admin.OrderDetails.DeliveryDate.NotYet");
                }

                this.lblOrderWeight.Text = string.Format("{0} [{1}]", order.OrderWeight, MeasureManager.BaseWeightIn.Name);

                this.divShippingNotRequired.Visible = false;
                this.divShippingAddress.Visible = true;
                this.divShippingWeight.Visible = true;
                this.divShippingMethod.Visible = true;
                this.divTrackingNumber.Visible = true;
                this.divShippedDate.Visible = true;
                this.divDeliveryDate.Visible = true;
            }
            else
            {
                this.divShippingNotRequired.Visible = true;
                this.divShippingAddress.Visible = false;
                this.divShippingWeight.Visible = false;
                this.divShippingMethod.Visible = false;
                this.divTrackingNumber.Visible = false;
                this.divShippedDate.Visible = false;
                this.divDeliveryDate.Visible = false;
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
                this.SelectTab(this.OrderTabs, this.TabId);
            }
        }
        
        protected override void OnPreRender(EventArgs e)
        {
            BindJQuery();
            
            this.btnEditBillingAddress.Attributes.Add("onclick", "toggleBillingAddress(true);return false;");
            this.btnCancelBillingAddress.Attributes.Add("onclick", "toggleBillingAddress(false);return false;");
            this.btnEditShippingAddress.Attributes.Add("onclick", "toggleShippingAddress(true);return false;");
            this.btnCancelShippingAddress.Attributes.Add("onclick", "toggleShippingAddress(false);return false;");
            
            base.OnPreRender(e);
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

        protected void btnSetAsDelivered_Click(object sender, EventArgs e)
        {
            try
            {
                OrderManager.Deliver(this.OrderId, true);
                BindData();
            }
            catch (Exception exc)
            {
                ProcessException(exc);
            }
        }

        protected void btnSaveBillingAddress_Click(object sender, EventArgs e)
        {
            try
            {
                var order = OrderManager.GetOrderById(this.OrderId);
                if (order != null)
                {
                    string billingFirstName = txtBillingFirstName.Text;
                    string billingLastName = this.txtBillingLastName.Text;
                    string billingPhoneNumber = this.txtBillingPhoneNumber.Text;
                    string billingEmail = this.txtBillingEmail.Text;
                    string billingFaxNumber = this.txtBillingFaxNumber.Text;
                    string billingCompany = this.txtBillingCompany.Text;
                    string billingAddress1 = this.txtBillingAddress1.Text;
                    string billingAddress2 = this.txtBillingAddress2.Text;
                    string billingCity = this.txtBillingCity.Text;
                    int billingCountryId = int.Parse(ddlBillingCountry.SelectedItem.Value);
                    string billingCountryStr = string.Empty;
                    var billingCountry = CountryManager.GetCountryById(billingCountryId);
                    if (billingCountry != null)
                    {
                        billingCountryStr = billingCountry.Name;
                    }
                    int billingStateProvinceId = int.Parse(ddlBillingStateProvince.SelectedItem.Value);
                    string billingStateProvinceStr = string.Empty;
                    var billingStateProvince = StateProvinceManager.GetStateProvinceById(billingStateProvinceId);
                    if (billingStateProvince != null)
                    {
                        billingStateProvinceStr = billingStateProvince.Name;
                    }
                    string billingZipPostalCode = this.txtBillingZipPostalCode.Text;

                    order = OrderManager.UpdateOrder(order.OrderId, order.OrderGuid, order.CustomerId, order.CustomerLanguageId,
                        order.CustomerTaxDisplayType, order.CustomerIP, order.OrderSubtotalInclTax, order.OrderSubtotalExclTax, order.OrderShippingInclTax,
                       order.OrderShippingExclTax, order.PaymentMethodAdditionalFeeInclTax, order.PaymentMethodAdditionalFeeExclTax,
                       order.OrderTax, order.OrderTotal, order.OrderDiscount,
                       order.OrderSubtotalInclTaxInCustomerCurrency, order.OrderSubtotalExclTaxInCustomerCurrency,
                       order.OrderShippingInclTaxInCustomerCurrency, order.OrderShippingExclTaxInCustomerCurrency,
                       order.PaymentMethodAdditionalFeeInclTaxInCustomerCurrency, order.PaymentMethodAdditionalFeeExclTaxInCustomerCurrency,
                       order.OrderTaxInCustomerCurrency, order.OrderTotalInCustomerCurrency,
                       order.OrderDiscountInCustomerCurrency,
                       order.CheckoutAttributeDescription, order.CheckoutAttributesXml,
                       order.CustomerCurrencyCode, order.OrderWeight,
                       order.AffiliateId, order.OrderStatus, order.AllowStoringCreditCardNumber, order.CardType,
                       order.CardName, order.CardNumber, order.MaskedCreditCardNumber,
                        order.CardCvv2, order.CardExpirationMonth, order.CardExpirationYear,
                        order.PaymentMethodId, order.PaymentMethodName, order.AuthorizationTransactionId,
                        order.AuthorizationTransactionCode, order.AuthorizationTransactionResult,
                        order.CaptureTransactionId, order.CaptureTransactionResult,
                        order.SubscriptionTransactionId, order.PurchaseOrderNumber,
                        order.PaymentStatus, order.PaidDate,
                        billingFirstName, billingLastName, billingPhoneNumber,
                        billingEmail, billingFaxNumber, billingCompany, billingAddress1,
                        billingAddress2, billingCity, billingStateProvinceStr,
                        billingStateProvinceId, billingZipPostalCode, billingCountryStr,
                        billingCountryId, order.ShippingStatus,
                        order.ShippingFirstName, order.ShippingLastName, order.ShippingPhoneNumber,
                        order.ShippingEmail, order.ShippingFaxNumber, order.ShippingCompany,
                        order.ShippingAddress1, order.ShippingAddress2, order.ShippingCity,
                        order.ShippingStateProvince, order.ShippingStateProvinceId, order.ShippingZipPostalCode,
                        order.ShippingCountry, order.ShippingCountryId,
                        order.ShippingMethod, order.ShippingRateComputationMethodId,
                        order.ShippedDate, order.DeliveryDate, order.TrackingNumber, order.Deleted,
                        order.CreatedOn);
                }

                string url = string.Format("{0}OrderDetails.aspx?OrderID={1}&TabID={2}", CommonHelper.GetStoreAdminLocation(), this.OrderId, this.GetActiveTabId(this.OrderTabs));
                Response.Redirect(url);
            }
            catch (Exception exc)
            {
                ProcessException(exc);
            }
        }

        protected void btnSaveShippingAddress_Click(object sender, EventArgs e)
        {
            try
            {
                var order = OrderManager.GetOrderById(this.OrderId);
                if (order != null)
                {
                    string shippingFirstName = txtShippingFirstName.Text;
                    string shippingLastName = this.txtShippingLastName.Text;
                    string shippingPhoneNumber = this.txtShippingPhoneNumber.Text;
                    string shippingEmail = this.txtShippingEmail.Text;
                    string shippingFaxNumber = this.txtShippingFaxNumber.Text;
                    string shippingCompany = this.txtShippingCompany.Text;
                    string shippingAddress1 = this.txtShippingAddress1.Text;
                    string shippingAddress2 = this.txtShippingAddress2.Text;
                    string shippingCity = this.txtShippingCity.Text;
                    int shippingCountryId = int.Parse(ddlShippingCountry.SelectedItem.Value);
                    string shippingCountryStr = string.Empty;
                    var shippingCountry = CountryManager.GetCountryById(shippingCountryId);
                    if (shippingCountry != null)
                    {
                        shippingCountryStr = shippingCountry.Name;
                    }
                    int shippingStateProvinceId = int.Parse(ddlShippingStateProvince.SelectedItem.Value);
                    string shippingStateProvinceStr = string.Empty;
                    var shippingStateProvince = StateProvinceManager.GetStateProvinceById(shippingStateProvinceId);
                    if (shippingStateProvince != null)
                    {
                        shippingStateProvinceStr = shippingStateProvince.Name;
                    }
                    string shippingZipPostalCode = this.txtShippingZipPostalCode.Text;

                    order = OrderManager.UpdateOrder(order.OrderId, order.OrderGuid, order.CustomerId, order.CustomerLanguageId,
                        order.CustomerTaxDisplayType, order.CustomerIP, order.OrderSubtotalInclTax, order.OrderSubtotalExclTax, order.OrderShippingInclTax,
                       order.OrderShippingExclTax, order.PaymentMethodAdditionalFeeInclTax, order.PaymentMethodAdditionalFeeExclTax,
                       order.OrderTax, order.OrderTotal, order.OrderDiscount,
                       order.OrderSubtotalInclTaxInCustomerCurrency, order.OrderSubtotalExclTaxInCustomerCurrency,
                       order.OrderShippingInclTaxInCustomerCurrency, order.OrderShippingExclTaxInCustomerCurrency,
                       order.PaymentMethodAdditionalFeeInclTaxInCustomerCurrency, order.PaymentMethodAdditionalFeeExclTaxInCustomerCurrency,
                       order.OrderTaxInCustomerCurrency, order.OrderTotalInCustomerCurrency,
                       order.OrderDiscountInCustomerCurrency,
                       order.CheckoutAttributeDescription, order.CheckoutAttributesXml,
                       order.CustomerCurrencyCode, order.OrderWeight,
                       order.AffiliateId, order.OrderStatus, order.AllowStoringCreditCardNumber, order.CardType,
                       order.CardName, order.CardNumber, order.MaskedCreditCardNumber,
                        order.CardCvv2, order.CardExpirationMonth, order.CardExpirationYear,
                        order.PaymentMethodId, order.PaymentMethodName, order.AuthorizationTransactionId,
                        order.AuthorizationTransactionCode, order.AuthorizationTransactionResult,
                        order.CaptureTransactionId, order.CaptureTransactionResult,
                        order.SubscriptionTransactionId, order.PurchaseOrderNumber,
                        order.PaymentStatus, order.PaidDate,
                        order.BillingFirstName, order.BillingLastName, order.BillingPhoneNumber,
                        order.BillingEmail, order.BillingFaxNumber, order.BillingCompany, order.BillingAddress1,
                        order.BillingAddress2, order.BillingCity, order.BillingStateProvince,
                        order.BillingStateProvinceId, order.BillingZipPostalCode, order.BillingCountry,
                        order.BillingCountryId, order.ShippingStatus,
                        shippingFirstName, shippingLastName, shippingPhoneNumber,
                        shippingEmail, shippingFaxNumber, shippingCompany,
                        shippingAddress1, shippingAddress2, shippingCity,
                        shippingStateProvinceStr, shippingStateProvinceId, shippingZipPostalCode,
                        shippingCountryStr, shippingCountryId,
                        order.ShippingMethod, order.ShippingRateComputationMethodId,
                        order.ShippedDate, order.DeliveryDate, order.TrackingNumber, order.Deleted,
                        order.CreatedOn);
                }

                string url = string.Format("{0}OrderDetails.aspx?OrderID={1}&TabID={2}", CommonHelper.GetStoreAdminLocation(), this.OrderId, this.GetActiveTabId(this.OrderTabs));
                Response.Redirect(url);
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

        protected void FillBillingCountryDropDowns(Order order)
        {
            this.ddlBillingCountry.Items.Clear();
            var countryCollection = CountryManager.GetAllCountriesForBilling();
            foreach (var country in countryCollection)
            {
                ListItem ddlCountryItem2 = new ListItem(country.Name, country.CountryId.ToString());
                this.ddlBillingCountry.Items.Add(ddlCountryItem2);
            }
        }

        protected void FillBillingStateProvinceDropDowns()
        {
            this.ddlBillingStateProvince.Items.Clear();
            int countryId = int.Parse(this.ddlBillingCountry.SelectedItem.Value);

            var stateProvinces = StateProvinceManager.GetStateProvincesByCountryId(countryId);
            foreach (StateProvince stateProvince in stateProvinces)
            {
                ListItem ddlStateProviceItem2 = new ListItem(stateProvince.Name, stateProvince.StateProvinceId.ToString());
                this.ddlBillingStateProvince.Items.Add(ddlStateProviceItem2);
            }
            if (stateProvinces.Count == 0)
            {
                ListItem ddlBillingStateProvinceItem = new ListItem(GetLocaleResourceString("Admin.Common.State.Other"), "0");
                this.ddlBillingStateProvince.Items.Add(ddlBillingStateProvinceItem);
            }
        }

        protected void ddlBillingCountry_SelectedIndexChanged(object sender, EventArgs e)
        {
            FillBillingStateProvinceDropDowns();
        }

        protected void FillShippingCountryDropDowns(Order order)
        {
            this.ddlShippingCountry.Items.Clear();
            var countryCollection = CountryManager.GetAllCountriesForShipping();
            foreach (var country in countryCollection)
            {
                ListItem ddlCountryItem2 = new ListItem(country.Name, country.CountryId.ToString());
                this.ddlShippingCountry.Items.Add(ddlCountryItem2);
            }
        }

        protected void FillShippingStateProvinceDropDowns()
        {
            this.ddlShippingStateProvince.Items.Clear();
            int countryId = int.Parse(this.ddlShippingCountry.SelectedItem.Value);

            var stateProvinces = StateProvinceManager.GetStateProvincesByCountryId(countryId);
            foreach (StateProvince stateProvince in stateProvinces)
            {
                ListItem ddlStateProviceItem2 = new ListItem(stateProvince.Name, stateProvince.StateProvinceId.ToString());
                this.ddlShippingStateProvince.Items.Add(ddlStateProviceItem2);
            }
            if (stateProvinces.Count == 0)
            {
                ListItem ddlShippingStateProvinceItem = new ListItem(GetLocaleResourceString("Admin.Common.State.Other"), "0");
                this.ddlShippingStateProvince.Items.Add(ddlShippingStateProvinceItem);
            }
        }

        protected void ddlShippingCountry_SelectedIndexChanged(object sender, EventArgs e)
        {
            FillShippingStateProvinceDropDowns();
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

        public string GetRecurringDescription(OrderProductVariant opv)
        {
            string result = string.Empty;
            if (opv.ProductVariant.IsRecurring)
            {
                result = string.Format(GetLocaleResourceString("Admin.OrderDetails.Products.RecurringPeriod"), opv.ProductVariant.CycleLength, ((RecurringProductCyclePeriodEnum)opv.ProductVariant.CyclePeriod).ToString());
                if (!String.IsNullOrEmpty(result))
                    result = "<br />" + result;
            }
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

        protected string TabId
        {
            get
            {
                return CommonHelper.QueryString("TabId");
            }
        }
    }
}