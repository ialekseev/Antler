using System;
using System.Linq;
using SmartElk.Antler.Core.Common.CodeContracts;
using SmartElk.Antler.Core.Domain;
using LinqToDB;

namespace SmartElk.Antler.Linq2Db
{
    public class Linq2DbRepository<TEntity>: IRepository<TEntity> where TEntity: class
    {
        private readonly LinqToDB.Data.DataConnection _dataConnection;
        public Linq2DbRepository(LinqToDB.Data.DataConnection dataConnection)
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
            throw new NotSupportedException();
        }
        
        public TEntity Insert(TEntity entity)
        {
            Requires.NotNull(entity, "entity");
            _dataConnection.Insert(entity);    //todo: use InsertWithIdentity ???                    
            return entity;
        }

        public TEntity Update(TEntity entity)
        {
            Requires.NotNull(entity, "entity");
            _dataConnection.Update(entity);
            return entity;
        }

        public void Delete(TEntity entity)
        {
            Requires.NotNull(entity, "entity");
            _dataConnection.Delete(entity);
        }

        public void Delete<TId>(TId id)
        {
            throw new NotSupportedException();            
        }        
    }
}
