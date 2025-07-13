using System;
using System.Collections.Generic;
using System.Linq;
using Chizl.Base.Internal.utils;

namespace Chizl
{
    public static class ChizlExtensions
    {
        #region Private Arrays
        private static readonly HashSet<Type> _validBoundaryTypes = new HashSet<Type>
        {
            typeof(byte),
            typeof(short),     // Int16
            typeof(int),       // Int32
            typeof(long),      // Int64
            typeof(float),     // Single
            typeof(double),    // Double
            typeof(decimal)    // Decimal
        };

        private static readonly HashSet<Type> _decimalTypes = new HashSet<Type>
        {
            typeof(float),     // Single
            typeof(double),    // Double
            typeof(decimal)    // Decimal
        };
        #endregion

        #region Public Methods
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
        #endregion

        /// <summary>
        /// Responds with numeric value passed in after forcing value to be within range given from Min to Max.<br/>
        /// If less than Min, Min will be the set value.<br/>
        /// If more than Max, Max will be the set value.
        /// </summary>
        /// <param name="min">Minimum value allowed</param>
        /// <param name="max">Maximum value allowed</param>
        /// <param name="decCount">(Range (0-4): Rounds to specific decimal.<br/>
        /// If the decCount value isn't passed in, is less than 0, or more than 4: Default: 0)</param>
        /// <returns>value forced within range.</returns>
        public static T SetBoundary<T>(this T value, T min, T max, byte decCount = 0) where T : IComparable<T>
        {
            if (!IsSupportedNumeric<T>())
                throw new NotSupportedException($"{typeof(T).Name} is not a supported numeric type.");

            //if this return type doesn't have decimal values, and decCount is greater than 0, set to 0;
            if (decCount > 0 && _decimalTypes.Where(w => w.Name.Equals(typeof(T).Name)).Count().Equals(0))
                decCount = 0;

            //validate decimal count based on response type.
            var dec = decCount.ClampTo<byte>(0, 4);
            //set range value
            var retVal = (value.CompareTo(min) < 0) ? min : (value.CompareTo(max) > 0) ? max : value;

            //convert to double for rounding, even if Int.  If Int, dec would of been forced to 0.
            //It will be removed later, but required for Math.Round() to be a decimal type value.
            //this allows for function to be generic across all _validBoundaryTypes
            var conv = (double)Convert.ChangeType(retVal, typeof(double));

            //validate min and max and round if necessary.
            return (T)Convert.ChangeType(Math.Round(conv, dec), typeof(T));
        }
        /// <summary>
        /// Since netstandard2.0 doesn't have Math.Clamp, this will do in.
        /// </summary>
        /// <typeparam name="T"></typeparam>
        /// <param name="value"></param>
        /// <param name="min"></param>
        /// <param name="max"></param>
        /// <returns></returns>
        public static T ClampTo<T>(this T value, T min, T max)
            where T : IComparable<T>
        {
            if (value.CompareTo(min) < 0) return min;
            if (value.CompareTo(max) > 0) return max;
            return value;
        }
        /// <summary>
        /// For some reason, MS didn't put a Guid.IsEmpty property, but has a Guid.Empty static property.  This fixed that.
        /// </summary>
        /// <returns>true: Guid was created with Guid.Empty static property.  false: Guid was initialized properly.</returns>
        public static bool IsEmpty(this Guid guid) => guid == Guid.Empty;

        #region Private Helper Methods
        private static bool IsSupportedNumeric<T>() => _validBoundaryTypes.Contains(typeof(T));
        #endregion
    }
}
