using Castle.Windsor;
using System;
using SmartElk.Antler.Core.Abstractions;

namespace SmartElk.Antler.Windsor
{
    public static class ContainerEx
    {
        public static void OnNative(this IContainer provider,  Action<IWindsorContainer> nativeAction)
        {
            if (!(provider is WindsorContainerAdapter))
                throw new InvalidOperationException("Exprected WindsorContainerAdapter, but was {0}" + provider.GetType().Name);

            nativeAction(((WindsorContainerAdapter) provider).NativeContainer);
        }
    }
}