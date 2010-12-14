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

using System.Collections.Generic;
using System.Data.Entity;
using System.Data.Entity.Database;
using System.Data.Entity.ModelConfiguration;
using Nop.Core.Domain;


namespace Nop.Core.Domain.Mapping
{
    public partial class SettingMap : EntityTypeConfiguration<Setting>
    {
        public SettingMap()
        {
            //modelBuilder.Entity<Setting>().ToTable("Nop_Setting");
            this.HasKey(s => s.Id);
            this.Property(s => s.Name).IsRequired().HasMaxLength(200);
            this.Property(s => s.Value).IsRequired().HasMaxLength(2000);
            this.Property(s => s.Description).IsRequired();
        }
    }
}