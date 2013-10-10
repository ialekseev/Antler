namespace SmartElk.Antler.Common
{
    public static class StringEx
    {
        public static string FormatWith(this string str, params object[] parts)
        {
            return string.Format(str, parts);
        }
    }
}
