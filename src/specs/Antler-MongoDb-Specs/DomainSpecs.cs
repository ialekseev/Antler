// ReSharper disable InconsistentNaming
using System;
using System.Linq;
using FluentAssertions;
using MongoDB.Driver.Builders;
using NUnit.Framework;
using SmartElk.Antler.Core;
using SmartElk.Antler.Core.Abstractions.Configuration;
using SmartElk.Antler.Core.Domain;
using SmartElk.Antler.MongoDb.Configuration;
using SmartElk.Antler.Specs.Shared.CommonSpecs;
using SmartElk.Antler.Specs.Shared.Entities;
using SmartElk.Antler.Windsor;

namespace SmartElk.Antler.MongoDb.Specs
{
    /***You need to have MongoDb instance running(start it from {Antler-project-root}\tools\MongoDb). See connection string below.***/
    
    public class DomainSpecs
    {

        [TestFixture]
        [Category("Integration")]
        public class when_trying_to_insert_employee : TestingScenario
        {
            [Test]
            public void should_insert()
            {
                CommonDomainSpecs.when_trying_to_insert_employee.should_insert();
            }
        }

        [TestFixture]
        [Category("Integration")]
        public class when_trying_to_insert_or_update_existing_team : TestingScenario
        {
            [Test]
            public void should_update()
            {
                CommonDomainSpecs.when_trying_to_insert_or_update_existing_team.should_update();
            }
        }

        [TestFixture]
        [Category("Integration")]
        public class when_trying_to_insert_or_update_non_existing_team : TestingScenario
        {
            [Test]
            public void should_insert()
            {
                CommonDomainSpecs.when_trying_to_insert_or_update_non_existing_team.should_insert();
            }
        }

        [TestFixture]
        [Category("Integration")]        
        public class when_trying_to_get_one_employee : TestingScenario
        {                                              
           [Test]
           public void should_return_employee()
           {
               CommonDomainSpecs.when_trying_to_get_one_employee.should_return_employee();                  
           }            
        }
        
        [TestFixture]
        [Category("Integration")]        
        public class when_trying_to_get_all_employees : TestingScenario
        {
            [Test]
            public static void should_return_all_employees()
            {
                CommonDomainSpecs.when_trying_to_get_all_employees.should_return_all_employees();
            }
        }
                
        [TestFixture]
        [Category("Integration")]        
        public class when_trying_to_find_employee_by_name : TestingScenario
        {
            [Test]
            public static void should_return_employee()
            {
                CommonDomainSpecs.when_trying_to_find_employee_by_name.should_return_employee();
            }
        }
                                       
        [TestFixture]
        [Category("Integration")]        
        public class when_trying_to_rollback_transaction : TestingScenario
        {
            [Test]
            public void should_rollback()
            {
                CommonDomainSpecs.when_trying_to_rollback_transaction.should_rollback();
            }
        }

        [TestFixture]
        [Category("Integration")]
        public class when_throwing_exception_after_insert : TestingScenario
        {
            [Test]
            public void should_rollback_transaction()
            {
                CommonDomainSpecs.when_throwing_exception_after_insert.should_rollback_transaction();
            }
        }

        [TestFixture]
        [Category("Integration")]
        public class when_throwing_exception_from_nested_uof_after_inserting_in_root_uof : TestingScenario
        {
            [Test]
            public void should_rollback_everything()
            {
                CommonDomainSpecs.when_throwing_exception_from_nested_uof_after_inserting_in_root_uof.should_rollback_everything();
            }
        }

        [TestFixture]
        [Category("Integration")]
        public class when_throwing_exception_from_root_unit_of_work_after_completing_nested_one : TestingScenario
        {
            [Test]
            public void should_rollback_everything()
            {
                CommonDomainSpecs.when_throwing_exception_from_root_unit_of_work_after_completing_nested_one.should_rollback_everything();
            }
        }

        [TestFixture]
        [Category("Integration")]
        public class when_trying_to_insert_new_team_without_providing_id : TestingScenario
        {
            [Test]
            public void should_insert_with_generated_id_because_identity_generator_has_been_configured_in_bootstrap()
            {
                UnitOfWork.Do(uow =>
                {
                    //arrange
                    var team1 = new Team { Name = "SuperTeam", Description = "Really super" };

                    //act                                                    
                    var result = uow.Repo<Team>().Insert(team1);

                    //assert
                    result.Id.Should().NotBe(0);
                });
            }
        }

