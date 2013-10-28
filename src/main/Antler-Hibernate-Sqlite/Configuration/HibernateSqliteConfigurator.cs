using System.Reflection;
using Antler.Hibernate;
using FluentNHibernate.Cfg;
using FluentNHibernate.Cfg.Db;
using SmartElk.Antler.Domain.Configuration;

namespace SmartElk.Antler.Hibernate.Sqlite.Configuration
{
    public class HibernateSqliteConfigurator
    {
        private readonly IDomainConfigurator _domainConfigurator;
        private readonly Assembly _assemblyWithMappings;

        public HibernateSqliteConfigurator(IDomainConfigurator domainConfigurator, Assembly assemblyWithMappings)
        {
            _domainConfigurator = domainConfigurator;
            _assemblyWithMappings = assemblyWithMappings;
        }

        public AsInMemoryStorageResult AsInMemoryStorage()
        {
            NHibernate.Cfg.Configuration configuration = null;
            var sessionFactory = Fluently.Configure()
                .Database(SQLiteConfiguration
                            .Standard.InMemory
                )
                .Mappings(x => x.FluentMappings.AddFromAssembly(_assemblyWithMappings)).
                ExposeConfiguration(x =>
                {
                    configuration = x;
                })
                .BuildSessionFactory();

            var sessionScopeFactory = new HibernateSessionScopeFactory(sessionFactory);
            _domainConfigurator.RegisterSessionScopeFactory(sessionScopeFactory);

            return new AsInMemoryStorageResult(sessionFactory, configuration);
        }
    }
}
