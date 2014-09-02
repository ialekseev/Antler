using NHibernate;
using SmartElk.Antler.Core.Common.CodeContracts;
using SmartElk.Antler.Core.Domain;
using SmartElk.Antler.NHibernate.Internal;

namespace SmartElk.Antler.NHibernate
{
    public class NHibernateSessionScopeFactory: ISessionScopeFactory, ISessionScopeFactoryEx
    {
        private readonly ISessionFactory _sessionFactory;
        private ISession _session;
        
        public NHibernateSessionScopeFactory(ISessionFactory sessionFactory)
        {
            Requires.NotNull(sessionFactory, "sessionFactory");            
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
            Requires.NotNull(session, "session");            
            _session = session;
        }

        void ISessionScopeFactoryEx.ResetSession()
        {
            _session = null;
        }
    }
}
