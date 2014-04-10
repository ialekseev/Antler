using System;
using System.Runtime.InteropServices;
using SmartElk.Antler.Core.Common;
using SmartElk.Antler.Core.Common.CodeContracts;

namespace SmartElk.Antler.Core.Domain
{           
    //todo: more tests(handling errors in root/nested unit of works)
    public class UnitOfWork: IDisposable
    {
        public ISessionScope SessionScope { get; private set; }

        [ThreadStatic]
        private static UnitOfWork _current;
        public static Option<UnitOfWork> Current
        {
            get { return _current.AsOption(); }            
        }

        public bool IsFinished
        {
            get { return _current == null; }
        }
        
        public bool IsRoot { get; private set; }
                
        public Guid Id { get; private set; }
        public UnitOfWorkSettings Settings { get; private set; }

        public static Func<ISessionScopeFactory> SessionScopeFactoryExtractor { get; set; }        
        public static Func<string, ISessionScopeFactory> SessionScopeFactoryNamedExtractor { get; set; }

        private UnitOfWork(UnitOfWorkSettings settings)            
        {
            SetSettings(settings);
            Assumes.True(SessionScopeFactoryExtractor != null, "SessionScopeFactoryExtractor should be set before using UnitOfWork. Wrong configuration?");
            var sessionScopeFactory = SessionScopeFactoryExtractor();            
            SetSession(sessionScopeFactory);
        }

        private UnitOfWork(string storageName, UnitOfWorkSettings settings)
        {
            SetSettings(settings);
            Assumes.True(SessionScopeFactoryNamedExtractor != null, "SessionScopeFactoryNamedExtractor should be set before using UnitOfWork with storage name. Wrong configuraiton?");
            var sessionScopeFactory = SessionScopeFactoryNamedExtractor(storageName);
            SetSession(sessionScopeFactory);
        }

         private void SetSettings(UnitOfWorkSettings settings)
         {
             Settings = settings ?? UnitOfWorkSettings.Default;
         }

        //todo: more unit & integration specs about nested UnitOfWork 
        private void SetSession(ISessionScopeFactory sessionScopeFactory)
         {
            Requires.NotNull(sessionScopeFactory, "Can't continue without SessionScopeFactory. Wrong configuration?");
            
             if (_current == null)
             {
                 SessionScope = sessionScopeFactory.Open();
                 IsRoot = true;                 
             }
             else
             {
                 SessionScope = _current.SessionScope;
                 IsRoot = false;
             }
                                                  
            _current = this;            
            Id = Guid.NewGuid();            
         }

        public static void Do(Action<UnitOfWork> work, UnitOfWorkSettings settings = null)
        {
            Requires.NotNull(work, "work");
            
            using (var uow = new UnitOfWork(settings))
            {
                work(uow);
            }
        }

        public static TResult Do<TResult>(Func<UnitOfWork, TResult> work, UnitOfWorkSettings settings = null)
        {
            Requires.NotNull(work, "work");

            using (var uow = new UnitOfWork(settings))
            {
                return work(uow);
            }
        }

        public static void Do(string storageName, Action<UnitOfWork> work, UnitOfWorkSettings settings = null)
        {
            Requires.NotNullOrEmpty(storageName, "storageName");
            Requires.NotNull(work, "work");

            using (var uow = new UnitOfWork(storageName, settings))
            {
                work(uow);
            }
        }

        public static TResult Do<TResult>(string storageName, Func<UnitOfWork, TResult> work, UnitOfWorkSettings settings = null)
        {
            Requires.NotNullOrEmpty(storageName, "storageName");
            Requires.NotNull(work, "work");

            using (var uow = new UnitOfWork(storageName, settings))
            {
                return work(uow);
            }
        }
        
        public void Dispose()        
        {
            if (Marshal.GetExceptionCode() == 0)
            {
                if (Settings.RollbackOnDispose)
                    Rollback();
                else
                    Commit();
            }
            else
            {
                if (IsRoot && !IsFinished)
                  CloseUnitOfWork();
            }
        }
        
        public void Commit()
        {                        
                Perform(() =>
                    {
                        if (Settings.EnableCommit) 
                            SessionScope.Commit();
                    });
        }
        
        public void Rollback()
        {
            Perform(() => SessionScope.Rollback());            
        }

        private void Perform(Action action)
        {
            Requires.NotNull(action, "action");
            if (IsRoot && !IsFinished)
            {
                try
                {
                    action();
                }
                finally 
                {
                    CloseUnitOfWork();
                }                                
            }
        }

        private void CloseUnitOfWork()
        {            
            SessionScope.Dispose();
            _current = null;                           
        }

        public IRepository<TEntity> Repo<TEntity>() where TEntity: class
        {
            return SessionScope.CreateRepository<TEntity>();
        }                   
    }
}
