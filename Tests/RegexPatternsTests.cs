using Chizl.RegexSupport;
using System.Collections.Generic;
using Xunit;

namespace Chizl.Base.Tests
{
    public class RegexPatternsTests
    {
        public static IEnumerable<object[]> MatchData =>
        [
            new object[] { RegexPatterns.Alpha, "Alpha_!1234 5Test.67", false },
            new object[] { RegexPatterns.Alpha, "AlphaTest", true },
            new object[] { RegexPatterns.Alpha, "-Alpha_!1234 5Test678.90", false },
            new object[] { RegexPatterns.Alpha, "aTesta", true },
            new object[] { RegexPatterns.AlphaWithSpaces, "Alpha_!1234 5Test.67", false },
            new object[] { RegexPatterns.AlphaWithSpaces, "Alpha Test", true },
            new object[] { RegexPatterns.AlphaWithSpaces, "-Alpha_!1234 5Test678.90", false },
            new object[] { RegexPatterns.AlphaWithSpaces, "a Testa", true },
            new object[] { RegexPatterns.Money, "Alpha_!1234 5Test.67", false },
            new object[] { RegexPatterns.Money, "12345.67", true },
            new object[] { RegexPatterns.Money, "-Alpha_!1234 5Test678.90", false },
            new object[] { RegexPatterns.Money, "12345678.90", true },
            new object[] { RegexPatterns.MoneyWithComma, "Alpha_!1234 5Test.67", false },
            new object[] { RegexPatterns.MoneyWithComma, "12345.67", false },
            new object[] { RegexPatterns.MoneyWithComma, "$12,345,678.90", true },
            new object[] { RegexPatterns.Decimal8, "Alpha_!1234 5Test.67", false },
            new object[] { RegexPatterns.Decimal8, "12345.67", true },
            new object[] { RegexPatterns.Decimal8, "-Alpha_!1234 5Test678.90", false },
            new object[] { RegexPatterns.Decimal8, "-12345678.90", true },
            new object[] { RegexPatterns.NumericUnsigned, "Alpha_!1234 5Test.67", false },
            new object[] { RegexPatterns.NumericUnsigned, "1234567", true },
            new object[] { RegexPatterns.NumericUnsigned, "-Alpha_!1234 5Test678.90", false },
            new object[] { RegexPatterns.NumericUnsigned, "1234567890", true },
            new object[] { RegexPatterns.NumericSigned, "Alpha_!1234 5Test.67", false },
            new object[] { RegexPatterns.NumericSigned, "1234567", true },
            new object[] { RegexPatterns.NumericSigned, "-Alpha_!1234 5Test678.90", false },
            new object[] { RegexPatterns.NumericSigned, "-1234567890", true },
            new object[] { RegexPatterns.PhoneUS07, "Alpha_!1234 5Test.67", false },
            new object[] { RegexPatterns.PhoneUS07, "1234567", false },
            new object[] { RegexPatterns.PhoneUS07, "123-4567", true },
            new object[] { RegexPatterns.PhoneUS10, "Alpha_!1234 5Test.67", false },
            new object[] { RegexPatterns.PhoneUS10, "1234567", false },
            new object[] { RegexPatterns.PhoneUS10, "123-456-7890", true },
            new object[] { RegexPatterns.PhoneUS10, "(123)456-7890", true },
            new object[] { RegexPatterns.PhoneUS11, "Alpha_!1234 5Test.67", false },
            new object[] { RegexPatterns.PhoneUS11, "1234567", false },
            new object[] { RegexPatterns.PhoneUS11, "+1(123)456-7890", true },
            new object[] { RegexPatterns.PhoneUS11, "1-123-456-7890", true },
            new object[] { RegexPatterns.Ssn, "Alpha_!1234 5Test.67", false },
            new object[] { RegexPatterns.Ssn, "1234567", false },
            new object[] { RegexPatterns.Ssn, "123-45-6789", true },
            new object[] { RegexPatterns.Hex, "Alpha_!1234 5Test.67", false },
            new object[] { RegexPatterns.Hex, "Aa12345e67", false },
            new object[] { RegexPatterns.Hex, "#12C4D6", true },
            new object[] { RegexPatterns.IPv4, "Alpha_!1234 5Test.67", false },
            new object[] { RegexPatterns.IPv4, "12345.67", false },
            new object[] { RegexPatterns.IPv4, "123.15.67.5", true },
            new object[] { RegexPatterns.IPv4, "123.15.67.112", true },
            new object[] { RegexPatterns.Password8v16, "Alpha_!1234 5Test.67", false },
            new object[] { RegexPatterns.Password8v16, "Alpha!12345Test67", false },
            new object[] { RegexPatterns.Password8v16, "A#h12C4D6", true },
            new object[] { RegexPatterns.Email, "Alpha_!1234 5Test.67", false },
            new object[] { RegexPatterns.Email, "Alpha_!12345Test.67@", false },
            new object[] { RegexPatterns.Email, "Aza!123+bbb@Test.cc.aa", true },
        ];

