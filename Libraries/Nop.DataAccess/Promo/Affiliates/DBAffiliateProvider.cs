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

namespace NopSolutions.NopCommerce.DataAccess.Promo.Affiliates
{
    /// <summary>
    /// Acts as a base class for deriving custom affiliate provider
    /// </summary>
    [DBProviderSectionName("nopDataProviders/AffiliateProvider")]
    public abstract partial class DBAffiliateProvider : BaseDBProvider
    {
        #region Methods

        /// <summary>
        /// Gets an affiliate by affiliate identifier
        /// </summary>
        /// <param name="affiliateId">Affiliate identifier</param>
        /// <returns>Affiliate</returns>
        public abstract DBAffiliate GetAffiliateById(int affiliateId);

        /// <summary>
        /// Gets all affiliates
        /// </summary>
        /// <returns>Affiliate collection</returns>
        public abstract DBAffiliateCollection GetAllAffiliates();

        /// <summary>
        /// Inserts an affiliate
        /// </summary>
        /// <param name="firstName">The first name</param>
        /// <param name="lastName">The last name</param>
        /// <param name="middleName">The middle name</param>
        /// <param name="phoneNumber">The phone number</param>
        /// <param name="email">The email</param>
        /// <param name="faxNumber">The fax number</param>
        /// <param name="company">The company</param>
        /// <param name="address1">The address 1</param>
        /// <param name="address2">The address 2</param>
        /// <param name="city">The city</param>
        /// <param name="stateProvince">The state/province</param>
        /// <param name="zipPostalCode">The zip/postal code</param>
        /// <param name="countryId">The country identifier</param>
        /// <param name="deleted">A value indicating whether the entity has been deleted</param>
        /// <param name="active">A value indicating whether the entity is active</param>
        /// <returns>An affiliate</returns>
        public abstract DBAffiliate InsertAffiliate(string firstName, 
            string lastName, string middleName, string phoneNumber, 
            string email, string faxNumber, string company, string address1,
            string address2, string city, string stateProvince, string zipPostalCode,
            int countryId, bool deleted, bool active);

        /// <summary>
        /// Updates the affiliate
        /// </summary>
        /// <param name="affiliateId">The affiliate identifier</param>
        /// <param name="firstName">The first name</param>
        /// <param name="lastName">The last name</param>
        /// <param name="middleName">The middle name</param>
        /// <param name="phoneNumber">The phone number</param>
        /// <param name="email">The email</param>
        /// <param name="faxNumber">The fax number</param>
        /// <param name="company">The company</param>
        /// <param name="address1">The address 1</param>
        /// <param name="address2">The address 2</param>
        /// <param name="city">The city</param>
        /// <param name="stateProvince">The state/province</param>
        /// <param name="zipPostalCode">The zip/postal code</param>
        /// <param name="countryId">The country identifier</param>
        /// <param name="deleted">A value indicating whether the entity has been deleted</param>
        /// <param name="active">A value indicating whether the entity is active</param>
        /// <returns>An affiliate</returns>
        public abstract DBAffiliate UpdateAffiliate(int affiliateId, string firstName,
            string lastName, string middleName, string phoneNumber,
            string email, string faxNumber, string company, string address1,
            string address2, string city, string stateProvince, string zipPostalCode,
            int countryId, bool deleted, bool active);

        #endregion
    }
}
