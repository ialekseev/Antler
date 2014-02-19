using System;
using SmartElk.Antler.Core.Abstractions.Configuration;

namespace SmartElk.Antler.StructureMap
{
    public static class ConfigurationEx
    {
        public static IAntlerConfigurator UseStructureMapContainer(this IAntlerConfigurator nodeConfigurator, Func<global::StructureMap.IContainer> containerProvider)
        {
            ((IAntlerConfiguratorEx)nodeConfigurator).SetContainerAdapter(new StructureMapContainerAdapter(containerProvider()));
            return nodeConfigurator;
        }

        public static IAntlerConfigurator UseStructureMapContainer(this IAntlerConfigurator nodeConfigurator)
        {
            ((IAntlerConfiguratorEx)nodeConfigurator).SetContainerAdapter(new StructureMapContainerAdapter());
            return nodeConfigurator;
        }

        public static IAntlerConfigurator UnUseStructureMapContainer(this IAntlerConfigurator nodeConfigurator)
        {
            ((IAntlerConfiguratorEx)nodeConfigurator).UnSetContainerAdapter();
            return nodeConfigurator;
        }
    }
}