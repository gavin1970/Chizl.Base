using System.Diagnostics;
using System.Text.RegularExpressions;

namespace Chizl
{
    internal class CustomSanitationMethods
    {
        /// <summary>
        /// Custom sanitizer method for Hex
        /// </summary>
        /// <param name="input">input string to clean</param>
        /// <param name="pattern">regex pattern, required for possible future custom methods, that require it.</param>
        /// <returns>cleaned string</returns>
        public static string SanitizeHexCodeGeneric(string input, string pattern, string replaceWith)
        {
            if (replaceWith == null)
                replaceWith = "";

            Debug.WriteLine($"INPUT: {input}, PATTERN: {pattern}, REPLACEMENT: {replaceWith}");
            //@"[^#a-fA-F0-9]"
            // First, remove invalid characters based on pattern. (not hex digits or '#')
            string sanitized = Regex.Replace(input, pattern, replaceWith);
            Debug.WriteLine($"sanitized: {sanitized}");

            // Then, remove '#' if it's not the first character
            if (sanitized.StartsWith("#"))
                return "#" + sanitized.Substring(1).Replace("#", "");
            else
                return sanitized.Replace("#", "");
        }
    }
}
