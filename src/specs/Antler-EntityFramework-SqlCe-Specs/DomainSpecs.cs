// ReSharper disable InconsistentNaming
using System.Data.Entity;
using System.Reflection;
using NUnit.Framework;
using SmartElk.Antler.Abstractions.Configuration;
using SmartElk.Antler.Domain.Configuration;
using SmartElk.Antler.EntityFramework.Internal;
using SmartElk.Antler.Specs.Shared.CommonSpecs;
using SmartElk.Antler.Windsor;

namespace SmartElk.Antler.EntityFramework.Sqlite.Specs
{
    public class DomainSpecs
    {
        public class TestingScenario
        {
            private static IAntlerConfigurator Configurator { get; set; }

            static TestingScenario()
            {
                Configurator = new AntlerConfigurator();
                Configurator.UseWindsorContainer();
                Configurator.UseDomain().AsInMemoryStorage(Assembly.GetExecutingAssembly(), new DropCreateDatabaseAlways<DataContext>());
            }
        }

        [TestFixture]
        [Category("Integration")]
        public class when_trying_to_get_one_employee : TestingScenario
        {
            [Test]
            public void should_return_employee()
            {
                CommonDomainSpecs.when_trying_to_get_one_employee.should_return_employee();
            }
        }

        [TestFixture]
        [Category("Integration")]
        public class when_trying_to_get_all_teams : TestingScenario
        {
            [Test]
            public void should_return_all_teams()
            {
                CommonDomainSpecs.when_trying_to_get_all_teams.should_return_all_teams();
            }
        }
    }
}

// ReSharper restore InconsistentNaming