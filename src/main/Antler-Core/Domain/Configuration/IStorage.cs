namespace SmartElk.Antler.Core.Domain.Configuration
{
    public interface IStorage
    {
        void Configure(IDomainConfigurator configurator);
    }
}
