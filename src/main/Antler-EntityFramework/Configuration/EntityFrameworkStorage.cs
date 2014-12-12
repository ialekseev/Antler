using System;
using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using SmartElk.Antler.Core.Common;
using SmartElk.Antler.Core.Common.CodeContracts;
using SmartElk.Antler.Core.Domain;
using SmartElk.Antler.Core.Domain.Configuration;
using SmartElk.Antler.EntityFramework.Internal;

namespace SmartElk.Antler.EntityFramework.Configuration
{
    public class EntityFrameworkStorage : AbstractOrmStorage<EntityFrameworkStorage>
    {                        
        private Option<string> _connectionString;        
        private Action _applyBeforeConfiguration;
        private IDatabaseInitializer<DataContext> _databaseInitializer;
        private Action<DbContextConfiguration> _applyOnConfiguration;
        private bool _recreateDatabase;
        
        protected EntityFrameworkStorage()
        {
            _connectionString = Option<string>.None;
            _applyBeforeConfiguration = () => { };
            _databaseInitializer = new CreateDatabaseIfNotExists<DataContext>();            
            _applyOnConfiguration = configuration => configuration.LazyLoadingEnabled = true;                         
        }
        
        public static EntityFrameworkStorage Use
        {
            get { return new EntityFrameworkStorage(); }
        }
        
        public EntityFrameworkStorage WithConnectionString(string connectionString)
        {            
            Requires.NotNullOrEmpty(connectionString, "connectionString");
            _connectionString = connectionString.AsOption();
            return this;
        }

        public EntityFrameworkStorage ApplyBeforeConfiguration(Action applyBeforeConfiguration)
        {
            _applyBeforeConfiguration = applyBeforeConfiguration;
            return this;
        }
        
        public EntityFrameworkStorage WithDatabaseInitializer(IDatabaseInitializer<DataContext> databaseInitializer)
        {
            Requires.NotNull(databaseInitializer, "databaseInitializer");
            _databaseInitializer = databaseInitializer;
            return this;
        }
        
        public EntityFrameworkStorage ApplyOnConfiguration(Action<DbContextConfiguration> applyOnConfiguration)
        {
            Requires.NotNull(applyOnConfiguration, "applyOnConfiguration");
            this._applyOnConfiguration += applyOnConfiguration;
            return this;
        }

        public EntityFrameworkStorage WithLazyLoading()
        {
            _applyOnConfiguration += configuration => configuration.LazyLoadingEnabled = true; 
            return this;
        }

        public EntityFrameworkStorage WithoutLazyLoading()
        {
            _applyOnConfiguration += configuration => configuration.LazyLoadingEnabled = false; 
            return this;
        }

        public EntityFrameworkStorage WithRecreatedDatabase(bool really)
        {
            if (really)
              this._recreateDatabase = true;
            return this;
        }

        protected override ISessionScopeFactory ConfigureInternal(IDomainConfigurator configurator)
        {
            Requires.NotNull(configurator, "configurator");

            Assumes.True(AssemblyWithMappings != null, "Specify Assembly with Mappings");

            _applyBeforeConfiguration();
            
            var dataContextFactory =  new DataContextFactory(_connectionString, AssemblyWithMappings, _applyOnConfiguration);                                                                                    
            var sessionScopeFactory = new EntityFrameworkSessionScopeFactory(dataContextFactory);
                                                
            Database.SetInitializer(_databaseInitializer);     
        
            if (_recreateDatabase)
            {
                var context = sessionScopeFactory.CreateContext();
                context.Database.Delete();
                context.Database.Create();
            }

            return sessionScopeFactory;
        }
    }
}
