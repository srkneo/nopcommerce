
using System.Data.Entity.ModelConfiguration;
using Nop.Core.Domain.Messages;


namespace Nop.Data.Mapping.Catalog
{
    public partial class MessageTemplateMap : EntityTypeConfiguration<MessageTemplate>
    {
        public MessageTemplateMap()
        {
            this.ToTable("MessageTemplate");
            this.HasKey(mt => mt.Id);

            this.Property(mt => mt.Name).IsRequired().HasMaxLength(200);
            this.Property(mt => mt.BccEmailAddresses).HasMaxLength(200);
            this.Property(mt => mt.Subject).HasMaxLength(200);
            this.Property(mt => mt.Body).IsMaxLength();

            this.HasRequired(mt => mt.EmailAccount)
                .WithMany(ea => ea.MessageTemplates)
                .HasForeignKey(mt => mt.EmailAccountId);
        }
    }
}