// ReSharper disable InconsistentNaming

using System.Linq;
using FluentNHibernate.Cfg.Db;
using NUnit.Framework;
using SmartElk.Antler.Core;
using SmartElk.Antler.Core.Abstractions.Configuration;
using SmartElk.Antler.Core.Common.Reflection;
using SmartElk.Antler.NHibernate.Configuration;
using SmartElk.Antler.Specs.Shared.CommonSpecs;
using SmartElk.Antler.Specs.Shared.NHibernate.Mappings;
using SmartElk.Antler.Windsor;
using SmartElk.Antler.NHibernate.SqlServer.Specs.Configuration;

namespace SmartElk.Antler.NHibernate.SqlServer.Specs
{
    /***You need to have "AntlerTest" database in your SQL SERVER. See connection string below***/

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
        public class when_trying_to_get_all_teams : TestingScenario
        {           
            [Test]
            public void should_return_all_teams()
            {
                CommonDomainSpecs.when_trying_to_get_all_teams.should_return_all_teams();
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
        public class when_trying_to_modify_employees_teams : TestingScenario
        {
            [Test]
            public static void should_modify_teams()
            {
                CommonDomainSpecs.when_trying_to_modify_employees_teams.should_modify_teams();
            }
        }

        [TestFixture]
        [Category("Integration")]
        public class when_trying_to_find_team_by_country_name : TestingScenario
        {
            [Test]
            public void should_return_country()
            {
                CommonDomainSpecs.when_trying_to_find_team_by_country_name.should_find_team();
            }
        }

        [TestFixture]
        [Category("Integration")]
        public class when_trying_to_delete_team : TestingScenario
        {
            [Test]
            public void should_delete_team()
            {
                CommonDomainSpecs.when_trying_to_delete_team.should_delete_team();
            }
        }
        
        [TestFixture]
        [Category("Integration")]
        public class when_trying_to_delete_team_by_id : TestingScenario
        {
            [Test]
            public void should_delete_team()
            {
                CommonDomainSpecs.when_trying_to_delete_team_by_id.should_delete_team();
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
        public class when_trying_to_insert_new_team : TestingScenario
        {
            [Test]
            public void should_return_generated_id()
            {                
                CommonDomainSpecs.when_trying_to_insert_new_team.should_return_generated_id();
            }
        }

        [TestFixture]
        [Category("Integration")]
        public class when_trying_to_query_using_nhibernate_internal_session_directly : TestingScenario
        {
            [Test]
            public void should_return_result()
            {
                Antler.Specs.Shared.NHibernate.CommonSpecs.CommonDomainSpecs.when_trying_to_query_using_nhibernate_internal_session_directly.should_return_result();
            }
        }
        
        #region Configuration
        public class TestingScenario
        {
            protected IAntlerConfigurator Configurator { get; set; }
            protected ConfigurationResult AsInMemoryStorageResult { get; set; }
            
            [SetUp]
            public void SetUp()
            {                                               
                Configurator = new AntlerConfigurator();
                Configurator.UseWindsorContainer().UseStorage(NHibernateStorage.Use.WithDatabaseConfiguration(MsSqlConfiguration.MsSql2008.ConnectionString("Data Source=.\\SQLEXPRESS;Initial Catalog=AntlerTest;Integrated Security=True")).WithGeneratedDatabase(Configurator.TryToCreateDatabaseCommand("AntlerTest")).WithMappings(From.AssemblyWithType<CountryMap>().First()));
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