using Antler.NHibernate;
using Antler.NHibernate.Configuration;
using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using NHibernate.Tool.hbm2ddl;
using SmartElk.Antler.Core.Domain;
using SmartElk.Antler.Core.Domain.Configuration;

namespace SmartElk.Antler.NHibernate.SqlServer
{
    public class NHibernatePlusSqlServer : NHibernateStorage<NHibernatePlusSqlServer>
    {                
        private string _connectionString;
        private MsSqlConfiguration _msSqlConfiguration;
        private bool _generateDatabase;
        private bool _dropBeforeGeneration;

        protected NHibernatePlusSqlServer(string connectionString)
        {
            _connectionString = connectionString;
            _msSqlConfiguration = MsSqlConfiguration.MsSql2008;
        }

        public static NHibernatePlusSqlServer Use(string connectionString)
        {
            return new NHibernatePlusSqlServer(connectionString);                         
        }

        public NHibernatePlusSqlServer WithConnectionString(string connectionString)
        {
            this._connectionString = connectionString;
            return this;
        }

        public NHibernatePlusSqlServer WithSqlConfiguration(MsSqlConfiguration msSqlConfiguration)
        {
            _msSqlConfiguration = msSqlConfiguration;
            return this;
        }

        public NHibernatePlusSqlServer GenerateDatabase(bool dropBeforeGeneration = false)
        {
            _generateDatabase = true;
            _dropBeforeGeneration = dropBeforeGeneration;
            return this;
        }

        public override void Configure(IDomainConfigurator configurator)
        {                                    
            global::NHibernate.Cfg.Configuration configuration = null;
            var sessionFactory = Fluently.Configure()
                .Database(_msSqlConfiguration.ConnectionString(_connectionString))
                .Mappings(x => x.FluentMappings.AddFromAssembly(AssemblyWithMappings)).
                ExposeConfiguration(x =>
                    {
                      if (_generateDatabase)
                      {
                          var export = new SchemaExport(x);
                          if (_dropBeforeGeneration)
                          {
                              export.Drop(true, true);
                          }
                          export.Execute(false, true, false);  
                      }                        
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
