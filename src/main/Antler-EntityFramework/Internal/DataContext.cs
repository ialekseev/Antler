using System.Data.Entity;

namespace SmartElk.Antler.EntityFramework.Internal
{
    public class DataContext: DbContext, IDataContext
    {
        public new IDbSet<TEntity> Set<TEntity>() where TEntity : class
        {
            return base.Set<TEntity>();
        }

        public new void SaveChanges()
        {
            base.SaveChanges();
        }
    }
}
