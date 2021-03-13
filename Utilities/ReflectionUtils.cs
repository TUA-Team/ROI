using System;
using System.Collections.Generic;
using System.Reflection;

namespace ROI.Utilities
{
    public static class ReflectionUtils
    {
        // TODO: (low prio) cache these
        public static T GetField<T>(this object parent, string name,
            BindingFlags flags = BindingFlags.NonPublic | BindingFlags.Instance)
        {
            return (T)parent.GetType().GetField(name, flags).GetValue(parent);
        }

        public static IEnumerable<Type> Concrete<T>(this IEnumerable<Type> types)
        {
            foreach (var type in types)
            {
                if (!type.IsAbstract && type.IsSubclassOf(typeof(T)))
                    yield return type;
            }
        }
    }
}
