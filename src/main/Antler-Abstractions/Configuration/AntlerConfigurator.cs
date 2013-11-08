using System;

namespace SmartElk.Antler.Abstractions.Configuration
{
    public class AntlerConfigurator : IAntlerConfigurator, IAntlerConfiguratorEx
    {
        private readonly IBasicConfiguration _configuration;

        public AntlerConfigurator()
        {
            _configuration = new BasicConfiguration();            
        }

        public IBasicConfiguration Configuration
        {
            get { return _configuration; }
        }

        void IAntlerConfiguratorEx.SetContainerAdapter(IContainer container)
        {
            if (_configuration.Container != null)
                throw new InvalidOperationException("Container adapter is already set to " + _configuration.Container.GetType().Name);

            _configuration.Container = container;
        }

        public void Dispose()
        {
            _configuration.Dispose();
        }
    }
}
