using System;
using Chizl.Base.Internal.utils;

namespace Chizl
{
    public static class ChizlExtensions
    {
        /// <summary>
        /// Gets a substring from this instance. The substring starts at a specified character position and has a specified length.<br/>
        /// This method uses modern C# range operators (`..`) on supported frameworks (.NET Standard 2.1+, .NET 6+) 
        /// and falls back to the classic Substring method on older frameworks.<br/>
        /// If the value is null, null will be returned instead of an exception.<br/>
        /// If startIndex or length is less than 0, null will be returned.<br/>
        /// If startIndex + length > str.length length will be auto adjusted and return from startIndex to end of string.<br/>
        /// </summary>
        /// <param name="str">The source string.</param>
        /// <param name="startIndex">The zero-based starting character position of a substring in this instance.</param>
        /// <param name="length">The number of characters in the substring.</param>
        /// <returns>A string that is equivalent to the substring of the specified length that begins at startIndex in this instance.</returns>
        public static string SubstringEx(this string str, int startIndex, int length)
        {
            // If the string is null, index is passed the length of string, index or
            // length is less than 0, then nothing can be done with this string.
            if (str == null || startIndex > str.Length || startIndex < 0 || length < 0)
                return null;

            if (str.Length < startIndex + length)
                length = str.Length - startIndex;

            // NETSTANDARD2_1_OR_GREATER: Is defined for .NET Standard 2.1 and any newer .NET Standard version.
            // NET6_0_OR_GREATER: Is defined for .NET 6, .NET 7, .NET 8, .NET 9 and any future versions.
            // By combining these, we target all frameworks that support C# 8.0 features like ranges.
#if NETSTANDARD2_1_OR_GREATER || NET6_0_OR_GREATER
            // For modern frameworks, use the more concise and readable range operator.
            // The end of the range is exclusive, so startIndex + length works correctly.
            return str[startIndex..(startIndex + length)];
#else
            // For older frameworks (net47, net48, netstandard2.0), fall back
            // to the traditional Substring method.
            return str.Substring(startIndex, length);
#endif
        }
        /// <summary>
        /// Finds and generates the default value based on Type
        /// </summary>
        /// <param name="t"></param>
        /// <returns>Default value based on Type</returns>
        public static object GetDefaultValue(this Type t) => Common.GetDefault(t);
    }
}
