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

namespace NopSolutions.NopCommerce.DataAccess.Content.Forums
{
    /// <summary>
    /// Forum provider for SQL Server
    /// </summary>
    public partial class SqlForumProvider : DBForumProvider
    {
        #region Fields
        private string _sqlConnectionString;
        #endregion

        #region Utilities
        private DBForumGroup GetForumGroupFromReader(IDataReader dataReader)
        {
            var item = new DBForumGroup();
            item.ForumGroupId = NopSqlDataHelper.GetInt(dataReader, "ForumGroupID");
            item.Name = NopSqlDataHelper.GetString(dataReader, "Name");
            item.Description = NopSqlDataHelper.GetString(dataReader, "Description");
            item.DisplayOrder = NopSqlDataHelper.GetInt(dataReader, "DisplayOrder");
            item.CreatedOn = NopSqlDataHelper.GetUtcDateTime(dataReader, "CreatedOn");
            item.UpdatedOn = NopSqlDataHelper.GetUtcDateTime(dataReader, "UpdatedOn");
            return item;
        }

        private DBForum GetForumFromReader(IDataReader dataReader)
        {
            var item = new DBForum();
            item.ForumId = NopSqlDataHelper.GetInt(dataReader, "ForumID");
            item.ForumGroupId = NopSqlDataHelper.GetInt(dataReader, "ForumGroupID");
            item.Name = NopSqlDataHelper.GetString(dataReader, "Name");
            item.Description = NopSqlDataHelper.GetString(dataReader, "Description");
            item.NumTopics = NopSqlDataHelper.GetInt(dataReader, "NumTopics");
            item.NumPosts = NopSqlDataHelper.GetInt(dataReader, "NumPosts");
            item.LastTopicId = NopSqlDataHelper.GetInt(dataReader, "LastTopicID");
            item.LastPostId = NopSqlDataHelper.GetInt(dataReader, "LastPostID");
            item.LastPostUserId = NopSqlDataHelper.GetInt(dataReader, "LastPostUserID");
            item.LastPostTime = NopSqlDataHelper.GetNullableUtcDateTime(dataReader, "LastPostTime");
            item.DisplayOrder = NopSqlDataHelper.GetInt(dataReader, "DisplayOrder");
            item.CreatedOn = NopSqlDataHelper.GetUtcDateTime(dataReader, "CreatedOn");
            item.UpdatedOn = NopSqlDataHelper.GetUtcDateTime(dataReader, "UpdatedOn");
            return item;
        }

        private DBForumTopic GetForumTopicFromReader(IDataReader dataReader)
        {
            var item = new DBForumTopic();
            item.ForumTopicId = NopSqlDataHelper.GetInt(dataReader, "TopicID");
            item.ForumId = NopSqlDataHelper.GetInt(dataReader, "ForumID");
            item.UserId = NopSqlDataHelper.GetInt(dataReader, "UserID");
            item.TopicTypeId = NopSqlDataHelper.GetInt(dataReader, "TopicTypeID");
            item.Subject = NopSqlDataHelper.GetString(dataReader, "Subject");
            item.NumPosts = NopSqlDataHelper.GetInt(dataReader, "NumPosts");
            item.Views = NopSqlDataHelper.GetInt(dataReader, "Views");
            item.LastPostId = NopSqlDataHelper.GetInt(dataReader, "LastPostID");
            item.LastPostUserId = NopSqlDataHelper.GetInt(dataReader, "LastPostUserID");
            item.LastPostTime = NopSqlDataHelper.GetNullableUtcDateTime(dataReader, "LastPostTime");
            item.CreatedOn = NopSqlDataHelper.GetUtcDateTime(dataReader, "CreatedOn");
            item.UpdatedOn = NopSqlDataHelper.GetUtcDateTime(dataReader, "UpdatedOn");
            return item;
        }

        private DBForumPost GetForumPostFromReader(IDataReader dataReader)
        {
            var item = new DBForumPost();
            item.ForumPostId = NopSqlDataHelper.GetInt(dataReader, "PostID");
            item.TopicId = NopSqlDataHelper.GetInt(dataReader, "TopicID");
            item.UserId = NopSqlDataHelper.GetInt(dataReader, "UserID");
            item.Text = NopSqlDataHelper.GetString(dataReader, "Text");
            item.IPAddress = NopSqlDataHelper.GetString(dataReader, "IPAddress");
            item.CreatedOn = NopSqlDataHelper.GetUtcDateTime(dataReader, "CreatedOn");
            item.UpdatedOn = NopSqlDataHelper.GetUtcDateTime(dataReader, "UpdatedOn");
            return item;
        }

