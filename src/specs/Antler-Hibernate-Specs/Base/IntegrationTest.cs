using System.Reflection;
using SmartElk.Antler.Abstractions.Configuration;
using SmartElk.Antler.Domain.Configuration;
using SmartElk.Antler.Hibernate.Sqlite;
using SmartElk.Antler.Windsor;

namespace SmartElk.Antler.Hibernate.Specs.Base
{
    public class IntegrationTest
    {
        private IAntlerConfigurator Configurator { get; set; }
        
        public IntegrationTest()
        {            
            Configurator = new AntlerConfigurator();                
            Configurator.UseWindsorContainer();
            Configurator.UseDomain().AsInMemoryStorage(Assembly.GetExecutingAssembly());                                                                              
        }
    }
}
