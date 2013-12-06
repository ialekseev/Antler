using System;

namespace SmartElk.Antler.Core.Abstractions
{
    /// <summary>
    /// Resolver using a factory function.
    /// </summary>
    public class DynamicResolver : IImplementationResolver
    {
        /// <summary>
        /// Factory function returning component implementation.
        /// </summary>
        public Func<object> FactoryFunc { get; protected set; }

        internal DynamicResolver(Func<object> factoryFunc)
        {
            FactoryFunc = factoryFunc;
        }
    }
}
