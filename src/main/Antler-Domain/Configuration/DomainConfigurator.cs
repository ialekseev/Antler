using SmartElk.Antler.Abstractions.Configuration;

namespace SmartElk.Antler.Domain.Configuration
{
    public class DomainConfigurator: IDomainConfigurator
    {
        public IBasicConfiguration Configuration { get; private set; }

        public DomainConfigurator(IBasicConfiguration configuration)
        {
            Configuration = configuration;
        }
    }
}
