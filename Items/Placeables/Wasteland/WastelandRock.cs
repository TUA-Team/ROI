using Terraria.ModLoader;
using Terraria.ID;

namespace ROI.Items.Placeables.Wasteland
{
    internal class WastelandRock : ModItem
    {
        public override void SetDefaults()
        {
            item.maxStack = 9999;
            item.consumable = true;
            item.width = 16;
            item.height = 16;
            item.useAnimation = 15;
            item.autoReuse = true;
            item.useTime = 10;
            item.useStyle = ItemUseStyleID.SwingThrow;
            item.useTurn = true;
            item.createTile = ModContent.TileType<Tiles.Wasteland.WastelandRock>();
        }
    }
}