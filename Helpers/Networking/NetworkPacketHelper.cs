using ROI.Helpers.Networking.Packets;
using System;
using System.Collections.Generic;
using System.IO;
using Terraria.ModLoader;

namespace ROI.Helpers.Networking
{
    public sealed class NetworkPacketHelper : BaseHelper
    {
        private byte _latestPacketTypeId = 1;
        private readonly Dictionary<byte, NetworkPacket> _networkPacketsById = new Dictionary<byte, NetworkPacket>();
        private readonly Dictionary<Type, NetworkPacket> _networkPacketsByType = new Dictionary<Type, NetworkPacket>();

        public override void Initialize(Mod mod)
        {
            PlayerSync = Add(new PlayerSyncPacket(mod)) as PlayerSyncPacket;
        }


        public PlayerSyncPacket PlayerSync { get; private set; }


        public NetworkPacket Add<T>(T networkPacket) where T : NetworkPacket
        {
            if (_networkPacketsById.ContainsValue(networkPacket))
                return _networkPacketsByType[networkPacket.GetType()];

            _networkPacketsById.Add(_latestPacketTypeId, networkPacket);
            _networkPacketsByType.Add(networkPacket.GetType(), networkPacket);

            networkPacket.PacketType = _latestPacketTypeId;
            _latestPacketTypeId++;

            return networkPacket;
        }

        public void HandlePacket(BinaryReader reader, int fromWho)
        {
            byte packetType = reader.ReadByte();

            _networkPacketsById[packetType].Receive(reader, fromWho);
        }


        public NetworkPacket this[byte packetType] => _networkPacketsById[packetType];
    }
}
