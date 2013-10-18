// ReSharper disable InconsistentNaming
using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using System.Reflection;
using FluentAssertions;
using NUnit.Framework;
using SmartElk.Antler.Abstractions.Configuration;
using SmartElk.Antler.Domain;
using SmartElk.Antler.Domain.Configuration;
using SmartElk.Antler.EntityFramework.Sqlite.Configuration;
using SmartElk.Antler.Specs.Shared.CommonSpecs;
using SmartElk.Antler.Specs.Shared.Entities;
using SmartElk.Antler.Windsor;

namespace SmartElk.Antler.EntityFramework.Sqlite.Specs
{
    public class DomainSpecs
    {                                                        
        [TestFixture]
        [Category("Integration")]
        public class when_trying_to_get_one_employee : TestingScenario<LazyLoading>
        {
            [Test]
            public void should_return_employee()
            {
                CommonDomainSpecs.when_trying_to_get_one_employee.should_return_employee();
            }
        }

        [TestFixture]
        [Category("Integration")]
        public class when_trying_to_get_all_teams : TestingScenario<LazyLoading>
        {
            [Test]
            public void should_return_all_teams()
            {
                CommonDomainSpecs.when_trying_to_get_all_teams.should_return_all_teams();
            }
        }

        [TestFixture]
        [Category("Integration")]
        public class when_trying_to_get_all_employees : TestingScenario<LazyLoading>
        {
            [Test]
            public static void should_return_all_employees()
            {
                CommonDomainSpecs.when_trying_to_get_all_employees.should_return_all_employees();
            }
        }

        [TestFixture]
        [Category("Integration")]
        public class when_trying_to_find_employee_by_name : TestingScenario<LazyLoading>
        {
            [Test]
            public static void should_return_employee()
            {
                CommonDomainSpecs.when_trying_to_find_employee_by_name.should_return_employee();
            }
        }

        [TestFixture]
        [Category("Integration")]
        public class when_trying_to_modify_employees_teams : TestingScenario<LazyLoading>
        {
            [Test]
            public static void should_modify_teams()
            {
                CommonDomainSpecs.when_trying_to_modify_employees_teams.should_modify_teams();
            }
        }

        [TestFixture]
        [Category("Integration")]
        public class when_trying_to_get_one_employee_without_lazy_loading : TestingScenario<EagerLoading>
        {
            [Test]
            public static void should_return_employee()
            {
                //arrange
                Team team;
                Employee employee2;
                using (var uow = new UnitOfWork())
                {
                    team = new Team() { Name = "Super", BusinessGroup = "SuperBg" };
                    uow.Repository<Team>().Insert(team);

                    var employee1 = new Employee { Id = "667", FirstName = "Jack", LastName = "Black" };
                    uow.Repository<Employee>().Insert(employee1);

                    employee2 = new Employee { Id = "666", FirstName = "John", LastName = "Smith", Teams = new List<Team>() { team } };
                    uow.Repository<Employee>().Insert(employee2);
                }

                using (var uow = new UnitOfWork())
                {
                    //act                    
                    var result = uow.Repository<Employee>().AsQueryable().Where(t => t.Id == employee2.Id).Include(t => t.Teams).First();

                    //assert
                    result.Id.Should().Be(employee2.Id);
                    result.FirstName.Should().Be(employee2.FirstName);
                    result.LastName.Should().Be(employee2.LastName);
                    result.Teams.First().Id.Should().Be(team.Id);
                    result.Teams.First().Name.Should().Be(team.Name);
                    result.Teams.First().BusinessGroup.Should().Be(team.BusinessGroup);
                }
            }
        }

        #region Configuration
        public class LazyLoading { }
        public class EagerLoading { }
        public class TestingScenario<T>
        {
            protected IAntlerConfigurator Configurator { get; set; }

            [SetUp]
            public void SetUp()
            {
                Configurator = new AntlerConfigurator();
                var configurator = Configurator.UseWindsorContainer().UseDomain().WithMappings(Assembly.GetExecutingAssembly());

                if (typeof(T) == typeof(LazyLoading))
                    configurator.AsInMemoryStorage();
                else
                    configurator.WithoutLazyLoading().AsInMemoryStorage();

                ((ISessionScopeFactoryEx)Configurator.Configuration.Container.Get<ISessionScopeFactory>()).CreateDataContext().Clear();
            }

            [TearDown]
            public void Dispose()
            {
                Configurator.Dispose();
            }
        } 
        #endregion
    }
}

// ReSharper restore InconsistentNaming