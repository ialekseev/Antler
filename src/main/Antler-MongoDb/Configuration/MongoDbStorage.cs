using SmartElk.Antler.Core.Common.CodeContracts;
using SmartElk.Antler.Core.Domain;
using SmartElk.Antler.Core.Domain.Configuration;

namespace SmartElk.Antler.MongoDb.Configuration
{
    public class MongoDbStorage : IStorage
    {
        private readonly string _connectionString;
        private readonly string _databaseName;
        private string _idPropertyName;

        private MongoDbStorage(string connectionString, string databaseName)
        {
            _connectionString = connectionString;
            _databaseName = databaseName;
            _idPropertyName = "Id";
        }

        public static MongoDbStorage Use(string connectionString, string databaseName)
        {
            Requires.NotNullOrEmpty(connectionString, "connectionString");
            Requires.NotNullOrEmpty(databaseName, "databaseName");
            
            return new MongoDbStorage(connectionString, databaseName);
        }
        
        public MongoDbStorage WithIdPropertyName(string idPropertyName)
        {
            Requires.NotNullOrEmpty(idPropertyName, "idPropertyName");

            _idPropertyName = idPropertyName;
            return this;
        }

        public void Configure(IDomainConfigurator configurator)
        {
            Requires.NotNull(configurator, "configurator");
            
            var sessionScopeFactory = new MongoDbSessionScopeFactory(_connectionString, _databaseName, _idPropertyName);
            configurator.Configuration.Container.PutWithNameOrDefault<ISessionScopeFactory>(sessionScopeFactory, configurator.Name);
        }
    }
}
