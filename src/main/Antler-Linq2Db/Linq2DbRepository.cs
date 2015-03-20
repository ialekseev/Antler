using System;
using System.Linq;
using LinqToDB.Data;
using SmartElk.Antler.Core.Common.CodeContracts;
using SmartElk.Antler.Core.Domain;
using LinqToDB;

namespace SmartElk.Antler.Linq2Db
{
    public class Linq2DbRepository<TEntity>: IRepository<TEntity> where TEntity: class
    {
        private readonly DataConnection _dataConnection;
        public Linq2DbRepository(DataConnection dataConnection)
        {
            Requires.NotNull(dataConnection, "dataConnection");
            _dataConnection = dataConnection;
        }

        public virtual IQueryable<TEntity> AsQueryable()
        {
            return _dataConnection.GetTable<TEntity>();            
        }

        public TEntity GetById<TId>(TId id)
        {
            throw new NotSupportedException("Use AsQueryable method for building GetById query");
        }
        
        public TEntity Insert(TEntity entity)
        {
            Requires.NotNull(entity, "entity");
            _dataConnection.Insert(entity);
            return entity;
        }

        public TId Insert<TId>(TEntity entity) where TId : struct
        {
            Requires.NotNull(entity, "entity");                                    
                        
            return (TId)_dataConnection.InsertWithIdentity(entity);
        }

        public TEntity Update(TEntity entity)
        {
            Requires.NotNull(entity, "entity");
            _dataConnection.Update(entity);
            return entity;
        }

        public TEntity InsertOrUpdate(TEntity entity)
        {            
            Requires.NotNull(entity, "entity");
            _dataConnection.InsertOrReplace(entity);
            return entity;
        }
        
        public void Delete(TEntity entity)
        {
            Requires.NotNull(entity, "entity");
            _dataConnection.Delete(entity);
        }

        public void Delete<TId>(TId id)
        {                        
            throw new NotSupportedException("Use overloaded method that accepts Entity as argument");            
        }        
    }
}
