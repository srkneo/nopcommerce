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
using System.Collections.Specialized;
using System.Configuration;
using System.Configuration.Provider;
using System.Data;
using System.Data.Common;
using System.Globalization;
using System.Text;
using System.Web.Configuration;
using System.Web.Hosting;


namespace NopSolutions.NopCommerce.DataAccess.CustomerManagement
{
    /// <summary>
    /// Acts as a base class for deriving custom customer provider
    /// </summary>
    [DBProviderSectionName("nopDataProviders/CustomerProvider")]
    public abstract partial class DBCustomerProvider : BaseDBProvider
    {
        #region Methods

        /// <summary>
        /// Deletes an address by address identifier 
        /// </summary>
        /// <param name="addressId">Address identifier</param>
        public abstract void DeleteAddress(int addressId);

        /// <summary>
        /// Gets an address by address identifier
        /// </summary>
        /// <param name="addressId">Address identifier</param>
        /// <returns>Address</returns>
        public abstract DBAddress GetAddressById(int addressId);

        /// <summary>
        /// Gets a collection of addresses by customer identifier
        /// </summary>
        /// <param name="customerId">Customer identifier</param>
        /// <param name="getBillingAddresses">Gets or sets a value indicating whether the addresses are billing or shipping</param>
        /// <returns>A collection of addresses</returns>
        public abstract DBAddressCollection GetAddressesByCustomerId(int customerId, bool getBillingAddresses);

        /// <summary>
        /// Inserts an address
        /// </summary>
        /// <param name="customerId">The customer identifier</param>
        /// <param name="isBillingAddress">A value indicating whether the address is billing or shipping</param>
        /// <param name="firstName">The first name</param>
        /// <param name="lastName">The last name</param>
        /// <param name="phoneNumber">The phone number</param>
        /// <param name="email">The email</param>
        /// <param name="faxNumber">The fax number</param>
        /// <param name="company">The company</param>
        /// <param name="address1">The address 1</param>
        /// <param name="address2">The address 2</param>
        /// <param name="city">The city</param>
        /// <param name="stateProvinceId">The state/province identifier</param>
        /// <param name="zipPostalCode">The zip/postal code</param>
        /// <param name="countryId">The country identifier</param>
        /// <param name="createdOn">The date and time of instance creation</param>
        /// <param name="updatedOn">The date and time of instance update</param>
        /// <returns>An address</returns>
        public abstract DBAddress InsertAddress(int customerId, bool isBillingAddress, 
            string firstName, string lastName, string phoneNumber, 
            string email, string faxNumber, string company, string address1,
            string address2, string city, int stateProvinceId, string zipPostalCode,
            int countryId, DateTime createdOn, DateTime updatedOn);

        /// <summary>
        /// Updates the address
        /// </summary>
        /// <param name="addressId">The address identifier</param>
        /// <param name="customerId">The customer identifier</param>
        /// <param name="isBillingAddress">A value indicating whether the address is billing or shipping</param>
        /// <param name="firstName">The first name</param>
        /// <param name="lastName">The last name</param>
        /// <param name="phoneNumber">The phone number</param>
        /// <param name="email">The email</param>
        /// <param name="faxNumber">The fax number</param>
        /// <param name="company">The company</param>
        /// <param name="address1">The address 1</param>
        /// <param name="address2">The address 2</param>
        /// <param name="city">The city</param>
        /// <param name="stateProvinceId">The state/province identifier</param>
        /// <param name="zipPostalCode">The zip/postal code</param>
        /// <param name="countryId">The country identifier</param>
        /// <param name="createdOn">The date and time of instance creation</param>
        /// <param name="updatedOn">The date and time of instance update</param>
        /// <returns>An address</returns>
        public abstract DBAddress UpdateAddress(int addressId, int customerId,
            bool isBillingAddress, string firstName, string lastName, string phoneNumber,
            string email, string faxNumber, string company, string address1,
            string address2, string city, int stateProvinceId, string zipPostalCode,
            int countryId, DateTime createdOn, DateTime updatedOn);

