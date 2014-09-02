using System.Linq;

namespace SmartElk.Antler.MongoDb.Internal
{
    public interface ISessionScopeEx
    {
        IQueryable<TEntity> AsQueryable<TEntity>();
        TEntity GetById<TId, TEntity>(TId id);
        void MarkAsNew<TEntity>(TEntity entity) where TEntity : class;
        void MarkAsUpdated<TEntity>(TEntity entity) where TEntity : class;
        void MarkAsDeleted<TEntity>(TEntity entity) where TEntity : class;
    }
}
