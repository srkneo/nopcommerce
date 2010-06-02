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
        /// Gets a poll
        /// </summary>
        /// <param name="pollId">The poll identifier</param>
        /// <returns>Poll</returns>
        public abstract DBPoll GetPollById(int pollId);

        /// <summary>
        /// Gets a poll
        /// </summary>
        /// <param name="systemKeyword">Poll system keyword</param>
        /// <returns>Poll</returns>
        public abstract DBPoll GetPollBySystemKeyword(string systemKeyword);

        /// <summary>
        /// Gets poll collection
        /// </summary>
        /// <param name="languageId">Language identifier. 0 if you want to get all news</param>
        /// <param name="pollCount">Poll count to load. 0 if you want to get all polls</param>
        /// <param name="showHidden">A value indicating whether to show hidden records</param>
        /// <returns>Poll collection</returns>
        public abstract DBPollCollection GetPolls(int languageId, 
            int pollCount, bool showHidden);

        /// <summary>
        /// Deletes a poll
        /// </summary>
        /// <param name="pollId">The poll identifier</param>
        public abstract void DeletePoll(int pollId);

        /// <summary>
        /// Inserts a poll
        /// </summary>
        /// <param name="languageId">The language identifier</param>
        /// <param name="name">The name</param>
        /// <param name="systemKeyword">The system keyword</param>
        /// <param name="published">A value indicating whether the entity is published</param>
        /// <param name="displayOrder">The display order</param>
        /// <returns>Poll</returns>
        public abstract DBPoll InsertPoll(int languageId, string name, string systemKeyword,
            bool published, int displayOrder);

        /// <summary>
        /// Updates the poll
        /// </summary>
        /// <param name="pollId">The poll identifier</param>
        /// <param name="languageId">The language identifier</param>
        /// <param name="name">The name</param>
        /// <param name="systemKeyword">The system keyword</param>
        /// <param name="published">A value indicating whether the entity is published</param>
        /// <param name="displayOrder">The display order</param>
        /// <returns>Poll</returns>
        public abstract DBPoll UpdatePoll(int pollId, int languageId, string name, 
            string systemKeyword, bool published, int displayOrder);

        /// <summary>
        /// Is voting record already exists
        /// </summary>
        /// <param name="pollId">Poll identifier</param>
        /// <param name="customerId">Customer identifier</param>
        /// <returns>Poll</returns>
        public abstract bool PollVotingRecordExists(int pollId, int customerId);

        /// <summary>
        /// Gets a poll answer
        /// </summary>
        /// <param name="pollAnswerId">Poll answer identifier</param>
        /// <returns>Poll answer</returns>
        public abstract DBPollAnswer GetPollAnswerById(int pollAnswerId);

        /// <summary>
        /// Gets a poll answers by poll identifier
        /// </summary>
        /// <param name="pollId">Poll identifier</param>
        /// <returns>Poll answer collection</returns>
        public abstract DBPollAnswerCollection GetPollAnswersByPollId(int pollId);

        /// <summary>
        /// Deletes a poll answer
        /// </summary>
        /// <param name="pollAnswerId">Poll answer identifier</param>
        public abstract void DeletePollAnswer(int pollAnswerId);

        /// <summary>
        /// Inserts a poll answer
        /// </summary>
        /// <param name="pollId">The poll identifier</param>
        /// <param name="name">The poll answer name</param>
        /// <param name="count">The current count</param>
        /// <param name="displayOrder">The display order</param>
        /// <returns>Poll answer</returns>
        public abstract DBPollAnswer InsertPollAnswer(int pollId, 
            string name, int count, int displayOrder);

        /// <summary>
        /// Updates the poll answer
        /// </summary>
        /// <param name="pollAnswerId">The poll answer identifier</param>
        /// <param name="pollId">The poll identifier</param>
        /// <param name="name">The poll answer name</param>
        /// <param name="count">The current count</param>
        /// <param name="displayOrder">The display order</param>
        /// <returns>Poll answer</returns>
        public abstract DBPollAnswer UpdatePollAnswer(int pollAnswerId, 
            int pollId, string name, int count, int displayOrder);

        /// <summary>
        /// Creates a poll voting record
        /// </summary>
        /// <param name="pollAnswerId">The poll answer identifier</param>
        /// <param name="customerId">Customer identifer</param>
        public abstract void CreatePollVotingRecord(int pollAnswerId, int customerId);

        #endregion
    }
}
