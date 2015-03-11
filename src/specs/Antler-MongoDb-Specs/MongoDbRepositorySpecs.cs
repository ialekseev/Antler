using System;
using System.Linq;
using FakeItEasy;
using FluentAssertions;
using NUnit.Framework;
using SmartElk.Antler.Core.Common;
using SmartElk.Antler.Core.Common.CodeContracts;
using SmartElk.Antler.MongoDb.Internal;

// ReSharper disable InconsistentNaming

namespace SmartElk.Antler.MongoDb.Specs
{
    public class MongoDbRepositorySpecs
    {
        public class MongoDbRepositoryTestingScenario
        {
            public class TestEntity
            {
                public int Id { get; set; }
                public string Field { get; set; }
            }

            protected ISessionScopeEx SessionScope { get; set; }
            protected string IdPropertyName { get; private set; }
            protected Option<Func<object>> IdentityGenerator { get; private set; }
            protected MongoDbRepository<TestEntity> Repository { get; private set; }

            public MongoDbRepositoryTestingScenario(string idPropertyName = "Id")
            {
                SessionScope = A.Fake<ISessionScopeEx>();                
                IdentityGenerator = Option<Func<object>>.Some(() => 777);
                Repository = new MongoDbRepository<TestEntity>(SessionScope, idPropertyName, IdentityGenerator);
            }

            public MongoDbRepositoryTestingScenario(Option<Func<object>> identityGenerator, string idPropertyName = "Id")
            {
                SessionScope = A.Fake<ISessionScopeEx>();
                IdentityGenerator = identityGenerator;
                Repository = new MongoDbRepository<TestEntity>(SessionScope, idPropertyName, IdentityGenerator);
            }            
        }
        
        [TestFixture]
        [Category("Unit")]
        public class when_trying_to_get_queryable : MongoDbRepositoryTestingScenario
        {
            [Test]
            public void should_get_it_from_session_scope()
            {
                //arrange
                A.CallTo(() => SessionScope.AsQueryable<TestEntity>()).Returns(A.Dummy<IQueryable<TestEntity>>());

                //act
                var result = Repository.AsQueryable();

                //assert
                A.CallTo(()=>SessionScope.AsQueryable<TestEntity>()).MustHaveHappened(Repeated.Exactly.Once);
                result.Should().NotBeNull();
            }
        }

        [TestFixture]
        [Category("Unit")]
        public class when_trying_to_get_entity_by_id : MongoDbRepositoryTestingScenario
        {
            [Test]
            public void should_get_it()
            {
                //arrange
                A.CallTo(() => SessionScope.GetById<int, TestEntity>(4)).Returns(new TestEntity(){Id=4, Field = "Super"});

                //act
                var result = Repository.GetById(4);

                //assert                
                result.Id.Should().Be(4);
                result.Field.Should().Be("Super");
            }
        }

        [TestFixture]
        [Category("Unit")]
        public class when_trying_to_insert_entity_with_wrong_id_property_name: MongoDbRepositoryTestingScenario
        {
            public when_trying_to_insert_entity_with_wrong_id_property_name(): base("Key") {}

            [Test]
            [ExpectedException(typeof(ArgumentException))]
            public void should_throw()
            {                                                
                //act
                Repository.Insert(new TestEntity() { Id = 3, Field = "Great!" });                
            }
        }

        [TestFixture]
        [Category("Unit")]
        public class when_trying_to_insert_entity_with_set_identity_field_value : MongoDbRepositoryTestingScenario
        {
            [Test]            
            public void should_mark_as_a_new_not_changing_identity_field()
            {                
                //act
                var result = Repository.Insert(new TestEntity() { Id = 3, Field = "Great!" });

                //assert
                A.CallTo(()=>SessionScope.MarkAsNew(A<TestEntity>.That.Matches(t=>t.Id==3 && t.Field=="Great!"))).MustHaveHappened(Repeated.Exactly.Once);
                result.Id.Should().Be(3);
                result.Field.Should().Be("Great!");
            }
        }

