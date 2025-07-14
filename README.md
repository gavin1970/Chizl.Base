# Chizl.Base
netstandard2.0/netstandard2.1 library.  Added some extensions and simple Regex parterns that can be validated on the fly, with no need to add regex to your own class.

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
