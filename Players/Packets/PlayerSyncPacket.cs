using API.Networking;
using System.IO;
using Terraria;

namespace ROI.Players.Packets
{
    public sealed class PlayerSyncPacket : NetworkPacket<ROIPlayer>
    {
        protected override void ReceiveData(BinaryReader reader, int fromWho)
        {
            var plr = ROIPlayer.Get(Main.player[fromWho]);
            plr.VoidAffinity = reader.ReadInt16();
            plr.MaxVoidAffinity = reader.ReadInt16();
        }

        protected override void WriteData(BinaryWriter writer)
        {
            writer.Write(state.VoidAffinity);
            writer.Write(state.MaxVoidAffinity);
        }
    }
}
