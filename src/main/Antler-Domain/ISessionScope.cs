using System;

namespace SmartElk.Antler.Domain
{
    public interface ISessionScope: IDisposable
    {
        void Commit();
        void Rollback();
        IRepository<TEntity, TId> Repository<TEntity, TId>() where TEntity : class;
        object InternalSession { get; }
    }
}
