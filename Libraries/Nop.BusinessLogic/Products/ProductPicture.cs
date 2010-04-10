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
using NopSolutions.NopCommerce.BusinessLogic.Media;



namespace NopSolutions.NopCommerce.BusinessLogic.Products
{
    /// <summary>
    /// Represents a product picture mapping
    /// </summary>
    public partial class ProductPicture : BaseEntity
    {
        #region Ctor
        /// <summary>
        /// Creates a new instance of the ProductPicture class
        /// </summary>
        public ProductPicture()
        {
        }
        #endregion

        #region Properties
        /// <summary>
        /// Gets or sets the ProductPicture identifier
        /// </summary>
        public int ProductPictureID { get; set; }

        /// <summary>
        /// Gets or sets the product identifier
        /// </summary>
        public int ProductID { get; set; }

        /// <summary>
        /// Gets or sets the picture identifier
        /// </summary>
        public int PictureID { get; set; }

        /// <summary>
        /// Gets or sets the display order
        /// </summary>
        public int DisplayOrder { get; set; }
        #endregion 
        
        #region Custom Properties
        /// <summary>
        /// Gets the picture
        /// </summary>
        public Picture Picture
        {
            get
            {
                return PictureManager.GetPictureByID(PictureID);
            }
        }
        #endregion
    }

}
