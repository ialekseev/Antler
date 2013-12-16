using SmartElk.Antler.Core.Domain;
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
                
        public IRepository<TEntity> CreateRepository<TEntity>() where TEntity:class
        {
            return new EntityFrameworkRepository<TEntity>(_dbContext);
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
