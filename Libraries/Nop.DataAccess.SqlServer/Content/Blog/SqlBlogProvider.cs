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
using Microsoft.Practices.EnterpriseLibrary.Data;
using System.Configuration.Provider;
using System.Collections.Specialized;

namespace NopSolutions.NopCommerce.DataAccess.Content.Blog
{
    /// <summary>
    /// Blog provider for SQL Server
    /// </summary>
    public partial class SqlBlogProvider : DBBlogProvider
    {
        #region Fields
        private string _sqlConnectionString;
        #endregion

        #region Utilities
        private DBBlogPost GetBlogPostFromReader(IDataReader dataReader)
        {
            var item = new DBBlogPost();
            item.BlogPostId = NopSqlDataHelper.GetInt(dataReader, "BlogPostID");
            item.LanguageId = NopSqlDataHelper.GetInt(dataReader, "LanguageID");
            item.BlogPostTitle = NopSqlDataHelper.GetString(dataReader, "BlogPostTitle");
            item.BlogPostBody = NopSqlDataHelper.GetString(dataReader, "BlogPostBody");
            item.BlogPostAllowComments = NopSqlDataHelper.GetBoolean(dataReader, "BlogPostAllowComments");
            item.CreatedById = NopSqlDataHelper.GetInt(dataReader, "CreatedByID");
            item.CreatedOn = NopSqlDataHelper.GetUtcDateTime(dataReader, "CreatedOn");
            return item;
        }

