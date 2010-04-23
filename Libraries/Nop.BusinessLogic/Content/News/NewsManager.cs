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
using System.Web;
using NopSolutions.NopCommerce.BusinessLogic.Caching;
using NopSolutions.NopCommerce.BusinessLogic.Configuration.Settings;
using NopSolutions.NopCommerce.BusinessLogic.CustomerManagement;
using NopSolutions.NopCommerce.BusinessLogic.Localization;
using NopSolutions.NopCommerce.BusinessLogic.Messages;
using NopSolutions.NopCommerce.BusinessLogic.Profile;
using NopSolutions.NopCommerce.BusinessLogic.Utils.Html;
using NopSolutions.NopCommerce.Common.Utils.Html;
using NopSolutions.NopCommerce.DataAccess;
using NopSolutions.NopCommerce.DataAccess.Content.NewsManagement;

namespace NopSolutions.NopCommerce.BusinessLogic.Content.NewsManagement
{
    /// <summary>
    /// News manager
    /// </summary>
    public partial class NewsManager
    {
        #region Constants
        private const string NEWS_BY_ID_KEY = "Nop.news.id-{0}";
        private const string NEWS_PATTERN_KEY = "Nop.news.";
        #endregion

        #region Utilities
        private static NewsCollection DBMapping(DBNewsCollection dbCollection)
        {
            if (dbCollection == null)
                return null;

            var collection = new NewsCollection();
            foreach (var dbItem in dbCollection)
            {
                var item = DBMapping(dbItem);
                collection.Add(item);
            }

            return collection;
        }

        private static News DBMapping(DBNews dbItem)
        {
            if (dbItem == null)
                return null;

            var item = new News();
            item.NewsId = dbItem.NewsId;
            item.LanguageId = dbItem.LanguageId;
            item.Title = dbItem.Title;
            item.Short = dbItem.Short;
            item.Full = dbItem.Full;
            item.Published = dbItem.Published;
            item.AllowComments = dbItem.AllowComments;
            item.CreatedOn = dbItem.CreatedOn;

            return item;
        }

        private static NewsCommentCollection DBMapping(DBNewsCommentCollection dbCollection)
        {
            if (dbCollection == null)
                return null;

            var collection = new NewsCommentCollection();
            foreach (var dbItem in dbCollection)
            {
                var item = DBMapping(dbItem);
                collection.Add(item);
            }

            return collection;
        }

        private static NewsComment DBMapping(DBNewsComment dbItem)
        {
            if (dbItem == null)
                return null;

            var item = new NewsComment();
            item.NewsCommentId = dbItem.NewsCommentId;
            item.NewsId = dbItem.NewsId;
            item.CustomerId = dbItem.CustomerId;
            item.Title = dbItem.Title;
            item.Comment = dbItem.Comment;
            item.CreatedOn = dbItem.CreatedOn;

            return item;
        }
        #endregion

        #region Methods
        /// <summary>
        /// Gets a news
        /// </summary>
        /// <param name="newsId">The news identifier</param>
        /// <returns>News</returns>
        public static News GetNewsById(int newsId)
        {
            if (newsId == 0)
                return null;

            string key = string.Format(NEWS_BY_ID_KEY, newsId);
            object obj2 = NopCache.Get(key);
            if (NewsManager.CacheEnabled && (obj2 != null))
            {
                return (News)obj2;
            }

            var dbItem = DBProviderManager<DBNewsProvider>.Provider.GetNewsById(newsId);
            var news = DBMapping(dbItem);

            if (NewsManager.CacheEnabled)
            {
                NopCache.Max(key, news);
            }
            return news;
        }

        /// <summary>
        /// Deletes a news
        /// </summary>
        /// <param name="newsId">The news identifier</param>
        public static void DeleteNews(int newsId)
        {
            DBProviderManager<DBNewsProvider>.Provider.DeleteNews(newsId);
            if (NewsManager.CacheEnabled)
            {
                NopCache.RemoveByPattern(NEWS_PATTERN_KEY);
            }
        }

