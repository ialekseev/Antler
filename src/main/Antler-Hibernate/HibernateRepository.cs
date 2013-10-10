using System.Collections.Generic;
using System.Linq;
using NHibernate;
using NHibernate.Linq;
using SmartElk.Antler.Domain;

namespace Antler.Hibernate
{
    public class HibernateRepository<TEntity, TId>: IRepository<TEntity, TId> where TEntity: class
    {
        private readonly ISession _session;
        public HibernateRepository(ISession session)
        {
            _session = session;
        }

        public virtual IQueryable<TEntity> AsQueryable()
        {
            return _session.Query<TEntity>();
        }
        
        public virtual TId Save(TEntity entity)
        {
            return (TId)_session.Save(entity);
        }

        public virtual void SaveOrUpdate(TEntity entity)
        {
            _session.SaveOrUpdate(entity);
        }

        public virtual TEntity Update(TEntity entity)
        {
            _session.Update(entity);
            return entity;
        }

        public virtual TEntity Get(TId id)
        {
            return _session.Get<TEntity>(id);            
        }

        public virtual IEnumerable<TEntity> GetAll()
        {
            return _session.QueryOver<TEntity>().Future();
        }
        
        public virtual void Delete(TId id)
        {
            _session.Delete(_session.Load<TEntity>(id));
        }        
    }
}
