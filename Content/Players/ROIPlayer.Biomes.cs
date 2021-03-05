using ROI.Content.Biomes.Wasteland;
using System.IO;
using Terraria;

namespace ROI.Content.Players
{
    partial class ROIPlayer
    {
        public bool ZoneWasteland;


        public override void UpdateBiomes()
        {
            ZoneWasteland = WastelandWorld.wastelandTiles > 100;
        }

        public override bool CustomBiomesMatch(Player other)
        {
            ROIPlayer modOther = other.GetModPlayer<ROIPlayer>();
            return ZoneWasteland == modOther.ZoneWasteland;
        }

        public override void CopyCustomBiomesTo(Player other)
        {
            ROIPlayer modOther = other.GetModPlayer<ROIPlayer>();
            modOther.ZoneWasteland = ZoneWasteland;
        }

        public override void SendCustomBiomes(BinaryWriter writer)
        {
            BitsByte flags = new BitsByte();
            flags[0] = ZoneWasteland;
            writer.Write(flags);
        }

        public override void ReceiveCustomBiomes(BinaryReader reader)
        {
            BitsByte flags = reader.ReadByte();
            ZoneWasteland = flags[0];
        }
    }
}
