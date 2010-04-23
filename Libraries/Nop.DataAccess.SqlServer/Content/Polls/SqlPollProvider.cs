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
using System.Configuration.Provider;
using System.Data;
using System.Data.Common;
using System.Globalization;
using System.Text;
using Microsoft.Practices.EnterpriseLibrary.Data;

namespace NopSolutions.NopCommerce.DataAccess.Content.Polls
{
    /// <summary>
    /// Poll provider for SQL Server
    /// </summary>
    public partial class SqlPollProvider : DBPollProvider
    {
        #region Fields
        private string _sqlConnectionString;
        #endregion

        #region Utilities
        private DBPoll GetPollFromReader(IDataReader dataReader)
        {
            var item = new DBPoll();
            item.PollId = NopSqlDataHelper.GetInt(dataReader, "PollID");
            item.LanguageId = NopSqlDataHelper.GetInt(dataReader, "LanguageID");
            item.Name = NopSqlDataHelper.GetString(dataReader, "Name");
            item.SystemKeyword = NopSqlDataHelper.GetString(dataReader, "SystemKeyword");
            item.Published = NopSqlDataHelper.GetBoolean(dataReader, "Published");
            item.DisplayOrder = NopSqlDataHelper.GetInt(dataReader, "DisplayOrder");
            return item;
        }

        private DBPollAnswer GetPollAnswerFromReader(IDataReader dataReader)
        {
            var item = new DBPollAnswer();
            item.PollAnswerId = NopSqlDataHelper.GetInt(dataReader, "PollAnswerID");
            item.PollId = NopSqlDataHelper.GetInt(dataReader, "PollID");
            item.Name = NopSqlDataHelper.GetString(dataReader, "Name");
            item.Count = NopSqlDataHelper.GetInt(dataReader, "Count");
            item.DisplayOrder = NopSqlDataHelper.GetInt(dataReader, "DisplayOrder");
            return item;
        }
        #endregion

        #region Methods

        /// <summary>
        /// Initializes the provider with the property values specified in the application's configuration file. This method is not intended to be used directly from your code
        /// </summary>
        /// <param name="name">The name of the provider instance to initialize</param>
        /// <param name="config">A NameValueCollection that contains the names and values of configuration options for the provider.</param>
        public override void Initialize(string name, NameValueCollection config)
        {
            if (config == null)
            {
                throw new ArgumentNullException("config");
            }

            base.Initialize(name, config);

            string connectionStringName = config["connectionStringName"];
            if (String.IsNullOrEmpty(connectionStringName))
                throw new ProviderException("Connection name not specified");
            this._sqlConnectionString = NopSqlDataHelper.GetConnectionString(connectionStringName);
            if ((this._sqlConnectionString == null) || (this._sqlConnectionString.Length < 1))
            {
                throw new ProviderException(string.Format("Connection string not found. {0}", connectionStringName));
            }
            config.Remove("connectionStringName");

            if (config.Count > 0)
            {
                string key = config.GetKey(0);
                if (!string.IsNullOrEmpty(key))
                {
                    throw new ProviderException(string.Format("Provider unrecognized attribute. {0}", new object[] { key }));
                }
            }
        }

        /// <summary>
        /// Gets a poll
        /// </summary>
        /// <param name="pollId">The poll identifier</param>
        /// <returns>Poll</returns>
        public override DBPoll GetPollById(int pollId)
        {
            DBPoll item = null;
            if (pollId == 0)
                return item;
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_PollLoadByPrimaryKey");
            db.AddInParameter(dbCommand, "PollID", DbType.Int32, pollId);
            using (IDataReader dataReader = db.ExecuteReader(dbCommand))
            {
                if (dataReader.Read())
                {
                    item = GetPollFromReader(dataReader);
                }
            }
            return item;
        }

        /// <summary>
        /// Gets a poll
        /// </summary>
        /// <param name="systemKeyword">Poll system keyword</param>
        /// <returns>Poll</returns>
        public override DBPoll GetPollBySystemKeyword(string systemKeyword)
        {
            DBPoll item = null;
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_PollLoadBySystemKeyword");
            db.AddInParameter(dbCommand, "SystemKeyword", DbType.String, systemKeyword);
            using (IDataReader dataReader = db.ExecuteReader(dbCommand))
            {
                if (dataReader.Read())
                {
                    item = GetPollFromReader(dataReader);
                }
            }
            return item;
        }

