using SmartElk.Antler.Core.Abstractions.Registration;

namespace SmartElk.Antler.Core.Abstractions
{    
    public abstract class LifestyleBasedBinding : IBinding
    {        
        public Lifestyle Lifestyle { get; internal set; }
        
        protected LifestyleBasedBinding()
        {
            Lifestyle = Lifestyle.Default;
        }
    }
}
