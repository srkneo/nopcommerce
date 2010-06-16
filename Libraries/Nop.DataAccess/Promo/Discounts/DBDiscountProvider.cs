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

namespace NopSolutions.NopCommerce.DataAccess.Promo.Discounts
{
    /// <summary>
    /// Acts as a base class for deriving custom discount provider
    /// </summary>
    [DBProviderSectionName("nopDataProviders/DiscountProvider")]
    public abstract partial class DBDiscountProvider : BaseDBProvider
    {
        #region Methods

        /// <summary>
        /// Gets all discount usage history entries
        /// </summary>
        /// <param name="discountId">Discount type identifier; null to load all</param>
        /// <param name="customerId">Customer identifier; null to load all</param>
        /// <param name="orderId">Order identifier; null to load all</param>
        /// <returns>Discount usage history entries</returns>
        public abstract DBDiscountUsageHistoryCollection GetAllDiscountUsageHistoryEntries(int? discountId,
            int? customerId, int? orderId);

        #endregion
    }
}
