using System;
using SmartElk.Antler.Domain;
using SmartElk.Antler.EntityFramework.Internal;

namespace SmartElk.Antler.EntityFramework
{
    public class EntityFrameworkSessionScope: ISessionScope
    {
        private readonly IDataContext _dbContext;        
        
        public EntityFrameworkSessionScope(IDataContextFactory dbContextFactory)
        {
            _dbContext = dbContextFactory.CreateDbContext();
        }

        public void Commit()
        {
            _dbContext.SaveChanges();
        }

        public void Rollback()
        {
            throw new NotImplementedException();
        }
        
        public IRepository<TEntity, TId> Repository<TEntity, TId>() where TEntity:class
        {
            return new EntityFrameworkRepository<TEntity, TId>(_dbContext);
        }

        public object InternalSession
        {
            get { return _dbContext; }
        }

        public void Dispose()
        {     
            _dbContext.Dispose();
        }
    }
}
