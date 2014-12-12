using System.Configuration;
// ReSharper disable InconsistentNaming
using System.Linq;
using FluentAssertions;
using NUnit.Framework;
using SmartElk.Antler.Core;
using SmartElk.Antler.Core.Abstractions.Configuration;
using SmartElk.Antler.Core.Common.Reflection;
using SmartElk.Antler.Core.Domain;
using SmartElk.Antler.EntityFramework.Configuration;
using SmartElk.Antler.Linq2Db.Configuration;
using SmartElk.Antler.Linq2Db.SqlServer.Specs.Entities;
using SmartElk.Antler.Specs.Shared.EntityFramework.Mappings;
using SmartElk.Antler.Windsor;

namespace SmartElk.Antler.Linq2Db.SqlServer.Specs
{
    /***You need to have "AntlerTest" database in your SQL SERVER. See connection string below***/
    
    public class DomainSpecs
    {                
        [TestFixture]
        [Category("Integration")]                        
        public class when_trying_to_get_one_employee : TestingScenario
        {
            [Test]
            public void should_return_employee()
            {                                
                //arrange                
                Employee employee2 = null;                
                UnitOfWork.Do(uow =>
                    {                                                                    
                    var team = new Team() { Name = "Super", Description = "SuperBg"};
                    team.Id = (int)uow.Repo<Team>().Insert<decimal>(team);

                    var employee1 = new Employee { Id = "667", FirstName = "Jack", LastName = "Black" };
                    uow.Repo<Employee>().Insert(employee1);

                    employee2 = new Employee { Id = "666", FirstName = "John", LastName = "Smith"};
                    uow.Repo<Employee>().Insert(employee2);

                    var teamEmployee2Map = new TeamEmployeeMap() { TeamId = team.Id, EmployeeId = employee2.Id};
                    uow.Repo<TeamEmployeeMap>().Insert(teamEmployee2Map);
                });
                
                UnitOfWork.Do(uow =>
                {
                    //act                    
                    var resultEmployee = uow.Repo<Employee>().AsQueryable().FirstOrDefault(t => t.Id == employee2.Id);

                    //assert
                    resultEmployee.Id.Should().Be(employee2.Id);
                    resultEmployee.FirstName.Should().Be(employee2.FirstName);
                    resultEmployee.LastName.Should().Be(employee2.LastName);                    
                });
            }
        }

        [TestFixture]
        [Category("Integration")]
        public class when_trying_to_get_all_teams : TestingScenario
        {
            [Test]
            public void should_return_all_teams()
            {
                UnitOfWork.Do(uow =>
                {
                    //arrange                    
                    var team1 = new Team { Name = "Super", Description = "SuperBg" };
                    uow.Repo<Team>().Insert(team1);

                    var team2 = new Team { Name = "Good", Description = "GoodBg" };
                    uow.Repo<Team>().Insert(team2);

                    var team3 = new Team { Name = "Bad", Description = "BadBg" };
                    uow.Repo<Team>().Insert(team3);
                });

                UnitOfWork.Do(uow =>
                {
                    //act                    
                    var result = uow.Repo<Team>().AsQueryable().ToArray();

                    //assert
                    result.Length.Should().Be(3);
                    result[0].Id.Should().Be(1);
                    result[0].Name.Should().Be("Super");
                    result[0].Description.Should().Be("SuperBg");
                    result[1].Id.Should().Be(2);
                    result[1].Name.Should().Be("Good");
                    result[1].Description.Should().Be("GoodBg");
                    result[2].Id.Should().Be(3);
                    result[2].Name.Should().Be("Bad");
                    result[2].Description.Should().Be("BadBg"); 
                });
            }
        }

