using System.Linq;
using System.Text.RegularExpressions;

namespace Chizl.RegexSupport
{
    internal sealed class CustomSanitationMethods
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
            => CommonParts(input, pattern, replaceWith, "-", (new char[] { '.' }));

        /// <summary>
        /// Custom sanitizer method for Hex
        /// </summary>
        /// <param name="input">input string to clean</param>
        /// <param name="pattern">regex pattern, required for possible future custom methods, that require it.</param>
        /// <param name="replaceWith">regex replacement char.</param>
        /// <returns>cleaned string</returns>
        public static string SanitizeMoneyGeneric(string input, string pattern, string replaceWith)
        {
            var sanitized = CommonParts(input, pattern, replaceWith, "$", (new char[] { '.', ' ', '-' }));
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
            var needsRemoval = sanitized.IndexOf(" ") > -1 && sanitized.IndexOf(" ") != 1;
            //if it's the 2 char, but the first char is not the $, then we still need it cleared.
            if (needsRemoval || !needsRemoval && sanitized.IndexOf("$") != 0)
                sanitized = sanitized.Replace(" ", replaceWith);

            return sanitized;
        }

        /// <summary>
        /// Will run Regex.Replace, but then validate only 3 '.' exists.<br/>
        /// If less than 3, returns string after cleaning from Regex.Replace.<br/>
        /// If more than 3, will break it down to the first 4 nodes.<br/>
        /// Will trim any node that are longer than 3 bytes digits.
        /// </summary>
        public static string SanitizeIPv4Generic(string input, string pattern, string replaceWith)
        {
            if (replaceWith == null)
                replaceWith = "";

            // First, remove invalid characters based on pattern. 
            var sanitized = Regex.Replace(input, pattern, replaceWith);
            var splIP = sanitized.Split(new char[] { '.' }, System.StringSplitOptions.RemoveEmptyEntries);

            if (splIP.Length < 4)
                return sanitized;   //invalid ipv4

            if (splIP.Length > 4)
            {
                splIP = splIP.Take(4).ToArray();
                sanitized = string.Join(".", splIP);
            }

            var hasUpdate = false;
            for (var i = 0; i < splIP.Length; i++)
            {
                if (splIP[i].Length > 3)
                {
                    splIP[i] = splIP[i].ToString().Substring(0, 3);
                    hasUpdate = true;
                }
            }

            if (hasUpdate)
                sanitized = string.Join(".", splIP);

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
                    sanitized = "";
                    //if a special char exists, put it back together, keeping the first and removing any duplicates.
                    for (int i=0; i<sans.Length; i++)
                        sanitized += $"{sans[i]}{(i.Equals(0) ? $"{ch}" : "")}";
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
