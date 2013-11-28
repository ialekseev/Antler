using SmartElk.Antler.Abstractions.Configuration;
using SmartElk.Antler.Domain;
using SmartElk.Antler.Domain.Configuration;

namespace SmartElk.Antler.EntityFramework.Sqlite.Specs.Configuration
{
    public static class ConfigurationEx
    {
        public static void RecreateEntityFrameworkDatabase(this IAntlerConfigurator configurator, string storageName=null)
        {
            ((ISessionScopeFactoryEx)configurator.Configuration.Container.GetWithNameOrDefault<ISessionScopeFactory>(storageName)).CreateDataContext().Clear();
        }        
    }
}
