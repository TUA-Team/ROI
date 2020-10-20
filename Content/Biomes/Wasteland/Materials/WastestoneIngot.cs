using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace ROI.Content.Biomes.Wasteland.Materials
{
    internal class WastestoneIngot : ModItem
    {
        public override void SetDefaults()
        {
            item.width = 30;
            item.height = 24;
            item.useStyle = ItemUseStyleID.SwingThrow;
            item.value = Item.sellPrice(0, 0, 99, 0);
            item.maxStack = 999;
            item.createTile = ModContent.TileType<Content.Tiles.Wasteland.WastestoneIngot>();
            item.consumable = true;
        }

        public override void AddRecipes() => new RecipeBuilder(this)
                .AddIngredient<WastestoneOre>(3)
                .Register();
    }
}