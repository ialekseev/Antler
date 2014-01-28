using Antler.NHibernate;
using Antler.NHibernate.Configuration;
using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using SmartElk.Antler.Core.Domain;
using SmartElk.Antler.Core.Domain.Configuration;

namespace SmartElk.Antler.NHibernate.Sqlite.Configuration
{
    public class NHibernatePlusSqlite : NHibernateStorage<NHibernatePlusSqlite>
    {        
        protected string FileName { get; set; }

        private bool IsFileStorage
        {
            get { return !string.IsNullOrEmpty(FileName); }
        }

        protected NHibernatePlusSqlite()
        {            
        }

        public static NHibernatePlusSqlite Use
        {
            get { return new NHibernatePlusSqlite();}                        
        }

        public NHibernatePlusSqlite AsInMemoryStorage()
        {
            FileName = null;
            return this;
        }

        public NHibernatePlusSqlite AsFileStorage(string fileName)
        {
            FileName = fileName;            
            return this;
        }

        public override void Configure(IDomainConfigurator configurator)
        {            
            global::NHibernate.Cfg.Configuration configuration = null;
            var sessionFactory = Fluently.Configure()
                .Database(IsFileStorage ? SQLiteConfiguration.Standard.UsingFile(FileName) : SQLiteConfiguration.Standard.InMemory())
                .Mappings(x => x.FluentMappings.AddFromAssembly(AssemblyWithMappings)).
                ExposeConfiguration(x =>
                {
                    configuration = x;
                })
                .BuildSessionFactory();

            var sessionScopeFactory = new NHibernateSessionScopeFactory(sessionFactory);
            configurator.Configuration.Container.PutWithNameOrDefault<ISessionScopeFactory>(sessionScopeFactory, configurator.Name); 
            
            LatestConfigurationResult = new ConfigurationResult(sessionFactory, configuration);
        }
        
        private static ConfigurationResult LatestConfigurationResult { get; set; }        
    }
}
