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
using System.Data;
using System.Configuration;
using System.Collections;
using System.Web;
using System.Web.Security;
using System.Web.UI;
using System.Web.UI.WebControls;
using System.Web.UI.WebControls.WebParts;
using System.Web.UI.HtmlControls;
using NopSolutions.NopCommerce.Common.Utils;
using NopSolutions.NopCommerce.BusinessLogic.Messages;
using NopSolutions.NopCommerce.BusinessLogic.Directory;
using System.Collections.Generic;
using System.Text;

namespace NopSolutions.NopCommerce.Web.Administration.Modules
{
    public partial class MessageTemplateDetailsControl : BaseNopAdministrationUserControl
    {
        private void BindData()
        {
            Language language = LanguageManager.GetLanguageById(this.LanguageId);
            MessageTemplate messageTemplate = MessageManager.GetMessageTemplateById(this.MessageTemplateId);
            if (language != null && messageTemplate != null)
            {
                StringBuilder allowedTokensString = new StringBuilder();
                string[] allowedTokens = MessageManager.GetListOfAllowedTokens();
                for (int i = 0; i < allowedTokens.Length; i++)
                {
                    string token = allowedTokens[i];
                    allowedTokensString.Append(token);
                    if (i != allowedTokens.Length - 1)
                        allowedTokensString.Append(", ");
                }
                this.lblAllowedTokens.Text = allowedTokensString.ToString();

                this.lblLanguage.Text = language.Name;
                this.lblTemplate.Text = messageTemplate.Name;
                LocalizedMessageTemplate localizedMessageTemplate = MessageManager.GetLocalizedMessageTemplate(messageTemplate.Name, this.LanguageId);
                if (localizedMessageTemplate != null)
                {
                    this.cbActive.Checked = localizedMessageTemplate.IsActive;
                    this.txtBCCEmailAddresses.Text = localizedMessageTemplate.BccEmailAddresses;
                    this.txtSubject.Text = localizedMessageTemplate.Subject;
                    this.txtBody.Content = localizedMessageTemplate.Body;
                }
                else
                {
                    this.SaveButton.Text = "Save";
                    this.DeleteButton.Visible = false;
                }
            }
            else
                Response.Redirect("MessageTemplates.aspx");
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
                    MessageTemplate messageTemplate = MessageManager.GetMessageTemplateById(this.MessageTemplateId);
                    if (messageTemplate != null)
                    {
                        LocalizedMessageTemplate localizedMessageTemplate = MessageManager.GetLocalizedMessageTemplate(messageTemplate.Name, this.LanguageId);
                        if (localizedMessageTemplate != null)
                        {
                            localizedMessageTemplate = MessageManager.UpdateLocalizedMessageTemplate(localizedMessageTemplate.MessageTemplateLocalizedId,
                                localizedMessageTemplate.MessageTemplateId, localizedMessageTemplate.LanguageId, 
                                txtBCCEmailAddresses.Text, txtSubject.Text, txtBody.Content, cbActive.Checked);
                            Response.Redirect("MessageTemplateDetails.aspx?MessageTemplateID=" + localizedMessageTemplate.MessageTemplateId.ToString() + "&LanguageID=" + localizedMessageTemplate.LanguageId.ToString());
                        }
                        else
                        {
                            localizedMessageTemplate = MessageManager.InsertLocalizedMessageTemplate(this.MessageTemplateId,
                                this.LanguageId, txtBCCEmailAddresses.Text, txtSubject.Text, txtBody.Content, cbActive.Checked);
                            Response.Redirect("MessageTemplateDetails.aspx?MessageTemplateID=" + this.MessageTemplateId.ToString() + "&LanguageID=" + this.LanguageId.ToString());
                        }
                    }
                    else
                        Response.Redirect("MessageTemplates.aspx");
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
                MessageTemplate messageTemplate = MessageManager.GetMessageTemplateById(this.MessageTemplateId);
                if (messageTemplate != null)
                {
                    LocalizedMessageTemplate localizedMessageTemplate = MessageManager.GetLocalizedMessageTemplate(messageTemplate.Name, this.LanguageId);
                    if (localizedMessageTemplate != null)
                        MessageManager.DeleteLocalizedMessageTemplate(localizedMessageTemplate.MessageTemplateLocalizedId);
                }

                Response.Redirect("MessageTemplates.aspx");
            }
            catch (Exception exc)
            {
                ProcessException(exc);
            }
        }

        public int MessageTemplateId
        {
            get
            {
                return CommonHelper.QueryStringInt("MessageTemplateId");
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