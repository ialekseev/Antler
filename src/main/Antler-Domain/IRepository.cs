using System.Collections.Generic;
using System.Linq;

namespace SmartElk.Antler.Domain
{
    public interface IRepository<TEntity, TId>
    {
        IQueryable<TEntity> AsQueryable();
        TId Save(TEntity entity);
        void SaveOrUpdate(TEntity entity);
        TEntity Update(TEntity entity);
        TEntity Get(TId id);
        IEnumerable<TEntity> GetAll();
        void Delete(TId id);
    }
}
