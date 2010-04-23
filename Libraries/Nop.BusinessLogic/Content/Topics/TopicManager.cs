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
using System.Data;
using System.Data.Common;
using System.Globalization;
using System.Net;
using System.Net.Mail;
using System.Text;
using System.Web;
using NopSolutions.NopCommerce.BusinessLogic.Audit;
using NopSolutions.NopCommerce.BusinessLogic.Caching;
using NopSolutions.NopCommerce.BusinessLogic.Configuration.Settings;
using NopSolutions.NopCommerce.BusinessLogic.CustomerManagement;
using NopSolutions.NopCommerce.BusinessLogic.Directory;
using NopSolutions.NopCommerce.BusinessLogic.Localization;
using NopSolutions.NopCommerce.BusinessLogic.Orders;
using NopSolutions.NopCommerce.BusinessLogic.Payment;
using NopSolutions.NopCommerce.BusinessLogic.Products;
using NopSolutions.NopCommerce.BusinessLogic.Profile;
using NopSolutions.NopCommerce.BusinessLogic.SEO;
using NopSolutions.NopCommerce.BusinessLogic.Tax;
using NopSolutions.NopCommerce.BusinessLogic.Utils;
using NopSolutions.NopCommerce.DataAccess;
using NopSolutions.NopCommerce.DataAccess.Content.Topics;

namespace NopSolutions.NopCommerce.BusinessLogic.Content.Topics
{
    /// <summary>
    /// Message manager
    /// </summary>
    public partial class TopicManager
    {
        #region Utilities

        private static TopicCollection DBMapping(DBTopicCollection dbCollection)
        {
            if (dbCollection == null)
                return null;

            var collection = new TopicCollection();
            foreach (var dbItem in dbCollection)
            {
                var item = DBMapping(dbItem);
                collection.Add(item);
            }

            return collection;
        }

        private static Topic DBMapping(DBTopic dbItem)
        {
            if (dbItem == null)
                return null;

            var item = new Topic();
            item.TopicId = dbItem.TopicId;
            item.Name = dbItem.Name;

            return item;
        }

        private static LocalizedTopicCollection DBMapping(DBLocalizedTopicCollection dbCollection)
        {
            if (dbCollection == null)
                return null;

            var collection = new LocalizedTopicCollection();
            foreach (var dbItem in dbCollection)
            {
                var item = DBMapping(dbItem);
                collection.Add(item);
            }

            return collection;
        }

        private static LocalizedTopic DBMapping(DBLocalizedTopic dbItem)
        {
            if (dbItem == null)
                return null;

            var item = new LocalizedTopic();
            item.TopicLocalizedId = dbItem.TopicLocalizedId;
            item.TopicId = dbItem.TopicId;
            item.LanguageId = dbItem.LanguageId;
            item.Title = dbItem.Title;
            item.Body = dbItem.Body;
            item.CreatedOn = dbItem.CreatedOn;
            item.UpdatedOn = dbItem.UpdatedOn;
            item.MetaDescription = dbItem.MetaDescription;
            item.MetaKeywords = dbItem.MetaKeywords;
            item.MetaTitle = dbItem.MetaTitle;

            return item;
        }

        #endregion

        #region Methods

        /// <summary>
        /// Deletes a topic
        /// </summary>
        /// <param name="topicId">Topic identifier</param>
        public static void DeleteTopic(int topicId)
        {
            DBProviderManager<DBTopicProvider>.Provider.DeleteTopic(topicId);
        }

        /// <summary>
        /// Inserts a topic
        /// </summary>
        /// <param name="name">The name</param>
        /// <returns>Topic</returns>
        public static Topic InsertTopic(string name)
        {
            var dbItem = DBProviderManager<DBTopicProvider>.Provider.InsertTopic(name);
            var topic = DBMapping(dbItem);
            return topic;
        }

        /// <summary>
        /// Updates the topic
        /// </summary>
        /// <param name="topicId">The topic identifier</param>
        /// <param name="name">The name</param>
        /// <returns>Topic</returns>
        public static Topic UpdateTopic(int topicId, string name)
        {
            var dbItem = DBProviderManager<DBTopicProvider>.Provider.UpdateTopic(topicId, name);
            var topic = DBMapping(dbItem);
            return topic;
        }

