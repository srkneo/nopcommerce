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

using System.Data.Entity.ModelConfiguration;
using Nop.Core.Domain.Directory;


namespace Nop.Data.Mapping.Directory
{
    public partial class CurrencyMap : EntityTypeConfiguration<Currency>
    {
        public CurrencyMap()
        {
            this.ToTable("Currency");
            this.HasKey(c =>c.Id);
            this.Property(c => c.Name).IsRequired().HasMaxLength(50);
            this.Property(c => c.CurrencyCode).IsRequired().HasMaxLength(5);
            this.Property(c => c.DisplayLocale).IsRequired().HasMaxLength(50);
            this.Property(c => c.CustomFormatting).HasMaxLength(50);
            this.Property(c => c.Rate).HasPrecision(18, 4);
        }
    }
}