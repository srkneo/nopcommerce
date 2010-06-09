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

namespace NopSolutions.NopCommerce.DataAccess.Payment
{
    /// <summary>
    /// Acts as a base class for deriving custom payment method provider
    /// </summary>
    [DBProviderSectionName("nopDataProviders/PaymentMethodProvider")]
    public abstract partial class DBPaymentMethodProvider : BaseDBProvider
    {
        #region Methods

        /// <summary>
        /// Gets all payment methods
        /// </summary>
        /// <param name="showHidden">A value indicating whether to show hidden records</param>
        /// <param name="filterByCountryId">The country indentifier</param>
        /// <returns>Payment method collection</returns>
        public abstract DBPaymentMethodCollection GetAllPaymentMethods(bool showHidden, 
            int? filterByCountryId);
        
        /// <summary>
        /// Inserts payment method country mapping
        /// </summary>
        /// <param name="paymentMethodId">The payment method identifier</param>
        /// <param name="countryId">The country identifier</param>
        public abstract void InsertPaymentMethodCountryMapping(int paymentMethodId, int countryId);

        /// <summary>
        /// Checking whether the payment method country mapping exists
        /// </summary>
        /// <param name="paymentMethodId">The payment method identifier</param>
        /// <param name="countryId">The country identifier</param>
        /// <returns>True if mapping exist, otherwise false</returns>
        public abstract bool DoesPaymentMethodCountryMappingExist(int paymentMethodId, int countryId);

        /// <summary>
        /// Deletes payment method country mapping
        /// </summary>
        /// <param name="paymentMethodId">The payment method identifier</param>
        /// <param name="countryId">The country identifier</param>
        public abstract void DeletePaymentMethodCountryMapping(int paymentMethodId, int countryId);
        
        #endregion
    }
}
