using System;

namespace SmartElk.Antler.Core.Abstractions.Registration
{
    public interface ISingleBindingSyntax : IBindingSyntax
    {
        ISingleBindingSyntax As<TService>() where TService : class;
        ISingleBindingSyntax As(Type service);
        ISingleBindingSyntax With(Lifestyle lifestyle);
        ISingleBindingSyntax Named(string name);
    }

    public interface ISingleBindingSyntax<in TImpl> : IBindingSyntax where TImpl : class
    {
        ISingleBindingSyntax<TImpl> As<TService>() where TService : class;
        ISingleBindingSyntax<TImpl> As(Type service);
        ISingleBindingSyntax<TImpl> With(Lifestyle lifestyle);
        ISingleBindingSyntax<TImpl> Named(string name);
    }
}
