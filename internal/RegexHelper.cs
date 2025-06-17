using System;
using System.Collections.Generic;
using System.Linq;
using System.Text.RegularExpressions;

namespace Chizl.Base.@Internal
{
    internal static class RegexHelper
    {
        // Caches for performance
        private static readonly Dictionary<RegexPatterns, Regex> _validationCache = new Dictionary<RegexPatterns, Regex>();
        private static readonly Dictionary<RegexPatterns, Regex> _sanitizationCache = new Dictionary<RegexPatterns, Regex>();

        internal static Regex GetValidationRegex(RegexPatterns pattern)
        {
            if (!_validationCache.ContainsKey(pattern))
            {
                var attr = GetAttribute(pattern);
                _validationCache[pattern] = new Regex(attr.ValidationPattern, RegexOptions.Compiled);
            }
            return _validationCache[pattern];
        }
        internal static Regex GetSanitizationRegex(RegexPatterns pattern)
        {
            if (!_sanitizationCache.ContainsKey(pattern))
            {
                var attr = GetAttribute(pattern);
                _sanitizationCache[pattern] = new Regex(attr.SanitizationPattern, RegexOptions.Compiled);
            }
            return _sanitizationCache[pattern];
        }
        internal static RegexDefinitionAttribute GetAttribute(RegexPatterns pattern)
        {
            var type = pattern.GetType();
            var retType = typeof(RegexDefinitionAttribute);
            var memInfo = type.GetMember(pattern.Name());
            var attributes = memInfo[0].GetCustomAttributes(retType, false);
            return (attributes.Length > 0) ? (RegexDefinitionAttribute)attributes[0] : null;
        }
    }
}


/*
public static class RegexExtensions
{
    // Compile the regex once and reuse it.
    private static readonly Regex _alphaRegex = new Regex("^[a-zA-Z]+$", RegexOptions.Compiled);
    private static readonly Regex _alphaWithSpacesRegex = new Regex(@"^[a-zA-Z\s]+$", RegexOptions.Compiled);

    public static bool IsMatch(this RegexPatterns val, string fullString)
    {
        Regex regexToUse;
        switch (val)
        {
            case RegexPatterns.Alpha:
                regexToUse = _alphaRegex;
                break;
            case RegexPatterns.AlphaWithSpaces:
                regexToUse = _alphaWithSpacesRegex;
                break;
            // ... other cases
            default:
                return false; // Or throw exception
        }
        return regexToUse.IsMatch(fullString);
    }
} 
*/