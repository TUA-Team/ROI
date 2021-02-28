using Microsoft.Xna.Framework;
using ROI.API;
using Terraria.ModLoader;

namespace ROI.Content.Biomes.Wasteland.Furniture.Tiles
{
    public class WastestoneIngot : LegacyIngot
    {
        public override Color MapEntryColor => new Color(68, 74, 100);
        public override string MapNameLegend => "Wastestone Ingot";
        public override int IngotDropName => ModContent.ItemType<Materials.WastestoneIngot>();
    }
}