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

namespace NopSolutions.NopCommerce.DataAccess.Content.Forums
{
    /// <summary>
    /// Acts as a base class for deriving custom forum provider
    /// </summary>
    [DBProviderSectionName("nopDataProviders/ForumProvider")]
    public abstract partial class DBForumProvider : BaseDBProvider
    {
        #region Methods

        /// <summary>
        /// Deletes a forum group
        /// </summary>
        /// <param name="forumGroupId">The forum group identifier</param>
        public abstract void DeleteForumGroup(int forumGroupId);

        /// <summary>
        /// Gets a forum group
        /// </summary>
        /// <param name="forumGroupId">The forum group identifier</param>
        /// <returns>Forum group</returns>
        public abstract DBForumGroup GetForumGroupById(int forumGroupId);

        /// <summary>
        /// Gets all forum groups
        /// </summary>
        /// <returns>Forum groups</returns>
        public abstract DBForumGroupCollection GetAllForumGroups();

        /// <summary>
        /// Inserts a forum group
        /// </summary>
        /// <param name="name">The language name</param>
        /// <param name="description">The description</param>
        /// <param name="displayOrder">The display order</param>        
        /// <param name="createdOn">The date and time of instance creation</param>
        /// <param name="updatedOn">The date and time of instance update</param>
        /// <returns>Forum group</returns>
        public abstract DBForumGroup InsertForumGroup(string name, string description,
            int displayOrder, DateTime createdOn, DateTime updatedOn);

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
        public abstract DBForumGroup UpdateForumGroup(int forumGroupId, 
            string name, string description, int displayOrder, 
            DateTime createdOn, DateTime updatedOn);

        /// <summary>
        /// Deletes a forum
        /// </summary>
        /// <param name="forumId">The forum identifier</param>
        public abstract void DeleteForum(int forumId);

        /// <summary>
        /// Gets a forum
        /// </summary>
        /// <param name="forumId">The forum identifier</param>
        /// <returns>Forum</returns>
        public abstract DBForum GetForumById(int forumId);

        /// <summary>
        /// Gets forums by group identifier
        /// </summary>
        /// <param name="forumGroupId">The forum group identifier</param>
        /// <returns>Forums</returns>
        public abstract DBForumCollection GetAllForumsByGroupId(int forumGroupId);

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
        public abstract DBForum InsertForum(int forumGroupId,
            string name, string description,
            int numTopics, int numPosts, int lastTopicId, int lastPostId,
            int lastPostUserId, DateTime? lastPostTime, int displayOrder,
            DateTime createdOn, DateTime updatedOn);

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
        public abstract DBForum UpdateForum(int forumId, 
            int forumGroupId, string name, string description,
            int numTopics, int numPosts, int lastTopicId, int lastPostId,
            int lastPostUserId, DateTime? lastPostTime, int displayOrder,
            DateTime createdOn, DateTime updatedOn);

        /// <summary>
        /// Update forum stats
        /// </summary>
        /// <param name="forumId">The forum identifier</param>
        public abstract void UpdateForumStats(int forumId);

        /// <summary>
        /// Deletes a topic
        /// </summary>
        /// <param name="forumTopicId">The topic identifier</param>
        public abstract void DeleteTopic(int forumTopicId);

        /// <summary>
        /// Gets a topic
        /// </summary>
        /// <param name="forumTopicId">The topic identifier</param>
        /// <param name="increaseViews">The value indicating whether to increase topic views</param>
        /// <returns>Topic</returns>
        public abstract DBForumTopic GetTopicById(int forumTopicId, bool increaseViews);

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
        public abstract DBForumTopicCollection GetAllTopics(int forumId, 
            int userId, string keywords, bool searchPosts, int pageSize, 
            int pageIndex, out int totalRecords);

        /// <summary>
        /// Gets active topics
        /// </summary>
        /// <param name="forumId">The forum group identifier</param>
        /// <param name="topicCount">Topic count. 0 if you want to get all topics</param>
        /// <returns>Topics</returns>
        public abstract DBForumTopicCollection GetActiveTopics(int forumId, int topicCount);

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
        public abstract DBForumTopic InsertTopic(int forumId, int userId,
            int topicTypeId, string subject,
            int numPosts, int views, int lastPostId,
            int lastPostUserId, DateTime? lastPostTime,
            DateTime createdOn, DateTime updatedOn);

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
        public abstract DBForumTopic UpdateTopic(int forumTopicId, 
            int forumId, int userId, int topicTypeId, string subject,
            int numPosts, int views, int lastPostId,
            int lastPostUserId, DateTime? lastPostTime,
            DateTime createdOn, DateTime updatedOn);

