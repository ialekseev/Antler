// ReSharper disable InconsistentNaming
using System;
using System.Linq;
using Antler.NHibernate.Configuration;
using FluentAssertions;
using NHibernate;
using NUnit.Framework;
using SmartElk.Antler.Core.Abstractions.Configuration;
using SmartElk.Antler.Core.Common.Reflection;
using SmartElk.Antler.Core.Domain;
using SmartElk.Antler.Core.Domain.Configuration;
using SmartElk.Antler.EntityFramework.SqlCe.Configuration;
using SmartElk.Antler.NHibernate.Sqlite.Configuration;
using SmartElk.Antler.Specs.Shared.Entities;
using SmartElk.Antler.Specs.Shared.EntityFramework.Configuration;
using SmartElk.Antler.Specs.Shared.NHibernate.Configuration;
using SmartElk.Antler.Specs.Shared.NHibernate.Mappings;
using SmartElk.Antler.Windsor;

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

                        var team1 = new Team() {Name = "Super", BusinessGroup = "SuperBg", Country = country1};
                        uow.Repo<Team>().Insert(team1);

                        var team2 = new Team() {Name = "Awesome", BusinessGroup = "AwesomeBg", Country = country2};
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
                Configurator.UseWindsorContainer().UseStorage(NHibernatePlusSqlite.Use.WithMappings(From.AssemblyWithType<CountryMap>().First())).
                                                   UseStorageNamed(EntityFrameworkPlusSqlCe.Use.WithConnectionString("Data Source=TestDB.sdf").WithMappings(From.AssemblyWithType<Antler.Specs.Shared.EntityFramework.Mappings.CountryMap>().First()), "EF");
                
                Configurator.ClearDatabase("EF");

                nhSession = Configurator.CreateNHibernateSession(typeof(NHibernatePlusSqlite));
            }

            [TearDown]
            public void TearDown()
            {
                Configurator.ResetNHibernateSession(nhSession);
                Configurator.UnUseWindsorContainer().UnUseStorage().Dispose();                
            }
        } 
        #endregion
    }
}

// ReSharper restore InconsistentNaming