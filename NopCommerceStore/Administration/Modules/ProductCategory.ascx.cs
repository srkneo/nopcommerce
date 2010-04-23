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

namespace NopSolutions.NopCommerce.Web.Administration.Modules
{
    public partial class ProductCategoryControl : BaseNopAdministrationUserControl
    {
        private void BindData()
        {
            List<ProductCategoryMappingHelperClass> productCategoryMappings = null;

            Product product = ProductManager.GetProductById(this.ProductId);
            if (product != null)
            {
                ProductCategoryCollection existingProductCategoryCollection = product.ProductCategories;
                productCategoryMappings = GetProductCategoryMappings(0, string.Empty, existingProductCategoryCollection);
            }
            else
            {
                productCategoryMappings = GetProductCategoryMappings(0, string.Empty, null);
            }
            
            gvCategoryMappings.DataSource = productCategoryMappings;
            gvCategoryMappings.DataBind();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                this.BindData();
            }
        }

        public void SaveInfo()
        {
            SaveInfo(this.ProductId);
        }

        public void SaveInfo(int prodId)
        {
            Product product = ProductManager.GetProductById(prodId);
            if (product != null)
            {
                foreach (GridViewRow row in gvCategoryMappings.Rows)
                {
                    CheckBox cbCategoryInfo = row.FindControl("cbCategoryInfo") as CheckBox;
                    HiddenField hfCategoryId = row.FindControl("hfCategoryId") as HiddenField;
                    HiddenField hfProductCategoryId = row.FindControl("hfProductCategoryId") as HiddenField;
                    CheckBox cbFeatured = row.FindControl("cbFeatured") as CheckBox;
                    NumericTextBox txtRowDisplayOrder = row.FindControl("txtDisplayOrder") as NumericTextBox;
                    int productCategoryId = int.Parse(hfProductCategoryId.Value);
                    int categoryId = int.Parse(hfCategoryId.Value);
                    int displayOrder = txtRowDisplayOrder.Value;

                    if (productCategoryId > 0 && !cbCategoryInfo.Checked)
                        CategoryManager.DeleteProductCategory(productCategoryId);
                    if (productCategoryId > 0 && cbCategoryInfo.Checked)
                        CategoryManager.UpdateProductCategory(productCategoryId, product.ProductId, categoryId, cbFeatured.Checked, displayOrder);
                    if (productCategoryId == 0 && cbCategoryInfo.Checked)
                        CategoryManager.InsertProductCategory(product.ProductId, categoryId, cbFeatured.Checked, displayOrder);
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

        private List<ProductCategoryMappingHelperClass> GetProductCategoryMappings(int forParentCategoryId,
            string prefix, ProductCategoryCollection ExistingProductCategoryCollection)
        {
            CategoryCollection categoryCollection = CategoryManager.GetAllCategories(forParentCategoryId);
            List<ProductCategoryMappingHelperClass> result = new List<ProductCategoryMappingHelperClass>();
            for (int i = 0; i < categoryCollection.Count; i++)
            {
                Category category = categoryCollection[i];
                ProductCategory existingProductCategory = null;
                if (ExistingProductCategoryCollection != null)
                    existingProductCategory = ExistingProductCategoryCollection.FindProductCategory(this.ProductId, category.CategoryId);
                ProductCategoryMappingHelperClass pcm = new ProductCategoryMappingHelperClass();
                if (existingProductCategory != null)
                {
                    pcm.ProductCategoryId = existingProductCategory.ProductCategoryId;
                    pcm.IsMapped = true;
                    pcm.IsFeatured = existingProductCategory.IsFeaturedProduct;
                    pcm.DisplayOrder = existingProductCategory.DisplayOrder;
                }
                else
                {
                    pcm.DisplayOrder = 1;
                }
                pcm.CategoryId = category.CategoryId;
                pcm.CategoryInfo = prefix + category.Name;
                result.Add(pcm);
                if (CategoryManager.GetAllCategories(category.CategoryId).Count > 0)
                    result.AddRange(GetProductCategoryMappings(category.CategoryId, prefix + "--", ExistingProductCategoryCollection));
            }

            return result;
        }
        
        private class ProductCategoryMappingHelperClass
        {
            public int ProductCategoryId { get; set; }
            public int CategoryId { get; set; }
            public string CategoryInfo { get; set; }
            public bool IsMapped { get; set; }
            public bool IsFeatured { get; set; }
            public int DisplayOrder { get; set; }
        }
    }
}