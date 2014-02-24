using System;
using SmartElk.Antler.Core.Common;
using SmartElk.Antler.Core.Common.CodeContracts;

namespace SmartElk.Antler.Core.Domain
{        
    public class UnitOfWork: IDisposable
    {
        public ISessionScope SessionScope { get; private set; }

        [ThreadStatic]
        private static UnitOfWork _current;
        public static Option<UnitOfWork> Current
        {
            get { return _current; }
            private set { _current = value.Value; }
        }
                
        public bool IsFinished { get; private set; }                
        public Guid Id { get; private set; }
        
        public static Func<ISessionScopeFactory> SessionScopeFactoryExtractor { get; set; }        
        public static Func<string, ISessionScopeFactory> SessionScopeFactoryNamedExtractor { get; set; }
                        
        private UnitOfWork()            
        {            
            Assumes.True(SessionScopeFactoryExtractor != null, "SessionScopeFactoryExtractor should be set before using UnitOfWork. Wrong configuration?");
            var sessionScopeFactory = SessionScopeFactoryExtractor();            
            SetSession(sessionScopeFactory);
        }

        private UnitOfWork(string storageName)            
        {            
            Assumes.True(SessionScopeFactoryNamedExtractor != null, "SessionScopeFactoryNamedExtractor should be set before using UnitOfWork with storage name. Wrong configuraiton?");
            var sessionScopeFactory = SessionScopeFactoryNamedExtractor(storageName);
            SetSession(sessionScopeFactory);
        }

         private void SetSession(ISessionScopeFactory sessionScopeFactory)
         {
            Requires.NotNull(sessionScopeFactory, "Can't continue without SessionScopeFactory. Wrong configuration?");
            Assumes.True(Current.IsNone, "Nested UnitOfWorks are not supported");  
                      
            SessionScope = sessionScopeFactory.Open();
            Current = this;
            IsFinished = false;
            Id = Guid.NewGuid();
        }

        public static void Do(Action<UnitOfWork> work)
        {
            Requires.NotNull(work, "work");
            
            using (var uow = new UnitOfWork())
            {
                work(uow);
            }
        }

        public static TResult Do<TResult>(Func<UnitOfWork, TResult> work)
        {
            Requires.NotNull(work, "work");

            using (var uow = new UnitOfWork())
            {
                return work(uow);
            }
        }

        public static void Do(string storageName, Action<UnitOfWork> work)
        {
            Requires.NotNullOrEmpty(storageName, "storageName");
            Requires.NotNull(work, "work");

            using (var uow = new UnitOfWork(storageName))
            {
                work(uow);
            }
        }

        public static TResult Do<TResult>(string storageName, Func<UnitOfWork, TResult> work)
        {
            Requires.NotNullOrEmpty(storageName, "storageName");
            Requires.NotNull(work, "work");

            using (var uow = new UnitOfWork(storageName))
            {
                return work(uow);
            }
        }

        public void Dispose()
        {
            Commit();
        }
        
        public void Commit()
        {            
            Perform(() => SessionScope.Commit());
        }
        
        public void Rollback()
        {
            Perform(() => SessionScope.Rollback());            
        }

        private void Perform(Action action)
        {
            Requires.NotNull(action, "action");            
            if (!IsFinished)
            {
                action();
                SessionScope.Dispose();
                IsFinished = true;
                _current = null;
            }
        }

        public IRepository<TEntity> Repo<TEntity>() where TEntity: class
        {
            return SessionScope.CreateRepository<TEntity>();
        }                   
    }
}
