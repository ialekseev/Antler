using System;
using SmartElk.Antler.Core.Abstractions;

namespace SmartElk.Antler.StructureMap
{
    public static class ContainerEx
    {
        public static void OnNative(this IContainer provider,  Action<global::StructureMap.IContainer> nativeAction)
        {
            if (!(provider is StructureMapContainerAdapter))
                throw new InvalidOperationException("Exprected StructureMapContainerAdapter, but was {0}" + provider.GetType().Name);

			nativeAction(((StructureMapContainerAdapter)provider).NativeContainer);
        }
    }
}