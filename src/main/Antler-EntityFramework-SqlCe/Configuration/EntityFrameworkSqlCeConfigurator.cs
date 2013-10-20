using System.Data.Entity;
using System.Reflection;
using SmartElk.Antler.Abstractions.Registration;
using SmartElk.Antler.Domain;
using SmartElk.Antler.Domain.Configuration;
using SmartElk.Antler.EntityFramework.Internal;

namespace SmartElk.Antler.EntityFramework.Sqlite.Configuration
{
    public class EntityFrameworkSqlCeConfigurator
    {
        private IDomainConfigurator _domainConfigurator;
        private Assembly _assemblyWithMappings;
        private IDatabaseInitializer<DataContext> _databaseInitializer;
        private bool _enableLazyLoading;

        private EntityFrameworkSqlCeConfigurator()
        {
            _assemblyWithMappings = Assembly.GetCallingAssembly();
            _databaseInitializer = new DropCreateDatabaseAlways<DataContext>();
            _enableLazyLoading = true;
        }

        public static EntityFrameworkSqlCeConfigurator Create(IDomainConfigurator domainConfigurator)
        {
            var configurator = new EntityFrameworkSqlCeConfigurator {_domainConfigurator = domainConfigurator};
            return configurator;
        }

        public EntityFrameworkSqlCeConfigurator WithMappings(Assembly assembly)
        {
            this._assemblyWithMappings = assembly;
            return this;
        }

        public EntityFrameworkSqlCeConfigurator WithDatabaseInitializer(IDatabaseInitializer<DataContext> databaseInitializer)
        {
            this._databaseInitializer = databaseInitializer;
            return this;
        }

        public EntityFrameworkSqlCeConfigurator WithLazyLoading()
        {
            this._enableLazyLoading = true;
            return this;
        }
        
        public EntityFrameworkSqlCeConfigurator WithoutLazyLoading()
        {
            this._enableLazyLoading = false;
            return this;
        }

        public void AsInMemoryStorage()
        {                                    
            var dataContextFactory = new DataContextFactory(_assemblyWithMappings, _enableLazyLoading);
            var sessionScopeFactory = new EntityFrameworkSessionScopeFactory(dataContextFactory);
            _domainConfigurator.Configuration.Container.Put(Binding.Use(sessionScopeFactory).As<ISessionScopeFactory>());

            Database.SetInitializer(_databaseInitializer);             
        }
    }
}
