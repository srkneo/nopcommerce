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


namespace NopSolutions.NopCommerce.DataAccess.Products.Attributes
{
    /// <summary>
    /// Represents a product variant attribute combination
    /// </summary>
    public partial class DBProductVariantAttributeCombination : BaseDBEntity
    {
        #region Ctor
        /// <summary>
        /// Creates a new instance of the DBProductVariantAttributeCombination class
        /// </summary>
        public DBProductVariantAttributeCombination()
        {
        }
        #endregion

        #region Properties
        /// <summary>
        /// Gets or sets the product variant attribute combination identifier
        /// </summary>
        public int ProductVariantAttributeCombinationID { get; set; }

        /// <summary>
        /// Gets or sets the product variant identifier
        /// </summary>
        public int ProductVariantID { get; set; }

        /// <summary>
        /// Gets or sets the attributes
        /// </summary>
        public string AttributesXML { get; set; }

        /// <summary>
        /// Gets or sets the stock quantity
        /// </summary>
        public int StockQuantity { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether to allow orders when out of stock
        /// </summary>
        public bool AllowOutOfStockOrders { get; set; }

        #endregion
    }

}
