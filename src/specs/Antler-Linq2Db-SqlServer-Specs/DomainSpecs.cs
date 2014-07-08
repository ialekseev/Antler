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
    //todo: fix test and write remaining tests    
    public class DomainSpecs
    {                                                        
        [TestFixture]
        [Category("Integration")]                
        [Ignore]
        public class when_trying_to_get_one_employee : TestingScenario
        {
            [Test]
            public void should_return_employee()
            {                                
                //arrange
                Team team = null;
                Employee employee2 = null;                
                UnitOfWork.Do(uow =>
                    {                                                                    
                    team = new Team() { Name = "Super", Description = "SuperBg"};
                    uow.Repo<Team>().Insert(team);

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
                    var resultEmployee = uow.Repo<Employee>().GetById(employee2.Id);

                    //assert
                    resultEmployee.Id.Should().Be(employee2.Id);
                    resultEmployee.FirstName.Should().Be(employee2.FirstName);
                    resultEmployee.LastName.Should().Be(employee2.LastName);

                    var resultEmployeeTeams = uow.Repo<TeamEmployeeMap>().GetById(employee2.Id);
                    resultEmployeeTeams.TeamId.Should().Be(team.Id);                                        
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

                const string connectionString = "Data Source=.\\SQLEXPRESS;Initial Catalog=AntlerTest;Integrated Security=True";

                //todo: Why configuration does not work when UseStorage and UseStorageNamed are used in the opposite order? Fix problem(probably due to IoC container)
                Configurator.UseWindsorContainer()
                            .UseStorage(Linq2DbStorage.Use(connectionString))
                            .UseStorageNamed(EntityFrameworkStorage.Use.WithConnectionString(connectionString)
                                                                   .WithMappings(
                                                                       From.AssemblyWithType<CountryMap>().First())
                                                                   .WithRecreatedDatabase(), "JustToGenerateDatabase");

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