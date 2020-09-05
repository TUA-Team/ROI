using ROI.Models.Networking;
using System.Collections.Generic;
using System.IO;
using Terraria.ModLoader;

namespace ROI.Loaders
{
    public sealed class NetworkPacketLoader : BaseLoader
    {
        private byte _latestPacketTypeId = 1;
        private readonly Dictionary<byte, NetworkPacket> _networkPacketsById = new Dictionary<byte, NetworkPacket>();

        public override void Initialize(Mod mod) {
            PlayerSync = Add(new PlayerSyncPacket(mod));
        }


        public PlayerSyncPacket PlayerSync { get; private set; }


        public T Add<T>(T networkPacket) where T : NetworkPacket {
            _networkPacketsById.Add(_latestPacketTypeId, networkPacket);

            networkPacket.PacketType = _latestPacketTypeId;
            _latestPacketTypeId++;

            return networkPacket;
        }

        public void HandlePacket(BinaryReader reader, int sender) {
            byte packetType = reader.ReadByte();

            _networkPacketsById[packetType].Receive(reader, sender);
        }


        // public NetworkPacket this[byte packetType] => _networkPacketsById[packetType];
    }
}