        [TestFixture]
        [Category("Unit")]
        public class when_trying_to_insert_entity_without_identity_field_value_using_repository_without_identity_generator : MongoDbRepositoryTestingScenario
        {
            public when_trying_to_insert_entity_without_identity_field_value_using_repository_without_identity_generator(): base(Option<Func<object>>.None){}

            [Test]
            public void should_mark_as_a_new_leaving_identity_field_without_value_because_identity_generator_has_been_configured()
            {
                //act                                
                var result = Repository.Insert(new TestEntity() { Field = "Great!" });

                //assert
                A.CallTo(() => SessionScope.MarkAsNew(A<TestEntity>.That.Matches(t => t.Id == 0 && t.Field == "Great!"))).MustHaveHappened(Repeated.Exactly.Once);
                result.Id.Should().Be(0);
                result.Field.Should().Be("Great!");
            }           
        }

        [TestFixture]
        [Category("Unit")]
        public class when_trying_to_insert_entity_without_identity_field_value_and_generator_that_generates_value_of_different_type : MongoDbRepositoryTestingScenario
        {
            public when_trying_to_insert_entity_without_identity_field_value_and_generator_that_generates_value_of_different_type() : base(Option<Func<object>>.Some(() => Guid.NewGuid())) { }

            [Test]
            [ExpectedException(typeof(Assumes.InternalErrorException))]
            public void should_throw_because_generated_identity_type_does_not_correspond_to_entity_id_type()
            {
                //act
                Repository.Insert(new TestEntity() { Field = "Great!" });
            }
        }
        
        [TestFixture]
        [Category("Unit")]
        public class when_trying_to_insert_entity_without_identity_field_value: MongoDbRepositoryTestingScenario               
        {
            [Test]
            public void should_mark_as_a_new_with_newly_generated_identity_field()
            {
                //act
                var result = Repository.Insert(new TestEntity() { Field = "Great!" });

                //assert
                A.CallTo(() => SessionScope.MarkAsNew(A<TestEntity>.That.Matches(t => t.Id == 777 && t.Field == "Great!"))).MustHaveHappened(Repeated.Exactly.Once);
                result.Id.Should().Be(777);
                result.Field.Should().Be("Great!");
            }
        }

        [TestFixture]
        [Category("Unit")]
        public class when_trying_to_update_entity : MongoDbRepositoryTestingScenario
        {
            [Test]
            public void should_mark_as_updated_and_return_entity()
            {                                
                //act
                var result = Repository.Update(new TestEntity() {Id = 4, Field = "Super"});

                //assert                
                A.CallTo(()=>SessionScope.MarkAsUpdated(A<TestEntity>.That.Matches(t=>t.Id==4 && t.Field=="Super"))).MustHaveHappened();                
                result.Id.Should().Be(4);
                result.Field.Should().Be("Super");
            }
        }

        [TestFixture]
        [Category("Unit")]
        public class when_trying_to_delete_entity : MongoDbRepositoryTestingScenario
        {
            [Test]
            public void should_mark_as_deleted()
            {
                //act
                Repository.Delete(new TestEntity() { Id = 4, Field = "Super" });

                //assert                
                A.CallTo(() => SessionScope.MarkAsDeleted(A<TestEntity>.That.Matches(t => t.Id == 4 && t.Field == "Super"))).MustHaveHappened();                
            }
        }

        [TestFixture]
        [Category("Unit")]
        public class when_trying_to_delete_entity_by_id : MongoDbRepositoryTestingScenario
        {
            [Test]
            public void should_mark_as_deleted()
            {
                //arrange
                A.CallTo(()=>SessionScope.GetById<int, TestEntity>(4)).Returns(new TestEntity(){Id=4, Field = "Super"});

                //act
                Repository.Delete(4);

                //assert                
                A.CallTo(()=>SessionScope.MarkAsDeleted(A<TestEntity>.That.Matches(t => t.Id == 4 && t.Field == "Super"))).MustHaveHappened();
            }
        }
    }
}

// ReSharper restore InconsistentNaming