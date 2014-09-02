using MongoDB.Driver;
using SmartElk.Antler.Core.Common.CodeContracts;
using SmartElk.Antler.Core.Domain;

namespace SmartElk.Antler.MongoDb
{
    public class MongoDbSessionScopeFactory: ISessionScopeFactory
    {
        private readonly string _connectionString;
        private readonly string _databaseName;
        private readonly string _idPropertyName;

        public MongoDbSessionScopeFactory(string connectionString, string databaseName, string idPropertyName)
        {
            Requires.NotNullOrEmpty(connectionString, "connectionString");
            Requires.NotNullOrEmpty(databaseName, "databaseName");
            Requires.NotNullOrEmpty(idPropertyName, "idPropertyName");

            _connectionString = connectionString;
            _databaseName = databaseName;
            _idPropertyName = idPropertyName;
        }

        public ISessionScope Open()
        {            
            var client = new MongoClient(_connectionString);
            var server = client.GetServer();
            var database = server.GetDatabase(_databaseName);
            return new MongoDbSessionScope(database, _idPropertyName);
        }                   
    }
}
