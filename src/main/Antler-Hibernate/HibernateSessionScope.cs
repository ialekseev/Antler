using NHibernate;
using SmartElk.Antler.Domain;

namespace Antler.Hibernate
{
    public class HibernateSessionScope: ISessionScope
    {
        private readonly ISession _session;
        private readonly ITransaction _transaction;
        private readonly bool _ownSession;
        
        public HibernateSessionScope(ISessionFactory sessionFactory)
        {                        
            _session = sessionFactory.OpenSession();            
            _transaction = _session.BeginTransaction();
            _ownSession = true;
        }

        public HibernateSessionScope(ISession session)
        {
            _session = session;
            _transaction = _session.BeginTransaction();
            _ownSession = false;
        }

        public void Commit()
        {
            try
            {
                _transaction.Commit();
            }
            catch (HibernateException)
            {
                _transaction.Rollback();
                throw;
            }            
        }
                
        public IRepository<TEntity> CreateRepository<TEntity>() where TEntity:class
        {
            return new HibernateRepository<TEntity>(_session);
        }

        public object InternalSession
        {
            get { return _session; }
        }

        public void Dispose()
        {            
            _transaction.Dispose();            
            if (_ownSession)                            
              _session.Dispose();                            
        }
    }
}
