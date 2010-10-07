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
using AjaxControlToolkit;
using NopSolutions.NopCommerce.BusinessLogic;
using NopSolutions.NopCommerce.BusinessLogic.Categories;
using NopSolutions.NopCommerce.BusinessLogic.Products;
using NopSolutions.NopCommerce.BusinessLogic.SEO;
using NopSolutions.NopCommerce.Common.Utils;

namespace NopSolutions.NopCommerce.Web.Modules
{
    public partial class ProductCategoryBreadcrumb : BaseNopUserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
                BindData();
        }

        protected void BindData()
        {
            var product = ProductManager.GetProductById(this.ProductId);
            if (product != null)
            {
                hlProduct.NavigateUrl = SEOHelper.GetProductUrl(product);

                var productCategories = product.ProductCategories;
                if (productCategories.Count > 0)
                {
                    var category = productCategories[0].Category;
                    if (category != null)
                    {
                        var breadCrumb = CategoryManager.GetBreadCrumb(category.CategoryId);
                        if (breadCrumb.Count > 0)
                        {
                            rptrCategoryBreadcrumb.DataSource = breadCrumb;
                            rptrCategoryBreadcrumb.DataBind();
                        }
                    }
                }

                hlProduct.Text = Server.HtmlEncode(product.LocalizedName);
            }
            else
                this.Visible = false;
        }

        public int ProductId
        {
            get
            {
                return CommonHelper.QueryStringInt("ProductId");
            }
        }
    }
}