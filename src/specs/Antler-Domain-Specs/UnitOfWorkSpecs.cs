// ReSharper disable InconsistentNaming
using System;
using FakeItEasy;
using FluentAssertions;
using NUnit.Framework;

namespace SmartElk.Antler.Domain.Specs
{
    public class UnitOfWorkSpecs
    {        
        public class UnitOfWorkScenario
        {
            protected ISessionScopeFactory SessionScopeFactory { get; set; }
            protected ISessionScope SessionScope { get; set; }
            public UnitOfWorkScenario()
            {
                SessionScopeFactory = A.Fake<ISessionScopeFactory>();
                SessionScope = A.Fake<ISessionScope>();
                A.CallTo(() => SessionScopeFactory.Open()).Returns(SessionScope);
                Func<ISessionScopeFactory> sessionScopeFactoryExtractor = () => SessionScopeFactory;
                UnitOfWork.SetSessionScopeFactoryExtractor(sessionScopeFactoryExtractor);
            }
        }
        
        public class UnitOfWorkNamedScenario
        {
            protected ISessionScopeFactory SessionScopeFactory { get; set; }
            protected ISessionScope SessionScope { get; set; }
            public UnitOfWorkNamedScenario()
            {
                SessionScopeFactory = A.Fake<ISessionScopeFactory>();
                SessionScope = A.Fake<ISessionScope>();
                A.CallTo(() => SessionScopeFactory.Open()).Returns(SessionScope);
                Func<string, ISessionScopeFactory> sessionScopeFactoryExtractor = name => (name=="SuperStorage" ? SessionScopeFactory: null);
                UnitOfWork.SetSessionScopeFactoryNamedExtractor(sessionScopeFactoryExtractor);
            }
        }
        
        [TestFixture]
        [Category("Unit")]
        public class when_trying_to_create_unit_of_work : UnitOfWorkScenario
        {
            [Test]
            public void should_open_session()
            {
                //act
                UnitOfWork.Do(uow => { });

                //assert
                A.CallTo(() => SessionScopeFactory.Open()).MustHaveHappened();
            }
        }
        
        [TestFixture]
        [Category("Unit")]
        public class when_trying_to_create_unit_of_work_with_storage_name : UnitOfWorkNamedScenario
        {
            [Test]
            public void should_open_session()
            {
                //act
                UnitOfWork.Do("SuperStorage", uow => { });

                //assert
                A.CallTo(() => SessionScopeFactory.Open()).MustHaveHappened();
            }
        }

        [TestFixture]
        [Category("Unit")]
        public class when_disposing_unit_of_work : UnitOfWorkScenario
        {
            [Test]
            public void should_commit_and_dispose()
            {                                
                //act
                UnitOfWork.Do(uow => { });

                //assert
                A.CallTo(() => SessionScope.Commit()).MustHaveHappened();
                A.CallTo(() => SessionScope.Dispose()).MustHaveHappened();
            }
        }

        [TestFixture]
        [Category("Unit")]
        public class when_trying_to_get_repository : UnitOfWorkScenario
        {
            public class TestEntity
            {
                public string Surname { get; set; }
            }

            [Test]
            public void should_return_repository()
            {
                //arrange
                var repository = A.Fake<IRepository<TestEntity>>();
                A.CallTo(() => SessionScope.CreateRepository<TestEntity>()).Returns(repository);

                //act
                UnitOfWork.Do(uow =>
                    {
                        var result = uow.Repo<TestEntity>();
                        
                        //assert
                        result.Should().NotBeNull();
                    });                
            }
        }

        [TestFixture]
        [Category("Unit")]
        public class when_trying_to_get_current_session : UnitOfWorkScenario
        {
            [Test]
            public void should_return_current_session()
            {
                //act
                UnitOfWork.Do(uow =>
                    {
                        //assert
                        uow.CurrentSession.Should().NotBeNull();
                    });
                
            }
        }

    }
}
// ReSharper restore InconsistentNaming

