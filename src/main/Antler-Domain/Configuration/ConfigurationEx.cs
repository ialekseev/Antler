using SmartElk.Antler.Abstractions;
using SmartElk.Antler.Abstractions.Configuration;

namespace SmartElk.Antler.Domain.Configuration
{
    public static class ConfigurationEx
    {        
        public static void UseStorage(this IAntlerConfigurator configurator, IStorage storage)
        {                        
            if (!configurator.HasContainer())
                throw new ContainerRequiredException("Please choose some IoC container");

            RegisterSessionScopeFactoryExtractor(configurator);
            storage.Configure(new DomainConfigurator(configurator.Configuration));                                    
        }

        public static void UseStorageNamed(this IAntlerConfigurator configurator, IStorage storage, string name)
        {
            if (!configurator.HasContainer())
                throw new ContainerRequiredException("Please choose some IoC container");

            RegisterSessionScopeFactoryExtractor(configurator);
            storage.Configure(new DomainConfigurator(configurator.Configuration).Named(name));            
        }
                
        private static void RegisterSessionScopeFactoryExtractor(IAntlerConfigurator configurator)
        {
            UnitOfWork.SetSessionScopeFactoryExtractor(name => string.IsNullOrEmpty(name) ? configurator.Configuration.Container.Get<ISessionScopeFactory>() : 
                                                               configurator.Configuration.Container.Get<ISessionScopeFactory>(name));
        }
    }    
}
