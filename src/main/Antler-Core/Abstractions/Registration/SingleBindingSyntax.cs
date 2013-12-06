using System;

namespace SmartElk.Antler.Core.Abstractions.Registration
{
    internal class SingleBindingSyntax<TImpl> : ISingleBindingSyntax<TImpl> where TImpl : class
    {
        private readonly SingleBinding _binding;
        IBinding IBindingSyntax.Binding { get { return _binding; } }

        public SingleBindingSyntax(IImplementationResolver resolver)
        {
            _binding = new SingleBinding
            {
                Service = typeof(TImpl),
                Resolver = resolver
            };
        }

        public ISingleBindingSyntax<TImpl> As<TService>() where TService : class
        {
            _binding.Service = typeof(TService);
            return this;
        }

        public ISingleBindingSyntax<TImpl> As(Type service)
        {
            _binding.Service = service;
            return this;
        }

        public ISingleBindingSyntax<TImpl> With(Lifestyle lifestyle)
        {
            _binding.Lifestyle = lifestyle;
            return this;
        }

        public ISingleBindingSyntax<TImpl> Named(string name)
        {
            _binding.Name = name;
            return this;
        }
    }

    internal class SingleBindingSyntax : ISingleBindingSyntax
    {
        private readonly SingleBinding _binding;
        IBinding IBindingSyntax.Binding { get { return _binding; } }

        public SingleBindingSyntax(Type implementation, IImplementationResolver resolver)
        {
            _binding = new SingleBinding
            {
                Service = implementation,
                Resolver = resolver
            };
        }

        public ISingleBindingSyntax As<TService>() where TService : class
        {
            _binding.Service = typeof(TService);
            return this;
        }

        public ISingleBindingSyntax As(Type service)
        {
            _binding.Service = service;
            return this;
        }

        public ISingleBindingSyntax With(Lifestyle lifestyle)
        {
            _binding.Lifestyle = lifestyle;
            return this;
        }

        public ISingleBindingSyntax Named(string name)
        {
            _binding.Name = name;
            return this;
        }
    }
}
