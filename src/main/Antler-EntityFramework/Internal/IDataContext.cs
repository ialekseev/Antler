using System;
using System.Data.Entity;

namespace SmartElk.Antler.EntityFramework.Internal
{
    public interface IDataContext: IDisposable
    {
        IDbSet<TEntity> Set<TEntity>() where TEntity : class;
        void SaveChanges();
    }
}
