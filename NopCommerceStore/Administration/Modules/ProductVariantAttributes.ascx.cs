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
using NopSolutions.NopCommerce.BusinessLogic;
using NopSolutions.NopCommerce.BusinessLogic.ExportImport;
using NopSolutions.NopCommerce.BusinessLogic.Media;
using NopSolutions.NopCommerce.BusinessLogic.Orders;
using NopSolutions.NopCommerce.BusinessLogic.Products;
using NopSolutions.NopCommerce.BusinessLogic.Products.Attributes;
using NopSolutions.NopCommerce.BusinessLogic.Promo.Discounts;
using NopSolutions.NopCommerce.BusinessLogic.Tax;
using NopSolutions.NopCommerce.BusinessLogic.Warehouses;
using NopSolutions.NopCommerce.Common.Utils;
using NopSolutions.NopCommerce.Web.Administration.Modules;

namespace NopSolutions.NopCommerce.Web.Administration.Modules
{
    public partial class ProductVariantAttributesControl : BaseNopAdministrationUserControl
    {
        protected void BindAttributes()
        {
            ProductVariant productVariant = ProductManager.GetProductVariantByID(this.ProductVariantID);
            if (productVariant != null)
            {
                pnlData.Visible = true;
                pnlMessage.Visible = false;

                ProductVariantAttributeCollection productVariantAttributes = productVariant.ProductVariantAttributes;
                if (productVariantAttributes.Count > 0)
                {
                    gvProductVariantAttributes.Visible = true;
                    gvProductVariantAttributes.DataSource = productVariantAttributes;
                    gvProductVariantAttributes.DataBind();
                }
                else
                {
                    gvProductVariantAttributes.Visible = false;
                }
            }
            else
            {
                pnlData.Visible = false;
                pnlMessage.Visible = true;
            }
        }

        protected void BindCombinations()
        {
            ProductVariant productVariant = ProductManager.GetProductVariantByID(this.ProductVariantID);
            if (productVariant != null)
            {
                var productVariantAttributes = productVariant.ProductVariantAttributes;
                if (productVariantAttributes.Count > 0)
                {
                    pnlCombinations.Visible = true;
                    var combinations = ProductAttributeManager.GetAllProductVariantAttributeCombinations(this.ProductVariantID);
                    if (combinations.Count > 0)
                    {
                        gvCombinations.Visible = true;
                        gvCombinations.DataSource = combinations;
                        gvCombinations.DataBind();
                    }
                    else
                    {
                        gvCombinations.Visible = false;
                    }
                }
                else
                {
                    pnlCombinations.Visible = false;
                }
            }
        }

        protected void RefreshSelectCombinationControl()
        {
            ctrlSelectProductAttributes.CreateAttributeControls();
        }
        
        private void FillDropDowns()
        {
            this.ddlNewProductAttributes.Items.Clear();
            ProductAttributeCollection productAttributes = ProductAttributeManager.GetAllProductAttributes();
            foreach (ProductAttribute pa in productAttributes)
            {
                ListItem item2 = new ListItem(pa.Name, pa.ProductAttributeID.ToString());
                this.ddlNewProductAttributes.Items.Add(item2);
            }

            CommonHelper.FillDropDownWithEnum(this.ddlAttributeControlType, typeof(AttributeControlTypeEnum));
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                this.FillDropDowns();
                this.BindAttributes();
                this.BindCombinations();
            }
        }

        protected override void OnInit(EventArgs e)
        {
            ctrlSelectProductAttributes.ProductVariantID = this.ProductVariantID;
            base.OnInit(e);
        }

        public void SaveInfo()
        {
            
        }

        protected void btnNewProductVariantAttribute_Click(object sender, EventArgs e)
        {
            try
            {
                ProductVariant productVariant = ProductManager.GetProductVariantByID(this.ProductVariantID);
                if (productVariant != null)
                {
                    if (ddlNewProductAttributes.SelectedItem == null)
                        return;

                    int productAttributeID = int.Parse(ddlNewProductAttributes.SelectedItem.Value);

                    AttributeControlTypeEnum attributeControlType = (AttributeControlTypeEnum)Enum.ToObject(typeof(AttributeControlTypeEnum), int.Parse(this.ddlAttributeControlType.SelectedItem.Value));

                    ProductVariantAttribute productVariantAttribute = ProductAttributeManager.InsertProductVariantAttribute(productVariant.ProductVariantID,
                        productAttributeID, txtNewTextPrompt.Text, cbNewProductVariantAttributeIsRequired.Checked, 
                        attributeControlType, txtNewProductVariantAttributeDisplayOrder.Value);

                    BindAttributes();
                    BindCombinations();
                    RefreshSelectCombinationControl();

                    txtNewProductVariantAttributeDisplayOrder.Value = 1;
                }
            }
            catch (Exception exc)
            {
                processAjaxError(exc);
            }
        }
        
