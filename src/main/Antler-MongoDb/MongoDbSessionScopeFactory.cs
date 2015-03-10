using System;
using MongoDB.Driver;
using SmartElk.Antler.Core.Common;
using SmartElk.Antler.Core.Common.CodeContracts;
using SmartElk.Antler.Core.Domain;

namespace SmartElk.Antler.MongoDb
{
    public class MongoDbSessionScopeFactory: ISessionScopeFactory
    {        
        private readonly string _idPropertyName;
        private readonly MongoDatabase _database;
        private readonly Option<Func<object>> _identityGenerator;

        public MongoDbSessionScopeFactory(string connectionString, string databaseName, string idPropertyName, Action<MongoClient> applyOnClientConfiguration, Action<MongoServer> applyOnServerConfiguration, Option<Func<object>> identityGenerator)
        {
            Requires.NotNullOrEmpty(connectionString, "connectionString");
            Requires.NotNullOrEmpty(databaseName, "databaseName");
            Requires.NotNullOrEmpty(idPropertyName, "idPropertyName");
            Requires.NotNull(applyOnClientConfiguration, "applyOnClientConfiguration");
            Requires.NotNull(applyOnServerConfiguration, "applyOnServerConfiguration");
            Requires.NotNull(identityGenerator, "identityGenerator");

            _idPropertyName = idPropertyName;
            _identityGenerator = identityGenerator;

            var client = new MongoClient(connectionString);
            applyOnClientConfiguration(client);

            var server = client.GetServer();
            applyOnServerConfiguration(server);

            _database = server.GetDatabase(databaseName);
        }

        public ISessionScope Open()
        {                                    
            return new MongoDbSessionScope(_database, _idPropertyName, _identityGenerator);
        }                
    }
}
