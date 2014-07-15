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
using SmartElk.Antler.Specs.Shared.CommonSpecs;
using SmartElk.Antler.Specs.Shared.EntityFramework.Mappings;
using SmartElk.Antler.Windsor;

namespace SmartElk.Antler.Linq2Db.SqlServer.Specs
{    
    //todo: write remaining tests    
    public class DomainSpecs
    {
        [TestFixture]
        [Category("Integration")]
        public class when_trying_to_insert_new_team : TestingScenario
        {
            [Test]
            public void should_return_generated_id()
            {
                CommonDomainSpecs.when_trying_to_insert_new_team.should_return_generated_id<Team, decimal>();
            }
        }
        
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
                
        #region Configuration        
        public class TestingScenario
        {
            protected IAntlerConfigurator Configurator { get; set; }

            [SetUp]
            public void SetUp()
            {
                Configurator = new AntlerConfigurator();

                const string connectionString = "Data Source=.\\SQLEXPRESS;Initial Catalog=AntlerTest;Integrated Security=True";
                
                Configurator.UseWindsorContainer()                            
                            .UseStorage(EntityFrameworkStorage.Use.WithConnectionString(connectionString)
                                                                   .WithMappings(From.AssemblyWithType<CountryMap>().First())
                                                                   .WithRecreatedDatabase(), "JustToGenerateDatabase")
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