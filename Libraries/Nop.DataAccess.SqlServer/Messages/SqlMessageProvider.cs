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

namespace NopSolutions.NopCommerce.DataAccess.Messages
{
    /// <summary>
    /// Message provider for SQL Server
    /// </summary>
    public partial class SqlMessageProvider : DBMessageProvider
    {
        #region Fields
        private string _sqlConnectionString;
        #endregion

        #region Utilities
        private DBMessageTemplate GetMessageTemplateFromReader(IDataReader dataReader)
        {
            var item = new DBMessageTemplate();
            item.MessageTemplateId = NopSqlDataHelper.GetInt(dataReader, "MessageTemplateID");
            item.Name = NopSqlDataHelper.GetString(dataReader, "Name");
            return item;
        }

        private DBLocalizedMessageTemplate GetLocalizedMessageTemplateFromReader(IDataReader dataReader)
        {
            var item = new DBLocalizedMessageTemplate();
            item.MessageTemplateLocalizedId = NopSqlDataHelper.GetInt(dataReader, "MessageTemplateLocalizedID");
            item.MessageTemplateId = NopSqlDataHelper.GetInt(dataReader, "MessageTemplateID");
            item.LanguageId = NopSqlDataHelper.GetInt(dataReader, "LanguageID");
            item.BccEmailAddresses = NopSqlDataHelper.GetString(dataReader, "BCCEmailAddresses");
            item.Subject = NopSqlDataHelper.GetString(dataReader, "Subject");
            item.Body = NopSqlDataHelper.GetString(dataReader, "Body");
            item.IsActive = NopSqlDataHelper.GetBoolean(dataReader, "IsActive");
            return item;
        }

        private DBQueuedEmail GetQueuedEmailFromReader(IDataReader dataReader)
        {
            var item = new DBQueuedEmail();
            item.QueuedEmailId = NopSqlDataHelper.GetInt(dataReader, "QueuedEmailID");
            item.Priority = NopSqlDataHelper.GetInt(dataReader, "Priority");
            item.From = NopSqlDataHelper.GetString(dataReader, "From");
            item.FromName = NopSqlDataHelper.GetString(dataReader, "FromName");
            item.To = NopSqlDataHelper.GetString(dataReader, "To");
            item.ToName = NopSqlDataHelper.GetString(dataReader, "ToName");
            item.CC = NopSqlDataHelper.GetString(dataReader, "Cc");
            item.Bcc = NopSqlDataHelper.GetString(dataReader, "Bcc");
            item.Subject = NopSqlDataHelper.GetString(dataReader, "Subject");
            item.Body = NopSqlDataHelper.GetString(dataReader, "Body");
            item.CreatedOn = NopSqlDataHelper.GetUtcDateTime(dataReader, "CreatedOn");
            item.SendTries = NopSqlDataHelper.GetInt(dataReader, "SendTries");
            item.SentOn = NopSqlDataHelper.GetNullableUtcDateTime(dataReader, "SentOn");
            return item;
        }