        /// <summary>
        /// Gets poll collection
        /// </summary>
        /// <param name="languageId">Language identifier. 0 if you want to get all news</param>
        /// <param name="pollCount">Poll count to load. 0 if you want to get all polls</param>
        /// <param name="showHidden">A value indicating whether to show hidden records</param>
        /// <returns>Poll collection</returns>
        public override DBPollCollection GetPolls(int languageId,
            int pollCount, bool showHidden)
        {
            var result = new DBPollCollection();
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_PollLoadAll");
            db.AddInParameter(dbCommand, "LanguageID", DbType.Int32, languageId);
            db.AddInParameter(dbCommand, "PollCount", DbType.Int32, pollCount);
            db.AddInParameter(dbCommand, "ShowHidden", DbType.Boolean, showHidden);
            using (IDataReader dataReader = db.ExecuteReader(dbCommand))
            {
                while (dataReader.Read())
                {
                    var item = GetPollFromReader(dataReader);
                    result.Add(item);
                }
            }

            return result;
        }

        /// <summary>
        /// Deletes a poll
        /// </summary>
        /// <param name="pollId">The poll identifier</param>
        public override void DeletePoll(int pollId)
        {
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_PollDelete");
            db.AddInParameter(dbCommand, "PollID", DbType.Int32, pollId);
            db.ExecuteNonQuery(dbCommand);
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
        public override DBPoll InsertPoll(int languageId, string name, string systemKeyword,
            bool published, int displayOrder)
        {
            DBPoll item = null;
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_PollInsert");
            db.AddOutParameter(dbCommand, "PollID", DbType.Int32, 0);
            db.AddInParameter(dbCommand, "LanguageID", DbType.Int32, languageId);
            db.AddInParameter(dbCommand, "Name", DbType.String, name);
            db.AddInParameter(dbCommand, "SystemKeyword", DbType.String, systemKeyword);
            db.AddInParameter(dbCommand, "Published", DbType.Boolean, published);
            db.AddInParameter(dbCommand, "DisplayOrder", DbType.Int32, displayOrder);
            if (db.ExecuteNonQuery(dbCommand) > 0)
            {
                int pollId = Convert.ToInt32(db.GetParameterValue(dbCommand, "@PollID"));
                item = GetPollById(pollId);
            }
            return item;
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
        public override DBPoll UpdatePoll(int pollId, int languageId, string name,
            string systemKeyword, bool published, int displayOrder)
        {
            DBPoll item = null;
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_PollUpdate");
            db.AddInParameter(dbCommand, "PollID", DbType.Int32, pollId);
            db.AddInParameter(dbCommand, "LanguageID", DbType.Int32, languageId);
            db.AddInParameter(dbCommand, "Name", DbType.String, name);
            db.AddInParameter(dbCommand, "SystemKeyword", DbType.String, systemKeyword);
            db.AddInParameter(dbCommand, "Published", DbType.Boolean, published);
            db.AddInParameter(dbCommand, "DisplayOrder", DbType.Int32, displayOrder);
            if (db.ExecuteNonQuery(dbCommand) > 0)
                item = GetPollById(pollId);

            return item;
        }

        /// <summary>
        /// Is voting record already exists
        /// </summary>
        /// <param name="pollId">Poll identifier</param>
        /// <param name="customerId">Customer identifier</param>
        /// <returns>Poll</returns>
        public override bool PollVotingRecordExists(int pollId, int customerId)
        {
            bool result = false;
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_PollVotingRecordExists");
            db.AddInParameter(dbCommand, "PollID", DbType.Int32, pollId);
            db.AddInParameter(dbCommand, "CustomerID", DbType.String, customerId);
            if ((int)db.ExecuteScalar(dbCommand) > 0)
                result = true;

            return result;
        }

        /// <summary>
        /// Gets a poll answer
        /// </summary>
        /// <param name="pollAnswerId">Poll answer identifier</param>
        /// <returns>Poll answer</returns>
        public override DBPollAnswer GetPollAnswerById(int pollAnswerId)
        {
            DBPollAnswer item = null;
            if (pollAnswerId == 0)
                return item;
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_PollAnswerLoadByPrimaryKey");
            db.AddInParameter(dbCommand, "PollAnswerID", DbType.Int32, pollAnswerId);
            using (IDataReader dataReader = db.ExecuteReader(dbCommand))
            {
                if (dataReader.Read())
                {
                    item = GetPollAnswerFromReader(dataReader);
                }
            }
            return item;
        }

        /// <summary>
        /// Gets a poll answers by poll identifier
        /// </summary>
        /// <param name="pollId">Poll identifier</param>
        /// <returns>Poll answer collection</returns>
        public override DBPollAnswerCollection GetPollAnswersByPollId(int pollId)
        {
            var result = new DBPollAnswerCollection();
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_PollAnswerLoadByPollID");
            db.AddInParameter(dbCommand, "PollID", DbType.Int32, pollId);
            using (IDataReader dataReader = db.ExecuteReader(dbCommand))
            {
                while (dataReader.Read())
                {
                    var item = GetPollAnswerFromReader(dataReader);
                    result.Add(item);
                }
            }

            return result;
        }

        /// <summary>
        /// Deletes a poll answer
        /// </summary>
        /// <param name="pollAnswerId">Poll answer identifier</param>
        public override void DeletePollAnswer(int pollAnswerId)
        {
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_PollAnswerDelete");
            db.AddInParameter(dbCommand, "PollAnswerID", DbType.Int32, pollAnswerId);
            db.ExecuteNonQuery(dbCommand);
        }

        /// <summary>
        /// Inserts a poll answer
        /// </summary>
        /// <param name="pollId">The poll identifier</param>
        /// <param name="name">The poll answer name</param>
        /// <param name="count">The current count</param>
        /// <param name="displayOrder">The display order</param>
        /// <returns>Poll answer</returns>
        public override DBPollAnswer InsertPollAnswer(int pollId,
            string name, int count, int displayOrder)
        {
            DBPollAnswer item = null;
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_PollAnswerInsert");
            db.AddOutParameter(dbCommand, "PollAnswerID", DbType.Int32, 0);
            db.AddInParameter(dbCommand, "PollID", DbType.Int32, pollId);
            db.AddInParameter(dbCommand, "Name", DbType.String, name);
            db.AddInParameter(dbCommand, "Count", DbType.Int32, count);
            db.AddInParameter(dbCommand, "DisplayOrder", DbType.Int32, displayOrder);
            if (db.ExecuteNonQuery(dbCommand) > 0)
            {
                int pollAnswerId = Convert.ToInt32(db.GetParameterValue(dbCommand, "@PollAnswerID"));
                item = GetPollAnswerById(pollAnswerId);
            }
            return item;
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
        public override DBPollAnswer UpdatePollAnswer(int pollAnswerId,
            int pollId, string name, int count, int displayOrder)
        {
            DBPollAnswer item = null;
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_PollAnswerUpdate");
            db.AddInParameter(dbCommand, "PollAnswerID", DbType.Int32, pollAnswerId);
            db.AddInParameter(dbCommand, "PollID", DbType.Int32, pollId);
            db.AddInParameter(dbCommand, "Name", DbType.String, name);
            db.AddInParameter(dbCommand, "Count", DbType.Int32, count);
            db.AddInParameter(dbCommand, "DisplayOrder", DbType.Int32, displayOrder);
            if (db.ExecuteNonQuery(dbCommand) > 0)
                item = GetPollAnswerById(pollAnswerId);

            return item;
        }

        /// <summary>
        /// Creates a poll voting record
        /// </summary>
        /// <param name="pollAnswerId">The poll answer identifier</param>
        /// <param name="customerId">Customer identifer</param>
        public override void CreatePollVotingRecord(int pollAnswerId, int customerId)
        {
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_PollVotingRecordCreate");
            db.AddInParameter(dbCommand, "PollAnswerID", DbType.Int32, pollAnswerId);
            db.AddInParameter(dbCommand, "CustomerID", DbType.Int32, customerId);
            db.ExecuteNonQuery(dbCommand);
        }

        #endregion
    }
}
