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
using System.Text;


namespace NopSolutions.NopCommerce.BusinessLogic.Shipping
{
    /// <summary>
    /// Represents a shipping option
    /// </summary>
    [Serializable]
    public partial class ShippingOption
    {
        private int shippingRateComputationMethodID;
        private decimal rate;
        private string name;
        private string description;
        private int appliedDiscountID;

        #region Properties

        /// <summary>
        /// Gets or sets shipping rate computation method
        /// </summary>
        public int ShippingRateComputationMethodID
        {
            get
            {
                return shippingRateComputationMethodID;
            }
            set
            {
                shippingRateComputationMethodID = value;
            }
        }

        /// <summary>
        /// Gets or sets a shipping rate
        /// </summary>
        public decimal Rate
        {
            get
            {
                return rate;
            }
            set
            {
                rate = value;
            }
        }

        /// <summary>
        /// Gets or sets a shipping option name
        /// </summary>
        public string Name
        {
            get
            {
                return name;
            }
            set
            {
                name = value;
            }
        }

        /// <summary>
        /// Gets or sets a shipping option description
        /// </summary>
        public string Description
        {
            get
            {
                return description;
            }
            set
            {
                description = value;
            }
        }

        /// <summary>
        /// Gets or sets an applied discount identifier
        /// </summary>
        public int AppliedDiscountID
        {
            get
            {
                return appliedDiscountID;
            }
            set
            {
                appliedDiscountID = value;
            }
        }
        #endregion
    }
}
