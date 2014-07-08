// ReSharper disable InconsistentNaming
using System;
using System.Linq;
using FluentAssertions;
using FluentNHibernate.Cfg.Db;
using NHibernate;
using NUnit.Framework;
using SmartElk.Antler.Core;
using SmartElk.Antler.Core.Abstractions.Configuration;
using SmartElk.Antler.Core.Common.Container;
using SmartElk.Antler.Core.Common.Reflection;
using SmartElk.Antler.Core.Domain;
using SmartElk.Antler.EntityFramework.SqlCe.Configuration;
using SmartElk.Antler.NHibernate.Configuration;
using SmartElk.Antler.NHibernate.Internal;
using SmartElk.Antler.Specs.Shared.Entities;
using SmartElk.Antler.Specs.Shared.NHibernate.Mappings;

namespace SmartElk.Antler.Storages.Specs
{
    public class DomainSpecs
    {                       
        [TestFixture]
        [Category("Integration")]
        public class when_trying_to_query_country_from_one_storage_and_put_into_another: TestingScenario
        {
            [Test]
            public void should_succeed()
            {
                //arrange
                UnitOfWork.Do(uow =>
                    {                        
                        var country1 = new Country {Name = "USA", Language = "English"};
                        uow.Repo<Country>().Insert(country1);

                        var country2 = new Country {Name = "Mexico", Language = "Spanish"};
                        uow.Repo<Country>().Insert(country2);

                        var team1 = new Team() {Name = "Super", Description = "SuperBg", Country = country1};
                        uow.Repo<Team>().Insert(team1);

                        var team2 = new Team() {Name = "Awesome", Description = "AwesomeBg", Country = country2};
                        uow.Repo<Team>().Insert(team2);                                                
                    });

                //act
                Func<UnitOfWork, Country> query = uow => uow.Repo<Country>().AsQueryable().First(t => t.Name == "Mexico");
                
                var countryToTransfer = UnitOfWork.Do(query);
                UnitOfWork.Do("EF", uow => uow.Repo<Country>().Insert(countryToTransfer));

                var result = UnitOfWork.Do("EF", query);

                //assert
                result.Name.Should().Be("Mexico");
                result.Language.Should().Be("Spanish");
            }
        }


        #region Configuration
        public class TestingScenario
        {
            protected IAntlerConfigurator Configurator { get; set; }
            protected ConfigurationResult NhConfigurationResult { get; set; }
            private ISession nhSession;

            [SetUp]
            public void SetUp()
            {
                Configurator = new AntlerConfigurator();
                Configurator.UseBuiltInContainer().UseStorage(NHibernateStorage.Use.WithDatabaseConfiguration(SQLiteConfiguration.Standard.InMemory()).WithMappings(From.AssemblyWithType<CountryMap>().First())).
                                                   UseStorageNamed(EntityFrameworkPlusSqlCe.Use.WithConnectionString("Data Source=TestDB.sdf").WithMappings(From.AssemblyWithType<Antler.Specs.Shared.EntityFramework.Mappings.CountryMap>().First()).WithRecreatedDatabase(), "EF");
                
                nhSession = NewSessionForTesting.CreateNHibernateSession(Configurator, typeof(NHibernateStorage));
            }

            [TearDown]
            public void TearDown()
            {
                NewSessionForTesting.ResetNHibernateSession(Configurator, nhSession);
                Configurator.UnUseContainer().UnUseStorage().Dispose();                
            }
        } 
        #endregion
    }
}

// ReSharper restore InconsistentNaming