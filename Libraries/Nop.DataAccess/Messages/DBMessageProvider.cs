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

namespace NopSolutions.NopCommerce.DataAccess.Messages
{
    /// <summary>
    /// Acts as a base class for deriving custom message template provider
    /// </summary>
    [DBProviderSectionName("nopDataProviders/MessageProvider")]
    public abstract partial class DBMessageProvider : BaseDBProvider
    {
        #region Methods

        /// <summary>
        /// Gets a message template by template identifier
        /// </summary>
        /// <param name="MessageTemplateID">Message template identifier</param>
        /// <returns>Message template</returns>
        public abstract DBMessageTemplate GetMessageTemplateByID(int MessageTemplateID);

        /// <summary>
        /// Gets all message templates
        /// </summary>
        /// <returns>Message template collection</returns>
        public abstract DBMessageTemplateCollection GetAllMessageTemplates();

        /// <summary>
        /// Gets a localized message template by identifier
        /// </summary>
        /// <param name="LocalizedMessageTemplateID">Localized message template identifier</param>
        /// <returns>Localized message template</returns>
        public abstract DBLocalizedMessageTemplate GetLocalizedMessageTemplateByID(int LocalizedMessageTemplateID);

        /// <summary>
        /// Gets a localized message template by name and language identifier
        /// </summary>
        /// <param name="Name">Message template name</param>
        /// <param name="LanguageID">Language identifier</param>
        /// <returns>Localized message template</returns>
        public abstract DBLocalizedMessageTemplate GetLocalizedMessageTemplate(string Name, int LanguageID);

        /// <summary>
        /// Deletes a localized message template
        /// </summary>
        /// <param name="LocalizedMessageTemplateID">Message template identifier</param>
        public abstract void DeleteLocalizedMessageTemplate(int LocalizedMessageTemplateID);

        /// <summary>
        /// Gets all localized message templates
        /// </summary>
        /// <param name="MessageTemplatesName">Message template name</param>
        /// <returns>Localized message template collection</returns>
        public abstract DBLocalizedMessageTemplateCollection GetAllLocalizedMessageTemplates(string MessageTemplatesName);

        /// <summary>
        /// Inserts a localized message template
        /// </summary>
        /// <param name="MessageTemplateID">The message template identifier</param>
        /// <param name="LanguageID">The language identifier</param>
        /// <param name="BCCEmailAddresses">The BCC Email addresses</param>
        /// <param name="Subject">The subject</param>
        /// <param name="Body">The body</param>
        /// <returns>Localized message template</returns>
        public abstract DBLocalizedMessageTemplate InsertLocalizedMessageTemplate(int MessageTemplateID,
            int LanguageID, string BCCEmailAddresses, string Subject, string Body);

        /// <summary>
        /// Updates the localized message template
        /// </summary>
        /// <param name="MessageTemplateLocalizedID">The localized message template identifier</param>
        /// <param name="MessageTemplateID">The message template identifier</param>
        /// <param name="LanguageID">The language identifier</param>
        /// <param name="BCCEmailAddresses">The BCC Email addresses</param>
        /// <param name="Subject">The subject</param>
        /// <param name="Body">The body</param>
        /// <returns>Localized message template</returns>
        public abstract DBLocalizedMessageTemplate UpdateLocalizedMessageTemplate(int MessageTemplateLocalizedID,
            int MessageTemplateID, int LanguageID, string BCCEmailAddresses,
            string Subject, string Body);

        /// <summary>
        /// Gets a queued email by identifier
        /// </summary>
        /// <param name="QueuedEmailID">Email item identifier</param>
        /// <returns>Email item</returns>
        public abstract DBQueuedEmail GetQueuedEmailByID(int QueuedEmailID);

        /// <summary>
        /// Deletes a queued email
        /// </summary>
        /// <param name="QueuedEmailID">Email item identifier</param>
        public abstract void DeleteQueuedEmail(int QueuedEmailID);

        /// <summary>
        /// Gets all queued emails
        /// </summary>
        /// <param name="FromEmail">From Email</param>
        /// <param name="ToEmail">To Email</param>
        /// <param name="StartTime">The start time</param>
        /// <param name="EndTime">The end time</param>
        /// <param name="QueuedEmailCount">Email item count. 0 if you want to get all items</param>
        /// <param name="LoadNotSentItemsOnly">A value indicating whether to load only not sent emails</param>
        /// <param name="MaxSendTries">Maximum send tries</param>
        /// <returns>Email item collection</returns>
        public abstract DBQueuedEmailCollection GetAllQueuedEmails(string FromEmail,
            string ToEmail, DateTime? StartTime, DateTime? EndTime,
            int QueuedEmailCount, bool LoadNotSentItemsOnly, int MaxSendTries);

