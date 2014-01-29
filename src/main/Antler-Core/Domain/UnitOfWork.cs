﻿using System;
using SmartElk.Antler.Core.Common.CodeContracts;

namespace SmartElk.Antler.Core.Domain
{
    //todo: add support for nested UoF
    public class UnitOfWork: IDisposable
    {        
        private readonly ISessionScope _sessionScope;
                                        
        public static Func<ISessionScopeFactory> SessionScopeFactoryExtractor { get; set; }        
        public static Func<string, ISessionScopeFactory> SessionScopeFactoryNamedExtractor { get; set; }
                        
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
