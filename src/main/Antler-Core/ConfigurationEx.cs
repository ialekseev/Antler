using SmartElk.Antler.Core.Abstractions;
using SmartElk.Antler.Core.Abstractions.Configuration;
using SmartElk.Antler.Core.Common.CodeContracts;
using SmartElk.Antler.Core.Domain;
using SmartElk.Antler.Core.Domain.Configuration;

namespace SmartElk.Antler.Core
{
    public static class ConfigurationEx
    {
        public static string NoContainerMessage =
            "Please choose some IoC container(e.g. use adapters for Castle Windsor, StructureMap or Built-in container)";

        /// <summary>
        /// Start to configure ORM/database using this method
        /// </summary>             
        public static IAntlerConfigurator UseStorage(this IAntlerConfigurator configurator, IStorage storage)
        {
            Requires.NotNull(storage, "storage", "Storage can't be null");
            Assumes.True<ContainerRequiredException>(configurator.HasContainer(), NoContainerMessage);                                    
            
            UnitOfWork.SessionScopeFactoryExtractor = () => configurator.Configuration.Container.Get<ISessionScopeFactory>();            
            storage.Configure(new DomainConfigurator(configurator.Configuration));
            return configurator;
        }

        /** <summary>
          Start to configure named ORM/database using this method. 
          Why named? If you have several different ORMs/databases in your application, you may need to distinguish them by name when using UnitOfWork
           </summary> */            
        public static IAntlerConfigurator UseStorageNamed(this IAntlerConfigurator configurator,  IStorage storage, string name)
        {
            Requires.NotNull(storage, "storage", "Storage can't be null");
            Requires.NotNull(storage, "storage", "Storage name can't be null");
            Assumes.True<ContainerRequiredException>(configurator.HasContainer(), NoContainerMessage);
            
            UnitOfWork.SessionScopeFactoryNamedExtractor = storageName => configurator.Configuration.Container.Get<ISessionScopeFactory>(storageName);
            storage.Configure(new DomainConfigurator(configurator.Configuration).Named(name));
            return configurator;
        }

        /** <summary>
         Set UnitOfWork settings that will be used by default. 
         For example, to disable commits or to rollback transaction on UnitOfWork completion(could be useful when writing Integration tests)
           </summary>*/    
        public static IAntlerConfigurator SetUnitOfWorkDefaultSettings(this IAntlerConfigurator configurator,
                                                                 UnitOfWorkSettings settings)
        {
            UnitOfWorkSettings.Default = settings;
            return configurator;
        }

        /// <summary>
        /// Reset current storage(mainly used in Unit/Integration testing)
        /// </summary>        
        public static IAntlerConfigurator UnUseStorage(this IAntlerConfigurator configurator)
        {
            UnitOfWork.SessionScopeFactoryExtractor = null;
            return configurator;
        }

        /// <summary>
        /// Reset current named storages(mainly used in Unit/Integration testing)
        /// </summary>        
        public static IAntlerConfigurator UnUseNamedStorage(this IAntlerConfigurator configurator)
        {
            UnitOfWork.SessionScopeFactoryNamedExtractor = null;
            return configurator;
        }

        /// <summary>
        /// Reset UnitOfWork default settings(mainly used in Unit/Integration testing)
        /// </summary>        
        public static IAntlerConfigurator UnSetUnitOfWorkDefaultSettings(this IAntlerConfigurator configurator)
        {
            UnitOfWorkSettings.Default = new UnitOfWorkSettings();
            return configurator;
        }

        /// <summary>
        /// Reset current IoC container(mainly used in Unit/Integration testing)
        /// </summary>        
        public static IAntlerConfigurator UnUseContainer(this IAntlerConfigurator nodeConfigurator)
        {
            ((IAntlerConfiguratorEx)nodeConfigurator).UnSetContainerAdapter();
            return nodeConfigurator;
        }
    }    
}