        [TestFixture]
        [Category("Integration")]
        public class when_trying_to_insert_new_team_with_provided_id : TestingScenario
        {
            [Test]
            public void should_insert_with_provided_id()
            {
                UnitOfWork.Do(uow =>
                {
                    //arrange
                    var team1 = new Team {Id = 5, Name = "SuperTeam", Description = "Really super" };

                    //act                                                    
                    var result = uow.Repo<Team>().Insert(team1);

                    //assert
                    result.Id.Should().Be(5);
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
                    var country1 = new Country {Id = 1, Name = "USA", Language = "English" };
                    uow.Repo<Country>().Insert(country1);

                    var country2 = new Country {Id = 2, Name = "Mexico", Language = "Spanish" };
                    uow.Repo<Country>().Insert(country2);

                    var team1 = new Team() {Id = 1, Name = "Super", Description = "SuperBg", Country = country1 };
                    uow.Repo<Team>().Insert(team1);

                    team2 = new Team() {Id = 2, Name = "Awesome", Description = "AwesomeBg", Country = country2 };
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
                    var team1 = new Team {Id = 1, Name = "Super", Description = "SuperBg" };
                    uow.Repo<Team>().Insert(team1);

                    var team2 = new Team {Id = 2, Name = "Good", Description = "GoodBg" };
                    uow.Repo<Team>().Insert(team2);

                    var team3 = new Team {Id = 3, Name = "Bad", Description = "BadBg" };
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
        public class when_trying_to_delete_team : TestingScenario
        {
            [Test]
            public void should_delete_team()
            {
                //arrange
                Team team = null;
                UnitOfWork.Do(uow =>
                {
                    team = new Team() {Id=1,  Name = "Super", Description = "SuperBg" };
                    uow.Repo<Team>().Insert(team);
                });

                UnitOfWork.Do(uow => uow.Repo<Team>().GetById(team.Id).Should().NotBeNull());

                //act
                UnitOfWork.Do(uow => uow.Repo<Team>().Delete(team));

                //assert
                UnitOfWork.Do(uow =>
                {                                        
                    var foundTeam = uow.Repo<Team>().GetById(team.Id);
                    foundTeam.Should().BeNull();
                });
            }
        }

        [TestFixture]
        [Category("Integration")]
        public class when_trying_to_delete_team_by_id : TestingScenario
        {
             [Test]
            public void should_delete_team()
            {
                //arrange
                Team team = null;
                UnitOfWork.Do(uow =>
                {
                    team = new Team() {Id = 1, Name = "Super", Description = "SuperBg" };
                    uow.Repo<Team>().Insert(team);
                });

                UnitOfWork.Do(uow => uow.Repo<Team>().GetById(team.Id).Should().NotBeNull());

                //act                    
                UnitOfWork.Do(uow => uow.Repo<Team>().Delete(team.Id));

                //assert
                UnitOfWork.Do(uow =>
                {
                    var foundTeam = uow.Repo<Team>().GetById(team.Id);
                    foundTeam.Should().BeNull();
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
                                                                
                Configurator.UseWindsorContainer()
                            .UseStorage(MongoDbStorage.Use("mongodb://localhost:27017", "AntlerTest")
                                                      .WithRecreatedDatabase(true)
                                                      .WithIdentityGenerator(() => new Random().Next(1, int.MaxValue))
                                                      .WithEnsuredIndexes(MongoDbIndexBuilder.Add<Employee>(IndexKeys<Employee>.Ascending(_ => _.Id), IndexOptions<Employee>.SetUnique(true))
                                                                          .ThenAdd<Team>(IndexKeys<Team>.Ascending(_ => _.Id), IndexOptions<Employee>.SetUnique(true))));
            }

            [TearDown]
            public void TearDown()
            {                
                Configurator.UnUseContainer().UnUseStorage().Dispose();
            }
        } 
        #endregion
    }
}

// ReSharper restore InconsistentNaming