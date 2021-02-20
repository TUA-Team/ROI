using ROI.API.CustomModLoader;
using Microsoft.Xna.Framework;
using Terraria.ModLoader;

namespace ROI.Content.Subworlds.Wasteland.Furniture.Tiles
{
    internal class WastestoneIngot : LegacyIngot
    {
        public override Color MapEntryColor => new Color(68, 74, 100);
        public override string MapNameLegend => "Wastestone Ingot";
        public override int IngotDropName => ModContent.ItemType<Materials.WastestoneIngot>();
    }
}