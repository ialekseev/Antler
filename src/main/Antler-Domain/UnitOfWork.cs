using System;

namespace SmartElk.Antler.Domain
{
    public class UnitOfWork: IDisposable
    {
        private readonly ISessionScope _sessionScope;        
        
        private static Action<ISessionScope> DoAfterSessionOpen { get; set; }
        public static void SetActionToDoAfterSessionOpen(Action<ISessionScope> doAfterSessionOpen)
        {
            DoAfterSessionOpen = doAfterSessionOpen;
        }
        
        private static Func<ISessionScopeFactory> SessionScopeFactoryExtractor { get; set; }
        public static void SetSessionScopeFactoryExtractor(Func<ISessionScopeFactory> extractor)
        {
            SessionScopeFactoryExtractor = extractor;
        }

        public UnitOfWork()
        {
            var sessionScopeFactory = SessionScopeFactoryExtractor();
            if (sessionScopeFactory == null)
                throw new Exception("You should set ISessionScopeFactory implementation before using UnitOfWork");
            
            _sessionScope = sessionScopeFactory.Open();

            if (DoAfterSessionOpen != null)
                DoAfterSessionOpen(_sessionScope);
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
