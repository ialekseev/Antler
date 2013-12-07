using SmartElk.Antler.Core.Abstractions.Configuration;

namespace SmartElk.Antler.Windsor
{
    public static class ConfigurationEx
    {
        public static IAntlerConfigurator UseWindsorContainer(this IAntlerConfigurator nodeConfigurator)
        {
            ((IAntlerConfiguratorEx)nodeConfigurator).SetContainerAdapter(new WindsorContainerAdapter());
            return nodeConfigurator;
        }
    }
}
