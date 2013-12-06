using System;

namespace SmartElk.Antler.Core.Abstractions.Configuration
{
    public interface IBasicConfiguration: IDisposable
    {
        IContainer Container { get; set; }
    }
}
