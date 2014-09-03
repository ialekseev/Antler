// ReSharper disable InconsistentNaming

using NUnit.Framework;
using SmartElk.Antler.Core;
using SmartElk.Antler.Core.Abstractions.Configuration;
using SmartElk.Antler.MongoDb.Configuration;
using SmartElk.Antler.Specs.Shared.CommonSpecs;
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
                      
        #region Configuration
        public class TestingScenario
        {
            protected IAntlerConfigurator Configurator { get; set; }
                        
            [SetUp]
            public void SetUp()
            {                                               
                Configurator = new AntlerConfigurator();
                Configurator.UseWindsorContainer().UseStorage(MongoDbStorage.Use("mongodb://localhost:27017", "AntlerTest").WithRecreatedDatabase());
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