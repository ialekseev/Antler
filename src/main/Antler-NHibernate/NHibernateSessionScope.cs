using NHibernate;
using SmartElk.Antler.Core.Common.CodeContracts;
using SmartElk.Antler.Core.Domain;

namespace Antler.NHibernate
{
    public class NHibernateSessionScope: ISessionScope
    {
        private readonly ISession _session;
        private readonly ITransaction _transaction;
        private readonly bool _ownSession;
        
        public NHibernateSessionScope(ISessionFactory sessionFactory)
        {                        
            Requires.NotNull(sessionFactory, "sessionFactory");
            
            _session = sessionFactory.OpenSession();            
            _transaction = _session.BeginTransaction();
            _ownSession = true;
        }

        public NHibernateSessionScope(ISession session)
        {
            Requires.NotNull(session, "session");
            
            _session = session;
            _transaction = _session.BeginTransaction();
            _ownSession = false;
        }

        public void Commit()
        {
            AssertIfDone();
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
        
        public void Rollback()
        {
            AssertIfDone();
            _transaction.Rollback();
        }
        
        private void AssertIfDone()
        {
            Assumes.True(!_transaction.WasCommitted, "Transaction already was commited");
            Assumes.True(!_transaction.WasRolledBack, "Transaction already was rolled back");
        }

        public IRepository<TEntity> CreateRepository<TEntity>() where TEntity:class
        {
            return new NHibernateRepository<TEntity>(_session);
        }

        public TInternal GetInternal<TInternal>() where TInternal : class
        {
            var internalSession = _session as TInternal;
            Assumes.True(internalSession != null, "Can't cast Internal Session to TInternal type");
            return internalSession;
        }

        public void Dispose()
        {                        
            _transaction.Dispose();            
            if (_ownSession)                            
              _session.Dispose();                            
        }        
    }
}
