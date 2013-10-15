using System.Reflection;
using Antler.Hibernate;
using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using NHibernate.Cfg;
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

            //todo: How to deal with the fact that Sqllite database exists only current for connect? http://stackoverflow.com/questions/189280/problem-using-sqlite-memory-with-nhibernate

            //Action<ISessionScope> action = sessionScope => new SchemaExport(configuration).Execute(false, true, false, ((ISession)(sessionScope.InternalSession)).Connection, null);
            
            var sessionScopeFactory = new HibernateSessionScopeFactory(sessionFactory);            
            domainConfigurator.Configuration.Container.Put(Binding.Use(sessionScopeFactory).As<ISessionScopeFactory>());               
        }        
    }
}
