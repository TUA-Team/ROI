using Terraria.ID;
using Terraria.ModLoader;

namespace ROI.Content.Biomes.Wasteland.WorldBuilding.Items
{
    public class WastelandView : ModItem
    {
        public override void SetDefaults()
        {
            Item.maxStack = 99;
            Item.width = 48;
            Item.height = 32;
            Item.useTime = 1;
            Item.useAnimation = 15;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.createTile = ModContent.TileType<Tiles.WastelandView>();
        }
    }
}