        [TestFixture]
        [Category("Integration")]
        public class when_trying_to_get_all_employees : TestingScenario
        {
            [Test]
            public void should_return_all_employees()
            {
                //arrange
                Team team = null;
                Employee employee1 = null;
                Employee employee2 = null;
                Employee employee3 = null;
                UnitOfWork.Do(uow =>
                {
                    team = new Team() { Name = "Super", Description = "SuperBg" };
                    team.Id = (int)uow.Repo<Team>().Insert<decimal>(team);

                    employee1 = new Employee { Id = "555", FirstName = "Jack", LastName = "Black" };
                    uow.Repo<Employee>().Insert(employee1);

                    employee2 = new Employee { Id = "666", FirstName = "John", LastName = "Smith" };
                    uow.Repo<Employee>().Insert(employee2);

                    employee3 = new Employee { Id = "777", FirstName = "Billy", LastName = "Bobby"};
                    uow.Repo<Employee>().Insert(employee3);

                    var teamEmployee2 = new TeamEmployeeMap {EmployeeId = employee2.Id, TeamId = team.Id};
                    uow.Repo<TeamEmployeeMap>().Insert(teamEmployee2);

                    var teamEmployee3 = new TeamEmployeeMap { EmployeeId = employee3.Id, TeamId = team.Id };
                    uow.Repo<TeamEmployeeMap>().Insert(teamEmployee3);
                });

                UnitOfWork.Do(uow =>
                {
                    //act                    
                    var resultEmployees = uow.Repo<Employee>().AsQueryable().OrderBy(t=>t.Id).ToArray();
                    var resultTeamEmployeeMaps = uow.Repo<TeamEmployeeMap>().AsQueryable().OrderBy(t=>t.EmployeeId).ToArray();

                    //assert
                    resultEmployees.Length.Should().Be(3);
                    
                    resultEmployees[0].Id.Should().Be(employee1.Id);
                    resultEmployees[0].FirstName.Should().Be(employee1.FirstName);
                    resultEmployees[0].LastName.Should().Be(employee1.LastName);
                    resultTeamEmployeeMaps.Count(t => t.EmployeeId == resultEmployees[0].Id).Should().Be(0);
                                        
                    resultEmployees[1].Id.Should().Be(employee2.Id);
                    resultEmployees[1].FirstName.Should().Be(employee2.FirstName);
                    resultEmployees[1].LastName.Should().Be(employee2.LastName);
                    resultTeamEmployeeMaps.First(t => t.EmployeeId == resultEmployees[1].Id).TeamId.Should().Be(team.Id);

                    resultEmployees[2].Id.Should().Be(employee3.Id);
                    resultEmployees[2].FirstName.Should().Be(employee3.FirstName);
                    resultEmployees[2].LastName.Should().Be(employee3.LastName);
                    resultTeamEmployeeMaps.First(t => t.EmployeeId == resultEmployees[2].Id).TeamId.Should().Be(team.Id);
                });
            }
        }

        [TestFixture]
        [Category("Integration")]
        public class when_trying_to_find_employee_by_name : TestingScenario
        {
            [Test]
            public void should_return_employee()
            {
                //arrange
                Team team = null;                
                Employee employee2 = null;                
                UnitOfWork.Do(uow =>
                {
                    team = new Team() { Name = "Super", Description = "SuperBg" };
                    team.Id = (int)uow.Repo<Team>().Insert<decimal>(team);

                    var employee1 = new Employee { Id = "555", FirstName = "Jack", LastName = "Black" };
                    uow.Repo<Employee>().Insert(employee1);

                    employee2 = new Employee { Id = "666", FirstName = "John", LastName = "Smith" };
                    uow.Repo<Employee>().Insert(employee2);

                    var employee3 = new Employee { Id = "777", FirstName = "Billy", LastName = "Bobby" };
                    uow.Repo<Employee>().Insert(employee3);

                    var teamEmployee2 = new TeamEmployeeMap { EmployeeId = employee2.Id, TeamId = team.Id };
                    uow.Repo<TeamEmployeeMap>().Insert(teamEmployee2);

                    var teamEmployee3 = new TeamEmployeeMap { EmployeeId = employee3.Id, TeamId = team.Id };
                    uow.Repo<TeamEmployeeMap>().Insert(teamEmployee3);
                });

                UnitOfWork.Do(uow =>
                {
                    //act                                        
                    var result = (from emp in uow.Repo<Employee>().AsQueryable()
                                 join teamEmp in uow.Repo<TeamEmployeeMap>().AsQueryable() on emp.Id equals teamEmp.EmployeeId
                                  where emp.FirstName=="John"
                                  select new {Emp = emp, TeamEmp=teamEmp}).ToList();   


                    //assert                                        
                    result.Count.Should().Be(1);
                    result[0].Emp.Id.Should().Be(employee2.Id);
                    result[0].Emp.FirstName.Should().Be(employee2.FirstName);
                    result[0].Emp.LastName.Should().Be(employee2.LastName);                    
                    result[0].TeamEmp.TeamId.Should().Be(team.Id);                    
                });
            }
        }

