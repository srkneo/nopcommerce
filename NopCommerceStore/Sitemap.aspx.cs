using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Text;
using NopSolutions.NopCommerce.BusinessLogic.Categories;
using NopSolutions.NopCommerce.BusinessLogic.SEO;
using NopSolutions.NopCommerce.BusinessLogic.Manufacturers;
using NopSolutions.NopCommerce.BusinessLogic.Products;
using NopSolutions.NopCommerce.BusinessLogic.Content.Topics;
using NopSolutions.NopCommerce.BusinessLogic;
using NopSolutions.NopCommerce.Common.Utils;
using NopSolutions.NopCommerce.BusinessLogic.Configuration.Settings;
using NopSolutions.NopCommerce.BusinessLogic.Audit;

namespace NopSolutions.NopCommerce.Web
{
    public partial class Sitemap : BaseNopPage
    {
        protected override void OnPreRender(EventArgs e)
        {
            BindData();
            base.OnPreRender(e);
        }

        protected void BindData()
        {
            try
            {
                StringBuilder sb = new StringBuilder();
                if (SettingManager.GetSettingValueBoolean("Sitemap.IncludeCategories", true))
                {
                    sb.Append("<li>");
                    sb.AppendFormat("<span>{0}</span>", GetLocaleResourceString("Sitemap.Categories"));
                    sb.Append("<ul>");
                    WriteCategories(sb, CategoryManager.GetAllCategories(0));
                    sb.Append("</ul>");
                    sb.Append("</li>");
                }
                if (SettingManager.GetSettingValueBoolean("Sitemap.IncludeManufacturers", true))
                {
                    sb.Append("<li>");
                    sb.AppendFormat("<span>{0}</span>", GetLocaleResourceString("Sitemap.Manufacturers"));
                    sb.Append("<ul>");
                    WriteManufacturers(sb, ManufacturerManager.GetAllManufacturers());
                    sb.Append("</ul>");
                    sb.Append("</li>");
                }
                if (SettingManager.GetSettingValueBoolean("Sitemap.IncludeProducts", true))
                {
                    sb.Append("<li>");
                    sb.AppendFormat("<span>{0}</span>", GetLocaleResourceString("Sitemap.Products"));
                    sb.Append("<ul>");
                    WriteProducts(sb, ProductManager.GetAllProducts());
                    sb.Append("</ul>");
                    sb.Append("</li>");
                }
                if (SettingManager.GetSettingValueBoolean("Sitemap.IncludeTopics", true))
                {
                    sb.Append("<li>");
                    sb.AppendFormat("<span>{0}</span>", GetLocaleResourceString("Sitemap.Topics"));
                    sb.Append("<ul>");
                    WriteTopics(sb, TopicManager.GetAllTopics());
                    sb.Append("</ul>");
                    sb.Append("</li>");
                }
                string otherPages = SettingManager.GetSettingValue("Sitemap.OtherPages");
                if (!String.IsNullOrEmpty(otherPages))
                {
                    sb.Append("<li>");
                    sb.AppendFormat("<span>{0}</span>", GetLocaleResourceString("Sitemap.OtherPages"));
                    sb.Append("<ul>");
                    WriteOtherPages(sb, otherPages.Split(new char[] { ',' }, StringSplitOptions.RemoveEmptyEntries));
                    sb.Append("</ul>");
                    sb.Append("</li>");
                }

                lSitemapContent.Text = sb.ToString();
            }
            catch (Exception ex)
            {
                lSitemapContent.Text = ex.Message;
                LogManager.InsertLog(LogTypeEnum.CommonError, ex.Message, ex);
            }
        }

        private void WriteCategories(StringBuilder sb, CategoryCollection categoryCollection)
        {
            foreach(Category category in categoryCollection)
            {
                sb.Append("<li>");
                sb.AppendFormat("<a href=\"{0}\">{1}</a>", SEOHelper.GetCategoryURL(category.CategoryID), Server.HtmlEncode(category.Name));
                CategoryCollection childCategoryCollection = CategoryManager.GetAllCategories(category.CategoryID);
                if(childCategoryCollection.Count > 0)
                {
                    sb.Append("<ul>");
                    WriteCategories(sb, childCategoryCollection);
                    sb.Append("</ul>");
                }
                sb.Append("</li>");
            }
        }

        private void WriteManufacturers(StringBuilder sb, ManufacturerCollection manufacturerCollection)
        {
            foreach(Manufacturer manufacturer in manufacturerCollection)
            {
                sb.AppendFormat("<li><a href=\"{0}\">{1}</a></li>", SEOHelper.GetManufacturerURL(manufacturer), Server.HtmlEncode(manufacturer.Name));
            }
        }

        private void WriteProducts(StringBuilder sb, ProductCollection productCollection)
        {
            foreach(Product product in productCollection)
            {
                sb.AppendFormat("<li><a href=\"{0}\">{1}</a></li>", SEOHelper.GetProductURL(product), Server.HtmlEncode(product.Name));
            }
        }

        private void WriteTopics(StringBuilder sb, TopicCollection topicCollection)
        {
            foreach(Topic topic in topicCollection)
            {
                LocalizedTopic localizedTopic = TopicManager.GetLocalizedTopic(topic.TopicID, NopContext.Current.WorkingLanguage.LanguageID);
                if(localizedTopic != null)
                {
                    sb.AppendFormat("<li><a href=\"{0}\">{1}</a></li>", SEOHelper.GetTopicUrl(localizedTopic.TopicID, localizedTopic.Title), (String.IsNullOrEmpty(localizedTopic.Title) ? localizedTopic.TopicID.ToString() : Server.HtmlEncode(localizedTopic.Title)));
                }
            }
        }

        private void WriteOtherPages(StringBuilder sb, string[] pages)
        {
            foreach(string page in pages)
            {
                sb.AppendFormat("<li><a href=\"{0}{1}\">{2}</a></li>", CommonHelper.GetStoreLocation(), page.Trim(), Server.HtmlEncode(page));
            }
        }
    }
}
