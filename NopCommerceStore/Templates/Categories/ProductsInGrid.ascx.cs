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
using System.Text;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Xml;
using NopSolutions.NopCommerce.BusinessLogic;
using NopSolutions.NopCommerce.BusinessLogic.Categories;
using NopSolutions.NopCommerce.BusinessLogic.Configuration.Settings;
using NopSolutions.NopCommerce.BusinessLogic.Directory;
using NopSolutions.NopCommerce.BusinessLogic.Localization;
using NopSolutions.NopCommerce.BusinessLogic.Media;
using NopSolutions.NopCommerce.BusinessLogic.Products;
using NopSolutions.NopCommerce.BusinessLogic.SEO;
using NopSolutions.NopCommerce.Common.Utils;

namespace NopSolutions.NopCommerce.Web.Templates.Categories
{
    public partial class ProductsInGrid : BaseNopUserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                BindData();
            }
        }

        protected void BindData()
        {
            Category category = CategoryManager.GetCategoryByID(CategoryID);

            rptrCategoryBreadcrumb.DataSource = CategoryManager.GetBreadCrumb(CategoryID);
            rptrCategoryBreadcrumb.DataBind();

            //lName.Text = Server.HtmlEncode(category.Name);
            lDescription.Text = category.Description;

            CategoryCollection subCategoryCollection = CategoryManager.GetAllCategories(category.CategoryID);
            if (subCategoryCollection.Count > 0)
            {
                dlSubCategories.DataSource = subCategoryCollection;
                dlSubCategories.DataBind();
            }
            else
                dlSubCategories.Visible = false;

            ProductCollection featuredProducts = category.FeaturedProducts;
            if (featuredProducts.Count > 0)
            {
                dlFeaturedProducts.DataSource = featuredProducts;
                dlFeaturedProducts.DataBind();
            }
            else
            {
                pnlFeaturedProducts.Visible = false;
            }

            this.ctrlPriceRangeFilter.PriceRanges = category.PriceRanges;

            int totalRecords = 0;
            int pageSize = 10;
            if (category.PageSize > 0)
            {
                pageSize = category.PageSize;
            }

            decimal? minPrice = null;
            decimal? maxPrice = null;
            decimal? minPriceConverted = null;
            decimal? maxPriceConverted = null;
            if (ctrlPriceRangeFilter.SelectedPriceRange != null)
            {
                minPrice = ctrlPriceRangeFilter.SelectedPriceRange.From;
                if (minPrice.HasValue)
                {
                    minPriceConverted = CurrencyManager.ConvertCurrency(minPrice.Value, NopContext.Current.WorkingCurrency, CurrencyManager.PrimaryStoreCurrency);
                }

                maxPrice = ctrlPriceRangeFilter.SelectedPriceRange.To;
                if (maxPrice.HasValue)
                {
                    maxPriceConverted = CurrencyManager.ConvertCurrency(maxPrice.Value, NopContext.Current.WorkingCurrency, CurrencyManager.PrimaryStoreCurrency);
                }
            }

            List<int> psoFilterOption = ctrlProductSpecificationFilter.GetAlreadyFilteredSpecOptionIDs();
            
            ProductCollection productCollection = ProductManager.GetAllProducts(this.CategoryID,
                0, false, minPriceConverted, maxPriceConverted, pageSize, this.CurrentPageIndex, psoFilterOption, out totalRecords);

            if (productCollection.Count > 0)
            {
                this.productsPager.PageSize = pageSize;
                this.productsPager.TotalRecords = totalRecords;
                this.productsPager.PageIndex = this.CurrentPageIndex;

                this.dlProducts.DataSource = productCollection;
                this.dlProducts.DataBind();
            }
            else
            {
                this.dlProducts.Visible = false;
            }
        }

        protected override void OnInit(EventArgs e)
        {
            base.OnInit(e);

            ctrlPriceRangeFilter.ExcludedQueryStringParams = productsPager.QueryStringProperty;

            ctrlProductSpecificationFilter.ExcludedQueryStringParams = productsPager.QueryStringProperty;
            ctrlProductSpecificationFilter.CategoryID = this.CategoryID;
            
            ctrlProductSpecificationFilter.ReservedQueryStringParams = "CategoryID,";
            ctrlProductSpecificationFilter.ReservedQueryStringParams += ctrlPriceRangeFilter.QueryStringProperty;
            ctrlProductSpecificationFilter.ReservedQueryStringParams += ",";
            ctrlProductSpecificationFilter.ReservedQueryStringParams += productsPager.QueryStringProperty;
        }

        protected override void OnPreRender(EventArgs e)
        {
            this.pnlFilters.Visible = ctrlPriceRangeFilter.Visible || ctrlProductSpecificationFilter.Visible;
            base.OnPreRender(e);
        }

        protected void dlSubCategories_ItemDataBound(object sender, DataListItemEventArgs e)
        {
            if (e.Item.ItemType == ListItemType.Item || e.Item.ItemType == ListItemType.AlternatingItem)
            {
                Category category = e.Item.DataItem as Category;
                string categoryURL = SEOHelper.GetCategoryURL(category.CategoryID);

                HyperLink hlImageLink = e.Item.FindControl("hlImageLink") as HyperLink;
                if (hlImageLink != null)
                {
                    hlImageLink.ImageUrl = PictureManager.GetPictureUrl(category.PictureID, SettingManager.GetSettingValueInteger("Media.Category.ThumbnailImageSize", 125), true);
                    hlImageLink.NavigateUrl = categoryURL;
                    hlImageLink.ToolTip = String.Format(GetLocaleResourceString("Media.Category.ImageLinkTitleFormat"), category.Name);
                    hlImageLink.Text = String.Format(GetLocaleResourceString("Media.Category.ImageAlternateTextFormat"), category.Name);
                }
                
                HyperLink hlCategory = e.Item.FindControl("hlCategory") as HyperLink;
                if (hlCategory != null)
                {
                    hlCategory.NavigateUrl = categoryURL;
                    hlCategory.ToolTip = String.Format(GetLocaleResourceString("Media.Category.ImageLinkTitleFormat"), category.Name);
                    hlCategory.Text = Server.HtmlEncode(category.Name);
                }
            }
        }

        public int CurrentPageIndex
        {
            get
            {
                int _pageIndex = CommonHelper.QueryStringInt(productsPager.QueryStringProperty);
                _pageIndex--;
                if (_pageIndex < 0)
                    _pageIndex = 0;
                return _pageIndex;
            }
        }

        public int CategoryID
        {
            get
            {
                return CommonHelper.QueryStringInt("CategoryID");
            }
        }
    }
}
