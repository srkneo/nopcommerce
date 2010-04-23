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
using System.Text;
using NopSolutions.NopCommerce.BusinessLogic.Caching;
using NopSolutions.NopCommerce.DataAccess;
using NopSolutions.NopCommerce.DataAccess.Promo.Affiliates;

namespace NopSolutions.NopCommerce.BusinessLogic.Promo.Affiliates
{
    /// <summary>
    /// Affiliate manager
    /// </summary>
    public partial class AffiliateManager
    {
        #region Utilities
        private static AffiliateCollection DBMapping(DBAffiliateCollection dbCollection)
        {
            if (dbCollection == null)
                return null;

            var collection = new AffiliateCollection();
            foreach (var dbItem in dbCollection)
            {
                var item = DBMapping(dbItem);
                collection.Add(item);
            }

            return collection;
        }

        private static Affiliate DBMapping(DBAffiliate dbItem)
        {
            if (dbItem == null)
                return null;

            var item = new Affiliate();
            item.AffiliateId = dbItem.AffiliateId;
            item.FirstName = dbItem.FirstName;
            item.LastName = dbItem.LastName;
            item.MiddleName = dbItem.MiddleName;
            item.PhoneNumber = dbItem.PhoneNumber;
            item.Email = dbItem.Email;
            item.FaxNumber = dbItem.FaxNumber;
            item.Company = dbItem.Company;
            item.Address1 = dbItem.Address1;
            item.Address2 = dbItem.Address2;
            item.City = dbItem.City;
            item.StateProvince = dbItem.StateProvince;
            item.ZipPostalCode = dbItem.ZipPostalCode;
            item.CountryId = dbItem.CountryId;
            item.Deleted = dbItem.Deleted;
            item.Active = dbItem.Active;
            return item;
        }
        #endregion

        #region Methods
        /// <summary>
        /// Gets an affiliate by affiliate identifier
        /// </summary>
        /// <param name="affiliateId">Affiliate identifier</param>
        /// <returns>Affiliate</returns>
        public static Affiliate GetAffiliateById(int affiliateId)
        {
            if (affiliateId == 0)
                return null;

            var dbItem = DBProviderManager<DBAffiliateProvider>.Provider.GetAffiliateById(affiliateId);
            var affiliate = DBMapping(dbItem);
            return affiliate;
        }

        /// <summary>
        /// Marks affiliate as deleted 
        /// </summary>
        /// <param name="affiliateId">Affiliate identifier</param>
        public static void MarkAffiliateAsDeleted(int affiliateId)
        {
            var affiliate = GetAffiliateById(affiliateId);
            if (affiliate != null)
            {
                affiliate = UpdateAffiliate(affiliate.AffiliateId, affiliate.FirstName, affiliate.LastName, affiliate.MiddleName, affiliate.PhoneNumber,
                      affiliate.Email, affiliate.FaxNumber, affiliate.Company, affiliate.Address1, affiliate.Address2, affiliate.City,
                      affiliate.StateProvince, affiliate.ZipPostalCode, affiliate.CountryId, true, affiliate.Active);
            }
        }

        /// <summary>
        /// Gets all affiliates
        /// </summary>
        /// <returns>Affiliate collection</returns>
        public static AffiliateCollection GetAllAffiliates()
        {
            var dbCollection = DBProviderManager<DBAffiliateProvider>.Provider.GetAllAffiliates();
            var affiliates = DBMapping(dbCollection);
            return affiliates;
        }

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
        public static Affiliate InsertAffiliate(string firstName,
            string lastName, string middleName, string phoneNumber,
            string email, string faxNumber, string company, string address1,
            string address2, string city, string stateProvince, string zipPostalCode,
            int countryId, bool deleted, bool active)
        {
            var dbItem = DBProviderManager<DBAffiliateProvider>.Provider.InsertAffiliate(firstName, 
                lastName, middleName, phoneNumber, email, faxNumber, company, address1,
                address2, city, stateProvince, zipPostalCode, countryId, deleted, active);
            var affiliate = DBMapping(dbItem);
            return affiliate;
        }

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
        public static Affiliate UpdateAffiliate(int affiliateId, string firstName,
            string lastName, string middleName, string phoneNumber,
            string email, string faxNumber, string company, string address1,
            string address2, string city, string stateProvince, string zipPostalCode,
            int countryId, bool deleted, bool active)
        {
            var dbItem = DBProviderManager<DBAffiliateProvider>.Provider.UpdateAffiliate(affiliateId, 
                firstName, lastName, middleName, phoneNumber, email, faxNumber, company,
                address1, address2, city, stateProvince, zipPostalCode, 
                countryId, deleted, active);
            var affiliate = DBMapping(dbItem);
            return affiliate;
        }
        #endregion
    }
}