        /// <summary>
        /// Gets all customers
        /// </summary>
        /// <param name="registrationFrom">Customer registration from; null to load all customers</param>
        /// <param name="registrationTo">Customer registration to; null to load all customers</param>
        /// <param name="email">Customer Email</param>
        /// <param name="username">Customer username</param>
        /// <param name="dontLoadGuestCustomers">A value indicating whether to don't load guest customers</param>
        /// <param name="pageSize">Page size</param>
        /// <param name="pageIndex">Page index</param>
        /// <param name="totalRecords">Total records</param>
        /// <returns>Customer collection</returns>
        public abstract DBCustomerCollection GetAllCustomers(DateTime? registrationFrom, 
            DateTime? registrationTo, string email, string username,
            bool dontLoadGuestCustomers, int pageSize, int pageIndex, out int totalRecords);

        /// <summary>
        /// Gets all customers by affiliate identifier
        /// </summary>
        /// <param name="affiliateId">Affiliate identifier</param>
        /// <returns>Customer collection</returns>
        public abstract DBCustomerCollection GetAffiliatedCustomers(int affiliateId);

        /// <summary>
        /// Gets all customers by customer role id
        /// </summary>
        /// <param name="customerRoleId">Customer role identifier</param>
        /// <param name="showHidden">A value indicating whether to show hidden records</param>
        /// <returns>Customer collection</returns>
        public abstract DBCustomerCollection GetCustomersByCustomerRoleId(int customerRoleId, bool showHidden);

        /// <summary>
        /// Gets a customer by email
        /// </summary>
        /// <param name="email">Customer Email</param>
        /// <returns>A customer</returns>
        public abstract DBCustomer GetCustomerByEmail(string email);

        /// <summary>
        /// Gets a customer by username
        /// </summary>
        /// <param name="username">Customer username</param>
        /// <returns>A customer</returns>
        public abstract DBCustomer GetCustomerByUsername(string username);

        /// <summary>
        /// Gets a customer
        /// </summary>
        /// <param name="customerId">Customer identifier</param>
        /// <returns>A customer</returns>
        public abstract DBCustomer GetCustomerById(int customerId);

        /// <summary>
        /// Gets a customer by GUID
        /// </summary>
        /// <param name="customerGuid">Customer GUID</param>
        /// <returns>A customer</returns>
        public abstract DBCustomer GetCustomerByGuid(Guid customerGuid);

        /// <summary>
        /// Adds a customer
        /// </summary>
        /// <param name="customerGuid">The customer identifier</param>
        /// <param name="email">The email</param>
        /// <param name="username">The username</param>
        /// <param name="passwordHash">The password hash</param>
        /// <param name="saltKey">The salt key</param>
        /// <param name="affiliateId">The affiliate identifier</param>
        /// <param name="billingAddressId">The billing address identifier</param>
        /// <param name="shippingAddressId">The shipping address identifier</param>
        /// <param name="lastPaymentMethodId">The last payment method identifier</param>
        /// <param name="lastAppliedCouponCode">The last applied coupon code</param>
        /// <param name="giftCardCouponCodes">The applied gift card coupon code</param>
        /// <param name="checkoutAttributes">The selected checkout attributes</param>
        /// <param name="languageId">The language identifier</param>
        /// <param name="currencyId">The currency identifier</param>
        /// <param name="taxDisplayTypeId">The tax display type identifier</param>
        /// <param name="isTaxExempt">A value indicating whether the customer is tax exempt</param>
        /// <param name="isAdmin">A value indicating whether the customer is administrator</param>
        /// <param name="isGuest">A value indicating whether the customer is guest</param>
        /// <param name="isForumModerator">A value indicating whether the customer is forum moderator</param>
        /// <param name="totalForumPosts">A forum post count</param>
        /// <param name="signature">Signature</param>
        /// <param name="adminComment">Admin comment</param>
        /// <param name="active">A value indicating whether the customer is active</param>
        /// <param name="deleted">A value indicating whether the customer has been deleted</param>
        /// <param name="registrationDate">The date and time of customer registration</param>
        /// <param name="timeZoneId">The time zone identifier</param>
        /// <param name="avatarId">The avatar identifier</param>
        /// <returns>A customer</returns>
        public abstract DBCustomer AddCustomer(Guid customerGuid, string email,
            string username, string passwordHash, string saltKey,
            int affiliateId, int billingAddressId,
            int shippingAddressId, int lastPaymentMethodId,
            string lastAppliedCouponCode, string giftCardCouponCodes, 
            string checkoutAttributes, int languageId, 
            int currencyId, int taxDisplayTypeId,
            bool isTaxExempt, bool isAdmin, bool isGuest, bool isForumModerator,
            int totalForumPosts, string signature, string adminComment, bool active, 
            bool deleted, DateTime registrationDate, string timeZoneId, int avatarId);