        private DBBlogComment GetBlogCommentFromReader(IDataReader dataReader)
        {
            var item = new DBBlogComment();
            item.BlogCommentId = NopSqlDataHelper.GetInt(dataReader, "BlogCommentID");
            item.BlogPostId = NopSqlDataHelper.GetInt(dataReader, "BlogPostID");
            item.CustomerId = NopSqlDataHelper.GetInt(dataReader, "CustomerID");
            item.CommentText = NopSqlDataHelper.GetString(dataReader, "CommentText");
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
        /// Deletes an blog post
        /// </summary>
        /// <param name="blogPostId">Blog post identifier</param>
        public override void DeleteBlogPost(int blogPostId)
        {
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_BlogPostDelete");
            db.AddInParameter(dbCommand, "BlogPostID", DbType.Int32, blogPostId);
            db.ExecuteNonQuery(dbCommand);
        }

        /// <summary>
        /// Gets an blog post
        /// </summary>
        /// <param name="blogPostId">Blog post identifier</param>
        /// <returns>Blog post</returns>
        public override DBBlogPost GetBlogPostById(int blogPostId)
        {
            DBBlogPost item = null;
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_BlogPostLoadByPrimaryKey");
            db.AddInParameter(dbCommand, "BlogPostID", DbType.Int32, blogPostId);
            using (IDataReader dataReader = db.ExecuteReader(dbCommand))
            {
                if (dataReader.Read())
                {
                    item = GetBlogPostFromReader(dataReader);
                }
            }
            return item;
        }

        /// <summary>
        /// Gets all blog posts
        /// </summary>
        /// <param name="languageId">Language identifier. 0 if you want to get all news</param>
        /// <param name="pageSize">Page size</param>
        /// <param name="pageIndex">Page index</param>
        /// <param name="totalRecords">Total records</param>
        /// <returns>Blog posts</returns>
        public override DBBlogPostCollection GetAllBlogPosts(int languageId, int pageSize,
            int pageIndex, out int totalRecords)
        {
            totalRecords = 0;
            var result = new DBBlogPostCollection();
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_BlogPostLoadAll");
            db.AddInParameter(dbCommand, "LanguageID", DbType.Int32, languageId);
            db.AddInParameter(dbCommand, "PageSize", DbType.Int32, pageSize);
            db.AddInParameter(dbCommand, "PageIndex", DbType.Int32, pageIndex);
            db.AddOutParameter(dbCommand, "TotalRecords", DbType.Int32, 0);
            using (IDataReader dataReader = db.ExecuteReader(dbCommand))
            {
                while (dataReader.Read())
                {
                    var item = GetBlogPostFromReader(dataReader);
                    result.Add(item);
                }
            }
            totalRecords = Convert.ToInt32(db.GetParameterValue(dbCommand, "@TotalRecords"));

            return result;
        }

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
        public override DBBlogPost InsertBlogPost(int languageId, string blogPostTitle,
            string blogPostBody, bool blogPostAllowComments,
            int createdById, DateTime createdOn)
        {
            DBBlogPost item = null;
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_BlogPostInsert");
            db.AddOutParameter(dbCommand, "BlogPostID", DbType.Int32, 0);
            db.AddInParameter(dbCommand, "LanguageID", DbType.Int32, languageId);
            db.AddInParameter(dbCommand, "BlogPostTitle", DbType.String, blogPostTitle);
            db.AddInParameter(dbCommand, "BlogPostBody", DbType.String, blogPostBody);
            db.AddInParameter(dbCommand, "BlogPostAllowComments", DbType.Boolean, blogPostAllowComments);
            db.AddInParameter(dbCommand, "CreatedByID", DbType.Int32, createdById);
            db.AddInParameter(dbCommand, "CreatedOn", DbType.DateTime, createdOn);
            if (db.ExecuteNonQuery(dbCommand) > 0)
            {
                int blogPostId = Convert.ToInt32(db.GetParameterValue(dbCommand, "@BlogPostID"));
                item = GetBlogPostById(blogPostId);
            }
            return item;
        }

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
        public override DBBlogPost UpdateBlogPost(int blogPostId,
            int languageId, string blogPostTitle,
            string blogPostBody, bool blogPostAllowComments,
            int createdById, DateTime createdOn)
        {
            DBBlogPost item = null;
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_BlogPostUpdate");
            db.AddInParameter(dbCommand, "BlogPostID", DbType.Int32, blogPostId);
            db.AddInParameter(dbCommand, "LanguageID", DbType.Int32, languageId);
            db.AddInParameter(dbCommand, "BlogPostTitle", DbType.String, blogPostTitle);
            db.AddInParameter(dbCommand, "BlogPostBody", DbType.String, blogPostBody);
            db.AddInParameter(dbCommand, "BlogPostAllowComments", DbType.Boolean, blogPostAllowComments);
            db.AddInParameter(dbCommand, "CreatedByID", DbType.Int32, createdById);
            db.AddInParameter(dbCommand, "CreatedOn", DbType.DateTime, createdOn);
            if (db.ExecuteNonQuery(dbCommand) > 0)
                item = GetBlogPostById(blogPostId);

            return item;
        }

        /// <summary>
        /// Deletes a blog comment
        /// </summary>
        /// <param name="blogCommentId">Blog comment identifier</param>
        public override void DeleteBlogComment(int blogCommentId)
        {
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_BlogCommentDelete");
            db.AddInParameter(dbCommand, "BlogCommentID", DbType.Int32, blogCommentId);
            db.ExecuteNonQuery(dbCommand);
        }

        /// <summary>
        /// Gets a blog comment
        /// </summary>
        /// <param name="blogCommentId">Blog comment identifier</param>
        /// <returns>A blog comment</returns>
        public override DBBlogComment GetBlogCommentById(int blogCommentId)
        {
            DBBlogComment item = null;
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_BlogCommentLoadByPrimaryKey");
            db.AddInParameter(dbCommand, "BlogCommentID", DbType.Int32, blogCommentId);
            using (IDataReader dataReader = db.ExecuteReader(dbCommand))
            {
                if (dataReader.Read())
                {
                    item = GetBlogCommentFromReader(dataReader);
                }
            }
            return item;
        }

        /// <summary>
        /// Gets a collection of blog comments by blog post identifier
        /// </summary>
        /// <param name="blogPostId">Blog post identifier</param>
        /// <returns>A collection of blog comments</returns>
        public override DBBlogCommentCollection GetBlogCommentsByBlogPostId(int blogPostId)
        {
            var result = new DBBlogCommentCollection();
            if (blogPostId == 0)
                return result;
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_BlogCommentLoadByBlogPostID");
            db.AddInParameter(dbCommand, "BlogPostID", DbType.Int32, blogPostId);
            using (IDataReader dataReader = db.ExecuteReader(dbCommand))
            {
                while (dataReader.Read())
                {
                    var item = GetBlogCommentFromReader(dataReader);
                    result.Add(item);
                }
            }
            return result;
        }

        /// <summary>
        /// Gets all blog comments
        /// </summary>
        /// <returns>Blog comments</returns>
        public override DBBlogCommentCollection GetAllBlogComments()
        {
            var result = new DBBlogCommentCollection();
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_BlogCommentLoadAll");
            using (IDataReader dataReader = db.ExecuteReader(dbCommand))
            {
                while (dataReader.Read())
                {
                    var item = GetBlogCommentFromReader(dataReader);
                    result.Add(item);
                }
            }

            return result;
        }

        /// <summary>
        /// Inserts a blog comment
        /// </summary>
        /// <param name="blogPostId">The blog post identifier</param>
        /// <param name="customerId">The customer identifier who commented the blog post</param>
        /// <param name="commentText">The comment text</param>
        /// <param name="createdOn">The date and time of instance creation</param>
        /// <returns>Blog comment</returns>
        public override DBBlogComment InsertBlogComment(int blogPostId,
            int customerId, string commentText, DateTime createdOn)
        {
            DBBlogComment item = null;
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_BlogCommentInsert");
            db.AddOutParameter(dbCommand, "BlogCommentID", DbType.Int32, 0);
            db.AddInParameter(dbCommand, "BlogPostID", DbType.Int32, blogPostId);
            db.AddInParameter(dbCommand, "CustomerID", DbType.Int32, customerId);
            db.AddInParameter(dbCommand, "CommentText", DbType.String, commentText);
            db.AddInParameter(dbCommand, "CreatedOn", DbType.DateTime, createdOn);
            if (db.ExecuteNonQuery(dbCommand) > 0)
            {
                int blogCommentId = Convert.ToInt32(db.GetParameterValue(dbCommand, "@BlogCommentID"));
                item = GetBlogCommentById(blogCommentId);
            }
            return item;
        }

        /// <summary>
        /// Updates the blog comment
        /// </summary>
        /// <param name="blogCommentId">The blog comment identifier</param>
        /// <param name="blogPostId">The blog post identifier</param>
        /// <param name="customerId">The customer identifier who commented the blog post</param>
        /// <param name="commentText">The comment text</param>
        /// <param name="createdOn">The date and time of instance creation</param>
        /// <returns>Blog comment</returns>
        public override DBBlogComment UpdateBlogComment(int blogCommentId, int blogPostId,
            int customerId, string commentText, DateTime createdOn)
        {
            DBBlogComment item = null;
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_BlogCommentUpdate");
            db.AddInParameter(dbCommand, "BlogCommentID", DbType.Int32, blogCommentId);
            db.AddInParameter(dbCommand, "BlogPostID", DbType.Int32, blogPostId);
            db.AddInParameter(dbCommand, "CustomerID", DbType.Int32, customerId);
            db.AddInParameter(dbCommand, "CommentText", DbType.String, commentText);
            db.AddInParameter(dbCommand, "CreatedOn", DbType.DateTime, createdOn);
            if (db.ExecuteNonQuery(dbCommand) > 0)
                item = GetBlogCommentById(blogCommentId);

            return item;
        }
        #endregion
    }
}
