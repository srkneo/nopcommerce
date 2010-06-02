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
using NopSolutions.NopCommerce.BusinessLogic.Configuration.Settings;
using NopSolutions.NopCommerce.DataAccess;
using NopSolutions.NopCommerce.DataAccess.Shipping;

namespace NopSolutions.NopCommerce.BusinessLogic.Shipping
{
    /// <summary>
    /// "ShippingByWeight" manager
    /// </summary>
    public partial class ShippingByWeightManager
    {
        #region Utilities
        private static ShippingByWeightCollection DBMapping(DBShippingByWeightCollection dbCollection)
        {
            if (dbCollection == null)
                return null;

            var collection = new ShippingByWeightCollection();
            foreach (var dbItem in dbCollection)
            {
                var item = DBMapping(dbItem);
                collection.Add(item);
            }

            return collection;
        }

        private static ShippingByWeight DBMapping(DBShippingByWeight dbItem)
        {
            if (dbItem == null)
                return null;

            var item = new ShippingByWeight();
            item.ShippingByWeightId = dbItem.ShippingByWeightId;
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
        /// Gets a ShippingByWeight
        /// </summary>
        /// <param name="shippingByWeightId">ShippingByWeight identifier</param>
        /// <returns>ShippingByWeight</returns>
        public static ShippingByWeight GetById(int shippingByWeightId)
        {
            if (shippingByWeightId == 0)
                return null;

            var dbItem = DBProviderManager<DBShippingByWeightProvider>.Provider.GetById(shippingByWeightId);
            var shippingByWeight = DBMapping(dbItem);
            return shippingByWeight;
        }

        /// <summary>
        /// Deletes a ShippingByWeight
        /// </summary>
        /// <param name="shippingByWeightId">ShippingByWeight identifier</param>
        public static void DeleteShippingByWeight(int shippingByWeightId)
        {
            DBProviderManager<DBShippingByWeightProvider>.Provider.DeleteShippingByWeight(shippingByWeightId);
        }

        /// <summary>
        /// Gets all ShippingByWeights
        /// </summary>
        /// <returns>ShippingByWeight collection</returns>
        public static ShippingByWeightCollection GetAll()
        {
            var dbCollection = DBProviderManager<DBShippingByWeightProvider>.Provider.GetAll();
            var collection = DBMapping(dbCollection);
            return collection;
        }

        /// <summary>
        /// Inserts a ShippingByWeight
        /// </summary>
        /// <param name="shippingMethodId">The shipping method identifier</param>
        /// <param name="from">The "from" value</param>
        /// <param name="to">The "to" value</param>
        /// <param name="usePercentage">A value indicating whether to use percentage</param>
        /// <param name="shippingChargePercentage">The shipping charge percentage</param>
        /// <param name="shippingChargeAmount">The shipping charge amount</param>
        /// <returns>ShippingByWeight</returns>
        public static ShippingByWeight InsertShippingByWeight(int shippingMethodId,
            decimal from, decimal to, bool usePercentage,
            decimal shippingChargePercentage, decimal shippingChargeAmount)
        {
            var dbItem = DBProviderManager<DBShippingByWeightProvider>.Provider.InsertShippingByWeight(shippingMethodId,
                from, to, usePercentage, shippingChargePercentage, shippingChargeAmount);
            var shippingByWeight = DBMapping(dbItem);
            return shippingByWeight;
        }

        /// <summary>
        /// Updates the ShippingByWeight
        /// </summary>
        /// <param name="shippingByWeightId">The ShippingByWeight identifier</param>
        /// <param name="shippingMethodId">The shipping method identifier</param>
        /// <param name="from">The "from" value</param>
        /// <param name="to">The "to" value</param>
        /// <param name="usePercentage">A value indicating whether to use percentage</param>
        /// <param name="shippingChargePercentage">The shipping charge percentage</param>
        /// <param name="shippingChargeAmount">The shipping charge amount</param>
        /// <returns>ShippingByWeight</returns>
        public static ShippingByWeight UpdateShippingByWeight(int shippingByWeightId,
            int shippingMethodId, decimal from, decimal to, bool usePercentage,
            decimal shippingChargePercentage, decimal shippingChargeAmount)
        {
            var dbItem = DBProviderManager<DBShippingByWeightProvider>.Provider.UpdateShippingByWeight(shippingByWeightId, 
                shippingMethodId, from, to, usePercentage,
                shippingChargePercentage, shippingChargeAmount);
            var shippingByWeight = DBMapping(dbItem);
            return shippingByWeight;
        }

        /// <summary>
        /// Gets all ShippingByWeights by shipping method identifier
        /// </summary>
        /// <param name="shippingMethodId">The shipping method identifier</param>
        /// <returns>ShippingByWeight collection</returns>
        public static ShippingByWeightCollection GetAllByShippingMethodId(int shippingMethodId)
        {
            var dbCollection = DBProviderManager<DBShippingByWeightProvider>.Provider.GetAllByShippingMethodId(shippingMethodId);
            var collection = DBMapping(dbCollection);
            return collection;
        }
        #endregion

        #region Properties

         /// <summary>
        /// Gets or sets a value indicating whether to calculate per weight unit (e.g. per lb)
        /// </summary>
        public static bool CalculatePerWeightUnit
        {
            get
            {
                bool val1 = SettingManager.GetSettingValueBoolean("ShippingByWeight.CalculatePerWeightUnit");
                return val1;
            }
            set
            {
                SettingManager.SetParam("ShippingByWeight.CalculatePerWeightUnit", value.ToString());
            }
        }

        #endregion
    }
}
