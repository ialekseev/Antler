using System.Data.Entity;
using System.Reflection;
using SmartElk.Antler.Domain;
using SmartElk.Antler.Domain.Configuration;
using SmartElk.Antler.EntityFramework.Internal;

namespace SmartElk.Antler.EntityFramework.Configuration
{
    public class EntityFrameworkConfigurator
    {
        private IDomainConfigurator _domainConfigurator;
        private Assembly _assemblyWithMappings;
        private IDatabaseInitializer<DataContext> _databaseInitializer;
        private bool _enableLazyLoading;

        private EntityFrameworkConfigurator()
        {
            _assemblyWithMappings = Assembly.GetCallingAssembly();
            _databaseInitializer = new DropCreateDatabaseAlways<DataContext>();
            _enableLazyLoading = true;
        }

        public static EntityFrameworkConfigurator Create(IDomainConfigurator domainConfigurator)
        {
            var configurator = new EntityFrameworkConfigurator { _domainConfigurator = domainConfigurator };
            return configurator;
        }

        public EntityFrameworkConfigurator WithMappings(Assembly assembly)
        {
            this._assemblyWithMappings = assembly;
            return this;
        }

        public EntityFrameworkConfigurator WithDatabaseInitializer(IDatabaseInitializer<DataContext> databaseInitializer)
        {
            this._databaseInitializer = databaseInitializer;
            return this;
        }

        public EntityFrameworkConfigurator WithLazyLoading()
        {
            this._enableLazyLoading = true;
            return this;
        }

        public EntityFrameworkConfigurator WithoutLazyLoading()
        {
            this._enableLazyLoading = false;
            return this;
        }

        public void AsInMemoryStorage()
        {                                    
            var dataContextFactory = new DataContextFactory(_assemblyWithMappings, _enableLazyLoading);
            var sessionScopeFactory = new EntityFrameworkSessionScopeFactory(dataContextFactory);
            _domainConfigurator.Configuration.Container.Put<ISessionScopeFactory>(sessionScopeFactory, _domainConfigurator.Name);                                    
            Database.SetInitializer(_databaseInitializer);             
        }
    }
}
