using System.IO;
using Microsoft.Xna.Framework;
using Microsoft.Xna.Framework.Graphics;
using ROI.Effects;
using Terraria;
using Terraria.Graphics.Effects;
using Terraria.ModLoader;
using Terraria.ModLoader.IO;

namespace ROI.Players
{
    public sealed partial class ROIPlayer : ModPlayer
    {
        //Radiation in precent
        public float radiationLevel;

        public bool ZoneWasteland = false;

        public override void Initialize()
        {
            radiationLevel = 0;
            VAInit();
            WastelandInit();
        }

        public override void PostUpdate()
        {
            VAUpdate();
            WastelandUpdate();
        }

        public override TagCompound Save() => new TagCompound { { "VA", VASave () }
        };

        public override void Load(TagCompound tag)
        {
            VALoad(tag.GetCompound("VA"));
        }

        public override void UpdateBiomes()
        {
            Vector2 pos = player.position / 16;
            ZoneWasteland = pos.Y > Main.maxTilesY - 200 && WorldGen.crimson;
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

        public override Texture2D GetMapBackgroundImage()
        {
            //TODO: wasteland bg
            return base.GetMapBackgroundImage();
        }

        bool underworldFilter;
        public override void UpdateBiomeVisuals()
        {
            if (ZoneWasteland)
            {
                float percent = (Main.LocalPlayer.position.Y / 16 - Main.maxTilesY + 300) / 300;
                if (!underworldFilter)
                {
                    Filters.Scene.Activate("ROI:UnderworldFilter", Main.LocalPlayer.Center)
                        .GetShader().UseColor(UnderworldDarkness.hell).UseIntensity(percent).UseOpacity(percent);
                    underworldFilter = true;
                }
                Filters.Scene["ROI:UnderworldFilter"].GetShader().UseColor(0f, 0f, 1f).UseIntensity(0.5f).UseOpacity(1f);
            }
            else
            {
                if (underworldFilter)
                {
                    Filters.Scene["ROI:UnderworldFilter"].Deactivate();
                }
            }
        }
    }
}