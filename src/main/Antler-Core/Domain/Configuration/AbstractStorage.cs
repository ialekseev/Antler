using System.ComponentModel;
using System.Reflection;
using SmartElk.Antler.Core.Common.CodeContracts;

namespace SmartElk.Antler.Core.Domain.Configuration
{
    public abstract class AbstractStorage<TStorage> : IStorage where TStorage: class
    {
        protected Assembly AssemblyWithMappings { get; set; }

        [EditorBrowsable(EditorBrowsableState.Never)]
        public abstract void Configure(IDomainConfigurator configurator);

        protected AbstractStorage()
        {
            AssemblyWithMappings = Assembly.GetCallingAssembly();
        }

        public TStorage WithMappings(Assembly assemblyWithMappings)
        {
            Requires.NotNull(assemblyWithMappings, "assemblyWithMappings");

            AssemblyWithMappings = assemblyWithMappings;
            return this as TStorage;
        }
        
        public TStorage WithMappings(string assemblyWithMappings)
        {
            Requires.NotNullOrEmpty(assemblyWithMappings, "assemblyWithMappings");

            AssemblyWithMappings = Assembly.Load(assemblyWithMappings);
            return this as TStorage;
        }
    }
}
