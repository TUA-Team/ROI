using Terraria;
using Terraria.ModLoader;

namespace ROI.Items.Materials.Wasteland
{
    class WastestoneIngot : ModItem
    {
        public override void SetDefaults()
        {
            item.width = 30;
            item.height = 24;
            item.useStyle = 1;
            item.value = Item.sellPrice(0, 0, 99, 0);
            item.maxStack = 999;
            item.createTile = ModContent.TileType<Tiles.Wasteland.WastestoneIngot>();
            item.consumable = true;
        }

        public override void AddRecipes()
        {
            var recipe = new ModRecipe(mod);
            //TODO: recipe.AddIngredient(wastelandforge);
            recipe.AddIngredient(ModContent.ItemType<WastestoneOre>(), 3);
            recipe.SetResult(this);
            recipe.AddRecipe();
        }
    }
}
