using System.Collections;
using System;
using System.Collections.Generic;
using System.Linq;
using SmartElk.Antler.Core.Abstractions;
using SmartElk.Antler.Core.Abstractions.Registration;
using StructureMap;
using IContainer = SmartElk.Antler.Core.Abstractions.IContainer;

namespace SmartElk.Antler.StructureMap
{
    public class StructureMapContainerAdapter : IContainer
    {
        public global::StructureMap.IContainer NativeContainer { get; protected set; }

		public StructureMapContainerAdapter(global::StructureMap.IContainer container)
        {
            NativeContainer = container;
        }

		public StructureMapContainerAdapter()
		{
			NativeContainer = new Container();
		}

        public IList<T> GetAll<T>()
        {
			return NativeContainer.GetAllInstances<T>();
        }

		public IList GetAll(Type type)
		{
			return NativeContainer.GetAllInstances(type);
		}
		
		public T Get<T>()
        {
			if (!Has<T>())
				throw new BindingNotRegisteredException(typeof(T), null);

			try
			{
				return NativeContainer.GetInstance<T>();
			}
			catch (StructureMapException ex)
			{
				throw new BindingNotRegisteredException(typeof(T), ex);
			}
        }

        public T Get<T>(string name)
        {
			if (!Has<T>(name))
				throw new BindingNotRegisteredException(typeof(T), name, null);

			try
	        {
				return NativeContainer.GetInstance<T>(name);
	        }
			catch (StructureMapException ex)
	        {
		        throw new BindingNotRegisteredException(typeof(T), name, ex);
	        }
		}

        public object Get(Type type)
        {
			if (!Has(type))
				throw new BindingNotRegisteredException(type, null);

			try
			{
				return NativeContainer.GetInstance(type);
			}
			catch (StructureMapException ex)
			{
				throw new BindingNotRegisteredException(type, ex);
			}
		}

        public object Get(Type type, string name)
        {
			if (!Has(type, name))
				throw new BindingNotRegisteredException(type, name, null);

			try
			{
				return NativeContainer.GetInstance(type, name);
			}
			catch (StructureMapException ex)
			{
				throw new BindingNotRegisteredException(type, name, ex);
			}
		}

        public void Release(object instance)
        {
        }

		public void Put(IBindingSyntax binding)
        {
            StructureMapContainerRegistrator.Register(NativeContainer, (dynamic)binding.Binding);
        }

        public bool Has<T>()
        {
            return NativeContainer.Model.HasImplementationsFor<T>();
        }

		public bool Has(Type type)
		{
			return NativeContainer.Model.HasImplementationsFor(type);
		}

		public bool Has<T>(string name)
		{
			return NativeContainer.Model.InstancesOf<T>().Any(x => x.Name == name);
		}

		public bool Has(Type type, string name)
		{
			return NativeContainer.Model.InstancesOf(type).Any(x => x.Name == name);
		}

        public void Dispose()
        {
            NativeContainer.Dispose();
        }
    }
}