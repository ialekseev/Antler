using System;
using LinqToDB.Data;
using SmartElk.Antler.Core.Common.CodeContracts;
using SmartElk.Antler.Core.Domain;

namespace SmartElk.Antler.Linq2Db
{
    public class Linq2DbSessionScope: ISessionScope
    {
        private readonly DataConnection _dataConnection;

        public Linq2DbSessionScope(DataConnection dataConnection)
        {
            Requires.NotNull(dataConnection, "dataConnection");

            _dataConnection = dataConnection;
            dataConnection.BeginTransaction();            
        }

        public void Commit()
        {            
            try
            {                
                _dataConnection.CommitTransaction();                                
            }
            catch (Exception)
            {
                _dataConnection.RollbackTransaction();
                throw;
            }
        }
                
        public void Rollback()
        {
            _dataConnection.RollbackTransaction();
        }
        
        public IRepository<TEntity> CreateRepository<TEntity>() where TEntity:class
        {
            return new Linq2DbRepository<TEntity>(_dataConnection);
        }

        public TInternal GetInternal<TInternal>() where TInternal:class
        {
            var internalSession = _dataConnection as TInternal;
            Assumes.True(internalSession != null, "Can't cast Internal Session to TInternal type");
            return internalSession;
        }

        public void Dispose()
        {
            _dataConnection.Dispose();            
        }          
    }
}
