using NHibernate;
using SmartElk.Antler.Core.Domain;

namespace Antler.NHibernate
{
    public class NHibernateSessionScopeFactory: ISessionScopeFactory, ISessionScopeFactoryEx
    {
        private readonly ISessionFactory _sessionFactory;
        private ISession _session;
        
        public NHibernateSessionScopeFactory(ISessionFactory sessionFactory)
        {
            _sessionFactory = sessionFactory;
        }
        
        public ISessionScope Open()
        {            
            if (_session == null)
              return new NHibernateSessionScope(_sessionFactory);

            return new NHibernateSessionScope(_session);
        }

        void ISessionScopeFactoryEx.SetSession(ISession session)
        {
            _session = session;
        }

        void ISessionScopeFactoryEx.ResetSession()
        {
            _session = null;
        }
    }
}
