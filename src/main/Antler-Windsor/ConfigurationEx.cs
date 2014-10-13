using Castle.Windsor;
using SmartElk.Antler.Core.Abstractions.Configuration;

namespace SmartElk.Antler.Windsor
{
    public static class ConfigurationEx
    {
        /// <summary>
        /// Use Castle Windsor IoC container.
        /// </summary>        
        public static IAntlerConfigurator UseWindsorContainer(this IAntlerConfigurator configurator)
        {
            ((IAntlerConfiguratorEx)configurator).SetContainerAdapter(new WindsorContainerAdapter());
            return configurator;
        }

        /// <summary>
        /// Use specified Castle Windsor IoC container.
        /// </summary>        
        public static IAntlerConfigurator UseWindsorContainer(this IAntlerConfigurator configurator, IWindsorContainer container)
        {
            ((IAntlerConfiguratorEx)configurator).SetContainerAdapter(new WindsorContainerAdapter(container));
            return configurator;
        }
    }
}
