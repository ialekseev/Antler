using System.Linq;

namespace SmartElk.Antler.Core.Domain
{
    public interface IRepository<TEntity>
    {
        /// <summary>
        /// IQueryable interface to make queries.
        /// </summary>   
        IQueryable<TEntity> AsQueryable();

        /// <summary>
        /// Get one entity by id.
        /// </summary>   
        TEntity GetById<TId>(TId id);

        /// <summary>
        /// Insert new entity and get it back.
        /// </summary>   
        TEntity Insert(TEntity entity);

        /// <summary>
        /// Insert new entity and get generated identifier back.
        /// </summary>   
        TId Insert<TId>(TEntity entity) where TId : struct;

        /// <summary>
        /// Update entity.
        /// </summary>   
        TEntity Update(TEntity entity);

        /// <summary>
        /// Insert or update entity and get it back.
        /// </summary>   
        TEntity InsertOrUpdate(TEntity entity);

        /// <summary>
        /// Delete entity.
        /// </summary>   
        void Delete(TEntity entity);

        /// <summary>
        /// Delete entity by id.
        /// </summary>   
        void Delete<TId>(TId id);
    }
}
