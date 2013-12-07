using System;

namespace SmartElk.Antler.Core.Abstractions
{
    public class ContainerRequiredException: Exception
    {
        public ContainerRequiredException()
        {
        }

        public ContainerRequiredException(string message) : base(message)
        {
        }
    }
}
