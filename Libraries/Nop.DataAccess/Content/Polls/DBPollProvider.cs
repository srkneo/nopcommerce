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
using System.Configuration;
using System.Web.Hosting;
using System.Web.Configuration;
using System.Collections.Specialized;
using System.Configuration.Provider;

namespace NopSolutions.NopCommerce.DataAccess.Content.Polls
{
    /// <summary>
    /// Acts as a base class for deriving custom poll provider
    /// </summary>
    [DBProviderSectionName("nopDataProviders/PollProvider")]
    public abstract partial class DBPollProvider : BaseDBProvider
    {
        #region Methods

        /// <summary>
        /// Is voting record already exists
        /// </summary>
        /// <param name="pollId">Poll identifier</param>
        /// <param name="customerId">Customer identifier</param>
        /// <returns>Poll</returns>
        public abstract bool PollVotingRecordExists(int pollId, int customerId);
        
        /// <summary>
        /// Creates a poll voting record
        /// </summary>
        /// <param name="pollAnswerId">The poll answer identifier</param>
        /// <param name="customerId">Customer identifer</param>
        public abstract void CreatePollVotingRecord(int pollAnswerId, int customerId);

        #endregion
    }
}
