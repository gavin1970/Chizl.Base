using Chizl.Base.Internal;
using System.Text.RegularExpressions;

namespace Chizl
{
    public static class RegexPatternExtensions
    {
        /// <summary>
        /// Uses RegexPatterns to do simple Regex patterns, without the need to add Regex to your class to match strings.<br/>
        /// <br/>
        /// <code>
        /// var fileName = "~Alpha_!1999Test";
        /// var match = RegexPatterns.AlphaNumeric.IsMatch(fileName);
        /// Output: false
        /// </code>
        /// </summary>
        /// <param name="fullString">String to match with</param>
        /// <returns>true if matched, false if not matched.</returns>
        public static bool IsMatch(this RegexPatterns val, string fullString)
        {
            if (string.IsNullOrEmpty(fullString)) return false;
            var regex = RegexHelper.GetValidationRegex(val);
            return regex.IsMatch(fullString);
        }
        /// <summary>
        /// Uses RegexPatterns to do simple Regex patterns, and removes unwanted characters through Regex.Replace(string, replaceWith).<br/>
        /// replaceWith: (optional) Default: "" // Empty String<br/>
        /// <code>
        /// var fileName = "~Alpha_!1999Test";
        /// fileName = RegexPatterns.AlphaNumeric.Sanitize(fileName, "");
        /// Output: "Alpha1999Test"
        /// </code>
        /// </summary>
        /// <param name="fullString">String to removed unwanted characters from</param>
        /// <param name="replaceWith">string to replace anything not wanted.  (Optional) Default to empty string.</param>
        /// <returns>new string with replacement string based on pattern</returns>
        public static string Sanitize(this RegexPatterns val, string fullString, string replaceWith = "")
        {
            if (string.IsNullOrEmpty(fullString)) return fullString;
            var regex = RegexHelper.GetSanitizationRegex(val);
            return regex.Replace(fullString, replaceWith);
        }
        /// <summary>
        /// Gets the Regex pattern based on type requested.
        /// </summary>
        /// <param name="regPatType">Match or Sanitize</param>
        /// <param name="escape">Auto adds escaps used with Regex.  Set to false for display only.</param>
        /// <returns></returns>
        public static string GetPattern(this RegexPatterns @this, RegexPatternType regPatType, bool escape = true)
        {
            var validation = regPatType.Equals(RegexPatternType.Match) ? 
                                            RegexHelper.GetAttribute(@this).ValidationPattern : 
                                            RegexHelper.GetAttribute(@this).SanitizationPattern;

            if (escape)
                validation = Regex.Escape(validation);

            return validation;
        }

    }
}