        /// <summary>
        /// Inserts a queued email
        /// </summary>
        /// <param name="Priority">The priority</param>
        /// <param name="From">From</param>
        /// <param name="FromName">From name</param>
        /// <param name="To">To</param>
        /// <param name="ToName">To name</param>
        /// <param name="Cc">Cc</param>
        /// <param name="Bcc">Bcc</param>
        /// <param name="Subject">Subject</param>
        /// <param name="Body">Body</param>
        /// <param name="CreatedOn">The date and time of item creation</param>
        /// <param name="SendTries">The send tries</param>
        /// <param name="SentOn">The sent date and time. Null if email is not sent yet</param>
        /// <returns>Queued email</returns>
        public abstract DBQueuedEmail InsertQueuedEmail(int Priority, string From,
            string FromName, string To, string ToName, string Cc, string Bcc,
            string Subject, string Body, DateTime CreatedOn, int SendTries, DateTime? SentOn);

        /// <summary>
        /// Updates a queued email
        /// </summary>
        /// <param name="QueuedEmailID">Email item identifier</param>
        /// <param name="Priority">The priority</param>
        /// <param name="From">From</param>
        /// <param name="FromName">From name</param>
        /// <param name="To">To</param>
        /// <param name="ToName">To name</param>
        /// <param name="Cc">Cc</param>
        /// <param name="Bcc">Bcc</param>
        /// <param name="Subject">Subject</param>
        /// <param name="Body">Body</param>
        /// <param name="CreatedOn">The date and time of item creation</param>
        /// <param name="SendTries">The send tries</param>
        /// <param name="SentOn">The sent date and time. Null if email is not sent yet</param>
        /// <returns>Queued email</returns>
        public abstract DBQueuedEmail UpdateQueuedEmail(int QueuedEmailID, int Priority, string From,
            string FromName, string To, string ToName, string Cc, string Bcc,
            string Subject, string Body, DateTime CreatedOn, int SendTries, DateTime? SentOn);

        /// <summary>
        /// Inserts the new newsletter subscription
        /// </summary>
        /// <param name="NewsLetterSubscriptionGuid">The newsletter subscription GUID</param>
        /// <param name="Email">The subscriber email</param>
        /// <param name="IsActive">A value indicating whether subscription is active</param>
        /// <param name="CreatedOn">The date and time of instance creation</param>
        /// <returns>NewsLetterSubscription entity</returns>
        public abstract DBNewsLetterSubscription InsertNewsLetterSubscription(Guid NewsLetterSubscriptionGuid, string Email, bool IsActive, DateTime CreatedOn);

        /// <summary>
        /// Gets the newsletter subscription by newsletter subscription identifier
        /// </summary>
        /// <param name="NewsLetterSubscriptionID">The newsletter subscription identifier</param>
        /// <returns>NewsLetterSubscription entity</returns>
        public abstract DBNewsLetterSubscription GetNewsLetterSubscriptionByID(int NewsLetterSubscriptionID);

        /// <summary>
        /// Gets the newsletter subscription by newsletter subscription GUID
        /// </summary>
        /// <param name="NewsLetterSubscriptionGuid">The newsletter subscription GUID</param>
        /// <returns>NewsLetterSubscription entity</returns>
        public abstract DBNewsLetterSubscription GetNewsLetterSubscriptionByGUID(Guid NewsLetterSubscriptionGuid);

        /// <summary>
        /// Gets the newsletter subscription by email
        /// </summary>
        /// <param name="Email">The Email</param>
        /// <returns>NewsLetterSubscription entity</returns>
        public abstract DBNewsLetterSubscription GetNewsLetterSubscriptionByEmail(string Email);

        /// <summary>
        /// Gets the newsletter subscription collection
        /// </summary>
        /// <param name="ShowHidden">A value indicating whether the not active subscriptions should be loaded</param>
        /// <returns>NewsLetterSubscription entity collection</returns>
        public abstract DBNewsLetterSubscriptionCollection GetAllNewsLetterSubscriptions(bool ShowHidden);

        /// <summary>
        /// Updates the newsletter subscription
        /// </summary>
        /// <param name="NewsLetterSubscriptionID">The newsletter subscription identifier</param>
        /// <param name="NewsLetterSubscriptionGuid">The newsletter subscription GUID</param>
        /// <param name="Email">The subscriber email</param>
        /// <param name="IsActive">A value indicating whether subscription is active</param>
        /// <param name="CreatedOn">The date and time of instance creation</param>
        /// <returns>NewsLetterSubscription entity</returns>
        public abstract DBNewsLetterSubscription UpdateNewsLetterSubscription(int NewsLetterSubscriptionID, Guid NewsLetterSubscriptionGuid, string Email, bool IsActive, DateTime CreatedOn);

        /// <summary>
        /// Deletes the newsletter subscription
        /// </summary>
        /// <param name="NewsLetterSubscriptionID">The newsletter subscription identifier</param>
        public abstract void DeleteNewsLetterSubscription(int NewsLetterSubscriptionID);
        #endregion
    }
}

