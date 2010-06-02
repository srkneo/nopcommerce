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
using System.Collections;
using System.Collections.Generic;
using System.Configuration;
using System.Data;
using System.Text;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.HtmlControls;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using NopSolutions.NopCommerce.BusinessLogic.Content.Topics;
using NopSolutions.NopCommerce.BusinessLogic.Directory;
using NopSolutions.NopCommerce.BusinessLogic.Profile;
using NopSolutions.NopCommerce.BusinessLogic.SEO;
using NopSolutions.NopCommerce.Common.Utils;

namespace NopSolutions.NopCommerce.Web.Administration.Modules
{
    public partial class TopicLocalizedDetailsControl : BaseNopAdministrationUserControl
    {
        private void BindData()
        {
            Language language = LanguageManager.GetLanguageById(this.LanguageId);
            Topic topic = TopicManager.GetTopicById(this.TopicId);
            if (language != null && topic != null)
            {
                this.lblLanguage.Text = language.Name;
                this.lblTopic.Text = topic.Name;
                LocalizedTopic localizedTopic = TopicManager.GetLocalizedTopic(topic.Name, this.LanguageId);
                if (localizedTopic != null)
                {
                    this.txtTitle.Text = localizedTopic.Title;
                    string topicURL = SEOHelper.GetTopicUrl(localizedTopic.TopicId, localizedTopic.Title);
                    this.hlURL.NavigateUrl = topicURL;
                    this.hlURL.Text = topicURL;
                    this.txtBody.Content = localizedTopic.Body;

                    this.pnlCreatedOn.Visible = true;
                    this.pnlUpdatedOn.Visible = true;
                    this.lblCreatedOn.Text = DateTimeHelper.ConvertToUserTime(localizedTopic.CreatedOn).ToString();
                    this.lblUpdatedOn.Text = DateTimeHelper.ConvertToUserTime(localizedTopic.UpdatedOn).ToString();

                    this.txtMetaKeywords.Text = localizedTopic.MetaKeywords;
                    this.txtMetaDescription.Text = localizedTopic.MetaDescription;
                    this.txtMetaTitle.Text = localizedTopic.MetaTitle;
                }
                else
                {
                    this.DeleteButton.Visible = false;

                    this.pnlCreatedOn.Visible = false;
                    this.pnlUpdatedOn.Visible = false;
                }
            }
            else
                Response.Redirect("Topics.aspx");
        }

        protected void Page_Load(object sender, EventArgs e)
        {
            if (!Page.IsPostBack)
            {
                this.BindData();
            }
        }

        protected void SaveButton_Click(object sender, EventArgs e)
        {
            if (Page.IsValid)
            {
                try
                {
                    Topic topic = TopicManager.GetTopicById(this.TopicId);
                    if (topic != null)
                    {
                        DateTime nowDT = DateTime.Now;
                        LocalizedTopic localizedTopic = TopicManager.GetLocalizedTopic(topic.Name, this.LanguageId);
                        if (localizedTopic != null)
                        {
                            localizedTopic = TopicManager.UpdateLocalizedTopic(localizedTopic.TopicLocalizedId,
                                localizedTopic.TopicId, localizedTopic.LanguageId, txtTitle.Text, txtBody.Content,
                                localizedTopic.CreatedOn, nowDT, txtMetaKeywords.Text, txtMetaDescription.Text, txtMetaTitle.Text);
                            Response.Redirect("TopicLocalizedDetails.aspx?TopicID=" + localizedTopic.TopicId.ToString() + "&LanguageID=" + localizedTopic.LanguageId.ToString());
                        }
                        else
                        {
                            localizedTopic = TopicManager.InsertLocalizedTopic(this.TopicId,
                                this.LanguageId, txtTitle.Text, txtBody.Content, nowDT, nowDT,
                                txtMetaKeywords.Text, txtMetaDescription.Text, txtMetaTitle.Text);
                            Response.Redirect("TopicLocalizedDetails.aspx?TopicID=" + this.TopicId.ToString() + "&LanguageID=" + this.LanguageId.ToString());
                        }
                    }
                    else
                        Response.Redirect("Topics.aspx");
                }
                catch (Exception exc)
                {
                    ProcessException(exc);
                }
            }
        }

        protected void DeleteButton_Click(object sender, EventArgs e)
        {
            try
            {
                Topic topic = TopicManager.GetTopicById(this.TopicId);
                if (topic != null)
                {
                    LocalizedTopic localizedTopic = TopicManager.GetLocalizedTopic(topic.Name, this.LanguageId);
                    if (localizedTopic != null)
                        TopicManager.DeleteLocalizedTopic(localizedTopic.TopicLocalizedId);
                }

                Response.Redirect("Topics.aspx");
            }
            catch (Exception exc)
            {
                ProcessException(exc);
            }
        }

        public int TopicId
        {
            get
            {
                return CommonHelper.QueryStringInt("TopicId");
            }
        }

        public int LanguageId
        {
            get
            {
                return CommonHelper.QueryStringInt("LanguageId");
            }
        }
    }
}