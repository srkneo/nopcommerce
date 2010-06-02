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
    /// Acts as a base class for deriving custom tax provider provider
    /// </summary>
    [DBProviderSectionName("nopDataProviders/TaxProviderProvider")]
    public abstract partial class DBTaxProviderProvider : BaseDBProvider
    {
        #region Methods

        /// <summary>
        /// Deletes a tax provider
        /// </summary>
        /// <param name="taxProviderId">Tax provider identifier</param>
        public abstract void DeleteTaxProvider(int taxProviderId);

        /// <summary>
        /// Gets a tax provider
        /// </summary>
        /// <param name="taxProviderId">Tax provider identifier</param>
        /// <returns>Tax provider</returns>
        public abstract DBTaxProvider GetTaxProviderById(int taxProviderId);

        /// <summary>
        /// Gets all tax providers
        /// </summary>
        /// <returns>Tax rate computation method collection</returns>
        public abstract DBTaxProviderCollection GetAllTaxProviders();

        /// <summary>
        /// Inserts a tax provider
        /// </summary>
        /// <param name="name">The name</param>
        /// <param name="description">The description</param>
        /// <param name="configureTemplatePath">The configure template path</param>
        /// <param name="className">The class name</param>
        /// <param name="displayOrder">The display order</param>
        /// <returns>Tax provider</returns>
        public abstract DBTaxProvider InsertTaxProvider(string name, string description,
           string configureTemplatePath, string className, int displayOrder);

        /// <summary>
        /// Updates the tax provider
        /// </summary>
        /// <param name="taxProviderId">The tax provider identifier</param>
        /// <param name="name">The name</param>
        /// <param name="description">The description</param>
        /// <param name="configureTemplatePath">The configure template path</param>
        /// <param name="className">The class name</param>
        /// <param name="displayOrder">The display order</param>
        /// <returns>Tax provider</returns>
        public abstract DBTaxProvider UpdateTaxProvider(int taxProviderId, 
            string name, string description, string configureTemplatePath, 
            string className, int displayOrder);

        #endregion
    }
}