        /// <summary>
        /// Gets news item collection
        /// </summary>
        /// <param name="languageId">Language identifier. 0 if you want to get all news</param>
        /// <param name="newsCount">News item count. 0 if you want to get all news</param>
        /// <returns>News item collection</returns>
        public static NewsCollection GetNews(int languageId, int newsCount)
        {
            bool showHidden = NopContext.Current.IsAdmin;
            var dbCollection = DBProviderManager<DBNewsProvider>.Provider.GetNews(languageId, newsCount, showHidden);
            var collection = DBMapping(dbCollection);
            return collection;
        }

        /// <summary>
        /// Gets all news
        /// </summary>
        /// <param name="languageId">Language identifier. 0 if you want to get all news</param>
        /// <returns>News item collection</returns>
        public static NewsCollection GetAllNews(int languageId)
        {
            return GetNews(languageId, 0);
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
        public static News InsertNews(int languageId, string title, string shortText,
            string fullText, bool published, bool allowComments, DateTime createdOn)
        {
            createdOn = DateTimeHelper.ConvertToUtcTime(createdOn);

            var dbItem = DBProviderManager<DBNewsProvider>.Provider.InsertNews(languageId, 
                title, shortText, fullText, published, allowComments, createdOn);
            var news = DBMapping(dbItem);

            if (NewsManager.CacheEnabled)
            {
                NopCache.RemoveByPattern(NEWS_PATTERN_KEY);
            }

            return news;
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
        public static News UpdateNews(int newsId, int languageId,
            string title, string shortText, string fullText,
            bool published, bool allowComments, DateTime createdOn)
        {
            createdOn = DateTimeHelper.ConvertToUtcTime(createdOn);

            var dbItem = DBProviderManager<DBNewsProvider>.Provider.UpdateNews(newsId, 
                languageId, title, shortText, fullText, published, allowComments, createdOn);
            var news = DBMapping(dbItem);

            if (NewsManager.CacheEnabled)
            {
                NopCache.RemoveByPattern(NEWS_PATTERN_KEY);
            }

            return news;
        }

        /// <summary>
        /// Gets a news comment
        /// </summary>
        /// <param name="newsCommentId">News comment identifer</param>
        /// <returns>News comment</returns>
        public static NewsComment GetNewsCommentById(int newsCommentId)
        {
            if (newsCommentId == 0)
                return null;

            var dbItem = DBProviderManager<DBNewsProvider>.Provider.GetNewsCommentById(newsCommentId);
            var newsComment = DBMapping(dbItem);
            return newsComment;
        }

        /// <summary>
        /// Gets a news comment collection by news identifier
        /// </summary>
        /// <param name="newsId">The news identifier</param>
        /// <returns>News comment collection</returns>
        public static NewsCommentCollection GetNewsCommentsByNewsId(int newsId)
        {
            var dbCollection = DBProviderManager<DBNewsProvider>.Provider.GetNewsCommentsByNewsId(newsId);
            var collection = DBMapping(dbCollection);
            return collection;
        }

        /// <summary>
        /// Deletes a news comment
        /// </summary>
        /// <param name="newsCommentId">The news comment identifier</param>
        public static void DeleteNewsComment(int newsCommentId)
        {
            DBProviderManager<DBNewsProvider>.Provider.DeleteNewsComment(newsCommentId);
        }

        /// <summary>
        /// Gets all news comments
        /// </summary>
        /// <returns>News comment collection</returns>
        public static NewsCommentCollection GetAllNewsComments()
        {
            var dbCollection = DBProviderManager<DBNewsProvider>.Provider.GetAllNewsComments();
            var collection = DBMapping(dbCollection);
            return collection;
        }

        /// <summary>
        /// Inserts a news comment
        /// </summary>
        /// <param name="newsId">The news identifier</param>
        /// <param name="customerId">The customer identifier</param>
        /// <param name="title">The title</param>
        /// <param name="comment">The comment</param>
        /// <param name="createdOn">The date and time of instance creation</param>
        /// <returns>News comment</returns>
        public static NewsComment InsertNewsComment(int newsId, int customerId,
            string title, string comment, DateTime createdOn)
        {
            return InsertNewsComment(newsId, customerId, title, comment, 
                createdOn, NewsManager.NotifyAboutNewNewsComments);
        }

        /// <summary>
        /// Inserts a news comment
        /// </summary>
        /// <param name="newsId">The news identifier</param>
        /// <param name="customerId">The customer identifier</param>
        /// <param name="title">The title</param>
        /// <param name="comment">The comment</param>
        /// <param name="createdOn">The date and time of instance creation</param>
        /// <param name="notify">A value indicating whether to notify the store owner</param>
        /// <returns>News comment</returns>
        public static NewsComment InsertNewsComment(int newsId, int customerId,
            string title, string comment, DateTime createdOn, bool notify)
        {
            createdOn = DateTimeHelper.ConvertToUtcTime(createdOn);

            var dbItem = DBProviderManager<DBNewsProvider>.Provider.InsertNewsComment(newsId, 
                customerId, title, comment, createdOn);
            var newsComment = DBMapping(dbItem);
            
            if (notify)
            {
                MessageManager.SendNewsCommentNotificationMessage(newsComment, LocalizationManager.DefaultAdminLanguage.LanguageId);
            }

            return newsComment;
        }

        /// <summary>
        /// Updates the news comment
        /// </summary>
        /// <param name="newsCommentId">The news comment identifier</param>
        /// <param name="newsId">The news identifier</param>
        /// <param name="customerId">The customer identifier</param>
        /// <param name="title">The title</param>
        /// <param name="comment">The comment</param>
        /// <param name="createdOn">The date and time of instance creation</param>
        /// <returns>News comment</returns>
        public static NewsComment UpdateNewsComment(int newsCommentId,
            int newsId, int customerId, string title,
            string comment, DateTime createdOn)
        {
            createdOn = DateTimeHelper.ConvertToUtcTime(createdOn);

            var dbItem = DBProviderManager<DBNewsProvider>.Provider.UpdateNewsComment(newsCommentId, 
                newsId, customerId, title, comment, createdOn);
            var newsComment = DBMapping(dbItem);
            return newsComment;
        }

        /// <summary>
        /// Formats the text
        /// </summary>
        /// <param name="text">Text</param>
        /// <returns>Formatted text</returns>
        public static string FormatCommentText(string text)
        {
            if (String.IsNullOrEmpty(text))
                return string.Empty;

            text = HtmlHelper.FormatText(text, false, true, false, false, false, false);
            return text;
        }
        
        #endregion

        #region Property
        /// <summary>
        /// Gets a value indicating whether cache is enabled
        /// </summary>
        public static bool CacheEnabled
        {
            get
            {
                return SettingManager.GetSettingValueBoolean("Cache.NewsManager.CacheEnabled");
            }
        }
        
        /// <summary>
        /// Gets or sets a value indicating whether news are enabled
        /// </summary>
        public static bool NewsEnabled
        {
            get
            {
                bool newsEnabled = SettingManager.GetSettingValueBoolean("News.NewsEnabled");
                return newsEnabled;
            }
            set
            {
                SettingManager.SetParam("News.NewsEnabled", value.ToString());
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether not registered user can leave comments
        /// </summary>
        public static bool AllowNotRegisteredUsersToLeaveComments
        {
            get
            {
                return SettingManager.GetSettingValueBoolean("News.AllowNotRegisteredUsersToLeaveComments");
            }
            set
            {
                SettingManager.SetParam("News.AllowNotRegisteredUsersToLeaveComments", value.ToString());
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether to notify about new news comments
        /// </summary>
        public static bool NotifyAboutNewNewsComments
        {
            get
            {
                return SettingManager.GetSettingValueBoolean("News.NotifyAboutNewNewsComments");
            }
            set
            {
                SettingManager.SetParam("News.NotifyAboutNewNewsComments", value.ToString());
            }
        }

        /// <summary>
        /// Gets or sets a value indicating whether to show news on the main page
        /// </summary>
        public static bool ShowNewsOnMainPage
        {
            get
            {
                bool showNewsOnMainPage = SettingManager.GetSettingValueBoolean("Display.ShowNewsOnMainPage");
                return showNewsOnMainPage;
            }
            set
            {
                SettingManager.SetParam("Display.ShowNewsOnMainPage", value.ToString());
            }
        }

        /// <summary>
        /// Gets or sets a value indicating news count displayed on the main page
        /// </summary>
        public static int MainPageNewsCount
        {
            get
            {
                int mainPageNewsCount = SettingManager.GetSettingValueInteger("Display.MainPageNewsCount");
                return mainPageNewsCount;
            }
            set
            {
                SettingManager.SetParam("Display.MainPageNewsCount", value.ToString());
            }
        }
        #endregion
    }
}
