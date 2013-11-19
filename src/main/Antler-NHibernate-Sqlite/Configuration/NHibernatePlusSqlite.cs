using Antler.NHibernate;
using Antler.NHibernate.Configuration;
using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using SmartElk.Antler.Domain;
using SmartElk.Antler.Domain.Configuration;

namespace SmartElk.Antler.NHibernate.Sqlite.Configuration
{
    public class NHibernatePlusSqlite: NHibernateStorage
    {                        
        protected NHibernatePlusSqlite()
        {            
        }

        public static NHibernatePlusSqlite Use
        {
            get { return new NHibernatePlusSqlite();}                        
        }
        
        public override void Configure(IDomainConfigurator configurator)
        {
            global::NHibernate.Cfg.Configuration configuration = null;
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

            var sessionScopeFactory = new NHibernateSessionScopeFactory(sessionFactory);
            configurator.Configuration.Container.Put<ISessionScopeFactory>(sessionScopeFactory, configurator.Name); 
            
            LatestConfigurationResult = new ConfigurationResult(sessionFactory, configuration);
        }
        
        private static ConfigurationResult LatestConfigurationResult { get; set; }
    }
}
