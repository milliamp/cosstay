using System;
using System.Collections.Generic;
using System.Linq;
using System.Web;

namespace CosStay.Site
{
    public static class UtilityMethods
    {
        public static string SafeUri(string toSanitise)
        {
            var safeString = "-_()!";
            return new string(toSanitise
                .Replace(" - ", "-")
                .Replace(" ", "-")
                .Replace(": ", "-")
                .Replace(":", "-")
                .Replace(", ", " ")
                .Replace("--", "-")
                .Where(c => Char.IsLetterOrDigit(c) || safeString.Contains(c))
                .ToArray())
                .ToLowerInvariant();
        }

        public static string Pluralize(int count, string noun, string pluralVersion = null)
        {
            return @count + " " + (count != 1 ? (pluralVersion != null ? pluralVersion : noun + "s") : noun);
        }
    }
}