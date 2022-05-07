using Terraria.ID;
using Terraria.ModLoader;

namespace ROI.Content.Biomes.Wasteland.WorldBuilding.Items
{
    public class WastelandForge : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Wasteland Forge");
            Tooltip.SetDefault("Used to transform Toxic Stone ore in bar");
        }

        public override void SetDefaults()
        {
            Item.maxStack = 99;
            Item.useAnimation = 15;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.useTime = 10;
            Item.consumable = true;
            Item.createTile = ModContent.TileType<Tiles.WastelandForge>();
        }
    }
}
