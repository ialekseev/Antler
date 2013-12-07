using System;
using System.Collections;
using System.Collections.Generic;
using Castle.Facilities.Startable;
using Castle.MicroKernel;
using Castle.MicroKernel.Resolvers.SpecializedResolvers;
using Castle.Windsor;
using SmartElk.Antler.Core.Abstractions;
using SmartElk.Antler.Core.Abstractions.Registration;

namespace SmartElk.Antler.Windsor
{
    public class WindsorContainerAdapter: IContainer
    {
        public IWindsorContainer NativeContainer { get; protected set; }

        public WindsorContainerAdapter(IWindsorContainer container)
        {
            NativeContainer = container;

            NativeContainer.Kernel.ReleasePolicy = new UnmanagedReleasePolicy(NativeContainer.Kernel);
            NativeContainer.Kernel.Resolver.AddSubResolver(new ListResolver(NativeContainer.Kernel));
            NativeContainer.Kernel.Resolver.AddSubResolver(new AppSettingsResolver(NativeContainer.Kernel));

            NativeContainer.AddFacility<StartableFacility>();
        }

        public WindsorContainerAdapter()
            : this(new WindsorContainer())
        {
        }

        public IList<T> GetAll<T>()
        {
            return NativeContainer.ResolveAll<T>();
        }

        public IList GetAll(Type type)
        {
            return NativeContainer.ResolveAll(type);
        }

        public T Get<T>()
        {
            try
            {
                return NativeContainer.Resolve<T>();
            }
            catch (ComponentNotFoundException ex)
            {
                throw new BindingNotRegisteredException(typeof(T), ex);
            }
        }

        public T Get<T>(string name)
        {
            try
            {
                return NativeContainer.Resolve<T>(name);
            }
            catch (ComponentNotFoundException ex)
            {
                throw new BindingNotRegisteredException(typeof(T), name, ex);
            }
        }

        public object Get(Type type)
        {
            try
            {
                return NativeContainer.Resolve(type);
            }
            catch (ComponentNotFoundException ex)
            {
                throw new BindingNotRegisteredException(type, ex);
            }
        }

        public object Get(Type type, string name)
        {
            try
            {
                return NativeContainer.Resolve(name, type);
            }
            catch (ComponentNotFoundException ex)
            {
                throw new BindingNotRegisteredException(type, name, ex);
            }
        }

        public void Release(object instance)
        {
            NativeContainer.Release(instance);
        }

        public void Put(IBindingSyntax binding)
        {
            WindsorContainerRegistrator.Register(NativeContainer, (dynamic)binding.Binding);
        }

        public bool Has<T>()
        {
            return NativeContainer.Kernel.HasComponent(typeof(T));
        }

        public bool Has(Type type)
        {
            return NativeContainer.Kernel.HasComponent(type);
        }

        public bool Has<T>(string name)
        {
            return NativeContainer.Kernel.HasComponent(name);
        }

        public bool Has(Type type, string name)
        {
            return NativeContainer.Kernel.HasComponent(name);
        }

        public void Dispose()
        {
            NativeContainer.Dispose();
        }
    }
}
