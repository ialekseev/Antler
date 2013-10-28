using SmartElk.Antler.Abstractions.Configuration;

namespace SmartElk.Antler.Windsor
{
    public static class ConfigurationEx
    {
        public static IBasicConfigurator UseWindsorContainer(this IBasicConfigurator nodeConfigurator)
        {
            ((IBasicConfiguratorEx)nodeConfigurator).SetContainerAdapter(new WindsorContainerAdapter());
            return nodeConfigurator;
        }
    }
}
