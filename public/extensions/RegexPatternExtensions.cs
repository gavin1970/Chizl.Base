using System;
using System.Reflection;
using System.Diagnostics;
using Chizl.Base.Internal;
using System.Text.RegularExpressions;

namespace Chizl
{
    public static class RegexPatternExtensions
    {
        #region Public Methods
        /// <summary>
        /// Uses RegexPatterns to do simple Regex patterns, without the need to add Regex to your class to match strings.<br/>
        /// <br/>
        /// <code>
        /// var fileName = "~Alpha_!1999Test";
        /// var match = RegexPatterns.AlphaNumeric.IsMatch(fileName);
        /// Output: false
        /// </code>
        /// </summary>
        /// <param name="input">String to match with</param>
        /// <returns>true if matched, false if not matched.</returns>
        public static bool IsMatch(this RegexPatterns val, string input)
        {
            if (string.IsNullOrEmpty(input)) return false;
            var regex = RegexHelper.GetValidationRegex(val);
            return regex.IsMatch(input);
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
        /// <param name="input">String to removed unwanted characters from</param>
        /// <param name="replaceWith">string to replace anything not wanted.  (Optional) Default to empty string.</param>
        /// <returns>new string with replacement string based on pattern</returns>
        public static string Sanitize(this RegexPatterns val, string input, string replaceWith = "")
        {
            if (string.IsNullOrEmpty(input)) return input;
            var attr = RegexHelper.GetAttribute(val);

            switch (attr.Strategy)
            {
                // Using the standard Regex.Replace only
                case SanitizeStrategy.SimpleReplace:
                    // Get Regex object that might be cached.
                    return RegexHelper.GetSanitizationRegex(val).Replace(input, replaceWith);
                // Pull matches and only keeping the first one.
                case SanitizeStrategy.KeepFirstMatch:
                    // Implement logic to keep only the first match of the SanitizationPattern
                    // This will likely involve a MatchEvaluator
                    return KeepFirstMatchSanitizer(input, attr.SanitizationPattern, replaceWith);
                // Fully customizable method to process sanitation.
                case SanitizeStrategy.CustomMethod:
                    // Use reflection to call the specified custom sanitizer method
                    return InvokeCustomSanitizer(input, attr.CustomSanitizerMethodName, attr.SanitizationPattern, replaceWith);

                default:
                    return input; // Default to returning input.  Should never occur since strategy a required parameter.
            }
        }
        /// <summary>
        /// Gets the Regex pattern based on type requested.
        /// </summary>
        /// <param name="regPatType">Match or Sanitize</param>
        /// <param name="escape">Auto adds escaps used with Regex.  Set to false for display only.</param>
        /// <returns></returns>
        public static string GetInfo(this RegexPatterns @this, RegexPatternType regPatType, bool escape = false)
        {
            var validation = "";

            switch (regPatType)
            {
                case RegexPatternType.Match:
                    validation = RegexHelper.GetAttribute(@this).ValidationPattern;
                    break;
                case RegexPatternType.Sanitize:
                    validation = RegexHelper.GetAttribute(@this).SanitizationPattern;
                    break;
                case RegexPatternType.SanitizeStrategy:
                    validation = RegexHelper.GetAttribute(@this).Strategy.Name();
                    break;
                case RegexPatternType.CustomMethodName:
                    validation = RegexHelper.GetAttribute(@this).CustomSanitizerMethodName;
                    break;
                case RegexPatternType.Examples:
                default:
                    validation = RegexHelper.GetAttribute(@this).Examples;
                    escape = false; //force, we don't want to escape examples.
                    break;
            }

            if (escape)
                validation = Regex.Escape(validation);

            return validation;
        }
        #endregion

        #region Private Method
        /// <summary>
        /// Implement the KeepFirstMatch sanitizer logic
        /// </summary>
        /// <param name="input"></param>
        /// <param name="pattern"></param>
        /// <returns></returns>
        private static string KeepFirstMatchSanitizer(string input, string pattern, string replaceWith)
        {
            int count = 0;
            string result = Regex.Replace(input, pattern, match =>
            {
                count++;
                if (count == 1)
                    return match.Value;
                else
                    return replaceWith;
            });
            return result;
        }
        /// <summary>
        /// Implement the custom method invoker
        /// </summary>
        /// <param name="input">input string to clean</param>
        /// <param name="methodName">method name to execute.</param>
        /// <param name="pattern">in case it too can be used.</param>
        /// <returns>cleaned string</returns>
        private static string InvokeCustomSanitizer(string input, string methodName, string pattern, string replaceWith)
        {
            // You need to define where your custom sanitizer methods are located.
            // For example, in this RegexHelper class or another dedicated class.
            MethodInfo method = typeof(CustomSanitationMethods).GetMethod(methodName, BindingFlags.Static | BindingFlags.Public | BindingFlags.Instance);

            if (method != null && method.ReturnType == typeof(string) 
                && method.GetParameters().Length > 0 
                && method.GetParameters()[0].ParameterType == typeof(string))
            {
                try
                {
                    if (method.GetParameters().Length == 3 && method.GetParameters()[0].ParameterType == typeof(string) && method.GetParameters()[1].ParameterType == typeof(string) && method.GetParameters()[2].ParameterType == typeof(string))
                        // Invoke the custom method
                        return (string)method.Invoke(null, new object[] { input, pattern, replaceWith });
                    if (method.GetParameters().Length == 2 && method.GetParameters()[0].ParameterType == typeof(string) && method.GetParameters()[1].ParameterType == typeof(string))
                        // Invoke the custom method
                        return (string)method.Invoke(null, new object[] { input, pattern });
                    else
                        // Invoke the custom method
                        return (string)method.Invoke(null, new object[] { input });
                }
                catch (Exception ex)
                {
                    // Log or handle the exception
                    Debug.WriteLine($"Error invoking custom sanitizer method '{methodName}': {ex.Message}");
                    return input; // Return original input in case of error
                }
            }
            else
            {
                // Method not found or has wrong signature
                Debug.WriteLine($"Custom sanitizer method '{methodName}' not found or has incorrect signature.");
                return input;
            }
        }
        #endregion
    }
}
