using System;
using SmartElk.Antler.Core.Common;

namespace SmartElk.Antler.Core.Abstractions.Configuration
{
    public interface IAntlerConfigurator : ISyntax, IDisposable
    {
        IBasicConfiguration Configuration { get; }
    }
}
