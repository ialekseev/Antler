// ReSharper disable InconsistentNaming

using FluentAssertions;
using NUnit.Framework;
using SmartElk.Antler.Core.Abstractions.Configuration;
using StructureMap;

namespace SmartElk.Antler.StructureMap.Specs
{
    public class ConfigurationSpecs
    {        
        [TestFixture]
        [Category("Integration")]
        public class when_trying_to_configure_to_use_container
        {
            [Test]
            public void should_set_container()
            {                                
                //arrange
                var configurator = new AntlerConfigurator();
                
                //act
                configurator.UseStructureMapContainer();

                //assert
                configurator.Configuration.Container.Should().BeOfType<StructureMapContainerAdapter>();
            }
        }

        [TestFixture]
        [Category("Integration")]        
        public class when_trying_to_configure_to_use_existing_container_with_registered_components
        {
            private class TestComponent { }

            [Test]
            public void should_set_container_with_registered_components()
            {
                //arrange
                var container = new Container(x => x.For<TestComponent>().Use<TestComponent>());                                
                var configurator = new AntlerConfigurator();

                //act
                configurator.UseStructureMapContainer(container);
                                                                                                                               
                //assert
                configurator.Configuration.Container.Should().BeOfType<StructureMapContainerAdapter>();                
                configurator.Configuration.Container.Get<TestComponent>().Should().BeOfType<TestComponent>();
            }
        }

        [TestFixture]
        [Category("Integration")]
        public class when_trying_to_use_configurator_without_container
        {
            [Test]
            public void should_not_have_any_container()
            {
                //arrange
                var configurator = new AntlerConfigurator();

                //assert
                configurator.Configuration.Container.Should().BeNull();
            }
        } 
    }
}

// ReSharper restore InconsistentNaming