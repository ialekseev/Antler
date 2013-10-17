// ReSharper disable InconsistentNaming
using System.Reflection;
using NUnit.Framework;
using SmartElk.Antler.Abstractions.Configuration;
using SmartElk.Antler.Domain;
using SmartElk.Antler.Domain.Configuration;
using SmartElk.Antler.EntityFramework.Sqlite.Configuration;
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
                Configurator.UseDomain().WithMappings(Assembly.GetExecutingAssembly()).AsInMemoryStorage();
            }

            [SetUp]
            public void SetUp()
            {                
                var sessionScopeFactory = (ISessionScopeFactoryEx)Configurator.Configuration.Container.Get<ISessionScopeFactory>();
                var dataContext = sessionScopeFactory.CreateDataContext();
                dataContext.Clear();
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

        [TestFixture]
        [Category("Integration")]
        public class when_trying_to_get_all_employees : TestingScenario
        {
            [Test]
            public static void should_return_all_employees()
            {
                CommonDomainSpecs.when_trying_to_get_all_employees.should_return_all_employees();
            }
        }

        [TestFixture]
        [Category("Integration")]
        public class when_trying_to_find_employee_by_name : TestingScenario
        {
            [Test]
            public static void should_return_employee()
            {
                CommonDomainSpecs.when_trying_to_find_employee_by_name.should_return_employee();
            }
        }

        [TestFixture]
        [Category("Integration")]
        public class when_trying_to_modify_employees_teams : TestingScenario
        {
            [Test]
            public static void should_modify_teams()
            {
                CommonDomainSpecs.when_trying_to_modify_employees_teams.should_modify_teams();
            }
        }
    }
}

// ReSharper restore InconsistentNaming