        /// <summary>
        /// Updates the customer
        /// </summary>
        /// <param name="customerId">The customer identifier</param>
        /// <param name="customerGuid">The customer identifier</param>
        /// <param name="email">The email</param>
        /// <param name="username">The username</param>
        /// <param name="passwordHash">The password hash</param>
        /// <param name="saltKey">The salt key</param>
        /// <param name="affiliateId">The affiliate identifier</param>
        /// <param name="billingAddressId">The billing address identifier</param>
        /// <param name="shippingAddressId">The shipping address identifier</param>
        /// <param name="lastPaymentMethodId">The last payment method identifier</param>
        /// <param name="lastAppliedCouponCode">The last applied coupon code</param>
        /// <param name="giftCardCouponCodes">The applied gift card coupon code</param>
        /// <param name="checkoutAttributes">The selected checkout attributes</param>
        /// <param name="languageId">The language identifier</param>
        /// <param name="currencyId">The currency identifier</param>
        /// <param name="taxDisplayTypeId">The tax display type identifier</param>
        /// <param name="isTaxExempt">A value indicating whether the customer is tax exempt</param>
        /// <param name="isAdmin">A value indicating whether the customer is administrator</param>
        /// <param name="isGuest">A value indicating whether the customer is guest</param>
        /// <param name="isForumModerator">A value indicating whether the customer is forum moderator</param>
        /// <param name="totalForumPosts">A forum post count</param>
        /// <param name="signature">Signature</param>
        /// <param name="adminComment">Admin comment</param>
        /// <param name="active">A value indicating whether the customer is active</param>
        /// <param name="deleted">A value indicating whether the customer has been deleted</param>
        /// <param name="registrationDate">The date and time of customer registration</param>
        /// <param name="timeZoneId">The time zone identifier</param>
        /// <param name="avatarId">The avatar identifier</param>
        /// <returns>A customer</returns>
        public abstract DBCustomer UpdateCustomer(int customerId,
            Guid customerGuid, string email,
            string username, string passwordHash, string saltKey,
            int affiliateId, int billingAddressId,
            int shippingAddressId, int lastPaymentMethodId,
            string lastAppliedCouponCode, string giftCardCouponCodes,
            string checkoutAttributes, int languageId,
            int currencyId, int taxDisplayTypeId,
            bool isTaxExempt, bool isAdmin, bool isGuest, bool isForumModerator,
            int totalForumPosts, string signature, string adminComment, bool active,
            bool deleted, DateTime registrationDate, string timeZoneId, int avatarId);

        /// <summary>
        /// Get best customers
        /// </summary>
        /// <param name="startTime">Order start time; null to load all</param>
        /// <param name="endTime">Order end time; null to load all</param>
        /// <param name="orderStatusId">Order status identifier; null to load all records</param>
        /// <param name="paymentStatusId">Order payment status identifier; null to load all records</param>
        /// <param name="shippingStatusId">Order shipping status identifier; null to load all records</param>
        /// <param name="orderBy">1 - order by order total, 2 - order by number of orders</param>
        /// <returns>Report</returns>
        public abstract IDataReader GetBestCustomersReport(DateTime? startTime,
            DateTime? endTime, int? orderStatusId, int? paymentStatusId, 
            int? shippingStatusId, int orderBy);

