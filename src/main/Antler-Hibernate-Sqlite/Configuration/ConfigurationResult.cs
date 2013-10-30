using NHibernate;

namespace SmartElk.Antler.Hibernate.Sqlite.Configuration
{
    public class ConfigurationResult
    {
        public ISessionFactory SessionFactory { get; set; }
        public NHibernate.Cfg.Configuration Configuration { get; set; }

        public ConfigurationResult(ISessionFactory sessionFactory, NHibernate.Cfg.Configuration configuration)
        {
            SessionFactory = sessionFactory;
            Configuration = configuration;
        }
    }
}
