using System.Text.RegularExpressions;

namespace DynamicList
{
    public static class StringExtensions
    {
        public static string SplitCamelCase(this string x) =>
            Regex.Replace(x, "([A-Z])", " $1", RegexOptions.Compiled).Trim();
    }
}