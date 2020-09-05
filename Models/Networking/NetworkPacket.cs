using System.IO;
using Terraria.ModLoader;

namespace ROI.Models.Networking
{
    public abstract class NetworkPacket
    {
        public NetworkPacket(Mod mod) {
            Mod = mod;
        }

        public abstract void Receive(BinaryReader reader, int fromWho);

        public void SendPacket(int toWho, int fromWho, params object[] args) {
            var p = MakePacket();
            SendPacket(p, toWho, fromWho, args);
            p.Send();
        }

        protected abstract void SendPacket(ModPacket packet, int toWho, int fromWho, params object[] args);

        // public void SendPacketToAllClients(int fromWho, params object[] args) => SendPacket(-1, fromWho, args);
        // public void SendPacketToServer(int fromWho, params object[] args) => SendPacket(256, fromWho, args);


        protected ModPacket MakePacket() {
            var p = Mod.GetPacket();
            p.Write(PacketType);

            return p;
        }

        public byte PacketType { get; internal set; }

        public Mod Mod { get; }
    }
}
