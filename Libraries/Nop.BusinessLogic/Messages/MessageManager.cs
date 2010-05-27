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
using NopSolutions.NopCommerce.BusinessLogic.Content.Blog;
using NopSolutions.NopCommerce.BusinessLogic.Content.Forums;
using NopSolutions.NopCommerce.BusinessLogic.Content.NewsManagement;
using NopSolutions.NopCommerce.BusinessLogic.CustomerManagement;
using NopSolutions.NopCommerce.BusinessLogic.Directory;
using NopSolutions.NopCommerce.BusinessLogic.Localization;
using NopSolutions.NopCommerce.BusinessLogic.Media;
using NopSolutions.NopCommerce.BusinessLogic.Orders;
using NopSolutions.NopCommerce.BusinessLogic.Payment;
using NopSolutions.NopCommerce.BusinessLogic.Products;
using NopSolutions.NopCommerce.BusinessLogic.Profile;
using NopSolutions.NopCommerce.BusinessLogic.SEO;
using NopSolutions.NopCommerce.BusinessLogic.Shipping;
using NopSolutions.NopCommerce.BusinessLogic.Tax;
using NopSolutions.NopCommerce.BusinessLogic.Utils;
using NopSolutions.NopCommerce.Common;
using NopSolutions.NopCommerce.Common.Utils;
using NopSolutions.NopCommerce.Common.Utils.Html;
using NopSolutions.NopCommerce.DataAccess;
using NopSolutions.NopCommerce.DataAccess.Messages;
using System.Text.RegularExpressions;

namespace NopSolutions.NopCommerce.BusinessLogic.Messages
{
    /// <summary>
    /// Message manager
    /// </summary>
    public partial class MessageManager
    {
        #region Utilities

        private static MessageTemplateCollection DBMapping(DBMessageTemplateCollection dbCollection)
        {
            if (dbCollection == null)
                return null;

            var collection = new MessageTemplateCollection();
            foreach (var dbItem in dbCollection)
            {
                var item = DBMapping(dbItem);
                collection.Add(item);
            }

            return collection;
        }

        private static MessageTemplate DBMapping(DBMessageTemplate dbItem)
        {
            if (dbItem == null)
                return null;

            var item = new MessageTemplate();
            item.MessageTemplateId = dbItem.MessageTemplateId;
            item.Name = dbItem.Name;

            return item;
        }

        private static LocalizedMessageTemplateCollection DBMapping(DBLocalizedMessageTemplateCollection dbCollection)
        {
            if (dbCollection == null)
                return null;

            var collection = new LocalizedMessageTemplateCollection();
            foreach (var dbItem in dbCollection)
            {
                var item = DBMapping(dbItem);
                collection.Add(item);
            }

            return collection;
        }

        private static LocalizedMessageTemplate DBMapping(DBLocalizedMessageTemplate dbItem)
        {
            if (dbItem == null)
                return null;

            var item = new LocalizedMessageTemplate();
            item.MessageTemplateLocalizedId = dbItem.MessageTemplateLocalizedId;
            item.MessageTemplateId = dbItem.MessageTemplateId;
            item.LanguageId = dbItem.LanguageId;
            item.BccEmailAddresses = dbItem.BccEmailAddresses;
            item.Name = dbItem.Name;
            item.Subject = dbItem.Subject;
            item.Body = dbItem.Body;
            item.IsActive = dbItem.IsActive;

            return item;
        }

        private static QueuedEmailCollection DBMapping(DBQueuedEmailCollection dbCollection)
        {
            if (dbCollection == null)
                return null;

            var collection = new QueuedEmailCollection();
            foreach (var dbItem in dbCollection)
            {
                var item = DBMapping(dbItem);
                collection.Add(item);
            }

            return collection;
        }

        private static QueuedEmail DBMapping(DBQueuedEmail dbItem)
        {
            if (dbItem == null)
                return null;

            var item = new QueuedEmail();
            item.QueuedEmailId = dbItem.QueuedEmailId;
            item.Priority = dbItem.Priority;
            item.From = dbItem.From;
            item.FromName = dbItem.FromName;
            item.To = dbItem.To;
            item.ToName = dbItem.ToName;
            item.CC = dbItem.CC;
            item.Bcc = dbItem.Bcc;
            item.Subject = dbItem.Subject;
            item.Body = dbItem.Body;
            item.CreatedOn = dbItem.CreatedOn;
            item.SendTries = dbItem.SendTries;
            item.SentOn = dbItem.SentOn;

            return item;
        }

        private static NewsLetterSubscription DBMapping(DBNewsLetterSubscription dbItem)
        {
            if(dbItem == null)
                return null;

            var item = new NewsLetterSubscription();
            item.NewsLetterSubscriptionId = dbItem.NewsLetterSubscriptionId;
            item.NewsLetterSubscriptionGuid = dbItem.NewsLetterSubscriptionGuid;
            item.Email = dbItem.Email;
            item.IsActive = dbItem.IsActive;
            item.CreatedOn = dbItem.CreatedOn;

            return item;
        }

        private static NewsLetterSubscriptionCollection DBMapping(DBNewsLetterSubscriptionCollection dbCollection)
        {
            var collection = new NewsLetterSubscriptionCollection();
            foreach (var dbItem in dbCollection)
            {
                collection.Add(DBMapping(dbItem));
            }
            return collection;
        }

