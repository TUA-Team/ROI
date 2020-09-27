using Terraria.ModLoader;

namespace ROI
{
    public class RecipeBuilder
    {
        ModRecipe recipe;

        public RecipeBuilder(ModItem item, int stack = 1)
        {
            recipe = new ModRecipe(item.mod);
            recipe.SetResult(item.item.type, stack);
        }

        public RecipeBuilder AddIngredient<T>(int stack = 1) where T : ModItem
        {
            return AddIngredient(ModContent.ItemType<T>(), stack);
        }

        public RecipeBuilder AddIngredient(int item, int count = 1)
        {
            recipe.AddIngredient(item, count);
            return this;
        }

        public RecipeBuilder AddTile(int tile)
        {
            recipe.AddTile(tile);
            return this;
        }

        public void Register()
        {
            recipe.AddRecipe();
        }
    }
}
