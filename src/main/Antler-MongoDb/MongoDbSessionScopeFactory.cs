using System;
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
        private readonly Action<MongoClient> _applyOnClientConfiguration;
        private readonly Action<MongoServer> _applyOnServerConfiguration;

        public MongoDbSessionScopeFactory(string connectionString, string databaseName, string idPropertyName, Action<MongoClient> applyOnClientConfiguration, Action<MongoServer> applyOnServerConfiguration)
        {
            Requires.NotNullOrEmpty(connectionString, "connectionString");
            Requires.NotNullOrEmpty(databaseName, "databaseName");
            Requires.NotNullOrEmpty(idPropertyName, "idPropertyName");
            Requires.NotNull(applyOnClientConfiguration, "applyOnClientConfiguration");
            Requires.NotNull(applyOnServerConfiguration, "applyOnServerConfiguration");

            _connectionString = connectionString;
            _databaseName = databaseName;
            _idPropertyName = idPropertyName;
            _applyOnClientConfiguration = applyOnClientConfiguration;
            _applyOnServerConfiguration = applyOnServerConfiguration;
        }

        public ISessionScope Open()
        {            
            var client = new MongoClient(_connectionString);
            _applyOnClientConfiguration(client);
            
            var server = client.GetServer();            
            _applyOnServerConfiguration(server);
            
            return new MongoDbSessionScope(server.GetDatabase(_databaseName), _idPropertyName);
        }                
    }
}
