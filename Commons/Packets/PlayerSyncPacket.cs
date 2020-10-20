﻿using API;
using ROI.Players;
using System.IO;
using Terraria;

namespace ROI.Commons.Packets
{
    public sealed class PlayerSyncPacket : NetworkPacket<ROIPlayer>
    {
        protected override void ReceiveData(BinaryReader reader, int fromWho)
        {
            var plr = ROIPlayer.Get(Main.player[fromWho]);
            plr.VoidAffinity = reader.ReadInt16();
            plr.MaxVoidAffinity = reader.ReadInt16();
        }

        protected override void WriteData(BinaryWriter writer, ROIPlayer state)
        {
            writer.Write(state.VoidAffinity);
            writer.Write(state.MaxVoidAffinity);
        }
    }
}