using System;

namespace Chizl
{
    [AttributeUsage(AttributeTargets.Field)]
    public class RegexDefinitionAttribute : Attribute
    {
        public string ValidationPattern { get; }
        public string SanitizationPattern { get; }

        public RegexDefinitionAttribute(string validationPattern, string sanitizationPattern)
        {
            ValidationPattern = validationPattern;
            SanitizationPattern = sanitizationPattern;
        }
    }
}
