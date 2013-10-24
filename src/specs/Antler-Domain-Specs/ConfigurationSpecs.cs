// ReSharper disable InconsistentNaming
using System;
using System.Reflection;
using FakeItEasy;
using FluentAssertions;
using NUnit.Framework;
using SmartElk.Antler.Abstractions;
using SmartElk.Antler.Abstractions.Configuration;
using SmartElk.Antler.Domain.Configuration;

namespace SmartElk.Antler.Domain.Specs
{
    public class ConfigurationSpecs
    {
        public class ConfigurationScenario
        {
            protected class TestSessionScopeFactory : ISessionScopeFactory
            {
                public ISessionScope Open()
                {
                    return null;
                }
            }
        }
        
        [TestFixture]
        public class when_trying_to_set_storage_on_basic_configurator: ConfigurationScenario
        {
            [Test]
            public void should_set_unit_of_work_and_return_domain_configurator()
            {
                //arrange
                var basicConfigurator = A.Fake<IBasicConfigurator>();
                var basicConfiguration = A.Fake<IBasicConfiguration>();
                var container = A.Fake<IContainer>();
                                
                A.CallTo(() => container.Get<ISessionScopeFactory>()).Returns(new TestSessionScopeFactory());
                A.CallTo(() => basicConfiguration.Container).Returns(container);
                A.CallTo(() => basicConfigurator.Configuration).Returns(basicConfiguration);
                
                //act
                var result = basicConfigurator.UseStorage();

                //assert
                var property = typeof(UnitOfWork).GetProperty("SessionScopeFactoryExtractor", BindingFlags.NonPublic | BindingFlags.Static);
                var sessionScopeFactoryExtractor = (Func<ISessionScopeFactory>)property.GetValue(null, null);
                var sessionScopeFactory = sessionScopeFactoryExtractor();
                
                sessionScopeFactory.Should().BeOfType<TestSessionScopeFactory>();
                result.Should().BeOfType<DomainConfigurator>();
            }
        }

        [TestFixture]
        public class when_trying_to_set_named_storage_on_basic_configurator : ConfigurationScenario
        {            
            [Test]
            public void should_set_unit_of_work_and_return_domain_configurator()
            {
                //arrange
                var basicConfigurator = A.Fake<IBasicConfigurator>();
                var basicConfiguration = A.Fake<IBasicConfiguration>();
                var container = A.Fake<IContainer>();

                A.CallTo(() => container.Get<ISessionScopeFactory>("SuperStorage")).Returns(new TestSessionScopeFactory());
                A.CallTo(() => basicConfiguration.Container).Returns(container);
                A.CallTo(() => basicConfigurator.Configuration).Returns(basicConfiguration);

                //act
                var result = basicConfigurator.UseNamedStorage("SuperStorage");

                //assert
                var property = typeof(UnitOfWork).GetProperty("SessionScopeFactoryExtractor", BindingFlags.NonPublic | BindingFlags.Static);
                var sessionScopeFactoryExtractor = (Func<ISessionScopeFactory>)property.GetValue(null, null);
                var sessionScopeFactory = sessionScopeFactoryExtractor();
                
                sessionScopeFactory.Should().BeOfType<TestSessionScopeFactory>();
                result.Should().BeOfType<DomainConfigurator>();
            }
        }
    }
}
// ReSharper restore InconsistentNaming
