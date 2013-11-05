using SmartElk.Antler.Abstractions.Configuration;

namespace SmartElk.Antler.Domain.Configuration
{
    public interface IDomainConfigurator
    {
        IBasicConfiguration Configuration { get; }
        string Name { get; }        
        IDomainConfigurator Named(string name);
    }
}
