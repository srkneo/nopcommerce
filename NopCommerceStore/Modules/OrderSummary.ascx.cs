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
using System.Collections.Generic;
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Text;
using System.Web;
using System.Web.Caching;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using NopSolutions.NopCommerce.BusinessLogic;
using NopSolutions.NopCommerce.BusinessLogic.Configuration.Settings;
using NopSolutions.NopCommerce.BusinessLogic.CustomerManagement;
using NopSolutions.NopCommerce.BusinessLogic.Directory;
using NopSolutions.NopCommerce.BusinessLogic.Localization;
using NopSolutions.NopCommerce.BusinessLogic.Orders;
using NopSolutions.NopCommerce.BusinessLogic.Payment;
using NopSolutions.NopCommerce.BusinessLogic.Products;
using NopSolutions.NopCommerce.BusinessLogic.Products.Attributes;
using NopSolutions.NopCommerce.BusinessLogic.Promo.Discounts;
using NopSolutions.NopCommerce.BusinessLogic.SEO;
using NopSolutions.NopCommerce.BusinessLogic.Shipping;
using NopSolutions.NopCommerce.BusinessLogic.Tax;
using NopSolutions.NopCommerce.Common.Utils;
using NopSolutions.NopCommerce.BusinessLogic.Media;

namespace NopSolutions.NopCommerce.Web.Modules
{
    public partial class OrderSummaryControl : BaseNopUserControl
    {
        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);
            this.BindData();
        }

        public void BindData()
        {
            var Cart = ShoppingCartManager.GetCurrentShoppingCart(ShoppingCartTypeEnum.ShoppingCart);

            if (Cart.Count > 0)
            {
                pnlEmptyCart.Visible = false;
                pnlCart.Visible = true;

                rptShoppingCart.DataSource = Cart;
                rptShoppingCart.DataBind();
                ValidateShoppingCart();
                ValidateCartItems();
            }
            else
            {
                pnlEmptyCart.Visible = true;
                pnlCart.Visible = false;
            }

            this.ctrlOrderTotals.BindData();
        }

        /// <summary>
        /// Validates shopping cart
        /// </summary>
        /// <returns>Indicates whether there're some warnings/errors</returns>
        protected bool ValidateShoppingCart()
        {
            bool hasErrors = false;

            //shopping cart
            var Cart = ShoppingCartManager.GetCurrentShoppingCart(ShoppingCartTypeEnum.ShoppingCart);
            var warnings = ShoppingCartManager.GetShoppingCartWarnings(Cart);
            if (warnings.Count > 0)
            {
                hasErrors = true;
                pnlCommonWarnings.Visible = true;
                lblCommonWarning.Visible = true;

                var scWarningsSb = new StringBuilder();
                for (int i = 0; i < warnings.Count; i++)
                {
                    scWarningsSb.Append(Server.HtmlEncode(warnings[i]));
                    if (i != warnings.Count - 1)
                    {
                        scWarningsSb.Append("<br />");
                    }
                }

                lblCommonWarning.Text = scWarningsSb.ToString();
            }
            else
            {
                pnlCommonWarnings.Visible = false;
                lblCommonWarning.Visible = false;
            }

            return hasErrors;
        }

        /// <summary>
        /// Validates shopping cart items
        /// </summary>
        /// <returns>Indicates whether there're some warnings/errors</returns>
        protected bool ValidateCartItems()
        {
            bool hasErrors = false;

            //individual items
            foreach (RepeaterItem item in rptShoppingCart.Items)
            {
                var txtQuantity = item.FindControl("txtQuantity") as TextBox;
                var lblShoppingCartItemID = item.FindControl("lblShoppingCartItemID") as Label;
                var cbRemoveFromCart = item.FindControl("cbRemoveFromCart") as CheckBox;
                var pnlWarnings = item.FindControl("pnlWarnings") as Panel;
                var lblWarning = item.FindControl("lblWarning") as Label;

                int shoppingCartItemID = 0;
                int quantity = 0;
                if (txtQuantity != null && lblShoppingCartItemID != null && cbRemoveFromCart != null)
                {
                    int.TryParse(lblShoppingCartItemID.Text, out shoppingCartItemID);

                    if (!cbRemoveFromCart.Checked)
                    {
                        int.TryParse(txtQuantity.Text, out quantity);
                        var sci = ShoppingCartManager.GetShoppingCartItemByID(shoppingCartItemID);

                        var warnings = ShoppingCartManager.GetShoppingCartItemWarnings(
                            sci.ShoppingCartType,
                            sci.ProductVariantID,
                            sci.AttributesXML,
                            sci.CustomerEnteredPrice,
                            quantity);

                        if (warnings.Count > 0)
                        {
                            hasErrors = true;
                            if (pnlWarnings != null && lblWarning != null)
                            {
                                pnlWarnings.Visible = true;
                                lblWarning.Visible = true;

                                var addToCartWarningsSb = new StringBuilder();
                                for (int i = 0; i < warnings.Count; i++)
                                {
                                    addToCartWarningsSb.Append(Server.HtmlEncode(warnings[i]));
                                    if (i != warnings.Count - 1)
                                    {
                                        addToCartWarningsSb.Append("<br />");
                                    }
                                }

                                lblWarning.Text = addToCartWarningsSb.ToString();
                            }
                        }
                    }
                }
            }
            return hasErrors;
        }

        protected void UpdateShoppingCart()
        {
            if (!IsShoppingCart)
                return;

            bool hasErrors = ValidateCartItems();

            if (!hasErrors)
            {
                foreach (RepeaterItem item in rptShoppingCart.Items)
                {
                    var txtQuantity = item.FindControl("txtQuantity") as TextBox;
                    var lblShoppingCartItemID = item.FindControl("lblShoppingCartItemID") as Label;
                    var cbRemoveFromCart = item.FindControl("cbRemoveFromCart") as CheckBox;
                    int shoppingCartItemID = 0;
                    int quantity = 0;
                    if (txtQuantity != null && lblShoppingCartItemID != null && cbRemoveFromCart != null)
                    {
                        int.TryParse(lblShoppingCartItemID.Text, out shoppingCartItemID);
                        if (cbRemoveFromCart.Checked)
                        {
                            ShoppingCartManager.DeleteShoppingCartItem(shoppingCartItemID, true);
                        }
                        else
                        {
                            int.TryParse(txtQuantity.Text, out quantity);
                            List<string> addToCartWarning = ShoppingCartManager.UpdateCart(shoppingCartItemID, quantity, true);
                        }
                    }
                }

                Response.Redirect("~/shoppingcart.aspx");
            }
        }

        protected void ContinueShopping()
        {
            string lastProductPageVisited = NopContext.Current.LastProductPageVisited;
            if (!String.IsNullOrEmpty(lastProductPageVisited))
                Response.Redirect(lastProductPageVisited);
            else
                Response.Redirect(CommonHelper.GetStoreLocation());
        }

        protected void Checkout()
        {
            if (NopContext.Current.User == null || NopContext.Current.User.IsGuest)
            {
                string loginURL = SEOHelper.GetLoginPageURL(true, true);
                Response.Redirect(loginURL);
            }
            else
            {
                Response.Redirect("~/checkout.aspx");
            }
        }

        protected void ApplyDiscountCouponCode()
        {
            string couponCode = this.txtDiscountCouponCode.Text.Trim();
            if (String.IsNullOrEmpty(couponCode))
                return;

            bool isDiscountValid = true;
            if (isDiscountValid)
            {
                pnlDiscountWarnings.Visible = false;
                lblDiscountWarning.Visible = false;

                CustomerManager.ApplyDiscountCouponCode(couponCode);
                this.BindData();
            }
            else
            {
                pnlDiscountWarnings.Visible = true;
                lblDiscountWarning.Visible = true;
                lblDiscountWarning.Text = GetLocaleResourceString("ShoppingCart.DiscountCouponCode.WrongDiscount");
            }
        }

        protected void ApplyGiftCardCouponCode()
        {
            string couponCode = this.txtGiftCardCouponCode.Text.Trim();
            if (String.IsNullOrEmpty(couponCode))
                return;

            bool isGiftCardValid = GiftCardHelper.IsGiftCardValid(couponCode);
            if (isGiftCardValid)
            {
                pnlGiftCardWarnings.Visible = false;
                lblGiftCardWarning.Visible = false;

                string couponCodesXML = string.Empty;
                if (NopContext.Current.User != null)
                    couponCodesXML = NopContext.Current.User.GiftCardCouponCodes;
                couponCodesXML = GiftCardHelper.AddCouponCode(couponCodesXML, couponCode);
                CustomerManager.ApplyGiftCardCouponCode(couponCodesXML);
                this.BindData();
            }
            else
            {
                pnlGiftCardWarnings.Visible = true;
                lblGiftCardWarning.Visible = true;
                lblGiftCardWarning.Text = GetLocaleResourceString("ShoppingCart.GiftCards.WrongGiftCard");
            }
        }
        
        public string GetProductVariantName(ShoppingCartItem shoppingCartItem)
        {
            var productVariant = shoppingCartItem.ProductVariant;
            if (productVariant != null)
                return productVariant.FullProductName;
            return "Not available";
        }

        public string GetProductVariantImageUrl(ShoppingCartItem shoppingCartItem)
        {
            string pictureUrl = String.Empty;
            var productVariant = shoppingCartItem.ProductVariant;
            if (productVariant != null)
            {
                var productVariantPicture = productVariant.Picture;
                pictureUrl = PictureManager.GetPictureUrl(productVariantPicture, SettingManager.GetSettingValueInteger("Media.ShoppingCart.ThumbnailImageSize", 80), false);
                if (String.IsNullOrEmpty(pictureUrl))
                {
                    var product = productVariant.Product;
                    var productPictures = product.ProductPictures;
                    if (productPictures.Count > 0)
                    {
                        pictureUrl = PictureManager.GetPictureUrl(productPictures[0].PictureID, SettingManager.GetSettingValueInteger("Media.ShoppingCart.ThumbnailImageSize", 80));
                    }
                    else
                    {
                        pictureUrl = PictureManager.GetDefaultPictureUrl(SettingManager.GetSettingValueInteger("Media.ShoppingCart.ThumbnailImageSize", 80));
                    }
                }
            }
            return pictureUrl;
         }

        public string GetProductURL(ShoppingCartItem shoppingCartItem)
        {
            var productVariant = shoppingCartItem.ProductVariant;
            if (productVariant != null)
                return SEOHelper.GetProductURL(productVariant.ProductID);
            return string.Empty;
        }

        public string GetAttributeDescription(ShoppingCartItem shoppingCartItem)
        {
            string result = ProductAttributeHelper.FormatAttributes(shoppingCartItem.ProductVariant, shoppingCartItem.AttributesXML);
            if (!String.IsNullOrEmpty(result))
                result = "<br />" + result;
            return result;
        }
        
        public string GetShoppingCartItemUnitPriceString(ShoppingCartItem shoppingCartItem)
        {
            var sb = new StringBuilder();
            decimal shoppingCartUnitPriceWithDiscountBase = TaxManager.GetPrice(shoppingCartItem.ProductVariant, PriceHelper.GetUnitPrice(shoppingCartItem, true));
            decimal shoppingCartUnitPriceWithDiscount = CurrencyManager.ConvertCurrency(shoppingCartUnitPriceWithDiscountBase, CurrencyManager.PrimaryStoreCurrency, NopContext.Current.WorkingCurrency);
            string unitPriceString = PriceHelper.FormatPrice(shoppingCartUnitPriceWithDiscount);
            
            sb.Append("<span class=\"productPrice\">");
            sb.Append(unitPriceString);
            sb.Append("</span>");
            return sb.ToString();
        }
        
        public string GetShoppingCartItemSubTotalString(ShoppingCartItem shoppingCartItem)
        {
            var sb = new StringBuilder();
            decimal shoppingCartItemSubTotalWithDiscountBase = TaxManager.GetPrice(shoppingCartItem.ProductVariant, PriceHelper.GetSubTotal(shoppingCartItem, true));
            decimal shoppingCartItemSubTotalWithDiscount = CurrencyManager.ConvertCurrency(shoppingCartItemSubTotalWithDiscountBase, CurrencyManager.PrimaryStoreCurrency, NopContext.Current.WorkingCurrency);
            string subTotalString = PriceHelper.FormatPrice(shoppingCartItemSubTotalWithDiscount);
            
            sb.Append("<span class=\"productPrice\">");
            sb.Append(subTotalString);
            sb.Append("</span>");

            decimal shoppingCartItemDiscountBase = TaxManager.GetPrice(shoppingCartItem.ProductVariant, PriceHelper.GetDiscountAmount(shoppingCartItem));
            if (shoppingCartItemDiscountBase > decimal.Zero)
            {
                decimal shoppingCartItemDiscount = CurrencyManager.ConvertCurrency(shoppingCartItemDiscountBase, CurrencyManager.PrimaryStoreCurrency, NopContext.Current.WorkingCurrency);
                string discountString = PriceHelper.FormatPrice(shoppingCartItemDiscount);

                sb.Append("<br />");
                sb.Append(GetLocaleResourceString("ShoppingCart.ItemYouSave"));
                sb.Append("&nbsp;&nbsp;");
                sb.Append(discountString);
            }
            return sb.ToString();
        }
        
        protected void btnUpdate_Click(object sender, EventArgs e)
        {
            UpdateShoppingCart();
        }

        protected void btnContinueShopping_Click(object sender, EventArgs e)
        {
            ContinueShopping();
        }

        protected void btnCheckout_Click(object sender, EventArgs e)
        {
            Checkout();
        }

        protected void btnApplyDiscountCouponCode_Click(object sender, EventArgs e)
        {
            ApplyDiscountCouponCode();
        }

        protected void btnApplyGiftCardCouponCode_Click(object sender, EventArgs e)
        {
            ApplyGiftCardCouponCode();
        }
        
        [DefaultValue(false)]
        public bool IsShoppingCart
        {
            get
            {
                object obj2 = this.ViewState["IsShoppingCart"];
                return ((obj2 != null) && ((bool)obj2));
            }
            set
            {
                this.ViewState["IsShoppingCart"] = value;
            }
        }
    }
}