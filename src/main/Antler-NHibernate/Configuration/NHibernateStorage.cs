using System.Reflection;
using SmartElk.Antler.Core.Domain.Configuration;

namespace Antler.NHibernate.Configuration
{
    public abstract class NHibernateStorage: INHibernateStorage
    {
        protected Assembly AssemblyWithMappings { get; set; }

        protected NHibernateStorage()
        {
            AssemblyWithMappings = Assembly.GetCallingAssembly();            
        }
        
        public INHibernateStorage WithMappings(Assembly assemblyWithMappings)
        {
            AssemblyWithMappings = assemblyWithMappings;
            return this;
        }

        public abstract void Configure(IDomainConfigurator configurator);
    }
}
