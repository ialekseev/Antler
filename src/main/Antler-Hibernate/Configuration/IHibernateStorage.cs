using System.Reflection;
using SmartElk.Antler.Domain.Configuration;

namespace Antler.Hibernate.Configuration
{
    public interface IHibernateStorage: IStorage
    {
        IHibernateStorage WithMappings(Assembly assemblyWithMappings);
    }
}
