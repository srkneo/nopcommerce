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

namespace NopSolutions.NopCommerce.DataAccess.Payment
{
    /// <summary>
    /// Acts as a base class for deriving custom credit card type provider
    /// </summary>
    [DBProviderSectionName("nopDataProviders/CreditCardTypeProvider")]
    public abstract partial class DBCreditCardTypeProvider : BaseDBProvider
    {
        #region Methods
        /// <summary>
        /// Gets a credit card type
        /// </summary>
        /// <param name="creditCardTypeId">Credit card type identifier</param>
        /// <returns>Credit card type</returns>
        public abstract DBCreditCardType GetCreditCardTypeById(int creditCardTypeId);

        /// <summary>
        /// Gets all credit card types
        /// </summary>
        /// <returns>Credit card type collection</returns>
        public abstract DBCreditCardTypeCollection GetAllCreditCardTypes();

        /// <summary>
        /// Inserts a credit card type
        /// </summary>
        /// <param name="name">The name</param>
        /// <param name="systemKeyword">The system keyword</param>
        /// <param name="displayOrder">The display order</param>
        /// <param name="deleted">A value indicating whether the entity has been deleted</param>
        /// <returns>A credit card type</returns>
        public abstract DBCreditCardType InsertCreditCardType(string name, 
            string systemKeyword, int displayOrder, bool deleted);

        /// <summary>
        /// Updates the credit card type
        /// </summary>
        /// <param name="creditCardTypeId">Credit card type identifier</param>
        /// <param name="name">The name</param>
        /// <param name="systemKeyword">The system keyword</param>
        /// <param name="displayOrder">The display order</param>
        /// <param name="deleted">A value indicating whether the entity has been deleted</param>
        /// <returns>A credit card type</returns>
        public abstract DBCreditCardType UpdateCreditCardType(int creditCardTypeId, 
            string name, string systemKeyword, int displayOrder, bool deleted);
        #endregion
    }
}
