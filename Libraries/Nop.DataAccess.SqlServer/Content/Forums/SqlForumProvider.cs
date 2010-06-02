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
        /// Deletes a forum group
        /// </summary>
        /// <param name="forumGroupId">The forum group identifier</param>
        public override void DeleteForumGroup(int forumGroupId)
        {
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_Forums_GroupDelete");
            db.AddInParameter(dbCommand, "ForumGroupID", DbType.Int32, forumGroupId);
            db.ExecuteNonQuery(dbCommand);
        }

        /// <summary>
        /// Gets a forum group
        /// </summary>
        /// <param name="forumGroupId">The forum group identifier</param>
        /// <returns>Forum group</returns>
        public override DBForumGroup GetForumGroupById(int forumGroupId)
        {
            DBForumGroup item = null;
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_Forums_GroupLoadByPrimaryKey");
            db.AddInParameter(dbCommand, "ForumGroupID", DbType.Int32, forumGroupId);
            using (IDataReader dataReader = db.ExecuteReader(dbCommand))
            {
                if (dataReader.Read())
                {
                    item = GetForumGroupFromReader(dataReader);
                }
            }
            return item;
        }

        /// <summary>
        /// Gets all forum groups
        /// </summary>
        /// <returns>Forum groups</returns>
        public override DBForumGroupCollection GetAllForumGroups()
        {
            var result = new DBForumGroupCollection();
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_Forums_GroupLoadAll");
            using (IDataReader dataReader = db.ExecuteReader(dbCommand))
            {
                while (dataReader.Read())
                {
                    var item = GetForumGroupFromReader(dataReader);
                    result.Add(item);
                }
            }

            return result;
        }

        /// <summary>
        /// Inserts a forum group
        /// </summary>
        /// <param name="name">The language name</param>
        /// <param name="description">The description</param>
        /// <param name="displayOrder">The display order</param>        
        /// <param name="createdOn">The date and time of instance creation</param>
        /// <param name="updatedOn">The date and time of instance update</param>
        /// <returns>Forum group</returns>
        public override DBForumGroup InsertForumGroup(string name, string description,
            int displayOrder, DateTime createdOn, DateTime updatedOn)
        {
            DBForumGroup item = null;
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_Forums_GroupInsert");
            db.AddOutParameter(dbCommand, "ForumGroupID", DbType.Int32, 0);
            db.AddInParameter(dbCommand, "Name", DbType.String, name);
            db.AddInParameter(dbCommand, "Description", DbType.String, description);
            db.AddInParameter(dbCommand, "DisplayOrder", DbType.Int32, displayOrder);
            db.AddInParameter(dbCommand, "CreatedOn", DbType.DateTime, createdOn);
            db.AddInParameter(dbCommand, "UpdatedOn", DbType.DateTime, updatedOn);
            if (db.ExecuteNonQuery(dbCommand) > 0)
            {
                int forumGroupId = Convert.ToInt32(db.GetParameterValue(dbCommand, "@ForumGroupID"));
                item = GetForumGroupById(forumGroupId);
            }
            return item;
        }

        /// <summary>
        /// Updates the forum group
        /// </summary>
        /// <param name="forumGroupId">The forum group identifier</param>
        /// <param name="name">The language name</param>
        /// <param name="description">The description</param>
        /// <param name="displayOrder">The display order</param>        
        /// <param name="createdOn">The date and time of instance creation</param>
        /// <param name="updatedOn">The date and time of instance update</param>
        /// <returns>Forum group</returns>
        public override DBForumGroup UpdateForumGroup(int forumGroupId,
            string name, string description, int displayOrder,
            DateTime createdOn, DateTime updatedOn)
        {
            DBForumGroup item = null;
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_Forums_GroupUpdate");
            db.AddInParameter(dbCommand, "ForumGroupID", DbType.Int32, forumGroupId);
            db.AddInParameter(dbCommand, "Name", DbType.String, name);
            db.AddInParameter(dbCommand, "Description", DbType.String, description);
            db.AddInParameter(dbCommand, "DisplayOrder", DbType.Int32, displayOrder);
            db.AddInParameter(dbCommand, "CreatedOn", DbType.DateTime, createdOn);
            db.AddInParameter(dbCommand, "UpdatedOn", DbType.DateTime, updatedOn);
            if (db.ExecuteNonQuery(dbCommand) > 0)
                item = GetForumGroupById(forumGroupId);

            return item;
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
        /// Gets a forum
        /// </summary>
        /// <param name="forumId">The forum identifier</param>
        /// <returns>Forum</returns>
        public override DBForum GetForumById(int forumId)
        {
            DBForum item = null;
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_Forums_ForumLoadByPrimaryKey");
            db.AddInParameter(dbCommand, "ForumID", DbType.Int32, forumId);
            using (IDataReader dataReader = db.ExecuteReader(dbCommand))
            {
                if (dataReader.Read())
                {
                    item = GetForumFromReader(dataReader);
                }
            }
            return item;
        }

        /// <summary>
        /// Gets forums by group identifier
        /// </summary>
        /// <param name="forumGroupId">The forum group identifier</param>
        /// <returns>Forums</returns>
        public override DBForumCollection GetAllForumsByGroupId(int forumGroupId)
        {
            var result = new DBForumCollection();
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_Forums_ForumLoadAllByForumGroupID");
            db.AddInParameter(dbCommand, "ForumGroupID", DbType.Int32, forumGroupId);
            using (IDataReader dataReader = db.ExecuteReader(dbCommand))
            {
                while (dataReader.Read())
                {
                    var item = GetForumFromReader(dataReader);
                    result.Add(item);
                }
            }
            return result;
        }

        /// <summary>
        /// Inserts a forum
        /// </summary>
        /// <param name="forumGroupId">The forum group identifier</param>
        /// <param name="name">The language name</param>
        /// <param name="description">The description</param>
        /// <param name="numTopics">The number of topics</param>
        /// <param name="numPosts">The number of posts</param>
        /// <param name="lastTopicId">The last topic identifier</param>
        /// <param name="lastPostId">The last post identifier</param>
        /// <param name="lastPostUserId">The last post user identifier</param>
        /// <param name="lastPostTime">The last post date and time</param>
        /// <param name="displayOrder">The display order</param>        
        /// <param name="createdOn">The date and time of instance creation</param>
        /// <param name="updatedOn">The date and time of instance update</param>
        /// <returns>Forum</returns>
        public override DBForum InsertForum(int forumGroupId,
            string name, string description,
            int numTopics, int numPosts, int lastTopicId, int lastPostId,
            int lastPostUserId, DateTime? lastPostTime, int displayOrder,
            DateTime createdOn, DateTime updatedOn)
        {
            DBForum item = null;
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_Forums_ForumInsert");
            db.AddOutParameter(dbCommand, "ForumID", DbType.Int32, 0);
            db.AddInParameter(dbCommand, "ForumGroupID", DbType.Int32, forumGroupId);
            db.AddInParameter(dbCommand, "Name", DbType.String, name);
            db.AddInParameter(dbCommand, "Description", DbType.String, description);
            db.AddInParameter(dbCommand, "NumTopics", DbType.Int32, numTopics);
            db.AddInParameter(dbCommand, "NumPosts", DbType.Int32, numPosts);
            db.AddInParameter(dbCommand, "LastTopicID", DbType.Int32, lastTopicId);
            db.AddInParameter(dbCommand, "LastPostID", DbType.Int32, lastPostId);
            db.AddInParameter(dbCommand, "LastPostUserID", DbType.Int32, lastPostUserId);
            if (lastPostTime.HasValue)
                db.AddInParameter(dbCommand, "LastPostTime", DbType.DateTime, lastPostTime.Value);
            else
                db.AddInParameter(dbCommand, "LastPostTime", DbType.DateTime, null);
            db.AddInParameter(dbCommand, "DisplayOrder", DbType.Int32, displayOrder);
            db.AddInParameter(dbCommand, "CreatedOn", DbType.DateTime, createdOn);
            db.AddInParameter(dbCommand, "UpdatedOn", DbType.DateTime, updatedOn);
            if (db.ExecuteNonQuery(dbCommand) > 0)
            {
                int forumId = Convert.ToInt32(db.GetParameterValue(dbCommand, "@ForumID"));
                item = GetForumById(forumId);
            }
            return item;
        }

        /// <summary>
        /// Updates the forum
        /// </summary>
        /// <param name="forumId">The forum identifier</param>
        /// <param name="forumGroupId">The forum group identifier</param>
        /// <param name="name">The language name</param>
        /// <param name="description">The description</param>
        /// <param name="numTopics">The number of topics</param>
        /// <param name="numPosts">The number of posts</param>
        /// <param name="lastTopicId">The last topic identifier</param>
        /// <param name="lastPostId">The last post identifier</param>
        /// <param name="lastPostUserId">The last post user identifier</param>
        /// <param name="lastPostTime">The last post date and time</param>
        /// <param name="displayOrder">The display order</param>        
        /// <param name="createdOn">The date and time of instance creation</param>
        /// <param name="updatedOn">The date and time of instance update</param>
        /// <returns>Forum</returns>
        public override DBForum UpdateForum(int forumId,
            int forumGroupId, string name, string description,
            int numTopics, int numPosts, int lastTopicId, int lastPostId,
            int lastPostUserId, DateTime? lastPostTime, int displayOrder,
            DateTime createdOn, DateTime updatedOn)
        {
            DBForum item = null;
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_Forums_ForumUpdate");
            db.AddInParameter(dbCommand, "ForumID", DbType.Int32, forumId);
            db.AddInParameter(dbCommand, "ForumGroupID", DbType.Int32, forumGroupId);
            db.AddInParameter(dbCommand, "Name", DbType.String, name);
            db.AddInParameter(dbCommand, "Description", DbType.String, description);
            db.AddInParameter(dbCommand, "NumTopics", DbType.Int32, numTopics);
            db.AddInParameter(dbCommand, "NumPosts", DbType.Int32, numPosts);
            db.AddInParameter(dbCommand, "LastTopicID", DbType.Int32, lastTopicId);
            db.AddInParameter(dbCommand, "LastPostID", DbType.Int32, lastPostId);
            db.AddInParameter(dbCommand, "LastPostUserID", DbType.Int32, lastPostUserId);
            if (lastPostTime.HasValue)
                db.AddInParameter(dbCommand, "LastPostTime", DbType.DateTime, lastPostTime.Value);
            else
                db.AddInParameter(dbCommand, "LastPostTime", DbType.DateTime, null);
            db.AddInParameter(dbCommand, "DisplayOrder", DbType.Int32, displayOrder);
            db.AddInParameter(dbCommand, "CreatedOn", DbType.DateTime, createdOn);
            db.AddInParameter(dbCommand, "UpdatedOn", DbType.DateTime, updatedOn);
            if (db.ExecuteNonQuery(dbCommand) > 0)
                item = GetForumById(forumId);

            return item;
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
        /// Deletes a topic
        /// </summary>
        /// <param name="forumTopicId">The topic identifier</param>
        public override void DeleteTopic(int forumTopicId)
        {
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_Forums_TopicDelete");
            db.AddInParameter(dbCommand, "TopicID", DbType.Int32, forumTopicId);
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
        /// Inserts a topic
        /// </summary>
        /// <param name="forumId">The forum identifier</param>
        /// <param name="userId">The user identifier</param>
        /// <param name="topicTypeId">The topic type identifier</param>
        /// <param name="subject">The subject</param>
        /// <param name="numPosts">The number of posts</param>
        /// <param name="views">The number of views</param>
        /// <param name="lastPostId">The last post identifier</param>
        /// <param name="lastPostUserId">The last post user identifier</param>
        /// <param name="lastPostTime">The last post date and time</param>
        /// <param name="createdOn">The date and time of instance creation</param>
        /// <param name="updatedOn">The date and time of instance update</param>
        /// <returns>Topic</returns>
        public override DBForumTopic InsertTopic(int forumId, int userId,
            int topicTypeId, string subject,
            int numPosts, int views, int lastPostId,
            int lastPostUserId, DateTime? lastPostTime,
            DateTime createdOn, DateTime updatedOn)
        {
            DBForumTopic item = null;
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_Forums_TopicInsert");
            db.AddOutParameter(dbCommand, "TopicID", DbType.Int32, 0);
            db.AddInParameter(dbCommand, "ForumID", DbType.Int32, forumId);
            db.AddInParameter(dbCommand, "UserID", DbType.Int32, userId);
            db.AddInParameter(dbCommand, "TopicTypeID", DbType.Int32, topicTypeId);
            db.AddInParameter(dbCommand, "Subject", DbType.String, subject);
            db.AddInParameter(dbCommand, "NumPosts", DbType.Int32, numPosts);
            db.AddInParameter(dbCommand, "Views", DbType.Int32, views);
            db.AddInParameter(dbCommand, "LastPostID", DbType.Int32, lastPostId);
            db.AddInParameter(dbCommand, "LastPostUserID", DbType.Int32, lastPostUserId);
            if (lastPostTime.HasValue)
                db.AddInParameter(dbCommand, "LastPostTime", DbType.DateTime, lastPostTime.Value);
            else
                db.AddInParameter(dbCommand, "LastPostTime", DbType.DateTime, null);
            db.AddInParameter(dbCommand, "CreatedOn", DbType.DateTime, createdOn);
            db.AddInParameter(dbCommand, "UpdatedOn", DbType.DateTime, updatedOn);
            if (db.ExecuteNonQuery(dbCommand) > 0)
            {
                int topicId = Convert.ToInt32(db.GetParameterValue(dbCommand, "@TopicID"));
                item = GetTopicById(topicId, false);
            }
            return item;
        }

        /// <summary>
        /// Updates the topic
        /// </summary>
        /// <param name="forumTopicId">The forum topic identifier</param>
        /// <param name="forumId">The forum identifier</param>
        /// <param name="userId">The user identifier</param>
        /// <param name="topicTypeId">The topic type identifier</param>
        /// <param name="subject">The subject</param>
        /// <param name="numPosts">The number of posts</param>
        /// <param name="views">The number of views</param>
        /// <param name="lastPostId">The last post identifier</param>
        /// <param name="lastPostUserId">The last post user identifier</param>
        /// <param name="lastPostTime">The last post date and time</param>
        /// <param name="createdOn">The date and time of instance creation</param>
        /// <param name="updatedOn">The date and time of instance update</param>
        /// <returns>Topic</returns>
        public override DBForumTopic UpdateTopic(int forumTopicId,
            int forumId, int userId, int topicTypeId, string subject,
            int numPosts, int views, int lastPostId,
            int lastPostUserId, DateTime? lastPostTime,
            DateTime createdOn, DateTime updatedOn)
        {
            DBForumTopic item = null;
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_Forums_TopicUpdate");
            db.AddInParameter(dbCommand, "TopicID", DbType.Int32, forumTopicId);
            db.AddInParameter(dbCommand, "ForumID", DbType.Int32, forumId);
            db.AddInParameter(dbCommand, "UserID", DbType.Int32, userId);
            db.AddInParameter(dbCommand, "TopicTypeID", DbType.Int32, topicTypeId);
            db.AddInParameter(dbCommand, "Subject", DbType.String, subject);
            db.AddInParameter(dbCommand, "NumPosts", DbType.Int32, numPosts);
            db.AddInParameter(dbCommand, "Views", DbType.Int32, views);
            db.AddInParameter(dbCommand, "LastPostID", DbType.Int32, lastPostId);
            db.AddInParameter(dbCommand, "LastPostUserID", DbType.Int32, lastPostUserId);
            if (lastPostTime.HasValue)
                db.AddInParameter(dbCommand, "LastPostTime", DbType.DateTime, lastPostTime.Value);
            else
                db.AddInParameter(dbCommand, "LastPostTime", DbType.DateTime, null);
            db.AddInParameter(dbCommand, "CreatedOn", DbType.DateTime, createdOn);
            db.AddInParameter(dbCommand, "UpdatedOn", DbType.DateTime, updatedOn);
            if (db.ExecuteNonQuery(dbCommand) > 0)
                item = GetTopicById(forumTopicId, false);

            return item;
        }

        /// <summary>
        /// Deletes a post
        /// </summary>
        /// <param name="forumPostId">The post identifier</param>
        public override void DeletePost(int forumPostId)
        {
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_Forums_PostDelete");
            db.AddInParameter(dbCommand, "PostID", DbType.Int32, forumPostId);
            db.ExecuteNonQuery(dbCommand);
        }

        /// <summary>
        /// Gets a post
        /// </summary>
        /// <param name="forumPostId">The post identifier</param>
        /// <returns>Post</returns>
        public override DBForumPost GetPostById(int forumPostId)
        {
            DBForumPost item = null;
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_Forums_PostLoadByPrimaryKey");
            db.AddInParameter(dbCommand, "PostID", DbType.Int32, forumPostId);
            using (IDataReader dataReader = db.ExecuteReader(dbCommand))
            {
                if (dataReader.Read())
                {
                    item = GetForumPostFromReader(dataReader);
                }
            }
            return item;
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
        /// Inserts a post
        /// </summary>
        /// <param name="forumTopicId">The forum topic identifier</param>
        /// <param name="userId">The user identifier</param>
        /// <param name="text">The text</param>
        /// <param name="ipAddress">The IP address</param>
        /// <param name="createdOn">The date and time of instance creation</param>
        /// <param name="updatedOn">The date and time of instance update</param>
        /// <returns>Post</returns>
        public override DBForumPost InsertPost(int forumTopicId, int userId,
            string text, string ipAddress, DateTime createdOn, DateTime updatedOn)
        {
            DBForumPost item = null;
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_Forums_PostInsert");
            db.AddOutParameter(dbCommand, "PostID", DbType.Int32, 0);
            db.AddInParameter(dbCommand, "TopicID", DbType.Int32, forumTopicId);
            db.AddInParameter(dbCommand, "UserID", DbType.Int32, userId);
            db.AddInParameter(dbCommand, "Text", DbType.String, text);
            db.AddInParameter(dbCommand, "IPAddress", DbType.String, ipAddress);
            db.AddInParameter(dbCommand, "CreatedOn", DbType.DateTime, createdOn);
            db.AddInParameter(dbCommand, "UpdatedOn", DbType.DateTime, updatedOn);
            if (db.ExecuteNonQuery(dbCommand) > 0)
            {
                int postId = Convert.ToInt32(db.GetParameterValue(dbCommand, "@PostID"));
                item = GetPostById(postId);
            }
            return item;
        }

        /// <summary>
        /// Updates the post
        /// </summary>
        /// <param name="forumPostId">The forum post identifier</param>
        /// <param name="forumTopicId">The forum topic identifier</param>
        /// <param name="userId">The user identifier</param>
        /// <param name="text">The text</param>
        /// <param name="ipAddress">The IP address</param>
        /// <param name="createdOn">The date and time of instance creation</param>
        /// <param name="updatedOn">The date and time of instance update</param>
        /// <returns>Post</returns>
        public override DBForumPost UpdatePost(int forumPostId, int forumTopicId, int userId,
            string text, string ipAddress, DateTime createdOn, DateTime updatedOn)
        {
            DBForumPost item = null;
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_Forums_PostUpdate");
            db.AddInParameter(dbCommand, "PostID", DbType.Int32, forumPostId);
            db.AddInParameter(dbCommand, "TopicID", DbType.Int32, forumTopicId);
            db.AddInParameter(dbCommand, "UserID", DbType.Int32, userId);
            db.AddInParameter(dbCommand, "Text", DbType.String, text);
            db.AddInParameter(dbCommand, "IPAddress", DbType.String, ipAddress);
            db.AddInParameter(dbCommand, "CreatedOn", DbType.DateTime, createdOn);
            db.AddInParameter(dbCommand, "UpdatedOn", DbType.DateTime, updatedOn);
            if (db.ExecuteNonQuery(dbCommand) > 0)
                item = GetPostById(forumPostId);

            return item;
        }

        /// <summary>
        /// Deletes a private message
        /// </summary>
        /// <param name="forumPrivateMessageId">The private message identifier</param>
        public override void DeletePrivateMessage(int forumPrivateMessageId)
        {
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_Forums_PrivateMessageDelete");
            db.AddInParameter(dbCommand, "PrivateMessageID", DbType.Int32, forumPrivateMessageId);
            db.ExecuteNonQuery(dbCommand);
        }

        /// <summary>
        /// Gets a private message
        /// </summary>
        /// <param name="forumPrivateMessageId">The private message identifier</param>
        /// <returns>Private message</returns>
        public override DBPrivateMessage GetPrivateMessageById(int forumPrivateMessageId)
        {
            DBPrivateMessage item = null;
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_Forums_PrivateMessageLoadByPrimaryKey");
            db.AddInParameter(dbCommand, "PrivateMessageID", DbType.Int32, forumPrivateMessageId);
            using (IDataReader dataReader = db.ExecuteReader(dbCommand))
            {
                if (dataReader.Read())
                {
                    item = GetPrivateMessageFromReader(dataReader);
                }
            }
            return item;
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
        /// Inserts a private message
        /// </summary>
        /// <param name="fromUserId">The user identifier who sent the message</param>
        /// <param name="toUserId">The user identifier who should receive the message</param>
        /// <param name="subject">The subject</param>
        /// <param name="text">The text</param>
        /// <param name="isRead">The value indivating whether message is read</param>
        /// <param name="isDeletedByAuthor">The value indivating whether message is deleted by author</param>
        /// <param name="isDeletedByRecipient">The value indivating whether message is deleted by recipient</param>
        /// <param name="createdOn">The date and time of instance creation</param>
        /// <returns>Private message</returns>
        public override DBPrivateMessage InsertPrivateMessage(int fromUserId,
            int toUserId, string subject, string text, bool isRead,
            bool isDeletedByAuthor, bool isDeletedByRecipient, DateTime createdOn)
        {
            DBPrivateMessage item = null;
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_Forums_PrivateMessageInsert");
            db.AddOutParameter(dbCommand, "PrivateMessageID", DbType.Int32, 0);
            db.AddInParameter(dbCommand, "FromUserID", DbType.Int32, fromUserId);
            db.AddInParameter(dbCommand, "ToUserID", DbType.Int32, toUserId);
            db.AddInParameter(dbCommand, "Subject", DbType.String, subject);
            db.AddInParameter(dbCommand, "Text", DbType.String, text);
            db.AddInParameter(dbCommand, "IsRead", DbType.Boolean, isRead);
            db.AddInParameter(dbCommand, "IsDeletedByAuthor", DbType.Boolean, isDeletedByAuthor);
            db.AddInParameter(dbCommand, "IsDeletedByRecipient", DbType.Boolean, isDeletedByRecipient);
            db.AddInParameter(dbCommand, "CreatedOn", DbType.DateTime, createdOn);
            if (db.ExecuteNonQuery(dbCommand) > 0)
            {
                int privateMessageId = Convert.ToInt32(db.GetParameterValue(dbCommand, "@PrivateMessageID"));
                item = GetPrivateMessageById(privateMessageId);
            }
            return item;
        }

        /// <summary>
        /// Updates the private message
        /// </summary>
        /// <param name="privateMessageId">The private message identifier</param>
        /// <param name="fromUserId">The user identifier who sent the message</param>
        /// <param name="toUserId">The user identifier who should receive the message</param>
        /// <param name="subject">The subject</param>
        /// <param name="text">The text</param>
        /// <param name="isRead">The value indivating whether message is read</param>
        /// <param name="isDeletedByAuthor">The value indivating whether message is deleted by author</param>
        /// <param name="isDeletedByRecipient">The value indivating whether message is deleted by recipient</param>
        /// <param name="createdOn">The date and time of instance creation</param>
        /// <returns>Private message</returns>
        public override DBPrivateMessage UpdatePrivateMessage(int privateMessageId,
            int fromUserId, int toUserId, string subject, string text, bool isRead,
            bool isDeletedByAuthor, bool isDeletedByRecipient, DateTime createdOn)
        {
            DBPrivateMessage item = null;
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_Forums_PrivateMessageUpdate");
            db.AddInParameter(dbCommand, "PrivateMessageID", DbType.Int32, privateMessageId);
            db.AddInParameter(dbCommand, "FromUserID", DbType.Int32, fromUserId);
            db.AddInParameter(dbCommand, "ToUserID", DbType.Int32, toUserId);
            db.AddInParameter(dbCommand, "Subject", DbType.String, subject);
            db.AddInParameter(dbCommand, "Text", DbType.String, text);
            db.AddInParameter(dbCommand, "IsRead", DbType.Boolean, isRead);
            db.AddInParameter(dbCommand, "IsDeletedByAuthor", DbType.Boolean, isDeletedByAuthor);
            db.AddInParameter(dbCommand, "IsDeletedByRecipient", DbType.Boolean, isDeletedByRecipient);
            db.AddInParameter(dbCommand, "CreatedOn", DbType.DateTime, createdOn);
            if (db.ExecuteNonQuery(dbCommand) > 0)
                item = GetPrivateMessageById(privateMessageId);

            return item;
        }

        /// <summary>
        /// Deletes a forum subscription
        /// </summary>
        /// <param name="forumSubscriptionId">The forum subscription identifier</param>
        public override void DeleteSubscription(int forumSubscriptionId)
        {
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_Forums_SubscriptionDelete");
            db.AddInParameter(dbCommand, "SubscriptionID", DbType.Int32, forumSubscriptionId);
            db.ExecuteNonQuery(dbCommand);
        }

        /// <summary>
        /// Gets a forum subscription
        /// </summary>
        /// <param name="forumSubscriptionId">The forum subscription identifier</param>
        /// <returns>Forum subscription</returns>
        public override DBForumSubscription GetSubscriptionById(int forumSubscriptionId)
        {
            DBForumSubscription item = null;
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_Forums_SubscriptionLoadByPrimaryKey");
            db.AddInParameter(dbCommand, "SubscriptionID", DbType.Int32, forumSubscriptionId);
            using (IDataReader dataReader = db.ExecuteReader(dbCommand))
            {
                if (dataReader.Read())
                {
                    item = GetForumSubscriptionFromReader(dataReader);
                }
            }
            return item;
        }

        /// <summary>
        /// Gets a forum subscription
        /// </summary>
        /// <param name="subscriptionGuid">The forum subscription identifier</param>
        /// <returns>Forum subscription</returns>
        public override DBForumSubscription GetSubscriptionByGuid(int subscriptionGuid)
        {
            DBForumSubscription item = null;
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_Forums_SubscriptionLoadByGUID");
            db.AddInParameter(dbCommand, "SubscriptionGUID", DbType.Guid, subscriptionGuid);
            using (IDataReader dataReader = db.ExecuteReader(dbCommand))
            {
                if (dataReader.Read())
                {
                    item = GetForumSubscriptionFromReader(dataReader);
                }
            }
            return item;
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

        /// <summary>
        /// Inserts a forum subscription
        /// </summary>
        /// <param name="subscriptionGuid">The forum subscription identifier</param>
        /// <param name="userId">The user identifier</param>
        /// <param name="forumId">The forum identifier</param>
        /// <param name="topicId">The topic identifier</param>
        /// <param name="createdOn">The date and time of instance creation</param>
        /// <returns>Forum subscription</returns>
        public override DBForumSubscription InsertSubscription(Guid subscriptionGuid,
            int userId, int forumId, int topicId, DateTime createdOn)
        {
            DBForumSubscription item = null;
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_Forums_SubscriptionInsert");
            db.AddOutParameter(dbCommand, "SubscriptionID", DbType.Int32, 0);
            db.AddInParameter(dbCommand, "SubscriptionGUID", DbType.Guid, subscriptionGuid);
            db.AddInParameter(dbCommand, "UserID", DbType.Int32, userId);
            db.AddInParameter(dbCommand, "ForumID", DbType.Int32, forumId);
            db.AddInParameter(dbCommand, "TopicID", DbType.Int32, topicId);
            db.AddInParameter(dbCommand, "CreatedOn", DbType.DateTime, createdOn);
            if (db.ExecuteNonQuery(dbCommand) > 0)
            {
                int subscriptionId = Convert.ToInt32(db.GetParameterValue(dbCommand, "@SubscriptionID"));
                item = GetSubscriptionById(subscriptionId);
            }
            return item;
        }

        /// <summary>
        /// Updates the forum subscription
        /// </summary>
        /// <param name="subscriptionId">The forum subscription identifier</param>
        /// <param name="subscriptionGuid">The forum subscription identifier</param>
        /// <param name="userId">The user identifier</param>
        /// <param name="forumId">The forum identifier</param>
        /// <param name="topicId">The topic identifier</param>
        /// <param name="createdOn">The date and time of instance creation</param>
        /// <returns>Forum subscription</returns>
        public override DBForumSubscription UpdateSubscription(int subscriptionId,
            Guid subscriptionGuid, int userId, int forumId, int topicId, DateTime createdOn)
        {
            DBForumSubscription item = null;
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_Forums_SubscriptionUpdate");
            db.AddInParameter(dbCommand, "SubscriptionID", DbType.Int32, subscriptionId);
            db.AddInParameter(dbCommand, "SubscriptionGUID", DbType.Guid, subscriptionGuid);
            db.AddInParameter(dbCommand, "UserID", DbType.Int32, userId);
            db.AddInParameter(dbCommand, "ForumID", DbType.Int32, forumId);
            db.AddInParameter(dbCommand, "TopicID", DbType.Int32, topicId);
            db.AddInParameter(dbCommand, "CreatedOn", DbType.DateTime, createdOn);
            if (db.ExecuteNonQuery(dbCommand) > 0)
                item = GetSubscriptionById(subscriptionId);

            return item;
        }
        #endregion
    }
}
