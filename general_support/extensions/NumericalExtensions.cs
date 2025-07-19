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
        /// <param name="decCount">(Range (0-6): Rounds to specific decimal.<br/>
        /// <returns>Same numeric type within min and max range.</returns>
        public static T Clamp<T>(this T value, T min, T max, byte decPoint = 0) 
            where T : IComparable<T>
        {
            if (!IsSupportedNumeric<T>())
                throw new NotSupportedException($"{typeof(T).Name} is not a supported numeric type.");

            var isDecimalType = _decimalTypes.Where(w => w.Name.Equals(typeof(T).Name)).Count()>0;
            //if this return type doesn't have decimal values, and decPoint is greater than 0, set to 0;
            if (decPoint > 0 && !isDecimalType)
                decPoint = 0;
            else
            {
                //check decimal size, max at 6 decimal point value
                if (decPoint.CompareTo(0) < 0) decPoint = 0;
                if (decPoint.CompareTo(6) > 0) decPoint = 6;
            }

            //set range value
            if (value.CompareTo(min) < 0) value = min;
            if (value.CompareTo(max) > 0) value = max;

            //if it's not a deciaml type like double, float, or decimal, then return validated value
            if (!isDecimalType)
                return value;

            //convert to double for rounding, then change on return to expected decimal type
            var conv = (double)Convert.ChangeType(value, typeof(double));

            //validate min and max and round if necessary.
            return (T)Convert.ChangeType(Math.Round(conv, decPoint), typeof(T));
        }

        private static bool IsSupportedNumeric<T>() => _validBoundaryTypes.Contains(typeof(T));
    }
}
