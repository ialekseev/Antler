using SmartElk.Antler.Core.Abstractions.Configuration;
using SmartElk.Antler.Core.Common.CodeContracts;

namespace SmartElk.Antler.Core.Domain.Configuration
{
    public class DomainConfigurator: IDomainConfigurator
    {
        public IBasicConfiguration Configuration { get; private set; }
        public string Name { get; private set; }
                        
        public DomainConfigurator(IBasicConfiguration configuration)
        {
            Requires.NotNull(configuration, "configuration");
            Configuration = configuration;            
        }
        
        public IDomainConfigurator Named(string name)
        {
            Requires.NotNullOrEmpty(name, "name");
            Name = name;
            return this;
        }
    }
}
