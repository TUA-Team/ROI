using System;
using System.Collections.Generic;
using System.IO;
using System.Runtime.Serialization;
using Terraria.ModLoader;

namespace API.Networking
{
    public static class PacketManager
    {
        private struct PacketStyleInfo
        {
            private readonly Mod mod;
            public readonly int id;
            public readonly Action<BinaryReader, int> Read;

            public PacketStyleInfo(Mod mod, int id, Action<BinaryReader, int> read)
            {
                this.mod = mod;
                this.id = id;
                Read = read;
            }

            public ModPacket MakePacket()
            {
                var p = mod.GetPacket();
                p.Write(id);
                return p;
            }
        }

        private static readonly List<PacketStyleInfo> styles = new List<PacketStyleInfo>();

        public static void LoadPacketsFrom(Mod mod)
        {
            foreach (var type in mod.Code.DefinedTypes)
            {
                if (type.IsAbstract)
                    continue;

                if (type.IsSubclassOf(typeof(NetworkPacket)))
                {
                    var instance = (NetworkPacket)FormatterServices.GetUninitializedObject(type);
                    var style = new PacketStyleInfo(mod, styles.Count, instance.Read);

                    IdByType.Register(type, style.id);
                    styles.Add(style);
                }
            }
        }

        public static ModPacket GetPacketFor(Type type) => styles[IdByType.Get(type)].MakePacket();

        public static void Handle(BinaryReader reader, int sender)
        {
            styles[reader.ReadInt32()].Read(reader, sender);
        }
    }
}
