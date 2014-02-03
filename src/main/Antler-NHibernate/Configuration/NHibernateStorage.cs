using System;
using System.Reflection;
using SmartElk.Antler.Core.Common.CodeContracts;
using SmartElk.Antler.Core.Domain.Configuration;

namespace Antler.NHibernate.Configuration
{
    public abstract class NHibernateStorage<TStorage> : AbstractStorage where TStorage : class                                                                
    {
        protected Assembly AssemblyWithMappings { get; set; }
        protected Action<global::NHibernate.Cfg.Configuration> ActionToApplyOnNHibernateConfiguration;

        protected NHibernateStorage()
        {
            AssemblyWithMappings = Assembly.GetCallingAssembly();            
        }

        public TStorage WithMappings(Assembly assemblyWithMappings)
        {
            Requires.NotNull(assemblyWithMappings, "assemblyWithMappings");
            AssemblyWithMappings = assemblyWithMappings;
            return this as TStorage;
        }

        public TStorage ApplyOnNHibernateConfiguration(Action<global::NHibernate.Cfg.Configuration> actionToApplyOnNHibernateConfiguration)
        {
            Requires.NotNull(actionToApplyOnNHibernateConfiguration, "actionToApplyOnNHibernateConfiguration");
            ActionToApplyOnNHibernateConfiguration = actionToApplyOnNHibernateConfiguration;
            return this as TStorage;
        }
    }
}
