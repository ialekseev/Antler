using System;
using System.Linq;
using SmartElk.Antler.Core.Common;
using SmartElk.Antler.Core.Common.CodeContracts;
using SmartElk.Antler.Core.Common.Extensions;
using SmartElk.Antler.Core.Common.Reflection;
using SmartElk.Antler.Core.Domain;
using SmartElk.Antler.MongoDb.Internal;

namespace SmartElk.Antler.MongoDb
{        
    public class MongoDbRepository<TEntity> :IRepository<TEntity> where TEntity : class
    {        
        private readonly ISessionScopeEx _sessionScope;
        private readonly string _idPropertyName;
        private readonly Option<Func<object>> _identityGenerator; 

        public MongoDbRepository(ISessionScopeEx sessionScope, string idPropertyName, Option<Func<object>> identityGenerator)
        {            
            Requires.NotNull(sessionScope, "sessionScope");
            Requires.NotNull(idPropertyName, "idPropertyName");
            Requires.NotNull(identityGenerator, "identityGenerator");

            _sessionScope = sessionScope;
            _idPropertyName = idPropertyName;
            _identityGenerator = identityGenerator;
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
            Requires.NotNull(entity, "entity");
            Requires.True(entity.HasProperty(_idPropertyName));

            ApplyGeneratedIdentity(entity);
            _sessionScope.MarkAsNew(entity);
            return entity;
        }

        public TId Insert<TId>(TEntity entity)
        {
            throw new NotSupportedException();            
        }

        public TEntity Update(TEntity entity)
        {
            Requires.NotNull(entity, "entity");
            
            _sessionScope.MarkAsUpdated(entity);
            return entity;            
        }

        public void Delete(TEntity entity)
        {
            Requires.NotNull(entity, "entity");
            
            _sessionScope.MarkAsDeleted(entity);
        }

        public void Delete<TId>(TId id)
        {
            var entity = _sessionScope.GetById<TId, TEntity>(id);
            _sessionScope.MarkAsDeleted(entity);
        }

        private void ApplyGeneratedIdentity(TEntity entity)
        {
            Requires.NotNull(entity, "entity");
            Requires.True(entity.HasProperty(_idPropertyName));

            if (_identityGenerator.IsNone)
                return;
            
            var currentPropertyValue = entity.GetPropertyValue(_idPropertyName);
            if (!currentPropertyValue.Equals(currentPropertyValue.GetType().GetDefaultValue()))
                return;

            var generatedValue = _identityGenerator.Value();
            
            Assumes.True(currentPropertyValue.GetType() == generatedValue.GetType(), "Type of a generated identity does not correspond to Entity's Id field type");

            entity.SetPropertyValue(_idPropertyName, generatedValue);            
        }
    }
}
