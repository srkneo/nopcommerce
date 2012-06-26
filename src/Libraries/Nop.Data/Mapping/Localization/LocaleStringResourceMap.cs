using System.Data.Entity.ModelConfiguration;
using Nop.Core.Domain.Localization;

namespace Nop.Data.Mapping.Localization
{
    public partial class LocaleStringResourceMap : EntityTypeConfiguration<LocaleStringResource>
    {
        public LocaleStringResourceMap()
        {
            this.ToTable("LocaleStringResource");
            this.HasKey(lsr => lsr.Id);

            //This column is indexed so make sure it is created as a varchar for MySql
            //MySql will by default create this as a variation of type TEXT unless specified
            this.Property(lsr => lsr.ResourceName).IsRequired().HasMaxLength(200).HasColumnType("varchar");

            this.Property(lsr => lsr.ResourceValue).IsRequired().IsMaxLength();


            this.HasRequired(lsr => lsr.Language)
                .WithMany(l => l.LocaleStringResources)
                .HasForeignKey(lsr => lsr.LanguageId);
        }
    }
}