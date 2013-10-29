using SmartElk.Antler.Abstractions.Configuration;

namespace SmartElk.Antler.Domain.Configuration
{
    public class DomainConfigurator: IDomainConfigurator
    {
        public IBasicConfiguration Configuration { get; private set; }
        public string Name { get; private set; }

        private bool HasName
        {
            get { return !string.IsNullOrEmpty(Name); }
        }
                
        public DomainConfigurator(IBasicConfiguration configuration)
        {
            Configuration = configuration;            
        }
        
        public IDomainConfigurator Named(string name)
        {
            Name = name;
            return this;
        }
    }
}
