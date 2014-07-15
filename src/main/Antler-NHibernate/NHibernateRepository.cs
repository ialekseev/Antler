using System.Linq;
using NHibernate;
using NHibernate.Linq;
using SmartElk.Antler.Core.Common.CodeContracts;
using SmartElk.Antler.Core.Domain;

namespace SmartElk.Antler.NHibernate
{
    public class NHibernateRepository<TEntity>: IRepository<TEntity> where TEntity: class
    {
        private readonly ISession _session;
        public NHibernateRepository(ISession session)
        {
            Requires.NotNull(session, "session");
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
        
        public TEntity Insert(TEntity entity)
        {
            Requires.NotNull(entity, "entity");
            _session.Save(entity);
            return entity;
        }

        public TId Insert<TId>(TEntity entity)
        {
            Requires.NotNull(entity, "entity");
            Requires.True(typeof(TId).IsValueType, "Only value type Ids are supported(int, decimal etc.)");

            return (TId)_session.Save(entity);
        }

        public TEntity Update(TEntity entity)
        {
            Requires.NotNull(entity, "entity");
            return _session.Merge(entity);
        }

        public void Delete(TEntity entity)
        {
            Requires.NotNull(entity, "entity");
            _session.Delete(entity);
        }

        public void Delete<TId>(TId id)
        {
            var entity = GetById(id);
            if (entity != null)
            {
                _session.Delete(entity);
            }            
        }        
    }
}
