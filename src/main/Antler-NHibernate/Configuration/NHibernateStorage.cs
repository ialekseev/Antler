using System.Reflection;
using SmartElk.Antler.Core.Domain.Configuration;

namespace Antler.NHibernate.Configuration
{
    public abstract class NHibernateStorage<TStorage> : INHibernateStorage<TStorage> where TStorage: class, IStorage                                                                 
    {
        protected Assembly AssemblyWithMappings { get; set; }

        protected NHibernateStorage()
        {
            AssemblyWithMappings = Assembly.GetCallingAssembly();            
        }

        public TStorage WithMappings(Assembly assemblyWithMappings)
        {
            AssemblyWithMappings = assemblyWithMappings;
            return this as TStorage;
        }

        public abstract void Configure(IDomainConfigurator configurator);
    }
}
