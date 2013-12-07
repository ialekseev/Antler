using System;

namespace SmartElk.Antler.Core.Abstractions
{
    public class StaticResolver : IImplementationResolver
    {
        public Type Target { get; protected set; }

        internal StaticResolver(Type target)
        {
            Target = target;
        }
    }
}
