﻿using System;
using NopSolutions.NopCommerce.BusinessLogic.CustomerManagement;

namespace NopSolutions.NopCommerce.BusinessLogic.Messages
{
    /// <summary>
    /// Represents NewsLetterSubscription entity
    /// </summary>
    public class NewsLetterSubscription : BaseEntity
    {
        #region Properties
        /// <summary>
        /// Gets or sets the newsletter subscription identifier
        /// </summary>
        public int NewsLetterSubscriptionID
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the newsletter subscription GUID
        /// </summary>
        public Guid NewsLetterSubscriptionGuid
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the subcriber email
        /// </summary>
        public string Email
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets a value indicating whether subscription is active
        /// </summary>
        public bool IsActive
        {
            get;
            set;
        }

        /// <summary>
        /// Gets or sets the date and time when subscription was created
        /// </summary>
        public DateTime CreatedOn
        {
            get;
            set;
        }

        /// <summary>
        /// Gets the customer associated with email
        /// </summary>
        public Customer Customer
        {
            get
            {
                return CustomerManager.GetCustomerByEmail(Email);
            }
        }
        #endregion
    }
}
