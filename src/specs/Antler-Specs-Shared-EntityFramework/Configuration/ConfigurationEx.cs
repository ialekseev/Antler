using SmartElk.Antler.Core.Abstractions.Configuration;
using SmartElk.Antler.Core.Domain;
using SmartElk.Antler.Core.Domain.Configuration;
using SmartElk.Antler.EntityFramework;

namespace SmartElk.Antler.Specs.Shared.EntityFramework.Configuration
{
    public static class ConfigurationEx
    {
        public static void ClearDatabase(this IAntlerConfigurator configurator, string storageName = null)
        {
            var context = ((EntityFrameworkSessionScopeFactory)configurator.Configuration.Container.GetWithNameOrDefault<ISessionScopeFactory>(storageName)).CreateContext();
            context.Database.Delete();
            context.Database.Create();
        }        
    }
}
