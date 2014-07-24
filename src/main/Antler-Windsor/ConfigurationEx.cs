using SmartElk.Antler.Core.Abstractions.Configuration;

namespace SmartElk.Antler.Windsor
{
    public static class ConfigurationEx
    {
        /// <summary>
        /// Use Castle Windsor IoC container.
        /// </summary>        
        public static IAntlerConfigurator UseWindsorContainer(this IAntlerConfigurator nodeConfigurator)
        {
            ((IAntlerConfiguratorEx)nodeConfigurator).SetContainerAdapter(new WindsorContainerAdapter());
            return nodeConfigurator;
        }        
    }
}