        /// <summary>
        /// Convert a collection to a HTML table
        /// </summary>
        /// <param name="order">Order</param>
        /// <param name="languageId">Language identifier</param>
        /// <returns>HTML table of products</returns>
        private static string ProductListToHtmlTable(Order order, int languageId)
        {
            string result = string.Empty;
            
            var language = LanguageManager.GetLanguageById(languageId);
            if (language == null)
            {
                language = NopContext.Current.WorkingLanguage;
                languageId = language.LanguageId;
            }

            StringBuilder sb = new StringBuilder();
            sb.AppendLine("<table border=\"0\" style=\"width:100%;\">");
            string color1 = SettingManager.GetSettingValue("MessageTemplate.Color1", "#b9babe");
            string color2 = SettingManager.GetSettingValue("MessageTemplate.Color2", "#ebecee");
            string color3 = SettingManager.GetSettingValue("MessageTemplate.Color3", "#dde2e6");
           
            #region Products
            sb.AppendLine(string.Format("<tr style=\"background-color:{0};text-align:center;\">", color1));
            sb.AppendLine(string.Format("<th>{0}</th>", LocalizationManager.GetLocaleResourceString("Order.ProductsGrid.Name", languageId)));
            sb.AppendLine(string.Format("<th>{0}</th>", LocalizationManager.GetLocaleResourceString("Order.ProductsGrid.Price", languageId)));
            sb.AppendLine(string.Format("<th>{0}</th>", LocalizationManager.GetLocaleResourceString("Order.ProductsGrid.Quantity", languageId)));
            sb.AppendLine(string.Format("<th>{0}</th>", LocalizationManager.GetLocaleResourceString("Order.ProductsGrid.Total", languageId)));
            sb.AppendLine("</tr>");

            var table = order.OrderProductVariants;
            for (int i = 0; i <= table.Count - 1; i++)
            {
                var opv = table[i];
                var productVariant = opv.ProductVariant;
                if (productVariant == null)
                    continue;

                sb.AppendLine(string.Format("<tr style=\"background-color: {0};text-align: center;\">", color2));

                sb.AppendLine("<td style=\"padding: 0.6em 0.4em;text-align: left;\">" + HttpUtility.HtmlEncode(productVariant.FullProductName));
                if (OrderManager.IsDownloadAllowed(opv))
                {
                    string downloadUrl = string.Format("<a class=\"link\" href=\"{0}\" >{1}</a>", DownloadManager.GetDownloadUrl(opv), LocalizationManager.GetLocaleResourceString("Order.Download", languageId));
                    sb.AppendLine("&nbsp;&nbsp;(");
                    sb.AppendLine(downloadUrl);
                    sb.AppendLine(")");
                }
                if (!String.IsNullOrEmpty(opv.AttributeDescription))
                {
                    sb.AppendLine("<br />");
                    sb.AppendLine(opv.AttributeDescription);
                }
                sb.AppendLine("</td>");

                string unitPriceStr = string.Empty;
                switch (order.CustomerTaxDisplayType)
                {
                    case TaxDisplayTypeEnum.ExcludingTax:
                        unitPriceStr = PriceHelper.FormatPrice(opv.UnitPriceExclTaxInCustomerCurrency, true, order.CustomerCurrencyCode, language, false);
                        break;
                    case TaxDisplayTypeEnum.IncludingTax:
                        unitPriceStr = PriceHelper.FormatPrice(opv.UnitPriceInclTaxInCustomerCurrency, true, order.CustomerCurrencyCode, language, true);
                        break;
                }
                sb.AppendLine(string.Format("<td style=\"padding: 0.6em 0.4em;text-align: right;\">{0}</td>", unitPriceStr));

                sb.AppendLine(string.Format("<td style=\"padding: 0.6em 0.4em;text-align: center;\">{0}</td>",opv.Quantity));

                string priceStr = string.Empty;
                switch (order.CustomerTaxDisplayType)
                {
                    case TaxDisplayTypeEnum.ExcludingTax:
                        priceStr = PriceHelper.FormatPrice(opv.PriceExclTaxInCustomerCurrency, true, order.CustomerCurrencyCode, language, false);
                        break;
                    case TaxDisplayTypeEnum.IncludingTax:
                        priceStr = PriceHelper.FormatPrice(opv.PriceInclTaxInCustomerCurrency, true, order.CustomerCurrencyCode, language, true);
                        break;
                }
                sb.AppendLine(string.Format("<td style=\"padding: 0.6em 0.4em;text-align: right;\">{0}</td>", priceStr));

                sb.AppendLine("</tr>");
            }
            #endregion

            #region Checkout Attributes

            if (!String.IsNullOrEmpty(order.CheckoutAttributeDescription))
            {
                sb.AppendLine("<tr><td style=\"text-align:right;\" colspan=\"1\">&nbsp;</td><td colspan=\"3\" style=\"text-align:right\">");
                sb.AppendLine(order.CheckoutAttributeDescription);
                sb.AppendLine("</td></tr>");
            }

            #endregion

            #region Totals

            string CusSubTotal = string.Empty;
            string CusShipTotal = string.Empty;
            string CusDiscount = string.Empty;
            string CusPaymentMethodAdditionalFee = string.Empty;
            string CusTaxTotal = string.Empty;
            string CusTotal = string.Empty;
            //subtotal, shipping, payment method fee
            switch (order.CustomerTaxDisplayType)
            {
                case TaxDisplayTypeEnum.ExcludingTax:
                    {
                        CusSubTotal = PriceHelper.FormatPrice(order.OrderSubtotalExclTaxInCustomerCurrency, true, order.CustomerCurrencyCode, language, false);
                        CusShipTotal = PriceHelper.FormatShippingPrice(order.OrderShippingExclTaxInCustomerCurrency, true, order.CustomerCurrencyCode, language, false);
                        CusPaymentMethodAdditionalFee = PriceHelper.FormatPaymentMethodAdditionalFee(order.PaymentMethodAdditionalFeeExclTaxInCustomerCurrency, true, order.CustomerCurrencyCode, language, false);
                    }
                    break;
                case TaxDisplayTypeEnum.IncludingTax:
                    {
                        CusSubTotal = PriceHelper.FormatPrice(order.OrderSubtotalInclTaxInCustomerCurrency, true, order.CustomerCurrencyCode, language, true);
                        CusShipTotal = PriceHelper.FormatShippingPrice(order.OrderShippingInclTaxInCustomerCurrency, true, order.CustomerCurrencyCode, language, true);
                        CusPaymentMethodAdditionalFee = PriceHelper.FormatPaymentMethodAdditionalFee(order.PaymentMethodAdditionalFeeInclTaxInCustomerCurrency, true, order.CustomerCurrencyCode, language, true);
                    }
                    break;
            }

            //discount
            bool dislayDiscount = false;
            if (order.OrderDiscountInCustomerCurrency > decimal.Zero)
            {
                CusDiscount = PriceHelper.FormatPrice(-order.OrderDiscountInCustomerCurrency, true, order.CustomerCurrencyCode, false);
                dislayDiscount = true;
            }

            //shipping
            bool dislayShipping = order.ShippingStatus != ShippingStatusEnum.ShippingNotRequired;

            //payment method fee
            bool displayPaymentMethodFee = true;
            if (order.PaymentMethodAdditionalFeeExclTaxInCustomerCurrency == decimal.Zero)
            {
                displayPaymentMethodFee = false;
            }

            //tax
            bool displayTax = true;
            if (TaxManager.HideTaxInOrderSummary && order.CustomerTaxDisplayType == TaxDisplayTypeEnum.IncludingTax)
            {
                displayTax = false;
            }
            else
            {
                if (order.OrderTax == 0 && TaxManager.HideZeroTax)
                {
                    displayTax = false;
                }
                else
                {
                    string taxStr = PriceHelper.FormatPrice(order.OrderTaxInCustomerCurrency, true, order.CustomerCurrencyCode, false);
                    CusTaxTotal = taxStr;
                }
            }
            
            //total
            CusTotal = PriceHelper.FormatPrice(order.OrderTotalInCustomerCurrency, true, order.CustomerCurrencyCode, false);




            //subtotal
            sb.AppendLine(string.Format("<tr style=\"text-align:right;\"><td>&nbsp;</td><td colspan=\"2\" style=\"background-color: {0};padding:0.6em 0.4 em;\"><strong>{1}</strong></td> <td style=\"background-color: {0};padding:0.6em 0.4 em;\"><strong>{2}</strong></td></tr>", color3, LocalizationManager.GetLocaleResourceString("Order.Sub-Total", languageId), CusSubTotal));
            //discount
            if (dislayDiscount)
            {
                sb.AppendLine(string.Format("<tr style=\"text-align:right;\"><td>&nbsp;</td><td colspan=\"2\" style=\"background-color: {0};padding:0.6em 0.4 em;\"><strong>{1}</strong></td> <td style=\"background-color: {0};padding:0.6em 0.4 em;\"><strong>{2}</strong></td></tr>", color3, LocalizationManager.GetLocaleResourceString("Order.Discount", languageId), CusDiscount));
            }

            //shipping
            if (dislayShipping)
            {
                sb.AppendLine(string.Format("<tr style=\"text-align:right;\"><td>&nbsp;</td><td colspan=\"2\" style=\"background-color: {0};padding:0.6em 0.4 em;\"><strong>{1}</strong></td> <td style=\"background-color: {0};padding:0.6em 0.4 em;\"><strong>{2}</strong></td></tr>", color3, LocalizationManager.GetLocaleResourceString("Order.Shipping", languageId), CusShipTotal));
            }

            //payment method fee
            if (displayPaymentMethodFee)
            {
                string paymentMethodFeeTitle = LocalizationManager.GetLocaleResourceString("Order.PaymentMethodAdditionalFee", languageId);
                sb.AppendLine(string.Format("<tr style=\"text-align:right;\"><td>&nbsp;</td><td colspan=\"2\" style=\"background-color: {0};padding:0.6em 0.4 em;\"><strong>{1}</strong></td> <td style=\"background-color: {0};padding:0.6em 0.4 em;\"><strong>{2}</strong></td></tr>", color3, paymentMethodFeeTitle, CusPaymentMethodAdditionalFee));
            }

            //tax
            if (displayTax)
            {
                sb.AppendLine(string.Format("<tr style=\"text-align:right;\"><td>&nbsp;</td><td colspan=\"2\" style=\"background-color: {0};padding:0.6em 0.4 em;\"><strong>{1}</strong></td> <td style=\"background-color: {0};padding:0.6em 0.4 em;\"><strong>{2}</strong></td></tr>", color3, LocalizationManager.GetLocaleResourceString("Order.Tax", languageId), CusTaxTotal));
            }

            //gift cards
            var gcuhC = OrderManager.GetAllGiftCardUsageHistoryEntries(null, null, order.OrderId);
            foreach (var giftCardUsageHistory in gcuhC)
            {
                string giftCardText = String.Format(LocalizationManager.GetLocaleResourceString("Order.GiftCardInfo", languageId), HttpUtility.HtmlEncode(giftCardUsageHistory.GiftCard.GiftCardCouponCode));
                string giftCardAmount = PriceHelper.FormatPrice(-giftCardUsageHistory.UsedValueInCustomerCurrency, true, order.CustomerCurrencyCode, false);
                sb.AppendLine(string.Format("<tr style=\"text-align:right;\"><td>&nbsp;</td><td colspan=\"2\" style=\"background-color: {0};padding:0.6em 0.4 em;\"><strong>{1}</strong></td> <td style=\"background-color: {0};padding:0.6em 0.4 em;\"><strong>{2}</strong></td></tr>", color3, giftCardText, giftCardAmount));
            }

            //reward points
            if (order.RedeemedRewardPoints != null)
            {
                string rpTitle = string.Format(LocalizationManager.GetLocaleResourceString("Order.Totals.RewardPoints", languageId), -order.RedeemedRewardPoints.Points);
                string rpAmount = PriceHelper.FormatPrice(-order.RedeemedRewardPoints.UsedAmountInCustomerCurrency, true, order.CustomerCurrencyCode, false);
                sb.AppendLine(string.Format("<tr style=\"text-align:right;\"><td>&nbsp;</td><td colspan=\"2\" style=\"background-color: {0};padding:0.6em 0.4 em;\"><strong>{1}</strong></td> <td style=\"background-color: {0};padding:0.6em 0.4 em;\"><strong>{2}</strong></td></tr>", color3, rpTitle, rpAmount));
            }

            //total
            sb.AppendLine(string.Format("<tr style=\"text-align:right;\"><td>&nbsp;</td><td colspan=\"2\" style=\"background-color: {0};padding:0.6em 0.4 em;\"><strong>{1}</strong></td> <td style=\"background-color: {0};padding:0.6em 0.4 em;\"><strong>{2}</strong></td></tr>", color3, LocalizationManager.GetLocaleResourceString("Order.OrderTotal", languageId), CusTotal));
            #endregion
            
            sb.AppendLine("</table>");
            result = sb.ToString();
            return result;
        }

        #endregion

        #region Methods

        #region Repository methods
        /// <summary>
        /// Gets a message template by template identifier
        /// </summary>
        /// <param name="messageTemplateId">Message template identifier</param>
        /// <returns>Message template</returns>
        public static MessageTemplate GetMessageTemplateById(int messageTemplateId)
        {
            if (messageTemplateId == 0)
                return null;

            var dbItem = DBProviderManager<DBMessageProvider>.Provider.GetMessageTemplateById(messageTemplateId);
            var messageTemplate = DBMapping(dbItem);
            return messageTemplate;
        }

        /// <summary>
        /// Gets all message templates
        /// </summary>
        /// <returns>Message template collection</returns>
        public static MessageTemplateCollection GetAllMessageTemplates()
        {
            var dbCollection = DBProviderManager<DBMessageProvider>.Provider.GetAllMessageTemplates();
            var collection = DBMapping(dbCollection);
            return collection;
        }

        /// <summary>
        /// Gets a localized message template by identifier
        /// </summary>
        /// <param name="localizedMessageTemplateId">Localized message template identifier</param>
        /// <returns>Localized message template</returns>
        public static LocalizedMessageTemplate GetLocalizedMessageTemplateById(int localizedMessageTemplateId)
        {
            if (localizedMessageTemplateId == 0)
                return null;

            var dbItem = DBProviderManager<DBMessageProvider>.Provider.GetLocalizedMessageTemplateById(localizedMessageTemplateId);
            var localizedMessageTemplate = DBMapping(dbItem);
            return localizedMessageTemplate;
        }

        /// <summary>
        /// Gets a localized message template by name and language identifier
        /// </summary>
        /// <param name="name">Message template name</param>
        /// <param name="languageId">Language identifier</param>
        /// <returns>Localized message template</returns>
        public static LocalizedMessageTemplate GetLocalizedMessageTemplate(string name, int languageId)
        {
            var dbItem = DBProviderManager<DBMessageProvider>.Provider.GetLocalizedMessageTemplate(name, languageId);
            var localizedMessageTemplate = DBMapping(dbItem);
            return localizedMessageTemplate;
        }

        /// <summary>
        /// Deletes a localized message template
        /// </summary>
        /// <param name="localizedMessageTemplateId">Message template identifier</param>
        public static void DeleteLocalizedMessageTemplate(int localizedMessageTemplateId)
        {
            DBProviderManager<DBMessageProvider>.Provider.DeleteLocalizedMessageTemplate(localizedMessageTemplateId);
        }

