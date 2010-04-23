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
using NopSolutions.NopCommerce.BusinessLogic.Products.Specs;
using NopSolutions.NopCommerce.Common.Utils;

namespace NopSolutions.NopCommerce.Web.Administration.Modules
{
    public partial class SpecificationAttributeOptionAddControl : BaseNopAdministrationUserControl
    {
        protected void BindData()
        {
            hlBack.NavigateUrl = "~/Administration/SpecificationAttributeDetails.aspx?SpecificationAttributeID=" + SpecificationAttributeId;

            if (this.HasLocalizableContent)
            {
                var languages = this.GetLocalizableLanguagesSupported();
                rptrLanguageTabs.DataSource = languages;
                rptrLanguageTabs.DataBind();
                rptrLanguageDivs.DataSource = languages;
                rptrLanguageDivs.DataBind();
            }

        }
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                BindData();
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
        
        protected void AddButton_Click(object sender, EventArgs e)
        {
            if (Page.IsValid)
            {
                try
                {
                    SpecificationAttributeOption sao = SpecificationAttributeManager.InsertSpecificationAttributeOption(SpecificationAttributeId, txtNewOptionName.Text, txtNewOptionDisplayOrder.Value);
                    SaveLocalizableContent(sao);
                    Response.Redirect("SpecificationAttributeDetails.aspx?SpecificationAttributeID=" + sao.SpecificationAttributeId.ToString());
                }
                catch (Exception exc)
                {
                    ProcessException(exc);
                }
            }
        }

        protected void SaveLocalizableContent(SpecificationAttributeOption sao)
        {
            if (sao == null)
                return;

            if (!this.HasLocalizableContent)
                return;

            foreach (RepeaterItem item in rptrLanguageDivs.Items)
            {
                if (item.ItemType == ListItemType.Item || item.ItemType == ListItemType.AlternatingItem)
                {
                    var txtNewLocalizedOptionName = (TextBox)item.FindControl("txtNewLocalizedOptionName");
                    var lblLanguageId = (Label)item.FindControl("lblLanguageId");

                    int languageId = int.Parse(lblLanguageId.Text);
                    string name = txtNewLocalizedOptionName.Text;

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

        protected void rptrLanguageDivs_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            
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