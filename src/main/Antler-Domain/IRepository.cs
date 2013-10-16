using System.Linq;

namespace SmartElk.Antler.Domain
{
    public interface IRepository<TEntity>
    {
        IQueryable<TEntity> AsQueryable();
        TEntity GetById<TId>(TId id);
        void Insert(TEntity entity);
        void Delete(TEntity entity);        
    }
}
