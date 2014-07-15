using System;
using NHibernate;
using NHibernate.Tool.hbm2ddl;
using SmartElk.Antler.Core.Abstractions.Configuration;
using SmartElk.Antler.Core.Common.Reflection;
using SmartElk.Antler.Core.Domain;

namespace SmartElk.Antler.NHibernate.Internal
{
    public class NewSessionForTesting
    {
        public static ISession CreateNHibernateSession(IAntlerConfigurator configurator, Type storageType, string storageName)
        {
            var nhConfigurationResult = storageType.AsStaticMembersDynamicWrapper().LatestConfigurationResult;

            var session = nhConfigurationResult.SessionFactory.OpenSession();

            var schemaExport = new SchemaExport(nhConfigurationResult.Configuration);
            schemaExport.Drop(true, true);
            schemaExport.Execute(false, true, false, session.Connection, null);

            var sessionScopeFactory = (ISessionScopeFactoryEx)configurator.Configuration.Container.Get<ISessionScopeFactory>(storageName);
            sessionScopeFactory.SetSession(session);

            return session;
        }

        public static void ResetNHibernateSession(IAntlerConfigurator configurator, ISession session, string storageName)
        {
            session.Dispose();
            var sessionScopeFactory = (ISessionScopeFactoryEx)configurator.Configuration.Container.Get<ISessionScopeFactory>(storageName);
            sessionScopeFactory.ResetSession();
        }
    }
}
