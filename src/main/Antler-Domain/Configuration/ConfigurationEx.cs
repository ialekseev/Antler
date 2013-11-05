using SmartElk.Antler.Abstractions.Configuration;

namespace SmartElk.Antler.Domain.Configuration
{
    public static class ConfigurationEx
    {        
        public static void UseStorage(this IBasicConfigurator configurator, IStorage storage)
        {                        
            RegisterSessionScopeFactoryExtractor(configurator);
            storage.Configure(new DomainConfigurator(configurator.Configuration));                                    
        }

        public static void UseStorageNamed(this IBasicConfigurator configurator, IStorage storage, string name)
        {
            RegisterSessionScopeFactoryExtractor(configurator);
            storage.Configure(new DomainConfigurator(configurator.Configuration).Named(name));            
        }
                
        private static void RegisterSessionScopeFactoryExtractor(IBasicConfigurator configurator)
        {
            UnitOfWork.SetSessionScopeFactoryExtractor(name => string.IsNullOrEmpty(name) ? configurator.Configuration.Container.Get<ISessionScopeFactory>() : 
                                                               configurator.Configuration.Container.Get<ISessionScopeFactory>(name));
        }
    }    
}
