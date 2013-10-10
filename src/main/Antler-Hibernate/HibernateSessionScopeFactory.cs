using NHibernate;
using SmartElk.Antler.Domain;

namespace Antler.Hibernate
{
    public class HibernateSessionScopeFactory: ISessionScopeFactory
    {
        private readonly ISessionFactory _sessionFactory;
        public HibernateSessionScopeFactory(ISessionFactory sessionFactory)
        {
            _sessionFactory = sessionFactory;
        }

        public ISessionScope Open()
        {            
            return new HibernateSessionScope(_sessionFactory);
        }
    }
}
