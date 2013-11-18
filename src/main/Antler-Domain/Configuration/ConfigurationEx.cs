using SmartElk.Antler.Abstractions;
using SmartElk.Antler.Abstractions.Configuration;
using SmartElk.Antler.Common.CodeContracts;

namespace SmartElk.Antler.Domain.Configuration
{
    public static class ConfigurationEx
    {        
        public static void UseStorage(this IAntlerConfigurator configurator, IStorage storage)
        {
            Assumes.True<ContainerRequiredException>(configurator.HasContainer(), "Please choose some IoC container");
                                    
            UnitOfWork.SetSessionScopeFactoryExtractor(() => configurator.Configuration.Container.Get<ISessionScopeFactory>());

            storage.Configure(new DomainConfigurator(configurator.Configuration));                                    
        }

        public static void UseStorageNamed(this IAntlerConfigurator configurator, IStorage storage, string name)
        {
            Assumes.True<ContainerRequiredException>(configurator.HasContainer(), "Please choose some IoC container");

            UnitOfWork.SetSessionScopeFactoryNamedExtractor(storageName => configurator.Configuration.Container.Get<ISessionScopeFactory>(storageName));

            storage.Configure(new DomainConfigurator(configurator.Configuration).Named(name));            
        }                        
    }    
}
