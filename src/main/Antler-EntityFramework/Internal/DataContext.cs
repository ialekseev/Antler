using System;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.ModelConfiguration;
using System.Linq;
using System.Reflection;
using SmartElk.Antler.Core.Common.CodeContracts;

namespace SmartElk.Antler.EntityFramework.Internal
{
    public class DataContext: DbContext
    {
        private readonly Assembly _assemblyWithMappings;

        public DataContext(string connectionString, Assembly assemblyWithMappings, Action<DbContextConfiguration> applyOnConfiguration)
            : base(connectionString)
        {
            Requires.NotNullOrEmpty(connectionString, "connectionString");
            Requires.NotNull(assemblyWithMappings, "assemblyWithMappings");
            Requires.NotNull(applyOnConfiguration, "applyOnConfiguration");

            _assemblyWithMappings = assemblyWithMappings;            
            applyOnConfiguration(this.Configuration);
        }

        public DataContext(Assembly assemblyWithMappings, Action<DbContextConfiguration> applyOnConfiguration)            
        {            
            Requires.NotNull(assemblyWithMappings, "assemblyWithMappings");
            Requires.NotNull(applyOnConfiguration, "applyOnConfiguration");

            _assemblyWithMappings = assemblyWithMappings;
            applyOnConfiguration(this.Configuration);
        }
        
                            
        protected override void OnModelCreating(DbModelBuilder modelBuilder)
        {                        
            RegisterMappings(modelBuilder);
            base.OnModelCreating(modelBuilder);
            modelBuilder.Conventions.Remove<System.Data.Entity.ModelConfiguration.Conventions.PluralizingTableNameConvention>();
        }

        private void RegisterMappings(DbModelBuilder modelBuilder)
        {
            Requires.NotNull(modelBuilder, "modelBuilder");

            var typesToRegister =
                _assemblyWithMappings.GetTypes().Where(
                    type =>
                    type.BaseType.IsGenericType &&
                    type.BaseType.GetGenericTypeDefinition() == typeof (EntityTypeConfiguration<>));

            foreach (object configurationInstance in typesToRegister.Select(Activator.CreateInstance))
            {
                modelBuilder.Configurations.Add((dynamic) configurationInstance);
            }
        }

        public DbContextTransaction BeginTransaction()
        {
            return this.Database.BeginTransaction();
        }        
    }
}
