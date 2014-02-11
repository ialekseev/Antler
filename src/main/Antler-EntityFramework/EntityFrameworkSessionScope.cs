using System;
using System.Data.Entity;
using SmartElk.Antler.Core.Common.CodeContracts;
using SmartElk.Antler.Core.Domain;
using SmartElk.Antler.EntityFramework.Internal;

namespace SmartElk.Antler.EntityFramework
{
    public class EntityFrameworkSessionScope: ISessionScope
    {
        private readonly DataContext _session;
        private readonly DbContextTransaction _transaction;
                
        public EntityFrameworkSessionScope(IDataContextFactory dbContextFactory)
        {
            Requires.NotNull(dbContextFactory, "dbContextFactory");
            _session = dbContextFactory.CreateDataContext();
            _transaction = _session.BeginTransaction();
        }

        public void Commit()
        {            
            try
            {
                _session.SaveChanges();
                _transaction.Commit();
            }
            catch (Exception)
            {
                _transaction.Rollback();
                throw;
            }
        }
                
        public void Rollback()
        {                        
            _transaction.Rollback();            
        }
        
        public IRepository<TEntity> CreateRepository<TEntity>() where TEntity:class
        {
            return new EntityFrameworkRepository<TEntity>(_session);
        }

        public TInternal GetInternal<TInternal>() where TInternal:class
        {
            var internalSession = _session as TInternal;
            Assumes.True(internalSession != null, "Can't cast Internal Session to TInternal type");
            return internalSession;
        }

        public void Dispose()
        {     
            _transaction.Dispose();
            _session.Dispose();
        }          
    }
}
