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
using NopSolutions.NopCommerce.DataAccess.Shipping;

namespace NopSolutions.NopCommerce.BusinessLogic.Shipping
{
    /// <summary>
    /// "Shipping by total" manager
    /// </summary>
    public partial class ShippingByTotalManager
    {
        #region Utilities
        private static ShippingByTotalCollection DBMapping(DBShippingByTotalCollection dbCollection)
        {
            if (dbCollection == null)
                return null;

            var collection = new ShippingByTotalCollection();
            foreach (var dbItem in dbCollection)
            {
                var item = DBMapping(dbItem);
                collection.Add(item);
            }

            return collection;
        }

        private static ShippingByTotal DBMapping(DBShippingByTotal dbItem)
        {
            if (dbItem == null)
                return null;

            var item = new ShippingByTotal();
            item.ShippingByTotalId = dbItem.ShippingByTotalId;
            item.ShippingMethodId = dbItem.ShippingMethodId;
            item.From = dbItem.From;
            item.To = dbItem.To;
            item.UsePercentage = dbItem.UsePercentage;
            item.ShippingChargePercentage = dbItem.ShippingChargePercentage;
            item.ShippingChargeAmount = dbItem.ShippingChargeAmount;

            return item;
        }
        #endregion

        #region Methods
        /// <summary>
        /// Get a ShippingByTotal
        /// </summary>
        /// <param name="shippingByTotalId">ShippingByTotal identifier</param>
        /// <returns>ShippingByTotal</returns>
        public static ShippingByTotal GetById(int shippingByTotalId)
        {
            if (shippingByTotalId == 0)
                return null;

            var dbItem = DBProviderManager<DBShippingByTotalProvider>.Provider.GetById(shippingByTotalId);
            var shippingByTotal = DBMapping(dbItem);
            return shippingByTotal;
        }

        /// <summary>
        /// Deletes a ShippingByTotal
        /// </summary>
        /// <param name="shippingByTotalId">ShippingByTotal identifier</param>
        public static void DeleteShippingByTotal(int shippingByTotalId)
        {
            DBProviderManager<DBShippingByTotalProvider>.Provider.DeleteShippingByTotal(shippingByTotalId);
        }

        /// <summary>
        /// Gets all ShippingByTotals
        /// </summary>
        /// <returns>ShippingByTotal collection</returns>
        public static ShippingByTotalCollection GetAll()
        {
            var dbCollection = DBProviderManager<DBShippingByTotalProvider>.Provider.GetAll();
            var collection = DBMapping(dbCollection);
            return collection;
        }

        /// <summary>
        /// Inserts a ShippingByTotal
        /// </summary>
        /// <param name="shippingMethodId">The shipping method identifier</param>
        /// <param name="from">The "from" value</param>
        /// <param name="to">The "to" value</param>
        /// <param name="usePercentage">A value indicating whether to use percentage</param>
        /// <param name="shippingChargePercentage">The shipping charge percentage</param>
        /// <param name="shippingChargeAmount">The shipping charge amount</param>
        /// <returns>ShippingByTotal</returns>
        public static ShippingByTotal InsertShippingByTotal(int shippingMethodId,
            decimal from, decimal to, bool usePercentage,
            decimal shippingChargePercentage, decimal shippingChargeAmount)
        {
            var dbItem = DBProviderManager<DBShippingByTotalProvider>.Provider.InsertShippingByTotal(shippingMethodId,
                from, to, usePercentage, shippingChargePercentage, shippingChargeAmount);
            var shippingByTotal = DBMapping(dbItem);
            return shippingByTotal;
        }

        /// <summary>
        /// Updates the ShippingByTotal
        /// </summary>
        /// <param name="shippingByTotalId">The ShippingByTotal identifier</param>
        /// <param name="shippingMethodId">The shipping method identifier</param>
        /// <param name="from">The "from" value</param>
        /// <param name="to">The "to" value</param>
        /// <param name="usePercentage">A value indicating whether to use percentage</param>
        /// <param name="shippingChargePercentage">The shipping charge percentage</param>
        /// <param name="shippingChargeAmount">The shipping charge amount</param>
        /// <returns>ShippingByTotal</returns>
        public static ShippingByTotal UpdateShippingByTotal(int shippingByTotalId,
            int shippingMethodId, decimal from, decimal to, bool usePercentage,
            decimal shippingChargePercentage, decimal shippingChargeAmount)
        {
            var dbItem = DBProviderManager<DBShippingByTotalProvider>.Provider.UpdateShippingByTotal(shippingByTotalId,
                shippingMethodId, from, to, usePercentage,
                shippingChargePercentage, shippingChargeAmount);
            var shippingByTotal = DBMapping(dbItem);
            return shippingByTotal;
        }

        /// <summary>
        /// Gets all ShippingByTotals by shipping method identifier
        /// </summary>
        /// <param name="shippingMethodId">The shipping method identifier</param>
        /// <returns>ShippingByTotal collection</returns>
        public static ShippingByTotalCollection GetAllByShippingMethodId(int shippingMethodId)
        {
            var dbCollection = DBProviderManager<DBShippingByTotalProvider>.Provider.GetAllByShippingMethodId(shippingMethodId);
            var collection = DBMapping(dbCollection);
            return collection;
        }
        #endregion
    }
}
