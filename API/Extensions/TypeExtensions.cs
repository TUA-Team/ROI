using System;
using System.Collections.Generic;
using System.Reflection;

namespace ROI.API.Extensions
{
    public static class TypeExtensions
    {
        // TODO: (low prio) cache these
        public static T GetField<T>(this object parent, string name,
            BindingFlags flags = BindingFlags.NonPublic | BindingFlags.Instance)
        {
            return (T)parent.GetType().GetField(name, flags).GetValue(parent);
        }

        public static T[] GenerateNestedTypes<T>(this Type type,
            BindingFlags flags = BindingFlags.Public | BindingFlags.NonPublic | BindingFlags.Instance)
        {
            var types = type.GetNestedTypes(flags);
            var list = new List<T>();
            for (int i = 0; i < types.Length; i++)
            {
                var nested = types[i];
                if (nested.IsAbstract)
                    continue;
                if (nested.ContainsGenericParameters)
                    continue;
                if (nested.IsSubclassOf(typeof(T)))
                {
                    list.Add((T)Activator.CreateInstance(nested));
                }
            }
            return list.ToArray();
        }
    }
}
