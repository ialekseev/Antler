// ReSharper disable InconsistentNaming
using System.Collections.Generic;
using System.Linq;
using FluentAssertions;
using SmartElk.Antler.Core.Domain;
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
                Team team = null;                
                Employee employee2 = null;
                UnitOfWork.Do(uow =>
                    {
                        team = new Team() { Name = "Super", BusinessGroup = "SuperBg" };
                        uow.Repo<Team>().Insert(team);

                        var employee1 = new Employee { Id = "667", FirstName = "Jack", LastName = "Black" };
                        uow.Repo<Employee>().Insert(employee1);

                        employee2 = new Employee { Id = "666", FirstName = "John", LastName = "Smith", Teams = new List<Team>() { team } };
                        uow.Repo<Employee>().Insert(employee2);
                    });

                UnitOfWork.Do(uow =>
                    {
                        //act                    
                        var result = uow.Repo<Employee>().GetById(employee2.Id);

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
        
        public static class when_trying_to_get_all_teams
        {            
            public static void should_return_all_teams()
            {
                UnitOfWork.Do(uow =>
                    {
                        //arrange                    
                        var team1 = new Team() {Name = "Super", BusinessGroup = "SuperBg"};
                        uow.Repo<Team>().Insert(team1);

                        var team2 = new Team() {Name = "Good", BusinessGroup = "GoodBg"};
                        uow.Repo<Team>().Insert(team2);

                        var team3 = new Team() {Name = "Bad", BusinessGroup = "BadBg"};
                        uow.Repo<Team>().Insert(team3);
                    });

                UnitOfWork.Do(uow =>
                    {
                        //act                    
                        var result = uow.Repo<Team>().AsQueryable().OrderBy(t => t.Name).ToArray();

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
                    });
            }
        }

        public static class when_trying_to_get_all_employees
        {
            public static void should_return_all_employees()
            {
                //arrange
                Team team = null;
                Employee employee1 = null;
                Employee employee2 = null;
                Employee employee3 = null;
                UnitOfWork.Do(uow =>
                    {
                        team = new Team() { Name = "Super", BusinessGroup = "SuperBg" };
                        uow.Repo<Team>().Insert(team);

                        employee1 = new Employee { Id = "667", FirstName = "Jack", LastName = "Black" };
                        uow.Repo<Employee>().Insert(employee1);

                        employee2 = new Employee { Id = "666", FirstName = "John", LastName = "Smith", Teams = new List<Team>() { team } };
                        uow.Repo<Employee>().Insert(employee2);

                        employee3 = new Employee { Id = "77", FirstName = "Billy", LastName = "Bobby", Teams = new List<Team>() { team } };
                        uow.Repo<Employee>().Insert(employee3);
                    });

                UnitOfWork.Do(uow =>
                    {
                        //act                    
                        var result = uow.Repo<Employee>().AsQueryable().OrderBy(t => t.FirstName).ToArray();

                        //assert
                        result.Length.Should().Be(3);
                        result[0].Id.Should().Be(employee3.Id);
                        result[0].FirstName.Should().Be(employee3.FirstName);
                        result[0].LastName.Should().Be(employee3.LastName);
                        result[0].Teams.First().Id.Should().Be(team.Id);
                        result[0].Teams.First().Name.Should().Be(team.Name);
                        result[0].Teams.First().BusinessGroup.Should().Be(team.BusinessGroup);
                        result[1].Id.Should().Be(employee1.Id);
                        result[1].FirstName.Should().Be(employee1.FirstName);
                        result[1].LastName.Should().Be(employee1.LastName);
                        result[1].Teams.Count.Should().Be(0);
                        result[2].Id.Should().Be(employee2.Id);
                        result[2].FirstName.Should().Be(employee2.FirstName);
                        result[2].LastName.Should().Be(employee2.LastName);
                        result[2].Teams.First().Id.Should().Be(team.Id);
                        result[2].Teams.First().BusinessGroup.Should().Be(team.BusinessGroup);
                    });
            }
        }

        public static class when_trying_to_find_employee_by_name
        {
            public static void should_return_employee()
            {
                //arrange
                Team team = null;
                Employee employee2 = null;
                UnitOfWork.Do(uow =>
                    {
                        team = new Team() { Name = "Super", BusinessGroup = "SuperBg" };
                        uow.Repo<Team>().Insert(team);

                        var employee1 = new Employee { Id = "667", FirstName = "Jack", LastName = "Black" };
                        uow.Repo<Employee>().Insert(employee1);

                        employee2 = new Employee { Id = "666", FirstName = "John", LastName = "Smith", Teams = new List<Team>() { team } };
                        uow.Repo<Employee>().Insert(employee2);

                        var employee3 = new Employee { Id = "77", FirstName = "Billy", LastName = "Bobby", Teams = new List<Team>() { team } };
                        uow.Repo<Employee>().Insert(employee3);
                    });

                UnitOfWork.Do(uow =>
                    {
                        //act                    
                        var result = uow.Repo<Employee>().AsQueryable().First(t => t.FirstName == "John");

                        //assert
                        result.Id.Should().Be(employee2.Id);
                        result.FirstName.Should().Be(employee2.FirstName);
                        result.LastName.Should().Be(employee2.LastName);
                        result.Teams.First().Id.Should().Be(team.Id);
                        result.Teams.First().BusinessGroup.Should().Be(team.BusinessGroup);
                    });
            }
        }

        public static class when_trying_to_modify_employees_teams
        {
            public static void should_modify_teams()
            {
                //arrange
                Employee employee = null;
                UnitOfWork.Do(uow =>
                    {
                        var team1 = new Team() { Name = "Super", BusinessGroup = "SuperBg" };
                        uow.Repo<Team>().Insert(team1);

                        var team2 = new Team() { Name = "Great", BusinessGroup = "GreatBg" };
                        uow.Repo<Team>().Insert(team2);

                        employee = new Employee { Id = "666", FirstName = "John", LastName = "Smith", Teams = new List<Team>() { team1, team2 } };
                        uow.Repo<Employee>().Insert(employee);
                    });

                //act
                UnitOfWork.Do(uow =>
                    {
                        var foundEmployee = uow.Repo<Employee>().GetById(employee.Id);
                        foundEmployee.Teams[0].Name = "Super-upd";
                        foundEmployee.Teams[0].BusinessGroup = "SuperBg-upd";
                        foundEmployee.Teams[1].Name = "Great-upd";
                        foundEmployee.Teams[1].BusinessGroup = "GreatBg-upd";

                        var newTeam = new Team() {Name = "Awesome", BusinessGroup = "AwesomeBg"};
                        uow.Repo<Team>().Insert(newTeam);
                        foundEmployee.Teams.Add(newTeam);
                    });

                //assert
                UnitOfWork.Do(uow =>
                    {
                        var foundEmployee = uow.Repo<Employee>().GetById(employee.Id);

                        foundEmployee.Teams.Count.Should().Be(3);
                        foundEmployee.Teams[0].Name.Should().Be("Super-upd");
                        foundEmployee.Teams[0].BusinessGroup.Should().Be("SuperBg-upd");
                        foundEmployee.Teams[1].Name.Should().Be("Great-upd");
                        foundEmployee.Teams[1].BusinessGroup.Should().Be("GreatBg-upd");
                        foundEmployee.Teams[2].Name.Should().Be("Awesome");
                        foundEmployee.Teams[2].BusinessGroup.Should().Be("AwesomeBg");
                    });
            }
        }

        public static class when_trying_to_find_team_by_country_name
        {
            public static void should_return_country()
            {
                //arrange
                Team team2 = null;
                UnitOfWork.Do(uow =>
                    {
                        var country1 = new Country {Name = "USA", Language = "English"};
                        uow.Repo<Country>().Insert(country1);

                        var country2 = new Country {Name = "Mexico", Language = "Spanish"};
                        uow.Repo<Country>().Insert(country2);

                        var team1 = new Team() {Name = "Super", BusinessGroup = "SuperBg", Country = country1};
                        uow.Repo<Team>().Insert(team1);

                        team2 = new Team() {Name = "Awesome", BusinessGroup = "AwesomeBg", Country = country2};
                        uow.Repo<Team>().Insert(team2);
                    });

                UnitOfWork.Do(uow =>
                    {
                        //act                    
                        var result = uow.Repo<Team>().AsQueryable().First(t => t.Country.Name == "Mexico");

                        //assert
                        result.Id.Should().Be(team2.Id);
                        result.Name.Should().Be("Awesome");
                        result.BusinessGroup.Should().Be("AwesomeBg");
                        result.Country.Name.Should().Be("Mexico");
                    });
            }
        }

    }
}
// ReSharper restore InconsistentNaming