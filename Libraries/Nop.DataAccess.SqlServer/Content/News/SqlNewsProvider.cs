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

namespace NopSolutions.NopCommerce.DataAccess.Content.NewsManagement
{
    /// <summary>
    /// News provider for SQL Server
    /// </summary>
    public partial class SqlNewsProvider : DBNewsProvider
    {
        #region Fields
        private string _sqlConnectionString;
        #endregion

        #region Utilities
        private DBNews GetNewsFromReader(IDataReader dataReader)
        {
            var item = new DBNews();
            item.NewsId = NopSqlDataHelper.GetInt(dataReader, "NewsID");
            item.LanguageId = NopSqlDataHelper.GetInt(dataReader, "LanguageID");
            item.Title = NopSqlDataHelper.GetString(dataReader, "Title");
            item.Short = NopSqlDataHelper.GetString(dataReader, "Short");
            item.Full = NopSqlDataHelper.GetString(dataReader, "Full");
            item.Published = NopSqlDataHelper.GetBoolean(dataReader, "Published");
            item.AllowComments = NopSqlDataHelper.GetBoolean(dataReader, "AllowComments");
            item.CreatedOn = NopSqlDataHelper.GetUtcDateTime(dataReader, "CreatedOn");
            return item;
        }

        private DBNewsComment GetNewsCommentFromReader(IDataReader dataReader)
        {
            var item = new DBNewsComment();
            item.NewsCommentId = NopSqlDataHelper.GetInt(dataReader, "NewsCommentID");
            item.NewsId = NopSqlDataHelper.GetInt(dataReader, "NewsID");
            item.CustomerId = NopSqlDataHelper.GetInt(dataReader, "CustomerID");
            item.IPAddress = NopSqlDataHelper.GetString(dataReader, "IPAddress");
            item.Title = NopSqlDataHelper.GetString(dataReader, "Title");
            item.Comment = NopSqlDataHelper.GetString(dataReader, "Comment");
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
        /// Gets a news
        /// </summary>
        /// <param name="newsId">The news identifier</param>
        /// <returns>News</returns>
        public override DBNews GetNewsById(int newsId)
        {
            DBNews item = null;
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_NewsLoadByPrimaryKey");
            db.AddInParameter(dbCommand, "NewsID", DbType.Int32, newsId);
            using (IDataReader dataReader = db.ExecuteReader(dbCommand))
            {
                if (dataReader.Read())
                {
                    item = GetNewsFromReader(dataReader);
                }
            }
            return item;
        }

        /// <summary>
        /// Deletes a news
        /// </summary>
        /// <param name="newsId">The news identifier</param>
        public override void DeleteNews(int newsId)
        {
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_NewsDelete");
            db.AddInParameter(dbCommand, "NewsID", DbType.Int32, newsId);
            db.ExecuteNonQuery(dbCommand);
        }

        /// <summary>
        /// Gets news item collection
        /// </summary>
        /// <param name="languageId">Language identifier. 0 if you want to get all news</param>
        /// <param name="newsCount">News item count. 0 if you want to get all news</param>
        /// <param name="showHidden">A value indicating whether to show hidden records</param>
        /// <returns>News item collection</returns>
        public override DBNewsCollection GetNews(int languageId, 
            int newsCount, bool showHidden)
        {
            var result = new DBNewsCollection();
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_NewsLoadAll");
            db.AddInParameter(dbCommand, "LanguageID", DbType.Int32, languageId);
            db.AddInParameter(dbCommand, "NewsCount", DbType.Int32, newsCount);
            db.AddInParameter(dbCommand, "ShowHidden", DbType.Boolean, showHidden);
            using (IDataReader dataReader = db.ExecuteReader(dbCommand))
            {
                while (dataReader.Read())
                {
                    var item = GetNewsFromReader(dataReader);
                    result.Add(item);
                }
            }

            return result;
        }

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
        public override DBNews InsertNews(int languageId, string title, string shortText,
            string fullText, bool published, bool allowComments, DateTime createdOn)
        {
            DBNews item = null;
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_NewsInsert");
            db.AddOutParameter(dbCommand, "NewsID", DbType.Int32, 0);
            db.AddInParameter(dbCommand, "LanguageID", DbType.Int32, languageId);
            db.AddInParameter(dbCommand, "Title", DbType.String, title);
            db.AddInParameter(dbCommand, "Short", DbType.String, shortText);
            db.AddInParameter(dbCommand, "Full", DbType.String, fullText);
            db.AddInParameter(dbCommand, "Published", DbType.Boolean, published);
            db.AddInParameter(dbCommand, "AllowComments", DbType.Boolean, allowComments);
            db.AddInParameter(dbCommand, "CreatedOn", DbType.DateTime, createdOn);
            if (db.ExecuteNonQuery(dbCommand) > 0)
            {
                int newsId = Convert.ToInt32(db.GetParameterValue(dbCommand, "@NewsID"));
                item = GetNewsById(newsId);
            }
            return item;
        }

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
        public override DBNews UpdateNews(int newsId, int languageId,
            string title, string shortText, string fullText, 
            bool published, bool allowComments, DateTime createdOn)
        {
            DBNews item = null;
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_NewsUpdate");
            db.AddInParameter(dbCommand, "NewsID", DbType.Int32, newsId);
            db.AddInParameter(dbCommand, "LanguageID", DbType.Int32, languageId);
            db.AddInParameter(dbCommand, "Title", DbType.String, title);
            db.AddInParameter(dbCommand, "Short", DbType.String, shortText);
            db.AddInParameter(dbCommand, "Full", DbType.String, fullText);
            db.AddInParameter(dbCommand, "Published", DbType.Boolean, published);
            db.AddInParameter(dbCommand, "AllowComments", DbType.Boolean, allowComments);
            db.AddInParameter(dbCommand, "CreatedOn", DbType.DateTime, createdOn);
            if (db.ExecuteNonQuery(dbCommand) > 0)
                item = GetNewsById(newsId);

            return item;
        }

        /// <summary>
        /// Gets a news comment
        /// </summary>
        /// <param name="newsCommentId">News comment identifer</param>
        /// <returns>News comment</returns>
        public override DBNewsComment GetNewsCommentById(int newsCommentId)
        {
            DBNewsComment item = null;
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_NewsCommentLoadByPrimaryKey");
            db.AddInParameter(dbCommand, "NewsCommentID", DbType.Int32, newsCommentId);
            using (IDataReader dataReader = db.ExecuteReader(dbCommand))
            {
                if (dataReader.Read())
                {
                    item = GetNewsCommentFromReader(dataReader);
                }
            }
            return item;
        }

        /// <summary>
        /// Gets a news comment collection by news identifier
        /// </summary>
        /// <param name="newsId">The news identifier</param>
        /// <returns>News comment collection</returns>
        public override DBNewsCommentCollection GetNewsCommentsByNewsId(int newsId)
        {
            var result = new DBNewsCommentCollection();
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_NewsCommentLoadByNewsID");
            db.AddInParameter(dbCommand, "NewsID", DbType.Int32, newsId);
            using (IDataReader dataReader = db.ExecuteReader(dbCommand))
            {
                while (dataReader.Read())
                {
                    var item = GetNewsCommentFromReader(dataReader);
                    result.Add(item);
                }
            }
            return result;
        }

        /// <summary>
        /// Deletes a news comment
        /// </summary>
        /// <param name="newsCommentId">The news comment identifier</param>
        public override void DeleteNewsComment(int newsCommentId)
        {
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_NewsCommentDelete");
            db.AddInParameter(dbCommand, "NewsCommentID", DbType.Int32, newsCommentId);
            db.ExecuteNonQuery(dbCommand);
        }

        /// <summary>
        /// Gets all news comments
        /// </summary>
        /// <returns>News comment collection</returns>
        public override DBNewsCommentCollection GetAllNewsComments()
        {
            var result = new DBNewsCommentCollection();
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_NewsCommentLoadAll");
            using (IDataReader dataReader = db.ExecuteReader(dbCommand))
            {
                while (dataReader.Read())
                {
                    var item = GetNewsCommentFromReader(dataReader);
                    result.Add(item);
                }
            }

            return result;
        }

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
        public override DBNewsComment InsertNewsComment(int newsId, int customerId, string ipAddress, 
            string title, string comment, DateTime createdOn)
        {
            DBNewsComment item = null;
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_NewsCommentInsert");
            db.AddOutParameter(dbCommand, "NewsCommentID", DbType.Int32, 0);
            db.AddInParameter(dbCommand, "NewsID", DbType.Int32, newsId);
            db.AddInParameter(dbCommand, "CustomerID", DbType.Int32, customerId);
            db.AddInParameter(dbCommand, "IPAddress", DbType.String, ipAddress);
            db.AddInParameter(dbCommand, "Title", DbType.String, title);
            db.AddInParameter(dbCommand, "Comment", DbType.String, comment);
            db.AddInParameter(dbCommand, "CreatedOn", DbType.DateTime, createdOn);
            if (db.ExecuteNonQuery(dbCommand) > 0)
            {
                int newsCommentId = Convert.ToInt32(db.GetParameterValue(dbCommand, "@NewsCommentID"));
                item = GetNewsCommentById(newsCommentId);
            }
            return item;
        }

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
        public override DBNewsComment UpdateNewsComment(int newsCommentId,
            int newsId, int customerId, string ipAddress, string title,
            string comment, DateTime createdOn)
        {
            DBNewsComment item = null;
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_NewsCommentUpdate");
            db.AddInParameter(dbCommand, "NewsCommentID", DbType.Int32, newsCommentId);
            db.AddInParameter(dbCommand, "NewsID", DbType.Int32, newsId);
            db.AddInParameter(dbCommand, "CustomerID", DbType.Int32, customerId);
            db.AddInParameter(dbCommand, "IPAddress", DbType.String, ipAddress);
            db.AddInParameter(dbCommand, "Title", DbType.String, title);
            db.AddInParameter(dbCommand, "Comment", DbType.String, comment);
            db.AddInParameter(dbCommand, "CreatedOn", DbType.DateTime, createdOn);

            if (db.ExecuteNonQuery(dbCommand) > 0)
                item = GetNewsCommentById(newsCommentId);

            return item;
        }
        #endregion
    }
}
