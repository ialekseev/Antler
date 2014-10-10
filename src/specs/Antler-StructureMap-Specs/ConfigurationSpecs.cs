// ReSharper disable InconsistentNaming

using System.Linq;
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
        [Ignore]
        public class when_trying_to_configure_to_use_existing_container_with_registered_components
        {
            private class TestComponent { }

            [Test]
            public void should_set_container_with_registered_components()
            {
                //arrange
                var container = new Container(x => x.For<TestComponent>());
                
                var configurator = new AntlerConfigurator();

                //act
                configurator.UseStructureMapContainer(container);

                //todo: with this line uncommented, test works. Another mistery after updating StructureMap to version 3(ask on stackoverflow?)...                
                //container.GetInstance<TestComponent>();
                                                                                                                               
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