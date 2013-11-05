using System.Reflection;
using SmartElk.Antler.Domain.Configuration;

namespace SmartElk.Antler.Hibernate.Sqlite.Configuration
{
    public static class ConfigurationEx
    {
        public static HibernateSqliteConfigurator WithNHibernatePlusSqlite(this IDomainConfigurator domainConfigurator, Assembly assemblyWithMappings)
        {
            return new HibernateSqliteConfigurator(domainConfigurator, assemblyWithMappings);
        }

        public static HibernateSqliteConfigurator WithNHibernatePlusSqlite(this IDomainConfigurator domainConfigurator)
        {
            return new HibernateSqliteConfigurator(domainConfigurator, Assembly.GetCallingAssembly());                        
        }        
    }
}
