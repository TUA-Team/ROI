using System;
using System.Collections.Generic;

namespace ROI.API
{
    public static class IdByType
    {
        private static readonly Dictionary<Type, int> typeToId = new Dictionary<Type, int>();

        public static void Register(Type type, int id)
        {
            typeToId.Add(type, id);
        }

        public static int Get(Type type) => typeToId[type];
    }

    public static class IdByType<T>
    {
        public static int Id { get; private set; }

        static IdByType()
        {
            IdByType.Get(typeof(T));
        }
    }
}