        /// <summary>
        /// Gets all localized message templates
        /// </summary>
        /// <param name="messageTemplatesName">Message template name</param>
        /// <returns>Localized message template collection</returns>
        public static LocalizedMessageTemplateCollection GetAllLocalizedMessageTemplates(string messageTemplatesName)
        {
            var dbCollection = DBProviderManager<DBMessageProvider>.Provider.GetAllLocalizedMessageTemplates(messageTemplatesName);
            var localizedMessageTemplates = DBMapping(dbCollection);
            return localizedMessageTemplates;
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
        public static LocalizedMessageTemplate InsertLocalizedMessageTemplate(int messageTemplateId,
            int languageId, string bccEmailAddresses, string subject, string body, bool isActive)
        {
            var dbItem = DBProviderManager<DBMessageProvider>.Provider.InsertLocalizedMessageTemplate(messageTemplateId,
                languageId, bccEmailAddresses, subject, body, isActive);
            var localizedMessageTemplate = DBMapping(dbItem);
            return localizedMessageTemplate;
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
        public static LocalizedMessageTemplate UpdateLocalizedMessageTemplate(int messageTemplateLocalizedId,
            int messageTemplateId, int languageId, string bccEmailAddresses,
            string subject, string body, bool isActive)
        {
            var dbItem = DBProviderManager<DBMessageProvider>.Provider.UpdateLocalizedMessageTemplate(messageTemplateLocalizedId,
                messageTemplateId, languageId, bccEmailAddresses, subject, body, isActive);
            var localizedMessageTemplate = DBMapping(dbItem);
            return localizedMessageTemplate;
        }

        /// <summary>
        /// Gets a queued email by identifier
        /// </summary>
        /// <param name="queuedEmailId">Email item identifier</param>
        /// <returns>Email item</returns>
        public static QueuedEmail GetQueuedEmailById(int queuedEmailId)
        {
            if (queuedEmailId == 0)
                return null;

            var dbItem = DBProviderManager<DBMessageProvider>.Provider.GetQueuedEmailById(queuedEmailId);
            var queuedEmail = DBMapping(dbItem);
            return queuedEmail;
        }

        /// <summary>
        /// Deletes a queued email
        /// </summary>
        /// <param name="queuedEmailId">Email item identifier</param>
        public static void DeleteQueuedEmail(int queuedEmailId)
        {
            DBProviderManager<DBMessageProvider>.Provider.DeleteQueuedEmail(queuedEmailId);
        }

        /// <summary>
        /// Gets all queued emails
        /// </summary>
        /// <param name="queuedEmailCount">Email item count. 0 if you want to get all items</param>
        /// <param name="loadNotSentItemsOnly">A value indicating whether to load only not sent emails</param>
        /// <param name="maxSendTries">Maximum send tries</param>
        /// <returns>Email item collection</returns>
        public static QueuedEmailCollection GetAllQueuedEmails(int queuedEmailCount, 
            bool loadNotSentItemsOnly, int maxSendTries)
        {
            return GetAllQueuedEmails(string.Empty, string.Empty, null, null, 
                queuedEmailCount, loadNotSentItemsOnly, maxSendTries);
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
        public static QueuedEmailCollection GetAllQueuedEmails(string fromEmail,
            string toEmail, DateTime? startTime, DateTime? endTime,
            int queuedEmailCount, bool loadNotSentItemsOnly, int maxSendTries)
        {
            if (fromEmail == null)
                fromEmail = string.Empty;
            fromEmail = fromEmail.Trim();

            if (toEmail == null)
                toEmail = string.Empty;
            toEmail = toEmail.Trim();

            var dbCollection = DBProviderManager<DBMessageProvider>.Provider.GetAllQueuedEmails(fromEmail,
                toEmail, startTime, endTime, queuedEmailCount, 
                loadNotSentItemsOnly, maxSendTries);
            var queuedEmails = DBMapping(dbCollection);
            return queuedEmails;
        }

        /// <summary>
        /// Inserts a queued email
        /// </summary>
        /// <param name="priority">The priority</param>
        /// <param name="from">From</param>
        /// <param name="to">To</param>
        /// <param name="cc">CC</param>
        /// <param name="bcc">BCC</param>
        /// <param name="subject">Subject</param>
        /// <param name="body">Body</param>
        /// <param name="createdOn">The date and time of item creation</param>
        /// <param name="sendTries">The send tries</param>
        /// <param name="sentOn">The sent date and time. Null if email is not sent yet</param>
        /// <returns>Queued email</returns>
        public static QueuedEmail InsertQueuedEmail(int priority, MailAddress from,
             MailAddress to, string cc, string bcc,
            string subject, string body, DateTime createdOn, int sendTries, DateTime? sentOn)
        {
            createdOn = DateTimeHelper.ConvertToUtcTime(createdOn);
            if (sentOn.HasValue)
                sentOn = DateTimeHelper.ConvertToUtcTime(sentOn.Value);

            return InsertQueuedEmail(priority, from.Address, from.DisplayName,
              to.Address, to.DisplayName, cc, bcc, subject, body, createdOn, sendTries, sentOn);
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
        public static QueuedEmail InsertQueuedEmail(int priority, string from,
            string fromName, string to, string toName, string cc, string bcc,
            string subject, string body, DateTime createdOn, int sendTries, DateTime? sentOn)
        {
            createdOn = DateTimeHelper.ConvertToUtcTime(createdOn);
            if (sentOn.HasValue)
                sentOn = DateTimeHelper.ConvertToUtcTime(sentOn.Value);

            var dbItem = DBProviderManager<DBMessageProvider>.Provider.InsertQueuedEmail(priority, from,
                fromName, to, toName, cc, bcc, subject, body, createdOn, sendTries, sentOn);
            var queuedEmail = DBMapping(dbItem);
            return queuedEmail;
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
        public static QueuedEmail UpdateQueuedEmail(int queuedEmailId,
            int priority, string from,
            string fromName, string to, string toName, string cc, string bcc,
            string subject, string body, DateTime createdOn, int sendTries, DateTime? sentOn)
        {
            createdOn = DateTimeHelper.ConvertToUtcTime(createdOn);
            if (sentOn.HasValue)
                sentOn = DateTimeHelper.ConvertToUtcTime(sentOn.Value);

            var dbItem = DBProviderManager<DBMessageProvider>.Provider.UpdateQueuedEmail(queuedEmailId, 
                priority, from, fromName, to, toName, cc, bcc, subject, body, 
                createdOn, sendTries, sentOn);
            var queuedEmail = DBMapping(dbItem);
            return queuedEmail;
        }

        /// <summary>
        /// Inserts the new newsletter subscription
        /// </summary>
        /// <param name="email">The subscriber email</param>
        /// <param name="isActive">A value indicating whether subscription is active</param>
        /// <returns>NewsLetterSubscription entity</returns>
        public static NewsLetterSubscription InsertNewsLetterSubscription(string email,
            bool isActive)
        {
            if(!CommonHelper.IsValidEmail(email))
            {
                throw new NopException("Email is not valid.");
            }

            email = email.Trim();

            return DBMapping(DBProviderManager<DBMessageProvider>.Provider.InsertNewsLetterSubscription(Guid.NewGuid(), email, isActive, DateTime.UtcNow));
        }

        /// <summary>
        /// Gets the newsletter subscription by newsletter subscription identifier
        /// </summary>
        /// <param name="newsLetterSubscriptionId">The newsletter subscription identifier</param>
        /// <returns>NewsLetterSubscription entity</returns>
        public static NewsLetterSubscription GetNewsLetterSubscriptionById(int newsLetterSubscriptionId)
        {
            return DBMapping(DBProviderManager<DBMessageProvider>.Provider.GetNewsLetterSubscriptionById(newsLetterSubscriptionId));
        }

        /// <summary>
        /// Gets the newsletter subscription by newsletter subscription GUID
        /// </summary>
        /// <param name="newsLetterSubscriptionGuid">The newsletter subscription GUID</param>
        /// <returns>NewsLetterSubscription entity</returns>
        public static NewsLetterSubscription GetNewsLetterSubscriptionByGuid(Guid newsLetterSubscriptionGuid)
        {
            if(newsLetterSubscriptionGuid == null)
            {
                return null;
            }
            return DBMapping(DBProviderManager<DBMessageProvider>.Provider.GetNewsLetterSubscriptionByGuid(newsLetterSubscriptionGuid));
        }

        /// <summary>
        /// Gets the newsletter subscription by email
        /// </summary>
        /// <param name="email">The Email</param>
        /// <returns>NewsLetterSubscription entity</returns>
        public static NewsLetterSubscription GetNewsLetterSubscriptionByEmail(string email)
        {
            if(!CommonHelper.IsValidEmail(email))
            {
                return null;
            }

            email = email.Trim();

            return DBMapping(DBProviderManager<DBMessageProvider>.Provider.GetNewsLetterSubscriptionByEmail(email));
        }

        /// <summary>
        /// Gets the newsletter subscription collection
        /// </summary>
        /// <param name="showHidden">A value indicating whether the not active subscriptions should be loaded</param>
        /// <returns>NewsLetterSubscription entity collection</returns>
        public static NewsLetterSubscriptionCollection GetAllNewsLetterSubscriptions(bool showHidden)
        {
            return DBMapping(DBProviderManager<DBMessageProvider>.Provider.GetAllNewsLetterSubscriptions(showHidden));
        }

        /// <summary>
        /// Updates the newsletter subscription
        /// </summary>
        /// <param name="newsLetterSubscriptionId">The newsletter subscription identifier</param>
        /// <param name="email">Email</param>
        /// <param name="isActive">The value indicating whether subscription is active</param>
        /// <returns>NewsLetterSubscription entity</returns>
        public static NewsLetterSubscription UpdateNewsLetterSubscription(int newsLetterSubscriptionId, string email, bool isActive)
        {
            var subscription = GetNewsLetterSubscriptionById(newsLetterSubscriptionId);
            if(subscription == null)
            {
                throw new NopException("Subscription does not exist.");
            }
            if(!CommonHelper.IsValidEmail(email))
            {
                throw new NopException("Email is not valid.");
            }
            return DBMapping(DBProviderManager<DBMessageProvider>.Provider.UpdateNewsLetterSubscription(newsLetterSubscriptionId, 
                subscription.NewsLetterSubscriptionGuid, email, isActive, subscription.CreatedOn));
        }

        /// <summary>
        /// Deletes the newsletter subscription
        /// </summary>
        /// <param name="newsLetterSubscriptionId">The newsletter subscription identifier</param>
        public static void DeleteNewsLetterSubscription(int newsLetterSubscriptionId)
        {
            DBProviderManager<DBMessageProvider>.Provider.DeleteNewsLetterSubscription(newsLetterSubscriptionId);
        }
        
        #endregion

        #region Workflow methods

        /// <summary>
        /// Sends an order completed notification to a customer
        /// </summary>
        /// <param name="order">Order instance</param>
        /// <param name="languageId">Message language identifier</param>
        /// <returns>Queued email identifier</returns>
        public static int SendOrderCompletedCustomerNotification(Order order, int languageId)
        {
            if (order == null)
                throw new ArgumentNullException("order");

            string templateName = "OrderCompleted.CustomerNotification";
            LocalizedMessageTemplate localizedMessageTemplate = MessageManager.GetLocalizedMessageTemplate(templateName, languageId);
            if(localizedMessageTemplate == null || !localizedMessageTemplate.IsActive)
                return 0;
                //throw new NopException(string.Format("Message template ({0}-{1}) couldn't be loaded", TemplateName, LanguageId));

            string subject = ReplaceMessageTemplateTokens(order, localizedMessageTemplate.Subject, languageId);
            string body = ReplaceMessageTemplateTokens(order, localizedMessageTemplate.Body, languageId);
            string bcc = localizedMessageTemplate.BccEmailAddresses;
            var from = new MailAddress(AdminEmailAddress, AdminEmailDisplayName);
            var to = new MailAddress(order.BillingEmail, order.BillingFullName);
            var queuedEmail = InsertQueuedEmail(5, from, to, string.Empty, bcc, subject, body, DateTime.Now, 0, null);
            return queuedEmail.QueuedEmailId;
        }

        /// <summary>
        /// Sends an order placed notification to a store owner
        /// </summary>
        /// <param name="order">Order instance</param>
        /// <param name="languageId">Message language identifier</param>
        /// <returns>Queued email identifier</returns>
        public static int SendOrderPlacedStoreOwnerNotification(Order order, int languageId)
        {
            if (order == null)
                throw new ArgumentNullException("order");


            string templateName = "OrderPlaced.StoreOwnerNotification";
            LocalizedMessageTemplate localizedMessageTemplate = MessageManager.GetLocalizedMessageTemplate(templateName, languageId);
            if(localizedMessageTemplate == null || !localizedMessageTemplate.IsActive)
                return 0;
                //throw new NopException(string.Format("Message template ({0}-{1}) couldn't be loaded", TemplateName, LanguageId));

            string subject = ReplaceMessageTemplateTokens(order, localizedMessageTemplate.Subject, languageId);
            string body = ReplaceMessageTemplateTokens(order, localizedMessageTemplate.Body, languageId);
            string bcc = localizedMessageTemplate.BccEmailAddresses;
            var from = new MailAddress(AdminEmailAddress, AdminEmailDisplayName);
            var to = new MailAddress(AdminEmailAddress, AdminEmailDisplayName);
            var queuedEmail = InsertQueuedEmail(5, from, to, string.Empty, bcc, subject, body, DateTime.Now, 0, null);
            return queuedEmail.QueuedEmailId;
        }

        /// <summary>
        /// Sends a "quantity below" notification to a store owner
        /// </summary>
        /// <param name="productVariant">Product variant</param>
        /// <param name="languageId">Message language identifier</param>
        /// <returns>Queued email identifier</returns>
        public static int SendQuantityBelowStoreOwnerNotification(ProductVariant productVariant, int languageId)
        {
            if (productVariant == null)
                throw new ArgumentNullException("productVariant");

            string templateName = "QuantityBelow.StoreOwnerNotification";
            LocalizedMessageTemplate localizedMessageTemplate = MessageManager.GetLocalizedMessageTemplate(templateName, languageId);
            if(localizedMessageTemplate == null || !localizedMessageTemplate.IsActive)
                return 0;
                //throw new NopException(string.Format("Message template ({0}-{1}) couldn't be loaded", TemplateName, LanguageId));

            string subject = ReplaceMessageTemplateTokens(productVariant, localizedMessageTemplate.Subject, languageId);
            string body = ReplaceMessageTemplateTokens(productVariant, localizedMessageTemplate.Body, languageId);
            string bcc = localizedMessageTemplate.BccEmailAddresses;
            var from = new MailAddress(AdminEmailAddress, AdminEmailDisplayName);
            var to = new MailAddress(AdminEmailAddress, AdminEmailDisplayName);
            var queuedEmail = InsertQueuedEmail(5, from, to, string.Empty, bcc, subject, body, DateTime.Now, 0, null);
            return queuedEmail.QueuedEmailId;
        }

        /// <summary>
        /// Sends an order placed notification to a customer
        /// </summary>
        /// <param name="order">Order instance</param>
        /// <param name="languageId">Message language identifier</param>
        /// <returns>Queued email identifier</returns>
        public static int SendOrderPlacedCustomerNotification(Order order, int languageId)
        {
            if (order == null)
                throw new ArgumentNullException("order");

            string templateName = "OrderPlaced.CustomerNotification";
            LocalizedMessageTemplate localizedMessageTemplate = MessageManager.GetLocalizedMessageTemplate(templateName, languageId);
            if(localizedMessageTemplate == null || !localizedMessageTemplate.IsActive)
                return 0;
                //throw new NopException(string.Format("Message template ({0}-{1}) couldn't be loaded", TemplateName, LanguageId));

            string subject = ReplaceMessageTemplateTokens(order, localizedMessageTemplate.Subject, languageId);
            string body = ReplaceMessageTemplateTokens(order, localizedMessageTemplate.Body, languageId);
            string bcc = localizedMessageTemplate.BccEmailAddresses;
            var from = new MailAddress(AdminEmailAddress, AdminEmailDisplayName);
            var to = new MailAddress(order.BillingEmail, order.BillingFullName);
            var queuedEmail = InsertQueuedEmail(5, from, to, string.Empty, bcc, subject, body, DateTime.Now, 0, null);
            return queuedEmail.QueuedEmailId;
        }

        /// <summary>
        /// Sends an order shipped notification to a customer
        /// </summary>
        /// <param name="order">Order instance</param>
        /// <param name="languageId">Message language identifier</param>
        /// <returns>Queued email identifier</returns>
        public static int SendOrderShippedCustomerNotification(Order order, int languageId)
        {
            if (order == null)
                throw new ArgumentNullException("order");

            string templateName = "OrderShipped.CustomerNotification";
            LocalizedMessageTemplate localizedMessageTemplate = MessageManager.GetLocalizedMessageTemplate(templateName, languageId);
            if(localizedMessageTemplate == null || !localizedMessageTemplate.IsActive)
                return 0;
                //throw new NopException(string.Format("Message template ({0}-{1}) couldn't be loaded", TemplateName, LanguageId));

            string subject = ReplaceMessageTemplateTokens(order, localizedMessageTemplate.Subject, languageId);
            string body = ReplaceMessageTemplateTokens(order, localizedMessageTemplate.Body, languageId);
            string bcc = localizedMessageTemplate.BccEmailAddresses;
            var from = new MailAddress(AdminEmailAddress, AdminEmailDisplayName);
            var to = new MailAddress(order.BillingEmail, order.BillingFullName);
            var queuedEmail = InsertQueuedEmail(5, from, to, string.Empty, bcc, subject, body, DateTime.Now, 0, null);
            return queuedEmail.QueuedEmailId;
        }

        /// <summary>
        /// Sends an order delivered notification to a customer
        /// </summary>
        /// <param name="order">Order instance</param>
        /// <param name="languageId">Message language identifier</param>
        /// <returns>Queued email identifier</returns>
        public static int SendOrderDeliveredCustomerNotification(Order order, int languageId)
        {
            if (order == null)
                throw new ArgumentNullException("order");

            string templateName = "OrderDelivered.CustomerNotification";
            LocalizedMessageTemplate localizedMessageTemplate = MessageManager.GetLocalizedMessageTemplate(templateName, languageId);
            if (localizedMessageTemplate == null || !localizedMessageTemplate.IsActive)
                return 0;
            //throw new NopException(string.Format("Message template ({0}-{1}) couldn't be loaded", TemplateName, LanguageId));

            string subject = ReplaceMessageTemplateTokens(order, localizedMessageTemplate.Subject, languageId);
            string body = ReplaceMessageTemplateTokens(order, localizedMessageTemplate.Body, languageId);
            string bcc = localizedMessageTemplate.BccEmailAddresses;
            var from = new MailAddress(AdminEmailAddress, AdminEmailDisplayName);
            var to = new MailAddress(order.BillingEmail, order.BillingFullName);
            var queuedEmail = InsertQueuedEmail(5, from, to, string.Empty, bcc, subject, body, DateTime.Now, 0, null);
            return queuedEmail.QueuedEmailId;
        }

        /// <summary>
        /// Sends an order cancelled notification to a customer
        /// </summary>
        /// <param name="order">Order instance</param>
        /// <param name="languageId">Message language identifier</param>
        /// <returns>Queued email identifier</returns>
        public static int SendOrderCancelledCustomerNotification(Order order, int languageId)
        {
            if (order == null)
                throw new ArgumentNullException("order");

            string templateName = "OrderCancelled.CustomerNotification";
            var localizedMessageTemplate = MessageManager.GetLocalizedMessageTemplate(templateName, languageId);
            if(localizedMessageTemplate == null || !localizedMessageTemplate.IsActive)
                return 0;
                //throw new NopException(string.Format("Message template ({0}-{1}) couldn't be loaded", TemplateName, LanguageId));

            string subject = ReplaceMessageTemplateTokens(order, localizedMessageTemplate.Subject, languageId);
            string body = ReplaceMessageTemplateTokens(order, localizedMessageTemplate.Body, languageId);
            string bcc = localizedMessageTemplate.BccEmailAddresses;
            var from = new MailAddress(AdminEmailAddress, AdminEmailDisplayName);
            var to = new MailAddress(order.BillingEmail, order.BillingFullName);
            var queuedEmail = InsertQueuedEmail(5, from, to, string.Empty, bcc, subject, body, DateTime.Now, 0, null);
            return queuedEmail.QueuedEmailId;
        }

        /// <summary>
        /// Sends a welcome message to a customer
        /// </summary>
        /// <param name="customer">Customer instance</param>
        /// <param name="languageId">Message language identifier</param>
        /// <returns>Queued email identifier</returns>
        public static int SendCustomerWelcomeMessage(Customer customer, int languageId)
        {
            if (customer == null)
                throw new ArgumentNullException("customer");


            string templateName = "Customer.WelcomeMessage";
            var localizedMessageTemplate = MessageManager.GetLocalizedMessageTemplate(templateName, languageId);
            if(localizedMessageTemplate == null || !localizedMessageTemplate.IsActive)
                return 0;
                //throw new NopException(string.Format("Message template ({0}-{1}) couldn't be loaded", TemplateName, LanguageId));

            string subject = ReplaceMessageTemplateTokens(customer, localizedMessageTemplate.Subject);
            string body = ReplaceMessageTemplateTokens(customer, localizedMessageTemplate.Body);
            string bcc = localizedMessageTemplate.BccEmailAddresses;
            var from = new MailAddress(AdminEmailAddress, AdminEmailDisplayName);
            var to = new MailAddress(customer.Email, customer.FullName);
            var queuedEmail = InsertQueuedEmail(5, from, to, string.Empty, bcc, subject, body, DateTime.Now, 0, null);
            return queuedEmail.QueuedEmailId;
        }

        /// <summary>
        /// Sends an email validation message to a customer
        /// </summary>
        /// <param name="customer">Customer instance</param>
        /// <param name="languageId">Message language identifier</param>
        /// <returns>Queued email identifier</returns>
        public static int SendCustomerEmailValidationMessage(Customer customer, int languageId)
        {
            if (customer == null)
                throw new ArgumentNullException("customer");


            string templateName = "Customer.EmailValidationMessage";
            var localizedMessageTemplate = MessageManager.GetLocalizedMessageTemplate(templateName, languageId);
            if(localizedMessageTemplate == null || !localizedMessageTemplate.IsActive)
                return 0;
                //throw new NopException(string.Format("Message template ({0}-{1}) couldn't be loaded", TemplateName, LanguageId));

            string subject = ReplaceMessageTemplateTokens(customer, localizedMessageTemplate.Subject);
            string body = ReplaceMessageTemplateTokens(customer, localizedMessageTemplate.Body);
            string bcc = localizedMessageTemplate.BccEmailAddresses;
            var from = new MailAddress(AdminEmailAddress, AdminEmailDisplayName);
            var to = new MailAddress(customer.Email, customer.FullName);
            var queuedEmail = InsertQueuedEmail(5, from, to, string.Empty, bcc, subject, body, DateTime.Now, 0, null);
            return queuedEmail.QueuedEmailId;
        }

        /// <summary>
        /// Sends password recovery message to a customer
        /// </summary>
        /// <param name="customer">Customer instance</param>
        /// <param name="languageId">Message language identifier</param>
        /// <returns>Queued email identifier</returns>
        public static int SendCustomerPasswordRecoveryMessage(Customer customer, int languageId)
        {
            if (customer == null)
                throw new ArgumentNullException("customer");

            string templateName = "Customer.PasswordRecovery";
            var localizedMessageTemplate = MessageManager.GetLocalizedMessageTemplate(templateName, languageId);
            if(localizedMessageTemplate == null || !localizedMessageTemplate.IsActive)
                return 0;
                //throw new NopException(string.Format("Message template ({0}-{1}) couldn't be loaded", TemplateName, LanguageId));

            string subject = ReplaceMessageTemplateTokens(customer, localizedMessageTemplate.Subject);
            string body = ReplaceMessageTemplateTokens(customer, localizedMessageTemplate.Body);
            string bcc = localizedMessageTemplate.BccEmailAddresses;
            var from = new MailAddress(AdminEmailAddress, AdminEmailDisplayName);
            var to = new MailAddress(customer.Email, customer.FullName);
            var queuedEmail = InsertQueuedEmail(5, from, to, string.Empty, bcc, subject, body, DateTime.Now, 0, null);
            return queuedEmail.QueuedEmailId;
        }

        /// <summary>
        /// Sends "email a friend" message
        /// </summary>
        /// <param name="customer">Customer instance</param>
        /// <param name="languageId">Message language identifier</param>
        /// <param name="product">Product instance</param>
        /// <param name="friendsEmail">Friend's email</param>
        /// <param name="personalMessage">Personal message</param>
        /// <returns>Queued email identifier</returns>
        public static int SendEmailAFriendMessage(Customer customer, int languageId, 
            Product product, string friendsEmail, string personalMessage)
        {
            if (customer == null)
                throw new ArgumentNullException("customer");
            if (product == null)
                throw new ArgumentNullException("product");

            string templateName = "Service.EmailAFriend";
            var localizedMessageTemplate = MessageManager.GetLocalizedMessageTemplate(templateName, languageId);
            if(localizedMessageTemplate == null || !localizedMessageTemplate.IsActive)
                return 0;
                //throw new NopException(string.Format("Message template ({0}-{1}) couldn't be loaded", TemplateName, LanguageId));

            var additinalKeys = new NameValueCollection();
            additinalKeys.Add("EmailAFriend.PersonalMessage", personalMessage);
            string subject = ReplaceMessageTemplateTokens(customer, product, localizedMessageTemplate.Subject, additinalKeys);
            string body = ReplaceMessageTemplateTokens(customer, product, localizedMessageTemplate.Body, additinalKeys);
            string bcc = localizedMessageTemplate.BccEmailAddresses;
            var from = new MailAddress(AdminEmailAddress, AdminEmailDisplayName);
            var to = new MailAddress(friendsEmail);
            var queuedEmail = InsertQueuedEmail(5, from, to, string.Empty, bcc, subject, body, DateTime.Now, 0, null);
            return queuedEmail.QueuedEmailId;
        }

        /// <summary>
        /// Sends a forum subscription message to a customer
        /// </summary>
        /// <param name="customer">Customer instance</param>
        /// <param name="forumTopic">Forum Topic</param>
        /// <param name="forum">Forum</param>
        /// <param name="languageId">Message language identifier</param>
        /// <returns>Queued email identifier</returns>
        public static int SendNewForumTopicMessage(Customer customer, 
            ForumTopic forumTopic, Forum forum, int languageId)
        {
            if (customer == null)
                throw new ArgumentNullException("customer");

            string templateName = "Forums.NewForumTopic";
            var localizedMessageTemplate = MessageManager.GetLocalizedMessageTemplate(templateName, languageId);
            if(localizedMessageTemplate == null || !localizedMessageTemplate.IsActive)
                return 0;
                //throw new NopException(string.Format("Message template ({0}-{1}) couldn't be loaded", TemplateName, LanguageId));

            string subject = ReplaceMessageTemplateTokens(customer, forumTopic, forum, localizedMessageTemplate.Subject);
            string body = ReplaceMessageTemplateTokens(customer, forumTopic, forum, localizedMessageTemplate.Body);
            string bcc = localizedMessageTemplate.BccEmailAddresses;
            var from = new MailAddress(AdminEmailAddress, AdminEmailDisplayName);
            var to = new MailAddress(customer.Email, customer.FullName);
            var queuedEmail = InsertQueuedEmail(5, from, to, string.Empty, bcc, subject, body, DateTime.Now, 0, null);
            return queuedEmail.QueuedEmailId;
        }

        /// <summary>
        /// Sends a forum subscription message to a customer
        /// </summary>
        /// <param name="customer">Customer instance</param>
        /// <param name="forumTopic">Forum Topic</param>
        /// <param name="forum">Forum</param>
        /// <param name="languageId">Message language identifier</param>
        /// <returns>Queued email identifier</returns>
        public static int SendNewForumPostMessage(Customer customer, ForumTopic forumTopic, 
            Forum forum, int languageId)
        {
            if (customer == null)
                throw new ArgumentNullException("customer");

            string templateName = "Forums.NewForumPost";
            LocalizedMessageTemplate localizedMessageTemplate = MessageManager.GetLocalizedMessageTemplate(templateName, languageId);
            if(localizedMessageTemplate == null || !localizedMessageTemplate.IsActive)
                return 0;
                //throw new NopException(string.Format("Message template ({0}-{1}) couldn't be loaded", TemplateName, LanguageId));

            string subject = ReplaceMessageTemplateTokens(customer, forumTopic, forum, localizedMessageTemplate.Subject);
            string body = ReplaceMessageTemplateTokens(customer, forumTopic, forum, localizedMessageTemplate.Body);
            string bcc = localizedMessageTemplate.BccEmailAddresses;
            var from = new MailAddress(AdminEmailAddress, AdminEmailDisplayName);
            var to = new MailAddress(customer.Email, customer.FullName);
            var queuedEmail = InsertQueuedEmail(5, from, to, string.Empty, bcc, subject, body, DateTime.Now, 0, null);
            return queuedEmail.QueuedEmailId;
        }

        /// <summary>
        /// Sends a news comment notification message to a store owner
        /// </summary>
        /// <param name="newsComment">News comment</param>
        /// <param name="languageId">Message language identifier</param>
        /// <returns>Queued email identifier</returns>
        public static int SendNewsCommentNotificationMessage(NewsComment newsComment, int languageId)
        {
            if (newsComment == null)
                throw new ArgumentNullException("newsComment");

            string templateName = "News.NewsComment";
            var localizedMessageTemplate = MessageManager.GetLocalizedMessageTemplate(templateName, languageId);
            if(localizedMessageTemplate == null || !localizedMessageTemplate.IsActive)
                return 0;
                //throw new NopException(string.Format("Message template ({0}-{1}) couldn't be loaded", TemplateName, LanguageId));

            string subject = ReplaceMessageTemplateTokens(newsComment, localizedMessageTemplate.Subject);
            string body = ReplaceMessageTemplateTokens(newsComment, localizedMessageTemplate.Body);
            string bcc = localizedMessageTemplate.BccEmailAddresses;
            var from = new MailAddress(AdminEmailAddress, AdminEmailDisplayName);
            var to = new MailAddress(AdminEmailAddress, AdminEmailDisplayName);
            var queuedEmail = InsertQueuedEmail(5, from, to, string.Empty, bcc, subject, body, DateTime.Now, 0, null);
            return queuedEmail.QueuedEmailId;
        }

        /// <summary>
        /// Sends a blog comment notification message to a store owner
        /// </summary>
        /// <param name="blogComment">Blog comment</param>
        /// <param name="languageId">Message language identifier</param>
        /// <returns>Queued email identifier</returns>
        public static int SendBlogCommentNotificationMessage(BlogComment blogComment, int languageId)
        {
            if (blogComment == null)
                throw new ArgumentNullException("blogComment");

            string templateName = "Blog.BlogComment";
            var localizedMessageTemplate = MessageManager.GetLocalizedMessageTemplate(templateName, languageId);
            if(localizedMessageTemplate == null || !localizedMessageTemplate.IsActive)
                return 0;
                //throw new NopException(string.Format("Message template ({0}-{1}) couldn't be loaded", TemplateName, LanguageId));

            string subject = ReplaceMessageTemplateTokens(blogComment, localizedMessageTemplate.Subject);
            string body = ReplaceMessageTemplateTokens(blogComment, localizedMessageTemplate.Body);
            string bcc = localizedMessageTemplate.BccEmailAddresses;
            var from = new MailAddress(AdminEmailAddress, AdminEmailDisplayName);
            var to = new MailAddress(AdminEmailAddress, AdminEmailDisplayName);
            var queuedEmail = InsertQueuedEmail(5, from, to, string.Empty, bcc, subject, body, DateTime.Now, 0, null);
            return queuedEmail.QueuedEmailId;
        }

        /// <summary>
        /// Sends a product review notification message to a store owner
        /// </summary>
        /// <param name="productReview">Product review</param>
        /// <param name="languageId">Message language identifier</param>
        /// <returns>Queued email identifier</returns>
        public static int SendProductReviewNotificationMessage(ProductReview productReview,
            int languageId)
        {
            if (productReview == null)
                throw new ArgumentNullException("productReview");

            string templateName = "Product.ProductReview";
            var localizedMessageTemplate = MessageManager.GetLocalizedMessageTemplate(templateName, languageId);
            if(localizedMessageTemplate == null || !localizedMessageTemplate.IsActive)
                return 0;
                //throw new NopException(string.Format("Message template ({0}-{1}) couldn't be loaded", TemplateName, LanguageId));

            string subject = ReplaceMessageTemplateTokens(productReview, localizedMessageTemplate.Subject);
            string body = ReplaceMessageTemplateTokens(productReview, localizedMessageTemplate.Body);
            string bcc = localizedMessageTemplate.BccEmailAddresses;
            var from = new MailAddress(AdminEmailAddress, AdminEmailDisplayName);
            var to = new MailAddress(AdminEmailAddress, AdminEmailDisplayName);
            var queuedEmail = InsertQueuedEmail(5, from, to, string.Empty, bcc, subject, body, DateTime.Now, 0, null);
            return queuedEmail.QueuedEmailId;
        }

        /// <summary>
        /// Sends a newsletter subscription activation message
        /// </summary>
        /// <param name="newsLetterSubscriptionId">Newsletter subscription identifier</param>
        /// <param name="languageId">Language identifier</param>
        /// <returns>Queued email identifier</returns>
        public static int SendNewsLetterSubscriptionActivationMessage(int newsLetterSubscriptionId,
            int languageId)
        {
            var localizedMessageTemplate = MessageManager.GetLocalizedMessageTemplate("NewsLetterSubscription.ActivationMessage", languageId);
            var subscription = GetNewsLetterSubscriptionById(newsLetterSubscriptionId);

            if(localizedMessageTemplate == null || !localizedMessageTemplate.IsActive || subscription == null)
            {
                return 0;
            }

            var from = new MailAddress(AdminEmailAddress, AdminEmailDisplayName);
            var to = new MailAddress(subscription.Email);
            string subject = ReplaceMessageTemplateTokens(subscription, localizedMessageTemplate.Subject);
            string body = ReplaceMessageTemplateTokens(subscription, localizedMessageTemplate.Body);

            var queuedEmail = InsertQueuedEmail(5, from, to, String.Empty, localizedMessageTemplate.BccEmailAddresses, subject, body, DateTime.Now, 0, null);

            return queuedEmail.QueuedEmailId;
        }

        /// <summary>
        /// Sends a newsletter subscription deactivation message
        /// </summary>
        /// <param name="newsLetterSubscriptionId">Newsletter subscription identifier</param>
        /// <param name="languageId">Language identifier</param>
        /// <returns>Queued email identifier</returns>
        public static int SendNewsLetterSubscriptionDeactivationMessage(int newsLetterSubscriptionId, 
            int languageId)
        {
            var localizedMessageTemplate = MessageManager.GetLocalizedMessageTemplate("NewsLetterSubscription.DeactivationMessage", languageId);
            var subscription = GetNewsLetterSubscriptionById(newsLetterSubscriptionId);

            if(localizedMessageTemplate == null || !localizedMessageTemplate.IsActive || subscription == null)
            {
                return 0;
            }

            var from = new MailAddress(AdminEmailAddress, AdminEmailDisplayName);
            var to = new MailAddress(subscription.Email);
            string subject = ReplaceMessageTemplateTokens(subscription, localizedMessageTemplate.Subject);
            string body = ReplaceMessageTemplateTokens(subscription, localizedMessageTemplate.Body);

            var queuedEmail = InsertQueuedEmail(5, from, to, String.Empty, localizedMessageTemplate.BccEmailAddresses, subject, body, DateTime.Now, 0, null);

            return queuedEmail.QueuedEmailId;
        }

        /// <summary>
        /// Sends a gift card notification
        /// </summary>
        /// <param name="giftCard">Gift card</param>
        /// <param name="languageId">Message language identifier</param>
        /// <returns>Queued email identifier</returns>
        public static int SendGiftCardNotification(GiftCard giftCard, int languageId)
        {
            if (giftCard == null)
                throw new ArgumentNullException("giftCard");

            string templateName = "GiftCard.Notification";
            var localizedMessageTemplate = MessageManager.GetLocalizedMessageTemplate(templateName, languageId);
            if(localizedMessageTemplate == null || !localizedMessageTemplate.IsActive)
                return 0;
            //throw new NopException(string.Format("Message template ({0}-{1}) couldn't be loaded", TemplateName, LanguageId));

            string subject = ReplaceMessageTemplateTokens(giftCard, localizedMessageTemplate.Subject);
            string body = ReplaceMessageTemplateTokens(giftCard, localizedMessageTemplate.Body);
            string bcc = localizedMessageTemplate.BccEmailAddresses;
            var from = new MailAddress(AdminEmailAddress, AdminEmailDisplayName);
            var to = new MailAddress(giftCard.RecipientEmail, giftCard.RecipientName);
            var queuedEmail = InsertQueuedEmail(5, from, to, string.Empty, bcc, subject, body, DateTime.Now, 0, null);
            return queuedEmail.QueuedEmailId;
        }

        /// <summary>
        /// Sends an email
        /// </summary>
        /// <param name="subject">Subject</param>
        /// <param name="body">Body</param>
        /// <param name="from">From</param>
        /// <param name="to">To</param>
        public static void SendEmail(string subject, string body, string from, string to)
        {
            SendEmail(subject, body, new MailAddress(from), new MailAddress(to), 
                new List<String>(), new List<String>());
        }

        /// <summary>
        /// Sends an email
        /// </summary>
        /// <param name="subject">Subject</param>
        /// <param name="body">Body</param>
        /// <param name="from">From</param>
        /// <param name="to">To</param>
        public static void SendEmail(string subject, string body, MailAddress from, 
            MailAddress to)
        {
            SendEmail(subject, body, from, to, new List<String>(), new List<String>());
        }

        /// <summary>
        /// Sends an email
        /// </summary>
        /// <param name="subject">Subject</param>
        /// <param name="body">Body</param>
        /// <param name="from">From</param>
        /// <param name="to">To</param>
        /// <param name="bcc">BCC</param>
        /// <param name="cc">CC</param>
        public static void SendEmail(string subject, string body, 
            MailAddress from, MailAddress to, List<string> bcc, List<string> cc)
        {
            var message = new MailMessage();
            message.From = from;
            message.To.Add(to);
            if (null != bcc)
                foreach (string address in bcc)
                {
                    if (address != null)
                    {
                        if (!String.IsNullOrEmpty(address.Trim()))
                        {
                            message.Bcc.Add(address.Trim());
                        }
                    }
                }
            if (null != cc)
                foreach (string address in cc)
                {
                    if (address != null)
                    {
                        if (!String.IsNullOrEmpty(address.Trim()))
                        {
                            message.CC.Add(address.Trim());
                        }
                    }
                }
            message.Subject = subject;
            message.Body = body;
            message.IsBodyHtml = true;

            var smtpClient = new SmtpClient();
            smtpClient.UseDefaultCredentials = AdminEmailUseDefaultCredentials;
            smtpClient.Host = AdminEmailHost;
            smtpClient.Port = AdminEmailPort;
            smtpClient.EnableSsl = AdminEmailEnableSsl;
            if (AdminEmailUseDefaultCredentials)
                smtpClient.Credentials = CredentialCache.DefaultNetworkCredentials;
            else
                smtpClient.Credentials = new NetworkCredential(AdminEmailUser, AdminEmailPassword);
            smtpClient.Send(message);
        }

        /// <summary>
        /// Gets list of allowed (supported) message tokens
        /// </summary>
        /// <returns></returns>
        public static string[] GetListOfAllowedTokens()
        {
            var allowedTokens = new List<string>();
            allowedTokens.Add("%Store.Name%");
            allowedTokens.Add("%Store.URL%");
            allowedTokens.Add("%Store.Email%");
            allowedTokens.Add("%Order.OrderNumber%");
            allowedTokens.Add("%Order.CustomerFullName%");
            allowedTokens.Add("%Order.CustomerEmail%");
            allowedTokens.Add("%Order.BillingFirstName%");
            allowedTokens.Add("%Order.BillingLastName%");
            allowedTokens.Add("%Order.BillingPhoneNumber%");
            allowedTokens.Add("%Order.BillingEmail%");
            allowedTokens.Add("%Order.BillingFaxNumber%");
            allowedTokens.Add("%Order.BillingCompany%");
            allowedTokens.Add("%Order.BillingAddress1%");
            allowedTokens.Add("%Order.BillingAddress2%");
            allowedTokens.Add("%Order.BillingCity%");
            allowedTokens.Add("%Order.BillingStateProvince%");
            allowedTokens.Add("%Order.BillingZipPostalCode%");
            allowedTokens.Add("%Order.BillingCountry%");
            allowedTokens.Add("%Order.ShippingMethod%");
            allowedTokens.Add("%Order.ShippingFirstName%");
            allowedTokens.Add("%Order.ShippingLastName%");
            allowedTokens.Add("%Order.ShippingPhoneNumber%");
            allowedTokens.Add("%Order.ShippingEmail%");
            allowedTokens.Add("%Order.ShippingFaxNumber%");
            allowedTokens.Add("%Order.ShippingCompany%");
            allowedTokens.Add("%Order.ShippingAddress1%");
            allowedTokens.Add("%Order.ShippingAddress2%");
            allowedTokens.Add("%Order.ShippingCity%");
            allowedTokens.Add("%Order.ShippingStateProvince%");
            allowedTokens.Add("%Order.ShippingZipPostalCode%");
            allowedTokens.Add("%Order.ShippingCountry%");
            allowedTokens.Add("%Order.TrackingNumber%");
            allowedTokens.Add("%Order.Product(s)%");
            allowedTokens.Add("%Order.CreatedOn%");
            allowedTokens.Add("%Order.OrderURLForCustomer%");
            allowedTokens.Add("%Customer.Email%");
            allowedTokens.Add("%Customer.Username%");
            allowedTokens.Add("%Customer.PasswordRecoveryURL%");
            allowedTokens.Add("%Customer.AccountActivationURL%");
            allowedTokens.Add("%Customer.FullName%");
            allowedTokens.Add("%Product.Name%");
            allowedTokens.Add("%Product.ShortDescription%");
            allowedTokens.Add("%Product.ProductURLForCustomer%");
            allowedTokens.Add("%ProductVariant.FullProductName%");
            allowedTokens.Add("%ProductVariant.StockQuantity%");
            allowedTokens.Add("%NewsComment.NewsTitle%");
            allowedTokens.Add("%BlogComment.BlogPostTitle%");
            allowedTokens.Add("%NewsLetterSubscription.Email%");
            allowedTokens.Add("%NewsLetterSubscription.ActivationUrl%");
            allowedTokens.Add("%NewsLetterSubscription.DeactivationUrl%");
            allowedTokens.Add("%GiftCard.SenderName%");
            allowedTokens.Add("%GiftCard.SenderEmail%");
            allowedTokens.Add("%GiftCard.RecipientName%");
            allowedTokens.Add("%GiftCard.RecipientEmail%");
            allowedTokens.Add("%GiftCard.Amount%");
            allowedTokens.Add("%GiftCard.CouponCode%");
            allowedTokens.Add("%GiftCard.Message%");
            
            return allowedTokens.ToArray();
        }

        /// <summary>
        /// Gets list of allowed (supported) message tokens for campaigns
        /// </summary>
        /// <returns>List of allowed (supported) message tokens for campaigns</returns>
        public static string[] GetListOfCampaignAllowedTokens()
        {
            var allowedTokens = new List<string>();
            allowedTokens.Add("%Store.Name%");
            allowedTokens.Add("%Store.URL%");
            allowedTokens.Add("%Store.Email%");
            allowedTokens.Add("%NewsLetterSubscription.Email%");
            allowedTokens.Add("%NewsLetterSubscription.ActivationUrl%");
            allowedTokens.Add("%NewsLetterSubscription.DeactivationUrl%");
            return allowedTokens.ToArray();
        }

        /// <summary>
        /// Replaces a message template tokens
        /// </summary>
        /// <param name="order">Order instance</param>
        /// <param name="template">Template</param>
        /// <param name="languageId">Language identifier</param>
        /// <returns>New template</returns>
        public static string ReplaceMessageTemplateTokens(Order order, string template, 
            int languageId)
        {
            var tokens = new NameValueCollection();
            tokens.Add("Store.Name", SettingManager.StoreName);
            tokens.Add("Store.URL", SettingManager.StoreUrl);
            tokens.Add("Store.Email", AdminEmailAddress);

            tokens.Add("Order.OrderNumber", order.OrderId.ToString());
            
            tokens.Add("Order.CustomerFullName", HttpUtility.HtmlEncode(order.BillingFullName));
            tokens.Add("Order.CustomerEmail", HttpUtility.HtmlEncode(order.BillingEmail));


            tokens.Add("Order.BillingFirstName", HttpUtility.HtmlEncode(order.BillingFirstName));
            tokens.Add("Order.BillingLastName", HttpUtility.HtmlEncode(order.BillingLastName));
            tokens.Add("Order.BillingPhoneNumber", HttpUtility.HtmlEncode(order.BillingPhoneNumber));
            tokens.Add("Order.BillingEmail", HttpUtility.HtmlEncode(order.BillingEmail.ToString()));
            tokens.Add("Order.BillingFaxNumber", HttpUtility.HtmlEncode(order.BillingFaxNumber));
            tokens.Add("Order.BillingCompany", HttpUtility.HtmlEncode(order.BillingCompany));
            tokens.Add("Order.BillingAddress1", HttpUtility.HtmlEncode(order.BillingAddress1));
            tokens.Add("Order.BillingAddress2", HttpUtility.HtmlEncode(order.BillingAddress2));
            tokens.Add("Order.BillingCity", HttpUtility.HtmlEncode(order.BillingCity));
            tokens.Add("Order.BillingStateProvince", HttpUtility.HtmlEncode(order.BillingStateProvince));
            tokens.Add("Order.BillingZipPostalCode", HttpUtility.HtmlEncode(order.BillingZipPostalCode));
            tokens.Add("Order.BillingCountry", HttpUtility.HtmlEncode(order.BillingCountry));

            tokens.Add("Order.ShippingMethod", HttpUtility.HtmlEncode(order.ShippingMethod));

            tokens.Add("Order.ShippingFirstName", HttpUtility.HtmlEncode(order.ShippingFirstName));
            tokens.Add("Order.ShippingLastName", HttpUtility.HtmlEncode(order.ShippingLastName));
            tokens.Add("Order.ShippingPhoneNumber", HttpUtility.HtmlEncode(order.ShippingPhoneNumber));
            tokens.Add("Order.ShippingEmail", HttpUtility.HtmlEncode(order.ShippingEmail.ToString()));
            tokens.Add("Order.ShippingFaxNumber", HttpUtility.HtmlEncode(order.ShippingFaxNumber));
            tokens.Add("Order.ShippingCompany", HttpUtility.HtmlEncode(order.ShippingCompany));
            tokens.Add("Order.ShippingAddress1", HttpUtility.HtmlEncode(order.ShippingAddress1));
            tokens.Add("Order.ShippingAddress2", HttpUtility.HtmlEncode(order.ShippingAddress2));
            tokens.Add("Order.ShippingCity", HttpUtility.HtmlEncode(order.ShippingCity));
            tokens.Add("Order.ShippingStateProvince", HttpUtility.HtmlEncode(order.ShippingStateProvince));
            tokens.Add("Order.ShippingZipPostalCode", HttpUtility.HtmlEncode(order.ShippingZipPostalCode));
            tokens.Add("Order.ShippingCountry", HttpUtility.HtmlEncode(order.ShippingCountry));

            tokens.Add("Order.TrackingNumber", HttpUtility.HtmlEncode(order.TrackingNumber));

            tokens.Add("Order.Product(s)", ProductListToHtmlTable(order, languageId));

            var language = LanguageManager.GetLanguageById(languageId);
            //UNDONE use time zone
            //1. Add new token for store owner
            //2. Convert the date and time according to time zone
            if (language != null && !String.IsNullOrEmpty(language.LanguageCulture))
            {
                tokens.Add("Order.CreatedOn", order.CreatedOn.ToString("D", new CultureInfo(language.LanguageCulture)));
            }
            else
            {
                tokens.Add("Order.CreatedOn", order.CreatedOn.ToString("D"));
            }
            tokens.Add("Order.OrderURLForCustomer", string.Format("{0}orderdetails.aspx?orderid={1}", SettingManager.StoreUrl, order.OrderId));

            foreach(string token in tokens.Keys)
            {
                template = Regex.Replace(template, String.Format(@"%{0}%", token), tokens[token], RegexOptions.IgnoreCase);
            }

            return template;
        }

        /// <summary>
        /// Replaces a message template tokens
        /// </summary>
        /// <param name="subscription">Subscription</param>
        /// <param name="template">Template</param>
        /// <returns>New template</returns>
        public static string ReplaceMessageTemplateTokens(NewsLetterSubscription subscription, 
            string template)
        {
            var tokens = new NameValueCollection();

            tokens.Add("Store.Name", SettingManager.StoreName);
            tokens.Add("Store.URL", SettingManager.StoreUrl);
            tokens.Add("Store.Email", AdminEmailAddress);
            tokens.Add("NewsLetterSubscription.Email", HttpUtility.HtmlEncode(subscription.Email));
            tokens.Add("NewsLetterSubscription.ActivationUrl", String.Format("{0}newslettersubscriptionactivation.aspx?t={1}&active=1", SettingManager.StoreUrl, subscription.NewsLetterSubscriptionGuid));
            tokens.Add("NewsLetterSubscription.DeactivationUrl", String.Format("{0}newslettersubscriptionactivation.aspx?t={1}&active=0", SettingManager.StoreUrl, subscription.NewsLetterSubscriptionGuid));

            var customer = subscription.Customer;
            if(customer != null)
            {
                template = ReplaceMessageTemplateTokens(customer, template);
            }

            foreach(string token in tokens.Keys)
            {
                template = Regex.Replace(template, String.Format(@"%{0}%", token), tokens[token], RegexOptions.IgnoreCase);
            }

            return template;
        }

        /// <summary>
        /// Replaces a message template tokens
        /// </summary>
        /// <param name="customer">Customer instance</param>
        /// <param name="template">Template</param>
        /// <returns>New template</returns>
        public static string ReplaceMessageTemplateTokens(Customer customer, string template)
        {
            var tokens = new NameValueCollection();
            tokens.Add("Store.Name", SettingManager.StoreName);
            tokens.Add("Store.URL", SettingManager.StoreUrl);
            tokens.Add("Store.Email", AdminEmailAddress);

            tokens.Add("Customer.Email", HttpUtility.HtmlEncode(customer.Email));
            tokens.Add("Customer.Username", HttpUtility.HtmlEncode(customer.Username));
            tokens.Add("Customer.FullName", HttpUtility.HtmlEncode(customer.FullName));

            string passwordRecoveryUrl = string.Empty;
            passwordRecoveryUrl = string.Format("{0}passwordrecovery.aspx?prt={1}&email={2}", SettingManager.StoreUrl, customer.PasswordRecoveryToken, customer.Email);
            tokens.Add("Customer.PasswordRecoveryURL", passwordRecoveryUrl);
            
            string accountActivationUrl = string.Empty;
            accountActivationUrl = string.Format("{0}accountactivation.aspx?act={1}&email={2}", SettingManager.StoreUrl, customer.AccountActivationToken, customer.Email);
            tokens.Add("Customer.AccountActivationURL", accountActivationUrl);

            foreach(string token in tokens.Keys)
            {
                template = Regex.Replace(template, String.Format(@"%{0}%", token), tokens[token], RegexOptions.IgnoreCase);
            }

            return template;
        }

        /// <summary>
        /// Replaces a message template tokens
        /// </summary>
        /// <param name="customer">Customer instance</param>
        /// <param name="product">Product instance</param>
        /// <param name="template">Template</param>
        /// <param name="additinalKeys">Additinal keys</param>
        /// <returns>New template</returns>
        public static string ReplaceMessageTemplateTokens(Customer customer, Product product,
            string template, NameValueCollection additinalKeys)
        {
            var tokens = new NameValueCollection();
            tokens.Add("Store.Name", SettingManager.StoreName);
            tokens.Add("Store.URL", SettingManager.StoreUrl);
            tokens.Add("Store.Email", AdminEmailAddress);

            tokens.Add("Customer.Email", HttpUtility.HtmlEncode(customer.Email));
            tokens.Add("Customer.Username", HttpUtility.HtmlEncode(customer.Username));
            tokens.Add("Customer.FullName", HttpUtility.HtmlEncode(customer.FullName));

            tokens.Add("Product.Name", HttpUtility.HtmlEncode(product.Name));
            tokens.Add("Product.ShortDescription", product.ShortDescription);
            tokens.Add("Product.ProductURLForCustomer", SEOHelper.GetProductUrl(product));

            foreach(string token in tokens.Keys)
            {
                template = Regex.Replace(template, String.Format(@"%{0}%", token), tokens[token], RegexOptions.IgnoreCase);
            }

            foreach(string token in additinalKeys.Keys)
            {
                template = Regex.Replace(template, String.Format(@"%{0}%", token), additinalKeys[token], RegexOptions.IgnoreCase);
            }

            return template;
        }

        /// <summary>
        /// Replaces a message template tokens
        /// </summary>
        /// <param name="customer">Customer instance</param>
        /// <param name="forumTopic">Forum Topic</param>
        /// <param name="forum">Forum</param>
        /// <param name="template">Template</param>
        /// <returns>New template</returns>
        public static string ReplaceMessageTemplateTokens(Customer customer, 
            ForumTopic forumTopic, Forum forum, 
            string template)
        {
            var tokens = new NameValueCollection();
            tokens.Add("Store.Name", SettingManager.StoreName);
            tokens.Add("Store.URL", SettingManager.StoreUrl);
            tokens.Add("Store.Email", AdminEmailAddress);

            tokens.Add("Customer.Email", HttpUtility.HtmlEncode(customer.Email));
            tokens.Add("Customer.Username", HttpUtility.HtmlEncode(customer.Username));
            tokens.Add("Customer.FullName", HttpUtility.HtmlEncode(customer.FullName));

            tokens.Add("Forums.TopicURL", SEOHelper.GetForumTopicUrl(forumTopic.ForumTopicId));
            tokens.Add("Forums.TopicName", HttpUtility.HtmlEncode(forumTopic.Subject));
            tokens.Add("Forums.ForumURL", SEOHelper.GetForumUrl(forum));
            tokens.Add("Forums.ForumName", HttpUtility.HtmlEncode(forum.Name));

            foreach(string token in tokens.Keys)
            {
                template = Regex.Replace(template, String.Format(@"%{0}%", token), tokens[token], RegexOptions.IgnoreCase);
            }

            return template;
        }

        /// <summary>
        /// Replaces a message template tokens
        /// </summary>
        /// <param name="productVariant">Product variant</param>
        /// <param name="template">Template</param>
        /// <param name="languageId">Language identifier</param>
        /// <returns>New template</returns>
        public static string ReplaceMessageTemplateTokens(ProductVariant productVariant, 
            string template, int languageId)
        {
            var tokens = new NameValueCollection();
            tokens.Add("Store.Name", SettingManager.StoreName);
            tokens.Add("Store.URL", SettingManager.StoreUrl);
            tokens.Add("Store.Email", AdminEmailAddress);

            tokens.Add("ProductVariant.ID", productVariant.ProductVariantId.ToString());
            tokens.Add("ProductVariant.FullProductName", HttpUtility.HtmlEncode(productVariant.FullProductName));
            tokens.Add("ProductVariant.StockQuantity", productVariant.StockQuantity.ToString());

            foreach(string token in tokens.Keys)
            {
                template = Regex.Replace(template, String.Format(@"%{0}%", token), tokens[token], RegexOptions.IgnoreCase);
            }

            return template;
        }

        /// <summary>
        /// Replaces a message template tokens
        /// </summary>
        /// <param name="newsComment">News comment</param>
        /// <param name="template">Template</param>
        /// <returns>New template</returns>
        public static string ReplaceMessageTemplateTokens(NewsComment newsComment, 
            string template)
        {
            var tokens = new NameValueCollection();
            tokens.Add("Store.Name", SettingManager.StoreName);
            tokens.Add("Store.URL", SettingManager.StoreUrl);
            tokens.Add("Store.Email", AdminEmailAddress);

            tokens.Add("NewsComment.NewsTitle", HttpUtility.HtmlEncode(newsComment.News.Title));

            foreach(string token in tokens.Keys)
            {
                template = Regex.Replace(template, String.Format(@"%{0}%", token), tokens[token], RegexOptions.IgnoreCase);
            }

            return template;
        }

        /// <summary>
        /// Replaces a message template tokens
        /// </summary>
        /// <param name="blogComment">Blog comment</param>
        /// <param name="template">Template</param>
        /// <returns>New template</returns>
        public static string ReplaceMessageTemplateTokens(BlogComment blogComment,
            string template)
        {
            var tokens = new NameValueCollection();
            tokens.Add("Store.Name", SettingManager.StoreName);
            tokens.Add("Store.URL", SettingManager.StoreUrl);
            tokens.Add("Store.Email", AdminEmailAddress);

            tokens.Add("BlogComment.BlogPostTitle", HttpUtility.HtmlEncode(blogComment.BlogPost.BlogPostTitle));

            foreach(string token in tokens.Keys)
            {
                template = Regex.Replace(template, String.Format(@"%{0}%", token), tokens[token], RegexOptions.IgnoreCase);
            }

            return template;
        }

        /// <summary>
        /// Replaces a message template tokens
        /// </summary>
        /// <param name="productReview">Product review</param>
        /// <param name="template">Template</param>
        /// <returns>New template</returns>
        public static string ReplaceMessageTemplateTokens(ProductReview productReview, 
            string template)
        {
            var tokens = new NameValueCollection();
            tokens.Add("Store.Name", SettingManager.StoreName);
            tokens.Add("Store.URL", SettingManager.StoreUrl);
            tokens.Add("Store.Email", AdminEmailAddress);

            tokens.Add("ProductReview.ProductName", HttpUtility.HtmlEncode(productReview.Product.Name));

            foreach(string token in tokens.Keys)
            {
                template = Regex.Replace(template, String.Format(@"%{0}%", token), tokens[token], RegexOptions.IgnoreCase);
            }

            return template;
        }

        /// <summary>
        /// Replaces a message template tokens
        /// </summary>
        /// <param name="giftCard">Gift card</param>
        /// <param name="template">Template</param>
        /// <returns>New template</returns>
        public static string ReplaceMessageTemplateTokens(GiftCard giftCard, string template)
        {
            var tokens = new NameValueCollection();
            tokens.Add("Store.Name", SettingManager.StoreName);
            tokens.Add("Store.URL", SettingManager.StoreUrl);
            tokens.Add("Store.Email", AdminEmailAddress);

            tokens.Add("GiftCard.SenderName", HttpUtility.HtmlEncode(giftCard.SenderName));
            tokens.Add("GiftCard.SenderEmail", HttpUtility.HtmlEncode(giftCard.SenderEmail));
            tokens.Add("GiftCard.RecipientName", HttpUtility.HtmlEncode(giftCard.RecipientName));
            tokens.Add("GiftCard.RecipientEmail", HttpUtility.HtmlEncode(giftCard.RecipientEmail));
            tokens.Add("GiftCard.Amount", HttpUtility.HtmlEncode(PriceHelper.FormatPrice(giftCard.Amount, true, false)));
            tokens.Add("GiftCard.CouponCode", HttpUtility.HtmlEncode(giftCard.GiftCardCouponCode));
            tokens.Add("GiftCard.Message", MessageManager.FormatContactUsFormText(giftCard.Message));

            foreach(string token in tokens.Keys)
            {
                template = Regex.Replace(template, String.Format(@"%{0}%", token), tokens[token], RegexOptions.IgnoreCase);
            }

            return template;
        }

        /// <summary>
        /// Formats the contact us form text
        /// </summary>
        /// <param name="text">Text</param>
        /// <returns>Formatted text</returns>
        public static string FormatContactUsFormText(string text)
        {
            if (String.IsNullOrEmpty(text))
                return string.Empty;

            text = HtmlHelper.FormatText(text, false, true, false, false, false, false);
            return text;
        }

        #endregion

        #endregion

        #region Properties
        /// <summary>
        /// Gets or sets an admin email address
        /// </summary>
        public static string AdminEmailAddress
        {
            get
            {
                return SettingManager.GetSettingValue("Email.AdminEmailAddress");
            }
            set
            {
                SettingManager.SetParam("Email.AdminEmailAddress", value.Trim());
            }
        }

        /// <summary>
        /// Gets or sets an admin email display name
        /// </summary>
        public static string AdminEmailDisplayName
        {
            get
            {
                return SettingManager.GetSettingValue("Email.AdminEmailDisplayName");
            }
            set
            {
                SettingManager.SetParam("Email.AdminEmailDisplayName", value.Trim());
            }
        }

        /// <summary>
        /// Gets or sets an admin email host
        /// </summary>
        public static string AdminEmailHost
        {
            get
            {
                return SettingManager.GetSettingValue("Email.AdminEmailHost");
            }
            set
            {
                SettingManager.SetParam("Email.AdminEmailHost", value.Trim());
            }
        }

        /// <summary>
        /// Gets or sets an admin email port
        /// </summary>
        public static int AdminEmailPort
        {
            get
            {
                return SettingManager.GetSettingValueInteger("Email.AdminEmailPort");
            }
            set
            {
                SettingManager.SetParam("Email.AdminEmailPort", value.ToString());
            }
        }

        /// <summary>
        /// Gets or sets an admin email user name
        /// </summary>
        public static string AdminEmailUser
        {
            get
            {
                return SettingManager.GetSettingValue("Email.AdminEmailUser");
            }
            set
            {
                SettingManager.SetParam("Email.AdminEmailUser", value.Trim());
            }
        }

        /// <summary>
        /// Gets or sets an admin email password
        /// </summary>
        public static string AdminEmailPassword
        {
            get
            {
                return SettingManager.GetSettingValue("Email.AdminEmailPassword");
            }
            set
            {
                SettingManager.SetParam("Email.AdminEmailPassword", value);
            }
        }

        /// <summary>
        /// Gets or sets a value that controls whether the default system credentials of the application are sent with requests.
        /// </summary>
        public static bool AdminEmailUseDefaultCredentials
        {
            get
            {
                return SettingManager.GetSettingValueBoolean("Email.AdminEmailUseDefaultCredentials");
            }
            set
            {
                SettingManager.SetParam("Email.AdminEmailUseDefaultCredentials", value.ToString());
            }
        }

        /// <summary>
        /// Gets or sets a value that controls whether the SmtpClient uses Secure Sockets Layer (SSL) to encrypt the connection
        /// </summary>
        public static bool AdminEmailEnableSsl
        {
            get
            {
                return SettingManager.GetSettingValueBoolean("Email.AdminEmailEnableSsl");
            }
            set
            {
                SettingManager.SetParam("Email.AdminEmailEnableSsl", value.ToString());
            }
        }

        #endregion
    }
}