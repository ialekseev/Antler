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
            if (_transaction.IsActive
                && !_transaction.WasCommitted
                && !_transaction.WasRolledBack                
                )
            {
                _transaction.Commit();
            }
        }

        public void Rollback()
        {
            if (_transaction.IsActive
                && !_transaction.WasCommitted
                && !_transaction.WasRolledBack                
                )
            {
                _transaction.Rollback();
            }
        }
        
        public IRepository<TEntity, TId> Repository<TEntity, TId>() where TEntity:class
        {
            return new HibernateRepository<TEntity, TId>(_session);
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
