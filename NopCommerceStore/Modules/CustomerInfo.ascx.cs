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
using System.Collections.ObjectModel;
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
using NopSolutions.NopCommerce.BusinessLogic.Content.Forums;
using NopSolutions.NopCommerce.BusinessLogic.CustomerManagement;
using NopSolutions.NopCommerce.BusinessLogic.Directory;
using NopSolutions.NopCommerce.BusinessLogic.Profile;
using NopSolutions.NopCommerce.BusinessLogic.SEO;
using NopSolutions.NopCommerce.Common.Utils;
using NopSolutions.NopCommerce.Common.Xml;

namespace NopSolutions.NopCommerce.Web.Modules
{
    public partial class CustomerInfoControl : BaseNopUserControl
    {
        protected void Page_Load(object sender, EventArgs e)
        {
            if (NopContext.Current.User == null)
            {
                string loginURL = SEOHelper.GetLoginPageURL(true);
                Response.Redirect(loginURL);
            }
            
            if (!Page.IsPostBack)
            {
                FillCountryDropDowns();
                FillStateProvinceDropDowns();
                FillTimeZones();
                BindData();
                TogglePanels();
            }
        }

        protected void TogglePanels()
        {
            trTimeZone.Visible = DateTimeHelper.AllowCustomersToSetTimeZone;
            trSignature.Visible = ForumManager.ForumsEnabled && ForumManager.SignaturesEnabled;
            divPreferences.Visible = trTimeZone.Visible || trSignature.Visible;
        }

        private void BindData()
        {
            var customer = NopContext.Current.User;

            txtEmail.Text = customer.Email;

            if (customer.Gender.ToLower() == "m")
                rbGenderM.Checked = true;
            else
                rbGenderF.Checked = true;

            txtFirstName.Text = customer.FirstName;
            txtLastName.Text = customer.LastName;

            dtDateOfBirth.SelectedDate = customer.DateOfBirth;

            txtCompany.Text = customer.Company;
            txtStreetAddress.Text = customer.StreetAddress;
            txtStreetAddress2.Text = customer.StreetAddress2;
            txtZipPostalCode.Text = customer.ZipPostalCode;
            txtCity.Text = customer.City;
            txtPhoneNumber.Text = customer.PhoneNumber;
            txtFaxNumber.Text = customer.FaxNumber;
            CommonHelper.SelectListItem(ddlCountry, customer.CountryID.ToString());

            FillStateProvinceDropDowns();

            CommonHelper.SelectListItem(ddlStateProvince, customer.StateProvinceID.ToString());

            cbNewsletter.Checked = customer.ReceiveNewsletter;

            if (DateTimeHelper.AllowCustomersToSetTimeZone)
            {
                CommonHelper.SelectListItem(this.ddlTimeZone, DateTimeHelper.CurrentTimeZone.Id);
            }

            if (ForumManager.ForumsEnabled && ForumManager.SignaturesEnabled)
            {
                txtSignature.Text = customer.Signature;
            }
        }

        protected void btnSaveCustomerInfo_Click(object sender, EventArgs e)
        {
            if (Page.IsValid)
            {
                try
                {
                    var customer = NopContext.Current.User;
                    if (customer.Email.ToLower() != txtEmail.Text.ToLower().Trim())
                    {
                        customer = CustomerManager.SetEmail(customer.CustomerID, txtEmail.Text.Trim());
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

                    if (DateTimeHelper.AllowCustomersToSetTimeZone)
                    {
                        if (ddlTimeZone.SelectedItem != null && !String.IsNullOrEmpty(ddlTimeZone.SelectedItem.Value))
                        {
                            string timeZoneID = ddlTimeZone.SelectedItem.Value;
                            DateTimeHelper.CurrentTimeZone = DateTimeHelper.FindTimeZoneById(timeZoneID);
                        }
                    }

                    if (ForumManager.ForumsEnabled && ForumManager.SignaturesEnabled)
                    {
                        customer = CustomerManager.SetCustomerSignature(customer.CustomerID, txtSignature.Text);
                    }
                }
                catch (Exception exc)
                {
                    ErrorMessage.Text = exc.Message;
                }
            }
        }

        private void FillCountryDropDowns()
        {
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

        private void FillTimeZones()
        {
            this.ddlTimeZone.Items.Clear();
            if (DateTimeHelper.AllowCustomersToSetTimeZone)
            {
                var timeZones = DateTimeHelper.GetSystemTimeZones();
                foreach (var timeZone in timeZones)
                {
                    string timeZoneName = timeZone.DisplayName;
                    var ddlTimeZoneItem2 = new ListItem(timeZoneName, timeZone.Id);
                    this.ddlTimeZone.Items.Add(ddlTimeZoneItem2);
                }
            }
        }

        protected void ddlCountry_SelectedIndexChanged(object sender, EventArgs e)
        {
            FillStateProvinceDropDowns();
        }
    }
}