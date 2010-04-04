using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using NopSolutions.NopCommerce.BusinessLogic.Products;
using NopSolutions.NopCommerce.Common.Utils;
using NopSolutions.NopCommerce.BusinessLogic.Audit;
using NopSolutions.NopCommerce.BusinessLogic.Products.Attributes;
using NopSolutions.NopCommerce.BusinessLogic.Orders;
using System.Text;
using NopSolutions.NopCommerce.Web.Modules;
using NopSolutions.NopCommerce.BusinessLogic.Configuration.Settings;
using NopSolutions.NopCommerce.BusinessLogic;
using NopSolutions.NopCommerce.BusinessLogic.Media;

namespace NopSolutions.NopCommerce.Web.Templates.Products
{
    public partial class OneVariant : BaseNopUserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if(!Page.IsPostBack)
            {
                BindData();
            }
        }
        protected void BindData()
        {
            var product = ProductManager.GetProductByID(ProductID);
            if(product == null || product.ProductVariants.Count == 0)
            {
                Response.Redirect(CommonHelper.GetStoreLocation());
            }
            ctrlProductRating.Visible = product.AllowCustomerRatings;
            BindProductVariantInfo(ProductVariant);
            BindProductInfo(product);
        }

        protected void BindProductInfo(Product product)
        {
            lProductName.Text = Server.HtmlEncode(product.Name);
            lShortDescription.Text = product.ShortDescription;
            lFullDescription.Text = product.FullDescription;

            var productPictures = product.ProductPictures;
            if(productPictures.Count > 1)
            {
                defaultImage.ImageUrl = PictureManager.GetPictureUrl(productPictures[0].PictureID, SettingManager.GetSettingValueInteger("Media.Product.DetailImageSize", 300));
                defaultImage.ToolTip = String.Format(GetLocaleResourceString("Media.Product.ImageAlternateTextFormat"), product.Name);
                defaultImage.AlternateText = String.Format(GetLocaleResourceString("Media.Product.ImageAlternateTextFormat"), product.Name);
                lvProductPictures.DataSource = productPictures;
                lvProductPictures.DataBind();
            }
            else if(productPictures.Count == 1)
            {
                defaultImage.ImageUrl = PictureManager.GetPictureUrl(productPictures[0].PictureID, SettingManager.GetSettingValueInteger("Media.Product.DetailImageSize", 300));
                defaultImage.ToolTip = String.Format(GetLocaleResourceString("Media.Product.ImageAlternateTextFormat"), product.Name);
                defaultImage.AlternateText = String.Format(GetLocaleResourceString("Media.Product.ImageAlternateTextFormat"), product.Name);
                lvProductPictures.Visible = false;
            }
            else
            {
                defaultImage.ImageUrl = PictureManager.GetDefaultPictureUrl(SettingManager.GetSettingValueInteger("Media.Product.DetailImageSize", 300));
                defaultImage.ToolTip = String.Format(GetLocaleResourceString("Media.Product.ImageAlternateTextFormat"), product.Name);
                defaultImage.AlternateText = String.Format(GetLocaleResourceString("Media.Product.ImageAlternateTextFormat"), product.Name);
                lvProductPictures.Visible = false;
            }
        }


        protected void BindProductVariantInfo(ProductVariant productVariant)
        {
            btnAddToWishlist.Visible = SettingManager.GetSettingValueBoolean("Common.EnableWishlist");

            ctrlTierPrices.ProductVariantID = productVariant.ProductVariantID;
            ctrlProductAttributes.ProductVariantID = ProductVariant.ProductVariantID;
            //ctrlProductPrice.ProductVariantID = productVariant.ProductVariantID;
            ctrlProductPrice2.ProductVariantID = productVariant.ProductVariantID;

            //stock
            if(pnlStockAvailablity != null && lblStockAvailablity != null)
            {
                if(productVariant.ManageInventory == (int)ManageInventoryMethodEnum.ManageStock
                        && productVariant.DisplayStockAvailability)
                {
                    if(productVariant.StockQuantity > 0 || productVariant.AllowOutOfStockOrders)
                    {
                        lblStockAvailablity.Text = string.Format(GetLocaleResourceString("Products.Availability"), GetLocaleResourceString("Products.InStock"));
                    }
                    else
                    {
                        lblStockAvailablity.Text = string.Format(GetLocaleResourceString("Products.Availability"), GetLocaleResourceString("Products.OutOfStock"));
                    }
                }
                else
                {
                    pnlStockAvailablity.Visible = false;
                }
            }

            //gift cards
            if(pnlGiftCardInfo != null)
            {
                if(productVariant.IsGiftCard)
                {
                    pnlGiftCardInfo.Visible = true;
                    if(NopContext.Current.User != null && !NopContext.Current.User.IsGuest)
                    {
                        txtSenderName.Text = NopContext.Current.User.FullName;
                        txtSenderEmail.Text = NopContext.Current.User.Email;
                    }
                }
                else
                {
                    pnlGiftCardInfo.Visible = false;
                }
            }

            if(!productVariant.DisableBuyButton)
            {
                txtQuantity.ValidationGroup = string.Format("ProductVariant{0}", productVariant.ProductVariantID);
                btnAddToCart.ValidationGroup = string.Format("ProductVariant{0}", productVariant.ProductVariantID);
                btnAddToWishlist.ValidationGroup = string.Format("ProductVariant{0}", productVariant.ProductVariantID);

                txtQuantity.Value = productVariant.OrderMinimumQuantity;
            }
            else
            {
                txtQuantity.Visible = false;
                btnAddToCart.Visible = false;
                btnAddToWishlist.Visible = false;
            }

            if(pnlDownloadSample != null && hlDownloadSample != null)
            {
                if(productVariant.IsDownload && productVariant.HasSampleDownload)
                {
                    pnlDownloadSample.Visible = true;
                    hlDownloadSample.NavigateUrl = DownloadManager.GetSampleDownloadUrl(productVariant);
                }
                else
                {
                    pnlDownloadSample.Visible = false;
                }
            }
        }

        protected override void OnPreRender(EventArgs e)
        {
            pnlProductReviews.Visible = ctrlProductReviews.Visible;
            pnlProductSpecs.Visible = ctrlProductSpecs.Visible;
            ProductsTabs.Visible = pnlProductReviews.Visible || pnlProductSpecs.Visible;

            //little hack here
            if(pnlProductSpecs.Visible)
            {
                ProductsTabs.ActiveTab = pnlProductSpecs;
            }
            if(pnlProductReviews.Visible)
            {
                ProductsTabs.ActiveTab = pnlProductReviews;
            }

            string jquery = CommonHelper.GetStoreLocation() + "Scripts/jquery-1.4.min.js";
            Page.ClientScript.RegisterClientScriptInclude(jquery, jquery);

            string slimBox = CommonHelper.GetStoreLocation() + "Scripts/slimbox2.js";
            Page.ClientScript.RegisterClientScriptInclude(slimBox, slimBox);

            base.OnPreRender(e);
        }

        public int ProductID
        {
            get
            {
                return CommonHelper.QueryStringInt("ProductID");
            }
        }

        public ProductVariant ProductVariant
        {
            get
            {
                Product product = ProductManager.GetProductByID(ProductID);
                if(product == null && product.ProductVariants.Count == 0)
                {
                    return null;
                }
                return product.ProductVariants[0];
            }
        }

        protected void OnCommand(object source, CommandEventArgs e)
        {
            var pv = ProductVariant;
            if(pv == null)
            {
                return;
            }

            string attributes = ctrlProductAttributes.SelectedAttributes;
            int quantity = txtQuantity.Value;

            //gift cards
            if(pv.IsGiftCard)
            {
                string recipientName = txtRecipientName.Text;
                string recipientEmail = txtRecipientEmail.Text;
                string senderName = txtSenderName.Text;
                string senderEmail = txtSenderEmail.Text;
                string giftCardMessage = txtGiftCardMessage.Text;

                attributes = ProductAttributeHelper.AddGiftCardAttribute(attributes, recipientName, recipientEmail, senderName, senderEmail, giftCardMessage);
            }

            try
            {
                if(e.CommandName == "AddToCart")
                {
                    var addToCartWarnings = ShoppingCartManager.AddToCart(ShoppingCartTypeEnum.ShoppingCart,
                        pv.ProductVariantID, attributes, quantity);
                    if(addToCartWarnings.Count == 0)
                    {
                        Response.Redirect("~/ShoppingCart.aspx");
                    }
                    else
                    {
                        var addToCartWarningsSb = new StringBuilder();
                        for(int i = 0; i < addToCartWarnings.Count; i++)
                        {
                            addToCartWarningsSb.Append(Server.HtmlEncode(addToCartWarnings[i]));
                            if(i != addToCartWarnings.Count - 1)
                            {
                                addToCartWarningsSb.Append("<br />");
                            }
                        }
                        lblError.Text = addToCartWarningsSb.ToString();
                    }
                }

                if(e.CommandName == "AddToWishlist")
                {
                    var addToCartWarnings = ShoppingCartManager.AddToCart(ShoppingCartTypeEnum.Wishlist,
                        pv.ProductVariantID, attributes, quantity);
                    if(addToCartWarnings.Count == 0)
                    {
                        Response.Redirect("~/Wishlist.aspx");
                    }
                    else
                    {
                        var addToCartWarningsSb = new StringBuilder();
                        for(int i = 0; i < addToCartWarnings.Count; i++)
                        {
                            addToCartWarningsSb.Append(Server.HtmlEncode(addToCartWarnings[i]));
                            if(i != addToCartWarnings.Count - 1)
                            {
                                addToCartWarningsSb.Append("<br />");
                            }
                        }
                        lblError.Text = addToCartWarningsSb.ToString();
                    }
                }
            }
            catch(Exception exc)
            {
                LogManager.InsertLog(LogTypeEnum.CustomerError, exc.Message, exc);
                lblError.Text = Server.HtmlEncode(exc.Message);
            }
        }
    }
}