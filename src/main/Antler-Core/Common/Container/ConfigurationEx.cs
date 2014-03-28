using SmartElk.Antler.Core.Abstractions.Configuration;

namespace SmartElk.Antler.Core.Common.Container
{
    public static class ConfigurationEx
    {
        public static IAntlerConfigurator UseBuiltInContainer(this IAntlerConfigurator nodeConfigurator)
        {
            ((IAntlerConfiguratorEx)nodeConfigurator).SetContainerAdapter(new BuiltInContainer());
            return nodeConfigurator;
        }

        public static IAntlerConfigurator UnUseBuiltInContainer(this IAntlerConfigurator nodeConfigurator)
        {
            ((IAntlerConfiguratorEx)nodeConfigurator).UnSetContainerAdapter();
            return nodeConfigurator;
        }
    }
}
