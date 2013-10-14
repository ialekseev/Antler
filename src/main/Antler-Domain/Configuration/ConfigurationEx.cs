using SmartElk.Antler.Abstractions.Configuration;

namespace SmartElk.Antler.Domain.Configuration
{
    public static class ConfigurationEx
    {        
        public static IDomainConfigurator UseDomain(this IAntlerConfigurator configurator)
        {                        
            UnitOfWork.SetSessionScopeFactoryExtractor(() => configurator.Configuration.Container.Get<ISessionScopeFactory>());
            return new DomainConfigurator(configurator.Configuration);
        }
    }    
}
