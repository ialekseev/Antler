using System;
using System.Collections.Generic;

namespace SmartElk.Antler.Core.Abstractions.Registration
{
    /// <summary>
    /// Define component(s) binding specification.
    /// </summary>
    public static class Binding
    {
        /// <summary>
        /// Register single component using concrete type.
        /// </summary>
        /// <param name="implementation">Component implementation type.</param>
        /// <returns>Additional registration options.</returns>
        public static ISingleBindingSyntax Use(Type implementation)
        {
            return new SingleBindingSyntax(implementation, new StaticResolver(implementation));
        }

        /// <summary>
        /// Register single component using concrete type.
        /// </summary>
        /// <typeparam name="TImpl">Component implementation type.</typeparam>
        /// <returns>Additional registration options.</returns>
        public static ISingleBindingSyntax<TImpl> Use<TImpl>() where TImpl : class
        {
            return new SingleBindingSyntax<TImpl>(new StaticResolver(typeof(TImpl)));
        }

        /// <summary>
        /// Register single component using factory method.
        /// </summary>
        /// <typeparam name="TImpl">Component implementation type.</typeparam>
        /// <param name="factoryFunc">Factory method to build component.</param>
        /// <returns>Additional registration options.</returns>
        public static ISingleBindingSyntax<TImpl> Use<TImpl>(Func<TImpl> factoryFunc) where TImpl : class
        {
            return new SingleBindingSyntax<TImpl>(new DynamicResolver(factoryFunc));
        }

        /// <summary>
        /// Register single component using component instance.
        /// </summary>
        /// <typeparam name="TImpl">Component implementation type.</typeparam>
        /// <param name="instance">Component instance.</param>
        /// <returns>Additional registration options.</returns>
        public static ISingleBindingSyntax<TImpl> Use<TImpl>(TImpl instance) where TImpl : class
        {
            return new SingleBindingSyntax<TImpl>(new InstanceResolver(instance));
        }

        /// <summary>
        /// Register multiple components for specific service type.
        /// </summary>
        /// <param name="services">Components service type.</param>
        /// <returns>Additional registration options.</returns>
        public static IMultipleBindingSyntax Use(IEnumerable<Type> services)
        {
            return new MultipleBindingSyntax(services);
        }
    }
}
