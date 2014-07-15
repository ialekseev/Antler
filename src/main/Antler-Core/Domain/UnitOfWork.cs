using System;
using System.Runtime.InteropServices;
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
            get { return _current.AsOption(); }            
        }

        public bool IsFinished
        {
            get { return _current == null; }
        }
        
        public bool IsRoot { get; private set; }
                
        public Guid Id { get; private set; }
        public UnitOfWorkSettings Settings { get; private set; }
        public string StorageName { get; private set; }

        public static Func<string, ISessionScopeFactory> SessionScopeFactoryExtractor { get; set; }
               
        private UnitOfWork(UnitOfWorkSettings settings)
        {
            Settings = settings ?? UnitOfWorkSettings.Default;

            Assumes.True(SessionScopeFactoryExtractor != null, "SessionScopeFactoryExtractor should be set before using UnitOfWork. Wrong configuraiton?");
            Assumes.True(!string.IsNullOrEmpty(Settings.StorageName), "Storage name can't be null or empty. Wrong configuration?");            
            var sessionScopeFactory = SessionScopeFactoryExtractor(Settings.StorageName);
            
            Assumes.True(sessionScopeFactory != null, "Can't find storage with name {0}. Wrong storage name?", Settings.StorageName);
            SetSession(sessionScopeFactory);
        }
                
        private void SetSession(ISessionScopeFactory sessionScopeFactory)
         {
            Requires.NotNull(sessionScopeFactory, "sessionScopeFactory");
            
             if (_current == null)
             {
                 SessionScope = sessionScopeFactory.Open();
                 IsRoot = true;                 
             }
             else
             {
                 if (Settings.ThrowIfNestedUnitOfWork)
                     throw new NotSupportedException("Nested UnitOfWorks are not supported due to UnitOfWorkSettings configuration");

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
