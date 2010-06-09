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
using NopSolutions.NopCommerce.BusinessLogic;
using NopSolutions.NopCommerce.BusinessLogic.CustomerManagement;
using NopSolutions.NopCommerce.BusinessLogic.Promo.Discounts;
using NopSolutions.NopCommerce.Common.Utils;
using NopSolutions.NopCommerce.Common;
using NopSolutions.NopCommerce.BusinessLogic.Products;
using System.Text;
 
namespace NopSolutions.NopCommerce.Web.Administration.Modules
{
    public partial class DiscountInfoControl : BaseNopAdministrationUserControl
    {
        private void FillDropDowns()
        {
            this.ddlDiscountType.Items.Clear();
            var discountTypes = DiscountManager.GetAllDiscountTypes();
            foreach (DiscountType discountType in discountTypes)
            {
                ListItem item2 = new ListItem(discountType.Name, discountType.DiscountTypeId.ToString());
                this.ddlDiscountType.Items.Add(item2);
            }

            this.ddlDiscountRequirement.Items.Clear();
            var discountRequirements = DiscountManager.GetAllDiscountRequirements();
            foreach (DiscountRequirement discountRequirement in discountRequirements)
            {
                ListItem item2 = new ListItem(discountRequirement.Name, discountRequirement.DiscountRequirementId.ToString());
                this.ddlDiscountRequirement.Items.Add(item2);
            }

            this.ddlDiscountLimitation.Items.Clear();
            var discountLimitations = DiscountManager.GetAllDiscountLimitations();
            foreach (DiscountLimitation discountLimitation in discountLimitations)
            {
                ListItem item2 = new ListItem(discountLimitation.Name, discountLimitation.DiscountLimitationId.ToString());
                this.ddlDiscountLimitation.Items.Add(item2);
            }
        }

        private string GenerateListOfRestrictedProductVariants(ProductVariantCollection productVariants)
        {
            StringBuilder result = new StringBuilder();
            for (int i = 0; i < productVariants.Count; i++)
            {
                ProductVariant pv = productVariants[i];
                result.Append(pv.ProductVariantId.ToString());
                if (i != productVariants.Count - 1)
                {
                    result.Append(", ");
                }
            }
            return result.ToString();
        }

        private int[] ParseListOfRestrictedProductVariants(string productVariants)
        {
            List<int> result = new List<int>();
            string[] values = productVariants.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
            foreach (string val1 in values)
            {
                if (!String.IsNullOrEmpty(val1.Trim()))
                {
                    int id = 0;
                    if (int.TryParse(val1.Trim(), out id))
                    {
                        if (ProductManager.GetProductVariantById(id) != null)
                            result.Add(id);
                    }
                }
            }
            return result.ToArray();
        }

        private void BindData()
        {
            Discount discount = DiscountManager.GetDiscountById(this.DiscountId);
            if (discount != null)
            {
                CommonHelper.SelectListItem(this.ddlDiscountType, discount.DiscountTypeId);
                CommonHelper.SelectListItem(this.ddlDiscountRequirement, discount.DiscountRequirementId);
                this.txtRestrictedProductVariants.Text = GenerateListOfRestrictedProductVariants(ProductManager.GetProductVariantsRestrictedByDiscountId(discount.DiscountId));
                CommonHelper.SelectListItem(this.ddlDiscountLimitation, discount.DiscountLimitationId);
                this.txtName.Text = discount.Name;
                this.cbUsePercentage.Checked = discount.UsePercentage;
                this.txtDiscountPercentage.Value = discount.DiscountPercentage;
                this.txtDiscountAmount.Value = discount.DiscountAmount;
                this.ctrlStartDatePicker.SelectedDate = discount.StartDate;
                this.ctrlEndDatePicker.SelectedDate = discount.EndDate;
                this.cbRequiresCouponCode.Checked = discount.RequiresCouponCode;
                this.txtCouponCode.Text = discount.CouponCode;

                CustomerRoleCollection customerRoles = discount.CustomerRoles;
                List<int> _customerRoleIds = new List<int>();
                foreach (CustomerRole customerRole in customerRoles)
                    _customerRoleIds.Add(customerRole.CustomerRoleId);
                CustomerRoleMappingControl.SelectedCustomerRoleIds = _customerRoleIds;
                CustomerRoleMappingControl.BindData();

            }
            else
            {
                List<int> _customerRoleIds = new List<int>();
                CustomerRoleMappingControl.SelectedCustomerRoleIds = _customerRoleIds;
                CustomerRoleMappingControl.BindData();

                this.pnlUsageHistory.Visible = false;
            }
        }

