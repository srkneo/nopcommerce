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
using System.Configuration.Provider;
using System.Configuration;
using System.Web.Hosting;
using System.Collections.Specialized;
using System.Web.Configuration;

namespace NopSolutions.NopCommerce.DataAccess.Content.Topics
{
    /// <summary>
    /// Acts as a base class for deriving custom topic provider
    /// </summary>
    [DBProviderSectionName("nopDataProviders/TopicProvider")]
    public abstract partial class DBTopicProvider : BaseDBProvider
    {
        #region Methods

        /// <summary>
        /// Deletes a topic
        /// </summary>
        /// <param name="topicId">Topic identifier</param>
        public abstract void DeleteTopic(int topicId);

        /// <summary>
        /// Gets a topic by template identifier
        /// </summary>
        /// <param name="topicId">Topic identifier</param>
        /// <returns>Topic</returns>
        public abstract DBTopic GetTopicById(int topicId);

        /// <summary>
        /// Inserts a topic
        /// </summary>
        /// <param name="name">The name</param>
        /// <returns>Topic</returns>
        public abstract DBTopic InsertTopic(string name);

        /// <summary>
        /// Updates the topic
        /// </summary>
        /// <param name="topicId">The topic identifier</param>
        /// <param name="name">The name</param>
        /// <returns>Topic</returns>
        public abstract DBTopic UpdateTopic(int topicId, string name);

        /// <summary>
        /// Gets all topics
        /// </summary>
        /// <returns>Topic collection</returns>
        public abstract DBTopicCollection GetAllTopics();

        /// <summary>
        /// Gets a localized topic by identifier
        /// </summary>
        /// <param name="localizedTopicId">Localized topic identifier</param>
        /// <returns>Localized topic</returns>
        public abstract DBLocalizedTopic GetLocalizedTopicById(int localizedTopicId);

        /// <summary>
        /// Gets a localized topic by parent topic identifier and language identifier
        /// </summary>
        /// <param name="topicId">The topic identifier</param>
        /// <param name="languageId">Language identifier</param>
        /// <returns>Localized topic</returns>
        public abstract DBLocalizedTopic GetLocalizedTopic(int topicId, int languageId);
        
        /// <summary>
        /// Gets a localized topic by name and language identifier
        /// </summary>
        /// <param name="name">Topic name</param>
        /// <param name="languageId">Language identifier</param>
        /// <returns>Localized topic</returns>
        public abstract DBLocalizedTopic GetLocalizedTopic(string name, int languageId);

        /// <summary>
        /// Deletes a localized topic
        /// </summary>
        /// <param name="localizedTopicId">Topic identifier</param>
        public abstract void DeleteLocalizedTopic(int localizedTopicId);

        /// <summary>
        /// Gets all localized topics
        /// </summary>
        /// <param name="topicName">Topic name</param>
        /// <returns>Localized topic collection</returns>
        public abstract DBLocalizedTopicCollection GetAllLocalizedTopics(string topicName);

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
        public abstract DBLocalizedTopic InsertLocalizedTopic(int topicId,
            int languageId, string title, string body,
            DateTime createdOn, DateTime updatedOn,
            string metaKeywords, string metaDescription, string metaTitle);

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
        public abstract DBLocalizedTopic UpdateLocalizedTopic(int topicLocalizedId,
            int topicId, int languageId, string title, string body,
            DateTime createdOn, DateTime updatedOn,
            string metaKeywords, string metaDescription, string metaTitle);

        #endregion
    }
}

