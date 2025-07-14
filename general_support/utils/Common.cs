using System;
using System.Collections.Concurrent;

namespace Chizl.Base.Utils
{
    internal static class Common
    {
        private static readonly ConcurrentDictionary<Type, object> _defaultCache = new ConcurrentDictionary<Type, object>();

        internal static object GetDefault(Type type)
        {
            if (type == null)
                throw new ArgumentNullException(nameof(type));

            return _defaultCache.GetOrAdd(type, GetDefaultValueInternal);
        }

        private static object GetDefaultValueInternal(Type type)
        {
            // Handle value types directly
            if (type.IsValueType)
                return Activator.CreateInstance(type); // or FormatterServices.GetUninitializedObject

            // Handle reference types
            return null;
        }
    }
}
