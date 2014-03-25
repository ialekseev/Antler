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
        
        public class Superman: ISuper
        {
            public Guid Id { get; set; }

            public Superman()
            {
                Id = Guid.NewGuid();
            }

            public void DoKindness()
            {                
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
    }
}

// ReSharper restore InconsistentNaming