using System.Data.Entity;
using System.Reflection;
using SmartElk.Antler.Domain.Configuration;
using SmartElk.Antler.EntityFramework.Internal;

namespace SmartElk.Antler.EntityFramework.Configuration
{
    public interface IEntityFrameworkStorage: IStorage
    {
        IEntityFrameworkStorage WithConnectionString(string connectionString);
        IEntityFrameworkStorage WithMappings(Assembly assembly);
        IEntityFrameworkStorage WithDatabaseInitializer(IDatabaseInitializer<DataContext> databaseInitializer);
        IEntityFrameworkStorage WithLazyLoading();
        IEntityFrameworkStorage WithoutLazyLoading();
    }
}
