using Terraria.ID;
using Terraria.ModLoader;

namespace ROI.Items.Placeables.Wasteland
{
    class WastelandRockWall : ModItem
    {
        public override void SetStaticDefaults()
        {
            DisplayName.SetDefault("Radioactive Rock Wall");
            Tooltip.SetDefault("Won't spawn any mobs");
        }

        public override void SetDefaults()
        {
            item.width = 16;
            item.height = 16;
            item.scale *= 0.5f;
            item.maxStack = 999;
            item.useAnimation = 15;
            item.useStyle = 1;
            item.useTime = 10;
            item.consumable = true;
            item.createWall = ModContent.WallType<Walls.Wasteland.WastelandRockWallSafe>();
        }

        public override void AddRecipes()
        {
            ModRecipe recipe = new ModRecipe(mod);
            recipe.AddIngredient(ModContent.ItemType<WastelandRock>());
            recipe.AddTile(TileID.WorkBenches);
            recipe.SetResult(this, 4);
            recipe.AddRecipe();
        }
    }
}
