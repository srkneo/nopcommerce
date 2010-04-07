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
using NopSolutions.NopCommerce.BusinessLogic.Configuration.Settings;
using NopSolutions.NopCommerce.BusinessLogic.CustomerManagement;
using NopSolutions.NopCommerce.BusinessLogic.Directory;
using NopSolutions.NopCommerce.Common.Utils;
using NopSolutions.NopCommerce.Common.Xml;

namespace NopSolutions.NopCommerce.Web.Modules
{
    public partial class CustomerRegisterControl : BaseNopUserControl
    {
        private void ApplyLocalization()
        {
            var EmailRequired = CreateUserWizardStep1.ContentTemplateContainer.FindControl("EmailRequired") as RequiredFieldValidator;
            if (EmailRequired != null)
            {
                if (CustomerManager.UsernamesEnabled)
                {
                    EmailRequired.ErrorMessage = GetLocaleResourceString("Account.E-MailRequired");
                    EmailRequired.ToolTip = GetLocaleResourceString("Account.E-MailRequired");
                }
                else
                {
                    //EmailRequired is not enabled
                }
            }

            var revEmail = CreateUserWizardStep1.ContentTemplateContainer.FindControl("revEmail") as RegularExpressionValidator;
            if (revEmail != null)
            {
                if (CustomerManager.UsernamesEnabled)
                {
                    revEmail.ErrorMessage = GetLocaleResourceString("Account.InvalidEmail");
                    revEmail.ToolTip = GetLocaleResourceString("Account.InvalidEmail");
                }
                else
                {
                    //revEmail is not enabled
                }
            }


            var lUsernameOrEmail = CreateUserWizardStep1.ContentTemplateContainer.FindControl("lUsernameOrEmail") as Literal;
            if (lUsernameOrEmail != null)
            {
                if (CustomerManager.UsernamesEnabled)
                {
                    lUsernameOrEmail.Text = GetLocaleResourceString("Account.Username");
                }
                else
                {
                    lUsernameOrEmail.Text = GetLocaleResourceString("Account.E-Mail");
                }
            }

            var UserNameOrEmailRequired = CreateUserWizardStep1.ContentTemplateContainer.FindControl("UserNameOrEmailRequired") as RequiredFieldValidator;
            if (UserNameOrEmailRequired != null)
            {
                if (CustomerManager.UsernamesEnabled)
                {
                    UserNameOrEmailRequired.ErrorMessage = GetLocaleResourceString("Account.UserNameRequired");
                    UserNameOrEmailRequired.ToolTip = GetLocaleResourceString("Account.UserNameRequired");
                }
                else
                {
                    UserNameOrEmailRequired.ErrorMessage = GetLocaleResourceString("Account.E-MailRequired");
                    UserNameOrEmailRequired.ToolTip = GetLocaleResourceString("Account.E-MailRequired");
                }
            }

            var refUserNameOrEmail = CreateUserWizardStep1.ContentTemplateContainer.FindControl("refUserNameOrEmail") as RegularExpressionValidator;
            if (refUserNameOrEmail != null)
            {
                if (CustomerManager.UsernamesEnabled)
                {
                    //refUserNameOrEmail is not enabled
                }
                else
                {
                    refUserNameOrEmail.ErrorMessage = GetLocaleResourceString("Account.InvalidEmail");
                    refUserNameOrEmail.ToolTip = GetLocaleResourceString("Account.InvalidEmail");
                }
            }

            var lblCompleteStep = CompleteWizardStep1.ContentTemplateContainer.FindControl("lblCompleteStep") as Label;
            if (lblCompleteStep != null)
            {
                switch (CustomerManager.CustomerRegistrationType)
                {
                    case CustomerRegistrationTypeEnum.Standard:
                        {
                            lblCompleteStep.Text = GetLocaleResourceString("Account.RegistrationCompleted");
                        }
                        break;
                    case CustomerRegistrationTypeEnum.EmailValidation:
                        {
                            lblCompleteStep.Text = GetLocaleResourceString("Account.ActivationEmailHasBeenSent");
                        }
                        break;
                    case CustomerRegistrationTypeEnum.AdminApproval:
                        {
                            lblCompleteStep.Text = GetLocaleResourceString("Account.AdminApprovalRequired");
                        }
                        break;
                    case CustomerRegistrationTypeEnum.Disabled:
                        {
                            lblCompleteStep.Text = "Registration method error";
                        }
                        break;
                    default:
                        {
                            lblCompleteStep.Text = "Registration method error";
                        }
                        break;
                }
            }
        }

