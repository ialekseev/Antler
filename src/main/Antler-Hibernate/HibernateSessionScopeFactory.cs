using NHibernate;
using SmartElk.Antler.Domain;

namespace Antler.Hibernate
{
    public class HibernateSessionScopeFactory: ISessionScopeFactory, ISessionScopeFactoryEx
    {
        private readonly ISessionFactory _sessionFactory;
        private ISession _session;
        
        public HibernateSessionScopeFactory(ISessionFactory sessionFactory)
        {
            _sessionFactory = sessionFactory;
        }
        
        public ISessionScope Open()
        {            
            if (_session == null)
              return new HibernateSessionScope(_sessionFactory);

            return new HibernateSessionScope(_session);
        }

        void ISessionScopeFactoryEx.SetSession(object session)
        {
            _session = (ISession)session;
        }

        void ISessionScopeFactoryEx.ResetSession()
        {
            _session = null;
        }
    }
}
