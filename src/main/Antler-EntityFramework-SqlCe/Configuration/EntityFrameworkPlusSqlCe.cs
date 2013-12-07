using System.Configuration;
using System.Data;
using System.Data.Entity;
using System.Linq;
using SmartElk.Antler.Core.Domain.Configuration;
using SmartElk.Antler.EntityFramework.Configuration;

namespace SmartElk.Antler.EntityFramework.SqlCe.Configuration
{
    public class EntityFrameworkPlusSqlCe : EntityFrameworkStorage
    {
        public new static IEntityFrameworkStorage Use
        {
           get { return new EntityFrameworkPlusSqlCe();}            
        }

        public override void Configure(IDomainConfigurator configurator)
        {
            DbConfiguration.SetConfiguration(new SqlCeDbConfiguration());
            RegisterDbProviderFactory();
            
            base.Configure(configurator);
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
