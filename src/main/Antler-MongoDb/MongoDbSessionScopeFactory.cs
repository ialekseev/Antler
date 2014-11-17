using System;
using MongoDB.Driver;
using SmartElk.Antler.Core.Common.CodeContracts;
using SmartElk.Antler.Core.Domain;

namespace SmartElk.Antler.MongoDb
{
    public class MongoDbSessionScopeFactory: ISessionScopeFactory
    {        
        private readonly string _idPropertyName;
        private readonly MongoDatabase _database;
        
        public MongoDbSessionScopeFactory(string connectionString, string databaseName, string idPropertyName, Action<MongoClient> applyOnClientConfiguration, Action<MongoServer> applyOnServerConfiguration)
        {
            Requires.NotNullOrEmpty(connectionString, "connectionString");
            Requires.NotNullOrEmpty(databaseName, "databaseName");
            Requires.NotNullOrEmpty(idPropertyName, "idPropertyName");
            Requires.NotNull(applyOnClientConfiguration, "applyOnClientConfiguration");
            Requires.NotNull(applyOnServerConfiguration, "applyOnServerConfiguration");
            
            _idPropertyName = idPropertyName;
            
            var client = new MongoClient(connectionString);
            applyOnClientConfiguration(client);

            var server = client.GetServer();
            applyOnServerConfiguration(server);

            _database = server.GetDatabase(databaseName);
        }

        public ISessionScope Open()
        {                                    
            return new MongoDbSessionScope(_database, _idPropertyName);
        }                
    }
}
