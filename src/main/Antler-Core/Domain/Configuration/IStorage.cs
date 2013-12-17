namespace SmartElk.Antler.Core.Domain.Configuration
{
    public interface IStorage
    {
        void Configure(IDomainConfigurator configurator); //todo: hide this method from framework clients
    }
}
