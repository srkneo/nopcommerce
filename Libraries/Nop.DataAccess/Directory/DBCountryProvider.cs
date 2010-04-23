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

namespace NopSolutions.NopCommerce.DataAccess.Directory
{
    /// <summary>
    /// Acts as a base class for deriving custom country provider
    /// </summary>
    [DBProviderSectionName("nopDataProviders/CountryProvider")]
    public abstract partial class DBCountryProvider : BaseDBProvider
    {
        #region Methods
        /// <summary>
        /// Deletes a country
        /// </summary>
        /// <param name="countryId">Country identifier</param>
        public abstract void DeleteCountry(int countryId);

        /// <summary>
        /// Gets all countries
        /// </summary>
        /// <returns>Country collection</returns>
        public abstract DBCountryCollection GetAllCountries(bool showHidden);

        /// <summary>
        /// Gets all countries that allow registration
        /// </summary>
        /// <returns>Country collection</returns>
        public abstract DBCountryCollection GetAllCountriesForRegistration(bool showHidden);

        /// <summary>
        /// Gets all countries that allow billing
        /// </summary>
        /// <returns>Country collection</returns>
        public abstract DBCountryCollection GetAllCountriesForBilling(bool showHidden);

        /// <summary>
        /// Gets all countries that allow shipping
        /// </summary>
        /// <returns>Country collection</returns>
        public abstract DBCountryCollection GetAllCountriesForShipping(bool showHidden);

        /// <summary>
        /// Gets a country 
        /// </summary>
        /// <param name="countryId">Country identifier</param>
        /// <returns>Country</returns>
        public abstract DBCountry GetCountryById(int countryId);

        /// <summary>
        /// Gets a country by two letter ISO code
        /// </summary>
        /// <param name="twoLetterIsoCode">Country two letter ISO code</param>
        /// <returns>Country</returns>
        public abstract DBCountry GetCountryByTwoLetterIsoCode(string twoLetterIsoCode);

        /// <summary>
        /// Gets a country by three letter ISO code
        /// </summary>
        /// <param name="threeLetterIsoCode">Country three letter ISO code</param>
        /// <returns>Country</returns>
        public abstract DBCountry GetCountryByThreeLetterIsoCode(string threeLetterIsoCode);

        /// <summary>
        /// Inserts a country
        /// </summary>
        /// <param name="name">The name</param>
        /// <param name="allowsRegistration">A value indicating whether registration is allowed to this country</param>
        /// <param name="allowsBilling">A value indicating whether billing is allowed to this country</param>
        /// <param name="allowsShipping">A value indicating whether shipping is allowed to this country</param>
        /// <param name="twoLetterIsoCode">The two letter ISO code</param>
        /// <param name="threeLetterIsoCode">The three letter ISO code</param>
        /// <param name="numericIsoCode">The numeric ISO code</param>
        /// <param name="published">A value indicating whether the entity is published</param>
        /// <param name="displayOrder">The display order</param>
        /// <returns>Country</returns>
        public abstract DBCountry InsertCountry(string name,
            bool allowsRegistration, bool allowsBilling, bool allowsShipping,
            string twoLetterIsoCode, string threeLetterIsoCode, int numericIsoCode,
            bool published, int displayOrder);

        /// <summary>
        /// Updates the country
        /// </summary>
        /// <param name="countryId">The country identifier</param>
        /// <param name="name">The name</param>
        /// <param name="allowsRegistration">A value indicating whether registration is allowed to this country</param>
        /// <param name="allowsBilling">A value indicating whether billing is allowed to this country</param>
        /// <param name="allowsShipping">A value indicating whether shipping is allowed to this country</param>
        /// <param name="twoLetterIsoCode">The two letter ISO code</param>
        /// <param name="threeLetterIsoCode">The three letter ISO code</param>
        /// <param name="numericIsoCode">The numeric ISO code</param>
        /// <param name="published">A value indicating whether the entity is published</param>
        /// <param name="displayOrder">The display order</param>
        /// <returns>Country</returns>
        public abstract DBCountry UpdateCountry(int countryId, string name,
            bool allowsRegistration, bool allowsBilling, bool allowsShipping,
            string twoLetterIsoCode, string threeLetterIsoCode, int numericIsoCode,
            bool published, int displayOrder);
        #endregion
    }
}
