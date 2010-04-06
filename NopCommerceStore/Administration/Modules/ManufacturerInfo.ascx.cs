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
using System.Configuration;
using System.Data;
using System.Threading;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using NopSolutions.NopCommerce.BusinessLogic.Manufacturers;
using NopSolutions.NopCommerce.BusinessLogic.Media;
using NopSolutions.NopCommerce.BusinessLogic.Products;
using NopSolutions.NopCommerce.BusinessLogic.Templates;
using NopSolutions.NopCommerce.Common.Utils;
using NopSolutions.NopCommerce.Web.Administration.Modules;

namespace NopSolutions.NopCommerce.Web.Administration.Modules
{
    public partial class ManufacturerInfoControl : BaseNopAdministrationUserControl
    {
        private void BindData()
        {
            var manufacturer = ManufacturerManager.GetManufacturerByID(this.ManufacturerID, 0);

            if (this.HasLocalizableContent)
            {
                var languages = this.GetLocalizableLanguagesSupported();
                rptrLanguageTabs.DataSource = languages;
                rptrLanguageTabs.DataBind();
                rptrLanguageDivs.DataSource = languages;
                rptrLanguageDivs.DataBind();
            }
            
            if (manufacturer != null)
            {
                this.txtName.Text = manufacturer.Name;
                this.txtDescription.Content = manufacturer.Description;
                CommonHelper.SelectListItem(this.ddlTemplate, manufacturer.TemplateID);

                var manufacturerPicture = manufacturer.Picture;
                btnRemoveManufacturerImage.Visible = manufacturerPicture != null;
                string pictureUrl = PictureManager.GetPictureUrl(manufacturerPicture, 100);
                this.iManufacturerPicture.Visible = true;
                this.iManufacturerPicture.ImageUrl = pictureUrl;

                this.txtPriceRanges.Text = manufacturer.PriceRanges;
                this.cbPublished.Checked = manufacturer.Published;
                this.txtDisplayOrder.Value = manufacturer.DisplayOrder;
            }
            else
            {
                this.btnRemoveManufacturerImage.Visible = false;
                this.iManufacturerPicture.Visible = false;
            }
        }

