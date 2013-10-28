using SmartElk.Antler.Abstractions.Configuration;
using SmartElk.Antler.Abstractions.Registration;

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

        public void RegisterSessionScopeFactory<T>(T sessionScopeFactory) where T:class
        {
            Configuration.Container.Put(HasName
                                            ? Binding.Use(sessionScopeFactory).As<ISessionScopeFactory>().Named(Name)
                                            : Binding.Use(sessionScopeFactory).As<ISessionScopeFactory>());
        }


        public IDomainConfigurator Named(string name)
        {
            Name = name;
            return this;
        }
    }
}
