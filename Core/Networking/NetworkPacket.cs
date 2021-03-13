using System.IO;
using Terraria.ModLoader;

namespace ROI.Core.Networking
{
    public abstract class NetworkPacket
    {
        private readonly ModPacket writer;

        protected NetworkPacket()
        {
            writer = PacketManager.Instance.GetPacketFor(GetType());
        }

        public void Send(int to = -1, int ignore = -1)
        {
            writer.Send(to, ignore);
        }


        public abstract void Read(BinaryReader reader, int sender);


        public BinaryWriter Writer => writer;
    }
}