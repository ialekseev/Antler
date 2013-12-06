namespace SmartElk.Antler.Core.Abstractions.Configuration
{
    public interface IAntlerConfiguratorEx
    {
        void SetContainerAdapter(IContainer container);
    }

    public static class AntlerConfiguratorEx
    {
        public static bool HasContainer(this IAntlerConfigurator configurator)
        {
            return configurator.Configuration.Container != null;
        }
    }
}
