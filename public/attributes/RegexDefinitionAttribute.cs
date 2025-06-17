using System;

namespace Chizl
{
    [AttributeUsage(AttributeTargets.Field)]
    public class RegexDefinitionAttribute : Attribute
    {
        public string ValidationPattern { get; }
        public string SanitizationPattern { get; }
        public string Examples { get; }

        public RegexDefinitionAttribute(string validationPattern, string sanitizationPattern, string examples)
        {
            ValidationPattern = validationPattern;
            SanitizationPattern = sanitizationPattern;
            Examples = examples;
        }
    }
}
