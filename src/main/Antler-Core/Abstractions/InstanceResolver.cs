namespace SmartElk.Antler.Core.Abstractions
{
    /// <summary>
    /// Resolver using a pre-built object instance.
    /// </summary>
    public class InstanceResolver : IImplementationResolver
    {
        /// <summary>
        /// Component implementation instance.
        /// </summary>
        public object Instance { get; protected set; }

        internal InstanceResolver(object instance)
        {
            Instance = instance;
        }
    }
}
