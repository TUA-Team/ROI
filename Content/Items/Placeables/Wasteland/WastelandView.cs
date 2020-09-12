using Terraria.ID;
using Terraria.ModLoader;

namespace ROI.Content.Items.Placeables.Wasteland
{
    internal class WastelandView : ModItem
    {
        public override void SetDefaults()
        {
            item.maxStack = 99;
            item.width = 48;
            item.height = 32;
            item.useTime = 1;
            item.useAnimation = 15;
            item.useStyle = ItemUseStyleID.Swing;
            item.createTile = ModContent.TileType<Tiles.Wasteland.WastelandView>();
        }
    }
}