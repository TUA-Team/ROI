using Terraria.ID;
using Terraria.ModLoader;

namespace ROI.Content.Biomes.Wasteland.WorldBuilding.Items
{
    public sealed class WastelandForge : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Wasteland Forge");
            Tooltip.SetDefault("Used to transform Toxic Stone ore in bar");
        }

        public override void SetDefaults()
        {
            item.maxStack = 99;
            item.useAnimation = 15;
            item.useStyle = ItemUseStyleID.SwingThrow;
            item.useTime = 10;
            item.consumable = true;
            item.createTile = ModContent.TileType<Tiles.WastelandForge>();
        }
    }
}
