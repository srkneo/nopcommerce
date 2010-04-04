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
using System.Collections.Generic;
using System.Data;
using System.Data.Common;
using System.Globalization;
using System.Security.Cryptography;
using System.Text;
using System.Web;
using System.Web.Security;
using NopSolutions.NopCommerce.BusinessLogic.Caching;
using NopSolutions.NopCommerce.BusinessLogic.Configuration.Settings;
using NopSolutions.NopCommerce.BusinessLogic.Localization;
using NopSolutions.NopCommerce.BusinessLogic.Messages;
using NopSolutions.NopCommerce.BusinessLogic.Orders;
using NopSolutions.NopCommerce.BusinessLogic.Profile;
using NopSolutions.NopCommerce.BusinessLogic.Promo.Affiliates;
using NopSolutions.NopCommerce.BusinessLogic.Tax;
using NopSolutions.NopCommerce.BusinessLogic.Utils;
using NopSolutions.NopCommerce.Common;
using NopSolutions.NopCommerce.Common.Utils;
using NopSolutions.NopCommerce.DataAccess;
using NopSolutions.NopCommerce.DataAccess.CustomerManagement;
 

namespace NopSolutions.NopCommerce.BusinessLogic.CustomerManagement
{
    /// <summary>
    /// Customer manager
    /// </summary>
    public partial class CustomerManager
    {
        #region Constants
        private const string CUSTOMERROLES_ALL_KEY = "Nop.customerrole.all-{0}";
        private const string CUSTOMERROLES_BY_ID_KEY = "Nop.customerrole.id-{0}";
        private const string CUSTOMERROLES_BY_DISCOUNTID_KEY = "Nop.customerrole.bydiscountid-{0}-{1}";
        private const string CUSTOMERROLES_PATTERN_KEY = "Nop.customerrole.";
        #endregion

        #region Utilities

        private static AddressCollection DBMapping(DBAddressCollection dbCollection)
        {
            if (dbCollection == null)
                return null;

            var collection = new AddressCollection();
            foreach (var dbItem in dbCollection)
            {
                var item = DBMapping(dbItem);
                collection.Add(item);
            }

            return collection;
        }

        private static Address DBMapping(DBAddress dbItem)
        {
            if (dbItem == null)
                return null;

            var item = new Address();
            item.AddressID = dbItem.AddressID;
            item.CustomerID = dbItem.CustomerID;
            item.IsBillingAddress = dbItem.IsBillingAddress;
            item.FirstName = dbItem.FirstName;
            item.LastName = dbItem.LastName;
            item.PhoneNumber = dbItem.PhoneNumber;
            item.Email = dbItem.Email;
            item.FaxNumber = dbItem.FaxNumber;
            item.Company = dbItem.Company;
            item.Address1 = dbItem.Address1;
            item.Address2 = dbItem.Address2;
            item.City = dbItem.City;
            item.StateProvinceID = dbItem.StateProvinceID;
            item.ZipPostalCode = dbItem.ZipPostalCode;
            item.CountryID = dbItem.CountryID;
            item.CreatedOn = dbItem.CreatedOn;
            item.UpdatedOn = dbItem.UpdatedOn;

            return item;
        }

        private static CustomerCollection DBMapping(DBCustomerCollection dbCollection)
        {
            if (dbCollection == null)
                return null;

            var collection = new CustomerCollection();
            foreach (var dbItem in dbCollection)
            {
                var item = DBMapping(dbItem);
                collection.Add(item);
            }

            return collection;
        }

        private static Customer DBMapping(DBCustomer dbItem)
        {
            if (dbItem == null)
                return null;

            var item = new Customer();
            item.CustomerID = dbItem.CustomerID;
            item.CustomerGUID = dbItem.CustomerGUID;
            item.Email = dbItem.Email;
            item.Username = dbItem.Username;
            item.PasswordHash = dbItem.PasswordHash;
            item.SaltKey = dbItem.SaltKey;
            item.AffiliateID = dbItem.AffiliateID;
            item.BillingAddressID = dbItem.BillingAddressID;
            item.ShippingAddressID = dbItem.ShippingAddressID;
            item.LastPaymentMethodID = dbItem.LastPaymentMethodID;
            item.LastAppliedCouponCode = dbItem.LastAppliedCouponCode;
            item.GiftCardCouponCodes = dbItem.GiftCardCouponCodes;
            item.LanguageID = dbItem.LanguageID;
            item.CurrencyID = dbItem.CurrencyID;
            item.TaxDisplayTypeID = dbItem.TaxDisplayTypeID;
            item.IsTaxExempt = dbItem.IsTaxExempt;
            item.IsAdmin = dbItem.IsAdmin;
            item.IsGuest = dbItem.IsGuest;
            item.IsForumModerator = dbItem.IsForumModerator;
            item.TotalForumPosts = dbItem.TotalForumPosts;
            item.Signature = dbItem.Signature;
            item.AdminComment = dbItem.AdminComment;
            item.Active = dbItem.Active;
            item.Deleted = dbItem.Deleted;
            item.RegistrationDate = dbItem.RegistrationDate;
            item.TimeZoneID = dbItem.TimeZoneID;
            item.AvatarID = dbItem.AvatarID;

            return item;
        }
        
        private static CustomerAttributeCollection DBMapping(DBCustomerAttributeCollection dbCollection)
        {
            if (dbCollection == null)
                return null;

            var collection = new CustomerAttributeCollection();
            foreach (var dbItem in dbCollection)
            {
                var item = DBMapping(dbItem);
                collection.Add(item);
            }

            return collection;
        }

        private static CustomerAttribute DBMapping(DBCustomerAttribute dbItem)
        {
            if (dbItem == null)
                return null;

            var item = new CustomerAttribute();
            item.CustomerAttributeID = dbItem.CustomerAttributeID;
            item.CustomerID = dbItem.CustomerID;
            item.Key = dbItem.Key;
            item.Value = dbItem.Value;

            return item;
        }

        private static CustomerRoleCollection DBMapping(DBCustomerRoleCollection dbCollection)
        {
            if (dbCollection == null)
                return null;

            var collection = new CustomerRoleCollection();
            foreach (var dbItem in dbCollection)
            {
                var item = DBMapping(dbItem);
                collection.Add(item);
            }

            return collection;
        }

        private static CustomerRole DBMapping(DBCustomerRole dbItem)
        {
            if (dbItem == null)
                return null;

            var item = new CustomerRole();
            item.CustomerRoleID = dbItem.CustomerRoleID;
            item.Name = dbItem.Name;
            item.FreeShipping = dbItem.FreeShipping;
            item.TaxExempt = dbItem.TaxExempt;
            item.Active = dbItem.Active;
            item.Deleted = dbItem.Deleted;

            return item;
        }

        private static CustomerSessionCollection DBMapping(DBCustomerSessionCollection dbCollection)
        {
            if (dbCollection == null)
                return null;

            var collection = new CustomerSessionCollection();
            foreach (var dbItem in dbCollection)
            {
                var item = DBMapping(dbItem);
                collection.Add(item);
            }

            return collection;
        }

