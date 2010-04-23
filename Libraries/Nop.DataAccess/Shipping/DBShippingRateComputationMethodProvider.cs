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
using System.Configuration.Provider;
using System.Configuration;
using System.Collections.Specialized;
using System.Web.Hosting;
using System.Web.Configuration;

namespace NopSolutions.NopCommerce.DataAccess.Shipping
{
    /// <summary>
    /// Acts as a base class for deriving custom shipping rate computation method provider
    /// </summary>
    [DBProviderSectionName("nopDataProviders/ShippingRateComputationMethodProvider")]
    public abstract partial class DBShippingRateComputationMethodProvider : BaseDBProvider
    {
        #region Methods

        /// <summary>
        /// Deletes a shipping rate computation method
        /// </summary>
        /// <param name="shippingRateComputationMethodId">Shipping rate computation method identifier</param>
        public abstract void DeleteShippingRateComputationMethod(int shippingRateComputationMethodId);

        /// <summary>
        /// Gets a shipping rate computation method
        /// </summary>
        /// <param name="shippingRateComputationMethodId">Shipping rate computation method identifier</param>
        /// <returns>Shipping rate computation method</returns>
        public abstract DBShippingRateComputationMethod GetShippingRateComputationMethodById(int shippingRateComputationMethodId);

        /// <summary>
        /// Gets all shipping rate computation methods
        /// </summary>
        /// <param name="showHidden">A value indicating whether to show hidden records</param>
        /// <returns>Shipping rate computation method collection</returns>
        public abstract DBShippingRateComputationMethodCollection GetAllShippingRateComputationMethods(bool showHidden);

        /// <summary>
        /// Inserts a shipping rate computation method
        /// </summary>
        /// <param name="name">The name</param>
        /// <param name="description">The description</param>
        /// <param name="configureTemplatePath">The configure template path</param>
        /// <param name="className">The class name</param>
        /// <param name="isActive">The value indicating whether the method is active</param>
        /// <param name="displayOrder">The display order</param>
        /// <returns>Shipping rate computation method</returns>
        public abstract DBShippingRateComputationMethod InsertShippingRateComputationMethod(string name, 
            string description, string configureTemplatePath, string className,
            bool isActive, int displayOrder);

        /// <summary>
        /// Updates the shipping rate computation method
        /// </summary>
        /// <param name="shippingRateComputationMethodId">The shipping rate computation method identifier</param>
        /// <param name="name">The name</param>
        /// <param name="description">The description</param>
        /// <param name="configureTemplatePath">The configure template path</param>
        /// <param name="className">The class name</param>
        /// <param name="isActive">The value indicating whether the method is active</param>
        /// <param name="displayOrder">The display order</param>
        /// <returns>Shipping rate computation method</returns>
        public abstract DBShippingRateComputationMethod UpdateShippingRateComputationMethod(int shippingRateComputationMethodId, 
            string name, string description, string configureTemplatePath, string className,
            bool isActive, int displayOrder);

        #endregion
    }
}
