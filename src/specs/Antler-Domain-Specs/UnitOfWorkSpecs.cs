// ReSharper disable InconsistentNaming
using System;
using FakeItEasy;

namespace SmartElk.Antler.Domain.Specs
{
    public class UnitOfWorkSpecs
    {
        public class UnitOfWorkScenario
        {
            protected ISessionScopeFactory SessionScopeFactory { get; set; }
            public UnitOfWorkScenario()
            {
                SessionScopeFactory = A.Fake<ISessionScopeFactory>();                
                Func<string, ISessionScopeFactory> sessionScopeFactoryExtractor = a => SessionScopeFactory;
                UnitOfWork.SetSessionScopeFactoryExtractor(sessionScopeFactoryExtractor);
            }
        }        
    }
}
// ReSharper restore InconsistentNaming

