// ReSharper disable InconsistentNaming
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using FluentAssertions;
using NUnit.Framework;
using SmartElk.Antler.Abstractions.Configuration;
using SmartElk.Antler.Domain;
using SmartElk.Antler.Domain.Configuration;
using SmartElk.Antler.Hibernate.Sqlite;
using SmartElk.Antler.Specs.Shared.Entities;
using SmartElk.Antler.Windsor;

namespace SmartElk.Antler.Hibernate.Specs
{
    public class DomainSpecs
    {
        public class TestingScenario
        {
            private IAntlerConfigurator Configurator { get; set; }

            public TestingScenario()
            {            
                Configurator = new AntlerConfigurator();                
                Configurator.UseWindsorContainer();
                Configurator.UseDomain().AsInMemoryStorage(Assembly.GetExecutingAssembly());                                                                              
            }
        }
        
        [TestFixture]
        [Category("Integration")]
        public class when_trying_to_get_one_employee : TestingScenario
        {
           [Test]
           public void should_return_employee()
           {
               using (var uow = new UnitOfWork())
               {                                      
                   //arrange
                   var teamRepository = uow.Repository<Team, int>();
                   var employeeRepository = uow.Repository<Employee, string>();

                   var team = new Team() { Id = 13, Name = "Super", BusinessGroup = "SuperBg" };                   
                   teamRepository.Insert(team);

                   var employee1 = new Employee { Id = "667", FirstName = "Jack", LastName = "Black" };
                   employeeRepository.Insert(employee1);
                                      
                   var employee2 = new Employee { Id = "666", FirstName = "John", LastName = "Smith", LineManager= employee1, Teams = new List<Team>() { team } };                   
                   employeeRepository.Insert(employee2);
                   
                   //act
                   var result = employeeRepository.GetById(employee2.Id);

                   //assert
                   result.Id.Should().Be(employee2.Id);
                   result.FirstName.Should().Be(employee2.FirstName);
                   result.LastName.Should().Be(employee2.LastName);
                   result.LineManager.Id.Should().Be(employee1.Id);
                   result.Teams.First().Id.Should().Be(team.Id);
                   result.Teams.First().Name.Should().Be(team.Name);
                   result.Teams.First().BusinessGroup.Should().Be(team.BusinessGroup);                                                         
               }                              
           }
        }

        [TestFixture]
        [Category("Integration")]
        public class when_trying_to_get_all_teams : TestingScenario
        {
            [Test]
            public void should_return_all_teams()
            {
                using (var uow = new UnitOfWork())
                {                                        
                    //arrange
                    var teamRepository = uow.Repository<Team, int>();
                    
                    var team1 = new Team() { Name = "Super", BusinessGroup = "SuperBg" };                    
                    teamRepository.Insert(team1);

                    var team2 = new Team() { Name = "Good", BusinessGroup = "GoodBg" };
                    teamRepository.Insert(team2);

                    var team3 = new Team() { Name = "Bad", BusinessGroup = "BadBg" };
                    teamRepository.Insert(team3);
                                                            
                    //act
                    var result = teamRepository.AsQueryable().OrderBy(t=>t.Name).ToArray();

                    //assert
                    result[0].Id.Should().Be(3);
                    result[0].Name.Should().Be("Bad");
                    result[0].BusinessGroup.Should().Be("BadBg");
                    result[1].Id.Should().Be(2);
                    result[1].Name.Should().Be("Good");
                    result[1].BusinessGroup.Should().Be("GoodBg");
                    result[2].Id.Should().Be(1);
                    result[2].Name.Should().Be("Super");
                    result[2].BusinessGroup.Should().Be("SuperBg");                                                            
                }
            }
        }
    }
}

// ReSharper restore InconsistentNaming