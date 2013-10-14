using System.Data.Entity;

namespace SmartElk.Antler.EntityFramework.Internal
{
    public interface IDataContext
    {
        IDbSet<TEntity> Set<TEntity>() where TEntity : class;
        void SaveChanges();
    }
}
