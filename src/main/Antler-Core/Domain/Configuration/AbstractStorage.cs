using System.ComponentModel;

namespace SmartElk.Antler.Core.Domain.Configuration
{
    public abstract class AbstractStorage : IStorage
    {
        [EditorBrowsable(EditorBrowsableState.Never)]
        public abstract void Configure(IDomainConfigurator configurator);
    }
}
