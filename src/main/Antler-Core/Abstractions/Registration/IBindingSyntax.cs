using SmartElk.Antler.Core.Common;

namespace SmartElk.Antler.Core.Abstractions.Registration
{
    public interface IBindingSyntax : ISyntax
    {
        IBinding Binding { get; } 
    }
}
