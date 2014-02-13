using System.Data.Entity;
using System.Reflection;
using SmartElk.Antler.Core.Common.CodeContracts;
using SmartElk.Antler.Core.Domain;
using SmartElk.Antler.Core.Domain.Configuration;
using SmartElk.Antler.EntityFramework.Internal;

namespace SmartElk.Antler.EntityFramework.Configuration
{
    public class EntityFrameworkStorage : AbstractStorage
    {        
        private Assembly _assemblyWithMappings;
        private string _connectionString;
        private IDatabaseInitializer<DataContext> _databaseInitializer;
        private bool _enableLazyLoading;
        private bool _recreateDatabase;

        protected EntityFrameworkStorage()
        {
            _assemblyWithMappings = Assembly.GetCallingAssembly();
            _databaseInitializer = new CreateDatabaseIfNotExists<DataContext>();
            _enableLazyLoading = true;
        }
        
        public static EntityFrameworkStorage Use
        {
            get { return new EntityFrameworkStorage(); }
        }
        
        public EntityFrameworkStorage WithConnectionString(string connectionString)
        {            
            Requires.NotNullOrEmpty(connectionString, "connectionString");
            _connectionString = connectionString;
            return this;
        }

        public EntityFrameworkStorage WithMappings(Assembly assembly)
        {
            Requires.NotNull(assembly, "assembly");
            _assemblyWithMappings = assembly;
            return this;
        }

        public EntityFrameworkStorage WithDatabaseInitializer(IDatabaseInitializer<DataContext> databaseInitializer)
        {
            Requires.NotNull(databaseInitializer, "databaseInitializer");
            _databaseInitializer = databaseInitializer;
            return this;
        }

        public EntityFrameworkStorage WithLazyLoading()
        {
            this._enableLazyLoading = true;
            return this;
        }

        public EntityFrameworkStorage WithoutLazyLoading()
        {
            this._enableLazyLoading = false;
            return this;
        }

        public EntityFrameworkStorage WithRecreatedDatabase()
        {
            this._recreateDatabase = true;
            return this;
        }

        public override void Configure(IDomainConfigurator configurator)
        {
            var dataContextFactory = string.IsNullOrEmpty(_connectionString)
                                         ? new DataContextFactory(_assemblyWithMappings, _enableLazyLoading)
                                         : new DataContextFactory(_connectionString, _assemblyWithMappings,
                                                                  _enableLazyLoading); 
            
            var sessionScopeFactory = new EntityFrameworkSessionScopeFactory(dataContextFactory);
            configurator.Configuration.Container.PutWithNameOrDefault<ISessionScopeFactory>(sessionScopeFactory, configurator.Name);                                    
            Database.SetInitializer(_databaseInitializer);     
        
            if (_recreateDatabase)
            {
                var context = sessionScopeFactory.CreateContext();
                context.Database.Delete();
                context.Database.Create();
            }
        }
    }
}
