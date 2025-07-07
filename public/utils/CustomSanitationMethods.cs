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
        /// <param name="replaceWith">regex replacement char.</param>
        /// <returns>cleaned string</returns>
        public static string SanitizeHexCodeGeneric(string input, string pattern, string replaceWith)
            => CommonParts(input, pattern, replaceWith, "#", null);

        /// <summary>
        /// Custom sanitizer method for Hex
        /// </summary>
        /// <param name="input">input string to clean</param>
        /// <param name="pattern">regex pattern, required for possible future custom methods, that require it.</param>
        /// <param name="replaceWith">regex replacement char.</param>
        /// <returns>cleaned string</returns>
        public static string SanitizeDecimalGeneric(string input, string pattern, string replaceWith) 
            => CommonParts(input, pattern, replaceWith, "-", (new char[1] { '.' }));

        /// <summary>
        /// Custom sanitizer method for Hex
        /// </summary>
        /// <param name="input">input string to clean</param>
        /// <param name="pattern">regex pattern, required for possible future custom methods, that require it.</param>
        /// <param name="replaceWith">regex replacement char.</param>
        /// <returns>cleaned string</returns>
        public static string SanitizeMoneyGeneric(string input, string pattern, string replaceWith)
        {
            var sanitized = CommonParts(input, pattern, replaceWith, "$", (new char[3] { '.', ' ', '-' }));

            //build array to make sure there isn't more than 1 decimal
            var sans = sanitized.Split(new char[] { '-' }, System.StringSplitOptions.RemoveEmptyEntries);
            //if a special char exists, put it back together, removing any duplicates.
            if (sans.Length > 1)
                sanitized = $"{sans[0]}{'-'}{sans[1]}";

            var negLoc = sanitized.IndexOf("-");
            switch(negLoc)
            {
                case -1:    //doesn't exist
                case 0:     //if it's at the start, this is ok.
                    break;
                case 2:     //only thing allowed in front of the negative is "$ ".  If something else, then remove the negative, it's invalid.
                    if (!sanitized.Substring(0, 2).Equals("$ "))
                        sanitized = sanitized.Replace("-", replaceWith);
                    break;
                default:
                    sanitized = sanitized.Replace("-", replaceWith);
                    break;
            }

            //More customization. Spaces are allowed, but only 1 and after the $.
            //duplicates have already been removed, but if there is a space and it's not after the $, remove it.
            if (sanitized.IndexOf(" ") > -1 && sanitized.IndexOf(" ") != 1)
                sanitized = sanitized.Replace(" ", replaceWith);

            return sanitized;
        }

        /// <summary>
        /// Duplication across all Sanitizers, this merges them together is the most common parts of them all.
        /// </summary>
        /// <param name="input"></param>
        /// <param name="pattern"></param>
        /// <param name="replaceWith"></param>
        /// <param name="allowedStarter"></param>
        /// <param name="removeDups"></param>
        /// <returns></returns>
        private static string CommonParts(string input, string pattern, string replaceWith, string allowedStarter, char[] removeDups)
        {
            if (replaceWith == null)
                replaceWith = "";

            // First, remove invalid characters based on pattern. 
            string sanitized = Regex.Replace(input, pattern, replaceWith);

            if (removeDups != null && removeDups.Length > 0)
            {
                foreach (char ch in removeDups)
                {
                    //build array to make sure there isn't more than 1 decimal
                    var sans = sanitized.Split(new char[] { ch }, System.StringSplitOptions.RemoveEmptyEntries);
                    //if a special char exists, put it back together, removing any duplicates.
                    if (sans.Length > 1)
                        sanitized = $"{sans[0]}{ch}{sans[1]}";
                }
            }

            // Then, remove special char if it's not the first character
            if (sanitized.StartsWith(allowedStarter))
                return allowedStarter + sanitized.Substring(1).Replace(allowedStarter, replaceWith);
            else
                return sanitized.Replace(allowedStarter, replaceWith);
        }
    }
}
