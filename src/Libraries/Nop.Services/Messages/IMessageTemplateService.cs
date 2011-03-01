﻿using System.Collections.Generic;
using Nop.Core.Domain.Messages;

namespace Nop.Services.Messages
{
    /// <summary>
    /// Message template service
    /// </summary>
    public partial interface IMessageTemplateService
    {
        /// <summary>
        /// Inserts a message template
        /// </summary>
        /// <param name="messageTemplate">Message template</param>
        void InsertMessageTemplate(MessageTemplate messageTemplate);

        /// <summary>
        /// Updates a message template
        /// </summary>
        /// <param name="messageTemplate">Message template</param>
        void UpdateMessageTemplate(MessageTemplate messageTemplate);

        /// <summary>
        /// Gets a message template
        /// </summary>
        /// <param name="messageTemplateId">Message template identifier</param>
        /// <returns>Message template</returns>
        MessageTemplate GetMessageTemplateById(int messageTemplateId);

        /// <summary>
        /// Gets all message templates
        /// </summary>
        /// <returns>Message template list</returns>
        IList<MessageTemplate> GetAllMessageTemplates();
    }
}