        public void CreatedUser(object sender, EventArgs e)
        {
            var rbGenderM = (RadioButton)CreateUserWizardStep1.ContentTemplateContainer.FindControl("rbGenderM");
            var rbGenderF = (RadioButton)CreateUserWizardStep1.ContentTemplateContainer.FindControl("rbGenderF");
            var txtFirstName = (TextBox)CreateUserWizardStep1.ContentTemplateContainer.FindControl("txtFirstName");
            var txtLastName = (TextBox)CreateUserWizardStep1.ContentTemplateContainer.FindControl("txtLastName");
            var txtDateOfBirth = (TextBox)CreateUserWizardStep1.ContentTemplateContainer.FindControl("txtDateOfBirth");
            var UserName = (TextBox)CreateUserWizardStep1.ContentTemplateContainer.FindControl("UserName");
            var txtCompany = (TextBox)CreateUserWizardStep1.ContentTemplateContainer.FindControl("txtCompany");
            var txtStreetAddress = (TextBox)CreateUserWizardStep1.ContentTemplateContainer.FindControl("txtStreetAddress");
            var txtStreetAddress2 = (TextBox)CreateUserWizardStep1.ContentTemplateContainer.FindControl("txtStreetAddress2");
            var txtZipPostalCode = (TextBox)CreateUserWizardStep1.ContentTemplateContainer.FindControl("txtZipPostalCode");
            var txtCity = (TextBox)CreateUserWizardStep1.ContentTemplateContainer.FindControl("txtCity");
            var txtPhoneNumber = (TextBox)CreateUserWizardStep1.ContentTemplateContainer.FindControl("txtPhoneNumber");
            var txtFaxNumber = (TextBox)CreateUserWizardStep1.ContentTemplateContainer.FindControl("txtFaxNumber");
            var ddlCountry = (DropDownList)CreateUserWizardStep1.ContentTemplateContainer.FindControl("ddlCountry");
            var ddlStateProvince = (DropDownList)CreateUserWizardStep1.ContentTemplateContainer.FindControl("ddlStateProvince");
            var cbNewsletter = (CheckBox)CreateUserWizardStep1.ContentTemplateContainer.FindControl("cbNewsletter");
            var dtDateOfBirth = CreateUserWizardStep1.ContentTemplateContainer.FindControl("dtDateOfBirth") as DatePicker2Control;



            Customer customer = null;
            if (CustomerManager.UsernamesEnabled)
            {
                customer = CustomerManager.GetCustomerByUsername(UserName.Text.Trim());
            }
            else
            {
                customer = CustomerManager.GetCustomerByEmail(UserName.Text.Trim());
            }

            if (rbGenderM.Checked)
                customer.Gender = "M";
            else
                customer.Gender = "F";

            customer.FirstName = txtFirstName.Text;
            customer.LastName = txtLastName.Text;
            customer.DateOfBirth = dtDateOfBirth.SelectedDate;

            customer.Company = txtCompany.Text;
            customer.StreetAddress = txtStreetAddress.Text;
            customer.StreetAddress2 = txtStreetAddress2.Text;
            customer.ZipPostalCode = txtZipPostalCode.Text;
            customer.City = txtCity.Text;
            customer.PhoneNumber = txtPhoneNumber.Text;
            customer.FaxNumber = txtFaxNumber.Text;
            customer.CountryID = int.Parse(ddlCountry.SelectedItem.Value);
            customer.StateProvinceID = int.Parse(ddlStateProvince.SelectedItem.Value);
            customer.ReceiveNewsletter = cbNewsletter.Checked;

            //billing address
            var billingAddress = new Address()
            {
                CustomerID = customer.CustomerID,
                IsBillingAddress = true,
                FirstName = customer.FirstName,
                LastName = customer.LastName,
                PhoneNumber = customer.PhoneNumber,
                Email = customer.Email,
                FaxNumber = customer.FaxNumber,
                Company = customer.Company,
                Address1 = customer.StreetAddress,
                Address2 = customer.StreetAddress2,
                City = customer.City,
                StateProvinceID = customer.StateProvinceID,
                ZipPostalCode = customer.ZipPostalCode,
                CountryID = customer.CountryID,
                CreatedOn = customer.RegistrationDate
            };
            if (CustomerManager.CanUseAddressAsBillingAddress(billingAddress))
            {
                billingAddress = CustomerManager.InsertAddress(billingAddress.CustomerID, billingAddress.IsBillingAddress,
                    billingAddress.FirstName, billingAddress.LastName, billingAddress.PhoneNumber,
                    billingAddress.Email, billingAddress.FaxNumber, billingAddress.Company,
                    billingAddress.Address1, billingAddress.Address2,
                    billingAddress.City, billingAddress.StateProvinceID,
                    billingAddress.ZipPostalCode, billingAddress.CountryID, DateTime.Now, DateTime.Now);
            }

            //shipping address
            var shippingAddress = new Address()
            {
                CustomerID = customer.CustomerID,
                IsBillingAddress = false,
                FirstName = customer.FirstName,
                LastName = customer.LastName,
                PhoneNumber = customer.PhoneNumber,
                Email = customer.Email,
                FaxNumber = customer.FaxNumber,
                Company = customer.Company,
                Address1 = customer.StreetAddress,
                Address2 = customer.StreetAddress2,
                City = customer.City,
                StateProvinceID = customer.StateProvinceID,
                ZipPostalCode = customer.ZipPostalCode,
                CountryID = customer.CountryID,
                CreatedOn = customer.RegistrationDate
            };
            if (CustomerManager.CanUseAddressAsShippingAddress(shippingAddress))
            {
                shippingAddress = CustomerManager.InsertAddress(shippingAddress.CustomerID, shippingAddress.IsBillingAddress,
                    shippingAddress.FirstName, shippingAddress.LastName, shippingAddress.PhoneNumber,
                    shippingAddress.Email, shippingAddress.FaxNumber, shippingAddress.Company,
                    shippingAddress.Address1, shippingAddress.Address2,
                    shippingAddress.City, shippingAddress.StateProvinceID,
                    shippingAddress.ZipPostalCode, shippingAddress.CountryID, DateTime.Now, DateTime.Now);
            }
        }

