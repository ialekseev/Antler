using System;
using Castle.MicroKernel.Registration;
using Castle.Windsor;
using SmartElk.Antler.Core.Abstractions;

namespace SmartElk.Antler.Windsor
{
    public class WindsorContainerRegistrator
    {
        public static void Register(IWindsorContainer container, SingleBinding binding)
        {
            ComponentRegistration<object> registration = Component.For(binding.Service);

            registration = ApplyResolver(registration, (dynamic)binding.Resolver);

            registration = ApplyLifestyle(registration, binding.Lifestyle);

            registration = ApplyName(registration, binding.Name);

            container.Register(registration);
        }

        public static void Register(IWindsorContainer container, MultipleBinding binding)
        {
            var registration = AllTypes.From(binding.Services).Pick();

            if (binding.BindTo.Count > 0)
                registration = registration.WithServices(binding.BindTo);

            registration = ApplyLifestyle(registration, binding.Lifestyle);

            container.Register(registration);
        }

        private static ComponentRegistration<object> ApplyResolver(ComponentRegistration<object> registration, StaticResolver resolver)
        {
            return registration.ImplementedBy(resolver.Target);
        }

        private static ComponentRegistration<object> ApplyResolver(ComponentRegistration<object> registration, InstanceResolver resolver)
        {
            return registration.Instance(resolver.Instance);
        }

        private static ComponentRegistration<object> ApplyResolver(ComponentRegistration<object> registration, DynamicResolver resolver)
        {
            return registration.UsingFactoryMethod(resolver.FactoryFunc);
        }

        private static ComponentRegistration<object> ApplyLifestyle(ComponentRegistration<object> registration, Lifestyle lifestyle)
        {
            if (lifestyle.Name == Lifestyle.Singleton.Name)
                return registration.LifeStyle.Singleton;

            if (lifestyle.Name == Lifestyle.Transient.Name)
                return registration.LifeStyle.Transient;

            if (lifestyle.Name == Lifestyle.PerWebRequest.Name)
                return registration.LifeStyle.PerWebRequest;

            if (lifestyle.Name == Lifestyle.Unmanaged.Name)
                return registration.LifeStyle.Custom<UnmanagedLifestyleManager>();

            if (lifestyle.Name == Lifestyle.Default.Name)
                return registration.LifeStyle.Singleton;

            if (lifestyle.Name == Lifestyle.ProviderDefault.Name)
                return registration;

            throw new ArgumentException(string.Format("Unknown lifestyle : {0}", lifestyle), "lifestyle");
        }

        private static BasedOnDescriptor ApplyLifestyle(BasedOnDescriptor registration, Lifestyle lifestyle)
        {
            if (lifestyle.Name == Lifestyle.Singleton.Name)
                return registration.LifestyleSingleton();

            if (lifestyle.Name == Lifestyle.Transient.Name)
                return registration.LifestyleTransient();

            if (lifestyle.Name == Lifestyle.PerWebRequest.Name)
                return registration.LifestylePerWebRequest();

            if (lifestyle.Name == Lifestyle.Unmanaged.Name)
                return registration.LifestyleCustom<UnmanagedLifestyleManager>();

            if (lifestyle.Name == Lifestyle.Default.Name)
                return registration.LifestyleSingleton();

            if (lifestyle.Name == Lifestyle.ProviderDefault.Name)
                return registration;

            throw new ArgumentException(string.Format("Unknown lifestyle : {0}", lifestyle), "lifestyle");
        }

        private static ComponentRegistration<object> ApplyName(ComponentRegistration<object> registration, string name)
        {
            if (string.IsNullOrEmpty(name))
                return registration;

            return registration.Named(name);
        }

    }
}
