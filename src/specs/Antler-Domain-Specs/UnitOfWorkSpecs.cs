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
                Func<string, ISessionScopeFactory> sessionScopeFactoryExtractor = name => (name == UnitOfWorkSettings.Default.StorageName ? SessionScopeFactory : null);
                UnitOfWork.SessionScopeFactoryExtractor = sessionScopeFactoryExtractor;
            }
        }
        
        public class UnitOfWorkNamedStorageScenario
        {
            protected ISessionScopeFactory SessionScopeFactory { get; set; }
            protected ISessionScope SessionScope { get; set; }
            public UnitOfWorkNamedStorageScenario()
            {
                SessionScopeFactory = A.Fake<ISessionScopeFactory>();
                SessionScope = A.Fake<ISessionScope>();
                A.CallTo(() => SessionScopeFactory.Open()).Returns(SessionScope);
                Func<string, ISessionScopeFactory> sessionScopeFactoryExtractor = name => (name=="SuperStorage" ? SessionScopeFactory: null);
                UnitOfWork.SessionScopeFactoryExtractor = sessionScopeFactoryExtractor;
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
               var result = UnitOfWork.Do(uow => 1);

                //assert
                A.CallTo(() => SessionScopeFactory.Open()).MustHaveHappened();
                result.Should().Be(1);
            }
        }

        [TestFixture]
        [Category("Unit")]
        public class when_trying_to_create_unit_of_work_with_storage_name : UnitOfWorkNamedStorageScenario
        {
            [Test]
            public void should_open_session()
            {
                //act
                UnitOfWork.Do(uow => { }, new UnitOfWorkSettings { StorageName = "SuperStorage" });

                //assert
                A.CallTo(() => SessionScopeFactory.Open()).MustHaveHappened();
            }
        }

        [TestFixture]
        [Category("Unit")]
        public class when_trying_to_create_unit_of_work_with_storage_name_returning_result : UnitOfWorkNamedStorageScenario
        {
            [Test]
            public void should_open_session_and_get_result()
            {
                //act
                var result = UnitOfWork.Do(uow => "super", new UnitOfWorkSettings { StorageName = "SuperStorage" });

                //assert
                A.CallTo(() => SessionScopeFactory.Open()).MustHaveHappened();
                result.Should().Be("super");
            }
        }

        [TestFixture]
        [Category("Unit")]
        public class when_explicitly_committing_unit_of_work : UnitOfWorkScenario
        {
            [Test]
            public void should_commit_and_dispose_only_once()
            {
                //act
                UnitOfWork.Do(uow => uow.Commit());

                //assert
                A.CallTo(() => SessionScope.Commit()).MustHaveHappened(Repeated.Exactly.Once);
                A.CallTo(() => SessionScope.Dispose()).MustHaveHappened(Repeated.Exactly.Once);
            }
        }

        [TestFixture]
        [Category("Unit")]
        public class when_rolling_back_unit_of_work : UnitOfWorkScenario
        {
            [Test]
            public void should_rollback_and_dispose()
            {
                //act
                UnitOfWork.Do(uow => uow.Rollback());

                //assert
                A.CallTo(() => SessionScope.Commit()).MustNotHaveHappened();
                A.CallTo(() => SessionScope.Rollback()).MustHaveHappened(Repeated.Exactly.Once);
                A.CallTo(() => SessionScope.Dispose()).MustHaveHappened(Repeated.Exactly.Once);
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
                A.CallTo(() => SessionScope.Commit()).MustHaveHappened(Repeated.Exactly.Once);
                A.CallTo(() => SessionScope.Dispose()).MustHaveHappened(Repeated.Exactly.Once);
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
            public void should_not_open_new_session_and_not_commit_nested_unit_of_work()
            {                
                //act
                UnitOfWork.Do(uow => UnitOfWork.Do(nested =>
                    {                        
                    }));

                //assert
                A.CallTo(()=>SessionScopeFactory.Open()).MustHaveHappened(Repeated.Exactly.Once);
                A.CallTo(() => SessionScope.Commit()).MustHaveHappened(Repeated.Exactly.Once);
            }
        }

        [TestFixture]
        [Category("Unit")]
        public class when_trying_to_explicitly_commit_nested_unit_of_work : UnitOfWorkScenario
        {
            [Test]
            public void should_not_commit_nested_unit_of_work()
            {
                //act
                UnitOfWork.Do(uow => UnitOfWork.Do(nested => nested.Commit()));

                //assert                
                A.CallTo(() => SessionScope.Commit()).MustHaveHappened(Repeated.Exactly.Once);
            }
        }

        [TestFixture]
        [Category("Unit")]
        public class when_trying_to_rollback_nested_unit_of_work : UnitOfWorkScenario
        {
            [Test]
            public void should_not_rollback_nested_unit_of_work()
            {
                //act
                UnitOfWork.Do(uow => UnitOfWork.Do(nested => nested.Rollback()));

                //assert                
                A.CallTo(() => SessionScope.Rollback()).MustNotHaveHappened();
            }
        }

        [TestFixture]
        [Category("Unit")]
        public class when_trying_to_get_current_uow_from_the_out_of_scope : UnitOfWorkScenario
        {
            [Test]
            public void should_return_none()
            {
                //arrange
                UnitOfWork.Do(uow =>
                {                    
                });

                //assert
                UnitOfWork.Current.IsNone.Should().BeTrue();
            }
        }

        [TestFixture]
        [Category("Unit")]
        public class when_trying_to_commit_unit_of_work_with_enabled_commits : UnitOfWorkScenario
        {
            [Test]
            public void should_commit()
            {
                //act
                UnitOfWork.Do(uow => { }, new UnitOfWorkSettings() { EnableCommit = true });

                //assert
                A.CallTo(() => SessionScope.Commit()).MustHaveHappened();
            }
        }
        
        [TestFixture]
        [Category("Unit")]
        public class when_trying_to_commit_unit_of_work_with_disabled_commits : UnitOfWorkScenario
        {
            [Test]
            public void should_not_commit()
            {                
                //act
                UnitOfWork.Do(uow => { }, new UnitOfWorkSettings() { EnableCommit = false });

                //assert
                A.CallTo(() => SessionScope.Commit()).MustNotHaveHappened();
            }
        }

        [TestFixture]
        [Category("Unit")]
        public class when_using_unit_of_work_with_rollback_on_dispose_setting : UnitOfWorkScenario
        {
            [Test]
            public void should_rollback()
            {
                //act
                UnitOfWork.Do(uow => { }, new UnitOfWorkSettings() { RollbackOnDispose = true});

                //assert
                A.CallTo(() => SessionScope.Commit()).MustNotHaveHappened();
                A.CallTo(() => SessionScope.Rollback()).MustHaveHappened();
            }
        }

        [TestFixture]
        [Category("Unit")]
        public class when_using_unit_of_work_without_rollback_on_dispose_setting : UnitOfWorkScenario
        {
            [Test]
            public void should_commit()
            {
                //act
                UnitOfWork.Do(uow => { }, new UnitOfWorkSettings() { RollbackOnDispose = false });

                //assert
                A.CallTo(() => SessionScope.Rollback()).MustNotHaveHappened();
                A.CallTo(() => SessionScope.Commit()).MustHaveHappened();
            }
        }

        [TestFixture]
        [Category("Unit")]
        public class when_throwing_exception_during_unit_of_work : UnitOfWorkScenario
        {
            [Test]
            public void should_not_try_to_commit_or_rollback_and_should_close_unit_of_work()
            {
                //act
                try
                {
                    UnitOfWork.Do(uow =>
                    {
                        throw new Exception();
                    });
                }
                catch (Exception)
                {
                    //assert                    
                    A.CallTo(() => SessionScope.Rollback()).MustNotHaveHappened();
                    A.CallTo(() => SessionScope.Commit()).MustNotHaveHappened();
                    
                    A.CallTo(() => SessionScope.Dispose()).MustHaveHappened();
                    UnitOfWork.Current.IsNone.Should().BeTrue();
                }
            }
        }
        
        [TestFixture]
        [Category("Unit")]
        public class when_throwing_exception_during_commit : UnitOfWorkScenario
        {
            [Test]
            public void should_close_unit_of_work()
            {
                //arrange
                A.CallTo(() => SessionScope.Commit()).Throws<Exception>();

                //act
                try
                {
                    UnitOfWork.Do(uow =>
                    {                        
                    });
                }
                catch (Exception)
                {
                    //assert
                    A.CallTo(() => SessionScope.Dispose()).MustHaveHappened();
                    UnitOfWork.Current.IsNone.Should().BeTrue();
                }                                                
            }
        }

        [TestFixture]
        [Category("Unit")]
        public class when_using_nested_unit_of_work_with_throw_if_nested_setting : UnitOfWorkScenario
        {
            [Test]
            [ExpectedException(typeof(NotSupportedException))]
            public void should_throw()
            {
                //act
                var setting = new UnitOfWorkSettings() {ThrowIfNestedUnitOfWork = true};

                UnitOfWork.Do(uow => UnitOfWork.Do(nested => nested, setting), setting);                
            }
        }
    }
}
// ReSharper restore InconsistentNaming

