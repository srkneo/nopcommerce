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
using NopSolutions.NopCommerce.BusinessLogic.Caching;
using NopSolutions.NopCommerce.BusinessLogic.Configuration.Settings;
using NopSolutions.NopCommerce.DataAccess;
using NopSolutions.NopCommerce.DataAccess.Content.Polls;

namespace NopSolutions.NopCommerce.BusinessLogic.Content.Polls
{
    /// <summary>
    /// Poll manager
    /// </summary>
    public partial class PollManager
    {
        #region Constants
        private const string POLLS_BY_ID_KEY = "Nop.polls.id-{0}";
        private const string POLLANSWERS_BY_POLLID_KEY = "Nop.pollanswers.pollid-{0}";
        private const string POLLS_PATTERN_KEY = "Nop.polls.";
        private const string POLLANSWERS_PATTERN_KEY = "Nop.pollanswers.";
        #endregion

        #region Utilities
        private static PollCollection DBMapping(DBPollCollection dbCollection)
        {
            if (dbCollection == null)
                return null;

            var collection = new PollCollection();
            foreach (var dbItem in dbCollection)
            {
                var item = DBMapping(dbItem);
                collection.Add(item);
            }

            return collection;
        }

        private static Poll DBMapping(DBPoll dbItem)
        {
            if (dbItem == null)
                return null;

            var item = new Poll();
            item.PollId = dbItem.PollId;
            item.LanguageId = dbItem.LanguageId;
            item.Name = dbItem.Name;
            item.SystemKeyword = dbItem.SystemKeyword;
            item.Published = dbItem.Published;
            item.DisplayOrder = dbItem.DisplayOrder;

            return item;
        }

        private static PollAnswerCollection DBMapping(DBPollAnswerCollection dbCollection)
        {
            if (dbCollection == null)
                return null;

            var collection = new PollAnswerCollection();
            foreach (var dbItem in dbCollection)
            {
                var item = DBMapping(dbItem);
                collection.Add(item);
            }

            return collection;
        }

        private static PollAnswer DBMapping(DBPollAnswer dbItem)
        {
            if (dbItem == null)
                return null;

            var item = new PollAnswer();
            item.PollAnswerId = dbItem.PollAnswerId;
            item.PollId = dbItem.PollId;
            item.Name = dbItem.Name;
            item.Count = dbItem.Count;
            item.DisplayOrder = dbItem.DisplayOrder;

            return item;
        }
        #endregion

        #region Methods
        /// <summary>
        /// Gets a poll
        /// </summary>
        /// <param name="pollId">The poll identifier</param>
        /// <returns>Poll</returns>
        public static Poll GetPollById(int pollId)
        {
            if (pollId == 0)
                return null;

            string key = string.Format(POLLS_BY_ID_KEY, pollId);
            object obj2 = NopCache.Get(key);
            if (PollManager.CacheEnabled && (obj2 != null))
            {
                return (Poll)obj2;
            }

            var dbItem = DBProviderManager<DBPollProvider>.Provider.GetPollById(pollId);
            var poll = DBMapping(dbItem);

            if (PollManager.CacheEnabled)
            {
                NopCache.Max(key, poll);
            }
            return poll;
        }

        /// <summary>
        /// Gets a poll
        /// </summary>
        /// <param name="systemKeyword">The poll system keyword</param>
        /// <returns>Poll</returns>
        public static Poll GetPollBySystemKeyword(string systemKeyword)
        {
            var dbItem = DBProviderManager<DBPollProvider>.Provider.GetPollBySystemKeyword(systemKeyword);
            var poll = DBMapping(dbItem);
            return poll;
        }

