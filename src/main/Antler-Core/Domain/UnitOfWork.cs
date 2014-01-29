﻿using System;
using SmartElk.Antler.Core.Common.CodeContracts;

namespace SmartElk.Antler.Core.Domain
{    
    //todo: add rollback support
    //todo: cover by Unit-tests
    public class UnitOfWork: IDisposable
    {
        public ISessionScope SessionScope { get; private set; }

        [ThreadStatic]
        private static UnitOfWork _current;
        public static UnitOfWork Current
        {
            get { return _current; }
        }

        private UnitOfWork _parent;
        
        public bool IsFinished { get; private set; }
        public bool IsRoot
        {
            get { return _parent == null; }
        }

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
            Requires.NotNullOrEmpty(storageName, "Storage name can't be null or empty");
            Assumes.True(SessionScopeFactoryNamedExtractor != null, "SessionScopeFactoryNamedExtractor should be set before using UnitOfWork with storage name. Wrong configuraiton?");
            var sessionScopeFactory = SessionScopeFactoryNamedExtractor(storageName);
            SetSession(sessionScopeFactory);
        }

        private void SetSession(ISessionScopeFactory sessionScopeFactory)
        {
            Requires.NotNull(sessionScopeFactory, "Can't continue without SessionScopeFactory. Wrong configuration?");

            if (_current != null)
            {
                _parent = _current;
                SessionScope = _parent.SessionScope;
            }
            else
            {
                SessionScope = sessionScopeFactory.Open();
            }                                    
           _current = this;
            IsFinished = false;
        }

        public static void Do(Action<UnitOfWork> work)
        {
            using (var uow = new UnitOfWork())
            {
                work(uow);
            }
        }

        public static TResult Do<TResult>(Func<UnitOfWork, TResult> work)
        {
            using (var uow = new UnitOfWork())
            {
                return work(uow);
            }
        }

        public static void Do(string storageName, Action<UnitOfWork> work)
        {
            using (var uow = new UnitOfWork(storageName))
            {
                work(uow);
            }
        }

        public static TResult Do<TResult>(string storageName, Func<UnitOfWork, TResult> work)
        {
            using (var uow = new UnitOfWork(storageName))
            {
                return work(uow);
            }
        }

        public void Dispose()
        {
            Assumes.True(!IsFinished, "This UnitOfWork is finished");

            if (!IsRoot)
                return;
            
           SessionScope.Commit();
           SessionScope.Dispose();
           
           IsFinished = true;
           _current = null;
        }
                
        public IRepository<TEntity> Repo<TEntity>() where TEntity: class
        {
            return SessionScope.CreateRepository<TEntity>();
        }                
    }
}
