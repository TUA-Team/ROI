using Terraria;
using Terraria.ModLoader;

namespace ROI.Items.Placeable.Wasteland
{
    class Wasteland_Ingot : ModItem
    {
        public override void SetDefaults()
        {
            item.width = 30;
            item.height = 24;
            item.useStyle = 1;
            item.value = Item.sellPrice(0, 0, 99, 0);
            item.maxStack = 999;
            item.createTile = mod.TileType("Wasteland_Bar");
            item.consumable = true;
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(mod.ItemType("Wasteland_Ore"), 3);
            recipe.SetResult(this, 1);
            recipe.AddRecipe();
        }
    }
}
