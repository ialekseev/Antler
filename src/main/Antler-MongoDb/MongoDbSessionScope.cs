using System;
using System.Collections.Generic;
using System.Linq;
using MongoDB.Driver;
using MongoDB.Driver.Builders;
using MongoDB.Driver.Linq;
using SmartElk.Antler.Core.Common.CodeContracts;
using SmartElk.Antler.Core.Common.Extensions;
using SmartElk.Antler.Core.Domain;
using SmartElk.Antler.MongoDb.Extensions;
using SmartElk.Antler.MongoDb.Internal;

namespace SmartElk.Antler.MongoDb
{
    public class MongoDbSessionScope : ISessionScope, ISessionScopeEx
    {
        private readonly MongoDatabase _session;
        private readonly Func<object, object> _idExtractor;
                        
        private readonly ISet<object> _newEntities = new HashSet<object>();
        private readonly ISet<object> _updatedEntities = new HashSet<object>();
        private readonly ISet<object> _deletedEntities = new HashSet<object>();  

        public MongoDbSessionScope(MongoDatabase session, Func<object, object> idExtractor)
        {
            Requires.NotNull(session, "session");
            Requires.NotNull(idExtractor, "idExtractor");

            _session = session;
            _idExtractor = idExtractor;           
        }

        public void Commit()
        {
            _newEntities.ForEach(t => Save(t));                        
            _newEntities.Clear();

            _updatedEntities.ForEach(t => Save(t));
            _updatedEntities.Clear();

            _deletedEntities.ForEach(Delete);
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
            return new MongoDbRepository<TEntity>(this);
        }
                
        public TInternal GetInternal<TInternal>() where TInternal  : class
        {                        
            var internalSession = _session as TInternal;
            Assumes.True(internalSession != null, "Can't cast Internal Session to TInternal type");
            return internalSession;
        }        
        
        public void Dispose()
        {
        }

        private bool Save(object entity)
        {
            Requires.NotNull(entity, "entity");

            var collection = _session.GetCollection(entity);
            var result = collection.Update(BuildRootQuery(_idExtractor(entity)), Update.Replace(entity), UpdateFlags.Upsert);
            return result.DocumentsAffected == 1;
        }

        private void Delete(object entity)
        {
            Requires.NotNull(entity, "entity");

            var collection = _session.GetCollection(entity);
            collection.Remove(BuildRootQuery(_idExtractor(entity)));
        }

        private static IMongoQuery BuildRootQuery(object id)
        {
            Requires.NotNull(id, "id");
            return Query.EQ("_id", id.AsIdValue());
        } 

        #region ISessionScopeEx
        IQueryable<TEntity> ISessionScopeEx.AsQueryable<TEntity>()
        {
            return _session.GetCollection<TEntity>().AsQueryable();
        }

        TEntity ISessionScopeEx.GetById<TId, TEntity>(TId id)
        {
            return _session.GetCollection<TEntity>().FindOneById(id.AsIdValue());
        }

        void ISessionScopeEx.MarkAsNew<TEntity>(TEntity entity)
        {
            Requires.NotNull(entity, "entity");            
            Mark(entity, _newEntities);                                                            
        }

        void ISessionScopeEx.MarkAsUpdated<TEntity>(TEntity entity)
        {
            Requires.NotNull(entity, "entity");
            Mark(entity, _updatedEntities);                                                            
        }

        void ISessionScopeEx.MarkAsDeleted<TEntity>(TEntity entity)
        {
            Requires.NotNull(entity, "entity");
            Mark(entity, _deletedEntities);                                                            
        }   
      
        private static void Mark<TEntity>(TEntity entity, ISet<object> set) where TEntity: class
        {
            Requires.NotNull(entity, "entity");
            Requires.NotNull(set, "set");
            Requires.True(entity is IEqualityComparer<TEntity>, "Entity should implement IEqualityComparer");
            
            set.Add(entity);
        }

        #endregion
    }
}
