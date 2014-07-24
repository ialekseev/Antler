using System;
using System.Linq;
using SmartElk.Antler.Core.Abstractions;
using SmartElk.Antler.Core.Common.CodeContracts;
using SmartElk.Antler.Core.Common.Extensions;
using StructureMap.Configuration.DSL.Expressions;
using StructureMap.Pipeline;

namespace SmartElk.Antler.StructureMap
{
    public static class StructureMapContainerRegistrator
    {
        public static void Register(global::StructureMap.IContainer container, SingleBinding binding)
        {
			container.Configure(ce =>
				{
					var registration = ce.For(binding.Service);

					registration = ApplyLifestyleSingle(registration, binding.Lifestyle);
				    
                    var unnamed = ApplyResolver(registration, binding.Resolver);

					ApplyName(unnamed, binding.Name);
				});

        }

		public static void Register(global::StructureMap.IContainer container, MultipleBinding binding)
		{
			container.Configure(ce =>
				{
					foreach (var implementation in binding.Services.Where(t => !t.IsInterface && !t.IsAbstract))
					{
						var tempImplementation = implementation;
						binding.BindTo.ForEach(t => ApplyLifestyleSingle(ce.For(t), binding.Lifestyle).Add(tempImplementation));

						if (binding.BindTo.Count == 0)
						{
							ApplyLifestyleSingle(ce.For(implementation), binding.Lifestyle).Add(implementation);
						}
					}
				});
		}

        //todo: very weirdly, after updating StructureMap to version 3, double dispatch via dynamic not works anymore 
        private static object ApplyResolver(GenericFamilyExpression builder, IImplementationResolver resolver)
        {
            if (resolver is StaticResolver)
                return builder.Add(((StaticResolver)resolver).Target);
           if (resolver is InstanceResolver)
                return builder.Add(((InstanceResolver)resolver).Instance);
           if (resolver is DynamicResolver)
                return builder.Add(c => ((DynamicResolver)resolver).FactoryFunc());
           
            Assumes.Fail("Unsupported IImplementationResolver implementation");
           return null;
        }
        
        /*private static ConfiguredInstance ApplyResolver(GenericFamilyExpression builder, StaticResolver resolver)
		{
			return builder.Add(resolver.Target);
		}

		private static ObjectInstance ApplyResolver(GenericFamilyExpression builder, InstanceResolver resolver)
		{
			return builder.Add(resolver.Instance);
		}

		private static LambdaInstance<object> ApplyResolver(GenericFamilyExpression builder, DynamicResolver resolver)
		{
			return builder.Add(c => resolver.FactoryFunc());
		}*/

		private static GenericFamilyExpression ApplyLifestyleSingle(GenericFamilyExpression registration, Lifestyle lifestyle)
        {
            if (lifestyle.Name == Lifestyle.Singleton.Name)
                return registration.Singleton();

            if (lifestyle.Name == Lifestyle.Transient.Name)
                return registration.LifecycleIs(new TransientLifecycle());

		    if (lifestyle.Name == Lifestyle.PerWebRequest.Name)
		        return registration.LifecycleIs(new UniquePerRequestLifecycle());

			if (lifestyle.Name == Lifestyle.Unmanaged.Name)
				return registration;

			if (lifestyle.Name == Lifestyle.ProviderDefault.Name)
				return registration;

			if (lifestyle.Name == Lifestyle.Default.Name)
				return registration.Singleton();

			throw new ArgumentException(string.Format("Unknown lifestyle : {0}", lifestyle), "lifestyle");
        }

		private static void ApplyName(object registration, string name)
        {
            if (string.IsNullOrEmpty(name))
                return;

            ((dynamic)registration).Named(name);
        }

    }
}