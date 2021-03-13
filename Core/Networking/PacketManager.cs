using ROI.Utilities;
using System;
using System.Collections.Generic;
using System.IO;
using System.Linq;
using System.Runtime.Serialization;
using Terraria.ModLoader;

namespace ROI.Core.Networking
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

        private Dictionary<Type, int> typeToIdMap;
        private PacketStyleInfo[] styles;

        public override void Load()
        {
            var types = Mod.Code.DefinedTypes.Concrete<NetworkPacket>();
            typeToIdMap = new Dictionary<Type, int>(types.Count());
            styles = new PacketStyleInfo[types.Count()];

            int i = 0;
            foreach (Type type in types)
            {
                typeToIdMap.Add(type, i);
                styles[i] = new PacketStyleInfo(i, ((NetworkPacket)FormatterServices.GetUninitializedObject(type)).Read);

                i++;
            }
        }

        public ModPacket GetPacketFor(Type type)
        {
            var packet = Mod.GetPacket();
            packet.Write(typeToIdMap[type]);
            return packet;
        }

        public void Handle(BinaryReader reader, int sender) => styles[reader.ReadInt32()].Read(reader, sender);
    }
}
