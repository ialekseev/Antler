using System;

namespace SmartElk.Antler.Domain
{
    public class UnitOfWork: IDisposable
    {        
        private readonly ISessionScope _sessionScope;
                                        
        private static Func<string, ISessionScopeFactory> SessionScopeFactoryExtractor { get; set; }
        public static void SetSessionScopeFactoryExtractor(Func<string, ISessionScopeFactory> extractor)
        {
            SessionScopeFactoryExtractor = extractor;
        }

        public static void Do(string storageName, Action<UnitOfWork> action)
        {
            using (var uow = new UnitOfWork(storageName))
            {
                action(uow);
            }
        }
        
        public static void Do(Action<UnitOfWork> action)
        {
            Do(null, action);
        }

        public UnitOfWork(string storageName)
        {
            var sessionScopeFactory = SessionScopeFactoryExtractor(storageName);
            if (sessionScopeFactory == null)
                throw new Exception("You should set ISessionScopeFactory implementation before using UnitOfWork");

            _sessionScope = sessionScopeFactory.Open();            
        }
        
        public UnitOfWork():this(null)
        {                                    
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
