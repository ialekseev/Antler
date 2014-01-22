using Antler.NHibernate;
using Antler.NHibernate.Configuration;
using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using SmartElk.Antler.Core.Common.CodeContracts;
using SmartElk.Antler.Core.Domain;
using SmartElk.Antler.Core.Domain.Configuration;

namespace SmartElk.Antler.NHibernate.SqlServer
{
    public class NHibernatePlusSqlServer : NHibernateStorage<NHibernatePlusSqlServer>
    {                
        private string _connectionString;

        protected NHibernatePlusSqlServer()
        {            
        }

        public static NHibernatePlusSqlServer Use
        {
            get { return new NHibernatePlusSqlServer(); }                        
        }

        public NHibernatePlusSqlServer WithConnectionString(string connectionString)
        {
            this._connectionString = connectionString;
            return this;
        }

        public override void Configure(IDomainConfigurator configurator)
        {            
            Assumes.True(!string.IsNullOrEmpty(_connectionString), "Can't configure without provided connection string");
            
            global::NHibernate.Cfg.Configuration configuration = null;
            var sessionFactory = Fluently.Configure()
                .Database(MsSqlConfiguration.MsSql2008.ConnectionString(_connectionString))
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
