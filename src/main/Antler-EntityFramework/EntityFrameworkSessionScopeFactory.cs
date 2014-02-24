using SmartElk.Antler.Core.Common.CodeContracts;
using SmartElk.Antler.Core.Domain;
using SmartElk.Antler.EntityFramework.Internal;

namespace SmartElk.Antler.EntityFramework
{
    public class EntityFrameworkSessionScopeFactory: ISessionScopeFactory
    {
        private readonly IDataContextFactory _dbContextFactory;
        public EntityFrameworkSessionScopeFactory(IDataContextFactory dbContextFactory)
        {
            Requires.NotNull(dbContextFactory, "dbContextFactory");
            _dbContextFactory = dbContextFactory;
        }

        public ISessionScope Open()
        {            
            return new EntityFrameworkSessionScope(_dbContextFactory);
        }   
        
        public DataContext CreateContext()
        {
            return _dbContextFactory.CreateDataContext();
        }
    }
}
