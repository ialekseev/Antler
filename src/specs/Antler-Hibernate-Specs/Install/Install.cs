using Antler.Hibernate;
using SmartElk.Antler.Abstractions.Configuration;
using SmartElk.Antler.Windsor;

namespace SmartElk.Antler.Hibernate.Specs.Install
{
    public class Install
    {
        private static IAntlerConfigurator Configurator { get; set; }
        
        public static void RegisterComponents()
        {
            if (Configurator == null)
            {
                Configurator = new AntlerConfigurator();
                Configurator.UseWindsorContainer();                
                Configurator.UseNHibernate(SqliteSessionFactoryCreator.CreateFactory());
            }            
        }                        
    }
}
