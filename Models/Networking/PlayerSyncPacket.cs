using API.Networking;
using ROI.Players;
using ROI.Void;
using System.IO;
using Terraria;
using Terraria.ModLoader;

namespace ROI.Models.Networking
{
    public sealed class PlayerSyncPacket : NetworkPacket
    {
        public PlayerSyncPacket(Mod mod) : base(mod) { }

        public override void Receive(BinaryReader reader, int fromWho) {
            ushort voidAffinity = reader.ReadUInt16();
            byte voidTier = reader.ReadByte();
            int voidItemCooldown = reader.ReadInt32();

            ROIPlayer roiPlayer = ROIPlayer.Get(Main.player[fromWho]);

            roiPlayer.VoidAffinity = voidAffinity;
            roiPlayer.VoidTier = (VoidTier)voidTier;
            roiPlayer.VoidItemCooldown = voidItemCooldown;
        }

        protected override void SendPacket(ModPacket packet, params object[] args) {
            packet.Write((ushort)args[0]);
            packet.Write((byte)args[1]);
            packet.Write((int)args[2]);
        }
    }
}
