using System.IO;
using Terraria.ModLoader;

namespace ROI.Helpers.Networking
{
    public abstract class NetworkPacket
    {
        public NetworkPacket(Mod mod)
        {
            Mod = mod;
        }

        public abstract bool Receive(BinaryReader reader, int fromWho);

        public void SendPacket(int toWho, int fromWho, params object[] args) => SendPacket(MakePacket(), toWho, fromWho, args);

        protected abstract void SendPacket(ModPacket packet, int toWho, int fromWho, params object[] args);

        public void SendPacketToAllClients(int fromWho, params object[] args) => SendPacket(-1, fromWho, args);
        public void SendPacketToServer(int fromWho, params object[] args) => SendPacket(256, fromWho, args);


        protected ModPacket MakePacket()
        {
            ModPacket packet = Mod.GetPacket();
            packet.Write(PacketType);

            return packet;
        }

        public byte PacketType { get; internal set; }

        public Mod Mod { get; }
    }
}
