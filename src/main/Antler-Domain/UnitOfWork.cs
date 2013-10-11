using System;

namespace SmartElk.Antler.Domain
{
    public class UnitOfWork: IDisposable
    {
        private readonly ISessionScope _sessionScope;        
        public static Action<ISessionScope> DoAfterSessionOpening { get; set; }
        public static Func<ISessionScopeFactory> SessionScopeFactoryExtractor { get; set; }

        public UnitOfWork()
        {
            var sessionScopeFactory = SessionScopeFactoryExtractor();
            if (sessionScopeFactory == null)
                throw new Exception("You should set ISessionScopeFactory implementation before using UnitOfWork");
            
            _sessionScope = sessionScopeFactory.Open();

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
