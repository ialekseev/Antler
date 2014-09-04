using System.Globalization;
using System.Linq;

namespace SmartElk.Antler.Core.Common
{
    public static class HashCodeGenerator
    {
        public static int ComposeHashCode(params object[] args)
        {
            unchecked // Overflow is fine, just wrap
            {
                return args.Aggregate(17, (current, arg) => current * 23 + arg.GetHashCode());
            }
        }

        public static string ComposeHashCodeAsString(params object[] args)
        {
            return ComposeHashCode(args).ToString(CultureInfo.InvariantCulture);
        }
    }
}
