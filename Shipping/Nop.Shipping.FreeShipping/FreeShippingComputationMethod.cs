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
using System.Linq;
using System.Text;
using NopSolutions.NopCommerce.BusinessLogic;
using NopSolutions.NopCommerce.BusinessLogic.Shipping;
using NopSolutions.NopCommerce.Common;
 

namespace NopSolutions.NopCommerce.Shipping.Methods.FreeShippingCM
{
    /// <summary>
    /// Free shipping computation method
    /// </summary>
    public class FreeShippingComputationMethod : IShippingRateComputationMethod
    {
        #region Methods

        /// <summary>
        ///  Gets available shipping options
        /// </summary>
        /// <param name="ShipmentPackage">Shipment package</param>
        /// <param name="Error">Error</param>
        /// <returns>Shipping options</returns>
        public ShippingOptionCollection GetShippingOptions(ShipmentPackage ShipmentPackage, ref string Error)
        {
            var shippingOptions = new ShippingOptionCollection();

            if (ShipmentPackage == null)
                throw new ArgumentNullException("ShipmentPackage");
            if (ShipmentPackage.Items == null)
                throw new NopException("No shipment items");
            if(ShipmentPackage.ShippingAddress == null)
            {
                Error = "Shipping address is not set";
                return shippingOptions;
            }
            if(ShipmentPackage.ShippingAddress.Country == null)
            {
                Error = "Shipping country is not set";
                return shippingOptions;
            }

            var shippingMethods = ShippingMethodManager.GetAllShippingMethods(ShipmentPackage.ShippingAddress.CountryID);
            foreach (var shippingMethod in shippingMethods)
            {
                var shippingOption = new ShippingOption();
                shippingOption.Name = shippingMethod.Name;
                shippingOption.Description = shippingMethod.Description;
                shippingOption.Rate = decimal.Zero;
                shippingOptions.Add(shippingOption);
            }

            return shippingOptions;
        }

        /// <summary>
        /// Gets fixed shipping rate (if shipping rate computation method allows it and the rate can be calculated before checkout).
        /// </summary>
        /// <param name="ShipmentPackage">Shipment package</param>
        /// <returns>Fixed shipping rate; or null if shipping rate could not be calculated before checkout</returns>
        public decimal? GetFixedRate(ShipmentPackage ShipmentPackage)
        {
            return decimal.Zero;
        }
        #endregion

        #region Properties

        /// <summary>
        /// Gets a shipping rate computation method type
        /// </summary>
        /// <returns>A shipping rate computation method type</returns>
        public ShippingRateComputationMethodTypeEnum ShippingRateComputationMethodType
        {
            get
            {
                return ShippingRateComputationMethodTypeEnum.Offline;
            }
        }

        #endregion
    }
}
