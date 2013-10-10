using System;

namespace SmartElk.Antler.Abstractions
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
