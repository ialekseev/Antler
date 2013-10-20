using System.Data.Entity;
using System.Reflection;
using SmartElk.Antler.Domain.Configuration;
using SmartElk.Antler.EntityFramework.Internal;


namespace SmartElk.Antler.EntityFramework.Sqlite.Configuration
{
    public static class ConfigurationEx
    {
        public static void AsInMemoryStorage(this IDomainConfigurator domainConfigurator)
        {
            var entityFrameworkSqlCeConfigurator = EntityFrameworkSqlCeConfigurator.Create(domainConfigurator);
            entityFrameworkSqlCeConfigurator.AsInMemoryStorage();
        }

        public static EntityFrameworkSqlCeConfigurator WithMappings(this IDomainConfigurator domainConfigurator, Assembly assemblyWithMappings)
        {
            var configurator = EntityFrameworkSqlCeConfigurator.Create(domainConfigurator);
            configurator.WithMappings(assemblyWithMappings);
            return configurator;
        }

        public static EntityFrameworkSqlCeConfigurator WithDatabaseInitializer(this IDomainConfigurator domainConfigurator, IDatabaseInitializer<DataContext> databaseInitializer)
        {
            var configurator = EntityFrameworkSqlCeConfigurator.Create(domainConfigurator);
            configurator.WithDatabaseInitializer(databaseInitializer);
            return configurator;
        }
    }
}
