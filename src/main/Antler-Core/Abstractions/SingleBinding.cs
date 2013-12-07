using System;

namespace SmartElk.Antler.Core.Abstractions
{
    /// <summary>
    /// Single component type binding specification.
    /// </summary>
    public class SingleBinding : LifestyleBasedBinding
    {
        /// <summary>
        /// Component service type.
        /// </summary>
        public Type Service { get; internal set; }
        /// <summary>
        /// Implementation resolver.
        /// </summary>
        public IImplementationResolver Resolver { get; internal set; }
        /// <summary>
        /// Optional name for component.
        /// </summary>
        public string Name { get; internal set; }

        internal SingleBinding()
        {
        }

    }
}
