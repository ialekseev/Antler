using SmartElk.Antler.Core.Common.CodeContracts;
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

        public TInternal GetInternal<TInternal>() where TInternal:class
        {
            var internalSession = _dbContext as TInternal;
            Assumes.True(internalSession != null, "Can't cast Internal Session to TInternal type");
            return internalSession;
        }

        public void Dispose()
        {     
            _dbContext.Dispose();
        }        
    }
}
