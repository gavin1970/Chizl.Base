# Chizl.Base
netstandard2.0/netstandard2.1 library.  Added some extensions and simple Regex parterns that can be validated on the fly, with no need to add regex to your own class.

## Solution Comes With:

* NET8 Demo project 
	- Goes through using each class and method and comments around each line to help understand it's reasoning.
* NET8 xUnit project with `coverlet.collector ` for code coverage.
	- Currenly only runs Regex Pattern test. (dotnet test / dotnet test --collect:"XPlat Code Coverage")

## Supporting
`Console Support` - Color extensions, to allow 24bit Color in your console.
- Color.<any>.BGAscii() - returns the Ascii Escape characters for any of the 24bit color palette for the background color, behind the text, in your console.

- Color.<any>.FGAscii() - returns the Ascii Escape characters for any of the 24bit color palette for the test color in your console.

- Color.<any>.ResetAscii() - returns the Ascii Escape characters to set FG and BG colors to console default.

- ConsoleHelper.GetColorReset - Identical to above, just a different way to get there.

`RegEx Pattern Support` - Prebuilt regex patterns for quick Match and or Sanitize. Some of the more complex patterns have a custom sanitation method to better strip down a string to the correct pattern.<br/>

**Supports:** Alpha, AlphaWithSpaces, Decimal8, Email, Hex, IPv4, Money, MoneyWithComma, NumericSigned, NumericUnsigned, Password8v16, PhoneUS07, PhoneUS10, PhoneUS11, and SSN

**Examples:**
```csharp
<bool> = RegexPatterns.Alpha.IsMatch("abcdef");				//true
<sting> = RegexPatterns.Alpha.Sanitize("ab2 999 cd2ef");	//Response: abcdef
```

`Enum Extensions:` 
```csharp
//work with any enum, but for this example, we'll use "RegexPatterns".
RegexPatterns.Alpha.Value()		//returns int value of Alpha (0).
RegexPatterns.Alpha.Name()		//returns string name "Alpha".
```

`Generic Extensions:`
- {NUM}.Clamp(min, max, dec) - a replacement for Math.Clamp().  This was created for netstandard2.0 and greater.  It works just like Math.Clamp(), but has a 3 parameter for rouding to specific decimal place. <br/>
Supports:
	- byte, short, int, long, float, double, decimal, and TimeSpan

## Preset Base Regex Patterns
Currently there are 12 preset extensive regex patterns and some with custom sanitation, "Replace", methods for clean up.
```csharp
using Chizl.RegexSupport;
```

### Sanitation Patterns:
Though the pattern is ran, some of the Regex patterns could be running on a different strategy afterwards.  Check the SanitizeStrategy property for each to know more.  In the following example Hex pattern uses for it's SanitizeStrategy a RegexPatternType.CustomMethod.  That method behind the scene is called SanitizeHexCodeGeneric().  It will do extra clean up, like double # and make sure the # is only the first character.  

This is the only resolve for Regex.Remove as that library doesn't support complex Replace patterns and if attempted, they will be ignored.
```csharp
using Chizl.RegexSupport;
var hexVal = RegexPatterns.Hex;

// Bad Hex value as it has (2) # and a 'Z' in it.
var hex = "#ZFF#00FF";
// Validate it
var valid = hexVal.IsMatch(hex);	// Response: false
// Lets clean up
hex = hexVal.Sanitize(hex);			// Response: #FF00FF
// Re-validate it
valid = hexVal.IsMatch(hex);        // Response: true
```

### Get Info on each RegexPatternType for each preset pattern.
```csharp
var hexVal = RegexPatterns.Hex;
Console.WriteLine(hexVal.GetInfo(RegexPatternType.Match));              //^#?([a-fA-F0-9]{8}|[a-fA-F0-9]{6}|[a-fA-F0-9]{3})$
Console.WriteLine(hexVal.GetInfo(RegexPatternType.Sanitize));           //[^#a-fA-F0-9$]
Console.WriteLine(hexVal.GetInfo(RegexPatternType.Examples));           //#CCC, CCC, #C0C0C0, C0C0C0, #FFC0C0C0, FFC0C0C0
Console.WriteLine(hexVal.GetInfo(RegexPatternType.SanitizeStrategy));   //CustomMethod
Console.WriteLine(hexVal.GetInfo(RegexPatternType.CustomMethodName));   //SanitizeHexCodeGeneric
```
