using System;
using System.Collections.Generic;
using Terraria.ModLoader;

namespace API
{
    /// <summary>
    /// This class assumes that there will never ever be another instance of the specified IHaveId object.
    /// This means that any type derived from IHaveId and used in tandem with a CollectionLoader must be a singleton
    /// </summary>
    public static class IdHolder
    {
        public static Dictionary<Type, IHaveId> objsByType = new Dictionary<Type, IHaveId>();

        internal static void Link(Type t, Action<byte> update) => objsByType[t].Link(update);

        public static void Register(IHaveId obj) => objsByType.Add(obj.GetType(), obj);
    }

    public static class IdHolder<T> where T : IHaveId
    {
        public static byte Id { get; private set; }

        static IdHolder()
        {
            ModLoader.GetMod("ROI").Logger.Info($"IdHolder registered for {nameof(T)}");
            IdHolder.Link(typeof(T), Link);
        }

        private static void Link(byte instance)
        {
            Id = instance;
        }
    }
}