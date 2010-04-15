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
using NopSolutions.NopCommerce.BusinessLogic.Directory;
using NopSolutions.NopCommerce.BusinessLogic.Localization;
using NopSolutions.NopCommerce.BusinessLogic.Orders;
using NopSolutions.NopCommerce.BusinessLogic.Products;
using NopSolutions.NopCommerce.BusinessLogic.Products.Attributes;
using NopSolutions.NopCommerce.BusinessLogic.Shipping;
using NopSolutions.NopCommerce.BusinessLogic.Tax;
using NopSolutions.NopCommerce.Common.Utils;

namespace NopSolutions.NopCommerce.Web.Modules
{
    public partial class CheckoutAttributesControl : BaseNopUserControl
    {
        protected ShoppingCart getCart()
        {
            return ShoppingCartManager.GetCurrentShoppingCart(ShoppingCartTypeEnum.ShoppingCart);
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            CreateAttributeControls(getCart());
        }

        public void CreateAttributeControls(ShoppingCart cart)
        {
            if (cart == null || cart.Count == 0)
            {
                this.Visible = false;
                return;
            }
            
            this.phAttributes.Controls.Clear();
            
            
            bool shoppingCartRequiresShipping = ShippingManager.ShoppingCartRequiresShipping(cart);
            var checkoutAttributes = CheckoutAttributeManager.GetAllCheckoutAttributes(!shoppingCartRequiresShipping);
            if (checkoutAttributes.Count > 0)
            {
                this.Visible = true;
                foreach (var attribute in checkoutAttributes)
                {
                    var divAttribute = new Panel();
                    var attributeTitle = new Label();
                    if (attribute.IsRequired)
                        attributeTitle.Text = "<span>*</span> ";

                    //text prompt / title
                    string textPrompt = string.Empty;
                    if (!string.IsNullOrEmpty(attribute.TextPrompt))
                        textPrompt = attribute.TextPrompt;
                    else
                        textPrompt = attribute.Name;

                    attributeTitle.Text += Server.HtmlEncode(textPrompt);
                    attributeTitle.Style.Add("font-weight", "bold");

                    bool addBreak = true;
                    switch (attribute.AttributeControlType)
                    {
                        case AttributeControlTypeEnum.TextBox:
                            {
                                addBreak = false;
                            }
                            break;
                        default:
                            break;
                    }
                    if (addBreak)
                    {
                        attributeTitle.Text += "<br />";
                    }
                    else
                    {
                        attributeTitle.Text += "&nbsp;&nbsp;&nbsp;";
                    }
                    divAttribute.Controls.Add(attributeTitle);

                    string controlID = attribute.CheckoutAttributeID.ToString();
                    switch (attribute.AttributeControlType)
                    {
                        case AttributeControlTypeEnum.DropdownList:
                            {
                                var ddlAttributes = new DropDownList();
                                ddlAttributes.ID = controlID;
                                if (!attribute.IsRequired)
                                {
                                    ddlAttributes.Items.Add(new ListItem("---", "0"));
                                }
                                var caValues = attribute.CheckoutAttributeValues;

                                bool preSelectedSet = false;
                                foreach (var caValue in caValues)
                                {
                                    string caValueName = caValue.Name;
                                    if (!this.HidePrices)
                                    {
                                        //decimal priceAdjustmentBase = TaxManager.GetPrice(productVariant, pvaValue.PriceAdjustment);
                                        //decimal priceAdjustment = CurrencyManager.ConvertCurrency(priceAdjustmentBase, CurrencyManager.PrimaryStoreCurrency, NopContext.Current.WorkingCurrency);
                                        //if (priceAdjustmentBase > decimal.Zero)
                                        //    caValueName += string.Format(" [+{0}]", PriceHelper.FormatPrice(priceAdjustment, false, false));
                                    }
                                    var caValueItem = new ListItem(caValueName, caValue.CheckoutAttributeValueID.ToString());
                                    if (!preSelectedSet && caValue.IsPreSelected)
                                    {
                                        caValueItem.Selected = caValue.IsPreSelected;
                                        preSelectedSet = true;
                                    }
                                    ddlAttributes.Items.Add(caValueItem);
                                }
                                divAttribute.Controls.Add(ddlAttributes);
                            }
                            break;
                        case AttributeControlTypeEnum.RadioList:
                            {
                                var rblAttributes = new RadioButtonList();
                                rblAttributes.ID = controlID;
                                var caValues = attribute.CheckoutAttributeValues;

                                bool preSelectedSet = false;
                                foreach (var caValue in caValues)
                                {
                                    string caValueName = caValue.Name;
                                    if (!this.HidePrices)
                                    {
                                        //decimal priceAdjustmentBase = TaxManager.GetPrice(productVariant, pvaValue.PriceAdjustment);
                                        //decimal priceAdjustment = CurrencyManager.ConvertCurrency(priceAdjustmentBase, CurrencyManager.PrimaryStoreCurrency, NopContext.Current.WorkingCurrency);
                                        //if (priceAdjustmentBase > decimal.Zero)
                                        //    pvaValueName += string.Format(" [+{0}]", PriceHelper.FormatPrice(priceAdjustment, false, false));
                                    }
                                    var caValueItem = new ListItem(Server.HtmlEncode(caValueName), caValue.CheckoutAttributeValueID.ToString());
                                    if (!preSelectedSet && caValue.IsPreSelected)
                                    {
                                        caValueItem.Selected = caValue.IsPreSelected;
                                        preSelectedSet = true;
                                    }
                                    rblAttributes.Items.Add(caValueItem);
                                }
                                divAttribute.Controls.Add(rblAttributes);
                            }
                            break;
                        case AttributeControlTypeEnum.Checkboxes:
                            {
                                var cblAttributes = new CheckBoxList();
                                cblAttributes.ID = controlID;
                                var caValues = attribute.CheckoutAttributeValues;
                                foreach (var caValue in caValues)
                                {
                                    string caValueName = caValue.Name;
                                    if (!this.HidePrices)
                                    {
                                        //decimal priceAdjustmentBase = TaxManager.GetPrice(productVariant, pvaValue.PriceAdjustment);
                                        //decimal priceAdjustment = CurrencyManager.ConvertCurrency(priceAdjustmentBase, CurrencyManager.PrimaryStoreCurrency, NopContext.Current.WorkingCurrency);
                                        //if (priceAdjustmentBase > decimal.Zero)
                                        //    pvaValueName += string.Format(" [+{0}]", PriceHelper.FormatPrice(priceAdjustment, false, false));
                                    }
                                    var caValueItem = new ListItem(Server.HtmlEncode(caValueName), caValue.CheckoutAttributeValueID.ToString());
                                    caValueItem.Selected = caValue.IsPreSelected;
                                    cblAttributes.Items.Add(caValueItem);
                                }
                                divAttribute.Controls.Add(cblAttributes);
                            }
                            break;
                        case AttributeControlTypeEnum.TextBox:
                            {
                                var txtAttribute = new TextBox();
                                txtAttribute.ID = controlID;
                                divAttribute.Controls.Add(txtAttribute);
                            }
                            break;
                        default:
                            break;
                    }
                    phAttributes.Controls.Add(divAttribute);
                }
            }
            else
            {
                this.Visible = false;
            }
        }

