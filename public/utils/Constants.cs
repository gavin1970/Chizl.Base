namespace Chizl
{
    public enum RegexPatterns
    {
        [RegexDefinition(@"^[a-zA-Z]+$", "[^a-zA-Z]")]
        Alpha,
        [RegexDefinition(@"^[a-zA-Z\s]+$", "[^a-zA-Z ]")]
        AlphaWithSpaces,
        [RegexDefinition(@"^[0-9]+$", "[^0-9]")]
        NumericUnsigned,
        [RegexDefinition(@"^-?(?!-)[0-9]+$", "[^-0-9]")]
        NumericSigned,
        [RegexDefinition(@"^\d{3}-\d{4}$", "[^0-9-]")]
        PhoneUS07,
        [RegexDefinition(@"^(\(\d{3}\)|\d{3}-)(\d{3}-\d{4})$", @"[^\(\)0-9-]")]
        PhoneUS10,
        [RegexDefinition(@"^(\+1\(\d{3}\)[\s]?|\+1-\d{3}-|1-\d{3}-)(\d{3}-\d{4})$", @"[^\+\(\)0-9-]")]
        PhoneUS11,
        [RegexDefinition(@"^(?!666|000|9\d{2})\d{3}-(?!00)\d{2}-(?!0000)\d{4}$", "[^0-9-]")]
        Ssn
    }

    public enum RegexPatternType
    {
        Match,
        Sanitize
    }

    public enum RegexPatterns2
    {
        Alpha = 0,
        AlphaNumeric,
        Numeric,
        NumericWithDecimal,
        PhoneHyphenUS07,
        PhoneHyphenUS10,
        PhoneWithCountryCodeUS11,
        /// <summary>
        /// ^: Asserts the start of the string.<br/>
        /// (?!666|000|9\d{2}): This is a negative lookahead that checks the first three digits.<br/>
        /// 	?!: Specifies that the following patterns must not be present at this position.<br/>
        /// 	666: The number 666 has never been assigned as an Area Number.<br/>
        /// 	000: An Area Number of 000 is invalid.<br/>
        /// 	9\d{2}: Area Numbers in the 900-999 range are not used for SSNs. They are reserved for Taxpayer Identification Numbers (TINs).<br/>
        /// \d{3}: Matches the first three digits (the Area Number), provided they do not violate the preceding negative lookahead.<br/>
        /// -: Matches the hyphen separator.<br/>
        /// (?!00): This negative lookahead ensures the Group Number (the middle two digits) is not 00.<br/>
        /// \d{2}: Matches the two digits of the Group Number.<br/>
        /// -: Matches the second hyphen separator.<br/>
        /// (?!0000): A final negative lookahead to ensure the Serial Number (the last four digits) is not 0000.<br/>
        /// \d{4}: Matches the four digits of the Serial Number.<br/>
        /// $: Asserts the end of the string.<br/>
        /// </summary>
        Ssn,
    }
}
