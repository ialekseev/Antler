using System.Linq;

namespace SmartElk.Antler.Domain
{
    public interface IRepository<TEntity, in TId>
    {
        IQueryable<TEntity> AsQueryable();
        TEntity GetById(TId id);
        void Insert(TEntity entity);
        void Delete(TEntity entity);        
    }
}
