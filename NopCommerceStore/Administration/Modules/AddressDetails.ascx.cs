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
using System.Web.UI;
using System.Web.UI.WebControls;
using NopSolutions.NopCommerce.BusinessLogic.CustomerManagement;
using NopSolutions.NopCommerce.BusinessLogic.Directory;
using NopSolutions.NopCommerce.Common.Utils;
using System.Collections.Generic;
using NopSolutions.NopCommerce.BusinessLogic.IoC;

namespace NopSolutions.NopCommerce.Web.Administration.Modules
{
    public partial class AddressDetailsControl : BaseNopAdministrationUserControl
    {
        protected void BindData()
        {
            Address address = IoCFactory.Resolve<ICustomerManager>().GetAddressById(this.AddressId);
            if (address != null)
            {
                Customer customer = address.Customer;
                if (customer != null)
                {
                    this.lblCustomer.Text = Server.HtmlEncode(customer.Email);
                    lnkBack.NavigateUrl = CommonHelper.GetStoreAdminLocation() + "CustomerDetails.aspx?CustomerID=" + customer.CustomerId.ToString();
                }
                else
                    Response.Redirect("Customers.aspx");

                this.FillCountryDropDowns(address);

                this.txtFirstName.Text = address.FirstName;
                this.txtLastName.Text = address.LastName;
                this.txtPhoneNumber.Text = address.PhoneNumber;
                this.txtEmail.Text = address.Email;
                this.txtFaxNumber.Text = address.FaxNumber;
                this.txtCompany.Text = address.Company;
                this.txtAddress1.Text = address.Address1;
                this.txtAddress2.Text = address.Address2;
                this.txtCity.Text = address.City;
                CommonHelper.SelectListItem(this.ddlCountry, address.CountryId);
                FillStateProvinceDropDowns();
                CommonHelper.SelectListItem(this.ddlStateProvince, address.StateProvinceId);
                this.txtZipPostalCode.Text = address.ZipPostalCode;
            }
            else
                Response.Redirect("Customers.aspx");
        }

        protected void FillCountryDropDowns(Address address)
        {
            this.ddlCountry.Items.Clear();
            List<Country> countryCollection = null;
            if (address.IsBillingAddress)
                countryCollection = IoCFactory.Resolve<ICountryManager>().GetAllCountriesForBilling();
            else
                countryCollection = IoCFactory.Resolve<ICountryManager>().GetAllCountriesForShipping();
            foreach (Country country in countryCollection)
            {
                ListItem ddlCountryItem2 = new ListItem(country.Name, country.CountryId.ToString());
                this.ddlCountry.Items.Add(ddlCountryItem2);
            }
        }

        protected void FillStateProvinceDropDowns()
        {
            this.ddlStateProvince.Items.Clear();
            int countryId = int.Parse(this.ddlCountry.SelectedItem.Value);

            var stateProvinceCollection = IoCFactory.Resolve<IStateProvinceManager>().GetStateProvincesByCountryId(countryId);
            foreach (StateProvince stateProvince in stateProvinceCollection)
            {
                ListItem ddlStateProviceItem2 = new ListItem(stateProvince.Name, stateProvince.StateProvinceId.ToString());
                this.ddlStateProvince.Items.Add(ddlStateProviceItem2);
            }
            if (stateProvinceCollection.Count == 0)
            {
                ListItem ddlStateProvinceItem = new ListItem(GetLocaleResourceString("Admin.Common.State.Other"), "0");
                this.ddlStateProvince.Items.Add(ddlStateProvinceItem);
            }
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                this.BindData();
            }
        }

        protected Address Save()
        {
            var address = IoCFactory.Resolve<ICustomerManager>().GetAddressById(this.AddressId);

            address.FirstName = txtFirstName.Text;
            address.LastName = txtLastName.Text;
            address.PhoneNumber = txtPhoneNumber.Text;
            address.Email = txtEmail.Text;
            address.FaxNumber = txtFaxNumber.Text;
            address.Company = txtCompany.Text;
            address.Address1 = txtAddress1.Text;
            address.Address2 = txtAddress2.Text;
            address.City = txtCity.Text;
            address.StateProvinceId = int.Parse(this.ddlStateProvince.SelectedItem.Value);
            address.ZipPostalCode = txtZipPostalCode.Text;
            address.CountryId = int.Parse(this.ddlCountry.SelectedItem.Value);
            address.UpdatedOn = DateTime.UtcNow;
            IoCFactory.Resolve<ICustomerManager>().UpdateAddress(address);

            return address;
        }

        protected void SaveButton_Click(object sender, EventArgs e)
        {
            if (Page.IsValid)
            {
                try
                {
                    Address address = Save();
                    Response.Redirect(string.Format("CustomerDetails.aspx?CustomerID={0}", address.CustomerId));
                }
                catch (Exception exc)
                {
                    ProcessException(exc);
                }
            }
        }

        protected void SaveAndStayButton_Click(object sender, EventArgs e)
        {
            if (Page.IsValid)
            {
                try
                {
                    Address address = Save();
                    Response.Redirect("AddressDetails.aspx?AddressID=" + address.AddressId.ToString());
                }
                catch (Exception exc)
                {
                    ProcessException(exc);
                }
            }
        }

        protected void DeleteButton_Click(object sender, EventArgs e)
        {
            try
            {
                Address address = IoCFactory.Resolve<ICustomerManager>().GetAddressById(this.AddressId);
                IoCFactory.Resolve<ICustomerManager>().DeleteAddress(this.AddressId);
                if (address != null)
                    Response.Redirect("CustomerDetails.aspx?CustomerID=" + address.CustomerId.ToString());
                else
                    Response.Redirect("Customers.aspx");
            }
            catch (Exception exc)
            {
                ProcessException(exc);
            }
        }

        protected void ddlCountry_SelectedIndexChanged(object sender, EventArgs e)
        {
            FillStateProvinceDropDowns();
        }

        public int AddressId
        {
            get
            {
                return CommonHelper.QueryStringInt("AddressId");
            }
        }
    }
}