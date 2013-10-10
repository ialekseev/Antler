using SmartElk.Antler.Common;

namespace SmartElk.Antler.Abstractions.Registration
{
    public interface IBindingSyntax : ISyntax
    {
        IBinding Binding { get; } 
    }
}
