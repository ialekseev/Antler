// ReSharper disable InconsistentNaming
using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using SmartElk.Antler.Domain;
using SmartElk.Antler.Specs.Shared.Entities;

namespace SmartElk.Antler.Specs.Shared.CommonSpecs
{
    public static class CommonDomainSpecs
    {        
        public static class when_trying_to_get_one_employee
        {            
            public static void should_return_employee()
            {
                //arrange
                Team team;                
                Employee employee2;                
                using (var uow = new UnitOfWork())
                {                    
                    var teamRepository = uow.Repository<Team, int>();
                    var employeeRepository = uow.Repository<Employee, string>();

                    team = new Team() { Id = 13, Name = "Super", BusinessGroup = "SuperBg" };
                    teamRepository.Insert(team);

                    var employee1 = new Employee { Id = "667", FirstName = "Jack", LastName = "Black" };
                    employeeRepository.Insert(employee1);

                    employee2 = new Employee { Id = "666", FirstName = "John", LastName = "Smith", Teams = new List<Team>() { team } };
                    employeeRepository.Insert(employee2);
                }

                using (var uow = new UnitOfWork())
                {
                    //act
                    var employeeRepository = uow.Repository<Employee, string>();
                    var result = employeeRepository.GetById(employee2.Id);

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
        
        public static class when_trying_to_get_all_teams
        {            
            public static void should_return_all_teams()
            {
                using (var uow = new UnitOfWork())
                {
                    //arrange
                    var teamRepository = uow.Repository<Team, int>();

                    var team1 = new Team() {Name = "Super", BusinessGroup = "SuperBg"};
                    teamRepository.Insert(team1);

                    var team2 = new Team() {Name = "Good", BusinessGroup = "GoodBg"};
                    teamRepository.Insert(team2);

                    var team3 = new Team() {Name = "Bad", BusinessGroup = "BadBg"};
                    teamRepository.Insert(team3);
                }
                
                using (var uow = new UnitOfWork())
                {
                    //act
                    var teamRepository = uow.Repository<Team, int>();
                    var result = teamRepository.AsQueryable().OrderBy(t => t.Name).ToArray();

                    //assert
                    result.Length.Should().Be(3);
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