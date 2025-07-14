using Xunit;
using Chizl.RegexSupport;
using System.Collections.Generic;

namespace Chizl.Base.Tests
{
    public class RegexPatternsTests
    {
        public static IEnumerable<object[]> MatchData =>
            new List<object[]>
            {
                new object[] { RegexPatterns.Hex, "#FF00FF", true },
                new object[] { RegexPatterns.Hex, "#ZFF00FF", false },
                new object[] { RegexPatterns.Hex, "#ZFF#00FF", false }
            };

        public static IEnumerable<object[]> SanitizeData =>
            new List<object[]>
            {
                new object[] { RegexPatterns.Hex, "#ZFF00FF", "#FF00FF" },
                new object[] { RegexPatterns.Hex, "#ZFF#00FF", "#FF00FF" }
            };

        [Theory]
        [MemberData(nameof(MatchData))]
        public void Test_IsMatch(RegexPatterns pattern, string input, bool expected)
        {
            bool result = pattern.IsMatch(input);
            Assert.Equal(expected, result);
        }

        [Theory]
        [MemberData(nameof(SanitizeData))]
        public void Test_Sanitize(RegexPatterns pattern, string input, string expected)
        {
            string result = pattern.Sanitize(input);
            Assert.Equal(expected, result);
        }
    }
}