        /// <summary>
        /// Deletes a post
        /// </summary>
        /// <param name="forumPostId">The post identifier</param>
        public abstract void DeletePost(int forumPostId);

        /// <summary>
        /// Gets a post
        /// </summary>
        /// <param name="forumPostId">The post identifier</param>
        /// <returns>Post</returns>
        public abstract DBForumPost GetPostById(int forumPostId);

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
        public abstract DBForumPostCollection GetAllPosts(int forumTopicId, int userId, 
            string keywords, bool ascSort, int pageSize, int pageIndex, out int totalRecords);

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
        public abstract DBForumPost InsertPost(int forumTopicId, int userId,
            string text, string ipAddress, DateTime createdOn, DateTime updatedOn);

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
        public abstract DBForumPost UpdatePost(int forumPostId, int forumTopicId, int userId,
            string text, string ipAddress, DateTime createdOn, DateTime updatedOn);

        /// <summary>
        /// Deletes a private message
        /// </summary>
        /// <param name="forumPrivateMessageId">The private message identifier</param>
        public abstract void DeletePrivateMessage(int forumPrivateMessageId);

        /// <summary>
        /// Gets a private message
        /// </summary>
        /// <param name="forumPrivateMessageId">The private message identifier</param>
        /// <returns>Private message</returns>
        public abstract DBPrivateMessage GetPrivateMessageById(int forumPrivateMessageId);

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
        public abstract DBPrivateMessageCollection GetAllPrivateMessages(int fromUserId, 
            int toUserId, bool? isRead, bool? isDeletedByAuthor, bool? isDeletedByRecipient, 
            string keywords, int pageSize, int pageIndex, out int totalRecords);

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
        public abstract DBPrivateMessage InsertPrivateMessage(int fromUserId, 
            int toUserId, string subject, string text, bool isRead,
            bool isDeletedByAuthor, bool isDeletedByRecipient, DateTime createdOn);

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
        public abstract DBPrivateMessage UpdatePrivateMessage(int privateMessageId, 
            int fromUserId,  int toUserId, string subject, string text, bool isRead,
            bool isDeletedByAuthor, bool isDeletedByRecipient, DateTime createdOn);

        /// <summary>
        /// Deletes a forum subscription
        /// </summary>
        /// <param name="forumSubscriptionId">The forum subscription identifier</param>
        public abstract void DeleteSubscription(int forumSubscriptionId);

        /// <summary>
        /// Gets a forum subscription
        /// </summary>
        /// <param name="forumSubscriptionId">The forum subscription identifier</param>
        /// <returns>Forum subscription</returns>
        public abstract DBForumSubscription GetSubscriptionById(int forumSubscriptionId);

        /// <summary>
        /// Gets a forum subscription
        /// </summary>
        /// <param name="subscriptionGuid">The forum subscription identifier</param>
        /// <returns>Forum subscription</returns>
        public abstract DBForumSubscription GetSubscriptionByGuid(int subscriptionGuid);

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
        public abstract DBForumSubscriptionCollection GetAllSubscriptions(int userId, 
            int forumId, int topicId, int pageSize, int pageIndex, out int totalRecords);

        /// <summary>
        /// Inserts a forum subscription
        /// </summary>
        /// <param name="subscriptionGuid">The forum subscription identifier</param>
        /// <param name="userId">The user identifier</param>
        /// <param name="forumId">The forum identifier</param>
        /// <param name="topicId">The topic identifier</param>
        /// <param name="createdOn">The date and time of instance creation</param>
        /// <returns>Forum subscription</returns>
        public abstract DBForumSubscription InsertSubscription(Guid subscriptionGuid, 
            int userId, int forumId, int topicId, DateTime createdOn);

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
        public abstract DBForumSubscription UpdateSubscription(int subscriptionId,
            Guid subscriptionGuid, int userId, int forumId, int topicId, DateTime createdOn);

        #endregion
    }
}
