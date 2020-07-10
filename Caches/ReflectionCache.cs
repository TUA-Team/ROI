using System;
using System.Collections.Generic;
using System.Linq;
using System.Reflection;
using System.Text;
using System.Threading.Tasks;

namespace ROI.Caches
{
    class ReflectionCache
    {
        internal static Dictionary<Type, Dictionary<string, Delegate>> methodCache = new Dictionary<Type, Dictionary<string, Delegate>>();
        internal static Dictionary<Type, Dictionary<string, PropertyInfo>> propertyCache = new Dictionary<Type, Dictionary<string, PropertyInfo>>();
        internal static Dictionary<Type, Dictionary<string, FieldInfo>> fieldCache = new Dictionary<Type, Dictionary<string, FieldInfo>>();
    }
}
