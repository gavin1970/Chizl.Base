using Chizl.Extensions;
using System;

namespace Chizl.RegexSupport
{
    [AttributeUsage(AttributeTargets.Field)]
    public class RegexDefinitionAttribute : Attribute
    {
        public string ValidationPattern { get; }
        public string SanitizationPattern { get; }
        public string Examples { get; }
        public SanitizeStrategy Strategy { get; }
        public string CustomSanitizerMethodName { get; } // Name of the custom method if Strategy is CustomMethod

        public RegexDefinitionAttribute(string validationPattern, string sanitizationPattern, string examples)
        {
            ValidationPattern = validationPattern;
            SanitizationPattern = sanitizationPattern;
            Examples = examples;
            Strategy = SanitizeStrategy.SimpleReplace;
            CustomSanitizerMethodName = string.Empty;
        }
        public RegexDefinitionAttribute(string validationPattern, string sanitizationPattern, string examples, SanitizeStrategy strategy, string customSanitizerMethodName)
        {
            if (strategy.Equals(SanitizeStrategy.CustomMethod) && string.IsNullOrWhiteSpace(customSanitizerMethodName))
                throw new ArgumentException($"'{nameof(customSanitizerMethodName)}' is required when using '{strategy.Name()}'.");

            ValidationPattern = validationPattern;
            SanitizationPattern = sanitizationPattern;
            Examples = examples;
            Strategy = strategy;
            CustomSanitizerMethodName = customSanitizerMethodName;
        }
    }
}
