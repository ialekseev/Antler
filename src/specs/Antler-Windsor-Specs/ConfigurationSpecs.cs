// ReSharper disable InconsistentNaming
using FluentAssertions;
using NUnit.Framework;
using SmartElk.Antler.Core.Abstractions.Configuration;

namespace SmartElk.Antler.Windsor.Specs
{
    public class ConfigurationSpecs
    {
        [TestFixture]
        [Category("Integration")]
        public class when_trying_to_configure_to_use_windsor
        {
            [Test]
            public void should_set_windsor_container()
            {
                //arrange
                var configurator = new AntlerConfigurator();
                
                //act
                configurator.UseWindsorContainer();

                //assert
                configurator.Configuration.Container.Should().BeOfType<WindsorContainerAdapter>();
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
