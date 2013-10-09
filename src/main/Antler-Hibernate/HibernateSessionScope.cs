using Microsoft.Practices.ServiceLocation;
using NHibernate;
using SmartElk.Antler.Domain;

namespace Antler.Hibernate
{
    public class HibernateSessionScope: ISessionScope
    {
        private readonly ISession _session;
        private readonly ITransaction _transaction;
        
        public HibernateSessionScope()
        {
            var factory = ServiceLocator.Current.GetInstance<ISessionFactory>();
            _session = factory.OpenSession();
            _transaction = _session.BeginTransaction();            
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
    }
}
