using System;

namespace SmartElk.Antler.Domain
{
    public class UnitOfWork: IDisposable
    {        
        private readonly ISessionScope _sessionScope;
                                        
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
        }
        
        public void Dispose()
        {
            _sessionScope.Commit();
            _sessionScope.Dispose();
        }
                
        public IRepository<TEntity> Repository<TEntity>() where TEntity: class
        {
            return _sessionScope.CreateRepository<TEntity>();
        }

        public ISessionScope CurrentSession
        {
            get { return _sessionScope;}
        }
    }
}
