using System;

namespace SmartElk.Antler.Abstractions.Configuration
{
    public interface IBasicConfiguration: IDisposable
    {
        IContainer Container { get; set; }
    }
}
