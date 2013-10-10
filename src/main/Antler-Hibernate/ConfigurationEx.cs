using NHibernate;
using SmartElk.Antler.Abstractions.Configuration;
using SmartElk.Antler.Abstractions.Registration;
using SmartElk.Antler.Domain;

namespace Antler.Hibernate
{
    public static class ConfigurationEx
    {        
        public static void UseNHibernate(this IAntlerConfigurator nodeConfigurator, ISessionFactory sessionFactory)
        {
            var sessionScopeFactory = new HibernateSessionScopeFactory(sessionFactory);            
            nodeConfigurator.Configuration.Container.Put(Binding.Use(sessionScopeFactory).As<ISessionScopeFactory>());
            
            UnitOfWork.SessionScopeFactory = nodeConfigurator.Configuration.Container.Get<ISessionScopeFactory>(); //todo: move out from here
        }
    }
}