        [TestFixture]
        [Category("Integration")]
        public class when_trying_to_modify_employees_teams : TestingScenario
        {
            [Test]
            public void should_modify_teams()
            {
                //arrange
                Employee employee = null;
                UnitOfWork.Do(uow =>
                {
                    var team1 = new Team() { Name = "Super", Description = "SuperBg" };
                    team1.Id = (int)uow.Repo<Team>().Insert<decimal>(team1);

                    var team2 = new Team() { Name = "Great", Description = "GreatBg" };
                    team2.Id = (int)uow.Repo<Team>().Insert<decimal>(team2);

                    employee = new Employee { Id = "666", FirstName = "John", LastName = "Smith" };
                    uow.Repo<Employee>().Insert(employee);

                    var team1Employee = new TeamEmployeeMap { EmployeeId = employee.Id, TeamId = team1.Id };
                    uow.Repo<TeamEmployeeMap>().Insert(team1Employee);

                    var team2Employee = new TeamEmployeeMap { EmployeeId = employee.Id, TeamId = team2.Id };
                    uow.Repo<TeamEmployeeMap>().Insert(team2Employee);
                });

                //act
                UnitOfWork.Do(uow =>
                {

                    var foundEmployeeTeams = (from emp in uow.Repo<Employee>().AsQueryable()
                                join teamEmp in uow.Repo<TeamEmployeeMap>().AsQueryable() on emp.Id equals teamEmp.EmployeeId
                                join team in uow.Repo<Team>().AsQueryable() on teamEmp.TeamId equals  team.Id                                
                                where emp.Id==employee.Id
                                select team).ToList();

                    foundEmployeeTeams[0].Name = "Super-upd";
                    foundEmployeeTeams[0].Description = "SuperBg-upd";
                    foundEmployeeTeams[1].Name = "Great-upd";
                    foundEmployeeTeams[1].Description = "GreatBg-upd";

                    uow.Repo<Team>().Update(foundEmployeeTeams[0]);
                    uow.Repo<Team>().Update(foundEmployeeTeams[1]);

                    var newTeam = new Team() { Name = "Awesome", Description = "AwesomeBg" };
                    newTeam.Id = (int)uow.Repo<Team>().Insert<decimal>(newTeam);

                    var newTeamEmployee = new TeamEmployeeMap { EmployeeId = employee.Id, TeamId = newTeam.Id };
                    uow.Repo<TeamEmployeeMap>().Insert(newTeamEmployee);
                });

                //assert
                UnitOfWork.Do(uow =>
                {
                    var resultEmployeeTeams = (from emp in uow.Repo<Employee>().AsQueryable()
                                              join teamEmp in uow.Repo<TeamEmployeeMap>().AsQueryable() on emp.Id equals teamEmp.EmployeeId
                                              join team in uow.Repo<Team>().AsQueryable() on teamEmp.TeamId equals team.Id
                                              where emp.Id == employee.Id
                                              orderby team.Id
                                              select team).ToList();

                    resultEmployeeTeams.Count.Should().Be(3);
                    resultEmployeeTeams[0].Name.Should().Be("Super-upd");
                    resultEmployeeTeams[0].Description.Should().Be("SuperBg-upd");
                    resultEmployeeTeams[1].Name.Should().Be("Great-upd");
                    resultEmployeeTeams[1].Description.Should().Be("GreatBg-upd");
                    resultEmployeeTeams[2].Name.Should().Be("Awesome");
                    resultEmployeeTeams[2].Description.Should().Be("AwesomeBg");                    
                });
            }
        }

