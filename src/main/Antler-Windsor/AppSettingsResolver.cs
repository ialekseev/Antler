using System;
using System.Configuration;
using Castle.Core;
using Castle.MicroKernel;
using Castle.MicroKernel.Context;
using SmartElk.Antler.Core.Common;

namespace SmartElk.Antler.Windsor
{
    public class AppSettingsResolver : ISubDependencyResolver
    {
        private readonly IKernel _kernel;

        public AppSettingsResolver(IKernel kernel)
        {            
            _kernel = kernel;
        }

        public string Key(ComponentModel model, DependencyModel dependency)
        {            
            return string.Format("{0}.{1}", model.Implementation.Name, dependency.DependencyKey);
        }

        public object Resolve(CreationContext context, ISubDependencyResolver parentResolver, ComponentModel model, DependencyModel dependency)
        {
            var key = Key(model, dependency);
            var value = ConfigurationManager.AppSettings[key];

            if (dependency.TargetType.IsValueType || dependency.TargetType == typeof(string))
                return Convert.ChangeType(value, dependency.TargetType);

            if (_kernel.HasComponent(value)) return _kernel.Resolve(value, dependency.TargetType);

            var possibleType = Type.GetType(value);
            if (_kernel.HasComponent(possibleType)) return _kernel.Resolve(possibleType);

            throw new ConfigurationErrorsException("Unable to resolve dependency {0} from AppSettings key {1}".FormatWith(dependency, key));
        }

        public bool CanResolve(CreationContext context, ISubDependencyResolver parentResolver, ComponentModel model, DependencyModel dependency)
        {
            var key = Key(model, dependency);
            return !string.IsNullOrEmpty(ConfigurationManager.AppSettings[key]);
        }
    }
}
