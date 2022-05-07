using Terraria;
using Terraria.ID;
using Terraria.ModLoader;

namespace ROI.Content.Biomes.Wasteland.Materials
{
    public class WastestoneIngot : ModItem
    {
        public override void SetDefaults()
        {
            Item.width = 30;
            Item.height = 24;
            Item.useStyle = ItemUseStyleID.Swing;
            Item.value = Item.sellPrice(0, 0, 99, 0);
            Item.maxStack = 999;
            Item.createTile = ModContent.TileType<Furniture.Tiles.WastestoneIngot>();
            Item.consumable = true;
        }

        public override void AddRecipes() => CreateRecipe()
                .AddIngredient<WastestoneOre>(3)
                .Register();
    }
}