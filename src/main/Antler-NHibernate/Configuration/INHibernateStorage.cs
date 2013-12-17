using System.Reflection;
using SmartElk.Antler.Core.Domain.Configuration;

namespace Antler.NHibernate.Configuration
{
    public interface INHibernateStorage<out TStorage> : IStorage
    {
        TStorage WithMappings(Assembly assemblyWithMappings);
    }
}
