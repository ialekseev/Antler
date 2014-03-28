// ReSharper disable InconsistentNaming

using System;
using FluentAssertions;
using NUnit.Framework;
using SmartElk.Antler.Core.Abstractions;
using SmartElk.Antler.Core.Abstractions.Registration;
using SmartElk.Antler.Core.Common.Container;

namespace SmartElk.Antler.Common.Specs
{
    public class BuiltInContainerSpecs
    {
        public class BuiltInContainerTestingScenario
        {
            protected IContainer Container { get; set; }

            public BuiltInContainerTestingScenario()
            {
                Container = new BuiltInContainer();
            }
        }
        
        public interface ISuper
        {
            Guid Id { get; }
            void DoKindness();
        }

        public interface IVillain
        {
            void DoHarm();
        }

        public class Joker: IVillain
        {
            public void DoHarm()
            {                
            }
        }

        public class Superman: ISuper, ICloneable
        {
            public Guid Id { get; set; }

            public Superman()
            {
                Id = Guid.NewGuid();
            }

            public Superman(Guid id)
            {
                Id = id;
            }

            public void DoKindness()
            {                
            }

            public override int GetHashCode()
            {
                return Id.GetHashCode();
            }

            public override bool Equals(object obj)
            {                
                return ((Superman)obj).Id.Equals(Id);
            }

            public object Clone()
            {
                return new Superman(Id);
            }
        }

        [TestFixture]
        [Category("Unit")]
        public class when_trying_to_get_from_container : BuiltInContainerTestingScenario
        {
            [Test]
            public void should_return_item()
            {
                //arrange
                var superman = new Superman();
                Container.Put(Binding.Use<ISuper>(superman));

                //act
                var result = Container.Get(typeof(ISuper));

                //assert
                result.Should().NotBeNull();
            }
        }

        [TestFixture]
        [Category("Unit")]
        public class when_trying_to_get_from_container_object_that_is_not_in_container : BuiltInContainerTestingScenario
        {
            [Test]
            public void should_return_null()
            {
                //arrange
                var superman = new Superman();
                Container.Put(Binding.Use<ISuper>(superman));

                //act
                var result = Container.Get(typeof(Superman));

                //assert
                result.Should().BeNull();
            }
        }

        [TestFixture]
        [Category("Unit")]
        public class when_trying_to_get_from_container_using_generic_method : BuiltInContainerTestingScenario
        {
            [Test]
            public void should_return_item()
            {
                //arrange
                var superman = new Superman();
                Container.Put(Binding.Use<ISuper>(superman));
                
                //act
                var result = Container.Get<ISuper>();

                //assert
                result.Should().NotBeNull();
            }
        }

        [TestFixture]
        [Category("Unit")]
        public class when_trying_to_get_from_container_by_name : BuiltInContainerTestingScenario
        {
            [Test]
            public void should_return_item()
            {
                //arrange
                var superman = new Superman();
                Container.Put(Binding.Use<ISuper>(superman).Named("bale"));

                //act
                var result = Container.Get(typeof(ISuper), "bale");

                //assert
                result.Should().NotBeNull();
            }
        }

        [TestFixture]
        [Category("Unit")]
        public class when_trying_to_get_from_container_by_name_using_generic_method : BuiltInContainerTestingScenario
        {
            [Test]
            public void should_return_item()
            {
                //arrange
                var superman = new Superman();
                Container.Put(Binding.Use<ISuper>(superman).Named("bale"));

                //act
                var result = Container.Get<ISuper>("bale");

                //assert
                result.Should().NotBeNull();
            }
        }

        [TestFixture]
        [Category("Unit")]
        public class when_trying_to_release_item_from_container : BuiltInContainerTestingScenario
        {
            [Test]
            public void should_release_item()
            {
                //arrange
                var superman = new Superman();
                Container.Put(Binding.Use<ISuper>(superman).Named("bale"));

                //act
                Container.Release(superman.Clone());

                //assert
                Container.Get<ISuper>("bale").Should().BeNull();
            }
        }

        [TestFixture]
        [Category("Unit")]
        public class when_trying_to_put_item_in_container : BuiltInContainerTestingScenario
        {
            [Test]
            public void should_put_item()
            {                                
                //act
                var superman = new Superman();
                Container.Put(Binding.Use<ISuper>(superman));

                //assert
                Container.Get<ISuper>().Should().NotBeNull();
            }
        }

        [TestFixture]
        [Category("Unit")]
        public class when_trying_to_check_if_existing_item_present_in_container : BuiltInContainerTestingScenario
        {
            [Test]
            public void should_return_true()
            {
                //arrange
                var superman = new Superman();
                Container.Put(Binding.Use<ISuper>(superman));

                //act                
                var result = Container.Has(typeof (ISuper));

                //assert
                result.Should().BeTrue();
            }
        }

        [TestFixture]
        [Category("Unit")]
        public class when_trying_to_check_if_non_existing_item_present_in_container : BuiltInContainerTestingScenario
        {
            [Test]
            public void should_return_false()
            {                
                //act                
                var result = Container.Has(typeof(ISuper));

                //assert
                result.Should().BeFalse();
            }
        }

        [TestFixture]
        [Category("Unit")]
        public class when_trying_dispose_container : BuiltInContainerTestingScenario
        {
            [Test]
            public void should_clear_all_items_in_container()
            {
                //arrange
                var superman = new Superman();                
                Container.Put(Binding.Use<ISuper>(superman));
                var joker = new Joker();
                Container.Put(Binding.Use<IVillain>(joker).Named("joker"));

                //act                
                Container.Dispose();

                //assert
                Container.Has(typeof (ISuper)).Should().BeFalse();
                Container.Has(typeof (IVillain), "joker").Should().BeFalse();
            }
        }
    }
}

// ReSharper restore InconsistentNaming