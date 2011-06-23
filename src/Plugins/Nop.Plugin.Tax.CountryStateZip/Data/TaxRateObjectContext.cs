

using System;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Reflection;
using Nop.Core;
using Nop.Data;
using Nop.Plugin.Tax.CountryStateZip.Domain;


namespace Nop.Plugin.Tax.CountryStateZip.Data
{
    /// <summary>
    /// Object context
    /// </summary>
    public class TaxRateObjectContext : DbContext, IDbContext
    {
        public TaxRateObjectContext(string nameOrConnectionString)
            : base(nameOrConnectionString)
        {
            //((IObjectContextAdapter) this).ObjectContext.ContextOptions.LazyLoadingEnabled = true;
        }

        public DbSet<TaxRate> TaxRate { get; set; }

        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {
            modelBuilder.Configurations.Add(new TaxRateMap());

            //disable EdmMetadata generation
            modelBuilder.Conventions.Remove<IncludeMetadataConvention>();
         base.OnModelCreating(modelBuilder);
        }

        public string CreateDatabaseScript()
        {
            return ((IObjectContextAdapter)this).ObjectContext.CreateDatabaseScript();
        }

        public new IDbSet<TEntity> Set<TEntity>() where TEntity : BaseEntity
        {
            return base.Set<TEntity>();
        }

        /// <summary>
        /// Install
        /// </summary>
        public void Install()
        {
            //create all tables
            var dbScript = CreateDatabaseScript();
            Database.ExecuteSqlCommand(dbScript);
            SaveChanges();
        }

        /// <summary>
        /// Uninstall
        /// </summary>
        public void Uninstall()
        {
            var dbScript = "DROP TABLE TaxRate";
            Database.ExecuteSqlCommand(dbScript);
            SaveChanges();
        }
    }
}