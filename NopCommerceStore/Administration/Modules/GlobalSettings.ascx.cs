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
using System.Collections.ObjectModel;
using System.Configuration;
using System.Data;
using System.IO;
using System.Net.Mail;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using NopSolutions.NopCommerce.BusinessLogic;
using NopSolutions.NopCommerce.BusinessLogic.Audit;
using NopSolutions.NopCommerce.BusinessLogic.Configuration.Settings;
using NopSolutions.NopCommerce.BusinessLogic.Content.Forums;
using NopSolutions.NopCommerce.BusinessLogic.CustomerManagement;
using NopSolutions.NopCommerce.BusinessLogic.Measures;
using NopSolutions.NopCommerce.BusinessLogic.Media;
using NopSolutions.NopCommerce.BusinessLogic.Messages;
using NopSolutions.NopCommerce.BusinessLogic.Orders;
using NopSolutions.NopCommerce.BusinessLogic.Products;
using NopSolutions.NopCommerce.BusinessLogic.Profile;
using NopSolutions.NopCommerce.BusinessLogic.Security;
using NopSolutions.NopCommerce.BusinessLogic.SEO;
using NopSolutions.NopCommerce.BusinessLogic.Utils;
using NopSolutions.NopCommerce.Common;
using NopSolutions.NopCommerce.Common.Utils;
using NopSolutions.NopCommerce.BusinessLogic.IoC;

