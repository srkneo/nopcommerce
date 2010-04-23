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
    public partial class ProductManufacturerControl : BaseNopAdministrationUserControl
    {
        private void BindData()
        {
            List<ProductManufacturerMappingHelperClass> productManufacturerMappings = null;

            Product product = ProductManager.GetProductById(this.ProductId);
            if (product != null)
            {
                ProductManufacturerCollection existingProductManufacturerCollection = product.ProductManufacturers;
                productManufacturerMappings = GetProductManufacturerMappings(existingProductManufacturerCollection);
            }
            else
            {
                productManufacturerMappings = GetProductManufacturerMappings(null);
            }

            gvManufacturerMappings.DataSource = productManufacturerMappings;
            gvManufacturerMappings.DataBind();
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
                foreach (GridViewRow row in gvManufacturerMappings.Rows)
                {
                    CheckBox cbManufacturerInfo = row.FindControl("cbManufacturerInfo") as CheckBox;
                    HiddenField hfManufacturerId = row.FindControl("hfManufacturerId") as HiddenField;
                    HiddenField hfProductManufacturerId = row.FindControl("hfProductManufacturerId") as HiddenField;
                    CheckBox cbFeatured = row.FindControl("cbFeatured") as CheckBox;
                    NumericTextBox txtRowDisplayOrder = row.FindControl("txtDisplayOrder") as NumericTextBox;
                    int productManufacturerId = int.Parse(hfProductManufacturerId.Value);
                    int manufacturerId = int.Parse(hfManufacturerId.Value);
                    int displayOrder = txtRowDisplayOrder.Value;

                    if (productManufacturerId > 0 && !cbManufacturerInfo.Checked)
                        ManufacturerManager.DeleteProductManufacturer(productManufacturerId);
                    if (productManufacturerId > 0 && cbManufacturerInfo.Checked)
                        ManufacturerManager.UpdateProductManufacturer(productManufacturerId, product.ProductId, manufacturerId, cbFeatured.Checked, displayOrder);
                    if (productManufacturerId == 0 && cbManufacturerInfo.Checked)
                        ManufacturerManager.InsertProductManufacturer(product.ProductId, manufacturerId, cbFeatured.Checked, displayOrder);
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

        private List<ProductManufacturerMappingHelperClass> GetProductManufacturerMappings(ProductManufacturerCollection ExistingProductManufacturerCollection)
        {
            ManufacturerCollection manufacturerCollection = ManufacturerManager.GetAllManufacturers();
            List<ProductManufacturerMappingHelperClass> result = new List<ProductManufacturerMappingHelperClass>();
            for (int i = 0; i < manufacturerCollection.Count; i++)
            {
                Manufacturer manufacturer = manufacturerCollection[i];
                ProductManufacturer existingProductManufacturer = null;
                if (ExistingProductManufacturerCollection != null)
                    existingProductManufacturer = ExistingProductManufacturerCollection.FindProductManufacturer(this.ProductId, manufacturer.ManufacturerId);
                ProductManufacturerMappingHelperClass pmm = new ProductManufacturerMappingHelperClass();
                if (existingProductManufacturer != null)
                {
                    pmm.ProductManufacturerId = existingProductManufacturer.ProductManufacturerId;
                    pmm.IsMapped = true;
                    pmm.IsFeatured = existingProductManufacturer.IsFeaturedProduct;
                    pmm.DisplayOrder = existingProductManufacturer.DisplayOrder;
                }
                else
                {
                    pmm.DisplayOrder = 1;
                }
                pmm.ManufacturerId = manufacturer.ManufacturerId;
                pmm.ManufacturerInfo = manufacturer.Name;

                result.Add(pmm);
            }

            return result;
        }

        private class ProductManufacturerMappingHelperClass
        {
            public int ProductManufacturerId { get; set; }
            public int ManufacturerId { get; set; }
            public string ManufacturerInfo { get; set; }
            public bool IsMapped { get; set; }
            public bool IsFeatured { get; set; }
            public int DisplayOrder { get; set; }
        }
    }
}