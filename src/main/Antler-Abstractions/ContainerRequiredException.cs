using System;

namespace SmartElk.Antler.Abstractions
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
