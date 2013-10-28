using System;

namespace SmartElk.Antler.Abstractions.Configuration
{
    public class BasicConfigurator : IBasicConfigurator, IBasicConfiguratorEx
    {
        private readonly IBasicConfiguration _configuration;

        public BasicConfigurator()
        {
            _configuration = new BasicConfiguration();            
        }

        public IBasicConfiguration Configuration
        {
            get { return _configuration; }
        }

        void IBasicConfiguratorEx.SetContainerAdapter(IContainer container)
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
