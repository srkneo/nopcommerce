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


namespace NopSolutions.NopCommerce.BusinessLogic.Audit
{
    /// <summary>
    /// Represents an activity log type collection
    /// </summary>
    public partial class ActivityLogTypeCollection : BaseEntityCollection<ActivityLogType>
    {
        /// <summary>
        /// Find activity log type by system keyword
        /// </summary>
        /// <param name="SystemKeyword">The system keyword</param>
        /// <returns>Activity log type item</returns>
        public ActivityLogType FindBySystemKeyword(string SystemKeyword)
        {
            for (int i = 0; i < Count; i++)
                if (this[i].SystemKeyword.ToLowerInvariant().Equals(SystemKeyword.ToLowerInvariant()))
                    return this[i];
            return null;
        }
    }
}