        private DBNewsLetterSubscription GetNewsLetterSubscriptionFromReader(IDataReader dataReader)
        {
            var item = new DBNewsLetterSubscription();

            item.NewsLetterSubscriptionId = NopSqlDataHelper.GetInt(dataReader, "NewsLetterSubscriptionID");
            item.NewsLetterSubscriptionGuid = NopSqlDataHelper.GetGuid(dataReader, "NewsLetterSubscriptionGuid");
            item.Email = NopSqlDataHelper.GetString(dataReader, "Email");
            item.IsActive = NopSqlDataHelper.GetBoolean(dataReader, "Active");
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
        /// Gets a message template by template identifier
        /// </summary>
        /// <param name="messageTemplateId">Message template identifier</param>
        /// <returns>Message template</returns>
        public override DBMessageTemplate GetMessageTemplateById(int messageTemplateId)
        {
            DBMessageTemplate item = null;
            if (messageTemplateId == 0)
                return item;
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_MessageTemplateLoadByPrimaryKey");
            db.AddInParameter(dbCommand, "MessageTemplateID", DbType.Int32, messageTemplateId);
            using (IDataReader dataReader = db.ExecuteReader(dbCommand))
            {
                if (dataReader.Read())
                {
                    item = GetMessageTemplateFromReader(dataReader);
                }
            }
            return item;
        }

        /// <summary>
        /// Gets all message templates
        /// </summary>
        /// <returns>Message template collection</returns>
        public override DBMessageTemplateCollection GetAllMessageTemplates()
        {
            var result = new DBMessageTemplateCollection();
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_MessageTemplateLoadAll");
            using (IDataReader dataReader = db.ExecuteReader(dbCommand))
            {
                while (dataReader.Read())
                {
                    var item = GetMessageTemplateFromReader(dataReader);
                    result.Add(item);
                }
            }
            return result;
        }

        /// <summary>
        /// Gets a localized message template by identifier
        /// </summary>
        /// <param name="localizedMessageTemplateId">Localized message template identifier</param>
        /// <returns>Localized message template</returns>
        public override DBLocalizedMessageTemplate GetLocalizedMessageTemplateById(int localizedMessageTemplateId)
        {
            DBLocalizedMessageTemplate item = null;
            if (localizedMessageTemplateId == 0)
                return item;
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_MessageTemplateLocalizedLoadByPrimaryKey");
            db.AddInParameter(dbCommand, "MessageTemplateLocalizedID", DbType.Int32, localizedMessageTemplateId);
            using (IDataReader dataReader = db.ExecuteReader(dbCommand))
            {
                if (dataReader.Read())
                {
                    item = GetLocalizedMessageTemplateFromReader(dataReader);
                }
            }
            return item;
        }

        /// <summary>
        /// Gets a localized message template by name and language identifier
        /// </summary>
        /// <param name="name">Message template name</param>
        /// <param name="languageId">Language identifier</param>
        /// <returns>Localized message template</returns>
        public override DBLocalizedMessageTemplate GetLocalizedMessageTemplate(string name,
            int languageId)
        {
            DBLocalizedMessageTemplate item = null;
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_MessageTemplateLocalizedLoadByNameAndLanguageID");
            db.AddInParameter(dbCommand, "Name", DbType.String, name);
            db.AddInParameter(dbCommand, "LanguageID", DbType.Int32, languageId);
            using (IDataReader dataReader = db.ExecuteReader(dbCommand))
            {
                if (dataReader.Read())
                {
                    item = GetLocalizedMessageTemplateFromReader(dataReader);
                }
            }
            return item;
        }

        /// <summary>
        /// Deletes a localized message template
        /// </summary>
        /// <param name="localizedMessageTemplateId">Message template identifier</param>
        public override void DeleteLocalizedMessageTemplate(int localizedMessageTemplateId)
        {
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_MessageTemplateLocalizedDelete");
            db.AddInParameter(dbCommand, "MessageTemplateLocalizedID", DbType.Int32, localizedMessageTemplateId);
            db.ExecuteNonQuery(dbCommand);
        }

        /// <summary>
        /// Gets all localized message templates
        /// </summary>
        /// <param name="messageTemplateName">Message template name</param>
        /// <returns>Localized message template collection</returns>
        public override DBLocalizedMessageTemplateCollection GetAllLocalizedMessageTemplates(string messageTemplateName)
        {
            var result = new DBLocalizedMessageTemplateCollection();
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_MessageTemplateLocalizedLoadAllByName");
            db.AddInParameter(dbCommand, "Name", DbType.String, messageTemplateName);
            using (IDataReader dataReader = db.ExecuteReader(dbCommand))
            {
                while (dataReader.Read())
                {
                    var item = GetLocalizedMessageTemplateFromReader(dataReader);
                    result.Add(item);
                }
            }
            return result;
        }

        /// <summary>
        /// Inserts a localized message template
        /// </summary>
        /// <param name="messageTemplateId">The message template identifier</param>
        /// <param name="languageId">The language identifier</param>
        /// <param name="bccEmailAddresses">The BCC Email addresses</param>
        /// <param name="subject">The subject</param>
        /// <param name="body">The body</param>
        /// <param name="isActive">A value indicating whether the message template is active</param>
        /// <returns>Localized message template</returns>
        public override DBLocalizedMessageTemplate InsertLocalizedMessageTemplate(int messageTemplateId,
            int languageId, string bccEmailAddresses, string subject, string body, bool isActive)
        {
            DBLocalizedMessageTemplate item = null;
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_MessageTemplateLocalizedInsert");
            db.AddOutParameter(dbCommand, "MessageTemplateLocalizedID", DbType.Int32, 0);
            db.AddInParameter(dbCommand, "MessageTemplateID", DbType.Int32, messageTemplateId);
            db.AddInParameter(dbCommand, "LanguageID", DbType.Int32, languageId);
            db.AddInParameter(dbCommand, "BCCEmailAddresses", DbType.String, bccEmailAddresses);
            db.AddInParameter(dbCommand, "Subject", DbType.String, subject);
            db.AddInParameter(dbCommand, "Body", DbType.String, body);
            db.AddInParameter(dbCommand, "IsActive", DbType.Boolean, isActive);
            if (db.ExecuteNonQuery(dbCommand) > 0)
            {
                int messageTemplateLocalizedId = Convert.ToInt32(db.GetParameterValue(dbCommand, "@MessageTemplateLocalizedID"));
                item = GetLocalizedMessageTemplateById(messageTemplateLocalizedId);
            }
            return item;
        }

        /// <summary>
        /// Updates the localized message template
        /// </summary>
        /// <param name="messageTemplateLocalizedId">The localized message template identifier</param>
        /// <param name="messageTemplateId">The message template identifier</param>
        /// <param name="languageId">The language identifier</param>
        /// <param name="bccEmailAddresses">The BCC Email addresses</param>
        /// <param name="subject">The subject</param>
        /// <param name="body">The body</param>
        /// <param name="isActive">A value indicating whether the message template is active</param>
        /// <returns>Localized message template</returns>
        public override DBLocalizedMessageTemplate UpdateLocalizedMessageTemplate(int messageTemplateLocalizedId,
            int messageTemplateId, int languageId, string bccEmailAddresses,
            string subject, string body, bool isActive)
        {
            DBLocalizedMessageTemplate item = null;
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_MessageTemplateLocalizedUpdate");
            db.AddInParameter(dbCommand, "MessageTemplateLocalizedID", DbType.Int32, messageTemplateLocalizedId);
            db.AddInParameter(dbCommand, "MessageTemplateID", DbType.Int32, messageTemplateId);
            db.AddInParameter(dbCommand, "LanguageID", DbType.Int32, languageId);
            db.AddInParameter(dbCommand, "BCCEmailAddresses", DbType.String, bccEmailAddresses);
            db.AddInParameter(dbCommand, "Subject", DbType.String, subject);
            db.AddInParameter(dbCommand, "Body", DbType.String, body);
            db.AddInParameter(dbCommand, "IsActive", DbType.Boolean, isActive);
            if (db.ExecuteNonQuery(dbCommand) > 0)
                item = GetLocalizedMessageTemplateById(messageTemplateLocalizedId);

            return item;
        }

        /// <summary>
        /// Gets a queued email by identifier
        /// </summary>
        /// <param name="queuedEmailId">Email item identifier</param>
        /// <returns>Email item</returns>
        public override DBQueuedEmail GetQueuedEmailById(int queuedEmailId)
        {
            DBQueuedEmail item = null;
            if (queuedEmailId == 0)
                return item;
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_QueuedEmailLoadByPrimaryKey");
            db.AddInParameter(dbCommand, "QueuedEmailID", DbType.Int32, queuedEmailId);
            using (IDataReader dataReader = db.ExecuteReader(dbCommand))
            {
                if (dataReader.Read())
                {
                    item = GetQueuedEmailFromReader(dataReader);
                }
            }
            return item;
        }

        /// <summary>
        /// Deletes a queued email
        /// </summary>
        /// <param name="queuedEmailId">Email item identifier</param>
        public override void DeleteQueuedEmail(int queuedEmailId)
        {
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_QueuedEmailDelete");
            db.AddInParameter(dbCommand, "QueuedEmailID", DbType.Int32, queuedEmailId);
            db.ExecuteNonQuery(dbCommand);
        }

        /// <summary>
        /// Gets all queued emails
        /// </summary>
        /// <param name="fromEmail">From Email</param>
        /// <param name="toEmail">To Email</param>
        /// <param name="startTime">The start time</param>
        /// <param name="endTime">The end time</param>
        /// <param name="queuedEmailCount">Email item count. 0 if you want to get all items</param>
        /// <param name="loadNotSentItemsOnly">A value indicating whether to load only not sent emails</param>
        /// <param name="maxSendTries">Maximum send tries</param>
        /// <returns>Email item collection</returns>
        public override DBQueuedEmailCollection GetAllQueuedEmails(string fromEmail,
            string toEmail, DateTime? startTime, DateTime? endTime,
            int queuedEmailCount, bool loadNotSentItemsOnly, int maxSendTries)
        {
            var result = new DBQueuedEmailCollection();
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_QueuedEmailLoadAll");
            db.AddInParameter(dbCommand, "FromEmail", DbType.String, fromEmail);
            db.AddInParameter(dbCommand, "ToEmail", DbType.String, toEmail);
            if (startTime.HasValue)
                db.AddInParameter(dbCommand, "StartTime", DbType.DateTime, startTime.Value);
            else
                db.AddInParameter(dbCommand, "StartTime", DbType.DateTime, DBNull.Value);
            if (endTime.HasValue)
                db.AddInParameter(dbCommand, "EndTime", DbType.DateTime, endTime.Value);
            else
                db.AddInParameter(dbCommand, "EndTime", DbType.DateTime, DBNull.Value);
            db.AddInParameter(dbCommand, "QueuedEmailCount", DbType.Int32, queuedEmailCount);
            db.AddInParameter(dbCommand, "LoadNotSentItemsOnly", DbType.Boolean, loadNotSentItemsOnly);
            db.AddInParameter(dbCommand, "MaxSendTries", DbType.Int32, maxSendTries);
            using (IDataReader dataReader = db.ExecuteReader(dbCommand))
            {
                while (dataReader.Read())
                {
                    var item = GetQueuedEmailFromReader(dataReader);
                    result.Add(item);
                }
            }

            return result;
        }

        /// <summary>
        /// Inserts a queued email
        /// </summary>
        /// <param name="priority">The priority</param>
        /// <param name="from">From</param>
        /// <param name="fromName">From name</param>
        /// <param name="to">To</param>
        /// <param name="toName">To name</param>
        /// <param name="cc">Cc</param>
        /// <param name="bcc">Bcc</param>
        /// <param name="subject">Subject</param>
        /// <param name="body">Body</param>
        /// <param name="createdOn">The date and time of item creation</param>
        /// <param name="sendTries">The send tries</param>
        /// <param name="sentOn">The sent date and time. Null if email is not sent yet</param>
        /// <returns>Queued email</returns>
        public override DBQueuedEmail InsertQueuedEmail(int priority, string from,
            string fromName, string to, string toName, string cc, string bcc,
            string subject, string body, DateTime createdOn, int sendTries, DateTime? sentOn)
        {
            DBQueuedEmail item = null;
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_QueuedEmailInsert");
            db.AddOutParameter(dbCommand, "QueuedEmailID", DbType.Int32, 0);
            db.AddInParameter(dbCommand, "Priority", DbType.Int32, priority);
            db.AddInParameter(dbCommand, "From", DbType.String, from);
            db.AddInParameter(dbCommand, "FromName", DbType.String, fromName);
            db.AddInParameter(dbCommand, "To", DbType.String, to);
            db.AddInParameter(dbCommand, "ToName", DbType.String, toName);
            db.AddInParameter(dbCommand, "Cc", DbType.String, cc);
            db.AddInParameter(dbCommand, "Bcc", DbType.String, bcc);
            db.AddInParameter(dbCommand, "Subject", DbType.String, subject);
            db.AddInParameter(dbCommand, "Body", DbType.String, body);
            db.AddInParameter(dbCommand, "CreatedOn", DbType.DateTime, createdOn);
            db.AddInParameter(dbCommand, "SendTries", DbType.Int32, sendTries);
            if (sentOn.HasValue)
                db.AddInParameter(dbCommand, "SentOn", DbType.DateTime, sentOn.Value);
            else
                db.AddInParameter(dbCommand, "SentOn", DbType.DateTime, DBNull.Value);

            if (db.ExecuteNonQuery(dbCommand) > 0)
            {
                int queuedEmailId = Convert.ToInt32(db.GetParameterValue(dbCommand, "@QueuedEmailID"));
                item = GetQueuedEmailById(queuedEmailId);
            }
            return item;
        }

        /// <summary>
        /// Updates a queued email
        /// </summary>
        /// <param name="queuedEmailId">Email item identifier</param>
        /// <param name="priority">The priority</param>
        /// <param name="from">From</param>
        /// <param name="fromName">From name</param>
        /// <param name="to">To</param>
        /// <param name="toName">To name</param>
        /// <param name="cc">Cc</param>
        /// <param name="bcc">Bcc</param>
        /// <param name="subject">Subject</param>
        /// <param name="body">Body</param>
        /// <param name="createdOn">The date and time of item creation</param>
        /// <param name="sendTries">The send tries</param>
        /// <param name="sentOn">The sent date and time. Null if email is not sent yet</param>
        /// <returns>Queued email</returns>
        public override DBQueuedEmail UpdateQueuedEmail(int queuedEmailId,
            int priority, string from,
            string fromName, string to, string toName, string cc, string bcc,
            string subject, string body, DateTime createdOn, int sendTries, DateTime? sentOn)
        {
            DBQueuedEmail item = null;
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_QueuedEmailUpdate");
            db.AddInParameter(dbCommand, "QueuedEmailID", DbType.Int32, queuedEmailId);
            db.AddInParameter(dbCommand, "Priority", DbType.Int32, priority);
            db.AddInParameter(dbCommand, "From", DbType.String, from);
            db.AddInParameter(dbCommand, "FromName", DbType.String, fromName);
            db.AddInParameter(dbCommand, "To", DbType.String, to);
            db.AddInParameter(dbCommand, "ToName", DbType.String, toName);
            db.AddInParameter(dbCommand, "Cc", DbType.String, cc);
            db.AddInParameter(dbCommand, "Bcc", DbType.String, bcc);
            db.AddInParameter(dbCommand, "Subject", DbType.String, subject);
            db.AddInParameter(dbCommand, "Body", DbType.String, body);
            db.AddInParameter(dbCommand, "CreatedOn", DbType.DateTime, createdOn);
            db.AddInParameter(dbCommand, "SendTries", DbType.Int32, sendTries);
            if (sentOn.HasValue)
                db.AddInParameter(dbCommand, "SentOn", DbType.DateTime, sentOn.Value);
            else
                db.AddInParameter(dbCommand, "SentOn", DbType.DateTime, DBNull.Value);

            if (db.ExecuteNonQuery(dbCommand) > 0)
                item = GetQueuedEmailById(queuedEmailId);

            return item;
        }

        /// <summary>
        /// Inserts the new newsletter subscription
        /// </summary>
        /// <param name="newsLetterSubscriptionGuid">The newsletter subscription GUID</param>
        /// <param name="email">The subscriber email</param>
        /// <param name="isActive">A value indicating whether subscription is active</param>
        /// <param name="createdOn">The date and time of instance creation</param>
        /// <returns>NewsLetterSubscription entity</returns>
        public override DBNewsLetterSubscription InsertNewsLetterSubscription(Guid newsLetterSubscriptionGuid,
            string email, bool isActive, DateTime createdOn)
        {
            DBNewsLetterSubscription item = null;
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_NewsLetterSubscriptionInsert");

            db.AddOutParameter(dbCommand, "NewsLetterSubscriptionID", DbType.Int32, 0);
            db.AddInParameter(dbCommand, "NewsLetterSubscriptionGuid", DbType.Guid, newsLetterSubscriptionGuid);
            db.AddInParameter(dbCommand, "Email", DbType.String, email);
            db.AddInParameter(dbCommand, "Active", DbType.Boolean, isActive);
            db.AddInParameter(dbCommand, "CreatedOn", DbType.DateTime, createdOn);

            if(db.ExecuteNonQuery(dbCommand) > 0)
            {
                item = GetNewsLetterSubscriptionById(Convert.ToInt32(db.GetParameterValue(dbCommand, "@NewsLetterSubscriptionID")));
            }
            return item;
        }

        /// <summary>
        /// Gets the newsletter subscription by newsletter subscription identifier
        /// </summary>
        /// <param name="newsLetterSubscriptionId">The newsletter subscription identifier</param>
        /// <returns>NewsLetterSubscription entity</returns>
        public override DBNewsLetterSubscription GetNewsLetterSubscriptionById(int newsLetterSubscriptionId)
        {
            DBNewsLetterSubscription item = null;
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_NewsLetterSubscriptionLoadByPrimaryKey");
            db.AddInParameter(dbCommand, "NewsLetterSubscriptionID", DbType.Int32, newsLetterSubscriptionId);
            using(IDataReader dataReader = db.ExecuteReader(dbCommand))
            {
                if(dataReader.Read())
                {
                    item = GetNewsLetterSubscriptionFromReader(dataReader);
                }
            }
            return item;
        }

        /// <summary>
        /// Gets the newsletter subscription by newsletter subscription GUID
        /// </summary>
        /// <param name="newsLetterSubscriptionGuid">The newsletter subscription GUID</param>
        /// <returns>NewsLetterSubscription entity</returns>
        public override DBNewsLetterSubscription GetNewsLetterSubscriptionByGuid(Guid newsLetterSubscriptionGuid)
        {
            DBNewsLetterSubscription item = null;
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_NewsLetterSubscriptionLoadByGuid");
            db.AddInParameter(dbCommand, "NewsLetterSubscriptionGuid", DbType.Guid, newsLetterSubscriptionGuid);
            using(IDataReader dataReader = db.ExecuteReader(dbCommand))
            {
                if(dataReader.Read())
                {
                    item = GetNewsLetterSubscriptionFromReader(dataReader);
                }
            }
            return item;
        }

        /// <summary>
        /// Gets the newsletter subscription by email
        /// </summary>
        /// <param name="email">The Email</param>
        /// <returns>NewsLetterSubscription entity</returns>
        public override DBNewsLetterSubscription GetNewsLetterSubscriptionByEmail(string email)
        {
            DBNewsLetterSubscription item = null;
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_NewsLetterSubscriptionLoadByEmail");
            db.AddInParameter(dbCommand, "Email", DbType.String, email);
            using(IDataReader dataReader = db.ExecuteReader(dbCommand))
            {
                if(dataReader.Read())
                {
                    item = GetNewsLetterSubscriptionFromReader(dataReader);
                }
            }
            return item;
        }

        /// <summary>
        /// Gets the newsletter subscription collection
        /// </summary>
        /// <param name="showHidden">A value indicating whether the not active subscriptions should be loaded</param>
        /// <returns>NewsLetterSubscription entity collection</returns>
        public override DBNewsLetterSubscriptionCollection GetAllNewsLetterSubscriptions(bool showHidden)
        {
            var result = new DBNewsLetterSubscriptionCollection();
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_NewsLetterSubscriptionLoadAll");
            db.AddInParameter(dbCommand, "ShowHidden", DbType.Boolean, showHidden);
            using(IDataReader dataReader = db.ExecuteReader(dbCommand))
            {
                while(dataReader.Read())
                {
                    var item = GetNewsLetterSubscriptionFromReader(dataReader);
                    result.Add(item);
                }
            }
            return result;
        }

        /// <summary>
        /// Updates the newsletter subscription
        /// </summary>
        /// <param name="newsLetterSubscriptionId">The newsletter subscription identifier</param>
        /// <param name="newsLetterSubscriptionGuid">The newsletter subscription GUID</param>
        /// <param name="email">The subscriber email</param>
        /// <param name="isActive">A value indicating whether subscription is active</param>
        /// <param name="createdOn">The date and time of instance creation</param>
        /// <returns>NewsLetterSubscription entity</returns>
        public override DBNewsLetterSubscription UpdateNewsLetterSubscription(int newsLetterSubscriptionId,
            Guid newsLetterSubscriptionGuid, string email, bool isActive, DateTime createdOn)
        {
            DBNewsLetterSubscription item = null;
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_NewsLetterSubscriptionUpdate");
            db.AddInParameter(dbCommand, "NewsLetterSubscriptionID", DbType.Int32, newsLetterSubscriptionId);
            db.AddInParameter(dbCommand, "NewsLetterSubscriptionGuid", DbType.Guid, newsLetterSubscriptionGuid);
            db.AddInParameter(dbCommand, "Email", DbType.String, email);
            db.AddInParameter(dbCommand, "Active", DbType.Boolean, isActive);
            db.AddInParameter(dbCommand, "CreatedOn", DbType.DateTime, createdOn);

            if(db.ExecuteNonQuery(dbCommand) > 0)
            {
                item = GetNewsLetterSubscriptionById(newsLetterSubscriptionId);
            }
            return item;
        }

        /// <summary>
        /// Deletes the newsletter subscription
        /// </summary>
        /// <param name="newsLetterSubscriptionId">The newsletter subscription identifier</param>
        public override void DeleteNewsLetterSubscription(int newsLetterSubscriptionId)
        {
            Database db = NopSqlDataHelper.CreateConnection(_sqlConnectionString);
            DbCommand dbCommand = db.GetStoredProcCommand("Nop_NewsLetterSubscriptionDelete");
            db.AddInParameter(dbCommand, "NewsLetterSubscriptionID", DbType.Int32, newsLetterSubscriptionId);
            db.ExecuteNonQuery(dbCommand);
        }
        #endregion
    }
}