        /// <summary>
        /// Gets a topic by template identifier
        /// </summary>
        /// <param name="topicId">topic identifier</param>
        /// <returns>topic</returns>
        public static Topic GetTopicById(int topicId)
        {
            if (topicId == 0)
                return null;

            var dbItem = DBProviderManager<DBTopicProvider>.Provider.GetTopicById(topicId);
            var Topic = DBMapping(dbItem);
            return Topic;
        }

        /// <summary>
        /// Gets all topics
        /// </summary>
        /// <returns>topic collection</returns>
        public static TopicCollection GetAllTopics()
        {
            var dbCollection = DBProviderManager<DBTopicProvider>.Provider.GetAllTopics();
            var collection = DBMapping(dbCollection);
            return collection;
        }

        /// <summary>
        /// Gets a localized topic by identifier
        /// </summary>
        /// <param name="localizedTopicId">Localized topic identifier</param>
        /// <returns>Localized topic</returns>
        public static LocalizedTopic GetLocalizedTopicById(int localizedTopicId)
        {
            if (localizedTopicId == 0)
                return null;

            var dbItem = DBProviderManager<DBTopicProvider>.Provider.GetLocalizedTopicById(localizedTopicId);
            var localizedTopic = DBMapping(dbItem);
            return localizedTopic;
        }

        /// <summary>
        /// Gets a localized topic by parent topic identifier and language identifier
        /// </summary>
        /// <param name="topicId">The topic identifier</param>
        /// <param name="languageId">Language identifier</param>
        /// <returns>Localized topic</returns>
        public static LocalizedTopic GetLocalizedTopic(int topicId, int languageId)
        {
            var dbItem = DBProviderManager<DBTopicProvider>.Provider.GetLocalizedTopic(topicId, languageId);
            var localizedTopic = DBMapping(dbItem);
            return localizedTopic;
        }
        
        /// <summary>
        /// Gets a localized topic by name and language identifier
        /// </summary>
        /// <param name="name">topic name</param>
        /// <param name="languageId">Language identifier</param>
        /// <returns>Localized topic</returns>
        public static LocalizedTopic GetLocalizedTopic(string name, int languageId)
        {
            var dbItem = DBProviderManager<DBTopicProvider>.Provider.GetLocalizedTopic(name, languageId);
            var localizedTopic = DBMapping(dbItem);
            return localizedTopic;
        }

        /// <summary>
        /// Deletes a localized topic
        /// </summary>
        /// <param name="localizedTopicId">topic identifier</param>
        public static void DeleteLocalizedTopic(int localizedTopicId)
        {
            DBProviderManager<DBTopicProvider>.Provider.DeleteLocalizedTopic(localizedTopicId);
        }

        /// <summary>
        /// Gets all localized topics
        /// </summary>
        /// <param name="topicName">topic name</param>
        /// <returns>Localized topic collection</returns>
        public static LocalizedTopicCollection GetAllLocalizedTopics(string topicName)
        {
            var dbCollection = DBProviderManager<DBTopicProvider>.Provider.GetAllLocalizedTopics(topicName);
            var localizedTopics = DBMapping(dbCollection);
            return localizedTopics;
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
        public static LocalizedTopic InsertLocalizedTopic(int topicId,
            int languageId, string title, string body,
            DateTime createdOn, DateTime updatedOn,
            string metaKeywords, string metaDescription, string metaTitle)
        {
            createdOn = DateTimeHelper.ConvertToUtcTime(createdOn);
            updatedOn = DateTimeHelper.ConvertToUtcTime(updatedOn);

            var dbItem = DBProviderManager<DBTopicProvider>.Provider.InsertLocalizedTopic(topicId,
                languageId, title, body, createdOn, updatedOn, metaKeywords, 
                metaDescription, metaTitle);
            var localizedTopic = DBMapping(dbItem);
            return localizedTopic;
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
        public static LocalizedTopic UpdateLocalizedTopic(int topicLocalizedId,
            int topicId, int languageId, string title, string body,
            DateTime createdOn, DateTime updatedOn,
            string metaKeywords, string metaDescription, string metaTitle)
        {
            createdOn = DateTimeHelper.ConvertToUtcTime(createdOn);
            updatedOn = DateTimeHelper.ConvertToUtcTime(updatedOn);

            var dbItem = DBProviderManager<DBTopicProvider>.Provider.UpdateLocalizedTopic(topicLocalizedId,
                topicId, languageId, title, body, createdOn, updatedOn, 
                metaKeywords, metaDescription, metaTitle);
            var localizedTopic = DBMapping(dbItem);
            return localizedTopic;
        }

        #endregion
    }
}