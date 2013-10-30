using System.Data.Entity;
using System.Reflection;
using SmartElk.Antler.Domain.Configuration;
using SmartElk.Antler.EntityFramework.Internal;

namespace SmartElk.Antler.EntityFramework.Configuration
{
    public static class ConfigurationEx
    {
        public static void AsInMemoryStorage(this IDomainConfigurator domainConfigurator)
        {
            var entityFrameworkSqlCeConfigurator = EntityFrameworkConfigurator.Create(domainConfigurator);
            entityFrameworkSqlCeConfigurator.Configure();
        }

        public static EntityFrameworkConfigurator WithEntityFramework(this IDomainConfigurator domainConfigurator, Assembly assemblyWithMappings)
        {
            var configurator = EntityFrameworkConfigurator.Create(domainConfigurator);
            configurator.WithMappings(assemblyWithMappings);
            return configurator;
        }

        public static EntityFrameworkConfigurator WithDatabaseInitializer(this IDomainConfigurator domainConfigurator, IDatabaseInitializer<DataContext> databaseInitializer)
        {
            var configurator = EntityFrameworkConfigurator.Create(domainConfigurator);
            configurator.WithDatabaseInitializer(databaseInitializer);
            return configurator;
        }
    }
}
