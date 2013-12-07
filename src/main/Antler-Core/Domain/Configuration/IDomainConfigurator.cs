using SmartElk.Antler.Core.Abstractions.Configuration;

namespace SmartElk.Antler.Core.Domain.Configuration
{
    public interface IDomainConfigurator
    {
        IBasicConfiguration Configuration { get; }
        string Name { get; }        
        IDomainConfigurator Named(string name);
    }
}
