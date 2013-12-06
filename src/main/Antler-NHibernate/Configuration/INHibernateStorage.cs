using System.Reflection;
using SmartElk.Antler.Core.Domain.Configuration;

namespace Antler.NHibernate.Configuration
{
    public interface INHibernateStorage: IStorage
    {
        INHibernateStorage WithMappings(Assembly assemblyWithMappings);
    }
}
