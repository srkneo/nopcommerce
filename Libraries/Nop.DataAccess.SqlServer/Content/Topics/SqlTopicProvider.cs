//------------------------------------------------------------------------------
// The contents of this file are title to the nopCommerce Public License Version 1.0 ("License"); you may not use this file except in compliance with the License.
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

namespace NopSolutions.NopCommerce.DataAccess.Content.Topics
{
    /// <summary>
    /// Topic provider for SQL Server
    /// </summary>
    public partial class SqlTopicProvider : DBTopicProvider
    {
        #region Fields
        private string _sqlConnectionString;
        #endregion

        #region Utilities
        private DBTopic GetTopicFromReader(IDataReader dataReader)
        {
            var item = new DBTopic();
            item.TopicId = NopSqlDataHelper.GetInt(dataReader, "TopicID");
            item.Name = NopSqlDataHelper.GetString(dataReader, "Name");
            return item;
        }

        private DBLocalizedTopic GetLocalizedTopicFromReader(IDataReader dataReader)
        {
            var item = new DBLocalizedTopic();
            item.TopicLocalizedId = NopSqlDataHelper.GetInt(dataReader, "TopicLocalizedID");
            item.TopicId = NopSqlDataHelper.GetInt(dataReader, "TopicID");
            item.LanguageId = NopSqlDataHelper.GetInt(dataReader, "LanguageID");
            item.Title = NopSqlDataHelper.GetString(dataReader, "Title");
            item.Body = NopSqlDataHelper.GetString(dataReader, "Body");
            item.CreatedOn = NopSqlDataHelper.GetUtcDateTime(dataReader, "CreatedOn");
            item.UpdatedOn = NopSqlDataHelper.GetUtcDateTime(dataReader, "UpdatedOn");
            item.MetaDescription = NopSqlDataHelper.GetString(dataReader, "MetaDescription");
            item.MetaKeywords = NopSqlDataHelper.GetString(dataReader, "MetaKeywords");
            item.MetaTitle = NopSqlDataHelper.GetString(dataReader, "MetaTitle");
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
        /// Deletes a topic
        /// </summary>
        /// <param name="topicId">Topic identifier</param>
        public override void DeleteTopic(int topicId)
        {
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_TopicDelete");
            db.AddInParameter(dbCommand, "TopicID", DbType.Int32, topicId);
            db.ExecuteNonQuery(dbCommand);
        }

        /// <summary>
        /// Gets a topic by template identifier
        /// </summary>
        /// <param name="topicId">Topic identifier</param>
        /// <returns>Topic</returns>
        public override DBTopic GetTopicById(int topicId)
        {
            DBTopic item = null;
            if (topicId == 0)
                return item;
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_TopicLoadByPrimaryKey");
            db.AddInParameter(dbCommand, "TopicID", DbType.Int32, topicId);
            using (IDataReader dataReader = db.ExecuteReader(dbCommand))
            {
                if (dataReader.Read())
                {
                    item = GetTopicFromReader(dataReader);
                }
            }
            return item;
        }

        /// <summary>
        /// Inserts a topic
        /// </summary>
        /// <param name="name">The name</param>
        /// <returns>Topic</returns>
        public override DBTopic InsertTopic(string name)
        {
            DBTopic item = null;
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_TopicInsert");
            db.AddOutParameter(dbCommand, "TopicID", DbType.Int32, 0);
            db.AddInParameter(dbCommand, "Name", DbType.String, name);
            if (db.ExecuteNonQuery(dbCommand) > 0)
            {
                int topicId = Convert.ToInt32(db.GetParameterValue(dbCommand, "@TopicID"));
                item = GetTopicById(topicId);
            }
            return item;
        }

        /// <summary>
        /// Updates the topic
        /// </summary>
        /// <param name="topicId">The topic identifier</param>
        /// <param name="name">The name</param>
        /// <returns>Topic</returns>
        public override DBTopic UpdateTopic(int topicId, string name)
        {
            DBTopic item = null;
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_TopicUpdate");
            db.AddInParameter(dbCommand, "TopicID", DbType.Int32, topicId);
            db.AddInParameter(dbCommand, "Name", DbType.String, name);
            if (db.ExecuteNonQuery(dbCommand) > 0)
                item = GetTopicById(topicId);

            return item;
        }

        /// <summary>
        /// Gets all topics
        /// </summary>
        /// <returns>Topic collection</returns>
        public override DBTopicCollection GetAllTopics()
        {
            var result = new DBTopicCollection();
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_TopicLoadAll");
            using (IDataReader dataReader = db.ExecuteReader(dbCommand))
            {
                while (dataReader.Read())
                {
                    var item = GetTopicFromReader(dataReader);
                    result.Add(item);
                }
            }
            return result;
        }

        /// <summary>
        /// Gets a localized topic by identifier
        /// </summary>
        /// <param name="localizedTopicId">Localized topic identifier</param>
        /// <returns>Localized topic</returns>
        public override DBLocalizedTopic GetLocalizedTopicById(int localizedTopicId)
        {
            DBLocalizedTopic item = null;
            if (localizedTopicId == 0)
                return item;
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_TopicLocalizedLoadByPrimaryKey");
            db.AddInParameter(dbCommand, "TopicLocalizedID", DbType.Int32, localizedTopicId);
            using (IDataReader dataReader = db.ExecuteReader(dbCommand))
            {
                if (dataReader.Read())
                {
                    item = GetLocalizedTopicFromReader(dataReader);
                }
            }
            return item;
        }

        /// <summary>
        /// Gets a localized topic by parent topic identifer and language identifier
        /// </summary>
        /// <param name="topicId">The topic identifier</param>
        /// <param name="languageId">Language identifier</param>
        /// <returns>Localized topic</returns>
        public override DBLocalizedTopic GetLocalizedTopic(int topicId, int languageId)
        {
            DBLocalizedTopic item = null;
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_TopicLocalizedLoadByTopicIDAndLanguageID");
            db.AddInParameter(dbCommand, "TopicID", DbType.Int32, topicId);
            db.AddInParameter(dbCommand, "LanguageID", DbType.Int32, languageId);
            using (IDataReader dataReader = db.ExecuteReader(dbCommand))
            {
                if (dataReader.Read())
                {
                    item = GetLocalizedTopicFromReader(dataReader);
                }
            }
            return item;
        }

        /// <summary>
        /// Gets a localized topic by name and language identifier
        /// </summary>
        /// <param name="name">Topic name</param>
        /// <param name="languageId">Language identifier</param>
        /// <returns>Localized topic</returns>
        public override DBLocalizedTopic GetLocalizedTopic(string name, int languageId)
        {
            DBLocalizedTopic item = null;
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_TopicLocalizedLoadByNameAndLanguageID");
            db.AddInParameter(dbCommand, "Name", DbType.String, name);
            db.AddInParameter(dbCommand, "LanguageID", DbType.Int32, languageId);
            using (IDataReader dataReader = db.ExecuteReader(dbCommand))
            {
                if (dataReader.Read())
                {
                    item = GetLocalizedTopicFromReader(dataReader);
                }
            }
            return item;
        }

        /// <summary>
        /// Deletes a localized topic
        /// </summary>
        /// <param name="localizedTopicId">Topic identifier</param>
        public override void DeleteLocalizedTopic(int localizedTopicId)
        {
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_TopicLocalizedDelete");
            db.AddInParameter(dbCommand, "TopicLocalizedID", DbType.Int32, localizedTopicId);
            db.ExecuteNonQuery(dbCommand);
        }

        /// <summary>
        /// Gets all localized topics
        /// </summary>
        /// <param name="topicName">Topic name</param>
        /// <returns>Localized topic collection</returns>
        public override DBLocalizedTopicCollection GetAllLocalizedTopics(string topicName)
        {
            var result = new DBLocalizedTopicCollection();
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_TopicLocalizedLoadAllByName");
            db.AddInParameter(dbCommand, "Name", DbType.String, topicName);
            using (IDataReader dataReader = db.ExecuteReader(dbCommand))
            {
                while (dataReader.Read())
                {
                    var item = GetLocalizedTopicFromReader(dataReader);
                    result.Add(item);
                }
            }
            return result;
        }

        /// <summary>
        /// Inserts a localized topic
        /// </summary>
        /// <param name="topicId">The topic identifier</param>
        /// <param name="languageId">The language identifier</param>
        /// <param name="title">The title</param>
        /// <param name="body">The body</param>
        /// <param name="createdOn">The date and time of instance creation</param>
        /// <param name="updatedOn">The date and time of instance update</param>
        /// <param name="metaKeywords">The meta keywords</param>
        /// <param name="metaDescription">The meta description</param>
        /// <param name="metaTitle">The meta title</param>
        /// <returns>Localized topic</returns>
        public override DBLocalizedTopic InsertLocalizedTopic(int topicId,
            int languageId, string title, string body,
            DateTime createdOn, DateTime updatedOn,
            string metaKeywords, string metaDescription, string metaTitle)
        {
            DBLocalizedTopic item = null;
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_TopicLocalizedInsert");
            db.AddOutParameter(dbCommand, "TopicLocalizedID", DbType.Int32, 0);
            db.AddInParameter(dbCommand, "TopicID", DbType.Int32, topicId);
            db.AddInParameter(dbCommand, "LanguageID", DbType.Int32, languageId);
            db.AddInParameter(dbCommand, "Title", DbType.String, title);
            db.AddInParameter(dbCommand, "Body", DbType.String, body);
            db.AddInParameter(dbCommand, "CreatedOn", DbType.DateTime, createdOn);
            db.AddInParameter(dbCommand, "UpdatedOn", DbType.DateTime, updatedOn);
            db.AddInParameter(dbCommand, "MetaKeywords", DbType.String, metaKeywords);
            db.AddInParameter(dbCommand, "MetaDescription", DbType.String, metaDescription);
            db.AddInParameter(dbCommand, "MetaTitle", DbType.String, metaTitle);

            if (db.ExecuteNonQuery(dbCommand) > 0)
            {
                int topicLocalizedId = Convert.ToInt32(db.GetParameterValue(dbCommand, "@TopicLocalizedID"));
                item = GetLocalizedTopicById(topicLocalizedId);
            }
            return item;
        }

        /// <summary>
        /// Updates the localized topic
        /// </summary>
        /// <param name="topicLocalizedId">The localized topic identifier</param>
        /// <param name="topicId">The topic identifier</param>
        /// <param name="languageId">The language identifier</param>
        /// <param name="title">The title</param>
        /// <param name="body">The body</param>
        /// <param name="createdOn">The date and time of instance creation</param>
        /// <param name="updatedOn">The date and time of instance update</param>
        /// <param name="metaKeywords">The meta keywords</param>
        /// <param name="metaDescription">The meta description</param>
        /// <param name="metaTitle">The meta title</param>
        /// <returns>Localized topic</returns>
        public override DBLocalizedTopic UpdateLocalizedTopic(int topicLocalizedId,
            int topicId, int languageId, string title, string body,
            DateTime createdOn, DateTime updatedOn,
            string metaKeywords, string metaDescription, string metaTitle)
        {
            DBLocalizedTopic item = null;
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_TopicLocalizedUpdate");
            db.AddInParameter(dbCommand, "TopicLocalizedID", DbType.Int32, topicLocalizedId);
            db.AddInParameter(dbCommand, "TopicID", DbType.Int32, topicId);
            db.AddInParameter(dbCommand, "LanguageID", DbType.Int32, languageId);
            db.AddInParameter(dbCommand, "Title", DbType.String, title);
            db.AddInParameter(dbCommand, "Body", DbType.String, body);
            db.AddInParameter(dbCommand, "CreatedOn", DbType.DateTime, createdOn);
            db.AddInParameter(dbCommand, "UpdatedOn", DbType.DateTime, updatedOn);
            db.AddInParameter(dbCommand, "MetaKeywords", DbType.String, metaKeywords);
            db.AddInParameter(dbCommand, "MetaDescription", DbType.String, metaDescription);
            db.AddInParameter(dbCommand, "MetaTitle", DbType.String, metaTitle);
            if (db.ExecuteNonQuery(dbCommand) > 0)
                item = GetLocalizedTopicById(topicLocalizedId);

            return item;
        }
        #endregion
    }
}

