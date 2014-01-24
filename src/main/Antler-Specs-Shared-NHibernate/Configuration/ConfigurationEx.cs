using System;
using Antler.NHibernate;
using NHibernate;
using NHibernate.Tool.hbm2ddl;
using SmartElk.Antler.Core.Abstractions.Configuration;
using SmartElk.Antler.Core.Common.Reflection;
using SmartElk.Antler.Core.Domain;
using SmartElk.Antler.Core.Domain.Configuration;

namespace SmartElk.Antler.Specs.Shared.NHibernate.Configuration
{
    public static class ConfigurationEx
    {
        public static ISession CreateNHibernateSession(this IAntlerConfigurator configurator, Type storageType, string storageName = null)
        {
            var nhConfigurationResult = storageType.AsStaticMembersDynamicWrapper().LatestConfigurationResult;

            var session = nhConfigurationResult.SessionFactory.OpenSession();

            //todo: How to actually create database here? Like in EF case
            var schemaExport = new SchemaExport(nhConfigurationResult.Configuration);            
            schemaExport.Drop(true, true);
            schemaExport.Execute(false, true, false, session.Connection, null);            

            //var schemaExport = new SchemaUpdate(nhConfigurationResult.Configuration);            
            //schemaExport.Execute(false, true);

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
