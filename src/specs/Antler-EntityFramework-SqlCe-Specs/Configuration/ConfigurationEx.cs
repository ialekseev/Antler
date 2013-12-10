using SmartElk.Antler.Core.Abstractions.Configuration;
using SmartElk.Antler.Core.Domain;
using SmartElk.Antler.Core.Domain.Configuration;

namespace SmartElk.Antler.EntityFramework.SqlCe.Specs.Configuration
{
    public static class ConfigurationEx
    {
        public static void RecreateEntityFrameworkDatabase(this IAntlerConfigurator configurator, string storageName=null)
        {
            ((ISessionScopeFactoryEx)configurator.Configuration.Container.GetWithNameOrDefault<ISessionScopeFactory>(storageName)).CreateDataContext().Clear();
        }        
    }
}
