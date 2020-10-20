using Microsoft.Xna.Framework;
using System.IO;
using Terraria;
using Terraria.Graphics.Effects;

namespace ROI.Players
{
    partial class ROIPlayer
    {
        public bool ZoneWasteland;


        public override void UpdateBiomes()
        {
            Point pos = player.position.ToTileCoordinates();
            ZoneWasteland = WorldGen.crimson && pos.Y > Main.maxTilesY - 200;
        }

        public override bool CustomBiomesMatch(Player other)
        {
            var otherPlr = other.GetModPlayer<ROIPlayer>();

            return ZoneWasteland == otherPlr.ZoneWasteland;
        }

        public override void CopyCustomBiomesTo(Player other)
        {
            var otherPlr = other.GetModPlayer<ROIPlayer>();

            otherPlr.ZoneWasteland = ZoneWasteland;
        }

        public override void SendCustomBiomes(BinaryWriter writer)
        {
            var flags = new BitsByte();
            flags[0] = ZoneWasteland;
            writer.Write(flags);
        }

        public override void ReceiveCustomBiomes(BinaryReader reader)
        {
            BitsByte flags = reader.ReadByte();
            ZoneWasteland = flags[0];
        }

        /*
        public override Texture2D GetMapBackgroundImage()
        {
            //TODO: wasteland bg
            return base.GetMapBackgroundImage();
        }
        */

        bool wastelandFilter;
        readonly Color color = new Color(64, 0, 0);
        public override void UpdateBiomeVisuals()
        {
            if (ZoneWasteland)
            {
                float percent = (player.position.Y / 16 - Main.maxTilesY + 300) / 300;
                if (!wastelandFilter)
                {
                    Filters.Scene.Activate("ROI:UnderworldFilter", player.Center)
                        .GetShader().UseColor(color).UseIntensity(percent).UseOpacity(percent);
                    wastelandFilter = true;
                }
                Filters.Scene["ROI:UnderworldFilter"].GetShader().UseColor(0f, 0f, 1f).UseIntensity(0.5f).UseOpacity(1f);
            }
            else
            {
                if (wastelandFilter)
                {
                    Filters.Scene["ROI:UnderworldFilter"].Deactivate();
                    wastelandFilter = false;
                }
            }
        }
    }
}
