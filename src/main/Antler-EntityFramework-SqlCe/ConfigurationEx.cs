using System.Data.Entity;
using System.Reflection;
using SmartElk.Antler.Abstractions.Registration;
using SmartElk.Antler.Domain;
using SmartElk.Antler.Domain.Configuration;
using SmartElk.Antler.EntityFramework.Internal;

namespace SmartElk.Antler.EntityFramework.Sqlite
{
    public static class ConfigurationEx
    {        
        public static void AsInMemoryStorage(this IDomainConfigurator domainConfigurator, Assembly assemblyWithMappings, IDatabaseInitializer<DataContext> dbInitializer)
        {
            var dataContextFactory = new DataContextFactory(assemblyWithMappings);
            var sessionScopeFactory = new EntityFrameworkSessionScopeFactory(dataContextFactory);
            domainConfigurator.Configuration.Container.Put(Binding.Use(sessionScopeFactory).As<ISessionScopeFactory>());

            Database.SetInitializer(dbInitializer);                        
        }
    }
}
