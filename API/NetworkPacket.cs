using System.IO;
using Terraria.ModLoader;

namespace API.Networking
{
    public abstract class NetworkPacket : IdBasedObject
    {
        public NetworkPacket(Mod mod) {
            Mod = mod;
        }

        public abstract void Receive(BinaryReader reader, int fromWho);

        public void SendPacket(int toWho, int fromWho, params object[] args) {
            var p = MakePacket();
            SendPacket(p, args);
            p.Send(toWho, fromWho);
        }

        protected abstract void SendPacket(ModPacket packet, params object[] args);

        // public void SendPacketToAllClients(int fromWho, params object[] args) => SendPacket(-1, fromWho, args);
        // public void SendPacketToServer(int fromWho, params object[] args) => SendPacket(256, fromWho, args);


        protected ModPacket MakePacket() {
            var p = Mod.GetPacket();
            p.Write(MyId);

            return p;
        }


        public Mod Mod { get; }
    }
}
