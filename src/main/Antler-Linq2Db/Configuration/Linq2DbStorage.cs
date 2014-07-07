using SmartElk.Antler.Core.Common.CodeContracts;
using SmartElk.Antler.Core.Domain;
using SmartElk.Antler.Core.Domain.Configuration;

namespace SmartElk.Antler.Linq2Db.Configuration
{
    public class Linq2DbStorage : IStorage
    {
        private readonly string _databaseName;

        private Linq2DbStorage(string databaseName)
        {
            _databaseName = databaseName;
        }
        
        public static Linq2DbStorage Use(string databaseName)
        {
            Requires.NotNullOrEmpty(databaseName, "databaseName");
            return new Linq2DbStorage(databaseName);
        }
        
        public void Configure(IDomainConfigurator configurator)
        {
            Requires.NotNull(configurator, "configurator");
            
            var sessionScopeFactory = new Linq2DbSessionScopeFactory(_databaseName);
            configurator.Configuration.Container.PutWithNameOrDefault<ISessionScopeFactory>(sessionScopeFactory, configurator.Name);
        }
    }
}
