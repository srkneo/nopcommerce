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
            SpecificationAttribute specificationAttribute = SpecificationAttributeManager.GetSpecificationAttributeById(this.SpecificationAttributeId, 0);

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

            SpecificationAttributeOptionCollection saoCol = SpecificationAttributeManager.GetSpecificationAttributeOptionsBySpecificationAttribute(SpecificationAttributeId, 0);
            grdSpecificationAttributeOptions.DataSource = saoCol;
            grdSpecificationAttributeOptions.DataBind();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                this.BindData();
            }

            if (this.SpecificationAttributeId <= 0)
                pnlSpecAttrOptions.Visible = false;
        }

        protected override void OnPreRender(EventArgs e)
        {
            BindJQuery();
            BindJQueryIdTabs();

            base.OnPreRender(e);
        }

        protected void btnAddSpecificationAttributeOption_Click(object sender, EventArgs e)
        {
            Response.Redirect("SpecificationAttributeOptionAdd.aspx?SpecificationAttributeID=" + this.SpecificationAttributeId);
        }

        public SpecificationAttribute SaveInfo()
        {
            SpecificationAttribute specificationAttribute = SpecificationAttributeManager.GetSpecificationAttributeById(this.SpecificationAttributeId, 0);

            if (specificationAttribute != null)
            {
                specificationAttribute = SpecificationAttributeManager.UpdateSpecificationAttribute(specificationAttribute.SpecificationAttributeId, txtName.Text, txtDisplayOrder.Value);
            }
            else
            {
                specificationAttribute = SpecificationAttributeManager.InsertSpecificationAttribute(txtName.Text, txtDisplayOrder.Value);
            }

            SaveLocalizableContent(specificationAttribute);

            return specificationAttribute;
        }

        protected void SaveLocalizableContent(SpecificationAttribute specificationAttribute)
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

                    int languageId = int.Parse(lblLanguageId.Text);
                    string name = txtLocalizedName.Text;

                    bool allFieldsAreEmpty = string.IsNullOrEmpty(name);

                    var content = SpecificationAttributeManager.GetSpecificationAttributeLocalizedBySpecificationAttributeIdAndLanguageId(specificationAttribute.SpecificationAttributeId, languageId);
                    if (content == null)
                    {
                        if (!allFieldsAreEmpty && languageId > 0)
                        {
                            //only insert if one of the fields are filled out (avoid too many empty records in db...)
                            content = SpecificationAttributeManager.InsertSpecificationAttributeLocalized(specificationAttribute.SpecificationAttributeId,
                                   languageId, name);
                        }
                    }
                    else
                    {
                        if (languageId > 0)
                        {
                            content = SpecificationAttributeManager.UpdateSpecificationAttributeLocalized(content.SpecificationAttributeLocalizedId, content.SpecificationAttributeId,
                                languageId, name);
                        }
                    }
                }
            }
        }

        protected void SaveLocalizableContent(SpecificationAttributeOption sao)
        {
            if (sao == null)
                return;

            if (!this.HasLocalizableContent)
                return;

            foreach (GridViewRow row in grdSpecificationAttributeOptions.Rows)
            {
                Repeater rptrLanguageDivs2 = row.FindControl("rptrLanguageDivs2") as Repeater;
                if (rptrLanguageDivs2 != null)
                {
                    HiddenField hfSpecificationAttributeOptionId = row.FindControl("hfSpecificationAttributeOptionId") as HiddenField;
                    int saoId = int.Parse(hfSpecificationAttributeOptionId.Value);
                    if (saoId == sao.SpecificationAttributeOptionId)
                    {
                        foreach (RepeaterItem item in rptrLanguageDivs2.Items)
                        {
                            if (item.ItemType == ListItemType.Item || item.ItemType == ListItemType.AlternatingItem)
                            {
                                var txtLocalizedOptionName = (TextBox)item.FindControl("txtLocalizedOptionName");
                                var lblLanguageId = (Label)item.FindControl("lblLanguageId");

                                int languageId = int.Parse(lblLanguageId.Text);
                                string name = txtLocalizedOptionName.Text;

                                bool allFieldsAreEmpty = string.IsNullOrEmpty(name);

                                var content = SpecificationAttributeManager.GetSpecificationAttributeOptionLocalizedBySpecificationAttributeOptionIdAndLanguageId(sao.SpecificationAttributeOptionId, languageId);
                                if (content == null)
                                {
                                    if (!allFieldsAreEmpty && languageId > 0)
                                    {
                                        //only insert if one of the fields are filled out (avoid too many empty records in db...)
                                        content = SpecificationAttributeManager.InsertSpecificationAttributeOptionLocalized(sao.SpecificationAttributeOptionId,
                                               languageId, name);
                                    }
                                }
                                else
                                {
                                    if (languageId > 0)
                                    {
                                        content = SpecificationAttributeManager.UpdateSpecificationAttributeOptionLocalized(content.SpecificationAttributeOptionLocalizedId,
                                            content.SpecificationAttributeOptionId, languageId, name);
                                    }
                                }
                            }
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

                int languageId = int.Parse(lblLanguageId.Text);

                var content = SpecificationAttributeManager.GetSpecificationAttributeLocalizedBySpecificationAttributeIdAndLanguageId(this.SpecificationAttributeId, languageId);

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
                HiddenField hfSpecificationAttributeOptionId = row.FindControl("hfSpecificationAttributeOptionId") as HiddenField;

                string name = txtName.Text;
                int displayOrder = txtDisplayOrder.Value;
                int saoId = int.Parse(hfSpecificationAttributeOptionId.Value);

                SpecificationAttributeOption sao = SpecificationAttributeManager.GetSpecificationAttributeOptionById(saoId, 0);
                if (sao != null)
                {
                    sao = SpecificationAttributeManager.UpdateSpecificationAttributeOptions(saoId, SpecificationAttributeId, name, displayOrder);
                    SaveLocalizableContent(sao);
                }

                BindData();
            }
        }

        protected void OnSpecificationAttributeOptionsDeleting(object sender, GridViewDeleteEventArgs e)
        {
            int saoId = (int)grdSpecificationAttributeOptions.DataKeys[e.RowIndex]["SpecificationAttributeOptionId"];
            SpecificationAttributeOption sao = SpecificationAttributeManager.GetSpecificationAttributeOptionById(saoId, 0);
            if (sao != null)
            {
                SpecificationAttributeManager.DeleteSpecificationAttributeOption(sao.SpecificationAttributeOptionId);
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

                Repeater rptrLanguageDivs2 = e.Row.FindControl("rptrLanguageDivs2") as Repeater;
                if (rptrLanguageDivs2 != null)
                {
                    if (this.HasLocalizableContent)
                    {
                        var languages = this.GetLocalizableLanguagesSupported();
                        rptrLanguageDivs2.DataSource = languages;
                        rptrLanguageDivs2.DataBind();
                    }
                }
            }
        }

        protected void rptrLanguageDivs2_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                var txtLocalizedOptionName = (TextBox)e.Item.FindControl("txtLocalizedOptionName");
                var lblLanguageId = (Label)e.Item.FindControl("lblLanguageId");
                var hfSpecificationAttributeOptionId = (HiddenField)e.Item.Parent.Parent.FindControl("hfSpecificationAttributeOptionId");

                int languageId = int.Parse(lblLanguageId.Text);
                int saoId = Convert.ToInt32(hfSpecificationAttributeOptionId.Value);
                SpecificationAttributeOption sao = SpecificationAttributeManager.GetSpecificationAttributeOptionById(saoId, 0);
                if (sao != null)
                {
                    var content = SpecificationAttributeManager.GetSpecificationAttributeOptionLocalizedBySpecificationAttributeOptionIdAndLanguageId(saoId, languageId);
                    if (content != null)
                    {
                        txtLocalizedOptionName.Text = content.Name;
                    }
                }
            }
        }

        public int SpecificationAttributeId
        {
            get
            {
                return CommonHelper.QueryStringInt("SpecificationAttributeId");
            }
        }
    }
}