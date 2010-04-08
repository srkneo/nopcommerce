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
using System.Web.UI;
using NopSolutions.NopCommerce.BusinessLogic.Products.Specs;
using NopSolutions.NopCommerce.Common.Utils;
using System.Web.UI.WebControls;

namespace NopSolutions.NopCommerce.Web.Administration.Modules
{
    public partial class SpecificationAttributeInfoControl : BaseNopAdministrationUserControl
    {
        private void BindData()
        {
            SpecificationAttribute specificationAttribute = SpecificationAttributeManager.GetSpecificationAttributeByID(this.SpecificationAttributeID, 0);

            if (this.HasLocalizableContent)
            {
                var languages = this.GetLocalizableLanguagesSupported();
                rptrLanguageTabs.DataSource = languages;
                rptrLanguageTabs.DataBind();
                rptrLanguageDivs.DataSource = languages;
                rptrLanguageDivs.DataBind();
            }

            if (specificationAttribute != null)
            {
                this.txtName.Text = specificationAttribute.Name;
                this.txtDisplayOrder.Value = specificationAttribute.DisplayOrder;
            }

            SpecificationAttributeOptionCollection saoCol = SpecificationAttributeManager.GetSpecificationAttributeOptionsBySpecificationAttribute(SpecificationAttributeID, 0);
            grdSpecificationAttributeOptions.DataSource = saoCol;
            grdSpecificationAttributeOptions.DataBind();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                this.BindData();
            }

            if (SpecificationAttributeID <= 0)
                pnlSpecAttrOptions.Visible = false;
        }

        protected override void OnPreRender(EventArgs e)
        {
            string jquery = CommonHelper.GetStoreLocation() + "Scripts/jquery-1.4.min.js";
            Page.ClientScript.RegisterClientScriptInclude(jquery, jquery);

            string jqueryTabs = CommonHelper.GetStoreLocation() + "Scripts/jquery.idTabs.min.js";
            Page.ClientScript.RegisterClientScriptInclude(jqueryTabs, jqueryTabs);

            base.OnPreRender(e);
        }

        protected void btnAddSpecificationAttributeOption_Click(object sender, EventArgs e)
        {
            Response.Redirect("SpecificationAttributeOptionAdd.aspx?SpecificationAttributeID=" + this.SpecificationAttributeID);
        }

        public SpecificationAttribute SaveInfo()
        {
            SpecificationAttribute specificationAttribute = SpecificationAttributeManager.GetSpecificationAttributeByID(this.SpecificationAttributeID, 0);

            if (specificationAttribute != null)
            {
                specificationAttribute = SpecificationAttributeManager.UpdateSpecificationAttribute(specificationAttribute.SpecificationAttributeID, txtName.Text, txtDisplayOrder.Value);
            }
            else
            {
                specificationAttribute = SpecificationAttributeManager.InsertSpecificationAttribute(txtName.Text, txtDisplayOrder.Value);
            }

            saveLocalizableContent(specificationAttribute);

            return specificationAttribute;
        }

        protected void saveLocalizableContent(SpecificationAttribute specificationAttribute)
        {
            if (specificationAttribute == null)
                return;

            if (!this.HasLocalizableContent)
                return;

            foreach (RepeaterItem item in rptrLanguageDivs.Items)
            {
                if (item.ItemType == ListItemType.Item || item.ItemType == ListItemType.AlternatingItem)
                {
                    var txtLocalizedName = (TextBox)item.FindControl("txtLocalizedName");
                    var lblLanguageId = (Label)item.FindControl("lblLanguageId");

                    int languageID = int.Parse(lblLanguageId.Text);
                    string name = txtLocalizedName.Text;

                    bool allFieldsAreEmpty = string.IsNullOrEmpty(name);

                    var content = SpecificationAttributeManager.GetSpecificationAttributeLocalizedBySpecificationAttributeIDAndLanguageID(specificationAttribute.SpecificationAttributeID, languageID);
                    if (content == null)
                    {
                        if (!allFieldsAreEmpty && languageID > 0)
                        {
                            //only insert if one of the fields are filled out (avoid too many empty records in db...)
                            content = SpecificationAttributeManager.InsertSpecificationAttributeLocalized(specificationAttribute.SpecificationAttributeID,
                                   languageID, name);
                        }
                    }
                    else
                    {
                        if (languageID > 0)
                        {
                            content = SpecificationAttributeManager.UpdateSpecificationAttributeLocalized(content.SpecificationAttributeLocalizedID, content.SpecificationAttributeID,
                                languageID, name);
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
                var lblLanguageId = (Label)e.Item.FindControl("lblLanguageId");

                int languageID = int.Parse(lblLanguageId.Text);

                var content = SpecificationAttributeManager.GetSpecificationAttributeLocalizedBySpecificationAttributeIDAndLanguageID(this.SpecificationAttributeID, languageID);

                if (content != null)
                {
                    txtLocalizedName.Text = content.Name;
                }

            }
        }


        protected void OnSpecificationAttributeOptionsCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "UpdateOption")
            {
                int index = Convert.ToInt32(e.CommandArgument);
                GridViewRow row = grdSpecificationAttributeOptions.Rows[index];
                SimpleTextBox txtName = row.FindControl("txtOptionName") as SimpleTextBox;
                NumericTextBox txtDisplayOrder = row.FindControl("txtOptionDisplayOrder") as NumericTextBox;
                HiddenField hfSpecificationAttributeOptionID = row.FindControl("hfSpecificationAttributeOptionID") as HiddenField;

                string name = txtName.Text;
                int displayOrder = txtDisplayOrder.Value;
                int saoID = int.Parse(hfSpecificationAttributeOptionID.Value);

                SpecificationAttributeOption sao = SpecificationAttributeManager.GetSpecificationAttributeOptionByID(saoID, 0);
                if (sao != null)
                    SpecificationAttributeManager.UpdateSpecificationAttributeOptions(saoID, SpecificationAttributeID, name, displayOrder);

                BindData();
            }
        }

        protected void OnSpecificationAttributeOptionsDeleting(object sender, GridViewDeleteEventArgs e)
        {
            int saoID = (int)grdSpecificationAttributeOptions.DataKeys[e.RowIndex]["SpecificationAttributeOptionID"];
            SpecificationAttributeOption sao = SpecificationAttributeManager.GetSpecificationAttributeOptionByID(saoID, 0);
            if (sao != null)
            {
                SpecificationAttributeManager.DeleteSpecificationAttributeOption(sao.SpecificationAttributeOptionID);
                BindData();
            }
        }

        protected void OnSpecificationAttributeOptionsDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                Button btnUpdate = e.Row.FindControl("btnUpdate") as Button;
                if (btnUpdate != null)
                    btnUpdate.CommandArgument = e.Row.RowIndex.ToString();
            }
        }

        public int SpecificationAttributeID
        {
            get
            {
                return CommonHelper.QueryStringInt("SpecificationAttributeID");
            }
        }
    }
}