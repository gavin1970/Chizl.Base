using System;
using System.Collections.Concurrent;

namespace Chizl.Extensions
{
    public static class ChizlExtensions
    {
        private static readonly ConcurrentDictionary<Type, object> _defaultTypeCache = new ConcurrentDictionary<Type, object>();

        #region Public Extensions
        /// <summary>
        /// Gets a substring from this instance. The substring starts at a specified character position and has a specified length.<br/>
        /// This method uses modern C# range operators (`..`) on supported frameworks (.NET Standard 2.1+, .NET 6+) 
        /// and falls back to the classic Substring method on older frameworks.<br/>
        /// Auto Rules:<br/>
        /// 1. If the 'str' is null, null will be returned instead of an exception.<br/>
        /// 2. If 'startIndex' is less than 0, it will be auto adjusted to 0.<br/>
        /// 3. If 'length' is less than 0, legth will be ignored all together and return startIndex to end of string.<br/>
        /// 4. If 'startIndex' + 'length' > 'str'.length, 'length' will be ignored and use rule #3.<br/>
        /// </summary>
        /// <param name="str">The source string.</param>
        /// <param name="startIndex">The zero-based starting character position of a substring in this instance.</param>
        /// <param name="length">(OPTIONAL) The number of characters to capture in the substring.  If zero or less, it's considered end of string.</param>
        /// <returns>A string that is equivalent to the substring of the specified length that begins at startIndex in this instance.</returns>
        public static string SubstringEx(this string str, int startIndex, int length = 0)
        {
            // If the string is null, return what was passed.
            if (str == null)
                return str;

            // index is passed the length of string
            if (startIndex > str.Length)
                return "";

            // audo adjust, assume math problem when called.
            if (startIndex < 0)
                startIndex = 0;

            // if if length is less than 0 or str.length is less than what is needed, ignore length.
            if (length < 0 || str.Length < startIndex + length)
                length = 0;

            // if length exists, SubString will be used differently than if missing.
            var useLength = length > 0;

            // NETSTANDARD2_1_OR_GREATER: Is defined for .NET Standard 2.1 and any newer .NET Standard version.
            // NET6_0_OR_GREATER: Is defined for .NET 6, .NET 7, .NET 8, .NET 9 and any future versions.
            // By combining these, we target all frameworks that support C# 8.0 features like ranges.
#if NETSTANDARD2_1_OR_GREATER || NET6_0_OR_GREATER
            // For modern frameworks, use the more concise and readable range operator.
            // The end of the range is exclusive, so startIndex + length works correctly.
            if (useLength)
                return str[startIndex..(startIndex + length)];
            else
                return str[startIndex..str.Length];
#else
            // For older frameworks (net47, net48, netstandard2.0), fall back
            // to the traditional Substring method.
            if (useLength)
                return str.Substring(startIndex, length);
            else
                return str.Substring(startIndex);
#endif
        }
        /// <summary>
        /// Finds and generates the default value based on Type.<br/>
        /// NOTE: This call caches the response so the next type this specific type is looked up, it's already in memory.
        /// </summary>
        /// <param name="t"></param>
        /// <returns>Default value based on Type</returns>
        public static object GetDefaultValue(this Type t) => GetDefault(t);

        /// <summary>
        /// For some reason, MS didn't put a Guid.IsEmpty property, but has a Guid.Empty static property.  This fixed that.
        /// </summary>
        /// <returns>true: Guid was created with Guid.Empty static property.  false: Guid was initialized properly.</returns>
        public static bool IsEmpty(this Guid guid) => guid == Guid.Empty;
        #endregion

        #region Private Helpers
        /// <summary>
        /// Takes the Type and pulls it from cache, unless it's not there, then pulls it from instance.
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        /// <exception cref="ArgumentNullException"></exception>
        private static object GetDefault(Type type)
        {
            if (type == null)
                throw new ArgumentNullException(nameof(type));

            return _defaultTypeCache.GetOrAdd(type, GetDefaultValueInternal);
        }
        /// <summary>
        /// If wasn't found in cache and if it has a value type, an instances is created to pull default value.
        /// </summary>
        /// <param name="type"></param>
        /// <returns></returns>
        private static object GetDefaultValueInternal(Type type)
        {
            // Handle value types directly
            if (type.IsValueType)
                return Activator.CreateInstance(type); // or FormatterServices.GetUninitializedObject

            // Handle reference types
            return null;
        }
        #endregion
    }
}
