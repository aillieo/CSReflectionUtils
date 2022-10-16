using System;

namespace AillieoUtils.CSReflectionUtils.Editor
{
    public static class StringExtensions
    {
        public static bool ContainsIgnoreCase(this string source, string value)
        {
            if (string.IsNullOrEmpty(source))
            {
                return false;
            }

            return source.IndexOf(value, StringComparison.InvariantCultureIgnoreCase) >= 0;
        }
    }
}
