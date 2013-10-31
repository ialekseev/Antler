using System.Configuration;
using System.Data;
using System.Data.Entity;
using System.Linq;
using SmartElk.Antler.Domain.Configuration;
using SmartElk.Antler.EntityFramework.Configuration;

namespace SmartElk.Antler.EntityFramework.SqlCe.Configuration
{
    public static class ConfigurationEx
    {       
        public static EntityFrameworkConfigurator WithEntityFrameworkPlusSqlCe(this IDomainConfigurator domainConfigurator)
        {                        
            DbConfiguration.SetConfiguration(new SqlCeDbConfiguration());
            RegisterDbProviderFactory();
                                    
            return EntityFrameworkConfigurator.Create(domainConfigurator);                        
        }        

        private static void RegisterDbProviderFactory()//todo: find the better way
        {
            var data = (DataSet)ConfigurationManager.GetSection("system.data");
            var providerFactories = data.Tables["DbProviderFactories"];
            var alreadyRegistered = providerFactories.Rows.Cast<DataRow>().Any(rowTyped => rowTyped[0].ToString().Contains("System.Data.SqlServerCe.4.0"));
            if (!alreadyRegistered)
            {
                providerFactories.Rows.Add("System.Data.SqlServerCe.4.0", "System.Data.SqlServerCe.4.0", "System.Data.SqlServerCe.4.0", "System.Data.SqlServerCe.SqlCeProviderFactory, System.Data.SqlServerCe");
            }                                    
        }
    }
}
