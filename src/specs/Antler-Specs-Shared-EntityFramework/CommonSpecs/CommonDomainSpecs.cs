// ReSharper disable InconsistentNaming

using System.Collections.Generic;
using System.Data.Entity;
using System.Linq;
using FluentAssertions;
using SmartElk.Antler.Core.Domain;
using SmartElk.Antler.Specs.Shared.Entities;

namespace SmartElk.Antler.Specs.Shared.EntityFramework.CommonSpecs
{
    public static class CommonDomainSpecs
    {
        public static class when_trying_to_get_one_employee_without_lazy_loading 
        {
            public static void should_return_employee()
            {
                //arrange
                Team team = null;
                Employee employee2 = null;
                UnitOfWork.Do(uow =>
                {
                    team = new Team() { Name = "Super", Description = "SuperBg" };
                    uow.Repo<Team>().Insert(team);

                    var employee1 = new Employee { Id = "667", FirstName = "Jack", LastName = "Black" };
                    uow.Repo<Employee>().Insert(employee1);

                    employee2 = new Employee { Id = "666", FirstName = "John", LastName = "Smith", Teams = new List<Team>() { team } };
                    uow.Repo<Employee>().Insert(employee2);
                });

                UnitOfWork.Do(uow =>
                {
                    //act                    
                    var result = uow.Repo<Employee>().AsQueryable().Where(t => t.Id == employee2.Id).Include(t => t.Teams).First();

                    //assert
                    result.Id.Should().Be(employee2.Id);
                    result.FirstName.Should().Be(employee2.FirstName);
                    result.LastName.Should().Be(employee2.LastName);
                    result.Teams.First().Id.Should().Be(team.Id);
                    result.Teams.First().Name.Should().Be(team.Name);
                    result.Teams.First().Description.Should().Be(team.Description);
                });
            }
        }
        
        public static class when_trying_to_find_team_by_country_name_without_lazy_loading
        {            
            public static void should_return_country()
            {
                //arrange
                Team team2 = null;
                UnitOfWork.Do(uow =>
                {
                    var country1 = new Country { Name = "USA", Language = "English" };
                    uow.Repo<Country>().Insert(country1);

                    var country2 = new Country { Name = "Mexico", Language = "Spanish" };
                    uow.Repo<Country>().Insert(country2);

                    Team team1 = new Team() { Name = "Super", Description = "SuperBg", Country = country1 };
                    uow.Repo<Team>().Insert(team1);

                    team2 = new Team() { Name = "Awesome", Description = "AwesomeBg", Country = country2 };
                    uow.Repo<Team>().Insert(team2);
                });

                UnitOfWork.Do(uow =>
                {
                    //act                    
                    var result = uow.Repo<Team>().AsQueryable().Include(t => t.Country).First(t => t.Country.Name == "Mexico");

                    //assert
                    result.Id.Should().Be(team2.Id);
                    result.Name.Should().Be("Awesome");
                    result.Description.Should().Be("AwesomeBg");
                    result.Country.Name.Should().Be("Mexico");
                });
            }
        }
        
    }
}
// ReSharper restore InconsistentNaming