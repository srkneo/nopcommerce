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
    /// "ShippingByWeightAndCountry" manager
    /// </summary>
    public partial class ShippingByWeightAndCountryManager
    {
        #region Utilities
        private static ShippingByWeightAndCountryCollection DBMapping(DBShippingByWeightAndCountryCollection dbCollection)
        {
            if (dbCollection == null)
                return null;

            var collection = new ShippingByWeightAndCountryCollection();
            foreach (var dbItem in dbCollection)
            {
                var item = DBMapping(dbItem);
                collection.Add(item);
            }

            return collection;
        }

        private static ShippingByWeightAndCountry DBMapping(DBShippingByWeightAndCountry dbItem)
        {
            if (dbItem == null)
                return null;

            var item = new ShippingByWeightAndCountry();
            item.ShippingByWeightAndCountryId = dbItem.ShippingByWeightAndCountryId;
            item.ShippingMethodId = dbItem.ShippingMethodId;
            item.CountryId = dbItem.CountryId;
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
        /// Gets a ShippingByWeightAndCountry
        /// </summary>
        /// <param name="shippingByWeightAndCountryId">ShippingByWeightAndCountry identifier</param>
        /// <returns>ShippingByWeightAndCountry</returns>
        public static ShippingByWeightAndCountry GetById(int shippingByWeightAndCountryId)
        {
            if (shippingByWeightAndCountryId == 0)
                return null;

            var dbItem = DBProviderManager<DBShippingByWeightAndCountryProvider>.Provider.GetById(shippingByWeightAndCountryId);
            var shippingByWeightAndCountry = DBMapping(dbItem);
            return shippingByWeightAndCountry;
        }

        /// <summary>
        /// Deletes a ShippingByWeightAndCountry
        /// </summary>
        /// <param name="shippingByWeightAndCountryId">ShippingByWeightAndCountry identifier</param>
        public static void DeleteShippingByWeightAndCountry(int shippingByWeightAndCountryId)
        {
            DBProviderManager<DBShippingByWeightAndCountryProvider>.Provider.DeleteShippingByWeightAndCountry(shippingByWeightAndCountryId);
        }

        /// <summary>
        /// Gets all ShippingByWeightAndCountrys
        /// </summary>
        /// <returns>ShippingByWeightAndCountry collection</returns>
        public static ShippingByWeightAndCountryCollection GetAll()
        {
            var dbCollection = DBProviderManager<DBShippingByWeightAndCountryProvider>.Provider.GetAll();
            var collection = DBMapping(dbCollection);
            return collection;
        }

        /// <summary>
        /// Inserts a ShippingByWeightAndCountry
        /// </summary>
        /// <param name="shippingMethodId">The shipping method identifier</param>
        /// <param name="countryId">The country identifier</param>
        /// <param name="from">The "from" value</param>
        /// <param name="to">The "to" value</param>
        /// <param name="usePercentage">A value indicating whether to use percentage</param>
        /// <param name="shippingChargePercentage">The shipping charge percentage</param>
        /// <param name="shippingChargeAmount">The shipping charge amount</param>
        /// <returns>ShippingByWeightAndCountry</returns>
        public static ShippingByWeightAndCountry InsertShippingByWeightAndCountry(int shippingMethodId,
            int countryId, decimal from, decimal to, bool usePercentage,
            decimal shippingChargePercentage, decimal shippingChargeAmount)
        {
            var dbItem = DBProviderManager<DBShippingByWeightAndCountryProvider>.Provider.InsertShippingByWeightAndCountry(shippingMethodId,
                countryId, from, to, usePercentage,
                shippingChargePercentage, shippingChargeAmount);
            var shippingByWeightAndCountry = DBMapping(dbItem);
            return shippingByWeightAndCountry;
        }

        /// <summary>
        /// Updates the ShippingByWeightAndCountry
        /// </summary>
        /// <param name="shippingByWeightAndCountryId">The ShippingByWeightAndCountry identifier</param>
        /// <param name="shippingMethodId">The shipping method identifier</param>
        /// <param name="countryId">The country identifier</param>
        /// <param name="from">The "from" value</param>
        /// <param name="to">The "to" value</param>
        /// <param name="usePercentage">A value indicating whether to use percentage</param>
        /// <param name="shippingChargePercentage">The shipping charge percentage</param>
        /// <param name="shippingChargeAmount">The shipping charge amount</param>
        /// <returns>ShippingByWeightAndCountry</returns>
        public static ShippingByWeightAndCountry UpdateShippingByWeightAndCountry(int shippingByWeightAndCountryId,
            int shippingMethodId, int countryId, decimal from, decimal to, bool usePercentage,
            decimal shippingChargePercentage, decimal shippingChargeAmount)
        {
            var dbItem = DBProviderManager<DBShippingByWeightAndCountryProvider>.Provider.UpdateShippingByWeightAndCountry(shippingByWeightAndCountryId,
                shippingMethodId, countryId, from, to, usePercentage,
                shippingChargePercentage, shippingChargeAmount);
            var shippingByWeightAndCountry = DBMapping(dbItem);
            return shippingByWeightAndCountry;
        }

        /// <summary>
        /// Gets all ShippingByWeightAndCountrys by shipping method identifier
        /// </summary>
        /// <param name="shippingMethodId">The shipping method identifier</param>
        /// <param name="countryId">The country identifier</param>
        /// <returns>ShippingByWeightAndCountry collection</returns>
        public static ShippingByWeightAndCountryCollection GetAllByShippingMethodIdAndCountryId(int shippingMethodId, 
            int countryId)
        {
            var dbCollection = DBProviderManager<DBShippingByWeightAndCountryProvider>.Provider.GetAllByShippingMethodIdAndCountryId(shippingMethodId, countryId);
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
                bool val1 = SettingManager.GetSettingValueBoolean("ShippingByWeightAndCountry.CalculatePerWeightUnit");
                return val1;
            }
            set
            {
                SettingManager.SetParam("ShippingByWeightAndCountry.CalculatePerWeightUnit", value.ToString());
            }
        }

        #endregion
    }
}
