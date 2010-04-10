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
using System.Text;
using NopSolutions.NopCommerce.BusinessLogic.Directory;


namespace NopSolutions.NopCommerce.BusinessLogic.Messages
{
    /// <summary>
    /// Represents a localized message template
    /// </summary>
    public partial class LocalizedMessageTemplate : BaseEntity
    {
        #region Ctor
        /// <summary>
        /// Creates a new instance of the LocalizedMessageTemplate class
        /// </summary>
        public LocalizedMessageTemplate()
        {
        }
        #endregion

        #region Properties
        /// <summary>
        /// Gets or sets the localized message template identifier
        /// </summary>
        public int MessageTemplateLocalizedID { get; set; }

        /// <summary>
        /// Gets or sets the message template identifier
        /// </summary>
        public int MessageTemplateID { get; set; }

        /// <summary>
        /// Gets or sets the language identifier
        /// </summary>
        public int LanguageID { get; set; }

        /// <summary>
        /// Gets or sets the name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets the subject
        /// </summary>
        public string Subject { get; set; }

        /// <summary>
        /// Gets or sets the body
        /// </summary>
        public string Body { get; set; }
        #endregion

        #region Custom Properties
        /// <summary>
        /// Gets the language
        /// </summary>
        public Language Language
        {
            get
            {
                return LanguageManager.GetLanguageByID(LanguageID);
            }
        }

        /// <summary>
        /// Gets the message template
        /// </summary>
        public MessageTemplate MessageTemplate
        {
            get
            {
                return MessageManager.GetMessageTemplateByID(MessageTemplateID);
            }
        }
        #endregion
    }

}
