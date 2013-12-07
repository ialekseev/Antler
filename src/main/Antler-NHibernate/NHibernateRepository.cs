using System.Linq;
using NHibernate;
using NHibernate.Linq;
using SmartElk.Antler.Core.Domain;

namespace Antler.NHibernate
{
    public class NHibernateRepository<TEntity>: IRepository<TEntity> where TEntity: class
    {
        private readonly ISession _session;
        public NHibernateRepository(ISession session)
        {
            _session = session;
        }

        public virtual IQueryable<TEntity> AsQueryable()
        {
            return _session.Query<TEntity>();
        }

        public TEntity GetById<TId>(TId id)
        {
            return _session.Get<TEntity>(id);  
        }

        public void Insert(TEntity entity)
        {
            _session.Save(entity);
        }

        public void Delete(TEntity entity)
        {
            _session.Delete(entity);
        }               
    }
}