        protected void gvProductVariantAttributes_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "UpdateProductVariantAttribute")
            {
                int index = Convert.ToInt32(e.CommandArgument);
                GridViewRow row = gvProductVariantAttributes.Rows[index];

                HiddenField hfProductVariantAttributeID = row.FindControl("hfProductVariantAttributeID") as HiddenField;
                DropDownList ddlProductAttribute = row.FindControl("ddlProductAttribute") as DropDownList;
                TextBox txtTextPrompt = row.FindControl("txtTextPrompt") as TextBox;
                CheckBox cbIsRequired = row.FindControl("cbIsRequired") as CheckBox;
                DropDownList ddlAttributeControlType = row.FindControl("ddlAttributeControlType") as DropDownList;
                NumericTextBox txtDisplayOrder = row.FindControl("txtDisplayOrder") as NumericTextBox;

                int productVariantAttributeID = int.Parse(hfProductVariantAttributeID.Value);
                int productAttributeID = int.Parse(ddlProductAttribute.SelectedItem.Value);
                string textPrompt = txtTextPrompt.Text;
                bool isRequired = cbIsRequired.Checked;
                AttributeControlTypeEnum attributeControlType = (AttributeControlTypeEnum)Enum.ToObject(typeof(AttributeControlTypeEnum), int.Parse(ddlAttributeControlType.SelectedItem.Value));
                int displayOrder = txtDisplayOrder.Value;

                ProductVariantAttribute productVariantAttribute = ProductAttributeManager.GetProductVariantAttributeByID(productVariantAttributeID);

                if (productVariantAttribute != null)
                    ProductAttributeManager.UpdateProductVariantAttribute(productVariantAttribute.ProductVariantAttributeID,
                       productVariantAttribute.ProductVariantID, productAttributeID, textPrompt,
                       isRequired, attributeControlType, displayOrder);

                BindAttributes();
                BindCombinations();
                RefreshSelectCombinationControl();
            }
        }

        protected void gvProductVariantAttributes_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                ProductVariantAttribute productVariantAttribute = (ProductVariantAttribute)e.Row.DataItem;

                Button btnUpdate = e.Row.FindControl("btnUpdate") as Button;
                if (btnUpdate != null)
                    btnUpdate.CommandArgument = e.Row.RowIndex.ToString();

                DropDownList ddlProductAttribute = e.Row.FindControl("ddlProductAttribute") as DropDownList;
                if (ddlProductAttribute != null)
                {
                    ddlProductAttribute.Items.Clear();
                    ProductAttributeCollection productAttributes = ProductAttributeManager.GetAllProductAttributes();
                    foreach (ProductAttribute productAttribute in productAttributes)
                    {
                        ListItem item = new ListItem(productAttribute.Name,
                                                     productAttribute.ProductAttributeID.ToString());
                        ddlProductAttribute.Items.Add(item);
                        if (productAttribute.ProductAttributeID == productVariantAttribute.ProductAttributeID)
                            item.Selected = true;
                    }
                }

                DropDownList ddlAttributeControlType = e.Row.FindControl("ddlAttributeControlType") as DropDownList;
                {
                    if (ddlAttributeControlType != null)
                        CommonHelper.FillDropDownWithEnum(ddlAttributeControlType, typeof (AttributeControlTypeEnum));
                    CommonHelper.SelectListItem(ddlAttributeControlType, productVariantAttribute.AttributeControlTypeID);
                }

                HyperLink hlAttributeValues = e.Row.FindControl("hlAttributeValues") as HyperLink;
                if (hlAttributeValues != null)
                {
                    if (productVariantAttribute.ShouldHaveValues)
                    {
                        hlAttributeValues.Visible = true;
                        hlAttributeValues.NavigateUrl = string.Format("{0}ProductVariantAttributeValues.aspx?ProductVariantAttributeID={1}",
                                          CommonHelper.GetStoreAdminLocation(),
                                          productVariantAttribute.ProductVariantAttributeID);
                        hlAttributeValues.Text = string.Format(GetLocaleResourceString("Admin.ProductVariantAttributes.Values.Count"), productVariantAttribute.ProductVariantAttributeValues.Count);
                    }
                    else
                    {
                        hlAttributeValues.Visible = false;
                    }
                }
            }
        }

        protected void gvProductVariantAttributes_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            int productVariantAttributeID = (int)gvProductVariantAttributes.DataKeys[e.RowIndex]["ProductVariantAttributeID"];
            ProductAttributeManager.DeleteProductVariantAttribute(productVariantAttributeID);
            
            BindAttributes();
            BindCombinations();
            RefreshSelectCombinationControl();
        }

        protected void btnNewProductVariantAttributeCombination_Click(object sender, EventArgs e)
        {
            try
            {
                ProductVariant productVariant = ProductManager.GetProductVariantByID(this.ProductVariantID);
                if (productVariant != null)
                {
                    string attributes = ctrlSelectProductAttributes.SelectedAttributes;
                    int stockQuantity = txtStockQuantity.Value;
                    bool allowOutOfStockOrders = cbAllowOutOfStockOrders.Checked;

                    List<string> warnings = ShoppingCartManager.GetShoppingCartItemAttributeWarnings(ShoppingCartTypeEnum.ShoppingCart,
                            productVariant.ProductVariantID, attributes, 1, false);
                    if (warnings.Count > 0)
                    {
                        StringBuilder warningsSb = new StringBuilder();
                        for (int i = 0; i < warnings.Count; i++)
                        {
                            warningsSb.Append(Server.HtmlEncode(warnings[i]));
                            if (i != warnings.Count - 1)
                            {
                                warningsSb.Append("<br />");
                            }
                        }

                        pnlCombinationWarningsr.Visible = true;
                        lCombinationWarnings.Text = warningsSb.ToString();
                    }
                    else
                    {
                        var combination = ProductAttributeManager.InsertProductVariantAttributeCombination(productVariant.ProductVariantID,
                            attributes,
                            stockQuantity,
                            allowOutOfStockOrders);
                    }
                    BindCombinations();
                }
            }
            catch (Exception exc)
            {
                processAjaxError(exc);
            }
        }

        protected void gvCombinations_RowCommand(object sender, GridViewCommandEventArgs e)
        {
            if (e.CommandName == "UpdateProductVariantAttributeCombination")
            {
                int index = Convert.ToInt32(e.CommandArgument);
                GridViewRow row = gvCombinations.Rows[index];

                HiddenField hfProductVariantAttributeCombinationID = row.FindControl("hfProductVariantAttributeCombinationID") as HiddenField;
                Label lblAttributes = row.FindControl("lblAttributes") as Label;
                Label lblWarnings = row.FindControl("lblWarnings") as Label;
                NumericTextBox txtStockQuantity = row.FindControl("txtStockQuantity") as NumericTextBox;
                CheckBox cbAllowOutOfStockOrders = row.FindControl("cbAllowOutOfStockOrders") as CheckBox;

                int productVariantAttributeCombinationID = int.Parse(hfProductVariantAttributeCombinationID.Value);
                int stockQuantity = txtStockQuantity.Value;            
                bool allowOutOfStockOrders = cbAllowOutOfStockOrders.Checked;

                var combination = ProductAttributeManager.GetProductVariantAttributeCombinationByID(productVariantAttributeCombinationID);

                if (combination != null)
                    ProductAttributeManager.UpdateProductVariantAttributeCombination(combination.ProductVariantAttributeCombinationID,
                       combination.ProductVariantID, combination.AttributesXML,
                       stockQuantity, allowOutOfStockOrders);

                BindCombinations();
            }
        }

        protected void gvCombinations_RowDataBound(object sender, GridViewRowEventArgs e)
        {
            if (e.Row.RowType == DataControlRowType.DataRow)
            {
                var productVariantAttribute = (ProductVariantAttributeCombination)e.Row.DataItem;

                Button btnUpdate = e.Row.FindControl("btnUpdate") as Button;
                if (btnUpdate != null)
                    btnUpdate.CommandArgument = e.Row.RowIndex.ToString();

                ProductVariant productVariant = ProductManager.GetProductVariantByID(this.ProductVariantID);
                if (productVariant == null)
                    return;

                Label lblAttributes = e.Row.FindControl("lblAttributes") as Label;
                lblAttributes.Text = ProductAttributeHelper.FormatAttributes(productVariant,
                    productVariantAttribute.AttributesXML, NopContext.Current.User, "<br />",
                    true, false, true, false);

                Label lblWarnings = e.Row.FindControl("lblWarnings") as Label;
                List<string> warnings = ShoppingCartManager.GetShoppingCartItemAttributeWarnings(ShoppingCartTypeEnum.ShoppingCart,
                            productVariant.ProductVariantID, productVariantAttribute.AttributesXML, 1, false);
                if (warnings.Count > 0)
                {
                    StringBuilder warningsSb = new StringBuilder();
                    for (int i = 0; i < warnings.Count; i++)
                    {
                        warningsSb.Append(Server.HtmlEncode(warnings[i]));
                        if (i != warnings.Count - 1)
                        {
                            warningsSb.Append("<br />");
                        }
                    }

                    lblWarnings.Visible = true;
                    lblWarnings.Text = warningsSb.ToString();
                }
                else
                {
                    lblWarnings.Visible = false;
                }
            }
        }

        protected void gvCombinations_RowDeleting(object sender, GridViewDeleteEventArgs e)
        {
            int productVariantAttributeCombinationID = (int)gvCombinations.DataKeys[e.RowIndex]["ProductVariantAttributeCombinationID"];
            ProductAttributeManager.DeleteProductVariantAttributeCombination(productVariantAttributeCombinationID);

            BindCombinations();
        }
       
        protected void processAjaxError(Exception exc)
        {
            ProcessException(exc, false);
            pnlError.Visible = true;
            lErrorTitle.Text = exc.Message;
        }

        public int ProductVariantID
        {
            get
            {
                return CommonHelper.QueryStringInt("ProductVariantID");
            }
        }
    }
}