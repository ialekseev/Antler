using System.Reflection;
using Antler.Hibernate;
using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using SmartElk.Antler.Abstractions.Registration;
using SmartElk.Antler.Domain;
using SmartElk.Antler.Domain.Configuration;

namespace SmartElk.Antler.Hibernate.Sqlite.Configuration
{
    public static class ConfigurationEx
    {           
        public static AsInMemoryStorageResult AsInMemoryStorage(this IDomainConfigurator domainConfigurator, Assembly assemblyWithMappings)
        {                        
            NHibernate.Cfg.Configuration configuration = null;
            var sessionFactory = Fluently.Configure()
                .Database(SQLiteConfiguration
                            .Standard.InMemory
                )
                .Mappings(x => x.FluentMappings.AddFromAssembly(assemblyWithMappings)).
                ExposeConfiguration(x =>
                {
                    configuration = x;
                })
                .BuildSessionFactory();

            var sessionScopeFactory = new HibernateSessionScopeFactory(sessionFactory);
            domainConfigurator.Configuration.Container.Put(Binding.Use(sessionScopeFactory).As<ISessionScopeFactory>());  
            
            return new AsInMemoryStorageResult(sessionFactory, configuration);                                    
        }        
    }
}
