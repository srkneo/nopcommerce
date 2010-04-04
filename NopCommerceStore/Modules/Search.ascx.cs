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
using System.ComponentModel;
using System.Configuration;
using System.Data;
using System.Text;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using NopSolutions.NopCommerce.BusinessLogic;
using NopSolutions.NopCommerce.BusinessLogic.Audit;
using NopSolutions.NopCommerce.BusinessLogic.Products;
using NopSolutions.NopCommerce.Common.Utils;
using NopSolutions.NopCommerce.BusinessLogic.Localization;
using NopSolutions.NopCommerce.Common;
using NopSolutions.NopCommerce.BusinessLogic.Configuration.Settings;

namespace NopSolutions.NopCommerce.Web.Modules
{
    public partial class SearchControl : BaseNopUserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                if (!String.IsNullOrEmpty(this.SearchTerms))
                    txtSearchTerm.Text = this.SearchTerms;
                BindData();
            }
        }

        protected void BindData()
        {
            try
            {
                string keywords = txtSearchTerm.Text.Trim();

                if (!String.IsNullOrEmpty(keywords))
                {
                    int searchTermMinimumLength = SettingManager.GetSettingValueInteger("Search.ProductSearchTermMinimumLength", 3);
                    if (keywords.Length < searchTermMinimumLength)
                        throw new NopException(string.Format(LocalizationManager.GetLocaleResourceString("Search.SearchTermMinimumLengthIsNCharacters"), searchTermMinimumLength));

                    int totalRecords = 0;
                    var products = ProductManager.GetAllProducts(keywords,
                        chSearchInProductDescriptions.Checked, 100, 0, out totalRecords);

                    lvProducts.DataSource = products;
                    lvProducts.DataBind();
                    lvProducts.Visible = products.Count > 0;
                    pagerProducts.Visible = products.Count > pagerProducts.PageSize;
                    lblNoResults.Visible = !lvProducts.Visible;
                    
                    int customerID = 0;
                    if (NopContext.Current.User != null)
                        customerID = NopContext.Current.User.CustomerID;
                    SearchLogManager.InsertSearchLog(txtSearchTerm.Text, customerID, DateTime.Now);
                }
                else
                {
                    pagerProducts.Visible = false;
                    lvProducts.Visible = false;
                }
            }
            catch (Exception exc)
            {
                lvProducts.Visible = false;
                pagerProducts.Visible = false;
                lblError.Text = Server.HtmlEncode(exc.Message);
            }
        }

        protected void lvProducts_OnPagePropertiesChanging(object sender, PagePropertiesChangingEventArgs e)
        {
            this.pagerProducts.SetPageProperties(e.StartRowIndex, e.MaximumRows, false);
            BindData();
        }

        protected void btnSearch_Click(object sender, EventArgs e)
        {
            BindData();
        }

        protected override void OnPreRender(EventArgs e)
        {
            txtSearchTerm.Attributes.Add("onkeydown", "if(event.which || event.keyCode){if ((event.which == 13) || (event.keyCode == 13)) {document.getElementById('" + btnSearch.ClientID + "').click();return false;}} else {return true}; ");
            base.OnPreRender(e);
        }

        public string SearchTerms
        {
            get
            {
                return CommonHelper.QueryString("SearchTerms");
            }
        }
    }
}