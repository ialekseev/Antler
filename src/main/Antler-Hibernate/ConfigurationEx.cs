using NHibernate;
using SmartElk.Antler.Abstractions.Configuration;
using SmartElk.Antler.Abstractions.Registration;
using SmartElk.Antler.Domain;

namespace Antler.Hibernate
{
    public static class ConfigurationEx
    {        
        public static void UseNHibernate(this IAntlerConfigurator configurator, ISessionFactory sessionFactory)
        {
            var sessionScopeFactory = new HibernateSessionScopeFactory(sessionFactory);            
            configurator.Configuration.Container.Put(Binding.Use(sessionScopeFactory).As<ISessionScopeFactory>());                        
        }
    }
}
