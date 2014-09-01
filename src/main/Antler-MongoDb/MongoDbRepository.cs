using System;
using System.Linq;
using MongoDB.Driver;
using MongoDB.Driver.Builders;
using MongoDB.Driver.Linq;
using SmartElk.Antler.Core.Common.CodeContracts;
using SmartElk.Antler.Core.Domain;
using SmartElk.Antler.MongoDb.Extensions;

namespace SmartElk.Antler.MongoDb
{    
    public class MongoDbRepository
    {
        protected MongoDatabase Session { get; private set; }
        protected Func<object, object> IdExtractor { get; private set; }

        public MongoDbRepository(MongoDatabase session, Func<object, object> idExtractor)
        {
            Requires.NotNull(session, "session");
            Requires.NotNull(idExtractor, "idExtractor");

            Session = session;
            IdExtractor = idExtractor;
        }

        public bool Save(object entity)
        {
            Requires.NotNull(entity, "entity");

            var collection = Session.GetCollection(entity);
            var result = collection.Update(BuildRootQuery(IdExtractor(entity)), Update.Replace(entity), UpdateFlags.Upsert);
            return result.DocumentsAffected == 1;
        }

        public void Delete(object entity)
        {
            Requires.NotNull(entity, "entity");

            var collection = Session.GetCollection(entity);
            collection.Remove(BuildRootQuery(IdExtractor(entity)));
        } 

        protected static IMongoQuery BuildRootQuery(object id)
        {
            Requires.NotNull(id, "id");

            return Query.EQ("_id", id.AsIdValue());
        }
    }

    public class MongoDbRepository<TEntity> : MongoDbRepository, IRepository<TEntity> where TEntity : class
    {        
        public MongoDbRepository(MongoDatabase session, Func<object, object> idExtractor): base(session, idExtractor)
        {            
        }

        public IQueryable<TEntity> AsQueryable()
        {
            return Session.GetCollection<TEntity>().AsQueryable();
        }

        public TEntity GetById<TId>(TId id)
        {
            return Session.GetCollection<TEntity>().FindOneById(id.AsIdValue());
        }

        public TEntity Insert(TEntity entity)
        {
            throw new NotSupportedException();            
        }

        public TId Insert<TId>(TEntity entity)
        {
            throw new NotSupportedException();            
        }

        public TEntity Update(TEntity entity)
        {
            throw new NotSupportedException();            
        }

        public void Delete(TEntity entity)
        {
            throw new NotSupportedException();            
        }

        public void Delete<TId>(TId id)
        {
            throw new NotSupportedException();            
        }                  
    }
}
