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
using SmartElk.Antler.EntityFramework.SqlCe.Configuration;
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
        public class when_trying_to_find_team_by_country_name : TestingScenario<LazyLoading>
        {
            [Test]
            public void should_return_country()
            {
                CommonDomainSpecs.when_trying_to_find_team_by_country_name.should_return_country();
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
                Team team = null;
                Employee employee2 = null;
                UnitOfWork.Do(uow =>
                    {
                        team = new Team() { Name = "Super", BusinessGroup = "SuperBg" };
                        uow.Repository<Team>().Insert(team);

                        var employee1 = new Employee { Id = "667", FirstName = "Jack", LastName = "Black" };
                        uow.Repository<Employee>().Insert(employee1);

                        employee2 = new Employee { Id = "666", FirstName = "John", LastName = "Smith", Teams = new List<Team>() { team } };
                        uow.Repository<Employee>().Insert(employee2);
                    });

                UnitOfWork.Do(uow =>
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
                    });
            }
        }

        [TestFixture]
        [Category("Integration")]
        public class when_trying_to_find_team_by_country_name_without_lazy_loading : TestingScenario<EagerLoading>
        {
            [Test]
            public void should_return_country()
            {
                //arrange
                Team team2 = null;
                UnitOfWork.Do(uow =>
                    {
                        var country1 = new Country {Name = "USA", Language = "English"};
                        uow.Repository<Country>().Insert(country1);

                        var country2 = new Country {Name = "Mexico", Language = "Spanish"};
                        uow.Repository<Country>().Insert(country2);

                        Team team1 = new Team() {Name = "Super", BusinessGroup = "SuperBg", Country = country1};
                        uow.Repository<Team>().Insert(team1);

                        team2 = new Team() {Name = "Awesome", BusinessGroup = "AwesomeBg", Country = country2};
                        uow.Repository<Team>().Insert(team2);
                    });

                UnitOfWork.Do(uow =>
                    {
                        //act                    
                        var result = uow.Repository<Team>().AsQueryable().Include(t => t.Country).First(t => t.Country.Name == "Mexico");

                        //assert
                        result.Id.Should().Be(team2.Id);
                        result.Name.Should().Be("Awesome");
                        result.BusinessGroup.Should().Be("AwesomeBg");
                        result.Country.Name.Should().Be("Mexico");
                    });
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

                Configurator.UseWindsorContainer()
                            .UseStorage(typeof (T) == typeof (LazyLoading)
                                            ? EntityFrameworkPlusSqlCe.Use.WithConnectionString("Data Source=TestDB.sdf")
                                                                      .WithMappings(Assembly.GetExecutingAssembly())
                                            : EntityFrameworkPlusSqlCe.Use.WithoutLazyLoading()
                                                                      .WithConnectionString("Data Source=TestDB.sdf")
                                                                      .WithMappings(Assembly.GetExecutingAssembly()));

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