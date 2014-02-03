using System;
using Antler.NHibernate.Configuration;
using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using SmartElk.Antler.Core.Common.CodeContracts;
using SmartElk.Antler.Core.Domain.Configuration;

namespace SmartElk.Antler.NHibernate.Oracle.Configuration
{
    public class NHibernatePlusSqlServer : NHibernateStorage<NHibernatePlusSqlServer>
    {                
        private readonly string _connectionString;
        private string _databaseSchemaName;
        private OracleDataClientConfiguration _oracleConfiguration;
                
        protected NHibernatePlusSqlServer(string connectionString)
        {
            _connectionString = connectionString;
            _oracleConfiguration = OracleDataClientConfiguration.Oracle10;
        }

        public static NHibernatePlusSqlServer Use(string connectionString)
        {
            Requires.NotNullOrEmpty(connectionString, "connectionString");
            return new NHibernatePlusSqlServer(connectionString);                         
        }

        public NHibernatePlusSqlServer WithDatabaseSchemaName(string databaseSchemaName)
        {
            Requires.NotNullOrEmpty(databaseSchemaName, "databaseSchemaName");
            _databaseSchemaName = databaseSchemaName;
            return this;
        }
        
        public NHibernatePlusSqlServer WithOracleConfiguration(OracleDataClientConfiguration oracleConfiguration)
        {            
            _oracleConfiguration = oracleConfiguration;
            return this;
        }
        
        public override void Configure(IDomainConfigurator configurator)
        {
            global::NHibernate.Cfg.Configuration configuration = null;

            var oracleConfiguration = _oracleConfiguration.ConnectionString(_connectionString);            
            if (!string.IsNullOrEmpty(_databaseSchemaName))
            {
                oracleConfiguration.DefaultSchema(_databaseSchemaName);
            }
            
            var sessionFactory = Fluently.Configure()
                                              .Database(oracleConfiguration)
                                              .Mappings(x => x.FluentMappings.AddFromAssembly(AssemblyWithMappings))
                                              .ExposeConfiguration(x =>
                                                  {
                                                      configuration = x;
                                                      if (ActionToApplyOnNHibernateConfiguration != null)
                                                      {
                                                          ActionToApplyOnNHibernateConfiguration(x);
                                                      }                                                                                                            
                                                  }).BuildSessionFactory();
            
            
            LatestConfigurationResult = new ConfigurationResult(sessionFactory, configuration);
        }
        
        private static ConfigurationResult LatestConfigurationResult { get; set; }        
    }
}
