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
        /// <summary>
        /// Convert IntPtr to int.  This will support 16, 32, and 64 bit systems based on local OS.<br/>
        /// ___________________
        /// <code>
        /// IntPtr myIntPtr = new IntPtr(10000);<br/>
        /// int myInt = myIntPtr.ToInt();<br/>
        /// </code>
        /// </summary>
        public static int ToInt(this IntPtr ptr) => unchecked(((uint)ptr).ToInt());
        /// <summary>
        /// Convert IntPtr to uint.  This will support 16, 32, and 64 bit systems based on local OS.<br/>
        /// ___________________
        /// <code>
        /// IntPtr myIntPtr = new IntPtr(10000);<br/>
        /// uint myUInt = myIntPtr.ToUInt();<br/>
        /// </code>
        /// </summary>
        public static uint ToUInt(this IntPtr ptr) => unchecked((uint)ptr);
        /// <summary>
        /// Convert uint to IntPtr.  This will support 16, 32, and 64 bit systems based on local OS.<br/>
        /// ___________________
        /// <code>
        /// uint myUint = 0x00D;<br/>
        /// IntPtr myIntPtr = myUint.ToIntPtr();<br/>
        /// </code>
        /// </summary>
        public static IntPtr ToIntPtr(this uint ui) => unchecked(new IntPtr(ui));
        /// <summary>
        /// Convert int to IntPtr.  This will support 16, 32, and 64 bit systems based on local OS.<br/>
        /// ___________________
        /// <code>
        /// int myInt = 0x00D;<br/>
        /// IntPtr myIntPtr = myInt.ToIntPtr();<br/>
        /// </code>
        /// </summary>
        public static IntPtr ToIntPtr(this int ui) => unchecked(new IntPtr(ui.ToUInt()));
        /// <summary>
        /// Convert UInt16 to Int16<br/>
        /// ___________________
        /// <code>
        /// UInt16 myUint = 0x00D;<br/>
        /// Int16 myInt = myUint.ToInt();<br/>
        /// </code>
        /// </summary>
        public static Int16 ToInt(this UInt16 i) => Convert.ToInt16(i);
        /// <summary>
        /// Convert UInt32 to Int32<br/>
        /// ___________________
        /// <code>
        /// UInt32 myUint = 0x00D;<br/>
        /// Int32 myInt = myUint.ToInt();<br/>
        /// </code>
        /// </summary>
        public static Int32 ToInt(this UInt32 i) => unchecked(Convert.ToInt32(i));
        /// <summary>
        /// Convert UInt64 to Int64<br/>
        /// ___________________
        /// <code>
        /// UInt64 myUint = 0x00D;<br/>
        /// Int64 myInt = myUint.ToInt();<br/>
        /// </code>
        /// </summary>
        public static Int64 ToInt(this UInt64 i) => Convert.ToInt64(i);
        /// <summary>
        /// Convert Int16 to UInt16<br/>
        /// ___________________
        /// <code>
        /// Int16 myInt = 3000;<br/>
        /// UInt16 myUint= myUint.ToUInt();<br/>
        /// </code>
        /// </summary>
        public static UInt16 ToUInt(this Int16 i) => Convert.ToUInt16(i);
        /// <summary>
        /// Convert Int32 to UInt32<br/>
        /// ___________________
        /// <code>
        /// Int32 myInt = 3000;<br/>
        /// UInt32 myUint= myUint.ToUInt();<br/>
        /// </code>
        /// </summary>
        public static UInt32 ToUInt(this Int32 i) => Convert.ToUInt32(i);
        /// <summary>
        /// Convert Int64 to UInt64<br/>
        /// ___________________
        /// <code>
        /// Int64 myInt = 3000;<br/>
        /// UInt64 myUint= myUint.ToUInt();<br/>
        /// </code>
        /// </summary>
        public static UInt64 ToUInt(this Int64 i) => Convert.ToUInt64(i);

        private static bool IsSupportedNumeric<T>() => _validBoundaryTypes.Contains(typeof(T));
    }
}
