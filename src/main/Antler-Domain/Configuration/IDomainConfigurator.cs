using SmartElk.Antler.Abstractions.Configuration;

namespace SmartElk.Antler.Domain.Configuration
{
    public interface IDomainConfigurator
    {
        IBasicConfiguration Configuration { get; }
        string Name { get; }
        void RegisterSessionScopeFactory<T>(T sessionScopeFactory) where T : class;
        IDomainConfigurator Named(string name);
    }
}
