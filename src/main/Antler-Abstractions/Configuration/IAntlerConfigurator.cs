using SmartElk.Antler.Common;

namespace SmartElk.Antler.Abstractions.Configuration
{
    public interface IAntlerConfigurator : ISyntax
    {
        AntlerConfiguration Configuration { get; }
    }
}
