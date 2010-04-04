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

namespace NopSolutions.NopCommerce.DataAccess.Orders
{
    /// <summary>
    /// Represents a recurring payment
    /// </summary>
    public partial class DBRecurringPayment : BaseDBEntity
    {
        #region Ctor
        /// <summary>
        /// Creates a new instance of the DBRecurringPayment class
        /// </summary>
        public DBRecurringPayment()
        {
        }
        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the recurring payment identifier
        /// </summary>
        public int RecurringPaymentID { get; set; }

        /// <summary>
        /// Gets or sets the initial order identifier
        /// </summary>
        public int InitialOrderID { get; set; }

        /// <summary>
        /// Gets or sets the cycle length
        /// </summary>
        public int CycleLength { get; set; }

        /// <summary>
        /// Gets or sets the cycle period
        /// </summary>
        public int CyclePeriod { get; set; }

        /// <summary>
        /// Gets or sets the total cycles
        /// </summary>
        public int TotalCycles { get; set; }

        /// <summary>
        /// Gets or sets the start date
        /// </summary>
        public DateTime StartDate { get; set; }
        
        /// <summary>
        /// Gets or sets a value indicating whether the payment is active
        /// </summary>
        public bool IsActive { get; set; }
        
        /// <summary>
        /// Gets or sets a value indicating whether the entity has been deleted
        /// </summary>
        public bool Deleted { get; set; }

        /// <summary>
        /// Gets or sets the date and time of payment creation
        /// </summary>
        public DateTime CreatedOn { get; set; }

        #endregion
    }
}
