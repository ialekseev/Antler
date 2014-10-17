using System.ComponentModel;
using SmartElk.Antler.Core.Common.CodeContracts;

namespace SmartElk.Antler.Core.Domain.Configuration
{
    public abstract class AbstractStorage : IStorage
    {
        [EditorBrowsable(EditorBrowsableState.Never)]
        public virtual void Configure(IDomainConfigurator configurator)
        {
            Requires.NotNull(configurator, "configurator");
            
            var sessionScopeFactory = ConfigureInternal(configurator);
            configurator.Configuration.Container.PutWithNameOrDefault(sessionScopeFactory, configurator.Name);
        }

        protected abstract ISessionScopeFactory ConfigureInternal(IDomainConfigurator configurator);
    }
}
