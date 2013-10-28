using SmartElk.Antler.Abstractions.Configuration;

namespace SmartElk.Antler.Domain.Configuration
{
    public static class ConfigurationEx
    {        
        public static IDomainConfigurator UseStorage(this IBasicConfigurator configurator)
        {                        
            RegisterSessionScopeFactoryExtractor(configurator);
            return new DomainConfigurator(configurator.Configuration);
        }
        
        private static void RegisterSessionScopeFactoryExtractor(IBasicConfigurator configurator)
        {
            UnitOfWork.SetSessionScopeFactoryExtractor(name => string.IsNullOrEmpty(name) ? configurator.Configuration.Container.Get<ISessionScopeFactory>() : 
                                                               configurator.Configuration.Container.Get<ISessionScopeFactory>(name));
        }
    }    
}
