using System.Reflection;
using SmartElk.Antler.Domain.Configuration;

namespace SmartElk.Antler.Hibernate.Sqlite.Configuration
{
    public static class ConfigurationEx
    {
        public static HibernateSqliteConfigurator WithMappings(this IDomainConfigurator domainConfigurator, Assembly assemblyWithMappings)
        {
            return new HibernateSqliteConfigurator(domainConfigurator, assemblyWithMappings);
        } 
                
        public static AsInMemoryStorageResult AsInMemoryStorage(this IDomainConfigurator domainConfigurator)
        {
            var hibernateSqliteConfigurator = new HibernateSqliteConfigurator(domainConfigurator, Assembly.GetCallingAssembly());
            return hibernateSqliteConfigurator.AsInMemoryStorage();
        }        
    }
}