        /// <summary>
        /// Gets poll collection
        /// </summary>
        /// <param name="languageId">Language identifier. 0 if you want to get all news</param>
        /// <param name="pollCount">Poll count to load. 0 if you want to get all polls</param>
        /// <returns>Poll collection</returns>
        public static PollCollection GetPolls(int languageId, int pollCount)
        {
            bool showHidden = NopContext.Current.IsAdmin;
            var dbCollection = DBProviderManager<DBPollProvider>.Provider.GetPolls(languageId, pollCount, showHidden);
            var collection = DBMapping(dbCollection);
            return collection;
        }

        /// <summary>
        /// Deletes a poll
        /// </summary>
        /// <param name="pollId">The poll identifier</param>
        public static void DeletePoll(int pollId)
        {
            DBProviderManager<DBPollProvider>.Provider.DeletePoll(pollId);
            if (PollManager.CacheEnabled)
            {
                NopCache.RemoveByPattern(POLLS_PATTERN_KEY);
                NopCache.RemoveByPattern(POLLANSWERS_PATTERN_KEY);
            }
        }

        /// <summary>
        /// Gets all polls
        /// </summary>
        /// <param name="languageId">Language identifier. 0 if you want to get all news</param>
        /// <returns>Poll collection</returns>
        public static PollCollection GetAllPolls(int languageId)
        {
            return GetPolls(languageId, 0);
        }

        /// <summary>
        /// Inserts a poll
        /// </summary>
        /// <param name="languageId">The language identifier</param>
        /// <param name="name">The name</param>
        /// <param name="systemKeyword">The system keyword</param>
        /// <param name="published">A value indicating whether the entity is published</param>
        /// <param name="displayOrder">The display order</param>
        /// <returns>Poll</returns>
        public static Poll InsertPoll(int languageId, string name, string systemKeyword,
            bool published, int displayOrder)
        {
            var dbItem = DBProviderManager<DBPollProvider>.Provider.InsertPoll(languageId,
                name, systemKeyword, published, displayOrder);
            var poll = DBMapping(dbItem);

            if (PollManager.CacheEnabled)
            {
                NopCache.RemoveByPattern(POLLS_PATTERN_KEY);
                NopCache.RemoveByPattern(POLLANSWERS_PATTERN_KEY);
            }

            return poll;
        }

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
        public static Poll UpdatePoll(int pollId, int languageId, string name,
            string systemKeyword, bool published, int displayOrder)
        {
            var dbItem = DBProviderManager<DBPollProvider>.Provider.UpdatePoll(pollId, 
                languageId, name, systemKeyword, published, displayOrder);
            var poll = DBMapping(dbItem);

            if (PollManager.CacheEnabled)
            {
                NopCache.RemoveByPattern(POLLS_PATTERN_KEY);
                NopCache.RemoveByPattern(POLLANSWERS_PATTERN_KEY);
            }

            return poll;
        }

        /// <summary>
        /// Is voting record already exists
        /// </summary>
        /// <param name="pollId">Poll identifier</param>
        /// <param name="customerId">Customer identifier</param>
        /// <returns>Poll</returns>
        public static bool PollVotingRecordExists(int pollId, int customerId)
        {
            return DBProviderManager<DBPollProvider>.Provider.PollVotingRecordExists(pollId, customerId);
        }

        /// <summary>
        /// Gets a poll answer
        /// </summary>
        /// <param name="pollAnswerId">Poll answer identifier</param>
        /// <returns>Poll answer</returns>
        public static PollAnswer GetPollAnswerById(int pollAnswerId)
        {
            if (pollAnswerId == 0)
                return null;

            var dbItem = DBProviderManager<DBPollProvider>.Provider.GetPollAnswerById(pollAnswerId);
            var pollAnswer = DBMapping(dbItem);
            return pollAnswer;
        }

