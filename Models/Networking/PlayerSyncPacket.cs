using ROI.Players;
using ROI.Void;
using System.IO;
using Terraria;

namespace ROI.Models.Networking
{
    public sealed class PlayerSyncPacket : NetworkPacket<ROIPlayer>
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

        protected override void WriteData(ROIPlayer state)
        {
            Write(state.VoidAffinity);
            Write((byte)state.VoidTier);
            Write(state.VoidItemCooldown);
        }
    }
}
