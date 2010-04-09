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

namespace NopSolutions.NopCommerce.DataAccess.Products
{
    /// <summary>
    /// Represents a product price by customer role
    /// </summary>
    public partial class DBCustomerRoleProductPrice : BaseDBEntity
    {
        #region Ctor
        /// <summary>
        /// Creates a new instance of the DBCustomerRoleProductPrice class
        /// </summary>
        public DBCustomerRoleProductPrice()
        {
        }
        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the identifier
        /// </summary>
        public int CustomerRoleProductPriceID { get; set; }

        /// <summary>
        /// Gets or sets the customer role identifier
        /// </summary>
        public int CustomerRoleID { get; set; }

        /// <summary>
        /// Gets or sets the product variant identifier
        /// </summary>
        public int ProductVariantID { get; set; }

        /// <summary>
        /// Gets or sets the price
        /// </summary>
        public decimal Price { get; set; }

        #endregion 
    }
}
