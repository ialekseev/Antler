using System.Reflection;
using SmartElk.Antler.Core.Domain.Configuration;

namespace Antler.NHibernate.Configuration
{
    public abstract class NHibernateStorage<TStorage> : AbstractStorage where TStorage : class                                                                
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
    }
}