        private void BindUsageHistory()
        {
            Discount discount = DiscountManager.GetDiscountById(this.DiscountId);
            if (discount != null)
            {
                gvDiscountUsageHistory.DataSource = DiscountManager.GetAllDiscountUsageHistoryEntries(discount.DiscountId, null, null);
                gvDiscountUsageHistory.DataBind();
            }
        }

        private void TogglePanels()
        {
            DiscountRequirementEnum discountRequirement = (DiscountRequirementEnum)int.Parse(this.ddlDiscountRequirement.SelectedItem.Value);
            pnlCustomerRoles.Visible = discountRequirement == DiscountRequirementEnum.MustBeAssignedToCustomerRole;
            pnlRestrictedProductVariants.Visible = (discountRequirement == DiscountRequirementEnum.HadPurchasedAllOfTheseProductVariants ||discountRequirement == DiscountRequirementEnum.HadPurchasedOneOfTheseProductVariants );
        }

        private void SetDefaultValues()
        {
            ctrlStartDatePicker.SelectedDate = DateTime.UtcNow.AddDays(-2);
            ctrlEndDatePicker.SelectedDate = DateTime.UtcNow.AddYears(1);
        }

        public Discount SaveInfo()
        {
            DiscountTypeEnum discountType = (DiscountTypeEnum)int.Parse(this.ddlDiscountType.SelectedItem.Value);
            DiscountRequirementEnum discountRequirement = (DiscountRequirementEnum)int.Parse(this.ddlDiscountRequirement.SelectedItem.Value);
            int[] restrictedProductVariantIds = new int[0];
            if (discountRequirement == DiscountRequirementEnum.HadPurchasedAllOfTheseProductVariants || discountRequirement == DiscountRequirementEnum.HadPurchasedOneOfTheseProductVariants)
                restrictedProductVariantIds = ParseListOfRestrictedProductVariants(txtRestrictedProductVariants.Text);
            DiscountLimitationEnum discountLimitation = (DiscountLimitationEnum)int.Parse(this.ddlDiscountLimitation.SelectedItem.Value);

            if(!ctrlStartDatePicker.SelectedDate.HasValue)
            {
                throw new NopException("Start date is not set");
            }

            DateTime discountStartDate = ctrlStartDatePicker.SelectedDate.Value;

            if(!ctrlEndDatePicker.SelectedDate.HasValue)
            {
                throw new NopException("End date is not set");
            }

            DateTime discountEndDate = ctrlEndDatePicker.SelectedDate.Value;

            discountStartDate = DateTime.SpecifyKind(discountStartDate, DateTimeKind.Utc);
            discountEndDate = DateTime.SpecifyKind(discountEndDate, DateTimeKind.Utc);
            
            Discount discount = DiscountManager.GetDiscountById(this.DiscountId);

            if (discount != null)
            {
                discount = DiscountManager.UpdateDiscount(discount.DiscountId,
                    discountType,
                    discountRequirement,
                    discountLimitation,
                    txtName.Text,
                    cbUsePercentage.Checked,
                    txtDiscountPercentage.Value,
                    txtDiscountAmount.Value,
                    discountStartDate,
                    discountEndDate,
                    cbRequiresCouponCode.Checked,
                    txtCouponCode.Text.Trim(),
                    discount.Deleted);

                //discount requirements
                foreach (CustomerRole customerRole in discount.CustomerRoles)
                    CustomerManager.RemoveDiscountFromCustomerRole(customerRole.CustomerRoleId, discount.DiscountId);
                foreach (int customerRoleId in CustomerRoleMappingControl.SelectedCustomerRoleIds)
                    CustomerManager.AddDiscountToCustomerRole(customerRoleId, discount.DiscountId);

                foreach (ProductVariant pv in ProductManager.GetProductVariantsRestrictedByDiscountId(discount.DiscountId))
                    DiscountManager.RemoveDiscountRestriction(pv.ProductVariantId, discount.DiscountId);
                foreach (int productVariantId in restrictedProductVariantIds)
                    DiscountManager.AddDiscountRestriction(productVariantId, discount.DiscountId);
            }
            else
            {
                discount = DiscountManager.InsertDiscount(discountType,
                    discountRequirement,
                    discountLimitation,
                    txtName.Text,
                    cbUsePercentage.Checked,
                    txtDiscountPercentage.Value,
                    txtDiscountAmount.Value,
                    discountStartDate,
                    discountEndDate,
                    cbRequiresCouponCode.Checked,
                    txtCouponCode.Text.Trim(),
                    false);

                //discount requirements
                foreach (int customerRoleId in CustomerRoleMappingControl.SelectedCustomerRoleIds)
                    CustomerManager.AddDiscountToCustomerRole(customerRoleId, discount.DiscountId);

                foreach (int productVariantId in restrictedProductVariantIds)
                    DiscountManager.AddDiscountRestriction(productVariantId, discount.DiscountId);
            }

            return discount;
        }

