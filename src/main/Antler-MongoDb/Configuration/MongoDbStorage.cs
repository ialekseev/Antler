using System;
using System.Linq;
using MongoDB.Driver;
using SmartElk.Antler.Core.Common;
using SmartElk.Antler.Core.Common.CodeContracts;
using SmartElk.Antler.Core.Domain;
using SmartElk.Antler.Core.Domain.Configuration;

namespace SmartElk.Antler.MongoDb.Configuration
{
    public class MongoDbStorage : AbstractStorage
    {
        private readonly string _connectionString;
        private readonly string _databaseName;
        private string _idPropertyName;
        private bool _recreateDatabase;        
        private Action<MongoClient> _applyOnClientConfiguration;
        private Action<MongoServer> _applyOnServerConfiguration;
        private Option<MongoDbIndexBuilder> _indexBuilder;

        private MongoDbStorage(string connectionString, string databaseName)
        {
            _connectionString = connectionString;            
            _databaseName = databaseName;
            _idPropertyName = "Id";
            _recreateDatabase = false;
            _applyOnClientConfiguration = client => { };
            _applyOnServerConfiguration = server => { };
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

        public MongoDbStorage WithRecreatedDatabase(bool really)
        {
            if (really)
             _recreateDatabase = true;
            return this;
        }

        public MongoDbStorage WithEnsuredIndexes(MongoDbIndexBuilder indexBuilder)
        {
            Requires.NotNull(indexBuilder, "indexBuilder");
            
            _indexBuilder = indexBuilder;
            return this;
        }

        public MongoDbStorage ApplyOnClientConfiguration(Action<MongoClient> applyOnClientConfiguration)
        {
            Requires.NotNull(applyOnClientConfiguration, "applyOnClientConfiguration");

            _applyOnClientConfiguration = applyOnClientConfiguration;
            return this;
        }
        
        public MongoDbStorage ApplyOnServerConfiguration(Action<MongoServer> applyOnServerConfiguration)
        {
            Requires.NotNull(applyOnServerConfiguration, "applyOnServerConfiguration");

            _applyOnServerConfiguration = applyOnServerConfiguration;
            return this;
        }

        protected override ISessionScopeFactory ConfigureInternal(IDomainConfigurator configurator)
        {
            Requires.NotNull(configurator, "configurator");
            
            if (_recreateDatabase)
            {
                DropDatabase();
            }

            if (_indexBuilder.IsSome)
            {
                EnsureIndexes();
            }
            
           return new MongoDbSessionScopeFactory(_connectionString, _databaseName, _idPropertyName, _applyOnClientConfiguration, _applyOnServerConfiguration);            
        }

        private void DropDatabase()
        {            
            var server = new MongoClient(_connectionString).GetServer();
            server.DropDatabase(_databaseName);            
        }

        private void EnsureIndexes()
        {
            var database = new MongoClient(_connectionString).GetServer().GetDatabase(_databaseName);
            var collectionNames = database.GetCollectionNames();

            foreach (var collectionName in collectionNames)
            {
                var collection = database.GetCollection(collectionName);                
                var indexesForCollection = _indexBuilder.Value.Get().Where(t => t.CollectionName.Equals(collectionName)).ToList();
                
                foreach (var mongoDbIndex in indexesForCollection)
                {
                    if (mongoDbIndex.IndexOptions.IsSome)                    
                        collection.CreateIndex(mongoDbIndex.Index, mongoDbIndex.IndexOptions.Value);
                    else
                        collection.CreateIndex(mongoDbIndex.Index);
                }
            }
        }
    }
}
