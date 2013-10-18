// ReSharper disable InconsistentNaming
using System.Reflection;
using Antler.Hibernate;
using NHibernate;
using NHibernate.Tool.hbm2ddl;
using NUnit.Framework;
using SmartElk.Antler.Abstractions.Configuration;
using SmartElk.Antler.Domain;
using SmartElk.Antler.Domain.Configuration;
using SmartElk.Antler.Hibernate.Sqlite.Configuration;
using SmartElk.Antler.Specs.Shared.CommonSpecs;
using SmartElk.Antler.Windsor;

namespace SmartElk.Antler.Hibernate.Specs
{
    public class DomainSpecs
    {                
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
                CommonDomainSpecs.when_trying_to_find_team_by_country_name.should_return_country();
            }
        }


        #region Configuration
        public class TestingScenario
        {
            protected IAntlerConfigurator Configurator { get; set; }
            protected AsInMemoryStorageResult AsInMemoryStorageResult { get; set; }
            private ISession session;

            [SetUp]
            public void SetUp()
            {
                Configurator = new AntlerConfigurator();
                AsInMemoryStorageResult = Configurator.UseWindsorContainer().UseDomain().WithMappings(Assembly.GetExecutingAssembly()).AsInMemoryStorage();

                session = AsInMemoryStorageResult.SessionFactory.OpenSession();
                new SchemaExport(AsInMemoryStorageResult.Configuration).Execute(false, true, false, session.Connection, null);
                var sessionScopeFactory = (ISessionScopeFactoryEx)Configurator.Configuration.Container.Get<ISessionScopeFactory>();
                sessionScopeFactory.SetSession(session);
            }

            [TearDown]
            public void TearDown()
            {
                session.Dispose();
                var sessionScopeFactory = (ISessionScopeFactoryEx)Configurator.Configuration.Container.Get<ISessionScopeFactory>();
                sessionScopeFactory.ResetSession();

                Configurator.Dispose();
            }
        } 
        #endregion
    }
}

// ReSharper restore InconsistentNaming