using System;
using System.Collections.Generic;
using Terraria.ModLoader;

namespace ROI.API
{
    public sealed class IdByType : ModType
    {
        private static readonly Dictionary<Type, int> typeToId = new Dictionary<Type, int>();

        public static void Register(Type type, int id)
        {
            typeToId.Add(type, id);
        }

        public static int Get(Type type) => typeToId[type];

        public override void Unload() => typeToId.Clear();
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
