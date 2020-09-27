using System;
using System.Collections.Generic;

namespace API
{
    public static class IdByType
    {
        private static readonly Dictionary<Type, int> typeToId = new Dictionary<Type, int>();

        public static void Register(Type type, int id)
        {
            typeToId.Add(type, id);
        }

        public static void Link(Type type, Action<int> action)
        {
            action(typeToId[type]);
        }
    }

    public static class IdByType<T> where T : IHaveId
    {
        public static int Id { get; private set; }

        static IdByType()
        {
            IdByType.Link(typeof(T), i => Id = i);
        }
    }
}
