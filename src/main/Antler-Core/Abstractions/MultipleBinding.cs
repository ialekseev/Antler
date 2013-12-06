using System;
using System.Collections.Generic;
using SmartElk.Antler.Core.Common;

namespace SmartElk.Antler.Core.Abstractions
{
    /// <summary>
    /// Binding configuration for multiple components registration.
    /// </summary>
    public class MultipleBinding : LifestyleBasedBinding
    {
        /// <summary>
        /// Components service types.
        /// </summary>
        public IEnumerable<Type> Services { get; private set; }
        /// <summary>
        /// Should use implementation type as service type.
        /// </summary>
        internal bool? BindAsSelf { get; private set; }
        /// <summary>
        /// Optional service types.
        /// </summary>
        public ISet<Type> BindTo { get; private set; }

        internal MultipleBinding(IEnumerable<Type> services)
        {
            Services = services;
            BindTo = new HashSet<Type>();
        }

        internal void AsSelf()
        {
            BindAsSelf = true;
        }

        internal void ForwardTo<TService>() where TService : class
        {
            BindTo.Add(typeof(TService));
        }

        internal void ForwardTo(params Type[] services)
        {
            services.ForEach(t => BindTo.Add(t));
        }
    }
}
