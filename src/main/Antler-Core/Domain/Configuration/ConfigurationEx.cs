using SmartElk.Antler.Core.Abstractions;
using SmartElk.Antler.Core.Abstractions.Configuration;
using SmartElk.Antler.Core.Common.CodeContracts;

namespace SmartElk.Antler.Core.Domain.Configuration
{
    public static class ConfigurationEx
    {        
        public static IAntlerConfigurator UseStorage(this IAntlerConfigurator configurator, IStorage storage)
        {
            Requires.NotNull(storage, "Storage can't be null");
            Assumes.True<ContainerRequiredException>(configurator.HasContainer(), "Please choose some IoC container");                                    
            
            UnitOfWork.SessionScopeFactoryExtractor = () => configurator.Configuration.Container.Get<ISessionScopeFactory>();            
            storage.Configure(new DomainConfigurator(configurator.Configuration));
            return configurator;
        }

        public static IAntlerConfigurator UseStorageNamed(this IAntlerConfigurator configurator,  IStorage storage, string name)
        {
            Requires.NotNull(storage, "Storage can't be null");
            Requires.NotNull(storage, "Storage name can't be null");
            Assumes.True<ContainerRequiredException>(configurator.HasContainer(), "Please choose some IoC container");
            
            UnitOfWork.SessionScopeFactoryNamedExtractor = storageName => configurator.Configuration.Container.Get<ISessionScopeFactory>(storageName);
            storage.Configure(new DomainConfigurator(configurator.Configuration).Named(name));
            return configurator;
        }        

        public static IAntlerConfigurator UnUseStorage(this IAntlerConfigurator configurator)
        {
            UnitOfWork.SessionScopeFactoryExtractor = null;
            return configurator;
        }

        public static IAntlerConfigurator UnUseNamedStorage(this IAntlerConfigurator configurator)
        {
            UnitOfWork.SessionScopeFactoryNamedExtractor = null;
            return configurator;
        }
    }    
}
