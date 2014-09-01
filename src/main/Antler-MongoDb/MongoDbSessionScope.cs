using System;
using System.Collections.Generic;
using MongoDB.Driver;
using SmartElk.Antler.Core.Common.CodeContracts;
using SmartElk.Antler.Core.Common.Extensions;
using SmartElk.Antler.Core.Domain;

namespace SmartElk.Antler.MongoDb
{
    public class MongoDbSessionScope: ISessionScope
    {
        private readonly MongoDatabase _session;
        private readonly Func<object, object> _idExtractor;
        private readonly MongoDbRepository _repository;
                
        private readonly ISet<object> _newEntities = new HashSet<object>();
        private readonly ISet<object> _updatedEntities = new HashSet<object>();
        private readonly ISet<object> _deletedEntities = new HashSet<object>();  

        public MongoDbSessionScope(MongoDatabase session, Func<object, object> idExtractor)
        {
            _session = session;
            _idExtractor = idExtractor;
            _repository = new MongoDbRepository(session, idExtractor);
        }

        public void Commit()
        {
            _newEntities.ForEach(t => _repository.Save(t));                        
            _newEntities.Clear();

            _updatedEntities.ForEach(t => _repository.Save(t));
            _updatedEntities.Clear();

            _deletedEntities.ForEach(t => _repository.Delete(t));
            _deletedEntities.Clear();
        }

        public void Rollback()
        {
            _newEntities.Clear();
            _updatedEntities.Clear();
            _deletedEntities.Clear();
        }

        public IRepository<TEntity> CreateRepository<TEntity>() where TEntity : class
        {
            return new MongoDbRepository<TEntity>(_session, _idExtractor);
        }

        public TInternal GetInternal<TInternal>() where TInternal : class
        {                        
            var internalSession = _session as TInternal;
            Assumes.True(internalSession != null, "Can't cast Internal Session to TInternal type");
            return internalSession;
        }
              
        public void MarkAsNew(object entity)
        {
            Requires.NotNull(entity, "entity");
            _newEntities.Add(entity);
        }

        public void MarkAsUpdated(object entity)
        {
            Requires.NotNull(entity, "entity");
            _updatedEntities.Add(entity);
        }

        public void MarkAsDeleted(object entity)
        {
            Requires.NotNull(entity, "entity");
            _deletedEntities.Add(entity);
        }

        public void Dispose()
        {
        }  
    }
}
