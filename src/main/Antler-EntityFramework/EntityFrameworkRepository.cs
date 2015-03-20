using System;
using System.Data.Entity;
using System.Linq;
using SmartElk.Antler.Core.Common.CodeContracts;
using SmartElk.Antler.Core.Domain;
using SmartElk.Antler.EntityFramework.Internal;

namespace SmartElk.Antler.EntityFramework
{
    public class EntityFrameworkRepository<TEntity>: IRepository<TEntity> where TEntity: class
    {
        private readonly DataContext _context;
        public EntityFrameworkRepository(DataContext context)
        {
            Requires.NotNull(context, "context");
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
        
        public TEntity Insert(TEntity entity)
        {
            Requires.NotNull(entity, "entity");

            _context.Entry(entity).State = EntityState.Added;            
            _context.SaveChanges();

            return entity;
        }

        public TId Insert<TId>(TEntity entity) where TId : struct
        {
            throw new NotSupportedException();
        }

        public TEntity Update(TEntity entity)
        {
            Requires.NotNull(entity, "entity");

            _context.Entry(entity).State = EntityState.Modified;
            _context.SaveChanges();

            return entity;
        }

        public TEntity InsertOrUpdate(TEntity entity)
        {
            //todo: Implement
            throw new NotImplementedException();
        }
        
        public void Delete(TEntity entity)
        {
            Requires.NotNull(entity, "entity");

            _context.Entry(entity).State = EntityState.Deleted;            
            _context.SaveChanges();
        }

        public void Delete<TId>(TId id)
        {
            var entity = GetById(id);
            if (entity != null)
            {
                _context.Entry(entity).State = EntityState.Deleted;                
                _context.SaveChanges();
            }                        
        }        
    }
}