        public void CreatingUser(object sender, LoginCancelEventArgs e)
        {
            if (SettingManager.GetSettingValueBoolean("Common.RegisterCaptchaImageEnabled"))
            {
                var CaptchaCtrl = CreateUserWizardStep1.ContentTemplateContainer.FindControl("CaptchaCtrl") as CaptchaControl;
                if (CaptchaCtrl != null)
                {
                    if (!CaptchaCtrl.ValidateCaptcha())
                    {
                        e.Cancel = true;
                    }
                }
            }
        }

        protected void CreateUserError(object sender, EventArgs e)
        {
            if (SettingManager.GetSettingValueBoolean("Common.RegisterCaptchaImageEnabled"))
            {
                var CaptchaCtrl = CreateUserWizardStep1.ContentTemplateContainer.FindControl("CaptchaCtrl") as CaptchaControl;
                if (CaptchaCtrl != null)
                {
                    CaptchaCtrl.RegenerateCode();
                }
            }
        }
        
        private void FillCountryDropDowns()
        {
            var ddlCountry = (DropDownList)CreateUserWizardStep1.ContentTemplateContainer.FindControl("ddlCountry");
            ddlCountry.Items.Clear();
            var countryCollection = CountryManager.GetAllCountriesForRegistration();
            foreach (var country in countryCollection)
            {
                var ddlCountryItem2 = new ListItem(country.Name, country.CountryID.ToString());
                ddlCountry.Items.Add(ddlCountryItem2);
            }
        }

        private void FillStateProvinceDropDowns()
        {
            var ddlCountry = (DropDownList)CreateUserWizardStep1.ContentTemplateContainer.FindControl("ddlCountry");
            var ddlStateProvince = (DropDownList)CreateUserWizardStep1.ContentTemplateContainer.FindControl("ddlStateProvince");
            ddlStateProvince.Items.Clear();
            int countryID = 0;
            if (ddlCountry.SelectedItem != null)
                countryID = int.Parse(ddlCountry.SelectedItem.Value);

            var stateProvinceCollection = StateProvinceManager.GetStateProvincesByCountryID(countryID);
            foreach (var stateProvince in stateProvinceCollection)
            {
                var ddlStateProviceItem2 = new ListItem(stateProvince.Name, stateProvince.StateProvinceID.ToString());
                ddlStateProvince.Items.Add(ddlStateProviceItem2);
            }
            if (stateProvinceCollection.Count == 0)
            {
                var ddlStateProvinceItem = new ListItem(GetLocaleResourceString("Address.StateProvinceNonUS"), "0");
                ddlStateProvince.Items.Add(ddlStateProvinceItem);
            }
        }

        protected void ddlCountry_SelectedIndexChanged(object sender, EventArgs e)
        {
            FillStateProvinceDropDowns();
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            this.ApplyLocalization();

            if (CustomerManager.CustomerRegistrationType == CustomerRegistrationTypeEnum.Disabled)
            {
                CreateUserForm.Visible = false;
                topicRegistrationNotAllowed.Visible = true;
            }
            else
            {
                CreateUserForm.Visible = true;
                topicRegistrationNotAllowed.Visible = false;
            }

            if (!Page.IsPostBack)
            {
                if (NopContext.Current.User != null && !NopContext.Current.User.IsGuest)
                {
                    CustomerManager.Logout();
                    Response.Redirect("~/register.aspx");
                }

                #region Username/emails hack
                var pnlEmail = CreateUserWizardStep1.ContentTemplateContainer.FindControl("pnlEmail") as HtmlTableRow;
                if (pnlEmail != null)
                {
                    pnlEmail.Visible = CustomerManager.UsernamesEnabled;
                }
                var refUserNameOrEmail = CreateUserWizardStep1.ContentTemplateContainer.FindControl("refUserNameOrEmail") as RegularExpressionValidator;
                if (refUserNameOrEmail != null)
                {
                    refUserNameOrEmail.Enabled = !CustomerManager.UsernamesEnabled;
                }
                #endregion

                this.FillCountryDropDowns();
                this.FillStateProvinceDropDowns();
                this.DataBind();
            }

            var CaptchaCtrl = CreateUserWizardStep1.ContentTemplateContainer.FindControl("CaptchaCtrl") as CaptchaControl;
            if (CaptchaCtrl != null)
            {
                CaptchaCtrl.Visible = SettingManager.GetSettingValueBoolean("Common.RegisterCaptchaImageEnabled");
            }
        }
    }
}