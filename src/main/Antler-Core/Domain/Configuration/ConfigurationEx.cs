using SmartElk.Antler.Core.Abstractions;
using SmartElk.Antler.Core.Abstractions.Configuration;
using SmartElk.Antler.Core.Common.CodeContracts;

namespace SmartElk.Antler.Core.Domain.Configuration
{
    public static class ConfigurationEx
    {        
        public static IAntlerConfigurator UseStorage(this IAntlerConfigurator configurator, IStorage storage)
        {
            Assumes.True<ContainerRequiredException>(configurator.HasContainer(), "Please choose some IoC container");                                    
            UnitOfWork.SetSessionScopeFactoryExtractor(() => configurator.Configuration.Container.Get<ISessionScopeFactory>());            
            storage.Configure(new DomainConfigurator(configurator.Configuration));
            return configurator;
        }

        public static IAntlerConfigurator UseStorageNamed(this IAntlerConfigurator configurator,  IStorage storage, string name)
        {
            Assumes.True<ContainerRequiredException>(configurator.HasContainer(), "Please choose some IoC container");
            UnitOfWork.SetSessionScopeFactoryNamedExtractor(storageName => configurator.Configuration.Container.Get<ISessionScopeFactory>(storageName));
            storage.Configure(new DomainConfigurator(configurator.Configuration).Named(name));
            return configurator;
        }        
    }    
}