        public static IEnumerable<object[]> SanitizeData =>
        [
            new object[] { RegexPatterns.Alpha, "Alpha_!1234 5Test.67", "AlphaTest" },
            new object[] { RegexPatterns.Alpha, "-Alpha_!1234 5Test678.90", "AlphaTest" },
            new object[] { RegexPatterns.Alpha, "~Alpha_!123-4 5Test.67", "AlphaTest" },
            new object[] { RegexPatterns.Alpha, "Alpha_!123-4 5Test6-78.90", "AlphaTest" },
            new object[] { RegexPatterns.AlphaWithSpaces, "Alpha_!1234 5Test.67", "Alpha Test" },
            new object[] { RegexPatterns.AlphaWithSpaces, "-Alpha_!1234 5Test678.90", "Alpha Test" },
            new object[] { RegexPatterns.AlphaWithSpaces, "~Alpha_!123-4 5Test.67", "Alpha Test" },
            new object[] { RegexPatterns.AlphaWithSpaces, "Alpha_!123-4 5Test6-78.90", "Alpha Test" },
            new object[] { RegexPatterns.Money, "Alpha_!1234 5Test.67", "12345.67" },
            new object[] { RegexPatterns.Money, "-Alpha_!1234 5Test678.90", "12345678.90" },
            new object[] { RegexPatterns.Money, "~Alpha_!123-4 5Test.67", "12345.67" },
            new object[] { RegexPatterns.Money, "Alpha_!123-4 5Test6-78.90", "12345678.90" },
            new object[] { RegexPatterns.MoneyWithComma, "Alpha_!1234 5Test.67", "12345.67" },
            new object[] { RegexPatterns.MoneyWithComma, "-Alpha_!1234 5Test678.90", "12345678.90" },
            new object[] { RegexPatterns.MoneyWithComma, "~Alpha_!123-4 5Test.67", "12345.67" },
            new object[] { RegexPatterns.MoneyWithComma, "Alpha_!123-4 5Test6-78.90", "12345678.90" },
            new object[] { RegexPatterns.Decimal8, "Alpha_!1234 5Test.67", "12345.67" },
            new object[] { RegexPatterns.Decimal8, "-Alpha_!1234 5Test678.90", "-12345678.90" },
            new object[] { RegexPatterns.Decimal8, "~Alpha_!123-4 5Test.67", "12345.67" },
            new object[] { RegexPatterns.Decimal8, "Alpha_!123-4 5Test6-78.90", "12345678.90" },
            new object[] { RegexPatterns.NumericUnsigned, "Alpha_!1234 5Test.67", "1234567" },
            new object[] { RegexPatterns.NumericUnsigned, "-Alpha_!1234 5Test678.90", "1234567890" },
            new object[] { RegexPatterns.NumericUnsigned, "~Alpha_!123-4 5Test.67", "1234567" },
            new object[] { RegexPatterns.NumericUnsigned, "Alpha_!123-4 5Test6-78.90", "1234567890" },
            new object[] { RegexPatterns.NumericSigned, "Alpha_!1234 5Test.67", "1234567" },
            new object[] { RegexPatterns.NumericSigned, "-Alpha_!1234 5Test678.90", "-1234567890" },
            new object[] { RegexPatterns.NumericSigned, "~Alpha_!123-4 5Test.67", "123-4567" },
            new object[] { RegexPatterns.NumericSigned, "Alpha_!123-4 5Test6-78.90", "123-456-7890" },
            new object[] { RegexPatterns.PhoneUS07, "Alpha_!1234 5Test.67", "1234567" },
            new object[] { RegexPatterns.PhoneUS07, "-Alpha_!1234 5Test678.90", "-1234567890" },
            new object[] { RegexPatterns.PhoneUS07, "~Alpha_!123-4 5Test.67", "123-4567" },
            new object[] { RegexPatterns.PhoneUS07, "Alpha_!123-4 5Test6-78.90", "123-456-7890" },
            new object[] { RegexPatterns.PhoneUS10, "Alpha_!1234 5Test.67", "1234567" },
            new object[] { RegexPatterns.PhoneUS10, "-Alpha_!1234 5Test678.90", "-1234567890" },
            new object[] { RegexPatterns.PhoneUS10, "~Alpha_!123-4 5Test.67", "123-4567" },
            new object[] { RegexPatterns.PhoneUS10, "Alpha_!123-4 5Test6-78.90", "123-456-7890" },
            new object[] { RegexPatterns.PhoneUS11, "Alpha_!1234 5Test.67", "1234567" },
            new object[] { RegexPatterns.PhoneUS11, "-Alpha_!1234 5Test678.90", "-1234567890" },
            new object[] { RegexPatterns.PhoneUS11, "~Alpha_!123-4 5Test.67", "123-4567" },
            new object[] { RegexPatterns.PhoneUS11, "Alpha_!123-4 5Test6-78.90", "123-456-7890" },
            new object[] { RegexPatterns.Ssn, "Alpha_!1234 5Test.67", "1234567" },
            new object[] { RegexPatterns.Ssn, "-Alpha_!1234 5Test678.90", "-1234567890" },
            new object[] { RegexPatterns.Ssn, "~Alpha_!123-4 5Test.67", "123-4567" },
            new object[] { RegexPatterns.Ssn, "Alpha_!123-4 5Test6-78.90", "123-456-7890" },
            new object[] { RegexPatterns.Hex, "Alpha_!1234 5Test.67", "Aa12345e67" },
            new object[] { RegexPatterns.Hex, "-Alpha_!1234 5Test678.90", "Aa12345e67890" },
            new object[] { RegexPatterns.Hex, "~Alpha_!123-4 5Test.67", "Aa12345e67" },
            new object[] { RegexPatterns.Hex, "Alpha_!123-4 5Test6-78.90", "Aa12345e67890" },
            new object[] { RegexPatterns.IPv4, "Alpha_!1234 5Test.67", "12345.67" },
            new object[] { RegexPatterns.IPv4, "-Alpha_!1234 5Test678.90", "12345678.90" },
            new object[] { RegexPatterns.IPv4, "~Alpha_!123-4 5Test.67", "12345.67" },
            new object[] { RegexPatterns.IPv4, "Alpha_!123-4 5Test6-78.90", "12345678.90" },
            new object[] { RegexPatterns.Password8v16, "Alpha_!1234 5Test.67", "Alpha!12345Test67" },
            new object[] { RegexPatterns.Password8v16, "-Alpha_!1234 5Test678.90", "-Alpha!12345Test67890" },
            new object[] { RegexPatterns.Password8v16, "~Alpha_!123-4 5Test.67", "Alpha!123-45Test67" },
            new object[] { RegexPatterns.Password8v16, "Alpha_!123-4 5Test6-78.90", "Alpha!123-45Test6-7890" },
            new object[] { RegexPatterns.Email, "Alpha_!1234 5Test.67", "Alpha_!12345Test.67@" },
            new object[] { RegexPatterns.Email, "-Alpha_!1234 5Test678.90", "-Alpha_!12345Test678.90@" },
            new object[] { RegexPatterns.Email, "~Alpha_!123-4 5Test.67", "Alpha_!123-45Test.67@" },
            new object[] { RegexPatterns.Email, "Alpha_!123-4 5Test6-78.90", "Alpha_!123-45Test6-78.90@" },
        ];

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
