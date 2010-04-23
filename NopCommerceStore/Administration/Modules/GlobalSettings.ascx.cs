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
using System.Net.Mail;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using NopSolutions.NopCommerce.BusinessLogic;
using NopSolutions.NopCommerce.BusinessLogic.Configuration.Settings;
using NopSolutions.NopCommerce.BusinessLogic.CustomerManagement;
using NopSolutions.NopCommerce.BusinessLogic.Measures;
using NopSolutions.NopCommerce.BusinessLogic.Messages;
using NopSolutions.NopCommerce.BusinessLogic.Profile;
using NopSolutions.NopCommerce.BusinessLogic.Security;
using NopSolutions.NopCommerce.Common.Utils;
using NopSolutions.NopCommerce.BusinessLogic.Content.Forums;
using NopSolutions.NopCommerce.BusinessLogic.Products;
using NopSolutions.NopCommerce.Common;
using System.IO;
using NopSolutions.NopCommerce.BusinessLogic.Orders;
using NopSolutions.NopCommerce.BusinessLogic.Media;
using NopSolutions.NopCommerce.BusinessLogic.Audit;
using NopSolutions.NopCommerce.BusinessLogic.Utils;

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
            txtStoreName.Text = SettingManager.StoreName;
            txtStoreURL.Text = SettingManager.StoreUrl;
            cbStoreClosed.Checked = SettingManager.GetSettingValueBoolean("Common.StoreClosed");
            cbAnonymousCheckoutAllowed.Checked = CustomerManager.AnonymousCheckoutAllowed;
            cbUseOnePageCheckout.Checked = SettingManager.GetSettingValueBoolean("Checkout.UseOnePageCheckout");
            cbCheckoutTermsOfService.Checked = SettingManager.GetSettingValueBoolean("Checkout.TermsOfServiceEnabled");

            cbStoreNameInTitle.Checked = SettingManager.GetSettingValueBoolean("SEO.IncludeStoreNameInTitle");
            txtDefaulSEOTitle.Text = SettingManager.GetSettingValue("SEO.DefaultTitle");
            txtDefaulSEODescription.Text = SettingManager.GetSettingValue("SEO.DefaultMetaDescription");
            txtDefaulSEOKeywords.Text = SettingManager.GetSettingValue("SEO.DefaultMetaKeywords");
            if (File.Exists(HttpContext.Current.Request.PhysicalApplicationPath + "favicon.ico"))
            {
                imgFavicon.ImageUrl = CommonHelper.GetStoreLocation() + "favicon.ico";
            }
            else
            {
                imgFavicon.Visible = false;
            }
            cbShowWelcomeMessage.Checked = SettingManager.GetSettingValueBoolean("Display.ShowWelcomeMessageOnMainPage");
            cbShowNewsHeaderRssURL.Checked = SettingManager.GetSettingValueBoolean("Display.ShowNewsHeaderRssURL");
            cbShowBlogHeaderRssURL.Checked = SettingManager.GetSettingValueBoolean("Display.ShowBlogHeaderRssURL");
            txtProductUrlRewriteFormat.Text = SettingManager.GetSettingValue("SEO.Product.UrlRewriteFormat");
            txtCategoryUrlRewriteFormat.Text = SettingManager.GetSettingValue("SEO.Category.UrlRewriteFormat");
            txtManufacturerUrlRewriteFormat.Text = SettingManager.GetSettingValue("SEO.Manufacturer.UrlRewriteFormat");
            txtNewsUrlRewriteFormat.Text = SettingManager.GetSettingValue("SEO.News.UrlRewriteFormat");
            txtBlogUrlRewriteFormat.Text = SettingManager.GetSettingValue("SEO.Blog.UrlRewriteFormat");
            txtTopicUrlRewriteFormat.Text = SettingManager.GetSettingValue("SEO.Topic.UrlRewriteFormat");
            txtForumUrlRewriteFormat.Text = SettingManager.GetSettingValue("SEO.Forum.UrlRewriteFormat");
            txtForumGroupUrlRewriteFormat.Text = SettingManager.GetSettingValue("SEO.ForumGroup.UrlRewriteFormat");
            txtForumTopicUrlRewriteFormat.Text = SettingManager.GetSettingValue("SEO.ForumTopic.UrlRewriteFormat");


            txtMaxImageSize.Value = SettingManager.GetSettingValueInteger("Media.MaximumImageSize");
            txtProductThumbSize.Value = SettingManager.GetSettingValueInteger("Media.Product.ThumbnailImageSize");
            txtProductDetailSize.Value = SettingManager.GetSettingValueInteger("Media.Product.DetailImageSize");
            txtProductVariantSize.Value = SettingManager.GetSettingValueInteger("Media.Product.VariantImageSize");
            txtCategoryThumbSize.Value = SettingManager.GetSettingValueInteger("Media.Category.ThumbnailImageSize");
            txtManufacturerThumbSize.Value = SettingManager.GetSettingValueInteger("Media.Manufacturer.ThumbnailImageSize");
            cbShowCartImages.Checked = SettingManager.GetSettingValueBoolean("Display.ShowProductImagesOnShoppingCart");
            cbShowWishListImages.Checked = SettingManager.GetSettingValueBoolean("Display.ShowProductImagesOnWishList");
            txtShoppingCartThumbSize.Value = SettingManager.GetSettingValueInteger("Media.ShoppingCart.ThumbnailImageSize");

            txtAdminEmailAddress.Text = MessageManager.AdminEmailAddress;
            txtAdminEmailDisplayName.Text = MessageManager.AdminEmailDisplayName;
            txtAdminEmailHost.Text = MessageManager.AdminEmailHost;
            txtAdminEmailPort.Text = MessageManager.AdminEmailPort.ToString();
            txtAdminEmailUser.Text = MessageManager.AdminEmailUser;
            txtAdminEmailPassword.Text = MessageManager.AdminEmailPassword;
            cbAdminEmailEnableSsl.Checked = MessageManager.AdminEmailEnableSsl;
            cbAdminEmailUseDefaultCredentials.Checked = MessageManager.AdminEmailUseDefaultCredentials;


            txtEncryptionPrivateKey.Text = SettingManager.GetSettingValue("Security.EncryptionPrivateKey");
            cbEnableLoginCaptchaImage.Checked = SettingManager.GetSettingValueBoolean("Common.LoginCaptchaImageEnabled");
            cbEnableRegisterCaptchaImage.Checked = SettingManager.GetSettingValueBoolean("Common.RegisterCaptchaImageEnabled");


            CommonHelper.SelectListItem(this.ddlCustomerNameFormat, (int)CustomerManager.CustomerNameFormatting);
            cbShowCustomersLocation.Checked = CustomerManager.ShowCustomersLocation;
            cbShowCustomersJoinDate.Checked = CustomerManager.ShowCustomersJoinDate;
            cbAllowPM.Checked = ForumManager.AllowPrivateMessages;
            cbAllowViewingProfiles.Checked = CustomerManager.AllowViewingProfiles;
            cbCustomersAllowedToUploadAvatars.Checked = CustomerManager.AllowCustomersToUploadAvatars;
            cbDefaultAvatarEnabled.Checked = CustomerManager.DefaultAvatarEnabled;
            lblCurrentTimeZone.Text = DateTimeHelper.CurrentTimeZone.DisplayName;
            TimeZoneInfo defaultStoreTimeZone = DateTimeHelper.DefaultStoreTimeZone;
            if (defaultStoreTimeZone != null)
                CommonHelper.SelectListItem(this.ddlDefaultStoreTimeZone, defaultStoreTimeZone.Id);
            cbAllowCustomersToSetTimeZone.Checked = DateTimeHelper.AllowCustomersToSetTimeZone;


            cbUsernamesEnabled.Checked = CustomerManager.UsernamesEnabled;
            CommonHelper.SelectListItem(this.ddlRegistrationMethod, (int)CustomerManager.CustomerRegistrationType);
            cbAllowNavigationOnlyRegisteredCustomers.Checked = CustomerManager.AllowNavigationOnlyRegisteredCustomers;
            cbEnableCompareProducts.Checked = ProductManager.CompareProductsEnabled;
            cbEnableWishlist.Checked = SettingManager.GetSettingValueBoolean("Common.EnableWishlist");
            cbIsReOrderAllowed.Checked = OrderManager.IsReOrderAllowed;
            cbEnableEmailAFriend.Checked = SettingManager.GetSettingValueBoolean("Common.EnableEmailAFirend");
            cbShowMiniShoppingCart.Checked = SettingManager.GetSettingValueBoolean("Common.ShowMiniShoppingCart");
            cbRecentlyViewedProductsEnabled.Checked = ProductManager.RecentlyViewedProductsEnabled;
            txtRecentlyViewedProductsNumber.Value = ProductManager.RecentlyViewedProductsNumber;
            cbRecentlyAddedProductsEnabled.Checked = ProductManager.RecentlyAddedProductsEnabled;
            txtRecentlyAddedProductsNumber.Value = ProductManager.RecentlyAddedProductsNumber;
            cbNotifyAboutNewProductReviews.Checked = ProductManager.NotifyAboutNewProductReviews;
            cbProductReviewsMustBeApproved.Checked = CustomerManager.ProductReviewsMustBeApproved;
            cbAllowAnonymousUsersToReviewProduct.Checked = CustomerManager.AllowAnonymousUsersToReviewProduct;
            cbAllowAnonymousUsersToSetProductRatings.Checked = CustomerManager.AllowAnonymousUsersToSetProductRatings;
            cbShowBestsellersOnHomePage.Checked = SettingManager.GetSettingValueBoolean("Display.ShowBestsellersOnMainPage");
            txtShowBestsellersOnHomePageNumber.Value = SettingManager.GetSettingValueInteger("Display.ShowBestsellersOnMainPageNumber");
            cbProductsAlsoPurchased.Checked = ProductManager.ProductsAlsoPurchasedEnabled;
            txtProductsAlsoPurchasedNumber.Value = ProductManager.ProductsAlsoPurchasedNumber;

            cbIsSMSAlertsEnabled.Checked = SMSManager.IsSMSAlertsEnabled;
            txtSMSAlertsPhoneNumber.Text = SMSManager.PhoneNumber;
            txtSMSAlertsClickatellAPIId.Text = SMSManager.ClickatellAPIId;
            txtSMSAlertsClickatellUsername.Text = SMSManager.ClickatellUsername;
            txtSMSAlertsClickatellPassword.Text = SMSManager.ClickatellPassword;

            cbLiveChatEnabled.Checked = SettingManager.GetSettingValueBoolean("LiveChat.Enabled", false);
            txtLiveChatBtnCode.Text = SettingManager.GetSettingValue("LiveChat.BtnCode");
            txtLiveChatMonCode.Text = SettingManager.GetSettingValue("LiveChat.MonCode");

            txtAllowedIPList.Text = SettingManager.GetSettingValue("Security.AdminAreaAllowedIP");

            if(File.Exists(PDFHelper.LogoFilePath))
            {
                imgPdfLogoPreview.ImageUrl = "~/images/pdflogo.img";
                btnPdfLogoRemove.Visible = true;
            }
            else
            {
                imgPdfLogoPreview.Visible = false;
            }
        }

        private void FillDropDowns()
        {
            this.ddlDefaultStoreTimeZone.Items.Clear();
            ReadOnlyCollection<TimeZoneInfo> timeZones = DateTimeHelper.GetSystemTimeZones();
            foreach (TimeZoneInfo timeZone in timeZones)
            {
                string timeZoneName = timeZone.DisplayName;
                ListItem ddlDefaultStoreTimeZoneItem2 = new ListItem(timeZoneName, timeZone.Id);
                this.ddlDefaultStoreTimeZone.Items.Add(ddlDefaultStoreTimeZoneItem2);
            }

            CommonHelper.FillDropDownWithEnum(this.ddlCustomerNameFormat, typeof(CustomerNameFormatEnum));
            CommonHelper.FillDropDownWithEnum(this.ddlRegistrationMethod, typeof(CustomerRegistrationTypeEnum));
        }

        protected override void OnPreRender(EventArgs e)
        {
            string jquery = CommonHelper.GetStoreLocation() + "Scripts/jquery-1.4.min.js";
            Page.ClientScript.RegisterClientScriptInclude(jquery, jquery);

            this.cbCustomersAllowedToUploadAvatars.Attributes.Add("onclick", "toggleCustomersAllowedToUploadAvatars();");
            this.cbProductsAlsoPurchased.Attributes.Add("onclick", "toggleProductsAlsoPurchased();");
            this.cbRecentlyViewedProductsEnabled.Attributes.Add("onclick", "toggleRecentlyViewedProducts();");
            this.cbRecentlyAddedProductsEnabled.Attributes.Add("onclick", "toggleRecentlyAddedProducts();");
            this.cbShowBestsellersOnHomePage.Attributes.Add("onclick", "toggleShowBestsellersOnHomePage();");
            this.cbIsSMSAlertsEnabled.Attributes.Add("onclick", "toggleSMSAlerts();");
            this.cbLiveChatEnabled.Attributes.Add("onclick", "toggleLiveChat();");

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
                    if(!String.IsNullOrEmpty(ipList))
                    {
                        foreach(string s in ipList.Split(new char[1]{','}))
                        {
                            if(!IpBlacklistManager.IsValidIp(s.Trim()))
                            {
                                throw new NopException("IP list is not valid.");
                            }
                        }
                    }

                    SettingManager.StoreName = txtStoreName.Text;
                    SettingManager.StoreUrl = txtStoreURL.Text;
                    SettingManager.SetParam("Common.StoreClosed", cbStoreClosed.Checked.ToString());
                    CustomerManager.AnonymousCheckoutAllowed = cbAnonymousCheckoutAllowed.Checked;
                    SettingManager.SetParam("Checkout.UseOnePageCheckout", cbUseOnePageCheckout.Checked.ToString());
                    SettingManager.SetParam("Checkout.TermsOfServiceEnabled", cbCheckoutTermsOfService.Checked.ToString());

                    SettingManager.SetParam("SEO.IncludeStoreNameInTitle", cbStoreNameInTitle.Checked.ToString());
                    SettingManager.SetParam("SEO.DefaultTitle", txtDefaulSEOTitle.Text);
                    SettingManager.SetParam("SEO.DefaultMetaDescription", txtDefaulSEODescription.Text);
                    SettingManager.SetParam("SEO.DefaultMetaKeywords", txtDefaulSEOKeywords.Text);
                    SettingManager.SetParam("Display.PublicStoreTheme", ctrlThemeSelector.SelectedTheme);
                    if (fileFavicon.HasFile)
                    {
                        HttpPostedFile postedFile = fileFavicon.PostedFile;
                        if (!postedFile.ContentType.Equals("image/x-icon"))
                        {
                            throw new NopException("Image format not recognized, allowed formats are: .ico");
                        }
                        postedFile.SaveAs(HttpContext.Current.Request.PhysicalApplicationPath + "favicon.ico");
                    }
                    SettingManager.SetParam("Display.ShowWelcomeMessageOnMainPage", cbShowWelcomeMessage.Checked.ToString());
                    SettingManager.SetParam("Display.ShowNewsHeaderRssURL", cbShowNewsHeaderRssURL.Checked.ToString());
                    SettingManager.SetParam("Display.ShowBlogHeaderRssURL", cbShowBlogHeaderRssURL.Checked.ToString());
                    SettingManager.SetParam("SEO.Product.UrlRewriteFormat", txtProductUrlRewriteFormat.Text);
                    SettingManager.SetParam("SEO.Category.UrlRewriteFormat", txtCategoryUrlRewriteFormat.Text);
                    SettingManager.SetParam("SEO.Manufacturer.UrlRewriteFormat", txtManufacturerUrlRewriteFormat.Text);
                    SettingManager.SetParam("SEO.News.UrlRewriteFormat", txtNewsUrlRewriteFormat.Text);
                    SettingManager.SetParam("SEO.Blog.UrlRewriteFormat", txtBlogUrlRewriteFormat.Text);
                    SettingManager.SetParam("SEO.Topic.UrlRewriteFormat", txtTopicUrlRewriteFormat.Text);
                    SettingManager.SetParam("SEO.Forum.UrlRewriteFormat", txtForumUrlRewriteFormat.Text);
                    SettingManager.SetParam("SEO.ForumGroup.UrlRewriteFormat", txtForumGroupUrlRewriteFormat.Text);
                    SettingManager.SetParam("SEO.ForumTopic.UrlRewriteFormat", txtForumTopicUrlRewriteFormat.Text);


                    SettingManager.SetParam("Media.MaximumImageSize", txtMaxImageSize.Value.ToString());
                    SettingManager.SetParam("Media.Product.ThumbnailImageSize", txtProductThumbSize.Value.ToString());
                    SettingManager.SetParam("Media.Product.DetailImageSize", txtProductDetailSize.Value.ToString());
                    SettingManager.SetParam("Media.Product.VariantImageSize", txtProductVariantSize.Value.ToString());
                    SettingManager.SetParam("Media.Category.ThumbnailImageSize", txtCategoryThumbSize.Value.ToString());
                    SettingManager.SetParam("Media.Manufacturer.ThumbnailImageSize", txtManufacturerThumbSize.Value.ToString());
                    SettingManager.SetParam("Display.ShowProductImagesOnShoppingCart", cbShowCartImages.Checked.ToString());
                    SettingManager.SetParam("Display.ShowProductImagesOnWishList", cbShowWishListImages.Checked.ToString());
                    SettingManager.SetParam("Media.ShoppingCart.ThumbnailImageSize", txtShoppingCartThumbSize.Value.ToString());


                    MessageManager.AdminEmailAddress = txtAdminEmailAddress.Text;
                    MessageManager.AdminEmailDisplayName = txtAdminEmailDisplayName.Text;
                    MessageManager.AdminEmailHost = txtAdminEmailHost.Text;
                    if (!String.IsNullOrEmpty(txtAdminEmailPort.Text))
                        MessageManager.AdminEmailPort = int.Parse(txtAdminEmailPort.Text);
                    else
                        MessageManager.AdminEmailPort = 0;
                    MessageManager.AdminEmailUser = txtAdminEmailUser.Text;
                    MessageManager.AdminEmailPassword = txtAdminEmailPassword.Text;
                    MessageManager.AdminEmailEnableSsl = cbAdminEmailEnableSsl.Checked;
                    MessageManager.AdminEmailUseDefaultCredentials = cbAdminEmailUseDefaultCredentials.Checked;

                    SettingManager.SetParam("Security.AdminAreaAllowedIP", ipList);

                    SettingManager.SetParam("Common.LoginCaptchaImageEnabled", cbEnableLoginCaptchaImage.Checked.ToString());
                    SettingManager.SetParam("Common.RegisterCaptchaImageEnabled", cbEnableRegisterCaptchaImage.Checked.ToString());


                    CustomerManager.CustomerNameFormatting = (CustomerNameFormatEnum)Enum.ToObject(typeof(CustomerNameFormatEnum), int.Parse(this.ddlCustomerNameFormat.SelectedItem.Value));
                    CustomerManager.ShowCustomersLocation = cbShowCustomersLocation.Checked;
                    CustomerManager.ShowCustomersJoinDate = cbShowCustomersJoinDate.Checked;
                    ForumManager.AllowPrivateMessages = cbAllowPM.Checked;
                    CustomerManager.AllowViewingProfiles = cbAllowViewingProfiles.Checked;
                    CustomerManager.AllowCustomersToUploadAvatars = cbCustomersAllowedToUploadAvatars.Checked;
                    CustomerManager.DefaultAvatarEnabled = cbDefaultAvatarEnabled.Checked;
                    string defaultStoreTimeZoneId = ddlDefaultStoreTimeZone.SelectedItem.Value;
                    DateTimeHelper.DefaultStoreTimeZone = DateTimeHelper.FindTimeZoneById(defaultStoreTimeZoneId);
                    DateTimeHelper.AllowCustomersToSetTimeZone = cbAllowCustomersToSetTimeZone.Checked;


                    CustomerManager.UsernamesEnabled = cbUsernamesEnabled.Checked;
                    CustomerManager.CustomerRegistrationType = (CustomerRegistrationTypeEnum)Enum.ToObject(typeof(CustomerRegistrationTypeEnum), int.Parse(this.ddlRegistrationMethod.SelectedItem.Value));
                    CustomerManager.AllowNavigationOnlyRegisteredCustomers = cbAllowNavigationOnlyRegisteredCustomers.Checked;
                    ProductManager.CompareProductsEnabled = cbEnableCompareProducts.Checked;
                    SettingManager.SetParam("Common.EnableWishlist", cbEnableWishlist.Checked.ToString());
                    OrderManager.IsReOrderAllowed = cbIsReOrderAllowed.Checked;
                    SettingManager.SetParam("Common.EnableEmailAFirend", cbEnableEmailAFriend.Checked.ToString());
                    SettingManager.SetParam("Common.ShowMiniShoppingCart", cbShowMiniShoppingCart.Checked.ToString());
                    ProductManager.RecentlyViewedProductsEnabled = cbRecentlyViewedProductsEnabled.Checked;
                    ProductManager.RecentlyViewedProductsNumber = txtRecentlyViewedProductsNumber.Value;
                    ProductManager.RecentlyAddedProductsEnabled = cbRecentlyAddedProductsEnabled.Checked;
                    ProductManager.RecentlyAddedProductsNumber = txtRecentlyAddedProductsNumber.Value;
                    ProductManager.NotifyAboutNewProductReviews = cbNotifyAboutNewProductReviews.Checked;
                    CustomerManager.ProductReviewsMustBeApproved = cbProductReviewsMustBeApproved.Checked;
                    CustomerManager.AllowAnonymousUsersToReviewProduct = cbAllowAnonymousUsersToReviewProduct.Checked;
                    CustomerManager.AllowAnonymousUsersToSetProductRatings = cbAllowAnonymousUsersToSetProductRatings.Checked;                    
                    SettingManager.SetParam("Display.ShowBestsellersOnMainPage", cbShowBestsellersOnHomePage.Checked.ToString());
                    SettingManager.SetParam("Display.ShowBestsellersOnMainPageNumber", txtShowBestsellersOnHomePageNumber.Value.ToString());
                    ProductManager.ProductsAlsoPurchasedEnabled = cbProductsAlsoPurchased.Checked;
                    ProductManager.ProductsAlsoPurchasedNumber = txtProductsAlsoPurchasedNumber.Value;

                    SMSManager.IsSMSAlertsEnabled = cbIsSMSAlertsEnabled.Checked;
                    SMSManager.PhoneNumber = txtSMSAlertsPhoneNumber.Text;
                    SMSManager.ClickatellAPIId = txtSMSAlertsClickatellAPIId.Text;
                    SMSManager.ClickatellUsername = txtSMSAlertsClickatellUsername.Text;
                    SMSManager.ClickatellPassword = txtSMSAlertsClickatellPassword.Text;

                    SettingManager.SetParam("LiveChat.Enabled", cbLiveChatEnabled.Checked.ToString());
                    SettingManager.SetParam("LiveChat.BtnCode", txtLiveChatBtnCode.Text);
                    SettingManager.SetParam("LiveChat.MonCode", txtLiveChatMonCode.Text);

                    if(uplPdfLogo.HasFile)
                    {
                        HttpPostedFile postedFile = uplPdfLogo.PostedFile;
                        if(!postedFile.ContentType.Equals("image/jpeg") && !postedFile.ContentType.Equals("image/gif") && !postedFile.ContentType.Equals("image/png"))
                        {
                            throw new NopException("Image format not recognized, allowed formats are: .png, .jpg, .jpeg, .gif");
                        }
                        postedFile.SaveAs(PDFHelper.LogoFilePath);
                    }

                    CustomerActivityManager.InsertActivity(
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

        protected void btnSendTestEmail_Click(object sender, EventArgs e)
        {
            if (Page.IsValid)
            {
                try
                {
                    MailAddress from = new MailAddress(MessageManager.AdminEmailAddress, MessageManager.AdminEmailDisplayName);
                    MailAddress to = new MailAddress(txtSendEmailTo.Text);
                    MessageManager.SendEmail("Test message", "Test message", from, to);
                    lblSendTestEmailResult.Text = GetLocaleResourceString("Admin.GlobalSettings.MailSettings.SendTestEmailSuccess");
                }
                catch (Exception exc)
                {
                    lblSendTestEmailResult.Text = exc.Message;
                }
            }
        }

        protected void BtnSendTestSMS_OnClick(object sender, EventArgs e)
        {
            if(Page.IsValid)
            {
                try
                {
                    if(SMSManager.Send("Test message", txtTestPhone.Text))
                    {
                        lblSendTestSmsResult.Text = GetLocaleResourceString("Admin.GlobalSettings.SMSAlerts.SendTestSMSSuccess");
                    }
                    else
                    {
                        lblSendTestSmsResult.Text = GetLocaleResourceString("Admin.GlobalSettings.SMSAlerts.SendTestSMSFail");
                    }
                }
                catch(Exception exc)
                {
                    lblSendTestSmsResult.Text = exc.Message;
                }
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
                ShowError(ex.Message);
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
