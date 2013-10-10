using Castle.Core;
using Castle.MicroKernel;
using Castle.MicroKernel.Releasers;
using Castle.Windsor.Diagnostics;

namespace SmartElk.Antler.Windsor
{
    public class UnmanagedReleasePolicy : LifecycledComponentsReleasePolicy
    {
        public UnmanagedReleasePolicy(IKernel kernel)
            : base(kernel)
        {
        }

        public UnmanagedReleasePolicy(ITrackedComponentsDiagnostic trackedComponentsDiagnostic, ITrackedComponentsPerformanceCounter trackedComponentsPerformanceCounter)
            : base(trackedComponentsDiagnostic, trackedComponentsPerformanceCounter)
        {
        }

        public override void Track(object instance, Burden burden)
        {
            if (burden.Model.LifestyleType == LifestyleType.Custom
                && burden.Model.CustomLifestyle == typeof(UnmanagedLifestyleManager))
                return;

            base.Track(instance, burden);
        }
    }
}
