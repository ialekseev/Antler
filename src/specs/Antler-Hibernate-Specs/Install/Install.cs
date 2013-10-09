using Antler.Hibernate;
using Castle.MicroKernel.Registration;
using Castle.Windsor;
using CommonServiceLocator.WindsorAdapter;
using Microsoft.Practices.ServiceLocation;
using NHibernate;
using SmartElk.Antler.Domain;

namespace SmartElk.Antler.Hibernate.Specs.Install
{
    public class Install //todo: fluent registrations here(abstract Windsor container + get rid of ServiceLocator)
    {
        private static WindsorContainer Container { get; set; }

        public static void RegisterComponents()
        {
            if (Container == null)
            {
                Container = new WindsorContainer();

                Container.Register(Component.For<ISessionScopeFactory>().ImplementedBy<HibernateSessionScopeFactory>());
                Container.Register(Component.For<ISessionFactory>().Instance(SqliteSessionFactoryCreator.CreateFactory()).LifeStyle.Singleton);

                ServiceLocator.SetLocatorProvider(() => new WindsorServiceLocator(Container));
            }            
        }
    }
}
