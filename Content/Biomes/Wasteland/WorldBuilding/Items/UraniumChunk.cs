using ROI.Content.Biomes.Wasteland.WorldBuilding.Tiles;
using Terraria.ID;
using Terraria.ModLoader;

namespace ROI.Content.Biomes.Wasteland.WorldBuilding.Items
{
    public class UraniumChunk : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Uranium chunk");
            Tooltip.SetDefault("Shiny green radioactivity.\nWill be used in the 0.2 update - The nuclear and reforge rework update");
        }

        public override void SetDefaults()
        {
            Item.maxStack = 999;
            Item.consumable = true;
            Item.width = 16;
            Item.height = 16;
            Item.useAnimation = 15;
            Item.autoReuse = true;
            Item.useTime = 10;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.useTurn = true;
            Item.createTile = ModContent.TileType<UraniumOre>();
        }
    }
}
