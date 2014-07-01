using NHibernate;
using SmartElk.Antler.Core.Common.CodeContracts;

namespace SmartElk.Antler.NHibernate.Configuration
{
    public class ConfigurationResult
    {
        public ISessionFactory SessionFactory { get; set; }
        public global::NHibernate.Cfg.Configuration Configuration { get; set; }

        public ConfigurationResult(ISessionFactory sessionFactory, global::NHibernate.Cfg.Configuration configuration)
        {
            Requires.NotNull(sessionFactory, "sessionFactory");
            Requires.NotNull(configuration, "configuration");

            SessionFactory = sessionFactory;
            Configuration = configuration;
        }
    }
}