        /// <summary>
        /// Gets a poll answers by poll identifier
        /// </summary>
        /// <param name="pollId">Poll identifier</param>
        /// <returns>Poll answer collection</returns>
        public static PollAnswerCollection GetPollAnswersByPollId(int pollId)
        {
            string key = string.Format(POLLANSWERS_BY_POLLID_KEY, pollId);
            object obj2 = NopCache.Get(key);
            if (PollManager.CacheEnabled && (obj2 != null))
            {
                return (PollAnswerCollection)obj2;
            }

            var dbCollection = DBProviderManager<DBPollProvider>.Provider.GetPollAnswersByPollId(pollId);
            var pollAnswerCollection = DBMapping(dbCollection);

            if (PollManager.CacheEnabled)
            {
                NopCache.Max(key, pollAnswerCollection);
            }
            return pollAnswerCollection;
        }

        /// <summary>
        /// Deletes a poll answer
        /// </summary>
        /// <param name="pollAnswerId">Poll answer identifier</param>
        public static void DeletePollAnswer(int pollAnswerId)
        {
            DBProviderManager<DBPollProvider>.Provider.DeletePollAnswer(pollAnswerId);
            if (PollManager.CacheEnabled)
            {
                NopCache.RemoveByPattern(POLLS_PATTERN_KEY);
                NopCache.RemoveByPattern(POLLANSWERS_PATTERN_KEY);
            }
        }

        /// <summary>
        /// Inserts a poll answer
        /// </summary>
        /// <param name="pollId">The poll identifier</param>
        /// <param name="name">The poll answer name</param>
        /// <param name="count">The current count</param>
        /// <param name="displayOrder">The display order</param>
        /// <returns>Poll answer</returns>
        public static PollAnswer InsertPollAnswer(int pollId,
            string name, int count, int displayOrder)
        {
            var dbItem = DBProviderManager<DBPollProvider>.Provider.InsertPollAnswer(pollId,
                name, count, displayOrder);
            var pollAnswer = DBMapping(dbItem);

            if (PollManager.CacheEnabled)
            {
                NopCache.RemoveByPattern(POLLS_PATTERN_KEY);
                NopCache.RemoveByPattern(POLLANSWERS_PATTERN_KEY);
            }

            return pollAnswer;
        }

        /// <summary>
        /// Updates the poll answer
        /// </summary>
        /// <param name="pollAnswerId">The poll answer identifier</param>
        /// <param name="pollId">The poll identifier</param>
        /// <param name="name">The poll answer name</param>
        /// <param name="count">The current count</param>
        /// <param name="displayOrder">The display order</param>
        /// <returns>Poll answer</returns>
        public static PollAnswer UpdatePoll(int pollAnswerId,
            int pollId, string name, int count, int displayOrder)
        {
            var dbItem = DBProviderManager<DBPollProvider>.Provider.UpdatePollAnswer(pollAnswerId, 
                pollId, name, count, displayOrder);
            var pollAnswer = DBMapping(dbItem);

            if (PollManager.CacheEnabled)
            {
                NopCache.RemoveByPattern(POLLS_PATTERN_KEY);
                NopCache.RemoveByPattern(POLLANSWERS_PATTERN_KEY);
            }

            return pollAnswer;
        }

        /// <summary>
        /// Creates a poll voting record
        /// </summary>
        /// <param name="pollAnswerId">The poll answer identifier</param>
        /// <param name="customerId">Customer identifer</param>
        public static void CreatePollVotingRecord(int pollAnswerId, int customerId)
        {
            DBProviderManager<DBPollProvider>.Provider.CreatePollVotingRecord(pollAnswerId,
                customerId);
            if (PollManager.CacheEnabled)
            {
                NopCache.RemoveByPattern(POLLS_PATTERN_KEY);
                NopCache.RemoveByPattern(POLLANSWERS_PATTERN_KEY);
            }
        }
        #endregion

        #region Property
        /// <summary>
        /// Gets a value indicating whether cache is enabled
        /// </summary>
        public static bool CacheEnabled
        {
            get
            {
                return SettingManager.GetSettingValueBoolean("Cache.PollManager.CacheEnabled");
            }
        }
        #endregion
    }
}
