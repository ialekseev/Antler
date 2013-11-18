using System;
using SmartElk.Antler.Common.CodeContracts;

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

        private static Func<string, ISessionScopeFactory> SessionScopeFactoryNamedExtractor { get; set; }
        public static void SetSessionScopeFactoryNamedExtractor(Func<string, ISessionScopeFactory> extractor)
        {
            SessionScopeFactoryNamedExtractor = extractor;
        }
        
        private UnitOfWork()            
        {            
            Assumes.True(SessionScopeFactoryExtractor != null, "SessionScopeFactoryExtractor should be set before using UnitOfWork. Wrong configuration?");
            var sessionScopeFactory = SessionScopeFactoryExtractor();
            Assumes.True(sessionScopeFactory !=null, "Can't continue without SessionScopeFactory. Wrong configuration?");
            _sessionScope = sessionScopeFactory.Open();            
        }

        private UnitOfWork(string storageName)            
        {
            Requires.NotNullOrEmpty(storageName, "Storage name can't be null or empty");
            Assumes.True(SessionScopeFactoryNamedExtractor != null, "SessionScopeFactoryNamedExtractor should be set before using UnitOfWork with storage name. Wrong configuraiton?");
            var sessionScopeFactory = SessionScopeFactoryNamedExtractor(storageName);
            Assumes.True(sessionScopeFactory != null, "Can't continue without SessionScopeFactory. Wrong configuration?");
            _sessionScope = sessionScopeFactory.Open();                                    
        }

        public static void Do(Action<UnitOfWork> action)
        {
            using (var uow = new UnitOfWork())
            {
                action(uow);
            }
        }
        
        public static void Do(string storageName, Action<UnitOfWork> action)
        {
            using (var uow = new UnitOfWork(storageName))
            {
                action(uow);
            }
        }
        
        public void Dispose()
        {
            _sessionScope.Commit();
            _sessionScope.Dispose();
        }
                
        public IRepository<TEntity> Repo<TEntity>() where TEntity: class
        {
            return _sessionScope.CreateRepository<TEntity>();
        }

        public ISessionScope CurrentSession
        {
            get { return _sessionScope;}
        }
    }
}
