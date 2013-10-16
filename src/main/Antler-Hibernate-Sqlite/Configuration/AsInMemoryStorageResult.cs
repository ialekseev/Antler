using NHibernate;

namespace SmartElk.Antler.Hibernate.Sqlite.Configuration
{
    public class AsInMemoryStorageResult
    {
        public ISessionFactory SessionFactory { get; set; }
        public NHibernate.Cfg.Configuration Configuration { get; set; }

        public AsInMemoryStorageResult(ISessionFactory sessionFactory, NHibernate.Cfg.Configuration configuration)
        {
            SessionFactory = sessionFactory;
            Configuration = configuration;
        }
    }
}
