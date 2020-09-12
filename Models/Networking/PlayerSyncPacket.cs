using API.Networking;
using ROI.Players;
using ROI.Void;
using System.IO;
using Terraria;

namespace ROI.Models.Networking
{
    public sealed class PlayerSyncPacket : NetworkPacket
    {
        public override void ReceiveData(BinaryReader reader, int fromWho)
        {
            ushort voidAffinity = reader.ReadUInt16();
            byte voidTier = reader.ReadByte();
            int voidItemCooldown = reader.ReadInt32();

            ROIPlayer roiPlayer = ROIPlayer.Get(Main.player[fromWho]);

            roiPlayer.VoidAffinity = voidAffinity;
            roiPlayer.VoidTier = (VoidTier)voidTier;
            roiPlayer.VoidItemCooldown = voidItemCooldown;
        }

        protected override void WriteData(object state)
        {
            var plr = state as ROIPlayer;

            Write(plr.VoidAffinity);
            Write((byte)plr.VoidTier);
            Write(plr.VoidItemCooldown);
        }
    }
}
