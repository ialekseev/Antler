using System.Reflection;
using SmartElk.Antler.Domain.Configuration;

namespace Antler.Hibernate.Configuration
{
    public abstract class HibernateStorage: IHibernateStorage
    {
        protected Assembly AssemblyWithMappings { get; set; }

        protected HibernateStorage()
        {
            AssemblyWithMappings = Assembly.GetCallingAssembly();            
        }
        
        public IHibernateStorage WithMappings(Assembly assemblyWithMappings)
        {
            AssemblyWithMappings = assemblyWithMappings;
            return this;
        }

        public abstract void Configure(IDomainConfigurator configurator);
    }
}
