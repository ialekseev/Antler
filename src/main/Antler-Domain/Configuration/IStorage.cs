namespace SmartElk.Antler.Domain.Configuration
{
    public interface IStorage
    {
        void Configure(IDomainConfigurator configurator);
    }
}
