using Antler.Hibernate;
using Antler.Hibernate.Configuration;
using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using SmartElk.Antler.Domain;
using SmartElk.Antler.Domain.Configuration;

namespace SmartElk.Antler.Hibernate.Sqlite.Configuration
{
    public class HibernatePlusSqlite: HibernateStorage
    {                        
        protected HibernatePlusSqlite()
        {            
        }

        public static HibernatePlusSqlite Use
        {
            get { return new HibernatePlusSqlite();}                        
        }
        
        public override void Configure(IDomainConfigurator configurator)
        {
            NHibernate.Cfg.Configuration configuration = null;
            var sessionFactory = Fluently.Configure()
                .Database(SQLiteConfiguration
                            .Standard.InMemory
                )
                .Mappings(x => x.FluentMappings.AddFromAssembly(AssemblyWithMappings)).
                ExposeConfiguration(x =>
                {
                    configuration = x;
                })
                .BuildSessionFactory();

            var sessionScopeFactory = new HibernateSessionScopeFactory(sessionFactory);
            configurator.Configuration.Container.Put<ISessionScopeFactory>(sessionScopeFactory, configurator.Name); 
            
            LatestConfigurationResult = new ConfigurationResult(sessionFactory, configuration);
        }
        
        public static ConfigurationResult LatestConfigurationResult { get; set; } //todo: hide this(make private and call using reflection?)
    }
}
