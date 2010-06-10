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
        /// Update forum stats
        /// </summary>
        /// <param name="forumId">The forum identifier</param>
        public abstract void UpdateForumStats(int forumId);

        /// <summary>
        /// Deletes a forum
        /// </summary>
        /// <param name="forumId">The forum identifier</param>
        public abstract void DeleteForum(int forumId);

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

        #endregion
    }
}
