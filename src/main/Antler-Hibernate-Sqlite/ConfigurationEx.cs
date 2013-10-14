using System.Reflection;
using Antler.Hibernate;
using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using NHibernate;
using NHibernate.Cfg;
using NHibernate.Tool.hbm2ddl;
using SmartElk.Antler.Abstractions.Registration;
using SmartElk.Antler.Domain;
using SmartElk.Antler.Domain.Configuration;

namespace SmartElk.Antler.Hibernate.Sqlite
{
    public static class ConfigurationEx
    {        
        public static void AsInMemoryStorage(this IDomainConfigurator domainConfigurator, Assembly assemblyWithMappings)
        {                        
            Configuration configuration = null;
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
  
            UnitOfWork.SetActionToDoAfterSessionOpen(sessionScope =>
                new SchemaExport(configuration).Execute(false, true, false, ((ISession)(sessionScope.InternalSession)).Connection, null));
        }
    }
}