        /// <summary>
        /// Deletes a customer attribute
        /// </summary>
        /// <param name="customerAttributeId">Customer attribute identifier</param>
        public abstract void DeleteCustomerAttribute(int customerAttributeId);

        /// <summary>
        /// Gets a customer attribute
        /// </summary>
        /// <param name="customerAttributeId">Customer attribute identifier</param>
        /// <returns>A customer attribute</returns>
        public abstract DBCustomerAttribute GetCustomerAttributeById(int customerAttributeId);

        /// <summary>
        /// Gets a collection of customer attributes by customer identifier
        /// </summary>
        /// <param name="customerId">Customer identifier</param>
        /// <returns>Customer attributes</returns>
        public abstract DBCustomerAttributeCollection GetCustomerAttributesByCustomerId(int customerId);

        /// <summary>
        /// Inserts a customer attribute
        /// </summary>
        /// <param name="customerId">Customer identifier</param>
        /// <param name="key">An attribute key</param>
        /// <param name="value">An attribute value</param>
        /// <returns>A customer attribute</returns>
        public abstract DBCustomerAttribute InsertCustomerAttribute(int customerId,
            string key, string value);

        /// <summary>
        /// Updates the customer attribute
        /// </summary>
        /// <param name="customerAttributeId">Customer attribute identifier</param>
        /// <param name="customerId">Customer identifier</param>
        /// <param name="key">An attribute key</param>
        /// <param name="value">An attribute value</param>
        /// <returns>A customer attribute</returns>
        public abstract DBCustomerAttribute UpdateCustomerAttribute(int customerAttributeId,
            int customerId, string key, string value);

        /// <summary>
        /// Gets customer roles by customer identifier
        /// </summary>
        /// <param name="customerId">Customer identifier</param>
        /// <param name="showHidden">A value indicating whether to show hidden records</param>
        /// <returns>Customer role collection</returns>
        public abstract DBCustomerRoleCollection GetCustomerRolesByCustomerId(int customerId, bool showHidden);

        /// <summary>
        /// Gets all customer roles
        /// </summary>
        /// <param name="showHidden">A value indicating whether to show hidden records</param>
        /// <returns>Customer role collection</returns>
        public abstract DBCustomerRoleCollection GetAllCustomerRoles(bool showHidden);

        /// <summary>
        /// Gets a customer role
        /// </summary>
        /// <param name="customerRoleId">Customer role identifier</param>
        /// <returns>Customer role</returns>
        public abstract DBCustomerRole GetCustomerRoleById(int customerRoleId);

        /// <summary>
        /// Inserts a customer role
        /// </summary>
        /// <param name="name">The customer role name</param>
        /// <param name="freeShipping">A value indicating whether the customer role is marked as free shiping</param>
        /// <param name="taxExempt">A value indicating whether the customer role is marked as tax exempt</param>
        /// <param name="active">A value indicating whether the customer role is active</param>
        /// <param name="deleted">A value indicating whether the customer role has been deleted</param>
        /// <returns>Customer role</returns>
        public abstract DBCustomerRole InsertCustomerRole(string name, 
            bool freeShipping, bool taxExempt, bool active, bool deleted);

        /// <summary>
        /// Updates the customer role
        /// </summary>
        /// <param name="customerRoleId">The customer role identifier</param>
        /// <param name="name">The customer role name</param>
        /// <param name="freeShipping">A value indicating whether the customer role is marked as free shiping</param>
        /// <param name="taxExempt">A value indicating whether the customer role is marked as tax exempt</param>
        /// <param name="active">A value indicating whether the customer role is active</param>
        /// <param name="deleted">A value indicating whether the customer role has been deleted</param>
        /// <returns>Customer role</returns>
        public abstract DBCustomerRole UpdateCustomerRole(int customerRoleId, string name,
            bool freeShipping, bool taxExempt, bool active, bool deleted);

