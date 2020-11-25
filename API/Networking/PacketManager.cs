using System;
using System.Collections.Generic;
using System.IO;
using Terraria.ModLoader;

namespace ROI.API.Networking
{
    public sealed class PacketManager : Singleton<PacketManager>
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

        private readonly List<PacketStyleInfo> styles = new List<PacketStyleInfo>();

        public void Register(Mod mod, NetworkPacket packet)
        {
            var type = packet.GetType();
            var style = new PacketStyleInfo(mod, styles.Count, packet.Read);

            IdByType.Register(type, style.id);
            styles.Add(style);
        }

        public ModPacket GetPacketFor(Type type) => styles[IdByType.Get(type)].MakePacket();

        public void Handle(BinaryReader reader, int sender)
        {
            styles[reader.ReadInt32()].Read(reader, sender);
        }
    }
}
