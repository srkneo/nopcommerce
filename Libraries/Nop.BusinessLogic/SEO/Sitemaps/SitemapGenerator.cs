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
using System.Globalization;
using System.IO;
using System.Security.Cryptography;
using System.Text;
using System.Text.RegularExpressions;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Xml;
using NopSolutions.NopCommerce.BusinessLogic.Categories;
using NopSolutions.NopCommerce.BusinessLogic.Configuration;
using NopSolutions.NopCommerce.BusinessLogic.Configuration.Settings;
using NopSolutions.NopCommerce.BusinessLogic.Content.Blog;
using NopSolutions.NopCommerce.BusinessLogic.Content.NewsManagement;
using NopSolutions.NopCommerce.BusinessLogic.Content.Topics;
using NopSolutions.NopCommerce.BusinessLogic.Manufacturers;
using NopSolutions.NopCommerce.BusinessLogic.Products;
using NopSolutions.NopCommerce.BusinessLogic.Utils;
using NopSolutions.NopCommerce.Common.Utils;
using NopSolutions.NopCommerce.Common.Xml;

namespace NopSolutions.NopCommerce.BusinessLogic.SEO.Sitemaps
{
    /// <summary>
    /// Represents a sitemap generator
    /// </summary>
    public partial class SitemapGenerator : BaseSitemapGenerator
    {
        #region Utilities

        /// <summary>
        /// Method that is overridden, that handles creation of child urls.
        /// Use the method WriteUrlLocation() within this method.
        /// </summary>
        protected override void GenerateUrlNodes()
        {
            bool IncludeCategories = SettingManager.GetSettingValueBoolean("SEO.Sitemaps.IncludeCategories", true);
            bool IncludeManufacturers = SettingManager.GetSettingValueBoolean("SEO.Sitemaps.IncludeManufacturers", false);
            bool IncludeProducts = SettingManager.GetSettingValueBoolean("SEO.Sitemaps.IncludeProducts", false);
            bool IncludeTopics = SettingManager.GetSettingValueBoolean("SEO.Sitemaps.IncludeTopics", false);
            string OtherPages = SettingManager.GetSettingValue("SEO.Sitemaps.OtherPages");
            
            if (IncludeCategories)
            {
                WriteCategories(0);
            }

            if (IncludeManufacturers)
            {
                WriteManufacturers();
            }

            if (IncludeProducts)
            {
                WriteProducts();
            }

            if (IncludeTopics)
            {
                WriteTopics();
            }

            WriteOtherPages(OtherPages);
        }

        private void WriteCategories(int ParentCategoryID)
        {
            var categories = CategoryManager.GetAllCategories(ParentCategoryID, false);
            foreach (var category in categories)
            {
                var url = SEOHelper.GetCategoryURL(category.CategoryID);
                var updateFrequency = UpdateFrequency.weekly;
                var updateTime = category.UpdatedOn;
                WriteUrlLocation(url, updateFrequency, updateTime);

                WriteCategories(category.CategoryID);
            }
        }

        private void WriteManufacturers()
        {
            var manufacturers = ManufacturerManager.GetAllManufacturers(false);
            foreach (var manufacturer in manufacturers)
            {
                var url = SEOHelper.GetManufacturerURL(manufacturer);
                var updateFrequency = UpdateFrequency.weekly;
                var updateTime = manufacturer.UpdatedOn;
                WriteUrlLocation(url, updateFrequency, updateTime);
            }
        }

        private void WriteProducts()
        {
            var products = ProductManager.GetAllProducts(false);
            foreach (var product in products)
            {
                var url = SEOHelper.GetProductURL(product);
                var updateFrequency = UpdateFrequency.weekly;
                var updateTime = product.UpdatedOn;
                WriteUrlLocation(url, updateFrequency, updateTime);
            }
        }

        private void WriteTopics()
        {
            var topics = TopicManager.GetAllTopics();
            foreach (Topic topic in topics)
            {
                var localizedTopics = TopicManager.GetAllLocalizedTopics(topic.Name);
                if (localizedTopics.Count > 0)
                {
                    //UNDONE add topic of one language only (they have the same URL now)
                    var localizedTopic = localizedTopics[0];
                    var url = SEOHelper.GetTopicUrl(localizedTopic.TopicID, localizedTopic.Title);
                    var updateFrequency = UpdateFrequency.weekly;
                    var updateTime = localizedTopic.UpdatedOn;
                    WriteUrlLocation(url, updateFrequency, updateTime);
                }
            }
        }

        private void WriteOtherPages(string OtherPages)
        {
            if (String.IsNullOrEmpty(OtherPages))
                return;

            string[] pages = OtherPages.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries);
            foreach (string page in pages)
            {
                string url = CommonHelper.GetStoreLocation() + page.Trim();
                var updateFrequency = UpdateFrequency.weekly;
                var updateTime = DateTime.UtcNow;
                WriteUrlLocation(url, updateFrequency, updateTime);
            }
        }

        #endregion
    }
}
