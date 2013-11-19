using NHibernate;

namespace SmartElk.Antler.NHibernate.Sqlite.Configuration
{
    public class ConfigurationResult
    {
        public ISessionFactory SessionFactory { get; set; }
        public global::NHibernate.Cfg.Configuration Configuration { get; set; }

        public ConfigurationResult(ISessionFactory sessionFactory, global::NHibernate.Cfg.Configuration configuration)
        {
            SessionFactory = sessionFactory;
            Configuration = configuration;
        }
    }
}
