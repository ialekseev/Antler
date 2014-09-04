using System.Collections.Generic;
using MongoDB.Driver;
using SmartElk.Antler.Core.Common;
using SmartElk.Antler.Core.Common.CodeContracts;

namespace SmartElk.Antler.MongoDb.Configuration
{
    public class MongoDbIndexBuilder
    {
        private readonly List<MongoDbIndex> _indexes;
                
        private MongoDbIndexBuilder(MongoDbIndex index)
        {
            _indexes = new List<MongoDbIndex> {index};
        }
        
        public static MongoDbIndexBuilder Add<TEntity>(IMongoIndexKeys index, IMongoIndexOptions options = null)
        {
            Requires.NotNull(index, "index");
            
            return new MongoDbIndexBuilder(new MongoDbIndex(typeof(TEntity).Name, index, options.AsOption()));            
        }

        public MongoDbIndexBuilder ThenAdd<TEntity>(IMongoIndexKeys index, IMongoIndexOptions options = null)
        {
            Requires.NotNull(index, "index");

            _indexes.Add(new MongoDbIndex(typeof (TEntity).Name, index, options.AsOption()));
            return this;
        }
        
        public List<MongoDbIndex> Get()
        {
            return _indexes;
        }        
    }

    public class MongoDbIndex
    {
        public string CollectionName { get; private set; }
        public IMongoIndexKeys Index { get; private set; }
        public Option<IMongoIndexOptions> IndexOptions { get; private set; }

        public MongoDbIndex(string collectionName, IMongoIndexKeys index, Option<IMongoIndexOptions> options)
        {
            Requires.NotNull(collectionName, "collectionName");
            Requires.NotNull(index, "index");
            Requires.NotNull(options, "options");

            CollectionName = collectionName;
            Index = index;
            IndexOptions = options;
        }                
    }
}
