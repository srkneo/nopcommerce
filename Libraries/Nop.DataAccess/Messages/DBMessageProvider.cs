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
        /// Inserts the new newsletter subscription
        /// </summary>
        /// <param name="newsLetterSubscriptionGuid">The newsletter subscription GUID</param>
        /// <param name="email">The subscriber email</param>
        /// <param name="isActive">A value indicating whether subscription is active</param>
        /// <param name="createdOn">The date and time of instance creation</param>
        /// <returns>NewsLetterSubscription entity</returns>
        public abstract DBNewsLetterSubscription InsertNewsLetterSubscription(Guid newsLetterSubscriptionGuid, 
            string email, bool isActive, DateTime createdOn);

        /// <summary>
        /// Gets the newsletter subscription by newsletter subscription identifier
        /// </summary>
        /// <param name="newsLetterSubscriptionId">The newsletter subscription identifier</param>
        /// <returns>NewsLetterSubscription entity</returns>
        public abstract DBNewsLetterSubscription GetNewsLetterSubscriptionById(int newsLetterSubscriptionId);

        /// <summary>
        /// Gets the newsletter subscription by newsletter subscription GUID
        /// </summary>
        /// <param name="newsLetterSubscriptionGuid">The newsletter subscription GUID</param>
        /// <returns>NewsLetterSubscription entity</returns>
        public abstract DBNewsLetterSubscription GetNewsLetterSubscriptionByGuid(Guid newsLetterSubscriptionGuid);

        /// <summary>
        /// Gets the newsletter subscription by email
        /// </summary>
        /// <param name="email">The Email</param>
        /// <returns>NewsLetterSubscription entity</returns>
        public abstract DBNewsLetterSubscription GetNewsLetterSubscriptionByEmail(string email);

        /// <summary>
        /// Gets the newsletter subscription collection
        /// </summary>
        /// <param name="showHidden">A value indicating whether the not active subscriptions should be loaded</param>
        /// <returns>NewsLetterSubscription entity collection</returns>
        public abstract DBNewsLetterSubscriptionCollection GetAllNewsLetterSubscriptions(bool showHidden);

        /// <summary>
        /// Updates the newsletter subscription
        /// </summary>
        /// <param name="newsLetterSubscriptionId">The newsletter subscription identifier</param>
        /// <param name="newsLetterSubscriptionGuid">The newsletter subscription GUID</param>
        /// <param name="email">The subscriber email</param>
        /// <param name="isActive">A value indicating whether subscription is active</param>
        /// <param name="createdOn">The date and time of instance creation</param>
        /// <returns>NewsLetterSubscription entity</returns>
        public abstract DBNewsLetterSubscription UpdateNewsLetterSubscription(int newsLetterSubscriptionId, 
            Guid newsLetterSubscriptionGuid, string email, bool isActive, DateTime createdOn);

        /// <summary>
        /// Deletes the newsletter subscription
        /// </summary>
        /// <param name="newsLetterSubscriptionId">The newsletter subscription identifier</param>
        public abstract void DeleteNewsLetterSubscription(int newsLetterSubscriptionId);
        #endregion
    }
}

