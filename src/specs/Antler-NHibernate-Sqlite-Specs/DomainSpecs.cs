// ReSharper disable InconsistentNaming
using System.Linq;
using FluentNHibernate.Cfg.Db;
using NHibernate;
using NUnit.Framework;
using SmartElk.Antler.Core;
using SmartElk.Antler.Core.Abstractions.Configuration;
using SmartElk.Antler.Core.Common.Reflection;
using SmartElk.Antler.Core.Domain;
using SmartElk.Antler.NHibernate.Configuration;
using SmartElk.Antler.NHibernate.Internal;
using SmartElk.Antler.Specs.Shared.CommonSpecs;
using SmartElk.Antler.Specs.Shared.NHibernate.Mappings;
using SmartElk.Antler.StructureMap;

namespace SmartElk.Antler.NHibernate.Sqlite.Specs
{
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
        public class when_trying_to_update_detached_team : TestingScenario
        {
            [Test]
            public void should_updated_team()
            {
                CommonDomainSpecs.when_trying_to_update_detached_team.should_updated_team();
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

        //todo: is it possible to make these tests pass considering Sqlite's session sharing?
        /*[TestFixture]
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
        }*/

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
            private ISession session;

            [SetUp]
            public void SetUp()
            {
                Configurator = new AntlerConfigurator();                
                Configurator.UseStructureMapContainer().UseStorage(NHibernateStorage.Use.WithDatabaseConfiguration(SQLiteConfiguration.Standard.InMemory()).WithMappings(From.AssemblyWithType<CountryMap>().First()));

                session = NewSessionForTesting.CreateNHibernateSession(Configurator, typeof(NHibernateStorage), UnitOfWorkSettings.Default.StorageName);
            }

            [TearDown]
            public void TearDown()
            {
                NewSessionForTesting.ResetNHibernateSession(Configurator, session, UnitOfWorkSettings.Default.StorageName);
                Configurator.UnUseContainer().UnUseStorage().Dispose();
            }
        } 
        #endregion
    }
}

// ReSharper restore InconsistentNaming