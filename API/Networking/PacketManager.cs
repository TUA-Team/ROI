using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using Terraria.ModLoader;

namespace ROI.API.Networking
{
    public class PacketManager : Singleton<PacketManager>
    {
        private struct PacketStyleInfo
        {
            public readonly int Id;
            public readonly Action<BinaryReader, int> Read;

            public PacketStyleInfo(int id, Action<BinaryReader, int> read)
            {
                Id = id;
                Read = read;
            }
        }

        private Dictionary<Type, int> byType;
        private PacketStyleInfo[] styles;

        public override void Load()
        {
            styles = Mod.Code.DefinedTypes.Concrete<NetworkPacket>()
                .Select((t, i) => new PacketStyleInfo(i, ((NetworkPacket)FormatterServices.GetUninitializedObject(t)).Read))
                .ToArray();
        }

        public ModPacket GetPacketFor(Type type)
        {
            var packet = Mod.GetPacket();
            packet.Write(styles[byType[type]].Id);
            return packet;
        }

        public void Handle(BinaryReader reader, int sender) => styles[reader.ReadInt32()].Read(reader, sender);
    }
}
