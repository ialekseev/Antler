using System;

namespace SmartElk.Antler.Domain
{
    public class UnitOfWork: IDisposable
    {
        private readonly ISessionScope _sessionScope;        
        public static Action<ISessionScope> DoAfterSessionOpening { get; set; }
        public static ISessionScopeFactory SessionScopeFactory { get; set; }

        public UnitOfWork()
        {                                    
            _sessionScope = SessionScopeFactory.Open();

            if (DoAfterSessionOpening != null)
                DoAfterSessionOpening(_sessionScope);
        }
        
        public void Dispose()
        {
            Commit();
        }

        public void Commit()
        {
            _sessionScope.Commit();
        }

        public void Rollback()
        {
            _sessionScope.Rollback();
        }

        public IRepository<TEntity, TId> Repository<TEntity, TId>() where TEntity: class
        {
            return _sessionScope.Repository<TEntity, TId>();
        }

        public ISessionScope CurrentSession
        {
            get { return _sessionScope;}
        }
    }
}
