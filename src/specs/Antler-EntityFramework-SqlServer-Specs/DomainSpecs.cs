// ReSharper disable InconsistentNaming

using System.Linq;
using NUnit.Framework;
using SmartElk.Antler.Core;
using SmartElk.Antler.Core.Abstractions.Configuration;
using SmartElk.Antler.Core.Common.Reflection;
using SmartElk.Antler.EntityFramework.Configuration;
using SmartElk.Antler.Specs.Shared.CommonSpecs;
using SmartElk.Antler.Specs.Shared.EntityFramework.Mappings;
using SmartElk.Antler.StructureMap;

namespace SmartElk.Antler.EntityFramework.SqlServer.Specs
{
    /***You need to have "AntlerTest" database in your SQL SERVER. See connection string below***/

    public class DomainSpecs
    {
        [TestFixture]
        [Category("Integration")]
        public class when_trying_to_insert_employee : TestingScenario<LazyLoading>
        {
            [Test]
            public void should_insert()
            {
                CommonDomainSpecs.when_trying_to_insert_employee.should_insert();
            }
        }

        [TestFixture]
        [Category("Integration")]        
        public class when_trying_to_get_one_employee : TestingScenario<LazyLoading>
        {
            [Test]
            public void should_return_employee()
            {
                CommonDomainSpecs.when_trying_to_get_one_employee.should_return_employee();
            }
        }

        [TestFixture]
        [Category("Integration")]        
        public class when_trying_to_get_all_teams : TestingScenario<LazyLoading>
        {
            [Test]
            public void should_return_all_teams()
            {
                CommonDomainSpecs.when_trying_to_get_all_teams.should_return_all_teams();
            }
        }

        [TestFixture]
        [Category("Integration")]        
        public class when_trying_to_get_all_employees : TestingScenario<LazyLoading>
        {
            [Test]
            public static void should_return_all_employees()
            {
                CommonDomainSpecs.when_trying_to_get_all_employees.should_return_all_employees();
            }
        }

        [TestFixture]
        [Category("Integration")]        
        public class when_trying_to_find_employee_by_name : TestingScenario<LazyLoading>
        {
            [Test]
            public static void should_return_employee()
            {
                CommonDomainSpecs.when_trying_to_find_employee_by_name.should_return_employee();
            }
        }

        [TestFixture]
        [Category("Integration")]        
        public class when_trying_to_modify_employees_teams : TestingScenario<LazyLoading>
        {
            [Test]
            public static void should_modify_teams()
            {
                CommonDomainSpecs.when_trying_to_modify_employees_teams.should_modify_teams();
            }
        }

        [TestFixture]
        [Category("Integration")]        
        public class when_trying_to_find_team_by_country_name : TestingScenario<LazyLoading>
        {
            [Test]
            public void should_return_country()
            {
                CommonDomainSpecs.when_trying_to_find_team_by_country_name.should_find_team();
            }
        }

        [TestFixture]
        [Category("Integration")]
        public class when_trying_to_delete_team : TestingScenario<LazyLoading>
        {
            [Test]
            public void should_delete_team()
            {
                CommonDomainSpecs.when_trying_to_delete_team.should_delete_team();
            }
        }
        
        [TestFixture]
        [Category("Integration")]        
        public class when_trying_to_delete_team_by_id : TestingScenario<LazyLoading>
        {
            [Test]
            public void should_delete_team()
            {
                CommonDomainSpecs.when_trying_to_delete_team_by_id.should_delete_team();
            }
        }

        [TestFixture]
        [Category("Integration")]
        public class when_trying_to_update_detached_team : TestingScenario<EagerLoading>
        {
            [Test]
            public void should_updated_team()
            {
                CommonDomainSpecs.when_trying_to_update_detached_team.should_updated_team();
            }
        }

        [TestFixture]
        [Category("Integration")]
        public class when_trying_to_rollback_transaction : TestingScenario<LazyLoading>
        {
            [Test]
            public void should_rollback()
            {
                CommonDomainSpecs.when_trying_to_rollback_transaction.should_rollback();
            }
        }

        [TestFixture]
        [Category("Integration")]
        public class when_throwing_exception_after_insert : TestingScenario<LazyLoading>
        {
            [Test]
            public void should_rollback_transaction()
            {
                CommonDomainSpecs.when_throwing_exception_after_insert.should_rollback_transaction();
            }
        }

        [TestFixture]
        [Category("Integration")]
        public class when_throwing_exception_from_nested_uof_after_inserting_in_root_uof : TestingScenario<EagerLoading>
        {
            [Test]
            public void should_rollback_root_transaction()
            {
                CommonDomainSpecs.when_throwing_exception_from_nested_uof_after_inserting_in_root_uof.should_rollback_root_transaction();
            }
        }

        [TestFixture]
        [Category("Integration")]        
        public class when_trying_to_get_one_employee_without_lazy_loading : TestingScenario<EagerLoading>
        {
            [Test]
            public static void should_return_employee()
            {
                Antler.Specs.Shared.EntityFramework.CommonSpecs.CommonDomainSpecs.when_trying_to_get_one_employee_without_lazy_loading.should_return_employee();
            }
        }

        [TestFixture]
        [Category("Integration")]        
        public class when_trying_to_find_team_by_country_name_without_lazy_loading : TestingScenario<EagerLoading>
        {
            [Test]
            public void should_return_country()
            {
                Antler.Specs.Shared.EntityFramework.CommonSpecs.CommonDomainSpecs.when_trying_to_find_team_by_country_name_without_lazy_loading.should_return_country();
            }
        }

        #region Configuration
        public class LazyLoading { }
        public class EagerLoading { }
        public class TestingScenario<T>
        {
            protected IAntlerConfigurator Configurator { get; set; }

            [SetUp]
            public void SetUp()
            {
                Configurator = new AntlerConfigurator();

                const string connectionString = "Data Source=.\\SQLEXPRESS;Initial Catalog=AntlerTest;Integrated Security=True";
                var assemblyWithMappings = From.AssemblyWithType<CountryMap>().First();
                Configurator.UseStructureMapContainer()
                            .UseStorage(typeof(T) == typeof(LazyLoading)
                                            ? EntityFrameworkStorage.Use.WithConnectionString(connectionString)
                                                                      .WithMappings(assemblyWithMappings).WithRecreatedDatabase(true)
                                            : EntityFrameworkStorage.Use.WithoutLazyLoading()
                                                                      .WithConnectionString(connectionString)
                                                                      .WithMappings(assemblyWithMappings).WithRecreatedDatabase(true));                
            }

            [TearDown]
            public void Dispose()
            {
                Configurator.UnUseContainer().UnUseStorage().Dispose();
            }
        } 
        #endregion
    }
}

// ReSharper restore InconsistentNaming