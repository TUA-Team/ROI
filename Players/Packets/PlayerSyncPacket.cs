using API.Networking;
using System.IO;
using Terraria;

namespace ROI.Players.Packets
{
    public sealed class PlayerSyncPacket : NetworkPacket<ROIPlayer>
    {
        public PlayerSyncPacket() : base((writer, state) => {
            writer.Write(state.VoidAffinity);
            writer.Write(state.MaxVoidAffinity);
        })
        { }

        public override void Receive(BinaryReader reader, int fromWho)
        {
            var plr = ROIPlayer.Get(Main.player[fromWho]);
            plr.VoidAffinity = reader.ReadInt16();
            plr.MaxVoidAffinity = reader.ReadInt16();
        }
    }
}
