using System;

namespace SmartElk.Antler.Abstractions.Registration
{
    public interface IMultipleBindingSyntax : IBindingSyntax
    {
        IMultipleBindingSyntax As<TService>() where TService : class;
        IMultipleBindingSyntax As(params Type[] services);
        IMultipleBindingSyntax With(Lifestyle lifestyle);
    }
}
