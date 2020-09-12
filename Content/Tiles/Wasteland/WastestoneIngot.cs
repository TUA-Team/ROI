using Microsoft.Xna.Framework;
using ROI.Content.Tiles;
using Terraria.ModLoader;

namespace ROI.Content.Tiles.Wasteland
{
    internal class WastestoneIngot : LegacyIngot
    {
        public override Color MapEntryColor => new Color(68, 74, 100);
        public override string MapNameLegend => "Wastestone Ingot";
        public override int IngotDropName => ModContent.ItemType<Items.Materials.Wasteland.WastestoneIngot>();
    }
}