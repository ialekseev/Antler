using System.Reflection;
using SmartElk.Antler.Domain.Configuration;

namespace Antler.NHibernate.Configuration
{
    public interface INHibernateStorage: IStorage
    {
        INHibernateStorage WithMappings(Assembly assemblyWithMappings);
    }
}
