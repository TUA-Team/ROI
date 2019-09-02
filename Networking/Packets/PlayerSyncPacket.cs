using System.IO;
using ROI.Players;
using ROI.Void;
using Terraria;
using Terraria.ModLoader;

namespace ROI.Networking.Packets
{
    public sealed class PlayerSyncPacket : NetworkPacket
    {
        public override bool Receive(BinaryReader reader, int fromWho)
        {
            int whichPlayer = reader.ReadInt32();
            ushort voidAffinity = reader.ReadUInt16();
            byte voidTier = reader.ReadByte();
            int voidItemCooldown = reader.ReadInt32();

            if (Main.dedServ)
                NetworkPacketManager.Instance.PlayerSync.SendPacketToAllClients(fromWho, whichPlayer, voidAffinity, voidTier, voidItemCooldown);

            ROIPlayer roiPlayer = ROIPlayer.Get(Main.player[whichPlayer]);

            roiPlayer.VoidAffinity = voidAffinity;
            roiPlayer.VoidTier = (VoidTiers) voidTier;
            roiPlayer.VoidItemCooldown = voidItemCooldown;

            return true;
        }

        protected override void SendPacket(ModPacket packet, int toWho, int fromWho, params object[] args)
        {
            packet.Write((int) args[0]);
            packet.Write((ushort) args[1]);
            packet.Write((byte) args[2]);
            packet.Write((int) args[3]);

            packet.Send(toWho, fromWho);
        }
    }
}
