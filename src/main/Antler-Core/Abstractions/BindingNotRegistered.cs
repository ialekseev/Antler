using System;

namespace SmartElk.Antler.Core.Abstractions
{
    public class BindingNotRegisteredException: Exception
    {
        private const string MessageForUnnamedService = "Binding for service '{0}' couldn't be found in container. Check inner exception for more details.";
        private const string MessageForNamedService = "Binding for service '{0}' with name '{1}' couldn't be found in container. Check inner exception for more details.";

        public Type Service { get; private set; }

        public BindingNotRegisteredException(Type service, Exception providerException)
            : base(string.Format(MessageForUnnamedService, service.FullName), providerException)
        {
            Service = service;
        }

        public BindingNotRegisteredException(Type service, string name, Exception providerException)
            : base(string.Format(MessageForNamedService, service.FullName, name), providerException)
        {
            Service = service;
        }
    }
}
