using System.Data.Entity;
using System.Reflection;
using SmartElk.Antler.Core.Domain;
using SmartElk.Antler.Core.Domain.Configuration;
using SmartElk.Antler.EntityFramework.Internal;

namespace SmartElk.Antler.EntityFramework.Configuration
{
    public class EntityFrameworkStorage: IEntityFrameworkStorage
    {        
        private Assembly _assemblyWithMappings;
        private string _connectionString;
        private IDatabaseInitializer<DataContext> _databaseInitializer;
        private bool _enableLazyLoading;

        protected EntityFrameworkStorage()
        {
            _assemblyWithMappings = Assembly.GetCallingAssembly();
            _databaseInitializer = new DropCreateDatabaseAlways<DataContext>();
            _enableLazyLoading = true;
        }

        public static IEntityFrameworkStorage Use()
        {
            return new EntityFrameworkStorage();            
        }

        public IEntityFrameworkStorage WithConnectionString(string connectionString)
        {            
            this._connectionString = connectionString;
            return this;
        }

        public IEntityFrameworkStorage WithMappings(Assembly assembly)
        {
            this._assemblyWithMappings = assembly;
            return this;
        }

        public IEntityFrameworkStorage WithDatabaseInitializer(IDatabaseInitializer<DataContext> databaseInitializer)
        {
            this._databaseInitializer = databaseInitializer;
            return this;
        }

        public IEntityFrameworkStorage WithLazyLoading()
        {
            this._enableLazyLoading = true;
            return this;
        }

        public IEntityFrameworkStorage WithoutLazyLoading()
        {
            this._enableLazyLoading = false;
            return this;
        }

        public virtual void Configure(IDomainConfigurator configurator)
        {
            var dataContextFactory = string.IsNullOrEmpty(_connectionString)
                                         ? new DataContextFactory(_assemblyWithMappings, _enableLazyLoading)
                                         : new DataContextFactory(_connectionString, _assemblyWithMappings,
                                                                  _enableLazyLoading); 
            
            var sessionScopeFactory = new EntityFrameworkSessionScopeFactory(dataContextFactory);
            configurator.Configuration.Container.PutWithNameOrDefault<ISessionScopeFactory>(sessionScopeFactory, configurator.Name);                                    
            Database.SetInitializer(_databaseInitializer);             
        }
    }
}