        /// <summary>
        /// Adds a customer to role
        /// </summary>
        /// <param name="customerId">Customer identifier</param>
        /// <param name="customerRoleId">Customer role identifier</param>
        public abstract void AddCustomerToRole(int customerId, int customerRoleId);

        /// <summary>
        /// Removes a customer from role
        /// </summary>
        /// <param name="customerId">Customer identifier</param>
        /// <param name="customerRoleId">Customer role identifier</param>
        public abstract void RemoveCustomerFromRole(int customerId, int customerRoleId);

        /// <summary>
        /// Adds a discount to a customer role
        /// </summary>
        /// <param name="customerRoleId">Customer role identifier</param>
        /// <param name="discountId">Discount identifier</param>
        public abstract void AddDiscountToCustomerRole(int customerRoleId, int discountId);

        /// <summary>
        /// Removes a discount from a customer role
        /// </summary>
        /// <param name="customerRoleId">Customer role identifier</param>
        /// <param name="discountId">Discount identifier</param>
        public abstract void RemoveDiscountFromCustomerRole(int customerRoleId, int discountId);

        /// <summary>
        /// Gets a customer roles assigned to discount
        /// </summary>
        /// <param name="discountId">Discount identifier</param>
        /// <param name="showHidden">A value indicating whether to show hidden records</param>
        /// <returns>Customer roles</returns>
        public abstract DBCustomerRoleCollection GetCustomerRolesByDiscountId(int discountId, bool showHidden);

        /// <summary>
        /// Gets a customer session
        /// </summary>
        /// <param name="customerSessionGuid">Customer session GUID</param>
        /// <returns>Customer session</returns>
        public abstract DBCustomerSession GetCustomerSessionByGuid(Guid customerSessionGuid);

        /// <summary>
        /// Gets a customer session by customer identifier
        /// </summary>
        /// <param name="customerId">Customer identifier</param>
        /// <returns>Customer session</returns>
        public abstract DBCustomerSession GetCustomerSessionByCustomerId(int customerId);

        /// <summary>
        /// Deletes a customer session
        /// </summary>
        /// <param name="customerSessionGuid">Customer session GUID</param>
        public abstract void DeleteCustomerSession(Guid customerSessionGuid);

        /// <summary>
        /// Deletes all expired customer sessions
        /// </summary>
        /// <param name="olderThan">Older than date and time</param>
        public abstract void DeleteExpiredCustomerSessions(DateTime olderThan);

        /// <summary>
        /// Gets all customer sessions
        /// </summary>
        /// <returns>Customer session collection</returns>
        public abstract DBCustomerSessionCollection GetAllCustomerSessions();

        /// <summary>
        /// Inserts a customer session
        /// </summary>
        /// <param name="customerSessionGuid">Customer session GUID</param>
        /// <param name="customerId">Customer identifier</param>
        /// <param name="lastAccessed">The last accessed date and time</param>
        /// <param name="isExpired">A value indicating whether the customer session is expired</param>
        /// <returns>Customer session</returns>
        public abstract DBCustomerSession InsertCustomerSession(Guid customerSessionGuid, 
            int customerId, DateTime lastAccessed, bool isExpired);

        /// <summary>
        /// Updates the customer session
        /// </summary>
        /// <param name="customerSessionGuid">Customer session GUID</param>
        /// <param name="customerId">Customer identifier</param>
        /// <param name="lastAccessed">The last accessed date and time</param>
        /// <param name="isExpired">A value indicating whether the customer session is expired</param>
        /// <returns>Customer session</returns>
        public abstract DBCustomerSession UpdateCustomerSession(Guid customerSessionGuid, 
            int customerId, DateTime lastAccessed, bool isExpired);

        /// <summary>
        /// Gets a report of customers registered from "dateTime" until today
        /// </summary>
        /// <param name="dateFrom">Customer registration date from</param>
        /// <returns>Customer count</returns>
        public abstract int GetRegisteredCustomersReport(DateTime dateFrom);
        #endregion
    }
}