        public string SelectedAttributes
        {
            get
            {
                string selectedAttributes = string.Empty;

                ShoppingCart cart = getCart();
                bool shoppingCartRequiresShipping = ShippingManager.ShoppingCartRequiresShipping(cart);
                var checkoutAttributes = CheckoutAttributeManager.GetAllCheckoutAttributes(!shoppingCartRequiresShipping);

                foreach (var attribute in checkoutAttributes)
                {
                    string controlID = attribute.CheckoutAttributeID.ToString();
                    switch (attribute.AttributeControlType)
                    {
                        case AttributeControlTypeEnum.DropdownList:
                            {
                                var ddlAttributes = phAttributes.FindControl(controlID) as DropDownList;
                                if (ddlAttributes != null)
                                {
                                    int selectedAttributeID = 0;
                                    if (!String.IsNullOrEmpty(ddlAttributes.SelectedValue))
                                    {
                                        selectedAttributeID = int.Parse(ddlAttributes.SelectedValue);
                                    }
                                    if (selectedAttributeID > 0)
                                    {
                                        selectedAttributes = CheckoutAttributeHelper.AddCheckoutAttribute(selectedAttributes,
                                            attribute, selectedAttributeID.ToString());
                                    }
                                }
                            }
                            break;
                        case AttributeControlTypeEnum.RadioList:
                            {
                                var rblAttributes =
                                    phAttributes.FindControl(controlID) as RadioButtonList;
                                if (rblAttributes != null)
                                {
                                    int selectedAttributeID = 0;
                                    if (!String.IsNullOrEmpty(rblAttributes.SelectedValue))
                                    {
                                        selectedAttributeID = int.Parse(rblAttributes.SelectedValue);
                                    }
                                    if (selectedAttributeID > 0)
                                    {
                                        selectedAttributes = CheckoutAttributeHelper.AddCheckoutAttribute(selectedAttributes,
                                            attribute, selectedAttributeID.ToString());
                                    }
                                }
                            }
                            break;
                        case AttributeControlTypeEnum.Checkboxes:
                            {
                                var cblAttributes = phAttributes.FindControl(controlID) as CheckBoxList;
                                if (cblAttributes != null)
                                {
                                    foreach (ListItem item in cblAttributes.Items)
                                    {
                                        if (item.Selected)
                                        {
                                            int selectedAttributeID = 0;
                                            if (!String.IsNullOrEmpty(item.Value))
                                            {
                                                selectedAttributeID = int.Parse(item.Value);
                                            }
                                            if (selectedAttributeID > 0)
                                            {
                                                selectedAttributes = CheckoutAttributeHelper.AddCheckoutAttribute(selectedAttributes, 
                                                    attribute, selectedAttributeID.ToString());
                                            }
                                        }
                                    }
                                }
                            }
                            break;
                        case AttributeControlTypeEnum.TextBox:
                            {
                                var txtAttribute = phAttributes.FindControl(controlID) as TextBox;
                                if (txtAttribute != null)
                                {
                                    string enteredText = txtAttribute.Text.Trim();
                                    if (!String.IsNullOrEmpty(enteredText))
                                    {
                                        selectedAttributes = CheckoutAttributeHelper.AddCheckoutAttribute(selectedAttributes,
                                            attribute, enteredText);
                                    }
                                }
                            }
                            break;
                        default:
                            break;
                    }
                }
                return selectedAttributes;
            }
        }

        public bool HidePrices
        {
            get
            {
                if (ViewState["HidePrices"] == null)
                    return false;
                else
                    return (bool)ViewState["HidePrices"];
            }
            set
            {
                ViewState["HidePrices"] = value;
            }
        }
    }
}