using System;

namespace SmartElk.Antler.Abstractions.Configuration
{
    public class AntlerConfiguration: IDisposable
    {
        public IContainer Container { get; internal set; }

        public void Dispose()
        {
            Container.Dispose();
        }
    }
}
