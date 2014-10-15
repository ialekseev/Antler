using System.Reflection;
using SmartElk.Antler.Core.Common.CodeContracts;

namespace SmartElk.Antler.Core.Domain.Configuration
{
    public abstract class AbstractOrmStorage<TStorage> : AbstractRelationalStorage<TStorage> where TStorage : class
    {
        protected Assembly AssemblyWithMappings { get; set; }

        protected AbstractOrmStorage()
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
