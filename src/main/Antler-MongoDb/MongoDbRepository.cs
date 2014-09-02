using System;
using System.Linq;
using SmartElk.Antler.Core.Domain;
using SmartElk.Antler.MongoDb.Internal;

namespace SmartElk.Antler.MongoDb
{        
    public class MongoDbRepository<TEntity> :IRepository<TEntity> where TEntity : class
    {        
        private readonly ISessionScopeEx _sessionScope;

        public MongoDbRepository(ISessionScopeEx sessionScope)
        {            
            _sessionScope = sessionScope;            
        }

        public IQueryable<TEntity> AsQueryable()
        {
            return _sessionScope.AsQueryable<TEntity>();            
        }

        public TEntity GetById<TId>(TId id)
        {
            return _sessionScope.GetById<TId, TEntity>(id);            
        }

        public TEntity Insert(TEntity entity)
        {
            _sessionScope.MarkAsNew(entity);
            return entity;
        }

        public TId Insert<TId>(TEntity entity)
        {
            throw new NotSupportedException();            
        }

        public TEntity Update(TEntity entity)
        {
            _sessionScope.MarkAsUpdated(entity);
            return entity;            
        }

        public void Delete(TEntity entity)
        {
            _sessionScope.MarkAsDeleted(entity);
        }

        public void Delete<TId>(TId id)
        {
            throw new NotSupportedException();            
        }         
    }
}
