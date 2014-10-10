using SmartElk.Antler.Core.Abstractions.Configuration;
using StructureMap;

namespace SmartElk.Antler.StructureMap
{
    public static class ConfigurationEx
    {        
        /// <summary>
        /// Use StructureMap IoC container.
        /// </summary>        
        public static IAntlerConfigurator UseStructureMapContainer(this IAntlerConfigurator configurator)
        {
            ((IAntlerConfiguratorEx)configurator).SetContainerAdapter(new StructureMapContainerAdapter());
            return configurator;
        }

        /// <summary>
        /// Use specified StructureMap IoC container.
        /// </summary>        
        public static IAntlerConfigurator UseStructureMapContainer(this IAntlerConfigurator configurator, IContainer container)
        {
            ((IAntlerConfiguratorEx)configurator).SetContainerAdapter(new StructureMapContainerAdapter(container));
            return configurator;
        }
    }
}