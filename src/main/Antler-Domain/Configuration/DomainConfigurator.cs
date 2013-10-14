using SmartElk.Antler.Abstractions.Configuration;

namespace SmartElk.Antler.Domain.Configuration
{
    public class DomainConfigurator: IDomainConfigurator
    {
        public AntlerConfiguration Configuration { get; private set; }

        public DomainConfigurator(AntlerConfiguration configuration)
        {
            Configuration = configuration;
        }
    }
}
