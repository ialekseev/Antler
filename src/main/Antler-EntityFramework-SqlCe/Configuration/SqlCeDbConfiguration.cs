using System.Data.Entity;
using System.Data.Entity.Infrastructure;
using System.Data.Entity.SqlServerCompact;


namespace SmartElk.Antler.EntityFramework.SqlCe.Configuration
{
    public class SqlCeDbConfiguration : DbConfiguration
    {
        public SqlCeDbConfiguration()
        {            
            SetProviderServices(
                SqlCeProviderServices.ProviderInvariantName,
                SqlCeProviderServices.Instance);

            SetDefaultConnectionFactory(new SqlCeConnectionFactory(SqlCeProviderServices.ProviderInvariantName));
        }
    }
}
