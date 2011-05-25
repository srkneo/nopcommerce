﻿using System.Collections.Generic;
using Nop.Core;

namespace Nop.Core.Domain.Logging
{
    /// <summary>
    /// Represents an activity log type record
    /// </summary>
    public partial class ActivityLogType : BaseEntity
    {
        #region Ctor
        public ActivityLogType()
        {
            this.ActivityLog = new List<ActivityLog>();
        }
        #endregion

        #region Properties

        /// <summary>
        /// Gets or sets the system keyword
        /// </summary>
        public string SystemKeyword { get; set; }

        /// <summary>
        /// Gets or sets the display name
        /// </summary>
        public string Name { get; set; }

        /// <summary>
        /// Gets or sets a value indicating whether the activity log type is enabled
        /// </summary>
        public bool Enabled { get; set; }
        #endregion

        #region Navigation Properties

        /// <summary>
        /// Gets the activity log
        /// </summary>
        public virtual ICollection<ActivityLog> ActivityLog { get; set; }

        #endregion
    }
}
