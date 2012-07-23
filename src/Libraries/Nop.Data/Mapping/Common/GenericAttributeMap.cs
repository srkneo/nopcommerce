using System.Data.Entity.ModelConfiguration;
using Nop.Core.Domain.Common;

namespace Nop.Data.Mapping.Common
{
    public partial class GenericAttributeMap : EntityTypeConfiguration<GenericAttribute>
    {
        public GenericAttributeMap()
        {
            this.ToTable("GenericAttribute");
            this.HasKey(ga => ga.Id);

            //This column is indexed so make sure it is created as a varchar for MySql
            //MySql will by default create this as a variation of type TEXT unless specified
            this.Property(ga => ga.KeyGroup).IsRequired().HasMaxLength(400).HasColumnType("varchar");

            this.Property(ga => ga.Key).IsRequired().HasMaxLength(400);
            this.Property(ga => ga.Value).IsRequired().IsMaxLength();
        }
    }
}