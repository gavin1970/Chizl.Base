using System.Collections.Generic;
using System.Text.RegularExpressions;

namespace Chizl.Base.Internal
{
    internal static class RegexHelper
    {
        // Caches for performance
        private static readonly Dictionary<RegexPatterns, Regex> _validationCache = 
                new Dictionary<RegexPatterns, Regex>();
        private static readonly Dictionary<RegexPatterns, Regex> _sanitizationCache = 
                new Dictionary<RegexPatterns, Regex>();
        private static readonly Dictionary<RegexPatterns, RegexDefinitionAttribute> _attribCache = 
                new Dictionary<RegexPatterns, RegexDefinitionAttribute>();

        internal static Regex GetValidationRegex(RegexPatterns pattern)
        {
            if (!_validationCache.ContainsKey(pattern))
            {
                var attr = GetAttribute(pattern);
                _validationCache[pattern] = new Regex(attr.ValidationPattern);
            }
            return _validationCache[pattern];
        }
        internal static Regex GetSanitizationRegex(RegexPatterns pattern)
        {
            if (!_sanitizationCache.ContainsKey(pattern))
            {
                var attr = GetAttribute(pattern);
                _sanitizationCache[pattern] = new Regex(attr.SanitizationPattern);
            }
            return _sanitizationCache[pattern];
        }
        internal static RegexDefinitionAttribute GetAttribute(RegexPatterns pattern)
        {
            if (!_attribCache.ContainsKey(pattern))
            {
                var type = typeof(RegexPatterns);
                var retType = typeof(RegexDefinitionAttribute);
                var memInfo = type.GetMember(pattern.Name());
                var attributes = memInfo[0].GetCustomAttributes(retType, false);
                var attr = (attributes.Length > 0) ? (RegexDefinitionAttribute)attributes[0] : null;
                _attribCache[pattern] = attr;
            }
            return _attribCache[pattern];
        }
    }
}