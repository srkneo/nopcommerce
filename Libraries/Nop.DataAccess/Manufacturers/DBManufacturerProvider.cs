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
using System.Configuration;
using System.Configuration.Provider;
using System.Web.Hosting;
using System.Web.Configuration;
using System.Collections.Specialized;

namespace NopSolutions.NopCommerce.DataAccess.Manufacturers
{
    /// <summary>
    /// Acts as a base class for deriving custom manufacturer provider
    /// </summary>
    [DBProviderSectionName("nopDataProviders/ManufacturerProvider")]
    public abstract partial class DBManufacturerProvider : BaseDBProvider
    {
        #region Methods

        /// <summary>
        /// Gets product product manufacturer collection
        /// </summary>
        /// <param name="manufacturerId">Manufacturer identifier</param>
        /// <param name="showHidden">A value indicating whether to show hidden records</param>
        /// <returns>Product manufacturer collection</returns>
        public abstract DBProductManufacturerCollection GetProductManufacturersByManufacturerId(int manufacturerId, bool showHidden);

        /// <summary>
        /// Gets a product manufacturer mapping collection
        /// </summary>
        /// <param name="productId">Product identifier</param>
        /// <param name="showHidden">A value indicating whether to show hidden records</param>
        /// <returns>Product manufacturer mapping collection</returns>
        public abstract DBProductManufacturerCollection GetProductManufacturersByProductId(int productId, bool showHidden);

        #endregion
    }
}
