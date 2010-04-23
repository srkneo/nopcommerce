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
    /// Acts as a base class for deriving custom tax category provider
    /// </summary>
    [DBProviderSectionName("nopDataProviders/TaxCategoryProvider")]
    public abstract partial class DBTaxCategoryProvider : BaseDBProvider
    {
        #region Methods

        /// <summary>
        /// Deletes a tax category
        /// </summary>
        /// <param name="taxCategoryId">The tax category identifier</param>
        public abstract void DeleteTaxCategory(int taxCategoryId);

        /// <summary>
        /// Gets all tax categories
        /// </summary>
        /// <returns>Tax category collection</returns>
        public abstract DBTaxCategoryCollection GetAllTaxCategories();

        /// <summary>
        /// Gets a tax category
        /// </summary>
        /// <param name="taxCategoryId">Tax category identifier</param>
        /// <returns>Tax category</returns>
        public abstract DBTaxCategory GetTaxCategoryById(int taxCategoryId);

        /// <summary>
        /// Inserts a tax category
        /// </summary>
        /// <param name="name">The name</param>
        /// <param name="displayOrder">The display order</param>
        /// <param name="createdOn">The date and time of instance creation</param>
        /// <param name="updatedOn">The date and time of instance update</param>
        /// <returns>Tax category</returns>
        public abstract DBTaxCategory InsertTaxCategory(string name,
            int displayOrder, DateTime createdOn, DateTime updatedOn);

        /// <summary>
        /// Updates the tax category
        /// </summary>
        /// <param name="taxCategoryId">The tax category identifier</param>
        /// <param name="name">The name</param>
        /// <param name="displayOrder">The display order</param>
        /// <param name="createdOn">The date and time of instance creation</param>
        /// <param name="updatedOn">The date and time of instance update</param>
        /// <returns>Tax category</returns>
        public abstract DBTaxCategory UpdateTaxCategory(int taxCategoryId, string name,
            int displayOrder, DateTime createdOn, DateTime updatedOn);

        #endregion
    }
}
