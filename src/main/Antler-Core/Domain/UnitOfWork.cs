using System;
using System.Runtime.InteropServices;
using System.Threading.Tasks;
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

        /// <summary>
        /// Start database transaction.
        /// </summary>   
        public static void Do(Action<UnitOfWork> work, UnitOfWorkSettings settings = null)
        {
            Requires.NotNull(work, "work");
            
            using (var uow = new UnitOfWork(settings))
            {
                work(uow);
            }
        }
        
        /// <summary>
        /// Start database transaction and return result from it.
        /// </summary>   
        public static TResult Do<TResult>(Func<UnitOfWork, TResult> work, UnitOfWorkSettings settings = null)
        {
            Requires.NotNull(work, "work");

            using (var uow = new UnitOfWork(settings))
            {
                return work(uow);
            }
        }

        /// <summary>
        /// Start database transaction(asynchronous version).
        /// </summary>   
        public static Task DoAsync(Action<UnitOfWork> work, UnitOfWorkSettings settings = null)
        {
            Requires.NotNull(work, "work");

            return Task.Factory.StartNew(() => Do(work, settings));
        }

        /// <summary>
        /// Start database transaction and return result from it(asynchronous version).
        /// </summary>   
        public static Task<TResult> DoAsync<TResult>(Func<UnitOfWork, TResult> work, UnitOfWorkSettings settings = null)
        {
            Requires.NotNull(work, "work");

            return Task<TResult>.Factory.StartNew(() => Do(work, settings));
        }

        /// <summary>
        /// Commit/Rollback transaction(depending on the configuration) explicitly. Will be called automatically in the end of the "Do" block.
        /// </summary> 
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
                {                                       
                    CloseUnitOfWork();
                }                  
            }
        }

        /// <summary>
        /// Commit database transaction explicitly(not necessarily to use in standard configuration, because transaction will be committed anyway in the end of the "Do" block).
        /// </summary>  
        public void Commit()
        {                        
                Perform(() =>
                    {
                        if (Settings.EnableCommit) 
                            SessionScope.Commit();
                    });
        }

        /// <summary>
        /// Rollback database transaction explicitly. As alternative you can use RollbackOnDispose setting to rollback transaction automatically in the end of the "Do" block(may be useful in testing).
        /// </summary>  
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

        /// <summary>
        /// Get Repository object to perform queries/operations on database.
        /// </summary> 
        public IRepository<TEntity> Repo<TEntity>() where TEntity: class
        {
            return SessionScope.CreateRepository<TEntity>();
        }                   
    }
}
