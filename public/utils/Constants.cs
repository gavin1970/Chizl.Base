namespace Chizl
{
    public enum RegexPatterns
    {
        /// <summary>
        /// <code>
        /// Verifies if a string is is only Alpha A through Z, not case sensitive, no size limit.
        /// Pattern: @"^[a-zA-Z]+$"
        /// Allows: SuperCaliFragiListicExpiAliDocious
        /// --------------------------------------------------
        /// ^                : Asserts the start of the string.
        /// [                : Starting allow chars
        /// a-zA-Z           : Alpha Only A through Z and a through z.
        /// ]                : Ending allow chars
        /// +                : No size limit
        /// $                : Asserts the end of the string.
        /// </code>
        /// </summary>
        [RegexDefinition(@"^[a-zA-Z]+$", "[^a-zA-Z]", "SuperCaliFragiListicExpiAliDocious")]
        Alpha,
        /// <summary>
        /// <code>
        /// Verifies if a string is is only Alpha A through Z, not case sensitive, no size limit and spaces.
        /// Pattern: @"^[a-zA-Z\s]+$"
        /// Allows: SuperCaliFragiListicExpiAliDocious
        /// --------------------------------------------------
        /// ^                : Asserts the start of the string.
        /// [                : Starting allow chars
        /// a-zA-Z           : Alpha Only A through Z and a through z.
        /// \s               : Allow Spaces
        /// ]                : Ending allow chars
        /// +                : No size limit
        /// $                : Asserts the end of the string.
        /// </code>
        /// </summary>
        [RegexDefinition(@"^[a-zA-Z\s]+$", @"[^a-zA-Z\s]", "20 Twenty Five")]
        AlphaWithSpaces,
        /// <summary>
        /// <code>
        /// Verifies if a string is numeric, which includes negatives and deciamals.
        /// Pattern: @"^(\$|\$\s-?|-)?\d{1,9}\.\d{2,4}$"
        /// Allows: 1.0, 1234567890, -1234567890, 1234567890.12345678, -1234567890.12345678, -0.12345678
        /// --------------------------------------------------
        /// ^                : Asserts the start of the string.
        /// (                : Starting optional group 1
        ///     \$           : Required $ Sign
        /// |                : OR
        ///     \$           : Required $ Sign
        ///     \s           : Required space
        /// )?               : End optional group 1
        /// \-?              : Optional negative
        /// \d{1,9}          : Required Digit only. Legnth: Min 1, Max 9
        /// \.               : Required decimal
        /// \d{2,4}          : Required Digit only. Legnth: Min 2, Max 4
        /// $                : Asserts the end of the string.
        /// </code>
        /// </summary>
        [RegexDefinition(@"^(\$|\$\s-?|-)?\d{1,9}\.\d{2,4}$", @"[^$\-\s\d\.]", "$ 1.00, $123456789.1234", SanitizeStrategy.CustomMethod, "SanitizeMoneyGeneric")]
        Money,
        /// <summary>
        /// <code>
        /// Verifies if a string is numeric, which includes optiona dollar sign, optional negative, required commas, and required deciamals.
        /// Requires commas only if more than $999 and requires length 2 to 4 decimal cents.
        /// Pattern: @"^(\$|\$\s-?|-)?((\d{1,3},){1,2}\d{3}|\d{1,3})\.\d{2,4}$"
        /// Allows: $ 1.56, $ -456,789,012.1234
        /// --------------------------------------------------
        /// ^                : Asserts the start of the string.
        /// (                : Starting optional group 1 option 1 of 3
        ///     \$           : Required $ Sign
        /// |                : OR option 2
        ///     \$           : Required $ Sign
        ///     \s           : Required space
        ///     -?           : Optional Negative
        /// |                : OR option 3
        ///     -            : Required Negative
        /// )?               : End optional group 1
        /// (                : Starting required group 2 option 1 of 2
        ///     (            : Starting Loop Validation
        ///         \d{1,3}  : Required Digit.   Length: Min 1, Max 3
        ///         ,        : Required Comma
        ///     ){1,2}       : Ending Loop Validation.  Required 1 Iteration, Maxed of 2.
        ///     \d{3}        : Required Digit, Length: Min 3, Max 3
        /// |                : OR option 2
        ///     \d{1,3}      : Required Digit, Length: Min 1, Max 3
        /// )                : Ending required group 2 
        /// \.               : Required decimal
        /// \d{2,4}          : Required Digit only. Legnth: Min 2, Max 4
        /// $                : Asserts the end of the string.
        /// </code>
        /// </summary>
        [RegexDefinition(@"^(\$|\$\s-?|-)?((\d{1,3},){1,2}\d{3}|\d{1,3})\.\d{2,4}$", @"[^\$,\-\s\d\.]", "$ 1.00, $123,456,789.1234, $ -123,456,789.1234", SanitizeStrategy.CustomMethod, "SanitizeMoneyGeneric")]
        MoneyWithComma,
        /// <summary>
        /// <code>
        /// Verifies if a string is numeric, which includes optional negatives and required deciamals.
        /// Pattern: @"^\-?\d{1,10}\.\d{1,8}$"
        /// Allows: 1.0, 1234567890.12345678, -1234567890.12345678, -0.12345678
        /// --------------------------------------------------
        /// ^                : Asserts the start of the string.
        /// \-?              : Optional negative
        /// \d{1,10}         : Required Digit only. Legnth: Min 1, Max 10
        /// \.               : Required decimal
        /// \d{1,8}          : Required Digit only. Legnth: Min 1, Max 8
        /// $                : Asserts the end of the string.
        /// </code>
        /// </summary>
        [RegexDefinition(@"^\-?\d{1,10}\.\d{1,8}$", @"[^\-\d\.]", "1.0, 1234567890.12345678, -1234567890.12345678, -0.12345678", SanitizeStrategy.CustomMethod, "SanitizeDecimalGeneric")]
        Decimal8,
        /// <summary>
        /// <code>
        /// Verifies if a string is numeric with no size limit.
        /// Pattern: "^[0-9]+$"
        /// Allows: 10, 1789012345678, 123456789012345678, 12345678
        /// --------------------------------------------------
        /// ^                : Asserts the start of the string.
        /// [0-9]            : Only digits 0 through 9 are allowed.
        /// $                : Asserts the end of the string.
        /// </code>
        /// </summary>
        [RegexDefinition("^[0-9]+$", "[^0-9]", "65535")]
        NumericUnsigned,
        /// <summary>
        /// <code>
        /// Verifies if a string is positive or negative numeric value with no size limit.
        /// Pattern: @"^\-?[0-9]+$"
        /// Allows: 10, 1789012345678, 123456789012345678, 12345678
        /// --------------------------------------------------
        /// ^                : Asserts the start of the string.
        /// [0-9]            : Only digits 0 through 9 are allowed.
        /// $                : Asserts the end of the string.
        /// </code>
        /// </summary>
        [RegexDefinition(@"^\-?[0-9]+$", @"[^\-0-9]", "-32768, 32767")]
        NumericSigned,
        /// <summary>
        /// <code>
        /// Pattern: @"^\d{3}\-\d{4}$"
        /// -------
        /// ^                : Asserts the start of the string.
        /// \d{3}            : Required 3 digit for prefix
        /// \-               : Required dash behind prefix
        /// \d{4}            : Required last 4 of phone number
        /// $                : Asserts the end of the string.
        /// </code>
        /// </summary>
        [RegexDefinition(@"^\d{3}\-\d{4}$", "[^0-9-]", "555-1212")]
        PhoneUS07,
        /// <summary>
        /// <code>
        /// Pattern: @"^(\(\d{3}\)[\s]?|\d{3}-)(\d{3}-\d{4})$"
        /// -------
        /// ^: Asserts the start of the string.
        /// (: Starting group 1 of 2. One of the two groups is required.
        /// 	\(: Required open bracket for area code
        /// 	\d{3}: Required 3 digit for area code
        /// 	\): Required close bracket for area code
        /// 	[\s]?: Optional space
        /// |: or group 2
        /// 	\d{3}: Required 3 digit for area code
        /// 	-: Required dash behind area code
        /// ): Ending group 1 of 2
        /// (: Starting required group 3.
        /// 	\d{3}: Required 3 digit for prefix
        /// 	-: Required dash behind prefix
        /// 	\d{4}: Required last 4 of phone number
        /// ): Ending group 3
        /// $: Asserts the end of the string.
        /// </code>
        /// </summary>
        [RegexDefinition(@"^(\(\d{3}\)[\s]?|\d{3}\-)(\d{3}\-\d{4})$", @"[^\(\)\d\-]", "(800) 555-1212, (800)555-1212, 800-555-1212")]
        PhoneUS10,
        /// <summary>
        /// <code>
        /// Pattern: @"^(\+1[\s]?\(\d{3}\)[\s]?|\+1-\d{3}-|1-\d{3}-)(\d{3}-\d{4})$"
        /// -------
        /// ^                : Asserts the start of the string.
        /// (                : Starting group 1 of 3. One of the three groups is required.
        /// 	\+1          : Required +1 at the start for US country code.
        /// 	[\s]?        : Optional space
        /// 	\(           : Required open bracket for area code
        /// 	\d{3}        : Required 3 digit for area code
        /// 	\)           : Required close bracket for area code
        /// 	[\s]?        : Optional space
        /// |                : or group 2
        /// 	\+1          : Required +1 at the start for US country code.
        /// 	-            : Required dash behind country code
        /// 	\d{3}        : Required 3 digit for area code
        /// 	-            : Required dash behind area code
        /// |                : or group 3
        /// 	1            : Required 1 at the start for US country code.
        /// 	-            : Required dash behind country code
        /// 	\d{3}        : Required 3 digit for area code
        /// 	-            : Required dash behind area code
        /// )                : Ending group 1 of 3
        /// \d{3}            : Required 3 digit for prefix
        /// -                : Required dash behind prefix
        /// \d{4}            : Required last 4 of phone number
        /// $                : Asserts the end of the string.
        /// </code>
        /// </summary>
        [RegexDefinition(@"^(\+1[\s]?\(\d{3}\)[\s]?|\+1\-\d{3}\-|1\-\d{3}\-)\d{3}\-\d{4}$", @"[^\+\(\)\d\-]", "1-800-555-1212, +1 (800) 555-1212, +1(800)555-1212")]
        PhoneUS11,
        /// <summary>
        /// <code>
        /// Pattern: @"^(?!666|000|9\d{2})\d{3}\-(?!00)\d{2}\-(?!0000)\d{4}$"
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
        [RegexDefinition(@"^(?!666|000|9\d{2})\d{3}\-(?!00)\d{2}\-(?!0000)\d{4}$", @"[^0-9\-]", "111-22-3333")]
        Ssn,
        /// <summary>
        /// <code>
        /// Pattern: "^#?([a-fA-F0-9]{8}|[a-fA-F0-9]{6}|[a-fA-F0-9]{3})$"
        /// Custom sanitation method added.  "#CF#CCFF" will return "#CFCCFF"
        /// -------
        /// ^                : Asserts the start of the string.
        /// #?               : Optional # at the start.
        /// (                : Starting group 1 of 3. One of the three groups is required.
        ///    [a-fA-F0-9]{8}: Required Alpha (A - F), upper or lower case, and/or Numeric with required length of 8 bytes.
        /// |                : OR group 2
        ///    a-fA-F0-9]{6} : Required Alpha (A - F), upper or lower case, and/or Numeric with required length of 6 bytes.
        /// |                : or group 3
        ///    [a-fA-F0-9]{3}:	Required Alpha (A - F), upper or lower case, and/or Numeric with required length of 3 bytes.
        /// )                : Ending group 1 of 3
        /// $: Asserts the end of the string.
        /// </code>
        /// </summary>
        [RegexDefinition(@"^#?([a-fA-F0-9]{8}|[a-fA-F0-9]{6}|[a-fA-F0-9]{3})$", "[^#a-fA-F0-9$]", "#CCC, CCC, #C0C0C0, C0C0C0, #FFC0C0C0, FFC0C0C0", SanitizeStrategy.CustomMethod, "SanitizeHexCodeGeneric")]
        Hex
    }

    public enum RegexPatternType
    {
        Match,
        Sanitize,
        SanitizeStrategy,
        CustomMethodName,
        Examples
    }

    public enum SanitizeStrategy
    {
        /// <summary>
        /// Use the SanitizationPattern for Regex.Replace
        /// </summary>
        SimpleReplace,
        /// <summary>
        /// Logic for keeping only the first match of the pattern (applied to SanitizationPattern)
        /// </summary>
        KeepFirstMatch,
        /// <summary>
        /// Use a specific custom method for sanitization
        /// </summary>
        CustomMethod
    }
}