namespace NopSolutions.NopCommerce.Web.Administration.Modules
{
    public partial class GlobalSettingsControl : BaseNopAdministrationUserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                this.SelectTab(this.CommonSettingsTabs, this.TabId);
                FillDropDowns();
                BindData();
            }
        }

        private void BindData()
        {
            txtStoreName.Text = IoCFactory.Resolve<ISettingManager>().StoreName;
            txtStoreURL.Text = IoCFactory.Resolve<ISettingManager>().StoreUrl;
            cbStoreClosed.Checked = IoCFactory.Resolve<ISettingManager>().GetSettingValueBoolean("Common.StoreClosed");
            cbStoreClosedForAdmins.Checked = IoCFactory.Resolve<ISettingManager>().GetSettingValueBoolean("Common.StoreClosed.AllowAdminAccess");
            cbAnonymousCheckoutAllowed.Checked = IoCFactory.Resolve<ICustomerService>().AnonymousCheckoutAllowed;
            cbUseOnePageCheckout.Checked = IoCFactory.Resolve<ISettingManager>().GetSettingValueBoolean("Checkout.UseOnePageCheckout");
            cbCheckoutTermsOfService.Checked = IoCFactory.Resolve<ISettingManager>().GetSettingValueBoolean("Checkout.TermsOfServiceEnabled");

            cbStoreNameInTitle.Checked = IoCFactory.Resolve<ISettingManager>().GetSettingValueBoolean("SEO.IncludeStoreNameInTitle");
            txtDefaulSEOTitle.Text = IoCFactory.Resolve<ISettingManager>().GetSettingValue("SEO.DefaultTitle");
            txtDefaulSEODescription.Text = IoCFactory.Resolve<ISettingManager>().GetSettingValue("SEO.DefaultMetaDescription");
            txtDefaulSEOKeywords.Text = IoCFactory.Resolve<ISettingManager>().GetSettingValue("SEO.DefaultMetaKeywords");
            cbConvertNonWesternChars.Checked = IoCFactory.Resolve<ISettingManager>().GetSettingValueBoolean("SEONames.ConvertNonWesternChars");
            cbAllowCustomerSelectTheme.Checked = IoCFactory.Resolve<ISettingManager>().GetSettingValueBoolean("Display.AllowCustomerSelectTheme");
            
            if (File.Exists(HttpContext.Current.Request.PhysicalApplicationPath + "favicon.ico"))
            {
                imgFavicon.ImageUrl = CommonHelper.GetStoreLocation() + "favicon.ico";
                imgFavicon.Visible = true;
                btnFaviconRemove.Visible = true;
            }
            else
            {
                imgFavicon.Visible = false;
                btnFaviconRemove.Visible = false;
            }
            cbShowWelcomeMessage.Checked = IoCFactory.Resolve<ISettingManager>().GetSettingValueBoolean("Display.ShowWelcomeMessageOnMainPage");
            cbShowNewsHeaderRssURL.Checked = IoCFactory.Resolve<ISettingManager>().GetSettingValueBoolean("Display.ShowNewsHeaderRssURL");
            cbShowBlogHeaderRssURL.Checked = IoCFactory.Resolve<ISettingManager>().GetSettingValueBoolean("Display.ShowBlogHeaderRssURL");
            cbEnableUrlRewriting.Checked = SEOHelper.EnableUrlRewriting;
            txtProductUrlRewriteFormat.Text = IoCFactory.Resolve<ISettingManager>().GetSettingValue("SEO.Product.UrlRewriteFormat");
            cbProductCanonicalUrl.Checked = IoCFactory.Resolve<ISettingManager>().GetSettingValueBoolean("SEO.CanonicalURLs.Products.Enabled");
            txtCategoryUrlRewriteFormat.Text = IoCFactory.Resolve<ISettingManager>().GetSettingValue("SEO.Category.UrlRewriteFormat");
            cbCategoryCanonicalUrl.Checked = IoCFactory.Resolve<ISettingManager>().GetSettingValueBoolean("SEO.CanonicalURLs.Category.Enabled");
            txtManufacturerUrlRewriteFormat.Text = IoCFactory.Resolve<ISettingManager>().GetSettingValue("SEO.Manufacturer.UrlRewriteFormat");
            cbManufacturerCanonicalUrl.Checked = IoCFactory.Resolve<ISettingManager>().GetSettingValueBoolean("SEO.CanonicalURLs.Manufacturer.Enabled");
            txtProductTagUrlRewriteFormat.Text = IoCFactory.Resolve<ISettingManager>().GetSettingValue("SEO.ProductTags.UrlRewriteFormat");
            txtNewsUrlRewriteFormat.Text = IoCFactory.Resolve<ISettingManager>().GetSettingValue("SEO.News.UrlRewriteFormat");
            txtBlogUrlRewriteFormat.Text = IoCFactory.Resolve<ISettingManager>().GetSettingValue("SEO.Blog.UrlRewriteFormat");
            txtTopicUrlRewriteFormat.Text = IoCFactory.Resolve<ISettingManager>().GetSettingValue("SEO.Topic.UrlRewriteFormat");
            txtForumUrlRewriteFormat.Text = IoCFactory.Resolve<ISettingManager>().GetSettingValue("SEO.Forum.UrlRewriteFormat");
            txtForumGroupUrlRewriteFormat.Text = IoCFactory.Resolve<ISettingManager>().GetSettingValue("SEO.ForumGroup.UrlRewriteFormat");
            txtForumTopicUrlRewriteFormat.Text = IoCFactory.Resolve<ISettingManager>().GetSettingValue("SEO.ForumTopic.UrlRewriteFormat");


            lStoreImagesInDBStorage.Text = IoCFactory.Resolve<IPictureService>().StoreInDB ? GetLocaleResourceString("Admin.GlobalSettings.Media.StoreImagesInDB.DB") : GetLocaleResourceString("Admin.GlobalSettings.Media.StoreImagesInDB.FS");
            txtMaxImageSize.Value = IoCFactory.Resolve<ISettingManager>().GetSettingValueInteger("Media.MaximumImageSize");
            txtProductThumbSize.Value = IoCFactory.Resolve<ISettingManager>().GetSettingValueInteger("Media.Product.ThumbnailImageSize");
            txtProductDetailSize.Value = IoCFactory.Resolve<ISettingManager>().GetSettingValueInteger("Media.Product.DetailImageSize");
            txtProductVariantSize.Value = IoCFactory.Resolve<ISettingManager>().GetSettingValueInteger("Media.Product.VariantImageSize");
            txtCategoryThumbSize.Value = IoCFactory.Resolve<ISettingManager>().GetSettingValueInteger("Media.Category.ThumbnailImageSize");
            txtManufacturerThumbSize.Value = IoCFactory.Resolve<ISettingManager>().GetSettingValueInteger("Media.Manufacturer.ThumbnailImageSize");
            cbShowCartImages.Checked = IoCFactory.Resolve<ISettingManager>().GetSettingValueBoolean("Display.ShowProductImagesOnShoppingCart");
            cbShowWishListImages.Checked = IoCFactory.Resolve<ISettingManager>().GetSettingValueBoolean("Display.ShowProductImagesOnWishList");
            txtShoppingCartThumbSize.Value = IoCFactory.Resolve<ISettingManager>().GetSettingValueInteger("Media.ShoppingCart.ThumbnailImageSize");
            cbShowAdminProductImages.Checked = IoCFactory.Resolve<ISettingManager>().GetSettingValueBoolean("Display.ShowAdminProductImages");

            txtEncryptionPrivateKey.Text = IoCFactory.Resolve<ISettingManager>().GetSettingValue("Security.EncryptionPrivateKey");
            cbEnableLoginCaptchaImage.Checked = IoCFactory.Resolve<ISettingManager>().GetSettingValueBoolean("Common.LoginCaptchaImageEnabled");
            cbEnableRegisterCaptchaImage.Checked = IoCFactory.Resolve<ISettingManager>().GetSettingValueBoolean("Common.RegisterCaptchaImageEnabled");
            
            CommonHelper.SelectListItem(this.ddlCustomerNameFormat, (int)IoCFactory.Resolve<ICustomerService>().CustomerNameFormatting);
            cbShowCustomersLocation.Checked = IoCFactory.Resolve<ICustomerService>().ShowCustomersLocation;
            cbShowCustomersJoinDate.Checked = IoCFactory.Resolve<ICustomerService>().ShowCustomersJoinDate;
            cbAllowPM.Checked = IoCFactory.Resolve<IForumService>().AllowPrivateMessages;
            cbNotifyAboutPrivateMessages.Checked = IoCFactory.Resolve<IForumService>().NotifyAboutPrivateMessages;
            cbAllowViewingProfiles.Checked = IoCFactory.Resolve<ICustomerService>().AllowViewingProfiles;
            cbCustomersAllowedToUploadAvatars.Checked = IoCFactory.Resolve<ICustomerService>().AllowCustomersToUploadAvatars;
            cbDefaultAvatarEnabled.Checked = IoCFactory.Resolve<ICustomerService>().DefaultAvatarEnabled;
            lblCurrentTimeZone.Text = DateTimeHelper.CurrentTimeZone.DisplayName;
            TimeZoneInfo defaultStoreTimeZone = DateTimeHelper.DefaultStoreTimeZone;
            if (defaultStoreTimeZone != null)
                CommonHelper.SelectListItem(this.ddlDefaultStoreTimeZone, defaultStoreTimeZone.Id);
            cbAllowCustomersToSetTimeZone.Checked = DateTimeHelper.AllowCustomersToSetTimeZone;


            cbUsernamesEnabled.Checked = IoCFactory.Resolve<ICustomerService>().UsernamesEnabled;
            cbAllowCustomersToChangeUsernames.Checked = IoCFactory.Resolve<ICustomerService>().AllowCustomersToChangeUsernames;
            cbNotifyAboutNewCustomerRegistration.Checked = IoCFactory.Resolve<ICustomerService>().NotifyNewCustomerRegistration;
            CommonHelper.SelectListItem(this.ddlRegistrationMethod, (int)IoCFactory.Resolve<ICustomerService>().CustomerRegistrationType);
            cbAllowNavigationOnlyRegisteredCustomers.Checked = IoCFactory.Resolve<ICustomerService>().AllowNavigationOnlyRegisteredCustomers;
            cbHideNewsletterBox.Checked = IoCFactory.Resolve<ISettingManager>().GetSettingValueBoolean("Display.HideNewsletterBox");

            cbShowCategoryProductNumber.Checked = IoCFactory.Resolve<ISettingManager>().GetSettingValueBoolean("Display.Products.ShowCategoryProductNumber");
            cbHidePricesForNonRegistered.Checked = IoCFactory.Resolve<ISettingManager>().GetSettingValueBoolean("Common.HidePricesForNonRegistered");
            txtMinOrderSubtotalAmount.Value = IoCFactory.Resolve<IOrderService>().MinOrderSubtotalAmount;
            txtMinOrderTotalAmount.Value = IoCFactory.Resolve<IOrderService>().MinOrderTotalAmount;
            cbShowDiscountCouponBox.Checked = IoCFactory.Resolve<ISettingManager>().GetSettingValueBoolean("Display.Checkout.DiscountCouponBox");
            cbShowGiftCardBox.Checked = IoCFactory.Resolve<ISettingManager>().GetSettingValueBoolean("Display.Checkout.GiftCardBox");
            cbShowSKU.Checked = IoCFactory.Resolve<ISettingManager>().GetSettingValueBoolean("Display.Products.ShowSKU");
            cbShowManufacturerPartNumber.Checked = IoCFactory.Resolve<ISettingManager>().GetSettingValueBoolean("Display.Products.ShowManufacturerPartNumber");
            cbDisplayCartAfterAddingProduct.Checked = IoCFactory.Resolve<ISettingManager>().GetSettingValueBoolean("Display.Products.DisplayCartAfterAddingProduct");
            cbEnableDynamicPriceUpdate.Checked = IoCFactory.Resolve<ISettingManager>().GetSettingValueBoolean("ProductAttribute.EnableDynamicPriceUpdate");
            cbAllowProductSorting.Checked = IoCFactory.Resolve<ISettingManager>().GetSettingValueBoolean("Common.AllowProductSorting");
            cbShowShareButton.Checked = IoCFactory.Resolve<IProductService>().ShowShareButton;
            cbDownloadableProductsTab.Checked = IoCFactory.Resolve<ISettingManager>().GetSettingValueBoolean("Display.DownloadableProductsTab");
            cbUseImagesForLanguageSelection.Checked = IoCFactory.Resolve<ISettingManager>().GetSettingValueBoolean("Common.UseImagesForLanguageSelection", false);
            cbEnableCompareProducts.Checked = IoCFactory.Resolve<IProductService>().CompareProductsEnabled;
            cbEnableWishlist.Checked = IoCFactory.Resolve<ISettingManager>().GetSettingValueBoolean("Common.EnableWishlist");
            cbEmailWishlist.Checked = IoCFactory.Resolve<ISettingManager>().GetSettingValueBoolean("Common.EmailWishlist");
            cbIsReOrderAllowed.Checked = IoCFactory.Resolve<IOrderService>().IsReOrderAllowed;
            cbEnableEmailAFriend.Checked = IoCFactory.Resolve<ISettingManager>().GetSettingValueBoolean("Common.EnableEmailAFirend");
            cbShowMiniShoppingCart.Checked = IoCFactory.Resolve<ISettingManager>().GetSettingValueBoolean("Common.ShowMiniShoppingCart");
            cbRecentlyViewedProductsEnabled.Checked = IoCFactory.Resolve<IProductService>().RecentlyViewedProductsEnabled;
            txtRecentlyViewedProductsNumber.Value = IoCFactory.Resolve<IProductService>().RecentlyViewedProductsNumber;
            cbRecentlyAddedProductsEnabled.Checked = IoCFactory.Resolve<IProductService>().RecentlyAddedProductsEnabled;
            txtRecentlyAddedProductsNumber.Value = IoCFactory.Resolve<IProductService>().RecentlyAddedProductsNumber;
            cbNotifyAboutNewProductReviews.Checked = IoCFactory.Resolve<IProductService>().NotifyAboutNewProductReviews;
            cbProductReviewsMustBeApproved.Checked = IoCFactory.Resolve<ICustomerService>().ProductReviewsMustBeApproved;
            cbAllowAnonymousUsersToReviewProduct.Checked = IoCFactory.Resolve<ICustomerService>().AllowAnonymousUsersToReviewProduct;
            cbAllowAnonymousUsersToEmailAFriend.Checked = IoCFactory.Resolve<ICustomerService>().AllowAnonymousUsersToEmailAFriend;
            cbAllowAnonymousUsersToVotePolls.Checked = IoCFactory.Resolve<ISettingManager>().GetSettingValueBoolean("Common.AllowAnonymousUsersToVotePolls");

            cbAllowAnonymousUsersToSetProductRatings.Checked = IoCFactory.Resolve<ICustomerService>().AllowAnonymousUsersToSetProductRatings;
            cbShowBestsellersOnHomePage.Checked = IoCFactory.Resolve<ISettingManager>().GetSettingValueBoolean("Display.ShowBestsellersOnMainPage");
            txtShowBestsellersOnHomePageNumber.Value = IoCFactory.Resolve<ISettingManager>().GetSettingValueInteger("Display.ShowBestsellersOnMainPageNumber");
            cbProductsAlsoPurchased.Checked = IoCFactory.Resolve<IProductService>().ProductsAlsoPurchasedEnabled;
            txtProductsAlsoPurchasedNumber.Value = IoCFactory.Resolve<IProductService>().ProductsAlsoPurchasedNumber;
            txtCrossSellsNumber.Value = IoCFactory.Resolve<IProductService>().CrossSellsNumber;
            txtSearchPageProductsPerPage.Value = IoCFactory.Resolve<IProductService>().SearchPageProductsPerPage;
            txtMaxShoppingCartItems.Value = IoCFactory.Resolve<ISettingManager>().GetSettingValueInteger("Common.MaximumShoppingCartItems");
            txtMaxWishlistItems.Value = IoCFactory.Resolve<ISettingManager>().GetSettingValueInteger("Common.MaximumWishlistItems");

            cbLiveChatEnabled.Checked = IoCFactory.Resolve<ISettingManager>().GetSettingValueBoolean("LiveChat.Enabled", false);
            txtLiveChatBtnCode.Text = IoCFactory.Resolve<ISettingManager>().GetSettingValue("LiveChat.BtnCode");
            txtLiveChatMonCode.Text = IoCFactory.Resolve<ISettingManager>().GetSettingValue("LiveChat.MonCode");

            //Google Adsense
            cbGoogleAdsenseEnabled.Checked = IoCFactory.Resolve<ISettingManager>().GetSettingValueBoolean("GoogleAdsense.Enabled", false);
            txtGoogleAdsenseCode.Text = IoCFactory.Resolve<ISettingManager>().GetSettingValue("GoogleAdsense.Code");

            //Google Analytics
            cbGoogleAnalyticsEnabled.Checked = IoCFactory.Resolve<ISettingManager>().GetSettingValueBoolean("Analytics.GoogleEnabled", false);
            txtGoogleAnalyticsId.Text = IoCFactory.Resolve<ISettingManager>().GetSettingValue("Analytics.GoogleID");
            txtGoogleAnalyticsJS.Text = IoCFactory.Resolve<ISettingManager>().GetSettingValue("Analytics.GoogleJS");
            CommonHelper.SelectListItem(this.ddlGoogleAnalyticsPlacement, IoCFactory.Resolve<ISettingManager>().GetSettingValue("Analytics.GooglePlacement", "Body"));

            txtAllowedIPList.Text = IoCFactory.Resolve<ISettingManager>().GetSettingValue("Security.AdminAreaAllowedIP");

            if(File.Exists(PDFHelper.LogoFilePath))
            {
                imgPdfLogoPreview.ImageUrl = "~/images/pdflogo.img";
                btnPdfLogoRemove.Visible = true;
            }
            else
            {
                imgPdfLogoPreview.Visible = false;
            }

            //reward point
            cbRewardPointsEnabled.Checked = IoCFactory.Resolve<IOrderService>().RewardPointsEnabled;
            txtRewardPointsRate.Value = IoCFactory.Resolve<IOrderService>().RewardPointsExchangeRate;
            txtRewardPointsForRegistration.Value = IoCFactory.Resolve<IOrderService>().RewardPointsForRegistration;
            txtRewardPointsForPurchases_Amount.Value = IoCFactory.Resolve<IOrderService>().RewardPointsForPurchases_Amount;
            txtRewardPointsForPurchases_Points.Value = IoCFactory.Resolve<IOrderService>().RewardPointsForPurchases_Points;
            CommonHelper.SelectListItem(ddlRewardPointsAwardedOrderStatus, ((int)IoCFactory.Resolve<IOrderService>().RewardPointsForPurchases_Awarded).ToString());
            CommonHelper.SelectListItem(ddlRewardPointsCanceledOrderStatus, ((int)IoCFactory.Resolve<IOrderService>().RewardPointsForPurchases_Canceled).ToString());

            //gift cards
            if (IoCFactory.Resolve<IOrderService>().GiftCards_Activated.HasValue)
                CommonHelper.SelectListItem(ddlGiftCardsActivationOrderStatus, ((int)IoCFactory.Resolve<IOrderService>().GiftCards_Activated).ToString());
            else
                CommonHelper.SelectListItem(ddlGiftCardsActivationOrderStatus, 0);
            if (IoCFactory.Resolve<IOrderService>().GiftCards_Deactivated.HasValue)
                CommonHelper.SelectListItem(ddlGiftCardsDeactivationOrderStatus, ((int)IoCFactory.Resolve<IOrderService>().GiftCards_Deactivated).ToString());
            else
                CommonHelper.SelectListItem(ddlGiftCardsDeactivationOrderStatus, 0);

            //form fields
            cbffGenderEnabled.Checked = IoCFactory.Resolve<ICustomerService>().FormFieldGenderEnabled;
            cbffDateOfBirthEnabled.Checked = IoCFactory.Resolve<ICustomerService>().FormFieldDateOfBirthEnabled;
            cbffCompanyEnabled.Checked = IoCFactory.Resolve<ICustomerService>().FormFieldCompanyEnabled;
            cbffCompanyRequired.Checked = IoCFactory.Resolve<ICustomerService>().FormFieldCompanyRequired;
            cbffStreetAddressEnabled.Checked = IoCFactory.Resolve<ICustomerService>().FormFieldStreetAddressEnabled;
            cbffStreetAddressRequired.Checked = IoCFactory.Resolve<ICustomerService>().FormFieldStreetAddressRequired;
            cbffStreetAddress2Enabled.Checked = IoCFactory.Resolve<ICustomerService>().FormFieldStreetAddress2Enabled;
            cbffStreetAddress2Required.Checked = IoCFactory.Resolve<ICustomerService>().FormFieldStreetAddress2Required;
            cbffPostCodeEnabled.Checked = IoCFactory.Resolve<ICustomerService>().FormFieldPostCodeEnabled;
            cbffPostCodeRequired.Checked = IoCFactory.Resolve<ICustomerService>().FormFieldPostCodeRequired;
            cbffCityEnabled.Checked = IoCFactory.Resolve<ICustomerService>().FormFieldCityEnabled;
            cbffCityRequired.Checked = IoCFactory.Resolve<ICustomerService>().FormFieldCityRequired;
            cbffCountryEnabled.Checked = IoCFactory.Resolve<ICustomerService>().FormFieldCountryEnabled;
            cbffStateEnabled.Checked = IoCFactory.Resolve<ICustomerService>().FormFieldStateEnabled;
            cbffPhoneEnabled.Checked = IoCFactory.Resolve<ICustomerService>().FormFieldPhoneEnabled;
            cbffPhoneRequired.Checked = IoCFactory.Resolve<ICustomerService>().FormFieldPhoneRequired;
            cbffFaxEnabled.Checked = IoCFactory.Resolve<ICustomerService>().FormFieldFaxEnabled;
            cbffFaxRequired.Checked = IoCFactory.Resolve<ICustomerService>().FormFieldFaxRequired;
            cbffNewsletterBoxEnabled.Checked = IoCFactory.Resolve<ICustomerService>().FormFieldNewsletterEnabled;
            cbffTimeZoneEnabled.Checked = IoCFactory.Resolve<ICustomerService>().FormFieldTimeZoneEnabled;

            //return requests (RMA)
            cbReturnRequestsEnabled.Checked = IoCFactory.Resolve<ISettingManager>().GetSettingValueBoolean("ReturnRequests.Enable");
            txtReturnReasons.Text = IoCFactory.Resolve<ISettingManager>().GetSettingValue("ReturnRequests.ReturnReasons");
            txtReturnActions.Text = IoCFactory.Resolve<ISettingManager>().GetSettingValue("ReturnRequests.ReturnActions");

            cbDisplayPageExecutionTime.Checked = IoCFactory.Resolve<ISettingManager>().GetSettingValueBoolean("Display.PageExecutionTimeInfoEnabled");
        }

        private void FillDropDowns()
        {
            OrderStatusEnum[] orderStatuses = (OrderStatusEnum[])Enum.GetValues(typeof(OrderStatusEnum));            
            

            //reward points
            this.ddlRewardPointsAwardedOrderStatus.Items.Clear();
            foreach (OrderStatusEnum orderStatus in orderStatuses)
            {
                ListItem item2 = new ListItem(orderStatus.GetOrderStatusName(), ((int)orderStatus).ToString());
                this.ddlRewardPointsAwardedOrderStatus.Items.Add(item2);
            }
            this.ddlRewardPointsCanceledOrderStatus.Items.Clear();
            foreach (OrderStatusEnum orderStatus in orderStatuses)
            {
                ListItem item2 = new ListItem(orderStatus.GetOrderStatusName(), ((int)orderStatus).ToString());
                this.ddlRewardPointsCanceledOrderStatus.Items.Add(item2);
            }

            //gift cards
            this.ddlGiftCardsActivationOrderStatus.Items.Clear();
            ListItem gcaosEmpty = new ListItem("---", "0");
            this.ddlGiftCardsActivationOrderStatus.Items.Add(gcaosEmpty);
            foreach (OrderStatusEnum orderStatus in orderStatuses)
            {
                ListItem item2 = new ListItem(orderStatus.GetOrderStatusName(), ((int)orderStatus).ToString());
                this.ddlGiftCardsActivationOrderStatus.Items.Add(item2);
            }
            this.ddlGiftCardsDeactivationOrderStatus.Items.Clear();
            ListItem gcdosEmpty = new ListItem("---", "0");
            this.ddlGiftCardsDeactivationOrderStatus.Items.Add(gcdosEmpty);
            foreach (OrderStatusEnum orderStatus in orderStatuses)
            {
                ListItem item2 = new ListItem(orderStatus.GetOrderStatusName(), ((int)orderStatus).ToString());
                this.ddlGiftCardsDeactivationOrderStatus.Items.Add(item2);
            }

            //timezones
            this.ddlDefaultStoreTimeZone.Items.Clear();
            var timeZones = DateTimeHelper.GetSystemTimeZones();
            foreach (TimeZoneInfo timeZone in timeZones)
            {
                string timeZoneName = timeZone.DisplayName;
                ListItem ddlDefaultStoreTimeZoneItem2 = new ListItem(timeZoneName, timeZone.Id);
                this.ddlDefaultStoreTimeZone.Items.Add(ddlDefaultStoreTimeZoneItem2);
            }

            //etc
            CommonHelper.FillDropDownWithEnum(this.ddlCustomerNameFormat, typeof(CustomerNameFormatEnum));
            CommonHelper.FillDropDownWithEnum(this.ddlRegistrationMethod, typeof(CustomerRegistrationTypeEnum));
        }

        protected override void OnPreRender(EventArgs e)
        {
            BindJQuery();
            
            this.cbStoreClosed.Attributes.Add("onclick", "toggleStoreClosed();");
            this.cbEnableWishlist.Attributes.Add("onclick", "toggleWishlist();");            
            this.cbEnableUrlRewriting.Attributes.Add("onclick", "toggleUrlRewriting();");
            this.cbCustomersAllowedToUploadAvatars.Attributes.Add("onclick", "toggleCustomersAllowedToUploadAvatars();");
            this.cbAllowPM.Attributes.Add("onclick", "togglePM();");
            this.cbProductsAlsoPurchased.Attributes.Add("onclick", "toggleProductsAlsoPurchased();");
            this.cbRecentlyViewedProductsEnabled.Attributes.Add("onclick", "toggleRecentlyViewedProducts();");
            this.cbRecentlyAddedProductsEnabled.Attributes.Add("onclick", "toggleRecentlyAddedProducts();");
            this.cbShowBestsellersOnHomePage.Attributes.Add("onclick", "toggleShowBestsellersOnHomePage();");
            this.cbLiveChatEnabled.Attributes.Add("onclick", "toggleLiveChat();");
            this.cbGoogleAdsenseEnabled.Attributes.Add("onclick", "toggleGoogleAdsense();");
            this.cbGoogleAnalyticsEnabled.Attributes.Add("onclick", "toggleGoogleAnalytics();");
            this.cbUsernamesEnabled.Attributes.Add("onclick", "toggleUsernames();");
            
            this.btnStoreImagesInDBToggle.Attributes.Add("onclick", "this.disabled = true;" + Page.ClientScript.GetPostBackEventReference(this.btnStoreImagesInDBToggle, ""));
            
            base.OnPreRender(e);
        }

        protected void btnSave_Click(object sender, EventArgs e)
        {
            if (Page.IsValid)
            {
                try
                {
                    // Check IP list
                    string ipList = txtAllowedIPList.Text.Trim();
                    if (!String.IsNullOrEmpty(ipList))
                    {
                        foreach (string s in ipList.Split(new char[1] { ',' }))
                        {
                            if (!IoCFactory.Resolve<IBlacklistService>().IsValidIp(s.Trim()))
                            {
                                throw new NopException("IP list is not valid.");
                            }
                        }
                    }

                    IoCFactory.Resolve<ISettingManager>().StoreName = txtStoreName.Text;
                    IoCFactory.Resolve<ISettingManager>().StoreUrl = txtStoreURL.Text;
                    IoCFactory.Resolve<ISettingManager>().SetParam("Common.StoreClosed", cbStoreClosed.Checked.ToString());
                    IoCFactory.Resolve<ISettingManager>().SetParam("Common.StoreClosed.AllowAdminAccess", cbStoreClosedForAdmins.Checked.ToString());
                    IoCFactory.Resolve<ICustomerService>().AnonymousCheckoutAllowed = cbAnonymousCheckoutAllowed.Checked;
                    IoCFactory.Resolve<ISettingManager>().SetParam("Checkout.UseOnePageCheckout", cbUseOnePageCheckout.Checked.ToString());
                    IoCFactory.Resolve<ISettingManager>().SetParam("Checkout.TermsOfServiceEnabled", cbCheckoutTermsOfService.Checked.ToString());

                    IoCFactory.Resolve<ISettingManager>().SetParam("SEO.IncludeStoreNameInTitle", cbStoreNameInTitle.Checked.ToString());
                    IoCFactory.Resolve<ISettingManager>().SetParam("SEO.DefaultTitle", txtDefaulSEOTitle.Text);
                    IoCFactory.Resolve<ISettingManager>().SetParam("SEO.DefaultMetaDescription", txtDefaulSEODescription.Text);
                    IoCFactory.Resolve<ISettingManager>().SetParam("SEO.DefaultMetaKeywords", txtDefaulSEOKeywords.Text);
                    IoCFactory.Resolve<ISettingManager>().SetParam("SEONames.ConvertNonWesternChars", cbConvertNonWesternChars.Checked.ToString());

                    IoCFactory.Resolve<ISettingManager>().SetParam("Display.PublicStoreTheme", ctrlThemeSelector.SelectedTheme);
                    IoCFactory.Resolve<ISettingManager>().SetParam("Display.AllowCustomerSelectTheme", cbAllowCustomerSelectTheme.Checked.ToString());
                    
                    if (fileFavicon.HasFile)
                    {
                        HttpPostedFile postedFile = fileFavicon.PostedFile;
                        if (!postedFile.ContentType.Equals("image/x-icon") &&
                            !postedFile.ContentType.Equals("image/icon"))
                        {
                            throw new NopException("Image format not recognized, allowed formats are: .ico");
                        }
                        postedFile.SaveAs(HttpContext.Current.Request.PhysicalApplicationPath + "favicon.ico");
                    }
                    IoCFactory.Resolve<ISettingManager>().SetParam("Display.ShowWelcomeMessageOnMainPage", cbShowWelcomeMessage.Checked.ToString());
                    IoCFactory.Resolve<ISettingManager>().SetParam("Display.ShowNewsHeaderRssURL", cbShowNewsHeaderRssURL.Checked.ToString());
                    IoCFactory.Resolve<ISettingManager>().SetParam("Display.ShowBlogHeaderRssURL", cbShowBlogHeaderRssURL.Checked.ToString());
                    SEOHelper.EnableUrlRewriting = cbEnableUrlRewriting.Checked;
                    IoCFactory.Resolve<ISettingManager>().SetParam("SEO.Product.UrlRewriteFormat", txtProductUrlRewriteFormat.Text);
                    IoCFactory.Resolve<ISettingManager>().SetParam("SEO.CanonicalURLs.Products.Enabled", cbProductCanonicalUrl.Checked.ToString());
                    IoCFactory.Resolve<ISettingManager>().SetParam("SEO.Category.UrlRewriteFormat", txtCategoryUrlRewriteFormat.Text);
                    IoCFactory.Resolve<ISettingManager>().SetParam("SEO.CanonicalURLs.Category.Enabled", cbCategoryCanonicalUrl.Checked.ToString());
                    IoCFactory.Resolve<ISettingManager>().SetParam("SEO.Manufacturer.UrlRewriteFormat", txtManufacturerUrlRewriteFormat.Text);
                    IoCFactory.Resolve<ISettingManager>().SetParam("SEO.CanonicalURLs.Manufacturer.Enabled", cbManufacturerCanonicalUrl.Checked.ToString());
                    IoCFactory.Resolve<ISettingManager>().SetParam("SEO.ProductTags.UrlRewriteFormat", txtProductTagUrlRewriteFormat.Text);
                    IoCFactory.Resolve<ISettingManager>().SetParam("SEO.News.UrlRewriteFormat", txtNewsUrlRewriteFormat.Text);
                    IoCFactory.Resolve<ISettingManager>().SetParam("SEO.Blog.UrlRewriteFormat", txtBlogUrlRewriteFormat.Text);
                    IoCFactory.Resolve<ISettingManager>().SetParam("SEO.Topic.UrlRewriteFormat", txtTopicUrlRewriteFormat.Text);
                    IoCFactory.Resolve<ISettingManager>().SetParam("SEO.Forum.UrlRewriteFormat", txtForumUrlRewriteFormat.Text);
                    IoCFactory.Resolve<ISettingManager>().SetParam("SEO.ForumGroup.UrlRewriteFormat", txtForumGroupUrlRewriteFormat.Text);
                    IoCFactory.Resolve<ISettingManager>().SetParam("SEO.ForumTopic.UrlRewriteFormat", txtForumTopicUrlRewriteFormat.Text);


                    IoCFactory.Resolve<ISettingManager>().SetParam("Media.MaximumImageSize", txtMaxImageSize.Value.ToString());
                    IoCFactory.Resolve<ISettingManager>().SetParam("Media.Product.ThumbnailImageSize", txtProductThumbSize.Value.ToString());
                    IoCFactory.Resolve<ISettingManager>().SetParam("Media.Product.DetailImageSize", txtProductDetailSize.Value.ToString());
                    IoCFactory.Resolve<ISettingManager>().SetParam("Media.Product.VariantImageSize", txtProductVariantSize.Value.ToString());
                    IoCFactory.Resolve<ISettingManager>().SetParam("Media.Category.ThumbnailImageSize", txtCategoryThumbSize.Value.ToString());
                    IoCFactory.Resolve<ISettingManager>().SetParam("Media.Manufacturer.ThumbnailImageSize", txtManufacturerThumbSize.Value.ToString());
                    IoCFactory.Resolve<ISettingManager>().SetParam("Display.ShowProductImagesOnShoppingCart", cbShowCartImages.Checked.ToString());
                    IoCFactory.Resolve<ISettingManager>().SetParam("Display.ShowProductImagesOnWishList", cbShowWishListImages.Checked.ToString());
                    IoCFactory.Resolve<ISettingManager>().SetParam("Media.ShoppingCart.ThumbnailImageSize", txtShoppingCartThumbSize.Value.ToString());
                    IoCFactory.Resolve<ISettingManager>().SetParam("Display.ShowAdminProductImages", cbShowAdminProductImages.Checked.ToString());

                    IoCFactory.Resolve<ISettingManager>().SetParam("Security.AdminAreaAllowedIP", ipList);
                    IoCFactory.Resolve<ISettingManager>().SetParam("Common.LoginCaptchaImageEnabled", cbEnableLoginCaptchaImage.Checked.ToString());
                    IoCFactory.Resolve<ISettingManager>().SetParam("Common.RegisterCaptchaImageEnabled", cbEnableRegisterCaptchaImage.Checked.ToString());


                    IoCFactory.Resolve<ICustomerService>().CustomerNameFormatting = (CustomerNameFormatEnum)Enum.ToObject(typeof(CustomerNameFormatEnum), int.Parse(this.ddlCustomerNameFormat.SelectedItem.Value));
                    IoCFactory.Resolve<ICustomerService>().ShowCustomersLocation = cbShowCustomersLocation.Checked;
                    IoCFactory.Resolve<ICustomerService>().ShowCustomersJoinDate = cbShowCustomersJoinDate.Checked;
                    IoCFactory.Resolve<IForumService>().AllowPrivateMessages = cbAllowPM.Checked;
                    IoCFactory.Resolve<IForumService>().NotifyAboutPrivateMessages = cbNotifyAboutPrivateMessages.Checked;
                    IoCFactory.Resolve<ICustomerService>().AllowViewingProfiles = cbAllowViewingProfiles.Checked;
                    IoCFactory.Resolve<ICustomerService>().AllowCustomersToUploadAvatars = cbCustomersAllowedToUploadAvatars.Checked;
                    IoCFactory.Resolve<ICustomerService>().DefaultAvatarEnabled = cbDefaultAvatarEnabled.Checked;
                    string defaultStoreTimeZoneId = ddlDefaultStoreTimeZone.SelectedItem.Value;
                    DateTimeHelper.DefaultStoreTimeZone = DateTimeHelper.FindTimeZoneById(defaultStoreTimeZoneId);
                    DateTimeHelper.AllowCustomersToSetTimeZone = cbAllowCustomersToSetTimeZone.Checked;


                    IoCFactory.Resolve<ICustomerService>().UsernamesEnabled = cbUsernamesEnabled.Checked;
                    IoCFactory.Resolve<ICustomerService>().AllowCustomersToChangeUsernames = cbAllowCustomersToChangeUsernames.Checked;
                    IoCFactory.Resolve<ICustomerService>().NotifyNewCustomerRegistration = cbNotifyAboutNewCustomerRegistration.Checked;
                    IoCFactory.Resolve<ICustomerService>().CustomerRegistrationType = (CustomerRegistrationTypeEnum)Enum.ToObject(typeof(CustomerRegistrationTypeEnum), int.Parse(this.ddlRegistrationMethod.SelectedItem.Value));
                    IoCFactory.Resolve<ICustomerService>().AllowNavigationOnlyRegisteredCustomers = cbAllowNavigationOnlyRegisteredCustomers.Checked;
                    IoCFactory.Resolve<ISettingManager>().SetParam("Display.HideNewsletterBox", cbHideNewsletterBox.Checked.ToString());

                    IoCFactory.Resolve<ISettingManager>().SetParam("Display.Products.ShowCategoryProductNumber", cbShowCategoryProductNumber.Checked.ToString());
                    IoCFactory.Resolve<ISettingManager>().SetParam("Common.HidePricesForNonRegistered", cbHidePricesForNonRegistered.Checked.ToString());
                    IoCFactory.Resolve<IOrderService>().MinOrderSubtotalAmount = txtMinOrderSubtotalAmount.Value;
                    IoCFactory.Resolve<IOrderService>().MinOrderTotalAmount = txtMinOrderTotalAmount.Value;
                    IoCFactory.Resolve<ISettingManager>().SetParam("Display.Checkout.DiscountCouponBox", cbShowDiscountCouponBox.Checked.ToString());
                    IoCFactory.Resolve<ISettingManager>().SetParam("Display.Checkout.GiftCardBox", cbShowGiftCardBox.Checked.ToString());
                    IoCFactory.Resolve<ISettingManager>().SetParam("Display.Products.ShowSKU", cbShowSKU.Checked.ToString());
                    IoCFactory.Resolve<ISettingManager>().SetParam("Display.Products.ShowManufacturerPartNumber", cbShowManufacturerPartNumber.Checked.ToString());
                    IoCFactory.Resolve<ISettingManager>().SetParam("Display.Products.DisplayCartAfterAddingProduct", cbDisplayCartAfterAddingProduct.Checked.ToString());
                    IoCFactory.Resolve<ISettingManager>().SetParam("ProductAttribute.EnableDynamicPriceUpdate", cbEnableDynamicPriceUpdate.Checked.ToString());
                    IoCFactory.Resolve<ISettingManager>().SetParam("Common.AllowProductSorting", cbAllowProductSorting.Checked.ToString());
                    IoCFactory.Resolve<IProductService>().ShowShareButton = cbShowShareButton.Checked;
                    IoCFactory.Resolve<ISettingManager>().SetParam("Display.DownloadableProductsTab", cbDownloadableProductsTab.Checked.ToString());
                    IoCFactory.Resolve<ISettingManager>().SetParam("Common.UseImagesForLanguageSelection", cbUseImagesForLanguageSelection.Checked.ToString());
                    IoCFactory.Resolve<IProductService>().CompareProductsEnabled = cbEnableCompareProducts.Checked;
                    IoCFactory.Resolve<ISettingManager>().SetParam("Common.EnableWishlist", cbEnableWishlist.Checked.ToString());
                    IoCFactory.Resolve<ISettingManager>().SetParam("Common.EmailWishlist", cbEmailWishlist.Checked.ToString());
                    IoCFactory.Resolve<IOrderService>().IsReOrderAllowed = cbIsReOrderAllowed.Checked;
                    IoCFactory.Resolve<ISettingManager>().SetParam("Common.EnableEmailAFirend", cbEnableEmailAFriend.Checked.ToString());
                    IoCFactory.Resolve<ISettingManager>().SetParam("Common.ShowMiniShoppingCart", cbShowMiniShoppingCart.Checked.ToString());
                    IoCFactory.Resolve<IProductService>().RecentlyViewedProductsEnabled = cbRecentlyViewedProductsEnabled.Checked;
                    IoCFactory.Resolve<IProductService>().RecentlyViewedProductsNumber = txtRecentlyViewedProductsNumber.Value;
                    IoCFactory.Resolve<IProductService>().RecentlyAddedProductsEnabled = cbRecentlyAddedProductsEnabled.Checked;
                    IoCFactory.Resolve<IProductService>().RecentlyAddedProductsNumber = txtRecentlyAddedProductsNumber.Value;
                    IoCFactory.Resolve<IProductService>().NotifyAboutNewProductReviews = cbNotifyAboutNewProductReviews.Checked;
                    IoCFactory.Resolve<ICustomerService>().ProductReviewsMustBeApproved = cbProductReviewsMustBeApproved.Checked;
                    IoCFactory.Resolve<ICustomerService>().AllowAnonymousUsersToReviewProduct = cbAllowAnonymousUsersToReviewProduct.Checked;
                    IoCFactory.Resolve<ICustomerService>().AllowAnonymousUsersToEmailAFriend = cbAllowAnonymousUsersToEmailAFriend.Checked;
                    IoCFactory.Resolve<ISettingManager>().SetParam("Common.AllowAnonymousUsersToVotePolls", cbAllowAnonymousUsersToVotePolls.Checked.ToString());

                    IoCFactory.Resolve<ICustomerService>().AllowAnonymousUsersToSetProductRatings = cbAllowAnonymousUsersToSetProductRatings.Checked;
                    IoCFactory.Resolve<ISettingManager>().SetParam("Display.ShowBestsellersOnMainPage", cbShowBestsellersOnHomePage.Checked.ToString());
                    IoCFactory.Resolve<ISettingManager>().SetParam("Display.ShowBestsellersOnMainPageNumber", txtShowBestsellersOnHomePageNumber.Value.ToString());
                    IoCFactory.Resolve<IProductService>().ProductsAlsoPurchasedEnabled = cbProductsAlsoPurchased.Checked;
                    IoCFactory.Resolve<IProductService>().ProductsAlsoPurchasedNumber = txtProductsAlsoPurchasedNumber.Value;
                    IoCFactory.Resolve<IProductService>().CrossSellsNumber = txtCrossSellsNumber.Value;
                    IoCFactory.Resolve<IProductService>().SearchPageProductsPerPage = txtSearchPageProductsPerPage.Value;
                    IoCFactory.Resolve<ISettingManager>().SetParam("Common.MaximumShoppingCartItems", txtMaxShoppingCartItems.Value.ToString());
                    IoCFactory.Resolve<ISettingManager>().SetParam("Common.MaximumWishlistItems", txtMaxWishlistItems.Value.ToString());
                    
                    IoCFactory.Resolve<ISettingManager>().SetParam("LiveChat.Enabled", cbLiveChatEnabled.Checked.ToString());
                    IoCFactory.Resolve<ISettingManager>().SetParam("LiveChat.BtnCode", txtLiveChatBtnCode.Text);
                    IoCFactory.Resolve<ISettingManager>().SetParam("LiveChat.MonCode", txtLiveChatMonCode.Text);

                    //Google Adsense
                    IoCFactory.Resolve<ISettingManager>().SetParam("GoogleAdsense.Enabled", cbGoogleAdsenseEnabled.Checked.ToString());
                    IoCFactory.Resolve<ISettingManager>().SetParam("GoogleAdsense.Code", txtGoogleAdsenseCode.Text);

                    //Google Analytics
                    IoCFactory.Resolve<ISettingManager>().SetParam("Analytics.GoogleEnabled", cbGoogleAnalyticsEnabled.Checked.ToString());
                    IoCFactory.Resolve<ISettingManager>().SetParam("Analytics.GoogleID", txtGoogleAnalyticsId.Text);
                    IoCFactory.Resolve<ISettingManager>().SetParam("Analytics.GoogleJS", txtGoogleAnalyticsJS.Text);
                    IoCFactory.Resolve<ISettingManager>().SetParam("Analytics.GooglePlacement", this.ddlGoogleAnalyticsPlacement.SelectedItem.Value);


                    if (uplPdfLogo.HasFile)
                    {
                        HttpPostedFile postedFile = uplPdfLogo.PostedFile;
                        if (!postedFile.ContentType.Equals("image/jpeg") && !postedFile.ContentType.Equals("image/gif") && !postedFile.ContentType.Equals("image/png"))
                        {
                            throw new NopException("Image format not recognized, allowed formats are: .png, .jpg, .jpeg, .gif");
                        }
                        postedFile.SaveAs(PDFHelper.LogoFilePath);
                    }

                    //reward point
                    IoCFactory.Resolve<IOrderService>().RewardPointsEnabled = cbRewardPointsEnabled.Checked;
                    IoCFactory.Resolve<IOrderService>().RewardPointsExchangeRate = txtRewardPointsRate.Value;
                    IoCFactory.Resolve<IOrderService>().RewardPointsForRegistration = txtRewardPointsForRegistration.Value;
                    IoCFactory.Resolve<IOrderService>().RewardPointsForPurchases_Amount = txtRewardPointsForPurchases_Amount.Value;
                    IoCFactory.Resolve<IOrderService>().RewardPointsForPurchases_Points = txtRewardPointsForPurchases_Points.Value;
                    OrderStatusEnum rppa = (OrderStatusEnum)int.Parse(ddlRewardPointsAwardedOrderStatus.SelectedItem.Value);
                    if (rppa != OrderStatusEnum.Pending)
                    {
                        IoCFactory.Resolve<IOrderService>().RewardPointsForPurchases_Awarded = rppa;
                    }
                    else
                    {
                        //ensure that order status is not pending
                        throw new NopException(GetLocaleResourceString("Admin.GlobalSettings.PendingOrderStatusNotAllowed"));
                    }
                    OrderStatusEnum rppc = (OrderStatusEnum)int.Parse(ddlRewardPointsCanceledOrderStatus.SelectedItem.Value);
                    if (rppc != OrderStatusEnum.Pending)
                    {
                        IoCFactory.Resolve<IOrderService>().RewardPointsForPurchases_Canceled = rppc;
                    }
                    else
                    {
                        //ensure that order status is not pending
                        throw new NopException(GetLocaleResourceString("Admin.GlobalSettings.PendingOrderStatusNotAllowed"));
                    }

                    //gift cards
                    int gcaos = int.Parse(ddlGiftCardsActivationOrderStatus.SelectedItem.Value);
                    if (gcaos > 0)
                    {
                        if ((OrderStatusEnum)gcaos != OrderStatusEnum.Pending)
                        {
                            IoCFactory.Resolve<IOrderService>().GiftCards_Activated = (OrderStatusEnum)gcaos;
                        }
                        else
                        {
                            //ensure that order status is not pending
                            throw new NopException(GetLocaleResourceString("Admin.GlobalSettings.PendingOrderStatusNotAllowed"));
                        }
                    }
                    else
                    {
                        IoCFactory.Resolve<IOrderService>().GiftCards_Activated = null;
                    }
                    int gcdos = int.Parse(ddlGiftCardsDeactivationOrderStatus.SelectedItem.Value);
                    if (gcdos > 0)
                    {
                        if ((OrderStatusEnum)gcdos != OrderStatusEnum.Pending)
                        {
                            IoCFactory.Resolve<IOrderService>().GiftCards_Deactivated = (OrderStatusEnum)gcdos;
                        }
                        else
                        {
                            //ensure that order status is not pending
                            throw new NopException(GetLocaleResourceString("Admin.GlobalSettings.PendingOrderStatusNotAllowed"));
                        }
                    }
                    else
                    {
                        IoCFactory.Resolve<IOrderService>().GiftCards_Deactivated = null;
                    }

                    //form fields
                    IoCFactory.Resolve<ICustomerService>().FormFieldGenderEnabled = cbffGenderEnabled.Checked;
                    IoCFactory.Resolve<ICustomerService>().FormFieldDateOfBirthEnabled = cbffDateOfBirthEnabled.Checked;
                    IoCFactory.Resolve<ICustomerService>().FormFieldCompanyEnabled = cbffCompanyEnabled.Checked;
                    IoCFactory.Resolve<ICustomerService>().FormFieldCompanyRequired = cbffCompanyRequired.Checked;
                    IoCFactory.Resolve<ICustomerService>().FormFieldStreetAddressEnabled = cbffStreetAddressEnabled.Checked;
                    IoCFactory.Resolve<ICustomerService>().FormFieldStreetAddressRequired = cbffStreetAddressRequired.Checked;
                    IoCFactory.Resolve<ICustomerService>().FormFieldStreetAddress2Enabled = cbffStreetAddress2Enabled.Checked;
                    IoCFactory.Resolve<ICustomerService>().FormFieldStreetAddress2Required = cbffStreetAddress2Required.Checked;
                    IoCFactory.Resolve<ICustomerService>().FormFieldPostCodeEnabled = cbffPostCodeEnabled.Checked;
                    IoCFactory.Resolve<ICustomerService>().FormFieldPostCodeRequired = cbffPostCodeRequired.Checked;
                    IoCFactory.Resolve<ICustomerService>().FormFieldCityEnabled = cbffCityEnabled.Checked;
                    IoCFactory.Resolve<ICustomerService>().FormFieldCityRequired = cbffCityRequired.Checked;
                    IoCFactory.Resolve<ICustomerService>().FormFieldCountryEnabled = cbffCountryEnabled.Checked;
                    IoCFactory.Resolve<ICustomerService>().FormFieldStateEnabled = cbffStateEnabled.Checked;
                    IoCFactory.Resolve<ICustomerService>().FormFieldPhoneEnabled = cbffPhoneEnabled.Checked;
                    IoCFactory.Resolve<ICustomerService>().FormFieldPhoneRequired = cbffPhoneRequired.Checked;
                    IoCFactory.Resolve<ICustomerService>().FormFieldFaxEnabled = cbffFaxEnabled.Checked;
                    IoCFactory.Resolve<ICustomerService>().FormFieldFaxRequired = cbffFaxRequired.Checked;
                    IoCFactory.Resolve<ICustomerService>().FormFieldNewsletterEnabled = cbffNewsletterBoxEnabled.Checked;
                    IoCFactory.Resolve<ICustomerService>().FormFieldTimeZoneEnabled = cbffTimeZoneEnabled.Checked;


                    IoCFactory.Resolve<ISettingManager>().SetParam("Display.PageExecutionTimeInfoEnabled", cbDisplayPageExecutionTime.Checked.ToString());

                    //return requests (RMA)
                    IoCFactory.Resolve<ISettingManager>().SetParam("ReturnRequests.Enable", cbReturnRequestsEnabled.Checked.ToString());
                    IoCFactory.Resolve<ISettingManager>().SetParam("ReturnRequests.ReturnReasons", txtReturnReasons.Text);
                    IoCFactory.Resolve<ISettingManager>().SetParam("ReturnRequests.ReturnActions", txtReturnActions.Text);

                    IoCFactory.Resolve<ICustomerActivityService>().InsertActivity(
                        "EditGlobalSettings",
                        GetLocaleResourceString("ActivityLog.EditGlobalSettings"));

                    Response.Redirect(string.Format("GlobalSettings.aspx?TabID={0}", this.GetActiveTabId(this.CommonSettingsTabs)));
                }
                catch (Exception exc)
                {
                    ProcessException(exc);
                }
            }
        }

        protected void btnStoreImagesInDBToggle_OnClick(object sender, EventArgs e)
        {
            try
            {
                IoCFactory.Resolve<IPictureService>().StoreInDB = !IoCFactory.Resolve<IPictureService>().StoreInDB;

                Response.Redirect(string.Format("GlobalSettings.aspx?TabID={0}", this.GetActiveTabId(this.CommonSettingsTabs)));
            }
            catch (Exception ex)
            {
                ProcessException(ex);
            }
        }

        protected void BtnPdfLogoRemove_OnClick(object sender, EventArgs e)
        {
            try
            {
                File.Delete(PDFHelper.LogoFilePath);
                imgPdfLogoPreview.Visible = false;
                btnPdfLogoRemove.Visible = false;
            }
            catch(Exception ex)
            {
                ProcessException(ex);
            }
        }

        protected void btnFaviconRemove_OnClick(object sender, EventArgs e)
        {
            try
            {
                File.Delete(HttpContext.Current.Request.PhysicalApplicationPath + "favicon.ico");
                imgFavicon.Visible = false;
                btnFaviconRemove.Visible = false;
            }
            catch (Exception ex)
            {
                ProcessException(ex);
            }
        }
        
        protected void btnChangeEncryptionPrivateKey_Click(object sender, EventArgs e)
        {
            if (Page.IsValid)
            {
                try
                {
                    SecurityHelper.ChangeEncryptionPrivateKey(txtEncryptionPrivateKey.Text);
                    lblChangeEncryptionPrivateKeyResult.Text = GetLocaleResourceString("Admin.GlobalSettings.Security.ChangeKeySuccess");
                }
                catch (Exception exc)
                {
                    lblChangeEncryptionPrivateKeyResult.Text = exc.Message;
                }
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
