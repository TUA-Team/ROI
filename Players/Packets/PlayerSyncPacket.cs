using ROI.API.Networking;
using System.IO;

namespace ROI.Players.Packets
{
    public sealed class PlayerSyncPacket : NetworkPacket
    {
        public PlayerSyncPacket(ROIPlayer plr)
        {
            Writer.Write(plr.VoidAffinity);
            Writer.Write(plr.MaxVoidAffinity);
        }

        public override void Read(BinaryReader reader, int sender)
        {
            var plr = ROIPlayer.Get(sender);

            plr.VoidAffinity = reader.ReadInt16();
            plr.MaxVoidAffinity = reader.ReadInt16();
        }
    }
}
