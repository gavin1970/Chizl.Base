namespace Chizl
{
    public enum RegexPatterns
    {
        [RegexDefinition(@"^[a-zA-Z]+$", "[^a-zA-Z]", "SuperCaliFragiListicExpiAliDocious")]
        Alpha,
        [RegexDefinition(@"^[a-zA-Z\s]+$", "[^a-zA-Z ]", "20TwentyFive")]
        AlphaWithSpaces,
        [RegexDefinition(@"^[0-9]+$", "[^0-9]", "65535")]
        NumericUnsigned,
        [RegexDefinition(@"^-?(?!-)[0-9]+$", "[^-0-9]", "-32768, 32767")]
        NumericSigned,
        [RegexDefinition(@"^\d{3}-\d{4}$", "[^0-9-]", "555-1212")]
        PhoneUS07,
        [RegexDefinition(@"^(\(\d{3}\)[\s]?|\d{3}-)(\d{3}-\d{4})$", @"[^\(\)0-9-]", "(800) 555-1212, (800)555-1212, 800-555-1212")]
        PhoneUS10,
        [RegexDefinition(@"^(\+1[\s]?\(\d{3}\)[\s]?|\+1-\d{3}-|1-\d{3}-)(\d{3}-\d{4})$", @"[^\+\(\)0-9-]", "1-800-555-1212, +1 (800) 555-1212, +1(800)555-1212")]
        PhoneUS11,
        /// <summary>
        /// <code>
        /// Pattern: @"^(?!666|000|9\d{2})\d{3}-(?!00)\d{2}-(?!0000)\d{4}$"
        /// -------
        /// ^: Asserts the start of the string.
        /// (?!666|000|9\d{2}): This is a negative lookahead that checks the first three digits.
        /// 	?!: Specifies that the following patterns must not be present at this position.
        /// 	666: The number 666 has never been assigned as an Area Number.
        /// 	000: An Area Number of 000 is invalid.
        /// 	9\d{2}: Area Numbers in the 900-999 range are not used for SSNs. They are reserved for Taxpayer Identification Numbers (TINs).
        /// \d{3}: Matches the first three digits (the Area Number), provided they do not violate the preceding negative lookahead.
        /// -: Matches the hyphen separator.
        /// (?!00): This negative lookahead ensures the Group Number (the middle two digits) is not 00.
        /// \d{2}: Matches the two digits of the Group Number.
        /// -: Matches the second hyphen separator.
        /// (?!0000): A final negative lookahead to ensure the Serial Number (the last four digits) is not 0000.
        /// \d{4}: Matches the four digits of the Serial Number.
        /// $: Asserts the end of the string.
        /// </code>
        /// </summary>
        [RegexDefinition(@"^(?!666|000|9\d{2})\d{3}-(?!00)\d{2}-(?!0000)\d{4}$", "[^0-9-]", "111-22-3333")]
        Ssn,
        [RegexDefinition(@"^[#]?(([a-fA-F0-9]{8})|([a-fA-F0-9]{6})|([a-fA-F0-9]{3}))$", "[^#a-fA-F0-9$]", "#CCC, CCC, #C0C0C0, C0C0C0, #FFC0C0C0, FFC0C0C0")]
        Hex
    }

    public enum RegexPatternType
    {
        Match,
        Sanitize,
        Examples
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
        Ssn,
        Hex
    }
}
