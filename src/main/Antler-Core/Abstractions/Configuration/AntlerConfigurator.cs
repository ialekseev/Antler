using System;
using SmartElk.Antler.Core.Common.CodeContracts;

namespace SmartElk.Antler.Core.Abstractions.Configuration
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
            Requires.NotNull(container, "container");
            
            if (_configuration.Container != null)
                throw new InvalidOperationException("Container adapter is already set to " + _configuration.Container.GetType().Name);

            _configuration.Container = container;
        }

        void IAntlerConfiguratorEx.UnSetContainerAdapter()
        {            
            _configuration.Container.Dispose();
            _configuration.Container = null;
        }

        public void Dispose()
        {
            _configuration.Dispose();
        }
    }
}
