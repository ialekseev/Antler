namespace SmartElk.Antler.Core.Abstractions
{
    public class Lifestyle
    {
        /// <summary>
        /// Default lifestyle (singleton).
        /// </summary>
        public static Lifestyle Default = new Lifestyle("Default");
        /// <summary>
        /// Default provider lifestyle.
        /// </summary>
        public static Lifestyle ProviderDefault = new Lifestyle("ProviderDefault");
        /// <summary>
        /// Resolved objects aren't managed and get garbage-collected when they're not referenced anymore.
        /// </summary>
        public static Lifestyle Unmanaged = new Lifestyle("Unmanaged");
        /// <summary>
        /// Singleton. Only one instance of component exists.
        /// </summary>
        public static Lifestyle Singleton = new Lifestyle("Singleton");
        /// <summary>
        /// New instance is created upon every resolving but explicit release may be required.
        /// </summary>
        public static Lifestyle Transient = new Lifestyle("Transient");
        /// <summary>
        /// Resolved instance will be disposed upon web request end.
        /// </summary>
        public static Lifestyle PerWebRequest = new Lifestyle("PerWebRequest");

        /// <summary>
        /// Use provider-specific or user-defined lifestyle.
        /// </summary>
        /// <param name="name">Lifestyle name.</param>
        /// <returns>Custom lifestyle descriptor.</returns>
        public static Lifestyle Custom(string name)
        {
            return new Lifestyle(name);
        }

        /// <summary>
        /// Lifestyle name.
        /// </summary>
        public string Name { get; private set; }

        private Lifestyle(string name)
        {
            Name = name;
        }

        protected bool Equals(Lifestyle other)
        {
            return string.Equals(Name, other.Name);
        }

        public override bool Equals(object obj)
        {
            if (ReferenceEquals(null, obj)) return false;
            if (ReferenceEquals(this, obj)) return true;
            if (obj.GetType() != GetType()) return false;
            return Equals((Lifestyle)obj);
        }

        public override int GetHashCode()
        {
            return (Name != null ? Name.GetHashCode() : 0);
        }

        public override string ToString()
        {
            return Name;
        }
    }
}
