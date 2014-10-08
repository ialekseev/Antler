using SmartElk.Antler.Core.Abstractions.Configuration;
using StructureMap;

namespace SmartElk.Antler.StructureMap
{
    public static class ConfigurationEx
    {        
        /// <summary>
        /// Use StructureMap IoC container.
        /// </summary>        
        public static IAntlerConfigurator UseStructureMapContainer(this IAntlerConfigurator nodeConfigurator)
        {
            ((IAntlerConfiguratorEx)nodeConfigurator).SetContainerAdapter(new StructureMapContainerAdapter());
            return nodeConfigurator;
        }

        /// <summary>
        /// Use specified StructureMap IoC container.
        /// </summary>        
        public static IAntlerConfigurator UseStructureMapContainer(this IAntlerConfigurator nodeConfigurator, IContainer container)
        {
            ((IAntlerConfiguratorEx)nodeConfigurator).SetContainerAdapter(new StructureMapContainerAdapter(container));
            return nodeConfigurator;
        }
    }
}