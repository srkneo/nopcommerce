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
using NopSolutions.NopCommerce.BusinessLogic.Products.Attributes;
using NopSolutions.NopCommerce.Common.Utils;
using NopSolutions.NopCommerce.BusinessLogic.Tax;

namespace NopSolutions.NopCommerce.Web.Administration.Modules
{
    public partial class CheckoutAttributeInfoControl : BaseNopAdministrationUserControl
    {
        private void BindData()
        {
            var checkoutAttribute = CheckoutAttributeManager.GetCheckoutAttributeByID(this.CheckoutAttributeID, 0);

            if (this.HasLocalizableContent)
            {
                var languages = this.GetLocalizableLanguagesSupported();
                rptrLanguageTabs.DataSource = languages;
                rptrLanguageTabs.DataBind();
                rptrLanguageDivs.DataSource = languages;
                rptrLanguageDivs.DataBind();
            }

            if (checkoutAttribute != null)
            {
                this.txtName.Text = checkoutAttribute.Name;
                this.txtTextPrompt.Text = checkoutAttribute.TextPrompt;
                this.cbAttributeRequired.Checked = checkoutAttribute.IsRequired;
                this.cbShippableProductRequired.Checked = checkoutAttribute.ShippableProductRequired;
                this.cbIsTaxExempt.Checked = checkoutAttribute.IsTaxExempt;
                CommonHelper.SelectListItem(this.ddlTaxCategory, checkoutAttribute.TaxCategoryID);
                CommonHelper.SelectListItem(this.ddlAttributeControlType, checkoutAttribute.AttributeControlTypeID);
                this.txtDisplayOrder.Value = checkoutAttribute.DisplayOrder;
            }
        }

        private void FillDropDowns()
        {
            this.ddlTaxCategory.Items.Clear();
            ListItem itemTaxCategory = new ListItem("---", "0");
            this.ddlTaxCategory.Items.Add(itemTaxCategory);
            TaxCategoryCollection taxCategoryCollection = TaxCategoryManager.GetAllTaxCategories();
            foreach (TaxCategory taxCategory in taxCategoryCollection)
            {
                ListItem item2 = new ListItem(taxCategory.Name, taxCategory.TaxCategoryID.ToString());
                this.ddlTaxCategory.Items.Add(item2);
            }

            CommonHelper.FillDropDownWithEnum(ddlAttributeControlType, typeof(AttributeControlTypeEnum));
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                this.FillDropDowns();
                this.BindData();
            }
        }

        protected override void OnPreRender(EventArgs e)
        {
            string jquery = CommonHelper.GetStoreLocation() + "Scripts/jquery-1.4.min.js";
            Page.ClientScript.RegisterClientScriptInclude(jquery, jquery);

            string jqueryTabs = CommonHelper.GetStoreLocation() + "Scripts/jquery.idTabs.min.js";
            Page.ClientScript.RegisterClientScriptInclude(jqueryTabs, jqueryTabs);

            base.OnPreRender(e);
        }

        public CheckoutAttribute SaveInfo()
        {
            string name = txtName.Text;
            string textPrompt = txtTextPrompt.Text;
            int taxCategoryID = int.Parse(this.ddlTaxCategory.SelectedItem.Value);
            bool isRequired = cbAttributeRequired.Checked;
            bool shippableProductRequired = cbShippableProductRequired.Checked;
            bool isTaxExempt = cbIsTaxExempt.Checked;
            int attributeControlTypeID = int.Parse(this.ddlAttributeControlType.SelectedItem.Value);
            int displayOrder = txtDisplayOrder.Value;

            var checkoutAttribute = CheckoutAttributeManager.GetCheckoutAttributeByID(this.CheckoutAttributeID, 0);
            if (checkoutAttribute != null)
            {
                checkoutAttribute = CheckoutAttributeManager.UpdateCheckoutAttribute(checkoutAttribute.CheckoutAttributeID,
                     name, textPrompt, isRequired, shippableProductRequired,
                     isTaxExempt, taxCategoryID, attributeControlTypeID, displayOrder);
            }
            else
            {
                checkoutAttribute = CheckoutAttributeManager.InsertCheckoutAttribute(name,
                    textPrompt, isRequired, shippableProductRequired,
                    isTaxExempt, taxCategoryID, attributeControlTypeID, displayOrder);
            }

            saveLocalizableContent(checkoutAttribute);

            return checkoutAttribute;
        }

        protected void saveLocalizableContent(CheckoutAttribute checkoutAttribute)
        {
            if (checkoutAttribute == null)
                return;

            if (!this.HasLocalizableContent)
                return;

            foreach (RepeaterItem item in rptrLanguageDivs.Items)
            {
                if (item.ItemType == ListItemType.Item || item.ItemType == ListItemType.AlternatingItem)
                {
                    var txtLocalizedName = (TextBox)item.FindControl("txtLocalizedName");
                    var txtLocalizedTextPrompt = (TextBox)item.FindControl("txtLocalizedTextPrompt");
                    var lblLanguageId = (Label)item.FindControl("lblLanguageId");

                    int languageID = int.Parse(lblLanguageId.Text);
                    string name = txtLocalizedName.Text;
                    string textPrompt = txtLocalizedTextPrompt.Text;

                    bool allFieldsAreEmpty = (string.IsNullOrEmpty(name) && string.IsNullOrEmpty(textPrompt));

                    var content = CheckoutAttributeManager.GetCheckoutAttributeLocalizedByCheckoutAttributeIDAndLanguageID(checkoutAttribute.CheckoutAttributeID, languageID);
                    if (content == null)
                    {
                        if (!allFieldsAreEmpty && languageID > 0)
                        {
                            //only insert if one of the fields are filled out (avoid too many empty records in db...)
                            content = CheckoutAttributeManager.InsertCheckoutAttributeLocalized(checkoutAttribute.CheckoutAttributeID,
                                   languageID, name, textPrompt);
                        }
                    }
                    else
                    {
                        if (languageID > 0)
                        {
                            content = CheckoutAttributeManager.UpdateCheckoutAttributeLocalized(content.CheckoutAttributeLocalizedID, content.CheckoutAttributeID,
                                languageID, name, textPrompt);
                        }
                    }
                }
            }
        }

        protected void rptrLanguageDivs_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                var txtLocalizedName = (TextBox)e.Item.FindControl("txtLocalizedName");
                var txtLocalizedTextPrompt = (TextBox)e.Item.FindControl("txtLocalizedTextPrompt");
                var lblLanguageId = (Label)e.Item.FindControl("lblLanguageId");

                int languageID = int.Parse(lblLanguageId.Text);

                var content = CheckoutAttributeManager.GetCheckoutAttributeLocalizedByCheckoutAttributeIDAndLanguageID(this.CheckoutAttributeID, languageID);

                if (content != null)
                {
                    txtLocalizedName.Text = content.Name;
                    txtLocalizedTextPrompt.Text = content.TextPrompt;
                }

            }
        }
        
        public int CheckoutAttributeID
        {
            get
            {
                return CommonHelper.QueryStringInt("CheckoutAttributeID");
            }
        }
    }
}