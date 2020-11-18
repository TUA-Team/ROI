using System.IO;
using Terraria.ModLoader;

namespace API.Networking
{
    public abstract class NetworkPacket
	{
        private readonly ModPacket writer;

        protected NetworkPacket()
        {
            writer = PacketManager.GetPacketFor(GetType());
        }

        public void Send(int to = -1, int ignore = -1)
        {
            writer.Send(to, ignore);
        }


        public abstract void Read(BinaryReader reader, int sender);


        protected BinaryWriter Writer => writer;
    }
}