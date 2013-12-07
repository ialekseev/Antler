using System.Data.Entity;
using System.Linq;
using SmartElk.Antler.Core.Domain;
using SmartElk.Antler.EntityFramework.Internal;

namespace SmartElk.Antler.EntityFramework
{
    public class EntityFrameworkRepository<TEntity>: IRepository<TEntity> where TEntity: class
    {
        private readonly IDataContext _context;
        public EntityFrameworkRepository(IDataContext context)
        {
            _context = context;
        }

        public IDbSet<TEntity> DbSet
        {
            get { return _context.Set<TEntity>();}
        }
        
        public IQueryable<TEntity> AsQueryable()
        {
            return DbSet.AsQueryable();
        }

        public TEntity GetById<TId>(TId id)
        {
            return DbSet.Find(id);
        }

        public void Insert(TEntity entity)
        {
            DbSet.Add(entity);
            _context.SaveChanges();
        }

        public void Delete(TEntity entity)
        {
            DbSet.Remove(entity);
            _context.SaveChanges();
        }
    }
}