        private static CustomerSession DBMapping(DBCustomerSession dbItem)
        {
            if (dbItem == null)
                return null;

            var item = new CustomerSession();
            item.CustomerSessionGUID = dbItem.CustomerSessionGUID;
            item.CustomerID = dbItem.CustomerID;
            item.LastAccessed = dbItem.LastAccessed;
            item.IsExpired = dbItem.IsExpired;

            return item;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Deletes an address by address identifier 
        /// </summary>
        /// <param name="AddressID">Address identifier</param>
        public static void DeleteAddress(int AddressID)
        {
            var address = GetAddressByID(AddressID);
            if (address != null)
            {
                DBProviderManager<DBCustomerProvider>.Provider.DeleteAddress(AddressID);
                var customer = address.Customer;
                if (customer != null)
                {
                    if (customer.BillingAddressID == address.AddressID)
                        customer = SetDefaultBillingAddress(customer.CustomerID, 0);

                    if (customer.ShippingAddressID == address.AddressID)
                        customer = SetDefaultShippingAddress(customer.CustomerID, 0);
                }
            }
        }

        /// <summary>
        /// Gets an address by address identifier
        /// </summary>
        /// <param name="AddressID">Address identifier</param>
        /// <returns>Address</returns>
        public static Address GetAddressByID(int AddressID)
        {
            if (AddressID == 0)
                return null;

            var dbItem = DBProviderManager<DBCustomerProvider>.Provider.GetAddressByID(AddressID);
            var address = DBMapping(dbItem);
            return address;
        }

        /// <summary>
        /// Gets a collection of addresses by customer identifier
        /// </summary>
        /// <param name="CustomerID">Customer identifier</param>
        /// <param name="GetBillingAddresses">Gets or sets a value indicating whether the addresses are billing or shipping</param>
        /// <returns>A collection of addresses</returns>
        public static AddressCollection GetAddressesByCustomerID(int CustomerID, bool GetBillingAddresses)
        {
            var dbCollection = DBProviderManager<DBCustomerProvider>.Provider.GetAddressesByCustomerID(CustomerID, GetBillingAddresses);
            var collection = DBMapping(dbCollection);
            return collection;
        }

        /// <summary>
        /// Inserts an address
        /// </summary>
        /// <param name="CustomerID">The customer identifier</param>
        /// <param name="IsBillingAddress">A value indicating whether the address is billing or shipping</param>
        /// <param name="FirstName">The first name</param>
        /// <param name="LastName">The last name</param>
        /// <param name="PhoneNumber">The phone number</param>
        /// <param name="Email">The email</param>
        /// <param name="FaxNumber">The fax number</param>
        /// <param name="Company">The company</param>
        /// <param name="Address1">The address 1</param>
        /// <param name="Address2">The address 2</param>
        /// <param name="City">The city</param>
        /// <param name="StateProvinceID">The state/province identifier</param>
        /// <param name="ZipPostalCode">The zip/postal code</param>
        /// <param name="CountryID">The country identifier</param>
        /// <param name="CreatedOn">The date and time of instance creation</param>
        /// <param name="UpdatedOn">The date and time of instance update</param>
        /// <returns>An address</returns>
        public static Address InsertAddress(int CustomerID, bool IsBillingAddress, string FirstName, string LastName,
            string PhoneNumber, string Email, string FaxNumber, string Company, string Address1,
            string Address2, string City, int StateProvinceID, string ZipPostalCode,
            int CountryID, DateTime CreatedOn, DateTime UpdatedOn)
        {
            if (FirstName == null)
                FirstName = string.Empty;
            if (LastName == null)
                LastName = string.Empty;
            if (PhoneNumber == null)
                PhoneNumber = string.Empty;
            if (Email == null)
                Email = string.Empty;
            if (FaxNumber == null)
                FaxNumber = string.Empty;
            if (Company == null)
                Company = string.Empty;
            if (Address1 == null)
                Address1 = string.Empty;
            if (Address2 == null)
                Address2 = string.Empty;
            if (City == null)
                City = string.Empty;
            if (ZipPostalCode == null)
                ZipPostalCode = string.Empty;
            FirstName = FirstName.Trim();
            LastName = LastName.Trim();
            PhoneNumber = PhoneNumber.Trim();
            Email = Email.Trim();
            FaxNumber = FaxNumber.Trim();
            Company = Company.Trim();
            Address1 = Address1.Trim();
            Address2 = Address2.Trim();
            City = City.Trim();
            ZipPostalCode = ZipPostalCode.Trim();

            CreatedOn = DateTimeHelper.ConvertToUtcTime(CreatedOn);
            UpdatedOn = DateTimeHelper.ConvertToUtcTime(UpdatedOn);

            var dbItem = DBProviderManager<DBCustomerProvider>.Provider.InsertAddress(CustomerID,
                IsBillingAddress, FirstName, LastName,
                PhoneNumber, Email, FaxNumber, Company, Address1,
                Address2, City, StateProvinceID, ZipPostalCode,
                CountryID, CreatedOn, UpdatedOn);
            var address = DBMapping(dbItem);
            return address;
        }

        /// <summary>
        /// Updates the address
        /// </summary>
        /// <param name="AddressID">The address identifier</param>
        /// <param name="CustomerID">The customer identifier</param>
        /// <param name="IsBillingAddress">A value indicating whether the address is billing or shipping</param>
        /// <param name="FirstName">The first name</param>
        /// <param name="LastName">The last name</param>
        /// <param name="PhoneNumber">The phone number</param>
        /// <param name="Email">The email</param>
        /// <param name="FaxNumber">The fax number</param>
        /// <param name="Company">The company</param>
        /// <param name="Address1">The address 1</param>
        /// <param name="Address2">The address 2</param>
        /// <param name="City">The city</param>
        /// <param name="StateProvinceID">The state/province identifier</param>
        /// <param name="ZipPostalCode">The zip/postal code</param>
        /// <param name="CountryID">The country identifier</param>
        /// <param name="CreatedOn">The date and time of instance creation</param>
        /// <param name="UpdatedOn">The date and time of instance update</param>
        /// <returns>An address</returns>
        public static Address UpdateAddress(int AddressID, int CustomerID, bool IsBillingAddress, string FirstName, string LastName,
            string PhoneNumber, string Email, string FaxNumber, string Company,
            string Address1, string Address2, string City, int StateProvinceID,
            string ZipPostalCode, int CountryID, DateTime CreatedOn, DateTime UpdatedOn)
        {
            if (FirstName == null)
                FirstName = string.Empty;
            if (LastName == null)
                LastName = string.Empty;
            if (PhoneNumber == null)
                PhoneNumber = string.Empty;
            if (Email == null)
                Email = string.Empty;
            if (FaxNumber == null)
                FaxNumber = string.Empty;
            if (Company == null)
                Company = string.Empty;
            if (Address1 == null)
                Address1 = string.Empty;
            if (Address2 == null)
                Address2 = string.Empty;
            if (City == null)
                City = string.Empty;
            if (ZipPostalCode == null)
                ZipPostalCode = string.Empty;
            FirstName = FirstName.Trim();
            LastName = LastName.Trim();
            PhoneNumber = PhoneNumber.Trim();
            Email = Email.Trim();
            FaxNumber = FaxNumber.Trim();
            Company = Company.Trim();
            Address1 = Address1.Trim();
            Address2 = Address2.Trim();
            City = City.Trim();
            ZipPostalCode = ZipPostalCode.Trim();

            CreatedOn = DateTimeHelper.ConvertToUtcTime(CreatedOn);
            UpdatedOn = DateTimeHelper.ConvertToUtcTime(UpdatedOn);

            var dbItem = DBProviderManager<DBCustomerProvider>.Provider.UpdateAddress(AddressID, 
                CustomerID, IsBillingAddress, FirstName, LastName,
                PhoneNumber, Email, FaxNumber, Company,
                Address1, Address2, City, StateProvinceID,
                ZipPostalCode, CountryID, CreatedOn, UpdatedOn);
            var address = DBMapping(dbItem);
            return address;
        }

        /// <summary>
        /// Gets a value indicating whether address can be used as billing address
        /// </summary>
        /// <param name="address">Address to validate</param>
        /// <returns>Result</returns>
        public static  bool CanUseAddressAsBillingAddress(Address address)
        {
            if (address == null)
                return false;

            if (address.Country == null)
                return false;

            if (!address.Country.AllowsBilling)
                return false;

            return true;
        }

        /// <summary>
        /// Gets a value indicating whether address can be used as shipping address
        /// </summary>
        /// <param name="address">Address to validate</param>
        /// <returns>Result</returns>
        public static bool CanUseAddressAsShippingAddress(Address address)
        {
            if (address == null)
                return false;

            if (address.Country == null)
                return false;

            if (!address.Country.AllowsShipping)
                return false;

            return true;
        }

        /// <summary>
        /// Reset data required for checkout
        /// </summary>
        /// <param name="CustomerID">Customer identifier</param>
        /// <param name="ClearCouponCode">A value indicating whether to clear coupon code</param>
        public static void ResetCheckoutData(int CustomerID, bool ClearCouponCode)
        {
            var customer = GetCustomerByID(CustomerID);
            if (customer != null)
            {
                customer = SetDefaultShippingAddress(customer.CustomerID, 0);
                customer = SetDefaultBillingAddress(customer.CustomerID, 0);
                customer.LastShippingOption = null;
                customer = SetLastPaymentMethodID(customer.CustomerID, 0);
                if (ClearCouponCode)
                {
                    customer = ApplyDiscountCouponCode(customer.CustomerID, string.Empty);
                    customer = ApplyGiftCardCouponCode(customer.CustomerID, string.Empty);
                }
            }
        }

        /// <summary>
        /// Sets a default billing address
        /// </summary>
        /// <param name="CustomerID">Customer identifier</param>
        /// <param name="BillingAddressID">Billing address identifier</param>
        /// <returns>Customer</returns>
        public static Customer SetDefaultBillingAddress(int CustomerID, int BillingAddressID)
        {
            var customer = GetCustomerByID(CustomerID);
            if (customer != null)
            {
                customer = UpdateCustomer(customer.CustomerID, customer.CustomerGUID, customer.Email,
                    customer.Username, customer.PasswordHash, customer.SaltKey, customer.AffiliateID, BillingAddressID,
                    customer.ShippingAddressID, customer.LastPaymentMethodID,
                    customer.LastAppliedCouponCode, customer.GiftCardCouponCodes, customer.LanguageID,
                    customer.CurrencyID, customer.TaxDisplayType, 
                    customer.IsTaxExempt, customer.IsAdmin,
                    customer.IsGuest, customer.IsForumModerator,
                    customer.TotalForumPosts, customer.Signature, customer.AdminComment, customer.Active,
                    customer.Deleted, customer.RegistrationDate, customer.TimeZoneID, customer.AvatarID);
            }
            return customer;
        }

        /// <summary>
        /// Sets a default shipping address
        /// </summary>
        /// <param name="CustomerID">Customer identifier</param>
        /// <param name="ShippingAddressID">Shipping address identifier</param>
        /// <returns>Customer</returns>
        public static Customer SetDefaultShippingAddress(int CustomerID, int ShippingAddressID)
        {
            var customer = GetCustomerByID(CustomerID);
            if (customer != null)
            {
                customer = UpdateCustomer(customer.CustomerID, customer.CustomerGUID, customer.Email,
                    customer.Username, customer.PasswordHash, customer.SaltKey, customer.AffiliateID,
                    customer.BillingAddressID,
                    ShippingAddressID, customer.LastPaymentMethodID,
                    customer.LastAppliedCouponCode, customer.GiftCardCouponCodes, customer.LanguageID,
                    customer.CurrencyID, customer.TaxDisplayType, 
                    customer.IsTaxExempt, customer.IsAdmin,
                    customer.IsGuest, customer.IsForumModerator,
                    customer.TotalForumPosts, customer.Signature, customer.AdminComment, customer.Active,
                    customer.Deleted, customer.RegistrationDate, 
                    customer.TimeZoneID, customer.AvatarID);
            }
            return customer;
        }

        /// <summary>
        /// Sets a customer payment method
        /// </summary>
        /// <param name="CustomerID">Customer identifier</param>
        /// <param name="PaymentMethodID">Payment method identifier</param>
        /// <returns>Customer</returns>
        public static Customer SetLastPaymentMethodID(int CustomerID, int PaymentMethodID)
        {
            var customer = GetCustomerByID(CustomerID);
            if (customer != null)
            {
               customer = UpdateCustomer(customer.CustomerID, customer.CustomerGUID, customer.Email,
                    customer.Username, customer.PasswordHash, customer.SaltKey, customer.AffiliateID, customer.BillingAddressID,
                    customer.ShippingAddressID, PaymentMethodID, customer.LastAppliedCouponCode,
                    customer.GiftCardCouponCodes, customer.LanguageID, customer.CurrencyID, 
                    customer.TaxDisplayType, customer.IsTaxExempt, customer.IsAdmin,
                    customer.IsGuest, customer.IsForumModerator, customer.TotalForumPosts,
                    customer.Signature, customer.AdminComment, customer.Active, customer.Deleted, 
                    customer.RegistrationDate, customer.TimeZoneID, customer.AvatarID);
            }
            return customer;
        }

        /// <summary>
        /// Sets a customer time zone
        /// </summary>
        /// <param name="CustomerID">Customer identifier</param>
        /// <param name="TimeZoneID">Time zone identifier</param>
        /// <returns>Customer</returns>
        public static Customer SetTimeZoneID(int CustomerID, string TimeZoneID)
        {
            var customer = GetCustomerByID(CustomerID);
            if (customer != null)
            {
                customer = UpdateCustomer(customer.CustomerID, customer.CustomerGUID, customer.Email,
                     customer.Username, customer.PasswordHash, customer.SaltKey, customer.AffiliateID, customer.BillingAddressID,
                     customer.ShippingAddressID, customer.LastPaymentMethodID, customer.LastAppliedCouponCode,
                     customer.GiftCardCouponCodes, customer.LanguageID, customer.CurrencyID, customer.TaxDisplayType,
                     customer.IsTaxExempt, customer.IsAdmin,
                     customer.IsGuest, customer.IsForumModerator, customer.TotalForumPosts,
                     customer.Signature, customer.AdminComment, customer.Active, customer.Deleted, customer.RegistrationDate, TimeZoneID, customer.AvatarID);
            }
            return customer;
        }

        /// <summary>
        /// Sets a customer email
        /// </summary>
        /// <param name="CustomerID">Customer identifier</param>
        /// <param name="NewEmail">New email</param>
        /// <returns>Customer</returns>
        public static Customer SetEmail(int CustomerID, string NewEmail)
        {
            if (NewEmail == null)
                NewEmail = string.Empty;
            NewEmail = NewEmail.Trim();

            var customer = GetCustomerByID(CustomerID);
            if (customer != null)
            {
                if (!CommonHelper.IsValidEmail(NewEmail))
                {
                    throw new NopException("New email is not valid");
                }

                var cust2 = GetCustomerByEmail(NewEmail);
                if (cust2 != null && customer.CustomerID != cust2.CustomerID)
                {
                    throw new NopException("The e-mail address is already in use.");
                }

                if (NewEmail.Length > 40)
                {
                    throw new NopException("E-mail address is too long.");
                }

                customer = UpdateCustomer(customer.CustomerID, customer.CustomerGUID, NewEmail,
                    customer.Username, customer.PasswordHash, customer.SaltKey, customer.AffiliateID, 
                    customer.BillingAddressID,  customer.ShippingAddressID, customer.LastPaymentMethodID,
                    customer.LastAppliedCouponCode, customer.GiftCardCouponCodes, customer.LanguageID,
                    customer.CurrencyID, customer.TaxDisplayType,
                    customer.IsTaxExempt, customer.IsAdmin,
                    customer.IsGuest, customer.IsForumModerator, customer.TotalForumPosts,
                    customer.Signature, customer.AdminComment, customer.Active, customer.Deleted, 
                    customer.RegistrationDate, customer.TimeZoneID, customer.AvatarID);
            }
            return customer;
        }

        /// <summary>
        /// Sets a customer sugnature
        /// </summary>
        /// <param name="CustomerID">Customer identifier</param>
        /// <param name="Signature">Signature</param>
        /// <returns>Customer</returns>
        public static Customer SetCustomerSignature(int CustomerID, string Signature)
        {
            if (Signature == null)
                Signature = string.Empty;
            Signature = Signature.Trim();

            int maxLength = 300;
            if (Signature.Length > maxLength)
                Signature = Signature.Substring(0, maxLength);

            var customer = GetCustomerByID(CustomerID);
            if (customer != null)
            {
                customer = UpdateCustomer(customer.CustomerID, customer.CustomerGUID, customer.Email,
                    customer.Username, customer.PasswordHash, customer.SaltKey, customer.AffiliateID,
                    customer.BillingAddressID, customer.ShippingAddressID, customer.LastPaymentMethodID,
                    customer.LastAppliedCouponCode, customer.GiftCardCouponCodes, customer.LanguageID,
                    customer.CurrencyID, customer.TaxDisplayType, 
                    customer.IsTaxExempt, customer.IsAdmin,
                    customer.IsGuest, customer.IsForumModerator, customer.TotalForumPosts,
                    Signature, customer.AdminComment, customer.Active, customer.Deleted,
                    customer.RegistrationDate, customer.TimeZoneID, customer.AvatarID);
            }
            return customer;
        }

        /// <summary>
        /// Sets a customer's affiliate
        /// </summary>
        /// <param name="CustomerID">Customer identifier</param>
        /// <param name="AffiliateID">Affiliate identifier</param>
        /// <returns>Customer</returns>
        public static Customer SetAffiliate(int CustomerID, int AffiliateID)
        {
            var customer = GetCustomerByID(CustomerID);
            if (customer != null)
            {
                customer = UpdateCustomer(customer.CustomerID, customer.CustomerGUID, customer.Email,
                    customer.Username, customer.PasswordHash, customer.SaltKey, AffiliateID,
                    customer.BillingAddressID, customer.ShippingAddressID, customer.LastPaymentMethodID,
                    customer.LastAppliedCouponCode, customer.GiftCardCouponCodes, customer.LanguageID,
                    customer.CurrencyID, customer.TaxDisplayType,
                    customer.IsTaxExempt, customer.IsAdmin,
                    customer.IsGuest, customer.IsForumModerator, customer.TotalForumPosts,
                    customer.Signature, customer.AdminComment, customer.Active, customer.Deleted, 
                    customer.RegistrationDate, customer.TimeZoneID, customer.AvatarID);
            }
            return customer;
        }

        /// <summary>
        /// Removes customer avatar
        /// </summary>
        /// <param name="CustomerID">Customer identifier</param>
        /// <param name="AvatarID">Customer avatar identifier</param>
        /// <returns>Customer</returns>
        public static Customer SetCustomerAvatarID(int CustomerID, int AvatarID)
        {
            var customer = GetCustomerByID(CustomerID);
            if (customer != null)
            {
                customer = UpdateCustomer(customer.CustomerID, customer.CustomerGUID, customer.Email,
                     customer.Username, customer.PasswordHash, customer.SaltKey, customer.AffiliateID, customer.BillingAddressID,
                     customer.ShippingAddressID, customer.LastPaymentMethodID, customer.LastAppliedCouponCode,
                     customer.GiftCardCouponCodes, customer.LanguageID, customer.CurrencyID,
                     customer.TaxDisplayType, customer.IsTaxExempt, customer.IsAdmin,
                     customer.IsGuest, customer.IsForumModerator, customer.TotalForumPosts,
                     customer.Signature, customer.AdminComment, customer.Active, customer.Deleted, 
                     customer.RegistrationDate, customer.TimeZoneID, AvatarID);
            }
            return customer;
        }

        /// <summary>
        /// Create anonymous user for current user
        /// </summary>
        /// <returns>Guest user</returns>
        public static void CreateAnonymousUser()
        {
            //create anonymous record
            string email = "anonymous@anonymous.com";
            string password = string.Empty;
            MembershipCreateStatus status = MembershipCreateStatus.UserRejected;
            var guestCustomer = AddCustomer(email, email, password, false, true, true, out status);
            if (guestCustomer != null && status == MembershipCreateStatus.Success)
            {
                NopContext.Current.User = guestCustomer;

                if (NopContext.Current.Session == null)
                {
                    NopContext.Current.Session = NopContext.Current.GetSession(true);
                }

                DateTime LastAccessed = DateTimeHelper.ConvertToUtcTime(DateTime.Now);

                NopContext.Current.Session = UpdateCustomerSession(NopContext.Current.Session.CustomerSessionGUID,
                    guestCustomer.CustomerID, LastAccessed, NopContext.Current.Session.IsExpired);
            }
        }

        /// <summary>
        /// Applies a discount coupon code to a current customer
        /// </summary>
        /// <param name="CouponCode">Coupon code</param>
        public static void ApplyDiscountCouponCode(string CouponCode)
        {
            if (NopContext.Current.User == null)
            {
                //create anonymous record
                CreateAnonymousUser();
            }
            NopContext.Current.User = ApplyDiscountCouponCode(NopContext.Current.User.CustomerID, CouponCode);
        }

        /// <summary>
        /// Applies a discount coupon code
        /// </summary>
        /// <param name="CustomerID">Customer identifier</param>
        /// <param name="CouponCode">Coupon code</param>
        /// <returns>Customer</returns>
        public static Customer ApplyDiscountCouponCode(int CustomerID, string CouponCode)
        {
            var customer = GetCustomerByID(CustomerID);
            if (customer != null)
            {
                customer = UpdateCustomer(customer.CustomerID, customer.CustomerGUID, customer.Email,
                    customer.Username, customer.PasswordHash, customer.SaltKey, customer.AffiliateID,
                    customer.BillingAddressID, customer.ShippingAddressID, customer.LastPaymentMethodID,
                    CouponCode, customer.GiftCardCouponCodes, customer.LanguageID, customer.CurrencyID,
                    customer.TaxDisplayType, customer.IsTaxExempt, customer.IsAdmin, customer.IsGuest,
                    customer.IsForumModerator, customer.TotalForumPosts, 
                    customer.Signature, customer.AdminComment, customer.Active,
                    customer.Deleted, customer.RegistrationDate, customer.TimeZoneID, customer.AvatarID);
            }
            return customer;
        }

        /// <summary>
        /// Applies a gift card coupon code to a current customer
        /// </summary>
        /// <param name="CouponCodesXML">Coupon code (XML)</param>
        public static void ApplyGiftCardCouponCode(string CouponCodesXML)
        {
            if (NopContext.Current.User == null)
            {
                //create anonymous record
                CreateAnonymousUser();
            }
            NopContext.Current.User = ApplyGiftCardCouponCode(NopContext.Current.User.CustomerID, CouponCodesXML);
        }

        /// <summary>
        /// Applies a gift card coupon code
        /// </summary>
        /// <param name="CustomerID">Customer identifier</param>
        /// <param name="CouponCodesXML">Coupon code (XML)</param>
        /// <returns>Customer</returns>
        public static Customer ApplyGiftCardCouponCode(int CustomerID, string CouponCodesXML)
        {
            var customer = GetCustomerByID(CustomerID);
            if (customer != null)
            {
                customer = UpdateCustomer(customer.CustomerID, customer.CustomerGUID, customer.Email,
                    customer.Username, customer.PasswordHash, customer.SaltKey, customer.AffiliateID,
                    customer.BillingAddressID, customer.ShippingAddressID, customer.LastPaymentMethodID,
                    customer.LastAppliedCouponCode, CouponCodesXML, customer.LanguageID, customer.CurrencyID,
                    customer.TaxDisplayType, customer.IsTaxExempt, customer.IsAdmin, customer.IsGuest,
                    customer.IsForumModerator, customer.TotalForumPosts,
                    customer.Signature, customer.AdminComment, customer.Active,
                    customer.Deleted, customer.RegistrationDate, customer.TimeZoneID, customer.AvatarID);
            }
            return customer;
        }

        /// <summary>
        /// Gets all customers
        /// </summary>
        /// <returns>Customer collection</returns>
        public static CustomerCollection GetAllCustomers()
        {
            int TotalRecords = 0;
            return GetAllCustomers(null, null, null, string.Empty, false, int.MaxValue, 0, out TotalRecords);
        }

        /// <summary>
        /// Gets all customers
        /// </summary>
        /// <param name="RegistrationFrom">Customer registration from; null to load all customers</param>
        /// <param name="RegistrationTo">Customer registration to; null to load all customers</param>
        /// <param name="Email">Customer Email</param>
        /// <param name="Username">Customer username</param>
        /// <param name="DontLoadGuestCustomers">A value indicating whether to don't load guest customers</param>
        /// <param name="PageSize">Page size</param>
        /// <param name="PageIndex">Page index</param>
        /// <param name="TotalRecords">Total records</param>
        /// <returns>Customer collection</returns>
        public static CustomerCollection GetAllCustomers(DateTime? RegistrationFrom, DateTime? RegistrationTo,
            string Email, string Username, bool DontLoadGuestCustomers, 
            int PageSize, int PageIndex, out int TotalRecords)
        {
            if (PageSize <= 0)
                PageSize = 10;
            if (PageSize == int.MaxValue)
                PageSize = int.MaxValue - 1;

            if (PageIndex < 0)
                PageIndex = 0;
            if (PageIndex == int.MaxValue)
                PageIndex = int.MaxValue - 1;

            if (Email == null)
                Email = string.Empty;

            if (Username == null)
                Username = string.Empty;

            var dbCollection = DBProviderManager<DBCustomerProvider>.Provider.GetAllCustomers(RegistrationFrom,
                RegistrationTo, Email, Username, DontLoadGuestCustomers, PageSize, PageIndex, out TotalRecords);
            var customers = DBMapping(dbCollection);
            return customers;
        }

        /// <summary>
        /// Gets all customers by affiliate identifier
        /// </summary>
        /// <param name="AffiliateID">Affiliate identifier</param>
        /// <returns>Customer collection</returns>
        public static CustomerCollection GetAffiliatedCustomers(int AffiliateID)
        {
            var dbCollection = DBProviderManager<DBCustomerProvider>.Provider.GetAffiliatedCustomers(AffiliateID);
            var customers = DBMapping(dbCollection);
            return customers;
        }

        /// <summary>
        /// Gets all customers by customer role id
        /// </summary>
        /// <param name="CustomerRoleID">Customer role identifier</param>
        /// <returns>Customer collection</returns>
        public static CustomerCollection GetCustomersByCustomerRoleID(int CustomerRoleID)
        {
            bool showHidden = NopContext.Current.IsAdmin;
            var dbCollection = DBProviderManager<DBCustomerProvider>.Provider.GetCustomersByCustomerRoleID(CustomerRoleID, showHidden);
            var customers = DBMapping(dbCollection);
            return customers;
        }

        /// <summary>
        /// Marks customer as deleted
        /// </summary>
        /// <param name="CustomerID">Customer identifier</param>
        public static void MarkCustomerAsDeleted(int CustomerID)
        {
            var customer = GetCustomerByID(CustomerID);
            if (customer != null)
            {
                UpdateCustomer(customer.CustomerID, customer.CustomerGUID, customer.Email, 
                    customer.Username, customer.PasswordHash, customer.SaltKey, 
                    customer.AffiliateID, customer.BillingAddressID,
                    customer.ShippingAddressID, customer.LastPaymentMethodID,
                    customer.LastAppliedCouponCode, customer.GiftCardCouponCodes, customer.LanguageID,
                    customer.CurrencyID, customer.TaxDisplayType,
                    customer.IsTaxExempt, customer.IsAdmin,
                    customer.IsGuest, customer.IsForumModerator,
                    customer.TotalForumPosts, customer.Signature, 
                    customer.AdminComment, customer.Active,
                    true, customer.RegistrationDate, customer.TimeZoneID, customer.AvatarID);
            }
        }

        /// <summary>
        /// Gets a customer by email
        /// </summary>
        /// <param name="Email">Customer Email</param>
        /// <returns>A customer</returns>
        public static Customer GetCustomerByEmail(string Email)
        {
            if (string.IsNullOrEmpty(Email))
                return null;

            var dbItem = DBProviderManager<DBCustomerProvider>.Provider.GetCustomerByEmail(Email);
            var customer = DBMapping(dbItem);
            return customer;
        }

        /// <summary>
        /// Gets a customer by email
        /// </summary>
        /// <param name="Username">Customer username</param>
        /// <returns>A customer</returns>
        public static Customer GetCustomerByUsername(string Username)
        {
            if (string.IsNullOrEmpty(Username))
                return null;

            var dbItem = DBProviderManager<DBCustomerProvider>.Provider.GetCustomerByUsername(Username);
            var customer = DBMapping(dbItem);
            return customer;
        }

        /// <summary>
        /// Gets a customer
        /// </summary>
        /// <param name="CustomerID">Customer identifier</param>
        /// <returns>A customer</returns>
        public static Customer GetCustomerByID(int CustomerID)
        {
            if (CustomerID == 0)
                return null;

            var dbItem = DBProviderManager<DBCustomerProvider>.Provider.GetCustomerByID(CustomerID);
            var customer = DBMapping(dbItem);
            return customer;
        }

        /// <summary>
        /// Gets a customer by GUID
        /// </summary>
        /// <param name="CustomerGUID">Customer GUID</param>
        /// <returns>A customer</returns>
        public static Customer GetCustomerByGUID(Guid CustomerGUID)
        {
            if (CustomerGUID == Guid.Empty)
                return null;
            var dbItem = DBProviderManager<DBCustomerProvider>.Provider.GetCustomerByGUID(CustomerGUID);
            var customer = DBMapping(dbItem);
            return customer;
        }

        /// <summary>
        /// Adds a customer
        /// </summary>
        /// <param name="Email">The email</param>
        /// <param name="Username">The username</param>
        /// <param name="Password">The password</param>
        /// <param name="IsAdmin">A value indicating whether the customer is administrator</param>
        /// <param name="IsGuest">A value indicating whether the customer is guest</param>
        /// <param name="Active">A value indicating whether the customer is active</param>
        /// <param name="status">Status</param>
        /// <returns>A customer</returns>
        public static Customer AddCustomer(string Email, string Username, string Password,
            bool IsAdmin, bool IsGuest,  bool Active, out MembershipCreateStatus status)
        {
            int affiliateID = 0;
            HttpCookie affiliateCookie = HttpContext.Current.Request.Cookies.Get("NopCommerce.AffiliateID");
            if (affiliateCookie != null)
            {
                Affiliate affiliate = AffiliateManager.GetAffiliateByID(Convert.ToInt32(affiliateCookie.Value));
                if (affiliate != null && affiliate.Active)
                    affiliateID = affiliate.AffiliateID;
            }

            var customer = AddCustomer(Guid.NewGuid(), Email, Username, Password, affiliateID,
                0, 0, 0, string.Empty, string.Empty,
                NopContext.Current.WorkingLanguage.LanguageID,
                NopContext.Current.WorkingCurrency.CurrencyID,
                NopContext.Current.TaxDisplayType, false, IsAdmin, IsGuest,
                false, 0, string.Empty, string.Empty, Active,
                false, DateTime.Now, string.Empty, 0, out status);

            if (status == MembershipCreateStatus.Success)
            {
                if (affiliateCookie != null)
                {
                    affiliateCookie.Expires = DateTime.Now.AddMonths(-1);
                    if (HttpContext.Current != null)
                    {
                        HttpContext.Current.Response.Cookies.Set(affiliateCookie);
                    }
                }
            }

            return customer;
        }

        /// <summary>
        /// Adds a customer
        /// </summary>
        /// <param name="Email">The email</param>
        /// <param name="Username">The username</param>
        /// <param name="Password">The password</param>
        /// <param name="AffiliateID">The affiliate identifier</param>
        /// <param name="IsAdmin">A value indicating whether the customer is administrator</param>
        /// <param name="IsGuest">A value indicating whether the customer is guest</param>
        /// <param name="Active">A value indicating whether the customer is active</param>
        /// <param name="status">Status</param>
        /// <returns>A customer</returns>
        public static Customer AddCustomer(string Email, string Username, string Password, int AffiliateID, 
            bool IsAdmin, bool IsGuest, bool Active, out MembershipCreateStatus status)
        {
            return AddCustomer(Guid.NewGuid(), Email, Username, Password,
                AffiliateID, 0, 0, 0, string.Empty, string.Empty,
                NopContext.Current.WorkingLanguage.LanguageID,
                NopContext.Current.WorkingCurrency.CurrencyID,
                NopContext.Current.TaxDisplayType, false,
                IsAdmin, IsGuest, false, 0, string.Empty, string.Empty, Active,
                false, DateTime.Now, string.Empty, 0, out status);
        }

        /// <summary>
        /// Adds a customer
        /// </summary>
        /// <param name="CustomerGUID">The customer identifier</param>
        /// <param name="Email">The email</param>
        /// <param name="Username">The username</param>
        /// <param name="Password">The password</param>
        /// <param name="AffiliateID">The affiliate identifier</param>
        /// <param name="BillingAddressID">The billing address identifier</param>
        /// <param name="ShippingAddressID">The shipping address identifier</param>
        /// <param name="LastPaymentMethodID">The last payment method identifier</param>
        /// <param name="LastAppliedCouponCode">The last applied coupon code</param>
        /// <param name="GiftCardCouponCodes">The applied gift card coupon code</param>
        /// <param name="LanguageID">The language identifier</param>
        /// <param name="CurrencyID">The currency identifier</param>
        /// <param name="TaxDisplayType">The tax display type</param>
        /// <param name="IsTaxExempt">A value indicating whether the customer is tax exempt</param>
        /// <param name="IsAdmin">A value indicating whether the customer is administrator</param>
        /// <param name="IsGuest">A value indicating whether the customer is guest</param>
        /// <param name="IsForumModerator">A value indicating whether the customer is forum moderator</param>
        /// <param name="TotalForumPosts">A forum post count</param>
        /// <param name="Signature">Signature</param>
        /// <param name="AdminComment">Admin comment</param>
        /// <param name="Active">A value indicating whether the customer is active</param>
        /// <param name="Deleted">A value indicating whether the customer has been deleted</param>
        /// <param name="RegistrationDate">The date and time of customer registration</param>
        /// <param name="TimeZoneID">The time zone identifier</param>
        /// <param name="AvatarID">The avatar identifier</param>
        /// <param name="status">Status</param>
        /// <returns>A customer</returns>
        public static Customer AddCustomer(Guid CustomerGUID, string Email, string Username,
            string Password, int AffiliateID, int BillingAddressID,
            int ShippingAddressID, int LastPaymentMethodID,
            string LastAppliedCouponCode, string GiftCardCouponCodes, int LanguageID, int CurrencyID, 
            TaxDisplayTypeEnum TaxDisplayType, bool IsTaxExempt, bool IsAdmin, bool IsGuest,
            bool IsForumModerator, int TotalForumPosts, string Signature, string AdminComment,
            bool Active, bool Deleted, DateTime RegistrationDate,
            string TimeZoneID, int AvatarID, out MembershipCreateStatus status)
        {
            Customer customer = null;

            RegistrationDate = DateTimeHelper.ConvertToUtcTime(RegistrationDate);

            if (Username == null)
                Username = string.Empty;
            Username = Username.Trim();

            if (Email == null)
                Email = string.Empty;
            Email = Email.Trim();

            if (Signature == null)
                Signature = string.Empty;
            Signature = Signature.Trim();

            string saltKey = string.Empty;
            string passwordHash = string.Empty;

            status = MembershipCreateStatus.UserRejected;
            if (!IsGuest)
            {
                if (!NopContext.Current.IsAdmin)
                {
                    if (CustomerManager.CustomerRegistrationType == CustomerRegistrationTypeEnum.Disabled)
                    {
                        status = MembershipCreateStatus.ProviderError;
                        return customer;
                    }
                }
                if (CustomerManager.UsernamesEnabled)
                {
                    if (GetCustomerByUsername(Username) != null)
                    {
                        status = MembershipCreateStatus.DuplicateUserName;
                        return customer;
                    }

                    if (Username.Length > 40)
                    {
                        status = MembershipCreateStatus.InvalidUserName;
                        return customer;
                    }
                }

                if (GetCustomerByEmail(Email) != null)
                {
                    status = MembershipCreateStatus.DuplicateEmail;
                    return customer;
                }

                if (!CommonHelper.IsValidEmail(Email))
                {
                    status = MembershipCreateStatus.InvalidEmail;
                    return customer;
                }

                if (Email.Length > 40)
                {
                    status = MembershipCreateStatus.InvalidEmail;
                    return customer;
                }

                if (!NopContext.Current.IsAdmin)
                {
                    if (CustomerManager.CustomerRegistrationType == CustomerRegistrationTypeEnum.EmailValidation ||
                        CustomerManager.CustomerRegistrationType == CustomerRegistrationTypeEnum.AdminApproval)
                    {
                        Active = false;
                    }
                }
                saltKey = CreateSalt(5);
                passwordHash = CreatePasswordHash(Password, saltKey);
            }

            customer = AddCustomerForced(CustomerGUID, Email, Username,
                passwordHash, saltKey, AffiliateID, BillingAddressID,
                ShippingAddressID, LastPaymentMethodID,
                LastAppliedCouponCode, GiftCardCouponCodes, LanguageID, CurrencyID, TaxDisplayType,
                IsTaxExempt, IsAdmin, IsGuest, IsForumModerator, 
                TotalForumPosts, Signature, AdminComment, Active, 
                Deleted, RegistrationDate, TimeZoneID, AvatarID);

            if (!IsGuest)
            {
                DateTime LastAccessed = DateTimeHelper.ConvertToUtcTime(DateTime.Now);
                SaveCustomerSession(Guid.NewGuid(), customer.CustomerID, LastAccessed, false);
            }

            status = MembershipCreateStatus.Success;

            if (!IsGuest)
            {
                if (Active)
                {
                    MessageManager.SendCustomerWelcomeMessage(customer, NopContext.Current.WorkingLanguage.LanguageID);
                }
                else
                {
                    if (CustomerManager.CustomerRegistrationType == CustomerRegistrationTypeEnum.EmailValidation)
                    {
                        Guid accountActivationToken = Guid.NewGuid();
                        customer.AccountActivationToken = accountActivationToken.ToString();

                        MessageManager.SendCustomerEmailValidationMessage(customer, NopContext.Current.WorkingLanguage.LanguageID);
                    }
                }
            }
            return customer;
        }

        /// <summary>
        /// Adds a customer withour any validations, welcome messages
        /// </summary>
        /// <param name="CustomerGUID">The customer identifier</param>
        /// <param name="Email">The email</param>
        /// <param name="Username">The username</param>
        /// <param name="PasswordHash">The password hash</param>
        /// <param name="SaltKey">The salk key</param>
        /// <param name="AffiliateID">The affiliate identifier</param>
        /// <param name="BillingAddressID">The billing address identifier</param>
        /// <param name="ShippingAddressID">The shipping address identifier</param>
        /// <param name="LastPaymentMethodID">The last payment method identifier</param>
        /// <param name="LastAppliedCouponCode">The last applied coupon code</param>
        /// <param name="GiftCardCouponCodes">The applied gift card coupon code</param>
        /// <param name="LanguageID">The language identifier</param>
        /// <param name="CurrencyID">The currency identifier</param>
        /// <param name="TaxDisplayType">The tax display type</param>
        /// <param name="IsTaxExempt">A value indicating whether the customer is tax exempt</param>
        /// <param name="IsAdmin">A value indicating whether the customer is administrator</param>
        /// <param name="IsGuest">A value indicating whether the customer is guest</param>
        /// <param name="IsForumModerator">A value indicating whether the customer is forum moderator</param>
        /// <param name="TotalForumPosts">A forum post count</param>
        /// <param name="Signature">Signature</param>
        /// <param name="AdminComment">Admin comment</param>
        /// <param name="Active">A value indicating whether the customer is active</param>
        /// <param name="Deleted">A value indicating whether the customer has been deleted</param>
        /// <param name="RegistrationDate">The date and time of customer registration</param>
        /// <param name="TimeZoneID">The time zone identifier</param>
        /// <param name="AvatarID">The avatar identifier</param>
        /// <returns>A customer</returns>
        public static Customer AddCustomerForced(Guid CustomerGUID, string Email, string Username,
            string PasswordHash, string SaltKey, int AffiliateID, int BillingAddressID,
            int ShippingAddressID, int LastPaymentMethodID,
            string LastAppliedCouponCode, string GiftCardCouponCodes, int LanguageID, int CurrencyID,
            TaxDisplayTypeEnum TaxDisplayType, bool IsTaxExempt, bool IsAdmin, bool IsGuest,
            bool IsForumModerator, int TotalForumPosts, string Signature, string AdminComment,
            bool Active, bool Deleted, DateTime RegistrationDate,
            string TimeZoneID, int AvatarID)
        {
            if (Username == null)
                Username = string.Empty;
            Username = Username.Trim();

            if (Email == null)
                Email = string.Empty;
            Email = Email.Trim();

            RegistrationDate = DateTimeHelper.ConvertToUtcTime(RegistrationDate);

            var dbItem = DBProviderManager<DBCustomerProvider>.Provider.AddCustomer(CustomerGUID, Email, Username,
                  PasswordHash, SaltKey, AffiliateID, BillingAddressID,
                  ShippingAddressID, LastPaymentMethodID,
                  LastAppliedCouponCode, GiftCardCouponCodes, LanguageID, CurrencyID, (int)TaxDisplayType,
                  IsTaxExempt, IsAdmin, IsGuest, IsForumModerator,
                  TotalForumPosts, Signature, AdminComment, Active,
                  Deleted, RegistrationDate, TimeZoneID, AvatarID);
            var customer = DBMapping(dbItem);

            return customer;
        }

        /// <summary>
        /// Updates the customer
        /// </summary>
        /// <param name="CustomerID">The customer identifier</param>
        /// <param name="CustomerGUID">The customer identifier</param>
        /// <param name="Email">The email</param>
        /// <param name="Username">The username</param>
        /// <param name="PasswordHash">The password hash</param>
        /// <param name="SaltKey">The salk key</param>
        /// <param name="AffiliateID">The affiliate identifier</param>
        /// <param name="BillingAddressID">The billing address identifier</param>
        /// <param name="ShippingAddressID">The shipping address identifier</param>
        /// <param name="LastPaymentMethodID">The last payment method identifier</param>
        /// <param name="LastAppliedCouponCode">The last applied coupon code</param>
        /// <param name="GiftCardCouponCodes">The applied gift card coupon code</param>
        /// <param name="LanguageID">The language identifier</param>
        /// <param name="CurrencyID">The currency identifier</param>
        /// <param name="TaxDisplayType">The tax display type</param>
        /// <param name="IsTaxExempt">A value indicating whether the customer is tax exempt</param>
        /// <param name="IsAdmin">A value indicating whether the customer is administrator</param>
        /// <param name="IsGuest">A value indicating whether the customer is guest</param>
        /// <param name="IsForumModerator">A value indicating whether the customer is forum moderator</param>
        /// <param name="TotalForumPosts">A forum post count</param>
        /// <param name="Signature">Signature</param>
        /// <param name="AdminComment">Admin comment</param>
        /// <param name="Active">A value indicating whether the customer is active</param>
        /// <param name="Deleted">A value indicating whether the customer has been deleted</param>
        /// <param name="RegistrationDate">The date and time of customer registration</param>
        /// <param name="TimeZoneID">The time zone identifier</param>
        /// <param name="AvatarID">The avatar identifier</param>
        /// <returns>A customer</returns>
        public static Customer UpdateCustomer(int CustomerID, Guid CustomerGUID, string Email,
            string Username, string PasswordHash, string SaltKey, int AffiliateID, int BillingAddressID,
            int ShippingAddressID, int LastPaymentMethodID,
            string LastAppliedCouponCode, string GiftCardCouponCodes, int LanguageID, int CurrencyID,
            TaxDisplayTypeEnum TaxDisplayType, bool IsTaxExempt, bool IsAdmin, bool IsGuest,
            bool IsForumModerator, int TotalForumPosts, string Signature, string AdminComment, bool Active,
            bool Deleted, DateTime RegistrationDate, string TimeZoneID, int AvatarID)
        {
            RegistrationDate = DateTimeHelper.ConvertToUtcTime(RegistrationDate);

            if (Username == null)
                Username = string.Empty;
            Username = Username.Trim();

            if (Email == null)
                Email = string.Empty;
            Email = Email.Trim();

            if (Signature == null)
                Signature = string.Empty;
            Signature = Signature.Trim();

            var customer = GetCustomerByID(CustomerID);
            if (customer == null)
                return null;

            var subscriptionOld = customer.NewsLetterSubscription;

            var dbItem = DBProviderManager<DBCustomerProvider>.Provider.UpdateCustomer(CustomerID, CustomerGUID, Email,
                Username, PasswordHash, SaltKey, AffiliateID, BillingAddressID,
                ShippingAddressID, LastPaymentMethodID,
                LastAppliedCouponCode, GiftCardCouponCodes, LanguageID,
                CurrencyID, (int)TaxDisplayType, IsTaxExempt, IsAdmin, IsGuest, IsForumModerator,
                TotalForumPosts, Signature, AdminComment, Active, Deleted, RegistrationDate, TimeZoneID, AvatarID);
            customer = DBMapping(dbItem);

            if (subscriptionOld != null && !Email.ToLower().Equals(subscriptionOld.Email.ToLower()))
            {
                MessageManager.UpdateNewsLetterSubscription(subscriptionOld.NewsLetterSubscriptionID, Email, subscriptionOld.IsActive);
            }

            return DBMapping(dbItem);
        }

        /// <summary>
        /// Modifies password
        /// </summary>
        /// <param name="Email">Customer email</param>
        /// <param name="Oldpassword">Old password</param>
        /// <param name="Password">Password</param>
        public static void ModifyPassword(string Email, string Oldpassword, string Password)
        {
            var customer = GetCustomerByEmail(Email);
            if (customer != null)
            {
                string oldPasswordHash = CreatePasswordHash(Oldpassword, customer.SaltKey);
                if (!customer.PasswordHash.Equals(oldPasswordHash))
                    throw new NopException("Current Password doesn't match.");

                ModifyPassword(customer.CustomerID, Password);
            }
        }

        /// <summary>
        /// Modifies password
        /// </summary>
        /// <param name="Email">Customer email</param>
        /// <param name="NewPassword">New password</param>
        public static void ModifyPassword(string Email, string NewPassword)
        {
            var customer = GetCustomerByEmail(Email);
            if(customer != null)
            {
                ModifyPassword(customer.CustomerID, NewPassword);
            }
        }

        /// <summary>
        /// Modifies password
        /// </summary>
        /// <param name="CustomerID">Customer identifier</param>
        /// <param name="NewPassword">New password</param>
        public static void ModifyPassword(int CustomerID, string NewPassword)
        {
            if (String.IsNullOrEmpty(NewPassword))
                throw new NopException(LocalizationManager.GetLocaleResourceString("Customer.PasswordIsRequired"));
            var customer = GetCustomerByID(CustomerID);
            if(customer != null)
            {
                string newPasswordSalt = CreateSalt(5);
                string newPasswordHash = CreatePasswordHash(NewPassword, newPasswordSalt);
                UpdateCustomer(customer.CustomerID, customer.CustomerGUID, customer.Email,
                    customer.Username, newPasswordHash, newPasswordSalt,
                    customer.AffiliateID, customer.BillingAddressID,
                    customer.ShippingAddressID, customer.LastPaymentMethodID,
                    customer.LastAppliedCouponCode, customer.GiftCardCouponCodes, customer.LanguageID,
                    customer.CurrencyID, customer.TaxDisplayType,
                    customer.IsTaxExempt, customer.IsAdmin, customer.IsGuest,
                    customer.IsForumModerator, customer.TotalForumPosts,
                    customer.Signature, customer.AdminComment, customer.Active,
                    customer.Deleted, customer.RegistrationDate, customer.TimeZoneID, customer.AvatarID);
            }
        }

        /// <summary>
        /// Activates a customer
        /// </summary>
        /// <param name="CustomerGUID">Customer identifier</param>
        public static void Activate(Guid CustomerGUID)
        {
            var customer = GetCustomerByGUID(CustomerGUID);
            if (customer != null)
            {
                Activate(customer.CustomerID);
            }
        }

        /// <summary>
        /// Activates a customer
        /// </summary>
        /// <param name="CustomerID">Customer identifier</param>
        public static void Activate(int CustomerID)
        {
            Activate(CustomerID, false);
        }

        /// <summary>
        /// Activates a customer
        /// </summary>
        /// <param name="CustomerID">Customer identifier</param>
        /// <param name="SendCustomerWelcomeMessage">A value indivating whether to send customer welcome message</param>
        public static void Activate(int CustomerID, bool SendCustomerWelcomeMessage)
        {
            var customer = GetCustomerByID(CustomerID);
            if (customer != null)
            {
                customer = UpdateCustomer(customer.CustomerID, customer.CustomerGUID, 
                    customer.Email, customer.Username, 
                    customer.PasswordHash, customer.SaltKey, customer.AffiliateID, customer.BillingAddressID,
                    customer.ShippingAddressID, customer.LastPaymentMethodID,
                    customer.LastAppliedCouponCode, customer.GiftCardCouponCodes, customer.LanguageID,
                    customer.CurrencyID, customer.TaxDisplayType,
                    customer.IsTaxExempt, customer.IsAdmin, customer.IsGuest,
                    customer.IsForumModerator, customer.TotalForumPosts,
                    customer.Signature, customer.AdminComment, true,
                    customer.Deleted, customer.RegistrationDate, customer.TimeZoneID, customer.AvatarID);

                if (SendCustomerWelcomeMessage)
                {
                    MessageManager.SendCustomerWelcomeMessage(customer, NopContext.Current.WorkingLanguage.LanguageID);
                }
            }
        }

        /// <summary>
        /// Deactivates a customer
        /// </summary>
        /// <param name="CustomerGUID">Customer identifier</param>
        public static void Deactivate(Guid CustomerGUID)
        {
            var customer = GetCustomerByGUID(CustomerGUID);
            if (customer != null)
            {
                Deactivate(customer.CustomerID);
            }
        }

        /// <summary>
        /// Deactivates a customer
        /// </summary>
        /// <param name="CustomerID">Customer identifier</param>
        public static void Deactivate(int CustomerID)
        {
            var customer = GetCustomerByID(CustomerID);
            if (customer != null)
            {
                customer = UpdateCustomer(customer.CustomerID, customer.CustomerGUID, customer.Email,
                    customer.Username, customer.PasswordHash, customer.SaltKey, customer.AffiliateID,
                    customer.BillingAddressID, customer.ShippingAddressID,
                    customer.LastPaymentMethodID, customer.LastAppliedCouponCode,
                    customer.GiftCardCouponCodes, customer.LanguageID, customer.CurrencyID, customer.TaxDisplayType,
                    customer.IsTaxExempt, customer.IsAdmin, 
                    customer.IsGuest, customer.IsForumModerator,
                    customer.TotalForumPosts, customer.Signature, 
                    customer.AdminComment, false, customer.Deleted,
                    customer.RegistrationDate, customer.TimeZoneID, customer.AvatarID);
            }
        }

        /// <summary>
        /// Login a customer
        /// </summary>
        /// <param name="Email">A customer email</param>
        /// <param name="Password">Password</param>
        /// <returns>Result</returns>
        public static bool Login(string Email, string Password)
        {
            if (Email == null)
                Email = string.Empty;
            Email = Email.Trim();

            var customer = GetCustomerByEmail(Email);

            if (customer == null)
                return false;

            if (!customer.Active)
                return false;

            if (customer.Deleted)
                return false;

            if (customer.IsGuest)
                return false;

            string passwordHash = CreatePasswordHash(Password, customer.SaltKey);
            bool result = customer.PasswordHash.Equals(passwordHash);
            if (result)
            {
                var registeredCustomerSession = GetCustomerSessionByCustomerID(customer.CustomerID);
                if (registeredCustomerSession != null)
                {
                    registeredCustomerSession.IsExpired = false;
                    var anonCustomerSession = NopContext.Current.Session;
                    var cart1 = ShoppingCartManager.GetCurrentShoppingCart(ShoppingCartTypeEnum.ShoppingCart);
                    var cart2 = ShoppingCartManager.GetCurrentShoppingCart(ShoppingCartTypeEnum.Wishlist);
                    NopContext.Current.Session = registeredCustomerSession;
                    
                    if ((anonCustomerSession != null) && (anonCustomerSession.CustomerSessionGUID != registeredCustomerSession.CustomerSessionGUID))
                    {
                        if (anonCustomerSession.Customer != null)
                        {
                            customer = ApplyDiscountCouponCode(customer.CustomerID, anonCustomerSession.Customer.LastAppliedCouponCode);
                            customer = ApplyGiftCardCouponCode(customer.CustomerID, anonCustomerSession.Customer.GiftCardCouponCodes);
                        }

                        foreach (ShoppingCartItem item in cart1)
                        {
                            ShoppingCartManager.AddToCart(item.ShoppingCartType, item.ProductVariantID, item.AttributesXML, item.Quantity);
                            ShoppingCartManager.DeleteShoppingCartItem(item.ShoppingCartItemID, true);
                        }
                        foreach (ShoppingCartItem item in cart2)
                        {
                            ShoppingCartManager.AddToCart(item.ShoppingCartType, item.ProductVariantID, item.AttributesXML, item.Quantity);
                            ShoppingCartManager.DeleteShoppingCartItem(item.ShoppingCartItemID, true);
                        }
                    }
                }
                if (NopContext.Current.Session == null)
                    NopContext.Current.Session = NopContext.Current.GetSession(true);
                NopContext.Current.Session.IsExpired = false;
                NopContext.Current.Session.LastAccessed = DateTimeHelper.ConvertToUtcTime(DateTime.Now);
                NopContext.Current.Session.CustomerID = customer.CustomerID;
                NopContext.Current.Session = SaveCustomerSession(NopContext.Current.Session.CustomerSessionGUID, NopContext.Current.Session.CustomerID, NopContext.Current.Session.LastAccessed, NopContext.Current.Session.IsExpired);
            }
            return result;
        }

        /// <summary>
        /// Logout customer
        /// </summary>
        public static void Logout()
        {
            if (NopContext.Current != null)
                NopContext.Current.ResetSession();

            if (HttpContext.Current != null && HttpContext.Current.Session != null)
                HttpContext.Current.Session.Abandon();
            FormsAuthentication.SignOut();
        }

        /// <summary>
        /// Creates a password hash
        /// </summary>
        /// <param name="Password">Password</param>
        /// <param name="Salt">Salt</param>
        /// <returns>Password hash</returns>
        private static string CreatePasswordHash(string Password, string Salt)
        {
            //MD5, SHA1
            string passwordFormat = SettingManager.GetSettingValue("Security.PasswordFormat");
            if (String.IsNullOrEmpty(passwordFormat))
                passwordFormat = "SHA1";

            return FormsAuthentication.HashPasswordForStoringInConfigFile(Password + Salt, passwordFormat);
        }

        /// <summary>
        /// Creates a salt
        /// </summary>
        /// <param name="size">A salt size</param>
        /// <returns>A salt</returns>
        private static string CreateSalt(int size)
        {
            var provider = new RNGCryptoServiceProvider();
            byte[] data = new byte[size];
            provider.GetBytes(data);
            return Convert.ToBase64String(data);
        }

        /// <summary>
        /// Deletes a customer attribute
        /// </summary>
        /// <param name="CustomerAttributeID">Customer attribute identifier</param>
        public static void DeleteCustomerAttribute(int CustomerAttributeID)
        {
            DBProviderManager<DBCustomerProvider>.Provider.DeleteCustomerAttribute(CustomerAttributeID);
        }

        /// <summary>
        /// Gets a customer attribute
        /// </summary>
        /// <param name="CustomerAttributeID">Customer attribute identifier</param>
        /// <returns>A customer attribute</returns>
        public static CustomerAttribute GetCustomerAttributeByID(int CustomerAttributeID)
        {
            if (CustomerAttributeID == 0)
                return null;

            var dbItem = DBProviderManager<DBCustomerProvider>.Provider.GetCustomerAttributeByID(CustomerAttributeID);
            var customerAttribute = DBMapping(dbItem);
            return customerAttribute;
        }

        /// <summary>
        /// Gets a collection of customer attributes by customer identifier
        /// </summary>
        /// <param name="CustomerID">Customer identifier</param>
        /// <returns>Customer attributes</returns>
        public static CustomerAttributeCollection GetCustomerAttributesByCustomerID(int CustomerID)
        {
            var dbCollection = DBProviderManager<DBCustomerProvider>.Provider.GetCustomerAttributesByCustomerID(CustomerID);
            var collection = DBMapping(dbCollection);
            return collection;
        }

        /// <summary>
        /// Inserts a customer attribute
        /// </summary>
        /// <param name="CustomerID">Customer identifier</param>
        /// <param name="Key">An attribute key</param>
        /// <param name="Value">An attribute value</param>
        /// <returns>A customer attribute</returns>
        public static CustomerAttribute InsertCustomerAttribute(int CustomerID, string Key, string Value)
        {
            if (CustomerID == 0)
                throw new NopException("Cannot insert attribute for non existing customer");

            if (Value == null)
                Value = string.Empty;

            var dbItem = DBProviderManager<DBCustomerProvider>.Provider.InsertCustomerAttribute(CustomerID, Key, Value);
            var customerAttribute = DBMapping(dbItem);
            return customerAttribute;
        }

        /// <summary>
        /// Updates the customer attribute
        /// </summary>
        /// <param name="CustomerAttributeID">Customer attribute identifier</param>
        /// <param name="CustomerID">Customer identifier</param>
        /// <param name="Key">An attribute key</param>
        /// <param name="Value">An attribute value</param>
        /// <returns>A customer attribute</returns>
        public static CustomerAttribute UpdateCustomerAttribute(int CustomerAttributeID, int CustomerID, string Key, string Value)
        {
            if (CustomerID == 0)
                throw new NopException("Cannot update attribute for non existing customer");

            if (Value == null)
                Value = string.Empty;

            var dbItem = DBProviderManager<DBCustomerProvider>.Provider.UpdateCustomerAttribute(CustomerAttributeID, CustomerID, Key, Value);
            var customerAttribute = DBMapping(dbItem);
            return customerAttribute;
        }

        /// <summary>
        /// Marks customer role as deleted
        /// </summary>
        /// <param name="CustomerRoleID">Customer role identifier</param>
        public static void MarkCustomerRoleAsDeleted(int CustomerRoleID)
        {
            var customerRole = GetCustomerRoleByID(CustomerRoleID);
            if (customerRole != null)
            {
                customerRole = UpdateCustomerRole(customerRole.CustomerRoleID, 
                    customerRole.Name, customerRole.FreeShipping, 
                    customerRole.TaxExempt, customerRole.Active, true);
            }

            if (CustomerManager.CacheEnabled)
            {
                NopCache.RemoveByPattern(CUSTOMERROLES_PATTERN_KEY);
            }
        }

        /// <summary>
        /// Gets a customer role
        /// </summary>
        /// <param name="CustomerRoleID">Customer role identifier</param>
        /// <returns>Customer role</returns>
        public static CustomerRole GetCustomerRoleByID(int CustomerRoleID)
        {
            if (CustomerRoleID == 0)
                return null;

            string key = string.Format(CUSTOMERROLES_BY_ID_KEY, CustomerRoleID);
            object obj2 = NopCache.Get(key);
            if (CustomerManager.CacheEnabled && (obj2 != null))
            {
                return (CustomerRole)obj2;
            }

            var dbItem = DBProviderManager<DBCustomerProvider>.Provider.GetCustomerRoleByID(CustomerRoleID);
            var customerRole = DBMapping(dbItem);

            if (CustomerManager.CacheEnabled)
            {
                NopCache.Max(key, customerRole);
            }
            return customerRole;
        }

        /// <summary>
        /// Gets all customer roles
        /// </summary>
        /// <returns>Customer role collection</returns>
        public static CustomerRoleCollection GetAllCustomerRoles()
        {
            bool showHidden = NopContext.Current.IsAdmin;
            string key = string.Format(CUSTOMERROLES_ALL_KEY, showHidden);
            object obj2 = NopCache.Get(key);
            if (CustomerManager.CacheEnabled && (obj2 != null))
            {
                return (CustomerRoleCollection)obj2;
            }

            var dbCollection = DBProviderManager<DBCustomerProvider>.Provider.GetAllCustomerRoles(showHidden);
            var customerRoleCollection = DBMapping(dbCollection);
            if (CustomerManager.CacheEnabled)
            {
                NopCache.Max(key, customerRoleCollection);
            }
            return customerRoleCollection;
        }

        /// <summary>
        /// Gets customer roles by customer identifier
        /// </summary>
        /// <param name="CustomerID">Customer identifier</param>
        /// <returns>Customer role collection</returns>
        public static CustomerRoleCollection GetCustomerRolesByCustomerID(int CustomerID)
        {
            bool showHidden = NopContext.Current.IsAdmin;
            return GetCustomerRolesByCustomerID(CustomerID, showHidden);
        }

        /// <summary>
        /// Gets customer roles by customer identifier
        /// </summary>
        /// <param name="CustomerID">Customer identifier</param>
        /// <param name="showHidden">A value indicating whether to show hidden records</param>
        /// <returns>Customer role collection</returns>
        public static CustomerRoleCollection GetCustomerRolesByCustomerID(int CustomerID, bool showHidden)
        {
            if (CustomerID == 0)
                return new CustomerRoleCollection();

            var dbCollection = DBProviderManager<DBCustomerProvider>.Provider.GetCustomerRolesByCustomerID(CustomerID, showHidden);
            var customerRoleCollection = DBMapping(dbCollection);
            return customerRoleCollection;
        }

        /// <summary>
        /// Inserts a customer role
        /// </summary>
        /// <param name="Name">The customer role name</param>
        /// <param name="FreeShipping">A value indicating whether the customer role is marked as free shiping</param>
        /// <param name="TaxExempt">A value indicating whether the customer role is marked as tax exempt</param>
        /// <param name="Active">A value indicating whether the customer role is active</param>
        /// <param name="Deleted">A value indicating whether the customer role has been deleted</param>
        /// <returns>Customer role</returns>
        public static CustomerRole InsertCustomerRole(string Name, bool FreeShipping, bool TaxExempt,
            bool Active, bool Deleted)
        {
            var dbItem = DBProviderManager<DBCustomerProvider>.Provider.InsertCustomerRole(Name,
                FreeShipping, TaxExempt, Active, Deleted);
            var customerRole = DBMapping(dbItem);

            if (CustomerManager.CacheEnabled)
            {
                NopCache.RemoveByPattern(CUSTOMERROLES_PATTERN_KEY);
            }

            return customerRole;
        }

        /// <summary>
        /// Updates the customer role
        /// </summary>
        /// <param name="CustomerRoleID">The customer role identifier</param>
        /// <param name="Name">The customer role name</param>
        /// <param name="FreeShipping">A value indicating whether the customer role is marked as free shiping</param>
        /// <param name="TaxExempt">A value indicating whether the customer role is marked as tax exempt</param>
        /// <param name="Active">A value indicating whether the customer role is active</param>
        /// <param name="Deleted">A value indicating whether the customer role has been deleted</param>
        /// <returns>Customer role</returns>
        public static CustomerRole UpdateCustomerRole(int CustomerRoleID, string Name,
            bool FreeShipping, bool TaxExempt, bool Active, bool Deleted)
        {
            var dbItem = DBProviderManager<DBCustomerProvider>.Provider.UpdateCustomerRole(CustomerRoleID, Name,
                FreeShipping, TaxExempt, Active, Deleted);
            var customerRole = DBMapping(dbItem);

            if (CustomerManager.CacheEnabled)
            {
                NopCache.RemoveByPattern(CUSTOMERROLES_PATTERN_KEY);
            }

            return customerRole;
        }

        /// <summary>
        /// Adds a customer to role
        /// </summary>
        /// <param name="CustomerID">Customer identifier</param>
        /// <param name="CustomerRoleID">Customer role identifier</param>
        public static void AddCustomerToRole(int CustomerID, int CustomerRoleID)
        {
            DBProviderManager<DBCustomerProvider>.Provider.AddCustomerToRole(CustomerID, CustomerRoleID);
        }

        /// <summary>
        /// Removes a customer from role
        /// </summary>
        /// <param name="CustomerID">Customer identifier</param>
        /// <param name="CustomerRoleID">Customer role identifier</param>
        public static void RemoveCustomerFromRole(int CustomerID, int CustomerRoleID)
        {
            DBProviderManager<DBCustomerProvider>.Provider.RemoveCustomerFromRole(CustomerID, CustomerRoleID);
        }

        /// <summary>
        /// Adds a discount to a customer role
        /// </summary>
        /// <param name="CustomerRoleID">Customer role identifier</param>
        /// <param name="DiscountID">Discount identifier</param>
        public static void AddDiscountToCustomerRole(int CustomerRoleID, int DiscountID)
        {
            DBProviderManager<DBCustomerProvider>.Provider.AddDiscountToCustomerRole(CustomerRoleID, DiscountID);
            if (CustomerManager.CacheEnabled)
            {
                NopCache.RemoveByPattern(CUSTOMERROLES_PATTERN_KEY);
            }
        }

        /// <summary>
        /// Removes a discount from a customer role
        /// </summary>
        /// <param name="CustomerRoleID">Customer role identifier</param>
        /// <param name="DiscountID">Discount identifier</param>
        public static void RemoveDiscountFromCustomerRole(int CustomerRoleID, int DiscountID)
        {
            DBProviderManager<DBCustomerProvider>.Provider.RemoveDiscountFromCustomerRole(CustomerRoleID, DiscountID);
            if (CustomerManager.CacheEnabled)
            {
                NopCache.RemoveByPattern(CUSTOMERROLES_PATTERN_KEY);
            }
        }

        /// <summary>
        /// Gets a customer roles assigned to discount
        /// </summary>
        /// <param name="DiscountID">Discount identifier</param>
        /// <returns>Customer roles</returns>
        public static CustomerRoleCollection GetCustomerRolesByDiscountID(int DiscountID)
        {
            bool showHidden = NopContext.Current.IsAdmin;
            string key = string.Format(CUSTOMERROLES_BY_DISCOUNTID_KEY, DiscountID, showHidden);
            object obj2 = NopCache.Get(key);
            if (CustomerManager.CacheEnabled && (obj2 != null))
            {
                return (CustomerRoleCollection)obj2;
            }

            var dbCollection = DBProviderManager<DBCustomerProvider>.Provider.GetCustomerRolesByDiscountID(DiscountID, showHidden);
            var customerRoles = DBMapping(dbCollection);
            if (CustomerManager.CacheEnabled)
            {
                NopCache.Max(key, customerRoles);
            }
            return customerRoles;
        }

        /// <summary>
        /// Gets a customer session
        /// </summary>
        /// <param name="CustomerSessionGUID">Customer session GUID</param>
        /// <returns>Customer session</returns>
        public static CustomerSession GetCustomerSessionByGUID(Guid CustomerSessionGUID)
        {
            if (CustomerSessionGUID == Guid.Empty)
                return null;

            var dbItem = DBProviderManager<DBCustomerProvider>.Provider.GetCustomerSessionByGUID(CustomerSessionGUID);
            var customerSession = DBMapping(dbItem);
            return customerSession;
        }

        /// <summary>
        /// Gets a customer session by customer identifier
        /// </summary>
        /// <param name="CustomerID">Customer identifier</param>
        /// <returns>Customer session</returns>
        public static CustomerSession GetCustomerSessionByCustomerID(int CustomerID)
        {
            if (CustomerID == 0)
                return null;

            var dbItem = DBProviderManager<DBCustomerProvider>.Provider.GetCustomerSessionByCustomerID(CustomerID);
            var customerSession = DBMapping(dbItem);
            return customerSession;
        }

        /// <summary>
        /// Deletes a customer session
        /// </summary>
        /// <param name="CustomerSessionGUID">Customer session GUID</param>
        public static void DeleteCustomerSession(Guid CustomerSessionGUID)
        {
            DBProviderManager<DBCustomerProvider>.Provider.DeleteCustomerSession(CustomerSessionGUID);
        }

        /// <summary>
        /// Gets all customer sessions
        /// </summary>
        /// <returns>Customer session collection</returns>
        public static CustomerSessionCollection GetAllCustomerSessions()
        {
            var dbCollection = DBProviderManager<DBCustomerProvider>.Provider.GetAllCustomerSessions();
            var collection = DBMapping(dbCollection);
            return collection;
        }

        /// <summary>
        /// Deletes all expired customer sessions
        /// </summary>
        /// <param name="OlderThan">Older than date and time</param>
        public static void DeleteExpiredCustomerSessions(DateTime OlderThan)
        {
            OlderThan = DateTimeHelper.ConvertToUtcTime(OlderThan);

            DBProviderManager<DBCustomerProvider>.Provider.DeleteExpiredCustomerSessions(OlderThan);
        }

        /// <summary>
        /// Saves a customer session to the data storage if it exists or creates new one
        /// </summary>
        /// <param name="CustomerSessionGUID">Customer session GUID</param>
        /// <param name="CustomerID">Customer identifier</param>
        /// <param name="LastAccessed">The last accessed date and time</param>
        /// <param name="IsExpired">A value indicating whether the customer session is expired</param>
        /// <returns>Customer session</returns>
        public static CustomerSession SaveCustomerSession(Guid CustomerSessionGUID, int CustomerID, DateTime LastAccessed, bool IsExpired)
        {
            LastAccessed = DateTimeHelper.ConvertToUtcTime(LastAccessed);

            if (GetCustomerSessionByGUID(CustomerSessionGUID) == null)
                return InsertCustomerSession(CustomerSessionGUID, CustomerID, LastAccessed, IsExpired);
            else
                return UpdateCustomerSession(CustomerSessionGUID, CustomerID, LastAccessed, IsExpired);
        }

        /// <summary>
        /// Inserts a customer session
        /// </summary>
        /// <param name="CustomerSessionGUID">Customer session GUID</param>
        /// <param name="CustomerID">Customer identifier</param>
        /// <param name="LastAccessed">The last accessed date and time</param>
        /// <param name="IsExpired">A value indicating whether the customer session is expired</param>
        /// <returns>Customer session</returns>
        protected static CustomerSession InsertCustomerSession(Guid CustomerSessionGUID, int CustomerID, DateTime LastAccessed, bool IsExpired)
        {
            LastAccessed = DateTimeHelper.ConvertToUtcTime(LastAccessed);

            var dbItem = DBProviderManager<DBCustomerProvider>.Provider.InsertCustomerSession(CustomerSessionGUID, CustomerID, LastAccessed, IsExpired);
            var customerSession = DBMapping(dbItem);
            return customerSession;
        }

        /// <summary>
        /// Updates the customer session
        /// </summary>
        /// <param name="CustomerSessionGUID">Customer session GUID</param>
        /// <param name="CustomerID">Customer identifier</param>
        /// <param name="LastAccessed">The last accessed date and time</param>
        /// <param name="IsExpired">A value indicating whether the customer session is expired</param>
        /// <returns>Customer session</returns>
        protected static CustomerSession UpdateCustomerSession(Guid CustomerSessionGUID, int CustomerID, DateTime LastAccessed, bool IsExpired)
        {
            LastAccessed = DateTimeHelper.ConvertToUtcTime(LastAccessed);

            var dbItem = DBProviderManager<DBCustomerProvider>.Provider.UpdateCustomerSession(CustomerSessionGUID, CustomerID, LastAccessed, IsExpired);
            var customerSession = DBMapping(dbItem);
            return customerSession;
        }

        /// <summary>
        /// Formats customer name
        /// </summary>
        /// <param name="customer">Customer</param>
        /// <returns>Name</returns>
        public static string FormatUserName(Customer customer)
        {
            if (customer == null)
                return string.Empty;

            if(customer.IsGuest)
            {
                return LocalizationManager.GetLocaleResourceString("Customer.Guest");
            }

            string result = string.Empty;
            switch (CustomerManager.CustomerNameFormatting)
            {
                case CustomerNameFormatEnum.ShowEmails:
                    result = customer.Email;
                    break;
                case CustomerNameFormatEnum.ShowFullNames:
                    result = customer.FullName;
                    break;
                case CustomerNameFormatEnum.ShowUsernames:
                    result = customer.Username;
                    break;
                default:
                    break;
            }

            return result;
        }

        /// <summary>
        /// Gets a report of customers registered in the last days
        /// </summary>
        /// <param name="days">Customers registered in the last days</param>
        /// <returns>Int</returns>
        public static int GetRegisteredCustomersReport(int days)
        {
            DateTime date = DateTimeHelper.ConvertToUserTime(DateTime.Now).AddDays(-days);
            int count = DBProviderManager<DBCustomerProvider>.Provider.GetRegisteredCustomersReport(date);
            return count;
        }

        #endregion

        #region Property
        /// <summary>
        /// Gets a value indicating whether cache is enabled
        /// </summary>
        public static bool CacheEnabled
        {
            get
            {
                return SettingManager.GetSettingValueBoolean("Cache.CustomerManager.CacheEnabled");
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether anonymous checkout allowed
        /// </summary>
        public static bool AnonymousCheckoutAllowed
        {
            get
            {
                bool allowed = SettingManager.GetSettingValueBoolean("Checkout.AnonymousCheckoutAllowed", false);
                return allowed;
            }
            set
            {
                SettingManager.SetParam("Checkout.AnonymousCheckoutAllowed", value.ToString());
            }
        }
        
        /// <summary>
        /// Gets or sets a value indicating whether usernames are used instead of emails
        /// </summary>
        public static bool UsernamesEnabled
        {
            get
            {
                bool usernamesEnabled = SettingManager.GetSettingValueBoolean("Customer.UsernamesEnabled");
                return usernamesEnabled;
            }
            set
            {
                SettingManager.SetParam("Customer.UsernamesEnabled", value.ToString());
            }
        }
        
        /// <summary>
        /// Customer name formatting
        /// </summary>
        public static CustomerNameFormatEnum CustomerNameFormatting
        {
            get
            {
                int customerNameFormatting = SettingManager.GetSettingValueInteger("Customer.CustomerNameFormatting");
                return (CustomerNameFormatEnum)customerNameFormatting;
            }
            set
            {
                SettingManager.SetParam("Customer.CustomerNameFormatting", ((int)value).ToString());
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether customers are allowed to upload avatars.
        /// </summary>
        public static bool AllowCustomersToUploadAvatars
        {
            get
            {
                bool allowCustomersToUploadAvatars = SettingManager.GetSettingValueBoolean("Customer.CustomersAllowedToUploadAvatars");
                return allowCustomersToUploadAvatars;
            }
            set
            {
                SettingManager.SetParam("Customer.CustomersAllowedToUploadAvatars", value.ToString());
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether to display default user avatar.
        /// </summary>
        public static bool DefaultAvatarEnabled
        {
            get
            {
                bool defaultAvatarEnabled = SettingManager.GetSettingValueBoolean("Customer.DefaultAvatarEnabled");
                return defaultAvatarEnabled;
            }
            set
            {
                SettingManager.SetParam("Customer.DefaultAvatarEnabled", value.ToString());
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether customers location is shown
        /// </summary>
        public static bool ShowCustomersLocation
        {
            get
            {
                bool showCustomersLocation = SettingManager.GetSettingValueBoolean("Customer.ShowCustomersLocation");
                return showCustomersLocation;
            }
            set
            {
                SettingManager.SetParam("Customer.ShowCustomersLocation", value.ToString());
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether to show customers join date
        /// </summary>
        public static bool ShowCustomersJoinDate
        {
            get
            {
                bool showCustomersJoinDate = SettingManager.GetSettingValueBoolean("Customer.ShowCustomersJoinDate");
                return showCustomersJoinDate;
            }
            set
            {
                SettingManager.SetParam("Customer.ShowCustomersJoinDate", value.ToString());
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether customers are allowed to view profiles of other customers
        /// </summary>
        public static bool AllowViewingProfiles
        {
            get
            {
                bool allowViewingProfiles = SettingManager.GetSettingValueBoolean("Customer.AllowViewingProfiles");
                return allowViewingProfiles;
            }
            set
            {
                SettingManager.SetParam("Customer.AllowViewingProfiles", value.ToString());
            }
        }

        /// <summary>
        /// Tax display type
        /// </summary>
        public static CustomerRegistrationTypeEnum CustomerRegistrationType
        {
            get
            {
                int customerRegistrationType = SettingManager.GetSettingValueInteger("Common.CustomerRegistrationType", (int)CustomerRegistrationTypeEnum.Standard);
                return (CustomerRegistrationTypeEnum)customerRegistrationType;
            }
            set
            {
                SettingManager.SetParam("Common.CustomerRegistrationType", ((int)value).ToString());
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether to allow navigation only for registered users.
        /// </summary>
        public static bool AllowNavigationOnlyRegisteredCustomers
        {
            get
            {
                bool allowOnlyRegisteredCustomers = SettingManager.GetSettingValueBoolean("Common.AllowNavigationOnlyRegisteredCustomers");
                return allowOnlyRegisteredCustomers;
            }
            set
            {
                SettingManager.SetParam("Common.AllowNavigationOnlyRegisteredCustomers", value.ToString());
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether product reviews must be approved by administrator.
        /// </summary>
        public static bool ProductReviewsMustBeApproved
        {
            get
            {
                bool productReviewsMustBeApproved = SettingManager.GetSettingValueBoolean("Common.ProductReviewsMustBeApproved");
                return productReviewsMustBeApproved;
            }
            set
            {
                SettingManager.SetParam("Common.ProductReviewsMustBeApproved", value.ToString());
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether to allow anonymous users write product reviews.
        /// </summary>
        public static bool AllowAnonymousUsersToReviewProduct
        {
            get
            {
                bool allowAnonymousUsersToReviewProduct = SettingManager.GetSettingValueBoolean("Common.AllowAnonymousUsersToReviewProduct");
                return allowAnonymousUsersToReviewProduct;
            }
            set
            {
                SettingManager.SetParam("Common.AllowAnonymousUsersToReviewProduct", value.ToString());
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether to allow anonymous users to set product ratings.
        /// </summary>
        public static bool AllowAnonymousUsersToSetProductRatings
        {
            get
            {
                bool allowAnonymousUsersToSetProductRatings = SettingManager.GetSettingValueBoolean("Common.AllowAnonymousUsersToSetProductRatings");
                return allowAnonymousUsersToSetProductRatings;
            }
            set
            {
                SettingManager.SetParam("Common.AllowAnonymousUsersToSetProductRatings", value.ToString());
            }
        }
        #endregion
    }
}
