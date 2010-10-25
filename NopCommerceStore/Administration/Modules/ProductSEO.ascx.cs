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
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using NopSolutions.NopCommerce.BusinessLogic.Categories;
using NopSolutions.NopCommerce.BusinessLogic.Manufacturers;
using NopSolutions.NopCommerce.BusinessLogic.Media;
using NopSolutions.NopCommerce.BusinessLogic.Products;
using NopSolutions.NopCommerce.BusinessLogic.Products.Specs;
using NopSolutions.NopCommerce.BusinessLogic.Templates;
using NopSolutions.NopCommerce.Common.Utils;
using NopSolutions.NopCommerce.Web.Administration.Modules;
using NopSolutions.NopCommerce.BusinessLogic.IoC;

namespace NopSolutions.NopCommerce.Web.Administration.Modules
{
    public partial class ProductSEOControl : BaseNopAdministrationUserControl
    {
        private void BindData()
        {
            Product product = IoCFactory.Resolve<IProductManager>().GetProductById(this.ProductId);

            if (this.HasLocalizableContent)
            {
                var languages = this.GetLocalizableLanguagesSupported();
                rptrLanguageTabs.DataSource = languages;
                rptrLanguageTabs.DataBind();
                rptrLanguageDivs.DataSource = languages;
                rptrLanguageDivs.DataBind();
            }

            if (product != null)
            {
                this.txtMetaKeywords.Text = product.MetaKeywords;
                this.txtMetaDescription.Text = product.MetaDescription;
                this.txtMetaTitle.Text = product.MetaTitle;
                this.txtSEName.Text = product.SEName;
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                this.BindData();
            }
        }

        protected override void OnPreRender(EventArgs e)
        {
            BindJQuery();
            BindJQueryIdTabs();

            base.OnPreRender(e);
        }

        public void SaveInfo()
        {
            SaveInfo(this.ProductId);
        }

        public void SaveInfo(int prodId)
        {
            Product product = IoCFactory.Resolve<IProductManager>().GetProductById(prodId);
            if (product != null)
            {
                product.MetaKeywords = txtMetaKeywords.Text;
                product.MetaDescription = txtMetaDescription.Text;
                product.MetaTitle = txtMetaTitle.Text;
                product.SEName = txtSEName.Text;
                product.UpdatedOn = DateTime.UtcNow;
                IoCFactory.Resolve<IProductManager>().UpdateProduct(product);
            }

            SaveLocalizableContent(product);
        }

        protected void SaveLocalizableContent(Product product)
        {
            if (product == null)
                return;

            if (!this.HasLocalizableContent)
                return;

            foreach (RepeaterItem item in rptrLanguageDivs.Items)
            {
                if (item.ItemType == ListItemType.Item || item.ItemType == ListItemType.AlternatingItem)
                {
                    var txtLocalizedMetaKeywords = (TextBox)item.FindControl("txtLocalizedMetaKeywords");
                    var txtLocalizedMetaDescription = (TextBox)item.FindControl("txtLocalizedMetaDescription");
                    var txtLocalizedMetaTitle = (TextBox)item.FindControl("txtLocalizedMetaTitle");
                    var txtLocalizedSEName = (TextBox)item.FindControl("txtLocalizedSEName");
                    var lblLanguageId = (Label)item.FindControl("lblLanguageId");

                    int languageId = int.Parse(lblLanguageId.Text);
                    string metaKeywords = txtLocalizedMetaKeywords.Text;
                    string metaDescription = txtLocalizedMetaDescription.Text;
                    string metaTitle = txtLocalizedMetaTitle.Text;
                    string seName = txtLocalizedSEName.Text;

                    bool allFieldsAreEmpty = (string.IsNullOrEmpty(metaKeywords) &&
                        string.IsNullOrEmpty(metaDescription) &&
                        string.IsNullOrEmpty(metaTitle) &&
                        string.IsNullOrEmpty(seName));

                    var content = IoCFactory.Resolve<IProductManager>().GetProductLocalizedByProductIdAndLanguageId(product.ProductId, languageId);
                    if (content == null)
                    {
                        if (!allFieldsAreEmpty && languageId > 0)
                        {
                            //only insert if one of the fields are filled out (avoid too many empty records in db...)
                            content = new ProductLocalized();
                            content.ProductId = product.ProductId;
                            content.LanguageId = languageId;
                            content.MetaKeywords = metaKeywords;
                            content.MetaDescription = metaDescription;
                            content.MetaTitle = metaTitle;
                            content.SEName = seName;
                            IoCFactory.Resolve<IProductManager>().InsertProductLocalized(content);
                        }
                    }
                    else
                    {
                        if (languageId > 0)
                        {
                            content.LanguageId = languageId;
                            content.MetaKeywords = metaKeywords;
                            content.MetaDescription = metaDescription;
                            content.MetaTitle = metaTitle;
                            content.SEName = seName;
                            IoCFactory.Resolve<IProductManager>().UpdateProductLocalized(content);
                        }
                    }
                }
            }
        }

        protected void rptrLanguageDivs_ItemDataBound(object sender, RepeaterItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                var txtLocalizedMetaKeywords = (TextBox)e.Item.FindControl("txtLocalizedMetaKeywords");
                var txtLocalizedMetaDescription = (TextBox)e.Item.FindControl("txtLocalizedMetaDescription");
                var txtLocalizedMetaTitle = (TextBox)e.Item.FindControl("txtLocalizedMetaTitle");
                var txtLocalizedSEName = (TextBox)e.Item.FindControl("txtLocalizedSEName");
                var lblLanguageId = (Label)e.Item.FindControl("lblLanguageId");

                int languageId = int.Parse(lblLanguageId.Text);

                var content = IoCFactory.Resolve<IProductManager>().GetProductLocalizedByProductIdAndLanguageId(this.ProductId, languageId);
                if (content != null)
                {
                    txtLocalizedMetaKeywords.Text = content.MetaKeywords;
                    txtLocalizedMetaDescription.Text = content.MetaDescription;
                    txtLocalizedMetaTitle.Text = content.MetaTitle;
                    txtLocalizedSEName.Text = content.SEName;
                }
            }
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