        private DBPrivateMessage GetPrivateMessageFromReader(IDataReader dataReader)
        {
            var item = new DBPrivateMessage();
            item.PrivateMessageId = NopSqlDataHelper.GetInt(dataReader, "PrivateMessageID");
            item.FromUserId = NopSqlDataHelper.GetInt(dataReader, "FromUserID");
            item.ToUserId = NopSqlDataHelper.GetInt(dataReader, "ToUserID");
            item.Subject = NopSqlDataHelper.GetString(dataReader, "Subject");
            item.Text = NopSqlDataHelper.GetString(dataReader, "Text");
            item.IsRead = NopSqlDataHelper.GetBoolean(dataReader, "IsRead");
            item.IsDeletedByAuthor = NopSqlDataHelper.GetBoolean(dataReader, "IsDeletedByAuthor");
            item.IsDeletedByRecipient = NopSqlDataHelper.GetBoolean(dataReader, "IsDeletedByRecipient");
            item.CreatedOn = NopSqlDataHelper.GetUtcDateTime(dataReader, "CreatedOn");
            return item;
        }

        private DBForumSubscription GetForumSubscriptionFromReader(IDataReader dataReader)
        {
            var item = new DBForumSubscription();
            item.ForumSubscriptionId = NopSqlDataHelper.GetInt(dataReader, "SubscriptionID");
            item.SubscriptionGuid = NopSqlDataHelper.GetGuid(dataReader, "SubscriptionGUID");
            item.UserId = NopSqlDataHelper.GetInt(dataReader, "UserID");
            item.ForumId = NopSqlDataHelper.GetInt(dataReader, "ForumID");
            item.TopicId = NopSqlDataHelper.GetInt(dataReader, "TopicID");
            item.CreatedOn = NopSqlDataHelper.GetUtcDateTime(dataReader, "CreatedOn");
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
        /// Update forum stats
        /// </summary>
        /// <param name="forumId">The forum identifier</param>
        public override void UpdateForumStats(int forumId)
        {
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_Forums_ForumUpdateCounts");
            db.AddInParameter(dbCommand, "ForumID", DbType.Int32, forumId);
            db.ExecuteNonQuery(dbCommand);
        }

        /// <summary>
        /// Deletes a forum
        /// </summary>
        /// <param name="forumId">The forum identifier</param>
        public override void DeleteForum(int forumId)
        {
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_Forums_ForumDelete");
            db.AddInParameter(dbCommand, "ForumID", DbType.Int32, forumId);
            db.ExecuteNonQuery(dbCommand);
        }

        /// <summary>
        /// Gets a topic
        /// </summary>
        /// <param name="forumTopicId">The topic identifier</param>
        /// <param name="increaseViews">The value indicating whether to increase topic views</param>
        /// <returns>Topic</returns>
        public override DBForumTopic GetTopicById(int forumTopicId, bool increaseViews)
        {
            DBForumTopic item = null;
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_Forums_TopicLoadByPrimaryKey");
            db.AddInParameter(dbCommand, "TopicID", DbType.Int32, forumTopicId);
            db.AddInParameter(dbCommand, "IncreaseViews", DbType.Boolean, increaseViews);
            using (IDataReader dataReader = db.ExecuteReader(dbCommand))
            {
                if (dataReader.Read())
                {
                    item = GetForumTopicFromReader(dataReader);
                }
            }
            return item;
        }
        
        /// <summary>
        /// Gets all topics
        /// </summary>
        /// <param name="forumId">The forum group identifier</param>
        /// <param name="userId">The user identifier</param>
        /// <param name="keywords">Keywords</param>
        /// <param name="searchPosts">A value indicating whether to search in posts</param>
        /// <param name="pageSize">Page size</param>
        /// <param name="pageIndex">Page index</param>
        /// <param name="totalRecords">Total records</param>
        /// <returns>Topics</returns>
        public override DBForumTopicCollection GetAllTopics(int forumId,
            int userId, string keywords, bool searchPosts, int pageSize,
            int pageIndex, out int totalRecords)
        {
            totalRecords = 0;
            var result = new DBForumTopicCollection();
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_Forums_TopicLoadAll");
            db.AddInParameter(dbCommand, "ForumID", DbType.Int32, forumId);
            db.AddInParameter(dbCommand, "UserID", DbType.Int32, userId);
            db.AddInParameter(dbCommand, "Keywords", DbType.String, keywords);
            db.AddInParameter(dbCommand, "SearchPosts", DbType.Boolean, searchPosts);
            db.AddInParameter(dbCommand, "PageSize", DbType.Int32, pageSize);
            db.AddInParameter(dbCommand, "PageIndex", DbType.Int32, pageIndex);
            db.AddOutParameter(dbCommand, "TotalRecords", DbType.Int32, 0);
            using (IDataReader dataReader = db.ExecuteReader(dbCommand))
            {
                while (dataReader.Read())
                {
                    var item = GetForumTopicFromReader(dataReader);
                    result.Add(item);
                }
            }
            totalRecords = Convert.ToInt32(db.GetParameterValue(dbCommand, "@TotalRecords"));

            return result;
        }

        /// <summary>
        /// Gets active topics
        /// </summary>
        /// <param name="forumId">The forum group identifier</param>
        /// <param name="topicCount">Topic count. 0 if you want to get all topics</param>
        /// <returns>Topics</returns>
        public override DBForumTopicCollection GetActiveTopics(int forumId, int topicCount)
        {
            var result = new DBForumTopicCollection();
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_Forums_TopicLoadActive");
            db.AddInParameter(dbCommand, "ForumID", DbType.Int32, forumId);
            db.AddInParameter(dbCommand, "TopicCount", DbType.Int32, topicCount);
            using (IDataReader dataReader = db.ExecuteReader(dbCommand))
            {
                while (dataReader.Read())
                {
                    var item = GetForumTopicFromReader(dataReader);
                    result.Add(item);
                }
            }
            return result;
        }
        
        /// <summary>
        /// Gets all posts
        /// </summary>
        /// <param name="forumTopicId">The forum topic identifier</param>
        /// <param name="userId">The user identifier</param>
        /// <param name="keywords">Keywords</param>
        /// <param name="ascSort">Sort order</param>
        /// <param name="pageSize">Page size</param>
        /// <param name="pageIndex">Page index</param>
        /// <param name="totalRecords">Total records</param>
        /// <returns>Posts</returns>
        public override DBForumPostCollection GetAllPosts(int forumTopicId, int userId,
            string keywords, bool ascSort, int pageSize, int pageIndex, out int totalRecords)
        {
            totalRecords = 0;
            var result = new DBForumPostCollection();
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_Forums_PostLoadAll");
            db.AddInParameter(dbCommand, "TopicID", DbType.Int32, forumTopicId);
            db.AddInParameter(dbCommand, "UserID", DbType.Int32, userId);
            db.AddInParameter(dbCommand, "Keywords", DbType.String, keywords);
            db.AddInParameter(dbCommand, "AscSort", DbType.Boolean, ascSort);
            db.AddInParameter(dbCommand, "PageSize", DbType.Int32, pageSize);
            db.AddInParameter(dbCommand, "PageIndex", DbType.Int32, pageIndex);
            db.AddOutParameter(dbCommand, "TotalRecords", DbType.Int32, 0);
            using (IDataReader dataReader = db.ExecuteReader(dbCommand))
            {
                while (dataReader.Read())
                {
                    var item = GetForumPostFromReader(dataReader);
                    result.Add(item);
                }
            }
            totalRecords = Convert.ToInt32(db.GetParameterValue(dbCommand, "@TotalRecords"));

            return result;
        }

        /// <summary>
        /// Gets private messages
        /// </summary>
        /// <param name="fromUserId">The user identifier who sent the message</param>
        /// <param name="toUserId">The user identifier who should receive the message</param>
        /// <param name="isRead">A value indicating whether loaded messages are read. false - to load not read messages only, 1 to load read messages only, null to load all messages</param>
        /// <param name="isDeletedByAuthor">A value indicating whether loaded messages are deleted by author. false - messages are not deleted by author, null to load all messages</param>
        /// <param name="isDeletedByRecipient">A value indicating whether loaded messages are deleted by recipient. false - messages are not deleted by recipient, null to load all messages</param>
        /// <param name="keywords">Keywords</param>
        /// <param name="pageSize">Page size</param>
        /// <param name="pageIndex">Page index</param>
        /// <param name="totalRecords">Total records</param>
        /// <returns>Private messages</returns>
        public override DBPrivateMessageCollection GetAllPrivateMessages(int fromUserId,
            int toUserId, bool? isRead, bool? isDeletedByAuthor, bool? isDeletedByRecipient,
            string keywords, int pageSize, int pageIndex, out int totalRecords)
        {
            totalRecords = 0;
            var result = new DBPrivateMessageCollection();
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_Forums_PrivateMessageLoadAll");
            db.AddInParameter(dbCommand, "FromUserID", DbType.Int32, fromUserId);
            db.AddInParameter(dbCommand, "ToUserID", DbType.Int32, toUserId);
            if (isRead.HasValue)
                db.AddInParameter(dbCommand, "IsRead", DbType.Boolean, isRead.Value);
            else
                db.AddInParameter(dbCommand, "IsRead", DbType.Boolean, null);
            if (isDeletedByAuthor.HasValue)
                db.AddInParameter(dbCommand, "IsDeletedByAuthor", DbType.Boolean, isDeletedByAuthor.Value);
            else
                db.AddInParameter(dbCommand, "IsDeletedByAuthor", DbType.Boolean, null);
            if (isDeletedByRecipient.HasValue)
                db.AddInParameter(dbCommand, "IsDeletedByRecipient", DbType.Boolean, isDeletedByRecipient.Value);
            else
                db.AddInParameter(dbCommand, "IsDeletedByRecipient", DbType.Boolean, null);
            db.AddInParameter(dbCommand, "Keywords", DbType.String, keywords);
            db.AddInParameter(dbCommand, "PageSize", DbType.Int32, pageSize);
            db.AddInParameter(dbCommand, "PageIndex", DbType.Int32, pageIndex);
            db.AddOutParameter(dbCommand, "TotalRecords", DbType.Int32, 0);
            using (IDataReader dataReader = db.ExecuteReader(dbCommand))
            {
                while (dataReader.Read())
                {
                    var item = GetPrivateMessageFromReader(dataReader);
                    result.Add(item);
                }
            }
            totalRecords = Convert.ToInt32(db.GetParameterValue(dbCommand, "@TotalRecords"));

            return result;
        }

        /// <summary>
        /// Gets forum subscriptions
        /// </summary>
        /// <param name="userId">The user identifier</param>
        /// <param name="forumId">The forum identifier</param>
        /// <param name="topicId">The topic identifier</param>
        /// <param name="pageSize">Page size</param>
        /// <param name="pageIndex">Page index</param>
        /// <param name="totalRecords">Total records</param>
        /// <returns>Forum subscriptions</returns>
        public override DBForumSubscriptionCollection GetAllSubscriptions(int userId,
            int forumId, int topicId, int pageSize, int pageIndex, out int totalRecords)
        {
            totalRecords = 0;
            var result = new DBForumSubscriptionCollection();
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_Forums_SubscriptionLoadAll");
            db.AddInParameter(dbCommand, "UserID", DbType.Int32, userId);
            db.AddInParameter(dbCommand, "ForumID", DbType.Int32, forumId);
            db.AddInParameter(dbCommand, "TopicID", DbType.Int32, topicId);
            db.AddInParameter(dbCommand, "PageSize", DbType.Int32, pageSize);
            db.AddInParameter(dbCommand, "PageIndex", DbType.Int32, pageIndex);
            db.AddOutParameter(dbCommand, "TotalRecords", DbType.Int32, 0);
            using (IDataReader dataReader = db.ExecuteReader(dbCommand))
            {
                while (dataReader.Read())
                {
                    var item = GetForumSubscriptionFromReader(dataReader);
                    result.Add(item);
                }
            }
            totalRecords = Convert.ToInt32(db.GetParameterValue(dbCommand, "@TotalRecords"));

            return result;
        }

        #endregion
    }
}
