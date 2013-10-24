using SmartElk.Antler.Abstractions.Configuration;

namespace SmartElk.Antler.Domain.Configuration
{
    public static class ConfigurationEx
    {        
        public static IDomainConfigurator UseStorage(this IBasicConfigurator configurator)
        {                        
            UnitOfWork.SetSessionScopeFactoryExtractor(() => configurator.Configuration.Container.Get<ISessionScopeFactory>());
            return new DomainConfigurator(configurator.Configuration);
        }

        public static IDomainConfigurator UseNamedStorage(this IBasicConfigurator configurator, string name)
        {
            UnitOfWork.SetSessionScopeFactoryExtractor(() => configurator.Configuration.Container.Get<ISessionScopeFactory>(name));
            return new DomainConfigurator(configurator.Configuration);
        }
    }    
}
