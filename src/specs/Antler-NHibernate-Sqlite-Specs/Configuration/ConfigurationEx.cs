using Antler.NHibernate;
using NHibernate;
using NHibernate.Tool.hbm2ddl;
using SmartElk.Antler.Abstractions.Configuration;
using SmartElk.Antler.Common.Reflection;
using SmartElk.Antler.Domain;
using SmartElk.Antler.Domain.Configuration;
using SmartElk.Antler.NHibernate.Sqlite.Configuration;

namespace SmartElk.Antler.NHibernate.Specs.Configuration
{
    public static class ConfigurationEx
    {
        public static ISession CreateNHibernateSession(this IAntlerConfigurator configurator, string storageName = null)
        {
            var nhConfigurationResult = typeof(NHibernatePlusSqlite).AsStaticMembersDynamicWrapper().LatestConfigurationResult;

            var session = nhConfigurationResult.SessionFactory.OpenSession();
            new SchemaExport(nhConfigurationResult.Configuration).Execute(false, true, false, session.Connection, null);
            var sessionScopeFactory = (ISessionScopeFactoryEx)configurator.Configuration.Container.GetWithNameOrDefault<ISessionScopeFactory>(storageName);
            sessionScopeFactory.SetSession(session);

            return session;
        }

        public static void ResetNHibernateSession(this IAntlerConfigurator configurator, ISession session, string storageName = null)
        {
            session.Dispose();
            var sessionScopeFactory = (ISessionScopeFactoryEx)configurator.Configuration.Container.GetWithNameOrDefault<ISessionScopeFactory>(storageName);
            sessionScopeFactory.ResetSession(); 
        }
    }
}
