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
        public class TestingScenario
        {
            protected static IAntlerConfigurator Configurator { get; set; }
            protected static AsInMemoryStorageResult AsInMemoryStorageResult { get; set; }
            
            private ISession session;

            static TestingScenario()
            {                
                Configurator = new AntlerConfigurator();
                Configurator.UseWindsorContainer();
                AsInMemoryStorageResult = Configurator.UseDomain().WithMappings(Assembly.GetExecutingAssembly()).AsInMemoryStorage();
            }

            [SetUp]
            public void SetUp()
            {
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
    }
}

// ReSharper restore InconsistentNaming