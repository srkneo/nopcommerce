﻿using System.Data.Entity.ModelConfiguration;
using Nop.Core.Domain.Forums;

namespace Nop.Data.Mapping.Content.Forums
{
    public partial class ForumMap : EntityTypeConfiguration<Forum>
    {
        public ForumMap()
        {
            this.ToTable("Forums_Forum");
            this.HasKey(f => f.Id);
            this.Property(f => f.Name).IsRequired().HasMaxLength(200);
            this.Property(f => f.Description).IsMaxLength();

            this.Ignore(f => f.LastPost);
            this.Ignore(f => f.LastTopic);
            this.Ignore(f => f.LastPostCustomer);

            this.HasRequired(f => f.ForumGroup)
                .WithMany(fg => fg.Forums)
                .HasForeignKey(f => f.ForumGroupId);
        }
    }
}