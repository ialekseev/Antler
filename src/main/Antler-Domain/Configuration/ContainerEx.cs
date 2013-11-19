using SmartElk.Antler.Abstractions;
using SmartElk.Antler.Abstractions.Registration;

namespace SmartElk.Antler.Domain.Configuration
{
    public static class ContainerEx
    {
        public static void PutWithNameOrDefault<T>(this IContainer container, T obj, string name) where T:class
        {
            var hasName = !string.IsNullOrEmpty(name);
            container.Put(hasName ? Binding.Use(obj).As<ISessionScopeFactory>().Named(name) : Binding.Use(obj).As<ISessionScopeFactory>());
        }

        public static T GetWithNameOrDefault<T>(this IContainer container, string name) where T : class
        {
            var hasName = !string.IsNullOrEmpty(name);
            return hasName ? container.Get<T>(name) : container.Get<T>();
        }
    }
}
