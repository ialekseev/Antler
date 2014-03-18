using System.Linq;

namespace SmartElk.Antler.Core.Domain
{
    public interface IRepository<TEntity>
    {
        IQueryable<TEntity> AsQueryable();
        TEntity GetById<TId>(TId id);        
        void Insert(TEntity entity);
        void Update(TEntity entity); //todo: cover by integration tests
        void Delete(TEntity entity);
        void Delete<TId>(TId id);
    }
}