        private void FillDropDowns()
        {
            this.ddlTemplate.Items.Clear();
            var manufacturerTemplates = TemplateManager.GetAllManufacturerTemplates();
            foreach (var manufacturerTemplate in manufacturerTemplates)
            {
                ListItem item2 = new ListItem(manufacturerTemplate.Name, manufacturerTemplate.ManufacturerTemplateID.ToString());
                this.ddlTemplate.Items.Add(item2);
            }
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
        
        public Manufacturer SaveInfo()
        {
            var manufacturer = ManufacturerManager.GetManufacturerByID(this.ManufacturerID, 0);

            if (manufacturer != null)
            {
                Picture manufacturerPicture = manufacturer.Picture;
                HttpPostedFile manufacturerPictureFile = fuManufacturerPicture.PostedFile;
                if ((manufacturerPictureFile != null) && (!String.IsNullOrEmpty(manufacturerPictureFile.FileName)))
                {
                    byte[] manufacturerPictureBinary = PictureManager.GetPictureBits(manufacturerPictureFile.InputStream, manufacturerPictureFile.ContentLength);
                    if (manufacturerPicture != null)
                        manufacturerPicture = PictureManager.UpdatePicture(manufacturerPicture.PictureID, manufacturerPictureBinary, manufacturerPictureFile.ContentType, true);
                    else
                        manufacturerPicture = PictureManager.InsertPicture(manufacturerPictureBinary, manufacturerPictureFile.ContentType, true);
                }
                int manufacturerPictureID = 0;
                if (manufacturerPicture != null)
                    manufacturerPictureID = manufacturerPicture.PictureID;

                manufacturer = ManufacturerManager.UpdateManufacturer(manufacturer.ManufacturerID, txtName.Text,
                    txtDescription.Content, int.Parse(this.ddlTemplate.SelectedItem.Value),
                    manufacturer.MetaKeywords, manufacturer.MetaDescription,
                    manufacturer.MetaTitle, manufacturer.SEName,
                    manufacturerPictureID, manufacturer.PageSize, txtPriceRanges.Text,
                    cbPublished.Checked, manufacturer.Deleted, txtDisplayOrder.Value,
                    manufacturer.CreatedOn, DateTime.Now);
            }
            else
            {
                Picture manufacturerPicture = null;
                HttpPostedFile manufacturerPictureFile = fuManufacturerPicture.PostedFile;
                if ((manufacturerPictureFile != null) && (!String.IsNullOrEmpty(manufacturerPictureFile.FileName)))
                {
                    byte[] manufacturerPictureBinary = PictureManager.GetPictureBits(manufacturerPictureFile.InputStream, manufacturerPictureFile.ContentLength);
                    manufacturerPicture = PictureManager.InsertPicture(manufacturerPictureBinary, manufacturerPictureFile.ContentType, true);
                }
                int manufacturerPictureID = 0;
                if (manufacturerPicture != null)
                    manufacturerPictureID = manufacturerPicture.PictureID;

                DateTime nowDt = DateTime.Now;
                manufacturer = ManufacturerManager.InsertManufacturer(txtName.Text, txtDescription.Content,
                    int.Parse(this.ddlTemplate.SelectedItem.Value),
                    string.Empty, string.Empty, string.Empty, string.Empty,
                    manufacturerPictureID, 10, txtPriceRanges.Text, cbPublished.Checked, false, txtDisplayOrder.Value, nowDt, nowDt);
            }

            saveLocalizableContent(manufacturer);

            return manufacturer;
        }

        protected void saveLocalizableContent(Manufacturer manufacturer)
        {
            if (manufacturer == null)
                return;

            if (!this.HasLocalizableContent)
                return;

            foreach (RepeaterItem item in rptrLanguageDivs.Items)
            {
                if (item.ItemType == ListItemType.Item || item.ItemType == ListItemType.AlternatingItem)
                {
                    var txtLocalizedName = (TextBox)item.FindControl("txtLocalizedName");
                    var txtLocalizedDescription = (AjaxControlToolkit.HTMLEditor.Editor)item.FindControl("txtLocalizedDescription");
                    var lblLanguageId = (Label)item.FindControl("lblLanguageId");

                    int languageID = int.Parse(lblLanguageId.Text);
                    string name = txtLocalizedName.Text;
                    string description = txtLocalizedDescription.Content;

                    bool allFieldsAreEmpty = (string.IsNullOrEmpty(name) && string.IsNullOrEmpty(description));

                    var content = ManufacturerManager.GetManufacturerLocalizedByManufacturerIDAndLanguageID(manufacturer.ManufacturerID, languageID);
                    if (content == null)
                    {
                        if (!allFieldsAreEmpty && languageID > 0)
                        {
                            //only insert if one of the fields are filled out (avoid too many empty records in db...)
                            content = ManufacturerManager.InsertManufacturerLocalized(manufacturer.ManufacturerID,
                                   languageID, name, description, string.Empty, string.Empty,
                                   string.Empty, string.Empty);
                        }
                    }
                    else
                    {
                        if (languageID > 0)
                        {
                            content = ManufacturerManager.UpdateManufacturerLocalized(content.ManufacturerLocalizedID, content.ManufacturerID,
                                languageID, name, description,
                                content.MetaKeywords, content.MetaDescription,
                                content.MetaTitle, content.SEName);
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
                var txtLocalizedDescription = (AjaxControlToolkit.HTMLEditor.Editor)e.Item.FindControl("txtLocalizedDescription");
                var lblLanguageId = (Label)e.Item.FindControl("lblLanguageId");

                int languageID = int.Parse(lblLanguageId.Text);

                var content = ManufacturerManager.GetManufacturerLocalizedByManufacturerIDAndLanguageID(this.ManufacturerID, languageID);

                if (content != null)
                {
                    txtLocalizedName.Text = content.Name;
                    txtLocalizedDescription.Content = content.Description;
                }

            }
        }

        protected void btnRemoveManufacturerImage_Click(object sender, EventArgs e)
        {
            try
            {
                Manufacturer manufacturer = ManufacturerManager.GetManufacturerByID(this.ManufacturerID, 0);
                if (manufacturer != null)
                {
                    PictureManager.DeletePicture(manufacturer.PictureID);
                    ManufacturerManager.RemoveManufacturerPicture(manufacturer.ManufacturerID);
                    BindData();
                }
            }
            catch (Exception exc)
            {
                ProcessException(exc);
            }
        }

        public int ManufacturerID
        {
            get
            {
                return CommonHelper.QueryStringInt("ManufacturerID");
            }
        }
    }
}