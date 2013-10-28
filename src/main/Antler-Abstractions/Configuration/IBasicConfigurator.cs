using System;
using SmartElk.Antler.Common;

namespace SmartElk.Antler.Abstractions.Configuration
{
    public interface IBasicConfigurator : ISyntax, IDisposable
    {
        IBasicConfiguration Configuration { get; }
    }
}
