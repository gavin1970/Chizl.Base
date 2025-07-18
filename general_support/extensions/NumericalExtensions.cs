using System;
using System.Linq;
using System.Collections.Generic;

namespace Chizl.Extensions
{
    public static class NumericalExtensions
    {
        private static readonly HashSet<Type> _validBoundaryTypes = new HashSet<Type>
        {
            typeof(byte),      // 0-255
            typeof(short),     // Int16
            typeof(int),       // Int32
            typeof(long),      // Int64
            typeof(float),     // Single
            typeof(double),    // Double
            typeof(decimal),   // Decimal
            typeof(TimeSpan)   // Its internal representation is based on a long value representing the number of "ticks"
        };

        private static readonly HashSet<Type> _decimalTypes = new HashSet<Type>
        {
            typeof(float),     // Single
            typeof(double),    // Double
            typeof(decimal)    // Decimal
        };

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
            var dec = decCount.Clamp<byte>(0, 4);
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
        /// Since netstandard2.0 doesn't have Math.Clamp, this will work for all library versions.<br/>
        /// <code>
        /// Supported Numerical Types:
        /// * byte       // 0-255
        /// * short      // Int16
        /// * int        // Int32
        /// * long       // Int64
        /// * float      // Single
        /// * double     // Double
        /// * decimal    // Decimal
        /// * TimeSpan   // Its internal representation is based on a long value representing the number of "ticks"
        /// ------------------
        /// Example:
        /// int num1 = 1024;
        /// double num2 = 24.55;
        /// var ts = TimeSpan.FromSeconds(20);
        /// num1 = num1.Clamp(0, 1000);         // return: 1000
        /// num2 = num2.Clamp(100.44, 1000.44); // return: 100.44
        /// ts = ts.Clamp(TimeSpan.FromSeconds(5), TimeSpan.FromSeconds(10));   // return: 00.00.10
        /// </code>
        /// </summary>
        /// <typeparam name="T">Numerical Type</typeparam>
        /// <param name="min">Min Value, must be same type as evaluated</param>
        /// <param name="max">Max Value, must be same type as evaluated</param>
        /// <returns>Same numeric type within min and max range.</returns>
        public static T Clamp<T>(this T value, T min, T max)
            where T : IComparable<T>
        {
            if (!IsSupportedNumeric<T>())
                throw new NotSupportedException($"{typeof(T).Name} is not a supported numeric type.");

            if (value.CompareTo(min) < 0) return min;
            if (value.CompareTo(max) > 0) return max;
            return value;
        }
        private static bool IsSupportedNumeric<T>() => _validBoundaryTypes.Contains(typeof(T));
    }
}
