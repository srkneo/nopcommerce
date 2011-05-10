﻿using System.Data.Entity.ModelConfiguration;
using Nop.Core.Domain.Logging;

namespace Nop.Data.Mapping.Logging
{
    public partial class ActivityLogMap : EntityTypeConfiguration<ActivityLog>
    {
        public ActivityLogMap()
        {
            this.ToTable("ActivityLog");
            this.HasKey(al => al.ActivityLogId);
            this.Property(al => al.Comment).IsRequired().HasMaxLength(4000);

            this.HasRequired(al => al.ActivityLogType)
                .WithMany(alt => alt.ActivityLog)
                .HasForeignKey(al => al.ActivityLogTypeId);
            this.HasRequired(al => al.Customer)
                .WithMany(alt => alt.ActivityLog)
                .HasForeignKey(al => al.CustomerId);
        }
    }
}