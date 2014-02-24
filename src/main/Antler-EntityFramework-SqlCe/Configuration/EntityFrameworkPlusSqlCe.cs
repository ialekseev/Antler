using System.Configuration;
using System.Data;
using System.Data.Entity;
using System.Data.Entity.SqlServerCompact;
using System.Linq;
using SmartElk.Antler.Core.Common.CodeContracts;
using SmartElk.Antler.Core.Domain.Configuration;
using SmartElk.Antler.EntityFramework.Configuration;

namespace SmartElk.Antler.EntityFramework.SqlCe.Configuration
{
    public class EntityFrameworkPlusSqlCe : EntityFrameworkStorage
    {
        public new static EntityFrameworkStorage Use
        {
           get { return new EntityFrameworkPlusSqlCe();}            
        }

        public override void Configure(IDomainConfigurator configurator)
        {
            Requires.NotNull(configurator, "configurator");

            DbConfiguration.SetConfiguration(new SqlCeDbConfiguration());
            RegisterDbProviderFactory();
            
            base.Configure(configurator);
        }

        private static void RegisterDbProviderFactory()//todo: find the better way
        {
            var data = (DataSet)ConfigurationManager.GetSection("system.data");
            var providerFactories = data.Tables["DbProviderFactories"];
            
            var alreadyRegistered = providerFactories.Rows.Cast<DataRow>().Any(rowTyped => rowTyped.ItemArray.Any(t => t.ToString().Contains(SqlCeProviderServices.ProviderInvariantName)));
            if (!alreadyRegistered)
            {
                providerFactories.Rows.Add(SqlCeProviderServices.ProviderInvariantName, SqlCeProviderServices.ProviderInvariantName, SqlCeProviderServices.ProviderInvariantName, "System.Data.SqlServerCe.SqlCeProviderFactory, System.Data.SqlServerCe");
            }
        }
    }
}
