using System;
using System.Reflection;
using System.Collections.Generic;

namespace Chizl.Base.@Internal.utils
{
    internal class Common
    {
        #region Private Strucs/Enums
        /// <summary>
        /// Used by TypeDefaults List for storage.
        /// </summary>
        private struct Type_Default
        {
            public Type dataType;
            public object defaultValue;
        }
        #endregion

        #region Private Static Vars.
        /// <summary>
        /// Search and add to TypeDefaults, we need to lock, just in case someone is multi-threading.
        /// </summary>
        private static readonly object _lockObj = new object();
        /// <summary>
        /// Used by GetDefault() and stores defaults after first use so the following times are faster response times.
        /// </summary>
        private static readonly List<Type_Default> TypeDefaults = new List<Type_Default>();
        #endregion

        #region Internal Helper Methods
        internal static object GetDefault(Type t)
        {
            object retVal;

            lock (_lockObj)
            {
                //don't want to constantly create generics for every column, if we already have type and it's default data in memory.
                var found = TypeDefaults.Find(f => f.dataType.Equals(t));

                if (found.dataType == t)
                    return found.defaultValue;

                var cmn = new Common();
                var metInfo = cmn.GetType().GetMethod("GetDefaultValue", BindingFlags.Static |
                                                                         BindingFlags.NonPublic |
                                                                         BindingFlags.Instance);

                //something has gone wrong, we can't access our own private method.
                if (metInfo == null)
                    return null;

                //setup generic to pass type
                var genericFooMethod = metInfo.MakeGenericMethod(t);
                //call and get the Type's default data.
                retVal = genericFooMethod.Invoke(cmn, null);

                //add to static for later use.
                TypeDefaults.Add(
                    new Type_Default()
                    {
                        dataType = t,
                        defaultValue = retVal
                    });
            }

            return retVal;
        }
        #endregion

        #region Private Helper Methods
        /// <summary>
        /// *** DO NOT REMOVE ***
        /// Dynamically used and required by 'GetDefault()'.
        /// </summary>
        private static T GetDefaultValue<T>() => default;
        #endregion
    }
}
