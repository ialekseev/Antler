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
        public static class when_trying_to_insert_employee
        {
            public static void should_insert()
            {
                //arrange
                Team team = null;
                Employee employee = null;
                UnitOfWork.Do(uow =>
                {
                    team = new Team() { Name = "Super", Description = "SuperBg" };
                    uow.Repo<Team>().Insert(team);
                    
                    employee = new Employee { Id = "666", FirstName = "John", LastName = "Smith", Teams = new List<Team>() { team } };
                    uow.Repo<Employee>().Insert(employee);
                });

                UnitOfWork.Do(uow =>
                {
                    //act                    
                    var result = uow.Repo<Employee>().GetById(employee.Id);

                    //assert
                    result.Id.Should().Be(employee.Id);
                    result.FirstName.Should().Be(employee.FirstName);
                    result.LastName.Should().Be(employee.LastName);
                    result.Teams.First().Id.Should().Be(team.Id);
                    result.Teams.First().Name.Should().Be(team.Name);
                    result.Teams.First().Description.Should().Be(team.Description);
                });
            }
        }
                
        public static class when_trying_to_get_one_employee
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
                        var result = uow.Repo<Employee>().GetById(employee2.Id);

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
        
        public static class when_trying_to_get_all_teams
        {            
            public static void should_return_all_teams()
            {
                UnitOfWork.Do(uow =>
                    {
                        //arrange                    
                        var team1 = new Team {Name = "Super", Description = "SuperBg"};
                        uow.Repo<Team>().Insert(team1);

                        var team2 = new Team {Name = "Good", Description = "GoodBg"};
                        uow.Repo<Team>().Insert(team2);

                        var team3 = new Team {Name = "Bad", Description = "BadBg"};
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
                        team = new Team() { Name = "Super", Description = "SuperBg" };
                        uow.Repo<Team>().Insert(team);

                        employee1 = new Employee { Id = "555", FirstName = "Jack", LastName = "Black" };
                        uow.Repo<Employee>().Insert(employee1);

                        employee2 = new Employee { Id = "666", FirstName = "John", LastName = "Smith", Teams = new List<Team>() { team } };
                        uow.Repo<Employee>().Insert(employee2);

                        employee3 = new Employee { Id = "777", FirstName = "Billy", LastName = "Bobby", Teams = new List<Team>() { team } };
                        uow.Repo<Employee>().Insert(employee3);
                    });

                UnitOfWork.Do(uow =>
                    {
                        //act                    
                        var result = uow.Repo<Employee>().AsQueryable().OrderBy(t => t.Id).ToArray();

                        //assert
                        result.Length.Should().Be(3);
                        result[0].Id.Should().Be(employee1.Id);
                        result[0].FirstName.Should().Be(employee1.FirstName);
                        result[0].LastName.Should().Be(employee1.LastName);
                        result[0].Teams.Count.Should().Be(0);
                        
                        result[1].Id.Should().Be(employee2.Id);
                        result[1].FirstName.Should().Be(employee2.FirstName);
                        result[1].LastName.Should().Be(employee2.LastName);
                        result[1].Teams.First().Id.Should().Be(team.Id);
                        result[1].Teams.First().Name.Should().Be(team.Name);
                        result[1].Teams.First().Description.Should().Be(team.Description);

                        result[2].Id.Should().Be(employee3.Id);
                        result[2].FirstName.Should().Be(employee3.FirstName);
                        result[2].LastName.Should().Be(employee3.LastName);
                        result[2].Teams.First().Id.Should().Be(team.Id);
                        result[2].Teams.First().Name.Should().Be(team.Name);
                        result[2].Teams.First().Description.Should().Be(team.Description);
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
                        team = new Team() { Name = "Super", Description = "SuperBg" };
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
                        result.Teams.First().Description.Should().Be(team.Description);
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
                        var team1 = new Team() { Name = "Super", Description = "SuperBg" };
                        uow.Repo<Team>().Insert(team1);

                        var team2 = new Team() { Name = "Great", Description = "GreatBg" };
                        uow.Repo<Team>().Insert(team2);

                        employee = new Employee { Id = "666", FirstName = "John", LastName = "Smith", Teams = new List<Team>() { team1, team2 } };
                        uow.Repo<Employee>().Insert(employee);
                    });

                //act
                UnitOfWork.Do(uow =>
                    {
                        var foundEmployee = uow.Repo<Employee>().GetById(employee.Id);
                        foundEmployee.Teams[0].Name = "Super-upd";
                        foundEmployee.Teams[0].Description = "SuperBg-upd";
                        foundEmployee.Teams[1].Name = "Great-upd";
                        foundEmployee.Teams[1].Description = "GreatBg-upd";

                        var newTeam = new Team() {Name = "Awesome", Description = "AwesomeBg"};
                        uow.Repo<Team>().Insert(newTeam);
                        foundEmployee.Teams.Add(newTeam);
                    });

                //assert
                UnitOfWork.Do(uow =>
                    {
                        var resultEmployee = uow.Repo<Employee>().GetById(employee.Id);

                        resultEmployee.Teams.Count.Should().Be(3);
                        resultEmployee.Teams[0].Name.Should().Be("Super-upd");
                        resultEmployee.Teams[0].Description.Should().Be("SuperBg-upd");
                        resultEmployee.Teams[1].Name.Should().Be("Great-upd");
                        resultEmployee.Teams[1].Description.Should().Be("GreatBg-upd");
                        resultEmployee.Teams[2].Name.Should().Be("Awesome");
                        resultEmployee.Teams[2].Description.Should().Be("AwesomeBg");
                    });
            }
        }

        public static class when_trying_to_find_team_by_country_name
        {
            public static void should_find_team()
            {
                //arrange
                Team team2 = null;
                UnitOfWork.Do(uow =>
                    {
                        var country1 = new Country {Name = "USA", Language = "English"};
                        uow.Repo<Country>().Insert(country1);

                        var country2 = new Country {Name = "Mexico", Language = "Spanish"};
                        uow.Repo<Country>().Insert(country2);

                        var team1 = new Team() {Name = "Super", Description = "SuperBg", Country = country1};
                        uow.Repo<Team>().Insert(team1);

                        team2 = new Team() {Name = "Awesome", Description = "AwesomeBg", Country = country2};
                        uow.Repo<Team>().Insert(team2);
                    });

                UnitOfWork.Do(uow =>
                    {
                        //act                    
                        var result = uow.Repo<Team>().AsQueryable().First(t => t.Country.Name == "Mexico");

                        //assert
                        result.Id.Should().Be(team2.Id);
                        result.Name.Should().Be("Awesome");
                        result.Description.Should().Be("AwesomeBg");
                        result.Country.Name.Should().Be("Mexico");
                    });
            }
        }

        public static class when_trying_to_delete_team
        {
            public static void should_delete_team()
            {
                //arrange
                Team team = null;
                UnitOfWork.Do(uow =>
                {
                    team = new Team() { Name = "Super", Description = "SuperBg" };
                    uow.Repo<Team>().Insert(team);
                });

                UnitOfWork.Do(uow => uow.Repo<Team>().GetById(team.Id).Should().NotBeNull());
                
                UnitOfWork.Do(uow =>
                {                    
                    //act                    
                    uow.Repo<Team>().Delete(team);

                    //assert
                    var foundTeam = uow.Repo<Team>().GetById(team.Id);
                    foundTeam.Should().BeNull();
                });
            }
        }
        
        public static class when_trying_to_delete_team_by_id
        {
            public static void should_delete_team()
            {
                //arrange
                Team team = null;
                UnitOfWork.Do(uow =>
                {
                    team = new Team() { Name = "Super", Description = "SuperBg" };
                    uow.Repo<Team>().Insert(team);                    
                });

                UnitOfWork.Do(uow => uow.Repo<Team>().GetById(team.Id).Should().NotBeNull());

                UnitOfWork.Do(uow =>
                    {                                          
                     //act                    
                     uow.Repo<Team>().Delete(team.Id);

                    //assert
                     var foundTeam = uow.Repo<Team>().GetById(team.Id);
                     foundTeam.Should().BeNull();
                    });
            }
        }

        public static class when_trying_to_update_detached_team
        {
            public static void should_updated_team()
            {
                //arrange
                Team team = null;
                UnitOfWork.Do(uow =>
                {
                    team = new Team() { Name = "Super", Description = "SuperBg" };
                    uow.Repo<Team>().Insert(team);
                });
                
                var detachedTeam = new Team() {Id = team.Id, Name = "Super-Duper", Description = "Duper"};
                
                //act
                UnitOfWork.Do(uow => uow.Repo<Team>().Update(detachedTeam));

                //assert
                UnitOfWork.Do(uow =>
                {                                        
                    var foundTeam = uow.Repo<Team>().GetById(team.Id);
                    foundTeam.Id.Should().Be(detachedTeam.Id);
                    foundTeam.Name.Should().Be("Super-Duper");
                    foundTeam.Description.Should().Be("Duper");
                });
            }
        }

        public static class when_trying_to_rollback_transaction
        {
            public static void should_rollback()
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
        
        public static class when_trying_to_insert_new_team
        {           
            public static void should_return_generated_id()
            {
                UnitOfWork.Do(uow =>
                {
                    //arrange
                    var team1 = new Team {Name = "SuperTeam", Description = "Really super"};

                    //act                                                    
                    var result = uow.Repo<Team>().Insert<int>(team1);

                    //assert
                    result.Should().BeGreaterThan(0);
                });
            }
        }
    }
}
// ReSharper restore InconsistentNaming