using API.Networking;
using ROI.Models.Networking;
using System.IO;
using Terraria.ModLoader;

namespace API
{
    // TODO: (low prio) create a cecil based generator for NetworkPacket
    public sealed class NetworkPacketLoader : CollectionLoader<NetworkPacket>
    {
        public override void Initialize(Mod mod) {
            PlayerSync = Add(new PlayerSyncPacket(mod));
        }


        public PlayerSyncPacket PlayerSync { get; private set; }


        public void HandlePacket(BinaryReader reader, int sender) {
            byte packetType = reader.ReadByte();

            base[packetType].Receive(reader, sender);
        }
    }
}
