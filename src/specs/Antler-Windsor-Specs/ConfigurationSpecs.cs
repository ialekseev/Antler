// ReSharper disable InconsistentNaming
using FluentAssertions;
using NUnit.Framework;
using SmartElk.Antler.Abstractions.Configuration;

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
                var basicConfigurator = new BasicConfigurator();
                
                //act
                basicConfigurator.UseWindsorContainer();

                //assert
                basicConfigurator.Configuration.Container.Should().BeOfType<WindsorContainerAdapter>();
            }
        }

        [TestFixture]
        [Category("Integration")]
        public class when_using_configurator_without_container
        {
            [Test]
            public void should_not_have_any_container()
            {
                //arrange
                var basicConfigurator = new BasicConfigurator();
                                
                //assert
                basicConfigurator.Configuration.Container.Should().BeNull();
            }
        }        
    }
}
// ReSharper restore InconsistentNaming
