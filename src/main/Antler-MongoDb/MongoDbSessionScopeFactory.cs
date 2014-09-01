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
        private readonly Func<object, object> _idExtractor;

        public MongoDbSessionScopeFactory(string connectionString, string databaseName, Func<object, object> idExtractor)
        {
            Requires.NotNullOrEmpty(connectionString, "connectionString");
            Requires.NotNullOrEmpty(databaseName, "databaseName");
            Requires.NotNull(idExtractor, "idExtractor");

            _connectionString = connectionString;
            _databaseName = databaseName;
            _idExtractor = idExtractor;
        }

        public ISessionScope Open()
        {            
            var client = new MongoClient(_connectionString);
            var server = client.GetServer();
            var database = server.GetDatabase(_databaseName);
            return new MongoDbSessionScope(database, _idExtractor);
        }                   
    }
}
