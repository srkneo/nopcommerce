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
        /// Deletes a payment method
        /// </summary>
        /// <param name="paymentMethodId">Payment method identifier</param>
        public abstract void DeletePaymentMethod(int paymentMethodId);

        /// <summary>
        /// Gets a payment method
        /// </summary>
        /// <param name="paymentMethodId">Payment method identifier</param>
        /// <returns>Payment method</returns>
        public abstract DBPaymentMethod GetPaymentMethodById(int paymentMethodId);

        /// <summary>
        /// Gets a payment method
        /// </summary>
        /// <param name="systemKeyword">Payment method system keyword</param>
        /// <returns>Payment method</returns>
        public abstract DBPaymentMethod GetPaymentMethodBySystemKeyword(string systemKeyword);

        /// <summary>
        /// Gets all payment methods
        /// </summary>
        /// <param name="showHidden">A value indicating whether to show hidden records</param>
        /// <param name="filterByCountryId">The country indentifier</param>
        /// <returns>Payment method collection</returns>
        public abstract DBPaymentMethodCollection GetAllPaymentMethods(bool showHidden, 
            int? filterByCountryId);

        /// <summary>
        /// Inserts a payment method
        /// </summary>
        /// <param name="name">The name</param>
        /// <param name="visibleName">The visible name</param>
        /// <param name="description">The description</param>
        /// <param name="configureTemplatePath">The configure template path</param>
        /// <param name="userTemplatePath">The user template path</param>
        /// <param name="className">The class name</param>
        /// <param name="systemKeyword">The system keyword</param>
        /// <param name="isActive">A value indicating whether the payment method is active</param>
        /// <param name="displayOrder">The display order</param>
        /// <returns>Payment method</returns>
        public abstract DBPaymentMethod InsertPaymentMethod(string name, 
            string visibleName, string description, string configureTemplatePath, 
            string userTemplatePath, string className, string systemKeyword,
            bool isActive, int displayOrder);

        /// <summary>
        /// Updates the payment method
        /// </summary>
        /// <param name="paymentMethodId">The payment method identifer</param>
        /// <param name="name">The name</param>
        /// <param name="visibleName">The visible name</param>
        /// <param name="description">The description</param>
        /// <param name="configureTemplatePath">The configure template path</param>
        /// <param name="userTemplatePath">The user template path</param>
        /// <param name="className">The class name</param>
        /// <param name="systemKeyword">The system keyword</param>
        /// <param name="isActive">A value indicating whether the payment method is active</param>
        /// <param name="displayOrder">The display order</param>
        /// <returns>Payment method</returns>
        public abstract DBPaymentMethod UpdatePaymentMethod(int paymentMethodId, 
            string name, string visibleName, string description, string configureTemplatePath, 
            string userTemplatePath, string className, string systemKeyword,
            bool isActive, int displayOrder);

        /// <summary>
        /// Inserts payment method country mapping
        /// </summary>
        /// <param name="paymentMethodId">The payment method identifier</param>
        /// <param name="countryId">The country identifier</param>
        public abstract void InsertPaymentMethodCountryMapping(int paymentMethodId, int countryId);

        /// <summary>
        /// Checking whether the payment method country mapping is exists
        /// </summary>
        /// <param name="paymentMethodId">The payment method identifier</param>
        /// <param name="countryId">The country identifier</param>
        /// <returns>True if mapping exist, otherwise false</returns>
        public abstract bool IsPaymentMethodCountryMappingExists(int paymentMethodId, int countryId);

        /// <summary>
        /// Deletes payment method country mapping
        /// </summary>
        /// <param name="paymentMethodId">The payment method identifier</param>
        /// <param name="countryId">The country identifier</param>
        public abstract void DeletePaymentMethodCountryMapping(int paymentMethodId, int countryId);
        
        #endregion
    }
}
