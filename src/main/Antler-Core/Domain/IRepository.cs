using System.Linq;

namespace SmartElk.Antler.Core.Domain
{
    public interface IRepository<TEntity>
    {
        IQueryable<TEntity> AsQueryable();
        TEntity GetById<TId>(TId id);        
        TEntity Insert(TEntity entity);
        TEntity Update(TEntity entity);
        void Delete(TEntity entity);
        void Delete<TId>(TId id);
    }
}
