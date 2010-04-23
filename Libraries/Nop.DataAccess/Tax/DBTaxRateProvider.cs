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

namespace NopSolutions.NopCommerce.DataAccess.Tax
{
    /// <summary>
    /// Acts as a base class for deriving custom tax rate provider
    /// </summary>
    [DBProviderSectionName("nopDataProviders/TaxRateProvider")]
    public abstract partial class DBTaxRateProvider : BaseDBProvider
    {
        #region Methods

        /// <summary>
        /// Deletes a tax rate
        /// </summary>
        /// <param name="taxRateId">Tax rate identifier</param>
        public abstract void DeleteTaxRate(int taxRateId);

        /// <summary>
        /// Gets a tax rate
        /// </summary>
        /// <param name="taxRateId">Tax rate identifier</param>
        /// <returns>Tax rate</returns>
        public abstract DBTaxRate GetTaxRateById(int taxRateId);

        /// <summary>
        /// Gets all tax rates
        /// </summary>
        /// <returns>Tax rate collection</returns>
        public abstract DBTaxRateCollection GetAllTaxRates();

        /// <summary>
        /// Inserts a tax rate
        /// </summary>
        /// <param name="taxCategoryId">The tax category identifier</param>
        /// <param name="countryId">The country identifier</param>
        /// <param name="stateProvinceId">The state/province identifier</param>
        /// <param name="zip">The zip</param>
        /// <param name="percentage">The percentage</param>
        /// <returns>Tax rate</returns>
        public abstract DBTaxRate InsertTaxRate(int taxCategoryId, int countryId,
            int stateProvinceId, string zip, decimal percentage);

        /// <summary>
        /// Updates the tax rate
        /// </summary>
        /// <param name="taxRateId">The tax rate identifier</param>
        /// <param name="taxCategoryId">The tax category identifier</param>
        /// <param name="countryId">The country identifier</param>
        /// <param name="stateProvinceId">The state/province identifier</param>
        /// <param name="zip">The zip</param>
        /// <param name="percentage">The percentage</param>
        /// <returns>Tax rate</returns>
        public abstract DBTaxRate UpdateTaxRate(int taxRateId, 
            int taxCategoryId, int countryId, int stateProvinceId, 
            string zip, decimal percentage);

        #endregion
    }
}
