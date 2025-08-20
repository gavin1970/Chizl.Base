using System;

namespace Chizl.Extensions
{
    public static class EnumExtensions
    {
        /// <summary>
        /// Returns the 'string' name of the Enums value being used.<br/>
        /// <code>
        /// Example:
        ///     var e = MyEnum.Property
        ///     Console.WriteLine($"Enum property name: {e.Name()}");
        /// </code>
        /// </summary>
        /// <returns>string name of enum property</returns>
        public static string Name<T>(this T @this) where T : Enum => $"{@this}";
        /// <summary>
		/// Returns the 'int' value of the Enums value being used.<br/>
        /// <code>
        /// Example:
        ///     var e = MyEnum.Property
        ///     Console.WriteLine($"Enum property value: {e.Value()}");
        /// </code>
        /// </summary>
        /// <returns>int value of enum property</returns>
        public static int Value<T>(this T @this) where T : Enum => @this.ToInt();
        /// <summary>
        /// Used for all Enum values and converts them to Int.<br/>
        /// <code>
        /// Example:<br/>
        ///     var e = MyEnum.Property<br/>
        ///     Console.WriteLine($"Enum property value: {e.ToInt()}");
        /// </code>
        /// </summary>
        /// <returns>string name of enum property as an Int</returns>
        public static int ToInt<T>(this T @this) where T : Enum => unchecked((int)(object)@this);
        /// <summary>
        /// Used for all Enum values and converts them to UInt.<br/>
        /// <code>
        /// Example:<br/>
        ///     var e = MyEnum.Property<br/>
        ///     Console.WriteLine($"Enum property value: {e.ToUInt()}");
        /// </code>
        /// </summary>
        /// <returns>string name of enum property as a UInt.</returns>
        public static uint ToUInt<T>(this T @this) where T : Enum => unchecked((uint)(object)@this);
    }
}
