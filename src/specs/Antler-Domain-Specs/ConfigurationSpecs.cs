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
        [Category("Unit")]
        public class when_trying_to_set_unnamed_storage_on_configurator: ConfigurationScenario
        {
            [Test]
            public void should_set_unit_of_work_and_perform_storage_configuring_without_name()
            {
                //arrange
                var configurator = A.Fake<IAntlerConfigurator>();
                var basicConfiguration = A.Fake<IBasicConfiguration>();
                var container = A.Fake<IContainer>();
                                
                A.CallTo(() => container.Get<ISessionScopeFactory>()).Returns(new TestSessionScopeFactory());
                A.CallTo(() => basicConfiguration.Container).Returns(container);
                A.CallTo(() => configurator.Configuration).Returns(basicConfiguration);

                var storage = A.Fake<IStorage>();

                //act
                configurator.UseStorage(storage);

                //assert
                var property = typeof(UnitOfWork).GetProperty("SessionScopeFactoryExtractor", BindingFlags.NonPublic | BindingFlags.Static);
                var sessionScopeFactoryExtractor = (Func<ISessionScopeFactory>)property.GetValue(null, null);
                var sessionScopeFactory = sessionScopeFactoryExtractor();
                
                sessionScopeFactory.Should().BeOfType<TestSessionScopeFactory>();
                A.CallTo(()=>storage.Configure(A<IDomainConfigurator>.That.Matches(t=>string.IsNullOrEmpty(t.Name)))).MustHaveHappened();
            }
        }

        [TestFixture]
        public class when_trying_to_set_named_storage_on_configurator : ConfigurationScenario
        {            
            [Test]
            public void should_set_unit_of_work_and_perform_storage_configuring_with_name()
            {
                //arrange
                var configurator = A.Fake<IAntlerConfigurator>();
                var basicConfiguration = A.Fake<IBasicConfiguration>();
                var container = A.Fake<IContainer>();

                A.CallTo(() => container.Get<ISessionScopeFactory>("SuperStorage")).Returns(new TestSessionScopeFactory());
                A.CallTo(() => basicConfiguration.Container).Returns(container);
                A.CallTo(() => configurator.Configuration).Returns(basicConfiguration);

                var storage = A.Fake<IStorage>();
                
                //act
                configurator.UseStorageNamed(storage, "SuperStorage");

                //assert
                var property = typeof(UnitOfWork).GetProperty("SessionScopeFactoryNamedExtractor", BindingFlags.NonPublic | BindingFlags.Static);
                var sessionScopeFactoryExtractor = (Func<string, ISessionScopeFactory>)property.GetValue(null, null);
                var sessionScopeFactory = sessionScopeFactoryExtractor("SuperStorage");
                
                sessionScopeFactory.Should().BeOfType<TestSessionScopeFactory>();
                A.CallTo(() => storage.Configure(A<IDomainConfigurator>.That.Matches(t => t.Name.Equals("SuperStorage")))).MustHaveHappened();
            }
        }
    }
}
// ReSharper restore InconsistentNaming
