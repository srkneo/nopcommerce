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


namespace NopSolutions.NopCommerce.DataAccess.Content.Blog
{
    /// <summary>
    /// Acts as a base class for deriving custom log provider
    /// </summary>
    [DBProviderSectionName("nopDataProviders/BlogProvider")]
    public abstract partial class DBBlogProvider : BaseDBProvider
    {
        #region Methods
        /// <summary>
        /// Deletes an blog post
        /// </summary>
        /// <param name="blogPostId">Blog post identifier</param>
        public abstract void DeleteBlogPost(int blogPostId);
        
        /// <summary>
        /// Gets an blog post
        /// </summary>
        /// <param name="blogPostId">Blog post identifier</param>
        /// <returns>Blog post</returns>
        public abstract DBBlogPost GetBlogPostById(int blogPostId);

        /// <summary>
        /// Gets all blog posts
        /// </summary>
        /// <param name="languageId">Language identifier. 0 if you want to get all news</param>
        /// <param name="pageSize">Page size</param>
        /// <param name="pageIndex">Page index</param>
        /// <param name="totalRecords">Total records</param>
        /// <returns>Blog posts</returns>
        public abstract DBBlogPostCollection GetAllBlogPosts(int languageId, int pageSize, 
            int pageIndex, out int totalRecords);

        /// <summary>
        /// Inserts an blog post
        /// </summary>
        /// <param name="languageId">The language identifier</param>
        /// <param name="blogPostTitle">The blog post title</param>
        /// <param name="blogPostBody">The blog post title</param>
        /// <param name="blogPostAllowComments">A value indicating whether the blog post comments are allowed</param>
        /// <param name="createdById">The user identifier who created the blog post</param>
        /// <param name="createdOn">The date and time of instance creation</param>
        /// <returns>Blog post</returns>
        public abstract DBBlogPost InsertBlogPost(int languageId, string blogPostTitle,
            string blogPostBody, bool blogPostAllowComments, 
            int createdById, DateTime createdOn);

        /// <summary>
        /// Updates the blog post
        /// </summary>
        /// <param name="blogPostId">The blog post identifier</param>
        /// <param name="languageId">The language identifier</param>
        /// <param name="blogPostTitle">The blog post title</param>
        /// <param name="blogPostBody">The blog post title</param>
        /// <param name="blogPostAllowComments">A value indicating whether the blog post comments are allowed</param>
        /// <param name="createdById">The user identifier who created the blog post</param>
        /// <param name="createdOn">The date and time of instance creation</param>
        /// <returns>Blog post</returns>
        public abstract DBBlogPost UpdateBlogPost(int blogPostId, 
            int languageId, string blogPostTitle,
            string blogPostBody, bool blogPostAllowComments,
            int createdById, DateTime createdOn);

        /// <summary>
        /// Deletes a blog comment
        /// </summary>
        /// <param name="blogCommentId">Blog comment identifier</param>
        public abstract void DeleteBlogComment(int blogCommentId);

        /// <summary>
        /// Gets a blog comment
        /// </summary>
        /// <param name="blogCommentId">Blog comment identifier</param>
        /// <returns>A blog comment</returns>
        public abstract DBBlogComment GetBlogCommentById(int blogCommentId);

        /// <summary>
        /// Gets a collection of blog comments by blog post identifier
        /// </summary>
        /// <param name="blogPostId">Blog post identifier</param>
        /// <returns>A collection of blog comments</returns>
        public abstract DBBlogCommentCollection GetBlogCommentsByBlogPostId(int blogPostId);

        /// <summary>
        /// Gets all blog comments
        /// </summary>
        /// <returns>Blog comments</returns>
        public abstract DBBlogCommentCollection GetAllBlogComments();

        /// <summary>
        /// Inserts a blog comment
        /// </summary>
        /// <param name="blogPostId">The blog post identifier</param>
        /// <param name="customerId">The customer identifier who commented the blog post</param>
        /// <param name="commentText">The comment text</param>
        /// <param name="createdOn">The date and time of instance creation</param>
        /// <returns>Blog comment</returns>
        public abstract DBBlogComment InsertBlogComment(int blogPostId,
            int customerId, string commentText, DateTime createdOn);

        /// <summary>
        /// Updates the blog comment
        /// </summary>
        /// <param name="blogCommentId">The blog comment identifier</param>
        /// <param name="blogPostId">The blog post identifier</param>
        /// <param name="customerId">The customer identifier who commented the blog post</param>
        /// <param name="commentText">The comment text</param>
        /// <param name="createdOn">The date and time of instance creation</param>
        /// <returns>Blog comment</returns>
        public abstract DBBlogComment UpdateBlogComment(int blogCommentId, int blogPostId,
            int customerId, string commentText, DateTime createdOn);
        #endregion
    }
}
