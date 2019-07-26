using System.IO;
using Terraria.ModLoader;

namespace ROI.Networking.Packets
{
    public sealed class PlayerSyncPacket : NetworkPacket
    {
        public override bool Receive(BinaryReader reader, int fromWho)
        {
            
        }

        protected override void SendPacket(ModPacket packet, int toWho, int fromWho, params object[] args)
        {
            
        }
    }
}
