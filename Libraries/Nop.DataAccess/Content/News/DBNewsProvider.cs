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

namespace NopSolutions.NopCommerce.DataAccess.Content.NewsManagement
{
    /// <summary>
    /// Acts as a base class for deriving custom news provider
    /// </summary>
    [DBProviderSectionName("nopDataProviders/NewsProvider")]
    public abstract partial class DBNewsProvider : BaseDBProvider
    {
        #region Methods
        /// <summary>
        /// Gets a news
        /// </summary>
        /// <param name="newsId">The news identifier</param>
        /// <returns>News</returns>
        public abstract DBNews GetNewsById(int newsId);
        
        /// <summary>
        /// Deletes a news
        /// </summary>
        /// <param name="newsId">The news identifier</param>
        public abstract void DeleteNews(int newsId);
        
        /// <summary>
        /// Gets news item collection
        /// </summary>
        /// <param name="languageId">Language identifier. 0 if you want to get all news</param>
        /// <param name="newsCount">News item count. 0 if you want to get all news</param>
        /// <param name="showHidden">A value indicating whether to show hidden records</param>
        /// <returns>News item collection</returns>
        public abstract DBNewsCollection GetNews(int languageId, 
            int newsCount, bool showHidden);
        
        /// <summary>
        /// Inserts a news item
        /// </summary>
        /// <param name="languageId">The language identifier</param>
        /// <param name="title">The news title</param>
        /// <param name="shortText">The short text</param>
        /// <param name="fullText">The full text</param>
        /// <param name="published">A value indicating whether the entity is published</param>
        /// <param name="allowComments">A value indicating whether the entity allows comments</param>
        /// <param name="createdOn">The date and time of instance creation</param>
        /// <returns>News item</returns>
        public abstract DBNews InsertNews(int languageId, string title, string shortText,
            string fullText, bool published, bool allowComments, DateTime createdOn);

        /// <summary>
        /// Updates the news item
        /// </summary>
        /// <param name="newsId">The news identifier</param>
        /// <param name="languageId">The language identifier</param>
        /// <param name="title">The news title</param>
        /// <param name="shortText">The short text</param>
        /// <param name="fullText">The full text</param>
        /// <param name="published">A value indicating whether the entity is published</param>
        /// <param name="allowComments">A value indicating whether the entity allows comments</param>
        /// <param name="createdOn">The date and time of instance creation</param>
        /// <returns>News item</returns>
        public abstract DBNews UpdateNews(int newsId, int languageId,
            string title, string shortText, string fullText, 
            bool published, bool allowComments, DateTime createdOn);

        /// <summary>
        /// Gets a news comment
        /// </summary>
        /// <param name="newsCommentId">News comment identifer</param>
        /// <returns>News comment</returns>
        public abstract DBNewsComment GetNewsCommentById(int newsCommentId);
        
        /// <summary>
        /// Gets a news comment collection by news identifier
        /// </summary>
        /// <param name="newsId">The news identifier</param>
        /// <returns>News comment collection</returns>
        public abstract DBNewsCommentCollection GetNewsCommentsByNewsId(int newsId);

        /// <summary>
        /// Deletes a news comment
        /// </summary>
        /// <param name="newsCommentId">The news comment identifier</param>
        public abstract void DeleteNewsComment(int newsCommentId);
        
        /// <summary>
        /// Gets all news comments
        /// </summary>
        /// <returns>News comment collection</returns>
        public abstract DBNewsCommentCollection GetAllNewsComments();

        /// <summary>
        /// Inserts a news comment
        /// </summary>
        /// <param name="newsId">The news identifier</param>
        /// <param name="customerId">The customer identifier</param>
        /// <param name="ipAddress">The IP address</param>
        /// <param name="title">The title</param>
        /// <param name="comment">The comment</param>
        /// <param name="createdOn">The date and time of instance creation</param>
        /// <returns>News comment</returns>
        public abstract DBNewsComment InsertNewsComment(int newsId, int customerId, string ipAddress, 
            string title, string comment, DateTime createdOn);

        /// <summary>
        /// Updates the news comment
        /// </summary>
        /// <param name="newsCommentId">The news comment identifier</param>
        /// <param name="newsId">The news identifier</param>
        /// <param name="customerId">The customer identifier</param>
        /// <param name="ipAddress">The IP address</param>
        /// <param name="title">The title</param>
        /// <param name="comment">The comment</param>
        /// <param name="createdOn">The date and time of instance creation</param>
        /// <returns>News comment</returns>
        public abstract DBNewsComment UpdateNewsComment(int newsCommentId,
            int newsId, int customerId, string ipAddress, string title,
            string comment, DateTime createdOn);
        #endregion
    }
}
