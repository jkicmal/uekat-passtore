using System.Text.RegularExpressions;

namespace Passtore.Utils
{
    public static class ValidationUtils
    {
        public static bool IsEmail(string text)
        {
            var regex = @"^([a-zA-Z0-9_\-\.]+)@([a-zA-Z0-9_\-\.]+)\.([a-zA-Z]{2,5})$";
            var match = Regex.Match(text, regex, RegexOptions.IgnoreCase);
            return match.Success;
        }

        public static bool IsUsername(string text)
        {
            var regex = @"^(?!.*[_\s-]{2,})[a-zA-Z0-9][a-zA-Z0-9_\s\-.]*[a-zA-Z0-9]$";
            var match = Regex.Match(text, regex, RegexOptions.IgnoreCase);
            return match.Success;
        }
    }
}
