// ReSharper disable InconsistentNaming
using System;
using FakeItEasy;
using FluentAssertions;
using NUnit.Framework;
using SmartElk.Antler.Core.Domain;

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
                UnitOfWork.SessionScopeFactoryExtractor = sessionScopeFactoryExtractor;
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
                UnitOfWork.SessionScopeFactoryNamedExtractor = sessionScopeFactoryExtractor;
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
        public class when_trying_to_create_unit_of_work_returning_result : UnitOfWorkScenario
        {
            [Test]
            public void should_open_session_and_get_result()
            {
                //act
               var result = UnitOfWork.Do(uow => { return 1; });

                //assert
                A.CallTo(() => SessionScopeFactory.Open()).MustHaveHappened();
                result.Should().Be(1);
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
        public class when_trying_to_create_unit_of_work_with_storage_name_returning_result : UnitOfWorkNamedScenario
        {
            [Test]
            public void should_open_session_and_get_result()
            {
                //act
                var result = UnitOfWork.Do("SuperStorage", uow => { return "super"; });

                //assert
                A.CallTo(() => SessionScopeFactory.Open()).MustHaveHappened();
                result.Should().Be("super");
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
                        uow.SessionScope.Should().NotBeNull();
                    });
                
            }
        }

        [TestFixture]
        [Category("Unit")]
        public class when_trying_to_create_nested_unit_of_work : UnitOfWorkScenario
        {
            [Test]
            public void should_set_hierarchy_properly()
            {                
                UnitOfWork.Do(uow =>
                    {
                        //assert
                        uow.IsRoot.Should().BeTrue();
                        uow.ParentId.HasValue.Should().BeFalse();
                        UnitOfWork.Current.Value.Id.Should().Be(uow.Id);

                        UnitOfWork.Do(nested =>
                            {
                                //assert
                                nested.IsRoot.Should().BeFalse();
                                nested.ParentId.Value.Should().Be(uow.Id);
                                UnitOfWork.Current.Value.Id.Should().Be(nested.Id);
                            });
                    });
            }
        }

        [TestFixture]
        [Category("Unit")]
        public class when_trying_to_commit_root_unit_of_work_ : UnitOfWorkScenario
        {
            [Test]
            public void should_commit_only_root()
            {
                UnitOfWork rootUow = null;
                UnitOfWork.Do(root =>
                    {
                    rootUow = root;
                    UnitOfWork nestedUow=null;
                    UnitOfWork.Do(nested =>
                    {                        
                        nestedUow = nested;                        
                    });
                        
                        //assert
                        nestedUow.IsFinished.Should().BeFalse();
                        A.CallTo(() => SessionScope.Commit()).MustNotHaveHappened();
                        A.CallTo(() => SessionScope.Dispose()).MustNotHaveHappened();
                        UnitOfWork.Current.Value.Id.Should().Be(rootUow.Id);                        
                    });
                
                //assert
                rootUow.IsFinished.Should().BeTrue();
                A.CallTo(() => SessionScope.Commit()).MustHaveHappened();
                A.CallTo(() => SessionScope.Dispose()).MustHaveHappened();
                UnitOfWork.Current.IsSome.Should().BeFalse();
            }
        }

        [TestFixture]
        [Category("Unit")]
        public class when_trying_to_get_current_uow_from_the_out_of_scope_ : UnitOfWorkScenario
        {
            [Test]
            public void should_return_null()
            {
                //arrange
                UnitOfWork.Do(uow =>
                {                    
                });

                //assert
                UnitOfWork.Current.IsSome.Should().BeFalse();
            }
        }
    }
}
// ReSharper restore InconsistentNaming

