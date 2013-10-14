using System.Data.Entity;
using System.Linq;
using SmartElk.Antler.Domain;
using SmartElk.Antler.EntityFramework.Internal;

namespace SmartElk.Antler.EntityFramework
{
    public class EntityFrameworkRepository<TEntity, TId>: IRepository<TEntity, TId> where TEntity: class
    {
        private readonly IDataContext _context;
        public EntityFrameworkRepository(IDataContext context)
        {
            _context = context;
        }

        private IDbSet<TEntity> DbSet
        {
            get { return _context.Set<TEntity>();}
        }
        
        public virtual IQueryable<TEntity> AsQueryable()
        {
            return DbSet.AsQueryable();                        
        }

        public TEntity GetById(TId id)
        {
            return DbSet.Find(id);
        }

        public void Insert(TEntity entity)
        {
            DbSet.Add(entity);
        }

        public void Delete(TEntity entity)
        {
            DbSet.Remove(entity);
        }
    }
}