        [TestFixture]
        [Category("Integration")]
        public class when_trying_to_find_team_by_country_name : TestingScenario
        {
            [Test]
            public void should_find_team()
            {
                //arrange
                Team team2 = null;
                UnitOfWork.Do(uow =>
                {
                    var country1 = new Country { Name = "USA", Language = "English" };
                    country1.Id = (int)uow.Repo<Country>().Insert<decimal>(country1);

                    var country2 = new Country { Name = "Mexico", Language = "Spanish" };
                    country2.Id = (int)uow.Repo<Country>().Insert<decimal>(country2);

                    var team1 = new Team() { Name = "Super", Description = "SuperBg", CountryId = country1.Id};
                    team1.Id = (int)uow.Repo<Team>().Insert<decimal>(team1);

                    team2 = new Team() { Name = "Awesome", Description = "AwesomeBg", CountryId = country2.Id };
                    team2.Id = (int)uow.Repo<Team>().Insert<decimal>(team2);
                });

                UnitOfWork.Do(uow =>
                {
                    //act                    
                    var result = (from team in uow.Repo<Team>().AsQueryable()
                                  join country in uow.Repo<Country>().AsQueryable() on team.CountryId equals country.Id
                                  where country.Name == "Mexico"
                                  select team).ToList();

                    //assert
                    result.Count.Should().Be(1);
                    result[0].Id.Should().Be(team2.Id);
                    result[0].Name.Should().Be("Awesome");
                    result[0].Description.Should().Be("AwesomeBg");                    
                });
            }
        }

        [TestFixture]
        [Category("Integration")]
        public class when_trying_to_delete_team : TestingScenario
        {
            [Test]
            public void should_delete_team()
            {
                //arrange
                Team team = null;
                UnitOfWork.Do(uow =>
                {
                    team = new Team() { Name = "Super", Description = "SuperBg" };
                    team.Id = (int)uow.Repo<Team>().Insert<decimal>(team);
                });

                UnitOfWork.Do(uow => uow.Repo<Team>().AsQueryable().FirstOrDefault(t => t.Id==team.Id).Should().NotBeNull());

                UnitOfWork.Do(uow =>
                {
                    //act                    
                    uow.Repo<Team>().Delete(team);

                    //assert
                    var foundTeam = uow.Repo<Team>().AsQueryable().FirstOrDefault(t => t.Id == team.Id);
                    foundTeam.Should().BeNull();
                });
            }
        }

        [TestFixture]
        [Category("Integration")]
        public class when_trying_to_rollback_transaction : TestingScenario
        {
            [Test]
            public void should_rollback()
            {
                //arrange
                Team team = null;
                UnitOfWork.Do(uow =>
                {
                    team = new Team() { Name = "Super", Description = "SuperBg" };
                    uow.Repo<Team>().Insert(team);

                    uow.Rollback();
                });

                UnitOfWork.Do(uow =>
                {
                    //assert
                    var foundTeam = uow.Repo<Team>().AsQueryable().FirstOrDefault(t => t.Name == "Super");
                    foundTeam.Should().BeNull();
                });
            }
        }
        
        [TestFixture]
        [Category("Integration")]
        public class when_trying_to_insert_new_team : TestingScenario
        {
            [Test]
            public void should_return_generated_id()
            {
                UnitOfWork.Do(uow =>
                {
                    //arrange
                    var team1 = new Team { Name = "SuperTeam", Description = "Really super" };

                    //act                                                    
                    var result = uow.Repo<Team>().Insert<decimal>(team1);

                    //assert
                    result.Should().BeGreaterThan(0);
                });
            }
        }
                        
        #region Configuration        
        public class TestingScenario
        {
            protected IAntlerConfigurator Configurator { get; set; }

            [SetUp]
            public void SetUp()
            {
                Configurator = new AntlerConfigurator();
                
                var connectionString = ConfigurationManager.AppSettings["ConnectionString"];                
                Configurator.UseWindsorContainer()                            
                            .UseStorage(EntityFrameworkStorage.Use.WithConnectionString(connectionString)
                                                                   .WithMappings(From.AssemblyWithType<CountryMap>().First())
                                                                   .WithRecreatedDatabase(true), "JustToGenerateStuff")
                            .UseStorage(Linq2DbStorage.Use(connectionString));

            }
            
            [TearDown]
            public void Dispose()
            {
                Configurator.UnUseContainer().UnUseStorage().Dispose();
            }
        }
 
        #endregion
    }
}

// ReSharper restore InconsistentNaming