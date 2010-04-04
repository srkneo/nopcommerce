using System;
using System.Collections.Generic;
using System.Linq;
using System.Text;

namespace NopSolutions.NopCommerce.DataAccess.Messages
{
    /// <summary>
    /// Represents DBNewsLetterSubscription entity
    /// </summary>
    public class DBNewsLetterSubscription : BaseDBEntity
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
        #endregion
    }
}
