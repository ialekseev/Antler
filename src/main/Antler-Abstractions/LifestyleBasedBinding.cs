using SmartElk.Antler.Abstractions.Registration;

namespace SmartElk.Antler.Abstractions
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