        protected string GetCustomerInfo(int customerId)
        {
            string customerInfo = string.Empty;
            Customer customer = CustomerManager.GetCustomerById(customerId);
            if (customer != null)
            {
                if (customer.IsGuest)
                {
                    customerInfo = string.Format("<a href=\"CustomerDetails.aspx?CustomerID={0}\">{1}</a>", customer.CustomerId, GetLocaleResourceString("Admin.DiscountInfo.UsageHistory.CustomerColumn.Guest"));
                }
                else
                {
                    customerInfo = string.Format("<a href=\"CustomerDetails.aspx?CustomerID={0}\">{1}</a>", customer.CustomerId, Server.HtmlEncode(customer.Email));
                }
            }
            return customerInfo;
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                this.FillDropDowns();
                this.SetDefaultValues();
                this.BindData();
                this.BindUsageHistory();
                this.TogglePanels();
            }
        }

        protected override void OnPreRender(EventArgs e)
        {
            BindJQuery();

            this.cbUsePercentage.Attributes.Add("onclick", "toggleUsePercentage();");
            this.cbRequiresCouponCode.Attributes.Add("onclick", "toggleRequiresCouponCode();");

            base.OnPreRender(e);
        }

        protected void ddlDiscountRequirement_SelectedIndexChanged(object sender, EventArgs e)
        {
            TogglePanels();
        }

        protected void gvDiscountUsageHistory_PageIndexChanging(object sender, GridViewPageEventArgs e)
        {
            this.gvDiscountUsageHistory.PageIndex = e.NewPageIndex;
            BindUsageHistory();
        }

        protected void DeleteUsageHistoryButton_OnCommand(object sender, CommandEventArgs e)
        {
            if (e.CommandName == "DeleteUsageHistory")
            {
                DiscountManager.DeleteDiscountUsageHistory(Convert.ToInt32(e.CommandArgument));
                BindUsageHistory();
            }
        }
        
        public int DiscountId
        {
            get
            {
                return CommonHelper.QueryStringInt("DiscountId");
            }
        }
    }
}