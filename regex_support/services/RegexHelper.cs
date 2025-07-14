using System;
using System.Collections.Concurrent;
using System.Text.RegularExpressions;

namespace Chizl.RegexSupport
{
    internal static class RegexHelper
    {
        // Thread-safe caches
        private static readonly ConcurrentDictionary<RegexPatterns, Regex> _validationCache =
            new ConcurrentDictionary<RegexPatterns, Regex>();

        private static readonly ConcurrentDictionary<RegexPatterns, Regex> _sanitizationCache =
            new ConcurrentDictionary<RegexPatterns, Regex>();

        private static readonly ConcurrentDictionary<RegexPatterns, RegexDefinitionAttribute> _attribCache =
            new ConcurrentDictionary<RegexPatterns, RegexDefinitionAttribute>();

        internal static Regex GetValidationRegex(RegexPatterns pattern)
        {
            return _validationCache.GetOrAdd(pattern, key =>
            {
                var attr = GetAttribute(key);
                return new Regex(attr.ValidationPattern, RegexOptions.Compiled | RegexOptions.CultureInvariant);
            });
        }

        internal static Regex GetSanitizationRegex(RegexPatterns pattern)
        {
            return _sanitizationCache.GetOrAdd(pattern, key =>
            {
                var attr = GetAttribute(key);
                return new Regex(attr.SanitizationPattern, RegexOptions.Compiled | RegexOptions.CultureInvariant);
            });
        }

        internal static RegexDefinitionAttribute GetAttribute(RegexPatterns pattern)
        {
            return _attribCache.GetOrAdd(pattern, key =>
            {
                var type = typeof(RegexPatterns);
                var retType = typeof(RegexDefinitionAttribute);
                var memInfo = type.GetMember(key.Name());
                var attributes = memInfo[0].GetCustomAttributes(retType, false);
                return (attributes.Length > 0) ? (RegexDefinitionAttribute)attributes[0] : null;
            });
        }
    }
}
